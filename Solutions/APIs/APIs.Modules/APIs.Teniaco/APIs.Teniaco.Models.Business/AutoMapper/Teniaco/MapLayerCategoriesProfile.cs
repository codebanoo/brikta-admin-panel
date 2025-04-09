using APIs.Teniaco.Models.Entities;
using AutoMapper;
using VM.Teniaco;

namespace APIs.Teniaco.Models.Business.AutoMapper.Teniaco
{
    public class MapLayerCategoriesProfile : Profile
    {
        public MapLayerCategoriesProfile()
        {
            CreateMap<MapLayerCategories, MapLayerCategoriesVM>();
            CreateMap<MapLayerCategoriesVM, MapLayerCategories>();

        }
    }
}
