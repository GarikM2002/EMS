using AutoMapper;
using DataAccess.Enities;
using DataAccess.Interfaces;
using Shared.DTOs;

namespace Services.Auth
{
    public class AuthenticationService(IMapper mapper,
        IEmployerRepository employerRepository, JwtTokenService jwtTokenService)
    {
        private readonly IMapper mapper = mapper;
        private readonly IEmployerRepository employerRepository = employerRepository;
        private readonly JwtTokenService jwtTokenService = jwtTokenService;

        /// <returns>Null if request model is incorect, otherwise jwt token</returns>
        public async Task<string?> TryGenerateToken(LoginViewModel loginRequest)
        {
            Employer? employer = await employerRepository.GetEmployerByEmailAsync(loginRequest.Email);

            if (employer == null || !AuthHelper.VerifyPasswordHash(loginRequest.Password,
                    employer.PasswordHash, employer.PasswordSalt))
            {
                return null;
            }

            return jwtTokenService.GenerateToken(employer); ;
        }

        public async Task<EmployerViewModel> RegistrateEmployer(RegistrationViewModel registerRequest)
        {
            var (hash, salt) = AuthHelper.CreatePasswordHash(registerRequest.Password);

            Employer employer = mapper.Map<Employer>(registerRequest);
            employer.PasswordHash = hash;
            employer.PasswordSalt = salt;
            employer.Id = await employerRepository.CreateEmployerAsync(employer);

            return mapper.Map<EmployerViewModel>(employer);
        }

        public async Task<bool> IsAlreadyRegistered(string email) =>
            await employerRepository.GetEmployerByEmailAsync(email) != null;
    }
}
