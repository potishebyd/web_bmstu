using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace web_bmstu.Models
{
    public class Playlist
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Playlist name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Playlist name must be between 3 and 100 characters.")]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        [ForeignKey("User")]
        public int UserId { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
