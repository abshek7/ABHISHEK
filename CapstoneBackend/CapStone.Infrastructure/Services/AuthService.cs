using AutoMapper;
using CapStone.Application.DTOs.Auth;
using CapStone.Application.Exceptions;
using CapStone.Application.Repositories;
using CapStone.Application.Services;
using CapStone.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CapStone.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;

        public AuthService(
            IRepository<User> userRepository,
            IUnitOfWork unitOfWork,
            IJwtService jwtService,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _mapper = mapper;
        }

        public async Task RegisterAsync(RegisterDto dto)
        {
            var exists = await _userRepository.GetQueryable().AnyAsync(x => x.Email == dto.Email);
            if (exists)
                throw new ConflictException("Email already exists");

            var user = _mapper.Map<User>(dto);
            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetQueryable().FirstOrDefaultAsync(x => x.Email == dto.Email);
            if (user == null)
                throw new UnauthorizedException("Invalid credentials");
            if (!user.IsActive)
                throw new UnauthorizedException("User account is inactive");

            var validPassword = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!validPassword)
                throw new UnauthorizedException("Invalid credentials");

            var token = _jwtService.GenerateToken(user.Id, user.Name, user.Role);
            return new AuthResponseDto
            {
                Token = token,
                Email = user.Email,
                Role = user.Role.ToString()
            };
        }
    }
}