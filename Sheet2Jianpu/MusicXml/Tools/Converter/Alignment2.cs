using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicXml.Tools.Converter
{
    public static class Alignment2
    {
        private class Part : List<string>
        {
            public List<int> NoteIndice = new List<int>();
            public List<ConvertedNote> Source = new List<ConvertedNote>();

            public Part(IEnumerable<string> collection) : base(collection)
            {
            }

            /// <summary>
            /// Inserts an element into the Part at the specified index and update note indice.
            /// </summary>
            /// <param name="index"></param>
            /// <param name="item"></param>
            public new void Insert(int index, string item)
            {
                base.Insert(index, item);
                // update indice
                for (int i = 0; i < NoteIndice.Count; i++)
                    if (NoteIndice[i] >= index)
                        NoteIndice[i]++;
            }

            /// <summary>
            /// Inserts the elements of a collection into the Part at the specified index and update note indice.
            /// </summary>
            /// <param name="index"></param>
            /// <param name="collection"></param>
            public new void InsertRange(int index, IEnumerable<string> collection)
            {
                base.InsertRange(index, collection);
                // update indice
                int count = collection.Count();
                for (int i = 0; i < NoteIndice.Count; i++)
                    if (NoteIndice[i] >= index)
                        NoteIndice[i] += count;
            }
        }

        private class Group : List<Part>
        {
            public Group() : base()
            { }

            public Group(IEnumerable<Part> collection) : base(collection)
            {
            }
        }

        private class Measure : List<Group>
        {
        }

        private static int divisions;
        private static int beats;
        private static int partNum;
        private static int NOTE_WIDTH = 3;
        private static int WIDTH_BETWEEN_BEATS;
        private static string SPACE = "l";
        private static string SKIP = "skip";

        public static List<List<char>> Align(
            List<List<ConvertedNote>> convertedMeasure,
            int divisions,
            int beats,
            int beatType)
        {
            List<List<char>> alignedMeasure = new List<List<char>>();
            Alignment2.divisions = divisions;
            Alignment2.beats = beats;
            partNum = convertedMeasure.Count;

            Measure measure = InitializeAlignment(convertedMeasure);
            WIDTH_BETWEEN_BEATS = DetermineWidthBetweenBeats(convertedMeasure);
            for (int beatId = 0; beatId < Alignment2.beats; beatId++)
            {
                Group beat = measure[beatId];
                CheckJianpuRules(ref beat);
                if (beatId > 0) // skip first beat
                    InsertHeadSpace(ref beat);
                InitializeSpace(ref beat);
                InsertAlters(ref beat);
                InsertDotsInBeat(ref beat);
            }
            Group combinedBeats = CombineBeats(ref measure);
            InsertDotsInMeasure(ref combinedBeats);
            DetermineTypes(ref combinedBeats);
            RemoveLastSpace(ref combinedBeats);

            // convert to char lists
            for (int partId = 0; partId < partNum; partId++)
            {
                string part = string.Empty;
                for (int i = 0; i < combinedBeats[partId].Count; i++)
                {
                    if (combinedBeats[partId][i] != SKIP)
                        part += combinedBeats[partId][i];
                }
                alignedMeasure.Add(part.ToList());
            }

            return alignedMeasure;
        }

        private static int DetermineWidthBetweenBeats(List<List<ConvertedNote>> measure_trans)
        {
            for (int i = 0; i < partNum; i++)
                if (measure_trans[i].Any(note => note.Duration % divisions != 0))
                    return NOTE_WIDTH;
            return NOTE_WIDTH * 2;
        }

        private static Measure InitializeAlignment(List<List<ConvertedNote>> measure_trans)
        {
            #region initialize Lists
            Measure measure = new Measure();
            for (int beatId = 0; beatId < beats; beatId++)
            {
                Group beat = new Group();
                for (int i = 0; i < partNum; i++)
                    beat.Add(new Part(Enumerable.Repeat(string.Empty, divisions * NOTE_WIDTH)));
                measure.Add(beat);
            }
            #endregion

            #region input note
            for (int partId = 0; partId < partNum; partId++)
            {
                var notes = measure_trans[partId];
                int length = 0;
                int beatId = 0;
                int index = 0;

                foreach (var note in notes)
                {
                    beatId = length / (divisions * NOTE_WIDTH);
                    index = length % (divisions * NOTE_WIDTH);
                    measure[beatId][partId][index] = note.Note;
                    measure[beatId][partId][index + 1] = measure[beatId][partId][index + 2] = SKIP;
                    measure[beatId][partId].Source.Add(note);
                    length += note.Duration * NOTE_WIDTH;
                }
            }
            #endregion

            #region remove empty places
            for (int beatId = 0; beatId < beats; beatId++)
            {
                for (int i = divisions * NOTE_WIDTH - 1; i >= 0; i--)
                {
                    int emptyCount = (from part in measure[beatId]
                                      where part[i] == string.Empty
                                      select part[i]).Count();

                    if (emptyCount == partNum)
                        for (int partId = 0; partId < partNum; partId++)
                            measure[beatId][partId].RemoveAt(i);
                }
            }
            #endregion

            #region determine note indice
            for (int beatId = 0; beatId < beats; beatId++)
            {
                int partLength = measure[beatId].First().Count;
                for (int partId = 0; partId < partNum; partId++)
                {
                    for (int i = 0; i < partLength; i++)
                    {
                        if (measure[beatId][partId][i] != "" && measure[beatId][partId][i] != SKIP)
                            measure[beatId][partId].NoteIndice.Add(i);
                    }
                }
            }
            #endregion

            return measure;
        }

        private static void CheckJianpuRules(ref Group beat)
        {
            for (int i = 0; i < partNum; i++)
            {
                Part part = beat[i];
                List<int> insertSpaceIndice = new List<int>();
                for (int j = 1; j < part.NoteIndice.Count; j++)
                {
                    var currNoteIndex = part.NoteIndice[j];
                    var currNoteDuration = part.Source[j].Duration *
                        part.Source[j].TimeModification.ActualNotes /
                        part.Source[j].TimeModification.NormalNotes;

                    var prevNoteIndex = part.NoteIndice[j - 1];
                    var prevNoteDuration = part.Source[j - 1].Duration *
                        part.Source[j - 1].TimeModification.ActualNotes /
                        part.Source[j - 1].TimeModification.NormalNotes;

                    if ((currNoteIndex - prevNoteIndex) == NOTE_WIDTH)
                    {
                        if ((currNoteDuration >= divisions && prevNoteDuration >= divisions) ||
                        (currNoteDuration >= divisions && prevNoteDuration < divisions) ||
                        (currNoteDuration < divisions && prevNoteDuration >= divisions))
                        {
                            insertSpaceIndice.Add(currNoteIndex);
                        }
                    }
                }

                if (insertSpaceIndice.Count > 0)
                {
                    insertSpaceIndice.Reverse(); // descending
                    for (int j = 0; j < partNum; j++)
                    {
                        foreach (int index in insertSpaceIndice)
                            beat[j].InsertRange(index, Enumerable.Repeat(string.Empty, NOTE_WIDTH));
                    }
                }
            }
        }

        private static void InsertHeadSpace(ref Group beat)
        {
            for (int i = 0; i < partNum; i++)
                beat[i].InsertRange(0, Enumerable.Repeat(string.Empty, WIDTH_BETWEEN_BEATS));
        }

        private static void InitializeSpace(ref Group beat)
        {
            int partLength = beat.First().Count;
            for (int partId = 0; partId < partNum; partId++)
                for (int i = 0; i < partLength; i++)
                    if (beat[partId][i] == string.Empty)
                        beat[partId][i] = SPACE;
        }

        private static void InsertAlters(ref Group beat)
        {
            for (int partId = 0; partId < partNum; partId++)
            {
                Part part = beat[partId];
                for (int i = part.Source.Count - 1; i >= 0; i--)
                {
                    string alter = part.Source[i].Alter;
                    if (alter != string.Empty)
                    {
                        int noteIndex = part.NoteIndice[i];
                        int alterIndex = noteIndex - 1;
                        if (noteIndex != 0 && part[alterIndex] == SPACE)
                            beat[partId][alterIndex] = alter;
                        // insert space for other parts
                        else
                        {
                            beat[partId].Insert(noteIndex, alter);
                            for (int j = 0; j < partNum; j++)
                            {
                                if (j == partId)
                                    continue;
                                if (noteIndex == 0)
                                    beat[j].Insert(noteIndex, SPACE);
                                else
                                {
                                    string prev = SetType(beat[j][alterIndex], 0);
                                    if (prev == "i" || prev == "o" || prev == "p")
                                        beat[j].Insert(alterIndex, SPACE);
                                    else
                                        beat[j].Insert(noteIndex, SPACE);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void InsertDotsInBeat(ref Group beat)
        {
            for (int partId = 0; partId < partNum; partId++)
            {
                Part part = beat[partId];
                for (int i = 0; i < part.Source.Count; i++)
                {
                    var note = part.Source[i];
                    if (note.Dot == "." && note.Duration < divisions)
                    {
                        if (i == part.Source.Count - 1)
                        {
                            int currIndex = part.NoteIndice[i];
                            int dotIndex = currIndex + NOTE_WIDTH;
                            if (dotIndex + NOTE_WIDTH > part.Count)
                            {
                                int insertIndex = part.Count;
                                int insertNum = (dotIndex + NOTE_WIDTH) - part.Count + 1;
                                // insert space to parts
                                for (int k = 0; k < partNum; k++)
                                    beat[k].InsertRange(insertIndex, Enumerable.Repeat(SPACE, insertNum));
                            }
                            beat[partId][dotIndex + 1] = note.Dot;
                        }
                        else
                        {
                            int currIndex = part.NoteIndice[i];
                            int nextIndex = part.NoteIndice[i + 1];

                            int interval = nextIndex - (currIndex + NOTE_WIDTH);
                            int dotIndex = currIndex + NOTE_WIDTH + ((interval - 1) / 2);

                            if (dotIndex >= part.Count || part[dotIndex] != SPACE)
                            {
                                // insert space to parts
                                for (int k = 0; k < partNum; k++)
                                    beat[k].Insert(dotIndex, SPACE);
                            }
                            beat[partId][dotIndex] = note.Dot;
                        }
                    }
                }
            }
        }

        private static Group CombineBeats(ref Measure measure)
        {
            Group combinedBeats = new Group(measure.Last());

            for (int beatId = measure.Count - 2; beatId >= 0; beatId--)
            {
                for (int partId = 0; partId < partNum; partId++)
                {
                    combinedBeats[partId].InsertRange(0, measure[beatId][partId]);
                    combinedBeats[partId].NoteIndice.InsertRange(0, measure[beatId][partId].NoteIndice);
                    combinedBeats[partId].Source.InsertRange(0, measure[beatId][partId].Source);
                }
            }

            return combinedBeats;
        }

        private static void InsertDotsInMeasure(ref Group combinedBeats)
        {
            #region for '.'
            for (int partId = 0; partId < partNum; partId++)
            {
                Part part = combinedBeats[partId];
                for (int i = 0; i < part.Source.Count; i++)
                {
                    var note = part.Source[i];
                    if (note.Dot == "." && note.Duration >= divisions)
                    {
                        int currIndex = part.NoteIndice[i];
                        int nextIndex = i == part.NoteIndice.Count - 1 ? part.Count : part.NoteIndice[i + 1];

                        int interval = nextIndex - (currIndex + NOTE_WIDTH);
                        int dotIndex = currIndex + NOTE_WIDTH + ((interval - 1) / 2);

                        if (part[dotIndex] != SPACE)
                        {
                            // insert space to parts
                            for (int k = 0; k < partNum; k++)
                                combinedBeats[k].Insert(dotIndex, SPACE);
                        }
                        combinedBeats[partId][dotIndex] = note.Dot;
                    }
                }
            }
            #endregion

            #region for '-'
            for (int partId = 0; partId < partNum; partId++)
            {
                Part part = combinedBeats[partId];
                for (int i = 0; i < part.Source.Count; i++)
                {
                    var note = part.Source[i];
                    if (note.Dot != string.Empty && note.Dot != ".")
                    {
                        int currIndex = part.NoteIndice[i];
                        int nextIndex = i == part.NoteIndice.Count - 1 ? part.Count : part.NoteIndice[i + 1];

                        int interval = nextIndex - (currIndex + NOTE_WIDTH);
                        int dotNum = note.Dot.Length;

                        #region determine dot insert step
                        int step = NOTE_WIDTH + NOTE_WIDTH; // the first NOTE_WIDTH is used to skip current note
                        int requiredInterval = (dotNum + (dotNum + 1)) * NOTE_WIDTH;
                        if (interval < requiredInterval)
                        {
                            int insertSpaceNum = requiredInterval - interval;
                            if (combinedBeats[partId][nextIndex - 1] != SPACE) // alter
                                combinedBeats[partId].InsertRange(nextIndex - 1, Enumerable.Repeat(SPACE, insertSpaceNum));
                            else
                                combinedBeats[partId].InsertRange(nextIndex, Enumerable.Repeat(SPACE, insertSpaceNum));
                        }
                        else
                        {
                            if (i == part.Source.Count - 1)
                                step = (int)(Math.Round(((interval - dotNum * NOTE_WIDTH) / (dotNum + 1.0)), 0, MidpointRounding.AwayFromZero)) + NOTE_WIDTH;
                            else
                                step = (interval - dotNum * NOTE_WIDTH) / (dotNum + 1) + NOTE_WIDTH;
                        }

                        #endregion

                        #region insert dots
                        for (int j = 0, dotIndex = (currIndex + step); j < dotNum; j++, dotIndex += step)
                        {
                            string partialDot = note.Dot[j].ToString();
                            if (partialDot == ".")
                            {
                                combinedBeats[partId][dotIndex + 1] = partialDot;
                            }
                            else // '-'
                            {
                                combinedBeats[partId][dotIndex] = partialDot;
                                for (int k = 1; k < NOTE_WIDTH; k++)
                                    combinedBeats[partId][dotIndex + k] = SKIP;
                            }
                        }
                        #endregion
                    }
                }
            }
            #endregion
        }

        private static void DetermineTypes(ref Group combinedBeats)
        {
            #region determine types of all places
            for (int partId = 0; partId < partNum; partId++)
            {
                var part = combinedBeats[partId];

                for (int i = 0; i < part.Source.Count; i++)
                {
                    var tranformmedNote = part.Source[i];
                    int currType = GetType(tranformmedNote.Note);

                    if (currType == 0)
                        continue;

                    int currIndex = part.NoteIndice[i];
                    int nextIndex = i == part.Source.Count - 1 ? part.Count : part.NoteIndice[i + 1];
                    int nextType = i == part.Source.Count - 1 ? 0 : GetType(part.Source[i + 1].Note);

                    if (i != part.Source.Count - 1 && nextType != 0)
                    {
                        for (int j = currIndex + NOTE_WIDTH; j < nextIndex; j++)
                        {
                            string note = combinedBeats[partId][j];
                            if (note == "i" || note == "o" || note == "p")
                                combinedBeats[partId][j] = SetType(combinedBeats[partId][j], Math.Min(currType, nextType));
                            else
                                combinedBeats[partId][j] = SetType(combinedBeats[partId][j], currType);
                        }
                    }
                    else
                    {
                        var partialPart = part.GetRange(currIndex + NOTE_WIDTH, nextIndex - (currIndex + NOTE_WIDTH));
                        int index = partialPart.FindIndex(x => x != SPACE && x != "i" && x != "o" && x != "p");
                        if (index != -1)
                        {
                            index += currIndex + NOTE_WIDTH;
                            for (int j = currIndex + NOTE_WIDTH; j <= index; j++)
                                combinedBeats[partId][j] = SetType(combinedBeats[partId][j], currType);
                        }
                    }
                }
            }
            #endregion

            #region determine type of spaces between beats
            for (int partId = 0; partId < partNum; partId++)
            {
                var part = combinedBeats[partId];
                int currentBeatId = 0;
                int length = 0;

                for (int i = 0; i < part.Source.Count - 1; i++)
                {
                    var note = part.Source[i];
                    length += note.Duration;
                    int beatId = length / divisions;
                    if (beatId != currentBeatId)
                    {
                        int currNoteIndex = part.NoteIndice[i];
                        int nextNoteIndex = part.NoteIndice[i + 1];

                        for (int j = nextNoteIndex - 1; j >= (currNoteIndex + NOTE_WIDTH); j--)
                        {
                            if (part[j] == ">" || part[j] == "+." || part[j] == "+>")
                                break;
                            if (part[j - 1] == ">" || part[j - 1] == "+." || part[j - 1] == "+>")
                                combinedBeats[partId][j] = SetType(part[j], GetType(part[j - 1]));
                            else
                                combinedBeats[partId][j] = SetType(part[j], 0);
                        }
                        currentBeatId = beatId;
                    }
                }
            }
            #endregion

            #region determine type of last spaces
            for (int i = 0; i < partNum; i++)
            {
                var part = combinedBeats[i];
                for (int j = part.Count - 1; j >= 0; j--)
                {
                    if (part[j] == SPACE)
                    {
                        if (part[j - 1] == ">" || part[j - 1] == "+." || part[j - 1] == "+>")
                            combinedBeats[i][j] = SetType(part[j], GetType(part[j - 1]));
                    }
                    else
                        break;
                }
            }
            #endregion
        }

        private static void RemoveLastSpace(ref Group combinedBeats)
        {
            for (int i = 0; i < partNum; i++)
            {
                var part = combinedBeats[i];
                for (int j = part.Count - 1; j >= 0; j--)
                {
                    if (part[j] == SPACE)
                        part.RemoveAt(j);
                    else
                        break;
                }
            }

        }

        private static int GetType(string note)
        {
            int type = -1;

            if (note == SKIP)
                return type;
            else if (note == "-")
                return 0;

            if (note.Contains("+")) // 2 or 3
                type = IsUpperOfTable(note.Last()) ? 3 : 2;
            else // 0 or 1
                type = IsUpperOfTable(note.Last()) ? 1 : 0;

            return type;
        }

        private static string SetType(string note, int type)
        {
            if (note == SKIP || note == "-")
                return note;

            string newNote = string.Empty;
            char baseNote = note.Last();

            if (type == 1 || type == 3)
                newNote = ToUpperOfTable(baseNote);
            else
                newNote = ToLowerOfTable(baseNote);

            if (type == 2 || type == 3)
                newNote = "+" + newNote;

            return newNote;
        }

        private static bool IsUpperOfTable(char note)
        {
            if (note <= 'Z' && note >= 'A')
                return true;
            if (note == '!' || note == '@' || note == '#' || note == '$' || note == '%' || note == '^' || note == '&' || note == '*' ||
                note == '(' || note == ')' || note == '{' || note == '}' || note == '<' || note == '>' || note == '?'
                )
                return true;
            return false;
        }

        private static string ToUpperOfTable(char note)
        {
            if (note <= 'Z' && note >= 'A')
                return note.ToString();
            if (note <= 'z' && note >= 'a')
                return note.ToString().ToUpper();
            if (note == '!' || note == '@' || note == '#' || note == '$' || note == '%' || note == '^' || note == '&' || note == '*' ||
                note == '(' || note == ')' || note == '{' || note == '}' || note == '<' || note == '>' || note == '?'
                )
                return note.ToString();
            switch (note)
            {
                case '1':
                    return "!";
                case '2':
                    return "@";
                case '3':
                    return "#";
                case '4':
                    return "$";
                case '5':
                    return "%";
                case '6':
                    return "^";
                case '7':
                    return "&";
                case '8':
                    return "*";
                case '9':
                    return "(";
                case '0':
                    return ")";
                case '[':
                    return "{";
                case ']':
                    return "}";
                case ',':
                    return "<";
                case '.':
                    return ">";
                case '/':
                    return "?";
            }
            return string.Empty;
        }

        private static string ToLowerOfTable(char note)
        {
            if (note <= 'z' && note >= 'a')
                return note.ToString();
            if (note <= 'Z' && note >= 'A')
                return note.ToString().ToLower();
            if (note == '1' || note == '2' || note == '3' || note == '4' || note == '5' || note == '6' || note == '7' || note == '8' ||
                note == '9' || note == '0' || note == '[' || note == ']' || note == ',' || note == '.' || note == '/')
                return note.ToString();
            switch (note)
            {
                case '!':
                    return "1";
                case '@':
                    return "2";
                case '#':
                    return "3";
                case '$':
                    return "4";
                case '%':
                    return "5";
                case '^':
                    return "6";
                case '&':
                    return "7";
                case '*':
                    return "8";
                case '(':
                    return "9";
                case ')':
                    return "0";
                case '{':
                    return "[";
                case '}':
                    return "]";
                case '<':
                    return ",";
                case '>':
                    return ".";
                case '?':
                    return "/";
            }
            return string.Empty;
        }
    }
}
