using Microsoft.IdentityModel.Tokens;
using StreamingSubscriptionTrackerAPI.DTOs;
using StreamingSubscriptionTrackerAPI.Models;
using StreamingSubscriptionTrackerAPI.Models.Context;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security;
using System.Text;

namespace StreamingSubscriptionTrackerAPI.Services.Impl
{
    public class UserServiceImpl : IUserService
    {
        private MSSQLContext _context;
        private readonly IConfiguration _configuration;

        public UserServiceImpl(MSSQLContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        //GET
        public List<UserResponseDTO> GetAll() =>
            _context.Users
                .Select(u => ToResponseDTO(u))
                .ToList();

        public List<UserResponseDTO> GetByActived(bool actived)
        {
            return _context.Users
                .Where(u => u.Actived == actived)
                .Select(u => ToResponseDTO(u))
                .ToList();
        }

        public UserResponseDTO GetByEmail(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) throw new ArgumentException($"User with email {email} not found.");
            return ToResponseDTO(user);
        }

        public UserResponseDTO GetById(long id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) throw new ArgumentException($"User with ID {id} not found.");
            return ToResponseDTO(user);
        }

        public UserResponseDTO GetByUsername(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null) throw new ArgumentException($"User with username {username} not found.");
            return ToResponseDTO(user);
        }

        //POST
        public UserResponseDTO Create(UserRequestDTO userDto)
        {
            var user = new User
            {
                Username = userDto.Name,
                Email = userDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                Actived = userDto.Actived,
                CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                Role = "User"
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return ToResponseDTO(user);
        }
        
        public UserLoginResponseDTO Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password)) throw new ArgumentException("Invalid user or password.");

            var token = GenerateToken(user);

            return new UserLoginResponseDTO { Token = token };
        }

        //PUT
        public UserResponseDTO Update(long id, UserRequestDTO userDto)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Id == id);

            if (existingUser == null) throw new ArgumentException($"User with ID {id} not found.");
            
            existingUser.Username = userDto.Name;
            existingUser.Email = userDto.Email;
            existingUser.Actived = userDto.Actived;

            _context.SaveChanges();
            return ToResponseDTO(existingUser);
        }

        public UserResponseDTO UpdateActived(long id, bool actived)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Id == id);
            if (existingUser == null) throw new ArgumentException($"User with ID {id} not found.");

            existingUser.Actived = actived;
            _context.SaveChanges();
            return ToResponseDTO(existingUser);
        }

        public UserResponseDTO UpdatePassword(long id, string password)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Id == id);
            if (existingUser == null) throw new ArgumentException($"User with ID {id} not found.");
            existingUser.Password = BCrypt.Net.BCrypt.HashPassword(password);
            _context.SaveChanges();
            return ToResponseDTO(existingUser);
        }

        //DELETE
        public UserResponseDTO Delete(long id)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Id == id);
            if (existingUser == null) throw new ArgumentException($"User with ID {id} not found.");
            _context.Users.Remove(existingUser);
            _context.SaveChanges();
            return ToResponseDTO(existingUser);
        }

        //GENERATE TOKEN
        private string GenerateToken(User user)
        {
            var jwtKey = _configuration["Jwt:Key"];
            
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //DTO UTILS
        private static UserResponseDTO ToResponseDTO(User user)
        {
            return new UserResponseDTO
            {
                Id = user.Id,   
                Username = user.Username,
                Email = user.Email,
                Actived = user.Actived,
                CreatedAt = user.CreatedAt,
                Role = user.Role
            };
        }
        private UserLoginResponseDTO ToResponseLoginDTO(User user)
        {
            return new UserLoginResponseDTO
            {
                Token = GenerateToken(user)
            };
        }
    }
}