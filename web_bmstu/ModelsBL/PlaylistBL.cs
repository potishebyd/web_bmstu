using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web_bmstu.ModelsBL
{
    public class PlaylistBL
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public int UserId { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
