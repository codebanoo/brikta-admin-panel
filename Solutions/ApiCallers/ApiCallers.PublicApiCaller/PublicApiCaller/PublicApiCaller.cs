using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using ApiCallers.BaseApiCaller;
using VM.PVM.Public;
using VM.Base;
using VM.PVM.Base;

namespace ApiCallers.PublicApiCaller
{
    public class PublicApiCaller : BaseCaller, IPublicApiCaller
    {
        public PublicApiCaller() : base()
        {
        }

        public PublicApiCaller(string _serviceUrl) : base(_serviceUrl)
        {
            serviceUrl = _serviceUrl;
        }

        public PublicApiCaller(string _serviceUrl,
            string _accessToken) :
            base(_serviceUrl,
                _accessToken)
        {
            serviceUrl = _serviceUrl;
        }


        #region CountriesManagement

        public ResponseApiCaller GetListOfCountries(GetListOfCountriesPVM getListOfCountriesPVM)
        {
            return GetRecords(getListOfCountriesPVM);
        }

        #endregion

        #region CitiesManagement

        public ResponseApiCaller GetAllCitiesList(GetAllCitiesListPVM getAllCitiesListPVM)
        {
            return GetRecords(getAllCitiesListPVM);
        }

        public ResponseApiCaller GetListOfCities(GetListOfCitiesPVM getListOfCitiesPVM)
        {
            return GetRecords(getListOfCitiesPVM);
        }

        public ResponseApiCaller AddToCities(AddToCitiesPVM addToCitiesPVM)
        {
            return GetRecord(addToCitiesPVM);
        }

        public ResponseApiCaller GetCityWithCityId(GetCityWithCityIdPVM getCityWithCityIdPVM)
        {
            return GetRecord(getCityWithCityIdPVM);
        }

        public ResponseApiCaller UpdateCities(UpdateCitiesPVM updateCitiesPVM)
        {
            return GetRecord(updateCitiesPVM);
        }

        public ResponseApiCaller ToggleActivationCities(ToggleActivationCitiesPVM toggleActivationCitiesPVM)
        {
            return GetResult(toggleActivationCitiesPVM);
        }

        public ResponseApiCaller TemporaryDeleteCities(TemporaryDeleteCitiesPVM temporaryDeleteCitiesPVM)
        {
            return GetResult(temporaryDeleteCitiesPVM);
        }

        public ResponseApiCaller CompleteDeleteCities(CompleteDeleteCitiesPVM completeDeleteCitiesPVM)
        {
            return GetResult(completeDeleteCitiesPVM);
        }

        #endregion

        #region ConstructionItemsManagement

        public ResponseApiCaller GetAllConstructionItemsList(GetAllConstructionItemsListPVM getAllConstructionItemsListPVM)
        {
            return GetRecords(getAllConstructionItemsListPVM);
        }

        public ResponseApiCaller GetListOfConstructionItems(GetListOfConstructionItemsPVM getListOfConstructionItemsPVM)
        {
            return GetRecords(getListOfConstructionItemsPVM);
        }

        public ResponseApiCaller AddToConstructionItems(AddToConstructionItemsPVM addToConstructionItemsPVM)
        {
            return GetRecord(addToConstructionItemsPVM);
        }

        public ResponseApiCaller GetConstructionItemWithConstructionItemId(GetConstructionItemWithConstructionItemIdPVM getConstructionItemWithConstructionItemIdPVM)
        {
            return GetRecord(getConstructionItemWithConstructionItemIdPVM);
        }

        public ResponseApiCaller UpdateConstructionItems(UpdateConstructionItemsPVM updateConstructionItemsPVM)
        {
            return GetRecord(updateConstructionItemsPVM);
        }

        public ResponseApiCaller ToggleActivationConstructionItems(ToggleActivationConstructionItemsPVM toggleActivationConstructionItemsPVM)
        {
            return GetResult(toggleActivationConstructionItemsPVM);
        }

        public ResponseApiCaller TemporaryDeleteConstructionItems(TemporaryDeleteConstructionItemsPVM temporaryDeleteConstructionItemsPVM)
        {
            return GetResult(temporaryDeleteConstructionItemsPVM);
        }

        public ResponseApiCaller CompleteDeleteConstructionItems(CompleteDeleteConstructionItemsPVM completeDeleteConstructionItemsPVM)
        {
            return GetResult(completeDeleteConstructionItemsPVM);
        }

        #endregion

        #region ConstructionSubItemsManagement

        public ResponseApiCaller GetAllConstructionSubItemsList(GetAllConstructionSubItemsListPVM getAllConstructionSubItemsListPVM)
        {
            return GetRecords(getAllConstructionSubItemsListPVM);
        }

        public ResponseApiCaller GetListOfConstructionSubItems(GetListOfConstructionSubItemsPVM getListOfConstructionSubItemsPVM)
        {
            return GetRecords(getListOfConstructionSubItemsPVM);
        }

        public ResponseApiCaller AddToConstructionSubItems(AddToConstructionSubItemsPVM addToConstructionSubItemsPVM)
        {
            return GetRecord(addToConstructionSubItemsPVM);
        }

        public ResponseApiCaller GetConstructionSubItemWithConstructionSubItemId(GetConstructionSubItemWithConstructionSubItemIdPVM getConstructionSubItemWithConstructionSubItemIdPVM)
        {
            return GetRecord(getConstructionSubItemWithConstructionSubItemIdPVM);
        }

        public ResponseApiCaller UpdateConstructionSubItems(UpdateConstructionSubItemsPVM updateConstructionSubItemsPVM)
        {
            return GetRecord(updateConstructionSubItemsPVM);
        }

        public ResponseApiCaller ToggleActivationConstructionSubItems(ToggleActivationConstructionSubItemsPVM toggleActivationConstructionSubItemsPVM)
        {
            return GetResult(toggleActivationConstructionSubItemsPVM);
        }

        public ResponseApiCaller TemporaryDeleteConstructionSubItems(TemporaryDeleteConstructionSubItemsPVM temporaryDeleteConstructionSubItemsPVM)
        {
            return GetResult(temporaryDeleteConstructionSubItemsPVM);
        }

        public ResponseApiCaller CompleteDeleteConstructionSubItems(CompleteDeleteConstructionSubItemsPVM completeDeleteConstructionSubItemsPVM)
        {
            return GetResult(completeDeleteConstructionSubItemsPVM);
        }

        #endregion

        #region ConstructionItemPricesHistoriesManagement

        public ResponseApiCaller GetAllConstructionItemPricesHistoriesList(GetAllConstructionItemPricesHistoriesListPVM getAllConstructionItemPricesHistoriesListPVM)
        {
            return GetRecords(getAllConstructionItemPricesHistoriesListPVM);
        }

        public ResponseApiCaller GetListOfConstructionItemPricesHistories(GetListOfConstructionItemPricesHistoriesPVM getListOfConstructionItemPricesHistoriesPVM)
        {
            return GetRecords(getListOfConstructionItemPricesHistoriesPVM);
        }

        public ResponseApiCaller AddToConstructionItemPricesHistories(AddToConstructionItemPricesHistoriesPVM addToConstructionItemPricesHistoriesPVM)
        {
            return GetRecord(addToConstructionItemPricesHistoriesPVM);
        }

        public ResponseApiCaller GetConstructionItemHistoryWithConstructionItemHistoryId(GetItemPriceHistoryWithItemPriceHistoryIdPVM getItemPriceHistoryWithItemPriceHistoryIdPVM)
        {
            return GetRecord(getItemPriceHistoryWithItemPriceHistoryIdPVM);
        }

        public ResponseApiCaller UpdateConstructionItemPricesHistories(UpdateConstructionItemPricesHistoriesPVM updateConstructionItemPricesHistoriesPVM)
        {
            return GetRecord(updateConstructionItemPricesHistoriesPVM);
        }

        public ResponseApiCaller ToggleActivationConstructionItemPricesHistories(ToggleActivationConstructionItemPricesHistoriesPVM toggleActivationConstructionItemPricesHistoriesPVM)
        {
            return GetResult(toggleActivationConstructionItemPricesHistoriesPVM);
        }

        public ResponseApiCaller TemporaryDeleteConstructionItemPricesHistories(TemporaryDeleteConstructionItemPricesHistoriesPVM temporaryDeleteConstructionItemPricesHistoriesPVM)
        {
            return GetResult(temporaryDeleteConstructionItemPricesHistoriesPVM);
        }

        public ResponseApiCaller CompleteDeleteConstructionItemPricesHistories(CompleteDeleteConstructionItemPricesHistoriesPVM completeDeleteConstructionItemPricesHistoriesPVM)
        {
            return GetResult(completeDeleteConstructionItemPricesHistoriesPVM);
        }

        #endregion

        #region DistrictsManagement

        public ResponseApiCaller GetAllDistrictsList(GetAllDistrictsListPVM getAllDistrictsListPVM)
        {
            return GetRecords(getAllDistrictsListPVM);
        }

        public ResponseApiCaller GetListOfDistricts(GetListOfDistrictsPVM getListOfDistrictsPVM)
        {
            return GetRecords(getListOfDistrictsPVM);
        }

        public ResponseApiCaller AddToDistricts(AddToDistrictsPVM addToDistrictsPVM)
        {
            return GetRecord(addToDistrictsPVM);
        }

        public ResponseApiCaller GetDistrictWithDistrictId(GetDistrictWithDistrictIdPVM getDistrictWithDistrictIdPVM)
        {
            return GetRecord(getDistrictWithDistrictIdPVM);
        }

        public ResponseApiCaller UpdateDistricts(UpdateDistrictsPVM updateDistrictsPVM)
        {
            return GetRecord(updateDistrictsPVM);
        }

        public ResponseApiCaller ToggleActivationDistricts(ToggleActivationDistrictsPVM toggleActivationDistrictsPVM)
        {
            return GetResult(toggleActivationDistrictsPVM);
        }

        public ResponseApiCaller TemporaryDeleteDistricts(TemporaryDeleteDistrictsPVM temporaryDeleteDistrictsPVM)
        {
            return GetResult(temporaryDeleteDistrictsPVM);
        }

        public ResponseApiCaller CompleteDeleteDistricts(CompleteDeleteDistrictsPVM completeDeleteDistrictsPVM)
        {
            return GetResult(completeDeleteDistrictsPVM);
        }
        #endregion

        #region DistrictFilesManagement
        public ResponseApiCaller GetListOfDistrictFiles(GetListOfDistrictFilesPVM getListOfDistrictFilesPVM)
        {
            return GetRecords(getListOfDistrictFilesPVM);
        }

        public ResponseApiCaller AddToDistrictFiles(AddToDistrictFilesPVM addToDistrictFilesPVM)
        {
            return GetRecord(addToDistrictFilesPVM);
        }

        public ResponseApiCaller GetDistrictFileWithDistrictFileId(GetDistrictFileWithDistrictFileIdPVM getDistrictFileWithDistrictFileIdPVM)
        {
            return GetRecord(getDistrictFileWithDistrictFileIdPVM);
        }

        public ResponseApiCaller UpdateDistrictFiles(UpdateDistrictFilesPVM updateDistrictFilesPVM)
        {
            return GetRecord(updateDistrictFilesPVM);
        }

        public ResponseApiCaller ToggleActivationDistrictFiles(ToggleActivationDistrictFilesPVM toggleActivationDistrictFilesPVM)
        {
            return GetResult(toggleActivationDistrictFilesPVM);
        }

        public ResponseApiCaller TemporaryDeleteDistrictFiles(TemporaryDeleteDistrictFilesPVM temporaryDeleteDistrictFilesPVM)
        {
            return GetResult(temporaryDeleteDistrictFilesPVM);
        }

        public ResponseApiCaller CompleteDeleteDistrictFiles(CompleteDeleteDistrictFilesPVM completeDeleteDistrictFilesPVM)
        {
            return GetResult(completeDeleteDistrictFilesPVM);
        }

        #endregion

        #region ElementTypesManagement

        public ResponseApiCaller GetAllElementTypesList(GetAllElementTypesListPVM getAllElementTypesListPVM)
        {
            return GetRecords(getAllElementTypesListPVM);
        }
        #endregion

        #region FormUsagesManagement

        public ResponseApiCaller GetAllFormUsagesList(GetAllFormUsagesListPVM getAllUsagesListPVM)
        {
            return GetRecords(getAllUsagesListPVM);
        }

        public ResponseApiCaller GetListOfFormUsages(GetListOfFormUsagesPVM getListOfUsagesPVM)
        {
            return GetRecords(getListOfUsagesPVM);
        }

        public ResponseApiCaller AddToFormUsages(AddToFormUsagesPVM addToUsagesPVM)
        {
            return GetRecord(addToUsagesPVM);
        }

        public ResponseApiCaller UpdateFormUsages(UpdateFormUsagesPVM updateUsagesPVM)
        {
            return GetRecord(updateUsagesPVM);
        }

        public ResponseApiCaller ToggleActivationFormUsages(ToggleActivationFormUsagesPVM toggleActivationUsagesPVM)
        {
            return GetResult(toggleActivationUsagesPVM);
        }

        public ResponseApiCaller TemporaryDeleteFormUsages(TemporaryDeleteFormUsagesPVM temporaryDeleteUsagesPVM)
        {
            return GetResult(temporaryDeleteUsagesPVM);
        }

        public ResponseApiCaller CompleteDeleteFormUsages(CompleteDeleteFormUsagesPVM completeDeleteUsagesPVM)
        {
            return GetResult(completeDeleteUsagesPVM);
        }

        public ResponseApiCaller GetFormUsageWithFormUsageId(GetFormUsageWithFormUsageIdPVM getUsageWithUsageIdPVM)
        {
            return GetRecord(getUsageWithUsageIdPVM);
        }

        #endregion

        #region GuildCategoriesManagement
        public ResponseApiCaller GetAllGuildCategoriesList(GetAllGuildCategoriesListPVM getAllGuildCategoriesListPVM)
        {
            return GetRecords(getAllGuildCategoriesListPVM);
        }
        #endregion

        #region IndicesManagement

        public ResponseApiCaller CompleteDeleteIndices(CompleteDeleteIndicesPVM completeDeleteIndicesPVM)
        {
            return GetResult(completeDeleteIndicesPVM);
        }

        public ResponseApiCaller TemporaryDeleteIndices(TemporaryDeleteIndicesPVM temporaryDeleteIndicesPVM)
        {
            return GetResult(temporaryDeleteIndicesPVM);
        }

        public ResponseApiCaller ToggleActivationIndices(ToggleActivationIndicesPVM toggleActivationIndicesPVM)
        {
            return GetResult(toggleActivationIndicesPVM);
        }

        public ResponseApiCaller UpdateIndices(UpdateIndicesPVM updateIndicesPVM)
        {
            return GetRecord(updateIndicesPVM);
        }

        public ResponseApiCaller AddToIndices(AddToIndicesPVM addToIndicesPVM)
        {
            return GetRecord(addToIndicesPVM);
        }

        public ResponseApiCaller GetListOfIndices(GetListOfIndicesPVM getListOfIndicesPVM)
        {
            return GetRecords(getListOfIndicesPVM);
        }

        public ResponseApiCaller GetAllIndicesList(GetAllIndicesListPVM getAllIndicesListPVM)
        {
            return GetRecords(getAllIndicesListPVM);
        }

        #endregion

        #region PersonsManagement

        public ResponseApiCaller GetListOfPersons(GetListOfPersonsPVM getListOfPersonsPVM)
        {
            return GetRecords(getListOfPersonsPVM);
        }

        public ResponseApiCaller AddToPersons(AddToPersonsPVM addToPersonsPVM)
        {
            return GetRecord(addToPersonsPVM);
        }

        public ResponseApiCaller UpdatePersons(UpdatePersonsPVM updatePersonsPVM)
        {
            return GetRecord(updatePersonsPVM);
        }

        public ResponseApiCaller ToggleActivationPersons(ToggleActivationPersonsPVM toggleActivationPersonsPVM)
        {
            return GetResult(toggleActivationPersonsPVM);
        }

        public ResponseApiCaller TemporaryDeletePersons(TemporaryDeletePersonsPVM temporaryDeletePersonsPVM)
        {
            return GetResult(temporaryDeletePersonsPVM);
        }

        public ResponseApiCaller CompleteDeletePersons(CompleteDeletePersonsPVM completeDeletePersonsPVM)
        {
            return GetResult(completeDeletePersonsPVM);
        }

        public ResponseApiCaller GetAllPersonsList(GetAllPersonsListPVM getAllPersonsListPVM)
        {
            return GetRecords(getAllPersonsListPVM);
        }

        public ResponseApiCaller GetPersonWithMobileNumber(GetPersonWithMobileNumberPVM getPersonWithMobileNumberPVM)
        {
            return GetRecord(getPersonWithMobileNumberPVM);
        }

        public ResponseApiCaller GetPersonWithUserId(GetPersonWithUserIdPVM getPersonWithUserIdPVM)
        {
            return GetRecord(getPersonWithUserIdPVM);
        }

        public ResponseApiCaller GetAllPersonsListWithUsers(GetAllPersonsListPVM getAllPersonsListPVM)
        {
            return GetRecords(getAllPersonsListPVM);
        }

        #endregion

        #region PersonTypesManagement

        public ResponseApiCaller GetAllPersonTypesList(GetAllPersonTypesListPVM getAllPersonTypesListPVM)
        {
            return GetRecords(getAllPersonTypesListPVM);
        }

        #endregion

        #region PricesListIndices

        public ResponseApiCaller CompleteDeletePricesListIndices(CompleteDeletePricesListIndicesPVM completeDeletePricesListIndicesPVM)
        {
            return GetResult(completeDeletePricesListIndicesPVM);
        }

        public ResponseApiCaller TemporaryDeletePricesListIndices(TemporaryDeletePricesListIndicesPVM temporaryDeletePricesListIndicesPVM)
        {
            return GetResult(temporaryDeletePricesListIndicesPVM);
        }

        public ResponseApiCaller ToggleActivationPricesListIndices(ToggleActivationPricesListIndicesPVM toggleActivationPricesListIndicesPVM)
        {
            return GetResult(toggleActivationPricesListIndicesPVM);
        }

        public ResponseApiCaller UpdatePricesListIndices(UpdatePricesListIndicesPVM updatePricesListIndicesPVM)
        {
            return GetRecord(updatePricesListIndicesPVM);
        }

        public ResponseApiCaller AddToPricesListIndices(AddToPricesListIndicesPVM addToPricesListIndicesPVM)
        {
            return GetRecord(addToPricesListIndicesPVM);
        }

        public ResponseApiCaller GetListOfPricesListIndices(GetListOfPricesListIndicesPVM getListOfPricesListIndicesPVM)
        {
            return GetRecords(getListOfPricesListIndicesPVM);
        }

        public ResponseApiCaller GetAllPricesListIndicesList(GetAllPricesListIndicesListPVM getAllPricesListIndicesListPVM)
        {
            return GetRecords(getAllPricesListIndicesListPVM);
        }

        #endregion

        #region StatesManagement

        public ResponseApiCaller GetListOfStates(GetListOfStatesPVM getListOfStatesPVM)
        {
            return GetRecords(getListOfStatesPVM);
        }

        #endregion

        #region SmsSendersManagement

        public ResponseApiCaller GetListOfSmsSenders(GetListOfSmsSendersPVM getListOfSmsSendersPVM)
        {
            return GetRecords(getListOfSmsSendersPVM);
        }

        public ResponseApiCaller AddToSmsSenders(AddToSmsSendersPVM addToSmsSendersPVM)
        {
            return GetRecord(addToSmsSendersPVM);
        }

        public ResponseApiCaller GetDefaultSmsSender()
        {
            return GetRecord("");
        }

        public ResponseApiCaller UpdateSmsSenders(UpdateSmsSendersPVM updateSmsSendersPVM)
        {
            return GetRecord(updateSmsSendersPVM);
        }

        public ResponseApiCaller ToggleActivationSmsSenders(ToggleActivationSmsSendersPVM toggleActivationSmsSendersPVM)
        {
            return GetResult(toggleActivationSmsSendersPVM);
        }

        public ResponseApiCaller TemporaryDeleteSmsSenders(TemporaryDeleteSmsSendersPVM temporaryDeleteSmsSendersPVM)
        {
            return GetResult(temporaryDeleteSmsSendersPVM);
        }

        public ResponseApiCaller CompleteDeleteSmsSenders(CompleteDeleteSmsSendersPVM completeDeleteSmsSendersPVM)
        {
            return GetResult(completeDeleteSmsSendersPVM);
        }

        public ResponseApiCaller GetAllSmsSendersList(GetAllSmsSendersListPVM getAllSmsSendersListPVM)
        {
            return GetRecords(getAllSmsSendersListPVM);
        }

        #endregion

        #region TypeOfUseLayersManagement

        public ResponseApiCaller GetAllTypeOfUseLayersList(GetAllTypeOfUseLayersListPVM getAllTypeOfUseLayersListPVM)
        {
            return GetRecords(getAllTypeOfUseLayersListPVM);
        }

        #endregion

        #region UnitsOfMeasurementManagement

        public ResponseApiCaller GetAllUnitsOfMeasurementList(GetAllUnitsOfMeasurementListPVM getAllUnitsOfMeasurementListPVM)
        {
            return GetRecords(getAllUnitsOfMeasurementListPVM);
        }

        #endregion

        #region ZonesManagement

        public ResponseApiCaller GetAllZonesList(GetAllZonesListPVM getAllZonesListPVM)
        {
            return GetRecords(getAllZonesListPVM);
        }

        public ResponseApiCaller GetListOfZones(GetListOfZonesPVM getListOfZonesPVM)
        {
            return GetRecords(getListOfZonesPVM);
        }

        public ResponseApiCaller AddToZones(AddToZonesPVM addToZonesPVM)
        {
            return GetRecord(addToZonesPVM);
        }

        public ResponseApiCaller GetZoneWithZoneId(GetZoneWithZoneIdPVM getZoneWithZoneIdPVM)
        {
            return GetRecord(getZoneWithZoneIdPVM);
        }

        public ResponseApiCaller UpdateZones(UpdateZonesPVM updateZonesPVM)
        {
            return GetRecord(updateZonesPVM);
        }

        public ResponseApiCaller ToggleActivationZones(ToggleActivationZonesPVM toggleActivationZonesPVM)
        {
            return GetResult(toggleActivationZonesPVM);
        }

        public ResponseApiCaller TemporaryDeleteZones(TemporaryDeleteZonesPVM temporaryDeleteZonesPVM)
        {
            return GetResult(temporaryDeleteZonesPVM);
        }

        public ResponseApiCaller CompleteDeleteZones(CompleteDeleteZonesPVM completeDeleteZonesPVM)
        {
            return GetResult(completeDeleteZonesPVM);
        }

        #endregion

        #region ZoneFilesManagement

        public ResponseApiCaller GetListOfZoneFiles(GetListOfZoneFilesPVM getListOfZoneFilesPVM)
        {
            return GetRecords(getListOfZoneFilesPVM);
        }

        public ResponseApiCaller AddToZoneFiles(AddToZoneFilesPVM addToZoneFilesPVM)
        {
            return GetRecord(addToZoneFilesPVM);
        }

        public ResponseApiCaller GetZoneFileWithZoneFileId(GetZoneFileWithZoneFileIdPVM getZoneFileWithZoneFileIdPVM)
        {
            return GetRecord(getZoneFileWithZoneFileIdPVM);
        }

        public ResponseApiCaller UpdateZoneFiles(UpdateZoneFilesPVM updateZoneFilesPVM)
        {
            return GetRecord(updateZoneFilesPVM);
        }

        public ResponseApiCaller ToggleActivationZoneFiles(ToggleActivationZoneFilesPVM toggleActivationZoneFilesPVM)
        {
            return GetResult(toggleActivationZoneFilesPVM);
        }

        public ResponseApiCaller TemporaryDeleteZoneFiles(TemporaryDeleteZoneFilesPVM temporaryDeleteZoneFilesPVM)
        {
            return GetResult(temporaryDeleteZoneFilesPVM);
        }

        public ResponseApiCaller CompleteDeleteZoneFiles(CompleteDeleteZoneFilesPVM completeDeleteZoneFilesPVM)
        {
            return GetResult(completeDeleteZoneFilesPVM);
        }

        #endregion

    }
}
