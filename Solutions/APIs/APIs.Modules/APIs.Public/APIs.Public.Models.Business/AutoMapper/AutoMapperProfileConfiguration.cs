using APIs.Public.Models.Entities;
using VM.Public;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;


namespace APIs.Public.Models.Business.AutoMapper
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
        {
            #region Public

            CreateMap<States, StatesVM>();
            CreateMap<StatesVM, States>();

            CreateMap<Countries, CountriesVM>();
            CreateMap<CountriesVM, Countries>();

            CreateMap<Cities, CitiesVM>();
            CreateMap<CitiesVM, Cities>();

            CreateMap<Zones, ZonesVM>();
            CreateMap<ZonesVM, Zones>();

            CreateMap<Districts, DistrictsVM>();
            CreateMap<DistrictsVM, Districts>();

            CreateMap<Persons, PersonsVM>();
            CreateMap<PersonsVM, Persons>();

            CreateMap<PersonTypes, PersonTypesVM>();
            CreateMap<PersonTypesVM, PersonTypes>();

            CreateMap<FormUsages, FormUsagesVM>();
            CreateMap<FormUsagesVM, FormUsages>();
            CreateMap<ElementTypes, ElementTypesVM>();
            CreateMap<ElementTypesVM, ElementTypes>();

            CreateMap<TypeOfUseLayers, TypeOfUseLayersVM>();
            CreateMap<TypeOfUseLayersVM, TypeOfUseLayers>();

            CreateMap<GuildCategories, GuildCategoriesVM>();
            CreateMap<GuildCategoriesVM, GuildCategories>();

            CreateMap<DistrictFiles, DistrictFilesVM>();
            CreateMap<DistrictFilesVM, DistrictFiles>();

            CreateMap<ZoneFiles, ZoneFilesVM>();
            CreateMap<ZoneFilesVM, ZoneFiles>();

            CreateMap<FormUsages, FormUsagesVM>();
            CreateMap<FormUsagesVM, FormUsages>();

            CreateMap<Indices, IndicesVM>();
            CreateMap<IndicesVM, Indices>();

            CreateMap<PricesListIndices, PricesListIndicesVM>();
            CreateMap<PricesListIndicesVM, PricesListIndices>();

            CreateMap<ConstructionItems, ConstructionItemsVM>();
            CreateMap<ConstructionItemsVM, ConstructionItems>();

            CreateMap<ConstructionSubItems, ConstructionSubItemsVM>();
            CreateMap<ConstructionSubItemsVM, ConstructionSubItems>();

            CreateMap<ConstructionItemPricesHistories, ConstructionItemPricesHistoriesVM>();
            CreateMap<ConstructionItemPricesHistoriesVM, ConstructionItemPricesHistories>();

            CreateMap<UnitsOfMeasurement, UnitsOfMeasurementVM>();
            CreateMap<UnitsOfMeasurementVM, UnitsOfMeasurement>();

            CreateMap<SmsSenders, SmsSendersVM>();
            CreateMap<SmsSendersVM, SmsSenders>();

            #endregion
        }
    }
}
