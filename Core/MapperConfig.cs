using AutoMapper;
using Core.Models;
using Data.Entities;

namespace Core
{
    public class MapperConfig
    {
        public IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Person, PersonViewModel>().ReverseMap();
            });
            return config.CreateMapper();
        }
    }
}
