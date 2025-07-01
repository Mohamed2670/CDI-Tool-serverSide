using BCrypt.Net;

namespace CDI_Tool.Model
{
    public class User : IEntity
    {
        public int Id { get; set; }
        public string UserName { get; set; } = "";
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public Role Role { get; set; } = Role.User;
        public string PasswordHash { get; set; } = "";
    }
}