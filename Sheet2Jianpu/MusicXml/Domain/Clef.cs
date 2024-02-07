namespace MusicXml.Domain
{
	public class Clef
	{
        public Clef()
		{
			Line = 0;
			Sign = string.Empty;
		}

		public int Line { get; set; }
		
		public string Sign { get; set; }
	}
}
