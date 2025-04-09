using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using ApiCallers.BaseApiCaller;
using VM.PVM.Projects;
using VM.Base;
using VM.PVM.Base;
using PVM.Projects.TeniacoSuggestions;
using VM.Projects.TeniacoSuggestions;

namespace ApiCallers.ProjectsApiCaller
{
    public class ProjectsApiCaller : BaseCaller, IProjectsApiCaller
    {
        public ProjectsApiCaller() : base()
        {
        }

        public ProjectsApiCaller(string _serviceUrl) : base(_serviceUrl)
        {
            serviceUrl = _serviceUrl;
        }

        public ProjectsApiCaller(string _serviceUrl,
            string _accessToken) :
            base(_serviceUrl,
                _accessToken)
        {
            serviceUrl = _serviceUrl;
        }

        #region AttachementFilesManagement
        public ResponseApiCaller GetAllAttachementFilesList(GetAllAttachementFilesListPVM getAllAttachementFilesListPVM)
        {
            return GetRecords(getAllAttachementFilesListPVM);
        }
        public ResponseApiCaller GetListOfAttachementFiles(GetListOfAttachementFilesPVM getListOfAttachementFilesPVM)
        {
            return GetRecords(getListOfAttachementFilesPVM);
        }
        public ResponseApiCaller AddToAttachementFiles(AddToAttachementFilesPVM addToAttachementFilesPVM)
        {
            return GetRecord(addToAttachementFilesPVM);
        }
        public ResponseApiCaller UpdateAttachementFiles(UpdateAttachementFilesPVM updateAttachementFilesPVM)
        {
            return GetRecord(updateAttachementFilesPVM);
        }
        public ResponseApiCaller ToggleActivationAttachementFiles(ToggleActivationAttachementFilesPVM toggleActivationAttachementFilesPVM)
        {
            return GetResult(toggleActivationAttachementFilesPVM);
        }
        public ResponseApiCaller TemporaryDeleteAttachementFiles(TemporaryDeleteAttachementFilesPVM temporaryDeleteAttachementFilesPVM)
        {
            return GetResult(temporaryDeleteAttachementFilesPVM);
        }
        public ResponseApiCaller CompleteDeleteAttachementFiles(CompleteDeleteAttachementFilesPVM completeDeleteAttachementFilesPVM)
        {
            return GetResult(completeDeleteAttachementFilesPVM);
        }
        public ResponseApiCaller GetAttachementFilesWithAttachementFileId(GetAttachementFilesWithAttachementFileIdPVM getAttachementFilesWithAttachementFileIdPVM)
        {
            return GetRecord(getAttachementFilesWithAttachementFileIdPVM);
        }

        public ResponseApiCaller GetAttachementsByAgreementIdAndTableTitle(GetAttachementsByAgreementIdAndTableTitlePVM getAttachementsByAgreementIdAndTableTitlePVM)
        {
            return GetRecords(getAttachementsByAgreementIdAndTableTitlePVM);
        }

        #endregion

        #region ConstructionProjectsManagement
        public ResponseApiCaller GetAllConstructionProjectsList(GetAllConstructionProjectsListPVM getAllConstructionProjectsListPVM)
        {
            return GetRecords(getAllConstructionProjectsListPVM);
        }
        public ResponseApiCaller GetListOfConstructionProjects(GetListOfConstructionProjectsPVM getListOfConstructionProjectsPVM)
        {
            try
            {
                inputJson = JsonConvert.SerializeObject(getListOfConstructionProjectsPVM);
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
        public ResponseApiCaller AddToConstructionProjects(AddToConstructionProjectsPVM addToConstructionProjectsPVM)
        {
            return GetRecord(addToConstructionProjectsPVM);
        }
        public ResponseApiCaller UpdateConstructionProjects(UpdateConstructionProjectsPVM updateConstructionProjectsPVM)
        {
            return GetRecord(updateConstructionProjectsPVM);
        }
        public ResponseApiCaller ToggleActivationConstructionProjects(ToggleActivationConstructionProjectsPVM toggleActivationConstructionProjectsPVM)
        {
            return GetResult(toggleActivationConstructionProjectsPVM);
        }
        public ResponseApiCaller ToggleShowInDashboardConstructionProjects(ToggleShowInDashboardConstructionProjectsPVM toggleShowInDashboardConstructionProjectsPVM)
        {
            return GetRecord(toggleShowInDashboardConstructionProjectsPVM);
        }
        public ResponseApiCaller TemporaryDeleteConstructionProjects(TemporaryDeleteConstructionProjectsPVM temporaryDeleteConstructionProjectsPVM)
        {
            return GetResult(temporaryDeleteConstructionProjectsPVM);
        }
        public ResponseApiCaller CompleteDeleteConstructionProjects(CompleteDeleteConstructionProjectsPVM completeDeleteConstructionProjectsPVM)
        {
            return GetResult(completeDeleteConstructionProjectsPVM);
        }
        public ResponseApiCaller GetConstructionProjectWithConstructionProjectId(GetConstructionProjectWithConstructionProjectIdPVM getConstructionProjectWithConstructionProjectIdPVM)
        {
            return GetRecord(getConstructionProjectWithConstructionProjectIdPVM);
        }
        public ResponseApiCaller GetListOfConstructionProjectsWithUserId(GetConstructionProjectWithUserIdPVM getConstructionProjectWithUserIdPVM)
        {
            return GetRecords(getConstructionProjectWithUserIdPVM);
        }
        public ResponseApiCaller GetConstructionProjectsDailyDataByConstructionProjectId(GetConstructionProjectsDailyDataByProjectIdPVM getConstructionProjectsDailyDataByProjectIdPVM)
        {
            return GetRecords(getConstructionProjectsDailyDataByProjectIdPVM);
        }
        public ResponseApiCaller GetAgreementDataByAgreementTypeAndConstructionProjectId(GetAgreementDataByAgreementTypeAndConstructionProjectIdPVM getAgreementDataByAgreementTypeAndConstructionProjectIdPVM)
        {
            return GetRecords(getAgreementDataByAgreementTypeAndConstructionProjectIdPVM);
        }
        public ResponseApiCaller GetListOfConstructionProjectsWithRepresentativeId(GetConstructionProjectWithUserIdPVM getConstructionProjectWithUserIdPVM)
        {
            return GetRecords(getConstructionProjectWithUserIdPVM);
        }

        #endregion

        #region ConstructionProjectTypesManagement

        public ResponseApiCaller GetAllConstructionProjectTypesList(GetAllConstructionProjectTypesListPVM getAllConstructionProjectTypesListPVM)
        {
            return GetRecords(getAllConstructionProjectTypesListPVM);
        }
        #endregion

        #region ContractAgreementsManagement
        public ResponseApiCaller GetAllContractAgreementsList(GetAllContractAgreementsListPVM getAllContractAgreementsListPVM)
        {
            return GetRecords(getAllContractAgreementsListPVM);
        }
        public ResponseApiCaller GetListOfContractAgreements(GetListOfContractAgreementsPVM getListOfContractAgreementsPVM)
        {
            return GetRecords(getListOfContractAgreementsPVM);
        }
        public ResponseApiCaller AddToContractAgreements(AddToContractAgreementsPVM addToContractAgreementsPVM)
        {
            return GetRecord(addToContractAgreementsPVM);
        }
        public ResponseApiCaller UpdateContractAgreements(UpdateContractAgreementsPVM updateContractAgreementsPVM)
        {
            return GetRecord(updateContractAgreementsPVM);
        }
        public ResponseApiCaller ToggleActivationContractAgreements(ToggleActivationContractAgreementsPVM toggleActivationContractAgreementsPVM)
        {
            return GetResult(toggleActivationContractAgreementsPVM);
        }
        public ResponseApiCaller TemporaryDeleteContractAgreements(TemporaryDeleteContractAgreementsPVM temporaryDeleteContractAgreementsPVM)
        {
            return GetResult(temporaryDeleteContractAgreementsPVM);
        }
        public ResponseApiCaller CompleteDeleteContractAgreements(CompleteDeleteContractAgreementsPVM completeDeleteContractAgreementsPVM)
        {
            return GetResult(completeDeleteContractAgreementsPVM);
        }
        public ResponseApiCaller GetContractAgreementsWithContractAgreementId(GetContractAgreementsWithContractAgreementIdPVM getContractAgreementsWithContractAgreementIdPVM)
        {
            return GetRecord(getContractAgreementsWithContractAgreementIdPVM);
        }

        #endregion

        #region ContractorsAgreementsManagement
        public ResponseApiCaller GetAllContractorsAgreementsList(GetAllContractorsAgreementsListPVM getAllContractorsAgreementsListPVM)
        {
            return GetRecords(getAllContractorsAgreementsListPVM);
        }
        public ResponseApiCaller GetListOfContractorsAgreements(GetListOfContractorsAgreementsPVM getListOfContractorsAgreementsPVM)
        {
            return GetRecords(getListOfContractorsAgreementsPVM);
        }
        public ResponseApiCaller AddToContractorsAgreements(AddToContractorsAgreementsPVM addToContractorsAgreementsPVM)
        {
            return GetRecord(addToContractorsAgreementsPVM);
        }
        public ResponseApiCaller UpdateContractorsAgreements(UpdateContractorsAgreementsPVM updateContractorsAgreementsPVM)
        {
            return GetRecord(updateContractorsAgreementsPVM);
        }
        public ResponseApiCaller ToggleActivationContractorsAgreements(ToggleActivationContractorsAgreementsPVM toggleActivationContractorsAgreementsPVM)
        {
            return GetResult(toggleActivationContractorsAgreementsPVM);
        }
        public ResponseApiCaller TemporaryDeleteContractorsAgreements(TemporaryDeleteContractorsAgreementsPVM temporaryDeleteContractorsAgreementsPVM)
        {
            return GetResult(temporaryDeleteContractorsAgreementsPVM);
        }
        public ResponseApiCaller CompleteDeleteContractorsAgreements(CompleteDeleteContractorsAgreementsPVM completeDeleteContractorsAgreementsPVM)
        {
            return GetResult(completeDeleteContractorsAgreementsPVM);
        }
        public ResponseApiCaller GetContractorsAgreementsWithContractorsAgreementId(GetContractorsAgreementsWithContractorsAgreementIdPVM getContractorsAgreementsWithContractorsAgreementIdPVM)
        {
            return GetRecord(getContractorsAgreementsWithContractorsAgreementIdPVM);
        }

        #endregion

        #region ConfirmationTypesManagement
        public ResponseApiCaller GetAllConfirmationTypesList(GetAllConfirmationTypesListPVM getAllConfirmationTypesListPVM)
        {
            return GetRecords(getAllConfirmationTypesListPVM);
        }

        #endregion

        #region ConfirmationAgreementsManagement
        public ResponseApiCaller GetAllConfirmationAgreementsList(GetAllConfirmationAgreementsListPVM getAllConfirmationAgreementsListPVM)
        {
            return GetRecords(getAllConfirmationAgreementsListPVM);
        }
        public ResponseApiCaller GetListOfConfirmationAgreements(GetListOfConfirmationAgreementsPVM getListOfConfirmationAgreementsPVM)
        {
            return GetRecords(getListOfConfirmationAgreementsPVM);
        }
        public ResponseApiCaller AddToConfirmationAgreements(AddToConfirmationAgreementsPVM addToConfirmationAgreementsPVM)
        {
            return GetRecord(addToConfirmationAgreementsPVM);
        }
        public ResponseApiCaller UpdateConfirmationAgreements(UpdateConfirmationAgreementsPVM updateConfirmationAgreementsPVM)
        {
            return GetRecord(updateConfirmationAgreementsPVM);
        }
        public ResponseApiCaller ToggleActivationConfirmationAgreements(ToggleActivationConfirmationAgreementsPVM toggleActivationConfirmationAgreementsPVM)
        {
            return GetResult(toggleActivationConfirmationAgreementsPVM);
        }
        public ResponseApiCaller TemporaryDeleteConfirmationAgreements(TemporaryDeleteConfirmationAgreementsPVM temporaryDeleteConfirmationAgreementsPVM)
        {
            return GetResult(temporaryDeleteConfirmationAgreementsPVM);
        }
        public ResponseApiCaller CompleteDeleteConfirmationAgreements(CompleteDeleteConfirmationAgreementsPVM completeDeleteConfirmationAgreementsPVM)
        {
            return GetResult(completeDeleteConfirmationAgreementsPVM);
        }
        public ResponseApiCaller GetConfirmationAgreementsWithConfirmationAgreementId(GetConfirmationAgreementsWithConfirmationAgreementIdPVM getConfirmationAgreementsWithConfirmationAgreementIdPVM)
        {
            return GetRecord(getConfirmationAgreementsWithConfirmationAgreementIdPVM);
        }
        //public ResponseApiCaller ConfirmConfirmationAgreements(ConfirmConfirmationAgreementsPVM confirmConfirmationAgreementsPVM)
        //{
        //    return GetResult(confirmConfirmationAgreementsPVM);
        //}
        #endregion

        #region ContractSidesManagement
        public ResponseApiCaller GetAllContractSidesList(GetAllContractSidesListPVM getAllContractSidesListPVM)
        {
            return GetRecords(getAllContractSidesListPVM);
        }
        public ResponseApiCaller GetListOfContractSides(GetListOfContractSidesPVM getListOfContractSidesPVM)
        {
            return GetRecords(getListOfContractSidesPVM);
        }
        public ResponseApiCaller AddToContractSides(AddToContractSidesPVM addToContractSidesPVM)
        {
            return GetRecord(addToContractSidesPVM);
        }
        public ResponseApiCaller UpdateContractSides(UpdateContractSidesPVM updateContractSidesPVM)
        {
            return GetRecord(updateContractSidesPVM);
        }
        public ResponseApiCaller ToggleActivationContractSides(ToggleActivationContractSidesPVM toggleActivationContractSidesPVM)
        {
            return GetResult(toggleActivationContractSidesPVM);
        }
        public ResponseApiCaller TemporaryDeleteContractSides(TemporaryDeleteContractSidesPVM temporaryDeleteContractSidesPVM)
        {
            return GetResult(temporaryDeleteContractSidesPVM);
        }
        public ResponseApiCaller CompleteDeleteContractSides(CompleteDeleteContractSidesPVM completeDeleteContractSidesPVM)
        {
            return GetResult(completeDeleteContractSidesPVM);
        }
        public ResponseApiCaller GetContractSidesWithContractSideId(GetContractSidesWithContractSideIdPVM getContractSidesWithContractSideIdPVM)
        {
            return GetRecord(getContractSidesWithContractSideIdPVM);
        }

        #endregion

        #region ConstructionProjectBillDelaysManagement

        public ResponseApiCaller GetAllConstructionProjectBillDelaysList(GetAllConstructionProjectBillDelaysListPVM getAllConstructionProjectBillDelaysListPVM)
        {
            return GetRecords(getAllConstructionProjectBillDelaysListPVM);
        }

        public ResponseApiCaller AddToConstructionProjectBillDelays(AddToConstructionProjectBillDelaysPVM addToConstructionProjectBillDelaysPVM)
        {
            return GetRecord(addToConstructionProjectBillDelaysPVM);
        }


        public ResponseApiCaller UpdateConstructionProjectBillDelays(UpdateConstructionProjectBillDelaysPVM updateConstructionProjectBillDelaysPVM)
        {
            return GetRecord(updateConstructionProjectBillDelaysPVM);
        }

        public ResponseApiCaller GetListOfBillDelaysForOuterDashboard(GetListOfBillDelaysForOuterDashboardPVM getListOfBillDelaysForOuterDashboardPVM)
        {
            return GetRecords(getListOfBillDelaysForOuterDashboardPVM);
        }

        #endregion

        #region ConstructionProjectDelaysManagement


        public ResponseApiCaller GetAllConstructionProjectDelays(GetAllConstructionProjectDelaysListPVM getAllConstructionProjectDelaysListPVM)
        {
            return GetRecords(getAllConstructionProjectDelaysListPVM);
        }


        public ResponseApiCaller GetListOfConstructionProjectDelays(GetListOfConstructionProjectDelaysPVM getListOfConstructionProjectDelays)
        {
            return GetRecords(getListOfConstructionProjectDelays);
        }

        public ResponseApiCaller AddToConstructionProjectDelays(AddToConstructionProjectDelaysPVM addToConstructionProjectDelaysPVM)
        {
            return GetRecord(addToConstructionProjectDelaysPVM);
        }

        public ResponseApiCaller UpdateConstructionProjectDelays(UpdateConstructionProjectDelaysPVM updateConstructionProjectDelaysPVM)
        {
            return GetRecord(updateConstructionProjectDelaysPVM);
        }



        public ResponseApiCaller ToggleActivationConstructionProjectDelays(ToggleActivationConstructionProjectDelaysPVM toggleActivationConstructionProjectDelaysPVM)
        {
            return GetResult(toggleActivationConstructionProjectDelaysPVM);
        }
        public ResponseApiCaller TemporaryDeleteConstructionProjectDelays(TemporaryDeleteConstructionProjectDelaysPVM temporaryDeleteConstructionProjectDelaysPVM)
        {
            return GetResult(temporaryDeleteConstructionProjectDelaysPVM);
        }
        public ResponseApiCaller CompleteDeleteConstructionProjectDelays(CompleteDeleteConstructionProjectDelaysPVM completeDeleteConstructionProjectsPVM)
        {
            return GetResult(completeDeleteConstructionProjectsPVM);
        }

        public ResponseApiCaller GetConstructionProjectDelayById(GetConstructionProjectDelayByIdPVM getConstructionProjectDelayByIdPVM)
        {
            return GetRecord(getConstructionProjectDelayByIdPVM);
        }

        #endregion

        #region ConstructionProjectDailyPicturesManagement

        public ResponseApiCaller GetListOfConstructionProjectDailyPicturesWithLastDate(
            GetListOfConstructionProjectDailyPicturesWithLastDatePVM getListOfConstructionProjectDailyPicturesWithLastDatePVM)
        {
            return GetRecords(getListOfConstructionProjectDailyPicturesWithLastDatePVM);
        }

        #endregion

        #region ConstructionProjectFinancialData

        public ResponseApiCaller GetConstructionProjectFinancialDataByConstructionProjectId(GetConstructionProjectFinancialDataByConstructionProjectIdPVM getConstructionProjectFinancialDataByConstructionProjectIdPVM)
        {
            return GetRecords(getConstructionProjectFinancialDataByConstructionProjectIdPVM);
        }

        public ResponseApiCaller GetPaymentsAndCostsList(GetPaymentsAndCostsListPVM getPaymentsAndCostsListPVM)
        {
            return GetRecords(getPaymentsAndCostsListPVM);
        }

        public ResponseApiCaller GetSumOfPrivateCostsList(GetSumOfPrivateCostsListPVM getSumOfPrivateCostsListPVM)
        {
            return GetRecords(getSumOfPrivateCostsListPVM);
        }

        public ResponseApiCaller GetSumOfPrivateCostsListForRepresantative(GetSumOfPrivateCostsListPVM getSumOfPrivateCostsListPVM)
        {
            return GetRecords(getSumOfPrivateCostsListPVM);
        }

        public ResponseApiCaller GetSumOfPublicCostsList(GetSumOfPublicCostsListPVM getSumOfPublicCostsListPVM)
        {
            return GetRecords(getSumOfPublicCostsListPVM);
        }


        public ResponseApiCaller GetSumOfPublicCostsListForRepresentative(GetSumOfPublicCostsForRepresentativeListPVM getSumOfPublicCostsListPVM)
        {
            return GetRecords(getSumOfPublicCostsListPVM);
        }

        #endregion

        #region ExcelSheetConfigsManagement
        public ResponseApiCaller GetAllExcelSheetConfigsList(GetAllExcelSheetConfigsListPVM getAllExcelSheetConfigsListPVM)
        {
            return GetRecords(getAllExcelSheetConfigsListPVM);
        }
        public ResponseApiCaller GetListOfExcelSheetConfigs(GetListOfExcelSheetConfigsPVM getListOfExcelSheetConfigsPVM)
        {
            return GetRecords(getListOfExcelSheetConfigsPVM);
        }
        public ResponseApiCaller AddToExcelSheetConfigs(AddToExcelSheetConfigsPVM addToExcelSheetConfigsPVM)
        {
            return GetRecord(addToExcelSheetConfigsPVM);
        }
        public ResponseApiCaller UpdateExcelSheetConfigs(UpdateExcelSheetConfigsPVM updateExcelSheetConfigsPVM)
        {
            return GetRecord(updateExcelSheetConfigsPVM);
        }
        public ResponseApiCaller ToggleActivationExcelSheetConfigs(ToggleActivationExcelSheetConfigsPVM toggleActivationExcelSheetConfigsPVM)
        {
            return GetResult(toggleActivationExcelSheetConfigsPVM);
        }
        public ResponseApiCaller TemporaryDeleteExcelSheetConfigs(TemporaryDeleteExcelSheetConfigsPVM temporaryDeleteExcelSheetConfigsPVM)
        {
            return GetResult(temporaryDeleteExcelSheetConfigsPVM);
        }
        public ResponseApiCaller CompleteDeleteExcelSheetConfigs(CompleteDeleteExcelSheetConfigsPVM completeDeleteExcelSheetConfigsPVM)
        {
            return GetResult(completeDeleteExcelSheetConfigsPVM);
        }
        public ResponseApiCaller GetExcelSheetConfigsWithExcelSheetConfigId(GetExcelSheetConfigsWithExcelSheetConfigIdPVM getExcelSheetConfigsWithExcelSheetConfigIdPVM)
        {
            return GetRecord(getExcelSheetConfigsWithExcelSheetConfigIdPVM);
        }

        #endregion

        #region ExcelSheetConfigHistoriesManagement
        public ResponseApiCaller GetAllExcelSheetConfigHistoriesList(GetAllExcelSheetConfigHistoriesListPVM getAllExcelSheetConfigHistoriesListPVM)
        {
            return GetRecords(getAllExcelSheetConfigHistoriesListPVM);
        }
        public ResponseApiCaller GetListOfExcelSheetConfigHistories(GetListOfExcelSheetConfigHistoriesPVM getListOfExcelSheetConfigHistoriesPVM)
        {
            return GetRecords(getListOfExcelSheetConfigHistoriesPVM);
        }
        public ResponseApiCaller AddToExcelSheetConfigHistories(AddToExcelSheetConfigHistoriesPVM addToExcelSheetConfigHistoriesPVM)
        {
            return GetRecord(addToExcelSheetConfigHistoriesPVM);
        }
        public ResponseApiCaller UpdateExcelSheetConfigHistories(UpdateExcelSheetConfigHistoriesPVM updateExcelSheetConfigHistoriesPVM)
        {
            return GetRecord(updateExcelSheetConfigHistoriesPVM);
        }
        public ResponseApiCaller ToggleActivationExcelSheetConfigHistories(ToggleActivationExcelSheetConfigHistoriesPVM toggleActivationExcelSheetConfigHistoriesPVM)
        {
            return GetResult(toggleActivationExcelSheetConfigHistoriesPVM);
        }
        public ResponseApiCaller TemporaryDeleteExcelSheetConfigHistories(TemporaryDeleteExcelSheetConfigHistoriesPVM temporaryDeleteExcelSheetConfigHistoriesPVM)
        {
            return GetResult(temporaryDeleteExcelSheetConfigHistoriesPVM);
        }
        public ResponseApiCaller CompleteDeleteExcelSheetConfigHistories(CompleteDeleteExcelSheetConfigHistoriesPVM completeDeleteExcelSheetConfigHistoriesPVM)
        {
            return GetResult(completeDeleteExcelSheetConfigHistoriesPVM);
        }
        public ResponseApiCaller GetExcelSheetConfigHistoriesWithExcelSheetConfigHistoryId(GetExcelSheetConfigHistoriesWithExcelSheetConfigHistoryIdPVM getExcelSheetConfigHistoriesWithExcelSheetConfigHistoryId)
        {
            return GetRecord(getExcelSheetConfigHistoriesWithExcelSheetConfigHistoryId);
        }

        #endregion

        #region GoogleSheetConfigsManagement
        public ResponseApiCaller GetAllGoogleSheetConfigsList(GetAllGoogleSheetConfigsListPVM getAllGoogleSheetConfigsListPVM)
        {
            return GetRecords(getAllGoogleSheetConfigsListPVM);
        }
        public ResponseApiCaller GetListOfGoogleSheetConfigs(GetListOfGoogleSheetConfigsPVM getListOfGoogleSheetConfigsPVM)
        {
            return GetRecords(getListOfGoogleSheetConfigsPVM);
        }
        public ResponseApiCaller AddToGoogleSheetConfigs(AddToGoogleSheetConfigsPVM addToGoogleSheetConfigsPVM)
        {
            return GetRecord(addToGoogleSheetConfigsPVM);
        }
        public ResponseApiCaller UpdateGoogleSheetConfigs(UpdateGoogleSheetConfigsPVM updateGoogleSheetConfigsPVM)
        {
            return GetRecord(updateGoogleSheetConfigsPVM);
        }
        public ResponseApiCaller ToggleActivationGoogleSheetConfigs(ToggleActivationGoogleSheetConfigsPVM toggleActivationGoogleSheetConfigsPVM)
        {
            return GetResult(toggleActivationGoogleSheetConfigsPVM);
        }
        public ResponseApiCaller TemporaryDeleteGoogleSheetConfigs(TemporaryDeleteGoogleSheetConfigsPVM temporaryDeleteGoogleSheetConfigsPVM)
        {
            return GetResult(temporaryDeleteGoogleSheetConfigsPVM);
        }
        public ResponseApiCaller CompleteDeleteGoogleSheetConfigs(CompleteDeleteGoogleSheetConfigsPVM completeDeleteGoogleSheetConfigsPVM)
        {
            return GetResult(completeDeleteGoogleSheetConfigsPVM);
        }
        public ResponseApiCaller GetGoogleSheetConfigsWithGoogleSheetConfigId(GetGoogleSheetConfigsWithGoogleSheetConfigIdPVM getGoogleSheetConfigsWithGoogleSheetConfigIdPVM)
        {
            return GetRecord(getGoogleSheetConfigsWithGoogleSheetConfigIdPVM);
        }

        #endregion

        #region InitialPlansManagement
        public ResponseApiCaller GetAllInitialPlansList(GetAllInitialPlansListPVM getAllInitialPlansListPVM)
        {
            return GetRecords(getAllInitialPlansListPVM);
        }
        public ResponseApiCaller GetListOfInitialPlans(GetListOfInitialPlansPVM getListOfInitialPlansPVM)
        {
            return GetRecords(getListOfInitialPlansPVM);
        }
        public ResponseApiCaller AddToInitialPlans(AddToInitialPlansPVM addToInitialPlansPVM)
        {
            return GetRecord(addToInitialPlansPVM);
        }
        public ResponseApiCaller UpdateInitialPlans(UpdateInitialPlansPVM updateInitialPlansPVM)
        {
            return GetRecord(updateInitialPlansPVM);
        }
        public ResponseApiCaller ToggleActivationInitialPlans(ToggleActivationInitialPlansPVM toggleActivationInitialPlansPVM)
        {
            return GetResult(toggleActivationInitialPlansPVM);
        }
        public ResponseApiCaller TemporaryDeleteInitialPlans(TemporaryDeleteInitialPlansPVM temporaryDeleteInitialPlansPVM)
        {
            return GetResult(temporaryDeleteInitialPlansPVM);
        }
        public ResponseApiCaller CompleteDeleteInitialPlans(CompleteDeleteInitialPlansPVM completeDeleteInitialPlansPVM)
        {
            return GetResult(completeDeleteInitialPlansPVM);
        }
        public ResponseApiCaller GetInitialPlansWithInitialPlanId(GetInitialPlansWithInitialPlanIdPVM getInitialPlansWithInitialPlanIdPVM)
        {
            return GetRecord(getInitialPlansWithInitialPlanIdPVM);
        }

        #endregion

        #region MeetingBoardsManagement
        public ResponseApiCaller GetAllMeetingBoardsList(GetAllMeetingBoardsListPVM getAllMeetingBoardsListPVM)
        {
            return GetRecords(getAllMeetingBoardsListPVM);
        }
        public ResponseApiCaller GetListOfMeetingBoards(GetListOfMeetingBoardsPVM getListOfMeetingBoardsPVM)
        {
            return GetRecords(getListOfMeetingBoardsPVM);
        }
        public ResponseApiCaller AddToMeetingBoards(AddToMeetingBoardsPVM addToMeetingBoardsPVM)
        {
            return GetRecord(addToMeetingBoardsPVM);
        }
        public ResponseApiCaller UpdateMeetingBoards(UpdateMeetingBoardsPVM updateMeetingBoardsPVM)
        {
            return GetRecord(updateMeetingBoardsPVM);
        }
        public ResponseApiCaller ToggleActivationMeetingBoards(ToggleActivationMeetingBoardsPVM toggleActivationMeetingBoardsPVM)
        {
            return GetResult(toggleActivationMeetingBoardsPVM);
        }
        public ResponseApiCaller TemporaryDeleteMeetingBoards(TemporaryDeleteMeetingBoardsPVM temporaryDeleteMeetingBoardsPVM)
        {
            return GetResult(temporaryDeleteMeetingBoardsPVM);
        }
        public ResponseApiCaller CompleteDeleteMeetingBoards(CompleteDeleteMeetingBoardsPVM completeDeleteMeetingBoardsPVM)
        {
            return GetResult(completeDeleteMeetingBoardsPVM);
        }
        public ResponseApiCaller GetMeetingBoardsWithMeetingBoardId(GetMeetingBoardsWithMeetingBoardIdPVM getMeetingBoardsWithMeetingBoardIdPVM)
        {
            return GetRecord(getMeetingBoardsWithMeetingBoardIdPVM);
        }

        #endregion

        #region PropertyProjectsManagement

        public ResponseApiCaller GetAllPropertyProjectsList(GetAllPropertyProjectsListPVM getAllPropertyProjectsListPVM)
        {
            return GetRecords(getAllPropertyProjectsListPVM);
        }

        public ResponseApiCaller GetListOfPropertyProjects(GetListOfPropertyProjectsPVM getListOfPropertyProjectsPVM)
        {
            return GetRecords(getListOfPropertyProjectsPVM);
        }

        public ResponseApiCaller AddToPropertyProjects(AddToPropertyProjectsPVM addToPropertyProjectsPVM)
        {
            return GetRecord(addToPropertyProjectsPVM);
        }

        public ResponseApiCaller GetPropertyProjectWithPropertyProjectId(GetPropertyProjectWithPropertyProjectIdPVM getPropertyProjectWithPropertyProjectIdPVM)
        {
            return GetRecord(getPropertyProjectWithPropertyProjectIdPVM);
        }

        public ResponseApiCaller UpdatePropertyProjects(UpdatePropertyProjectsPVM updatePropertyProjectsPVM)
        {
            return GetRecord(updatePropertyProjectsPVM);
        }

        public ResponseApiCaller ToggleActivationPropertyProjects(ToggleActivationPropertyProjectsPVM toggleActivationPropertyProjectsPVM)
        {
            return GetResult(toggleActivationPropertyProjectsPVM);
        }

        public ResponseApiCaller TemporaryDeletePropertyProjects(TemporaryDeletePropertyProjectsPVM temporaryDeletePropertyProjectsPVM)
        {
            return GetResult(temporaryDeletePropertyProjectsPVM);
        }

        public ResponseApiCaller CompleteDeletePropertyProjects(CompleteDeletePropertyProjectsPVM completeDeletePropertyProjectsPVM)
        {
            return GetResult(completeDeletePropertyProjectsPVM);
        }

        #endregion

        #region PartnershipAgreementsManagement
        public ResponseApiCaller GetAllPartnershipAgreementsList(GetAllPartnershipAgreementsListPVM getAllPartnershipAgreementsListPVM)
        {
            return GetRecords(getAllPartnershipAgreementsListPVM);
        }
        public ResponseApiCaller GetListOfPartnershipAgreements(GetListOfPartnershipAgreementsPVM getListOfPartnershipAgreementsPVM)
        {
            return GetRecords(getListOfPartnershipAgreementsPVM);
        }
        public ResponseApiCaller AddToPartnershipAgreements(AddToPartnershipAgreementsPVM addToPartnershipAgreementsPVM)
        {
            return GetRecord(addToPartnershipAgreementsPVM);
        }
        public ResponseApiCaller UpdatePartnershipAgreements(UpdatePartnershipAgreementsPVM updatePartnershipAgreementsPVM)
        {
            return GetRecord(updatePartnershipAgreementsPVM);
        }
        public ResponseApiCaller ToggleActivationPartnershipAgreements(ToggleActivationPartnershipAgreementsPVM toggleActivationPartnershipAgreementsPVM)
        {
            return GetResult(toggleActivationPartnershipAgreementsPVM);
        }
        public ResponseApiCaller TemporaryDeletePartnershipAgreements(TemporaryDeletePartnershipAgreementsPVM temporaryDeletePartnershipAgreementsPVM)
        {
            return GetResult(temporaryDeletePartnershipAgreementsPVM);
        }
        public ResponseApiCaller CompleteDeletePartnershipAgreements(CompleteDeletePartnershipAgreementsPVM completeDeletePartnershipAgreementsPVM)
        {
            return GetResult(completeDeletePartnershipAgreementsPVM);
        }
        public ResponseApiCaller GetPartnershipAgreementsWithPartnershipAgreementId(GetPartnershipAgreementsWithPartnershipAgreementIdPVM getPartnershipAgreementsWithPartnershipAgreementIdPVM)
        {
            return GetRecord(getPartnershipAgreementsWithPartnershipAgreementIdPVM);
        }

        //public ResponseApiCaller GetListOfPartnershipAgreementsWithNoParent(GetListOfPartnershipAgreementsWithNoParentPVM getListOfPartnershipAgreementsWithNoParentPVM)
        //{
        //    return GetRecords(getListOfPartnershipAgreementsWithNoParentPVM);
        //}
        //public ResponseApiCaller GetPartnershipAgreementsWithParentId(GetPartnershipAgreementsWithParentIdPVM getPartnershipAgreementsWithParentIdPVM)
        //{
        //    return GetRecords(getPartnershipAgreementsWithParentIdPVM);
        //}
        #endregion

        #region ProgressPicturesManagement
        public ResponseApiCaller GetAllProgressPicturesList(GetAllProgressPicturesListPVM getAllProgressPicturesListPVM)
        {
            return GetRecords(getAllProgressPicturesListPVM);
        }
        public ResponseApiCaller GetListOfProgressPictures(GetListOfProgressPicturesPVM getListOfProgressPicturesPVM)
        {
            return GetRecords(getListOfProgressPicturesPVM);
        }
        public ResponseApiCaller AddToProgressPictures(AddToProgressPicturesPVM addToProgressPicturesPVM)
        {
            return GetRecord(addToProgressPicturesPVM);
        }
        public ResponseApiCaller UpdateProgressPictures(UpdateProgressPicturesPVM updateProgressPicturesPVM)
        {
            return GetRecord(updateProgressPicturesPVM);
        }
        public ResponseApiCaller ToggleActivationProgressPictures(ToggleActivationProgressPicturesPVM toggleActivationProgressPicturesPVM)
        {
            return GetResult(toggleActivationProgressPicturesPVM);
        }
        public ResponseApiCaller TemporaryDeleteProgressPictures(TemporaryDeleteProgressPicturesPVM temporaryDeleteProgressPicturesPVM)
        {
            return GetResult(temporaryDeleteProgressPicturesPVM);
        }
        public ResponseApiCaller CompleteDeleteProgressPictures(CompleteDeleteProgressPicturesPVM completeDeleteProgressPicturesPVM)
        {
            return GetResult(completeDeleteProgressPicturesPVM);
        }
        public ResponseApiCaller GetProgressPicturesWithProgressPictureId(GetProgressPicturesWithProgressPictureIdPVM getProgressPicturesWithProgressPictureIdPVM)
        {
            return GetRecord(getProgressPicturesWithProgressPictureIdPVM);
        }

        #endregion

        #region PitchDecksManagement
        public ResponseApiCaller GetAllPitchDecksList(GetAllPitchDecksListPVM getAllPitchDecksListPVM)
        {
            return GetRecords(getAllPitchDecksListPVM);
        }
        public ResponseApiCaller GetListOfPitchDecks(GetListOfPitchDecksPVM getListOfPitchDecksPVM)
        {
            return GetRecords(getListOfPitchDecksPVM);
        }
        public ResponseApiCaller AddToPitchDecks(AddToPitchDecksPVM addToPitchDecksPVM)
        {
            return GetRecord(addToPitchDecksPVM);
        }
        public ResponseApiCaller UpdatePitchDecks(UpdatePitchDecksPVM updatePitchDecksPVM)
        {
            return GetRecord(updatePitchDecksPVM);
        }
        public ResponseApiCaller ToggleActivationPitchDecks(ToggleActivationPitchDecksPVM toggleActivationPitchDecksPVM)
        {
            return GetResult(toggleActivationPitchDecksPVM);
        }
        public ResponseApiCaller TemporaryDeletePitchDecks(TemporaryDeletePitchDecksPVM temporaryDeletePitchDecksPVM)
        {
            return GetResult(temporaryDeletePitchDecksPVM);
        }
        public ResponseApiCaller CompleteDeletePitchDecks(CompleteDeletePitchDecksPVM completeDeletePitchDecksPVM)
        {
            return GetResult(completeDeletePitchDecksPVM);
        }
        public ResponseApiCaller GetPitchDecksWithPitchDeckId(GetPitchDecksWithPitchDeckIdPVM getPitchDecksWithPitchDeckIdPVM)
        {
            return GetRecord(getPitchDecksWithPitchDeckIdPVM);
        }

        #endregion

        #region TimeTablesManagement
        public ResponseApiCaller GetAllTimeTablesList(GetAllTimeTablesListPVM getAllTimeTablesListPVM)
        {
            return GetRecords(getAllTimeTablesListPVM);
        }
        public ResponseApiCaller GetListOfTimeTables(GetListOfTimeTablesPVM getListOfTimeTablesPVM)
        {
            return GetRecords(getListOfTimeTablesPVM);
        }
        public ResponseApiCaller AddToTimeTables(AddToTimeTablesPVM addToTimeTablesPVM)
        {
            return GetRecord(addToTimeTablesPVM);
        }
        public ResponseApiCaller GetTimeTableWithTimeTableId(GetTimeTableWithTimeTableIdPVM getTimeTableWithTimeTableIdPVM)
        {
            return GetRecord(getTimeTableWithTimeTableIdPVM);
        }
        public ResponseApiCaller UpdateTimeTables(UpdateTimeTablesPVM updateTimeTablesPVM)
        {
            return GetRecord(updateTimeTablesPVM);
        }
        public ResponseApiCaller ToggleActivationTimeTables(ToggleActivationTimeTablesPVM toggleActivationTimeTablesPVM)
        {
            return GetResult(toggleActivationTimeTablesPVM);
        }
        public ResponseApiCaller TemporaryDeleteTimeTables(TemporaryDeleteTimeTablesPVM temporaryDeleteTimeTablesPVM)
        {
            return GetResult(temporaryDeleteTimeTablesPVM);
        }
        public ResponseApiCaller CompleteDeleteTimeTables(CompleteDeleteTimeTablesPVM completeDeleteTimeTablesPVM)
        {
            return GetResult(completeDeleteTimeTablesPVM);
        }

        #endregion

        #region TimeTableItemsManagement
        public ResponseApiCaller GetAllTimeTableItemsList(GetAllTimeTableItemsListPVM getAllTimeTableItemsListPVM)
        {
            return GetRecords(getAllTimeTableItemsListPVM);
        }
        public ResponseApiCaller GetListOfTimeTableItems(GetListOfTimeTableItemsPVM getListOfTimeTableItemsPVM)
        {
            return GetRecords(getListOfTimeTableItemsPVM);
        }
        public ResponseApiCaller AddToTimeTableItems(AddToTimeTableItemsPVM addToTimeTableItemsPVM)
        {
            return GetRecord(addToTimeTableItemsPVM);
        }
        public ResponseApiCaller GetTimeTableItemWithTimeTableId(GetTimeTableItemWithTimeTableItemIdPVM getTimeTableItemWithTimeTableItemIdPVM)
        {
            return GetRecord(getTimeTableItemWithTimeTableItemIdPVM);
        }
        public ResponseApiCaller UpdateTimeTableItems(UpdateTimeTableItemsPVM updateTimeTableItemsPVM)
        {
            return GetRecord(updateTimeTableItemsPVM);
        }
        public ResponseApiCaller ToggleActivationTimeTableItems(ToggleActivationTimeTableItemsPVM toggleActivationTimeTableItemsPVM)
        {
            return GetResult(toggleActivationTimeTableItemsPVM);
        }
        public ResponseApiCaller TemporaryDeleteTimeTableItems(TemporaryDeleteTimeTableItemsPVM temporaryDeleteTimeTableItemsPVM)
        {
            return GetResult(temporaryDeleteTimeTableItemsPVM);
        }
        public ResponseApiCaller CompleteDeleteTimeTableItems(CompleteDeleteTimeTableItemsPVM completeDeleteTimeTableItemsPVM)
        {
            return GetResult(completeDeleteTimeTableItemsPVM);
        }

        #endregion

        #region TeniacoSuggestionFiles
        public ResponseApiCaller GetListOfTeniacoSuggestionFilesWithConstructionProjectId(GetListOfTeniacoSuggestionFilesWithConstructionProjectIdPVM getTeniacoSuggestionFilesWithConstructionProjectIdPVM)
        {
            return GetRecords(getTeniacoSuggestionFilesWithConstructionProjectIdPVM);
        }


        public ResponseApiCaller GetListOfTeniacoSuggestedProjects(GetListOfTeniacoSuggestedProjectsPVM getListOfTeniacoSuggestedProjectsPVM)
        {
            return GetRecords(getListOfTeniacoSuggestedProjectsPVM);
        }

        public ResponseApiCaller AddToTeniacoSuggestionFiles(AddToTeniacoSuggestionFilesPVM addToTeniacoSuggestionFilesPVM)
        {
            return GetRecord(addToTeniacoSuggestionFilesPVM);
        }

        public ResponseApiCaller DeleteTeniacoSuggestionFile(DeleteTeniacoSuggestionFilePVM deleteTeniacoSuggestionFilePVM)
        {
            return GetRecord(deleteTeniacoSuggestionFilePVM);
        }

        public ResponseApiCaller EditTeniacoSuggestionFile(EditTeniacoSuggestionFilePVM editTeniacoSuggestionFile)
        {
            return GetRecord(editTeniacoSuggestionFile);
        }

        public ResponseApiCaller ChangeTeniacoSuggestionFilesOrder(ChangeTeniacoSuggestionFilesOrderPVM changeTeniacoSuggestionFilesOrder)
        {
            return GetRecord(changeTeniacoSuggestionFilesOrder);
        }
        #endregion

        #region FileStatesManagement
        public ResponseApiCaller GetAllFileStatesList(GetAllFileStatesListPVM getAllFileStatesListPVM)
        {
            return GetRecords(getAllFileStatesListPVM);
        }
        #endregion

        #region FileStatesLogsManagement
        public ResponseApiCaller GetAllFileStatesLogsList(GetAllFileStatesLogsListPVM getAllFileStatesLogsListPVM)
        {
            return GetRecords(getAllFileStatesLogsListPVM);
        }
        public ResponseApiCaller GetListOfFileStatesLogs(GetListOfFileStatesLogsPVM getListOfFileStatesLogsPVM)
        {
            return GetRecords(getListOfFileStatesLogsPVM);
        }
        public ResponseApiCaller AddToFileStatesLogs(AddToFileStatesLogsPVM addToFileStatesLogsPVM)
        {
            return GetRecord(addToFileStatesLogsPVM);
        }
        public ResponseApiCaller UpdateFileStatesLogs(UpdateFileStatesLogsPVM updateFileStatesLogsPVM)
        {
            return GetRecord(updateFileStatesLogsPVM);
        }
        public ResponseApiCaller ToggleActivationFileStatesLogs(ToggleActivationFileStatesLogsPVM toggleActivationFileStatesLogsPVM)
        {
            return GetResult(toggleActivationFileStatesLogsPVM);
        }
        public ResponseApiCaller TemporaryDeleteFileStatesLogs(TemporaryDeleteFileStatesLogsPVM temporaryDeleteFileStatesLogsPVM)
        {
            return GetResult(temporaryDeleteFileStatesLogsPVM);
        }
        public ResponseApiCaller CompleteDeleteFileStatesLogs(CompleteDeleteFileStatesLogsPVM completeDeleteFileStatesLogsPVM)
        {
            return GetResult(completeDeleteFileStatesLogsPVM);
        }
        public ResponseApiCaller GetFileStateLogWithFileStateLogId(GetFileStateLogWithFileStateLogIdPVM getFileStateLogWithFileStateLogIdPVM)
        {
            return GetRecord(getFileStateLogWithFileStateLogIdPVM);
        }
        #endregion

        #region FileStatesLogsManagement
        public ResponseApiCaller GetAllConversationLogsList(GetAllConversationLogsListPVM getAllConversationLogsListPVM)
        {
            return GetRecords(getAllConversationLogsListPVM);
        }
        public ResponseApiCaller GetListOfConversationLogs(GetListOfConversationLogsPVM getListOfConversationLogsPVM)
        {
            return GetRecords(getListOfConversationLogsPVM);
        }
        public ResponseApiCaller AddToConversationLogs(AddToConversationLogsPVM addToConversationLogsPVM)
        {
            return GetRecord(addToConversationLogsPVM);
        }
        public ResponseApiCaller UpdateConversationLogs(UpdateConversationLogsPVM updateConversationLogsPVM)
        {
            return GetRecord(updateConversationLogsPVM);
        }
        public ResponseApiCaller ToggleActivationConversationLogs(ToggleActivationConversationLogsPVM toggleActivationConversationLogsPVM)
        {
            return GetResult(toggleActivationConversationLogsPVM);
        }
        public ResponseApiCaller TemporaryDeleteConversationLogs(TemporaryDeleteConversationLogsPVM temporaryDeleteConversationLogsPVM)
        {
            return GetResult(temporaryDeleteConversationLogsPVM);
        }
        public ResponseApiCaller CompleteDeleteConversationLogs(CompleteDeleteConversationLogsPVM completeDeleteConversationLogsPVM)
        {
            return GetResult(completeDeleteConversationLogsPVM);
        }
        public ResponseApiCaller GetConversationLogWithConversationLogId(GetConversationLogWithConversationLogIdPVM getConversationLogWithConversationLogIdPVM)
        {
            return GetRecord(getConversationLogWithConversationLogIdPVM);
        }


        public ResponseApiCaller GetConversationDataByAgreementTypeAndRecordId(GetConversationDataByAgreementTypeAndRecordIdPVM getConversationDataByAgreementTypeAndRecordIdPVM)
        {
            return GetRecords(getConversationDataByAgreementTypeAndRecordIdPVM);
        }
        #endregion

        #region ProgressItemsManagement

        public ResponseApiCaller GetHierarchyOfAllProgressItemsList(GetHierarchyOfAllProgressItemsListPVM getHierarchyOfAllProgressItemsListPVM)
        {
            return GetRecords(getHierarchyOfAllProgressItemsListPVM);
        }


        public ResponseApiCaller GetAllProgressDataList(GetConstructionProjectWithUserIdPVM getConstructionProjectWithUserIdPVM)
        {
            return GetRecords(getConstructionProjectWithUserIdPVM);
        }
        #endregion
    }
}
