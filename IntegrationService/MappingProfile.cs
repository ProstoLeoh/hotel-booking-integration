using AutoMapper;

namespace IntegrationService;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CartItemDto, OrderLineDto>();
    }
}
