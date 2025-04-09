using APIs.Public.Models.Entities;
using FrameWork;
using Microsoft.EntityFrameworkCore;
using Models.Business.ConsoleBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VM.Public;


namespace APIs.Public.Models.Business
{
    public interface IPublicApiBusiness
    {
        PublicApiContext PublicApiDb { get; set; }

        #region Public

        #region Methods For Work With Countries

         int AddToCountries(string countryName, string countryLatinName, string countryAbbreviationName, string countryCode, string countryFlagPath);

         List<CountriesVM> GetListOfCountries();

        #endregion

        #region Methods For Work With ConstructionItems

         List<ConstructionItemsVM> GetAllConstructionItemsList(ref int listCount,
            long? ConstructionItemParentId,
            string ConstructionItemTitle,
            DateTime? dateTimeTo);

         List<ConstructionItemsVM> GetListOfConstructionItems(int jtStartIndex,
            int jtPageSize,
            ref int listCount,
            long? ConstructionItemParentId,
            string ConstructionItemTitle,
            DateTime? dateTimeTo,
            string jtSorting = null);

         long AddToConstructionItems(ConstructionItemsVM constructionItemsVM/*,
            List<long> childsUsersIds*/);

         ConstructionItemsVM GetConstructionItemWithConstructionItemId(long constructionItemId/*,
            List<long> childsUsersIds*/);

         long UpdateConstructionItems(ref ConstructionItemsVM constructionItemsVM/*,
            List<long> childsUsersIds*/);

         bool ToggleActivationConstructionItems(long constructionItemId,
            long userId,
            List<long> childsUsersIds);

         bool TemporaryDeleteConstructionItems(long constructionItemId,
            long userId,
            List<long> childsUsersIds);

         bool CompleteDeleteConstructionItems(long constructionItemId,
            List<long> childsUsersIds,
            ref string returnMessage);
        #endregion

        #region Methods For Work With ConstructionSubItems

         List<ConstructionSubItemsVM> GetAllConstructionSubItemsList(ref int listCount,
            long? ConstructionItemId,
            string ConstructionSubItemTitle);

         List<ConstructionSubItemsVM> GetListOfConstructionSubItems(int jtStartIndex,
            int jtPageSize,
            ref int listCount,
            long? ConstructionItemId,
            string ConstructionSubItemTitle,
            string jtSorting = null);

         long AddToConstructionSubItems(ConstructionSubItemsVM ConstructionSubItemsVM/*,
            List<long> childsUsersIds*/);

         ConstructionSubItemsVM GetConstructionSubItemWithConstructionSubItemId(long ConstructionSubItemId/*,
            List<long> childsUsersIds*/);

         long UpdateConstructionSubItems(ref ConstructionSubItemsVM ConstructionSubItemsVM/*,
            List<long> childsUsersIds*/);
         bool ToggleActivationConstructionSubItems(long ConstructionSubItemId,
            long userId,
            List<long> childsUsersIds);

         bool TemporaryDeleteConstructionSubItems(long ConstructionSubItemId,
            long userId,
            List<long> childsUsersIds);

         bool CompleteDeleteConstructionSubItems(long ConstructionSubItemId,
            List<long> childsUsersIds);

        #endregion

        #region Methods For Work With ConstructionItemPricesHistories

         List<ConstructionItemPricesHistoriesVM> GetAllConstructionItemPricesHistoriesList(ref int listCount,
            string parentType,
            long parentId);

         List<ConstructionItemPricesHistoriesVM> GetListOfConstructionItemPricesHistories(int jtStartIndex,
            int jtPageSize,
            ref int listCount,
            string parentType,
            long parentId,
            DateTime? dateTimeTo,
            string jtSorting = null);

         long AddToConstructionItemPricesHistories(ConstructionItemPricesHistoriesVM ConstructionItemPricesHistoriesVM/*,
            List<long> childsUsersIds*/);

         ConstructionItemPricesHistoriesVM GetConstructionItemHistoryWithConstructionItemHistoryId(long constructionItemPricesHistoryId/*,
            List<long> childsUsersIds*/);

         long UpdateConstructionItemPricesHistories(ref ConstructionItemPricesHistoriesVM ConstructionItemPricesHistoriesVM/*,
            List<long> childsUsersIds*/);

         bool ToggleActivationConstructionItemPricesHistories(long constructionItemPricesHistoryId,
            long userId,
            List<long> childsUsersIds);

         bool TemporaryDeleteConstructionItemPricesHistories(long constructionItemPricesHistoryId,
            long userId,
            List<long> childsUsersIds);

         bool CompleteDeleteConstructionItemPricesHistories(long constructionItemPricesHistoryId,
            List<long> childsUsersIds,
            ref string returnMessage);

        #endregion

        #region Methods For Work With Cities

         List<CitiesVM> GetAllCitiesList(ref int listCount,
            long? StateId,
            string cityName,
            string cityCode);

         List<CitiesVM> GetAllCitiesListWithOutStrPolygon(ref int listCount,
            long? StateId,
            string cityName,
            string cityCode);

         List<CitiesVM> GetListOfCities(int jtStartIndex,
            int jtPageSize,
            ref int listCount,
            long? StateId,
            string cityName,
            string cityCode,
            string jtSorting = null);

         List<CitiesVM> GetListOfCitiesWithOutStrPolygon(int jtStartIndex,
            int jtPageSize,
            ref int listCount,
            long? StateId,
            string cityName,
            string cityCode,
            string jtSorting = null);

         long AddToCities(CitiesVM citysVM);
         CitiesVM GetCityWithCityId(long CityId/*,
            List<long> childsUsersIds*/);

         long UpdateCities(ref CitiesVM citysVM/*,
            List<long> childsUsersIds*/);

         bool ToggleActivationCities(long cityId,
            long userId,
            List<long> childsUsersIds);

         bool TemporaryDeleteCities(long cityId,
            long userId,
            List<long> childsUsersIds);

         bool CompleteDeleteCities(long cityId,
            List<long> childsUsersIds);

        #endregion

        #region Methods For Work With Districts

         List<DistrictsVM> GetAllDistrictsList(
            long? stateId = null,
            long? cityId = null,
            long? zoneId = null,
           string? districtName = "");

         List<DistrictsVM> GetAllDistrictsListWithOutStrPolygon(
            long? stateId = null,
            long? cityId = null,
            long? zoneId = null,
           string? districtName = "");


         List<DistrictsVM> GetListOfDistricts(int jtStartIndex,
             int jtPageSize,
             ref int listCount,
             long? stateId = null,
             long? cityId = null,
             long? zoneId = null,
             string? districtName = "",
             string jtSorting = null);


         List<DistrictsVM> GetListOfDistrictsWithOutStrPolygon(
             int jtStartIndex,
             int jtPageSize,
             ref int listCount,
             long? stateId = null,
             long? cityId = null,
             long? zoneId = null,
             string? districtName = "",
             string jtSorting = null);

         long AddToDistricts(DistrictsVM districtsVM);

         DistrictsVM GetDistrictWithDistrictId(long districtId);

         long UpdateDistricts(ref DistrictsVM districtsVM,
            List<long> childsUsersIds);

         bool ToggleActivationDistricts(long districtId,
           long userId,
           List<long> childsUsersIds);


         bool TemporaryDeleteDistricts(long districtId,
           long userId,
           List<long> childsUsersIds);

         bool CompleteDeleteDistricts(long districtId,
           List<long> childsUsersIds);

        #endregion

        #region Methods For Work With DistrictFiles

         List<DistrictFilesVM> GetListOfDistrictFiles(int jtStartIndex,
             int jtPageSize,
             ref int listCount,
             List<long> childsUsersIds,
             long? districtId = null,
             string districtFileTitle = "",
             string districtFileType = "",
             string jtSorting = null);



         bool AddToDistrictFiles(List<DistrictFilesVM> districtFilesVMList);

         DistrictFilesVM GetDistrictFileWithDistrictFileId(int districtFileId,
            List<long> childsUsersIds);

         bool UpdateDistrictFiles(ref DistrictFilesVM districtFilesVM,
            List<long> childsUsersIds);

         bool ToggleActivationDistrictFiles(int districtFileId,
            long userId,
            List<long> childsUsersIds);

         bool TemporaryDeleteDistrictFiles(int districtFileId,
            long userId,
            List<long> childsUsersIds);

         bool CompleteDeleteDistrictFiles(int districtFileId,
            List<long> childsUsersIds);

        #endregion

        #region Methods For Work With ElementTypesPublic
         List<ElementTypesVM> GetAllElementTypesList();
        #endregion

        #region Methods For Work With FormUsages

         List<FormUsagesVM> GetAllFormUsagesList();

        #endregion

        #region Methods For Work With FormUsages

         List<FormUsagesVM> GetAllFormUsagesList(
           string? formUsageTitle = "");



         List<FormUsagesVM> GetListOfFormUsages(int jtStartIndex,
             int jtPageSize,
             ref int listCount,
             string? formUsageTitle = "",
             string jtSorting = null);


         int AddToFormUsages(FormUsagesVM formUsagesVM);


         FormUsagesVM GetFormUsageWithFormUsageId(int formUsageId);


         int UpdateFormUsages(ref FormUsagesVM formUsagesVM,
           List<long> childsUsersIds);

         bool ToggleActivationFormUsages(int formUsageId,
           long userId,
           List<long> childsUsersIds);


         bool TemporaryDeleteFormUsages(int formUsageId,
           long userId,
           List<long> childsUsersIds);

         bool CompleteDeleteFormUsages(int formUsageId,
           List<long> childsUsersIds);
        #endregion

        #region Methods For Work With GuildCategories
         List<GuildCategoriesVM> GetAllGuildCategoriesList(
            long? guildCategoryId = null);

        #endregion

        #region Methods For Work with Indices

         List<IndicesVM> GetAllIndicesList(int? IndiceId);

         List<IndicesVM> GetListOfIndices(int jtStartIndex, int jtPageSize, ref int listCount, List<long> childsUsersIds, string? jtSorting);

         int AddToIndices(IndicesVM indicesVM);

         int UpdateIndices(IndicesVM indicesVM, List<long> childsUsersIds);

         bool ToggleActivationIndices(int IndiceId, long userId, List<long> childsUsersIds);

         bool TemporaryDeleteIndices(int IndiceId, long userId, List<long> childsUsersIds);

         bool CompleteDeleteIndices(int IndiceId, List<long> childsUsersIds);

        #endregion

        #region Methods For Work With Persons

        List<PersonsVM> GetBuyersList(
    List<long> childsUsersIds);

         List<PersonsVM> GetListOfPersons(
            List<long> childsUsersIds,
            int jtStartIndex,
            int jtPageSize,
            ref int listCount,
            string searchText/*,
           List<long> childsUsersIds,
           string Lang = null,
           string jtSorting = null
           /*long userId = 0*/);

         List<PersonsVM> GetAllPersonsList(
            List<long> childsUsersIds,
            ref int listCount,
            string searchText);

         long AddToPersons(PersonsVM personsVM, List<long> childsUsersIds);

         long UpdatePersons(ref PersonsVM personsVM,
            //long userId,
            List<long> childsUsersIds);

         bool ToggleActivationPersons(long personId, long userId, List<long> childsUsersIds);

         bool TemporaryDeletePersons(long personId,
            long userId,
            List<long> childsUsersIds);

         bool CompleteDeletePersons(long personId,
            //long userId,
            List<long> childsUsersIds);

         PersonsVM GetPersonWithMobileNumber(string mobileNumber,
           List<long> childsUsersIds);

         PersonsVM GetPersonWithUserId(long userId,
          List<long> childsUsersIds);

        List<PersonsVM> GetAllPersonsListWithUsers(
             List<long> childsUsersIds,
             IConsoleBusiness consoleBusiness);

        #endregion

        #region Methods For Work With PersonTypes

        List<PersonTypesVM> GetAllPersonTypesList();

        #endregion

        #region Methods For Work with PricesListIndices

         List<PricesListIndicesVM> GetAllPricesListIndicesList(int? PricesListIndicesId);

         List<PricesListIndicesVM> GetListOfPricesListIndices(int IndicesId, DateTime? PBDate, DateTime? PEDate,
            int jtStartIndex, int jtPageSize, ref int listCount, List<long> childsUsersIds, string jtSorting);
         int AddToPricesListIndices(PricesListIndicesVM pricesListIndicesVM);

         int UpdatePricesListIndices(PricesListIndicesVM pricesListIndicesVM, List<long> childsUsersIds);

         bool ToggleActivationPricesListIndices(int PricesListIndicesId, long userId, List<long> childsUsersIds);
         bool TemporaryDeletePricesListIndices(int PricesListIndicesId, long userId, List<long> childsUsersIds);

         bool CompleteDeletePricesListIndices(int PricesListIndicesId, List<long> childsUsersIds);

        #endregion

        #region Methods For Work With States

         List<StatesVM> GetListOfStates(int? countryId);

         StatesVM GetStateWithStateId(long StateId, List<long> childsUsersIds);

        #endregion

        #region Methods For Work With SmsSenders
         SmsSendersVM GetDefaultSmsSender();
         List<SmsSendersVM> GetListOfSmsSenders(
            List<long> childsUsersIds,
            int jtStartIndex,
            int jtPageSize,
            ref int listCount,
            string searchText/*,
           List<long> childsUsersIds,
           string Lang = null,
           string jtSorting = null
           /*long userId = 0*/);

         List<SmsSendersVM> GetAllSmsSendersList(
            List<long> childsUsersIds,
            ref int listCount,
            string searchText);

         long AddToSmsSenders(SmsSendersVM smsSendersVM, List<long> childsUsersIds);

         long UpdateSmsSenders(ref SmsSendersVM smsSendersVM,
            //long userId,
            List<long> childsUsersIds);

         bool ToggleActivationSmsSenders(long smsSenderId, long userId, List<long> childsUsersIds);

         bool TemporaryDeleteSmsSenders(long smsSenderId,
            long userId,
            List<long> childsUsersIds);

         bool CompleteDeleteSmsSenders(long smsSenderId,
            //long userId,
            List<long> childsUsersIds);

        #endregion

        #region Methods For Work With TypeOfUseLayers
         List<TypeOfUseLayersVM> GetAllTypeOfUseLayersList(
          ref int listCount,
          List<long> childsUsersIds,
          int? typeOfUseLayersId);

        #endregion

        #region Methods For Work With UnitsOfMeasurement

         List<UnitsOfMeasurementVM> GetAllUnitsOfMeasurementList(ref int listCount);

        #endregion

        #region Methods For Work With Zones

         List<ZonesVM> GetAllZonesList(ref int listCount,
            long? stateId = null,
            long? cityId = null,
            string searchTitle = null,
            string jtSorting = null);

         List<ZonesVM> GetAllZonesListWithOutStrPolygon(ref int listCount,
            long? stateId = null,
            long? cityId = null,
            string searchTitle = null,
            string jtSorting = null);

         List<ZonesVM> GetListOfZones(int jtStartIndex,
              int jtPageSize,
              ref int listCount,
              //List<long> childsUsersIds,
              long? stateId = null,
              long? cityId = null,
              string searchTitle = null,
              string jtSorting = null);



         List<ZonesVM> GetListOfZonesWithOutStrPolygon(int jtStartIndex,
             int jtPageSize,
             ref int listCount,
             long? stateId = null,
             long? cityId = null,
             string searchTitle = null,
             string jtSorting = null);

         long AddToZones(ZonesVM zonesVM/*,
            List<long> childsUsersIds*/);

         ZonesVM GetZoneWithZoneId(long zoneId);

         long UpdateZones(ref ZonesVM zonesVM/*,
            List<long> childsUsersIds*/);

         bool ToggleActivationZones(long zoneId,
            long userId,
            List<long> childsUsersIds);

         bool TemporaryDeleteZones(long zoneId,
            long userId,
            List<long> childsUsersIds);

         bool CompleteDeleteZones(long zoneId,
            List<long> childsUsersIds);

        #endregion

        #region Methods For Work With ZoneFiles

         List<ZoneFilesVM> GetListOfZoneFiles(int jtStartIndex,
             int jtPageSize,
             ref int listCount,
             List<long> childsUsersIds,
             long? zoneId = null,
             string zoneFileTitle = "",
             string zoneFileType = "",
             string jtSorting = null);



         bool AddToZoneFiles(List<ZoneFilesVM> zoneFilesVMList);

         ZoneFilesVM GetZoneFileWithZoneFileId(int zoneFileId,
            List<long> childsUsersIds);

         bool UpdateZoneFiles(ref ZoneFilesVM zoneFilesVM,
            List<long> childsUsersIds);

         bool ToggleActivationZoneFiles(int zoneFileId,
            long userId,
            List<long> childsUsersIds);

         bool TemporaryDeleteZoneFiles(int zoneFileId,
            long userId,
            List<long> childsUsersIds);

         bool CompleteDeleteZoneFiles(int zoneFileId,
            List<long> childsUsersIds);

        #endregion

        #endregion
    }
}
