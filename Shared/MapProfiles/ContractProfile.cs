using System.Data.SqlClient;
using AutoMapper;
using DataAccess.Enities;
using Shared.DTOs;

namespace Shared.MapProfiles;

public class ContractProfile : Profile
{
    public ContractProfile()
    {
        CreateMap<Contract, ContractViewModel>();

        CreateMap<ContractViewModel, Contract>();
    }
}
