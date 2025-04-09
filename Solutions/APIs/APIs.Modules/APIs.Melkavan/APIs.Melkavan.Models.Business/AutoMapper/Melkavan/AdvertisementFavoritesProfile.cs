using APIs.Melkavan.Models.Entities;
using AutoMapper;
using VM.Melkavan;

namespace APIs.Melkavan.Models.Business.AutoMapper.Melkavan
{
    public class AdvertisementFavoritesProfile : Profile
    {
        public AdvertisementFavoritesProfile()
        {
            CreateMap<AdvertisementFavorites, AdvertisementFavoritesVM>();
            CreateMap<AdvertisementFavoritesVM, AdvertisementFavorites>();
        }
    }
}
