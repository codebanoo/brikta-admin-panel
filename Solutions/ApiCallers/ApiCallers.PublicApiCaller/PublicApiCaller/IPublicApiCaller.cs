using ApiCallers.BaseApiCaller;
using VM.PVM.Public;

namespace ApiCallers.PublicApiCaller
{
    public interface IPublicApiCaller
    {

        #region CountriesManagement

        ResponseApiCaller GetListOfCountries(GetListOfCountriesPVM getListOfCountriesPVM);

        #endregion

        #region CitiesManagement

        ResponseApiCaller GetAllCitiesList(GetAllCitiesListPVM getAllCitiesListPVM);

        ResponseApiCaller GetListOfCities(GetListOfCitiesPVM getListOfCitiesPVM);

        ResponseApiCaller AddToCities(AddToCitiesPVM addToCitiesPVM);

        ResponseApiCaller GetCityWithCityId(GetCityWithCityIdPVM getCityWithCityIdPVM);

        ResponseApiCaller UpdateCities(UpdateCitiesPVM updateCitiesPVM);

        ResponseApiCaller ToggleActivationCities(ToggleActivationCitiesPVM toggleActivationCitiesPVM);

        ResponseApiCaller TemporaryDeleteCities(TemporaryDeleteCitiesPVM temporaryDeleteCitiesPVM);

        ResponseApiCaller CompleteDeleteCities(CompleteDeleteCitiesPVM completeDeleteCitiesPVM);

        #endregion

        #region ConstructionItemsManagement

        ResponseApiCaller GetAllConstructionItemsList(GetAllConstructionItemsListPVM getAllConstructionItemsListPVM);

        ResponseApiCaller GetListOfConstructionItems(GetListOfConstructionItemsPVM getListOfConstructionItemsPVM);

        ResponseApiCaller AddToConstructionItems(AddToConstructionItemsPVM addToConstructionItemsPVM);

        ResponseApiCaller GetConstructionItemWithConstructionItemId(GetConstructionItemWithConstructionItemIdPVM getConstructionItemWithConstructionItemIdPVM);

        ResponseApiCaller UpdateConstructionItems(UpdateConstructionItemsPVM updateConstructionItemsPVM);

        ResponseApiCaller ToggleActivationConstructionItems(ToggleActivationConstructionItemsPVM toggleActivationConstructionItemsPVM);

        ResponseApiCaller TemporaryDeleteConstructionItems(TemporaryDeleteConstructionItemsPVM temporaryDeleteConstructionItemsPVM);

        ResponseApiCaller CompleteDeleteConstructionItems(CompleteDeleteConstructionItemsPVM completeDeleteConstructionItemsPVM);

        #endregion

        #region ConstructionSubItemsManagement

        ResponseApiCaller GetAllConstructionSubItemsList(GetAllConstructionSubItemsListPVM getAllConstructionSubItemsListPVM);

        ResponseApiCaller GetListOfConstructionSubItems(GetListOfConstructionSubItemsPVM getListOfConstructionSubItemsPVM);

        ResponseApiCaller AddToConstructionSubItems(AddToConstructionSubItemsPVM addToConstructionSubItemsPVM);

        ResponseApiCaller GetConstructionSubItemWithConstructionSubItemId(GetConstructionSubItemWithConstructionSubItemIdPVM getConstructionSubItemWithConstructionSubItemIdPVM);

        ResponseApiCaller UpdateConstructionSubItems(UpdateConstructionSubItemsPVM updateConstructionSubItemsPVM);

        ResponseApiCaller ToggleActivationConstructionSubItems(ToggleActivationConstructionSubItemsPVM toggleActivationConstructionSubItemsPVM);

        ResponseApiCaller TemporaryDeleteConstructionSubItems(TemporaryDeleteConstructionSubItemsPVM temporaryDeleteConstructionSubItemsPVM);

        ResponseApiCaller CompleteDeleteConstructionSubItems(CompleteDeleteConstructionSubItemsPVM completeDeleteConstructionSubItemsPVM);

        #endregion

        #region ConstructionItemPricesHistoriesManagement

        ResponseApiCaller GetAllConstructionItemPricesHistoriesList(GetAllConstructionItemPricesHistoriesListPVM getAllConstructionItemPricesHistoriesListPVM);

        ResponseApiCaller GetListOfConstructionItemPricesHistories(GetListOfConstructionItemPricesHistoriesPVM getListOfConstructionItemPricesHistoriesPVM);

        ResponseApiCaller AddToConstructionItemPricesHistories(AddToConstructionItemPricesHistoriesPVM addToConstructionItemPricesHistoriesPVM);

        ResponseApiCaller GetConstructionItemHistoryWithConstructionItemHistoryId(GetItemPriceHistoryWithItemPriceHistoryIdPVM getItemPriceHistoryWithItemPriceHistoryIdPVM);

        ResponseApiCaller UpdateConstructionItemPricesHistories(UpdateConstructionItemPricesHistoriesPVM updateConstructionItemPricesHistoriesPVM);

        ResponseApiCaller ToggleActivationConstructionItemPricesHistories(ToggleActivationConstructionItemPricesHistoriesPVM toggleActivationConstructionItemPricesHistoriesPVM);

        ResponseApiCaller TemporaryDeleteConstructionItemPricesHistories(TemporaryDeleteConstructionItemPricesHistoriesPVM temporaryDeleteConstructionItemPricesHistoriesPVM);

        ResponseApiCaller CompleteDeleteConstructionItemPricesHistories(CompleteDeleteConstructionItemPricesHistoriesPVM completeDeleteConstructionItemPricesHistoriesPVM);

        #endregion

        #region DistrictsManagement
        ResponseApiCaller GetAllDistrictsList(GetAllDistrictsListPVM getAllDistrictsListPVM);
        ResponseApiCaller GetListOfDistricts(GetListOfDistrictsPVM getListOfDistrictsPVM);
        ResponseApiCaller AddToDistricts(AddToDistrictsPVM addToDistrictsPVM);
        ResponseApiCaller GetDistrictWithDistrictId(GetDistrictWithDistrictIdPVM getDistrictWithDistrictIdPVM);
        ResponseApiCaller UpdateDistricts(UpdateDistrictsPVM updateDistrictsPVM);
        ResponseApiCaller ToggleActivationDistricts(ToggleActivationDistrictsPVM toggleActivationDistrictsPVM);
        ResponseApiCaller TemporaryDeleteDistricts(TemporaryDeleteDistrictsPVM temporaryDeleteDistrictsPVM);
        ResponseApiCaller CompleteDeleteDistricts(CompleteDeleteDistrictsPVM completeDeleteDistrictsPVM);
        #endregion

        #region DistrictFilesManagement
        ResponseApiCaller GetListOfDistrictFiles(GetListOfDistrictFilesPVM getListOfDistrictFilesPVM);
        ResponseApiCaller AddToDistrictFiles(AddToDistrictFilesPVM addToDistrictFilesPVM);
        ResponseApiCaller GetDistrictFileWithDistrictFileId(GetDistrictFileWithDistrictFileIdPVM getDistrictFileWithDistrictFileIdPVM);
        ResponseApiCaller UpdateDistrictFiles(UpdateDistrictFilesPVM updateDistrictFilesPVM);
        ResponseApiCaller ToggleActivationDistrictFiles(ToggleActivationDistrictFilesPVM toggleActivationDistrictFilesPVM);
        ResponseApiCaller TemporaryDeleteDistrictFiles(TemporaryDeleteDistrictFilesPVM temporaryDeleteDistrictFilesPVM);
        ResponseApiCaller CompleteDeleteDistrictFiles(CompleteDeleteDistrictFilesPVM completeDeleteDistrictFilesPVM);
        #endregion

        #region ElementTypesManagement

        ResponseApiCaller GetAllElementTypesList(GetAllElementTypesListPVM getAllElementTypesListPVM);

        #endregion

        #region FormUsagesManagement

        ResponseApiCaller GetAllFormUsagesList(GetAllFormUsagesListPVM getAllUsagesListPVM);
        ResponseApiCaller GetListOfFormUsages(GetListOfFormUsagesPVM getListOfUsagesPVM);
        ResponseApiCaller AddToFormUsages(AddToFormUsagesPVM addToUsagesPVM);
        ResponseApiCaller UpdateFormUsages(UpdateFormUsagesPVM updateUsagesPVM);
        ResponseApiCaller ToggleActivationFormUsages(ToggleActivationFormUsagesPVM toggleActivationUsagesPVM);
        ResponseApiCaller TemporaryDeleteFormUsages(TemporaryDeleteFormUsagesPVM temporaryDeleteUsagesPVM);
        ResponseApiCaller CompleteDeleteFormUsages(CompleteDeleteFormUsagesPVM completeDeleteUsagesPVM);
        ResponseApiCaller GetFormUsageWithFormUsageId(GetFormUsageWithFormUsageIdPVM getUsageWithUsageIdPVM);

        #endregion

        #region GuildCategoriesManagement
        ResponseApiCaller GetAllGuildCategoriesList(GetAllGuildCategoriesListPVM getAllGuildCategoriesListPVM);

        #endregion

        #region IndicesManagement

        ResponseApiCaller CompleteDeleteIndices(CompleteDeleteIndicesPVM completeDeleteIndicesPVM);
        ResponseApiCaller TemporaryDeleteIndices(TemporaryDeleteIndicesPVM temporaryDeleteIndicesPVM);
        ResponseApiCaller ToggleActivationIndices(ToggleActivationIndicesPVM toggleActivationIndicesPVM);
        ResponseApiCaller UpdateIndices(UpdateIndicesPVM updateIndicesPVM);
        ResponseApiCaller AddToIndices(AddToIndicesPVM addToIndicesPVM);
        ResponseApiCaller GetListOfIndices(GetListOfIndicesPVM getListOfIndicesPVM);
        ResponseApiCaller GetAllIndicesList(GetAllIndicesListPVM getAllIndicesListPVM);

        #endregion

        #region PersonsManagement

        ResponseApiCaller GetListOfPersons(GetListOfPersonsPVM getListOfPersonsPVM);

        ResponseApiCaller AddToPersons(AddToPersonsPVM addToPersonsPVM);

        ResponseApiCaller UpdatePersons(UpdatePersonsPVM updatePersonsPVM);

        ResponseApiCaller ToggleActivationPersons(ToggleActivationPersonsPVM toggleActivationPersonsPVM);

        ResponseApiCaller TemporaryDeletePersons(TemporaryDeletePersonsPVM temporaryDeletePersonsPVM);

        ResponseApiCaller CompleteDeletePersons(CompleteDeletePersonsPVM completeDeletePersonsPVM);

        ResponseApiCaller GetAllPersonsList(GetAllPersonsListPVM getAllPersonsListPVM);

        ResponseApiCaller GetPersonWithMobileNumber(GetPersonWithMobileNumberPVM getPersonWithMobileNumberPVM);

        ResponseApiCaller GetPersonWithUserId(GetPersonWithUserIdPVM getPersonWithUserIdPVM);

        ResponseApiCaller GetAllPersonsListWithUsers(GetAllPersonsListPVM getAllPersonsListPVM);
        #endregion

        #region PersonTypesManagement

        ResponseApiCaller GetAllPersonTypesList(GetAllPersonTypesListPVM getAllPersonTypesListPVM);

        #endregion

        #region PricesListIndices
        ResponseApiCaller CompleteDeletePricesListIndices(CompleteDeletePricesListIndicesPVM completeDeletePricesListIndicesPVM);
        ResponseApiCaller TemporaryDeletePricesListIndices(TemporaryDeletePricesListIndicesPVM temporaryDeletePricesListIndicesPVM);
        ResponseApiCaller ToggleActivationPricesListIndices(ToggleActivationPricesListIndicesPVM toggleActivationPricesListIndicesPVM);
        ResponseApiCaller UpdatePricesListIndices(UpdatePricesListIndicesPVM updatePricesListIndicesPVM);

        ResponseApiCaller AddToPricesListIndices(AddToPricesListIndicesPVM addToPricesListIndicesPVM);
        ResponseApiCaller GetListOfPricesListIndices(GetListOfPricesListIndicesPVM getListOfPricesListIndicesPVM);
        ResponseApiCaller GetAllPricesListIndicesList(GetAllPricesListIndicesListPVM getAllPricesListIndicesListPVM);



        #endregion

        #region StatesManagement

        ResponseApiCaller GetListOfStates(GetListOfStatesPVM getListOfStatesPVM);

        #endregion

        #region SmsSendersManagement

        ResponseApiCaller GetListOfSmsSenders(GetListOfSmsSendersPVM getListOfSmsSendersPVM);

        ResponseApiCaller AddToSmsSenders(AddToSmsSendersPVM addToSmsSendersPVM);

        ResponseApiCaller GetDefaultSmsSender();

        ResponseApiCaller UpdateSmsSenders(UpdateSmsSendersPVM updateSmsSendersPVM);

        ResponseApiCaller ToggleActivationSmsSenders(ToggleActivationSmsSendersPVM toggleActivationSmsSendersPVM);

        ResponseApiCaller TemporaryDeleteSmsSenders(TemporaryDeleteSmsSendersPVM temporaryDeleteSmsSendersPVM);

        ResponseApiCaller CompleteDeleteSmsSenders(CompleteDeleteSmsSendersPVM completeDeleteSmsSendersPVM);

        ResponseApiCaller GetAllSmsSendersList(GetAllSmsSendersListPVM getAllSmsSendersListPVM);

        #endregion

        #region TypeOfUseLayersManagement

        ResponseApiCaller GetAllTypeOfUseLayersList(GetAllTypeOfUseLayersListPVM getAllTypeOfUseLayersListPVM);

        #endregion

        #region UnitsOfMeasurementManagement

        ResponseApiCaller GetAllUnitsOfMeasurementList(GetAllUnitsOfMeasurementListPVM getAllUnitsOfMeasurementListPVM);

        #endregion

        #region ZonesManagement

        ResponseApiCaller GetAllZonesList(GetAllZonesListPVM getAllZonesListPVM);

        ResponseApiCaller GetListOfZones(GetListOfZonesPVM getListOfZonesPVM);

        ResponseApiCaller AddToZones(AddToZonesPVM addToZonesPVM);

        ResponseApiCaller GetZoneWithZoneId(GetZoneWithZoneIdPVM getZoneWithZoneIdPVM);

        ResponseApiCaller UpdateZones(UpdateZonesPVM updateZonesPVM);

        ResponseApiCaller ToggleActivationZones(ToggleActivationZonesPVM toggleActivationZonesPVM);

        ResponseApiCaller TemporaryDeleteZones(TemporaryDeleteZonesPVM temporaryDeleteZonesPVM);

        ResponseApiCaller CompleteDeleteZones(CompleteDeleteZonesPVM completeDeleteZonesPVM);

        #endregion

        #region ZoneFilesManagement

        public ResponseApiCaller GetListOfZoneFiles(GetListOfZoneFilesPVM getListOfZoneFilesPVM);

        public ResponseApiCaller AddToZoneFiles(AddToZoneFilesPVM addToZoneFilesPVM);

        public ResponseApiCaller GetZoneFileWithZoneFileId(GetZoneFileWithZoneFileIdPVM getZoneFileWithZoneFileIdPVM);

        public ResponseApiCaller UpdateZoneFiles(UpdateZoneFilesPVM updateZoneFilesPVM);

        public ResponseApiCaller ToggleActivationZoneFiles(ToggleActivationZoneFilesPVM toggleActivationZoneFilesPVM);

        public ResponseApiCaller TemporaryDeleteZoneFiles(TemporaryDeleteZoneFilesPVM temporaryDeleteZoneFilesPVM);

        public ResponseApiCaller CompleteDeleteZoneFiles(CompleteDeleteZoneFilesPVM completeDeleteZoneFilesPVM);


        #endregion

    }
}
