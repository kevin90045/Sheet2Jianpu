namespace MusicXml.Domain
{
	public class MeasureAttributes
	{
        public MeasureAttributes()
		{
			Divisions = 0;
		}

		public int Divisions { get; set; }
		
		public Key Key { get; set; }
		
		public Time Time { get; set; }
		 
		public Clef Clef { get; set; }
	}
}
