using EventsApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using EventApi.Data;
using EventApi.Dtos;
using EventApi.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EventApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _repository;
        private readonly PasswordHasher<User> _hasher = new();
        private readonly IConfiguration _config;

        public AuthService(IUserRepository repository, IConfiguration config)
        {
            _repository = repository;
            _config = config;
        }

        public async Task<bool> RegisterAsync(RegisterDto dto)
        {
            var user = await _repository.GetByUsernameAsync(dto.Username);
            if (user is not null)
            {
                // business regel: username moet uniek zijn, return false
                return false;
            }

            var newUser = new User()
            {
                Username = dto.Username
            };
            newUser.PasswordHash = _hasher.HashPassword(newUser, dto.Password);
            await _repository.AddAsync(newUser);
            return true;
        }

        public async Task<TokenDto?> LoginAsync(LoginDto dto)
        {
            // stap 1: bestaat de username?
            var user = await _repository.GetByUsernameAsync(dto.Username);
            if (user is null)
            {
                return null;
            }

            // stap 2: klopt het paswoord?
            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return new TokenDto() { Token = GenerateToken(user) };
        }

        private string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role,user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:key"]
                    ?? throw new InvalidOperationException("Jwt:key ontbreekt in appsettings.json"))
                );

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
