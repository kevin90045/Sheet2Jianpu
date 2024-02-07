namespace MusicXml.Domain
{
	public class Identification
	{
        public Identification()
		{
			Composer = string.Empty;
			Rights = string.Empty;
		}

		public string Composer { get; set; }
		
		public string Rights { get; set; }
		
		public Encoding Encoding { get; set; }
	}
}
