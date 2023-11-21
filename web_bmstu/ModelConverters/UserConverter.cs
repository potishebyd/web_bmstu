using web_bmstu.DTO;
using web_bmstu.ModelsBL;
using web_bmstu.Services;

namespace web_bmstu.ModelsConverters
{
    public class UserConverters
    {
        private readonly IUserService userService;

        public UserConverters(IUserService userService)
        {
            this.userService = userService;
        }

        public UserBL convertPatch(int id, UserPasswordDto user)
        {
            var existedUser = userService.GetByID(id);

            return new UserBL
            {
                Id = id,
                Login = user.Login ?? existedUser.Login,
                Password = user.Password ?? existedUser.Password,
                Permission = user.Permission ?? existedUser.Permission,
                Email = user.Email ?? existedUser.Email
            };
        }
    }
}