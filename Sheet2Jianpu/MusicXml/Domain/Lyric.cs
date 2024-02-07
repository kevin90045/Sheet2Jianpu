namespace MusicXml.Domain
{
	public enum Syllabic
	{
		None,
		Begin,
		Single,
		End,
		Middle
	}

	public class Lyric
	{
        public Lyric()
		{
		}

		public Syllabic Syllabic { get; set; }
		
		public string Text { get; set; }
	}
}