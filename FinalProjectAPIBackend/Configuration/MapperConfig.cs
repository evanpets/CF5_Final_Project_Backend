using AutoMapper;
using FinalProjectAPIBackend.Data;
using FinalProjectAPIBackend.DTO.Event;
using FinalProjectAPIBackend.DTO.Performer;
using FinalProjectAPIBackend.DTO.User;
using FinalProjectAPIBackend.DTO.Venue;

namespace FinalProjectAPIBackend.Configuration
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<User, UserUpdateDTO>().ReverseMap();
            CreateMap<User, UserInsertDTO>().ReverseMap();
            CreateMap<User, UserSignupDTO>().ReverseMap();
            CreateMap<User, UserReadOnlyDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();


            CreateMap<Event, EventInsertDTO>()
                .ForMember(dest => dest.PerformerIds, opt => opt.MapFrom(src => src.Performers!.Select(p => p.PerformerId)))
                .ForMember(dest => dest.NewPerformers, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.VenueId, opt => opt.MapFrom(src => src.VenueId)).ReverseMap();

            CreateMap<Event, EventUpdateDTO>()
                .ForMember(dest => dest.Performers, opt => opt.MapFrom(src => src.Performers!.Select(p => new PerformerUpdateDTO { Name = p.Name }))) 
                .ForMember(dest => dest.VenueStreet, opt => opt.MapFrom(src => src.Venue!.VenueAddress!.Street))
                .ForMember(dest => dest.VenueStreetNumber, opt => opt.MapFrom(src => src.Venue!.VenueAddress!.StreetNumber))
                .ForMember(dest => dest.VenueZipCode, opt => opt.MapFrom(src => src.Venue!.VenueAddress!.ZipCode))
                .ForMember(dest => dest.VenueCity, opt => opt.MapFrom(src => src.Venue!.VenueAddress!.City))
                .ReverseMap();

            CreateMap<Event, EventReadOnlyDTO>()
                    .ForMember(dest => dest.VenueName, opt => opt.MapFrom(src => src.Venue!.Name))
                    .ForMember(dest => dest.VenueStreet, opt => opt.MapFrom(src => src.Venue!.VenueAddress!.Street))
                    .ForMember(dest => dest.VenueStreetNumber, opt => opt.MapFrom(src => src.Venue!.VenueAddress!.StreetNumber))
                    .ForMember(dest => dest.VenueZipCode, opt => opt.MapFrom(src => src.Venue!.VenueAddress!.ZipCode))
                    .ForMember(dest => dest.VenueCity, opt => opt.MapFrom(src => src.Venue!.VenueAddress!.City))
                    .ForMember(dest => dest.Performers, opt => opt.MapFrom(src => src.Performers!.Select(p => new PerformerReadOnlyDTO { Name = p.Name }))) 
                    .ReverseMap();

            CreateMap<Performer, PerformerInsertDTO>().ReverseMap();
            CreateMap<Performer, PerformerUpdateDTO>().ReverseMap();
            CreateMap<Performer, PerformerReadOnlyDTO>().ReverseMap();

            CreateMap<Venue, VenueInsertDTO>().ReverseMap();
            CreateMap<Venue, VenueUpdateDTO>().ReverseMap();
            CreateMap<Venue, VenueReadOnlyDTO>().ReverseMap();

            CreateMap<VenueAddress, VenueAddressInsertDTO>().ReverseMap();
            CreateMap<VenueAddress, VenueAddressUpdateDTO>().ReverseMap();
            CreateMap<VenueAddress, VenueAddressReadOnlyDTO>().ReverseMap();

        }
    }
}
