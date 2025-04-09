using ApiCallers.BaseApiCaller;
using System.Collections.Generic;
using VM.PVM.Melkavan;
using VM.PVM.Teniaco;
using VM.Teniaco;

namespace ApiCallers.TeniacoApiCaller
{
    public interface ITeniacoApiCaller
    {
        #region AgenciesManagement
        ResponseApiCaller GetAllAgenciesList(GetAllAgenciesListPVM getAllAgenciesListPVM);

        ResponseApiCaller GetListOfAgencies(GetListOfAgenciesPVM getListOfAgenciesPVM);

        ResponseApiCaller AddToAgencies(AddToAgenciesPVM addToAgenciesPVM);

        ResponseApiCaller UpdateAgencies(UpdateAgenciesPVM updateAgenciesPVM);

        ResponseApiCaller ToggleActivationAgencies(ToggleActivationAgenciesPVM toggleActivationAgenciesPVM);

        ResponseApiCaller TemporaryDeleteAgencies(TemporaryDeleteAgenciesPVM temporaryDeleteAgenciesPVM);

        ResponseApiCaller CompleteDeleteAgencies(CompleteDeleteAgenciesPVM completeDeleteAgenciesPVM);

        ResponseApiCaller GetAgencyWithAgencyId(GetAgencyWithAgencyIdPVM getAgencyWithAgencyIdPVM);

        #endregion

        #region AgencyLocationManagement
        ResponseApiCaller UpdateAgencyLocation(UpdateAgencyLocationPVM updateAgencyLocationPVM);
        #endregion

        #region AgencyStaffsManagement

        ResponseApiCaller GetAllAgencyStaffsList(GetAllAgencyStaffsListPVM getAllAgencyStaffsListPVM);

        ResponseApiCaller GetListOfAgencyStaffs(GetListOfAgencyStaffsPVM getListOfAgencyStaffsPVM);

        ResponseApiCaller AddToAgencyStaffs(AddToAgencyStaffsPVM addToAgencyStaffsPVM);

        ResponseApiCaller UpdateAgencyStaffs(UpdateAgencyStaffsPVM updateAgencyStaffsPVM);

        ResponseApiCaller ToggleActivationAgencyStaffs(ToggleActivationAgencyStaffsPVM toggleActivationAgencyStaffsPVM);

        ResponseApiCaller TemporaryDeleteAgencyStaffs(TemporaryDeleteAgencyStaffsPVM temporaryDeleteAgencyStaffsPVM);

        ResponseApiCaller CompleteDeleteAgencyStaffs(CompleteDeleteAgencyStaffsPVM completeDeleteAgencyStaffsPVM);

        ResponseApiCaller GetAgencyStaffsWithAgencyStaffId(GetAgencyStaffWithAgencyStaffIdPVM getAgencyStaffsWithAgencyStaffIdPVM);

        #endregion

        #region ComparePropertiesManagement

        ResponseApiCaller GetAllComparePropertiesListByPersonId(GetAllComparePropertiesListByPersonIdPVM getAllComparePropertiesListByPersonIdPVM);

        ResponseApiCaller GetListOfComparePropertiesForBasicInfo(GetListOfComparePropertiesForBasicInfoPVM getListOfComparePropertiesForBasicInfoPVM);

        ResponseApiCaller GetListOfCompareFeatureValues(GetListOfCompareFeatureValuesPVM getListOfCompareFeatureValuesPVM);


        ResponseApiCaller GetListOfComparePropertiesAddress(GetListOfComparePropertiesAddressPVM getListOfComparePropertiesAddressPVM);

        ResponseApiCaller GetListOfComparePropertiesPricesHistories(GetListOfComparePropertiesPricesHistoriesPVM getListOfComparePropertiesPricesHistoriesPVM);

        #endregion

        #region ContractorsManagement
        ResponseApiCaller GetAllContractorsList(GetAllContractorsListPVM getAllContractorsListPVM);
        ResponseApiCaller GetListOfContractors(GetListOfContractorsPVM getListOfContractorsPVM);
        ResponseApiCaller AddToContractors(AddToContractorsPVM addToContractorsPVM);
        ResponseApiCaller UpdateContractors(UpdateContractorsPVM updateContractorsPVM);
        ResponseApiCaller ToggleActivationContractors(ToggleActivationContractorsPVM toggleActivationContractorsPVM);
        ResponseApiCaller TemporaryDeleteContractors(TemporaryDeleteContractorsPVM temporaryDeleteContractorsPVM);
        ResponseApiCaller CompleteDeleteContractors(CompleteDeleteContractorsPVM completeDeleteContractorsPVM);
        ResponseApiCaller GetContractorWithContractorId(GetContractorWithContractorIdPVM getContractorWithContractorIdPVM);
        #endregion

        #region DocumentOwnershipTypesManagement

        ResponseApiCaller GetAllDocumentOwnershipTypesList(GetAllDocumentOwnershipTypesListPVM getAllDocumentOwnershipTypesListPVM);

        #endregion

        #region DocumentRootTypesManagement

        ResponseApiCaller GetAllDocumentRootTypesList(GetAllDocumentRootTypesListPVM getAllDocumentRootTypesListPVM);

        #endregion

        #region DocumentTypesManagement

        ResponseApiCaller GetAllDocumentTypesList(GetAllDocumentTypesListPVM getAllDocumentTypesListPVM);

        #endregion

        #region EvaluationSubjectManagement
        ResponseApiCaller GetAllEvaluationSubjectsList(GetAllEvaluationSubjectsListPVM getAllEvaluationSubjectsList);

        #endregion

        #region EvaluationsManagement

        ResponseApiCaller GetAllEvaluationsList(GetAllEvaluationsListPVM getAllEvaluationsListPVM);

        ResponseApiCaller GetListOfEvaluations(GetListOfEvaluationsPVM GetListOfEvaluationsPVM);

        ResponseApiCaller AddToEvaluations(AddToEvaluationsPVM addToEvaluationsPVM);

        ResponseApiCaller UpdateEvaluations(UpdateEvaluationsPVM updateEvaluationsPVM);

        ResponseApiCaller ToggleActivationEvaluations(ToggleActivationEvaluationsPVM toggleActivationEvaluationsPVM);

        ResponseApiCaller TemporaryDeleteEvaluations(TemporaryDeleteEvaluationsPVM temporaryDeleteEvaluationsPVM);

        ResponseApiCaller CompleteDeleteEvaluations(CompleteDeleteEvaluationsPVM completeDeleteEvaluationsPVM);

        ResponseApiCaller GetEvaluationsWithEvaluationId(GetEvaluationsWithEvaluationIdPVM getEvaluationsWithEvaluationsIdPVM);

        #endregion

        #region EvaluationCategoriesManagement

        ResponseApiCaller GetAllEvaluationCategoriesList(GetAllEvaluationCategoriesListPVM getAllEvaluationCategoriesListPVM);

        ResponseApiCaller GetListOfEvaluationCategories(GetListOfEvaluationCategoriesPVM getListOfEvaluationCategoriesPVM);

        ResponseApiCaller AddToEvaluationCategories(AddToEvaluationCategoriesPVM addToEvaluationCategoriesPVM);

        ResponseApiCaller GetEvaluationCategoryWithEvaluationCategoryId(GetEvaluationCategoryWithEvaluationCategoryIdPVM getEvaluationCategoryWithEvaluationCategoryIdPVM);

        ResponseApiCaller UpdateEvaluationCategories(UpdateEvaluationCategoriesPVM updateEvaluationCategoriesPVM);

        ResponseApiCaller ToggleActivationEvaluationCategories(ToggleActivationEvaluationCategoriesPVM toggleActivationEvaluationCategoriesPVM);

        ResponseApiCaller TemporaryDeleteEvaluationCategories(TemporaryDeleteEvaluationCategoriesPVM temporaryDeleteEvaluationCategoriesPVM);

        ResponseApiCaller CompleteDeleteEvaluationCategories(CompleteDeleteEvaluationCategoriesPVM completeDeleteEvaluationCategoriesPVM);

        ResponseApiCaller GetAllDivisionOfEvaluationsListByParentId(GetAllDivisionOfEvaluationsListByParentIdPVM getAllDivisionOfEvaluationsListByParentIdPVM);
        #endregion

        #region EvaluationQuestionsManagement
        ResponseApiCaller GetAllEvaluationQuestionsList(GetAllEvaluationQuestionsListPVM getAllEvaluationQuestionsListPVM);

        ResponseApiCaller GetListOfEvaluationQuestions(GetListOfEvaluationQuestionsPVM getListOfEvaluationQuestionsPVM);

        ResponseApiCaller AddToEvaluationQuestions(AddToEvaluationQuestionsPVM addToEvaluationQuestionsPVM);
        ResponseApiCaller GetEvaluationQuestionWithEvaluationQuestionId(GetEvaluationQuestionWithEvaluationQuestionIdPVM getEvaluationQuestionWithEvaluationQuestionIdPVM);

        ResponseApiCaller UpdateEvaluationQuestions(UpdateEvaluationQuestionsPVM updateEvaluationQuestionsPVM);

        ResponseApiCaller ToggleActivationEvaluationQuestions(ToggleActivationEvaluationQuestionsPVM toggleActivationEvaluationQuestionsPVM);

        ResponseApiCaller TemporaryDeleteEvaluationQuestions(TemporaryDeleteEvaluationQuestionsPVM temporaryDeleteEvaluationQuestionsPVM);

        ResponseApiCaller CompleteDeleteEvaluationQuestions(CompleteDeleteEvaluationQuestionsPVM completeDeleteEvaluationQuestionsPVM);

        #endregion

        #region EvaluationItemsManagement

        ResponseApiCaller GetAllEvaluationItemsList(GetAllEvaluationItemsListPVM getAllEvaluationItemsListPVM);

        ResponseApiCaller GetListOfEvaluationItems(GetListOfEvaluationItemsPVM getListOfEvaluationItemsPVM);

        ResponseApiCaller AddToEvaluationItems(AddToEvaluationItemsPVM addToEvaluationItemsPVM);

        ResponseApiCaller GetEvaluationItemWithEvaluationItemId(GetEvaluationItemWithEvaluationItemIdPVM getEvaluationItemWithEvaluationItemIdPVM);

        ResponseApiCaller UpdateEvaluationItems(UpdateEvaluationItemsPVM updateEvaluationItemsPVM);

        ResponseApiCaller ToggleActivationEvaluationItems(ToggleActivationEvaluationItemsPVM toggleActivationEvaluationItemsPVM);

        ResponseApiCaller TemporaryDeleteEvaluationItems(TemporaryDeleteEvaluationItemsPVM temporaryDeleteEvaluationItemsPVM);

        ResponseApiCaller CompleteDeleteEvaluationItems(CompleteDeleteEvaluationItemsPVM completeDeleteEvaluationItemsPVM);

        #endregion

        #region EvaluationItemValuesManagement

        ResponseApiCaller GetEvaluationItemValuesByParentId(GetEvaluationItemValuesByParentIdPVM getEvaluationItemValuesByParentIdPVM);

        ResponseApiCaller AddToEvaluationItemValues(QuestionSheetPVM questionSheetPVM);

        ResponseApiCaller UpdateEvaluationItemValues(UpdateEvaluationItemValuesPVM updateEvaluationItemValuesPVM);

        ResponseApiCaller AddToEvaluationItemValues(AddToEvaluationItemValuesPVM addToEvaluationItemValuesPVM);

        ResponseApiCaller UpdateEvaluationItemValuesList(UpdateEvaluationItemValuesListPVM updateEvaluationItemValuesListPVM);
        #endregion

        #region FeaturesManagement

        ResponseApiCaller GetListOfFeatures(GetListOfFeaturesPVM getListOfFeaturesPVM);

        ResponseApiCaller AddToFeatures(AddToFeaturesPVM addToFeaturesPVM);

        ResponseApiCaller GetFeatureWithFeatureId(GetFeatureWithFeatureIdPVM getFeatureWithFeatureIdPVM);

        ResponseApiCaller UpdateFeatures(UpdateFeaturesPVM updateFeaturesPVM);

        ResponseApiCaller ToggleActivationFeatures(ToggleActivationFeaturesPVM toggleActivationFeaturesPVM);

        ResponseApiCaller TemporaryDeleteFeatures(TemporaryDeleteFeaturesPVM temporaryDeleteFeaturesPVM);

        ResponseApiCaller CompleteDeleteFeatures(CompleteDeleteFeaturesPVM completeDeleteFeaturesPVM);

        ResponseApiCaller GetAllFeaturesList(GetAllFeaturesListPVM getAllFeaturesListPVM);

        #endregion

        #region FeaturesOptionsManagement

        ResponseApiCaller GetAllFeaturesOptionsList(GetAllFeaturesOptionsListPVM getAllFeaturesOptionsListPVM);

        ResponseApiCaller GetListOfFeaturesOptions(GetListOfFeaturesOptionsPVM getListOfFeaturesOptionsPVM);

        ResponseApiCaller AddToFeaturesOptions(AddToFeaturesOptionsPVM addToFeaturesOptionsPVM);

        ResponseApiCaller UpdateFeaturesOptions(UpdateFeaturesOptionsPVM updateFeaturesOptionsPVM);

        ResponseApiCaller ToggleActivationFeaturesOptions(ToggleActivationFeaturesOptionsPVM toggleActivationFeaturesOptionsPVM);

        ResponseApiCaller TemporaryDeleteFeaturesOptions(TemporaryDeleteFeaturesOptionsPVM temporaryDeleteFeaturesOptionsPVM);

        ResponseApiCaller CompleteDeleteFeaturesOptions(CompleteDeleteFeaturesOptionsPVM completeDeleteFeaturesOptionsPVM);

        #endregion

        #region FeaturesManagement
        #endregion

        #region FeaturesOptionsManagement
        #endregion

        #region FeaturesValuesManagement
        #endregion

        #region FundsManagement

        ResponseApiCaller GetAllFundsList(GetAllFundsListPVM getAllFundsListPVM);
        ResponseApiCaller GetListOfFunds(GetListOfFundsPVM getListOfFundsPVM);
        ResponseApiCaller AddToFunds(AddToFundsPVM addToFundsPVM);
        ResponseApiCaller UpdateFunds(UpdateFundPVM updateFundPVM);
        ResponseApiCaller ToggleActivationFunds(ToggleActivationFundPVM toggleActivationFundPVM);
        ResponseApiCaller TemporaryDeleteFunds(TemporaryDeleteFundPVM temporaryDeleteFundPVM);
        ResponseApiCaller CompleteDeleteFunds(CompleteDeleteFundsPVM completeDeleteFundsPVM);
        ResponseApiCaller GetFundWithFundId(GetFundWithFundIdPVM getFundWithFundIdPVM);

        #endregion

        #region GeoJsonFileManagement

        ResponseApiCaller AddToMapLayerFiles(AddToMapLayerFilesPVM addToMapLayerFilesPVM);

        #endregion

        #region IntroductionMethodsManagement
        #endregion

        #region InvestorsManagement
        ResponseApiCaller GetAllInvestorsList(GetAllInvestorsListPVM getAllInvestorsListPVM);
        ResponseApiCaller GetListOfInvestors(GetListOfInvestorsPVM getListOfInvestorsPVM);
        ResponseApiCaller AddToInvestors(AddToInvestorsPVM addToInvestorsPVM);
        ResponseApiCaller UpdateInvestors(UpdateInvestorsPVM updateInvestorsPVM);
        ResponseApiCaller ToggleActivationInvestors(ToggleActivationInvestorsPVM toggleActivationInvestorsPVM);
        ResponseApiCaller TemporaryDeleteInvestors(TemporaryDeleteInvestorsPVM temporaryDeleteInvestorsPVM);
        ResponseApiCaller CompleteDeleteInvestors(CompleteDeleteInvestorsPVM completeDeleteInvestorsPVM);
        ResponseApiCaller GetInvestorWithInvestorId(GetInvestorWithInvestorIdPVM getInvestorWithInvestorIdPVM);
        #endregion

        #region InvestorsFavoritesManagement
        #endregion

        #region InvestorsRequestsManagement
        #endregion

        #region MyProperties

        #region MyPropertiesManagement

        ResponseApiCaller GetAllPropertiesInfo(GetAllPropertiesInfoPVM getAllPropertiesInfoPVM);
        ResponseApiCaller GetAllMyPropertiesList(GetAllMyPropertiesListPVM getAllMyPropertiesListPVM);

        ResponseApiCaller GetListOfMyProperties(GetListOfMyPropertiesPVM getListOfMyPropertiesPVM);

        ResponseApiCaller AddToMyProperties(AddToMyPropertiesPVM addToMyPropertiesPVM);

        ResponseApiCaller GetMyPropertyWithMyPropertyId(GetMyPropertyWithMyPropertyIdPVM getMyPropertyWithMyPropertyIdPVM);

        ResponseApiCaller UpdateMyProperties(UpdateMyPropertiesPVM updateMyPropertiesPVM);

        ResponseApiCaller ToggleActivationMyProperties(ToggleActivationMyPropertiesPVM toggleActivationMyPropertiesPVM);

        ResponseApiCaller TemporaryDeleteMyProperties(TemporaryDeleteMyPropertiesPVM temporaryDeleteMyPropertiesPVM);

        ResponseApiCaller CompleteDeleteMyProperties(CompleteDeleteMyPropertiesPVM completeDeleteMyPropertiesPVM);

        ResponseApiCaller GetAllMyFeaturesValuesCompare(GetMyFeaturesValuesComparePVM getMyFeaturesValuesComparePVM);

        ResponseApiCaller GetMyPropertiesCompareBasicInfo(GetMyPropertiesCompareBasicInfoPVM getMyPropertiesCompareBasicInfoPVM);

        ResponseApiCaller GetAllMyPropertiesCompareTopic(GetAllMyPropertiesCompareTopicPVM getAllMyPropertiesCompareTopicPVM);
        #endregion

        #region MyPropertiesLocationManagement

        public ResponseApiCaller UpdateMyPropertyLocation(UpdateMyPropertyLocationPVM updateMyPropertylocationPVM);

        #endregion

        #region MyPropertyFilesManagement

        public ResponseApiCaller UpdateListPropertyFilesTitle(List<UpdateListPropertyFilesTitlePVM> lst);
        public ResponseApiCaller GetAllMyPropertyFilesList(GetAllMyPropertyFilesListPVM getAllMyPropertyFilesListPVM);

        public ResponseApiCaller GetListOfMyPropertyFiles(GetListOfMyPropertyFilesPVM getListOfMyPropertyFilesPVM);

        public ResponseApiCaller AddToMyPropertyFiles(AddToMyPropertyFilesPVM addToMyPropertyFilesPVM);
        public ResponseApiCaller AddToMyPropertyFilesReg(AddToMyPropertyFilesPVM addToMyPropertyFilesPVM);

        public ResponseApiCaller GetMyPropertyFileWithMyPropertyFileId(GetMyPropertyFileWithMyPropertyFileIdPVM getMyPropertyFileWithMyPropertyFileIdPVM);

        public ResponseApiCaller UpdateMyPropertyFiles(UpdateMyPropertyFilesPVM updateMyPropertyFilesPVM);

        public ResponseApiCaller ToggleActivationMyPropertyFiles(ToggleActivationMyPropertyFilesPVM toggleActivationMyPropertyFilesPVM);

        public ResponseApiCaller TemporaryDeleteMyPropertyFiles(TemporaryDeleteMyPropertyFilesPVM temporaryDeleteMyPropertyFilesPVM);

        public ResponseApiCaller CompleteDeleteMyPropertyFiles(CompleteDeleteMyPropertyFilesPVM completeDeleteMyPropertyFilesPVM);

        #endregion

        #region MyPropertiesFeaturesManagement

        ResponseApiCaller GetMyPropertyFeaturesValues(GetMyPropertyFeaturesValuesPVM getMyPropertyFeaturesValuesPVM);
        ResponseApiCaller UpdateMyPropertyFeatures(UpdateMyPropertyFeaturesPVM updateMyPropertyFeaturesPVM);
        #endregion

        #region MyPropertyTypesManagement
        ResponseApiCaller GetAllMyPropertyTypesList(GetAllPropertyTypesListPVM getAllPropertyTypesListPVM);

        #endregion

        #region MyPropertiesPricesHistoriesManagement

        ResponseApiCaller GetListOfMyPropertiesPricesHistories(GetListOfMyPropertiesPricesHistoriesPVM getListOfMyPropertiesPricesHistoriesPVM);
        ResponseApiCaller GetLastPropertiesPriceHistoryByPropertyId(GetListOfPropertiesPricesHistoriesPVM getListOfPropertiesPricesHistoriesPVM);

        #endregion

        #endregion

        #region MyPropertiesForInvestors

        #region MyPropertiesForInvestorsManagement
        ResponseApiCaller GetAllMyPropertiesForInvestorsList(GetAllMyPropertiesForInvestorsListPVM getAllMyPropertiesForInvestorsListPVM);

        ResponseApiCaller GetListOfMyPropertiesForInvestors(GetListOfMyPropertiesForInvestorsPVM getListOfMyPropertiesForInvestorsPVM);

        ResponseApiCaller AddToMyPropertiesForInvestors(AddToMyPropertiesForInvestorsPVM addToMyPropertiesForInvestorsPVM);

        ResponseApiCaller GetMyPropertyWithMyPropertyIdForInvestors(GetMyPropertyWithMyPropertyIdForInvestorsPVM getMyPropertyWithMyPropertyIdForInvestorsPVM);

        ResponseApiCaller UpdateMyPropertiesForInvestors(UpdateMyPropertiesForInvestorsPVM updateMyPropertiesForInvestorsPVM);

        ResponseApiCaller CompleteDeleteMyPropertiesForInvestors(CompleteDeleteMyPropertiesForInvestorsPVM completeDeleteMyPropertiesForInvestorsPVM);

        #endregion

        #region MyPropertiesLocationForInvestorsManagement

        ResponseApiCaller UpdateMyPropertyLocationForInvestors(UpdateMyPropertyLocationForInvestorsPVM updateMyPropertylocationForInvestorsPVM);

        #endregion

        #region MyPropertyFilesManagement

        ResponseApiCaller GetAllMyPropertyFilesForInvestorsList(GetAllMyPropertyFilesForInvestorsListPVM getAllMyPropertyFilesForInvestorsListPVM);

        ResponseApiCaller GetListOfMyPropertyFilesForInvestors(GetListOfMyPropertyFilesForInvestorsPVM getListOfMyPropertyFilesForInvestorsPVM);
        ResponseApiCaller AddToMyPropertyFilesForInvestors(AddToMyPropertyFilesForInvestorsPVM addToMyPropertyFilesForInvestorsPVM);

        ResponseApiCaller UpdateMyPropertyFilesForInvestors(UpdateMyPropertyFilesForInvestorsPVM updateMyPropertyFilesForInvestorsPVM);

        ResponseApiCaller CompleteDeleteMyPropertyFilesForInvestors(CompleteDeleteMyPropertyFilesForInvestorsPVM completeDeleteMyPropertyFilesForInvestorsPVM);

        #endregion

        #region MyPropertiesFeaturesForInvestorsManagement

        ResponseApiCaller GetMyPropertyFeaturesValuesForInvestors(GetMyPropertyFeaturesValuesForInvestorsPVM getMyPropertyFeaturesValuesForInvestorsPVM);

        ResponseApiCaller UpdateMyPropertyFeaturesForInvestors(UpdateMyPropertyFeaturesForInvestorsPVM updateMyPropertyFeaturesForInvestorsPVM);

        #endregion

        #region MyPropertiesPricesHistoriesForInvestorsManagement

        ResponseApiCaller GetListOfMyPropertiesPricesHistoriesForInvestors(GetListOfMyPropertiesPricesHistoriesForInvestorsPVM getListOfMyPropertiesPricesHistoriesForInvestorsPVM);
        #endregion

        #endregion

        #region MelkavanProperties
        ResponseApiCaller GetListOfMelkavanProperties(GetListOfMelkavanPropertiesPVM getListOfMelkavanPropertiesPVM);

        ResponseApiCaller GetListOfNearAdvertisementsWithPropertyId(GetListOfNearAdvertisementsWithPropertyIdPVM getListOfNearAdvertisementsWithPropertyIdPVM);
        #endregion

        #region MapLayerCategoriesManagement

        ResponseApiCaller GetAllMapLayerCategoriesList(GetAllMapLayerCategoriesListPVM getAllMapLayerCategoriesListPVM);
        ResponseApiCaller GetListOfMapLayerCategories(GetListOfMapLayerCategoriesPVM getListOfMapLayerCategoriesPVM);
        ResponseApiCaller AddToMapLayerCategories(AddToMapLayerCategoriesPVM addToMapLayerCategoriesPVM);
        ResponseApiCaller UpdateMapLayerCategories(UpdateMapLayerCategoriesPVM updateMapLayerCategoriesPVM);
        ResponseApiCaller ToggleActivationMapLayerCategories(ToggleActivationMapLayerCategoriesPVM toggleActivationMapLayerCategoriesPVM);
        ResponseApiCaller TemporaryDeleteMapLayerCategories(TemporaryDeleteMapLayerCategoriesPVM temporaryDeleteMapLayerCategoriesPVM);
        ResponseApiCaller CompleteDeleteMapLayerCategories(CompleteDeleteMapLayerCategoriesPVM completeDeleteMapLayerCategoriesPVM);
        ResponseApiCaller GetMapLayerCategoryWithMapLayerCategoryId(GetMapLayerCategoryWithMapLayerCategoryIdPVM getMapLayerCategoryWithMapLayerCategoryIdPVM);
        #endregion

        #region MapLayersManagement

        ResponseApiCaller GetAllMapLayersList(GetAllMapLayersListPVM getAllMapLayersListPVM);
        ResponseApiCaller GetListOfMapLayers(GetListOfMapLayersPVM getListOfMapLayersPVM);
        ResponseApiCaller AddToMapLayersWithJsonData(AddToMapLayersJsonDataPVM addToMapLayersWithJsonData);
        ResponseApiCaller AddToMapLayers(AddToMapLayersPVM addToMapLayersPVM);
        ResponseApiCaller UpdateMapLayers(UpdateMapLayersPVM updateMapLayersPVM);
        ResponseApiCaller ToggleActivationMapLayers(ToggleActivationMapLayersPVM toggleActivationMapLayersPVM);
        ResponseApiCaller TemporaryDeleteMapLayers(TemporaryDeleteMapLayersPVM temporaryDeleteMapLayersPVM);
        ResponseApiCaller CompleteDeleteMapLayers(CompleteDeleteMapLayersPVM completeDeleteMapLayersPVM);
        ResponseApiCaller CompleteDeleteMapLayersIds(CompleteDeleteMapLayersIdsPVM completeDeleteMapLayersIdsPVM);
        ResponseApiCaller GetMapLayerWithMapLayerId(GetMapLayerWithMapLayerIdPVM getMapLayerWithMapLayerIdPVM);

        #endregion

        #region ProjectsManagement
        #endregion

        #region ProjectStatesManagement
        #endregion

        #region ProjectTypesManagement
        #endregion

        #region PropertiesManagement

        ResponseApiCaller GetAllPropertiesList(GetAllPropertiesListPVM getAllPropertiesListPVM);

        ResponseApiCaller GetListOfProperties(GetListOfPropertiesPVM getListOfPropertiesPVM);

        ResponseApiCaller GetListOfPropertiesAdvanceSearch(GetListOfPropertiesAdvanceSearchPVM getListOfPropertiesAdvanceSearchPVM);

        ResponseApiCaller AddToProperties(AddToPropertiesPVM addToPropertiesPVM);

        ResponseApiCaller GetPropertyWithPropertyId(GetPropertyWithPropertyIdPVM getPropertyWithPropertyIdPVM);

        ResponseApiCaller GetPropertyDetailsWithPropertyId(GetPropertyWithPropertyIdPVM getPropertyWithPropertyIdPVM);

        ResponseApiCaller UpdateProperties(UpdatePropertiesPVM updatePropertiesPVM);

        ResponseApiCaller ToggleActivationProperties(ToggleActivationPropertiesPVM toggleActivationPropertiesPVM);

        ResponseApiCaller TemporaryDeleteProperties(TemporaryDeletePropertiesPVM temporaryDeletePropertiesPVM);
        ResponseApiCaller TemporaryDeletePropertiesWithChild(TemporaryDeletePropertiesWithChildPVM temporaryDeletePropertiesWithChildPVM);

        ResponseApiCaller CompleteDeleteProperties(CompleteDeletePropertiesPVM completeDeletePropertiesPVM);
        ResponseApiCaller GetAllPropertiesCompareTopic(GetAllPropertiesCompareTopicPVM getAllPropertiesCompareTopicPVM);

        ResponseApiCaller GetAllFeaturesValuesCompare(GetFeaturesValuesComparePVM getFeaturesValuesComparePVM);

        ResponseApiCaller GetPropertiesCompareBasicInfo(GetPropertiesCompareBasicInfoPVM getPropertiesCompareBasicInfoPVM);

        ResponseApiCaller GetAllPropertiesListWithoutAddress(GetAllPropertiesListWithoutAddressPVM getAllPropertiesListWithoutAddressPVM);
        ResponseApiCaller ToggleActivationShowInMelkavan(ToggleActivationShowInMelkavanPVM toggleActivationShowInMelkavanPVM);
        ResponseApiCaller AddPropertiesInMelkavan(AddPropertiesInMelkavanPVM addPropertiesInMelkavanPVM);

        #endregion

        #region PropertiesLocationManagement

        ResponseApiCaller UpdatePropertyLocation(UpdatePropertyLocationPVM updatePropertylocationPVM);

        #endregion

        #region PropertyAddressManagement
        #endregion

        #region PropertyFilesManagement

        ResponseApiCaller GetAllPropertyFilesList(GetAllPropertyFilesListPVM getAllPropertyFilesListPVM);

        ResponseApiCaller GetListOfPropertyFiles(GetListOfPropertyFilesPVM getListOfPropertyFilesPVM);

        ResponseApiCaller AddToPropertyFiles(AddToPropertyFilesPVM addToPropertyFilesPVM);

        ResponseApiCaller GetPropertyFileWithPropertyFileId(GetPropertyFileWithPropertyFileIdPVM getPropertyFileWithPropertyFileIdPVM);

        ResponseApiCaller UpdatePropertyFiles(UpdatePropertyFilesPVM updatePropertyFilesPVM);

        ResponseApiCaller ToggleActivationPropertyFiles(ToggleActivationPropertyFilesPVM toggleActivationPropertyFilesPVM);

        ResponseApiCaller TemporaryDeletePropertyFiles(TemporaryDeletePropertyFilesPVM temporaryDeletePropertyFilesPVM);

        ResponseApiCaller CompleteDeletePropertyFiles(CompleteDeletePropertyFilesPVM completeDeletePropertyFilesPVM);

        #endregion

        #region PropertiesFeaturesManagement

        ResponseApiCaller GetPropertyFeaturesValues(GetPropertyFeaturesValuesPVM getPropertyFeaturesValuesPVM);

        ResponseApiCaller UpdatePropertyFeatures(UpdatePropertyFeaturesPVM updatePropertyFeaturesPVM);

        #endregion

        #region PropertyStatesManagement
        #endregion

        #region PropertyTypesManagement

        ResponseApiCaller GetAllPropertyTypesList(GetAllPropertyTypesListPVM getAllPropertyTypesListPVM);

        #endregion

        #region PropertiesPricesHistoriesManagement

        ResponseApiCaller GetListOfPropertiesPricesHistories(GetListOfPropertiesPricesHistoriesPVM getListOfPropertiesPricesHistoriesPVM);

        ResponseApiCaller GetListOfPropertiesPricesForMap(GetListOfPropertiesPricesForMapPVM getListOfPropertiesPricesForMapPVM);

        #endregion

        #region ProposedProjectsManagement
        #endregion

        #region PositionsManagement

        ResponseApiCaller GetAllPositionsList(GetAllPositionsListPVM getAllPositionsListPVM);

        #endregion

        #region TypeOfUsesManagement

        ResponseApiCaller GetAllTypeOfUsesList(GetAllTypeOfUsesListPVM getAllTypeOfUsesListPVM);

        #endregion

        #region OutterDashboard

        ResponseApiCaller GetOutterDashboardPricesBlock(GetOutterDashboardPricesBlockPVM getOutterDashboardPricesBlockPVM);

        ResponseApiCaller GetListOfMyFundsDispersion(GetListOfMyFundsDispersionPVM getListOfMyFundsDispersionPVM);

        ResponseApiCaller GetListOfMyFundsGrowth(GetListOfMyFundsGrowthPVM getListOfMyFundsGrowthPVM);

        ResponseApiCaller GetDetailsForOuterDashboard(GetDetailsForOuterDashboardPVM getDetailsForOuterDashboardPVM);

        ResponseApiCaller GetUnreadConversationsAndUnverifiedFilesCount(GetUnreadConversationsAndUnverifiedFilesCountPVM getUnreadConversationsAndUnverifiedFilesCountPVM);

        #endregion

    }
}
