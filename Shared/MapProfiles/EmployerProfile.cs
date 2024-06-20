using AutoMapper;
using DataAccess.Enities;
using Shared.DTOs;

namespace Shared.MapProfiles;

public class EmployerProfile : Profile
{
    public EmployerProfile()
    {
        CreateMap<RegistrationViewModel, Employer>();

        CreateMap<Employer, EmployerViewModel>();
        CreateMap<EmployerViewModel, Employer>();
    }
}
