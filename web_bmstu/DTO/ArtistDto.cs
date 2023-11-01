namespace web_bmstu.DTO
{
    public class ArtistBaseDto
    {
        public string Name { get; set; }
        public string Country { get; set; }
    }

    public class ArtistDto : ArtistBaseDto
    {
        public int Id { get; set; }
    }
    public class ArtistFilterDto
    {
        public string Name { get; set; }
        public string Country { get; set; }
    }
}
