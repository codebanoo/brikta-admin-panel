using APIs.Projects.Models.Entities;
using APIs.Public.Models.Business;
using APIs.Teniaco.Models.Business;
using Models.Business.ConsoleBusiness;
using System;
using System.Collections.Generic;
using VM.Projects;
using VM.PVM.Projects;

namespace APIs.Projects.Models.Business
{
    public interface IProjectsApiBusiness
    {
        ProjectsApiContext ProjectsApiDb { get; set; }

        #region Projects

        #region Methods For Work With AttachementFiles

        List<AttachementFilesVM> GetAllAttachementFilesList(
             List<long> childsUsersIds,
             long? attachementParentId = 0,
             string? AttachementTableTitle = "");
        List<AttachementFilesVM> GetListOfAttachementFiles(int jtStartIndex,
             int jtPageSize,
             ref int listCount,
             List<long> childsUsersIds,
             string? AttachementFileTitle = "",
             long? attachementParentId = 0,
             string jtSorting = null);


        long AddToAttachementFiles(AttachementFilesVM attachementFilesVM);

        AttachementFilesVM UpdateAttachementFiles(
             List<long> childsUsersIds,
           AttachementFilesVM attachementFilesVM);

        bool ToggleActivationAttachementFiles(long attachementFileId,
           long userId,
           List<long> childsUsersIds);

        bool TemporaryDeleteAttachementFiles(long attachementFileId,
           long userId,
           List<long> childsUsersIds);

        bool CompleteDeleteAttachementFiles(long attachementFileId,
           long userId,
           List<long> childsUsersIds);

        AttachementFilesVM GetAttachementFilesWithAttachementFileId(long attachementFileId,
           long userId,
           List<long> childsUsersIds);

        List<AttachementFilesVM> GetAttachementsByAgreementIdAndTableTitle(
                long agreeemntId,
                string tableTitle,
                long userId

            );
        #endregion

        #region Methods For Work With ConstructionProjects
        List<ConstructionProjectsVM> GetAllConstructionProjectsList(
               List<long> childsUsersIds,
           string? constructionProjectTitle = "");
        List<ConstructionProjectsVM> GetListOfConstructionProjects(int jtStartIndex,
             int jtPageSize,
             ref int listCount,
             List<long> childsUsersIds,
             string? ConstructionProjectTitle = "",
             string jtSorting = null);

        long AddToConstructionProjects(
           ConstructionProjectsVM constructionProjectsVM);


        long UpdateConstructionProjects(
               List<long> childsUsersIds,
               ConstructionProjectsVM constructionProjectsVM);


        bool ToggleActivationConstructionProjects(long constructionProjectId,
           long userId,
           List<long> childsUsersIds);


        bool TemporaryDeleteConstructionProjects(long constructionProjectId,
           long userId,
           List<long> childsUsersIds);

        bool? CompleteDeleteConstructionProjects(long constructionProjectId,
           long userId,
           List<long> childsUsersIds);


        ConstructionProjectsVM GetConstructionProjectWithConstructionProjectId(long constructionProjectId,
         List<long> childsUsersIds);



        string ToggleShowInDashboardConstructionProjects(long constructionProjectId,
                        long userId,
                        List<long> childsUsersIds);

        //outterDashboard
        List<ConstructionProjectsVM> GetListOfConstructionProjectsWithUserId(GetConstructionProjectWithUserIdPVM getConstructionProjectWithUserIdPVM);

        List<ConstructionProjectDailyDataForOuterDashbordVM> GetConstructionProjectsDailyDataByConstructionProjectId(int jtStartIndex,
             int jtPageSize,
             ref int listCount,
              long ConstructionProjectId,
             string jtSorting = null
              );

        List<AgreementFileWithAttachmentsVM> GetAgreementDataByAgreementTypeAndConstructionProjectId(
          long constructionProjectId,
           string ContractType,
           long userId,
           bool HaveAttachments,
           bool HaveConversations
          );

        List<ConstructionProjectDataTypeCounts> GetListConstructionProjectDataTypeCounts(List<long> constructionProjectIds);


        List<ConstructionProjectsVM> GetListOfConstructionProjectsWithRepresentativeId(GetConstructionProjectWithUserIdPVM getConstructionProjectWithUserIdPVM, IConsoleBusiness consoleBusiness);
        #endregion

        #region Methods For Work With ConstructionProjectTypes
        List<ConstructionProjectTypesVM> GetAllConstructionProjectTypesList(
                List<long> childsUsersIds);
        #endregion

        #region Methods For Work With ConfirmationAgreements

        List<ConfirmationAgreementsVM> GetAllConfirmationAgreementsList(
             List<long> childsUsersIds,
           string? ConfirmationAgreementTitle = "",
           long? ConstructionProjectId = 0);


        List<ConfirmationAgreementsVM> GetListOfConfirmationAgreements(int jtStartIndex,
             int jtPageSize,
             ref int listCount,
             List<long> childsUsersIds,
             string? ConfirmationAgreementTitle = "",
             long? ConstructionProjectId = 0,
             string jtSorting = null);


        long AddToConfirmationAgreements(ConfirmationAgreementsVM confirmationAgreementsVM);
        ConfirmationAgreementsVM UpdateConfirmationAgreements(
             List<long> childsUsersIds,
           ConfirmationAgreementsVM confirmationAgreementsVM);

        bool ToggleActivationConfirmationAgreements(long confirmationAgreementId,
           long userId,
           List<long> childsUsersIds);

        bool TemporaryDeleteConfirmationAgreements(long confirmationAgreementId,
           long userId,
           List<long> childsUsersIds);

        bool? CompleteDeleteConfirmationAgreements(long confirmationAgreementId,
           long userId,
           List<long> childsUsersIds);

        ConfirmationAgreementsVM GetConfirmationAgreementsWithConfirmationAgreementId(long confirmationAgreementId,
           long userId,
           List<long> childsUsersIds);
        #endregion

        #region Methods For Work With ConfirmationTypes
        List<ConfirmationTypesVM> GetAllConfirmationTypesList();
        #endregion

        #region Methods For Work With ContractorsAgreements

        List<ContractorsAgreementsVM> GetAllContractorsAgreementsList(
             List<long> childsUsersIds,
           string? ContractorsAgreementTitle = "",
           long? ConstructionProjectId = 0);
        List<ContractorsAgreementsVM> GetListOfContractorsAgreements(int jtStartIndex,
             int jtPageSize,
             ref int listCount,
             List<long> childsUsersIds,
             string? ContractorsAgreementTitle = "",
             long? ConstructionProjectId = 0,
             string jtSorting = null);


        long AddToContractorsAgreements(ContractorsAgreementsVM contractorsAgreementsVM);

        ContractorsAgreementsVM UpdateContractorsAgreements(
             List<long> childsUsersIds,
           ContractorsAgreementsVM contractorsAgreementsVM);

        bool ToggleActivationContractorsAgreements(long contractorsAgreementId,
           long userId,
           List<long> childsUsersIds);

        bool TemporaryDeleteContractorsAgreements(long contractorsAgreementId,
           long userId,
           List<long> childsUsersIds);

        bool? CompleteDeleteContractorsAgreements(long contractorsAgreementId,
           long userId,
           List<long> childsUsersIds);

        ContractorsAgreementsVM GetContractorsAgreementsWithContractorsAgreementId(long contractorsAgreementId,
           long userId,
           List<long> childsUsersIds);
        #endregion

        #region Methods For Work With ContractAgreements

        List<ContractAgreementsVM> GetAllContractAgreementsList(
             List<long> childsUsersIds,
           string? ContractAgreementTitle = "",
           long? ConstructionProjectId = 0);
        List<ContractAgreementsVM> GetListOfContractAgreements(int jtStartIndex,
             int jtPageSize,
             ref int listCount,
             List<long> childsUsersIds,
             string? ContractAgreementTitle = "",
             long? ConstructionProjectId = 0,
             string jtSorting = null);



        long AddToContractAgreements(ContractAgreementsVM contractAgreementsVM);

        ContractAgreementsVM UpdateContractAgreements(
             List<long> childsUsersIds,
           ContractAgreementsVM contractAgreementsVM);

        bool ToggleActivationContractAgreements(long contractAgreementId,
           long userId,
           List<long> childsUsersIds);

        bool TemporaryDeleteContractAgreements(long contractAgreementId,
           long userId,
           List<long> childsUsersIds);

        bool? CompleteDeleteContractAgreements(long contractAgreementId,
           long userId,
           List<long> childsUsersIds);

        ContractAgreementsVM GetContractAgreementsWithContractAgreementId(long contractAgreementId,
           long userId,
           List<long> childsUsersIds);
        #endregion

        #region Methods For Work With ContractSides

        List<ContractSidesVM> GetAllContractSidesList(
             List<long> childsUsersIds,
             long? ParentId = 0,
             long? TableRecordId = 0,
             string? TableTitle = "");
        List<ContractSidesVM> GetListOfContractSides(int jtStartIndex,
             int jtPageSize,
             ref int listCount,
             List<long> childsUsersIds,
             long? ParentId = 0,
             long? TableRecordId = 0,
             string? TableTitle = "",
             string jtSorting = null);


        long AddToContractSides(ContractSidesVM contractSidesVM);

        ContractSidesVM UpdateContractSides(
             List<long> childsUsersIds,
           ContractSidesVM contractSidesVM);

        bool ToggleActivationContractSides(long contractSideId,
           long userId,
           List<long> childsUsersIds);

        bool TemporaryDeleteContractSides(long contractSideId,
           long userId,
           List<long> childsUsersIds);

        bool CompleteDeleteContractSides(long contractSideId,
           long userId,
           List<long> childsUsersIds);

        ContractSidesVM GetContractSidesWithContractSideId(long contractSideId,
           long userId,
           List<long> childsUsersIds);
        #endregion

        #region Methods For Work With ConstructionProjectDailyData

        List<ConstructionProjectDailyDataVM> GetAllConstructionProjectDailyDataList(long? constructionProjectId);

        List<ConstructionProjectDailyDataVM> GetListOfConstructionProjectDailyData(int jtStartIndex,
              int jtPageSize,
              ref int listCount,
              long? constructionProjectId,
              string jtSorting = null);

        long AddToConstructionProjectDailyData(ConstructionProjectDailyDataVM constructionProjectDailyDataVM);

        bool GroupAddToConstructionProjectDailyData(List<ConstructionProjectDailyDataVM> constructionProjectDailyDataVMList);

        bool UpdateConstructionProjectDailyData(ConstructionProjectDailyDataVM constructionProjectDailyDataVM);

        bool ToggleActivationConstructionProjectDailyData(long constructionProjectDailyDataId, long userId);

        bool TemporaryDeleteConstructionProjectDailyData(long constructionProjectDailyDataId, long userId);

        bool CompleteDeleteConstructionProjectDailyData(long constructionProjectDailyDataId);

        bool GroupCompleteDeleteConstructionProjectDailyData(long constructionProjectId);

        ConstructionProjectDailyDataVM GetDailyDataWithDailyDataId(long constructionProjectDailyDataId);

        #endregion

        #region Methods For Work With ExcelSheetConfigs

        List<ExcelSheetConfigsVM> GetAllExcelSheetConfigsList(
            //List<long> childsUsersIds,
            string? ExcelSheetConfigTitle = "",
            long? GoogleSheetConfigId = 0);
        List<ExcelSheetConfigsVM> GetListOfExcelSheetConfigs(int jtStartIndex,
              int jtPageSize,
              ref int listCount,
              //List<long> childsUsersIds,
              string? ExcelSheetConfigTitle = "",
              long? GoogleSheetConfigId = 0,
              string jtSorting = null);


        long AddToExcelSheetConfigs(ExcelSheetConfigsVM excelSheetConfigsVM);

        long UpdateExcelSheetConfigs(
            //List<long> childsUsersIds,
            ExcelSheetConfigsVM excelSheetConfigsVM);

        bool ToggleActivationExcelSheetConfigs(long excelSheetConfigId,
            long userId);
        //List<long> childsUsersIds);

        bool TemporaryDeleteExcelSheetConfigs(long excelSheetConfigId,
            long userId);
        //List<long> childsUsersIds);

        bool CompleteDeleteExcelSheetConfigs(long excelSheetConfigId,
            long userId);
        //List<long> childsUsersIds);

        ExcelSheetConfigsVM GetExcelSheetConfigsWithExcelSheetConfigId(long excelSheetConfigId,
            long userId);
        //List<long> childsUsersIds);
        #endregion

        #region Methods For Work With ExcelSheetConfigHistories

        List<ExcelSheetConfigHistoriesVM> GetAllExcelSheetConfigHistoriesList(
            //List<long> childsUsersIds,
            //string? ExcelSheetConfigTitle = "",
            long? ExcelSheetConfigId = 0);

        List<ExcelSheetConfigHistoriesVM> GetListOfExcelSheetConfigHistories(int jtStartIndex,
              int jtPageSize,
              ref int listCount,
              //List<long> childsUsersIds,
              //string? ExcelSheetConfigTitle = "",
              long? ExcelSheetConfigId = 0,
              string jtSorting = null);

        long AddToExcelSheetConfigHistories(
            ExcelSheetConfigHistoriesVM excelSheetConfigHistoriesVM);

        long UpdateExcelSheetConfigHistories(
            //List<long> childsUsersIds,
            ExcelSheetConfigHistoriesVM excelSheetConfigHistoriesVM);

        bool ToggleActivationExcelSheetConfigHistories(long excelSheetConfigHistoryId,
            long userId
            //List<long> childsUsersIds)
            );

        bool TemporaryDeleteExcelSheetConfigHistories(long excelSheetConfigHistoryId,
            long userId
            //List<long> childsUsersIds)
            );

        bool CompleteDeleteExcelSheetConfigHistories(long excelSheetConfigHistoryId,
            long userId
            //List<long> childsUsersIds)
            );

        ExcelSheetConfigHistoriesVM GetExcelSheetConfigHistoriesWithExcelSheetConfigHistoryId(long excelSheetConfigHistoryId,
            long userId//,
                       //List<long> childsUsersIds)
            );
        #endregion

        #region Methods For Work With GoogleSheetConfigs

        List<GoogleSheetConfigsVM> GetAllGoogleSheetConfigsList(
            //List<long> childsUsersIds,
            //string? GoogleSheetConfigTitle = "",
            //long? ConstructionProjectId = 0
            );
        List<GoogleSheetConfigsVM> GetListOfGoogleSheetConfigs(int jtStartIndex,
              int jtPageSize,
              ref int listCount,
              //List<long> childsUsersIds,
              //string? GoogleSheetConfigTitle = "",
              //long? ConstructionProjectId = 0,
              string jtSorting = null);


        long AddToGoogleSheetConfigs(GoogleSheetConfigsVM googleSheetConfigsVM);

        long UpdateGoogleSheetConfigs(
            //List<long> childsUsersIds,
            GoogleSheetConfigsVM googleSheetConfigsVM);

        bool ToggleActivationGoogleSheetConfigs(long googleSheetConfigId
            , long userId
            );
        //List<long> childsUsersIds);

        bool TemporaryDeleteGoogleSheetConfigs(long googleSheetConfigId
            , long userId
            );
        //List<long> childsUsersIds);

        bool CompleteDeleteGoogleSheetConfigs(long googleSheetConfigId
            //,long userId
            );
        //List<long> childsUsersIds);

        GoogleSheetConfigsVM GetGoogleSheetConfigsWithGoogleSheetConfigId(long googleSheetConfigId
            //long userId
            );
        //List<long> childsUsersIds);
        #endregion

        #region Methods For Work With InitialPlans

        List<InitialPlansVM> GetAllInitialPlansList(
             List<long> childsUsersIds,
           string? InitialPlanTitle = "",
           long? ConstructionProjectId = 0);
        List<InitialPlansVM> GetListOfInitialPlans(int jtStartIndex,
             int jtPageSize,
             ref int listCount,
             List<long> childsUsersIds,
             string? InitialPlanTitle = "",
             long? ConstructionProjectId = 0,
             string jtSorting = null);

        long AddToInitialPlans(InitialPlansVM initialPlansVM);

        InitialPlansVM UpdateInitialPlans(
             List<long> childsUsersIds,
           InitialPlansVM initialPlansVM);

        bool ToggleActivationInitialPlans(long initialPlanId,
           long userId,
           List<long> childsUsersIds);

        bool TemporaryDeleteInitialPlans(long initialPlanId,
           long userId,
           List<long> childsUsersIds);

        bool? CompleteDeleteInitialPlans(long initialPlanId,
           long userId,
           List<long> childsUsersIds);

        InitialPlansVM GetInitialPlansWithInitialPlanId(long initialPlanId,
           long userId,
           List<long> childsUsersIds);
        #endregion

        #region Methods For Work With MeetingBoards

        List<MeetingBoardsVM> GetAllMeetingBoardsList(
             List<long> childsUsersIds,
           string? MeetingBoardTitle = "",
           long? ConstructionProjectId = 0);
        List<MeetingBoardsVM> GetListOfMeetingBoards(int jtStartIndex,
             int jtPageSize,
             ref int listCount,
             List<long> childsUsersIds,
             string? MeetingBoardTitle = "",
             long? ConstructionProjectId = 0,
             string jtSorting = null);


        long AddToMeetingBoards(MeetingBoardsVM meetingBoardsVM);

        MeetingBoardsVM UpdateMeetingBoards(
             List<long> childsUsersIds,
           MeetingBoardsVM meetingBoardsVM);

        bool ToggleActivationMeetingBoards(long meetingBoardId,
           long userId,
           List<long> childsUsersIds);

        bool TemporaryDeleteMeetingBoards(long meetingBoardId,
           long userId,
           List<long> childsUsersIds);

        bool? CompleteDeleteMeetingBoards(long meetingBoardId,
           long userId,
           List<long> childsUsersIds);

        MeetingBoardsVM GetMeetingBoardsWithMeetingBoardId(long meetingBoardId,
           long userId,
           List<long> childsUsersIds);
        #endregion

        #region Methods For Work With OperationsOnProperty

        #endregion

        #region Methods For Work With OperationOnPropertyProjects

        #endregion

        #region Methods For Work With PropertyProjects

        List<PropertyProjectsVM> GetAllPropertyProjectsList(ref int listCount,
           List<long> childsUsersIds,
           long? propertyId = null,
           int? propertyProjectTypeId = null,
           bool? isPrivate = null,
           string jtSorting = null);

        List<PropertyProjectsVM> GetListOfPropertyProjects(int jtStartIndex,
             int jtPageSize,
             ref int listCount,
             List<long> childsUsersIds,
             long? propertyId = null,
             int? propertyProjectTypeId = null,
             bool? isPrivate = null,
             string jtSorting = null);

        int AddToPropertyProjects(PropertyProjectsVM propertyProjectsVM);

        PropertyProjectsVM GetPropertyProjectWithPropertyProjectId(int propertyProjectId,
           List<long> childsUsersIds);
        int UpdatePropertyProjects(ref PropertyProjectsVM propertyProjectsVM,
           List<long> childsUsersIds);

        bool ToggleActivationPropertyProjects(int propertyProjectId,
           long userId,
           List<long> childsUsersIds);

        bool TemporaryDeletePropertyProjects(int propertyProjectId,
           long userId,
           List<long> childsUsersIds);

        bool CompleteDeletePropertyProjects(int propertyProjectId,
           List<long> childsUsersIds);

        #endregion

        #region Methods For Work With PropertyProjectTypes

        #endregion

        #region Methods For Work With PartnershipAgreements

        List<PartnershipAgreementsVM> GetAllPartnershipAgreementsList(
             List<long> childsUsersIds,
           string? PartnershipAgreementTitle = "",
           long? ConstructionProjectId = 0);


        List<PartnershipAgreementsVM> GetListOfPartnershipAgreements(int jtStartIndex,
             int jtPageSize,
             ref int listCount,
             List<long> childsUsersIds,
             long? userId,
             string? PartnershipAgreementTitle = "",
             long? ConstructionProjectId = 0,
             string jtSorting = null);

        long AddToPartnershipAgreements(PartnershipAgreementsVM partnershipAgreementsVM);

        PartnershipAgreementsVM UpdatePartnershipAgreements(
             List<long> childsUsersIds,
           PartnershipAgreementsVM partnershipAgreementsVM);

        bool ToggleActivationPartnershipAgreements(long partnershipAgreementId,
           long userId,
           List<long> childsUsersIds);

        bool TemporaryDeletePartnershipAgreements(long partnershipAgreementId,
           long userId,
           List<long> childsUsersIds);

        bool? CompleteDeletePartnershipAgreements(long partnershipAgreementId,
           long userId,
           List<long> childsUsersIds);

        PartnershipAgreementsVM GetPartnershipAgreementsWithPartnershipAgreementId(long partnershipAgreementId,
           long userId,
           List<long> childsUsersIds);

        #endregion

        #region Methods For Work With PitchDecks

        List<PitchDecksVM> GetAllPitchDecksList(
             List<long> childsUsersIds,
           string? PitchDeckTitle = "",
           long? ConstructionProjectId = 0);
        List<PitchDecksVM> GetListOfPitchDecks(int jtStartIndex,
             int jtPageSize,
             ref int listCount,
             List<long> childsUsersIds,
             string? PitchDeckTitle = "",
             long? ConstructionProjectId = 0,
             string jtSorting = null);


        long AddToPitchDecks(PitchDecksVM pitchDecksVM);

        PitchDecksVM UpdatePitchDecks(
             List<long> childsUsersIds,
           PitchDecksVM pitchDecksVM);

        bool ToggleActivationPitchDecks(long pitchDeckId,
           long userId,
           List<long> childsUsersIds);

        bool TemporaryDeletePitchDecks(long pitchDeckId,
           long userId,
           List<long> childsUsersIds);

        bool? CompleteDeletePitchDecks(long pitchDeckId,
           long userId,
           List<long> childsUsersIds);

        PitchDecksVM GetPitchDecksWithPitchDeckId(long pitchDeckId,
           long userId,
           List<long> childsUsersIds);
        #endregion

        #region Methods For Work With ProgressPictures

        List<ProgressPicturesVM> GetAllProgressPicturesList(
             List<long> childsUsersIds,
           string? ProgressPictureTitle = "",
           long? ConstructionProjectId = 0);
        List<ProgressPicturesVM> GetListOfProgressPictures(int jtStartIndex,
             int jtPageSize,
             ref int listCount,
             List<long> childsUsersIds,
             string? ProgressPictureTitle = "",
             long? ConstructionProjectId = 0,
             string jtSorting = null);

        long AddToProgressPictures(ProgressPicturesVM progressPicturesVM);

        ProgressPicturesVM UpdateProgressPictures(
             List<long> childsUsersIds,
           ProgressPicturesVM progressPicturesVM);

        bool ToggleActivationProgressPictures(long progressPictureId,
           long userId,
           List<long> childsUsersIds);

        bool TemporaryDeleteProgressPictures(long progressPictureId,
           long userId,
           List<long> childsUsersIds);

        bool? CompleteDeleteProgressPictures(long progressPictureId,
           long userId,
           List<long> childsUsersIds);

        ProgressPicturesVM GetProgressPicturesWithProgressPictureId(long progressPictureId,
           long userId,
           List<long> childsUsersIds);
        #endregion

        #region Methods For Work With TimeTables
        List<TimeTablesVM> GetAllTimeTablesList(
                List<long> childsUsersIds,
                long? constructionProjectId
            );
        List<TimeTablesVM> GetListOfTimeTables(int jtStartIndex,
              int jtPageSize,
              ref int listCount,
              List<long> childsUsersIds,
              long? constructionProjectId,
              string jtSorting = null);

        bool AddToTimeTables(
            TimeTablesVM timeTablesVM);
        bool UpdateTimeTables(
                List<long> childsUsersIds,
            TimeTablesVM timeTablesVM);
        bool ToggleActivationTimeTables(long timeTableId,
            long userId,
            List<long> childsUsersIds);
        bool TemporaryDeleteTimeTables(long timeTableId,
            long userId,
            List<long> childsUsersIds);
        bool CompleteDeleteTimeTables(long timeTableId,
            long userId,
            List<long> childsUsersIds,
            ref string returnMessage);

        #endregion

        #region Methods For Work With TimeTableItems
        List<TimeTableItemsVM> GetAllTimeTableItemsList(
                List<long> childsUsersIds,
                long? timeTableId,
                long? timeTableItemParentId,
                string? timeTableItemTitle = ""
            );
        List<TimeTableItemsVM> GetListOfTimeTableItems(int jtStartIndex,
              int jtPageSize,
              ref int listCount,
              List<long> childsUsersIds,
              string jtSorting = null,
              long? timeTableId = 0,
              long? timeTableItemParentId = 0,
              string? timeTableItemTitle = "");

        bool AddToTimeTableItems(
            TimeTableItemsVM timeTableItemsVM);
        bool UpdateTimeTableItems(
                List<long> childsUsersIds,
            TimeTableItemsVM timeTableItemsVM);
        bool ToggleActivationTimeTableItems(long timeTableItemId,
            long userId,
            List<long> childsUsersIds);
        bool TemporaryDeleteTimeTableItems(long timeTableItemId,
            long userId,
            List<long> childsUsersIds);
        string CompleteDeleteTimeTableItems(long timeTableItemId,
            long userId,
            List<long> childsUsersIds);

        #endregion

        #region Methods For Work With ConstructionProjectDailyPictures

        List<ConstructionProjectDailyPicturesVM> GetAllConstructionProjectDailyPicturesList(
                List<long> childsUsersIds,
                long? constructionProjectId);

        List<ConstructionProjectDailyPicturesVM> GetListOfConstructionProjectDailyPictures(int jtStartIndex,
              int jtPageSize,
              ref int listCount,
              List<long> childsUsersIds,
              string jtSorting = null,
              long? constructionProjectId = 0);

        List<ConstructionProjectDailyPicturesVM> GetListOfConstructionProjectDailyPicturesWithLastDate(int jtStartIndex,
              int jtPageSize,
              ref int listCount,
              List<long> childsUsersIds,
              string jtSorting = null,
              long? constructionProjectId = 0,
              DateTime? lastDate = null);

        #endregion

        #region Methods For Work With FileStates

        List<FileStatesVM> GetAllFileStatesList();

        #endregion

        #region Methods For Work With FileStatesLogs

        List<FileStatesLogsVM> GetAllFileStatesLogsList(
             List<long> childsUsersIds,
             string? TableTitle = "",
             long? RecordId = 0,
             int? FileStateId = 0);

        List<FileStatesLogsVM> GetListOfFileStatesLogs(int jtStartIndex,
             int jtPageSize,
             ref int listCount,
             List<long> childsUsersIds,
             string? TableTitle = "",
             long? RecordId = 0,
             int? FileStateId = 0,
             string jtSorting = null);

        long AddToFileStatesLogs(FileStatesLogsVM fileStatesLogsVM);

        FileStatesLogsVM UpdateFileStatesLogs(
            List<long> childsUsersIds,
            FileStatesLogsVM fileStatesLogsVM);

        bool ToggleActivationFileStatesLogs(long fileStateLogId,
           long userId,
           List<long> childsUsersIds);

        bool TemporaryDeleteFileStatesLogs(long fileStateLogId,
           long userId,
           List<long> childsUsersIds);

        bool? CompleteDeleteFileStatesLogs(long fileStateLogId,
           long userId,
           List<long> childsUsersIds);

        FileStatesLogsVM GetFileStateLogWithFileStateLogId(long fileStateLogId,
           long userId,
           List<long> childsUsersIds);

        #endregion

        #region Methods For Work With ConversationLogs

        List<ConversationLogsVM> GetAllConversationLogsList(
             List<long> childsUsersIds,
             string? ConversationLogTitle = "",
             string? ConversationLogDescription = "",
             string? TableTitle = "",
             long? RecordId = 0);

        List<ConversationLogsVM> GetListOfConversationLogs(int jtStartIndex,
             int jtPageSize,
             ref int listCount,
             List<long> childsUsersIds,
             string? ConversationLogTitle = "",
             string? ConversationLogDescription = "",
             string? TableTitle = "",
             long? RecordId = 0,
             string jtSorting = null);

        long AddToConversationLogs(ConversationLogsVM ConversationLogsVM);

        ConversationLogsVM UpdateConversationLogs(
            List<long> childsUsersIds,
            ConversationLogsVM ConversationLogsVM);

        bool ToggleActivationConversationLogs(long conversationLogId,
           long userId,
           List<long> childsUsersIds);

        bool TemporaryDeleteConversationLogs(long conversationLogId,
           long userId,
           List<long> childsUsersIds);

        bool? CompleteDeleteConversationLogs(long conversationLogId,
           long userId,
           List<long> childsUsersIds);

        ConversationLogsVM GetConversationLogWithConversationLogId(long conversationLogId,
           long userId,
           List<long> childsUsersIds);


        List<ConversationLogsVM> GetConversationDataByAgreementTypeAndRecordId(
         long recordId,
         string contractType,
         long userId);

        #endregion

        #region Methods For Work With ConstructionProjectFinancialData

        FinancialDetailsDataVM GetConstructionProjectFinancialDataByConstructionProjectId(
            long constructionProjectId,
            string type,
            long userId,
            long? ownerUserId = null);

        List<PaymentsAndCostsVM> GetPaymentsAndCostsList(
            long constructionProjectId,
            long userId,
            string? type = null,
            long? representativeId = null);

        List<SumOfPrivateCostsVM> GetSumOfPrivateCostsList(long constructionProjectId);

        List<SumOfPrivateCostsForRepresantativeVM> GetSumOfPrivateCostsListForRepresantative(
            long constructionProjectId,
            string? type = null,
            long? representativeId = null);
        List<SumOfPublicCostsVM> GetSumOfPublicCostsList(long constructionProjectId);


        List<SumOfPublicCostsRepresentativesVM> GetSumOfPublicCostsListForRepresentative(long constructionProjectId,
                    string? type = null,
                    long? representativeId = null);
        #endregion

        #region Methods For Work With ConstructionProjectProgressData

        List<ConstructionProjectProgressDataVM> GetProjectProgressDataVM(List<long> constructionProjectIds);

        List<ConstructionProjectProgressDataVM> GetAllProgressDataList(List<long> constructionProjectIds);

        #endregion

        #region Methods For Work With ConstructionProjectProgressItems

        List<HierarchyProjectProgressItemsVM> GetHierarchyOfAllProgressItemsList(List<long> childsUsersIds,
            long constructionProjectId,
             long? representativeId = null,
            string? type = "");

        #endregion

        #region Methods For Work With ConstructionProjectDelays


        List<ConstructionProjectDelaysVM> GetAllConstructionProjectDelays(
          List<long> childsUsersIds,
                int? constructionProjectBillDelayId = 0,
                long? ConstructionProjectId = 0);

        List<ConstructionProjectDelaysVM> GetListOfConstructionProjectDelays(int jtStartIndex,
                 int jtPageSize,
                 ref int listCount,
                 List<long> childsUsersIds,
                 int? constructionProjectBillDelayId = 0,
                 long? ConstructionProjectId = null,
                 string jtSorting = null);



        long AddToConstructionProjectDelays(List<long> childsUsersIds,
                ConstructionProjectDelaysVM constructionProjectDelaysVM);


        bool UpdateConstructionProjectDelays(List<long> childsUsersIds,
                ConstructionProjectDelaysVM constructionProjectDelaysVM);


        bool ToggleActivationConstructionProjectDelays(
               long ConstructionProjectsDelayId,
               long userId,
               List<long> childsUsersIds);



        bool TemporaryDeleteConstructionProjectDelays(
            long ConstructionProjectsDelayId,
                 long userId,
            List<long> childsUsersIds);


        bool? CompleteDeleteConstructionProjectDelays(
            long constructionProjectDelayId,
            long userId,
            List<long> childsUsersIds);




        ConstructionProjectDelays GetConstructionProjectDelayById(
            List<long> childsUsersIds,
            long? ConstructionProjectDelayId = null);




        #endregion

        #region Methods For Work With ConstructionProjectBillDelays

        List<ConstructionProjectBillDelaysVM> GetAllConstructionProjectBillDelaysList(List<long> childsUsersIds);

        long AddToConstructionProjectBillDelays(List<long> childsUsersIds,
ConstructionProjectBillDelaysVM constructionProjectBillDelaysVM);


        bool UpdateConstructionProjectBillDelays(List<long> childsUsersIds,
ConstructionProjectBillDelaysVM constructionProjectBillDelaysVM);


        List<ConstructionProjectBillDelaysForOuterDashboardVM> GetListOfBillDelaysForOuterDashboard(long? ConstructionProjectId, List<long> childsUsersIds);

        #endregion



        #region Methods For Work With TeniacoSuggestionFiles

        List<TeniacoSuggestionFilesVM> GetListOfTeniacoSuggestionFilesWithConstructionProjectId(long ConstructionProjectId,
            List<long> childsUsersIds);

        List<SuggestedConstructionProjectsVM> GetListOfTeniacoSuggestedProjects(
                 int pageNumber,
                 int pageSize,
                 List<long> childsUsersIds,
                 ITeniacoApiBusiness teniacoApiDb,
                 IPublicApiBusiness publicApiDb);

        long AddToTeniacoSuggestionFiles(
     List<TeniacoSuggestionFilesVM> teniacoSuggestionFilesVM,
     List<long> childsUsersIds,
     long userId,
     string? SuggestionPageTitle);



        long DeleteTeniacoSuggestionFile(
     long SuggestionFileId,
     List<long> childsUsersIds);



        long EditTeniacoSuggestionFile(
       List<TeniacoSuggestionFilesVM> teniacoSuggestionFilesVM,
       List<long> childsUsersIds);


        #endregion




        #endregion
    }
}
