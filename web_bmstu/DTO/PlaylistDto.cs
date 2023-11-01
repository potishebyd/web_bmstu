namespace web_bmstu.DTO
{
    public class PlaylistBaseDto
    {
        public string Name { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public int? UserId { get; set; }
        public TimeSpan Duration { get; set; }
    }

    public class PlaylistDto : PlaylistBaseDto 
    {
        public int Id { get; set; }
    }

    public class PlaylistFilterDto
    {
        public string Name { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public string UserName { get; set; }
        public TimeSpan Duration { get; set; }

    }
}
