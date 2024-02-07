using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicXml.Tools.Converter
{
    public class ConvertedNote : Domain.Note
    {
        public string Alter { get; set; }
        public string Note { get; set; }
        public string Dot { get; set; }

        public ConvertedNote(Domain.Note source, string alter, string note, string dot) :
            base()
        {
            this.Alter = alter;
            this.Note = note;
            this.Dot = dot;
            Duration = source.Duration;
            IsChordTone = source.IsChordTone;
            IsRest = source.IsRest;
            Lyric = source.Lyric;
            Pitch = source.Pitch;
            Staff = source.Staff;
            TimeModification = source.TimeModification;
            Type = source.Type;
            Voice = source.Voice;
        }

        public string GetFullNote()
        {
            return Alter + Note + Dot;
        }
    }
}
