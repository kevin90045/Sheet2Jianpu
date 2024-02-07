namespace MusicXml.Domain
{
	public class Key
	{
        public Key()
		{
			Fifths = 0;
			Mode = string.Empty;
		}

		public int Fifths { get; set; }

		public string Mode { get; set; }
	}
}
