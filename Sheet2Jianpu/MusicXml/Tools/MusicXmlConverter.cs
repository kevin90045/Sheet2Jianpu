using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MusicXml;
using MusicXml.Domain;
using MusicXml.Tools.Converter;
using System.Windows.Forms;

namespace MusicXml.Tools
{
    public static partial class MusicXmlConverter
    {
        private static Score score;
        private static Instruments currentInstrument;
        private static List<Instruments> allInstruments;
        private static List<int> changedNoteIndices;
        private static List<int> sharpFlatNoteIndices; // 儲存當前小節升或降的音
        private static int divisions;
        private static int beats;
        private static int beatType;

        public static bool Convert(string filePath, List<Instruments> instruments, out List<List<List<char>>> convertedScore)
        {
            convertedScore = new List<List<List<char>>>();

            score = MusicXmlParser.GetScore(filePath);
            allInstruments = instruments;

            int measureNum = score.Parts.First().Measures.Count;
            int partNum = Math.Min(score.Parts.Count, allInstruments.Count);
            
            for (int measureId = 0; measureId < measureNum; measureId++)
            {
                List<List<ConvertedNote>> convertedMeasure = new List<List<ConvertedNote>>();

                for (int partId = 0; partId < partNum; partId++)
                {
                    var part = score.Parts[partId].Measures[measureId];

                    try
                    {
                        // update variables
                        changedNoteIndices = new List<int>();
                        sharpFlatNoteIndices = new List<int>();
                        currentInstrument = allInstruments[partId];
                        UpdateAttributes(part.Attributes);

                        // get original notes
                        List<Note> noteCandidates = GetNotes(part);
                        List<Note> notes = GetNonChordNotes(noteCandidates);

                        // convert notes into string
                        List<ConvertedNote> convertedPart = new List<ConvertedNote>();
                        foreach (var note in notes)
                        {
                            if (note.IsRest)
                                convertedPart.AddRange(GetConvertedZeroNotes(note));
                            else
                                convertedPart.Add(GetConvertedNote(note));
                        }
                        convertedMeasure.Add(convertedPart);
                    }
                    catch (Exception e)
                    {
                        convertedMeasure.Add(new List<ConvertedNote>());
                        MessageBox.Show("Convert error: " + measureId.ToString() + "," + partId.ToString());                        
                    }
                }

                // alignment
                if (Options.GetInstance().Align)
                {
                    try
                    {
                        int realBeats = (int)(Math.Ceiling(beats / (double)(beatType / 4)));
                        List<List<char>> alignedMeasure = Alignment.Align(convertedMeasure, divisions, beats);
                        convertedScore.Add(alignedMeasure);
                    }
                    catch(Exception e)
                    {
                        List<List<char>> measure = new List<List<char>>();
                        foreach (var part in convertedMeasure)
                        {
                            string notes = string.Empty;
                            foreach (var note in part)
                                notes += note.GetFullNote();
                            measure.Add(notes.ToCharArray().ToList());
                        }
                        convertedScore.Add(measure);

                        MessageBox.Show("Align error: " + measureId.ToString());                        
                    }
                }
                else
                {
                    List<List<char>> measure = new List<List<char>>();
                    foreach (var part in convertedMeasure)
                    {
                        string notes = string.Empty;
                        foreach (var note in part)
                            notes += note.GetFullNote();
                        measure.Add(notes.ToCharArray().ToList());
                    }
                    convertedScore.Add(measure);
                }
            }            
            return true;
        }
        
        private static void UpdateAttributes(MeasureAttributes measureAttributes)
        {
            if (measureAttributes != null)
            {
                if (measureAttributes.Divisions != 0)
                    divisions = measureAttributes.Divisions;
                if (measureAttributes.Time.Beats != 0)
                    beats = measureAttributes.Time.Beats;
                if (measureAttributes.Time.Mode != string.Empty)
                    beatType = int.Parse(measureAttributes.Time.Mode);
                DurationDictionary.Set(divisions);
            }
        }

        private static List<Note> GetNotes(Measure part)
        {
            return (from element in part.MeasureElements
                    where element.Type == MeasureElementType.Note
                    select element.Element as Note).ToList();
        }

        private static List<Note> GetNonChordNotes(List<Note> notesCandidates)
        {
            List<Note> notes = new List<Note>();
            var isChordTone = (from note in notesCandidates select note.IsChordTone).ToList();
            isChordTone.Add(false);

            for (int i = 0; i < notesCandidates.Count; i++)
            {
                if (!isChordTone[i + 1] && notesCandidates[i].Duration > 0)
                    notes.Add(notesCandidates[i]);
            }
            return notes;
        }

        private static List<ConvertedNote> GetConvertedZeroNotes(Note note)
        {
            List<ConvertedNote> zeros = new List<ConvertedNote>();
            int duration = GetDuration(note);
            if (duration < divisions)
                zeros.Add(GetConvertedNote(note));
            else
            {
                int quarterNum = duration / divisions;
                if (duration % divisions != 0)
                {
                    for (int i = 0; i < quarterNum - 1; i++)
                    {
                        Note zero = new Note();
                        zero.Type = "quarter";
                        zero.Duration = divisions;
                        zero.IsRest = true;
                        zeros.Add(new ConvertedNote(zero, "", "0", ""));
                    }
                    {
                        Note zero = new Note();
                        zero.Type = "quarter";
                        zero.Duration = divisions + divisions / 2;
                        zero.IsRest = true;
                        zeros.Add(new ConvertedNote(zero, "", "0", "."));
                    }
                }
                else
                {
                    for (int i = 0; i < quarterNum; i++)
                    {
                        Note zero = new Note();
                        zero.Type = "quarter";
                        zero.Duration = divisions;
                        zero.IsRest = true;
                        zeros.Add(new ConvertedNote(zero, "", "0", ""));
                    }
                }
            }
            return zeros;
        }

        private static ConvertedNote GetConvertedNote(Note note)
        {
            // type
            int typeId = GetTypeId(note);

            // octave & step
            int octaveId = GetOctaveId(note);
            int stepId = GetStepId(note);

            // duration
            int duration = GetDuration(note);

            // alter
            string alter = GetAlter(note, duration, ref stepId, ref octaveId);

            // dot
            string dot = GetDot(note);

            return new ConvertedNote(note, alter, JianpuTable.Get(octaveId, stepId, typeId), dot);
        }

        private static int GetTypeId(Note note)
        {
            int typeId = 0;
            switch (note.Type)
            {
                case "eighth":
                    typeId = 1;
                    break;
                case "16th":
                    typeId = 2;
                    break;
                case "32nd":
                    typeId = 3;
                    break;
                default:
                    typeId = 0;
                    break;
            }
            return typeId;
        }

        private static int GetOctaveId(Note note)
        {
            if (currentInstrument == Instruments.ChordHarmonica)
                return note.IsRest ? 0 : 2;
            else if (note.Pitch == null || note.IsRest)
                return 0;

            int octave = note.Pitch.Octave;
            int octaveId = 0;
            if (currentInstrument == Instruments.ContraBassHarmonica ||
                currentInstrument == Instruments.DoubleBassHarmonica)
            {
                if (octave == 7 || octave == 6 || octave == 5 || octave == 4)
                    octaveId = 0;
                else if (octave == 3)
                    octaveId = 1;
                else if (octave == 2)
                    octaveId = 2;
                else
                    octaveId = 3;
            }
            else
            {
                if (octave == 7 || octave == 6)
                    octaveId = 0;
                else if (octave == 5)
                    octaveId = 1;
                else if (octave == 4)
                    octaveId = 2;
                else
                    octaveId = 3;
            }

            return octaveId;
        }

        private static int GetStepId(Note note)
        {
            if (currentInstrument == Instruments.ChordHarmonica || note.Pitch == null || note.IsRest)
                return 8;

            char step = note.Pitch.Step;
            int stepId = 0;
            switch (step)
            {
                case 'C':
                    stepId = 0;
                    break;
                case 'D':
                    stepId = 1;
                    break;
                case 'E':
                    stepId = 2;
                    break;
                case 'F':
                    stepId = 3;
                    break;
                case 'G':
                    stepId = 4;
                    break;
                case 'A':
                    stepId = 5;
                    break;
                case 'B':
                    stepId = 6;
                    break;
            }
            return stepId;
        }

        private static int GetDuration(Note note)
        {
            return note.Duration *
                 note.TimeModification.ActualNotes /
                 note.TimeModification.NormalNotes; ;
        }

        private static string GetAlter(Note note, int duration, ref int stepId, ref int octaveId)
        {
            if (currentInstrument == Instruments.ChordHarmonica ||
                note.Pitch == null || note.IsRest)
                return string.Empty;

            int alter = note.Pitch.Alter;
            #region set stepInd and octaveInd
            {
                int stepChange = alter / 2;
                alter = alter % 2;
                if (stepChange > 0)
                {
                    for (int i = stepChange; i != 0; i--)
                    {
                        if (stepId == 6) // 7
                        {
                            stepId = 0;
                            octaveId = octaveId == 0 ? 0 : octaveId - 1;
                            alter++;
                        }
                        else if (stepId == 2) // 3
                        {
                            stepId++;
                            alter++;
                        }
                        else
                            stepId++;
                    }
                }
                if (stepChange < 0)
                {
                    for (int i = stepChange; i != 0; i++)
                    {
                        if (stepId == 0) // 1
                        {
                            stepId = 6;
                            octaveId = octaveId == 3 ? 3 : octaveId + 1;
                            alter--;
                        }
                        else if (stepId == 3) // 4
                        {
                            stepId--;
                            alter--;
                        }
                        else
                            stepId--;
                    }
                }
            }
            #endregion

            string newAlter = string.Empty;
            int newStepId = stepId + octaveId * 7;
            int newOctaveId = octaveId;

            switch (alter)
            {
                case 1:
                    newAlter = "o";
                    break;
                case -1:
                    newAlter = "p";
                    break;
            }

            // 處理記號
            if (newAlter == "p" && Options.GetInstance().AllSharpAlter) // 把降記號換成升記號(考慮半音)
            {
                newOctaveId = stepId - 1 >= 0 ? newOctaveId : newOctaveId + 1;
                newOctaveId = newOctaveId > 3 ? 3 : newOctaveId; // 極限是倍低音
                newStepId = stepId - 1 >= 0 ? newStepId - 1 : 6 + newOctaveId * 7;

                if (sharpFlatNoteIndices.Exists(x => x == newStepId)) // 代表前面有同音高的音符有升記號了
                    newAlter = "";
                else // 該音高在小節第一次升(或還原後第一次)
                {
                    if (newStepId % 7 == 6 || newStepId % 7 == 2) // 降1跟降4為7跟3，不用升記號
                        newAlter = "";
                    else
                    {
                        newAlter = "o";
                        sharpFlatNoteIndices.Add(newStepId);
                    }
                }
            }
            else if (newAlter == "p" || newAlter == "o")
            {
                if (sharpFlatNoteIndices.Exists(x => x == newStepId))
                    newAlter = "";
                else
                    sharpFlatNoteIndices.Add(newStepId);
            }
            else
            {
                for (int i = sharpFlatNoteIndices.Count - 1; i >= 0; i--)
                {
                    if (newStepId % 7 == sharpFlatNoteIndices[i] % 7)
                    {
                        newAlter = "i"; // 加上還原記號
                        sharpFlatNoteIndices.RemoveAt(i);
                    }
                }
            }

            stepId = newStepId % 7;
            octaveId = newOctaveId;
            return newAlter;
        }

        private static string GetDot(Note note)
        {
            int duration = GetDuration(note);
            string type = note.Type;
            string dot = string.Empty;

            if (type == "")
            {
                for (int i = 0; i < DurationDictionary.Count; i++)
                {
                    if (duration >= DurationDictionary.ElementAt(i).Value)
                    {
                        type = DurationDictionary.ElementAt(i).Key;
                        break;
                    }
                }
            }

            if (duration != DurationDictionary.Get(type)) //不同的話肯定是加附點
            {
                switch (type)
                {
                    case "maxima":
                        dot = "-----------------------------------------------";
                        break;
                    case "long":
                        dot = "-----------------------";
                        break;
                    case "breve":
                        dot = "-----------";
                        break;
                    case "whole":
                        dot = "-----";
                        break;
                    case "half":
                        dot = "-.";
                        break;
                    default:
                        dot = ".";
                        break;
                }
            }
            else
            {
                switch (type)
                {
                    case "maxima":
                        dot = "-------------------------------";
                        break;
                    case "long":
                        dot = "---------------";
                        break;
                    case "breve":
                        dot = "-------";
                        break;
                    case "whole":
                        dot = "---";
                        break;
                    case "half":
                        dot = "-";
                        break;
                }
            }
            return dot;
        }
    }
}
