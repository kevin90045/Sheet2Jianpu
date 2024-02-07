using System.Collections.Generic;

namespace MusicXml.Domain
{
	public class Score
	{
        public Score()
		{
			Parts = new List<Part>();
			MovementTitle = string.Empty;
		}

		public string MovementTitle { get; set; }

		public Identification Identification { get; set; }

		public List<Part> Parts { get; set; }
	}
}
