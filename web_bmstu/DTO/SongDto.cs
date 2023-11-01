namespace web_bmstu.DTO
{
    public class SongBaseDto
    {
        public string Title { get; set; }
        public string AlbumTitle { get; set; }
        public TimeSpan Duration { get; set; }
        public string Genre { get; set; }
        public int? ArtistId { get; set; }
        public int? RecordingStudioId { get; set; }
    }
    public class SongDto : SongBaseDto
    {
        public int Id { get; set; }
    }

    public class SongFilterDto
    {
        public string Title { get; set; }
        public string AlbumTitle { get; set; }
        public TimeSpan Duration { get; set; }
        public string Genre { get; set; }
        public string ArtistName { get; set; }
        public string RecordingStudioName { get; set; }
    }
    public class SongIdDto
    {
        public int Id { get; set; }
    }
}
