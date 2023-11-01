using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace web_bmstu.ModelsBL
{
    public class SongBL
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AlbumTitle { get; set; } 
        public TimeSpan Duration { get; set; }
        public string Genre { get; set; }
        public int ArtistId { get; set; }
        public int RecordingStudioId { get; set; }
    }
}
