using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web_bmstu.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; } // Пароль пользователя

        [Required]
        public string Permission { get; set; }

        // Другие свойства пользователя
    }
}
