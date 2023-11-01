using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web_bmstu.Models
{
    public class RecordingStudio
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public int YearFounded { get; set; }

        [Required]
        public string Country { get; set; }
    }
}
