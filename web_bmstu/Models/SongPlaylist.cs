using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web_bmstu.Models
{
    public class SongPlaylist
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Song")]
        public int SongId { get; set; }
        [Required]
        [ForeignKey("Playlist")]
        public int PlaylistId { get; set; }
    }
}
