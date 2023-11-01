using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using NodaTime;

namespace web_bmstu.Models
{
    public class Song
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Song title is required.")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Song title must be between 3 and 200 characters.")]
        public string Title { get; set; }

        public string AlbumTitle { get; set; } // Название альбома
        public TimeSpan Duration { get; set; }

        [Required(ErrorMessage = "Genre is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Genre must be between 3 and 50 characters.")]
        public string Genre { get; set; }


        [Required(ErrorMessage = "Artist ID is required.")]
        [ForeignKey("Artist")]
        public int ArtistId { get; set; }

        [Required(ErrorMessage = "RecordingStudio ID is required.")]
        [ForeignKey("RecordingStudio")]
        public int RecordingStudioId { get; set; }
    }
}
