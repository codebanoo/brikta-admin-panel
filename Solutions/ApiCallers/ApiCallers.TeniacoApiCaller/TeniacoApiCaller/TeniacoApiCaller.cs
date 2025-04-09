using ApiCallers.BaseApiCaller;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using VM.Base;
using VM.PVM.Melkavan;
using VM.PVM.Teniaco;
using VM.Teniaco;

namespace ApiCallers.TeniacoApiCaller
{
    public class TeniacoApiCaller : BaseCaller, ITeniacoApiCaller
    {
        public TeniacoApiCaller() : base()
        {
        }

        public TeniacoApiCaller(string _serviceUrl) : base(_serviceUrl)
        {
            serviceUrl = _serviceUrl;
        }

        public TeniacoApiCaller(string _serviceUrl,
            string _accessToken) :
            base(_serviceUrl,
                _accessToken)
        {
            serviceUrl = _serviceUrl;
        }

        #region AgenciesManagement

        public ResponseApiCaller GetAllAgenciesList(GetAllAgenciesListPVM getAllAgenciesListPVM)
        {
            return GetRecords(getAllAgenciesListPVM);


        }

        public ResponseApiCaller GetListOfAgencies(GetListOfAgenciesPVM getListOfAgenciesPVM)
        {
            return GetRecords(getListOfAgenciesPVM);
        }

        public ResponseApiCaller AddToAgencies(AddToAgenciesPVM addToAgenciesPVM)
        {
            return GetRecord(addToAgenciesPVM);

        }

        public ResponseApiCaller UpdateAgencies(UpdateAgenciesPVM updateAgenciesPVM)
        {
            return GetRecord(updateAgenciesPVM);
        }

        public ResponseApiCaller ToggleActivationAgencies(ToggleActivationAgenciesPVM toggleActivationAgenciesPVM)
        {
            return GetResult(toggleActivationAgenciesPVM);
        }

        public ResponseApiCaller TemporaryDeleteAgencies(TemporaryDeleteAgenciesPVM temporaryDeleteAgenciesPVM)
        {
            return GetResult(temporaryDeleteAgenciesPVM);
        }

        public ResponseApiCaller CompleteDeleteAgencies(CompleteDeleteAgenciesPVM completeDeleteAgenciesPVM)
        {
            return GetResult(completeDeleteAgenciesPVM);
        }

        public ResponseApiCaller GetAgencyWithAgencyId(GetAgencyWithAgencyIdPVM getAgencyWithAgencyIdPVM)
        {
            return GetRecord(getAgencyWithAgencyIdPVM);
        }

        #endregion

        #region AgencyLocationManagement
        public ResponseApiCaller UpdateAgencyLocation(UpdateAgencyLocationPVM updateAgencyLocationPVM)
        {
            return GetRecord(updateAgencyLocationPVM);
        }
        #endregion

        #region AgencyStaffsManagement

        public ResponseApiCaller GetAllAgencyStaffsList(GetAllAgencyStaffsListPVM getAllAgencyStaffsListPVM)
        {
            return GetRecords(getAllAgencyStaffsListPVM);
        }

        public ResponseApiCaller GetListOfAgencyStaffs(GetListOfAgencyStaffsPVM getListOfAgencyStaffsPVM)
        {
            return GetRecords(getListOfAgencyStaffsPVM);
        }

        public ResponseApiCaller AddToAgencyStaffs(AddToAgencyStaffsPVM addToAgencyStaffsPVM)
        {
            return GetRecord(addToAgencyStaffsPVM);

        }

        public ResponseApiCaller UpdateAgencyStaffs(UpdateAgencyStaffsPVM updateAgencyStaffsPVM)
        {
            return GetRecord(updateAgencyStaffsPVM);
        }

        public ResponseApiCaller ToggleActivationAgencyStaffs(ToggleActivationAgencyStaffsPVM toggleActivationAgencyStaffsPVM)
        {
            return GetResult(toggleActivationAgencyStaffsPVM);
        }

        public ResponseApiCaller TemporaryDeleteAgencyStaffs(TemporaryDeleteAgencyStaffsPVM temporaryDeleteAgencyStaffsPVM)
        {
            return GetResult(temporaryDeleteAgencyStaffsPVM);
        }

        public ResponseApiCaller CompleteDeleteAgencyStaffs(CompleteDeleteAgencyStaffsPVM completeDeleteAgencyStaffsPVM)
        {
            return GetResult(completeDeleteAgencyStaffsPVM);
        }

        public ResponseApiCaller GetAgencyStaffsWithAgencyStaffId(GetAgencyStaffWithAgencyStaffIdPVM getAgencyStaffsWithAgencyStaffIdPVM)
        {
            return GetRecord(getAgencyStaffsWithAgencyStaffIdPVM);
        }

        #endregion

        #region ContractorsManagement
        public ResponseApiCaller GetAllContractorsList(GetAllContractorsListPVM getAllContractorsListPVM)
        {
            return GetRecords(getAllContractorsListPVM);
        }

        public ResponseApiCaller GetListOfContractors(GetListOfContractorsPVM getListOfContractorsPVM)
        {
            return GetRecords(getListOfContractorsPVM);
        }

        public ResponseApiCaller AddToContractors(AddToContractorsPVM addToContractorsPVM)
        {
            return GetRecord(addToContractorsPVM);
        }

        public ResponseApiCaller UpdateContractors(UpdateContractorsPVM updateContractorsPVM)
        {
            return GetRecord(updateContractorsPVM);
        }

        public ResponseApiCaller ToggleActivationContractors(ToggleActivationContractorsPVM toggleActivationContractorsPVM)
        {
            return GetResult(toggleActivationContractorsPVM);
        }

        public ResponseApiCaller TemporaryDeleteContractors(TemporaryDeleteContractorsPVM temporaryDeleteContractorsPVM)
        {
            return GetResult(temporaryDeleteContractorsPVM);
        }

        public ResponseApiCaller CompleteDeleteContractors(CompleteDeleteContractorsPVM completeDeleteContractorsPVM)
        {
            return GetResult(completeDeleteContractorsPVM);
        }

        public ResponseApiCaller GetContractorWithContractorId(GetContractorWithContractorIdPVM getContractorWithContractorIdPVM)
        {
            return GetRecord(getContractorWithContractorIdPVM);
        }

        #endregion

        #region ComparePropertiesManagement

        public ResponseApiCaller GetAllComparePropertiesListByPersonId(GetAllComparePropertiesListByPersonIdPVM getAllComparePropertiesListByPersonIdPVM)
        {
            try
            {
                inputJson = JsonConvert.SerializeObject(getAllComparePropertiesListByPersonIdPVM);
                inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                response = client.PostAsync(serviceUrl, inputContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    responseApiCaller.IsSuccessStatusCode = response.IsSuccessStatusCode;

                    byte[] bytes = response.Content.ReadAsByteArrayAsync().Result;
                    string utfString = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

                    responseApiCaller.JsonResultWithRecordsObjectVM = JsonConvert.DeserializeObject<JsonResultWithRecordsObjectVM>(
                            utfString, new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
                }
            }
            catch (Exception exc)
            { }

            return responseApiCaller;
        }
        public ResponseApiCaller GetListOfComparePropertiesForBasicInfo(GetListOfComparePropertiesForBasicInfoPVM getListOfComparePropertiesForBasicInfoPVM)
        {
            try
            {
                inputJson = JsonConvert.SerializeObject(getListOfComparePropertiesForBasicInfoPVM);
                inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                response = client.PostAsync(serviceUrl, inputContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    responseApiCaller.IsSuccessStatusCode = response.IsSuccessStatusCode;

                    byte[] bytes = response.Content.ReadAsByteArrayAsync().Result;
                    string utfString = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

                    responseApiCaller.JsonResultWithRecordObjectVM = JsonConvert.DeserializeObject<JsonResultWithRecordObjectVM>(
                            utfString, new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
                }
            }
            catch (Exception exc)
            { }

            return responseApiCaller;
        }
        public ResponseApiCaller GetListOfCompareFeatureValues(GetListOfCompareFeatureValuesPVM getListOfCompareFeatureValuesPVM)
        {
            try
            {
                inputJson = JsonConvert.SerializeObject(getListOfCompareFeatureValuesPVM);
                inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                response = client.PostAsync(serviceUrl, inputContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    responseApiCaller.IsSuccessStatusCode = response.IsSuccessStatusCode;

                    byte[] bytes = response.Content.ReadAsByteArrayAsync().Result;
                    string utfString = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

                    responseApiCaller.JsonResultWithRecordsObjectVM = JsonConvert.DeserializeObject<JsonResultWithRecordsObjectVM>(
                            utfString, new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
                }
            }
            catch (Exception exc)
            { }

            return responseApiCaller;
        }
        public ResponseApiCaller GetListOfComparePropertiesAddress(GetListOfComparePropertiesAddressPVM getListOfComparePropertiesAddressPVM)
        {
            try
            {
                inputJson = JsonConvert.SerializeObject(getListOfComparePropertiesAddressPVM);
                inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                response = client.PostAsync(serviceUrl, inputContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    responseApiCaller.IsSuccessStatusCode = response.IsSuccessStatusCode;

                    byte[] bytes = response.Content.ReadAsByteArrayAsync().Result;
                    string utfString = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

                    responseApiCaller.JsonResultWithRecordsObjectVM = JsonConvert.DeserializeObject<JsonResultWithRecordsObjectVM>(
                            utfString, new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
                }
            }
            catch (Exception exc)
            { }

            return responseApiCaller;
        }
        public ResponseApiCaller GetListOfComparePropertiesPricesHistories(GetListOfComparePropertiesPricesHistoriesPVM getListOfComparePropertiesPricesHistoriesPVM)
        {
            try
            {
                inputJson = JsonConvert.SerializeObject(getListOfComparePropertiesPricesHistoriesPVM);
                inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                response = client.PostAsync(serviceUrl, inputContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    responseApiCaller.IsSuccessStatusCode = response.IsSuccessStatusCode;

                    byte[] bytes = response.Content.ReadAsByteArrayAsync().Result;
                    string utfString = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

                    responseApiCaller.JsonResultWithRecordsObjectVM = JsonConvert.DeserializeObject<JsonResultWithRecordsObjectVM>(
                            utfString, new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
                }
            }
            catch (Exception exc)
            { }

            return responseApiCaller;
        }
        #endregion

        #region DocumentOwnershipTypesManagement

        public ResponseApiCaller GetAllDocumentOwnershipTypesList(GetAllDocumentOwnershipTypesListPVM getAllDocumentOwnershipTypesListPVM)
        {
            return GetRecords(getAllDocumentOwnershipTypesListPVM);
        }

        #endregion

        #region DocumentRootTypesManagement

        public ResponseApiCaller GetAllDocumentRootTypesList(GetAllDocumentRootTypesListPVM getAllDocumentRootTypesListPVM)
        {
            return GetRecords(getAllDocumentRootTypesListPVM);
        }

        #endregion

        #region DocumentTypesManagement

        public ResponseApiCaller GetAllDocumentTypesList(GetAllDocumentTypesListPVM getAllDocumentTypesListPVM)
        {
            return GetRecords(getAllDocumentTypesListPVM);
        }

        #endregion

        #region EvaluationSubjectManagement
        public ResponseApiCaller GetAllEvaluationSubjectsList(GetAllEvaluationSubjectsListPVM getAllEvaluationSubjectsList)
        {
            return GetRecords(getAllEvaluationSubjectsList);
        }

        #endregion

        #region EvaluationsManagement

        public ResponseApiCaller GetAllEvaluationsList(GetAllEvaluationsListPVM getAllEvaluationsListPVM)
        {
            return GetRecords(getAllEvaluationsListPVM);
        }

        public ResponseApiCaller GetListOfEvaluations(GetListOfEvaluationsPVM GetListOfEvaluationsPVM)
        {
            return GetRecords(GetListOfEvaluationsPVM);
        }

        public ResponseApiCaller AddToEvaluations(AddToEvaluationsPVM addToEvaluationsPVM)
        {
            return GetRecord(addToEvaluationsPVM);

        }

        public ResponseApiCaller UpdateEvaluations(UpdateEvaluationsPVM updateEvaluationsPVM)
        {
            return GetRecord(updateEvaluationsPVM);
        }

        public ResponseApiCaller ToggleActivationEvaluations(ToggleActivationEvaluationsPVM toggleActivationEvaluationsPVM)
        {
            return GetResult(toggleActivationEvaluationsPVM);
        }

        public ResponseApiCaller TemporaryDeleteEvaluations(TemporaryDeleteEvaluationsPVM temporaryDeleteEvaluationsPVM)
        {
            return GetResult(temporaryDeleteEvaluationsPVM);
        }

        public ResponseApiCaller CompleteDeleteEvaluations(CompleteDeleteEvaluationsPVM completeDeleteEvaluationsPVM)
        {
            return GetResult(completeDeleteEvaluationsPVM);
        }

        public ResponseApiCaller GetEvaluationsWithEvaluationId(GetEvaluationsWithEvaluationIdPVM getEvaluationsWithEvaluationsIdPVM)
        {
            return GetRecord(getEvaluationsWithEvaluationsIdPVM);
        }

        #endregion

        #region EvaluationCategoriesManagement

        public ResponseApiCaller GetAllEvaluationCategoriesList(GetAllEvaluationCategoriesListPVM getAllEvaluationCategoriesListPVM)
        {
            return GetRecords(getAllEvaluationCategoriesListPVM);
        }

        public ResponseApiCaller GetListOfEvaluationCategories(GetListOfEvaluationCategoriesPVM getListOfEvaluationCategoriesPVM)
        {
            return GetRecords(getListOfEvaluationCategoriesPVM);
        }

        public ResponseApiCaller AddToEvaluationCategories(AddToEvaluationCategoriesPVM addToEvaluationCategoriesPVM)
        {
            return GetRecord(addToEvaluationCategoriesPVM);
        }

        public ResponseApiCaller GetEvaluationCategoryWithEvaluationCategoryId(GetEvaluationCategoryWithEvaluationCategoryIdPVM getEvaluationCategoryWithEvaluationCategoryIdPVM)
        {
            return GetRecord(getEvaluationCategoryWithEvaluationCategoryIdPVM);
        }

        public ResponseApiCaller UpdateEvaluationCategories(UpdateEvaluationCategoriesPVM updateEvaluationCategoriesPVM)
        {
            return GetRecord(updateEvaluationCategoriesPVM);
        }

        public ResponseApiCaller ToggleActivationEvaluationCategories(ToggleActivationEvaluationCategoriesPVM toggleActivationEvaluationCategoriesPVM)
        {
            return GetResult(toggleActivationEvaluationCategoriesPVM);
        }

        public ResponseApiCaller TemporaryDeleteEvaluationCategories(TemporaryDeleteEvaluationCategoriesPVM temporaryDeleteEvaluationCategoriesPVM)
        {
            return GetResult(temporaryDeleteEvaluationCategoriesPVM);
        }

        public ResponseApiCaller CompleteDeleteEvaluationCategories(CompleteDeleteEvaluationCategoriesPVM completeDeleteEvaluationCategoriesPVM)
        {
            return GetResult(completeDeleteEvaluationCategoriesPVM);
        }


        public ResponseApiCaller GetAllDivisionOfEvaluationsListByParentId(GetAllDivisionOfEvaluationsListByParentIdPVM getAllDivisionOfEvaluationsListByParentIdPVM)
        {
            return GetRecords(getAllDivisionOfEvaluationsListByParentIdPVM);
        }

        #endregion

        #region EvaluationQuestionsManagement
        public ResponseApiCaller GetAllEvaluationQuestionsList(GetAllEvaluationQuestionsListPVM getAllEvaluationQuestionsListPVM)
        {
            return GetRecords(getAllEvaluationQuestionsListPVM);
        }

        public ResponseApiCaller GetListOfEvaluationQuestions(GetListOfEvaluationQuestionsPVM getListOfEvaluationQuestionsPVM)
        {
            return GetRecords(getListOfEvaluationQuestionsPVM);
        }

        public ResponseApiCaller AddToEvaluationQuestions(AddToEvaluationQuestionsPVM addToEvaluationQuestionsPVM)
        {
            return GetRecord(addToEvaluationQuestionsPVM);
        }

        public ResponseApiCaller GetEvaluationQuestionWithEvaluationQuestionId(GetEvaluationQuestionWithEvaluationQuestionIdPVM getEvaluationQuestionWithEvaluationQuestionIdPVM)
        {
            return GetRecord(getEvaluationQuestionWithEvaluationQuestionIdPVM);
        }

        public ResponseApiCaller UpdateEvaluationQuestions(UpdateEvaluationQuestionsPVM updateEvaluationQuestionsPVM)
        {
            return GetRecord(updateEvaluationQuestionsPVM);
        }

        public ResponseApiCaller ToggleActivationEvaluationQuestions(ToggleActivationEvaluationQuestionsPVM toggleActivationEvaluationQuestionsPVM)
        {
            return GetResult(toggleActivationEvaluationQuestionsPVM);
        }

        public ResponseApiCaller TemporaryDeleteEvaluationQuestions(TemporaryDeleteEvaluationQuestionsPVM temporaryDeleteEvaluationQuestionsPVM)
        {
            return GetResult(temporaryDeleteEvaluationQuestionsPVM);
        }

        public ResponseApiCaller CompleteDeleteEvaluationQuestions(CompleteDeleteEvaluationQuestionsPVM completeDeleteEvaluationQuestionsPVM)
        {
            return GetResult(completeDeleteEvaluationQuestionsPVM);
        }

        #endregion

        #region EvaluationItemsManagement

        public ResponseApiCaller GetAllEvaluationItemsList(GetAllEvaluationItemsListPVM getAllEvaluationItemsListPVM)
        {
            return GetRecords(getAllEvaluationItemsListPVM);
        }

        public ResponseApiCaller GetListOfEvaluationItems(GetListOfEvaluationItemsPVM getListOfEvaluationItemsPVM)
        {
            return GetRecords(getListOfEvaluationItemsPVM);
        }

        public ResponseApiCaller AddToEvaluationItems(AddToEvaluationItemsPVM addToEvaluationItemsPVM)
        {
            return GetRecord(addToEvaluationItemsPVM);
        }

        public ResponseApiCaller GetEvaluationItemWithEvaluationItemId(GetEvaluationItemWithEvaluationItemIdPVM getEvaluationItemWithEvaluationItemIdPVM)
        {
            return GetRecord(getEvaluationItemWithEvaluationItemIdPVM);
        }

        public ResponseApiCaller UpdateEvaluationItems(UpdateEvaluationItemsPVM updateEvaluationItemsPVM)
        {
            return GetRecord(updateEvaluationItemsPVM);
        }

        public ResponseApiCaller ToggleActivationEvaluationItems(ToggleActivationEvaluationItemsPVM toggleActivationEvaluationItemsPVM)
        {
            return GetResult(toggleActivationEvaluationItemsPVM);
        }

        public ResponseApiCaller TemporaryDeleteEvaluationItems(TemporaryDeleteEvaluationItemsPVM temporaryDeleteEvaluationItemsPVM)
        {
            return GetResult(temporaryDeleteEvaluationItemsPVM);
        }

        public ResponseApiCaller CompleteDeleteEvaluationItems(CompleteDeleteEvaluationItemsPVM completeDeleteEvaluationItemsPVM)
        {
            return GetResult(completeDeleteEvaluationItemsPVM);
        }

        #endregion

        #region EvaluationItemValuesManagement

        public ResponseApiCaller GetEvaluationItemValuesByParentId(GetEvaluationItemValuesByParentIdPVM getEvaluationItemValuesByParentIdPVM)
        {
            return GetRecords(getEvaluationItemValuesByParentIdPVM);
        }

        public ResponseApiCaller AddToEvaluationItemValues(QuestionSheetPVM questionSheetPVM)
        {
            return GetRecord(questionSheetPVM);
        }

        public ResponseApiCaller UpdateEvaluationItemValues(UpdateEvaluationItemValuesPVM updateEvaluationItemValuesPVM)
        {
            return GetRecord(updateEvaluationItemValuesPVM);
        }


        public ResponseApiCaller AddToEvaluationItemValues(AddToEvaluationItemValuesPVM addToEvaluationItemValuesPVM)
        {
            return GetRecord(addToEvaluationItemValuesPVM);
        }


        public ResponseApiCaller UpdateEvaluationItemValuesList(UpdateEvaluationItemValuesListPVM updateEvaluationItemValuesListPVM)
        {
            return GetRecord(updateEvaluationItemValuesListPVM);

        }


        #endregion

        #region FeaturesManagement
        #endregion

        #region FeaturesCategoriesManagement

        public ResponseApiCaller GetAllFeaturesCategoriesList(GetAllFeaturesCategoriesListPVM getAllFeaturesCategoriesListPVM)
        {
            return GetRecords(getAllFeaturesCategoriesListPVM);
        }

        public ResponseApiCaller CreateFeaturesCategories(CreateFeaturesCategoriesPVM createFeaturesCategoriesPVM)
        {
            return GetRecord(createFeaturesCategoriesPVM);
        }

        #endregion

        #region FeaturesOptionsManagement
        #endregion

        #region FeaturesValuesManagement
        #endregion

        #region FeaturesManagement

        public ResponseApiCaller GetListOfFeatures(GetListOfFeaturesPVM getListOfFeaturesPVM)
        {
            return GetRecords(getListOfFeaturesPVM);
        }

        public ResponseApiCaller AddToFeatures(AddToFeaturesPVM addToFeaturesPVM)
        {
            return GetRecord(addToFeaturesPVM);
        }

        public ResponseApiCaller GetFeatureWithFeatureId(GetFeatureWithFeatureIdPVM getFeatureWithFeatureIdPVM)
        {
            return GetRecord(getFeatureWithFeatureIdPVM);
        }

        public ResponseApiCaller UpdateFeatures(UpdateFeaturesPVM updateFeaturesPVM)
        {
            return GetRecord(updateFeaturesPVM);
        }

        public ResponseApiCaller ToggleActivationFeatures(ToggleActivationFeaturesPVM toggleActivationFeaturesPVM)
        {
            return GetResult(toggleActivationFeaturesPVM);
        }

        public ResponseApiCaller TemporaryDeleteFeatures(TemporaryDeleteFeaturesPVM temporaryDeleteFeaturesPVM)
        {
            return GetResult(temporaryDeleteFeaturesPVM);
        }

        public ResponseApiCaller CompleteDeleteFeatures(CompleteDeleteFeaturesPVM completeDeleteFeaturesPVM)
        {
            return GetRecord(completeDeleteFeaturesPVM);
        }

        public ResponseApiCaller GetAllFeaturesList(GetAllFeaturesListPVM getAllFeaturesListPVM)
        {
            return GetRecords(getAllFeaturesListPVM);
        }

        #endregion

        #region FeaturesOptionsManagement

        public ResponseApiCaller GetAllFeaturesOptionsList(GetAllFeaturesOptionsListPVM getAllFeaturesOptionsListPVM)
        {
            return GetRecords(getAllFeaturesOptionsListPVM);
        }

        public ResponseApiCaller GetListOfFeaturesOptions(GetListOfFeaturesOptionsPVM getListOfFeaturesOptionsPVM)
        {
            return GetRecords(getListOfFeaturesOptionsPVM);
        }

        public ResponseApiCaller AddToFeaturesOptions(AddToFeaturesOptionsPVM addToFeaturesOptionsPVM)
        {
            return GetRecord(addToFeaturesOptionsPVM);
        }

        public ResponseApiCaller UpdateFeaturesOptions(UpdateFeaturesOptionsPVM updateFeaturesOptionsPVM)
        {
            return GetRecord(updateFeaturesOptionsPVM);
        }

        public ResponseApiCaller ToggleActivationFeaturesOptions(ToggleActivationFeaturesOptionsPVM toggleActivationFeaturesOptionsPVM)
        {
            return GetResult(toggleActivationFeaturesOptionsPVM);
        }

        public ResponseApiCaller TemporaryDeleteFeaturesOptions(TemporaryDeleteFeaturesOptionsPVM temporaryDeleteFeaturesOptionsPVM)
        {
            return GetResult(temporaryDeleteFeaturesOptionsPVM);
        }

        public ResponseApiCaller CompleteDeleteFeaturesOptions(CompleteDeleteFeaturesOptionsPVM completeDeleteFeaturesOptionsPVM)
        {
            return GetResult(completeDeleteFeaturesOptionsPVM);
        }

        #endregion

        #region FundsManagement
        public ResponseApiCaller GetAllFundsList(GetAllFundsListPVM getAllFundsListPVM)
        {
            return GetRecords(getAllFundsListPVM);
        }
        public ResponseApiCaller GetListOfFunds(GetListOfFundsPVM getListOfFundsPVM)
        {
            return GetRecords(getListOfFundsPVM);
        }
        public ResponseApiCaller AddToFunds(AddToFundsPVM addToFundsPVM)
        {
            return GetRecord(addToFundsPVM);

        }
        public ResponseApiCaller UpdateFunds(UpdateFundPVM updateFundPVM)
        {
            return GetRecord(updateFundPVM);
        }
        public ResponseApiCaller ToggleActivationFunds(ToggleActivationFundPVM toggleActivationFundPVM)
        {
            return GetResult(toggleActivationFundPVM);
        }
        public ResponseApiCaller TemporaryDeleteFunds(TemporaryDeleteFundPVM temporaryDeleteFundPVM)
        {
            return GetResult(temporaryDeleteFundPVM);
        }
        public ResponseApiCaller CompleteDeleteFunds(CompleteDeleteFundsPVM completeDeleteFundsPVM)
        {
            return GetResult(completeDeleteFundsPVM);
        }
        public ResponseApiCaller GetFundWithFundId(GetFundWithFundIdPVM getFundWithFundIdPVM)
        {
            return GetRecord(getFundWithFundIdPVM);
        }

        #endregion

        #region GeoJsonFileManagement
        public ResponseApiCaller AddToMapLayerFiles(AddToMapLayerFilesPVM addToMapLayerFilesPVM)
        {
            return GetRecord(addToMapLayerFilesPVM);

        }

        #endregion

        #region InvestorsManagement

        public ResponseApiCaller GetAllInvestorsList(GetAllInvestorsListPVM getAllInvestorsListPVM)
        {
            return GetRecords(getAllInvestorsListPVM);
        }

        public ResponseApiCaller GetListOfInvestors(GetListOfInvestorsPVM getListOfInvestorsPVM)
        {
            return GetRecords(getListOfInvestorsPVM);
        }

        public ResponseApiCaller AddToInvestors(AddToInvestorsPVM addToInvestorsPVM)
        {
            return GetRecord(addToInvestorsPVM);

        }

        public ResponseApiCaller UpdateInvestors(UpdateInvestorsPVM updateInvestorsPVM)
        {

            #region comments

            //try
            //{
            //    inputJson = JsonConvert.SerializeObject(updateInvestorsPVM);
            //    inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
            //    response = client.PostAsync(serviceUrl, inputContent).Result;

            //    if (response.IsSuccessStatusCode)
            //    {
            //        responseApiCaller.IsSuccessStatusCode = response.IsSuccessStatusCode;

            //        byte[] bytes = response.Content.ReadAsByteArrayAsync().Result;
            //        string utfString = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

            //        responseApiCaller.JsonResultWithRecordsObjectVM = JsonConvert.DeserializeObject<JsonResultWithRecordsObjectVM>(
            //                utfString, new JsonSerializerSettings
            //                {
            //                    NullValueHandling = NullValueHandling.Ignore
            //                });
            //    }
            //}
            //catch (Exception exc)
            //{ }

            //return responseApiCaller;
            #endregion

            return GetRecord(updateInvestorsPVM);
        }

        public ResponseApiCaller ToggleActivationInvestors(ToggleActivationInvestorsPVM toggleActivationInvestorsPVM)
        {
            return GetResult(toggleActivationInvestorsPVM);
        }

        public ResponseApiCaller TemporaryDeleteInvestors(TemporaryDeleteInvestorsPVM temporaryDeleteInvestorsPVM)
        {
            return GetResult(temporaryDeleteInvestorsPVM);
        }

        public ResponseApiCaller CompleteDeleteInvestors(CompleteDeleteInvestorsPVM completeDeleteInvestorsPVM)
        {
            return GetResult(completeDeleteInvestorsPVM);
        }

        public ResponseApiCaller GetInvestorWithInvestorId(GetInvestorWithInvestorIdPVM getInvestorWithInvestorIdPVM)
        {
            return GetRecord(getInvestorWithInvestorIdPVM);
        }

        #endregion

        #region IntroductionMethodsManagement
        #endregion

        #region InvestorsFavoritesManagement
        #endregion

        #region InvestorsRequestsManagement
        #endregion

        #region MapLayerCategoriesManagement

        public ResponseApiCaller GetAllMapLayerCategoriesList(GetAllMapLayerCategoriesListPVM getAllMapLayerCategoriesListPVM)
        {
            return GetRecords(getAllMapLayerCategoriesListPVM);
        }

        public ResponseApiCaller GetListOfMapLayerCategories(GetListOfMapLayerCategoriesPVM getListOfMapLayerCategoriesPVM)
        {
            return GetRecords(getListOfMapLayerCategoriesPVM);
        }

        public ResponseApiCaller AddToMapLayerCategories(AddToMapLayerCategoriesPVM addToMapLayerCategoriesPVM)
        {
            return GetRecord(addToMapLayerCategoriesPVM);
        }

        public ResponseApiCaller UpdateMapLayerCategories(UpdateMapLayerCategoriesPVM updateMapLayerCategoriesPVM)
        {
            return GetRecord(updateMapLayerCategoriesPVM);
        }

        public ResponseApiCaller ToggleActivationMapLayerCategories(ToggleActivationMapLayerCategoriesPVM toggleActivationMapLayerCategoriesPVM)
        {
            return GetResult(toggleActivationMapLayerCategoriesPVM);
        }

        public ResponseApiCaller TemporaryDeleteMapLayerCategories(TemporaryDeleteMapLayerCategoriesPVM temporaryDeleteMapLayerCategoriesPVM)
        {
            return GetResult(temporaryDeleteMapLayerCategoriesPVM);
        }

        public ResponseApiCaller CompleteDeleteMapLayerCategories(CompleteDeleteMapLayerCategoriesPVM completeDeleteMapLayerCategoriesPVM)
        {
            return GetResult(completeDeleteMapLayerCategoriesPVM);
        }

        public ResponseApiCaller GetMapLayerCategoryWithMapLayerCategoryId(GetMapLayerCategoryWithMapLayerCategoryIdPVM getMapLayerCategoryWithMapLayerCategoryIdPVM)
        {
            return GetRecord(getMapLayerCategoryWithMapLayerCategoryIdPVM);
        }
        #endregion

        #region MapLayersManagement
        public ResponseApiCaller GetAllMapLayersList(GetAllMapLayersListPVM getAllMapLayersListPVM)
        {
            return GetRecords(getAllMapLayersListPVM);
        }

        public ResponseApiCaller GetListOfMapLayers(GetListOfMapLayersPVM getListOfMapLayersPVM)
        {
            return GetRecords(getListOfMapLayersPVM);
        }

        public ResponseApiCaller AddToMapLayersWithJsonData(AddToMapLayersJsonDataPVM addToMapLayersWithJsonData)
        {
            return GetRecord(addToMapLayersWithJsonData);
        }
        public ResponseApiCaller AddToMapLayers(AddToMapLayersPVM addToMapLayersPVM)
        {
            return GetRecord(addToMapLayersPVM);

        }
        public ResponseApiCaller UpdateMapLayers(UpdateMapLayersPVM updateMapLayersPVM)
        {
            return GetRecord(updateMapLayersPVM);
        }
        public ResponseApiCaller ToggleActivationMapLayers(ToggleActivationMapLayersPVM toggleActivationMapLayersPVM)
        {
            return GetResult(toggleActivationMapLayersPVM);
        }
        public ResponseApiCaller TemporaryDeleteMapLayers(TemporaryDeleteMapLayersPVM temporaryDeleteMapLayersPVM)
        {
            return GetResult(temporaryDeleteMapLayersPVM);
        }
        public ResponseApiCaller CompleteDeleteMapLayers(CompleteDeleteMapLayersPVM completeDeleteMapLayersPVM)
        {
            return GetResult(completeDeleteMapLayersPVM);
        }
        public ResponseApiCaller CompleteDeleteMapLayersIds(CompleteDeleteMapLayersIdsPVM completeDeleteMapLayersIdsPVM)
        {
            return GetResult(completeDeleteMapLayersIdsPVM);
        }
        public ResponseApiCaller GetMapLayerWithMapLayerId(GetMapLayerWithMapLayerIdPVM getMapLayerWithMapLayerIdPVM)
        {
            return GetRecord(getMapLayerWithMapLayerIdPVM);
        }
        #endregion

        #region MyProperties

        #region MyPropertiesManagement
        public ResponseApiCaller GetAllPropertiesInfo(GetAllPropertiesInfoPVM getAllPropertiesInfoPVM)
        {
            return GetRecords(getAllPropertiesInfoPVM);
        }
        public ResponseApiCaller GetAllMyPropertiesList(GetAllMyPropertiesListPVM getAllMyPropertiesListPVM)
        {
            return GetRecords(getAllMyPropertiesListPVM);
        }

        public ResponseApiCaller GetListOfMyProperties(GetListOfMyPropertiesPVM getListOfMyPropertiesPVM)
        {
            return GetRecords(getListOfMyPropertiesPVM);
        }

        public ResponseApiCaller AddToMyProperties(AddToMyPropertiesPVM addToMyPropertiesPVM)
        {
            return GetRecord(addToMyPropertiesPVM);
        }

        public ResponseApiCaller GetMyPropertyWithMyPropertyId(GetMyPropertyWithMyPropertyIdPVM getMyPropertyWithMyPropertyIdPVM)
        {
            return GetRecord(getMyPropertyWithMyPropertyIdPVM);
        }

        public ResponseApiCaller UpdateMyProperties(UpdateMyPropertiesPVM updateMyPropertiesPVM)
        {
            return GetRecord(updateMyPropertiesPVM);
        }

        public ResponseApiCaller ToggleActivationMyProperties(ToggleActivationMyPropertiesPVM toggleActivationMyPropertiesPVM)
        {
            return GetResult(toggleActivationMyPropertiesPVM);
        }

        public ResponseApiCaller TemporaryDeleteMyProperties(TemporaryDeleteMyPropertiesPVM temporaryDeleteMyPropertiesPVM)
        {
            return GetResult(temporaryDeleteMyPropertiesPVM);
        }

        public ResponseApiCaller CompleteDeleteMyProperties(CompleteDeleteMyPropertiesPVM completeDeleteMyPropertiesPVM)
        {
            return GetResult(completeDeleteMyPropertiesPVM);
        }

        public ResponseApiCaller GetAllMyFeaturesValuesCompare(GetMyFeaturesValuesComparePVM getMyFeaturesValuesComparePVM)
        {
            return GetRecords(getMyFeaturesValuesComparePVM);
        }

        public ResponseApiCaller GetMyPropertiesCompareBasicInfo(GetMyPropertiesCompareBasicInfoPVM getMyPropertiesCompareBasicInfoPVM)
        {
            return GetRecord(getMyPropertiesCompareBasicInfoPVM);
        }

        public ResponseApiCaller GetAllMyPropertiesCompareTopic(GetAllMyPropertiesCompareTopicPVM getAllMyPropertiesCompareTopicPVM)
        {
            return GetRecords(getAllMyPropertiesCompareTopicPVM);
        }
        #endregion

        #region MyPropertiesLocationManagement

        public ResponseApiCaller UpdateMyPropertyLocation(UpdateMyPropertyLocationPVM updateMyPropertylocationPVM)
        {
            return GetResult(updateMyPropertylocationPVM);
        }

        #endregion

        #region MyPropertyFilesManagement

        public ResponseApiCaller GetAllMyPropertyFilesList(GetAllMyPropertyFilesListPVM getAllMyPropertyFilesListPVM)
        {
            return GetRecords(getAllMyPropertyFilesListPVM);
        }

        public ResponseApiCaller GetListOfMyPropertyFiles(GetListOfMyPropertyFilesPVM getListOfMyPropertyFilesPVM)
        {
            return GetRecords(getListOfMyPropertyFilesPVM);
        }
        public ResponseApiCaller UpdateListPropertyFilesTitle(List<UpdateListPropertyFilesTitlePVM> lst)
        {
            return GetResult(lst);
        }
        public ResponseApiCaller AddToMyPropertyFilesReg(AddToMyPropertyFilesPVM addToMyPropertyFilesPVM)
        {
            return GetRecords(addToMyPropertyFilesPVM);
        }
        public ResponseApiCaller AddToMyPropertyFiles(AddToMyPropertyFilesPVM addToMyPropertyFilesPVM)
        {
            return GetRecord(addToMyPropertyFilesPVM);
        }

        public ResponseApiCaller GetMyPropertyFileWithMyPropertyFileId(GetMyPropertyFileWithMyPropertyFileIdPVM getMyPropertyFileWithMyPropertyFileIdPVM)
        {
            return GetRecord(getMyPropertyFileWithMyPropertyFileIdPVM);
        }

        public ResponseApiCaller UpdateMyPropertyFiles(UpdateMyPropertyFilesPVM updateMyPropertyFilesPVM)
        {
            return GetRecord(updateMyPropertyFilesPVM);
        }

        public ResponseApiCaller ToggleActivationMyPropertyFiles(ToggleActivationMyPropertyFilesPVM toggleActivationMyPropertyFilesPVM)
        {
            return GetResult(toggleActivationMyPropertyFilesPVM);
        }

        public ResponseApiCaller TemporaryDeleteMyPropertyFiles(TemporaryDeleteMyPropertyFilesPVM temporaryDeleteMyPropertyFilesPVM)
        {
            return GetResult(temporaryDeleteMyPropertyFilesPVM);
        }

        public ResponseApiCaller CompleteDeleteMyPropertyFiles(CompleteDeleteMyPropertyFilesPVM completeDeleteMyPropertyFilesPVM)
        {
            return GetResult(completeDeleteMyPropertyFilesPVM);
        }

        #endregion

        #region MyPropertiesFeaturesManagement

        public ResponseApiCaller GetMyPropertyFeaturesValues(GetMyPropertyFeaturesValuesPVM getMyPropertyFeaturesValuesPVM)
        {
            return GetRecord(getMyPropertyFeaturesValuesPVM);
        }

        public ResponseApiCaller UpdateMyPropertyFeatures(UpdateMyPropertyFeaturesPVM updateMyPropertyFeaturesPVM)
        {
            return GetResult(updateMyPropertyFeaturesPVM);
        }

        #endregion

        #region MyPropertyTypesManagement

        public ResponseApiCaller GetAllMyPropertyTypesList(GetAllPropertyTypesListPVM getAllPropertyTypesListPVM)
        {
            return GetRecords(getAllPropertyTypesListPVM);
        }

        #endregion

        #region MyPropertiesPricesHistoriesManagement

        public ResponseApiCaller GetListOfMyPropertiesPricesHistories(GetListOfMyPropertiesPricesHistoriesPVM getListOfMyPropertiesPricesHistoriesPVM)
        {
            return GetRecords(getListOfMyPropertiesPricesHistoriesPVM);
        }

        public ResponseApiCaller GetLastPropertiesPriceHistoryByPropertyId(GetListOfPropertiesPricesHistoriesPVM getListOfPropertiesPricesHistoriesPVM)
        {
            return GetRecord(getListOfPropertiesPricesHistoriesPVM);
        }

        #endregion

        #region MyPropertiesForInvestors

        #region MyPropertiesForInvestorsManagement
        public ResponseApiCaller GetAllMyPropertiesForInvestorsList(GetAllMyPropertiesForInvestorsListPVM getAllMyPropertiesForInvestorsListPVM)
        {
            return GetRecords(getAllMyPropertiesForInvestorsListPVM);
        }

        public ResponseApiCaller GetListOfMyPropertiesForInvestors(GetListOfMyPropertiesForInvestorsPVM getListOfMyPropertiesForInvestorsPVM)
        {
            return GetRecords(getListOfMyPropertiesForInvestorsPVM);
        }

        public ResponseApiCaller AddToMyPropertiesForInvestors(AddToMyPropertiesForInvestorsPVM addToMyPropertiesForInvestorsPVM)
        {
            return GetRecord(addToMyPropertiesForInvestorsPVM);
        }

        public ResponseApiCaller GetMyPropertyWithMyPropertyIdForInvestors(GetMyPropertyWithMyPropertyIdForInvestorsPVM getMyPropertyWithMyPropertyIdForInvestorsPVM)
        {
            return GetRecord(getMyPropertyWithMyPropertyIdForInvestorsPVM);
        }

        public ResponseApiCaller UpdateMyPropertiesForInvestors(UpdateMyPropertiesForInvestorsPVM updateMyPropertiesForInvestorsPVM)
        {
            return GetRecord(updateMyPropertiesForInvestorsPVM);
        }

        public ResponseApiCaller CompleteDeleteMyPropertiesForInvestors(CompleteDeleteMyPropertiesForInvestorsPVM completeDeleteMyPropertiesForInvestorsPVM)
        {
            return GetResult(completeDeleteMyPropertiesForInvestorsPVM);
        }

        #endregion

        #region MyPropertiesLocationForInvestorsManagement

        public ResponseApiCaller UpdateMyPropertyLocationForInvestors(UpdateMyPropertyLocationForInvestorsPVM updateMyPropertylocationForInvestorsPVM)
        {
            return GetResult(updateMyPropertylocationForInvestorsPVM);
        }

        #endregion

        #region MyPropertyFilesManagement

        public ResponseApiCaller GetAllMyPropertyFilesForInvestorsList(GetAllMyPropertyFilesForInvestorsListPVM getAllMyPropertyFilesForInvestorsListPVM)
        {
            return GetRecords(getAllMyPropertyFilesForInvestorsListPVM);
        }

        public ResponseApiCaller GetListOfMyPropertyFilesForInvestors(GetListOfMyPropertyFilesForInvestorsPVM getListOfMyPropertyFilesForInvestorsPVM)
        {
            return GetRecords(getListOfMyPropertyFilesForInvestorsPVM);
        }
        public ResponseApiCaller AddToMyPropertyFilesForInvestors(AddToMyPropertyFilesForInvestorsPVM addToMyPropertyFilesForInvestorsPVM)
        {
            return GetRecord(addToMyPropertyFilesForInvestorsPVM);
        }

        public ResponseApiCaller UpdateMyPropertyFilesForInvestors(UpdateMyPropertyFilesForInvestorsPVM updateMyPropertyFilesForInvestorsPVM)
        {
            return GetRecord(updateMyPropertyFilesForInvestorsPVM);
        }

        public ResponseApiCaller CompleteDeleteMyPropertyFilesForInvestors(CompleteDeleteMyPropertyFilesForInvestorsPVM completeDeleteMyPropertyFilesForInvestorsPVM)
        {
            return GetResult(completeDeleteMyPropertyFilesForInvestorsPVM);
        }

        #endregion

        #region MyPropertiesFeaturesForInvestorsManagement

        public ResponseApiCaller GetMyPropertyFeaturesValuesForInvestors(GetMyPropertyFeaturesValuesForInvestorsPVM getMyPropertyFeaturesValuesForInvestorsPVM)
        {
            return GetRecord(getMyPropertyFeaturesValuesForInvestorsPVM);
        }

        public ResponseApiCaller UpdateMyPropertyFeaturesForInvestors(UpdateMyPropertyFeaturesForInvestorsPVM updateMyPropertyFeaturesForInvestorsPVM)
        {
            return GetResult(updateMyPropertyFeaturesForInvestorsPVM);
        }

        #endregion

        #region MyPropertiesPricesHistoriesForInvestorsManagement

        public ResponseApiCaller GetListOfMyPropertiesPricesHistoriesForInvestors(GetListOfMyPropertiesPricesHistoriesForInvestorsPVM getListOfMyPropertiesPricesHistoriesForInvestorsPVM)
        {
            return GetRecords(getListOfMyPropertiesPricesHistoriesForInvestorsPVM);
        }
        #endregion

        #endregion

        #endregion

        #region MelkavanProperties
        public ResponseApiCaller GetListOfMelkavanProperties(GetListOfMelkavanPropertiesPVM getListOfMelkavanPropertiesPVM)
        {
            return GetRecords(getListOfMelkavanPropertiesPVM);
        }

       
        public ResponseApiCaller GetListOfNearAdvertisementsWithPropertyId(GetListOfNearAdvertisementsWithPropertyIdPVM getListOfNearAdvertisementsWithPropertyIdPVM)
        {
            return GetRecords(getListOfNearAdvertisementsWithPropertyIdPVM);
        }


        #endregion

        #region ProjectsManagement
        #endregion

        #region ProjectStatesManagement
        #endregion

        #region ProjectTypesManagement
        #endregion

        #region PropertiesManagement
        public ResponseApiCaller GetAllPropertiesList(GetAllPropertiesListPVM getAllPropertiesListPVM)
        {
            return GetRecords(getAllPropertiesListPVM);
        }

        public ResponseApiCaller GetListOfProperties(GetListOfPropertiesPVM getListOfPropertiesPVM)
        {
            return GetRecords(getListOfPropertiesPVM);
        }

        public ResponseApiCaller GetListOfPropertiesAdvanceSearch(GetListOfPropertiesAdvanceSearchPVM getListOfPropertiesAdvanceSearchPVM)
        {
            return GetRecords(getListOfPropertiesAdvanceSearchPVM);
        }

        public ResponseApiCaller AddToProperties(AddToPropertiesPVM addToPropertiesPVM)
        {
            return GetRecord(addToPropertiesPVM);
        }

        public ResponseApiCaller GetPropertyWithPropertyId(GetPropertyWithPropertyIdPVM getPropertyWithPropertyIdPVM)
        {
            return GetRecord(getPropertyWithPropertyIdPVM);
        }

        public ResponseApiCaller GetPropertyDetailsWithPropertyId(GetPropertyWithPropertyIdPVM getPropertyWithPropertyIdPVM)
        {
            return GetRecord(getPropertyWithPropertyIdPVM);
        }

        public ResponseApiCaller UpdateProperties(UpdatePropertiesPVM updatePropertiesPVM)
        {
            return GetRecord(updatePropertiesPVM);
        }

        public ResponseApiCaller ToggleActivationProperties(ToggleActivationPropertiesPVM toggleActivationPropertiesPVM)
        {
            return GetResult(toggleActivationPropertiesPVM);
        }

        public ResponseApiCaller TemporaryDeleteProperties(TemporaryDeletePropertiesPVM temporaryDeletePropertiesPVM)
        {
            return GetResult(temporaryDeletePropertiesPVM);
        }

        public ResponseApiCaller TemporaryDeletePropertiesWithChild(TemporaryDeletePropertiesWithChildPVM temporaryDeletePropertiesWithChildPVM)
        {
            return GetResult(temporaryDeletePropertiesWithChildPVM);
        }
        public ResponseApiCaller CompleteDeleteProperties(CompleteDeletePropertiesPVM completeDeletePropertiesPVM)
        {
            return GetResult(completeDeletePropertiesPVM);
        }

        public ResponseApiCaller GetAllFeaturesValuesCompare(GetFeaturesValuesComparePVM getFeaturesValuesComparePVM)
        {
            return GetRecords(getFeaturesValuesComparePVM);
        }

        public ResponseApiCaller GetPropertiesCompareBasicInfo(GetPropertiesCompareBasicInfoPVM getPropertiesCompareBasicInfoPVM)
        {
            return GetRecord(getPropertiesCompareBasicInfoPVM);
        }

        public ResponseApiCaller GetAllPropertiesCompareTopic(GetAllPropertiesCompareTopicPVM getAllPropertiesCompareTopicPVM)
        {
            return GetRecords(getAllPropertiesCompareTopicPVM);
        }

        public ResponseApiCaller GetAllPropertiesListWithoutAddress(GetAllPropertiesListWithoutAddressPVM getAllPropertiesListWithoutAddressPVM)
        {
            return GetRecords(getAllPropertiesListWithoutAddressPVM);
        }

        public ResponseApiCaller ToggleActivationShowInMelkavan(ToggleActivationShowInMelkavanPVM toggleActivationShowInMelkavanPVM)
        {
            return GetResult(toggleActivationShowInMelkavanPVM);
        }


        public ResponseApiCaller AddPropertiesInMelkavan(AddPropertiesInMelkavanPVM addPropertiesInMelkavanPVM)
        {
            return GetRecord(addPropertiesInMelkavanPVM);

        }

        #endregion

        #region PropertiesLocationManagement

        public ResponseApiCaller UpdatePropertyLocation(UpdatePropertyLocationPVM updatePropertylocationPVM)
        {
            return GetResult(updatePropertylocationPVM);
        }

        #endregion

        #region PropertyAddressManagement
        #endregion

        #region PropertyFilesManagement

        public ResponseApiCaller GetAllPropertyFilesList(GetAllPropertyFilesListPVM getAllPropertyFilesListPVM)
        {
            return GetRecords(getAllPropertyFilesListPVM);
        }

        public ResponseApiCaller GetListOfPropertyFiles(GetListOfPropertyFilesPVM getListOfPropertyFilesPVM)
        {
            return GetRecords(getListOfPropertyFilesPVM);
        }

        public ResponseApiCaller AddToPropertyFiles(AddToPropertyFilesPVM addToPropertyFilesPVM)
        {
            return GetRecord(addToPropertyFilesPVM);
        }

        public ResponseApiCaller GetPropertyFileWithPropertyFileId(GetPropertyFileWithPropertyFileIdPVM getPropertyFileWithPropertyFileIdPVM)
        {
            return GetRecord(getPropertyFileWithPropertyFileIdPVM);
        }

        public ResponseApiCaller UpdatePropertyFiles(UpdatePropertyFilesPVM updatePropertyFilesPVM)
        {
            return GetRecord(updatePropertyFilesPVM);
        }

        public ResponseApiCaller ToggleActivationPropertyFiles(ToggleActivationPropertyFilesPVM toggleActivationPropertyFilesPVM)
        {
            return GetResult(toggleActivationPropertyFilesPVM);
        }

        public ResponseApiCaller TemporaryDeletePropertyFiles(TemporaryDeletePropertyFilesPVM temporaryDeletePropertyFilesPVM)
        {
            return GetResult(temporaryDeletePropertyFilesPVM);
        }

        public ResponseApiCaller CompleteDeletePropertyFiles(CompleteDeletePropertyFilesPVM completeDeletePropertyFilesPVM)
        {
            return GetResult(completeDeletePropertyFilesPVM);
        }

        #endregion

        #region PropertiesFeaturesManagement

        public ResponseApiCaller GetPropertyFeaturesValues(GetPropertyFeaturesValuesPVM getPropertyFeaturesValuesPVM)
        {
            return GetRecord(getPropertyFeaturesValuesPVM);
        }

        public ResponseApiCaller UpdatePropertyFeatures(UpdatePropertyFeaturesPVM updatePropertyFeaturesPVM)
        {
            return GetResult(updatePropertyFeaturesPVM);
        }

        #endregion

        #region PropertyStatesManagement
        #endregion

        #region PropertyTypesManagement

        public ResponseApiCaller GetAllPropertyTypesList(GetAllPropertyTypesListPVM getAllPropertyTypesListPVM)
        {
            return GetRecords(getAllPropertyTypesListPVM);
        }

        #endregion

        #region PropertiesPricesHistoriesManagement

        public ResponseApiCaller GetListOfPropertiesPricesHistories(GetListOfPropertiesPricesHistoriesPVM getListOfPropertiesPricesHistoriesPVM)
        {
            return GetRecords(getListOfPropertiesPricesHistoriesPVM);
        }

        public ResponseApiCaller GetListOfPropertiesPricesForMap(GetListOfPropertiesPricesForMapPVM getListOfPropertiesPricesForMapPVM)
        {
            return GetRecords(getListOfPropertiesPricesForMapPVM);
        }

        #endregion

        #region ProposedProjectsManagement
        #endregion

        #region PositionManagement

        public ResponseApiCaller GetAllPositionsList(GetAllPositionsListPVM getAllPositionsListPVM)
        {
            return GetRecords(getAllPositionsListPVM);
        }

        #endregion

        #region TypeOfUsesManagement

        public ResponseApiCaller GetAllTypeOfUsesList(GetAllTypeOfUsesListPVM getAllTypeOfUsesListPVM)
        {
            return GetRecords(getAllTypeOfUsesListPVM);
        }

        #endregion

        #region OutterDashboard

        public ResponseApiCaller GetOutterDashboardPricesBlock(GetOutterDashboardPricesBlockPVM getOutterDashboardPricesBlockPVM)
        {
            return GetRecord(getOutterDashboardPricesBlockPVM);
        }

        public ResponseApiCaller GetListOfMyFundsDispersion(GetListOfMyFundsDispersionPVM getListOfMyFundsDispersionPVM)
        {
            return GetRecords(getListOfMyFundsDispersionPVM);
        }

        public ResponseApiCaller GetListOfMyFundsGrowth(GetListOfMyFundsGrowthPVM getListOfMyFundsGrowthPVM)
        {
            return GetRecords(getListOfMyFundsGrowthPVM);
        }

        public ResponseApiCaller GetDetailsForOuterDashboard(GetDetailsForOuterDashboardPVM getDetailsForOuterDashboardPVM)
        {
            return GetRecord(getDetailsForOuterDashboardPVM);
        }

        public ResponseApiCaller GetUnreadConversationsAndUnverifiedFilesCount(GetUnreadConversationsAndUnverifiedFilesCountPVM getUnreadConversationsAndUnverifiedFilesCountPVM)
        {
            return GetRecord(getUnreadConversationsAndUnverifiedFilesCountPVM);
        } 

        #endregion



    }
}
