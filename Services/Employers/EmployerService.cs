using AutoMapper;
using DataAccess.Enities;
using DataAccess.Interfaces;
using Services.Auth;
using Shared.DTOs;

namespace Services.Employers;

public class EmployerService(IEmployerRepository employerRepository, IMapper mapper)
    : IEmployerService
{
    private readonly IEmployerRepository employerRepository = employerRepository;
    private readonly IMapper mapper = mapper;

    public async Task<int> DeleteEmployerAsync(int id)
    {
        return await employerRepository.DeleteEmployerAsync(id);
    }

    public async Task<IEnumerable<EmployerViewModel>> GetAllEmployersAsync()
    {
        var employers = await employerRepository.GetAllEmployersAsync();
        return mapper.Map<IEnumerable<Employer>, IEnumerable<EmployerViewModel>>(employers);
    }

    public async Task<EmployerViewModel?> GetEmployerByIdAsync(int id)
    {
        var employer = await employerRepository.GetEmployerByIdAsync(id);

        return employer is null ? null : mapper.Map<Employer, EmployerViewModel>(employer);
    }

    public async Task<EmployerViewModel?> GetEmployerByEmailAsync(string email)
    {
        var employer = await employerRepository.GetEmployerByEmailAsync(email);

        return employer is null ? null : mapper.Map<Employer, EmployerViewModel>(employer);
    }

    public async Task<int> UpdateEmployerAsync(EmployerViewModel model)
    {
        var employer = await employerRepository.GetEmployerByIdAsync(model.Id);
        var (hash, salt) = (employer!.PasswordHash, employer.PasswordSalt);
        
        employer = mapper.Map<EmployerViewModel, Employer>(model);        
        employer.PasswordHash = hash;
        employer.PasswordSalt = salt;

        return await employerRepository.UpdateEmployerAsync(employer);
    }

    public async Task<bool> ExistsAsync(string email)
    {
        return (await employerRepository.GetEmployerByEmailAsync(email)) != null;
    }
}
