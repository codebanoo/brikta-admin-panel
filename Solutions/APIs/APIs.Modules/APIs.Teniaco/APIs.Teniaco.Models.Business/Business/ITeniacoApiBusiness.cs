using APIs.Melkavan.Models;
using APIs.Melkavan.Models.Business;
using APIs.Projects.Models.Business;
using APIs.Public.Models;
using APIs.Public.Models.Business;
using APIs.Teniaco.Models.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using Models.Business.ConsoleBusiness;
using System.Collections.Generic;
using VM.Console;
using VM.Melkavan;
using VM.PVM.Teniaco;
using VM.Teniaco;

namespace APIs.Teniaco.Models.Business
{
    public interface ITeniacoApiBusiness
    {
        TeniacoApiContext TeniacoApiDb { get; set; }

        #region Teniaco

        #region Methods for Work With Agencies

        List<AgenciesVM> GetAllAgenciesList(
           PublicApiContext publicApiDb,
           ref int listCount,
           List<long> childsUsersIds,
           //int? AgencyId = null,
           string AgencyName = "",
           long? stateId = null,
           long? cityId = null,
           long? zoneId = null);

        List<AgenciesVM> GetListOfAgencies(
           PublicApiContext publicApiDb,
           int jtStartIndex,
           int jtPageSize,
           ref int listCount,
           List<long> childsUsersIds,
           // int? AgencyId = null,
           string AgencyName = "",
           long? stateId = null,
           long? cityId = null,
           long? zoneId = null,
           string jtSorting = null);

        int AddToAgencies(AgenciesVM agenciesVM,
           IConsoleBusiness consoleBusiness);

        int UpdateAgencies(ref AgenciesVM agenciesVM,
           List<long> childsUsersIds);

        bool ToggleActivationAgencies(int agencyId,
           long userId,
           IConsoleBusiness consoleBusiness,
           List<long> childsUsersIds);

        bool TemporaryDeleteAgencies(int agencyId,
           long userId,
           IConsoleBusiness consoleBusiness,
           List<long> childsUsersIds);

        bool CompleteDeleteAgencies(
            int agencyId,
            long ThisUserId,
           List<long> childsUsersIds,
            IConsoleBusiness consoleBusiness);

        AgenciesVM GetAgencyWithAgencyId(
           int? agencyId,
           List<long> childsUsersIds);

        #endregion

        #region Methods for Work With AgencyStaffs

        List<AgencyStaffsVM> GetAllAgencyStaffsList(
           ref int listCount,
           List<long> childsUsersIds,
           int? agencyId);
        List<AgencyStaffsVM> GetListOfAgencyStaffs(
           int jtStartIndex,
           int jtPageSize,
           ref int listCount,
           List<long> childsUsersIds,
           int? agencyId,
           string jtSorting = null);

        int AddToAgencyStaffs(AgencyStaffsVM agencyStaffsVM,
           IConsoleBusiness consoleBusiness,
           string domainName);

        int UpdateAgencyStaffs(ref AgencyStaffsVM agencyStaffsVM,
           List<long> childsUsersIds,
           IConsoleBusiness consoleBusiness);

        AgencyStaffsVM GetAgencyStaffWithAgencyStaffId(
           int? agencyStaffId,
           List<long> childsUsersIds);

        bool ToggleActivationAgencyStaffs(int agencyStaffId,
           long userId,
           List<long> childsUsersIds,
           IConsoleBusiness consoleBusiness);

        bool TemporaryDeleteAgencyStaffs(int agencyStaffId,
           long userId,
           List<long> childsUsersIds,
           IConsoleBusiness consoleBusiness);

        bool CompleteDeleteAgencyStaffs(int agencyStaffId,
           List<long> childsUsersIds,
           IConsoleBusiness consoleBusiness);

        #endregion

        #region Methods for Work With AgencyLocation
        bool UpdateAgencylocation(
           int agencyId,
           double locationLat,
           double locationLon);

        #endregion

        #region Methods For Work With CompareProperties

        List<ComparePropertiesByPersonIdVM> GetAllComparePropertiesListByPersonId(
             List<long> childsUsersIds,
             PublicApiContext publicApiDb);



        ComparePropertiesForBasicInfoVM GetListOfComparePropertiesForBasicInfo(
            List<long> childsUsersIds,
             long propertyId = 0);

        List<CompareFeatureValuesVM> GetListOfCompareFeatureValues(
            List<long> childsUsersIds,
             long propertyId = 0);



        List<ComparePropertiesAddressVM> GetListOfComparePropertiesAddress(
           PublicApiContext publicApiDb,
           List<long> childsUsersIds,
             long propertyId = 0);


        List<ComparePropertiesPricesHistoriesVM> GetListOfComparePropertiesPricesHistories(
            List<long> childsUsersIds,
             long propertyId = 0);


        #endregion

        #region Methods for Work With Contractors

        List<ContractorsVM> GetAllContractorsList(
         PublicApiContext publicApiDb,
         ref int listCount,
         List<long> childsUsersIds,
         string contractorName = "",
         long? stateId = null,
         long? cityId = null,
         long? zoneId = null);

        List<ContractorsVM> GetListOfContractors(
           PublicApiContext publicApiDb,
           int jtStartIndex,
           int jtPageSize,
           ref int listCount,
           List<long> childsUsersIds,
           string contractorName = "",
           long? stateId = null,
           long? cityId = null,
           long? zoneId = null,
           string jtSorting = null);

        int AddToContractors(ContractorsVM contractorsVM,
           IPublicApiBusiness publicApiBusiness,
           IConsoleBusiness consoleBusiness,
           string domainName);

        int UpdateContractors(ref ContractorsVM contractorsVM,
           List<long> childsUsersIds);

        bool ToggleActivationContractors(int contractorId,
           long userId,
           IConsoleBusiness consoleBusiness,
           List<long> childsUsersIds);

        bool TemporaryDeleteContractors(int contractorId,
           long userId,
           IConsoleBusiness consoleBusiness,
           List<long> childsUsersIds);

        bool CompleteDeleteContractors(int contractorId,
           List<long> childsUsersIds,
            IConsoleBusiness consoleBusiness);

        ContractorsVM GetContractorWithContractorId(
           int? contractorId,
           List<long> childsUsersIds);

        #endregion

        #region Methods For Work With DocumentOwnershipTypes

        List<DocumentOwnershipTypesVM> GetAllDocumentOwnershipTypesList();

        #endregion

        #region Methods For Work With DocumentRootTypes

        List<DocumentRootTypesVM> GetAllDocumentRootTypesList();

        #endregion

        #region Methods For Work With DocumentTypes

        List<DocumentTypesVM> GetAllDocumentTypesList();

        #endregion

        #region Methods For Work With EvaluationsSubject


        List<EvaluationSubjectsVM> GetAllEvaluationSubjectsList(
           ref int listCount,
           List<long> childsUsersIds);

        #endregion

        #region Methods For Work With Evaluations

        List<EvaluationsVM> GetAllEvaluationsList(
         int? EvaluationSubjectId,
         string? EvaluationTitle);

        List<EvaluationsVM> GetListOfEvaluations(
           int jtStartIndex,
           int jtPageSize,
           ref int listCount,
           List<long> childsUsersIds,
           string EvaluationTitle = "",
           string jtSorting = null);


        int AddToEvaluations(EvaluationsVM evaluationsVM);

        public int UpdateEvaluations(ref EvaluationsVM evaluationsVM,
           List<long> childsUsersIds);

        bool ToggleActivationEvaluations(int evaluationId,
         long userId,
         List<long> childsUsersIds);



        bool TemporaryDeleteEvaluations(int evaluationId,
        long userId,
        List<long> childsUsersIds);


        bool CompleteDeleteEvaluations(int evaluationId,
           List<long> childsUsersIds);


        EvaluationsVM GetEvaluationsWithEvaluationId(
       int? evaluationId,
       List<long> childsUsersIds);

        #endregion

        #region Methods For Work With EvaluationCategories

        List<EvaluationCategoriesVM> GetAllEvaluationCategoriesList(
       ref int listCount,
       List<long> childsUsersIds,
       int? evaluationId,
        string? evaluationCategoryTitleSearch = "");

        List<EvaluationCategoriesVM> GetListOfEvaluationCategories(
           int jtStartIndex,
           int jtPageSize,
           ref int listCount,
           List<long> childsUsersIds,
           int? evaluationId,
           string? evaluationCategoryTitle,
           string jtSorting = null);


        int AddToEvaluationCategories(EvaluationCategoriesVM evaluationCategoriesVM);


        int UpdateEvaluationCategories(ref EvaluationCategoriesVM evaluationCategoriesVM,
          List<long> childsUsersIds);

        bool ToggleActivationEvaluationCategories(
           int evaluationCategoryId,
           long userId,
           List<long> childsUsersIds);


        bool TemporaryDeleteEvaluationCategories(int evaluationCategoryId,
           long userId,
           List<long> childsUsersIds);

        bool CompleteDeleteEvaluationCategories(int evaluationCategoryId,
           //long userId,
           List<long> childsUsersIds);


        EvaluationCategoriesVM GetEvaluationCategoryWithEvaluationCategoryId(int evaluationCategoryId);

        List<EvaluationCategoriesVM> GetAllDivisionOfEvaluationsListByParentId(
       List<long> childsUsersIds,
       int? evaluationId);


        #endregion

        #region Methods For Work With EvaluationQuestions
        List<EvaluationQuestionsVM> GetAllEvaluationQuestionsList(
           ref int listCount,
           List<long> childsUsersIds,
           int? evaluationCategoryId = null,
           string? evaluationQuestion = "");


        List<EvaluationQuestionsVM> GetListOfEvaluationQuestions(
           int jtStartIndex,
           int jtPageSize,
           ref int listCount,
           List<long> childsUsersIds,
           int? evaluationCategoryId = null,
           string? evaluationQuestion = "",
           string jtSorting = null);


        int AddToEvaluationQuestions(EvaluationQuestionsVM evaluationQuestionsVM);



        int UpdateEvaluationQuestions(ref EvaluationQuestionsVM evaluationQuestionsVM,
           List<long> childsUsersIds);

        bool ToggleActivationEvaluationQuestions(
           int evaluationQuestionId,
           long userId,
           List<long> childsUsersIds);

        bool TemporaryDeleteEvaluationQuestions(
           int evaluationQuestionId,
           long userId,
           List<long> childsUsersIds);


        bool CompleteDeleteEvaluationQuestions(int evaluationQuestionId,
           //long userId,
           List<long> childsUsersIds);

        List<EvaluationQuestionsVM> GetEvaluationQuestionsWithEvalCategoriesIds(List<int> evalCategoriesIds);

        EvaluationQuestionsVM GetEvaluationQuestionWithEvaluationQuestionId(int evaluationQuestionId);


        #endregion

        #region Methods For Work With EvaluationItems

        List<EvaluationItemsVM> GetAllEvaluationItemsList(
            ref int listCount,
            List<long> childsUsersIds,
            int? evaluationQuestionId = null,
            string? evaluationAnswer = "");


        List<EvaluationItemsVM> GetListOfEvaluationItems(
           int jtStartIndex,
           int jtPageSize,
           ref int listCount,
           List<long> childsUsersIds,
           int? evaluationQuestionId = null,
           string jtSorting = null);


        int AddToEvaluationItems(EvaluationItemsVM evaluationItemsVM);


        int UpdateEvaluationItems(ref EvaluationItemsVM evaluationItemsVM,
           //long userId,
           List<long> childsUsersIds);

        bool ToggleActivationEvaluationItems(int evaluationItemId, long userId, List<long> childsUsersIds);

        bool TemporaryDeleteEvaluationItems(int evaluationItemId,
           long userId,
           List<long> childsUsersIds);

        bool CompleteDeleteEvaluationItems(int evaluationItemId,
           //long userId,
           List<long> childsUsersIds);


        List<EvaluationItemsVM> GetEvaluationItemsWithEvalQuestionIds(List<int?> evalQuestionIds);


        EvaluationItemsVM GetEvaluationItemWithEvaluationItemId(int evaluationItemId);

        #endregion

        #region Methods For Work with EvaluationItemValues

        List<EvaluationItemValuesVM> GetEvaluationItemValuesByParentId(GetEvaluationItemValuesByParentIdPVM getEvaluationItemValuesByParentIdPVM);

        int UpdateEvaluationItemValues(UpdateEvaluationItemValuesPVM updateEvaluationItemValuesPVM);

        List<EvaluationItemValuesVM> GetEvaluationItemValuesWithEvalItemIds(List<int> evalItemIds);


        int AddToEvaluationItemValues(EvaluationItemValuesVM evaluationItemValuesVM);


        bool UpdateEvaluationItemValuesList(List<EvaluationItemValuesVM> evaluationItemValuesVMList);

        #endregion

        #region Methods For Work With ElementTypes



        #endregion

        #region Methods For Work With Features

        List<FeaturesVM> GetAllFeaturesList(ref int listCount,
             int? propertyTypeId = null,
             string featureTitleSearch = "");



        List<FeaturesVM> GetListOfFeatures(
            int jtStartIndex,
          int jtPageSize,
          ref int listCount,
          int? propertyTypeId = null,
           string? featureTitleSearch = ""/*,
           List<long> childsUsersIds,
           string Lang = null,
           string jtSorting = null
           /*long userId = 0*/);

        int AddToFeatures(FeaturesVM featuresVM, List<long> childsUsersIds);

        FeaturesVM GetFeatureWithFeatureId(int featureId);

        int UpdateFeatures(ref FeaturesVM featuresVM,
           //long userId,
           List<long> childsUsersIds);

        bool ToggleActivationFeatures(int featureId, long userId, List<long> childsUsersIds);

        bool TemporaryDeleteFeatures(int featureId,
           long userId,
           List<long> childsUsersIds);

        bool CompleteDeleteFeatures(int featureId,
           //long userId,
           List<long> childsUsersIds);
        #endregion

        #region Methods For Work With FeaturesCategories

        List<FeaturesCategoriesVM> GetAllFeaturesCategoriesList();

        int CreateFeaturesCategories(FeaturesCategoriesVM featuresCategoriesVM);

        #endregion

        #region Methods For Work With FeaturesOptions


        List<FeaturesOptionsVM> GetAllFeaturesOptionsList(ref int listCount,
           int featureId = 0);


        List<FeaturesOptionsVM> GetListOfFeaturesOptions(
           int jtStartIndex,
           int jtPageSize,
           ref int listCount,
           int featureId = 0);


        int AddToFeaturesOptions(FeaturesOptionsVM featuresOptionsVM);

        int UpdateFeaturesOptions(ref FeaturesOptionsVM featuresOptionsVM);

        bool ToggleActivationFeaturesOptions(int featureOptionId, long userId);

        bool TemporaryDeleteFeaturesOptions(int featureOptionId, long userId);

        bool CompleteDeleteFeaturesOptions(int featureOptionId);

        #endregion

        #region Methods For Work With FeaturesValues

        bool UpdatePropertyFeatures(long propertyId, List<FeaturesValuesVM> featuresValuesVMList);

        #endregion

        #region Methods for Work With Funds


        List<FundsVM> GetAllFundsList(
       int? fundId = null);
        List<FundsVM> GetListOfFunds(
           int jtStartIndex,
           int jtPageSize,
           ref int listCount,
           List<long> childsUsersIds,
           string? jtSorting = null);

        int AddToFunds(FundsVM fundsVM);

        int UpdateFunds(ref FundsVM fundsVM,
         List<long> childsUsersIds);

        bool ToggleActivationFunds(int FundId,
           long userId,
           List<long> childsUsersIds);

        bool TemporaryDeleteFunds(int FundId,
           long userId,
           List<long> childsUsersIds);

        bool CompleteDeleteFunds(int FundId,
           List<long> childsUsersIds);

        #endregion

        #region Methods For Work With IntroductionMethods
        #endregion

        #region Methods For Work With Investors
        List<InvestorsVM> GetAllInvestorsList(
         List<long> childsUsersIds,
         long? userId = null,
         string companyName = "",
         int? fundId = null);

        List<InvestorsVM> GetListOfInvestors(
                int jtStartIndex,
                int jtPageSize,
                ref int listCount,
                List<long> childsUsersIds,
                long? userId = null,
                string companyName = "",
                int? fundId = null,
                string jtSorting = null);

        int AddToInvestors(
                InvestorsVM investorsVM,
                IConsoleBusiness consoleBusiness,
                string domainName);


        int UpdateInvestors(ref InvestorsVM investorsVM,
          List<long> childsUsersIds);

        bool ToggleActivationInvestors(
            int investorId,
            long userId,
            IConsoleBusiness consoleBusiness,
            List<long> childsUsersIds);

        bool TemporaryDeleteInvestors(
                int investorId,
                long userId,
                List<long> childsUsersIds);


        bool CompleteDeleteInvestors(
            int investorId,
           List<long> childsUsersIds);


        InvestorsVM GetInvestorWithInvestorId(
            int? investorId,
            List<long> childsUsersIds);
        #endregion

        #region Methods for Work With MapLayerCategories

        List<MapLayerCategoriesVM> GetAllMapLayerCategoriesList(
         ref int listCount,
         List<long> childsUsersIds,
         string mapLayerCategoryTitle = "");

        List<MapLayerCategoriesVM> GetListOfMapLayerCategories(
           int jtStartIndex,
           int jtPageSize,
           ref int listCount,
           List<long> childsUsersIds,
           string mapLayerCategoryTitle = "",
           string jtSorting = null);


        int AddToMapLayerCategories(MapLayerCategoriesVM mapLayerCategoriesVM,
            IConsoleBusiness consoleBusiness,
           string domainName);

        int UpdateMapLayerCategories(ref MapLayerCategoriesVM mapLayerCategoriesVM,
           List<long> childsUsersIds);

        bool ToggleActivationMapLayerCategories(int mapLayerCategoryId,
           long userId,
           List<long> childsUsersIds);

        bool TemporaryDeleteMapLayerCategories(int mapLayerCategoryId,
           long userId,
           List<long> childsUsersIds);

        bool CompleteDeleteMapLayerCategories(int mapLayerCategoryId,
           List<long> childsUsersIds);

        MapLayerCategoriesVM GetMapLayerCategoryWithMapLayerCategoryId(
           int? mapLayerCategoryId,
           List<long> childsUsersIds);

        List<ChartMapCatNodeWithDataVM> GetAllMapLayerCategoriesHierarchical();

        List<PropertiesPricesForMapVM> GetListOfPropertiesPricesForMap(List<long> childsUsersIds,
            int Platform,//all, teniaco, melkavan
            long? PriceFrom,
            long? PriceTo,
            int? StateId,
            int? CityId,
            int? ZoneId,
            int? DistrictId,
            int? typeOfUseId,
            int? PropertyTypeId);


        #endregion

        #region Methods for Work With MapLayers

        List<MapLayersVM> GetAllMapLayersList(
       ref int listCount,
       List<long> childsUsersIds,
       int? mapLayerCategoryId = null);

        List<MapLayersVM> GetListOfMapLayers(
           int jtStartIndex,
           int jtPageSize,
           ref int listCount,
           List<long> childsUsersIds,
           int? mapLayerCategoryId = null,
           string jtSorting = null);


        int AddToMapLayersWithJsonData(MapLayersVM mapLayersVM, List<string> StrPolygonList, IConsoleBusiness consoleBusiness, string domainName);

        int AddToMapLayers(MapLayersVM mapLayersVM,
            IConsoleBusiness consoleBusiness,
           string domainName);

        int UpdateMapLayers(ref MapLayersVM mapLayersVM,
           List<long> childsUsersIds);


        bool ToggleActivationMapLayers(int mapLayerId,
          long userId,
          List<long> childsUsersIds);


        bool TemporaryDeleteMapLayers(int mapLayerId,
           long userId,
           List<long> childsUsersIds);

        bool CompleteDeleteMapLayersIds(CompleteDeleteMapLayersIdsPVM completeDeleteMapLayersIdsPVM);
        bool CompleteDeleteMapLayers(int mapLayerId,
           List<long> childsUsersIds);

        MapLayersVM GetMapLayerWithMapLayerId(
           int? mapLayerId,
           List<long> childsUsersIds);

        #endregion

        #region Methods For Work With MyProperties

        #region Methods For Work With MyPropertiesManagement
        List<MyPropertiesVM> GetAllMyPropertiesList(ref int listCount,
          List<long> childsUsersIds,
          PublicApiContext publicApiDb,
          int? propertyTypeId = null,
          int? typeOfUseId = null,
          int? documentTypeId = null,
          long? ConsultantUserId = null,
          long? ownerPersonId = null,
          string propertyCodeName = null,
          long? stateId = null,
          long? cityId = null,
          long? zoneId = null,
          long? districtId = null,
          string jtSorting = null);


        List<MyPropertiesVM> GetListOfMyProperties(int jtStartIndex,
             int jtPageSize,
             ref int listCount,
             List<long> childsUsersIds,
             PublicApiContext publicApiDb,
             int? propertyTypeId = null,
             int? typeOfUseId = null,
             int? documentTypeId = null,
             long? ConsultantUserId = null,
             long? ownerPersonId = null,
             string propertyCodeName = null,
             long? stateId = null,
             long? cityId = null,
             long? zoneId = null,
             long? districtId = null,
             string jtSorting = null,
             long? userId = null);


        long AddToMyProperties(MyPropertiesVM myPropertiesVM,
           IPublicApiBusiness publicApiBusiness);


        MyPropertiesVM GetMyPropertyWithMyPropertyId(long propertyId,
           List<long> childsUsersIds,
           PublicApiContext publicApiDb);
        long UpdateMyProperties(ref MyPropertiesVM myPropertiesVM,
          List<long> childsUsersIds);

        bool ToggleActivationMyProperties(long propertyId,
           long userId,
           List<long> childsUsersIds);

        bool TemporaryDeleteMyProperties(long propertyId,
           long userId,
           List<long> childsUsersIds);

        bool CompleteDeleteMyProperties(long propertyId,
           List<long> childsUsersIds);

        List<MyPropertiesCompareTopicVM> GetAllMyPropertiesCompareTopic(
            PublicApiContext publicApiDb,
            GetAllMyPropertiesCompareTopicPVM getAllMyPropertiesCompareTopicPVM);

        List<MyFeaturesValuesCompareVM> GetAllMyFeaturesValuesCompare(
           GetMyFeaturesValuesComparePVM getMyFeaturesValuesComparePVM);


        MyPropertiesCompareBasicInfoVM GetMyPropertiesCompareBasicInfo(
           PublicApiContext publicApiDb,
           GetMyPropertiesCompareBasicInfoPVM getMyPropertiesCompareBasicInfoPVM);

        List<PropertiesInfoVM> GetAllPropertiesInfo(GetAllPropertiesInfoPVM getAllPropertiesInfoPVM);
        #endregion

        #region Methods for Work With MyPropertyTypesManagement
        List<MyPropertyTypesVM> GetAllMyPropertyTypesList();
        #endregion

        #region  Methods for Work With MyPropertiesFeaturesManagement
        MyPropertyFeaturesValuesVM GetMyPropertyFeaturesValues(long propertyId,
       int propertyTypeId);


        bool UpdateMyPropertyFeatures(long propertyId, List<FeaturesValuesVM> featuresValuesVMList);
        #endregion

        #region Methods for Work With MyPropertyFilesManagement

        List<MyPropertyFilesVM> GetAllMyPropertyFilesList(ref int listCount,
       List<long> childsUsersIds,
       long? propertyId = null,
       string propertyFileTitle = "",
       string propertyFileType = "",
       string jtSorting = null);

        List<MyPropertyFilesVM> GetListOfMyPropertyFiles(int jtStartIndex,
            int jtPageSize,
            ref int listCount,
            List<long> childsUsersIds,
            long? propertyId = null,
            string propertyFileTitle = "",
            string propertyFileType = "",
            string jtSorting = null);

        bool AddToMyPropertyFiles(List<MyPropertyFilesVM> myPropertyFilesVMList);

        MyPropertyFilesVM GetMyPropertyFileWithMyPropertyFileId(int propertyFileId,
          List<long> childsUsersIds);

        bool UpdateMyPropertyFiles(ref MyPropertyFilesVM myPropertyFilesVM,
          List<long> childsUsersIds);

        bool ToggleActivationMyPropertyFiles(int propertyFileId,
        long userId,
        List<long> childsUsersIds);

        bool TemporaryDeleteMyPropertyFiles(int propertyFileId,
         long userId,
         List<long> childsUsersIds);
        bool CompleteDeleteMyPropertyFiles(int propertyFileId,
           List<long> childsUsersIds);

        bool UpdateListPropertyFilesTitle(List<UpdateListPropertyFilesTitlePVM> list, long userid, string edittime);

        (bool IsSuccess, List<PropertiesInfoVM>? finalResult) AddToMyPropertyFilesReg(List<MyPropertyFilesVM> myPropertyFilesVMList, int CurrentUserId);
        #endregion

        #region Methods For Work With MyPropertylocationManagement
        bool UpdateMyPropertyLocation(long userId,
           long propertyId,
           int stateId,
           int cityId,
           int zoneId,
           int districtId,
           string address,
           double locationLat,
           double locationLon);
        #endregion

        #region Methods For Work With MyPropertiesPricesHistoriesManagement
        List<MyPropertiesPricesHistoriesVM> GetListOfMyPropertiesPricesHistories(
            int jtStartIndex,
            int jtPageSize,
            ref int listCount,
            List<long> childsUsersIds,
            long propertyId,
            string jtSorting = null);
        #endregion

        #endregion

        #region OutterDashboard

        OutterDashboardPricesBlockVM GetOutterDashboardPricesBlock(long userId,
            long personId,
            IProjectsApiBusiness projectsApiBusiness);


        //پراکندگی سرمایه
        List<MyFundsVM> GetListOfMyFundsDispersion(long userId,
            long personId,
            IProjectsApiBusiness projectsApiBusiness);

        //رشد سرمایه
        List<MyFundsVM> GetListOfMyFundsGrowth(long userId,
            long personId,
            IProjectsApiBusiness projectsApiBusiness);

        //جزئیات داشبورد ( انحراف ها و تعداد مدارک تایید نشده و آگهی های نزدیک ، لیست مکالمات خوانده نشده و مدارک تایید نشده)
        Task<DetailsForOuterDashboardVM> GetDetailsForOuterDashboard(long userId,
            IProjectsApiBusiness projectsApiBusiness,
            ITeniacoApiBusiness teniacoApiBusiness,
            IMelkavanApiBusiness melkavanApiBusiness);

        // تعداد مدارک تایید نشده و مکالمات خوانده نشده برای زنگوله
        Task<GetUnreadConversationsAndUnverifiedFilesCountVM> GetUnreadConversationsAndUnverifiedFilesCount(long userId,
            IProjectsApiBusiness projectsApiBusiness);


        #region Methods For Work With MyProperties

        #region Methods For Work With MyPropertiesForInvestorsManagement
        List<PropertiesVM> GetAllMyPropertiesForInvestorsList(ref int listCount,
           List<long> childsUsersIds,
           PublicApiContext publicApiDb,
           int? propertyTypeId = null,
           int? typeOfUseId = null,
           int? documentTypeId = null,
           long? ConsultantUserId = null,
           long? OwnerId = null,
           string propertyCodeName = null,
           long? stateId = null,
           long? cityId = null,
           long? zoneId = null,
           long? districtId = null);


        List<MyPropertiesForInvestorsAdvanceSearchVM> GetListOfMyPropertiesForInvestors(int jtStartIndex,
               int jtPageSize,
               ref int listCount,
               List<long> childsUsersIds,
               PublicApiContext publicApiDb,
               int pageNum,
               int pageSize,
               int? propertyTypeId = null,
               int? typeOfUseId = null,
               int? documentTypeId = null,
               long? ConsultantUserId = null,
               long? OwnerId = null,
               string propertyCodeName = null,
               long? stateId = null,
               long? cityId = null,
               long? zoneId = null,
               long? districtId = null,
               long? userId = null);





        long AddToMyPropertiesForInvestors(PropertiesVM PropertiesVM,
            IPublicApiBusiness publicApiBusiness);


        PropertiesVM GetMyPropertyWithMyPropertyIdForInvestors(long propertyId,
            List<long> childsUsersIds,
            PublicApiContext publicApiDb);
        long UpdateMyPropertiesForInvestors(ref PropertiesVM PropertiesVM,
           List<long> childsUsersIds,
           long? UserId);

        bool CompleteDeleteMyPropertiesForInvestors(long propertyId,
            List<long> childsUsersIds);

        #endregion

        #region  Methods for Work With MyPropertiesFeaturesForInvestorsManagement
        PropertyFeaturesValuesVM GetMyPropertyFeaturesValuesForInvestors(long propertyId,
        int propertyTypeId);


        bool UpdateMyPropertyFeaturesForInvestors(long propertyId, List<FeaturesValuesVM> featuresValuesVMList);
        #endregion

        #region Methods for Work With MyPropertyFilesForInvestorsManagement

        List<PropertyFilesVM> GetAllMyPropertyFilesForInvestorsList(ref int listCount,
        List<long> childsUsersIds,
        long? propertyId = null,
        string propertyFileTitle = "",
        string propertyFileType = "");

        List<PropertyFilesVM> GetListOfMyPropertyFilesForInvestors(int jtStartIndex,
             int jtPageSize,
             ref int listCount,
             List<long> childsUsersIds,
             long? propertyId = null,
             string propertyFileTitle = "",
             string propertyFileType = "");

        bool AddToMyPropertyFilesForInvestors(List<PropertyFilesVM> myPropertyFilesVMList, long? PropertyId);

        bool AddMediaToPropertyFilesForInvestors(List<PropertyFilesVM> propertyFilesVMList,
          List<int>? DeletedPhotosIDs,
          bool? IsMainChanged,
          long? PropertyId);

        bool UpdateMyPropertyFilesForInvestors(ref PropertyFilesVM myPropertyFilesVM,
          List<long> childsUsersIds);

        bool CompleteDeleteMyPropertyFilesForInvestors(int propertyFileId,
           List<long> childsUsersIds);

        #endregion

        #region Methods For Work With MyPropertylocationForInvestorsManagement
        bool UpdateMyPropertyLocationForInvestors(long? userId,
           long propertyId,
           int? stateId,
           int? cityId,
           int? zoneId,
           int? districtId,
           string? address,
           double? locationLat,
           double? locationLon);
        #endregion

        #region Methods For Work With MyPropertiesPricesHistoriesForInvestorsManagement
        List<PropertiesPricesHistoriesVM> GetListOfMyPropertiesPricesHistoriesForInvestors(
            int jtStartIndex,
            int jtPageSize,
            ref int listCount,
            List<long> childsUsersIds,
            long propertyId);
        #endregion

        #endregion


        #region Methods For Work With MelkavanProperties


        List<MyPropertiesForInvestorsAdvanceSearchVM> GetListOfMelkavanProperties(int jtStartIndex,
                int jtPageSize,
                ref int listCount,
                List<long> childsUsersIds,
                PublicApiContext publicApiDb,
                int pageNum,
                int pageSize,
                int? propertyTypeId = null,
                int? typeOfUseId = null,
                int? documentTypeId = null,
                long? ConsultantUserId = null,
                long? OwnerId = null,
                string propertyCodeName = null,
                long? stateId = null,
                long? cityId = null,
                long? zoneId = null,
                long? districtId = null,
                long? userId = null);



        List<AdvertisementsListForMelkavanPropertiesVM> GetListOfNearAdvertisementsWithPropertyId(
                  int jtStartIndex,
                  int jtPageSize,
                  ref int listCount,
                  List<long> childsUsersIds,
                  PublicApiContext publicApiDb,
                  TeniacoApiContext teniacoApiDb,
                  MelkavanApiContext melkavanApiDb,
                  bool HaveCallers,
                  bool HaveAddress,
                  bool HaveFeature,
                  bool HaveViewers,
                  bool HaveDetails,
                  bool HaveTags,
                  bool HaveFiles,
                  long propertyId,
                  string recordType,
                  int? advertisementTypeId = null,
                  int? propertyTypeId = null,
                  int? typeOfUseId = null,
                  int? documentTypeId = null,
                  string propertyCodeName = null,
                  long? stateId = null,
                  long? cityId = null,
                  long? zoneId = null,
                  long? districtId = null,
                  string jtSorting = null,
                  long? userId = null,
                  string advertisementTitle = null);
        #endregion

        #endregion

        #region Methods For Work With Properties

        List<PropertiesVM> GetAllPropertiesList(ref int listCount,
            List<long> childsUsersIds,
            PublicApiContext publicApiDb,
            int? propertyTypeId = null,
            int? typeOfUseId = null,
            int? documentTypeId = null,
            long? ConsultantUserId = null,
            long? ownerPersonId = null,
            //int? documentOwnershipTypeId = null,
            //int? documentRootTypeId = null,
            string propertyCodeName = null,
            long? stateId = null,
            long? cityId = null,
            long? zoneId = null,
            long? districtId = null,
            bool? getFiles = null,
            bool? getAddress = null,
            bool? getPrices = null,
            bool? getPrice = null,
            bool? getFeatures = null,
            //string intermediary = null,
            //bool? isPrivate = null,
            string jtSorting = null);

        List<PropertiesVM> GetListOfProperties(int jtStartIndex,
            int jtPageSize,
            ref int listCount,
            List<long> childsUsersIds,
            PublicApiContext publicApiDb,
            int? propertyTypeId = null,
            int? typeOfUseId = null,
            int? documentTypeId = null,
            long? ConsultantUserId = null,
            long? ownerPersonId = null,
            //int? documentOwnershipTypeId = null,
            //int? documentRootTypeId = null,
            string propertyCodeName = null,
            long? stateId = null,
            long? cityId = null,
            long? zoneId = null,
            long? districtId = null,
            //string intermediary = null,
            //bool? isPrivate = null,
            string jtSorting = null,
            long? priceFrom = null,
            long? priceTo = null);

        List<PropertiesAdvanceSearchVM> GetListOfPropertiesAdvanceSearch(int jtStartIndex,
            int jtPageSize,
            ref int listCount,
            List<long> childsUsersIds,
            PublicApiContext publicApiDb,
            MelkavanApiContext melkavanApiDb,
            IConsoleBusiness consoleBusiness,
            List<int>? platform,
            int? propertyTypeId,
            int? slcPrice,
            double? priceFrom,
            double? priceTo,
            int? slcArea,
            double? areaFrom,
            double? areaTo,
            string? address,
            string? featuresAndDesc,
            int? typeOfUseId,
            int? documentTypeId,
            int? documentRootTypeId,
            int? documentOwnershipTypeId,
            string? propertyCodeName,
            long? consultantUserId,
            long? OwnerId,
            long? InvestorId,
            long? AdvertiserId,
            //List<string>?features,
            Dictionary<string, string>? features,
            long? stateId,
            long? cityId,
            long? zoneId,
            long? districtId,
            long? ThisUserId = null,
            bool? Participable = false,
            bool? Exchangeable = false,
            string jtSorting = null);

        long AddToProperties(PropertiesVM propertiesVM,
           IPublicApiBusiness publicApiBusiness);

        PropertiesVM GetPropertyWithPropertyId(long propertyId,
           List<long> childsUsersIds,
           PublicApiContext publicApiDb);

        long UpdateProperties(ref PropertiesVM propertiesVM,
           List<long> childsUsersIds);

        bool ToggleActivationProperties(long propertyId,
           long userId,
           List<long> childsUsersIds);

        bool TemporaryDeleteProperties(long propertyId,
           long userId,
           List<long> childsUsersIds);

        bool CompleteDeleteProperties(long propertyId,
           List<long> childsUsersIds);

        bool TemporaryDeletePropertiesWithChild(long propertyId,
          List<long> childsUsersIds);

        List<PropertiesVM> GetAllPropertiesListWithoutAddress(List<long> childsUsersIds);

        bool ToggleActivationShowInMelkavan(long propertyId,
             long userId);


        long AddPropertiesInMelkavan(ref PropertiesInMelkavanVM propertiesVM,
              List<long> childsUsersIds,
              IConsoleBusiness consoleBusiness);

        #endregion

        #region Methods For Work With PropertyAddress
        #endregion

        #region Methods For Work With Propertylocation

        bool UpdatePropertylocation(long userId,
             long propertyId,
             int stateId,
             int cityId,
             int zoneId,
             string townName,
             string address,
             double locationLat,
             double locationLon);

        #endregion

        #region Methods For Work With PropertyFiles

        List<PropertyFilesVM> GetAllPropertyFilesList(ref int listCount,
           List<long> childsUsersIds,
           long? propertyId = null,
           string propertyFileTitle = "",
           string propertyFileType = "",
           string jtSorting = null);

        List<PropertyFilesVM> GetListOfPropertyFiles(int jtStartIndex,
             int jtPageSize,
             ref int listCount,
             List<long> childsUsersIds,
             long? propertyId = null,
             string propertyFileTitle = "",
             string propertyFileType = "",
             string jtSorting = null);

        //bool AddToPropertyFiles(List<PropertyFilesVM> propertyFilesVMList);


        bool AddToPropertyFiles(List<PropertyFilesVM> propertyFilesVMList,
            List<int>? DeletedPhotosIDs,
            bool? IsMainChanged,
            long? PropertyId);
        PropertyFilesVM GetPropertyFileWithPropertyFileId(int propertyFileId,
           List<long> childsUsersIds);

        bool UpdatePropertyFiles(ref PropertyFilesVM propertyFilesVM,
           List<long> childsUsersIds);

        bool ToggleActivationPropertyFiles(int propertyFileId,
           long userId,
           List<long> childsUsersIds);

        bool TemporaryDeletePropertyFiles(int propertyFileId,
           long userId,
           List<long> childsUsersIds);

        bool CompleteDeletePropertyFiles(int propertyFileId,
           List<long> childsUsersIds);

        PropertyFeaturesValuesVM GetPropertyFeaturesValues(long propertyId,
           int propertyTypeId);

        #endregion

        #region Methods For Work With PropertyStates
        #endregion

        #region Methods For Work With PropertyTypes

        List<PropertyTypesVM> GetAllPropertyTypesList(
           ref int listCount,
           List<long> childsUsersIds);

        #endregion

        #region Methods for Work With PropertiesPricesHistories

        List<PropertiesPricesHistoriesVM> GetListOfPropertiesPricesHistories(
          int jtStartIndex,
          int jtPageSize,
          ref int listCount,
          List<long> childsUsersIds,
          long propertyId,
          string jtSorting = null);

        PropertiesPricesHistoriesVM GetLastPropertiesPriceHistoryByPropertyId(
            List<long> childsUsersIds,
           long propertyId);

        #endregion

        #region Methods for Work With Positions

        List<PositionsVM> GetAllPositionsList(
           ref int listCount,
           List<long> childsUsersIds,
           int? positionId);

        #endregion

        #region Methods For Work With PropertiesViewers

        List<PropertiesViewersVM> GetPropertiesViewersWithPropertyId(long? propertyId);


        int AddToPropertiesViewers(PropertiesViewersVM propertiesViewersVM);


        #endregion

        #region Methods For Work With PropertiesCallers
        List<PropertiesCallersVM> GetPropertiesCallersWithPropertyId(long? propertyId);


        int AddToPropertiesCallers(PropertiesCallersVM propertiesCallersVM);


        #endregion

        #region Methods For Work With PropertiesFavorites


        int AddToPropertiesFavorites(PropertiesFavoritesVM propertiesFavoritesVM);


        bool CompleteDeletePropertiesFavorites(long propertyId, long userId);
        #endregion

        #region Methods For Work With TypeOfUses

        List<TypeOfUsesVM> GetAllTypeOfUsesList();

        #endregion



        #endregion
    }
}
