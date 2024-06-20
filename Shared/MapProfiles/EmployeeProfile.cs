using AutoMapper;
using DataAccess.Enities;
using Shared.DTOs;

namespace Shared.MapProfiles;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<EmployeeViewModel, Employee>();

        CreateMap<Employee, EmployeeViewModel>();
    }
}
