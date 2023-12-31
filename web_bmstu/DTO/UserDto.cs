﻿namespace web_bmstu.DTO
{
    public class UserBaseDto
    {
        public string Login { get; set; }
        public string Permission { get; set; }
        public string Email { get; set; }
    }

    public class UserPasswordDto : UserBaseDto
    {
        public string Password { get; set; }
    }

    public class UserIdPasswordDto : UserPasswordDto
    {
        public int Id { get; set; }
    }

    public class UserDto : UserBaseDto
    {
        public int Id { get; set; }
    }

    public class LoginDto
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class TokenDto
    {
        public string AccessToken { get; set; }
        public string Username { get; set; }
    }
}
