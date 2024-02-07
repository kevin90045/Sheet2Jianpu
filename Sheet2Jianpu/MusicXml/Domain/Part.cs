using System.Collections.Generic;

namespace MusicXml.Domain
{
	public class Part
	{
        public Part()
		{
			Id = string.Empty;
			Name = string.Empty; 
			Measures = new List<Measure>();	
		}

		public string Id { get; set; }
		
		public string Name { get; set; }
		
		public List<Measure> Measures { get; set; }
	}
}
