using CDI_Tool.Model;

namespace CDI_Tool.Dtos.UserDtos
{
    public class UserAddDto
    {
        public string UserName { get; set; } = "";
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public Role Role { get; set; } = Role.User;
        public string Password { get; set; } = ""; // plain text from form
    }
}