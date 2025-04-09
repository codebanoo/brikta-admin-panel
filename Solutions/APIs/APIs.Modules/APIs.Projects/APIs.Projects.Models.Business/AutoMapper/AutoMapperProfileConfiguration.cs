using APIs.Projects.Models.Entities;
using VM.Projects;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIs.Projects.Models.Business.AutoMapper
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
        {
            #region Projects

            CreateMap<AttachementFiles, AttachementFilesVM>();
            CreateMap<AttachementFilesVM, AttachementFiles>();

            CreateMap<PropertyProjects, PropertyProjectsVM>();
            CreateMap<PropertyProjectsVM, PropertyProjects>();

            CreateMap<PropertyProjectTypes, PropertyProjectTypesVM>();
            CreateMap<PropertyProjectTypesVM, PropertyProjectTypes>();

            CreateMap<OperationsOnProperty, OperationsOnPropertyVM>();
            CreateMap<OperationsOnPropertyVM, OperationsOnProperty>();

            CreateMap<OperationOnPropertyProjects, OperationOnPropertyProjectsVM>();
            CreateMap<OperationOnPropertyProjectsVM, OperationOnPropertyProjects>();

            CreateMap<ContractAgreements, ContractAgreementsVM>();
            CreateMap<ContractAgreementsVM, ContractAgreements>();

            CreateMap<ConstructionProjects, ConstructionProjectsVM>();
            CreateMap<ConstructionProjectsVM, ConstructionProjects>();

            CreateMap<ConfirmationAgreements, ConfirmationAgreementsVM>();
            CreateMap<ConfirmationAgreementsVM, ConfirmationAgreements>();

            CreateMap<PartnershipAgreements, PartnershipAgreementsVM>();
            CreateMap<PartnershipAgreementsVM, PartnershipAgreements>();

            CreateMap<ProgressPictures, ProgressPicturesVM>();
            CreateMap<ProgressPicturesVM, ProgressPictures>();

            CreateMap<MeetingBoards, MeetingBoardsVM>();
            CreateMap<MeetingBoardsVM, MeetingBoards>();

            CreateMap<TimeTables, TimeTablesVM>();
            CreateMap<TimeTablesVM, TimeTables>();

            CreateMap<TimeTableItems, TimeTableItemsVM>();
            CreateMap<TimeTableItemsVM, TimeTableItems>();

            CreateMap<GoogleSheetConfigs, GoogleSheetConfigsVM>();
            CreateMap<GoogleSheetConfigsVM, GoogleSheetConfigs>();

            CreateMap<ExcelSheetConfigs, ExcelSheetConfigsVM>();
            CreateMap<ExcelSheetConfigsVM, ExcelSheetConfigs>();

            CreateMap<ExcelSheetConfigHistories, ExcelSheetConfigHistoriesVM>();
            CreateMap<ExcelSheetConfigHistoriesVM, ExcelSheetConfigHistories>();

            CreateMap<ConfirmationTypes, ConfirmationTypesVM>();
            CreateMap<ConfirmationTypesVM, ConfirmationTypes>();

            CreateMap<ContractorsAgreements, ContractorsAgreementsVM>();
            CreateMap<ContractorsAgreementsVM, ContractorsAgreements>();

            CreateMap<ContractSides, ContractSidesVM>();
            CreateMap<ContractSidesVM, ContractSides>();

            CreateMap<PitchDecks, PitchDecksVM>();
            CreateMap<PitchDecksVM, PitchDecks>();

            CreateMap<InitialPlans, InitialPlansVM>();
            CreateMap<InitialPlansVM, InitialPlans>();

            CreateMap<ConstructionProjectDailyData, ConstructionProjectDailyDataVM>();
            CreateMap<ConstructionProjectDailyDataVM, ConstructionProjectDailyData>();

            CreateMap<ConstructionProjectOwnerUsers, ConstructionProjectOwnerUsersVM>();
            CreateMap<ConstructionProjectOwnerUsersVM, ConstructionProjectOwnerUsers>();

            CreateMap<ConstructionProjectTypes, ConstructionProjectTypesVM>();
            CreateMap<ConstructionProjectTypesVM, ConstructionProjectTypes>();

            CreateMap<ConstructionProjectPriceHistories, ConstructionProjectPriceHistoriesVM>();
            CreateMap<ConstructionProjectPriceHistoriesVM, ConstructionProjectPriceHistories>();

            CreateMap<ConstructionProjectDailyPictures, ConstructionProjectDailyPicturesVM>();
            CreateMap<ConstructionProjectDailyPicturesVM, ConstructionProjectDailyPictures>();

            CreateMap<FileStates, FileStatesVM>();
            CreateMap<FileStatesVM, FileStates>();

            CreateMap<FileStatesLogs, FileStatesLogsVM>();
            CreateMap<FileStatesLogsVM, FileStatesLogs>();


            CreateMap<ConversationLogs, ConversationLogsVM>();
            CreateMap<ConversationLogsVM, ConversationLogs>();

            CreateMap<ConstructionProjectProgressData, ConstructionProjectProgressDataVM>();
            CreateMap<ConstructionProjectProgressDataVM, ConstructionProjectProgressData>();

            CreateMap<ConstructionProjectRepresentatives, ConstructionProjectRepresentativesVM>();
            CreateMap<ConstructionProjectRepresentativesVM, ConstructionProjectRepresentatives>();

            CreateMap<ConstructionProjectDelays, ConstructionProjectDelaysVM>();
            CreateMap<ConstructionProjectDelaysVM, ConstructionProjectDelays>();

            CreateMap<ConstructionProjectBillDelays, ConstructionProjectBillDelaysVM>();
            CreateMap<ConstructionProjectBillDelaysVM, ConstructionProjectBillDelays>();


            CreateMap<TeniacoSuggestionFiles, TeniacoSuggestionFilesVM>();
            CreateMap<TeniacoSuggestionFilesVM, TeniacoSuggestionFiles>();
            #endregion
        }
    }
}
