using System.ComponentModel.DataAnnotations;

namespace web_bmstu.ModelsBL
{
    public class RecordingStudioBL
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int YearFounded { get; set; }
        public string Country { get; set; }
    }
}
