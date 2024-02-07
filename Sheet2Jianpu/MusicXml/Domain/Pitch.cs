using System;

namespace MusicXml.Domain
{
	public class Pitch
	{
        public Pitch()
		{
			Alter = 0;
			Octave = 0;
			Step = new Char();
		}

		public int Alter { get; set; }

		public int Octave { get; set; }

		public char Step { get; set; }
	}
}