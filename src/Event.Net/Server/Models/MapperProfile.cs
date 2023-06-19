using AutoMapper;
using Event.Net.Shared;

namespace Event.Net.Server.Models
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Event, EventDto>()
                .ForMember(dest => dest.EventId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.EventName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.OrganizerId, opt => opt.MapFrom(src => src.UserId))
                .ReverseMap();
        }
    }
}
