using AutoMapper;
using DemoBackendArchitecture.Application.Common.Model.Product;
using DemoBackendArchitecture.Application.Common.Model.User;
using DemoBackendArchitecture.Domain.Entities;

namespace DemoBackendArchitecture.Application.Mapping;

public class MapProfile : Profile
{
    
    public MapProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<User, UserSignInRequest>().ReverseMap();
        CreateMap<User, UserSignInResponse>().ReverseMap();
        CreateMap<User, UserSignUpRequest>().ReverseMap();
        CreateMap<User, UserSignUpResponse>().ReverseMap();
    }
}