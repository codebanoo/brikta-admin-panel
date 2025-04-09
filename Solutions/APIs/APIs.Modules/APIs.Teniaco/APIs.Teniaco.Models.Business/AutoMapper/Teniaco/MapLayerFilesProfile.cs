using APIs.Teniaco.Models.Entities;
using AutoMapper;
using VM.Teniaco;

namespace APIs.Teniaco.Models.Business.AutoMapper.Teniaco
{
    public class MapLayerFilesProfile : Profile
    {
        public MapLayerFilesProfile()
        {

            CreateMap<MapLayerFiles, MapLayerFilesVM>();
            CreateMap<MapLayerFilesVM, MapLayerFiles>();
        }
    }
}
