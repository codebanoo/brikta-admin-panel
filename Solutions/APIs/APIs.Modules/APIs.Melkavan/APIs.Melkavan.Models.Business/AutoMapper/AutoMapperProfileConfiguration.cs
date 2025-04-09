using APIs.Melkavan.Models.Entities;
using VM.Melkavan;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;


namespace APIs.Melkavan.Models.Business.AutoMapper
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
        {
            #region Melkavan

            CreateMap<Banners, BannersVM>();
            CreateMap<BannersVM, Banners>();

            CreateMap<Smses, SmsesVM>();
            CreateMap<SmsesVM, Smses>();

            CreateMap<AdvertisementFeaturesValues, AdvertisementFeaturesValuesVM>();
            CreateMap<AdvertisementFeaturesValuesVM, AdvertisementFeaturesValues>();

            CreateMap<AdvertisementFeatures, AdvertisementFeaturesVM>();
            CreateMap<AdvertisementFeaturesVM, AdvertisementFeatures>();

            CreateMap<Advertisement, AdvertisementVM>();
            CreateMap<AdvertisementVM, Advertisement>();

            CreateMap<AdvertisementAddress, AdvertisementAddressVM>();
            CreateMap<AdvertisementAddressVM, AdvertisementAddress>();

            CreateMap<AdvertisementDetails, AdvertisementDetailsVM>();
            CreateMap<AdvertisementDetailsVM, AdvertisementDetails>();


            CreateMap<AdvertisementFiles, AdvertisementFilesVM>();
            CreateMap<AdvertisementFilesVM, AdvertisementFiles>();

            CreateMap<AdvertisementOwners, AdvertisementOwnersVM>();
            CreateMap<AdvertisementOwnersVM, AdvertisementOwners>();

            CreateMap<AdvertisementViewers, AdvertisementViewersVM>();
            CreateMap<AdvertisementViewersVM, AdvertisementViewers>();

            CreateMap<AdvertisementCallers, AdvertisementCallersVM>();
            CreateMap<AdvertisementCallersVM, AdvertisementCallers>();

            CreateMap<AdvertisementFavorites, AdvertisementFavoritesVM>();
            CreateMap<AdvertisementFavoritesVM, AdvertisementFavorites>();

            CreateMap<AdvertisementPricesHistories, AdvertisementPricesHistoriesVM>();
            CreateMap<AdvertisementPricesHistoriesVM, AdvertisementPricesHistories>();

            CreateMap<BuildingLifes, BuildingLifesVM>();
            CreateMap<BuildingLifesVM, BuildingLifes>();

            #endregion
        }
    }
}
