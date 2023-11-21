using System.ComponentModel.DataAnnotations;

namespace web_bmstu.ModelsBL
{
    public class UserBL
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Permission { get; set; }
        public string Email { get; set; }
    }
}
