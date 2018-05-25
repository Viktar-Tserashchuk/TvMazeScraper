using AutoMapper;
using TvMazeScraper.Core.Model;

namespace TvMazeScraper.WebApi.Mapping
{
    public class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize(config =>
            {
                config
                    .CreateMap<Show, Model.ShowDto>()
                    .ForMember(dest => dest.Cast, opt => opt.MapFrom(src => src.GetActorsOrderedByBirthday()));
                config
                    .CreateMap<Actor, Model.ActorDto>()
                    .ForMember(
                        dest => dest.Birthday, 
                        opt => opt.MapFrom(src => src.Birthday == null ? null : src.Birthday.Value.ToString("yyyy-MM-dd"))
                    );
            });
        }
    }
}
