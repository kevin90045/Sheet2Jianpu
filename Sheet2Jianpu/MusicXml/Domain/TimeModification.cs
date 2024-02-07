using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicXml.Domain
{
    public class TimeModification
    {
        public TimeModification()
        {
            ActualNotes = 1;
            NormalNotes = 1;
        }

        public int ActualNotes { get; set; }

        public int NormalNotes { get; set; }
    }
}
