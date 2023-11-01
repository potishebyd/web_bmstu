namespace web_bmstu.DTO
{
    public class SongPlaylistBaseDto
    {
        public int? SongId { get; set; }
        public int? PlaylistId { get; set; }
    }
    
    public class SongPlaylistDto : SongPlaylistBaseDto
    {
        public int Id { get; set; }
    }
}
