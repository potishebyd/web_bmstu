namespace web_bmstu.DTO
{
    public class RecordingStudioBaseDto
    {
        public string Name { get; set; }
        public int? YearFounded { get; set; }
        public string Country { get; set; }
    }
    public class RecordingStudioDto : RecordingStudioBaseDto
    {
        public int Id { get; set; }
    }

    public class RecordingStudioFilterDto
    {
        public string Name { get; set; }
        public int? YearFounded { get; set; }
        public string Country { get; set; }
    }

}
