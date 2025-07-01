using AutoMapper;
using CDI_Tool.Data;
using CDI_Tool.Dtos.UserDtos;
using CDI_Tool.Model;
using Microsoft.EntityFrameworkCore;

namespace CDI_Tool.Repository
{
    public class UserRepository : GenericRepository<User>
    {
        private readonly CDIDBContext _context;
        private readonly IMapper _mapper;
        public UserRepository(CDIDBContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }
        internal async Task<bool> UserAdd(UserAddDto userAddDto)
        {
            var user = _mapper.Map<User>(userAddDto);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userAddDto.Password);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user!=null;
        }
        internal async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
        }
        internal async Task<User> GetUserByUserName(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == userName.ToLower());

        }
    }
}