using ApiCallers.BaseApiCaller;
using PVM.Projects.TeniacoSuggestions;
using VM.Projects.TeniacoSuggestions;
using VM.PVM.Projects;

namespace ApiCallers.ProjectsApiCaller
{
    public interface IProjectsApiCaller
    {
        #region AttachementFilesManagement
        ResponseApiCaller GetAllAttachementFilesList(GetAllAttachementFilesListPVM getAllAttachementFilesListPVM);
        ResponseApiCaller GetListOfAttachementFiles(GetListOfAttachementFilesPVM getListOfAttachementFilesPVM);
        ResponseApiCaller AddToAttachementFiles(AddToAttachementFilesPVM addToAttachementFilesPVM);
        ResponseApiCaller UpdateAttachementFiles(UpdateAttachementFilesPVM updateAttachementFilesPVM);
        ResponseApiCaller ToggleActivationAttachementFiles(ToggleActivationAttachementFilesPVM toggleActivationAttachementFilesPVM);
        ResponseApiCaller TemporaryDeleteAttachementFiles(TemporaryDeleteAttachementFilesPVM temporaryDeleteAttachementFilesPVM);
        ResponseApiCaller CompleteDeleteAttachementFiles(CompleteDeleteAttachementFilesPVM completeDeleteAttachementFilesPVM);
        ResponseApiCaller GetAttachementFilesWithAttachementFileId(GetAttachementFilesWithAttachementFileIdPVM getAttachementFilesWithAttachementFileIdPVM);
        ResponseApiCaller GetAttachementsByAgreementIdAndTableTitle(GetAttachementsByAgreementIdAndTableTitlePVM getAttachementsByAgreementIdAndTableTitlePVM);
        #endregion

        #region ConstructionProjectsManagement
        ResponseApiCaller GetAllConstructionProjectsList(GetAllConstructionProjectsListPVM getAllConstructionProjectsListPVM);
        ResponseApiCaller GetListOfConstructionProjects(GetListOfConstructionProjectsPVM getListOfConstructionProjectsPVM);
        ResponseApiCaller AddToConstructionProjects(AddToConstructionProjectsPVM addToConstructionProjectsPVM);
        ResponseApiCaller UpdateConstructionProjects(UpdateConstructionProjectsPVM updateConstructionProjectsPVM);
        ResponseApiCaller ToggleActivationConstructionProjects(ToggleActivationConstructionProjectsPVM toggleActivationConstructionProjectsPVM);
        ResponseApiCaller TemporaryDeleteConstructionProjects(TemporaryDeleteConstructionProjectsPVM temporaryDeleteConstructionProjectsPVM);
        ResponseApiCaller CompleteDeleteConstructionProjects(CompleteDeleteConstructionProjectsPVM completeDeleteConstructionProjectsPVM);
        ResponseApiCaller GetConstructionProjectWithConstructionProjectId(GetConstructionProjectWithConstructionProjectIdPVM getConstructionProjectWithConstructionProjectIdPVM);
        ResponseApiCaller GetListOfConstructionProjectsWithUserId(GetConstructionProjectWithUserIdPVM getConstructionProjectWithUserIdPVM);
        ResponseApiCaller GetConstructionProjectsDailyDataByConstructionProjectId(GetConstructionProjectsDailyDataByProjectIdPVM getConstructionProjectsDailyDataByProjectIdPVM);
        ResponseApiCaller GetAgreementDataByAgreementTypeAndConstructionProjectId(GetAgreementDataByAgreementTypeAndConstructionProjectIdPVM getAgreementDataByAgreementTypeAndConstructionProjectIdPVM);
        ResponseApiCaller GetListOfConstructionProjectsWithRepresentativeId(GetConstructionProjectWithUserIdPVM getConstructionProjectWithUserIdPVM);
        #endregion

        #region ConstructionProjectTypesManagement
        ResponseApiCaller GetAllConstructionProjectTypesList(GetAllConstructionProjectTypesListPVM getAllConstructionProjectTypesListPVM);
        #endregion

        #region ConfirmationAgreementsManagement
        ResponseApiCaller GetAllConfirmationAgreementsList(GetAllConfirmationAgreementsListPVM getAllConfirmationAgreementsListPVM);
        ResponseApiCaller GetListOfConfirmationAgreements(GetListOfConfirmationAgreementsPVM getListOfConfirmationAgreementsPVM);
        ResponseApiCaller AddToConfirmationAgreements(AddToConfirmationAgreementsPVM addToConfirmationAgreementsPVM);
        ResponseApiCaller UpdateConfirmationAgreements(UpdateConfirmationAgreementsPVM updateConfirmationAgreementsPVM);
        ResponseApiCaller ToggleActivationConfirmationAgreements(ToggleActivationConfirmationAgreementsPVM toggleActivationConfirmationAgreementsPVM);
        ResponseApiCaller TemporaryDeleteConfirmationAgreements(TemporaryDeleteConfirmationAgreementsPVM temporaryDeleteConfirmationAgreementsPVM);
        ResponseApiCaller CompleteDeleteConfirmationAgreements(CompleteDeleteConfirmationAgreementsPVM completeDeleteConfirmationAgreementsPVM);
        ResponseApiCaller GetConfirmationAgreementsWithConfirmationAgreementId(GetConfirmationAgreementsWithConfirmationAgreementIdPVM getConfirmationAgreementsWithConfirmationAgreementIdPVM);
        //ResponseApiCaller ConfirmConfirmationAgreements(ConfirmConfirmationAgreementsPVM confirmConfirmationAgreementsPVM);

        #endregion

        #region ContractorsAgreementsManagement
        ResponseApiCaller GetAllContractorsAgreementsList(GetAllContractorsAgreementsListPVM getAllContractorsAgreementsListPVM);
        ResponseApiCaller GetListOfContractorsAgreements(GetListOfContractorsAgreementsPVM getListOfContractorsAgreementsPVM);
        ResponseApiCaller AddToContractorsAgreements(AddToContractorsAgreementsPVM addToContractorsAgreementsPVM);
        ResponseApiCaller UpdateContractorsAgreements(UpdateContractorsAgreementsPVM updateContractorsAgreementsPVM);
        ResponseApiCaller ToggleActivationContractorsAgreements(ToggleActivationContractorsAgreementsPVM toggleActivationContractorsAgreementsPVM);
        ResponseApiCaller TemporaryDeleteContractorsAgreements(TemporaryDeleteContractorsAgreementsPVM temporaryDeleteContractorsAgreementsPVM);
        ResponseApiCaller CompleteDeleteContractorsAgreements(CompleteDeleteContractorsAgreementsPVM completeDeleteContractorsAgreementsPVM);
        ResponseApiCaller GetContractorsAgreementsWithContractorsAgreementId(GetContractorsAgreementsWithContractorsAgreementIdPVM getContractorsAgreementsWithContractorsAgreementIdPVM);

        #endregion

        #region ContractAgreementsManagement
        ResponseApiCaller GetAllContractAgreementsList(GetAllContractAgreementsListPVM getAllContractAgreementsListPVM);
        ResponseApiCaller GetListOfContractAgreements(GetListOfContractAgreementsPVM getListOfContractAgreementsPVM);
        ResponseApiCaller AddToContractAgreements(AddToContractAgreementsPVM addToContractAgreementsPVM);
        ResponseApiCaller UpdateContractAgreements(UpdateContractAgreementsPVM updateContractAgreementsPVM);
        ResponseApiCaller ToggleActivationContractAgreements(ToggleActivationContractAgreementsPVM toggleActivationContractAgreementsPVM);
        ResponseApiCaller TemporaryDeleteContractAgreements(TemporaryDeleteContractAgreementsPVM temporaryDeleteContractAgreementsPVM);
        ResponseApiCaller CompleteDeleteContractAgreements(CompleteDeleteContractAgreementsPVM completeDeleteContractAgreementsPVM);
        ResponseApiCaller GetContractAgreementsWithContractAgreementId(GetContractAgreementsWithContractAgreementIdPVM getContractAgreementsWithContractAgreementIdPVM);

        #endregion

        #region ConfirmationTypesManagement
        ResponseApiCaller GetAllConfirmationTypesList(GetAllConfirmationTypesListPVM getAllConfirmationTypesListPVM);

        #endregion

        #region ContractSidesManagement
        ResponseApiCaller GetAllContractSidesList(GetAllContractSidesListPVM getAllContractSidesListPVM);
        ResponseApiCaller GetListOfContractSides(GetListOfContractSidesPVM getListOfContractSidesPVM);
        ResponseApiCaller AddToContractSides(AddToContractSidesPVM addToContractSidesPVM);
        ResponseApiCaller UpdateContractSides(UpdateContractSidesPVM updateContractSidesPVM);
        ResponseApiCaller ToggleActivationContractSides(ToggleActivationContractSidesPVM toggleActivationContractSidesPVM);
        ResponseApiCaller TemporaryDeleteContractSides(TemporaryDeleteContractSidesPVM temporaryDeleteContractSidesPVM);
        ResponseApiCaller CompleteDeleteContractSides(CompleteDeleteContractSidesPVM completeDeleteContractSidesPVM);
        ResponseApiCaller GetContractSidesWithContractSideId(GetContractSidesWithContractSideIdPVM getContractSidesWithContractSideIdPVM);

        #endregion

        #region ExcelSheetConfigsManagement
        ResponseApiCaller GetAllExcelSheetConfigsList(GetAllExcelSheetConfigsListPVM getAllExcelSheetConfigsListPVM);
        ResponseApiCaller GetListOfExcelSheetConfigs(GetListOfExcelSheetConfigsPVM getListOfExcelSheetConfigsPVM);
        ResponseApiCaller AddToExcelSheetConfigs(AddToExcelSheetConfigsPVM addToExcelSheetConfigsPVM);
        ResponseApiCaller UpdateExcelSheetConfigs(UpdateExcelSheetConfigsPVM updateExcelSheetConfigsPVM);
        ResponseApiCaller ToggleActivationExcelSheetConfigs(ToggleActivationExcelSheetConfigsPVM toggleActivationExcelSheetConfigsPVM);
        ResponseApiCaller TemporaryDeleteExcelSheetConfigs(TemporaryDeleteExcelSheetConfigsPVM temporaryDeleteExcelSheetConfigsPVM);
        ResponseApiCaller CompleteDeleteExcelSheetConfigs(CompleteDeleteExcelSheetConfigsPVM completeDeleteExcelSheetConfigsPVM);
        ResponseApiCaller GetExcelSheetConfigsWithExcelSheetConfigId(GetExcelSheetConfigsWithExcelSheetConfigIdPVM getExcelSheetConfigsWithExcelSheetConfigIdPVM);

        #endregion

        #region ExcelSheetConfigHistoriesManagement
        public ResponseApiCaller GetAllExcelSheetConfigHistoriesList(GetAllExcelSheetConfigHistoriesListPVM getAllExcelSheetConfigHistoriesListPVM);
        public ResponseApiCaller GetListOfExcelSheetConfigHistories(GetListOfExcelSheetConfigHistoriesPVM getListOfExcelSheetConfigHistoriesPVM);
        public ResponseApiCaller AddToExcelSheetConfigHistories(AddToExcelSheetConfigHistoriesPVM addToExcelSheetConfigHistoriesPVM);
        public ResponseApiCaller UpdateExcelSheetConfigHistories(UpdateExcelSheetConfigHistoriesPVM updateExcelSheetConfigHistoriesPVM);
        public ResponseApiCaller ToggleActivationExcelSheetConfigHistories(ToggleActivationExcelSheetConfigHistoriesPVM toggleActivationExcelSheetConfigHistoriesPVM);
        public ResponseApiCaller TemporaryDeleteExcelSheetConfigHistories(TemporaryDeleteExcelSheetConfigHistoriesPVM temporaryDeleteExcelSheetConfigHistoriesPVM);
        public ResponseApiCaller CompleteDeleteExcelSheetConfigHistories(CompleteDeleteExcelSheetConfigHistoriesPVM completeDeleteExcelSheetConfigHistoriesPVM);
        public ResponseApiCaller GetExcelSheetConfigHistoriesWithExcelSheetConfigHistoryId(GetExcelSheetConfigHistoriesWithExcelSheetConfigHistoryIdPVM getExcelSheetConfigHistoriesWithExcelSheetConfigHistoryId);

        #endregion

        #region GoogleSheetConfigsManagement
        ResponseApiCaller GetAllGoogleSheetConfigsList(GetAllGoogleSheetConfigsListPVM getAllGoogleSheetConfigsListPVM);
        ResponseApiCaller GetListOfGoogleSheetConfigs(GetListOfGoogleSheetConfigsPVM getListOfGoogleSheetConfigsPVM);

        ResponseApiCaller AddToGoogleSheetConfigs(AddToGoogleSheetConfigsPVM addToGoogleSheetConfigsPVM);

        ResponseApiCaller UpdateGoogleSheetConfigs(UpdateGoogleSheetConfigsPVM updateGoogleSheetConfigsPVM);

        ResponseApiCaller ToggleActivationGoogleSheetConfigs(ToggleActivationGoogleSheetConfigsPVM toggleActivationGoogleSheetConfigsPVM);

        ResponseApiCaller TemporaryDeleteGoogleSheetConfigs(TemporaryDeleteGoogleSheetConfigsPVM temporaryDeleteGoogleSheetConfigsPVM);

        ResponseApiCaller CompleteDeleteGoogleSheetConfigs(CompleteDeleteGoogleSheetConfigsPVM completeDeleteGoogleSheetConfigsPVM);

        ResponseApiCaller GetGoogleSheetConfigsWithGoogleSheetConfigId(GetGoogleSheetConfigsWithGoogleSheetConfigIdPVM getGoogleSheetConfigsWithGoogleSheetConfigIdPVM);

        #endregion

        #region InitialPlansManagement
        ResponseApiCaller GetAllInitialPlansList(GetAllInitialPlansListPVM getAllInitialPlansListPVM);
        ResponseApiCaller GetListOfInitialPlans(GetListOfInitialPlansPVM getListOfInitialPlansPVM);
        ResponseApiCaller AddToInitialPlans(AddToInitialPlansPVM addToInitialPlansPVM);
        ResponseApiCaller UpdateInitialPlans(UpdateInitialPlansPVM updateInitialPlansPVM);
        ResponseApiCaller ToggleActivationInitialPlans(ToggleActivationInitialPlansPVM toggleActivationInitialPlansPVM);
        ResponseApiCaller TemporaryDeleteInitialPlans(TemporaryDeleteInitialPlansPVM temporaryDeleteInitialPlansPVM);
        ResponseApiCaller CompleteDeleteInitialPlans(CompleteDeleteInitialPlansPVM completeDeleteInitialPlansPVM);
        ResponseApiCaller GetInitialPlansWithInitialPlanId(GetInitialPlansWithInitialPlanIdPVM getInitialPlansWithInitialPlanIdPVM);

        #endregion

        #region MeetingBoardsManagement
        ResponseApiCaller GetAllMeetingBoardsList(GetAllMeetingBoardsListPVM getAllMeetingBoardsListPVM);
        ResponseApiCaller GetListOfMeetingBoards(GetListOfMeetingBoardsPVM getListOfMeetingBoardsPVM);
        ResponseApiCaller AddToMeetingBoards(AddToMeetingBoardsPVM addToMeetingBoardsPVM);
        ResponseApiCaller UpdateMeetingBoards(UpdateMeetingBoardsPVM updateMeetingBoardsPVM);
        ResponseApiCaller ToggleActivationMeetingBoards(ToggleActivationMeetingBoardsPVM toggleActivationMeetingBoardsPVM);
        ResponseApiCaller TemporaryDeleteMeetingBoards(TemporaryDeleteMeetingBoardsPVM temporaryDeleteMeetingBoardsPVM);
        ResponseApiCaller CompleteDeleteMeetingBoards(CompleteDeleteMeetingBoardsPVM completeDeleteMeetingBoardsPVM);
        ResponseApiCaller GetMeetingBoardsWithMeetingBoardId(GetMeetingBoardsWithMeetingBoardIdPVM getMeetingBoardsWithMeetingBoardIdPVM);

        #endregion

        #region PropertyProjectsManagement

        ResponseApiCaller GetAllPropertyProjectsList(GetAllPropertyProjectsListPVM getAllPropertyProjectsListPVM);

        ResponseApiCaller GetListOfPropertyProjects(GetListOfPropertyProjectsPVM getListOfPropertyProjectsPVM);

        ResponseApiCaller AddToPropertyProjects(AddToPropertyProjectsPVM addToPropertyProjectsPVM);

        ResponseApiCaller GetPropertyProjectWithPropertyProjectId(GetPropertyProjectWithPropertyProjectIdPVM getPropertyProjectWithPropertyProjectIdPVM);

        ResponseApiCaller UpdatePropertyProjects(UpdatePropertyProjectsPVM updatePropertyProjectsPVM);

        ResponseApiCaller ToggleActivationPropertyProjects(ToggleActivationPropertyProjectsPVM toggleActivationPropertyProjectsPVM);

        ResponseApiCaller TemporaryDeletePropertyProjects(TemporaryDeletePropertyProjectsPVM temporaryDeletePropertyProjectsPVM);

        ResponseApiCaller CompleteDeletePropertyProjects(CompleteDeletePropertyProjectsPVM completeDeletePropertyProjectsPVM);

        #endregion

        #region PartnershipAgreementsManagement
        ResponseApiCaller GetAllPartnershipAgreementsList(GetAllPartnershipAgreementsListPVM getAllPartnershipAgreementsListPVM);
        ResponseApiCaller GetListOfPartnershipAgreements(GetListOfPartnershipAgreementsPVM getListOfPartnershipAgreementsPVM);
        ResponseApiCaller AddToPartnershipAgreements(AddToPartnershipAgreementsPVM addToPartnershipAgreementsPVM);
        ResponseApiCaller UpdatePartnershipAgreements(UpdatePartnershipAgreementsPVM updatePartnershipAgreementsPVM);
        ResponseApiCaller ToggleActivationPartnershipAgreements(ToggleActivationPartnershipAgreementsPVM toggleActivationPartnershipAgreementsPVM);
        ResponseApiCaller TemporaryDeletePartnershipAgreements(TemporaryDeletePartnershipAgreementsPVM temporaryDeletePartnershipAgreementsPVM);
        ResponseApiCaller CompleteDeletePartnershipAgreements(CompleteDeletePartnershipAgreementsPVM completeDeletePartnershipAgreementsPVM);
        ResponseApiCaller GetPartnershipAgreementsWithPartnershipAgreementId(GetPartnershipAgreementsWithPartnershipAgreementIdPVM getPartnershipAgreementsWithPartnershipAgreementIdPVM);
        //ResponseApiCaller GetListOfPartnershipAgreementsWithNoParent(GetListOfPartnershipAgreementsWithNoParentPVM getListOfPartnershipAgreementsWithNoParentPVM);
        //ResponseApiCaller GetPartnershipAgreementsWithParentId(GetPartnershipAgreementsWithParentIdPVM getPartnershipAgreementsWithParentIdPVM);
        //PartnershipAgreementId
        #endregion

        #region PitchDecksManagement
        ResponseApiCaller GetAllPitchDecksList(GetAllPitchDecksListPVM getAllPitchDecksListPVM);
        ResponseApiCaller GetListOfPitchDecks(GetListOfPitchDecksPVM getListOfPitchDecksPVM);
        ResponseApiCaller AddToPitchDecks(AddToPitchDecksPVM addToPitchDecksPVM);
        ResponseApiCaller UpdatePitchDecks(UpdatePitchDecksPVM updatePitchDecksPVM);
        ResponseApiCaller ToggleActivationPitchDecks(ToggleActivationPitchDecksPVM toggleActivationPitchDecksPVM);
        ResponseApiCaller TemporaryDeletePitchDecks(TemporaryDeletePitchDecksPVM temporaryDeletePitchDecksPVM);
        ResponseApiCaller CompleteDeletePitchDecks(CompleteDeletePitchDecksPVM completeDeletePitchDecksPVM);
        ResponseApiCaller GetPitchDecksWithPitchDeckId(GetPitchDecksWithPitchDeckIdPVM getPitchDecksWithPitchDeckIdPVM);

        #endregion

        #region ProgressPicturesManagement
        ResponseApiCaller GetAllProgressPicturesList(GetAllProgressPicturesListPVM getAllProgressPicturesListPVM);
        ResponseApiCaller GetListOfProgressPictures(GetListOfProgressPicturesPVM getListOfProgressPicturesPVM);

        ResponseApiCaller AddToProgressPictures(AddToProgressPicturesPVM addToProgressPicturesPVM);

        ResponseApiCaller UpdateProgressPictures(UpdateProgressPicturesPVM updateProgressPicturesPVM);

        ResponseApiCaller ToggleActivationProgressPictures(ToggleActivationProgressPicturesPVM toggleActivationProgressPicturesPVM);

        ResponseApiCaller TemporaryDeleteProgressPictures(TemporaryDeleteProgressPicturesPVM temporaryDeleteProgressPicturesPVM);

        ResponseApiCaller CompleteDeleteProgressPictures(CompleteDeleteProgressPicturesPVM completeDeleteProgressPicturesPVM);

        ResponseApiCaller GetProgressPicturesWithProgressPictureId(GetProgressPicturesWithProgressPictureIdPVM getProgressPicturesWithProgressPictureIdPVM);

        #endregion

        #region TimeTablesManagement
        ResponseApiCaller GetAllTimeTablesList(GetAllTimeTablesListPVM getAllTimeTablesListPVM);
        ResponseApiCaller GetListOfTimeTables(GetListOfTimeTablesPVM getListOfTimeTablesPVM);
        ResponseApiCaller AddToTimeTables(AddToTimeTablesPVM addToTimeTablesPVM);
        ResponseApiCaller GetTimeTableWithTimeTableId(GetTimeTableWithTimeTableIdPVM getTimeTableWithTimeTableIdPVM);
        ResponseApiCaller UpdateTimeTables(UpdateTimeTablesPVM updateTimeTablesPVM);
        ResponseApiCaller ToggleActivationTimeTables(ToggleActivationTimeTablesPVM toggleActivationTimeTablesPVM);
        ResponseApiCaller TemporaryDeleteTimeTables(TemporaryDeleteTimeTablesPVM temporaryDeleteTimeTablesPVM);
        ResponseApiCaller CompleteDeleteTimeTables(CompleteDeleteTimeTablesPVM completeDeleteTimeTablesPVM);
        #endregion

        #region TimeTableItemsManagement
        ResponseApiCaller GetAllTimeTableItemsList(GetAllTimeTableItemsListPVM getAllTimeTableItemsListPVM);
        ResponseApiCaller GetListOfTimeTableItems(GetListOfTimeTableItemsPVM getListOfTimeTableItemsPVM);
        ResponseApiCaller AddToTimeTableItems(AddToTimeTableItemsPVM addToTimeTableItemsPVM);
        ResponseApiCaller GetTimeTableItemWithTimeTableId(GetTimeTableItemWithTimeTableItemIdPVM getTimeTableItemWithTimeTableItemIdPVM);
        ResponseApiCaller UpdateTimeTableItems(UpdateTimeTableItemsPVM updateTimeTableItemsPVM);
        ResponseApiCaller ToggleActivationTimeTableItems(ToggleActivationTimeTableItemsPVM toggleActivationTimeTableItemsPVM);
        ResponseApiCaller TemporaryDeleteTimeTableItems(TemporaryDeleteTimeTableItemsPVM temporaryDeleteTimeTableItemsPVM);
        ResponseApiCaller CompleteDeleteTimeTableItems(CompleteDeleteTimeTableItemsPVM completeDeleteTimeTableItemsPVM);
        #endregion

        #region TeniacoSuggestionFiles
        ResponseApiCaller GetListOfTeniacoSuggestionFilesWithConstructionProjectId(GetListOfTeniacoSuggestionFilesWithConstructionProjectIdPVM getTeniacoSuggestionFilesWithConstructionProjectIdPVM);


        ResponseApiCaller GetListOfTeniacoSuggestedProjects(GetListOfTeniacoSuggestedProjectsPVM getListOfTeniacoSuggestedProjectsPVM);
        ResponseApiCaller AddToTeniacoSuggestionFiles(AddToTeniacoSuggestionFilesPVM addToTeniacoSuggestionFilesPVM);

        ResponseApiCaller DeleteTeniacoSuggestionFile(DeleteTeniacoSuggestionFilePVM deleteTeniacoSuggestionFilePVM);

        ResponseApiCaller EditTeniacoSuggestionFile(EditTeniacoSuggestionFilePVM editTeniacoSuggestionFile);

        ResponseApiCaller ChangeTeniacoSuggestionFilesOrder(ChangeTeniacoSuggestionFilesOrderPVM changeTeniacoSuggestionFilesOrder);
        #endregion

        #region FileStatesManagement
        ResponseApiCaller GetAllFileStatesList(GetAllFileStatesListPVM getAllFileStatesListPVM);

        #endregion

        #region FileStatesLogsManagement
        ResponseApiCaller GetAllFileStatesLogsList(GetAllFileStatesLogsListPVM getAllFileStatesLogsListPVM);
        ResponseApiCaller GetListOfFileStatesLogs(GetListOfFileStatesLogsPVM getListOfFileStatesLogsPVM);
        ResponseApiCaller AddToFileStatesLogs(AddToFileStatesLogsPVM addToFileStatesLogsPVM);
        ResponseApiCaller UpdateFileStatesLogs(UpdateFileStatesLogsPVM updateFileStatesLogsPVM);
        ResponseApiCaller ToggleActivationFileStatesLogs(ToggleActivationFileStatesLogsPVM toggleActivationFileStatesLogsPVM);
        ResponseApiCaller TemporaryDeleteFileStatesLogs(TemporaryDeleteFileStatesLogsPVM temporaryDeleteFileStatesLogsPVM);
        ResponseApiCaller CompleteDeleteFileStatesLogs(CompleteDeleteFileStatesLogsPVM completeDeleteFileStatesLogsPVM);
        ResponseApiCaller GetFileStateLogWithFileStateLogId(GetFileStateLogWithFileStateLogIdPVM getFileStateLogWithFileStateLogIdPVM);
        ResponseApiCaller GetConversationDataByAgreementTypeAndRecordId(GetConversationDataByAgreementTypeAndRecordIdPVM getConversationDataByAgreementTypeAndRecordIdPVM);
        #endregion

        #region ConversationLogsManagement
        ResponseApiCaller GetAllConversationLogsList(GetAllConversationLogsListPVM getAllConversationLogsListPVM);
        ResponseApiCaller GetListOfConversationLogs(GetListOfConversationLogsPVM getListOfConversationLogsPVM);
        ResponseApiCaller AddToConversationLogs(AddToConversationLogsPVM addToConversationLogsPVM);
        ResponseApiCaller UpdateConversationLogs(UpdateConversationLogsPVM updateConversationLogsPVM);
        ResponseApiCaller ToggleActivationConversationLogs(ToggleActivationConversationLogsPVM toggleActivationConversationLogsPVM);
        ResponseApiCaller TemporaryDeleteConversationLogs(TemporaryDeleteConversationLogsPVM temporaryDeleteConversationLogsPVM);
        ResponseApiCaller CompleteDeleteConversationLogs(CompleteDeleteConversationLogsPVM completeDeleteConversationLogsPVM);
        ResponseApiCaller GetConversationLogWithConversationLogId(GetConversationLogWithConversationLogIdPVM getConversationLogWithConversationLogIdPVM);
        #endregion

        #region ConstructionProjectDailyPicturesManagement

        ResponseApiCaller GetListOfConstructionProjectDailyPicturesWithLastDate(
            GetListOfConstructionProjectDailyPicturesWithLastDatePVM getListOfConstructionProjectDailyPicturesWithLastDatePVM);

        #endregion

        #region ConstructionProjectFinancialData

        ResponseApiCaller GetConstructionProjectFinancialDataByConstructionProjectId(GetConstructionProjectFinancialDataByConstructionProjectIdPVM getConstructionProjectFinancialDataByConstructionProjectIdPVM);

        ResponseApiCaller GetPaymentsAndCostsList(GetPaymentsAndCostsListPVM getPaymentsAndCostsListPVM);

        ResponseApiCaller GetSumOfPrivateCostsList(GetSumOfPrivateCostsListPVM getSumOfPrivateCostsListPVM);

        ResponseApiCaller GetSumOfPrivateCostsListForRepresantative(GetSumOfPrivateCostsListPVM getSumOfPrivateCostsListPVM);
        ResponseApiCaller GetSumOfPublicCostsList(GetSumOfPublicCostsListPVM getSumOfPublicCostsListPVM);


        ResponseApiCaller GetSumOfPublicCostsListForRepresentative(GetSumOfPublicCostsForRepresentativeListPVM getSumOfPublicCostsListPVM);
        #endregion

        #region ProgressItemsManagement

        ResponseApiCaller GetHierarchyOfAllProgressItemsList(GetHierarchyOfAllProgressItemsListPVM getHierarchyOfAllProgressItemsListPVM);

        ResponseApiCaller GetAllProgressDataList(GetConstructionProjectWithUserIdPVM getConstructionProjectWithUserIdPVM);
        #endregion

        #region ConstructionProjectBillDelaysManagement

        ResponseApiCaller GetAllConstructionProjectBillDelaysList(GetAllConstructionProjectBillDelaysListPVM getAllConstructionProjectBillDelaysListPVM);

        ResponseApiCaller AddToConstructionProjectBillDelays(AddToConstructionProjectBillDelaysPVM addToConstructionProjectBillDelaysPVM);


        ResponseApiCaller UpdateConstructionProjectBillDelays(UpdateConstructionProjectBillDelaysPVM updateConstructionProjectBillDelaysPVM);


        ResponseApiCaller GetListOfBillDelaysForOuterDashboard(GetListOfBillDelaysForOuterDashboardPVM getListOfBillDelaysForOuterDashboardPVM);
        #endregion

        #region ConstructionProjectDelaysManagement


        ResponseApiCaller GetAllConstructionProjectDelays(GetAllConstructionProjectDelaysListPVM getAllConstructionProjectDelaysListPVM);


        ResponseApiCaller GetListOfConstructionProjectDelays(GetListOfConstructionProjectDelaysPVM getListOfConstructionProjectDelays);

        ResponseApiCaller AddToConstructionProjectDelays(AddToConstructionProjectDelaysPVM addToConstructionProjectDelaysPVM);

        ResponseApiCaller UpdateConstructionProjectDelays(UpdateConstructionProjectDelaysPVM updateConstructionProjectDelaysPVM);



        ResponseApiCaller ToggleActivationConstructionProjectDelays(ToggleActivationConstructionProjectDelaysPVM toggleActivationConstructionProjectDelaysPVM);
        ResponseApiCaller TemporaryDeleteConstructionProjectDelays(TemporaryDeleteConstructionProjectDelaysPVM temporaryDeleteConstructionProjectDelaysPVM);
        ResponseApiCaller CompleteDeleteConstructionProjectDelays(CompleteDeleteConstructionProjectDelaysPVM completeDeleteConstructionProjectsPVM);

        ResponseApiCaller GetConstructionProjectDelayById(GetConstructionProjectDelayByIdPVM getConstructionProjectDelayByIdPVM);

        #endregion
    }
}
