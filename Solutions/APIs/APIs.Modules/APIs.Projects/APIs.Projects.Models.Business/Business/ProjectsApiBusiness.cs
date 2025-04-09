using APIs.Projects.Models.Entities;
using APIs.Public.Models.Business;
using APIs.Teniaco.Models.Business;
using AutoMapper;
using Castle.Core.Internal;
using FrameWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Models.Business.ConsoleBusiness;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VM.Projects;
using VM.PVM.Projects;
using VM.Teniaco;

namespace APIs.Projects.Models.Business
{
    public class ProjectsApiBusiness : IProjectsApiBusiness, IDisposable
    {
        private ProjectsApiContext projectsApiDb = new ProjectsApiContext();

        private IMapper _mapper;

        private IHostEnvironment hostingEnvironment;

        public ProjectsApiContext ProjectsApiDb
        {
            get { return this.projectsApiDb; }
            set { }
            //private set { }
        }

        public void Dispose()
        {
            projectsApiDb.Dispose();
        }

        public ProjectsApiBusiness(IMapper mapper,
            ProjectsApiContext _projectsApiDb,
            IHostEnvironment _hostingEnvironment)
        {
            try
            {
                _mapper = mapper;

                projectsApiDb = _projectsApiDb;

                ProjectsApiDb = projectsApiDb;

                hostingEnvironment = _hostingEnvironment;
            }
            catch (Exception exc)
            {
            }
        }

        #region Projects

        #region Methods For Work With AttachementFiles

        public List<AttachementFilesVM> GetAllAttachementFilesList(
              List<long> childsUsersIds,
              long? attachementParentId = 0,
              string? AttachementTableTitle = "")
        {

            List<AttachementFilesVM> attachementFilesVMList = new List<AttachementFilesVM>();

            try
            {
                var list = (from p in projectsApiDb.AttachementFiles
                            where childsUsersIds.Contains(p.UserIdCreator.Value) &&
                           p.IsDeleted.Value.Equals(false) &&
                           p.IsActivated.Value.Equals(true)
                            select new AttachementFilesVM
                            {
                                AttachementId = p.AttachementId,
                                AttachementTitle = p.AttachementTitle,
                                AttachementParentId = p.AttachementParentId,
                                AttachementTableTitle = p.AttachementTableTitle,
                                AttachementDescription = p.AttachementDescription,
                                AttachementFilePath = p.AttachementFilePath,
                                AttachementFileExt = p.AttachementFileExt,
                                AttachementFileOrder = p.AttachementFileOrder,
                                AttachementFileType = p.AttachementFileType,
                                //IsConfirm = p.IsConfirm,
                                //UserIdConfirmation = p.UserIdConfirmation,
                                //ConfirmationDate = p.ConfirmationDate,
                                //ConfirmationTime = p.ConfirmationTime,
                                //IsView = p.IsView,
                                //UserIdViewer = p.UserIdViewer,
                                //ViewDate = p.ViewDate,
                                //ViewTime = p.ViewTime,
                                //IsSend = p.IsSend,
                                //UserIdSender = p.UserIdSender,
                                //SendDate = p.SendDate,
                                //SendTime = p.SendTime,
                                UserIdCreator = p.UserIdCreator.Value,
                                CreateEnDate = p.CreateEnDate,
                                CreateTime = p.CreateTime,
                                EditEnDate = p.EditEnDate,
                                EditTime = p.EditTime,
                                UserIdEditor = p.UserIdEditor.Value,
                                RemoveEnDate = p.RemoveEnDate,
                                RemoveTime = p.EditTime,
                                UserIdRemover = p.UserIdRemover.Value,
                                IsActivated = p.IsActivated,
                                IsDeleted = p.IsDeleted,

                            })
                               .AsEnumerable();

                if (attachementParentId.HasValue && attachementParentId.Value != 0)
                {
                    list = list.Where(a => a.AttachementParentId == attachementParentId);
                }

                if (!string.IsNullOrEmpty(AttachementTableTitle))
                    list = list.Where(a => a.AttachementTableTitle.Equals(AttachementTableTitle));
                attachementFilesVMList = list.OrderByDescending(s => s.AttachementId).ToList();
            }
            catch (Exception exc)
            { }
            return attachementFilesVMList;
        }
        public List<AttachementFilesVM> GetListOfAttachementFiles(int jtStartIndex,
              int jtPageSize,
              ref int listCount,
              List<long> childsUsersIds,
              string? AttachementFileTitle = "",
              long? attachementParentId = 0,
              string jtSorting = null)
        {

            List<AttachementFilesVM> attachementFilesVMList = new List<AttachementFilesVM>();

            try
            {
                var list = (from p in projectsApiDb.AttachementFiles
                            where childsUsersIds.Contains(p.UserIdCreator.Value)
                            select new AttachementFilesVM
                            {
                                AttachementId = p.AttachementId,
                                AttachementTitle = p.AttachementTitle,
                                AttachementParentId = p.AttachementParentId,
                                AttachementTableTitle = p.AttachementTableTitle,
                                AttachementDescription = p.AttachementDescription,
                                AttachementFilePath = p.AttachementFilePath,
                                AttachementFileExt = p.AttachementFileExt,
                                AttachementFileOrder = p.AttachementFileOrder,
                                AttachementFileType = p.AttachementFileType,
                                //IsConfirm = p.IsConfirm,
                                //UserIdConfirmation = p.UserIdConfirmation,
                                //ConfirmationDate = p.ConfirmationDate,
                                //ConfirmationTime = p.ConfirmationTime,
                                //IsView = p.IsView,
                                //UserIdViewer = p.UserIdViewer,
                                //ViewDate = p.ViewDate,
                                //ViewTime = p.ViewTime,
                                //IsSend = p.IsSend,
                                //UserIdSender = p.UserIdSender,
                                //SendDate = p.SendDate,
                                //SendTime = p.SendTime,
                                UserIdCreator = p.UserIdCreator.Value,
                                CreateEnDate = p.CreateEnDate,
                                CreateTime = p.CreateTime,
                                EditEnDate = p.EditEnDate,
                                EditTime = p.EditTime,
                                UserIdEditor = p.UserIdEditor.Value,
                                RemoveEnDate = p.RemoveEnDate,
                                RemoveTime = p.EditTime,
                                UserIdRemover = p.UserIdRemover.Value,
                                IsActivated = p.IsActivated,
                                IsDeleted = p.IsDeleted,

                            })
                               .AsEnumerable();




                if (attachementParentId.HasValue && attachementParentId.Value != 0)
                {
                    list = list.Where(a => a.AttachementParentId == attachementParentId);
                }

                if (!string.IsNullOrEmpty(AttachementFileTitle))
                    list = list.Where(a => a.AttachementTableTitle.Equals(AttachementFileTitle));



                listCount = list.Count();
                try
                {
                    if (string.IsNullOrEmpty(jtSorting))
                    {

                        if (listCount > jtPageSize)
                        {

                            attachementFilesVMList = list.OrderByDescending(s => s.AttachementId)
                                     .Skip(jtStartIndex).Take(jtPageSize).ToList();
                        }
                        else
                            attachementFilesVMList = list.OrderByDescending(s => s.AttachementId).ToList();
                    }
                    else
                    {


                        switch (jtSorting)
                        {
                            case "AttachementTitle ASC":
                                list = list.OrderBy(l => l.AttachementTitle);
                                break;
                            case "AttachementTitle DESC":
                                list = list.OrderByDescending(l => l.AttachementTitle);
                                break;
                        }
                        if (listCount > jtPageSize)
                        {
                            attachementFilesVMList = list.Skip(jtStartIndex).Take(jtPageSize).ToList();
                        }
                        else
                        {

                            attachementFilesVMList = list.ToList();
                        }
                    }
                }
                catch (Exception exc)
                { }


                foreach (var item in attachementFilesVMList)
                {
                    var constructionProjectId = (long)0;
                    if (AttachementFileTitle.Equals("MeetingBoards"))
                    {

                        constructionProjectId = projectsApiDb.MeetingBoards.Where(c => c.MeetingBoardId.Equals(attachementParentId)).Select(c => c.ConstructionProjectId).FirstOrDefault();
                        item.ConstructionProjectId = constructionProjectId;
                    }
                    else if (AttachementFileTitle.Equals("ConfirmationAgreement"))
                    {
                        constructionProjectId = projectsApiDb.ConfirmationAgreements.Where(c => c.ConfirmationAgreementId.Equals(attachementParentId)).Select(c => c.ConstructionProjectId).FirstOrDefault();
                        item.ConstructionProjectId = constructionProjectId;
                    }
                    else if (AttachementFileTitle.Equals("ContractAgreement"))
                    {
                        constructionProjectId = projectsApiDb.ContractAgreements.Where(c => c.ContractAgreementId.Equals(attachementParentId)).Select(c => c.ConstructionProjectId).FirstOrDefault();
                        item.ConstructionProjectId = constructionProjectId;
                    }
                    else if (AttachementFileTitle.Equals("InitialPlan"))
                    {
                        constructionProjectId = projectsApiDb.InitialPlans.Where(c => c.InitialPlanId.Equals(attachementParentId)).Select(c => c.ConstructionProjectId).FirstOrDefault();
                        item.ConstructionProjectId = constructionProjectId;
                    }


                }
            }
            catch (Exception exc)
            { }


            return attachementFilesVMList;
        }


        public long AddToAttachementFiles(AttachementFilesVM attachementFilesVM)
        {
            try
            {

                AttachementFiles attachementFile = _mapper.Map<AttachementFilesVM, AttachementFiles>(attachementFilesVM);


                projectsApiDb.AttachementFiles.Add(attachementFile);
                projectsApiDb.SaveChanges();
                return attachementFile.AttachementId;
            }
            catch (Exception)
            {
            }
            return 0;

        }

        public AttachementFilesVM UpdateAttachementFiles(
              List<long> childsUsersIds,
            AttachementFilesVM attachementFilesVM)
        {
            var AttachementFileId = attachementFilesVM.AttachementId;
            if (projectsApiDb.AttachementFiles.Where(x => childsUsersIds.Contains(x.UserIdCreator.Value))
                .Where(x => x.AttachementId.Equals(AttachementFileId)).Any())
            {
                try
                {
                    bool? isActivated = attachementFilesVM.IsActivated.HasValue ? attachementFilesVM.IsActivated.Value : (bool?)true;
                    bool? isDeleted = attachementFilesVM.IsDeleted.HasValue ? attachementFilesVM.IsDeleted.Value : (bool?)true;

                    AttachementFiles attachementFile = projectsApiDb.AttachementFiles.Where(a => a.AttachementId == AttachementFileId).FirstOrDefault();
                    attachementFile.AttachementTitle = attachementFilesVM.AttachementTitle;
                    //attachementFile.AttachementFileFileExt = attachementFilesVM.AttachementFileFileExt;
                    //attachementFile.AttachementFileFilePath = attachementFilesVM.AttachementFileFilePath;
                    attachementFile.AttachementFileOrder = attachementFilesVM.AttachementFileOrder;
                    //attachementFile.AttachementFileNumber = attachementFilesVM.AttachementFileNumber.Value;
                    attachementFile.AttachementDescription = attachementFilesVM.AttachementDescription;
                    attachementFile.EditTime = attachementFilesVM.EditTime;
                    attachementFile.EditEnDate = attachementFilesVM.EditEnDate;
                    attachementFile.UserIdEditor = attachementFilesVM.UserIdEditor;
                    attachementFile.IsActivated = isActivated;
                    attachementFile.IsDeleted = isDeleted;

                    projectsApiDb.Entry<AttachementFiles>(attachementFile).State = EntityState.Modified;

                    projectsApiDb.SaveChanges();
                    return _mapper.Map<AttachementFiles, AttachementFilesVM>(attachementFile);
                }
                catch (Exception)
                {

                }
            }
            return null;
        }

        public bool ToggleActivationAttachementFiles(long attachementFileId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var attachementFiles = (from c in projectsApiDb.AttachementFiles
                                        where c.AttachementId == attachementFileId &&
                                        childsUsersIds.Contains(c.UserIdCreator.Value)
                                        select c).FirstOrDefault();

                if (attachementFiles != null)
                {
                    attachementFiles.IsActivated = !attachementFiles.IsActivated;
                    attachementFiles.EditEnDate = DateTime.Now;
                    attachementFiles.EditTime = PersianDate.TimeNow;
                    attachementFiles.UserIdEditor = userId;

                    projectsApiDb.Entry<Entities.AttachementFiles>(attachementFiles).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool TemporaryDeleteAttachementFiles(long attachementFileId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var attachementFiles = (from c in projectsApiDb.AttachementFiles
                                        where c.AttachementId == attachementFileId &&
                                        childsUsersIds.Contains(c.UserIdCreator.Value)
                                        select c).FirstOrDefault();

                if (attachementFiles != null)
                {
                    attachementFiles.IsDeleted = !attachementFiles.IsDeleted;
                    attachementFiles.EditEnDate = DateTime.Now;
                    attachementFiles.EditTime = PersianDate.TimeNow;
                    attachementFiles.UserIdEditor = userId;

                    projectsApiDb.Entry<Entities.AttachementFiles>(attachementFiles).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool CompleteDeleteAttachementFiles(long attachementFileId,
            long userId,
            List<long> childsUsersIds)
        {

            try
            {
                var attachementFile = (from c in projectsApiDb.AttachementFiles
                                       where c.AttachementId == attachementFileId &&
                                       childsUsersIds.Contains(c.UserIdCreator.Value)
                                       select c).FirstOrDefault();

                if (attachementFile != null)
                {
                    projectsApiDb.AttachementFiles.Remove(attachementFile);
                    projectsApiDb.SaveChanges();


                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;


        }

        public AttachementFilesVM GetAttachementFilesWithAttachementFileId(long attachementFileId,
            long userId,
            List<long> childsUsersIds)
        {
            AttachementFilesVM attachementFilesVM = new AttachementFilesVM();

            try
            {
                attachementFilesVM = _mapper.Map<AttachementFiles,
                    AttachementFilesVM>(projectsApiDb.AttachementFiles
                    .Where(p => childsUsersIds.Contains(p.UserIdCreator.Value))
                     .Where(e => e.AttachementId.Equals(attachementFileId)).FirstOrDefault());

            }
            catch (Exception exc)
            { }

            return attachementFilesVM;
        }




        public List<AttachementFilesVM> GetAttachementsByAgreementIdAndTableTitle(
               long agreeemntId,
                string tableTittle,
                long userId
            )
        {
            List<AttachementFilesVM> attachments = new List<AttachementFilesVM>();
            try
            {
                List<FileStateLogStatusVM> AttachmentFileState = new List<FileStateLogStatusVM>();
                attachments = (from attachment in projectsApiDb.AttachementFiles
                               where agreeemntId.Equals(attachment.AttachementParentId.Value)
                               && attachment.AttachementTableTitle.Contains(tableTittle)
                               && attachment.IsDeleted.Value.Equals(false)
                               && attachment.IsActivated.Value.Equals(true)
                               select new AttachementFilesVM
                               {
                                   AttachementDescription = attachment.AttachementDescription,
                                   AttachementFileExt = attachment.AttachementFileExt,
                                   AttachementFileOrder = attachment.AttachementFileOrder,
                                   AttachementFilePath = attachment.AttachementFilePath,
                                   AttachementFileType = attachment.AttachementFileType,
                                   AttachementId = attachment.AttachementId,
                                   AttachementParentId = attachment.AttachementParentId,
                                   AttachementTableTitle = attachment.AttachementTableTitle,
                                   AttachementTitle = attachment.AttachementTitle,
                               }).OrderBy(s => s.AttachementId).ToList();
                if (attachments != null)
                {
                    if (attachments.Count > 0)
                    {
                        var attachmentIds = attachments.Select(x => x.AttachementId).ToList();
                        AttachmentFileState = projectsApiDb.FileStatesLogs
                        .Where(x => attachmentIds.Contains(x.RecordId) &&
                                    x.TableTitle.Equals("Attachments") &&
                                    (x.FileStateId == 3 || x.FileStateId == 4))
                        .GroupBy(x => x.RecordId)
                        .Select(group => new FileStateLogStatusVM
                        {
                            AgreementId = group.Key,
                            IsView = group.Any(x => x.FileStateId == 3),
                            IsConfirm = group.Any(x => x.FileStateId == 4)
                        })
                        .ToList();
                        /// 
                        foreach (var attachmentItem in attachments)
                        {
                            // Attachment fileStates ->
                            if (AttachmentFileState.Where(ad => ad.AgreementId == attachmentItem.AttachementId).Any())
                            {
                                try
                                {
                                    attachmentItem.IsView = AttachmentFileState.Any(x => x.AgreementId == attachmentItem.AttachementId && x.IsView);
                                    attachmentItem.IsConfirm = AttachmentFileState.Any(x => x.AgreementId == attachmentItem.AttachementId && x.IsConfirm);
                                }
                                catch (Exception ex)
                                { }
                            }
                            else
                            {
                                attachmentItem.IsView = null;
                                attachmentItem.IsConfirm = null;
                            }
                            // Attachment Is Read Count ->
                            attachmentItem.ConversationIsReadCount = projectsApiDb.ConversationLogs.Where(x => x.RecordId.Equals(attachmentItem.AttachementId) && x.IsRead == false && x.UserIdCreator != userId && x.TableTitle.Contains("Attachment")).Count();
                        }
                    }
                }
            }
            catch (Exception exc)
            { }
            return attachments;
        }

        #endregion

        #region Methods For Work With ConstructionProjects

        public List<ConstructionProjectsVM> GetAllConstructionProjectsList(
                List<long> childsUsersIds,
            string? constructionProjectTitle = "")
        {


            List<ConstructionProjectsVM> constructionProjectsVMList = new List<ConstructionProjectsVM>();

            try
            {
                var list = (from c in projectsApiDb.ConstructionProjects
                            where childsUsersIds.Contains(c.UserIdCreator.Value) &&
                                c.IsActivated.Value.Equals(true) &&
                                c.IsDeleted.Value.Equals(false)
                            select new ConstructionProjectsVM
                            {
                                ConstructionProjectId = c.ConstructionProjectId,
                                ConstructionProjectDescription = c.ConstructionProjectDescription,
                                ConstructionProjectTitle = c.ConstructionProjectTitle,
                                WorkshopName = c.WorkshopName,
                                UserId = c.UserId,
                                PropertyId = c.PropertyId,
                                ConstructionProjectTypeId = c.ConstructionProjectTypeId,
                                UserIdCreator = c.UserIdCreator.Value,
                                CreateEnDate = c.CreateEnDate,
                                CreateTime = c.CreateTime,
                                EditEnDate = c.EditEnDate,
                                EditTime = c.EditTime,
                                UserIdEditor = c.UserIdEditor.Value,
                                RemoveEnDate = c.RemoveEnDate,
                                RemoveTime = c.EditTime,
                                UserIdRemover = c.UserIdRemover.Value,
                                IsActivated = c.IsActivated,
                                IsDeleted = c.IsDeleted,
                                Foundation = c.Foundation,
                                StartDateEn = c.StartDateEn,
                                MonthsLeftUntilTheEnd = c.MonthsLeftUntilTheEnd,
                                ConstructionProjectOwnerUsersVM = new List<ConstructionProjectOwnerUsersVM>(),
                            }).AsEnumerable();


                if (!string.IsNullOrEmpty(constructionProjectTitle))
                    list = list.Where(a => a.ConstructionProjectTitle.Contains(constructionProjectTitle));

                constructionProjectsVMList = list.OrderByDescending(s => s.ConstructionProjectId).ToList();

            }
            catch (Exception exc)
            { }
            return constructionProjectsVMList;
        }

        public List<ConstructionProjectsVM> GetListOfConstructionProjects(int jtStartIndex,
                     int jtPageSize,
                     ref int listCount,
                     List<long> childsUsersIds,
                     string? ConstructionProjectTitle = "",
                     string jtSorting = null)
        {

            List<ConstructionProjectsVM> constructionProjectsVMList = new List<ConstructionProjectsVM>();

            try
            {
                var list = (from c in projectsApiDb.ConstructionProjects
                            where childsUsersIds.Contains(c.UserIdCreator.Value)
                            select new ConstructionProjectsVM
                            {
                                ConstructionProjectId = c.ConstructionProjectId,
                                ConstructionProjectDescription = c.ConstructionProjectDescription,
                                ConstructionProjectTitle = c.ConstructionProjectTitle,
                                UserId = c.UserId,
                                PropertyId = c.PropertyId,
                                ConstructionProjectTypeId = c.ConstructionProjectTypeId,
                                UserIdCreator = c.UserIdCreator.Value,
                                CreateEnDate = c.CreateEnDate,
                                CreateTime = c.CreateTime,
                                Foundation = c.Foundation,
                                WorkshopName = c.WorkshopName,
                                StartDateEn = c.StartDateEn,
                                MonthsLeftUntilTheEnd = c.MonthsLeftUntilTheEnd,
                                ShowInDashboard = c.ShowInDashboard,
                                IsActivated = c.IsActivated,
                                IsDeleted = c.IsDeleted,
                                ConstructionProjectOwnerUsersVM = new List<ConstructionProjectOwnerUsersVM>(),
                            }).AsEnumerable();



                if (!string.IsNullOrEmpty(ConstructionProjectTitle))
                    list = list.Where(a => a.ConstructionProjectTitle.Contains(ConstructionProjectTitle));

                listCount = list.Count();

                try
                {
                    if (string.IsNullOrEmpty(jtSorting))
                    {

                        if (listCount > jtPageSize)
                        {

                            constructionProjectsVMList = list.OrderByDescending(s => s.ConstructionProjectId)
                                     .Skip(jtStartIndex).Take(jtPageSize).ToList();
                        }
                        else
                            constructionProjectsVMList = list.OrderByDescending(s => s.ConstructionProjectId).ToList();
                    }
                    else
                    {


                        switch (jtSorting)
                        {
                            case "ConstructionProjectTitle ASC":
                                list = list.OrderBy(l => l.ConstructionProjectTitle);
                                break;
                            case "ConstructionProjectTitle DESC":
                                list = list.OrderByDescending(l => l.ConstructionProjectTitle);
                                break;
                        }
                        if (listCount > jtPageSize)
                        {
                            constructionProjectsVMList = list.Skip(jtStartIndex).Take(jtPageSize).ToList();
                        }
                        else
                        {

                            constructionProjectsVMList = list.ToList();
                        }
                    }
                }
                catch (Exception exc)
                { }
            }
            catch (Exception exc)
            { }
            return constructionProjectsVMList;
        }

        public long AddToConstructionProjects(
              ConstructionProjectsVM constructionProjectsVM)
        {
            using (var transaction = projectsApiDb.Database.BeginTransaction())
            {
                try
                {
                    //List<ContractSides> contractSideList = new List<ContractSides>();


                    #region ConstructionProjects
                    ConstructionProjects constructionProject = _mapper.Map<ConstructionProjectsVM, ConstructionProjects>(constructionProjectsVM);


                    constructionProject.IsActivated = true;
                    constructionProject.IsDeleted = false;

                    projectsApiDb.ConstructionProjects.Add(constructionProject);
                    projectsApiDb.SaveChanges();


                    #endregion


                    if (constructionProjectsVM.ConstructionProjectOwnerUsersVM != null)
                    {
                        #region ContractSides


                        //contractSideList.Add(new ContractSides
                        //{
                        //    ContractSideTypeId = 1,
                        //    ParentId = constructionProject.UserId,
                        //    TableRecordId = constructionProject.ConstructionProjectId,
                        //    TableTitle = "ConstructionProjects",
                        //    UserIdCreator = constructionProject.UserIdCreator,
                        //    CreateEnDate = constructionProject.CreateEnDate,
                        //    CreateTime = constructionProject.CreateTime,
                        //    IsActivated = true,
                        //    IsDeleted = false,
                        //});

                        //projectsApiDb.ContractSides.AddRange(contractSideList);
                        //projectsApiDb.SaveChanges();


                        #endregion

                        #region ConstructionProjectOwnerUsers 
                        //سهامداران

                        var newConstructionProjectOwnerPersonsList = _mapper.Map<List<ConstructionProjectOwnerUsersVM>,
                                  List<ConstructionProjectOwnerUsers>>(constructionProjectsVM.ConstructionProjectOwnerUsersVM);

                        foreach (var item in newConstructionProjectOwnerPersonsList)
                        {
                            item.ConstructionProjectId = constructionProject.ConstructionProjectId;
                            item.IsActivated = true;
                            item.IsDeleted = false;


                        }

                        projectsApiDb.ConstructionProjectOwnerUsers.AddRange(newConstructionProjectOwnerPersonsList);
                        projectsApiDb.SaveChanges();

                        #endregion

                        #region ConstructionProjectRepresentatives
                        //نمایندگان

                        var newConstructionProjectRepresentativesList = _mapper.Map<List<ConstructionProjectRepresentativesVM>,
                        List<ConstructionProjectRepresentatives>>(constructionProjectsVM.ConstructionProjectRepresentativesVM);


                        if (newConstructionProjectRepresentativesList != null)
                        {

                            foreach (var item in newConstructionProjectRepresentativesList)
                            {

                                if (!item.RepresentativeId.Equals(0))
                                {
                                    item.ConstructionProjectId = constructionProject.ConstructionProjectId;
                                    item.IsActivated = true;
                                    item.IsDeleted = false;
                                }
                            }


                            projectsApiDb.ConstructionProjectRepresentatives.AddRange(newConstructionProjectRepresentativesList);
                            projectsApiDb.SaveChanges();

                        }


                        #endregion

                        #region ConstructionProjectPriceHistories

                        ConstructionProjectPriceHistories constructionProjectHistories = new ConstructionProjectPriceHistories();

                        constructionProjectHistories.CurrentValueOfProject = constructionProjectsVM.CurrentValueOfProject.HasValue ? constructionProjectsVM.CurrentValueOfProject.Value : 0;
                        constructionProjectHistories.ProjectEstimate = constructionProjectsVM.StrProjectEstimate != "" ? long.Parse((constructionProjectsVM.StrProjectEstimate.Replace(",", ""))) : 0;
                        constructionProjectHistories.PrevisionOfCost = constructionProjectsVM.StrPrevisionOfCost != "" ? long.Parse((constructionProjectsVM.StrPrevisionOfCost.Replace(",", ""))) : 0;

                        //constructionProjectHistories.CurrentValueOfProject = constructionProjectsVM.CurrentValueOfProject.HasValue ? constructionProjectsVM.CurrentValueOfProject.Value : 0;
                        //constructionProjectHistories.ProjectEstimate = constructionProjectsVM.ProjectEstimate.HasValue ? constructionProjectsVM.ProjectEstimate.Value : 0;
                        ///constructionProjectHistories.PrevisionOfCost = constructionProjectsVM.PrevisionOfCost.HasValue ? constructionProjectsVM.PrevisionOfCost.Value : 0;
                        constructionProjectHistories.ConstructionProjectId = constructionProject.ConstructionProjectId;
                        constructionProjectHistories.PriceTypeRegister = 0;

                        constructionProjectHistories.CreateEnDate = constructionProject.CreateEnDate.Value;
                        constructionProjectHistories.CreateTime = constructionProject.CreateTime;
                        constructionProjectHistories.UserIdCreator = constructionProject.UserIdCreator.Value;
                        constructionProjectHistories.IsActivated = true;
                        constructionProjectHistories.IsDeleted = false;

                        projectsApiDb.ConstructionProjectPriceHistories.Add(constructionProjectHistories);
                        projectsApiDb.SaveChanges();


                        #endregion


                        //projectsApiDb.SaveChanges();
                    }

                    transaction.Commit();

                    return constructionProject.ConstructionProjectId;
                }
                catch (Exception exc)
                {
                    transaction.Rollback();

                }
            }
            return 0;

        }

        public long UpdateConstructionProjects(
           List<long> childsUsersIds,
           ConstructionProjectsVM constructionProjectsVM)
        {
            var constructionProjectId = constructionProjectsVM.ConstructionProjectId;

            using (var transaction = projectsApiDb.Database.BeginTransaction())
            {

                var record = projectsApiDb.ConstructionProjects.Where(x => x.ConstructionProjectId.Equals(constructionProjectId)).AsQueryable();

                #region childsUsersIds

                if (childsUsersIds != null)
                {
                    if (childsUsersIds.Count > 1)
                    {
                        record = record.Where(c => childsUsersIds.Contains(c.UserIdCreator.Value));
                    }
                    else
                    {
                        if (childsUsersIds.Count == 1)
                        {
                            if (childsUsersIds.FirstOrDefault() > 0)
                            {
                                record = record.Where(c => childsUsersIds.Contains(c.UserIdCreator.Value));
                            }
                        }
                    }
                }
                #endregion

                ConstructionProjects constructionProject = record.FirstOrDefault();

                if (constructionProject != null)

                {
                    if (constructionProject.ConstructionProjectId > 0)
                    {
                        try
                        {
                            List<ContractSides> contractSideList = new List<ContractSides>();


                            #region ConstructionProjects

                            bool? isActivated = constructionProjectsVM.IsActivated.HasValue ? constructionProjectsVM.IsActivated.Value : (bool?)true;
                            bool? isDeleted = constructionProjectsVM.IsDeleted.HasValue ? constructionProjectsVM.IsDeleted.Value : (bool?)false;

                            //constructionProject = projectsApiDb.ConstructionProjects.Where(a => a.ConstructionProjectId == constructionProjectId).FirstOrDefault();
                            constructionProject.ConstructionProjectTitle = constructionProjectsVM.ConstructionProjectTitle;
                            constructionProject.ConstructionProjectTypeId = constructionProjectsVM.ConstructionProjectTypeId;
                            constructionProject.WorkshopName = constructionProjectsVM.WorkshopName;
                            constructionProject.PropertyId = constructionProjectsVM.PropertyId;
                            constructionProject.ConstructionProjectDescription = constructionProjectsVM.ConstructionProjectDescription;
                            constructionProject.UserId = constructionProjectsVM.UserId;
                            constructionProject.StartDateEn = constructionProjectsVM.StartDateEn;
                            constructionProject.MonthsLeftUntilTheEnd = constructionProjectsVM.MonthsLeftUntilTheEnd;
                            constructionProject.Foundation = constructionProjectsVM.Foundation;


                            constructionProject.EditTime = constructionProjectsVM.EditTime;
                            constructionProject.EditEnDate = constructionProjectsVM.EditEnDate;
                            constructionProject.UserIdEditor = constructionProjectsVM.UserIdEditor;
                            constructionProject.IsActivated = isActivated;
                            constructionProject.IsDeleted = isDeleted;

                            projectsApiDb.Entry<ConstructionProjects>(constructionProject).State = EntityState.Modified;
                            projectsApiDb.SaveChanges();

                            #endregion

                            #region ConstructionProjectOwnerUsers
                            if (projectsApiDb.ConstructionProjectOwnerUsers.Where(o => o.ConstructionProjectId.Equals(constructionProjectId)).Any())
                            {
                                #region remove old ConstructionProjectOwnerUsers

                                var oldConstructionProjectOwnerUsersList = projectsApiDb.ConstructionProjectOwnerUsers.Where(o => o.ConstructionProjectId.Equals(constructionProjectId)).ToList();
                                projectsApiDb.ConstructionProjectOwnerUsers.RemoveRange(oldConstructionProjectOwnerUsersList);
                                projectsApiDb.SaveChanges();

                                #endregion

                                #region add new ConstructionProjectOwnerUsers

                                if (constructionProjectsVM.ConstructionProjectOwnerUsersVM != null)
                                {
                                    #region old codes - comments

                                    //if (projectsApiDb.ContractSides.Where(p => p.TableTitle == "ConstructionProjects" && p.TableRecordId == constructionProjectId && p.ContractSideTypeId == 1).Any())
                                    //{
                                    //    var contractSide = projectsApiDb.ContractSides.Where(p => p.TableTitle == "ConstructionProjects" && p.TableRecordId == constructionProjectId && p.ContractSideTypeId == 1).ToList();
                                    //    projectsApiDb.ContractSides.RemoveRange(contractSide);
                                    //    projectsApiDb.SaveChanges();
                                    //}

                                    #endregion


                                    var newConstructionProjectOwnerPersonsList = _mapper.Map<List<ConstructionProjectOwnerUsersVM>,
                                        List<ConstructionProjectOwnerUsers>>(constructionProjectsVM.ConstructionProjectOwnerUsersVM);


                                    foreach (var item in newConstructionProjectOwnerPersonsList)
                                    {
                                        item.ConstructionProjectId = constructionProject.ConstructionProjectId;
                                        item.IsActivated = true;
                                        item.IsDeleted = false;

                                        #region old codes - comments

                                        //contractSideList.Add(new ContractSides
                                        //{
                                        //    ContractSideTypeId = 1,
                                        //    ParentId = item.OwnerUserId,
                                        //    TableRecordId = constructionProject.ConstructionProjectId,
                                        //    TableTitle = "ConstructionProjects",
                                        //    UserIdCreator = constructionProject.UserIdCreator,
                                        //    CreateEnDate = constructionProject.CreateEnDate,
                                        //    CreateTime = constructionProject.CreateTime,
                                        //    IsActivated = true,
                                        //    IsDeleted = false,
                                        //});

                                        //projectsApiDb.ContractSides.AddRange(contractSideList);
                                        #endregion

                                    }
                                    projectsApiDb.ConstructionProjectOwnerUsers.AddRange(newConstructionProjectOwnerPersonsList);
                                    projectsApiDb.SaveChanges();
                                }

                                #endregion
                            }
                            #endregion

                            #region ConstructionProjectRepresentatives


                            if (projectsApiDb.ConstructionProjectRepresentatives.Where(o => o.ConstructionProjectId.Equals(constructionProjectId)).Any())
                            {
                                #region remove old ConstructionProjectRepresentatives

                                var oldConstructionProjectRepresentativesList = projectsApiDb.ConstructionProjectRepresentatives.Where(o => o.ConstructionProjectId.Equals(constructionProjectId)).ToList();
                                projectsApiDb.ConstructionProjectRepresentatives.RemoveRange(oldConstructionProjectRepresentativesList);
                                projectsApiDb.SaveChanges();

                                #endregion

                                #region add new ConstructionProjectRepresentatives

                                if (constructionProjectsVM.ConstructionProjectRepresentativesVM != null)
                                {

                                    var newConstructionProjectRepresentativesList = _mapper.Map<List<ConstructionProjectRepresentativesVM>,
                                        List<ConstructionProjectRepresentatives>>(constructionProjectsVM.ConstructionProjectRepresentativesVM);


                                    foreach (var item in newConstructionProjectRepresentativesList)
                                    {
                                        if (!item.RepresentativeId.Equals(0))
                                        {
                                            item.ConstructionProjectId = constructionProject.ConstructionProjectId;
                                            item.IsActivated = true;
                                            item.IsDeleted = false;

                                        }
                                    }
                                    projectsApiDb.ConstructionProjectRepresentatives.AddRange(newConstructionProjectRepresentativesList);
                                    projectsApiDb.SaveChanges();
                                }

                                #endregion
                            }
                            else
                            {
                                #region add new ConstructionProjectRepresentatives

                                if (constructionProjectsVM.ConstructionProjectRepresentativesVM != null)
                                {

                                    var newConstructionProjectRepresentativesList = _mapper.Map<List<ConstructionProjectRepresentativesVM>,
                                        List<ConstructionProjectRepresentatives>>(constructionProjectsVM.ConstructionProjectRepresentativesVM);


                                    foreach (var item in newConstructionProjectRepresentativesList)
                                    {
                                        if (!item.RepresentativeId.Equals(0))
                                        {
                                            item.ConstructionProjectId = constructionProject.ConstructionProjectId;
                                            item.IsActivated = true;
                                            item.IsDeleted = false;

                                        }

                                    }
                                    projectsApiDb.ConstructionProjectRepresentatives.AddRange(newConstructionProjectRepresentativesList);
                                    projectsApiDb.SaveChanges();
                                }

                                #endregion 
                            }
                            #endregion

                            #region ConstructionProjectPriceHistories

                            if (constructionProjectsVM.PriceTypeRegister == 0)//اصلاح  قبلی
                            {
                                ConstructionProjectPriceHistories constructionProjectHistories = (from h in projectsApiDb.ConstructionProjectPriceHistories
                                                                                                  where h.ConstructionProjectId == constructionProjectId
                                                                                                  select h).OrderByDescending(c => c.CreateEnDate)
                                                                                        .ThenByDescending(c => c.CreateTime)
                                                                                        .FirstOrDefault();


                                constructionProjectHistories.CurrentValueOfProject = constructionProjectsVM.CurrentValueOfProject.HasValue ? constructionProjectsVM.CurrentValueOfProject.Value : 0;
                                constructionProjectHistories.ProjectEstimate = constructionProjectsVM.ProjectEstimate.HasValue ? constructionProjectsVM.ProjectEstimate.Value : 0;
                                constructionProjectHistories.PrevisionOfCost = constructionProjectsVM.PrevisionOfCost.HasValue ? constructionProjectsVM.PrevisionOfCost.Value : 0;
                                constructionProjectHistories.PriceTypeRegister = constructionProjectsVM.PriceTypeRegister.HasValue ? constructionProjectsVM.PriceTypeRegister.Value : 0;

                                constructionProjectHistories.EditEnDate = DateTime.Now;
                                constructionProjectHistories.EditTime = PersianDate.TimeNow;
                                constructionProjectHistories.IsActivated = true;
                                constructionProjectHistories.IsDeleted = false;

                                projectsApiDb.Entry<ConstructionProjectPriceHistories>(constructionProjectHistories).State = EntityState.Modified;
                                projectsApiDb.SaveChanges();
                            }
                            else//ثبت  جدید
                            {
                                ConstructionProjectPriceHistories constructionProjectHistories = new ConstructionProjectPriceHistories();
                                constructionProjectHistories.CurrentValueOfProject = constructionProjectsVM.CurrentValueOfProject.HasValue ? constructionProjectsVM.CurrentValueOfProject.Value : (long)0;
                                constructionProjectHistories.ProjectEstimate = constructionProjectsVM.ProjectEstimate.HasValue ? constructionProjectsVM.ProjectEstimate.Value : (long)0;
                                constructionProjectHistories.PrevisionOfCost = constructionProjectsVM.PrevisionOfCost.HasValue ? constructionProjectsVM.PrevisionOfCost.Value : (long)0;
                                constructionProjectHistories.PriceTypeRegister = constructionProjectsVM.PriceTypeRegister.HasValue ? constructionProjectsVM.PriceTypeRegister.Value : (int)1;
                                constructionProjectHistories.ConstructionProjectId = constructionProject.ConstructionProjectId;

                                constructionProjectHistories.CreateEnDate = DateTime.Now;
                                constructionProjectHistories.CreateTime = PersianDate.TimeNow;

                                constructionProjectHistories.UserIdCreator = constructionProject.UserIdCreator.Value;
                                constructionProjectHistories.IsActivated = true;
                                constructionProjectHistories.IsDeleted = false;

                                projectsApiDb.ConstructionProjectPriceHistories.Add(constructionProjectHistories);
                                projectsApiDb.SaveChanges();


                            }
                            #endregion



                            transaction.Commit();


                            return constructionProject.ConstructionProjectId;

                        }
                        catch (Exception)
                        { }
                    }

                }

            }
            return 0;
        }


        public bool ToggleActivationConstructionProjects(long constructionProjectId,
                   long userId,
                   List<long> childsUsersIds)
        {
            try
            {
                var constructionProjects = (from c in projectsApiDb.ConstructionProjects
                                            where c.ConstructionProjectId == constructionProjectId &&
                                            childsUsersIds.Contains(c.UserIdCreator.Value)
                                            select c).FirstOrDefault();

                if (constructionProjects != null)
                {
                    constructionProjects.IsActivated = !constructionProjects.IsActivated;
                    constructionProjects.EditEnDate = DateTime.Now;
                    constructionProjects.EditTime = PersianDate.TimeNow;
                    constructionProjects.UserIdEditor = userId;

                    projectsApiDb.Entry<Entities.ConstructionProjects>(constructionProjects).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool TemporaryDeleteConstructionProjects(long constructionProjectId,
                 long userId,
                 List<long> childsUsersIds)
        {
            try
            {
                var constructionProjects = (from c in projectsApiDb.ConstructionProjects
                                            where c.ConstructionProjectId == constructionProjectId &&
                                            childsUsersIds.Contains(c.UserIdCreator.Value)
                                            select c).FirstOrDefault();

                if (constructionProjects != null)
                {
                    constructionProjects.IsDeleted = !constructionProjects.IsDeleted;
                    constructionProjects.EditEnDate = DateTime.Now;
                    constructionProjects.EditTime = PersianDate.TimeNow;
                    constructionProjects.UserIdEditor = userId;

                    projectsApiDb.Entry<Entities.ConstructionProjects>(constructionProjects).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool? CompleteDeleteConstructionProjects(long constructionProjectId,
            long userId,
            List<long> childsUsersIds)
        {

            try
            {

                #region Attachements

                var haveAttachements = (from a in projectsApiDb.PartnershipAgreements
                                        where a.ConstructionProjectId == constructionProjectId
                                        select a).Any() ||
                                      (from a in projectsApiDb.ConfirmationAgreements
                                       where a.ConstructionProjectId == constructionProjectId
                                       select a).Any() ||
                                      (from a in projectsApiDb.ContractorsAgreements
                                       where a.ConstructionProjectId == constructionProjectId
                                       select a).Any() ||
                                      (from a in projectsApiDb.ContractAgreements
                                       where a.ConstructionProjectId == constructionProjectId
                                       select a).Any() ||
                                      (from a in projectsApiDb.InitialPlans
                                       where a.ConstructionProjectId == constructionProjectId
                                       select a).Any() ||
                                      (from a in projectsApiDb.MeetingBoards
                                       where a.ConstructionProjectId == constructionProjectId
                                       select a).Any() ||
                                      (from a in projectsApiDb.PitchDecks
                                       where a.ConstructionProjectId == constructionProjectId
                                       select a).Any() ||
                                      (from a in projectsApiDb.ProgressPictures
                                       where a.ConstructionProjectId == constructionProjectId
                                       select a).Any()
                                      ;
                if (haveAttachements)
                {
                    return null;
                }

                #endregion




                var constructionProject = (from c in projectsApiDb.ConstructionProjects
                                           where c.ConstructionProjectId == constructionProjectId &&
                                           childsUsersIds.Contains(c.UserIdCreator.Value)
                                           select c).FirstOrDefault();




                if (constructionProject != null)
                {
                    using (var transaction = projectsApiDb.Database.BeginTransaction())
                    {
                        try
                        {

                            #region ConstructionProjectOwnerUsers

                            var constructionProjectOwnerUsers = (from c in projectsApiDb.ConstructionProjectOwnerUsers
                                                                 where c.ConstructionProjectId == constructionProjectId
                                                                 select c).ToList();

                            if (constructionProjectOwnerUsers != null)
                            {
                                if (constructionProjectOwnerUsers.Count > 0)
                                {
                                    projectsApiDb.ConstructionProjectOwnerUsers.RemoveRange(constructionProjectOwnerUsers);
                                    projectsApiDb.SaveChanges();

                                }


                            }

                            #endregion


                            #region ConstructionProjectRepresentatives


                            var constructionProjectRepresentatives = (from c in projectsApiDb.ConstructionProjectRepresentatives
                                                                      where c.ConstructionProjectId == constructionProjectId
                                                                      select c).ToList();


                            if (constructionProjectRepresentatives != null)
                            {
                                if (constructionProjectRepresentatives.Count > 0)
                                {

                                    projectsApiDb.ConstructionProjectRepresentatives.RemoveRange(constructionProjectRepresentatives);
                                    projectsApiDb.SaveChanges();
                                }

                            }

                            #endregion


                            projectsApiDb.ConstructionProjects.Remove(constructionProject);
                            projectsApiDb.SaveChanges();


                            transaction.Commit();

                            return true;

                        }
                        catch (Exception exc)
                        {
                            transaction.Rollback();
                        }
                    }
                }


            }
            catch (Exception exc)
            { }

            return false;


        }

        public ConstructionProjectsVM GetConstructionProjectWithConstructionProjectId(long constructionProjectId,
         List<long> childsUsersIds)
        {
            ConstructionProjectsVM constructionProjectsVM = new ConstructionProjectsVM();

            try
            {
                #region ConstructionProjects
                constructionProjectsVM = _mapper.Map<ConstructionProjects,
                    ConstructionProjectsVM>(projectsApiDb.ConstructionProjects
                    //.Where(p => childsUsersIds.Contains(p.UserIdCreator.Value))
                    .Where(e => e.ConstructionProjectId.Equals(constructionProjectId)).FirstOrDefault());

                #endregion

                #region ConstructionProjectOwnerUsers
                if (projectsApiDb.ConstructionProjectOwnerUsers.Where(o => constructionProjectId.Equals(o.ConstructionProjectId)).Any())
                    constructionProjectsVM.ConstructionProjectOwnerUsersVM = _mapper.Map<List<ConstructionProjectOwnerUsers>,
                            List<ConstructionProjectOwnerUsersVM>>(projectsApiDb.ConstructionProjectOwnerUsers.Where(o => constructionProjectsVM.ConstructionProjectId.Equals(o.ConstructionProjectId)).ToList());

                #endregion


                #region ConstructionProjectRepresentatives

                if (projectsApiDb.ConstructionProjectRepresentatives.Where(o => constructionProjectId.Equals(o.ConstructionProjectId)).Any())
                    constructionProjectsVM.ConstructionProjectRepresentativesVM = _mapper.Map<List<ConstructionProjectRepresentatives>,
                            List<ConstructionProjectRepresentativesVM>>(projectsApiDb.ConstructionProjectRepresentatives.Where(o => constructionProjectsVM.ConstructionProjectId.Equals(o.ConstructionProjectId)).ToList());

                #endregion

                #region ConstructionProjectPriceHistories

                constructionProjectsVM.ConstructionProjectPriceHistoriesVM = new List<ConstructionProjectPriceHistoriesVM>();

                constructionProjectsVM.ConstructionProjectPriceHistoriesVM.Add(_mapper.Map<ConstructionProjectPriceHistories,
                    ConstructionProjectPriceHistoriesVM>(projectsApiDb.ConstructionProjectPriceHistories
                   .Where(h => h.ConstructionProjectId.Equals(constructionProjectId)).OrderByDescending(c => c.CreateEnDate)
                    .ThenByDescending(c => c.CreateTime).FirstOrDefault()));

                #endregion

            }
            catch (Exception exc)
            { }

            return constructionProjectsVM;
        }



        public string ToggleShowInDashboardConstructionProjects(long constructionProjectId,
                        long userId,
                        List<long> childsUsersIds)
        {
            try
            {
                // پروژه ای که تیک نمایش در داشبورد دارد
                var projectWithShowInDashboardTrue = projectsApiDb.ConstructionProjects.Where(p => p.ShowInDashboard == true).FirstOrDefault();

                // if one projects has showInDashboard
                if (projectWithShowInDashboardTrue != null && projectWithShowInDashboardTrue.ConstructionProjectId != constructionProjectId)
                {
                    return projectWithShowInDashboardTrue.ConstructionProjectTitle;
                }
                else if (projectsApiDb.ConstructionProjects.Where(c => c.ConstructionProjectId.Equals(constructionProjectId)).Any())
                {
                    var constructionProjects = projectsApiDb.ConstructionProjects.Where(c => c.ConstructionProjectId.Equals(constructionProjectId)).FirstOrDefault();

                    constructionProjects.ShowInDashboard = !constructionProjects.ShowInDashboard;
                    projectsApiDb.Entry<Entities.ConstructionProjects>(constructionProjects).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();
                    return "ok";
                }






            }
            catch (Exception exc)
            { }

            return "error";
        }


        #region outterDashboard - users
        //outterDashboard
        //investors



        #region new - code

        public List<ConstructionProjectsVM> GetListOfConstructionProjectsWithUserId(GetConstructionProjectWithUserIdPVM getConstructionProjectWithUserIdPVM)
        {
            List<ConstructionProjectsVM> constructionProjectsVMList = new List<ConstructionProjectsVM>();

            try
            {
                var projectIds = projectsApiDb.ConstructionProjectOwnerUsers.Where(x => x.OwnerUserId == getConstructionProjectWithUserIdPVM.UserId)
                    .Select(x => x.ConstructionProjectId).ToList();

                if (projectIds != null)
                {
                    if (projectIds.Count > 0)
                    {
                        var constructionProjects = projectsApiDb.ConstructionProjects
                        .Where(x => projectIds.Contains(x.ConstructionProjectId))
                        .Select(x => new ConstructionProjectsVM
                        {
                            ConstructionProjectDescription = x.ConstructionProjectDescription,
                            CreateTime = x.CreateTime,
                            ConstructionProjectId = x.ConstructionProjectId,
                            ConstructionProjectTitle = x.ConstructionProjectTitle,
                            CreateEnDate = x.CreateEnDate,
                            ExecutionStartDate = x.ExecutionStartDate,
                            DesignStartDate = x.DesignStartDate,
                            ConstructionProjectTypeId = x.ConstructionProjectTypeId,
                            MonthsLeftUntilTheEnd = x.MonthsLeftUntilTheEnd,
                            StartDateEn = x.StartDateEn.HasValue ? x.StartDateEn.Value : null,
                            EndDate = (x.StartDateEn.HasValue && !string.IsNullOrEmpty(x.MonthsLeftUntilTheEnd)) ? x.StartDateEn.Value.AddMonths(int.Parse(x.MonthsLeftUntilTheEnd)).Date.ToString() : null,
                            EditEnDate = x.EditEnDate,
                            IsActivated = x.IsActivated,
                            PropertyId = x.PropertyId,
                        })
                        .ToList();

                        var constructionProjectPriceHistories = projectsApiDb.ConstructionProjectPriceHistories.Where(p => projectIds.Contains(p.ConstructionProjectId)).
                            OrderByDescending(c => c.CreateEnDate.Value).ToList();

                        constructionProjectsVMList.AddRange(constructionProjects);
                        foreach (var item in constructionProjects)
                        {
                            try
                            {
                                item.DaysLeftUntilTheEnd = (DateTime.Parse(item.EndDate) - DateTime.Now).Days.ToString();
                            }
                            catch (Exception exc) { }

                            try
                            {
                                if (constructionProjectPriceHistories.Where(p => p.ConstructionProjectId.Equals(item.ConstructionProjectId)).Any())
                                    item.PrevisionOfCost = constructionProjectPriceHistories.Where(p => p.ConstructionProjectId.Equals(item.ConstructionProjectId)).FirstOrDefault().PrevisionOfCost;
                            }
                            catch (Exception exc) { }

                            #region related to projects

                            //  کل هزینه پروژه
                            var totalCost = projectsApiDb.ConstructionProjectFinancialData
                                            .FromSqlRaw(@"
                                                  SELECT REPLACE(CellData, ',', '') AS CellData
                                                  FROM ConstructionProjectFinancialData
                                                  WHERE 
                                                  ParentType = 'constructionproject'
                                                  AND RecordType = 'private'
                                                  AND CellIndex > 0
                                                  AND RowIndex = 9
                                                  AND ParentId = {0}
                                                  AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
                                                  SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId 
                                                  FROM ExcelSheetConfigs
                                                  INNER JOIN ConstructionProjects ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                                                  WHERE 
                                                  ExcelSheetConfigs.ReportType = 'financial'
                                                  AND ConstructionProjects.ConstructionProjectId = {0}
                                                  )", item.ConstructionProjectId).Select(x => new RowsDataVM { CellData = x.CellData })
                                            .ToList();

                            var publicTotalcost = totalCost.Sum(x => long.Parse(x.CellData));

                            item.ProjectTotalCost = publicTotalcost;






                            // کل هزینه عمومی 
                            var ProjectPublicTotalCost = projectsApiDb.ConstructionProjectFinancialData
                           .FromSqlRaw(@"
                                SELECT REPLACE(CellData, ',', '') AS CellData
                                FROM ConstructionProjectFinancialData
                                WHERE 
                                ParentType = 'constructionproject'
                                AND RecordType = 'public'
                                AND CellIndex > 0
                                AND RowIndex = 4
                                AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
                                SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId 
                                FROM ExcelSheetConfigs
                                INNER JOIN ConstructionProjects ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                                WHERE 
                                ExcelSheetConfigs.ReportType = 'financial'
                                AND ConstructionProjects.ConstructionProjectId = {0}
                                )", item.ConstructionProjectId).Select(x => new RowsDataVM { CellData = x.CellData })
                           .ToList();



                            var totalProjectPublicTotalCost = ProjectPublicTotalCost.Sum(x => long.Parse(x.CellData));

                            item.ProjectPublicTotalCost = totalProjectPublicTotalCost;



                            // سهم از هزینه عمومی
                            var rowsData = projectsApiDb.ConstructionProjectFinancialData
                          .FromSqlRaw(@"
                                   SELECT ParentId, RowIndex, CellIndex, REPLACE(CellData, ',', '') AS CellData
                                   FROM ConstructionProjectFinancialData
                                   WHERE ParentType = 'constructionproject'
                                   AND RecordType = 'private'
                                   AND ParentId  = {0}
                                   AND CellIndex > 0
                                   And RowIndex = 5
                                   AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
                                   SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId
                                   FROM ExcelSheetConfigs
                                   INNER JOIN ConstructionProjects
                                       ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                                   WHERE ExcelSheetConfigs.ReportType = 'financial'
                                       AND ConstructionProjects.ConstructionProjectId = {0}
                               )", item.ConstructionProjectId).
                              Select(result => new RowsDataVM
                              {
                                  RowIndex = result.RowIndex,
                                  CellIndex = result.CellIndex,
                                  CellData = result.CellData,
                                  ParentId = result.ParentId
                              })
                           .ToList();

                            var totalGeneralCost = rowsData.Sum(x => long.Parse(x.CellData));
                            //عدد
                            item.ProjectShareOfGeneralCost = totalGeneralCost;

                            //درصد
                            //decimal percent = Convert.ToInt64(publicTotalcost / totalGeneralCost) * 100;


                            decimal percent = (decimal)totalGeneralCost / totalProjectPublicTotalCost;

                            item.ProjectShareOfGeneralCostPercent = Math.Round((percent * 100));

                            #region comment


                            // سهم از هزینه عمومی
                            //var rowsData = projectsApiDb.ConstructionProjectFinancialData
                            //.FromSqlRaw(@"
                            //       SELECT ParentId, RowIndex, CellIndex, REPLACE(CellData, ',', '') AS CellData
                            //       FROM ConstructionProjectFinancialData
                            //       WHERE ParentType = 'constructionproject'
                            //       AND RecordType = 'private'
                            //       AND ParentId  = {0}
                            //       AND CellIndex > 0
                            //       And RowIndex = 5
                            //       AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
                            //       SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId
                            //       FROM ExcelSheetConfigs
                            //       INNER JOIN ConstructionProjects
                            //           ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                            //       WHERE ExcelSheetConfigs.ReportType = 'financial'
                            //           AND ConstructionProjects.ConstructionProjectId = {0}
                            //   )", item.ConstructionProjectId)
                            // .Select(result => new RowsDataVM
                            // {
                            //     RowIndex = result.RowIndex,
                            //     CellIndex = result.CellIndex,
                            //     CellData = result.CellData,
                            //     ParentId = result.ParentId
                            // })
                            //.ToList();





                            //var ProjectShareOfGeneralCost = projectsApiDb.ConstructionProjectFinancialData
                            //      .FromSqlRaw($@"
                            //        SELECT CellIndex, REPLACE(CellData, ',', '') AS CellData
                            //        FROM ConstructionProjectFinancialData
                            //        WHERE ParentType = 'constructionproject'
                            //        AND RecordType = 'public'
                            //        AND CellIndex > 0
                            //        and RowIndex > 5
                            //        AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
                            //        SELECT DISTINCT ec.ExcelSheetConfigId
                            //        FROM ExcelSheetConfigs ec
                            //        INNER JOIN ConstructionProjects cp ON ec.ExcelSheetConfigName = cp.WorkshopName
                            //        WHERE ec.ReportType = 'financial'
                            //            AND cp.ConstructionProjectId = {item.ConstructionProjectId})
                            //        ")
                            //      .GroupBy(result => result.CellIndex)
                            //      .Select(group => new RowsDataVM
                            //      {
                            //          CellIndex = group.Key,
                            //          CellData = group.Sum(item => Convert.ToInt64(item.CellData.Replace(",", ""))).ToString()
                            //      })
                            //      .ToList();

                            //List<decimal> percentageValues = rowsData.Select(x => decimal.Parse(x.CellData)).ToList();
                            //List<decimal> projectShareOfGeneralCostPrecent = new List<decimal>();
                            //for (int i = 0; i < Math.Min(percentageValues.Count, ProjectShareOfGeneralCost.Count); i++)
                            //{
                            //    decimal percentage = percentageValues[i] / 100;
                            //    decimal value = decimal.Parse(ProjectShareOfGeneralCost[i].CellData);
                            //    decimal product = percentage * value;
                            //    decimal roundedProduct = Math.Round(product);
                            //    projectShareOfGeneralCostPrecent.Add(roundedProduct);
                            //}
                            //var totalProjectShareOfGeneralCost = ProjectShareOfGeneralCost.Sum(x => long.Parse(x.CellData));
                            //item.ProjectShareOfGeneralCostPercent = Math.Round((projectShareOfGeneralCostPrecent.Sum() / totalProjectShareOfGeneralCost) * 100);
                            //item.ProjectShareOfGeneralCost = Convert.ToInt64(projectShareOfGeneralCostPrecent.Sum());

                            #endregion

                            // کل هزینه اختصاصی
                            var ProjectPrivateTotalCost = projectsApiDb.ConstructionProjectFinancialData
                                .FromSqlRaw(@"
                                        SELECT REPLACE(CellData, ',', '') AS CellData
                                        FROM ConstructionProjectFinancialData
                                        WHERE ParentType = 'constructionproject'
                                        AND RecordType = 'private'
                                        AND ParentId  = {0}
                                        AND RowIndex = 10
                                        And CellIndex > 0
                                        AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
                                            SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId
                                            FROM ExcelSheetConfigs
                                            INNER JOIN ConstructionProjects
                                                ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                                            WHERE ExcelSheetConfigs.ReportType = 'financial'
                                                AND ConstructionProjects.ConstructionProjectId = {0}
                                        )", item.ConstructionProjectId).Select(x => new RowsDataVM { CellData = x.CellData })
                               .ToList();

                            var totalProjectPrivateTotalCost = ProjectPrivateTotalCost.Sum(x => long.Parse(x.CellData));

                            item.ProjectPrivateTotalCost = totalProjectPrivateTotalCost;




                            // سهم از کل هزینه پروژه
                            var shareOfTotalProjectCost = projectsApiDb.ConstructionProjectOwnerUsers.Where(x =>
                            x.OwnerUserId.Equals(getConstructionProjectWithUserIdPVM.UserId)
                            && x.ConstructionProjectId.Equals(item.ConstructionProjectId)
                            && x.IsActivated.Value.Equals(true)
                            && x.IsDeleted.Value.Equals(false)).FirstOrDefault();

                            item.ShareOfTotalProjectCostPercent = shareOfTotalProjectCost.SharePercent;
                            item.ShareOfTotalProjectCost = Math.Round(Convert.ToDecimal(item.ProjectTotalCost * (shareOfTotalProjectCost.SharePercent / 100.0)));


                            // سهم پرداختی پروژه
                            var ProjectPaidShare = projectsApiDb.ConstructionProjectFinancialData
                                .FromSqlRaw(@"
                                         SELECT CellIndex, REPLACE(CellData, ',', '') AS CellData  
                                         FROM (
                                           SELECT *, MAX(CellIndex) OVER() AS MaxCellIndex
                                           FROM ConstructionProjectFinancialData
                                           WHERE ParentType = 'person' 
                                             AND RecordType = 'private'
                                            AND UserIdCreator = {0}
                                               AND ParentId = {1}
                                             AND ExcelSheetConfigId IN (
                                               SELECT ExcelSheetConfigId
                                               FROM ExcelSheetConfigs
                                               INNER JOIN ConstructionProjects
                                                 ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                                               WHERE ExcelSheetConfigs.ReportType = 'financial'
                                                 AND ConstructionProjects.ConstructionProjectId= {2})
                                ) t
                                WHERE CellIndex = MaxCellIndex", getConstructionProjectWithUserIdPVM.UserId, item.ConstructionProjectId, item.ConstructionProjectId)
                                .Select(result => new RowsDataVM
                                {
                                    CellData = result.CellData
                                })
                                .ToList();


                            var totalProjectPaidShare = ProjectPaidShare.Sum(x => long.Parse(x.CellData));

                            item.ProjectPaidShare = totalProjectPaidShare;




                            // مانده هزینه
                            item.ProjectRemainingCost = Math.Round((item.ShareOfTotalProjectCost.Value - totalProjectPaidShare));

                            #endregion



                            // سقف تنخواه
                            var fundsCeiling = projectsApiDb.ConstructionProjectFinancialData
                            .FromSqlRaw(@"
                                        SELECT CellIndex, REPLACE(CellData, ',', '') AS CellData
                                        FROM ConstructionProjectFinancialData
                                        WHERE ParentType = 'constructionproject'
                                        AND RecordType = 'private'
                                        AND ParentId  = {0}
                                        And RowIndex = 7
                                        And CellIndex > 0
                                        AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
                                        SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId
                                        FROM ExcelSheetConfigs
                                        INNER JOIN ConstructionProjects
                                            ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                                        WHERE ExcelSheetConfigs.ReportType = 'financial'
                                            AND ConstructionProjects.ConstructionProjectId = {0})        
                            ", item.ConstructionProjectId).Select(x => new RowsDataVM { CellData = x.CellData, CellIndex = x.CellIndex })
                              .ToList();


                            if (fundsCeiling.Count > 0)
                            {
                                item.FundsCeiling = Convert.ToInt64(fundsCeiling.LastOrDefault().CellData);
                                item.FundsCeiling = (long)(item.FundsCeiling.Value * item.ShareOfTotalProjectCostPercent.Value) / 100;

                                item.RemainingDebt = item.FundsCeiling + Convert.ToInt64(item.ProjectRemainingCost);

                            }
                            else
                            {
                                item.FundsCeiling = 0;
                                item.FundsCeiling = 0;

                                item.RemainingDebt = 0;
                            }




                            //تعهدات
                            var Obligations = projectsApiDb.ConstructionProjectFinancialData
                           .FromSqlRaw(@"
                                        SELECT CellIndex, REPLACE(CellData, ',', '') AS CellData
                                        FROM ConstructionProjectFinancialData
                                        WHERE ParentType = 'constructionproject'
                                        AND RecordType = 'private'
                                        AND ParentId  = {0}
                                        And RowIndex >= 12
                                        And CellIndex = 1
                                        AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
                                        SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId
                                        FROM ExcelSheetConfigs
                                        INNER JOIN ConstructionProjects
                                            ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                                        WHERE ExcelSheetConfigs.ReportType = 'financial'
                                            AND ConstructionProjects.ConstructionProjectId = {0})        
                            ", item.ConstructionProjectId).Select(x => new RowsDataVM { CellData = x.CellData, CellIndex = x.CellIndex })
                             .ToList();


                            var publicObligations = Obligations.Sum(x => long.Parse(x.CellData));

                            item.Obligations = publicObligations;


                            #region ConstructionProjectProgressData - LastUpdate
                            //آخرین آپدیت گزارشات روزانه


                            //var LastProgessUpdateDate = projectsApiDb.ConstructionProjectProgressData
                            //            .FromSqlRaw(@"
                            //            SELECT CellIndex, CellData AS CellData
                            //            FROM ConstructionProjectProgressData
                            //            WHERE ConstructionProjectId = {0}
                            //            And RowIndex = 1
                            //            --And CellIndex = 1
                            //            AND ConstructionProjectProgressData.ExcelSheetConfigId IN (
                            //            SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId
                            //            FROM ExcelSheetConfigs
                            //            INNER JOIN ConstructionProjects
                            //                ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                            //            WHERE ExcelSheetConfigs.ReportType = 'progress'
                            //                AND ConstructionProjects.ConstructionProjectId = {0})          
                            //             ", item.ConstructionProjectId).ToList();

                            #endregion



                        }


                    }
                }

            }
            catch (Exception ex)
            { }
            return constructionProjectsVMList;
        }
        #endregion

        public List<ConstructionProjectDailyDataForOuterDashbordVM> GetConstructionProjectsDailyDataByConstructionProjectId(
            int jtStartIndex,
            int jtPageSize,
            ref int listCount,
            long ConstructionProjectId,
            string jtSorting = null)
        {
            List<ConstructionProjectDailyDataForOuterDashbordVM> constructionProjectDailyDataForOuterDashbordVMs = new List<ConstructionProjectDailyDataForOuterDashbordVM>();

            var getProjectsDaily = projectsApiDb.ConstructionProjectDailyData
                .Where(x => x.ConstructionProjectId == ConstructionProjectId &&
                            (x.CellIndex == 1 || x.CellIndex == 2 || x.CellIndex == 7 ||
                             x.CellIndex == 12 || x.CellIndex == 21 || x.CellIndex == 22 ||
                             x.CellIndex == 23))
                .AsNoTracking()
                .ToList();

            //21: سرپرست => OperatorApprove
            //22 : pmo => WorkshopSupervisorSignatureApprove
            // 23: کنترل پروژه = > ProjectControlAndRegistrationApprove

            var filteredData = getProjectsDaily
                .GroupBy(x => x.RowIndex)
                .Select(x => new ConstructionProjectDailyDataForOuterDashbordVM
                {
                    RecordDate = DateTime.ParseExact(x.FirstOrDefault(s => s.CellIndex == 2)?.CellData, "d-MMM-yyyy", CultureInfo.InvariantCulture),
                    OperatorApprove = x.FirstOrDefault(v => v.CellIndex == 21)?.CellData,
                    WorkshopSupervisorSignatureApprove = x.FirstOrDefault(v => v.CellIndex == 22)?.CellData,
                    ProjectControlAndRegistrationApprove = x.FirstOrDefault(v => v.CellIndex == 23)?.CellData,
                    DescriptionOfOperation = x.FirstOrDefault(v => v.CellIndex == 7)?.CellData,
                    PersianRecordDate = x.FirstOrDefault(v => v.CellIndex == 1)?.CellData,
                    Progress = x.FirstOrDefault(v => v.CellIndex == 12)?.CellData,
                })
                .OrderByDescending(x => x.RecordDate)
                .ToList();

            listCount = filteredData.Count();

            if (listCount >= jtPageSize)
            {
                int skip = (jtStartIndex - 1) * jtPageSize;
                filteredData = filteredData.Skip(skip).Take(jtPageSize).ToList();
            }

            return filteredData;
        }


        public List<AgreementFileWithAttachmentsVM> GetAgreementDataByAgreementTypeAndConstructionProjectId(
             long constructionProjectId,
             string ContractType,
             long userId,
             bool HaveAttachments,
             bool HaveConversations)
        {
            List<AgreementFileWithAttachmentsVM> list = new List<AgreementFileWithAttachmentsVM>();
            try
            {
                switch (ContractType)
                {
                    case "PartnershipAgreement":
                        list = (from agreement in projectsApiDb.PartnershipAgreements
                                where agreement.ConstructionProjectId.Equals(constructionProjectId)
                                && agreement.IsActivated.Value.Equals(true)
                                && agreement.IsDeleted.Value.Equals(false)
                                select new AgreementFileWithAttachmentsVM
                                {
                                    AgreementId = agreement.PartnershipAgreementId,
                                    ConstructionProjectId = agreement.ConstructionProjectId,
                                    AgreementType = "PartnershipAgreement",
                                    AgreementTitle = agreement.PartnershipAgreementTitle,
                                    AgreementDescription = agreement.PartnershipAgreementDescription,
                                    AgreementNumber = agreement.PartnershipAgreementNumber,
                                    AgreementFilePath = agreement.PartnershipAgreementFilePath,
                                    AgreementFileExt = agreement.PartnershipAgreementFileExt,
                                    AgreementFileOrder = agreement.PartnershipAgreementFileOrder,
                                    AgreementFileType = agreement.PartnershipAgreementFileType,
                                }).ToList();
                        break;
                    case "ContractAgreement":
                        list = (from agreement in projectsApiDb.ContractAgreements
                                where agreement.ConstructionProjectId.Equals(constructionProjectId)
                                && agreement.IsActivated.Value.Equals(true)
                                && agreement.IsDeleted.Value.Equals(false)
                                select new AgreementFileWithAttachmentsVM
                                {
                                    AgreementId = agreement.ContractAgreementId,
                                    ConstructionProjectId = agreement.ConstructionProjectId,
                                    AgreementType = "ContractAgreement",
                                    AgreementTitle = agreement.ContractAgreementTitle,
                                    AgreementDescription = agreement.ContractAgreementDescription,
                                    AgreementNumber = agreement.ContractAgreementNumber,
                                    AgreementFilePath = agreement.ContractAgreementFilePath,
                                    AgreementFileExt = agreement.ContractAgreementFileExt,
                                    AgreementFileOrder = agreement.ContractAgreementFileOrder,
                                    AgreementFileType = agreement.ContractAgreementFileType,
                                }).ToList();
                        break;
                    case "ConfirmationAgreement":
                        list = (from agreement in projectsApiDb.ConfirmationAgreements
                                where agreement.ConstructionProjectId.Equals(constructionProjectId)
                                && agreement.IsActivated.Value.Equals(true)
                                && agreement.IsDeleted.Value.Equals(false)
                                select new AgreementFileWithAttachmentsVM
                                {
                                    AgreementId = agreement.ConfirmationAgreementId,
                                    ConstructionProjectId = agreement.ConstructionProjectId,
                                    AgreementType = "ConfirmationAgreement",
                                    AgreementTitle = agreement.ConfirmationAgreementTitle,
                                    AgreementDescription = agreement.ConfirmationAgreementDescription,
                                    AgreementNumber = agreement.ConfirmationAgreementNumber,
                                    AgreementFilePath = agreement.ConfirmationAgreementFilePath,
                                    AgreementFileExt = agreement.ConfirmationAgreementFileExt,
                                    AgreementFileOrder = agreement.ConfirmationAgreementFileOrder,
                                    AgreementFileType = agreement.ConfirmationAgreementFileType,
                                }).ToList();
                        break;
                    case "ContractorsAgreement":
                        list = (from agreement in projectsApiDb.ContractorsAgreements
                                where agreement.ConstructionProjectId.Equals(constructionProjectId)
                                && agreement.IsActivated.Value.Equals(true)
                                && agreement.IsDeleted.Value.Equals(false)
                                select new AgreementFileWithAttachmentsVM
                                {
                                    AgreementId = agreement.ContractorsAgreementId,
                                    ConstructionProjectId = agreement.ConstructionProjectId,
                                    AgreementType = "ContractorsAgreement",
                                    AgreementTitle = agreement.ContractorsAgreementTitle,
                                    AgreementDescription = agreement.ContractorsAgreementDescription,
                                    AgreementNumber = agreement.ContractorsAgreementNumber,
                                    AgreementFilePath = agreement.ContractorsAgreementFilePath,
                                    AgreementFileExt = agreement.ContractorsAgreementFileExt,
                                    AgreementFileOrder = agreement.ContractorsAgreementFileOrder,
                                    AgreementFileType = agreement.ContractorsAgreementFileType,
                                }).ToList();
                        break;
                    case "InitialPlan":
                        list = (from agreement in projectsApiDb.InitialPlans
                                where agreement.ConstructionProjectId.Equals(constructionProjectId)
                                && agreement.IsActivated.Value.Equals(true)
                                && agreement.IsDeleted.Value.Equals(false)
                                select new AgreementFileWithAttachmentsVM
                                {
                                    AgreementId = agreement.InitialPlanId,
                                    ConstructionProjectId = agreement.ConstructionProjectId,
                                    AgreementType = "InitialPlan",
                                    AgreementTitle = agreement.InitialPlanTitle,
                                    AgreementDescription = agreement.InitialPlanDescription,
                                    AgreementNumber = agreement.InitialPlanNumber,
                                    AgreementFilePath = agreement.InitialPlanFilePath,
                                    AgreementFileExt = agreement.InitialPlanFileExt,
                                    AgreementFileOrder = agreement.InitialPlanFileOrder,
                                    AgreementFileType = agreement.InitialPlanFileType,
                                }).ToList();
                        break;
                    case "PitchDeck":
                        list = (from agreement in projectsApiDb.PitchDecks
                                where agreement.ConstructionProjectId.Equals(constructionProjectId)
                                && agreement.IsActivated.Value.Equals(true)
                                && agreement.IsDeleted.Value.Equals(false)
                                select new AgreementFileWithAttachmentsVM
                                {
                                    AgreementId = agreement.PitchDeckId,
                                    ConstructionProjectId = agreement.ConstructionProjectId,
                                    AgreementType = "PitchDeck",
                                    AgreementTitle = agreement.PitchDeckTitle,
                                    AgreementDescription = agreement.PitchDeckDescription,
                                    AgreementNumber = agreement.PitchDeckNumber,
                                    AgreementFilePath = agreement.PitchDeckFilePath,
                                    AgreementFileExt = agreement.PitchDeckFileExt,
                                    AgreementFileOrder = agreement.PitchDeckFileOrder,
                                    AgreementFileType = agreement.PitchDeckFileType,
                                }).ToList();
                        break;
                    case "MeetingBoard":
                        list = (from agreement in projectsApiDb.MeetingBoards
                                where agreement.ConstructionProjectId.Equals(constructionProjectId)
                                && agreement.IsActivated.Value.Equals(true)
                                && agreement.IsDeleted.Value.Equals(false)
                                select new AgreementFileWithAttachmentsVM
                                {
                                    AgreementId = agreement.MeetingBoardId,
                                    ConstructionProjectId = agreement.ConstructionProjectId,
                                    AgreementType = "MeetingBoard",
                                    AgreementTitle = agreement.MeetingBoardTitle,
                                    AgreementDescription = agreement.MeetingBoardDescription,
                                    AgreementNumber = agreement.MeetingBoardNumber,
                                    AgreementFilePath = agreement.MeetingBoardFilePath,
                                    AgreementFileExt = agreement.MeetingBoardFileExt,
                                    AgreementFileOrder = agreement.MeetingBoardFileOrder,
                                    AgreementFileType = agreement.MeetingBoardFileType,
                                }).ToList();
                        break;
                    case "ProgressPicture":
                        list = (from agreement in projectsApiDb.ProgressPictures
                                where agreement.ConstructionProjectId.Equals(constructionProjectId)
                                && agreement.IsActivated.Value.Equals(true)
                                && agreement.IsDeleted.Value.Equals(false)
                                select new AgreementFileWithAttachmentsVM
                                {
                                    AgreementId = agreement.ProgressPictureId,
                                    ConstructionProjectId = agreement.ConstructionProjectId,
                                    AgreementType = "ProgressPicture",
                                    AgreementTitle = agreement.ProgressPictureTitle,
                                    AgreementDescription = agreement.ProgressPictureDescription,
                                    AgreementNumber = agreement.ProgressPictureNumber,
                                    AgreementFilePath = agreement.ProgressPictureFilePath,
                                    AgreementFileExt = agreement.ProgressPictureFileExt,
                                    AgreementFileOrder = agreement.ProgressPictureFileOrder,
                                    AgreementFileType = agreement.ProgressPictureFileType,
                                }).ToList();
                        break;
                }
                var agreementids = list.Select(x => x.AgreementId).ToList();
                if (list != null)
                {
                    if (list.Count > 0)
                    {
                        List<AttachementFilesVM> attachments = new List<AttachementFilesVM>();
                        List<FileStateLogStatusVM> fileState = new List<FileStateLogStatusVM>();

                        /// Get IsView And IsConfirm For Agreements
                        fileState = projectsApiDb.FileStatesLogs
                        .Where(x => agreementids.Contains(x.RecordId) &&
                                    x.TableTitle.Equals(ContractType) &&
                                    (x.FileStateId == 3 || x.FileStateId == 4))
                        .GroupBy(x => x.RecordId)
                        .Select(group => new FileStateLogStatusVM
                        {
                            AgreementId = group.Key,
                            IsView = group.Any(x => x.FileStateId == 3),
                            IsConfirm = group.Any(x => x.FileStateId == 4)
                        })
                        .ToList();
                        /////

                        if (HaveAttachments)
                        {
                            attachments = (from attachment in projectsApiDb.AttachementFiles
                                           where agreementids.Contains(attachment.AttachementParentId.Value)
                                           && attachment.AttachementTableTitle.Contains(ContractType)
                                           select new AttachementFilesVM
                                           {
                                               AttachementFileExt = attachment.AttachementFileExt,
                                               AttachementFileOrder = attachment.AttachementFileOrder,
                                               AttachementFilePath = attachment.AttachementFilePath,
                                               AttachementFileType = attachment.AttachementFileType,
                                               AttachementId = attachment.AttachementId,
                                               AttachementParentId = attachment.AttachementParentId,
                                               AttachementTitle = attachment.AttachementTitle,
                                           }).OrderByDescending(s => s.AttachementId).ToList();
                        }
                        foreach (var item in list)
                        {
                            // Agreement Attachments ->
                            if (HaveAttachments)
                            {

                                if (attachments.Where(ad => ad.AttachementParentId == item.AgreementId).Any())
                                {

                                    try
                                    {
                                        item.AttachmentFilesVMs = attachments.Where(x => x.AttachementParentId.Equals(item.AgreementId)).ToList();
                                    }
                                    catch (Exception ex)
                                    { }
                                }
                                else
                                {
                                    item.AttachmentFilesVMs = new List<AttachementFilesVM> { };
                                }
                            }

                            if (fileState.Where(ad => ad.AgreementId == item.AgreementId).Any())
                            {
                                try
                                {
                                    item.IsView = fileState.Any(x => x.AgreementId == item.AgreementId && x.IsView);
                                    item.IsConfirm = fileState.Any(x => x.AgreementId == item.AgreementId && x.IsConfirm);
                                }
                                catch (Exception ex)
                                { }
                            }
                            else
                            {
                                item.IsView = null;
                                item.IsConfirm = null;
                            }
                            item.ConversationIsReadCount = projectsApiDb.ConversationLogs.Where(x => x.RecordId.Equals(item.AgreementId) && x.IsRead == false && x.TableTitle.Equals(item.AgreementType) && x.UserIdCreator != userId).Count();
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
            return list;
        }

        public List<ConstructionProjectDataTypeCounts> GetListConstructionProjectDataTypeCounts(List<long> constructionProjectIds)
        {
            List<ConstructionProjectDataTypeCounts> constructionProjectDataTypeCountsList = new List<ConstructionProjectDataTypeCounts>();

            try
            {
                string sp = @"select ROW_NUMBER() OVER (ORDER BY (SELECT 1)) as ConstructionProjectDataTypeCountId, ConstructionProjectId, DataType, Count
                    from
                    (
	                        select ConstructionProjectId, 'ConfirmationAgreements' DataType, COUNT(ConfirmationAgreementId) as Count
	                        from ConfirmationAgreements
	                        group by ConstructionProjectId
	                        
	                        union all 
	                        
	                        select ConstructionProjectId, 'ContractAgreements' DataType, COUNT(ContractAgreementId) as Count
	                        from ContractAgreements
	                        group by ConstructionProjectId
	                        
	                        union all
	                        
	                        select ConstructionProjectId, 'ContractorsAgreements' DataType, COUNT(ContractorsAgreementId) as Count
	                        from ContractorsAgreements
	                        group by ConstructionProjectId
	                        
	                        union all
	                        
	                        select ConstructionProjectId, 'InitialPlans' DataType, COUNT(InitialPlanId) as Count
	                        from InitialPlans
	                        group by ConstructionProjectId
	                        
	                        union all
	                        
	                        select ConstructionProjectId, 'MeetingBoards' DataType, COUNT(MeetingBoardId) as Count
	                        from MeetingBoards
	                        group by ConstructionProjectId
	                        
	                        union all
	                        
	                        select ConstructionProjectId, 'PartnershipAgreements' DataType, COUNT(PartnershipAgreementId) as Count
	                        from PartnershipAgreements
	                        group by ConstructionProjectId
	                        
	                        union all
	                        
	                        select ConstructionProjectId, 'PitchDecks' DataType, COUNT(PitchDeckId) as Count
	                        from PitchDecks
	                        group by ConstructionProjectId
	                        
	                        union all
	                        
	                        select ConstructionProjectId, 'ProgressPictures' DataType, COUNT(ProgressPictureId) as Count
	                        from ProgressPictures
	                        group by ConstructionProjectId

                            union all
	                        
	                        select ConstructionProjectId, 'BillDelays' DataType, COUNT(ConstructionProjectBillDelayId) as Count
	                        from ConstructionProjectDelays
	                        group by ConstructionProjectId
	                        )  as Counts
                ";

                constructionProjectDataTypeCountsList = projectsApiDb.ConstructionProjectDataTypeCounts.FromSqlRaw(sp).Where(p => constructionProjectIds.Contains(p.ConstructionProjectId)).ToList();
            }
            catch (Exception exc)
            { }

            return constructionProjectDataTypeCountsList;
        }



        #endregion


        #region outterDashboard - Representatives
        //outterDashboard


        public List<ConstructionProjectsVM> GetListOfConstructionProjectsWithRepresentativeId
           (GetConstructionProjectWithUserIdPVM getConstructionProjectWithUserIdPVM, IConsoleBusiness consoleBusiness)
        {
            List<ConstructionProjectsVM> constructionProjectsVMList = new List<ConstructionProjectsVM>();

            try
            {
                //لیست پروژه هایی که نماینده آن است
                var projectIds = projectsApiDb.ConstructionProjectRepresentatives.Where(x => x.RepresentativeId == getConstructionProjectWithUserIdPVM.UserId).Select(x => x.ConstructionProjectId).ToList();

                if (projectIds != null)
                {
                    if (projectIds.Count > 0)
                    {


                        var ownerUsersList = projectsApiDb.ConstructionProjectRepresentatives.Where(c =>
                            c.RepresentativeId.Equals(getConstructionProjectWithUserIdPVM.UserId)
                            && projectIds.Contains(c.ConstructionProjectId)).ToList();


                        foreach (var project in ownerUsersList)
                        {

                            var constructionProjects = projectsApiDb.ConstructionProjects.Where(c => c.ConstructionProjectId.Equals(project.ConstructionProjectId)).Select(x => new ConstructionProjectsVM
                            {
                                ConstructionProjectDescription = x.ConstructionProjectDescription,
                                CreateTime = x.CreateTime,
                                ConstructionProjectId = x.ConstructionProjectId,
                                ConstructionProjectTitle = x.ConstructionProjectTitle,
                                CreateEnDate = x.CreateEnDate,
                                ExecutionStartDate = x.ExecutionStartDate,
                                DesignStartDate = x.DesignStartDate,
                                ConstructionProjectTypeId = x.ConstructionProjectTypeId,
                                MonthsLeftUntilTheEnd = x.MonthsLeftUntilTheEnd,
                                StartDateEn = x.StartDateEn.HasValue ? x.StartDateEn.Value : null,
                                EndDate = (x.StartDateEn.HasValue && !string.IsNullOrEmpty(x.MonthsLeftUntilTheEnd)) ? x.StartDateEn.Value.AddMonths(int.Parse(x.MonthsLeftUntilTheEnd)).Date.ToString() : null,
                                EditEnDate = x.EditEnDate,
                                IsActivated = x.IsActivated,
                                PropertyId = x.PropertyId,
                            }).ToList();

                            constructionProjectsVMList.AddRange(constructionProjects);

                            var constructionProjectPriceHistories = projectsApiDb.ConstructionProjectPriceHistories.Where(p => projectIds.Contains(p.ConstructionProjectId)).
                                OrderByDescending(c => c.CreateEnDate.Value).ToList();


                            foreach (var item in constructionProjects)
                            {
                                #region Related to the project

                                try
                                {
                                    item.DaysLeftUntilTheEnd = (DateTime.Parse(item.EndDate) - DateTime.Now).Days.ToString();
                                }
                                catch (Exception exc) { }

                                try
                                {
                                    if (constructionProjectPriceHistories.Where(p => p.ConstructionProjectId.Equals(item.ConstructionProjectId)).Any())
                                        item.PrevisionOfCost = constructionProjectPriceHistories.Where(p => p.ConstructionProjectId.Equals(item.ConstructionProjectId)).FirstOrDefault().PrevisionOfCost;
                                }
                                catch (Exception exc) { }

                                //  کل هزینه پروژه
                                var totalCost = projectsApiDb.ConstructionProjectFinancialData
                                           .FromSqlRaw(@"
                                                  SELECT REPLACE(CellData, ',', '') AS CellData
                                                  FROM ConstructionProjectFinancialData
                                                  WHERE 
                                                  ParentType = 'constructionproject'
                                                  AND RecordType = 'private'
                                                  AND CellIndex > 0
                                                  AND RowIndex = 9
                                                  AND ParentId = {0}
                                                  AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
                                                  SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId 
                                                  FROM ExcelSheetConfigs
                                                  INNER JOIN ConstructionProjects ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                                                  WHERE 
                                                  ExcelSheetConfigs.ReportType = 'financial'
                                                  AND ConstructionProjects.ConstructionProjectId = {0}
                                                  )", item.ConstructionProjectId).Select(x => new RowsDataVM { CellData = x.CellData })
                                           .ToList();

                                var publicTotalcost = totalCost.Sum(x => long.Parse(x.CellData));

                                item.ProjectTotalCost = publicTotalcost;

                                // کل هزینه عمومی 
                                var ProjectPublicTotalCost = projectsApiDb.ConstructionProjectFinancialData
                                                    .FromSqlRaw(@"
                                                     SELECT REPLACE(CellData, ',', '') AS CellData
                                                     FROM ConstructionProjectFinancialData
                                                     WHERE 
                                                     ParentType = 'constructionproject'
                                                     AND RecordType = 'public'
                                                     AND CellIndex > 0
                                                     AND RowIndex = 4
                                                     AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
                                                     SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId 
                                                     FROM ExcelSheetConfigs
                                                     INNER JOIN ConstructionProjects ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                                                     WHERE 
                                                     ExcelSheetConfigs.ReportType = 'financial'
                                                     AND ConstructionProjects.ConstructionProjectId = {0}
                                                     )", item.ConstructionProjectId).Select(x => new RowsDataVM { CellData = x.CellData })
                                                    .ToList();



                                var totalProjectPublicTotalCost = ProjectPublicTotalCost.Sum(x => long.Parse(x.CellData));

                                item.ProjectPublicTotalCost = totalProjectPublicTotalCost;





                                // سهم از هزینه عمومی
                                var rowsData = projectsApiDb.ConstructionProjectFinancialData
                                                .FromSqlRaw(@"
                                                     SELECT ParentId, RowIndex, CellIndex, REPLACE(CellData, ',', '') AS CellData
                                                     FROM ConstructionProjectFinancialData
                                                     WHERE ParentType = 'constructionproject'
                                                     AND RecordType = 'private'
                                                     AND ParentId  = {0}
                                                     AND CellIndex > 0
                                                     And RowIndex = 5
                                                     AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
                                                     SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId
                                                     FROM ExcelSheetConfigs
                                                     INNER JOIN ConstructionProjects
                                                         ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                                                     WHERE ExcelSheetConfigs.ReportType = 'financial'
                                                         AND ConstructionProjects.ConstructionProjectId = {0}
                                                 )", item.ConstructionProjectId).
                                  Select(result => new RowsDataVM
                                  {
                                      RowIndex = result.RowIndex,
                                      CellIndex = result.CellIndex,
                                      CellData = result.CellData,
                                      ParentId = result.ParentId
                                  })
                               .ToList();

                                var totalGeneralCost = rowsData.Sum(x => long.Parse(x.CellData));
                                //عدد
                                item.ProjectShareOfGeneralCost = totalGeneralCost;

                                //درصد
                                //decimal percent = Convert.ToInt64(publicTotalcost / totalGeneralCost) * 100;

                                //decimal percent = (decimal)totalGeneralCost / totalProjectPublicTotalCost;


                                decimal percent = 0;
                                if (totalProjectPublicTotalCost != 0)
                                {
                                    percent = (decimal)totalGeneralCost / totalProjectPublicTotalCost;
                                }
                                else
                                {
                                    percent = 0;
                                }

                                item.ProjectShareOfGeneralCostPercent = Math.Round((percent * 100));



                                // کل هزینه اختصاصی
                                var ProjectPrivateTotalCost = projectsApiDb.ConstructionProjectFinancialData
                                    .FromSqlRaw(@"
                                        SELECT REPLACE(CellData, ',', '') AS CellData
                                        FROM ConstructionProjectFinancialData
                                        WHERE ParentType = 'constructionproject'
                                        AND RecordType = 'private'
                                        AND ParentId  = {0}
                                        AND RowIndex = 10
                                        And CellIndex > 0
                                        AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
                                            SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId
                                            FROM ExcelSheetConfigs
                                            INNER JOIN ConstructionProjects
                                                ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                                            WHERE ExcelSheetConfigs.ReportType = 'financial'
                                                AND ConstructionProjects.ConstructionProjectId = {0}
                                        )", item.ConstructionProjectId).Select(x => new RowsDataVM { CellData = x.CellData })
                                   .ToList();

                                var totalProjectPrivateTotalCost = ProjectPrivateTotalCost.Sum(x => long.Parse(x.CellData));

                                item.ProjectPrivateTotalCost = totalProjectPrivateTotalCost;

                                #region ConstructionProjectProgressData - LastUpdate
                                //آخرین آپدیت گزارشات روزانه


                                //var LastProgessUpdateDate = projectsApiDb.ConstructionProjectProgressData
                                //            .FromSqlRaw(@"
                                //            SELECT CellIndex, CellData AS CellData
                                //            FROM ConstructionProjectProgressData
                                //            WHERE ConstructionProjectId = {0}
                                //            And RowIndex = 1
                                //            --And CellIndex = 1
                                //            AND ConstructionProjectProgressData.ExcelSheetConfigId IN (
                                //            SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId
                                //            FROM ExcelSheetConfigs
                                //            INNER JOIN ConstructionProjects
                                //                ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                                //            WHERE ExcelSheetConfigs.ReportType = 'progress'
                                //                AND ConstructionProjects.ConstructionProjectId = {0})          
                                //             ", item.ConstructionProjectId).ToList();

                                #endregion

                                #endregion

                                #region Related to investor

                       
                                // سهم از کل هزینه پروژه
                                var shareOfTotalProjectCost = projectsApiDb.ConstructionProjectOwnerUsers.Where(x =>
                                x.OwnerUserId.Equals(project.OwnerUserId)
                                && x.ConstructionProjectId.Equals(item.ConstructionProjectId)
                                && x.IsActivated.Value.Equals(true)
                                && x.IsDeleted.Value.Equals(false)).FirstOrDefault();

                                item.ShareOfTotalProjectCostPercent = shareOfTotalProjectCost.SharePercent;
                                item.ShareOfTotalProjectCost = Math.Round(Convert.ToDecimal(item.ProjectTotalCost * (shareOfTotalProjectCost.SharePercent / 100.0)));




                                // سهم پرداختی پروژه
                                var ProjectPaidShare = projectsApiDb.ConstructionProjectFinancialData
                                    .FromSqlRaw(@"
                                         SELECT CellIndex, REPLACE(CellData, ',', '') AS CellData  
                                         FROM (
                                           SELECT *, MAX(CellIndex) OVER() AS MaxCellIndex
                                           FROM ConstructionProjectFinancialData
                                           WHERE ParentType = 'person' 
                                             AND RecordType = 'private'
                                            AND UserIdCreator = {0}
                                               AND ParentId = {1}
                                             AND ExcelSheetConfigId IN (
                                               SELECT ExcelSheetConfigId
                                               FROM ExcelSheetConfigs
                                               INNER JOIN ConstructionProjects
                                                 ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                                               WHERE ExcelSheetConfigs.ReportType = 'financial'
                                                 AND ConstructionProjects.ConstructionProjectId= {2})
                                    ) t
                                        WHERE CellIndex = MaxCellIndex", project.OwnerUserId, item.ConstructionProjectId, item.ConstructionProjectId)
                                    .Select(result => new RowsDataVM
                                    {
                                        CellData = result.CellData
                                    })
                                    .ToList();


                                var totalProjectPaidShare = ProjectPaidShare.Sum(x => long.Parse(x.CellData));

                                item.ProjectPaidShare = totalProjectPaidShare;



                                // مانده هزینه
                                item.ProjectRemainingCost = Math.Round((item.ShareOfTotalProjectCost.Value - totalProjectPaidShare));



                                // سقف تنخواه
                                var fundsCeiling = projectsApiDb.ConstructionProjectFinancialData
                                .FromSqlRaw(@"
                                        SELECT CellIndex, REPLACE(CellData, ',', '') AS CellData
                                        FROM ConstructionProjectFinancialData
                                        WHERE ParentType = 'constructionproject'
                                        AND RecordType = 'private'
                                        AND ParentId  = {0}
                                        And RowIndex = 7
                                        And CellIndex > 0
                                        AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
                                        SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId
                                        FROM ExcelSheetConfigs
                                        INNER JOIN ConstructionProjects
                                            ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                                        WHERE ExcelSheetConfigs.ReportType = 'financial'
                                            AND ConstructionProjects.ConstructionProjectId = {0})        
                                ", item.ConstructionProjectId).Select(x => new RowsDataVM { CellData = x.CellData, CellIndex = x.CellIndex })
                                  .ToList();


                                if (fundsCeiling.Count > 0)
                                {
                                    item.FundsCeiling = Convert.ToInt64(fundsCeiling.LastOrDefault().CellData);

                                    item.FundsCeiling = (long)(item.FundsCeiling.Value * item.ShareOfTotalProjectCostPercent.Value) / 100;

                                    item.RemainingDebt = item.FundsCeiling + Convert.ToInt64(item.ProjectRemainingCost);

                                }
                                else
                                {
                                    item.FundsCeiling = 0;

                                    item.FundsCeiling = 0;

                                    item.RemainingDebt = 0;

                                }




                                //نماینده
                                item.ConstructionProjectRepresentativeId = project.ConstructionProjectRepresentativeId;

                                //سهامدار
                                item.OwnerUserId = project.OwnerUserId;

                                #endregion


                                //سهامداران
                                var ownerUser = projectsApiDb.ConstructionProjectOwnerUsers.Where(c => c.OwnerUserId.Equals(project.OwnerUserId) && c.ConstructionProjectId.Equals(item.ConstructionProjectId)).FirstOrDefault();

                                if (ownerUser != null)
                                {

                                    var userFamily = consoleBusiness.CmsDb.UsersProfile.Where(c => c.UserId.Equals(ownerUser.OwnerUserId)).FirstOrDefault();
                                    if (userFamily != null)
                                    {
                                        if (userFamily.UserId > 0)
                                        {
                                            item.OwnerUserFamiy = userFamily.Name + ' ' + userFamily.Family;
                                        }
                                        else
                                        {
                                            item.OwnerUserFamiy = "";
                                        }
                                    }

                                }

                            }

                        }

                    }
                }

            }
            catch (Exception ex)
            { }
            return constructionProjectsVMList;
        }




        #endregion

        #endregion

        #region Methods For Work With ConstructionProjectTypes

        public List<ConstructionProjectTypesVM> GetAllConstructionProjectTypesList(
        List<long> childsUsersIds)
        {
            List<ConstructionProjectTypesVM> constructionProjectTypesVMList = new List<ConstructionProjectTypesVM>();

            try
            {
                var list = (from c in projectsApiDb.ConstructionProjectTypes
                            where
                                c.IsActivated.Value.Equals(true) &&
                                c.IsDeleted.Value.Equals(false)
                            select new ConstructionProjectTypesVM
                            {
                                ConstructionProjectTypeId = c.ConstructionProjectTypeId,
                                ConstructionProjectTypeTitle = c.ConstructionProjectTypeTitle,
                                UserIdCreator = c.UserIdCreator.Value,
                                CreateEnDate = c.CreateEnDate,
                                CreateTime = c.CreateTime,
                                EditEnDate = c.EditEnDate,
                                EditTime = c.EditTime,
                                UserIdEditor = c.UserIdEditor.Value,
                                RemoveEnDate = c.RemoveEnDate,
                                RemoveTime = c.EditTime,
                                UserIdRemover = c.UserIdRemover.Value,
                                IsActivated = c.IsActivated,
                                IsDeleted = c.IsDeleted,
                            }).AsQueryable();


                if (childsUsersIds != null)
                {
                    if (childsUsersIds.Count > 1)
                    {
                        list = list.Where(c => childsUsersIds.Contains(c.UserIdCreator.Value));
                    }
                    else
                    {
                        if (childsUsersIds.Count == 1)
                        {
                            if (childsUsersIds.FirstOrDefault() > 0)
                            {
                                list = list.Where(c => childsUsersIds.Contains(c.UserIdCreator.Value));
                            }
                        }
                    }
                }

                constructionProjectTypesVMList = list.OrderByDescending(s => s.ConstructionProjectTypeId).ToList();

            }
            catch (Exception exc)
            { }
            return constructionProjectTypesVMList;
        }

        #endregion

        #region Methods For Work With ConfirmationAgreements

        public List<ConfirmationAgreementsVM> GetAllConfirmationAgreementsList(
              List<long> childsUsersIds,
            string? ConfirmationAgreementTitle = "",
            long? ConstructionProjectId = 0)
        {

            List<ConfirmationAgreementsVM> confirmationAgreementsVMList = new List<ConfirmationAgreementsVM>();

            try
            {
                var list = (from p in projectsApiDb.ConfirmationAgreements
                            where childsUsersIds.Contains(p.UserIdCreator.Value) &&
                              p.IsDeleted.Value.Equals(false) &&
                              p.IsActivated.Value.Equals(true)
                            select new ConfirmationAgreementsVM
                            {
                                ConfirmationAgreementId = p.ConfirmationAgreementId,
                                ConfirmationTypeId = p.ConfirmationTypeId,
                                ConfirmationAgreementTitle = p.ConfirmationAgreementTitle,
                                ConfirmationAgreementDescription = p.ConfirmationAgreementDescription,
                                ConfirmationAgreementFileExt = p.ConfirmationAgreementFileExt,
                                ConfirmationAgreementFilePath = p.ConfirmationAgreementFilePath,
                                ConfirmationAgreementNumber = p.ConfirmationAgreementNumber,
                                ConstructionProjectId = p.ConstructionProjectId,
                                ConfirmationAgreementFileOrder = p.ConfirmationAgreementFileOrder,
                                ConfirmationAgreementFileType = p.ConfirmationAgreementFileType,
                                //IsConfirm = p.IsConfirm,
                                //UserId = p.UserId,
                                //ConfirmationDate = p.ConfirmationDate,
                                //ConfirmationTime = p.ConfirmationTime,
                                UserIdCreator = p.UserIdCreator.Value,
                                CreateEnDate = p.CreateEnDate,
                                CreateTime = p.CreateTime,
                                EditEnDate = p.EditEnDate,
                                EditTime = p.EditTime,
                                UserIdEditor = p.UserIdEditor.Value,
                                RemoveEnDate = p.RemoveEnDate,
                                RemoveTime = p.EditTime,
                                UserIdRemover = p.UserIdRemover.Value,
                                IsActivated = p.IsActivated,
                                IsDeleted = p.IsDeleted,

                            })
                               .AsEnumerable();

                if (ConstructionProjectId.HasValue && ConstructionProjectId.Value != 0)
                {
                    list = list.Where(a => a.ConstructionProjectId == ConstructionProjectId);
                }

                if (!string.IsNullOrEmpty(ConfirmationAgreementTitle))
                    list = list.Where(a => a.ConfirmationAgreementTitle.Contains(ConfirmationAgreementTitle));
                confirmationAgreementsVMList = list.OrderByDescending(s => s.ConfirmationAgreementId).ToList();
            }
            catch (Exception exc)
            { }
            return confirmationAgreementsVMList;
        }


        public List<ConfirmationAgreementsVM> GetListOfConfirmationAgreements(int jtStartIndex,
              int jtPageSize,
              ref int listCount,
              List<long> childsUsersIds,
              string? ConfirmationAgreementTitle = "",
              long? ConstructionProjectId = 0,
              string jtSorting = null)
        {

            List<ConfirmationAgreementsVM> confirmationAgreementsVMList = new List<ConfirmationAgreementsVM>();

            try
            {
                var list = (from p in projectsApiDb.ConfirmationAgreements
                            where childsUsersIds.Contains(p.UserIdCreator.Value)
                            select new ConfirmationAgreementsVM
                            {
                                ConfirmationAgreementId = p.ConfirmationAgreementId,
                                ConfirmationTypeId = p.ConfirmationTypeId,
                                ConfirmationAgreementTitle = p.ConfirmationAgreementTitle,
                                ConfirmationAgreementDescription = p.ConfirmationAgreementDescription,
                                ConfirmationAgreementFileExt = p.ConfirmationAgreementFileExt,
                                ConfirmationAgreementFilePath = p.ConfirmationAgreementFilePath,
                                ConfirmationAgreementNumber = p.ConfirmationAgreementNumber,
                                ConstructionProjectId = p.ConstructionProjectId,
                                ConfirmationAgreementFileOrder = p.ConfirmationAgreementFileOrder,
                                ConfirmationAgreementFileType = p.ConfirmationAgreementFileType,
                                //UserId = p.UserId,
                                //IsConfirm = p.IsConfirm,
                                UserIdCreator = p.UserIdCreator.Value,
                                CreateEnDate = p.CreateEnDate,
                                CreateTime = p.CreateTime,
                                EditEnDate = p.EditEnDate,
                                EditTime = p.EditTime,
                                UserIdEditor = p.UserIdEditor.Value,
                                RemoveEnDate = p.RemoveEnDate,
                                RemoveTime = p.EditTime,
                                UserIdRemover = p.UserIdRemover.Value,
                                IsActivated = p.IsActivated,
                                IsDeleted = p.IsDeleted,

                            })
                               .AsEnumerable();

                if (ConstructionProjectId.HasValue && ConstructionProjectId.Value != 0)
                {
                    list = list.Where(a => a.ConstructionProjectId == ConstructionProjectId);
                }

                if (!string.IsNullOrEmpty(ConfirmationAgreementTitle))
                    list = list.Where(a => a.ConfirmationAgreementTitle.Contains(ConfirmationAgreementTitle));

                listCount = list.Count();
                try
                {
                    if (string.IsNullOrEmpty(jtSorting))
                    {

                        if (listCount > jtPageSize)
                        {

                            confirmationAgreementsVMList = list.OrderByDescending(s => s.ConfirmationAgreementId)
                                     .Skip(jtStartIndex).Take(jtPageSize).ToList();
                        }
                        else
                            confirmationAgreementsVMList = list.OrderByDescending(s => s.ConfirmationAgreementId).ToList();
                    }
                    else
                    {


                        switch (jtSorting)
                        {
                            case "ConfirmationAgreementTitle ASC":
                                list = list.OrderBy(l => l.ConfirmationAgreementTitle);
                                break;
                            case "ConfirmationAgreementTitle DESC":
                                list = list.OrderByDescending(l => l.ConfirmationAgreementTitle);
                                break;
                        }
                        if (listCount > jtPageSize)
                        {
                            confirmationAgreementsVMList = list.Skip(jtStartIndex).Take(jtPageSize).ToList();
                        }
                        else
                        {

                            confirmationAgreementsVMList = list.ToList();
                        }
                    }
                }
                catch (Exception exc)
                { }
            }
            catch (Exception exc)
            { }
            return confirmationAgreementsVMList;
        }


        public long AddToConfirmationAgreements(ConfirmationAgreementsVM confirmationAgreementsVM)
        {
            try
            {

                ConfirmationAgreements confirmationAgreement = _mapper.Map<ConfirmationAgreementsVM, ConfirmationAgreements>(confirmationAgreementsVM);


                projectsApiDb.ConfirmationAgreements.Add(confirmationAgreement);
                projectsApiDb.SaveChanges();
                return confirmationAgreement.ConfirmationAgreementId;
            }
            catch (Exception)
            {
            }
            return 0;

        }

        public ConfirmationAgreementsVM UpdateConfirmationAgreements(
              List<long> childsUsersIds,
            ConfirmationAgreementsVM confirmationAgreementsVM)
        {
            var ConfirmationAgreementId = confirmationAgreementsVM.ConfirmationAgreementId;
            if (projectsApiDb.ConfirmationAgreements.Where(x => childsUsersIds.Contains(x.UserIdCreator.Value))
                .Where(x => x.ConfirmationAgreementId.Equals(ConfirmationAgreementId)).Any())
            {
                try
                {
                    bool? isActivated = confirmationAgreementsVM.IsActivated.HasValue ? confirmationAgreementsVM.IsActivated.Value : (bool?)true;
                    bool? isDeleted = confirmationAgreementsVM.IsDeleted.HasValue ? confirmationAgreementsVM.IsDeleted.Value : (bool?)true;

                    ConfirmationAgreements confirmationAgreement = projectsApiDb.ConfirmationAgreements.Where(a => a.ConfirmationAgreementId == ConfirmationAgreementId).FirstOrDefault();
                    confirmationAgreement.ConfirmationAgreementTitle = confirmationAgreementsVM.ConfirmationAgreementTitle;
                    confirmationAgreement.ConfirmationTypeId = confirmationAgreementsVM.ConfirmationTypeId;
                    //confirmationAgreement.ConfirmationAgreementFileExt = confirmationAgreementsVM.ConfirmationAgreementFileExt;
                    //confirmationAgreement.ConfirmationAgreementFilePath = confirmationAgreementsVM.ConfirmationAgreementFilePath;
                    confirmationAgreement.ConfirmationAgreementFileOrder = confirmationAgreementsVM.ConfirmationAgreementFileOrder;
                    confirmationAgreement.ConfirmationAgreementNumber = confirmationAgreementsVM.ConfirmationAgreementNumber.Value;
                    confirmationAgreement.ConfirmationAgreementDescription = confirmationAgreementsVM.ConfirmationAgreementDescription;
                    confirmationAgreement.EditTime = confirmationAgreementsVM.EditTime;
                    confirmationAgreement.EditEnDate = confirmationAgreementsVM.EditEnDate;
                    confirmationAgreement.UserIdEditor = confirmationAgreementsVM.UserIdEditor;
                    confirmationAgreement.IsActivated = isActivated;
                    confirmationAgreement.IsDeleted = isDeleted;

                    projectsApiDb.Entry<ConfirmationAgreements>(confirmationAgreement).State = EntityState.Modified;

                    projectsApiDb.SaveChanges();
                    return _mapper.Map<ConfirmationAgreements, ConfirmationAgreementsVM>(confirmationAgreement);
                }
                catch (Exception)
                {

                }
            }
            return null;
        }

        public bool ToggleActivationConfirmationAgreements(long confirmationAgreementId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var confirmationAgreements = (from c in projectsApiDb.ConfirmationAgreements
                                              where c.ConfirmationAgreementId == confirmationAgreementId &&
                                              childsUsersIds.Contains(c.UserIdCreator.Value)
                                              select c).FirstOrDefault();

                if (confirmationAgreements != null)
                {
                    confirmationAgreements.IsActivated = !confirmationAgreements.IsActivated;
                    confirmationAgreements.EditEnDate = DateTime.Now;
                    confirmationAgreements.EditTime = PersianDate.TimeNow;
                    confirmationAgreements.UserIdEditor = userId;

                    projectsApiDb.Entry<Entities.ConfirmationAgreements>(confirmationAgreements).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool TemporaryDeleteConfirmationAgreements(long confirmationAgreementId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var confirmationAgreements = (from c in projectsApiDb.ConfirmationAgreements
                                              where c.ConfirmationAgreementId == confirmationAgreementId &&
                                              childsUsersIds.Contains(c.UserIdCreator.Value)
                                              select c).FirstOrDefault();

                if (confirmationAgreements != null)
                {
                    confirmationAgreements.IsDeleted = !confirmationAgreements.IsDeleted;
                    confirmationAgreements.EditEnDate = DateTime.Now;
                    confirmationAgreements.EditTime = PersianDate.TimeNow;
                    confirmationAgreements.UserIdEditor = userId;

                    projectsApiDb.Entry<Entities.ConfirmationAgreements>(confirmationAgreements).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool? CompleteDeleteConfirmationAgreements(long confirmationAgreementId,
            long userId,
            List<long> childsUsersIds)
        {
            var haveAttachements = (from a in projectsApiDb.AttachementFiles
                                    where a.AttachementTableTitle == "ConfirmationAgreements" &&
                                    a.AttachementParentId == confirmationAgreementId
                                    select a).Any();
            if (haveAttachements)
            {
                return null;
            }
            try
            {
                var confirmationAgreement = (from c in projectsApiDb.ConfirmationAgreements
                                             where c.ConfirmationAgreementId == confirmationAgreementId &&
                                             childsUsersIds.Contains(c.UserIdCreator.Value)
                                             select c).FirstOrDefault();

                if (confirmationAgreement != null)
                {
                    projectsApiDb.ConfirmationAgreements.Remove(confirmationAgreement);
                    projectsApiDb.SaveChanges();


                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;


        }

        public ConfirmationAgreementsVM GetConfirmationAgreementsWithConfirmationAgreementId(long confirmationAgreementId,
            long userId,
            List<long> childsUsersIds)
        {
            ConfirmationAgreementsVM confirmationAgreementsVM = new ConfirmationAgreementsVM();

            try
            {
                confirmationAgreementsVM = _mapper.Map<ConfirmationAgreements,
                    ConfirmationAgreementsVM>(projectsApiDb.ConfirmationAgreements
                    .Where(p => childsUsersIds.Contains(p.UserIdCreator.Value))
                     .Where(e => e.ConfirmationAgreementId.Equals(confirmationAgreementId)).FirstOrDefault());

            }
            catch (Exception exc)
            { }

            return confirmationAgreementsVM;
        }

        //public bool ConfirmConfirmationAgreements(int confirmationAgreementId,
        //    long userId,
        //    List<long> childsUsersIds)
        //{
        //    try
        //    {
        //        var confirmationAgreements = (from c in projectsApiDb.ConfirmationAgreements
        //                                      where c.ConfirmationAgreementId == confirmationAgreementId &&
        //                                      childsUsersIds.Contains(c.UserIdCreator.Value)
        //                                      select c).FirstOrDefault();
        //        if (confirmationAgreements != null)
        //        {
        //            var isConfirm = confirmationAgreements.IsConfirm ?? false;
        //            confirmationAgreements.IsConfirm = !isConfirm;
        //            confirmationAgreements.ConfirmationDate = DateTime.Now;
        //            confirmationAgreements.ConfirmationTime = PersianDate.TimeNow;
        //            confirmationAgreements.UserId = userId;

        //            projectsApiDb.Entry<Entities.ConfirmationAgreements>(confirmationAgreements).State = EntityState.Modified;
        //            projectsApiDb.SaveChanges();

        //            return true;
        //        }
        //    }
        //    catch (Exception exc)
        //    { }

        //    return false;
        //}

        #endregion

        #region Methods For Work With ConfirmationTypes
        public List<ConfirmationTypesVM> GetAllConfirmationTypesList()
        {

            List<ConfirmationTypesVM> confirmationTypesVMList = new List<ConfirmationTypesVM>();

            try
            {
                var list = (from p in projectsApiDb.ConfirmationTypes
                            select new ConfirmationTypesVM
                            {
                                ConfirmationTypeId = p.ConfirmationTypeId,
                                ConfirmationTypeTitle = p.ConfirmationTypeTitle,
                                UserIdCreator = p.UserIdCreator.Value,
                                CreateEnDate = p.CreateEnDate,
                                CreateTime = p.CreateTime,
                                EditEnDate = p.EditEnDate,
                                EditTime = p.EditTime,
                                UserIdEditor = p.UserIdEditor.Value,
                                RemoveEnDate = p.RemoveEnDate,
                                RemoveTime = p.EditTime,
                                UserIdRemover = p.UserIdRemover.Value,
                                IsActivated = p.IsActivated,
                                IsDeleted = p.IsDeleted,

                            })
                               .AsEnumerable();

                confirmationTypesVMList = list.OrderByDescending(s => s.ConfirmationTypeId).ToList();
            }
            catch (Exception exc)
            { }
            return confirmationTypesVMList;
        }
        #endregion

        #region Methods For Work With ContractAgreements

        public List<ContractAgreementsVM> GetAllContractAgreementsList(
              List<long> childsUsersIds,
            string? ContractAgreementTitle = "",
            long? ConstructionProjectId = 0)
        {

            List<ContractAgreementsVM> contractAgreementsVMList = new List<ContractAgreementsVM>();

            try
            {
                var list = (from p in projectsApiDb.ContractAgreements
                            where childsUsersIds.Contains(p.UserIdCreator.Value) &&
                           p.IsDeleted.Value.Equals(false) &&
                           p.IsActivated.Value.Equals(true)
                            select new ContractAgreementsVM
                            {
                                ContractAgreementId = p.ContractAgreementId,
                                ContractAgreementTitle = p.ContractAgreementTitle,
                                ContractAgreementDescription = p.ContractAgreementDescription,
                                ContractAgreementFileExt = p.ContractAgreementFileExt,
                                ContractAgreementFilePath = p.ContractAgreementFilePath,
                                ContractAgreementNumber = p.ContractAgreementNumber,
                                ConstructionProjectId = p.ConstructionProjectId,
                                ContractAgreementFileOrder = p.ContractAgreementFileOrder,
                                ContractAgreementFileType = p.ContractAgreementFileType,
                                UserIdCreator = p.UserIdCreator.Value,
                                CreateEnDate = p.CreateEnDate,
                                CreateTime = p.CreateTime,
                                EditEnDate = p.EditEnDate,
                                EditTime = p.EditTime,
                                UserIdEditor = p.UserIdEditor.Value,
                                RemoveEnDate = p.RemoveEnDate,
                                RemoveTime = p.EditTime,
                                UserIdRemover = p.UserIdRemover.Value,
                                IsActivated = p.IsActivated,
                                IsDeleted = p.IsDeleted,

                            })
                               .AsEnumerable();

                if (ConstructionProjectId.HasValue && ConstructionProjectId.Value != 0)
                {
                    list = list.Where(a => a.ConstructionProjectId == ConstructionProjectId);
                }

                if (!string.IsNullOrEmpty(ContractAgreementTitle))
                    list = list.Where(a => a.ContractAgreementTitle.Contains(ContractAgreementTitle));
                contractAgreementsVMList = list.OrderByDescending(s => s.ContractAgreementId).ToList();
            }
            catch (Exception exc)
            { }
            return contractAgreementsVMList;
        }
        public List<ContractAgreementsVM> GetListOfContractAgreements(int jtStartIndex,
              int jtPageSize,
              ref int listCount,
              List<long> childsUsersIds,
              string? ContractAgreementTitle = "",
              long? ConstructionProjectId = 0,
              string jtSorting = null)
        {

            List<ContractAgreementsVM> contractAgreementsVMList = new List<ContractAgreementsVM>();

            try
            {
                var list = (from p in projectsApiDb.ContractAgreements
                            where childsUsersIds.Contains(p.UserIdCreator.Value)
                            select new ContractAgreementsVM
                            {
                                ContractAgreementId = p.ContractAgreementId,
                                ContractAgreementTitle = p.ContractAgreementTitle,
                                ContractAgreementDescription = p.ContractAgreementDescription,
                                ContractAgreementFileExt = p.ContractAgreementFileExt,
                                ContractAgreementFilePath = p.ContractAgreementFilePath,
                                ContractAgreementNumber = p.ContractAgreementNumber,
                                ConstructionProjectId = p.ConstructionProjectId,
                                ContractAgreementFileOrder = p.ContractAgreementFileOrder,
                                ContractAgreementFileType = p.ContractAgreementFileType,
                                UserIdCreator = p.UserIdCreator.Value,
                                CreateEnDate = p.CreateEnDate,
                                CreateTime = p.CreateTime,
                                EditEnDate = p.EditEnDate,
                                EditTime = p.EditTime,
                                UserIdEditor = p.UserIdEditor.Value,
                                RemoveEnDate = p.RemoveEnDate,
                                RemoveTime = p.EditTime,
                                UserIdRemover = p.UserIdRemover.Value,
                                IsActivated = p.IsActivated,
                                IsDeleted = p.IsDeleted,

                            })
                               .AsEnumerable();


                if (ConstructionProjectId.HasValue && ConstructionProjectId.Value != 0)
                {
                    list = list.Where(a => a.ConstructionProjectId == ConstructionProjectId);
                }

                if (!string.IsNullOrEmpty(ContractAgreementTitle))
                    list = list.Where(a => a.ContractAgreementTitle.Contains(ContractAgreementTitle));

                listCount = list.Count();
                try
                {
                    if (string.IsNullOrEmpty(jtSorting))
                    {

                        if (listCount > jtPageSize)
                        {

                            contractAgreementsVMList = list.OrderByDescending(s => s.ContractAgreementId)
                                     .Skip(jtStartIndex).Take(jtPageSize).ToList();
                        }
                        else
                            contractAgreementsVMList = list.OrderByDescending(s => s.ContractAgreementId).ToList();
                    }
                    else
                    {


                        switch (jtSorting)
                        {
                            case "ContractAgreementTitle ASC":
                                list = list.OrderBy(l => l.ContractAgreementTitle);
                                break;
                            case "ContractAgreementTitle DESC":
                                list = list.OrderByDescending(l => l.ContractAgreementTitle);
                                break;
                        }
                        if (listCount > jtPageSize)
                        {
                            contractAgreementsVMList = list.Skip(jtStartIndex).Take(jtPageSize).ToList();
                        }
                        else
                        {

                            contractAgreementsVMList = list.ToList();
                        }
                    }
                }
                catch (Exception exc)
                { }
            }
            catch (Exception exc)
            { }
            return contractAgreementsVMList;
        }


        public long AddToContractAgreements(ContractAgreementsVM contractAgreementsVM)
        {
            try
            {

                ContractAgreements contractAgreement = _mapper.Map<ContractAgreementsVM, ContractAgreements>(contractAgreementsVM);


                projectsApiDb.ContractAgreements.Add(contractAgreement);
                projectsApiDb.SaveChanges();
                return contractAgreement.ContractAgreementId;
            }
            catch (Exception)
            {
            }
            return 0;

        }

        public ContractAgreementsVM UpdateContractAgreements(
              List<long> childsUsersIds,
            ContractAgreementsVM contractAgreementsVM)
        {
            var ContractAgreementId = contractAgreementsVM.ContractAgreementId;
            if (projectsApiDb.ContractAgreements.Where(x => childsUsersIds.Contains(x.UserIdCreator.Value))
                .Where(x => x.ContractAgreementId.Equals(ContractAgreementId)).Any())
            {
                try
                {
                    bool? isActivated = contractAgreementsVM.IsActivated.HasValue ? contractAgreementsVM.IsActivated.Value : (bool?)true;
                    bool? isDeleted = contractAgreementsVM.IsDeleted.HasValue ? contractAgreementsVM.IsDeleted.Value : (bool?)true;

                    ContractAgreements contractAgreement = projectsApiDb.ContractAgreements.Where(a => a.ContractAgreementId == ContractAgreementId).FirstOrDefault();
                    contractAgreement.ContractAgreementTitle = contractAgreementsVM.ContractAgreementTitle;
                    //contractAgreement.ContractAgreementFileExt = contractAgreementsVM.ContractAgreementFileExt;
                    //contractAgreement.ContractAgreementFilePath = contractAgreementsVM.ContractAgreementFilePath;
                    contractAgreement.ContractAgreementFileOrder = contractAgreementsVM.ContractAgreementFileOrder;
                    contractAgreement.ContractAgreementNumber = contractAgreementsVM.ContractAgreementNumber.Value;
                    contractAgreement.ContractAgreementDescription = contractAgreementsVM.ContractAgreementDescription;
                    contractAgreement.EditTime = contractAgreementsVM.EditTime;
                    contractAgreement.EditEnDate = contractAgreementsVM.EditEnDate;
                    contractAgreement.UserIdEditor = contractAgreementsVM.UserIdEditor;
                    contractAgreement.IsActivated = isActivated;
                    contractAgreement.IsDeleted = isDeleted;

                    projectsApiDb.Entry<ContractAgreements>(contractAgreement).State = EntityState.Modified;

                    projectsApiDb.SaveChanges();
                    return _mapper.Map<ContractAgreements, ContractAgreementsVM>(contractAgreement);
                }
                catch (Exception)
                {

                }
            }
            return null;
        }

        public bool ToggleActivationContractAgreements(long contractAgreementId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var contractAgreements = (from c in projectsApiDb.ContractAgreements
                                          where c.ContractAgreementId == contractAgreementId &&
                                          childsUsersIds.Contains(c.UserIdCreator.Value)
                                          select c).FirstOrDefault();

                if (contractAgreements != null)
                {
                    contractAgreements.IsActivated = !contractAgreements.IsActivated;
                    contractAgreements.EditEnDate = DateTime.Now;
                    contractAgreements.EditTime = PersianDate.TimeNow;
                    contractAgreements.UserIdEditor = userId;

                    projectsApiDb.Entry<Entities.ContractAgreements>(contractAgreements).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool TemporaryDeleteContractAgreements(long contractAgreementId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var contractAgreements = (from c in projectsApiDb.ContractAgreements
                                          where c.ContractAgreementId == contractAgreementId &&
                                          childsUsersIds.Contains(c.UserIdCreator.Value)
                                          select c).FirstOrDefault();

                if (contractAgreements != null)
                {
                    contractAgreements.IsDeleted = !contractAgreements.IsDeleted;
                    contractAgreements.EditEnDate = DateTime.Now;
                    contractAgreements.EditTime = PersianDate.TimeNow;
                    contractAgreements.UserIdEditor = userId;

                    projectsApiDb.Entry<Entities.ContractAgreements>(contractAgreements).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool? CompleteDeleteContractAgreements(long contractAgreementId,
            long userId,
            List<long> childsUsersIds)
        {

            try
            {

                #region Attachemments
                var haveAttachements = (from a in projectsApiDb.AttachementFiles
                                        where a.AttachementTableTitle == "ContractAgreements" &&
                                        a.AttachementParentId == contractAgreementId
                                        select a).Any();
                if (haveAttachements)
                {
                    return null;
                }
                #endregion

                #region conversations

                var conversations = projectsApiDb.ConversationLogs.Where
                            (f => f.RecordId.Equals(contractAgreementId) && f.TableTitle.Equals("ContractAgreement")).ToList();



                if (conversations != null)
                {
                    projectsApiDb.ConversationLogs.RemoveRange(conversations);
                    projectsApiDb.SaveChanges();
                }


                #endregion

                #region contractAgreement

                var contractAgreement = (from c in projectsApiDb.ContractAgreements
                                         where c.ContractAgreementId == contractAgreementId &&
                                         childsUsersIds.Contains(c.UserIdCreator.Value)
                                         select c).FirstOrDefault();

                if (contractAgreement != null)
                {
                    projectsApiDb.ContractAgreements.Remove(contractAgreement);
                    projectsApiDb.SaveChanges();


                    return true;
                }
                #endregion

            }
            catch (Exception exc)
            { }

            return false;


        }

        public ContractAgreementsVM GetContractAgreementsWithContractAgreementId(long contractAgreementId,
            long userId,
            List<long> childsUsersIds)
        {
            ContractAgreementsVM contractAgreementsVM = new ContractAgreementsVM();

            try
            {
                contractAgreementsVM = _mapper.Map<ContractAgreements,
                    ContractAgreementsVM>(projectsApiDb.ContractAgreements
                    .Where(p => childsUsersIds.Contains(p.UserIdCreator.Value))
                     .Where(e => e.ContractAgreementId.Equals(contractAgreementId)).FirstOrDefault());

            }
            catch (Exception exc)
            { }

            return contractAgreementsVM;
        }
        #endregion

        #region Methods For Work With ContractSides

        public List<ContractSidesVM> GetAllContractSidesList(
              List<long> childsUsersIds,
              long? ParentId = 0,
              long? TableRecordId = 0,
              string? TableTitle = "")
        {

            List<ContractSidesVM> contractSidesVMList = new List<ContractSidesVM>();

            try
            {
                var list = (from p in projectsApiDb.ContractSides
                            where childsUsersIds.Contains(p.UserIdCreator.Value) &&
                           p.IsDeleted.Value.Equals(false) &&
                           p.IsActivated.Value.Equals(true)
                            select new ContractSidesVM
                            {
                                ContractSideId = p.ContractSideId,
                                ParentId = p.ParentId,
                                TableRecordId = p.TableRecordId,
                                TableTitle = p.TableTitle,
                                ContractSideTypeId = p.ContractSideTypeId,
                                UserIdCreator = p.UserIdCreator.Value,
                                CreateEnDate = p.CreateEnDate,
                                CreateTime = p.CreateTime,
                                EditEnDate = p.EditEnDate,
                                EditTime = p.EditTime,
                                UserIdEditor = p.UserIdEditor.Value,
                                RemoveEnDate = p.RemoveEnDate,
                                RemoveTime = p.EditTime,
                                UserIdRemover = p.UserIdRemover.Value,
                                IsActivated = p.IsActivated,
                                IsDeleted = p.IsDeleted,

                            })
                               .AsEnumerable();

                if (ParentId.HasValue && ParentId.Value != 0)
                {
                    list = list.Where(a => a.ParentId == ParentId);
                }

                if (TableRecordId.HasValue && TableRecordId.Value != 0)
                {
                    list = list.Where(a => a.TableRecordId == TableRecordId);
                }

                if (!string.IsNullOrEmpty(TableTitle))
                    list = list.Where(a => a.TableTitle.Equals(TableTitle));
                contractSidesVMList = list.OrderByDescending(s => s.ContractSideId).ToList();
            }
            catch (Exception exc)
            { }
            return contractSidesVMList;
        }
        public List<ContractSidesVM> GetListOfContractSides(int jtStartIndex,
              int jtPageSize,
              ref int listCount,
              List<long> childsUsersIds,
              long? ParentId = 0,
              long? TableRecordId = 0,
              string? TableTitle = "",
              string jtSorting = null)
        {

            List<ContractSidesVM> contractSidesVMList = new List<ContractSidesVM>();

            try
            {
                var list = (from p in projectsApiDb.ContractSides
                            where childsUsersIds.Contains(p.UserIdCreator.Value) &&
                           p.IsDeleted.Value.Equals(false) &&
                           p.IsActivated.Value.Equals(true)
                            select new ContractSidesVM
                            {

                                ContractSideId = p.ContractSideId,
                                ParentId = p.ParentId,
                                TableRecordId = p.TableRecordId,
                                TableTitle = p.TableTitle,
                                ContractSideTypeId = p.ContractSideTypeId,
                                UserIdCreator = p.UserIdCreator.Value,
                                CreateEnDate = p.CreateEnDate,
                                CreateTime = p.CreateTime,
                                EditEnDate = p.EditEnDate,
                                EditTime = p.EditTime,
                                UserIdEditor = p.UserIdEditor.Value,
                                RemoveEnDate = p.RemoveEnDate,
                                RemoveTime = p.EditTime,
                                UserIdRemover = p.UserIdRemover.Value,
                                IsActivated = p.IsActivated,
                                IsDeleted = p.IsDeleted,


                            })
                               .AsEnumerable();

                if (ParentId.HasValue && ParentId.Value != 0)
                {
                    list = list.Where(a => a.ParentId == ParentId);
                }

                if (TableRecordId.HasValue && TableRecordId.Value != 0)
                {
                    list = list.Where(a => a.TableRecordId == TableRecordId);
                }

                if (!string.IsNullOrEmpty(TableTitle))
                    list = list.Where(a => a.TableTitle.Equals(TableTitle));

                listCount = list.Count();
                try
                {


                    contractSidesVMList = list.ToList();


                }
                catch (Exception exc)
                { }
            }
            catch (Exception exc)
            { }
            return contractSidesVMList;
        }


        public long AddToContractSides(ContractSidesVM contractSidesVM)
        {
            try
            {

                ContractSides contractSide = _mapper.Map<ContractSidesVM, ContractSides>(contractSidesVM);


                projectsApiDb.ContractSides.Add(contractSide);
                projectsApiDb.SaveChanges();
                return contractSide.ContractSideId;
            }
            catch (Exception)
            {
            }
            return 0;

        }

        public ContractSidesVM UpdateContractSides(
              List<long> childsUsersIds,
            ContractSidesVM contractSidesVM)
        {
            var ContractSideId = contractSidesVM.ContractSideId;
            if (projectsApiDb.ContractSides.Where(x => childsUsersIds.Contains(x.UserIdCreator.Value))
                .Where(x => x.ContractSideId.Equals(ContractSideId)).Any())
            {
                try
                {
                    bool? isActivated = contractSidesVM.IsActivated.HasValue ? contractSidesVM.IsActivated.Value : (bool?)true;
                    bool? isDeleted = contractSidesVM.IsDeleted.HasValue ? contractSidesVM.IsDeleted.Value : (bool?)true;

                    ContractSides contractSide = projectsApiDb.ContractSides.Where(a => a.ContractSideId == ContractSideId).FirstOrDefault();
                    contractSide.ParentId = contractSidesVM.ParentId;
                    contractSide.TableRecordId = contractSidesVM.TableRecordId;
                    contractSide.TableTitle = contractSidesVM.TableTitle;
                    contractSide.ContractSideTypeId = contractSidesVM.ContractSideTypeId;
                    contractSide.EditTime = contractSidesVM.EditTime;
                    contractSide.EditEnDate = contractSidesVM.EditEnDate;
                    contractSide.UserIdEditor = contractSidesVM.UserIdEditor;
                    contractSide.IsActivated = isActivated;
                    contractSide.IsDeleted = isDeleted;

                    projectsApiDb.Entry<ContractSides>(contractSide).State = EntityState.Modified;

                    projectsApiDb.SaveChanges();
                    return _mapper.Map<ContractSides, ContractSidesVM>(contractSide);
                }
                catch (Exception)
                {

                }
            }
            return null;
        }

        public bool ToggleActivationContractSides(long contractSideId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var contractSides = (from c in projectsApiDb.ContractSides
                                     where c.ContractSideId == contractSideId &&
                                     childsUsersIds.Contains(c.UserIdCreator.Value)
                                     select c).FirstOrDefault();

                if (contractSides != null)
                {
                    contractSides.IsActivated = !contractSides.IsActivated;
                    contractSides.EditEnDate = DateTime.Now;
                    contractSides.EditTime = PersianDate.TimeNow;
                    contractSides.UserIdEditor = userId;

                    projectsApiDb.Entry<Entities.ContractSides>(contractSides).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool TemporaryDeleteContractSides(long contractSideId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var contractSides = (from c in projectsApiDb.ContractSides
                                     where c.ContractSideId == contractSideId &&
                                     childsUsersIds.Contains(c.UserIdCreator.Value)
                                     select c).FirstOrDefault();

                if (contractSides != null)
                {
                    contractSides.IsDeleted = !contractSides.IsDeleted;
                    contractSides.EditEnDate = DateTime.Now;
                    contractSides.EditTime = PersianDate.TimeNow;
                    contractSides.UserIdEditor = userId;

                    projectsApiDb.Entry<Entities.ContractSides>(contractSides).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool CompleteDeleteContractSides(long contractSideId,
            long userId,
            List<long> childsUsersIds)
        {

            try
            {
                var contractSide = (from c in projectsApiDb.ContractSides
                                    where c.ContractSideId == contractSideId &&
                                    childsUsersIds.Contains(c.UserIdCreator.Value)
                                    select c).FirstOrDefault();

                if (contractSide != null)
                {
                    projectsApiDb.ContractSides.Remove(contractSide);
                    projectsApiDb.SaveChanges();


                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;


        }

        public ContractSidesVM GetContractSidesWithContractSideId(long contractSideId,
            long userId,
            List<long> childsUsersIds)
        {
            ContractSidesVM contractSidesVM = new ContractSidesVM();

            try
            {
                contractSidesVM = _mapper.Map<ContractSides,
                    ContractSidesVM>(projectsApiDb.ContractSides
                    .Where(p => childsUsersIds.Contains(p.UserIdCreator.Value))
                     .Where(e => e.ContractSideId.Equals(contractSideId)).FirstOrDefault());

            }
            catch (Exception exc)
            { }

            return contractSidesVM;
        }
        #endregion

        #region Methods For Work With ConstructionProjectDailyData

        public List<ConstructionProjectDailyDataVM> GetAllConstructionProjectDailyDataList(long? constructionProjectId)
        {
            List<ConstructionProjectDailyDataVM> ConstructionProjectDailyDataVMList = new List<ConstructionProjectDailyDataVM>();

            try
            {
                var constructionProjectDailyDataList = projectsApiDb.ConstructionProjectDailyData.AsQueryable();

                if (constructionProjectId.HasValue)
                    if (constructionProjectId.Value > 0)
                        constructionProjectDailyDataList = constructionProjectDailyDataList.Where(d => d.ConstructionProjectId.Equals(constructionProjectId.Value));

                ConstructionProjectDailyDataVMList = _mapper.Map<List<ConstructionProjectDailyData>, List<ConstructionProjectDailyDataVM>>(constructionProjectDailyDataList.ToList());
            }
            catch (Exception exc)
            { }

            return ConstructionProjectDailyDataVMList;
        }

        public List<ConstructionProjectDailyDataVM> GetListOfConstructionProjectDailyData(int jtStartIndex,
              int jtPageSize,
              ref int listCount,
              long? constructionProjectId,
              string jtSorting = null)
        {
            List<ConstructionProjectDailyDataVM> ConstructionProjectDailyDataVMList = new List<ConstructionProjectDailyDataVM>();

            try
            {
                var list = projectsApiDb.ConstructionProjectDailyData.AsQueryable();

                if (constructionProjectId.HasValue)
                    if (constructionProjectId.Value > 0)
                        list = list.Where(d => d.ConstructionProjectId.Equals(constructionProjectId.Value));


                if (listCount > jtPageSize)
                {
                    ConstructionProjectDailyDataVMList = _mapper.Map<List<ConstructionProjectDailyData>, List<ConstructionProjectDailyDataVM>>(
                        list.OrderByDescending(s => s.ConstructionProjectDailyDataId)
                        .Skip(jtStartIndex).Take(jtPageSize).ToList());
                }
                else
                    ConstructionProjectDailyDataVMList = _mapper.Map<List<ConstructionProjectDailyData>, List<ConstructionProjectDailyDataVM>>(
                        list.OrderByDescending(s => s.ConstructionProjectDailyDataId).ToList());
            }
            catch (Exception exc)
            { }

            return ConstructionProjectDailyDataVMList;
        }

        public long AddToConstructionProjectDailyData(ConstructionProjectDailyDataVM constructionProjectDailyDataVM)
        {
            try
            { }
            catch (Exception exc)
            { }

            return 0;
        }

        public bool GroupAddToConstructionProjectDailyData(List<ConstructionProjectDailyDataVM> constructionProjectDailyDataVMList)
        {
            try
            { }
            catch (Exception exc)
            { }

            return false;
        }

        public bool UpdateConstructionProjectDailyData(ConstructionProjectDailyDataVM constructionProjectDailyDataVM)
        {
            try
            { }
            catch (Exception exc)
            { }

            return false;
        }

        public bool ToggleActivationConstructionProjectDailyData(long constructionProjectDailyDataId, long userId)
        {
            try
            { }
            catch (Exception exc)
            { }

            return false;
        }

        public bool TemporaryDeleteConstructionProjectDailyData(long constructionProjectDailyDataId, long userId)
        {
            try
            { }
            catch (Exception exc)
            { }

            return false;
        }

        public bool CompleteDeleteConstructionProjectDailyData(long constructionProjectDailyDataId)
        {
            try
            { }
            catch (Exception exc)
            { }

            return false;
        }

        public bool GroupCompleteDeleteConstructionProjectDailyData(long constructionProjectId)
        {
            try
            { }
            catch (Exception exc)
            { }

            return false;
        }

        public ConstructionProjectDailyDataVM GetDailyDataWithDailyDataId(long constructionProjectDailyDataId)
        {
            ConstructionProjectDailyDataVM constructionProjectDailyDataVM = new ConstructionProjectDailyDataVM();

            try
            { }
            catch (Exception exc)
            { }

            return constructionProjectDailyDataVM;
        }

        #endregion

        #region Methods For Work With ContractorsAgreements

        public List<ContractorsAgreementsVM> GetAllContractorsAgreementsList(
              List<long> childsUsersIds,
            string? ContractorsAgreementTitle = "",
            long? ConstructionProjectId = 0)
        {

            List<ContractorsAgreementsVM> contractorsAgreementsVMList = new List<ContractorsAgreementsVM>();

            try
            {
                var list = (from p in projectsApiDb.ContractorsAgreements
                            where childsUsersIds.Contains(p.UserIdCreator.Value) &&
                           p.IsDeleted.Value.Equals(false) &&
                           p.IsActivated.Value.Equals(true)
                            select new ContractorsAgreementsVM
                            {
                                ContractorsAgreementId = p.ContractorsAgreementId,
                                ContractorsAgreementTitle = p.ContractorsAgreementTitle,
                                ContractorsAgreementDescription = p.ContractorsAgreementDescription,
                                ContractorsAgreementFileExt = p.ContractorsAgreementFileExt,
                                ContractorsAgreementFilePath = p.ContractorsAgreementFilePath,
                                ContractorsAgreementNumber = p.ContractorsAgreementNumber,
                                ConstructionProjectId = p.ConstructionProjectId,
                                ContractorsAgreementFileOrder = p.ContractorsAgreementFileOrder,
                                ContractorsAgreementFileType = p.ContractorsAgreementFileType,
                                UserIdCreator = p.UserIdCreator.Value,
                                CreateEnDate = p.CreateEnDate,
                                CreateTime = p.CreateTime,
                                EditEnDate = p.EditEnDate,
                                EditTime = p.EditTime,
                                UserIdEditor = p.UserIdEditor.Value,
                                RemoveEnDate = p.RemoveEnDate,
                                RemoveTime = p.EditTime,
                                UserIdRemover = p.UserIdRemover.Value,
                                IsActivated = p.IsActivated,
                                IsDeleted = p.IsDeleted,

                            })
                               .AsEnumerable();

                if (ConstructionProjectId.HasValue && ConstructionProjectId.Value != 0)
                {
                    list = list.Where(a => a.ConstructionProjectId == ConstructionProjectId);
                }

                if (!string.IsNullOrEmpty(ContractorsAgreementTitle))
                    list = list.Where(a => a.ContractorsAgreementTitle.Contains(ContractorsAgreementTitle));
                contractorsAgreementsVMList = list.OrderByDescending(s => s.ContractorsAgreementId).ToList();
            }
            catch (Exception exc)
            { }
            return contractorsAgreementsVMList;
        }
        public List<ContractorsAgreementsVM> GetListOfContractorsAgreements(int jtStartIndex,
              int jtPageSize,
              ref int listCount,
              List<long> childsUsersIds,
              string? ContractorsAgreementTitle = "",
              long? ConstructionProjectId = 0,
              string jtSorting = null)
        {

            List<ContractorsAgreementsVM> contractorsAgreementsVMList = new List<ContractorsAgreementsVM>();

            try
            {
                var list = (from p in projectsApiDb.ContractorsAgreements
                            where childsUsersIds.Contains(p.UserIdCreator.Value)
                            select new ContractorsAgreementsVM
                            {
                                ContractorsAgreementId = p.ContractorsAgreementId,
                                ContractorsAgreementTitle = p.ContractorsAgreementTitle,
                                ContractorsAgreementDescription = p.ContractorsAgreementDescription,
                                ContractorsAgreementFileExt = p.ContractorsAgreementFileExt,
                                ContractorsAgreementFilePath = p.ContractorsAgreementFilePath,
                                ContractorsAgreementNumber = p.ContractorsAgreementNumber,
                                ConstructionProjectId = p.ConstructionProjectId,
                                ContractorsAgreementFileOrder = p.ContractorsAgreementFileOrder,
                                ContractorsAgreementFileType = p.ContractorsAgreementFileType,
                                UserIdCreator = p.UserIdCreator.Value,
                                CreateEnDate = p.CreateEnDate,
                                CreateTime = p.CreateTime,
                                EditEnDate = p.EditEnDate,
                                EditTime = p.EditTime,
                                UserIdEditor = p.UserIdEditor.Value,
                                RemoveEnDate = p.RemoveEnDate,
                                RemoveTime = p.EditTime,
                                UserIdRemover = p.UserIdRemover.Value,
                                IsActivated = p.IsActivated,
                                IsDeleted = p.IsDeleted,

                            })
                               .AsEnumerable();

                if (ConstructionProjectId.HasValue && ConstructionProjectId.Value != 0)
                {
                    list = list.Where(a => a.ConstructionProjectId == ConstructionProjectId);
                }

                if (!string.IsNullOrEmpty(ContractorsAgreementTitle))
                    list = list.Where(a => a.ContractorsAgreementTitle.Contains(ContractorsAgreementTitle));

                listCount = list.Count();
                try
                {
                    if (string.IsNullOrEmpty(jtSorting))
                    {

                        if (listCount > jtPageSize)
                        {

                            contractorsAgreementsVMList = list.OrderByDescending(s => s.ContractorsAgreementId)
                                     .Skip(jtStartIndex).Take(jtPageSize).ToList();
                        }
                        else
                            contractorsAgreementsVMList = list.OrderByDescending(s => s.ContractorsAgreementId).ToList();
                    }
                    else
                    {


                        switch (jtSorting)
                        {
                            case "ContractorsAgreementTitle ASC":
                                list = list.OrderBy(l => l.ContractorsAgreementTitle);
                                break;
                            case "ContractorsAgreementTitle DESC":
                                list = list.OrderByDescending(l => l.ContractorsAgreementTitle);
                                break;
                        }
                        if (listCount > jtPageSize)
                        {
                            contractorsAgreementsVMList = list.Skip(jtStartIndex).Take(jtPageSize).ToList();
                        }
                        else
                        {

                            contractorsAgreementsVMList = list.ToList();
                        }
                    }
                }
                catch (Exception exc)
                { }
            }
            catch (Exception exc)
            { }
            return contractorsAgreementsVMList;
        }


        public long AddToContractorsAgreements(ContractorsAgreementsVM contractorsAgreementsVM)
        {
            try
            {

                ContractorsAgreements contractorsAgreement = _mapper.Map<ContractorsAgreementsVM, ContractorsAgreements>(contractorsAgreementsVM);


                projectsApiDb.ContractorsAgreements.Add(contractorsAgreement);
                projectsApiDb.SaveChanges();
                return contractorsAgreement.ContractorsAgreementId;
            }
            catch (Exception)
            {
            }
            return 0;

        }

        public ContractorsAgreementsVM UpdateContractorsAgreements(
              List<long> childsUsersIds,
            ContractorsAgreementsVM contractorsAgreementsVM)
        {
            var ContractorsAgreementId = contractorsAgreementsVM.ContractorsAgreementId;
            if (projectsApiDb.ContractorsAgreements.Where(x => childsUsersIds.Contains(x.UserIdCreator.Value))
                .Where(x => x.ContractorsAgreementId.Equals(ContractorsAgreementId)).Any())
            {
                try
                {
                    bool? isActivated = contractorsAgreementsVM.IsActivated.HasValue ? contractorsAgreementsVM.IsActivated.Value : (bool?)true;
                    bool? isDeleted = contractorsAgreementsVM.IsDeleted.HasValue ? contractorsAgreementsVM.IsDeleted.Value : (bool?)true;

                    ContractorsAgreements contractorsAgreement = projectsApiDb.ContractorsAgreements.Where(a => a.ContractorsAgreementId == ContractorsAgreementId).FirstOrDefault();
                    contractorsAgreement.ContractorsAgreementTitle = contractorsAgreementsVM.ContractorsAgreementTitle;
                    //contractorsAgreement.ContractorsAgreementFileExt = contractorsAgreementsVM.ContractorsAgreementFileExt;
                    //contractorsAgreement.ContractorsAgreementFilePath = contractorsAgreementsVM.ContractorsAgreementFilePath;
                    contractorsAgreement.ContractorsAgreementFileOrder = contractorsAgreementsVM.ContractorsAgreementFileOrder;
                    contractorsAgreement.ContractorsAgreementNumber = contractorsAgreementsVM.ContractorsAgreementNumber.Value;
                    contractorsAgreement.ContractorsAgreementDescription = contractorsAgreementsVM.ContractorsAgreementDescription;
                    contractorsAgreement.EditTime = contractorsAgreementsVM.EditTime;
                    contractorsAgreement.EditEnDate = contractorsAgreementsVM.EditEnDate;
                    contractorsAgreement.UserIdEditor = contractorsAgreementsVM.UserIdEditor;
                    contractorsAgreement.IsActivated = isActivated;
                    contractorsAgreement.IsDeleted = isDeleted;

                    projectsApiDb.Entry<ContractorsAgreements>(contractorsAgreement).State = EntityState.Modified;

                    projectsApiDb.SaveChanges();
                    return _mapper.Map<ContractorsAgreements, ContractorsAgreementsVM>(contractorsAgreement);
                }
                catch (Exception)
                {

                }
            }
            return null;
        }

        public bool ToggleActivationContractorsAgreements(long contractorsAgreementId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var contractorsAgreements = (from c in projectsApiDb.ContractorsAgreements
                                             where c.ContractorsAgreementId == contractorsAgreementId &&
                                             childsUsersIds.Contains(c.UserIdCreator.Value)
                                             select c).FirstOrDefault();

                if (contractorsAgreements != null)
                {
                    contractorsAgreements.IsActivated = !contractorsAgreements.IsActivated;
                    contractorsAgreements.EditEnDate = DateTime.Now;
                    contractorsAgreements.EditTime = PersianDate.TimeNow;
                    contractorsAgreements.UserIdEditor = userId;

                    projectsApiDb.Entry<Entities.ContractorsAgreements>(contractorsAgreements).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool TemporaryDeleteContractorsAgreements(long contractorsAgreementId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var contractorsAgreements = (from c in projectsApiDb.ContractorsAgreements
                                             where c.ContractorsAgreementId == contractorsAgreementId &&
                                             childsUsersIds.Contains(c.UserIdCreator.Value)
                                             select c).FirstOrDefault();

                if (contractorsAgreements != null)
                {
                    contractorsAgreements.IsDeleted = !contractorsAgreements.IsDeleted;
                    contractorsAgreements.EditEnDate = DateTime.Now;
                    contractorsAgreements.EditTime = PersianDate.TimeNow;
                    contractorsAgreements.UserIdEditor = userId;

                    projectsApiDb.Entry<Entities.ContractorsAgreements>(contractorsAgreements).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool? CompleteDeleteContractorsAgreements(long contractorsAgreementId,
            long userId,
            List<long> childsUsersIds)
        {

            try
            {
                var haveAttachements = (from a in projectsApiDb.AttachementFiles
                                        where a.AttachementTableTitle == "ContractorsAgreements" &&
                                        a.AttachementParentId == contractorsAgreementId
                                        select a).Any();
                if (haveAttachements)
                {
                    return null;
                }
                var contractorsAgreement = (from c in projectsApiDb.ContractorsAgreements
                                            where c.ContractorsAgreementId == contractorsAgreementId &&
                                            childsUsersIds.Contains(c.UserIdCreator.Value)
                                            select c).FirstOrDefault();

                if (contractorsAgreement != null)
                {
                    projectsApiDb.ContractorsAgreements.Remove(contractorsAgreement);
                    projectsApiDb.SaveChanges();


                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;


        }

        public ContractorsAgreementsVM GetContractorsAgreementsWithContractorsAgreementId(long contractorsAgreementId,
            long userId,
            List<long> childsUsersIds)
        {
            ContractorsAgreementsVM contractorsAgreementsVM = new ContractorsAgreementsVM();

            try
            {
                contractorsAgreementsVM = _mapper.Map<ContractorsAgreements,
                    ContractorsAgreementsVM>(projectsApiDb.ContractorsAgreements
                    .Where(p => childsUsersIds.Contains(p.UserIdCreator.Value))
                     .Where(e => e.ContractorsAgreementId.Equals(contractorsAgreementId)).FirstOrDefault());

            }
            catch (Exception exc)
            { }

            return contractorsAgreementsVM;
        }
        #endregion

        #region Methods For Work With ConstructionProjectDailyPictures

        public List<ConstructionProjectDailyPicturesVM> GetAllConstructionProjectDailyPicturesList(
                List<long> childsUsersIds,
                long? constructionProjectId)
        {
            List<ConstructionProjectDailyPicturesVM> constructionProjectDailyPicturesVMList = new List<ConstructionProjectDailyPicturesVM>();

            try
            {
                var list = projectsApiDb.ConstructionProjectDailyPictures.AsQueryable();

                if (childsUsersIds != null)
                {
                    if (childsUsersIds.Count > 1)
                    {
                        list = list.Where(l => l.UserIdCreator.HasValue).Where(l => childsUsersIds.Contains(l.UserIdCreator.Value));
                    }
                    else
                    {
                        if (childsUsersIds.Count == 1)
                        {
                            if (childsUsersIds.FirstOrDefault() > 0)
                            {
                                list = list.Where(l => l.UserIdCreator.HasValue).Where(l => childsUsersIds.Contains(l.UserIdCreator.Value));
                            }
                        }
                    }
                }

                if (constructionProjectId.HasValue)
                    if (constructionProjectId.Value > 0)
                        list = list.Where(a => a.ConstructionProjectId.Equals(constructionProjectId.Value));

                constructionProjectDailyPicturesVMList = _mapper.Map<List<ConstructionProjectDailyPictures>, List<ConstructionProjectDailyPicturesVM>>(list.ToList());

            }
            catch (Exception exc)
            { }
            return constructionProjectDailyPicturesVMList;
        }

        public List<ConstructionProjectDailyPicturesVM> GetListOfConstructionProjectDailyPictures(int jtStartIndex,
              int jtPageSize,
              ref int listCount,
              List<long> childsUsersIds,
              string jtSorting = null,
              long? constructionProjectId = 0)
        {

            List<ConstructionProjectDailyPicturesVM> constructionProjectDailyPicturesVMList = new List<ConstructionProjectDailyPicturesVM>();

            try
            {
                var list = projectsApiDb.ConstructionProjectDailyPictures.AsQueryable();

                if (childsUsersIds != null)
                {
                    if (childsUsersIds.Count > 1)
                    {
                        list = list.Where(l => l.UserIdCreator.HasValue).Where(l => childsUsersIds.Contains(l.UserIdCreator.Value));
                    }
                    else
                    {
                        if (childsUsersIds.Count == 1)
                        {
                            if (childsUsersIds.FirstOrDefault() > 0)
                            {
                                list = list.Where(l => l.UserIdCreator.HasValue).Where(l => childsUsersIds.Contains(l.UserIdCreator.Value));
                            }
                        }
                    }
                }

                if (constructionProjectId.HasValue)
                    if (constructionProjectId.Value > 0)
                        list = list.Where(a => a.ConstructionProjectId.Equals(constructionProjectId.Value));

                listCount = list.Count();

                try
                {
                    if (string.IsNullOrEmpty(jtSorting))
                    {
                        if (listCount > jtPageSize)
                        {
                            constructionProjectDailyPicturesVMList = _mapper.Map<List<ConstructionProjectDailyPictures>,
                                List<ConstructionProjectDailyPicturesVM>>(list.OrderByDescending(s => s.ConstructionProjectDailyPictureId)
                                     .Skip(jtStartIndex).Take(jtPageSize).ToList());
                        }
                        else
                            constructionProjectDailyPicturesVMList = _mapper.Map<List<ConstructionProjectDailyPictures>,
                                List<ConstructionProjectDailyPicturesVM>>(list.OrderByDescending(s => s.ConstructionProjectDailyPictureId)
                                     .ToList());
                    }
                    else
                    {
                        switch (jtSorting)
                        {
                            case "ProjectName ASC":
                                list = list.OrderBy(l => l.ProjectName);
                                break;
                            case "ProjectName DESC":
                                list = list.OrderByDescending(l => l.ProjectName);
                                break;
                        }
                        if (listCount > jtPageSize)
                        {
                            constructionProjectDailyPicturesVMList = _mapper.Map<List<ConstructionProjectDailyPictures>,
                                List<ConstructionProjectDailyPicturesVM>>(list.OrderByDescending(s => s.ConstructionProjectDailyPictureId)
                                     .Skip(jtStartIndex).Take(jtPageSize).ToList());
                        }
                        else
                        {

                            constructionProjectDailyPicturesVMList = _mapper.Map<List<ConstructionProjectDailyPictures>,
                                List<ConstructionProjectDailyPicturesVM>>(list.OrderByDescending(s => s.ConstructionProjectDailyPictureId)
                                     .ToList());
                        }
                    }
                }
                catch (Exception exc)
                { }
            }
            catch (Exception exc)
            { }
            return constructionProjectDailyPicturesVMList;
        }


        #region  old - code of GetListOfConstructionProjectDailyPicturesWithLastDate

        //        public List<ConstructionProjectDailyPicturesVM> GetListOfConstructionProjectDailyPicturesWithLastDate(int jtStartIndex,
        //              int jtPageSize,
        //              ref int listCount,
        //              List<long> childsUsersIds,
        //              string jtSorting = null,
        //              long? constructionProjectId = 0,
        //              DateTime? lastDate = null)
        //        {
        //            List<ConstructionProjectDailyPicturesVM> constructionProjectDailyPicturesVMList = new List<ConstructionProjectDailyPicturesVM>();

        //            //constructionProjectId = 10059;

        //            try
        //            {
        //                if (projectsApiDb.ConstructionProjectDailyPictures.Where(p => p.ConstructionProjectId.Equals(constructionProjectId)).Any())
        //                {
        //                    //listCount = projectsApiDb.ConstructionProjectDailyPictures.Where(p => p.ConstructionProjectId.Equals(constructionProjectId)).Count();

        //                    string strLastDate = "  DECLARE @lastDate DATE = (select top(1) EnDate from ConstructionProjectDailyPictures where FileName <> '' and ConstructionProjectId = " +
        //                        constructionProjectId.Value + "  order by EnDate desc); ";

        //                    if (lastDate.HasValue)
        //                        strLastDate = "  DECLARE @lastDate DATE = '" + lastDate.Value.Date.ToString() + "'   set @lastDate = DateAdd(d, -1, @lastDate)  ";

        //                    string sp = "";

        //                    if (lastDate.Equals(null))
        //                    {
        //                        sp = strLastDate + @"

        //  DECLARE @fromDate DATE = DateAdd(m, -2, @lastDate) 

        //SELECT 
        //	'' AS YEAR, 
        //	'' AS MONTH, 
        //	'' AS DAY, 
        //	'' AS MONTHDAY, 

        //	--YEAR(EnDate) AS YEAR, 
        //	--MONTH(EnDate) AS MONTH, 
        //	--DAY(EnDate) AS DAY, 
        //	--CONCAT(MONTH(EnDate), '/', DAY(EnDate)) AS MONTHDAY, 
        //	* 

        //  FROM ConstructionProjectDailyPictures 
        //  where FileName <> '' and ConstructionProjectId = " + constructionProjectId.Value + @" 
        //  and (CONVERT(VARCHAR(10), EnDate, 111) <= CONVERT(VARCHAR(10), @lastDate, 111) ) and (CONVERT(VARCHAR(10), EnDate, 111)  > CONVERT(VARCHAR(10), @fromDate, 111) )
        //    order by EnDate desc
        //";
        //                    }
        //                    else
        //                    {
        //                        sp = strLastDate + @"

        //  DECLARE @fromDate DATE = DateAdd(m, -2, @lastDate) 

        //SELECT 
        //	'' AS YEAR, 
        //	'' AS MONTH, 
        //	'' AS DAY, 
        //	'' AS MONTHDAY, 

        //	--YEAR(EnDate) AS YEAR, 
        //	--MONTH(EnDate) AS MONTH, 
        //	--DAY(EnDate) AS DAY, 
        //	--CONCAT(MONTH(EnDate), '/', DAY(EnDate)) AS MONTHDAY, 
        //	* 

        //  FROM ConstructionProjectDailyPictures 
        //  where FileName <> '' and ConstructionProjectId = " + constructionProjectId.Value + @" 
        //  and (CONVERT(VARCHAR(10), EnDate, 111) <= CONVERT(VARCHAR(10), @lastDate, 111) ) 
        //    order by EnDate desc
        //";
        //                    }



        //                    constructionProjectDailyPicturesVMList = _mapper.Map<List<ConstructionProjectDailyPictures>, List<ConstructionProjectDailyPicturesVM>>(
        //                        projectsApiDb.ConstructionProjectDailyPictures.FromSqlRaw(sp).ToList());

        //                    //var a = constructionProjectDailyPicturesVMList.GroupBy(c => new { c.Year, c.Month }).ToList();


        //                }

        //            }
        //            catch (Exception exc)
        //            { }

        //            return constructionProjectDailyPicturesVMList;
        //        }


        #endregion



        public List<ConstructionProjectDailyPicturesVM> GetListOfConstructionProjectDailyPicturesWithLastDate(int jtStartIndex,
                int jtPageSize,
                ref int listCount,
                List<long> childsUsersIds,
                string jtSorting = null,
                long? constructionProjectId = 0,
                DateTime? lastDate = null)
        {
            List<ConstructionProjectDailyPicturesVM> constructionProjectDailyPicturesVMList = new List<ConstructionProjectDailyPicturesVM>();

            //constructionProjectId = 10059;

            if (lastDate != null || lastDate.HasValue) // showMore
            {
                try
                {
                    if (projectsApiDb.ConstructionProjectDailyPictures.Where(p => p.ConstructionProjectId.Equals(constructionProjectId)).Any())
                    {
                        //listCount = projectsApiDb.ConstructionProjectDailyPictures.Where(p => p.ConstructionProjectId.Equals(constructionProjectId)).Count();

                        string strLastDate = "  DECLARE @lastDate DATE = (select top(1) EnDate from ConstructionProjectDailyPictures where FileName <> '' and ConstructionProjectId = " +
                            constructionProjectId.Value + "  order by EnDate desc); ";

                        if (lastDate.HasValue)
                            strLastDate = "  DECLARE @lastDate DATE = '" + lastDate.Value.Date.ToString() + "'   set @lastDate = DateAdd(d, -1, @lastDate)  ";

                        string sp = "";

                        if (lastDate.Equals(null))
                        {
                            sp = strLastDate + @"

                                      DECLARE @fromDate DATE = DateAdd(m, -2, @lastDate) 
                            
                                    SELECT 
                                    	'' AS YEAR, 
                                    	'' AS MONTH, 
                                    	'' AS DAY, 
                                    	'' AS MONTHDAY, 
                            
                                    	--YEAR(EnDate) AS YEAR, 
                                    	--MONTH(EnDate) AS MONTH, 
                                    	--DAY(EnDate) AS DAY, 
                                    	--CONCAT(MONTH(EnDate), '/', DAY(EnDate)) AS MONTHDAY, 
                                    	* 
                            
                                      FROM ConstructionProjectDailyPictures 
                                      where FileName <> '' and ConstructionProjectId = " + constructionProjectId.Value + @" 
                                      AND (CONVERT(VARCHAR(10), EnDate, 111) <= CONVERT(VARCHAR(10), @lastDate, 111)) 
                                      AND (CONVERT(VARCHAR(10), EnDate, 111) > CONVERT(VARCHAR(10), @fromDate, 111))
                                        order by EnDate desc
                                    ";
                        }
                        else
                        {
                            sp = strLastDate + @"

                                      DECLARE @fromDate DATE = DateAdd(m, -2, @lastDate) 
                            
                                    SELECT 
                                    	'' AS YEAR, 
                                    	'' AS MONTH, 
                                    	'' AS DAY, 
                                    	'' AS MONTHDAY, 
                            
                                    	--YEAR(EnDate) AS YEAR, 
                                    	--MONTH(EnDate) AS MONTH, 
                                    	--DAY(EnDate) AS DAY, 
                                    	--CONCAT(MONTH(EnDate), '/', DAY(EnDate)) AS MONTHDAY, 
                                    	* 
                            
                                      FROM ConstructionProjectDailyPictures 
                                      where FileName <> '' and ConstructionProjectId = " + constructionProjectId.Value + @" 
                                      AND (CONVERT(VARCHAR(10), EnDate, 111) <= CONVERT(VARCHAR(10), @lastDate, 111)) 
                                      AND (CONVERT(VARCHAR(10), EnDate, 111) > CONVERT(VARCHAR(10), @fromDate, 111))
                                        order by EnDate desc
                                    ";
                        }



                        constructionProjectDailyPicturesVMList = _mapper.Map<List<ConstructionProjectDailyPictures>, List<ConstructionProjectDailyPicturesVM>>(
                            projectsApiDb.ConstructionProjectDailyPictures.FromSqlRaw(sp).ToList());

                        //var a = constructionProjectDailyPicturesVMList.GroupBy(c => new { c.Year, c.Month }).ToList();


                    }

                }
                catch (Exception exc)
                { }
            }
            else
            {


                try
                {
                    if (projectsApiDb.ConstructionProjectDailyPictures.Where(p => p.ConstructionProjectId.Equals(constructionProjectId)).Any())
                    {
                        //listCount = projectsApiDb.ConstructionProjectDailyPictures.Where(p => p.ConstructionProjectId.Equals(constructionProjectId)).Count();

                        string strLastDate = "  DECLARE @lastDate DATE = (select top(1) EnDate from ConstructionProjectDailyPictures where FileName <> '' and ConstructionProjectId = " +
                            constructionProjectId.Value + "  order by EnDate desc); ";

                        if (lastDate.HasValue)
                            strLastDate = "  DECLARE @lastDate DATE = '" + lastDate.Value.Date.ToString() + "'   set @lastDate = DateAdd(d, -1, @lastDate)  ";

                        string sp = "";

                        if (lastDate.Equals(null))
                        {
                            sp = strLastDate + @"

                                      DECLARE @fromDate DATE = DateAdd(m, -2, @lastDate) 
                            
                                    SELECT 
                                    	'' AS YEAR, 
                                    	'' AS MONTH, 
                                    	'' AS DAY, 
                                    	'' AS MONTHDAY, 
                            
                                    	--YEAR(EnDate) AS YEAR, 
                                    	--MONTH(EnDate) AS MONTH, 
                                    	--DAY(EnDate) AS DAY, 
                                    	--CONCAT(MONTH(EnDate), '/', DAY(EnDate)) AS MONTHDAY, 
                                    	* 
                            
                                      FROM ConstructionProjectDailyPictures 
                                      where FileName <> '' and ConstructionProjectId = " + constructionProjectId.Value + @" 
                                      and (CONVERT(VARCHAR(10), EnDate, 111) <= CONVERT(VARCHAR(10), @lastDate, 111) ) and (CONVERT(VARCHAR(10), EnDate, 111)  > CONVERT(VARCHAR(10), @fromDate, 111) )
                                        order by EnDate desc
                                    ";
                        }
                        else
                        {
                            sp = strLastDate + @"

                                      DECLARE @fromDate DATE = DateAdd(m, -2, @lastDate) 
                            
                                    SELECT 
                                    	'' AS YEAR, 
                                    	'' AS MONTH, 
                                    	'' AS DAY, 
                                    	'' AS MONTHDAY, 
                            
                                    	--YEAR(EnDate) AS YEAR, 
                                    	--MONTH(EnDate) AS MONTH, 
                                    	--DAY(EnDate) AS DAY, 
                                    	--CONCAT(MONTH(EnDate), '/', DAY(EnDate)) AS MONTHDAY, 
                                    	* 
                            
                                      FROM ConstructionProjectDailyPictures 
                                      where FileName <> '' and ConstructionProjectId = " + constructionProjectId.Value + @" 
                                      and (CONVERT(VARCHAR(10), EnDate, 111) <= CONVERT(VARCHAR(10), @lastDate, 111) ) 
                                        order by EnDate desc
                                    ";
                        }



                        constructionProjectDailyPicturesVMList = _mapper.Map<List<ConstructionProjectDailyPictures>, List<ConstructionProjectDailyPicturesVM>>(
                            projectsApiDb.ConstructionProjectDailyPictures.FromSqlRaw(sp).ToList());

                        //var a = constructionProjectDailyPicturesVMList.GroupBy(c => new { c.Year, c.Month }).ToList();


                    }

                }
                catch (Exception exc)
                { }
            }
            return constructionProjectDailyPicturesVMList;
        }

        #endregion

        #region Methods For Work With ConversationLogs

        public List<ConversationLogsVM> GetAllConversationLogsList(
             List<long> childsUsersIds,
             string? ConversationLogTitle = "",
             string? ConversationLogDescription = "",
             string? TableTitle = "",
             long? RecordId = 0)
        {

            List<ConversationLogsVM> conversationLogsVMList = new List<ConversationLogsVM>();

            try
            {
                var list = (from p in projectsApiDb.ConversationLogs
                            where p.IsDeleted.Value.Equals(false) &&
                            p.IsActivated.Value.Equals(true)
                            select p)
                            .AsQueryable();

                if (childsUsersIds != null)
                {
                    if (childsUsersIds.Count > 1)
                    {
                        list = list.Where(p => p.UserIdCreator.HasValue).Where(p => childsUsersIds.Contains(p.UserIdCreator.Value));
                    }
                    else
                    {
                        if (childsUsersIds.Count == 1)
                        {
                            if (childsUsersIds.FirstOrDefault() > 0)
                            {
                                list = list.Where(p => p.UserIdCreator.HasValue).Where(p => childsUsersIds.Contains(p.UserIdCreator.Value));
                            }
                        }
                    }
                }

                if (RecordId.HasValue)
                    if (RecordId.Value > 0)
                        list = list.Where(a => a.RecordId.Equals(RecordId));

                if (!string.IsNullOrEmpty(ConversationLogTitle))
                    list = list.Where(a => a.ConversationLogTitle.Contains(ConversationLogTitle));

                if (!string.IsNullOrEmpty(ConversationLogDescription))
                    list = list.Where(a => a.ConversationLogDescription.Contains(ConversationLogDescription));

                if (!string.IsNullOrEmpty(TableTitle))
                    list = list.Where(a => a.TableTitle.Contains(TableTitle));

                conversationLogsVMList = _mapper.Map<List<ConversationLogs>, List<ConversationLogsVM>>(list.OrderByDescending(s => s.ConversationLogId).ToList());
            }
            catch (Exception exc)
            { }
            return conversationLogsVMList;
        }

        public List<ConversationLogsVM> GetListOfConversationLogs(int jtStartIndex,
             int jtPageSize,
             ref int listCount,
             List<long> childsUsersIds,
             string? ConversationLogTitle = "",
             string? ConversationLogDescription = "",
             string? TableTitle = "",
             long? RecordId = 0,
             string jtSorting = null)
        {

            List<ConversationLogsVM> conversationLogsVMList = new List<ConversationLogsVM>();

            try
            {
                var list = (from p in projectsApiDb.ConversationLogs
                            where p.IsDeleted.Value.Equals(false) &&
                            p.IsActivated.Value.Equals(true)
                            select p)
                            .AsQueryable();

                if (childsUsersIds != null)
                {
                    if (childsUsersIds.Count > 1)
                    {
                        list = list.Where(p => p.UserIdCreator.HasValue).Where(p => childsUsersIds.Contains(p.UserIdCreator.Value));
                    }
                    else
                    {
                        if (childsUsersIds.Count == 1)
                        {
                            if (childsUsersIds.FirstOrDefault() > 0)
                            {
                                list = list.Where(p => p.UserIdCreator.HasValue).Where(p => childsUsersIds.Contains(p.UserIdCreator.Value));
                            }
                        }
                    }
                }

                if (RecordId.HasValue)
                    if (RecordId.Value > 0)
                        list = list.Where(a => a.RecordId.Equals(RecordId));

                if (!string.IsNullOrEmpty(ConversationLogTitle))
                    list = list.Where(a => a.ConversationLogTitle.Contains(ConversationLogTitle));

                if (!string.IsNullOrEmpty(ConversationLogDescription))
                    list = list.Where(a => a.ConversationLogDescription.Contains(ConversationLogDescription));

                if (!string.IsNullOrEmpty(TableTitle))
                    list = list.Where(a => a.TableTitle.Contains(TableTitle));

                listCount = list.Count();
                try
                {
                    if (string.IsNullOrEmpty(jtSorting))
                    {

                        if (listCount > jtPageSize)
                        {
                            conversationLogsVMList = _mapper.Map<List<ConversationLogs>, List<ConversationLogsVM>>(list.OrderByDescending(s => s.ConversationLogId)
                                     .Skip(jtStartIndex).Take(jtPageSize).ToList());
                        }
                        else
                            conversationLogsVMList = _mapper.Map<List<ConversationLogs>, List<ConversationLogsVM>>(list.OrderByDescending(s => s.ConversationLogId).ToList());
                    }
                    else
                    {


                        //switch (jtSorting)
                        //{
                        //    case "InitialPlanTitle ASC":
                        //        list = list.OrderBy(l => l.InitialPlanTitle);
                        //        break;
                        //    case "InitialPlanTitle DESC":
                        //        list = list.OrderByDescending(l => l.InitialPlanTitle);
                        //        break;
                        //}

                        if (listCount > jtPageSize)
                        {
                            conversationLogsVMList = _mapper.Map<List<ConversationLogs>, List<ConversationLogsVM>>(list.OrderByDescending(s => s.ConversationLogId)
                                     .Skip(jtStartIndex).Take(jtPageSize).ToList());
                        }
                        else
                            conversationLogsVMList = _mapper.Map<List<ConversationLogs>, List<ConversationLogsVM>>(list.OrderByDescending(s => s.ConversationLogId).ToList());
                    }
                }
                catch (Exception exc)
                { }
            }
            catch (Exception exc)
            { }
            return conversationLogsVMList;
        }

        public long AddToConversationLogs(ConversationLogsVM conversationLogsVM)
        {
            try
            {
                string[] lines = conversationLogsVM.ConversationLogDescription.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

                string messageToSave = string.Join("\r\n", lines);

                conversationLogsVM.ConversationLogDescription = messageToSave;
                conversationLogsVM.OperationEnDate = DateTime.Now;
                conversationLogsVM.OperationTime = PersianDate.TimeNow;
                conversationLogsVM.IsRead = false;
                ConversationLogs conversationLogs = _mapper.Map<ConversationLogsVM, ConversationLogs>(conversationLogsVM);
                projectsApiDb.ConversationLogs.Add(conversationLogs);
                projectsApiDb.SaveChanges();


                return conversationLogs.ConversationLogId;
            }
            catch (Exception)
            {
            }
            return 0;

        }

        public ConversationLogsVM UpdateConversationLogs(
            List<long> childsUsersIds,
            ConversationLogsVM conversationLogsVM)
        {
            var conversationLogId = conversationLogsVM.ConversationLogId;

            if (projectsApiDb.ConversationLogs.Where(x => childsUsersIds.Contains(x.UserIdCreator.Value))
                .Where(x => x.ConversationLogId.Equals(conversationLogId)).Any())
            {
                try
                {
                    bool? isActivated = conversationLogsVM.IsActivated.HasValue ? conversationLogsVM.IsActivated.Value : (bool?)true;
                    bool? isDeleted = conversationLogsVM.IsDeleted.HasValue ? conversationLogsVM.IsDeleted.Value : (bool?)true;

                    ConversationLogs conversationLogs = projectsApiDb.ConversationLogs.Where(a => a.ConversationLogId == conversationLogId).FirstOrDefault();

                    conversationLogs.ConversationLogTitle = conversationLogsVM.ConversationLogTitle;
                    conversationLogs.ConversationLogDescription = conversationLogsVM.ConversationLogDescription;
                    conversationLogs.TableTitle = conversationLogsVM.TableTitle;
                    conversationLogs.RecordId = conversationLogsVM.RecordId;

                    conversationLogs.EditTime = conversationLogsVM.EditTime;
                    conversationLogs.EditEnDate = conversationLogsVM.EditEnDate;
                    conversationLogs.UserIdEditor = conversationLogsVM.UserIdEditor;
                    conversationLogs.IsActivated = isActivated;
                    conversationLogs.IsDeleted = isDeleted;

                    projectsApiDb.Entry<ConversationLogs>(conversationLogs).State = EntityState.Modified;

                    projectsApiDb.SaveChanges();

                    return _mapper.Map<ConversationLogs, ConversationLogsVM>(conversationLogs);
                }
                catch (Exception)
                {

                }
            }
            return null;
        }

        public bool ToggleActivationConversationLogs(long conversationLogId,
           long userId,
           List<long> childsUsersIds)
        {
            try
            {
                var conversationLogs = (from c in projectsApiDb.ConversationLogs
                                        where c.ConversationLogId == conversationLogId &&
                                        childsUsersIds.Contains(c.UserIdCreator.Value)
                                        select c).FirstOrDefault();

                if (conversationLogs != null)
                {
                    conversationLogs.IsActivated = !conversationLogs.IsActivated;
                    conversationLogs.EditEnDate = DateTime.Now;
                    conversationLogs.EditTime = PersianDate.TimeNow;
                    conversationLogs.UserIdEditor = userId;

                    projectsApiDb.Entry<ConversationLogs>(conversationLogs).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool TemporaryDeleteConversationLogs(long conversationLogId,
           long userId,
           List<long> childsUsersIds)
        {
            try
            {
                var conversationLogs = (from c in projectsApiDb.ConversationLogs
                                        where c.ConversationLogId == conversationLogId &&
                                        childsUsersIds.Contains(c.UserIdCreator.Value)
                                        select c).FirstOrDefault();

                if (conversationLogs != null)
                {
                    conversationLogs.IsDeleted = !conversationLogs.IsDeleted;
                    conversationLogs.EditEnDate = DateTime.Now;
                    conversationLogs.EditTime = PersianDate.TimeNow;
                    conversationLogs.UserIdEditor = userId;

                    projectsApiDb.Entry<ConversationLogs>(conversationLogs).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool? CompleteDeleteConversationLogs(long conversationLogId,
           long userId,
           List<long> childsUsersIds)
        {

            try
            {
                var conversationLogs = (from c in projectsApiDb.ConversationLogs
                                        where c.ConversationLogId == conversationLogId &&
                                        childsUsersIds.Contains(c.UserIdCreator.Value)
                                        select c).FirstOrDefault();

                if (conversationLogs != null)
                {
                    projectsApiDb.ConversationLogs.Remove(conversationLogs);
                    projectsApiDb.SaveChanges();


                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;


        }

        public ConversationLogsVM GetConversationLogWithConversationLogId(long conversationLogId,
           long userId,
           List<long> childsUsersIds)
        {
            ConversationLogsVM conversationLogsVM = new ConversationLogsVM();

            try
            {
                conversationLogsVM = _mapper.Map<ConversationLogs,
                    ConversationLogsVM>(projectsApiDb.ConversationLogs
                    .Where(p => childsUsersIds.Contains(p.UserIdCreator.Value))
                     .Where(e => e.ConversationLogId.Equals(conversationLogId)).FirstOrDefault());

            }
            catch (Exception exc)
            { }

            return conversationLogsVM;
        }


        public bool ChangeConversationIsReadByAggreementIdAndAttachmentId(
                  long recordId,
                   string tableTitle,
                  long userId,
            bool IsAdmin

          )
        {
            try
            {
                List<ConversationLogs> conversationLogs = new List<ConversationLogs>();
                if (IsAdmin)
                {
                    conversationLogs = (from conversation in projectsApiDb.ConversationLogs
                                        where recordId.Equals(conversation.RecordId)
                                        && conversation.TableTitle.Equals(tableTitle)
                                        && conversation.IsRead == false
                                        && conversation.OperatorUserId != null
                                        select conversation).ToList();
                }
                else
                {
                    conversationLogs = (from conversation in projectsApiDb.ConversationLogs
                                        where conversation.RecordId.Equals(recordId)
                                        && conversation.TableTitle.Equals(tableTitle)
                                        && conversation.IsRead == false
                                        && conversation.OperatorUserId == null
                                        select conversation).ToList();
                }
                foreach (var conversation in conversationLogs)
                {
                    conversation.IsRead = true;
                    conversation.EditEnDate = DateTime.Now;
                    conversation.EditTime = PersianDate.TimeNow;
                    conversation.UserIdEditor = userId;
                    projectsApiDb.Entry<Entities.ConversationLogs>(conversation).State = EntityState.Modified;
                }
                projectsApiDb.SaveChanges();
                return true;
            }
            catch (Exception exc)
            { }
            return false;
        }



        public List<ConversationLogsVM> GetConversationDataByAgreementTypeAndRecordId(
         long recordId,
         string contractType,
         long userId
            )
        {
            List<ConversationLogsVM> conversationLogsVM = new List<ConversationLogsVM>();
            List<ConversationLogs> conversationLogs = new List<ConversationLogs>();
            try
            {
                conversationLogs = projectsApiDb.ConversationLogs
               .Where(conversationDb => recordId.Equals(conversationDb.RecordId) && conversationDb.TableTitle.Contains(contractType))
               .OrderBy(x => x.CreateEnDate).ToList();
            }
            catch (Exception exc)
            { }
            try
            {

                if (conversationLogs != null)
                {
                    if (conversationLogs.Count > 0)
                    {
                        List<ConversationLogs> conversations = new List<ConversationLogs>();
                        conversations = conversationLogs.Where(x => x.IsRead == false && x.UserIdCreator != userId).ToList();
                        foreach (var conversationItem in conversations)
                        {
                            conversationItem.IsRead = true;
                            conversationItem.EditEnDate = DateTime.Now;
                            conversationItem.EditTime = PersianDate.TimeNow;
                            conversationItem.UserIdEditor = userId;
                            projectsApiDb.Entry<Entities.ConversationLogs>(conversationItem).State = EntityState.Modified;
                        }
                        projectsApiDb.SaveChanges();
                    }
                }
            }
            catch (Exception exc)
            { }
            conversationLogsVM = _mapper.Map<List<ConversationLogsVM>>(conversationLogs);
            return conversationLogsVM;
        }



        #endregion

        #region Methods For Work With ConstructionProjectFinancialData

        public FinancialDetailsDataVM GetConstructionProjectFinancialDataByConstructionProjectId(
            long constructionProjectId,
            string type,
            long userId,
            long? ownerUserId = null)
        {
            FinancialDetailsDataVM financialDetailsDataVM = new FinancialDetailsDataVM();

            if (ownerUserId != null)
            {

                //منوی پروژه های تحت نمایندگی
                var constructionProjectRepresentative = projectsApiDb.ConstructionProjectRepresentatives.Where(c => c.RepresentativeId.Equals(userId)
                           && c.ConstructionProjectId.Equals(constructionProjectId) && c.OwnerUserId.Equals(ownerUserId)).FirstOrDefault();

                #region service


                try
                {
                    switch (type)
                    {
                        case "TotalPublicCost": // کل هزینه عمومی
                            try
                            {
                                financialDetailsDataVM.HeaderMonthsVM = projectsApiDb.ConstructionProjectFinancialTitles
                                .FromSqlRaw(@"
                                        SELECT  CellTitle, CellIndex
                                        FROM ConstructionProjectFinancialTitles
                                        WHERE RecordType = 'public'
                                        AND CellIndex >= 1
                                        AND ConstructionProjectFinancialTitles.ExcelSheetConfigId IN (
                                        SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId
                                        FROM ExcelSheetConfigs
                                        INNER JOIN ConstructionProjects
                                            ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                                        WHERE ExcelSheetConfigs.ReportType = 'financial'
                                            AND ConstructionProjects.ConstructionProjectId = {0})", constructionProjectId)
                                .Select(result => new HeaderMonthsVM
                                {
                                    CellTitle = result.CellTitle,
                                    CellIndex = result.CellIndex
                                })
                                .ToList();
                            }
                            catch (Exception ex)
                            {

                                financialDetailsDataVM.HeaderMonthsVM = new List<HeaderMonthsVM>();
                            }
                            try
                            {
                                financialDetailsDataVM.RowsDataVM = projectsApiDb.ConstructionProjectFinancialData
                                .FromSqlRaw(@"
                                        SELECT  RowIndex, CellIndex, CellData 
                                        FROM ConstructionProjectFinancialData
                                        WHERE 
                                        ParentType = 'constructionproject'
                                        AND RecordType = 'public'
                                        AND RowIndex > 4
                                        AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
                                        SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId 
                                        FROM ExcelSheetConfigs
                                        INNER JOIN ConstructionProjects ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                                        WHERE 
                                        ExcelSheetConfigs.ReportType = 'financial'
                                        AND ConstructionProjects.ConstructionProjectId = {0}
                                        )", constructionProjectId)
                                .Select(result => new RowsDataVM
                                {
                                    RowIndex = result.RowIndex,
                                    CellIndex = result.CellIndex,
                                    CellData = result.CellData
                                })
                                .ToList();
                            }
                            catch (Exception ex)
                            {
                                financialDetailsDataVM.RowsDataVM = new List<RowsDataVM>();
                            }
                            break;
                        case "ShareOfTheGeneralCost": // سهم از هزینه عمومی

                            List<HeaderMonthsVM> headersData = new List<HeaderMonthsVM>();
                            List<RowsDataVM> totalPublicCost = new List<RowsDataVM>();
                            try
                            {
                                headersData = projectsApiDb.ConstructionProjectFinancialTitles
                               .FromSqlRaw(@"
                                        SELECT CellTitle, CellIndex
                                        FROM ConstructionProjectFinancialTitles
                                        WHERE RecordType = 'private'
                                            AND CellIndex >= 2
                                            AND ConstructionProjectFinancialTitles.ExcelSheetConfigId IN (
                                                SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId
                                                FROM ExcelSheetConfigs
                                                INNER JOIN ConstructionProjects
                                                    ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                                                WHERE ExcelSheetConfigs.ReportType = 'financial'
                                                    AND ConstructionProjects.ConstructionProjectId = {0})", constructionProjectId)
                               .Select(result => new HeaderMonthsVM
                               {
                                   CellTitle = result.CellTitle,
                                   CellIndex = result.CellIndex
                               }).ToList();

                            }
                            catch (Exception ex)
                            {
                                financialDetailsDataVM.HeaderMonthsVM = new List<HeaderMonthsVM>();
                            }
                            try
                            {

                                financialDetailsDataVM.RowsDataVM = projectsApiDb.ConstructionProjectFinancialData
                                 .FromSqlRaw(@"
                                          SELECT ParentId, RowIndex, CellIndex, CellData
                                          FROM ConstructionProjectFinancialData
                                          WHERE ParentType = 'constructionproject'
                                          AND RecordType = 'private'
                                          AND ParentId  = {0}
                                          AND RowIndex = 6
                                          AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
                                              SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId
                                              FROM ExcelSheetConfigs
                                              INNER JOIN ConstructionProjects
                                                  ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                                              WHERE ExcelSheetConfigs.ReportType = 'financial'
                                                  AND ConstructionProjects.ConstructionProjectId = {0})", constructionProjectId)
                                .Select(result => new RowsDataVM
                                {
                                    RowIndex = result.RowIndex,
                                    CellIndex = result.CellIndex,
                                    CellData = result.CellData,
                                    ParentId = result.ParentId
                                }).ToList();
                            }
                            catch (Exception ex)
                            {
                                financialDetailsDataVM.RowsDataVM = new List<RowsDataVM>();
                            }

                            try
                            {
                                var monthsIndex = string.Join(",", headersData.Select(x => x.CellIndex).ToArray());

                                totalPublicCost = projectsApiDb.ConstructionProjectFinancialData
                                   .FromSqlRaw($@"
                                             SELECT RowIndex, CellIndex, CellData
                                             FROM ConstructionProjectFinancialData
                                             WHERE ParentType = 'constructionproject'
                                             AND RecordType = 'public'
                                             AND  RowIndex > 4
                                             AND CellIndex IN (1,{monthsIndex})  
                                             AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
                                                 SELECT DISTINCT ec.ExcelSheetConfigId
                                                 FROM ExcelSheetConfigs ec
                                                 INNER JOIN ConstructionProjects cp ON ec.ExcelSheetConfigName = cp.WorkshopName
                                                 WHERE ec.ReportType = 'financial'
                                                     AND cp.ConstructionProjectId = {constructionProjectId})")
                                   .GroupBy(result => result.CellIndex)
                                   .Select(group => new RowsDataVM
                                   {
                                       CellIndex = group.Key,
                                       CellData = group.Sum(item => Convert.ToInt64(item.CellData.Replace(",", ""))).ToString()
                                   }).ToList();
                            }
                            catch (Exception ex)
                            {
                                financialDetailsDataVM.RowsDataVM = new List<RowsDataVM>();
                            }



                            financialDetailsDataVM.HeaderMonthsVM = headersData;
                            financialDetailsDataVM.TotalPublicCost = totalPublicCost;



                            break;
                        case "TotalPrivateCost": // کل هزینه اختصاصی
                            try
                            {
                                financialDetailsDataVM.HeaderMonthsVM = projectsApiDb.ConstructionProjectFinancialTitles
                                .FromSqlRaw(@"
                                          SELECT CellTitle, CellIndex
                                          FROM ConstructionProjectFinancialTitles
                                          WHERE RecordType = 'private'
                                              AND CellIndex >= 1
                                              AND ConstructionProjectFinancialTitles.ExcelSheetConfigId IN (
                                                  SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId
                                                  FROM ExcelSheetConfigs
                                                  INNER JOIN ConstructionProjects
                                                      ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                                                  WHERE ExcelSheetConfigs.ReportType = 'financial'
                                                      AND ConstructionProjects.ConstructionProjectId = {0})", constructionProjectId)
                                .Select(result => new HeaderMonthsVM
                                {
                                    CellTitle = result.CellTitle,
                                    CellIndex = result.CellIndex
                                }).ToList();

                            }
                            catch (Exception ex)
                            {
                                financialDetailsDataVM.HeaderMonthsVM = new List<HeaderMonthsVM>();
                            }
                            try
                            {
                                financialDetailsDataVM.RowsDataVM = projectsApiDb.ConstructionProjectFinancialData
                                 .FromSqlRaw(@"
                                         SELECT ParentId, RowIndex, CellIndex, CellData
                                         FROM ConstructionProjectFinancialData
                                         WHERE ParentType = 'constructionproject'
                                         AND RecordType = 'private'
                                         AND ParentId  = {0}
                                         And RowIndex > 10
                                         AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
                                             SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId
                                             FROM ExcelSheetConfigs
                                             INNER JOIN ConstructionProjects
                                                 ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                                             WHERE ExcelSheetConfigs.ReportType = 'financial'
                                                 AND ConstructionProjects.ConstructionProjectId = {0})", constructionProjectId)
                                .Select(result => new RowsDataVM
                                {
                                    RowIndex = result.RowIndex,
                                    CellIndex = result.CellIndex,
                                    CellData = result.CellData,
                                    ParentId = result.ParentId
                                }).ToList();
                            }
                            catch (Exception ex)
                            {
                                financialDetailsDataVM.RowsDataVM = new List<RowsDataVM>();
                            }
                            break;
                        case "PaidShareOfTheProject": // سهم پرداختی پروژه

                            try
                            {

                                financialDetailsDataVM.HeaderMonthsVM = projectsApiDb.ConstructionProjectFinancialTitles
                                  .FromSqlRaw(@"
                                           SELECT  CellTitle, CellIndex
                                           FROM ConstructionProjectFinancialTitles
                                           WHERE RecordType = 'person'
                                           AND ConstructionProjectFinancialTitles.ExcelSheetConfigId IN (
                                           SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId
                                           FROM ExcelSheetConfigs
                                           INNER JOIN ConstructionProjects
                                               ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                                           WHERE ExcelSheetConfigs.ReportType = 'financial'
                                               AND ConstructionProjects.ConstructionProjectId = {0})", constructionProjectId)
                                  .Select(result => new HeaderMonthsVM
                                  {
                                      CellTitle = result.CellTitle,
                                      CellIndex = result.CellIndex
                                  }).ToList();
                            }
                            catch (Exception ex)
                            {

                                financialDetailsDataVM.HeaderMonthsVM = new List<HeaderMonthsVM>();
                            }
                            try
                            {

                                financialDetailsDataVM.RowsDataVM = projectsApiDb.ConstructionProjectFinancialData
                                  .FromSqlRaw(@"
                                           SELECT  ParentId, RowIndex, CellIndex, CellData 
                                           FROM ConstructionProjectFinancialData
                                           Where ParentType = 'person'
                                           And RecordType = 'private'  
                                           And UserIdCreator = {0} 
                                           And ParentId = {1}
                                           AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
                                           SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId 
                                           FROM ExcelSheetConfigs
                                           INNER JOIN ConstructionProjects ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                                           WHERE 
                                           ExcelSheetConfigs.ReportType = 'financial'
                                           AND ConstructionProjects.ConstructionProjectId = {2}
                                           )", constructionProjectRepresentative.OwnerUserId, constructionProjectId, constructionProjectId)
                                  .Select(result => new RowsDataVM
                                  {
                                      RowIndex = result.RowIndex,
                                      CellIndex = result.CellIndex,
                                      CellData = result.CellData,
                                      ParentId = result.ParentId
                                  }).ToList();
                            }
                            catch (Exception ex)
                            {
                                financialDetailsDataVM.RowsDataVM = new List<RowsDataVM>();
                            }
                            break;
                    }
                }
                catch (Exception ex)
                { }
                #endregion

            }
            else
            {
                //منوی پروژه های من
                #region service

                try
                {
                    switch (type)
                    {
                        case "TotalPublicCost": // کل هزینه عمومی
                            try
                            {
                                financialDetailsDataVM.HeaderMonthsVM = projectsApiDb.ConstructionProjectFinancialTitles
                                .FromSqlRaw(@"
                                        SELECT  CellTitle, CellIndex
                                        FROM ConstructionProjectFinancialTitles
                                        WHERE RecordType = 'public'
                                        AND CellIndex >= 1
                                        AND ConstructionProjectFinancialTitles.ExcelSheetConfigId IN (
                                        SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId
                                        FROM ExcelSheetConfigs
                                        INNER JOIN ConstructionProjects
                                            ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                                        WHERE ExcelSheetConfigs.ReportType = 'financial'
                                            AND ConstructionProjects.ConstructionProjectId = {0})", constructionProjectId)
                                .Select(result => new HeaderMonthsVM
                                {
                                    CellTitle = result.CellTitle,
                                    CellIndex = result.CellIndex
                                })
                                .ToList();
                            }
                            catch (Exception ex)
                            {

                                financialDetailsDataVM.HeaderMonthsVM = new List<HeaderMonthsVM>();
                            }
                            try
                            {
                                financialDetailsDataVM.RowsDataVM = projectsApiDb.ConstructionProjectFinancialData
                                .FromSqlRaw(@"
                                        SELECT  RowIndex, CellIndex, CellData 
                                        FROM ConstructionProjectFinancialData
                                        WHERE 
                                        ParentType = 'constructionproject'
                                        AND RecordType = 'public'
                                        AND RowIndex > 4
                                        AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
                                        SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId 
                                        FROM ExcelSheetConfigs
                                        INNER JOIN ConstructionProjects ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                                        WHERE 
                                        ExcelSheetConfigs.ReportType = 'financial'
                                        AND ConstructionProjects.ConstructionProjectId = {0}
                                        )", constructionProjectId)
                                .Select(result => new RowsDataVM
                                {
                                    RowIndex = result.RowIndex,
                                    CellIndex = result.CellIndex,
                                    CellData = result.CellData
                                })
                                .ToList();
                            }
                            catch (Exception ex)
                            {
                                financialDetailsDataVM.RowsDataVM = new List<RowsDataVM>();
                            }
                            break;
                        case "ShareOfTheGeneralCost": // سهم از هزینه عمومی

                            List<HeaderMonthsVM> headersData = new List<HeaderMonthsVM>();
                            List<RowsDataVM> totalPublicCost = new List<RowsDataVM>();
                            try
                            {
                                headersData = projectsApiDb.ConstructionProjectFinancialTitles
                               .FromSqlRaw(@"
                                        SELECT CellTitle, CellIndex
                                        FROM ConstructionProjectFinancialTitles
                                        WHERE RecordType = 'private'
                                            AND CellIndex >= 2
                                            AND ConstructionProjectFinancialTitles.ExcelSheetConfigId IN (
                                                SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId
                                                FROM ExcelSheetConfigs
                                                INNER JOIN ConstructionProjects
                                                    ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                                                WHERE ExcelSheetConfigs.ReportType = 'financial'
                                                    AND ConstructionProjects.ConstructionProjectId = {0})", constructionProjectId)
                               .Select(result => new HeaderMonthsVM
                               {
                                   CellTitle = result.CellTitle,
                                   CellIndex = result.CellIndex
                               }).ToList();

                            }
                            catch (Exception ex)
                            {
                                financialDetailsDataVM.HeaderMonthsVM = new List<HeaderMonthsVM>();
                            }
                            try
                            {

                                financialDetailsDataVM.RowsDataVM = projectsApiDb.ConstructionProjectFinancialData
                                 .FromSqlRaw(@"
                                          SELECT ParentId, RowIndex, CellIndex, CellData
                                          FROM ConstructionProjectFinancialData
                                          WHERE ParentType = 'constructionproject'
                                          AND RecordType = 'private'
                                          AND ParentId  = {0}
                                          And RowIndex = 6
                                          AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
                                              SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId
                                              FROM ExcelSheetConfigs
                                              INNER JOIN ConstructionProjects
                                                  ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                                              WHERE ExcelSheetConfigs.ReportType = 'financial'
                                                  AND ConstructionProjects.ConstructionProjectId = {0})", constructionProjectId)
                                .Select(result => new RowsDataVM
                                {
                                    RowIndex = result.RowIndex,
                                    CellIndex = result.CellIndex,
                                    CellData = result.CellData,
                                    ParentId = result.ParentId
                                }).ToList();
                            }
                            catch (Exception ex)
                            {
                                financialDetailsDataVM.RowsDataVM = new List<RowsDataVM>();
                            }

                            try
                            {
                                var monthsIndex = string.Join(",", headersData.Select(x => x.CellIndex).ToArray());

                                totalPublicCost = projectsApiDb.ConstructionProjectFinancialData
                                   .FromSqlRaw($@"
                                             SELECT RowIndex, CellIndex, CellData
                                             FROM ConstructionProjectFinancialData
                                             WHERE ParentType = 'constructionproject'
                                             AND RecordType = 'public'
                                             AND  RowIndex > 4
                                             AND CellIndex IN (1,{monthsIndex}) 
                                             AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
                                                 SELECT DISTINCT ec.ExcelSheetConfigId
                                                 FROM ExcelSheetConfigs ec
                                                 INNER JOIN ConstructionProjects cp ON ec.ExcelSheetConfigName = cp.WorkshopName
                                                 WHERE ec.ReportType = 'financial'
                                                     AND cp.ConstructionProjectId = {constructionProjectId})")
                                   .GroupBy(result => result.CellIndex)
                                   .Select(group => new RowsDataVM
                                   {
                                       CellIndex = group.Key,
                                       CellData = group.Sum(item => Convert.ToInt64(item.CellData.Replace(",", ""))).ToString()
                                   }).ToList();


                            }
                            catch (Exception ex)
                            {
                                financialDetailsDataVM.RowsDataVM = new List<RowsDataVM>();
                            }



                            financialDetailsDataVM.HeaderMonthsVM = headersData;
                            financialDetailsDataVM.TotalPublicCost = totalPublicCost;



                            break;
                        case "TotalPrivateCost": // کل هزینه اختصاصی
                            try
                            {
                                financialDetailsDataVM.HeaderMonthsVM = projectsApiDb.ConstructionProjectFinancialTitles
                                .FromSqlRaw(@"
                                          SELECT CellTitle, CellIndex
                                          FROM ConstructionProjectFinancialTitles
                                          WHERE RecordType = 'private'
                                              AND CellIndex >= 1
                                              AND ConstructionProjectFinancialTitles.ExcelSheetConfigId IN (
                                                  SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId
                                                  FROM ExcelSheetConfigs
                                                  INNER JOIN ConstructionProjects
                                                      ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                                                  WHERE ExcelSheetConfigs.ReportType = 'financial'
                                                      AND ConstructionProjects.ConstructionProjectId = {0})", constructionProjectId)
                                .Select(result => new HeaderMonthsVM
                                {
                                    CellTitle = result.CellTitle,
                                    CellIndex = result.CellIndex
                                }).ToList();

                            }
                            catch (Exception ex)
                            {
                                financialDetailsDataVM.HeaderMonthsVM = new List<HeaderMonthsVM>();
                            }
                            try
                            {
                                financialDetailsDataVM.RowsDataVM = projectsApiDb.ConstructionProjectFinancialData
                                 .FromSqlRaw(@"
                                         SELECT ParentId, RowIndex, CellIndex, CellData
                                         FROM ConstructionProjectFinancialData
                                         WHERE ParentType = 'constructionproject'
                                         AND RecordType = 'private'
                                         AND ParentId  = {0}
                                         And RowIndex > 10
                                         AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
                                             SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId
                                             FROM ExcelSheetConfigs
                                             INNER JOIN ConstructionProjects
                                                 ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                                             WHERE ExcelSheetConfigs.ReportType = 'financial'
                                                 AND ConstructionProjects.ConstructionProjectId = {0})", constructionProjectId)
                                .Select(result => new RowsDataVM
                                {
                                    RowIndex = result.RowIndex,
                                    CellIndex = result.CellIndex,
                                    CellData = result.CellData,
                                    ParentId = result.ParentId
                                }).ToList();
                            }
                            catch (Exception ex)
                            {
                                financialDetailsDataVM.RowsDataVM = new List<RowsDataVM>();
                            }
                            break;
                        case "PaidShareOfTheProject": // سهم پرداختی پروژه

                            try
                            {

                                financialDetailsDataVM.HeaderMonthsVM = projectsApiDb.ConstructionProjectFinancialTitles
                                  .FromSqlRaw(@"
                                           SELECT  CellTitle, CellIndex
                                           FROM ConstructionProjectFinancialTitles
                                           WHERE RecordType = 'person'
                                           AND ConstructionProjectFinancialTitles.ExcelSheetConfigId IN (
                                           SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId
                                           FROM ExcelSheetConfigs
                                           INNER JOIN ConstructionProjects
                                               ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                                           WHERE ExcelSheetConfigs.ReportType = 'financial'
                                               AND ConstructionProjects.ConstructionProjectId = {0})", constructionProjectId)
                                  .Select(result => new HeaderMonthsVM
                                  {
                                      CellTitle = result.CellTitle,
                                      CellIndex = result.CellIndex
                                  }).ToList();
                            }
                            catch (Exception ex)
                            {

                                financialDetailsDataVM.HeaderMonthsVM = new List<HeaderMonthsVM>();
                            }
                            try
                            {

                                financialDetailsDataVM.RowsDataVM = projectsApiDb.ConstructionProjectFinancialData
                                  .FromSqlRaw(@"
                                           SELECT  ParentId, RowIndex, CellIndex, CellData 
                                           FROM ConstructionProjectFinancialData
                                           Where ParentType = 'person'
                                           And RecordType = 'private'  
                                           And UserIdCreator = {0} 
                                           And ParentId = {1}
                                           AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
                                           SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId 
                                           FROM ExcelSheetConfigs
                                           INNER JOIN ConstructionProjects ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                                           WHERE 
                                           ExcelSheetConfigs.ReportType = 'financial'
                                           AND ConstructionProjects.ConstructionProjectId = {2}
                                           )", userId, constructionProjectId, constructionProjectId)
                                  .Select(result => new RowsDataVM
                                  {
                                      RowIndex = result.RowIndex,
                                      CellIndex = result.CellIndex,
                                      CellData = result.CellData,
                                      ParentId = result.ParentId
                                  }).ToList();
                            }
                            catch (Exception ex)
                            {
                                financialDetailsDataVM.RowsDataVM = new List<RowsDataVM>();
                            }
                            break;
                    }
                }
                catch (Exception ex)
                { }
                #endregion
            }

            return financialDetailsDataVM;
        }



        public List<PaymentsAndCostsVM> GetPaymentsAndCostsList(
            long constructionProjectId,
            long userId,
            string? type = null,
            long? representativeId = null)
        {
            List<PaymentsAndCostsVM> paymentsAndCostsVMList = new List<PaymentsAndCostsVM>();

            List<PaymentsAndCostsDataVM> costsList = new List<PaymentsAndCostsDataVM>();
            List<PaymentsAndCostsDataVM> paymentsList = new List<PaymentsAndCostsDataVM>();

            string sp = "";

            try
            {

                if (type != null)
                {

                    if (type.Equals("Representative"))
                    {
                        #region costs - Representative


                        #region  old - sp


                        //     sp = @"

                        //select ROW_NUMBER() OVER (ORDER BY (SELECT 1)) RowNumber,
                        //	case 
                        //	    WHEN A.Date is  null then b.Date else a.Date 
                        //	end Date,
                        //	case 
                        //	    WHEN A.CellIndex is  null then b.CellIndex else a.CellIndex 
                        //	end CellIndex,
                        //	a.Amount + b.Amount Amount
                        //	--* 
                        //	from 


                        //(
                        //	select 
                        //	t.CellTitle Date,
                        //	t.CellIndex,
                        //	CASE  
                        //        WHEN CellData IS NULL THEN PublicSumOfMonth
                        //		ELSE (PublicSumOfMonth * CellData) / 100
                        //	END Amount--PublicCostShare--,
                        //	from (
                        //	select PublicCosts.PublicSumOfMonth PublicSumOfMonth, PublicCosts.CellIndex CellIndex, Percents.CellData CellData from 
                        //	(select SUM(CellData) PublicSumOfMonth, CellIndex from (
                        //		select RowIndex, CellIndex, 
                        //			CAST(REPLACE(CellData, ',', '') AS float) AS CellData
                        //		from ConstructionProjectFinancialData
                        //                                    WHERE ParentType = 'constructionproject'
                        //                                    AND RecordType = 'public'
                        //                                    --AND CellIndex > 0
                        //									AND ParentId IS NULL
                        //									AND RowIndex >= 4
                        //									AND CellIndex > 0
                        //                                    AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
                        //                                    SELECT DISTINCT ec.ExcelSheetConfigId
                        //                                    FROM ExcelSheetConfigs ec
                        //                                    INNER JOIN ConstructionProjects cp ON ec.ExcelSheetConfigName = cp.WorkshopName
                        //                                    WHERE ec.ReportType = 'financial')
                        //									) t group by t.CellIndex) PublicCosts

                        //	left join 

                        //	(select CellIndex, 
                        //		CAST(CellData AS float) AS CellData
                        //	from ConstructionProjectFinancialData
                        //                                    WHERE ParentType = 'constructionproject'
                        //                                    AND RecordType = 'private'
                        //                                    --AND CellIndex > 0
                        //									AND ParentId = " + constructionProjectId + @"
                        //									AND RowIndex = 2
                        //									AND CellIndex > 0
                        //                                    AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
                        //                                    SELECT DISTINCT ec.ExcelSheetConfigId
                        //                                    FROM ExcelSheetConfigs ec
                        //                                    INNER JOIN ConstructionProjects cp ON ec.ExcelSheetConfigName = cp.WorkshopName
                        //                                    WHERE ec.ReportType = 'financial')) Percents on PublicCosts.CellIndex = Percents.CellIndex
                        //	) PercentOfPublicCosts
                        //	left join 
                        //	(
                        //		select CellTitle, CellIndex from ConstructionProjectFinancialTitles
                        //		where RecordType = 'public' and CellIndex > 0 
                        //		AND ConstructionProjectFinancialTitles.ExcelSheetConfigId IN (
                        //                                    SELECT DISTINCT ec.ExcelSheetConfigId
                        //                                    FROM ExcelSheetConfigs ec
                        //                                    INNER JOIN ConstructionProjects cp ON ec.ExcelSheetConfigName = cp.WorkshopName
                        //                                    WHERE ec.ReportType = 'financial')									
                        //	) t
                        //	on PercentOfPublicCosts.CellIndex = t.CellIndex

                        //	) a


                        //	full outer join

                        //	(

                        //select CellTitle Date, 
                        //	t.CellIndex,
                        //	Amount
                        //	from (
                        //	select SUM(CellData) Amount,
                        //		CellIndex from (
                        //		select 
                        //			RowIndex, 
                        //			CellIndex, 
                        //			CAST(REPLACE(CellData, ',', '') AS float) AS CellData
                        //		from ConstructionProjectFinancialData
                        //                                    WHERE ParentType = 'constructionproject'
                        //                                    AND RecordType = 'private'
                        //									AND ParentId = " + constructionProjectId + @"
                        //									AND RowIndex >= 4
                        //									AND CellIndex > 0
                        //                                    AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
                        //                                    SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId
                        //                                    FROM ExcelSheetConfigs
                        //                                    INNER JOIN ConstructionProjects ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                        //                                    WHERE ExcelSheetConfigs.ReportType = 'financial')
                        //									) t group by CellIndex
                        //	) PrivateCostMonthly
                        //	left join 
                        //	(
                        //		select CellTitle, CellIndex from ConstructionProjectFinancialTitles
                        //		where RecordType = 'public' and CellIndex > 0 
                        //		AND ConstructionProjectFinancialTitles.ExcelSheetConfigId IN (
                        //                                    SELECT DISTINCT ec.ExcelSheetConfigId
                        //                                    FROM ExcelSheetConfigs ec
                        //                                    INNER JOIN ConstructionProjects cp ON ec.ExcelSheetConfigName = cp.WorkshopName
                        //                                    WHERE ec.ReportType = 'financial')									
                        //	) t
                        //	on PrivateCostMonthly.CellIndex = t.CellIndex

                        //	) b on a.Date = b.Date



                        //";
                        #endregion


                        #region sp - new


                        sp = @"
                            SELECT ROW_NUMBER() OVER (ORDER BY (SELECT 1)) RowNumber,
                                    Titles.CellTitle AS Date,
                                    Titles.CellIndex, 
	                                CONVERT(FLOAT, REPLACE(Data.CellData, ',', '')) AS Amount   
                            FROM
                                    ConstructionProjectFinancialTitles AS Titles
                            INNER JOIN 
                                    ConstructionProjectFinancialData AS Data 
                            ON 
                                    Titles.CellIndex = Data.CellIndex 
                                    AND Titles.ExcelSheetConfigId = Data.ExcelSheetConfigId
                            WHERE 
                                   Titles.RecordType = 'private' 
                                   AND Titles.CellIndex >= 1 
                                   AND Data.RowIndex = 9 
                                   AND Data.ParentType = 'constructionproject' 
                                   AND Data.CellIndex >= 1 
                                   AND Data.ExcelSheetConfigId IN(
                                        SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId 
									    FROM ExcelSheetConfigs
									    INNER JOIN ConstructionProjects ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
									    WHERE 
									    ExcelSheetConfigs.ReportType = 'financial'
									    AND ConstructionProjects.ConstructionProjectId = " + constructionProjectId + @")
                                   AND Data.ParentId = " + constructionProjectId + @"
                            ";

                        #endregion


                        costsList = projectsApiDb.PaymentsAndCostsDataVM.FromSqlRaw(sp).AsNoTracking().ToList();

                        try
                        {

                            var representatives = projectsApiDb.ConstructionProjectRepresentatives.Where(c =>
                                c.RepresentativeId.Equals(userId) && c.ConstructionProjectId.Equals(constructionProjectId)
                                && c.ConstructionProjectRepresentativeId.Equals(representativeId)).FirstOrDefault();


                            var ownerUser = projectsApiDb.ConstructionProjectOwnerUsers.Where(o => o.OwnerUserId.Equals(representatives.OwnerUserId) &&
                                o.ConstructionProjectId.Equals(constructionProjectId)).FirstOrDefault();


                            if (representatives != null)
                            {
                                foreach (var cost in costsList)
                                {
                                    try
                                    {
                                        if (cost.Amount.HasValue)
                                            //if (cost.Amount.Value > 0)
                                            //if (ownerUser.SharePercent > 0)
                                            cost.Amount = (cost.Amount.Value * ownerUser.SharePercent) / 100;
                                    }
                                    catch (Exception exc)
                                    {
                                    }
                                }
                            }
                        }
                        catch (Exception exc)
                        { }

                        #endregion
                    }
                }
                else
                {
                    #region costs 


                    #region sp - old


                    //sp = @"

                    //select ROW_NUMBER() OVER (ORDER BY (SELECT 1)) RowNumber,
                    //	case 
                    //	    WHEN A.Date is  null then b.Date else a.Date 
                    //	end Date,
                    //	case 
                    //	    WHEN A.CellIndex is  null then b.CellIndex else a.CellIndex 
                    //	end CellIndex,
                    //	a.Amount + b.Amount Amount
                    //	--* 
                    //	from 


                    //(
                    //	select 
                    //	t.CellTitle Date,
                    //	t.CellIndex,
                    //	CASE  
                    //        WHEN CellData IS NULL THEN PublicSumOfMonth
                    //		ELSE (PublicSumOfMonth * CellData) / 100
                    //	END Amount--PublicCostShare--,
                    //	from (
                    //	select PublicCosts.PublicSumOfMonth PublicSumOfMonth, PublicCosts.CellIndex CellIndex, Percents.CellData CellData from 
                    //	(select SUM(CellData) PublicSumOfMonth, CellIndex from (
                    //		select RowIndex, CellIndex, 
                    //			CAST(REPLACE(CellData, ',', '') AS float) AS CellData
                    //		from ConstructionProjectFinancialData
                    //                                    WHERE ParentType = 'constructionproject'
                    //                                    AND RecordType = 'public'
                    //                                    --AND CellIndex > 0
                    //									AND ParentId IS NULL
                    //									AND RowIndex >= 4
                    //									AND CellIndex > 0
                    //                                    AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
                    //                                    SELECT DISTINCT ec.ExcelSheetConfigId
                    //                                    FROM ExcelSheetConfigs ec
                    //                                    INNER JOIN ConstructionProjects cp ON ec.ExcelSheetConfigName = cp.WorkshopName
                    //                                    WHERE ec.ReportType = 'financial')
                    //									) t group by t.CellIndex) PublicCosts

                    //	left join 

                    //	(select CellIndex, 
                    //		CAST(CellData AS float) AS CellData
                    //	from ConstructionProjectFinancialData
                    //                                    WHERE ParentType = 'constructionproject'
                    //                                    AND RecordType = 'private'
                    //                                    --AND CellIndex > 0
                    //									AND ParentId = " + constructionProjectId + @"
                    //									AND RowIndex = 2
                    //									AND CellIndex > 0
                    //                                    AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
                    //                                    SELECT DISTINCT ec.ExcelSheetConfigId
                    //                                    FROM ExcelSheetConfigs ec
                    //                                    INNER JOIN ConstructionProjects cp ON ec.ExcelSheetConfigName = cp.WorkshopName
                    //                                    WHERE ec.ReportType = 'financial')) Percents on PublicCosts.CellIndex = Percents.CellIndex
                    //	) PercentOfPublicCosts
                    //	left join 
                    //	(
                    //		select CellTitle, CellIndex from ConstructionProjectFinancialTitles
                    //		where RecordType = 'public' and CellIndex > 0 
                    //		AND ConstructionProjectFinancialTitles.ExcelSheetConfigId IN (
                    //                                    SELECT DISTINCT ec.ExcelSheetConfigId
                    //                                    FROM ExcelSheetConfigs ec
                    //                                    INNER JOIN ConstructionProjects cp ON ec.ExcelSheetConfigName = cp.WorkshopName
                    //                                    WHERE ec.ReportType = 'financial')									
                    //	) t
                    //	on PercentOfPublicCosts.CellIndex = t.CellIndex

                    //	) a


                    //	full outer join

                    //	(

                    //select CellTitle Date, 
                    //	t.CellIndex,
                    //	Amount
                    //	from (
                    //	select SUM(CellData) Amount,
                    //		CellIndex from (
                    //		select 
                    //			RowIndex, 
                    //			CellIndex, 
                    //			CAST(REPLACE(CellData, ',', '') AS float) AS CellData
                    //		from ConstructionProjectFinancialData
                    //                                    WHERE ParentType = 'constructionproject'
                    //                                    AND RecordType = 'private'
                    //									AND ParentId = " + constructionProjectId + @"
                    //									AND RowIndex >= 4
                    //									AND CellIndex > 0
                    //                                    AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
                    //                                    SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId
                    //                                    FROM ExcelSheetConfigs
                    //                                    INNER JOIN ConstructionProjects ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                    //                                    WHERE ExcelSheetConfigs.ReportType = 'financial')
                    //									) t group by CellIndex
                    //	) PrivateCostMonthly
                    //	left join 
                    //	(
                    //		select CellTitle, CellIndex from ConstructionProjectFinancialTitles
                    //		where RecordType = 'public' and CellIndex > 0 
                    //		AND ConstructionProjectFinancialTitles.ExcelSheetConfigId IN (
                    //                                    SELECT DISTINCT ec.ExcelSheetConfigId
                    //                                    FROM ExcelSheetConfigs ec
                    //                                    INNER JOIN ConstructionProjects cp ON ec.ExcelSheetConfigName = cp.WorkshopName
                    //                                    WHERE ec.ReportType = 'financial')									
                    //	) t
                    //	on PrivateCostMonthly.CellIndex = t.CellIndex

                    //	) b on a.Date = b.Date



                    //";

                    #endregion

                    #region sp - new


                    sp = @"
                            SELECT ROW_NUMBER() OVER (ORDER BY (SELECT 1)) RowNumber,
                                    Titles.CellTitle AS Date,
                                    Titles.CellIndex, 
	                                CONVERT(FLOAT, REPLACE(Data.CellData, ',', '')) AS Amount   
                            FROM
                                    ConstructionProjectFinancialTitles AS Titles
                            INNER JOIN 
                                    ConstructionProjectFinancialData AS Data 
                            ON 
                                    Titles.CellIndex = Data.CellIndex 
                                    AND Titles.ExcelSheetConfigId = Data.ExcelSheetConfigId
                            WHERE 
                                   Titles.RecordType = 'private' 
                                   AND Titles.CellIndex >= 1 
                                   AND Data.RowIndex = 9 
                                   AND Data.ParentType = 'constructionproject' 
                                   AND Data.CellIndex >= 1 
                                   AND Data.ExcelSheetConfigId IN(
                                        SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId 
									    FROM ExcelSheetConfigs
									    INNER JOIN ConstructionProjects ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
									    WHERE 
									    ExcelSheetConfigs.ReportType = 'financial'
									    AND ConstructionProjects.ConstructionProjectId = " + constructionProjectId + @")
                                   AND Data.ParentId = " + constructionProjectId + @"
                            ";

                    #endregion

                    costsList = projectsApiDb.PaymentsAndCostsDataVM.FromSqlRaw(sp).AsNoTracking().ToList();

                    try
                    {
                        var ownerUser = projectsApiDb.ConstructionProjectOwnerUsers.Where(o => o.OwnerUserId.Equals(userId) &&
                            o.ConstructionProjectId.Equals(constructionProjectId)).FirstOrDefault();

                        if (ownerUser != null)
                        {
                            foreach (var cost in costsList)
                            {
                                try
                                {
                                    if (cost.Amount.HasValue)
                                        //if (cost.Amount.Value > 0)
                                        //if (ownerUser.SharePercent > 0)
                                        cost.Amount = (cost.Amount.Value * ownerUser.SharePercent) / 100;
                                }
                                catch (Exception exc)
                                {
                                }
                            }
                        }
                    }
                    catch (Exception exc)
                    { }

                    #endregion


                }



            }
            catch (Exception exc)
            { }

            try
            {
                if (type != null)
                {
                    if (type.Equals("Representative"))
                    {
                        var representativesList = projectsApiDb.ConstructionProjectRepresentatives.Where(c =>
                                c.RepresentativeId.Equals(userId) && c.ConstructionProjectId.Equals(constructionProjectId) && c.ConstructionProjectRepresentativeId.Equals(representativeId)).ToList();



                        foreach (var ownerUserId in representativesList)
                        {


                            #region payments

                            sp = "";


                            sp = @"

                                WITH PaymentData AS (
                                	SELECT 
                                	*, RN = ROW_NUMBER() OVER(PARTITION BY RowIndex ORDER BY CellIndex)
                                	FROM (
                                	SELECT 
                                		RowIndex, CellIndex, CellData from ConstructionProjectFinancialData
                                		where 
                                		(
                                			CellIndex = (
                                				SELECT DISTINCT MIN(CellIndex) minCellIndex 
                                				FROM ConstructionProjectFinancialData
                                				WHERE 
                                					ParentId = " + constructionProjectId + @"
                                					AND RecordType = 'private'
                                					AND ParentType = 'person'
                                					AND UserIdCreator = " + ownerUserId.OwnerUserId + @"
                                				group by RowIndex
                                			)
                                			OR
                                			CellIndex = (
                                				SELECT DISTINCT MAX(CellIndex) minCellIndex 
                                				FROM ConstructionProjectFinancialData
                                				WHERE 
                                					ParentId = " + constructionProjectId + @"
                                					AND RecordType = 'private'
                                					AND ParentType = 'person'
                                					AND UserIdCreator = " + ownerUserId.OwnerUserId + @"
                                				group by RowIndex
                                			)
                                		)
                                			AND ParentId = " + constructionProjectId + @"
                                			AND RecordType = 'private'
                                			AND ParentType = 'person'
                                			AND UserIdCreator = " + ownerUserId.OwnerUserId + @"
                                	GROUP BY RowIndex, CellIndex, CellData
                                	) tmp
                                )
                                select 
                                ROW_NUMBER() OVER (ORDER BY (SELECT 1)) RowNumber,
                                PaymentDate Date,
                                NULL CellIndex,
                                SUM(Payment) Amount
                                from
                                	(
                                	select 
                                		PaymentDate = MAX(CASE WHEN RN = 1 THEN SUBSTRING(REPLACE(CAST(CellData AS nvarchar(50)), '/', ''), 0, 7) ELSE '' END),
                                		Payment = MAX(CASE WHEN RN = 2 THEN CAST(REPLACE(CAST(CellData AS nvarchar(50)), ',', '') AS float) ELSE '' END)
                                	FROM PaymentData
                                	group by RowIndex
                                	) tbl
                                	group by PaymentDate
                                
                                ";


                            paymentsList = projectsApiDb.PaymentsAndCostsDataVM.FromSqlRaw(sp).AsNoTracking().ToList();

                            #endregion

                            var dates = costsList.Where(c => !string.IsNullOrEmpty(c.Date)).Select(c => c.Date).ToList();
                            dates.AddRange(paymentsList.Select(p => p.Date).ToList());

                            List<int> intDates = dates.Distinct().Select(int.Parse).OrderBy(x => x).ToList();


                            int i = 1;
                            foreach (var date in intDates)
                            {
                                try
                                {
                                    PaymentsAndCostsDataVM cost = new PaymentsAndCostsDataVM();
                                    PaymentsAndCostsDataVM payment = new PaymentsAndCostsDataVM();

                                    double costAmount = 0;
                                    double paymentAmount = 0;

                                    if (costsList.Where(c => !string.IsNullOrEmpty(c.Date)).Where(c => c.Date.Equals(date.ToString())).Any())
                                    {
                                        cost = costsList.Where(c => !string.IsNullOrEmpty(c.Date)).Where(c => c.Date.Equals(date.ToString())).FirstOrDefault();

                                        if (cost != null)
                                        {
                                            costAmount = cost.Amount.HasValue ? cost.Amount.Value : 0;
                                        }
                                    }

                                    if (paymentsList.Where(c => !string.IsNullOrEmpty(c.Date)).Where(c => c.Date.Equals(date.ToString())).Any())
                                    {
                                        payment = paymentsList.Where(c => !string.IsNullOrEmpty(c.Date)).Where(c => c.Date.Equals(date.ToString())).FirstOrDefault();

                                        if (payment != null)
                                        {
                                            paymentAmount = payment.Amount.HasValue ? payment.Amount.Value : 0;
                                        }
                                    }

                                    PaymentsAndCostsVM paymentsAndCostsVM = new PaymentsAndCostsVM()
                                    {
                                        RowNumber = i++,
                                        Date = date,
                                        Cost = costAmount,//((costsList.Where(c => c.Date.Equals(date.ToString())).Any()) ? costsList.Where(c => c.Date.Equals(date.ToString())).FirstOrDefault().Amount.Value : 0),
                                        Payment = paymentAmount,//((paymentsList.Where(c => c.Date.Equals(date.ToString())).Any()) ? paymentsList.Where(c => c.Date.Equals(date.ToString())).FirstOrDefault().Amount.Value : 0)
                                        ConstructionProjectRepresentativeId = ownerUserId.ConstructionProjectRepresentativeId,
                                    };

                                    paymentsAndCostsVMList.Add(paymentsAndCostsVM);
                                }
                                catch (Exception exc)
                                { }
                            }
                        }
                    }

                }
                else
                {

                    #region payments

                    sp = "";

                    sp = @"

                    WITH PaymentData AS (
                    	SELECT 
                    	*, RN = ROW_NUMBER() OVER(PARTITION BY RowIndex ORDER BY CellIndex)
                    	FROM (
                    	SELECT 
                    		RowIndex, CellIndex, CellData from ConstructionProjectFinancialData
                    		where 
                    		(
                    			CellIndex = (
                    				SELECT DISTINCT MIN(CellIndex) minCellIndex 
                    				FROM ConstructionProjectFinancialData
                    				WHERE 
                    					ParentId = " + constructionProjectId + @"
                    					AND RecordType = 'private'
                    					AND ParentType = 'person'
                    					AND UserIdCreator = " + userId + @"
                    				group by RowIndex
                    			)
                    			OR
                    			CellIndex = (
                    				SELECT DISTINCT MAX(CellIndex) minCellIndex 
                    				FROM ConstructionProjectFinancialData
                    				WHERE 
                    					ParentId = " + constructionProjectId + @"
                    					AND RecordType = 'private'
                    					AND ParentType = 'person'
                    					AND UserIdCreator = " + userId + @"
                    				group by RowIndex
                    			)
                    		)
                    			AND ParentId = " + constructionProjectId + @"
                    			AND RecordType = 'private'
                    			AND ParentType = 'person'
                    			AND UserIdCreator = " + userId + @"
                    	GROUP BY RowIndex, CellIndex, CellData
                    	) tmp
                    )
                    select 
                    ROW_NUMBER() OVER (ORDER BY (SELECT 1)) RowNumber,
                    PaymentDate Date,
                    NULL CellIndex,
                    SUM(Payment) Amount
                    from
                    	(
                    	select 
                    		PaymentDate = MAX(CASE WHEN RN = 1 THEN SUBSTRING(REPLACE(CAST(CellData AS nvarchar(50)), '/', ''), 0, 7) ELSE '' END),
                    		Payment = MAX(CASE WHEN RN = 2 THEN CAST(REPLACE(CAST(CellData AS nvarchar(50)), ',', '') AS float) ELSE '' END)
                    	FROM PaymentData
                    	group by RowIndex
                    	) tbl
                    	group by PaymentDate
                    
                    ";

                    paymentsList = projectsApiDb.PaymentsAndCostsDataVM.FromSqlRaw(sp).AsNoTracking().ToList();

                    #endregion

                    var dates = costsList.Where(c => !string.IsNullOrEmpty(c.Date)).Select(c => c.Date).ToList();
                    dates.AddRange(paymentsList.Select(p => p.Date).ToList());

                    List<int> intDates = dates.Distinct().Select(int.Parse).OrderBy(x => x).ToList();

                    int i = 1;
                    foreach (var date in intDates)
                    {
                        try
                        {
                            PaymentsAndCostsDataVM cost = new PaymentsAndCostsDataVM();
                            PaymentsAndCostsDataVM payment = new PaymentsAndCostsDataVM();

                            double costAmount = 0;
                            double paymentAmount = 0;

                            if (costsList.Where(c => !string.IsNullOrEmpty(c.Date)).Where(c => c.Date.Equals(date.ToString())).Any())
                            {
                                cost = costsList.Where(c => !string.IsNullOrEmpty(c.Date)).Where(c => c.Date.Equals(date.ToString())).FirstOrDefault();

                                if (cost != null)
                                {
                                    costAmount = cost.Amount.HasValue ? cost.Amount.Value : 0;
                                }
                            }

                            if (paymentsList.Where(c => !string.IsNullOrEmpty(c.Date)).Where(c => c.Date.Equals(date.ToString())).Any())
                            {
                                payment = paymentsList.Where(c => !string.IsNullOrEmpty(c.Date)).Where(c => c.Date.Equals(date.ToString())).FirstOrDefault();

                                if (payment != null)
                                {
                                    paymentAmount = payment.Amount.HasValue ? payment.Amount.Value : 0;
                                }
                            }

                            PaymentsAndCostsVM paymentsAndCostsVM = new PaymentsAndCostsVM()
                            {
                                RowNumber = i++,
                                Date = date,
                                Cost = costAmount,//((costsList.Where(c => c.Date.Equals(date.ToString())).Any()) ? costsList.Where(c => c.Date.Equals(date.ToString())).FirstOrDefault().Amount.Value : 0),
                                Payment = paymentAmount,//((paymentsList.Where(c => c.Date.Equals(date.ToString())).Any()) ? paymentsList.Where(c => c.Date.Equals(date.ToString())).FirstOrDefault().Amount.Value : 0)
                            };

                            paymentsAndCostsVMList.Add(paymentsAndCostsVM);
                            paymentsAndCostsVMList = paymentsAndCostsVMList.OrderByDescending(c => c.RowNumber).ToList();
                        }
                        catch (Exception exc)
                        { }
                    }

                }



            }
            catch (Exception exc)
            { }

            return paymentsAndCostsVMList;
        }




        #region Old code - GetPaymentsAndCostsList


        //        public List<PaymentsAndCostsVM> GetPaymentsAndCostsList(long constructionProjectId,
        //            long userId)
        //        {
        //            List<PaymentsAndCostsVM> paymentsAndCostsVMList = new List<PaymentsAndCostsVM>();

        //            List<PaymentsAndCostsDataVM> costsList = new List<PaymentsAndCostsDataVM>();
        //            List<PaymentsAndCostsDataVM> paymentsList = new List<PaymentsAndCostsDataVM>();

        //            string sp = "";

        //            try
        //            {
        //                #region costs

        //                sp = @"

        //select ROW_NUMBER() OVER (ORDER BY (SELECT 1)) RowNumber,
        //	case 
        //	    WHEN A.Date is  null then b.Date else a.Date 
        //	end Date,
        //	case 
        //	    WHEN A.CellIndex is  null then b.CellIndex else a.CellIndex 
        //	end CellIndex,
        //	a.Amount + b.Amount Amount
        //	--* 
        //	from 


        //(
        //	select 
        //	t.CellTitle Date,
        //	t.CellIndex,
        //	CASE  
        //        WHEN CellData IS NULL THEN PublicSumOfMonth
        //		ELSE (PublicSumOfMonth * CellData) / 100
        //	END Amount--PublicCostShare--,
        //	from (
        //	select PublicCosts.PublicSumOfMonth PublicSumOfMonth, PublicCosts.CellIndex CellIndex, Percents.CellData CellData from 
        //	(select SUM(CellData) PublicSumOfMonth, CellIndex from (
        //		select RowIndex, CellIndex, 
        //			CAST(REPLACE(CellData, ',', '') AS float) AS CellData
        //		from ConstructionProjectFinancialData
        //                                    WHERE ParentType = 'constructionproject'
        //                                    AND RecordType = 'public'
        //                                    --AND CellIndex > 0
        //									AND ParentId IS NULL
        //									AND RowIndex >= 4
        //									AND CellIndex > 0
        //                                    AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
        //                                    SELECT DISTINCT ec.ExcelSheetConfigId
        //                                    FROM ExcelSheetConfigs ec
        //                                    INNER JOIN ConstructionProjects cp ON ec.ExcelSheetConfigName = cp.WorkshopName
        //                                    WHERE ec.ReportType = 'financial')
        //									) t group by t.CellIndex) PublicCosts

        //	left join 

        //	(select CellIndex, 
        //		CAST(CellData AS float) AS CellData
        //	from ConstructionProjectFinancialData
        //                                    WHERE ParentType = 'constructionproject'
        //                                    AND RecordType = 'private'
        //                                    --AND CellIndex > 0
        //									AND ParentId = " + constructionProjectId + @"
        //									AND RowIndex = 2
        //									AND CellIndex > 0
        //                                    AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
        //                                    SELECT DISTINCT ec.ExcelSheetConfigId
        //                                    FROM ExcelSheetConfigs ec
        //                                    INNER JOIN ConstructionProjects cp ON ec.ExcelSheetConfigName = cp.WorkshopName
        //                                    WHERE ec.ReportType = 'financial')) Percents on PublicCosts.CellIndex = Percents.CellIndex
        //	) PercentOfPublicCosts
        //	left join 
        //	(
        //		select CellTitle, CellIndex from ConstructionProjectFinancialTitles
        //		where RecordType = 'public' and CellIndex > 0 
        //		AND ConstructionProjectFinancialTitles.ExcelSheetConfigId IN (
        //                                    SELECT DISTINCT ec.ExcelSheetConfigId
        //                                    FROM ExcelSheetConfigs ec
        //                                    INNER JOIN ConstructionProjects cp ON ec.ExcelSheetConfigName = cp.WorkshopName
        //                                    WHERE ec.ReportType = 'financial')									
        //	) t
        //	on PercentOfPublicCosts.CellIndex = t.CellIndex

        //	) a


        //	full outer join

        //	(

        //select CellTitle Date, 
        //	t.CellIndex,
        //	Amount
        //	from (
        //	select SUM(CellData) Amount,
        //		CellIndex from (
        //		select 
        //			RowIndex, 
        //			CellIndex, 
        //			CAST(REPLACE(CellData, ',', '') AS float) AS CellData
        //		from ConstructionProjectFinancialData
        //                                    WHERE ParentType = 'constructionproject'
        //                                    AND RecordType = 'private'
        //									AND ParentId = " + constructionProjectId + @"
        //									AND RowIndex >= 4
        //									AND CellIndex > 0
        //                                    AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
        //                                    SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId
        //                                    FROM ExcelSheetConfigs
        //                                    INNER JOIN ConstructionProjects ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
        //                                    WHERE ExcelSheetConfigs.ReportType = 'financial')
        //									) t group by CellIndex
        //	) PrivateCostMonthly
        //	left join 
        //	(
        //		select CellTitle, CellIndex from ConstructionProjectFinancialTitles
        //		where RecordType = 'public' and CellIndex > 0 
        //		AND ConstructionProjectFinancialTitles.ExcelSheetConfigId IN (
        //                                    SELECT DISTINCT ec.ExcelSheetConfigId
        //                                    FROM ExcelSheetConfigs ec
        //                                    INNER JOIN ConstructionProjects cp ON ec.ExcelSheetConfigName = cp.WorkshopName
        //                                    WHERE ec.ReportType = 'financial')									
        //	) t
        //	on PrivateCostMonthly.CellIndex = t.CellIndex

        //	) b on a.Date = b.Date



        //";

        //                costsList = projectsApiDb.PaymentsAndCostsDataVM.FromSqlRaw(sp).AsNoTracking().ToList();

        //                try
        //                {
        //                    var ownerUser = projectsApiDb.ConstructionProjectOwnerUsers.Where(o => o.OwnerUserId.Equals(userId) &&
        //                        o.ConstructionProjectId.Equals(constructionProjectId)).FirstOrDefault();

        //                    if (ownerUser != null)
        //                    {
        //                        foreach (var cost in costsList)
        //                        {
        //                            try
        //                            {
        //                                if (cost.Amount.HasValue)
        //                                    //if (cost.Amount.Value > 0)
        //                                    //if (ownerUser.SharePercent > 0)
        //                                    cost.Amount = (cost.Amount.Value * ownerUser.SharePercent) / 100;
        //                            }
        //                            catch (Exception exc)
        //                            {
        //                            }
        //                        }
        //                    }
        //                }
        //                catch (Exception exc)
        //                { }

        //                #endregion

        //            }
        //            catch (Exception exc)
        //            { }

        //            try
        //            {

        //                #region payments

        //                sp = "";

        //                sp = @"

        //WITH PaymentData AS (
        //	SELECT 
        //	*, RN = ROW_NUMBER() OVER(PARTITION BY RowIndex ORDER BY CellIndex)
        //	FROM (
        //	SELECT 
        //		RowIndex, CellIndex, CellData from ConstructionProjectFinancialData
        //		where 
        //		(
        //			CellIndex = (
        //				SELECT DISTINCT MIN(CellIndex) minCellIndex 
        //				FROM ConstructionProjectFinancialData
        //				WHERE 
        //					ParentId = " + constructionProjectId + @"
        //					AND RecordType = 'private'
        //					AND ParentType = 'person'
        //					AND UserIdCreator = " + userId + @"
        //				group by RowIndex
        //			)
        //			OR
        //			CellIndex = (
        //				SELECT DISTINCT MAX(CellIndex) minCellIndex 
        //				FROM ConstructionProjectFinancialData
        //				WHERE 
        //					ParentId = " + constructionProjectId + @"
        //					AND RecordType = 'private'
        //					AND ParentType = 'person'
        //					AND UserIdCreator = " + userId + @"
        //				group by RowIndex
        //			)
        //		)
        //			AND ParentId = " + constructionProjectId + @"
        //			AND RecordType = 'private'
        //			AND ParentType = 'person'
        //			AND UserIdCreator = " + userId + @"
        //	GROUP BY RowIndex, CellIndex, CellData
        //	) tmp
        //)
        //select 
        //ROW_NUMBER() OVER (ORDER BY (SELECT 1)) RowNumber,
        //PaymentDate Date,
        //NULL CellIndex,
        //SUM(Payment) Amount
        //from
        //	(
        //	select 
        //		PaymentDate = MAX(CASE WHEN RN = 1 THEN SUBSTRING(REPLACE(CAST(CellData AS nvarchar(50)), '/', ''), 0, 7) ELSE '' END),
        //		Payment = MAX(CASE WHEN RN = 2 THEN CAST(REPLACE(CAST(CellData AS nvarchar(50)), ',', '') AS float) ELSE '' END)
        //	FROM PaymentData
        //	group by RowIndex
        //	) tbl
        //	group by PaymentDate

        //";

        //                paymentsList = projectsApiDb.PaymentsAndCostsDataVM.FromSqlRaw(sp).AsNoTracking().ToList();

        //                #endregion

        //                var dates = costsList.Where(c => !string.IsNullOrEmpty(c.Date)).Select(c => c.Date).ToList();
        //                dates.AddRange(paymentsList.Select(p => p.Date).ToList());

        //                List<int> intDates = dates.Distinct().Select(int.Parse).OrderBy(x => x).ToList();

        //                int i = 1;
        //                foreach (var date in intDates)
        //                {
        //                    try
        //                    {
        //                        PaymentsAndCostsDataVM cost = new PaymentsAndCostsDataVM();
        //                        PaymentsAndCostsDataVM payment = new PaymentsAndCostsDataVM();

        //                        double costAmount = 0;
        //                        double paymentAmount = 0;

        //                        if (costsList.Where(c => !string.IsNullOrEmpty(c.Date)).Where(c => c.Date.Equals(date.ToString())).Any())
        //                        {
        //                            cost = costsList.Where(c => !string.IsNullOrEmpty(c.Date)).Where(c => c.Date.Equals(date.ToString())).FirstOrDefault();

        //                            if (cost != null)
        //                            {
        //                                costAmount = cost.Amount.HasValue ? cost.Amount.Value : 0;
        //                            }
        //                        }

        //                        if (paymentsList.Where(c => !string.IsNullOrEmpty(c.Date)).Where(c => c.Date.Equals(date.ToString())).Any())
        //                        {
        //                            payment = paymentsList.Where(c => !string.IsNullOrEmpty(c.Date)).Where(c => c.Date.Equals(date.ToString())).FirstOrDefault();

        //                            if (payment != null)
        //                            {
        //                                paymentAmount = payment.Amount.HasValue ? payment.Amount.Value : 0;
        //                            }
        //                        }

        //                        PaymentsAndCostsVM paymentsAndCostsVM = new PaymentsAndCostsVM()
        //                        {
        //                            RowNumber = i++,
        //                            Date = date,
        //                            Cost = costAmount,//((costsList.Where(c => c.Date.Equals(date.ToString())).Any()) ? costsList.Where(c => c.Date.Equals(date.ToString())).FirstOrDefault().Amount.Value : 0),
        //                            Payment = paymentAmount,//((paymentsList.Where(c => c.Date.Equals(date.ToString())).Any()) ? paymentsList.Where(c => c.Date.Equals(date.ToString())).FirstOrDefault().Amount.Value : 0)
        //                        };

        //                        paymentsAndCostsVMList.Add(paymentsAndCostsVM);
        //                    }
        //                    catch (Exception exc)
        //                    { }
        //                }
        //            }
        //            catch (Exception exc)
        //            { }

        //            return paymentsAndCostsVMList;
        //        }

        #endregion


        #region GetSumOfPrivateCostsList - Representatives

        public List<SumOfPrivateCostsForRepresantativeVM> GetSumOfPrivateCostsListForRepresantative(
            long constructionProjectId,
            string? type = null,
            long? representativeId = null)
        {
            List<SumOfPrivateCostsForRepresantativeVM> sumOfPrivateCostsForRepresantativesList = new List<SumOfPrivateCostsForRepresantativeVM>();


            try
            {

                if (type != null)
                {
                    if (type.Equals("Representative"))
                    {

                        #region RepresentativeProjects

                        #region Load the Data


                        #region Old - sp


                        //                 string sp = @"
                        //         		select 
                        //         			ROW_NUMBER() OVER (ORDER BY (SELECT 1)) RowNumber,
                        //         			tbl2.CellData, tbl1.SumOfPrivateCost, tbl3.ConstructionProjectRepresentativeId from	
                        //         			(SELECT SUM(CAST(REPLACE(CAST(CellData AS nvarchar(50)), ',', '') AS float)) SumOfPrivateCost, RowIndex
                        //         				from ConstructionProjectFinancialData
                        //         					where ParentId = " + constructionProjectId + @"
                        //         					AND ParentType = 'constructionproject'
                        //         					AND RecordType = 'private'
                        //         					AND RowIndex > 10
                        //         					AND CellIndex > 0
                        //         					GROUP BY RowIndex
                        //         			) tbl1
                        //         			inner join 
                        //         			(SELECT 
                        //         			 --   CASE  
                        //         			 --       WHEN CellIndex IS NULL THEN PublicSumOfMonth
                        //         				--	ELSE (PublicSumOfMonth * CellData) / 100
                        //         				--END PublicCostShare,
                        //         				RowIndex, CellIndex, CellData from ConstructionProjectFinancialData
                        //         				where ParentId = " + constructionProjectId + @"
                        //         				AND ParentType = 'constructionproject'
                        //         				AND RecordType = 'private'
                        //         				AND RowIndex > 10
                        //         				AND CellIndex = 0
                        //         				GROUP BY RowIndex, CellIndex, CellData
                        //         			) tbl2
                        //                     cross join
                        //(SELECT 

                        //         				ConstructionProjectRepresentativeId from ConstructionProjectRepresentatives
                        //         				where ConstructionProjectRepresentativeId =  " + representativeId + @"
                        //         				AND ConstructionProjectId = " + constructionProjectId + @"

                        //         			) tbl3
                        //         			on tbl1.RowIndex = tbl2.RowIndex";


                        #endregion


                        string sp = @"
                                 		;WITH PrivateCosts AS (
                                            SELECT 
                                                SUM(CAST(REPLACE(CAST(CellData AS NVARCHAR(50)), ',', '') AS FLOAT)) AS SumOfPrivateCost,
                                                RowIndex
                                            FROM ConstructionProjectFinancialData
                                            WHERE ParentId = "+constructionProjectId+ @"
                                                AND ParentType = 'constructionproject'
                                                AND RecordType = 'private'
                                                AND RowIndex > 10
                                                AND CellIndex > 0
                                            GROUP BY RowIndex
                                        ),
                                        PrivateCells AS (
                                            SELECT RowIndex, CellIndex, CellData 
                                            FROM ConstructionProjectFinancialData
                                            WHERE ParentId = "+constructionProjectId+ @"
                                                AND ParentType = 'constructionproject'
                                                AND RecordType = 'private'
                                                AND RowIndex > 10
                                                AND CellIndex = 0
                                        ),
                                        SpecificPrivateCosts_2 AS (
                                            SELECT 
                                                SUM(CAST(REPLACE(CAST(CellData AS NVARCHAR(50)), ',', '') AS FLOAT)) AS SumOfPrivateCost,
                                                RowIndex
                                            FROM ConstructionProjectFinancialData
                                            WHERE ParentId =  "+constructionProjectId+ @"
                                                AND ParentType = 'constructionproject'
                                                AND RecordType = 'private'
                                                AND RowIndex = 2
                                                AND CellIndex > 0
                                            GROUP BY RowIndex
                                        ),
                                        SpecificPrivateCells_2 AS (
                                            SELECT RowIndex, CellIndex, CellData 
                                            FROM ConstructionProjectFinancialData
                                            WHERE ParentId =  "+constructionProjectId+ @"
                                                AND ParentType = 'constructionproject'
                                                AND RecordType = 'private'
                                                AND RowIndex = 2
                                                AND CellIndex = 0
                                        ),
                                        SpecificPrivateCosts_4 AS (
                                            SELECT 
                                                SUM(CAST(REPLACE(CAST(CellData AS NVARCHAR(50)), ',', '') AS FLOAT)) AS SumOfPrivateCost,
                                                RowIndex
                                            FROM ConstructionProjectFinancialData
                                            WHERE ParentId = "+constructionProjectId+ @"
                                                AND ParentType = 'constructionproject'
                                                AND RecordType = 'private'
                                                AND RowIndex = 4
                                                AND CellIndex > 0
                                            GROUP BY RowIndex
                                        ),
                                        SpecificPrivateCells_4 AS (
                                            SELECT RowIndex, CellIndex, CellData 
                                            FROM ConstructionProjectFinancialData
                                            WHERE ParentId = "+constructionProjectId+ @"
                                                AND ParentType = 'constructionproject'
                                                AND RecordType = 'private'
                                                AND RowIndex = 4
                                                AND CellIndex = 0
                                        ),
                                        CombinedData AS (
                                            SELECT 
                                                p.CellData, c.SumOfPrivateCost
                                            FROM PrivateCells p
                                            INNER JOIN PrivateCosts c ON p.RowIndex = c.RowIndex
                                        
                                            UNION ALL
                                        
                                            SELECT 
                                                s.CellData, sp.SumOfPrivateCost
                                            FROM SpecificPrivateCells_2 s
                                            INNER JOIN SpecificPrivateCosts_2 sp ON s.RowIndex = sp.RowIndex
                                        
                                            UNION ALL
                                        
                                            SELECT 
                                                s.CellData, sp.SumOfPrivateCost
                                            FROM SpecificPrivateCells_4 s
                                            INNER JOIN SpecificPrivateCosts_4 sp ON s.RowIndex = sp.RowIndex
                                        )
                                        SELECT 
                                            ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS RowNumber,
                                            c.CellData, 
                                            c.SumOfPrivateCost,
                                            r.ConstructionProjectRepresentativeId
                                        FROM CombinedData c
                                        INNER JOIN ConstructionProjectRepresentatives r 
                                            ON r.ConstructionProjectId =  "+constructionProjectId+ @" 
                                            AND r.ConstructionProjectRepresentativeId =  "+representativeId+@";";

                        
                        sumOfPrivateCostsForRepresantativesList = projectsApiDb.SumOfPrivateCostsForRepresantativeVM.FromSqlRaw(sp).AsEnumerable().OrderByDescending(p => p.SumOfPrivateCost).ToList();

                        #region comments


                        //sumOfPrivateCostsForRepresantativesList = projectsApiDb.SumOfPrivateCostsForRepresantativeVM.FromSqlRaw(sp).OrderByDescending(p => p.SumOfPrivateCost).ToList();


                        //var ProjectShareOfGeneralCost = projectsApiDb.ConstructionProjectFinancialData
                        //        .FromSqlRaw($@"
                        //                   SELECT CellIndex, REPLACE(CellData, ',', '') AS CellData
                        //                   FROM ConstructionProjectFinancialData
                        //                   WHERE ParentType = 'constructionproject'
                        //                   AND RecordType = 'public'
                        //                   AND CellIndex > 0
                        //                   AND RowIndex > 4
                        //                   AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
                        //                   SELECT DISTINCT ec.ExcelSheetConfigId
                        //                   FROM ExcelSheetConfigs ec
                        //                   INNER JOIN ConstructionProjects cp ON ec.ExcelSheetConfigName = cp.WorkshopName
                        //                   WHERE ec.ReportType = 'financial'
                        //                       AND cp.ConstructionProjectId = {constructionProjectId})
                        //                   ")
                        //        .GroupBy(result => result.CellIndex)
                        //        .Select(group => new RowsDataVM
                        //        {
                        //            CellIndex = group.Key,
                        //            CellData = group.Sum(item => Convert.ToInt64(item.CellData.Replace(",", ""))).ToString()
                        //        })
                        //        .ToList();

                        //var rowsData = projectsApiDb.ConstructionProjectFinancialData
                        //            .FromSqlRaw(@"
                        //                              SELECT ParentId, RowIndex, CellIndex, REPLACE(CellData, '%', '') AS CellData
                        //                              FROM ConstructionProjectFinancialData
                        //                              WHERE ParentType = 'constructionproject'
                        //                              AND RecordType = 'private'
                        //                              AND ParentId  = {0}
                        //                              AND CellIndex > 0
                        //                              And RowIndex = 6
                        //                              AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
                        //                              SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId
                        //                              FROM ExcelSheetConfigs
                        //                              INNER JOIN ConstructionProjects
                        //                                  ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                        //                              WHERE ExcelSheetConfigs.ReportType = 'financial'
                        //                                  AND ConstructionProjects.ConstructionProjectId = {0}
                        //                          )", constructionProjectId)
                        //             .Select(result => new RowsDataVM
                        //             {
                        //                 RowIndex = result.RowIndex,
                        //                 CellIndex = result.CellIndex,
                        //                 CellData = result.CellData,
                        //                 ParentId = result.ParentId
                        //             })
                        //            .ToList();

                        //List<decimal> percentageValues = rowsData.Select(x => decimal.Parse(x.CellData)).ToList();
                        //List<decimal> projectShareOfGeneralCostPrecent = new List<decimal>();
                        //for (int i = 0; i < Math.Min(percentageValues.Count, ProjectShareOfGeneralCost.Count); i++)
                        //{
                        //    decimal percentage = percentageValues[i] / 100;
                        //    decimal value = decimal.Parse(ProjectShareOfGeneralCost[i].CellData);
                        //    decimal product = percentage * value;
                        //    decimal roundedProduct = Math.Round(product);
                        //    projectShareOfGeneralCostPrecent.Add(roundedProduct);
                        //}
                        //var totalProjectShareOfGeneralCost = ProjectShareOfGeneralCost.Sum(x => long.Parse(x.CellData));
                        ////item.ProjectShareOfGeneralCostPercent = Math.Round((projectShareOfGeneralCostPrecent.Sum() / totalProjectShareOfGeneralCost) * 100);
                        //var projectShareOfGeneralCost = Convert.ToInt64(projectShareOfGeneralCostPrecent.Sum());


                        //if (sumOfPrivateCostsForRepresantativesList.Count > 0)
                        //{
                        //    var rowNumber = sumOfPrivateCostsForRepresantativesList.LastOrDefault().RowNumber;
                        //    sumOfPrivateCostsForRepresantativesList.Add(new SumOfPrivateCostsForRepresantativeVM()
                        //    {
                        //        CellData = "سهم از هزینه عمومی",
                        //        SumOfPrivateCost = projectShareOfGeneralCost,
                        //        RowNumber = rowNumber + 1,
                        //        //ConstructionProjectRepresentativeId = representativeId,
                        //    });

                        //}
                        //else
                        //{
                        //    var rowNumber = 0;

                        //    sumOfPrivateCostsForRepresantativesList.Add(new SumOfPrivateCostsForRepresantativeVM()
                        //    {
                        //        CellData = "سهم از هزینه عمومی",
                        //        SumOfPrivateCost = projectShareOfGeneralCost,
                        //        RowNumber = rowNumber + 1,
                        //        //ConstructionProjectRepresentativeId = representativeId,
                        //    });
                        //}
                        #endregion



                        #endregion

                        sumOfPrivateCostsForRepresantativesList = sumOfPrivateCostsForRepresantativesList.OrderByDescending(c => c.SumOfPrivateCost).ToList();

                        #endregion

                        #region RepresentativeProjects - comments - old code

                        #region Load the Data

                        //                 string sp = @"
                        //         		select 
                        //         			ROW_NUMBER() OVER (ORDER BY (SELECT 1)) RowNumber,
                        //         			tbl2.CellData, tbl1.SumOfPrivateCost, tbl3.ConstructionProjectRepresentativeId from	
                        //         			(SELECT SUM(CAST(REPLACE(CAST(CellData AS nvarchar(50)), ',', '') AS float)) SumOfPrivateCost, RowIndex
                        //         				from ConstructionProjectFinancialData
                        //         					where ParentId = " + constructionProjectId + @"
                        //         					AND ParentType = 'constructionproject'
                        //         					AND RecordType = 'private'
                        //         					AND RowIndex > 10
                        //         					AND CellIndex > 0
                        //         					GROUP BY RowIndex
                        //         			) tbl1
                        //         			inner join 
                        //         			(SELECT 
                        //         			 --   CASE  
                        //         			 --       WHEN CellIndex IS NULL THEN PublicSumOfMonth
                        //         				--	ELSE (PublicSumOfMonth * CellData) / 100
                        //         				--END PublicCostShare,
                        //         				RowIndex, CellIndex, CellData from ConstructionProjectFinancialData
                        //         				where ParentId = " + constructionProjectId + @"
                        //         				AND ParentType = 'constructionproject'
                        //         				AND RecordType = 'private'
                        //         				AND RowIndex > 10
                        //         				AND CellIndex = 0
                        //         				GROUP BY RowIndex, CellIndex, CellData
                        //         			) tbl2
                        //                     cross join
                        //(SELECT 

                        //         				ConstructionProjectRepresentativeId from ConstructionProjectRepresentatives
                        //         				where ConstructionProjectRepresentativeId =  " + representativeId + @"
                        //         				AND ConstructionProjectId = " + constructionProjectId + @"

                        //         			) tbl3
                        //         			on tbl1.RowIndex = tbl2.RowIndex";

                        //                 sumOfPrivateCostsForRepresantativesList = projectsApiDb.SumOfPrivateCostsForRepresantativeVM.FromSqlRaw(sp).OrderByDescending(p => p.SumOfPrivateCost).ToList();



                        //                 var ProjectShareOfGeneralCost = projectsApiDb.ConstructionProjectFinancialData
                        //                         .FromSqlRaw($@"
                        //                                    SELECT CellIndex, REPLACE(CellData, ',', '') AS CellData
                        //                                    FROM ConstructionProjectFinancialData
                        //                                    WHERE ParentType = 'constructionproject'
                        //                                    AND RecordType = 'public'
                        //                                    AND CellIndex > 0
                        //                                    AND RowIndex > 4
                        //                                    AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
                        //                                    SELECT DISTINCT ec.ExcelSheetConfigId
                        //                                    FROM ExcelSheetConfigs ec
                        //                                    INNER JOIN ConstructionProjects cp ON ec.ExcelSheetConfigName = cp.WorkshopName
                        //                                    WHERE ec.ReportType = 'financial'
                        //                                        AND cp.ConstructionProjectId = {constructionProjectId})
                        //                                    ")
                        //                         .GroupBy(result => result.CellIndex)
                        //                         .Select(group => new RowsDataVM
                        //                         {
                        //                             CellIndex = group.Key,
                        //                             CellData = group.Sum(item => Convert.ToInt64(item.CellData.Replace(",", ""))).ToString()
                        //                         })
                        //                         .ToList();

                        //                 var rowsData = projectsApiDb.ConstructionProjectFinancialData
                        //                             .FromSqlRaw(@"
                        //                                               SELECT ParentId, RowIndex, CellIndex, REPLACE(CellData, '%', '') AS CellData
                        //                                               FROM ConstructionProjectFinancialData
                        //                                               WHERE ParentType = 'constructionproject'
                        //                                               AND RecordType = 'private'
                        //                                               AND ParentId  = {0}
                        //                                               AND CellIndex > 0
                        //                                               And RowIndex = 6
                        //                                               AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
                        //                                               SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId
                        //                                               FROM ExcelSheetConfigs
                        //                                               INNER JOIN ConstructionProjects
                        //                                                   ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                        //                                               WHERE ExcelSheetConfigs.ReportType = 'financial'
                        //                                                   AND ConstructionProjects.ConstructionProjectId = {0}
                        //                                           )", constructionProjectId)
                        //                              .Select(result => new RowsDataVM
                        //                              {
                        //                                  RowIndex = result.RowIndex,
                        //                                  CellIndex = result.CellIndex,
                        //                                  CellData = result.CellData,
                        //                                  ParentId = result.ParentId
                        //                              })
                        //                             .ToList();

                        //                 List<decimal> percentageValues = rowsData.Select(x => decimal.Parse(x.CellData)).ToList();
                        //                 List<decimal> projectShareOfGeneralCostPrecent = new List<decimal>();
                        //                 for (int i = 0; i < Math.Min(percentageValues.Count, ProjectShareOfGeneralCost.Count); i++)
                        //                 {
                        //                     decimal percentage = percentageValues[i] / 100;
                        //                     decimal value = decimal.Parse(ProjectShareOfGeneralCost[i].CellData);
                        //                     decimal product = percentage * value;
                        //                     decimal roundedProduct = Math.Round(product);
                        //                     projectShareOfGeneralCostPrecent.Add(roundedProduct);
                        //                 }
                        //                 var totalProjectShareOfGeneralCost = ProjectShareOfGeneralCost.Sum(x => long.Parse(x.CellData));
                        //                 //item.ProjectShareOfGeneralCostPercent = Math.Round((projectShareOfGeneralCostPrecent.Sum() / totalProjectShareOfGeneralCost) * 100);
                        //                 var projectShareOfGeneralCost = Convert.ToInt64(projectShareOfGeneralCostPrecent.Sum());


                        //                 if (sumOfPrivateCostsForRepresantativesList.Count > 0)
                        //                 {
                        //                     var rowNumber = sumOfPrivateCostsForRepresantativesList.LastOrDefault().RowNumber;
                        //                     sumOfPrivateCostsForRepresantativesList.Add(new SumOfPrivateCostsForRepresantativeVM()
                        //                     {
                        //                         CellData = "سهم از هزینه عمومی",
                        //                         SumOfPrivateCost = projectShareOfGeneralCost,
                        //                         RowNumber = rowNumber + 1,
                        //                         //ConstructionProjectRepresentativeId = representativeId,
                        //                     });

                        //                 }
                        //                 else
                        //                 {
                        //                     var rowNumber = 0;

                        //                     sumOfPrivateCostsForRepresantativesList.Add(new SumOfPrivateCostsForRepresantativeVM()
                        //                     {
                        //                         CellData = "سهم از هزینه عمومی",
                        //                         SumOfPrivateCost = projectShareOfGeneralCost,
                        //                         RowNumber = rowNumber + 1,
                        //                         //ConstructionProjectRepresentativeId = representativeId,
                        //                     });
                        //                 }

                        //                 #endregion


                        //                 sumOfPrivateCostsForRepresantativesList = sumOfPrivateCostsForRepresantativesList.OrderByDescending(c => c.SumOfPrivateCost).ToList();

                        #endregion


                        #endregion
                    }

                }

            }
            catch (Exception exc)
            { }



            return sumOfPrivateCostsForRepresantativesList;
        }


        #endregion


        #region GetSumOfPrivateCostsList - MyProjects


        public List<SumOfPrivateCostsVM> GetSumOfPrivateCostsList(
            long constructionProjectId)
        {
            List<SumOfPrivateCostsVM> sumOfPrivateCostsVMList = new List<SumOfPrivateCostsVM>();

            try
            {

                #region MyProjects

                #region Load the Data

                string sp = @"
                              ;WITH PrivateCosts AS (
                                    SELECT 
                                        SUM(CAST(REPLACE(CAST(CellData AS NVARCHAR(50)), ',', '') AS FLOAT)) AS SumOfPrivateCost,
                                        RowIndex
                                    FROM ConstructionProjectFinancialData
                                    WHERE ParentId = " + constructionProjectId + @"
                                        AND ParentType = 'constructionproject'
                                        AND RecordType = 'private'
                                        AND RowIndex > 10
                                        AND CellIndex > 0
                                    GROUP BY RowIndex
                                ),
                                PrivateCells AS (
                                    SELECT RowIndex, CellIndex, CellData 
                                    FROM ConstructionProjectFinancialData
                                    WHERE ParentId = " + constructionProjectId + @"
                                        AND ParentType = 'constructionproject'
                                        AND RecordType = 'private'
                                        AND RowIndex > 10
                                        AND CellIndex = 0
                                ),
                                SpecificPrivateCosts_2 AS (
                                    SELECT 
                                        SUM(CAST(REPLACE(CAST(CellData AS NVARCHAR(50)), ',', '') AS FLOAT)) AS SumOfPrivateCost,
                                        RowIndex
                                    FROM ConstructionProjectFinancialData
                                    WHERE ParentId = " + constructionProjectId + @"
                                        AND ParentType = 'constructionproject'
                                        AND RecordType = 'private'
                                        AND RowIndex = 2
                                        AND CellIndex > 0
                                    GROUP BY RowIndex
                                ),
                                SpecificPrivateCells_2 AS (
                                    SELECT RowIndex, CellIndex, CellData 
                                    FROM ConstructionProjectFinancialData
                                    WHERE ParentId = " + constructionProjectId + @"
                                        AND ParentType = 'constructionproject'
                                        AND RecordType = 'private'
                                        AND RowIndex = 2
                                        AND CellIndex = 0
                                ),
                                SpecificPrivateCosts_4 AS (
                                    SELECT 
                                        SUM(CAST(REPLACE(CAST(CellData AS NVARCHAR(50)), ',', '') AS FLOAT)) AS SumOfPrivateCost,
                                        RowIndex
                                    FROM ConstructionProjectFinancialData
                                    WHERE ParentId = " + constructionProjectId + @"
                                        AND ParentType = 'constructionproject'
                                        AND RecordType = 'private'
                                        AND RowIndex = 4
                                        AND CellIndex > 0
                                    GROUP BY RowIndex
                                ),
                                SpecificPrivateCells_4 AS (
                                    SELECT RowIndex, CellIndex, CellData 
                                    FROM ConstructionProjectFinancialData
                                    WHERE ParentId = " + constructionProjectId + @"
                                        AND ParentType = 'constructionproject'
                                        AND RecordType = 'private'
                                        AND RowIndex = 4
                                        AND CellIndex = 0
                                )
                                
                                , CombinedData AS (
                                    SELECT 
                                        p.CellData, c.SumOfPrivateCost
                                    FROM PrivateCells p
                                    INNER JOIN PrivateCosts c ON p.RowIndex = c.RowIndex
                                
                                    UNION ALL
                                
                                    SELECT 
                                        s.CellData, sp.SumOfPrivateCost
                                    FROM SpecificPrivateCells_2 s
                                    INNER JOIN SpecificPrivateCosts_2 sp ON s.RowIndex = sp.RowIndex
                                
                                    UNION ALL
                                
                                    SELECT 
                                        s.CellData, sp.SumOfPrivateCost
                                    FROM SpecificPrivateCells_4 s
                                    INNER JOIN SpecificPrivateCosts_4 sp ON s.RowIndex = sp.RowIndex
                                )
                                SELECT 
                                    ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS RowNumber,
                                    CellData, 
                                    SumOfPrivateCost
                                FROM CombinedData;
                                
                        ";



                //sumOfPrivateCostsVMList = projectsApiDb.SumOfPrivateCostsVM.FromSqlRaw(sp).OrderByDescending(p => p.SumOfPrivateCost).ToList();
                sumOfPrivateCostsVMList = projectsApiDb.SumOfPrivateCostsVM.FromSqlRaw(sp).AsEnumerable().OrderByDescending(p => p.SumOfPrivateCost).ToList();

                #endregion



                sumOfPrivateCostsVMList = sumOfPrivateCostsVMList.OrderByDescending(c => c.SumOfPrivateCost).ToList();

                #endregion

                #region MyProjects - comments - old

                #region Load the Data
                //string sp = @"
                //        		select 
                //        			ROW_NUMBER() OVER (ORDER BY (SELECT 1)) RowNumber,
                //        			tbl2.CellData, tbl1.SumOfPrivateCost from	
                //        			(SELECT SUM(CAST(REPLACE(CAST(CellData AS nvarchar(50)), ',', '') AS float)) SumOfPrivateCost, RowIndex
                //        				from ConstructionProjectFinancialData
                //        					where ParentId = " + constructionProjectId + @"
                //        					AND ParentType = 'constructionproject'
                //        					AND RecordType = 'private'
                //        					AND RowIndex > 10
                //        					AND CellIndex > 0
                //        					GROUP BY RowIndex
                //        			) tbl1
                //        			inner join 
                //        			(SELECT 
                //        			 --   CASE  
                //        			 --       WHEN CellIndex IS NULL THEN PublicSumOfMonth
                //        				--	ELSE (PublicSumOfMonth * CellData) / 100
                //        				--END PublicCostShare,
                //        				RowIndex, CellIndex, CellData from ConstructionProjectFinancialData
                //        				where ParentId = " + constructionProjectId + @"
                //        				AND ParentType = 'constructionproject'
                //        				AND RecordType = 'private'
                //        				AND RowIndex > 10
                //        				AND CellIndex = 0
                //        				GROUP BY RowIndex, CellIndex, CellData
                //        			) tbl2
                //        			on tbl1.RowIndex = tbl2.RowIndex

                //        ";

                //sumOfPrivateCostsVMList = projectsApiDb.SumOfPrivateCostsVM.FromSqlRaw(sp).OrderByDescending(p => p.SumOfPrivateCost).ToList();

                ////var share = projectsApiDb.ConstructionProjects.Where(c => c.ConstructionProjectId.Equals(constructionProjectId)).FirstOrDefault().sha

                //var ProjectShareOfGeneralCost = projectsApiDb.ConstructionProjectFinancialData
                //.FromSqlRaw($@"
                //            SELECT CellIndex, REPLACE(CellData, ',', '') AS CellData
                //            FROM ConstructionProjectFinancialData
                //            WHERE ParentType = 'constructionproject'
                //            AND RecordType = 'public'
                //            AND CellIndex > 0
                //            AND RowIndex > 4
                //            AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
                //            SELECT DISTINCT ec.ExcelSheetConfigId
                //            FROM ExcelSheetConfigs ec
                //            INNER JOIN ConstructionProjects cp ON ec.ExcelSheetConfigName = cp.WorkshopName
                //            WHERE ec.ReportType = 'financial'
                //                AND cp.ConstructionProjectId = {constructionProjectId})
                //            ")
                //.GroupBy(result => result.CellIndex)
                //.Select(group => new RowsDataVM
                //{
                //    CellIndex = group.Key,
                //    CellData = group.Sum(item => Convert.ToInt64(item.CellData.Replace(",", ""))).ToString()
                //})
                //.ToList();

                //var rowsData = projectsApiDb.ConstructionProjectFinancialData
                //.FromSqlRaw(@"
                //                   SELECT ParentId, RowIndex, CellIndex, REPLACE(CellData, '%', '') AS CellData
                //                   FROM ConstructionProjectFinancialData
                //                   WHERE ParentType = 'constructionproject'
                //                   AND RecordType = 'private'
                //                   AND ParentId  = {0}
                //                   AND CellIndex > 0
                //                   And RowIndex = 6
                //                   AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
                //                   SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId
                //                   FROM ExcelSheetConfigs
                //                   INNER JOIN ConstructionProjects
                //                       ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                //                   WHERE ExcelSheetConfigs.ReportType = 'financial'
                //                       AND ConstructionProjects.ConstructionProjectId = {0}
                //               )", constructionProjectId)
                // .Select(result => new RowsDataVM
                // {
                //     RowIndex = result.RowIndex,
                //     CellIndex = result.CellIndex,
                //     CellData = result.CellData,
                //     ParentId = result.ParentId
                // })
                //.ToList();

                //List<decimal> percentageValues = rowsData.Select(x => decimal.Parse(x.CellData)).ToList();
                //List<decimal> projectShareOfGeneralCostPrecent = new List<decimal>();
                //for (int i = 0; i < Math.Min(percentageValues.Count, ProjectShareOfGeneralCost.Count); i++)
                //{
                //    decimal percentage = percentageValues[i] / 100;
                //    decimal value = decimal.Parse(ProjectShareOfGeneralCost[i].CellData);
                //    decimal product = percentage * value;
                //    decimal roundedProduct = Math.Round(product);
                //    projectShareOfGeneralCostPrecent.Add(roundedProduct);
                //}
                //var totalProjectShareOfGeneralCost = ProjectShareOfGeneralCost.Sum(x => long.Parse(x.CellData));
                ////item.ProjectShareOfGeneralCostPercent = Math.Round((projectShareOfGeneralCostPrecent.Sum() / totalProjectShareOfGeneralCost) * 100);
                //var projectShareOfGeneralCost = Convert.ToInt64(projectShareOfGeneralCostPrecent.Sum());

                //var rowNumber = (long)0;
                //if (sumOfPrivateCostsVMList.Count > 0)
                //{
                //    rowNumber = sumOfPrivateCostsVMList.LastOrDefault().RowNumber;
                //}
                //else
                //{
                //    rowNumber = (long)0;
                //}



                //#endregion

                //sumOfPrivateCostsVMList.Add(new SumOfPrivateCostsVM()
                //{
                //    CellData = "سهم از هزینه عمومی",
                //    SumOfPrivateCost = projectShareOfGeneralCost,
                //    RowNumber = rowNumber + 1
                //});

                //sumOfPrivateCostsVMList = sumOfPrivateCostsVMList.OrderByDescending(c => c.SumOfPrivateCost).ToList();

                #endregion

                #endregion  


            }
            catch (Exception exc)
            { }


            return sumOfPrivateCostsVMList;
        }

        #endregion


        #region GetSumOfPrivateCostsList - MyProjects - BackUp - old

        //public List<SumOfPrivateCostsVM> GetSumOfPrivateCostsList(
        //         long constructionProjectId)
        //{
        //    List<SumOfPrivateCostsVM> sumOfPrivateCostsVMList = new List<SumOfPrivateCostsVM>();

        //    try
        //    {

        //        #region MyProjects

        //        #region Load the Data

        //        string sp = @"
        //                		select 
        //                			ROW_NUMBER() OVER (ORDER BY (SELECT 1)) RowNumber,
        //                			tbl2.CellData, tbl1.SumOfPrivateCost from	
        //                			(SELECT SUM(CAST(REPLACE(CAST(CellData AS nvarchar(50)), ',', '') AS float)) SumOfPrivateCost, RowIndex
        //                				from ConstructionProjectFinancialData
        //                					where ParentId = " + constructionProjectId + @"
        //                					AND ParentType = 'constructionproject'
        //                					AND RecordType = 'private'
        //                					AND RowIndex > 10
        //                					AND CellIndex > 0
        //                					GROUP BY RowIndex
        //                			) tbl1
        //                			inner join 
        //                			(SELECT 
        //                			 --   CASE  
        //                			 --       WHEN CellIndex IS NULL THEN PublicSumOfMonth
        //                				--	ELSE (PublicSumOfMonth * CellData) / 100
        //                				--END PublicCostShare,
        //                				RowIndex, CellIndex, CellData from ConstructionProjectFinancialData
        //                				where ParentId = " + constructionProjectId + @"
        //                				AND ParentType = 'constructionproject'
        //                				AND RecordType = 'private'
        //                				AND RowIndex > 10
        //                				AND CellIndex = 0
        //                				GROUP BY RowIndex, CellIndex, CellData
        //                			) tbl2
        //                			on tbl1.RowIndex = tbl2.RowIndex

        //                ";



        //        sumOfPrivateCostsVMList = projectsApiDb.SumOfPrivateCostsVM.FromSqlRaw(sp).OrderByDescending(p => p.SumOfPrivateCost).ToList();

        //        //var share = projectsApiDb.ConstructionProjects.Where(c => c.ConstructionProjectId.Equals(constructionProjectId)).FirstOrDefault().sha

        //        var ProjectShareOfGeneralCost = projectsApiDb.ConstructionProjectFinancialData
        //        .FromSqlRaw($@"
        //                    SELECT CellIndex, REPLACE(CellData, ',', '') AS CellData
        //                    FROM ConstructionProjectFinancialData
        //                    WHERE ParentType = 'constructionproject'
        //                    AND RecordType = 'public'
        //                    AND CellIndex > 0
        //                    AND RowIndex > 4
        //                    AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
        //                    SELECT DISTINCT ec.ExcelSheetConfigId
        //                    FROM ExcelSheetConfigs ec
        //                    INNER JOIN ConstructionProjects cp ON ec.ExcelSheetConfigName = cp.WorkshopName
        //                    WHERE ec.ReportType = 'financial'
        //                        AND cp.ConstructionProjectId = {constructionProjectId})
        //                    ")
        //        .GroupBy(result => result.CellIndex)
        //        .Select(group => new RowsDataVM
        //        {
        //            CellIndex = group.Key,
        //            CellData = group.Sum(item => Convert.ToInt64(item.CellData.Replace(",", ""))).ToString()
        //        })
        //        .ToList();







        //        var rowsData = projectsApiDb.ConstructionProjectFinancialData
        //        .FromSqlRaw(@"
        //                           SELECT ParentId, RowIndex, CellIndex, REPLACE(CellData, '%', '') AS CellData
        //                           FROM ConstructionProjectFinancialData
        //                           WHERE ParentType = 'constructionproject'
        //                           AND RecordType = 'private'
        //                           AND ParentId  = {0}
        //                           AND CellIndex > 0
        //                           And RowIndex = 6
        //                           AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
        //                           SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId
        //                           FROM ExcelSheetConfigs
        //                           INNER JOIN ConstructionProjects
        //                               ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
        //                           WHERE ExcelSheetConfigs.ReportType = 'financial'
        //                               AND ConstructionProjects.ConstructionProjectId = {0}
        //                       )", constructionProjectId)
        //         .Select(result => new RowsDataVM
        //         {
        //             RowIndex = result.RowIndex,
        //             CellIndex = result.CellIndex,
        //             CellData = result.CellData,
        //             ParentId = result.ParentId
        //         })
        //        .ToList();




        //        List<decimal> percentageValues = rowsData.Select(x => decimal.Parse(x.CellData)).ToList();
        //        List<decimal> projectShareOfGeneralCostPrecent = new List<decimal>();
        //        for (int i = 0; i < Math.Min(percentageValues.Count, ProjectShareOfGeneralCost.Count); i++)
        //        {
        //            decimal percentage = percentageValues[i] / 100;
        //            decimal value = decimal.Parse(ProjectShareOfGeneralCost[i].CellData);
        //            decimal product = percentage * value;
        //            decimal roundedProduct = Math.Round(product);
        //            projectShareOfGeneralCostPrecent.Add(roundedProduct);
        //        }
        //        var totalProjectShareOfGeneralCost = ProjectShareOfGeneralCost.Sum(x => long.Parse(x.CellData));
        //        //item.ProjectShareOfGeneralCostPercent = Math.Round((projectShareOfGeneralCostPrecent.Sum() / totalProjectShareOfGeneralCost) * 100);
        //        var projectShareOfGeneralCost = Convert.ToInt64(projectShareOfGeneralCostPrecent.Sum());

        //        var rowNumber = (long)0;
        //        if (sumOfPrivateCostsVMList.Count > 0)
        //        {
        //            rowNumber = sumOfPrivateCostsVMList.LastOrDefault().RowNumber;
        //        }
        //        else
        //        {
        //            rowNumber = (long)0;
        //        }



        //        #endregion

        //        sumOfPrivateCostsVMList.Add(new SumOfPrivateCostsVM()
        //        {
        //            CellData = "سهم از عمومی ثابت",
        //            SumOfPrivateCost = projectShareOfGeneralCost,
        //            RowNumber = rowNumber + 1
        //        });

        //        sumOfPrivateCostsVMList = sumOfPrivateCostsVMList.OrderByDescending(c => c.SumOfPrivateCost).ToList();

        //        #endregion

        //    }
        //    catch (Exception exc)
        //    { }


        //    return sumOfPrivateCostsVMList;
        //}


        #endregion

        #region GetSumOfPublicCostsList - MyProject


        public List<SumOfPublicCostsVM> GetSumOfPublicCostsList(long constructionProjectId)
        {
            List<SumOfPublicCostsVM> sumOfPublicCostsVMList = new List<SumOfPublicCostsVM>();

            try
            {
                string sp = @"

		select 
			ROW_NUMBER() OVER (ORDER BY (SELECT 1)) RowNumber,
			tbl2.CellData, tbl1.SumOfPublicCost from	
			(SELECT SUM(CAST(REPLACE(CAST(CellData AS nvarchar(50)), ',', '') AS float)) SumOfPublicCost, RowIndex
				from ConstructionProjectFinancialData
					where RecordType = 'public'
					AND RowIndex > 4
					AND CellIndex > 0
					AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
                                    SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId
                                    FROM ExcelSheetConfigs
                                    INNER JOIN ConstructionProjects ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                                    WHERE ExcelSheetConfigs.ReportType = 'financial'
									AND ConstructionProjects.ConstructionProjectId = " + constructionProjectId + @")
					GROUP BY RowIndex
			) tbl1
			inner join 
			(SELECT 
			 --   CASE  
			 --       WHEN CellIndex IS NULL THEN PublicSumOfMonth
				--	ELSE (PublicSumOfMonth * CellData) / 100
				--END PublicCostShare,
				RowIndex, CellIndex, CellData from ConstructionProjectFinancialData
				where RecordType = 'public'
				AND RowIndex > 4
				AND CellIndex = 0
				AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
                                    SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId
                                    FROM ExcelSheetConfigs
                                    INNER JOIN ConstructionProjects ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                                    WHERE ExcelSheetConfigs.ReportType = 'financial'
									AND ConstructionProjects.ConstructionProjectId = " + constructionProjectId + @")
				GROUP BY RowIndex, CellIndex, CellData
			) tbl2
			on tbl1.RowIndex = tbl2.RowIndex

";
                sumOfPublicCostsVMList = projectsApiDb.SumOfPublicCostsVM.FromSqlRaw(sp).OrderByDescending(p => p.SumOfPublicCost).ToList();

                sumOfPublicCostsVMList = sumOfPublicCostsVMList.OrderByDescending(c => c.SumOfPublicCost).ToList();
            }
            catch (Exception exc)
            { }



            return sumOfPublicCostsVMList;



        }
        #endregion


        #region GetSumOfPublicCostsList - Representative

        public List<SumOfPublicCostsRepresentativesVM> GetSumOfPublicCostsListForRepresentative(long constructionProjectId,
                  string? type = null,
                  long? representativeId = null)
        {
            List<SumOfPublicCostsRepresentativesVM> sumOfPublicCostsListForRepresentativeVM = new List<SumOfPublicCostsRepresentativesVM>();

            try
            {

                if (!type.IsNullOrEmpty())
                {
                    if (type.Equals("Representative"))
                    {
                        string sp = @"

                                		select 
                                			ROW_NUMBER() OVER (ORDER BY (SELECT 1)) RowNumber,
                                			tbl2.CellData, tbl1.SumOfPublicCost, tbl3.ConstructionProjectRepresentativeId from	
                                			(SELECT SUM(CAST(REPLACE(CAST(CellData AS nvarchar(50)), ',', '') AS float)) SumOfPublicCost, RowIndex
                                				from ConstructionProjectFinancialData
                                					where RecordType = 'public'
                                					AND RowIndex > 4
                                					AND CellIndex > 0
                                					AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
                                                                    SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId
                                                                    FROM ExcelSheetConfigs
                                                                    INNER JOIN ConstructionProjects ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                                                                    WHERE ExcelSheetConfigs.ReportType = 'financial'
                                									AND ConstructionProjects.ConstructionProjectId = " + constructionProjectId + @")
                                					GROUP BY RowIndex
                                			) tbl1
                                			inner join 
                                			(SELECT 
                                			 --   CASE  
                                			 --       WHEN CellIndex IS NULL THEN PublicSumOfMonth
                                				--	ELSE (PublicSumOfMonth * CellData) / 100
                                				--END PublicCostShare,
                                				RowIndex, CellIndex, CellData from ConstructionProjectFinancialData
                                				where RecordType = 'public'
                                				AND RowIndex > 4
                                				AND CellIndex = 0
                                				AND ConstructionProjectFinancialData.ExcelSheetConfigId IN (
                                                                    SELECT DISTINCT ExcelSheetConfigs.ExcelSheetConfigId
                                                                    FROM ExcelSheetConfigs
                                                                    INNER JOIN ConstructionProjects ON ExcelSheetConfigs.ExcelSheetConfigName = ConstructionProjects.WorkshopName
                                                                    WHERE ExcelSheetConfigs.ReportType = 'financial'
                                									AND ConstructionProjects.ConstructionProjectId = " + constructionProjectId + @")
                                				GROUP BY RowIndex, CellIndex, CellData
                                			) tbl2
                                cross join
                            				(SELECT 
                             			 
                                				ConstructionProjectRepresentativeId from ConstructionProjectRepresentatives
                                				where ConstructionProjectRepresentativeId = " + representativeId + @"
                                				AND ConstructionProjectId = " + constructionProjectId + @"
                            					
                             			) tbl3
                            			on tbl1.RowIndex = tbl2.RowIndex
                            
                            ";
                        sumOfPublicCostsListForRepresentativeVM = projectsApiDb.SumOfPublicCostsRepresentativesVM.FromSqlRaw(sp).OrderByDescending(p => p.SumOfPublicCost).ToList();

                        sumOfPublicCostsListForRepresentativeVM = sumOfPublicCostsListForRepresentativeVM.OrderByDescending(c => c.SumOfPublicCost).ToList();
                    }
                }

            }
            catch (Exception exc)
            { }



            return sumOfPublicCostsListForRepresentativeVM;



        }
        #endregion

        #endregion

        #region Methods For Work With ConstructionProjectProgressData

        public List<ConstructionProjectProgressDataVM> GetProjectProgressDataVM(List<long> constructionProjectIds)
        {
            List<ConstructionProjectProgressDataVM> constructionProjectProgressDataVMList = new List<ConstructionProjectProgressDataVM>();

            try
            {
                if (projectsApiDb.ConstructionProjectProgressData.Where(p => constructionProjectIds.Contains(p.ConstructionProjectId.Value) &&
                    p.RowIndex.Equals(2) && (p.CellIndex.Equals(6) || p.CellIndex.Equals(7) || p.CellIndex.Equals(8))).Any())
                {
                    constructionProjectProgressDataVMList = _mapper.Map<List<ConstructionProjectProgressData>, List<ConstructionProjectProgressDataVM>>(
                        projectsApiDb.ConstructionProjectProgressData.Where(p => constructionProjectIds.Contains(p.ConstructionProjectId.Value) &&
                        p.RowIndex.Equals(2) && (
                            p.CellIndex.Equals(6) || //برنامه
                            p.CellIndex.Equals(7) || //عملکرد
                            p.CellIndex.Equals(8) || //انحراف
                            p.CellIndex.Equals(10) || //شروع برنامه
                            p.CellIndex.Equals(11)) //پایان برنامه
                            ).ToList());
                }
            }
            catch (Exception exc)
            { }

            return constructionProjectProgressDataVMList;
        }

        public List<ConstructionProjectProgressDataVM> GetAllProgressDataList(List<long> constructionProjectIds)
        {
            List<ConstructionProjectProgressDataVM> constructionProjectProgressDataVMList = new List<ConstructionProjectProgressDataVM>();

            try
            {
                if (projectsApiDb.ConstructionProjectProgressData.Where(p => constructionProjectIds.Contains(p.ConstructionProjectId.Value) &&
                    p.RowIndex.Equals(2) && (p.CellIndex.Equals(6) || p.CellIndex.Equals(7) || p.CellIndex.Equals(8))).Any())
                {
                    constructionProjectProgressDataVMList = _mapper.Map<List<ConstructionProjectProgressData>, List<ConstructionProjectProgressDataVM>>(
                        projectsApiDb.ConstructionProjectProgressData.Where(p => constructionProjectIds.Contains(p.ConstructionProjectId.Value) &&
                            p.RowIndex.Equals(2) && (
                            p.CellIndex.Equals(6) || //برنامه
                            p.CellIndex.Equals(7) || //عملکرد
                            p.CellIndex.Equals(8) || //انحراف
                            p.CellIndex.Equals(10) || //شروع برنامه
                            p.CellIndex.Equals(11)) //پایان برنامه
                            ).ToList());
                }
            }
            catch (Exception exc)
            { }

            return constructionProjectProgressDataVMList;
        }


        #endregion

        #region Methods For Work With ConstructionProjectProgressItems

        public List<HierarchyProjectProgressItemsVM> GetHierarchyOfAllProgressItemsList(List<long> childsUsersIds,
            long constructionProjectId,
            long? representativeId = null,
            string? type = "")
        {
            List<HierarchyProjectProgressItemsVM> hierarchyProjectProgressItemsVMList = new List<HierarchyProjectProgressItemsVM>();
            if (type != null)
            {
                if (type.Equals("Representative"))
                {
                    #region Representative


                    try
                    {
                        if (projectsApiDb.ConstructionProjectProgressItems.Where(p => p.ConstructionProjectId.Equals(constructionProjectId)).Any())
                        {

                            var constructionProjectRepresentativeId = projectsApiDb.ConstructionProjectRepresentatives.Where(c => c.ConstructionProjectId.Equals(constructionProjectId) &&
                                                    c.ConstructionProjectRepresentativeId.Equals(representativeId)).FirstOrDefault();

                            var list = projectsApiDb.ConstructionProjectProgressItems.Where(p => p.ConstructionProjectId.Equals(constructionProjectId)).AsQueryable();

                            if (childsUsersIds != null)
                            {
                                if (childsUsersIds.Count > 1)
                                {
                                    list = list.Where(p => p.UserIdCreator.HasValue).Where(p => childsUsersIds.Contains(p.UserIdCreator.Value));
                                }
                                else
                                {
                                    if (childsUsersIds.Count == 1)
                                    {
                                        if (childsUsersIds.FirstOrDefault() > 0)
                                        {
                                            list = list.Where(p => p.UserIdCreator.HasValue).Where(p => childsUsersIds.Contains(p.UserIdCreator.Value));
                                        }
                                    }
                                }
                            }

                            var constructionProjectProgressItemsList = list.ToList();

                            var mainRoot = constructionProjectProgressItemsList.Where(i => !i.ConstructionProjectProgressParentItemId.HasValue).FirstOrDefault();

                            if (mainRoot != null)
                            {
                                var roots = constructionProjectProgressItemsList.Where(i => i.ConstructionProjectProgressParentItemId.HasValue).
                                    Where(i => i.ConstructionProjectProgressParentItemId.Value.Equals(mainRoot.ConstructionProjectProgressItemId)).ToList();

                                if (roots != null)
                                {
                                    if (roots.Count > 0)
                                    {
                                        foreach (var root in roots)
                                        {
                                            try
                                            {
                                                HierarchyProjectProgressItemsVM hierarchyProjectProgressItemsVM = new HierarchyProjectProgressItemsVM();
                                                hierarchyProjectProgressItemsVM.name = root.ConstructionProjectProgressItemTitle;

                                                hierarchyProjectProgressItemsVM.ConstructionProjectRepresentativeId = constructionProjectRepresentativeId.ConstructionProjectRepresentativeId;

                                                var childs = constructionProjectProgressItemsList.Where(i => i.ConstructionProjectProgressParentItemId.HasValue).
                                                    Where(i => i.ConstructionProjectProgressParentItemId.Value.Equals(root.ConstructionProjectProgressItemId)).ToList();

                                                List<Data> childsData = new List<Data>();

                                                int y = 0;

                                                Data rootData = new Data();
                                                rootData.name = root.ConstructionProjectProgressItemTitle;
                                                rootData.id = root.ConstructionProjectProgressItemId.ToString();
                                                //childData.parent = root.ConstructionProjectProgressItemId.ToString();

                                                rootData.start = PersianDate.ToGregorianDate(root.ActivityStart).ToShortDateString().Replace("-", "/");
                                                rootData.end = PersianDate.ToGregorianDate(root.ActivityEnd).ToShortDateString().Replace("-", "/");

                                                //rootData.start = root.ActivityStart;
                                                //rootData.end = root.ActivityEnd;

                                                //childData.milestone = "false";
                                                rootData.dependency = "";
                                                rootData.owner = "";
                                                rootData.y = y;

                                                rootData.completed = new Completed();

                                                if (!string.IsNullOrEmpty(root.CumulativeOperation))
                                                    rootData.completed.amount = (double.Parse(root.CumulativeOperation.Replace("%", "")) / 100);
                                                else
                                                    rootData.completed.amount = 0;

                                                //childData.Completed.Fill = "#e80";

                                                childsData.Add(rootData);
                                                y++;

                                                foreach (var child in childs)
                                                {
                                                    try
                                                    {
                                                        Data childData = new Data();
                                                        childData.name = child.ConstructionProjectProgressItemTitle;
                                                        childData.id = child.ConstructionProjectProgressItemId.ToString();
                                                        childData.parent = root.ConstructionProjectProgressItemId.ToString();

                                                        childData.start = PersianDate.ToGregorianDate(child.ActivityStart).ToShortDateString().Replace("-", "/");
                                                        childData.end = PersianDate.ToGregorianDate(child.ActivityEnd).ToShortDateString().Replace("-", "/");

                                                        //childData.start = child.ActivityStart;
                                                        //childData.end = child.ActivityEnd;

                                                        //childData.milestone = "false";
                                                        childData.dependency = "";
                                                        childData.owner = "";
                                                        childData.y = y;

                                                        childData.completed = new Completed();

                                                        childData.completed.fill = "#e80";

                                                        if (!string.IsNullOrEmpty(child.CumulativeOperation))
                                                            childData.completed.amount = (double.Parse(child.CumulativeOperation.Replace("%", "")) / 100);
                                                        else
                                                            childData.completed.amount = 0;

                                                        //childData.Completed.Fill = "#e80";

                                                        childsData.Add(childData);
                                                        y++;
                                                    }
                                                    catch (Exception exc) { }
                                                }

                                                hierarchyProjectProgressItemsVM.data = childsData;

                                                hierarchyProjectProgressItemsVMList.Add(hierarchyProjectProgressItemsVM);
                                            }
                                            catch (Exception exc)
                                            { }
                                        }
                                    }
                                }
                            }
                        }




                    }
                    catch (Exception exc)
                    { }

                    #endregion
                }


            }
            else
            {
                #region MyProjects

                try
                {
                    if (projectsApiDb.ConstructionProjectProgressItems.Where(p => p.ConstructionProjectId.Equals(constructionProjectId)).Any())
                    {
                        var list = projectsApiDb.ConstructionProjectProgressItems.Where(p => p.ConstructionProjectId.Equals(constructionProjectId)).AsQueryable();

                        if (childsUsersIds != null)
                        {
                            if (childsUsersIds.Count > 1)
                            {
                                list = list.Where(p => p.UserIdCreator.HasValue).Where(p => childsUsersIds.Contains(p.UserIdCreator.Value));
                            }
                            else
                            {
                                if (childsUsersIds.Count == 1)
                                {
                                    if (childsUsersIds.FirstOrDefault() > 0)
                                    {
                                        list = list.Where(p => p.UserIdCreator.HasValue).Where(p => childsUsersIds.Contains(p.UserIdCreator.Value));
                                    }
                                }
                            }
                        }

                        var constructionProjectProgressItemsList = list.ToList();

                        var mainRoot = constructionProjectProgressItemsList.Where(i => !i.ConstructionProjectProgressParentItemId.HasValue).FirstOrDefault();

                        if (mainRoot != null)
                        {
                            var roots = constructionProjectProgressItemsList.Where(i => i.ConstructionProjectProgressParentItemId.HasValue).
                                Where(i => i.ConstructionProjectProgressParentItemId.Value.Equals(mainRoot.ConstructionProjectProgressItemId)).ToList();

                            if (roots != null)
                            {
                                if (roots.Count > 0)
                                {
                                    foreach (var root in roots)
                                    {
                                        try
                                        {
                                            HierarchyProjectProgressItemsVM hierarchyProjectProgressItemsVM = new HierarchyProjectProgressItemsVM();
                                            hierarchyProjectProgressItemsVM.name = root.ConstructionProjectProgressItemTitle;

                                            var childs = constructionProjectProgressItemsList.Where(i => i.ConstructionProjectProgressParentItemId.HasValue).
                                                Where(i => i.ConstructionProjectProgressParentItemId.Value.Equals(root.ConstructionProjectProgressItemId)).ToList();

                                            List<Data> childsData = new List<Data>();

                                            int y = 0;

                                            Data rootData = new Data();
                                            rootData.name = root.ConstructionProjectProgressItemTitle;
                                            rootData.id = root.ConstructionProjectProgressItemId.ToString();
                                            //childData.parent = root.ConstructionProjectProgressItemId.ToString();

                                            rootData.start = PersianDate.ToGregorianDate(root.ActivityStart).ToShortDateString().Replace("-", "/");
                                            rootData.end = PersianDate.ToGregorianDate(root.ActivityEnd).ToShortDateString().Replace("-", "/");

                                            //rootData.start = root.ActivityStart;
                                            //rootData.end = root.ActivityEnd;

                                            //childData.milestone = "false";
                                            rootData.dependency = "";
                                            rootData.owner = "";
                                            rootData.y = y;

                                            rootData.completed = new Completed();

                                            if (!string.IsNullOrEmpty(root.CumulativeOperation))
                                                rootData.completed.amount = (double.Parse(root.CumulativeOperation.Replace("%", "")) / 100);
                                            else
                                                rootData.completed.amount = 0;

                                            //childData.Completed.Fill = "#e80";

                                            childsData.Add(rootData);
                                            y++;

                                            foreach (var child in childs)
                                            {
                                                try
                                                {
                                                    Data childData = new Data();
                                                    childData.name = child.ConstructionProjectProgressItemTitle;
                                                    childData.id = child.ConstructionProjectProgressItemId.ToString();
                                                    childData.parent = root.ConstructionProjectProgressItemId.ToString();

                                                    childData.start = PersianDate.ToGregorianDate(child.ActivityStart).ToShortDateString().Replace("-", "/");
                                                    childData.end = PersianDate.ToGregorianDate(child.ActivityEnd).ToShortDateString().Replace("-", "/");

                                                    //childData.start = child.ActivityStart;
                                                    //childData.end = child.ActivityEnd;

                                                    //childData.milestone = "false";
                                                    childData.dependency = "";
                                                    childData.owner = "";
                                                    childData.y = y;

                                                    childData.completed = new Completed();

                                                    childData.completed.fill = "#e80";

                                                    if (!string.IsNullOrEmpty(child.CumulativeOperation))
                                                        childData.completed.amount = (double.Parse(child.CumulativeOperation.Replace("%", "")) / 100);
                                                    else
                                                        childData.completed.amount = 0;

                                                    //childData.Completed.Fill = "#e80";

                                                    childsData.Add(childData);
                                                    y++;
                                                }
                                                catch (Exception exc) { }
                                            }

                                            hierarchyProjectProgressItemsVM.data = childsData;

                                            hierarchyProjectProgressItemsVMList.Add(hierarchyProjectProgressItemsVM);
                                        }
                                        catch (Exception exc)
                                        { }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                #endregion
            }

            return hierarchyProjectProgressItemsVMList;
        }


        #endregion

        #region Methods For Work With ConstructionProjectDelays

        public List<ConstructionProjectDelaysVM> GetAllConstructionProjectDelays(
          List<long> childsUsersIds,
                int? constructionProjectBillDelayId = 0,
                long? ConstructionProjectId = 0)
        {

            List<ConstructionProjectDelaysVM> constructionProjectDelaysVMList = new List<ConstructionProjectDelaysVM>();

            try
            {
                var list = (from p in projectsApiDb.ConstructionProjectDelays

                            select new ConstructionProjectDelaysVM
                            {
                                ConstructionProjectBillDelayId = p.ConstructionProjectBillDelayId,
                                ConstructionProjectId = p.ConstructionProjectId,
                                ConstructionProjectsDelayDates = p.ConstructionProjectsDelayDates,
                                ConstructionProjectsDelayId = p.ConstructionProjectsDelayId,
                                UserIdCreator = p.UserIdCreator.Value,
                                CreateEnDate = p.CreateEnDate,
                                CreateTime = p.CreateTime,
                                EditEnDate = p.EditEnDate,
                                EditTime = p.EditTime,
                                UserIdEditor = p.UserIdEditor.Value,
                                RemoveEnDate = p.RemoveEnDate,
                                RemoveTime = p.EditTime,
                                UserIdRemover = p.UserIdRemover.Value,
                                IsActivated = p.IsActivated,
                                IsDeleted = p.IsDeleted,

                            })
                               .AsEnumerable();

                if (ConstructionProjectId.HasValue && ConstructionProjectId.Value != 0)
                {
                    list = list.Where(a => a.ConstructionProjectId == ConstructionProjectId);
                }



                if (constructionProjectBillDelayId.HasValue && constructionProjectBillDelayId.Value != 0)
                {
                    list = list.Where(a => a.ConstructionProjectBillDelayId == constructionProjectBillDelayId);
                }


                constructionProjectDelaysVMList = list.OrderByDescending(s => s.ConstructionProjectsDelayId).ToList();
            }
            catch (Exception exc)
            { }
            return constructionProjectDelaysVMList;
        }




        public List<ConstructionProjectDelaysVM> GetListOfConstructionProjectDelays(int jtStartIndex,
                int jtPageSize,
                ref int listCount,
                List<long> childsUsersIds,
                int? constructionProjectBillDelayId = 0,
                long? ConstructionProjectId = null,
                string jtSorting = null)
        {

            List<ConstructionProjectDelaysVM> constructionProjectDelaysVMList = new List<ConstructionProjectDelaysVM>();

            try
            {
                var list = (from p in projectsApiDb.ConstructionProjectDelays
                            where childsUsersIds.Contains(p.UserIdCreator.Value) &&
                            p.ConstructionProjectId.Equals(ConstructionProjectId)
                            select new ConstructionProjectDelaysVM
                            {
                                ConstructionProjectBillDelayId = p.ConstructionProjectBillDelayId,
                                ConstructionProjectId = p.ConstructionProjectId,
                                ConstructionProjectsDelayDates = p.ConstructionProjectsDelayDates,
                                ConstructionProjectsDelayId = p.ConstructionProjectsDelayId,
                                UserIdCreator = p.UserIdCreator.Value,
                                CreateEnDate = p.CreateEnDate,
                                CreateTime = p.CreateTime,
                                EditEnDate = p.EditEnDate,
                                EditTime = p.EditTime,
                                UserIdEditor = p.UserIdEditor.Value,
                                RemoveEnDate = p.RemoveEnDate,
                                RemoveTime = p.EditTime,
                                UserIdRemover = p.UserIdRemover.Value,
                                IsActivated = p.IsActivated,
                                IsDeleted = p.IsDeleted,

                            }).AsEnumerable();


                if (ConstructionProjectId.HasValue && ConstructionProjectId.Value != 0)
                {
                    list = list.Where(a => a.ConstructionProjectId == ConstructionProjectId);
                }




                if (constructionProjectBillDelayId.HasValue && constructionProjectBillDelayId.Value != 0)
                {
                    list = list.Where(a => a.ConstructionProjectBillDelayId == constructionProjectBillDelayId);
                }


                listCount = list.Count();

                try
                {
                    if (string.IsNullOrEmpty(jtSorting))
                    {

                        if (listCount > jtPageSize)
                        {

                            constructionProjectDelaysVMList = list.OrderByDescending(s => s.ConstructionProjectsDelayId)
                                     .Skip(jtStartIndex).Take(jtPageSize).ToList();
                        }
                        else
                            constructionProjectDelaysVMList = list.OrderByDescending(s => s.ConstructionProjectsDelayId).ToList();
                    }
                    else
                    {


                        switch (jtSorting)
                        {
                            case "CreateEnDate ASC":
                                list = list.OrderBy(l => l.CreateEnDate);
                                break;
                            case "CreateEnDate DESC":
                                list = list.OrderByDescending(l => l.CreateEnDate);
                                break;
                        }
                        if (listCount > jtPageSize)
                        {
                            constructionProjectDelaysVMList = list.Skip(jtStartIndex).Take(jtPageSize).ToList();
                        }
                        else
                        {

                            constructionProjectDelaysVMList = list.ToList();
                        }
                    }
                }
                catch (Exception exc)
                { }
            }
            catch (Exception exc)
            { }
            return constructionProjectDelaysVMList;
        }


        public long AddToConstructionProjectDelays(
            List<long> childsUsersIds,
            ConstructionProjectDelaysVM constructionProjectDelaysVM)
        {

            try
            {
                ConstructionProjectDelays constructionProjectDelays = _mapper.Map<ConstructionProjectDelaysVM, ConstructionProjectDelays>(constructionProjectDelaysVM);
                constructionProjectDelays.IsActivated = true;
                constructionProjectDelays.IsDeleted = false;

                projectsApiDb.ConstructionProjectDelays.Add(constructionProjectDelays);
                projectsApiDb.SaveChanges();



                return constructionProjectDelays.ConstructionProjectsDelayId;
            }
            catch (Exception exc)
            { }

            return 0;

        }



        public bool UpdateConstructionProjectDelays(
         List<long> childsUsersIds,
         ConstructionProjectDelaysVM constructionProjectDelaysVM)
        {

            try
            {
                var constructionProjectDelayId = constructionProjectDelaysVM.ConstructionProjectsDelayId;

                ConstructionProjectDelays projectDelay = projectsApiDb.ConstructionProjectDelays.Where(c => c.ConstructionProjectsDelayId == constructionProjectDelayId).FirstOrDefault();
                projectDelay.ConstructionProjectBillDelayId = (int)constructionProjectDelaysVM.ConstructionProjectBillDelayId;
                projectDelay.ConstructionProjectsDelayDates = constructionProjectDelaysVM.ConstructionProjectsDelayDates;

                projectsApiDb.ConstructionProjectDelays.Update(projectDelay);
                projectsApiDb.SaveChanges();



                return true;
            }
            catch (Exception exc)
            { }

            return false;

        }




        public bool ToggleActivationConstructionProjectDelays(
                long ConstructionProjectsDelayId,
                long userId,
                List<long> childsUsersIds)
        {
            try
            {
                var constructionProjectDelays = (from c in projectsApiDb.ConstructionProjectDelays
                                                 where c.ConstructionProjectsDelayId == ConstructionProjectsDelayId &&
                                                 childsUsersIds.Contains(c.UserIdCreator.Value)
                                                 select c).FirstOrDefault();

                if (constructionProjectDelays != null)
                {
                    constructionProjectDelays.IsActivated = !constructionProjectDelays.IsActivated;
                    constructionProjectDelays.EditEnDate = DateTime.Now;
                    constructionProjectDelays.EditTime = PersianDate.TimeNow;
                    constructionProjectDelays.UserIdEditor = userId;

                    projectsApiDb.Entry<Entities.ConstructionProjectDelays>(constructionProjectDelays).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }




        public bool TemporaryDeleteConstructionProjectDelays(
            long ConstructionProjectsDelayId,
                 long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var constructionProjectDelays = (from c in projectsApiDb.ConstructionProjectDelays
                                                 where c.ConstructionProjectsDelayId == ConstructionProjectsDelayId &&
                                                 childsUsersIds.Contains(c.UserIdCreator.Value)
                                                 select c).FirstOrDefault();

                if (constructionProjectDelays != null)
                {
                    constructionProjectDelays.IsDeleted = !constructionProjectDelays.IsDeleted;
                    constructionProjectDelays.EditEnDate = DateTime.Now;
                    constructionProjectDelays.EditTime = PersianDate.TimeNow;
                    constructionProjectDelays.UserIdEditor = userId;

                    projectsApiDb.Entry<Entities.ConstructionProjectDelays>(constructionProjectDelays).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }



        public bool? CompleteDeleteConstructionProjectDelays(
            long constructionProjectDelayId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var constructionDelayProject = projectsApiDb.ConstructionProjectDelays.Where(c => c.ConstructionProjectsDelayId == constructionProjectDelayId).FirstOrDefault();

                if (constructionDelayProject != null)
                {

                    try
                    {
                        projectsApiDb.ConstructionProjectDelays.Remove(constructionDelayProject);
                        projectsApiDb.SaveChanges();

                        return true;

                    }
                    catch (Exception exc)
                    {

                    }

                }


            }
            catch (Exception exc)
            { }

            return false;
        }




        public ConstructionProjectDelays GetConstructionProjectDelayById(
            List<long> childsUsersIds,
             long? ConstructionProjectDelayId = null)
        {
            ConstructionProjectDelays constructionProjectDelay = new ConstructionProjectDelays();

            try
            {
                constructionProjectDelay = projectsApiDb.ConstructionProjectDelays.Where(c => c.ConstructionProjectsDelayId == ConstructionProjectDelayId).FirstOrDefault();
            }

            catch (Exception exc)
            { }

            return constructionProjectDelay;
        }



        #endregion

        #region Methods For Work With ConstructionProjectBillDelays


        public List<ConstructionProjectBillDelaysVM> GetAllConstructionProjectBillDelaysList(List<long> childsUsersIds)
        {
            List<ConstructionProjectBillDelaysVM> constructionProjectBillDelaysVMList = new List<ConstructionProjectBillDelaysVM>();

            try
            {
                var list = projectsApiDb.ConstructionProjectBillDelays.ToList();
                constructionProjectBillDelaysVMList = _mapper.Map<List<ConstructionProjectBillDelays>, List<ConstructionProjectBillDelaysVM>>(list);
            }
            catch (Exception exc)
            { }

            return constructionProjectBillDelaysVMList;
        }



        public long AddToConstructionProjectBillDelays(List<long> childsUsersIds,
ConstructionProjectBillDelaysVM constructionProjectBillDelaysVM)
        {
            using (var transaction = projectsApiDb.Database.BeginTransaction())
            {
                try
                {
                    ConstructionProjectBillDelays constructionProjectBillDelays = _mapper.Map<ConstructionProjectBillDelaysVM, ConstructionProjectBillDelays>(constructionProjectBillDelaysVM);
                    constructionProjectBillDelays.IsActivated = true;
                    constructionProjectBillDelays.IsDeleted = false;

                    projectsApiDb.ConstructionProjectBillDelays.Add(constructionProjectBillDelays);
                    projectsApiDb.SaveChanges();

                    transaction.Commit();

                    return constructionProjectBillDelays.ConstructionProjectBillDelayId;
                }
                catch (Exception exc)
                {
                    transaction.Rollback();
                }
            }
            return 0;

        }




        public bool UpdateConstructionProjectBillDelays(List<long> childsUsersIds,
ConstructionProjectBillDelaysVM constructionProjectBillDelaysVM)
        {
            using (var transaction = projectsApiDb.Database.BeginTransaction())
            {
                try
                {
                    ConstructionProjectBillDelays constructionProjectBillDelays = projectsApiDb.ConstructionProjectBillDelays.Where(c => c.ConstructionProjectBillDelayId == constructionProjectBillDelaysVM.ConstructionProjectBillDelayId).FirstOrDefault();
                    constructionProjectBillDelays.ConstructionProjectBillDelayTitle = constructionProjectBillDelaysVM.ConstructionProjectBillDelayTitle;

                    projectsApiDb.ConstructionProjectBillDelays.Update(constructionProjectBillDelays);
                    projectsApiDb.SaveChanges();

                    transaction.Commit();

                    return true;
                }
                catch (Exception exc)
                {
                    transaction.Rollback();
                }
            }
            return false;

        }



        public List<ConstructionProjectBillDelaysForOuterDashboardVM> GetListOfBillDelaysForOuterDashboard(long? ConstructionProjectId, List<long> childsUsersIds)
        {
            List<ConstructionProjectBillDelaysForOuterDashboardVM> constructionProjectBillDelaysForOuterDashboardVMList = new List<ConstructionProjectBillDelaysForOuterDashboardVM>();

            try
            {
                var DelaysList = projectsApiDb.ConstructionProjectDelays.Where(c => c.ConstructionProjectId == ConstructionProjectId).OrderBy(c => c.ConstructionProjectBillDelayId).ToList();
                var BillDelaysList = projectsApiDb.ConstructionProjectBillDelays.ToList();

                #region Commented - Show bills that are available in delay list
                //foreach(var delay in DelaysList)
                //{
                //    if (constructionProjectBillDelaysForOuterDashboardVMList.Where(c => c.ConstructionProjectBillDelayId == delay.ConstructionProjectBillDelayId).Any())
                //    {
                //        var constructionProjectBillDelaysForOuterDashboard = constructionProjectBillDelaysForOuterDashboardVMList.Where(c => c.ConstructionProjectBillDelayId == delay.ConstructionProjectBillDelayId).FirstOrDefault();
                //        constructionProjectBillDelaysForOuterDashboard.DelaysCount += delay.ConstructionProjectsDelayDates.Split(',').Length;
                //    }
                //    else
                //    {
                //        ConstructionProjectBillDelaysForOuterDashboardVM constructionProjectBillDelaysForOuterDashboard = new ConstructionProjectBillDelaysForOuterDashboardVM();
                //        constructionProjectBillDelaysForOuterDashboard.ConstructionProjectBillDelayId = delay.ConstructionProjectBillDelayId;
                //        constructionProjectBillDelaysForOuterDashboard.ConstructionProjectBillDelayTitle = BillDelaysList.Where(b => b.ConstructionProjectBillDelayId == delay.ConstructionProjectBillDelayId).Select(b => b.ConstructionProjectBillDelayTitle).FirstOrDefault();
                //        constructionProjectBillDelaysForOuterDashboard.DelaysCount = delay.ConstructionProjectsDelayDates.Split(',').Length;
                //        constructionProjectBillDelaysForOuterDashboardVMList.Add(constructionProjectBillDelaysForOuterDashboard);
                //    }
                //}
                #endregion

                foreach (var bill in BillDelaysList)
                {
                    ConstructionProjectBillDelaysForOuterDashboardVM constructionProjectBillDelaysForOuterDashboard = new ConstructionProjectBillDelaysForOuterDashboardVM();
                    constructionProjectBillDelaysForOuterDashboard.ConstructionProjectBillDelayId = bill.ConstructionProjectBillDelayId;
                    constructionProjectBillDelaysForOuterDashboard.ConstructionProjectBillDelayTitle = bill.ConstructionProjectBillDelayTitle;
                    constructionProjectBillDelaysForOuterDashboard.DelaysCount = 0;
                    var MatchedDelaysWithBillIdList = DelaysList.Where(d => d.ConstructionProjectBillDelayId == bill.ConstructionProjectBillDelayId).ToList();
                    foreach (var delay in MatchedDelaysWithBillIdList)
                    {
                        constructionProjectBillDelaysForOuterDashboard.DelaysCount += delay.ConstructionProjectsDelayDates.Split(',').Length;
                    }
                    constructionProjectBillDelaysForOuterDashboardVMList.Add(constructionProjectBillDelaysForOuterDashboard);
                }


            }
            catch (Exception exc)
            { }

            return constructionProjectBillDelaysForOuterDashboardVMList;
        }
        #endregion

        #region Methods For Work With ExcelSheetConfigs

        public List<ExcelSheetConfigsVM> GetAllExcelSheetConfigsList(
            //List<long> childsUsersIds,
            string? ExcelSheetConfigName = "",
            long? GoogleSheetConfigId = 0)
        {

            List<ExcelSheetConfigsVM> excelSheetConfigsVMList = new List<ExcelSheetConfigsVM>();

            try
            {
                var list = (from p in projectsApiDb.ExcelSheetConfigs
                            select new ExcelSheetConfigsVM
                            {
                                ExcelSheetConfigId = p.ExcelSheetConfigId,
                                ExcelSheetConfigName = p.ExcelSheetConfigName,
                                ExcelSheetHour = p.ExcelSheetHour,
                                GId = p.GId,
                                GoogleSheetConfigId = p.GoogleSheetConfigId,

                                UserIdCreator = p.UserIdCreator.Value,
                                CreateEnDate = p.CreateEnDate,
                                CreateTime = p.CreateTime,
                                EditEnDate = p.EditEnDate,
                                EditTime = p.EditTime,
                                UserIdEditor = p.UserIdEditor.Value,
                                RemoveEnDate = p.RemoveEnDate,
                                RemoveTime = p.EditTime,
                                UserIdRemover = p.UserIdRemover.Value,
                                IsActivated = p.IsActivated,
                                IsDeleted = p.IsDeleted,

                            })
                               .AsEnumerable();

                if (GoogleSheetConfigId.HasValue && GoogleSheetConfigId.Value != 0)
                {
                    list = list.Where(a => a.GoogleSheetConfigId == GoogleSheetConfigId);
                }

                if (!string.IsNullOrEmpty(ExcelSheetConfigName))
                    list = list.Where(a => a.ExcelSheetConfigName.Contains(ExcelSheetConfigName));
                excelSheetConfigsVMList = list.OrderByDescending(s => s.ExcelSheetConfigId).ToList();
            }
            catch (Exception exc)
            { }
            return excelSheetConfigsVMList;
        }
        public List<ExcelSheetConfigsVM> GetListOfExcelSheetConfigs(int jtStartIndex,
              int jtPageSize,
              ref int listCount,
              //List<long> childsUsersIds,
              string? ExcelSheetConfigName = "",
              long? GoogleSheetConfigId = 0,
              string jtSorting = null)
        {

            List<ExcelSheetConfigsVM> excelSheetConfigsVMList = new List<ExcelSheetConfigsVM>();

            try
            {
                var list = (from p in projectsApiDb.ExcelSheetConfigs
                            select new ExcelSheetConfigsVM
                            {
                                ExcelSheetConfigId = p.ExcelSheetConfigId,
                                ExcelSheetConfigName = p.ExcelSheetConfigName,
                                ExcelSheetHour = p.ExcelSheetHour,
                                GId = p.GId,
                                GoogleSheetConfigId = p.GoogleSheetConfigId,

                                UserIdCreator = p.UserIdCreator.Value,
                                CreateEnDate = p.CreateEnDate,
                                CreateTime = p.CreateTime,
                                EditEnDate = p.EditEnDate,
                                EditTime = p.EditTime,
                                UserIdEditor = p.UserIdEditor.Value,
                                RemoveEnDate = p.RemoveEnDate,
                                RemoveTime = p.EditTime,
                                UserIdRemover = p.UserIdRemover.Value,
                                IsActivated = p.IsActivated,
                                IsDeleted = p.IsDeleted,

                            })
                               .AsEnumerable();

                if (GoogleSheetConfigId.HasValue && GoogleSheetConfigId.Value != 0)
                {
                    list = list.Where(a => a.GoogleSheetConfigId == GoogleSheetConfigId);
                }

                if (!string.IsNullOrEmpty(ExcelSheetConfigName))
                    list = list.Where(a => a.ExcelSheetConfigName.Contains(ExcelSheetConfigName));

                listCount = list.Count();
                try
                {

                    if (listCount > jtPageSize)
                    {

                        excelSheetConfigsVMList = list.OrderByDescending(s => s.ExcelSheetConfigId)
                                 .Skip(jtStartIndex).Take(jtPageSize).ToList();
                    }
                    else
                        excelSheetConfigsVMList = list.OrderByDescending(s => s.ExcelSheetConfigId).ToList();

                }
                catch (Exception exc)
                { }
            }
            catch (Exception exc)
            { }
            return excelSheetConfigsVMList;
        }


        public long AddToExcelSheetConfigs(ExcelSheetConfigsVM excelSheetConfigsVM)
        {

            try
            {

                ExcelSheetConfigs excelSheetConfig = _mapper.Map<ExcelSheetConfigsVM, ExcelSheetConfigs>(excelSheetConfigsVM);


                projectsApiDb.ExcelSheetConfigs.Add(excelSheetConfig);
                projectsApiDb.SaveChanges();

                return excelSheetConfig.ExcelSheetConfigId;
            }
            catch (Exception)
            {
            }

            return 0;

        }

        public long UpdateExcelSheetConfigs(
            //List<long> childsUsersIds,
            ExcelSheetConfigsVM excelSheetConfigsVM)
        {
            var ExcelSheetConfigId = excelSheetConfigsVM.ExcelSheetConfigId;
            if (projectsApiDb.ExcelSheetConfigs//.Where(x => childsUsersIds.Contains(x.UserIdCreator.Value))
                .Where(x => x.ExcelSheetConfigId.Equals(ExcelSheetConfigId)).Any())
            {
                try
                {
                    bool? isActivated = excelSheetConfigsVM.IsActivated.HasValue ? excelSheetConfigsVM.IsActivated.Value : (bool?)true;
                    bool? isDeleted = excelSheetConfigsVM.IsDeleted.HasValue ? excelSheetConfigsVM.IsDeleted.Value : (bool?)true;

                    ExcelSheetConfigs excelSheetConfig = projectsApiDb.ExcelSheetConfigs.Where(a => a.ExcelSheetConfigId == ExcelSheetConfigId).FirstOrDefault();
                    excelSheetConfig.GoogleSheetConfigId = excelSheetConfigsVM.GoogleSheetConfigId;
                    excelSheetConfig.ExcelSheetConfigName = excelSheetConfigsVM.ExcelSheetConfigName;
                    excelSheetConfig.ExcelSheetHour = excelSheetConfigsVM.ExcelSheetHour;
                    excelSheetConfig.GId = excelSheetConfigsVM.GId;


                    excelSheetConfig.EditTime = excelSheetConfigsVM.EditTime;
                    excelSheetConfig.EditEnDate = excelSheetConfigsVM.EditEnDate;
                    excelSheetConfig.UserIdEditor = excelSheetConfigsVM.UserIdEditor;
                    excelSheetConfig.IsActivated = isActivated;
                    excelSheetConfig.IsDeleted = isDeleted;

                    projectsApiDb.Entry<ExcelSheetConfigs>(excelSheetConfig).State = EntityState.Modified;

                    projectsApiDb.SaveChanges();
                    return excelSheetConfig.ExcelSheetConfigId;
                }
                catch (Exception)
                {

                }
            }
            return 0;
        }

        public bool ToggleActivationExcelSheetConfigs(long excelSheetConfigId,
            long userId//,
                       //List<long> childsUsersIds
            )
        {
            try
            {
                var excelSheetConfigs = (from c in projectsApiDb.ExcelSheetConfigs
                                         where c.ExcelSheetConfigId == excelSheetConfigId //&&
                                         //childsUsersIds.Contains(c.UserIdCreator.Value)
                                         select c).FirstOrDefault();

                if (excelSheetConfigs != null)
                {
                    excelSheetConfigs.IsActivated = !excelSheetConfigs.IsActivated;
                    excelSheetConfigs.EditEnDate = DateTime.Now;
                    excelSheetConfigs.EditTime = PersianDate.TimeNow;
                    excelSheetConfigs.UserIdEditor = userId;

                    projectsApiDb.Entry<ExcelSheetConfigs>(excelSheetConfigs).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool TemporaryDeleteExcelSheetConfigs(long excelSheetConfigId,
            long userId//,
                       //List<long> childsUsersIds
            )
        {
            try
            {
                var excelSheetConfigs = (from c in projectsApiDb.ExcelSheetConfigs
                                         where c.ExcelSheetConfigId == excelSheetConfigId// &&
                                         //childsUsersIds.Contains(c.UserIdCreator.Value)
                                         select c).FirstOrDefault();

                if (excelSheetConfigs != null)
                {
                    excelSheetConfigs.IsDeleted = !excelSheetConfigs.IsDeleted;
                    excelSheetConfigs.EditEnDate = DateTime.Now;
                    excelSheetConfigs.EditTime = PersianDate.TimeNow;
                    excelSheetConfigs.UserIdEditor = userId;

                    projectsApiDb.Entry<ExcelSheetConfigs>(excelSheetConfigs).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool CompleteDeleteExcelSheetConfigs(long excelSheetConfigId,
            long userId//,
                       //List<long> childsUsersIds
            )
        {

            try
            {
                var excelSheetConfig = (from c in projectsApiDb.ExcelSheetConfigs
                                        where c.ExcelSheetConfigId == excelSheetConfigId //&&
                                        //childsUsersIds.Contains(c.UserIdCreator.Value)
                                        select c).FirstOrDefault();

                if (excelSheetConfig != null)
                {
                    projectsApiDb.ExcelSheetConfigs.Remove(excelSheetConfig);
                    projectsApiDb.SaveChanges();


                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;


        }

        public ExcelSheetConfigsVM GetExcelSheetConfigsWithExcelSheetConfigId(long excelSheetConfigId,
            long userId//,
                       //List<long> childsUsersIds)
            )
        {
            ExcelSheetConfigsVM excelSheetConfigsVM = new ExcelSheetConfigsVM();

            try
            {
                excelSheetConfigsVM = _mapper.Map<ExcelSheetConfigs,
                    ExcelSheetConfigsVM>(projectsApiDb.ExcelSheetConfigs
                     //.Where(p => childsUsersIds.Contains(p.UserIdCreator.Value))
                     .Where(e => e.ExcelSheetConfigId.Equals(excelSheetConfigId)).FirstOrDefault());

            }
            catch (Exception exc)
            { }

            return excelSheetConfigsVM;
        }
        #endregion

        #region Methods For Work With ExcelSheetConfigHistories

        public List<ExcelSheetConfigHistoriesVM> GetAllExcelSheetConfigHistoriesList(
            //List<long> childsUsersIds,
            //string? ExcelSheetConfigTitle = "",
            long? ExcelSheetConfigId = 0)
        {

            List<ExcelSheetConfigHistoriesVM> excelSheetConfigHistoriesVMList = new List<ExcelSheetConfigHistoriesVM>();

            try
            {
                var list = (from p in projectsApiDb.ExcelSheetConfigHistories
                            select new ExcelSheetConfigHistoriesVM
                            {
                                ExcelSheetConfigHistoryId = p.ExcelSheetConfigHistoryId,
                                ExcelSheetConfigId = p.ExcelSheetConfigId,
                                CountOfReadRows = p.CountOfReadRows,
                                ExcelSheetConfigHistoryFileExt = p.ExcelSheetConfigHistoryFileExt,
                                ExcelSheetConfigHistoryFilePath = p.ExcelSheetConfigHistoryFilePath,
                                LastReadRow = p.LastReadRow,

                                UserIdCreator = p.UserIdCreator.Value,
                                CreateEnDate = p.CreateEnDate,
                                CreateTime = p.CreateTime,
                                EditEnDate = p.EditEnDate,
                                EditTime = p.EditTime,
                                UserIdEditor = p.UserIdEditor.Value,
                                RemoveEnDate = p.RemoveEnDate,
                                RemoveTime = p.EditTime,
                                UserIdRemover = p.UserIdRemover.Value,
                                IsActivated = p.IsActivated,
                                IsDeleted = p.IsDeleted,

                            })
                               .AsEnumerable();

                if (ExcelSheetConfigId.HasValue && ExcelSheetConfigId.Value != 0)
                {
                    list = list.Where(a => a.ExcelSheetConfigId == ExcelSheetConfigId);
                }

                //if (!string.IsNullOrEmpty(ExcelSheetConfigTitle))
                //    list = list.Where(a => a.ExcelSheetConfigTitle.Contains(ExcelSheetConfigTitle));
                excelSheetConfigHistoriesVMList = list.OrderByDescending(s => s.ExcelSheetConfigId).ToList();
            }
            catch (Exception exc)
            { }
            return excelSheetConfigHistoriesVMList;
        }

        public List<ExcelSheetConfigHistoriesVM> GetListOfExcelSheetConfigHistories(int jtStartIndex,
              int jtPageSize,
              ref int listCount,
              //List<long> childsUsersIds,
              //string? ExcelSheetConfigTitle = "",
              long? ExcelSheetConfigId = 0,
              string jtSorting = null)
        {

            List<ExcelSheetConfigHistoriesVM> excelSheetConfigHistoriesVMList = new List<ExcelSheetConfigHistoriesVM>();

            try
            {
                var list = (from p in projectsApiDb.ExcelSheetConfigHistories
                            select new ExcelSheetConfigHistoriesVM
                            {
                                ExcelSheetConfigHistoryId = p.ExcelSheetConfigHistoryId,
                                ExcelSheetConfigId = p.ExcelSheetConfigId,
                                CountOfReadRows = p.CountOfReadRows,
                                ExcelSheetConfigHistoryFileExt = p.ExcelSheetConfigHistoryFileExt,
                                ExcelSheetConfigHistoryFilePath = p.ExcelSheetConfigHistoryFilePath,
                                LastReadRow = p.LastReadRow,
                                UserIdCreator = p.UserIdCreator.Value,
                                CreateEnDate = p.CreateEnDate,
                                CreateTime = p.CreateTime,
                                EditEnDate = p.EditEnDate,
                                EditTime = p.EditTime,
                                UserIdEditor = p.UserIdEditor.Value,
                                RemoveEnDate = p.RemoveEnDate,
                                RemoveTime = p.EditTime,
                                UserIdRemover = p.UserIdRemover.Value,
                                IsActivated = p.IsActivated,
                                IsDeleted = p.IsDeleted,

                            })
                               .AsEnumerable();

                if (ExcelSheetConfigId.HasValue && ExcelSheetConfigId.Value != 0)
                {
                    list = list.Where(a => a.ExcelSheetConfigId == ExcelSheetConfigId);
                }

                //if (!string.IsNullOrEmpty(ExcelSheetConfigTitle))
                //    list = list.Where(a => a.ExcelSheetConfigTitle.Contains(ExcelSheetConfigTitle));

                listCount = list.Count();
                try
                {

                    if (listCount > jtPageSize)
                    {

                        excelSheetConfigHistoriesVMList = list.OrderByDescending(s => s.ExcelSheetConfigId)
                                 .Skip(jtStartIndex).Take(jtPageSize).ToList();
                    }
                    else
                        excelSheetConfigHistoriesVMList = list.OrderByDescending(s => s.ExcelSheetConfigId).ToList();

                }
                catch (Exception exc)
                { }
            }
            catch (Exception exc)
            { }
            return excelSheetConfigHistoriesVMList;
        }

        public long AddToExcelSheetConfigHistories(
            ExcelSheetConfigHistoriesVM excelSheetConfigHistoriesVM)
        {

            try
            {

                ExcelSheetConfigHistories excelSheetConfig = _mapper.Map<ExcelSheetConfigHistoriesVM, ExcelSheetConfigHistories>(excelSheetConfigHistoriesVM);
                projectsApiDb.ExcelSheetConfigHistories.Add(excelSheetConfig);
                projectsApiDb.SaveChanges();

                return excelSheetConfig.ExcelSheetConfigId;
            }
            catch (Exception)
            {
            }

            return 0;

        }

        public long UpdateExcelSheetConfigHistories(
            //List<long> childsUsersIds,
            ExcelSheetConfigHistoriesVM excelSheetConfigHistoriesVM)
        {
            var ExcelSheetConfigHistoryId = excelSheetConfigHistoriesVM.ExcelSheetConfigHistoryId;
            if (projectsApiDb.ExcelSheetConfigHistories//.Where(x => childsUsersIds.Contains(x.UserIdCreator.Value))
                .Where(x => x.ExcelSheetConfigHistoryId.Equals(ExcelSheetConfigHistoryId)).Any())
            {
                try
                {
                    bool? isActivated = excelSheetConfigHistoriesVM.IsActivated.HasValue ? excelSheetConfigHistoriesVM.IsActivated.Value : (bool?)true;
                    bool? isDeleted = excelSheetConfigHistoriesVM.IsDeleted.HasValue ? excelSheetConfigHistoriesVM.IsDeleted.Value : (bool?)true;

                    ExcelSheetConfigHistories excelSheetConfigHistories = projectsApiDb.ExcelSheetConfigHistories.Where(a => a.ExcelSheetConfigId == ExcelSheetConfigHistoryId).FirstOrDefault();
                    //excelSheetConfig.ExcelSheetConfigHistoryId = excelSheetConfigHistoriesVM.ExcelSheetConfigHistoryId;
                    excelSheetConfigHistories.ExcelSheetConfigId = excelSheetConfigHistoriesVM.ExcelSheetConfigId;
                    excelSheetConfigHistories.CountOfReadRows = excelSheetConfigHistoriesVM.CountOfReadRows;
                    excelSheetConfigHistories.ExcelSheetConfigHistoryFileExt = excelSheetConfigHistoriesVM.ExcelSheetConfigHistoryFileExt;
                    excelSheetConfigHistories.ExcelSheetConfigHistoryFilePath = excelSheetConfigHistoriesVM.ExcelSheetConfigHistoryFilePath;
                    excelSheetConfigHistories.LastReadRow = excelSheetConfigHistoriesVM.LastReadRow;
                    excelSheetConfigHistories.EditTime = excelSheetConfigHistoriesVM.EditTime;
                    excelSheetConfigHistories.EditEnDate = excelSheetConfigHistoriesVM.EditEnDate;
                    excelSheetConfigHistories.UserIdEditor = excelSheetConfigHistoriesVM.UserIdEditor;
                    excelSheetConfigHistories.IsActivated = isActivated;
                    excelSheetConfigHistories.IsDeleted = isDeleted;

                    projectsApiDb.Entry<ExcelSheetConfigHistories>(excelSheetConfigHistories).State = EntityState.Modified;

                    projectsApiDb.SaveChanges();
                    return excelSheetConfigHistories.ExcelSheetConfigHistoryId;
                }
                catch (Exception)
                {

                }
            }
            return 0;
        }

        public bool ToggleActivationExcelSheetConfigHistories(long excelSheetConfigHistoryId,
            long userId
            //List<long> childsUsersIds)
            )
        {
            try
            {
                var excelSheetConfigHistories = (from c in projectsApiDb.ExcelSheetConfigHistories
                                                 where c.ExcelSheetConfigHistoryId == excelSheetConfigHistoryId //&&
                                                                                                                //childsUsersIds.Contains(c.UserIdCreator.Value)
                                                 select c).FirstOrDefault();

                if (excelSheetConfigHistories != null)
                {
                    excelSheetConfigHistories.IsActivated = !excelSheetConfigHistories.IsActivated;
                    excelSheetConfigHistories.EditEnDate = DateTime.Now;
                    excelSheetConfigHistories.EditTime = PersianDate.TimeNow;
                    excelSheetConfigHistories.UserIdEditor = userId;

                    projectsApiDb.Entry<Entities.ExcelSheetConfigHistories>(excelSheetConfigHistories).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool TemporaryDeleteExcelSheetConfigHistories(long excelSheetConfigHistoryId,
            long userId
            //List<long> childsUsersIds)
            )
        {
            try
            {
                var excelSheetConfigHistories = (from c in projectsApiDb.ExcelSheetConfigHistories
                                                 where c.ExcelSheetConfigHistoryId == excelSheetConfigHistoryId //&&
                                                                                                                //childsUsersIds.Contains(c.UserIdCreator.Value)
                                                 select c).FirstOrDefault();

                if (excelSheetConfigHistories != null)
                {
                    excelSheetConfigHistories.IsDeleted = !excelSheetConfigHistories.IsDeleted;
                    excelSheetConfigHistories.EditEnDate = DateTime.Now;
                    excelSheetConfigHistories.EditTime = PersianDate.TimeNow;
                    excelSheetConfigHistories.UserIdEditor = userId;

                    projectsApiDb.Entry<Entities.ExcelSheetConfigHistories>(excelSheetConfigHistories).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool CompleteDeleteExcelSheetConfigHistories(long excelSheetConfigHistoryId,
            long userId
            //List<long> childsUsersIds)
            )
        {

            try
            {
                var excelSheetConfigHistories = (from c in projectsApiDb.ExcelSheetConfigHistories
                                                 where c.ExcelSheetConfigHistoryId == excelSheetConfigHistoryId //&&
                                                                                                                //childsUsersIds.Contains(c.UserIdCreator.Value)
                                                 select c).FirstOrDefault();

                if (excelSheetConfigHistories != null)
                {
                    projectsApiDb.ExcelSheetConfigHistories.Remove(excelSheetConfigHistories);
                    projectsApiDb.SaveChanges();


                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public ExcelSheetConfigHistoriesVM GetExcelSheetConfigHistoriesWithExcelSheetConfigHistoryId(long excelSheetConfigHistoryId,
            long userId//,
                       //List<long> childsUsersIds)
            )
        {
            ExcelSheetConfigHistoriesVM excelSheetConfigHistoriesVM = new ExcelSheetConfigHistoriesVM();

            try
            {
                excelSheetConfigHistoriesVM = _mapper.Map<ExcelSheetConfigHistories,
                    ExcelSheetConfigHistoriesVM>(projectsApiDb.ExcelSheetConfigHistories
                     //.Where(p => childsUsersIds.Contains(p.UserIdCreator.Value))
                     .Where(e => e.ExcelSheetConfigHistoryId.Equals(excelSheetConfigHistoryId)).FirstOrDefault());

            }
            catch (Exception exc)
            { }

            return excelSheetConfigHistoriesVM;
        }
        #endregion

        #region Methods For Work With FileStates

        public List<FileStatesVM> GetAllFileStatesList()
        {

            List<FileStatesVM> fileStatesVMList = new List<FileStatesVM>();

            try
            {
                var list = projectsApiDb.FileStates.ToList();

                fileStatesVMList = _mapper.Map<List<FileStates>, List<FileStatesVM>>(list);
            }
            catch (Exception exc)
            { }
            return fileStatesVMList;
        }

        #endregion

        #region Methods For Work With FileStatesLogs

        public List<FileStatesLogsVM> GetAllFileStatesLogsList(
             List<long> childsUsersIds,
             string? TableTitle = "",
             long? RecordId = 0,
             int? FileStateId = 0)
        {

            List<FileStatesLogsVM> fileStatesLogsVMList = new List<FileStatesLogsVM>();

            try
            {
                var list = (from p in projectsApiDb.FileStatesLogs
                            where childsUsersIds.Contains(p.UserIdCreator.Value) &&
                            p.IsDeleted.Value.Equals(false) &&
                            p.IsActivated.Value.Equals(true)
                            select p)
                            .AsQueryable();

                if (childsUsersIds != null)
                {
                    if (childsUsersIds.Count > 1)
                    {
                        list = list.Where(p => p.UserIdCreator.HasValue).Where(p => childsUsersIds.Contains(p.UserIdCreator.Value));
                    }
                    else
                    {
                        if (childsUsersIds.Count == 1)
                        {
                            if (childsUsersIds.FirstOrDefault() > 0)
                            {
                                list = list.Where(p => p.UserIdCreator.HasValue).Where(p => childsUsersIds.Contains(p.UserIdCreator.Value));
                            }
                        }
                    }
                }

                if (RecordId.HasValue)
                    if (RecordId.Value > 0)
                        list = list.Where(a => a.RecordId.Equals(RecordId));

                if (FileStateId.HasValue)
                    if (FileStateId.Value > 0)
                        list = list.Where(a => a.FileStateId.Equals(FileStateId));

                if (!string.IsNullOrEmpty(TableTitle))
                    list = list.Where(a => a.TableTitle.Contains(TableTitle));

                fileStatesLogsVMList = _mapper.Map<List<FileStatesLogs>, List<FileStatesLogsVM>>(list.OrderByDescending(s => s.FileStateLogId).ToList());
            }
            catch (Exception exc)
            { }
            return fileStatesLogsVMList;
        }

        public List<FileStatesLogsVM> GetListOfFileStatesLogs(int jtStartIndex,
             int jtPageSize,
             ref int listCount,
             List<long> childsUsersIds,
             string? TableTitle = "",
             long? RecordId = 0,
             int? FileStateId = 0,
             string jtSorting = null)
        {

            List<FileStatesLogsVM> fileStatesLogsVMList = new List<FileStatesLogsVM>();

            try
            {
                var list = (from p in projectsApiDb.FileStatesLogs
                            where childsUsersIds.Contains(p.UserIdCreator.Value) &&
                            p.IsDeleted.Value.Equals(false) &&
                            p.IsActivated.Value.Equals(true)
                            select p)
                            .AsQueryable();

                if (childsUsersIds != null)
                {
                    if (childsUsersIds.Count > 1)
                    {
                        list = list.Where(p => p.UserIdCreator.HasValue).Where(p => childsUsersIds.Contains(p.UserIdCreator.Value));
                    }
                    else
                    {
                        if (childsUsersIds.Count == 1)
                        {
                            if (childsUsersIds.FirstOrDefault() > 0)
                            {
                                list = list.Where(p => p.UserIdCreator.HasValue).Where(p => childsUsersIds.Contains(p.UserIdCreator.Value));
                            }
                        }
                    }
                }

                if (RecordId.HasValue)
                    if (RecordId.Value > 0)
                        list = list.Where(a => a.RecordId.Equals(RecordId));

                if (FileStateId.HasValue)
                    if (FileStateId.Value > 0)
                        list = list.Where(a => a.FileStateId.Equals(FileStateId));

                if (!string.IsNullOrEmpty(TableTitle))
                    list = list.Where(a => a.TableTitle.Contains(TableTitle));

                listCount = list.Count();
                try
                {
                    if (string.IsNullOrEmpty(jtSorting))
                    {

                        if (listCount > jtPageSize)
                        {
                            fileStatesLogsVMList = _mapper.Map<List<FileStatesLogs>, List<FileStatesLogsVM>>(list.OrderByDescending(s => s.FileStateLogId)
                                     .Skip(jtStartIndex).Take(jtPageSize).ToList());
                        }
                        else
                            fileStatesLogsVMList = _mapper.Map<List<FileStatesLogs>, List<FileStatesLogsVM>>(list.OrderByDescending(s => s.FileStateLogId).ToList());
                    }
                    else
                    {


                        //switch (jtSorting)
                        //{
                        //    case "InitialPlanTitle ASC":
                        //        list = list.OrderBy(l => l.InitialPlanTitle);
                        //        break;
                        //    case "InitialPlanTitle DESC":
                        //        list = list.OrderByDescending(l => l.InitialPlanTitle);
                        //        break;
                        //}

                        if (listCount > jtPageSize)
                        {
                            fileStatesLogsVMList = _mapper.Map<List<FileStatesLogs>, List<FileStatesLogsVM>>(list.OrderByDescending(s => s.FileStateLogId)
                                     .Skip(jtStartIndex).Take(jtPageSize).ToList());
                        }
                        else
                            fileStatesLogsVMList = _mapper.Map<List<FileStatesLogs>, List<FileStatesLogsVM>>(list.OrderByDescending(s => s.FileStateLogId).ToList());
                    }
                }
                catch (Exception exc)
                { }
            }
            catch (Exception exc)
            { }
            return fileStatesLogsVMList;
        }

        public long AddToFileStatesLogs(FileStatesLogsVM fileStatesLogsVM)
        {
            try
            {
                FileStatesLogs fileStatesLogs = _mapper.Map<FileStatesLogsVM, FileStatesLogs>(fileStatesLogsVM);

                projectsApiDb.FileStatesLogs.Add(fileStatesLogs);
                projectsApiDb.SaveChanges();

                return fileStatesLogs.FileStateLogId;
            }
            catch (Exception)
            {
            }
            return 0;

        }

        public FileStatesLogsVM UpdateFileStatesLogs(
            List<long> childsUsersIds,
            FileStatesLogsVM fileStatesLogsVM)
        {
            var fileStateLogId = fileStatesLogsVM.FileStateLogId;

            if (projectsApiDb.FileStatesLogs.Where(x => childsUsersIds.Contains(x.UserIdCreator.Value))
                .Where(x => x.FileStateLogId.Equals(fileStateLogId)).Any())
            {
                try
                {
                    bool? isActivated = fileStatesLogsVM.IsActivated.HasValue ? fileStatesLogsVM.IsActivated.Value : (bool?)true;
                    bool? isDeleted = fileStatesLogsVM.IsDeleted.HasValue ? fileStatesLogsVM.IsDeleted.Value : (bool?)true;

                    FileStatesLogs fileStatesLogs = projectsApiDb.FileStatesLogs.Where(a => a.FileStateLogId == fileStateLogId).FirstOrDefault();

                    fileStatesLogs.TableTitle = fileStatesLogsVM.TableTitle;
                    fileStatesLogs.RecordId = fileStatesLogsVM.RecordId;
                    fileStatesLogs.FileStateId = fileStatesLogsVM.FileStateId;

                    fileStatesLogs.EditTime = fileStatesLogsVM.EditTime;
                    fileStatesLogs.EditEnDate = fileStatesLogsVM.EditEnDate;
                    fileStatesLogs.UserIdEditor = fileStatesLogsVM.UserIdEditor;
                    fileStatesLogs.IsActivated = isActivated;
                    fileStatesLogs.IsDeleted = isDeleted;

                    projectsApiDb.Entry<FileStatesLogs>(fileStatesLogs).State = EntityState.Modified;

                    projectsApiDb.SaveChanges();

                    return _mapper.Map<FileStatesLogs, FileStatesLogsVM>(fileStatesLogs);
                }
                catch (Exception)
                {

                }
            }
            return null;
        }

        public bool ToggleActivationFileStatesLogs(long fileStateLogId,
           long userId,
           List<long> childsUsersIds)
        {
            try
            {
                var fileStatesLogs = (from c in projectsApiDb.FileStatesLogs
                                      where c.FileStateLogId == fileStateLogId &&
                                      childsUsersIds.Contains(c.UserIdCreator.Value)
                                      select c).FirstOrDefault();

                if (fileStatesLogs != null)
                {
                    fileStatesLogs.IsActivated = !fileStatesLogs.IsActivated;
                    fileStatesLogs.EditEnDate = DateTime.Now;
                    fileStatesLogs.EditTime = PersianDate.TimeNow;
                    fileStatesLogs.UserIdEditor = userId;

                    projectsApiDb.Entry<FileStatesLogs>(fileStatesLogs).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool TemporaryDeleteFileStatesLogs(long fileStateLogId,
           long userId,
           List<long> childsUsersIds)
        {
            try
            {
                var fileStatesLogs = (from c in projectsApiDb.FileStatesLogs
                                      where c.FileStateLogId == fileStateLogId &&
                                      childsUsersIds.Contains(c.UserIdCreator.Value)
                                      select c).FirstOrDefault();

                if (fileStatesLogs != null)
                {
                    fileStatesLogs.IsDeleted = !fileStatesLogs.IsDeleted;
                    fileStatesLogs.EditEnDate = DateTime.Now;
                    fileStatesLogs.EditTime = PersianDate.TimeNow;
                    fileStatesLogs.UserIdEditor = userId;

                    projectsApiDb.Entry<FileStatesLogs>(fileStatesLogs).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool? CompleteDeleteFileStatesLogs(long fileStateLogId,
            long userId,
            List<long> childsUsersIds)
        {

            try
            {
                var fileStatesLogs = (from c in projectsApiDb.FileStatesLogs
                                      where c.FileStateLogId == fileStateLogId &&
                                      childsUsersIds.Contains(c.UserIdCreator.Value)
                                      select c).FirstOrDefault();

                if (fileStatesLogs != null)
                {
                    projectsApiDb.FileStatesLogs.Remove(fileStatesLogs);
                    projectsApiDb.SaveChanges();


                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;


        }

        public FileStatesLogsVM GetFileStateLogWithFileStateLogId(long fileStateLogId,
            long userId,
            List<long> childsUsersIds)
        {
            FileStatesLogsVM fileStatesLogsVM = new FileStatesLogsVM();

            try
            {
                fileStatesLogsVM = _mapper.Map<FileStatesLogs,
                    FileStatesLogsVM>(projectsApiDb.FileStatesLogs
                    .Where(p => childsUsersIds.Contains(p.UserIdCreator.Value))
                     .Where(e => e.FileStateLogId.Equals(fileStateLogId)).FirstOrDefault());

            }
            catch (Exception exc)
            { }

            return fileStatesLogsVM;
        }

        #endregion

        #region Methods For Work With GoogleSheetConfigs

        public List<GoogleSheetConfigsVM> GetAllGoogleSheetConfigsList(
            //List<long> childsUsersIds,
            //string? GoogleSheetConfigTitle = "",
            //long? ConstructionProjectId = 0
            )
        {

            List<GoogleSheetConfigsVM> googleSheetConfigsVMList = new List<GoogleSheetConfigsVM>();

            try
            {
                var list = (from p in projectsApiDb.GoogleSheetConfigs
                            select new GoogleSheetConfigsVM
                            {
                                GoogleSheetConfigId = p.GoogleSheetConfigId,
                                ApiKey = p.ApiKey,
                                //ConstructionProjectId = p.ConstructionProjectId,
                                ConfigJson = p.ConfigJson,
                                GoogleSheetId = p.GoogleSheetId,
                                CredentialsJson = p.CredentialsJson,
                                GmailAddress = p.GmailAddress,
                                GmailPassword = p.GmailPassword,
                                GoogleDriveId = p.GoogleDriveId,
                                GoogleDriveTitle = p.GoogleDriveTitle,
                                GoogleSheetTitle = p.GoogleSheetTitle,


                                UserIdCreator = p.UserIdCreator.Value,
                                CreateEnDate = p.CreateEnDate,
                                CreateTime = p.CreateTime,
                                EditEnDate = p.EditEnDate,
                                EditTime = p.EditTime,
                                UserIdEditor = p.UserIdEditor.Value,
                                RemoveEnDate = p.RemoveEnDate,
                                RemoveTime = p.EditTime,
                                UserIdRemover = p.UserIdRemover.Value,
                                IsActivated = p.IsActivated,
                                IsDeleted = p.IsDeleted,

                            })
                               .AsEnumerable();

                //if (ConstructionProjectId.HasValue && ConstructionProjectId.Value != 0)
                //{
                //    list = list.Where(a => a.ConstructionProjectId == ConstructionProjectId);
                //}

                //if (!string.IsNullOrEmpty(GoogleSheetConfigTitle))
                //    list = list.Where(a => a.GoogleSheetConfigTitle.Contains(GoogleSheetConfigTitle));
                googleSheetConfigsVMList = list.OrderByDescending(s => s.GoogleSheetConfigId).ToList();
            }
            catch (Exception exc)
            { }
            return googleSheetConfigsVMList;
        }
        public List<GoogleSheetConfigsVM> GetListOfGoogleSheetConfigs(int jtStartIndex,
              int jtPageSize,
              ref int listCount,
              //List<long> childsUsersIds,
              //string? GoogleSheetConfigTitle = "",
              //long? ConstructionProjectId = 0,
              string jtSorting = null)
        {

            List<GoogleSheetConfigsVM> googleSheetConfigsVMList = new List<GoogleSheetConfigsVM>();

            try
            {
                var list = (from p in projectsApiDb.GoogleSheetConfigs
                            select new GoogleSheetConfigsVM
                            {
                                GoogleSheetConfigId = p.GoogleSheetConfigId,
                                //ConstructionProjectId = p.ConstructionProjectId,
                                ApiKey = p.ApiKey,
                                ConfigJson = p.ConfigJson,
                                GoogleSheetId = p.GoogleSheetId,
                                CredentialsJson = p.CredentialsJson,
                                GmailAddress = p.GmailAddress,
                                GmailPassword = p.GmailPassword,
                                GoogleDriveId = p.GoogleDriveId,
                                GoogleDriveTitle = p.GoogleDriveTitle,
                                GoogleSheetTitle = p.GoogleSheetTitle,

                                UserIdCreator = p.UserIdCreator.Value,
                                CreateEnDate = p.CreateEnDate,
                                CreateTime = p.CreateTime,
                                EditEnDate = p.EditEnDate,
                                EditTime = p.EditTime,
                                UserIdEditor = p.UserIdEditor.Value,
                                RemoveEnDate = p.RemoveEnDate,
                                RemoveTime = p.EditTime,
                                UserIdRemover = p.UserIdRemover.Value,
                                IsActivated = p.IsActivated,
                                IsDeleted = p.IsDeleted,

                            })
                               .AsEnumerable();

                //if (ConstructionProjectId.HasValue && ConstructionProjectId.Value != 0)
                //{
                //    list = list.Where(a => a.ConstructionProjectId == ConstructionProjectId);
                //}

                //if (!string.IsNullOrEmpty(GoogleSheetConfigTitle))
                //    list = list.Where(a => a.GoogleSheetConfigTitle.Contains(GoogleSheetConfigTitle));

                listCount = list.Count();
                try
                {

                    if (listCount > jtPageSize)
                    {

                        googleSheetConfigsVMList = list.OrderByDescending(s => s.GoogleSheetConfigId)
                                 .Skip(jtStartIndex).Take(jtPageSize).ToList();
                    }
                    else
                        googleSheetConfigsVMList = list.OrderByDescending(s => s.GoogleSheetConfigId).ToList();

                }
                catch (Exception exc)
                { }
            }
            catch (Exception exc)
            { }
            return googleSheetConfigsVMList;
        }


        public long AddToGoogleSheetConfigs(GoogleSheetConfigsVM googleSheetConfigsVM)
        {
            try
            {

                GoogleSheetConfigs googleSheetConfig = _mapper.Map<GoogleSheetConfigsVM, GoogleSheetConfigs>(googleSheetConfigsVM);


                projectsApiDb.GoogleSheetConfigs.Add(googleSheetConfig);
                projectsApiDb.SaveChanges();
                return googleSheetConfig.GoogleSheetConfigId;
            }
            catch (Exception)
            {
            }
            return 0;

        }

        public long UpdateGoogleSheetConfigs(
            //List<long> childsUsersIds,
            GoogleSheetConfigsVM googleSheetConfigsVM)
        {
            var GoogleSheetConfigId = googleSheetConfigsVM.GoogleSheetConfigId;
            if (projectsApiDb.GoogleSheetConfigs//.Where(x => childsUsersIds.Contains(x.UserIdCreator.Value))
                .Where(x => x.GoogleSheetConfigId.Equals(GoogleSheetConfigId)).Any())
            {
                try
                {
                    bool? isActivated = googleSheetConfigsVM.IsActivated.HasValue ? googleSheetConfigsVM.IsActivated.Value : (bool?)true;
                    bool? isDeleted = googleSheetConfigsVM.IsDeleted.HasValue ? googleSheetConfigsVM.IsDeleted.Value : (bool?)true;

                    GoogleSheetConfigs googleSheetConfig = projectsApiDb.GoogleSheetConfigs.Where(a => a.GoogleSheetConfigId == GoogleSheetConfigId).FirstOrDefault();
                    googleSheetConfig.ApiKey = googleSheetConfigsVM.ApiKey;
                    googleSheetConfig.ConfigJson = googleSheetConfigsVM.ConfigJson;
                    googleSheetConfig.GoogleSheetId = googleSheetConfigsVM.GoogleSheetId;
                    googleSheetConfig.GoogleSheetId = googleSheetConfigsVM.GoogleSheetId;
                    googleSheetConfig.CredentialsJson = googleSheetConfigsVM.CredentialsJson;
                    googleSheetConfig.GmailAddress = googleSheetConfigsVM.GmailAddress;
                    googleSheetConfig.GmailPassword = googleSheetConfigsVM.GmailPassword;
                    googleSheetConfig.GoogleDriveId = googleSheetConfigsVM.GoogleDriveId;
                    googleSheetConfig.GoogleDriveTitle = googleSheetConfigsVM.GoogleDriveTitle;
                    googleSheetConfig.GoogleSheetTitle = googleSheetConfigsVM.GoogleSheetTitle;

                    googleSheetConfig.EditTime = googleSheetConfigsVM.EditTime;
                    googleSheetConfig.EditEnDate = googleSheetConfigsVM.EditEnDate;
                    googleSheetConfig.UserIdEditor = googleSheetConfigsVM.UserIdEditor;
                    googleSheetConfig.IsActivated = isActivated;
                    googleSheetConfig.IsDeleted = isDeleted;

                    projectsApiDb.Entry<GoogleSheetConfigs>(googleSheetConfig).State = EntityState.Modified;

                    projectsApiDb.SaveChanges();
                    return googleSheetConfig.GoogleSheetConfigId;
                }
                catch (Exception)
                {

                }
            }
            return 0;
        }

        public bool ToggleActivationGoogleSheetConfigs(long googleSheetConfigId,
            long userId
            //List<long> childsUsersIds
            )
        {
            try
            {
                var googleSheetConfigs = (from c in projectsApiDb.GoogleSheetConfigs
                                          where c.GoogleSheetConfigId == googleSheetConfigId// &&
                                          //childsUsersIds.Contains(c.UserIdCreator.Value)
                                          select c).FirstOrDefault();

                if (googleSheetConfigs != null)
                {
                    googleSheetConfigs.IsActivated = !googleSheetConfigs.IsActivated;
                    googleSheetConfigs.EditEnDate = DateTime.Now;
                    googleSheetConfigs.EditTime = PersianDate.TimeNow;
                    googleSheetConfigs.UserIdEditor = userId;

                    projectsApiDb.Entry<GoogleSheetConfigs>(googleSheetConfigs).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool TemporaryDeleteGoogleSheetConfigs(long googleSheetConfigId,
            long userId
            //List<long> childsUsersIds
            )
        {
            try
            {
                var googleSheetConfigs = (from c in projectsApiDb.GoogleSheetConfigs
                                          where c.GoogleSheetConfigId == googleSheetConfigId //&&
                                          //childsUsersIds.Contains(c.UserIdCreator.Value)
                                          select c).FirstOrDefault();

                if (googleSheetConfigs != null)
                {
                    googleSheetConfigs.IsDeleted = !googleSheetConfigs.IsDeleted;
                    googleSheetConfigs.EditEnDate = DateTime.Now;
                    googleSheetConfigs.EditTime = PersianDate.TimeNow;
                    googleSheetConfigs.UserIdEditor = userId;

                    projectsApiDb.Entry<GoogleSheetConfigs>(googleSheetConfigs).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool CompleteDeleteGoogleSheetConfigs(long googleSheetConfigId
            //long userId
            //List<long> childsUsersIds
            )
        {
            using (var transaction = projectsApiDb.Database.BeginTransaction())
            {
                try
                {
                    var googleSheetConfig = (from c in projectsApiDb.GoogleSheetConfigs
                                             where c.GoogleSheetConfigId == googleSheetConfigId //&&
                                                                                                //childsUsersIds.Contains(c.UserIdCreator.Value)
                                             select c).FirstOrDefault();

                    if (googleSheetConfig != null)
                    {
                        var excelConfigList = (from c in projectsApiDb.ExcelSheetConfigs
                                               where c.GoogleSheetConfigId == googleSheetConfigId
                                               select c).ToList();
                        if (excelConfigList.Count > 0)
                        {

                            var excelIds = excelConfigList.Select(c => c.ExcelSheetConfigId).ToList();

                            var excelConfigHistoryList = (from c in projectsApiDb.ExcelSheetConfigHistories
                                                          where excelIds.Contains(c.ExcelSheetConfigId)
                                                          select c).ToList();

                            if (excelConfigHistoryList.Count > 0)
                            {
                                projectsApiDb.ExcelSheetConfigHistories.RemoveRange(excelConfigHistoryList);
                                projectsApiDb.SaveChanges();
                            }

                            projectsApiDb.ExcelSheetConfigs.RemoveRange(excelConfigList);
                            projectsApiDb.SaveChanges();
                        }

                        projectsApiDb.GoogleSheetConfigs.Remove(googleSheetConfig);
                        projectsApiDb.SaveChanges();

                        transaction.Commit();

                        return true;
                    }
                }
                catch (Exception exc)
                {
                    transaction.Rollback();
                }
            }
            return false;


        }

        public GoogleSheetConfigsVM GetGoogleSheetConfigsWithGoogleSheetConfigId(long googleSheetConfigId
            //long userId
            //List<long> childsUsersIds
            )
        {
            GoogleSheetConfigsVM googleSheetConfigsVM = new GoogleSheetConfigsVM();

            try
            {
                googleSheetConfigsVM = _mapper.Map<GoogleSheetConfigs,
                    GoogleSheetConfigsVM>(projectsApiDb.GoogleSheetConfigs
                     //.Where(p => childsUsersIds.Contains(p.UserIdCreator.Value))
                     .Where(e => e.GoogleSheetConfigId.Equals(googleSheetConfigId)).FirstOrDefault());

            }
            catch (Exception exc)
            { }

            return googleSheetConfigsVM;
        }
        #endregion

        #region Methods For Work With InitialPlans

        public List<InitialPlansVM> GetAllInitialPlansList(
              List<long> childsUsersIds,
            string? InitialPlanTitle = "",
            long? ConstructionProjectId = 0)
        {

            List<InitialPlansVM> initialPlansVMList = new List<InitialPlansVM>();

            try
            {
                var list = (from p in projectsApiDb.InitialPlans
                            where childsUsersIds.Contains(p.UserIdCreator.Value) &&
                           p.IsDeleted.Value.Equals(false) &&
                           p.IsActivated.Value.Equals(true)
                            select new InitialPlansVM
                            {
                                InitialPlanId = p.InitialPlanId,
                                InitialPlanTitle = p.InitialPlanTitle,
                                InitialPlanDescription = p.InitialPlanDescription,
                                InitialPlanFileExt = p.InitialPlanFileExt,
                                InitialPlanFilePath = p.InitialPlanFilePath,
                                InitialPlanNumber = p.InitialPlanNumber,
                                ConstructionProjectId = p.ConstructionProjectId,
                                InitialPlanFileOrder = p.InitialPlanFileOrder,
                                InitialPlanFileType = p.InitialPlanFileType,
                                UserIdCreator = p.UserIdCreator.Value,
                                CreateEnDate = p.CreateEnDate,
                                CreateTime = p.CreateTime,
                                EditEnDate = p.EditEnDate,
                                EditTime = p.EditTime,
                                UserIdEditor = p.UserIdEditor.Value,
                                RemoveEnDate = p.RemoveEnDate,
                                RemoveTime = p.EditTime,
                                UserIdRemover = p.UserIdRemover.Value,
                                IsActivated = p.IsActivated,
                                IsDeleted = p.IsDeleted,

                            })
                               .AsEnumerable();

                if (ConstructionProjectId.HasValue && ConstructionProjectId.Value != 0)
                {
                    list = list.Where(a => a.ConstructionProjectId == ConstructionProjectId);
                }

                if (!string.IsNullOrEmpty(InitialPlanTitle))
                    list = list.Where(a => a.InitialPlanTitle.Contains(InitialPlanTitle));
                initialPlansVMList = list.OrderByDescending(s => s.InitialPlanId).ToList();
            }
            catch (Exception exc)
            { }
            return initialPlansVMList;
        }
        public List<InitialPlansVM> GetListOfInitialPlans(int jtStartIndex,
              int jtPageSize,
              ref int listCount,
              List<long> childsUsersIds,
              string? InitialPlanTitle = "",
              long? ConstructionProjectId = 0,
              string jtSorting = null)
        {

            List<InitialPlansVM> initialPlansVMList = new List<InitialPlansVM>();

            try
            {
                var list = (from p in projectsApiDb.InitialPlans
                            where childsUsersIds.Contains(p.UserIdCreator.Value)
                            select new InitialPlansVM
                            {
                                InitialPlanId = p.InitialPlanId,
                                InitialPlanTitle = p.InitialPlanTitle,
                                InitialPlanDescription = p.InitialPlanDescription,
                                InitialPlanFileExt = p.InitialPlanFileExt,
                                InitialPlanFilePath = p.InitialPlanFilePath,
                                InitialPlanNumber = p.InitialPlanNumber,
                                ConstructionProjectId = p.ConstructionProjectId,
                                InitialPlanFileOrder = p.InitialPlanFileOrder,
                                InitialPlanFileType = p.InitialPlanFileType,
                                UserIdCreator = p.UserIdCreator.Value,
                                CreateEnDate = p.CreateEnDate,
                                CreateTime = p.CreateTime,
                                EditEnDate = p.EditEnDate,
                                EditTime = p.EditTime,
                                UserIdEditor = p.UserIdEditor.Value,
                                RemoveEnDate = p.RemoveEnDate,
                                RemoveTime = p.EditTime,
                                UserIdRemover = p.UserIdRemover.Value,
                                IsActivated = p.IsActivated,
                                IsDeleted = p.IsDeleted,

                            })
                               .AsEnumerable();

                if (ConstructionProjectId.HasValue && ConstructionProjectId.Value != 0)
                {
                    list = list.Where(a => a.ConstructionProjectId == ConstructionProjectId);
                }

                if (!string.IsNullOrEmpty(InitialPlanTitle))
                    list = list.Where(a => a.InitialPlanTitle.Contains(InitialPlanTitle));

                listCount = list.Count();
                try
                {
                    if (string.IsNullOrEmpty(jtSorting))
                    {

                        if (listCount > jtPageSize)
                        {

                            initialPlansVMList = list.OrderByDescending(s => s.InitialPlanId)
                                     .Skip(jtStartIndex).Take(jtPageSize).ToList();
                        }
                        else
                            initialPlansVMList = list.OrderByDescending(s => s.InitialPlanId).ToList();
                    }
                    else
                    {


                        switch (jtSorting)
                        {
                            case "InitialPlanTitle ASC":
                                list = list.OrderBy(l => l.InitialPlanTitle);
                                break;
                            case "InitialPlanTitle DESC":
                                list = list.OrderByDescending(l => l.InitialPlanTitle);
                                break;
                        }
                        if (listCount > jtPageSize)
                        {
                            initialPlansVMList = list.Skip(jtStartIndex).Take(jtPageSize).ToList();
                        }
                        else
                        {

                            initialPlansVMList = list.ToList();
                        }
                    }
                }
                catch (Exception exc)
                { }
            }
            catch (Exception exc)
            { }
            return initialPlansVMList;
        }


        public long AddToInitialPlans(InitialPlansVM initialPlansVM)
        {
            try
            {

                InitialPlans initialPlan = _mapper.Map<InitialPlansVM, InitialPlans>(initialPlansVM);


                projectsApiDb.InitialPlans.Add(initialPlan);
                projectsApiDb.SaveChanges();
                return initialPlan.InitialPlanId;
            }
            catch (Exception)
            {
            }
            return 0;

        }

        public InitialPlansVM UpdateInitialPlans(
              List<long> childsUsersIds,
            InitialPlansVM initialPlansVM)
        {
            var InitialPlanId = initialPlansVM.InitialPlanId;
            if (projectsApiDb.InitialPlans.Where(x => childsUsersIds.Contains(x.UserIdCreator.Value))
                .Where(x => x.InitialPlanId.Equals(InitialPlanId)).Any())
            {
                try
                {
                    bool? isActivated = initialPlansVM.IsActivated.HasValue ? initialPlansVM.IsActivated.Value : (bool?)true;
                    bool? isDeleted = initialPlansVM.IsDeleted.HasValue ? initialPlansVM.IsDeleted.Value : (bool?)true;

                    InitialPlans initialPlan = projectsApiDb.InitialPlans.Where(a => a.InitialPlanId == InitialPlanId).FirstOrDefault();
                    initialPlan.InitialPlanTitle = initialPlansVM.InitialPlanTitle;
                    //initialPlan.InitialPlanFileExt = initialPlansVM.InitialPlanFileExt;
                    //initialPlan.InitialPlanFilePath = initialPlansVM.InitialPlanFilePath;
                    initialPlan.InitialPlanFileOrder = initialPlansVM.InitialPlanFileOrder;
                    initialPlan.InitialPlanNumber = initialPlansVM.InitialPlanNumber.Value;
                    initialPlan.InitialPlanDescription = initialPlansVM.InitialPlanDescription;
                    initialPlan.EditTime = initialPlansVM.EditTime;
                    initialPlan.EditEnDate = initialPlansVM.EditEnDate;
                    initialPlan.UserIdEditor = initialPlansVM.UserIdEditor;
                    initialPlan.IsActivated = isActivated;
                    initialPlan.IsDeleted = isDeleted;

                    projectsApiDb.Entry<InitialPlans>(initialPlan).State = EntityState.Modified;

                    projectsApiDb.SaveChanges();
                    return _mapper.Map<InitialPlans, InitialPlansVM>(initialPlan);
                }
                catch (Exception)
                {

                }
            }
            return null;
        }

        public bool ToggleActivationInitialPlans(long initialPlanId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var initialPlans = (from c in projectsApiDb.InitialPlans
                                    where c.InitialPlanId == initialPlanId &&
                                    childsUsersIds.Contains(c.UserIdCreator.Value)
                                    select c).FirstOrDefault();

                if (initialPlans != null)
                {
                    initialPlans.IsActivated = !initialPlans.IsActivated;
                    initialPlans.EditEnDate = DateTime.Now;
                    initialPlans.EditTime = PersianDate.TimeNow;
                    initialPlans.UserIdEditor = userId;

                    projectsApiDb.Entry<Entities.InitialPlans>(initialPlans).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool TemporaryDeleteInitialPlans(long initialPlanId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var initialPlans = (from c in projectsApiDb.InitialPlans
                                    where c.InitialPlanId == initialPlanId &&
                                    childsUsersIds.Contains(c.UserIdCreator.Value)
                                    select c).FirstOrDefault();

                if (initialPlans != null)
                {
                    initialPlans.IsDeleted = !initialPlans.IsDeleted;
                    initialPlans.EditEnDate = DateTime.Now;
                    initialPlans.EditTime = PersianDate.TimeNow;
                    initialPlans.UserIdEditor = userId;

                    projectsApiDb.Entry<Entities.InitialPlans>(initialPlans).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool? CompleteDeleteInitialPlans(long initialPlanId,
            long userId,
            List<long> childsUsersIds)
        {

            try
            {
                var haveAttachements = (from a in projectsApiDb.AttachementFiles
                                        where a.AttachementTableTitle == "InitialPlans" &&
                                        a.AttachementParentId == initialPlanId
                                        select a).Any();
                if (haveAttachements)
                {
                    return null;
                }
                var initialPlan = (from c in projectsApiDb.InitialPlans
                                   where c.InitialPlanId == initialPlanId &&
                                   childsUsersIds.Contains(c.UserIdCreator.Value)
                                   select c).FirstOrDefault();

                if (initialPlan != null)
                {
                    projectsApiDb.InitialPlans.Remove(initialPlan);
                    projectsApiDb.SaveChanges();


                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;


        }

        public InitialPlansVM GetInitialPlansWithInitialPlanId(long initialPlanId,
            long userId,
            List<long> childsUsersIds)
        {
            InitialPlansVM initialPlansVM = new InitialPlansVM();

            try
            {
                initialPlansVM = _mapper.Map<InitialPlans,
                    InitialPlansVM>(projectsApiDb.InitialPlans
                    .Where(p => childsUsersIds.Contains(p.UserIdCreator.Value))
                     .Where(e => e.InitialPlanId.Equals(initialPlanId)).FirstOrDefault());

            }
            catch (Exception exc)
            { }

            return initialPlansVM;
        }
        #endregion

        #region Methods For Work With MeetingBoards

        public List<MeetingBoardsVM> GetAllMeetingBoardsList(
              List<long> childsUsersIds,
            string? MeetingBoardTitle = "",
            long? ConstructionProjectId = 0)
        {

            List<MeetingBoardsVM> meetingBoardsVMList = new List<MeetingBoardsVM>();

            try
            {
                var list = (from p in projectsApiDb.MeetingBoards
                            where childsUsersIds.Contains(p.UserIdCreator.Value) &&
                           p.IsDeleted.Value.Equals(false) &&
                           p.IsActivated.Value.Equals(true)
                            select new MeetingBoardsVM
                            {
                                MeetingBoardId = p.MeetingBoardId,
                                MeetingBoardTitle = p.MeetingBoardTitle,
                                MeetingBoardDescription = p.MeetingBoardDescription,
                                MeetingBoardFileExt = p.MeetingBoardFileExt,
                                MeetingBoardFilePath = p.MeetingBoardFilePath,
                                MeetingBoardNumber = p.MeetingBoardNumber,
                                ConstructionProjectId = p.ConstructionProjectId,
                                MeetingBoardFileOrder = p.MeetingBoardFileOrder,
                                MeetingBoardFileType = p.MeetingBoardFileType,
                                UserIdCreator = p.UserIdCreator.Value,
                                CreateEnDate = p.CreateEnDate,
                                CreateTime = p.CreateTime,
                                EditEnDate = p.EditEnDate,
                                EditTime = p.EditTime,
                                UserIdEditor = p.UserIdEditor.Value,
                                RemoveEnDate = p.RemoveEnDate,
                                RemoveTime = p.EditTime,
                                UserIdRemover = p.UserIdRemover.Value,
                                IsActivated = p.IsActivated,
                                IsDeleted = p.IsDeleted,

                            })
                               .AsEnumerable();

                if (ConstructionProjectId.HasValue && ConstructionProjectId.Value != 0)
                {
                    list = list.Where(a => a.ConstructionProjectId == ConstructionProjectId);
                }

                if (!string.IsNullOrEmpty(MeetingBoardTitle))
                    list = list.Where(a => a.MeetingBoardTitle.Contains(MeetingBoardTitle));
                meetingBoardsVMList = list.OrderByDescending(s => s.MeetingBoardId).ToList();
            }
            catch (Exception exc)
            { }
            return meetingBoardsVMList;
        }
        public List<MeetingBoardsVM> GetListOfMeetingBoards(int jtStartIndex,
              int jtPageSize,
              ref int listCount,
              List<long> childsUsersIds,
              string? MeetingBoardTitle = "",
              long? ConstructionProjectId = 0,
              string jtSorting = null)
        {

            List<MeetingBoardsVM> meetingBoardsVMList = new List<MeetingBoardsVM>();

            try
            {
                var list = (from p in projectsApiDb.MeetingBoards
                            where childsUsersIds.Contains(p.UserIdCreator.Value)
                            select new MeetingBoardsVM
                            {
                                MeetingBoardId = p.MeetingBoardId,
                                MeetingBoardTitle = p.MeetingBoardTitle,
                                MeetingBoardDescription = p.MeetingBoardDescription,
                                MeetingBoardFileExt = p.MeetingBoardFileExt,
                                MeetingBoardFilePath = p.MeetingBoardFilePath,
                                MeetingBoardNumber = p.MeetingBoardNumber,
                                ConstructionProjectId = p.ConstructionProjectId,
                                MeetingBoardFileOrder = p.MeetingBoardFileOrder,
                                MeetingBoardFileType = p.MeetingBoardFileType,
                                UserIdCreator = p.UserIdCreator.Value,
                                CreateEnDate = p.CreateEnDate,
                                CreateTime = p.CreateTime,
                                EditEnDate = p.EditEnDate,
                                EditTime = p.EditTime,
                                UserIdEditor = p.UserIdEditor.Value,
                                RemoveEnDate = p.RemoveEnDate,
                                RemoveTime = p.EditTime,
                                UserIdRemover = p.UserIdRemover.Value,
                                IsActivated = p.IsActivated,
                                IsDeleted = p.IsDeleted,

                            })
                               .AsEnumerable();

                if (ConstructionProjectId.HasValue && ConstructionProjectId.Value != 0)
                {
                    list = list.Where(a => a.ConstructionProjectId == ConstructionProjectId);
                }

                if (!string.IsNullOrEmpty(MeetingBoardTitle))
                    list = list.Where(a => a.MeetingBoardTitle.Contains(MeetingBoardTitle));

                listCount = list.Count();
                try
                {
                    if (string.IsNullOrEmpty(jtSorting))
                    {

                        if (listCount > jtPageSize)
                        {

                            meetingBoardsVMList = list.OrderByDescending(s => s.MeetingBoardId)
                                     .Skip(jtStartIndex).Take(jtPageSize).ToList();
                        }
                        else
                            meetingBoardsVMList = list.OrderByDescending(s => s.MeetingBoardId).ToList();
                    }
                    else
                    {


                        switch (jtSorting)
                        {
                            case "MeetingBoardTitle ASC":
                                list = list.OrderBy(l => l.MeetingBoardTitle);
                                break;
                            case "MeetingBoardTitle DESC":
                                list = list.OrderByDescending(l => l.MeetingBoardTitle);
                                break;
                        }
                        if (listCount > jtPageSize)
                        {
                            meetingBoardsVMList = list.Skip(jtStartIndex).Take(jtPageSize).ToList();
                        }
                        else
                        {

                            meetingBoardsVMList = list.ToList();
                        }
                    }
                }
                catch (Exception exc)
                { }
            }
            catch (Exception exc)
            { }
            return meetingBoardsVMList;
        }


        public long AddToMeetingBoards(MeetingBoardsVM meetingBoardsVM)
        {
            try
            {

                MeetingBoards meetingBoard = _mapper.Map<MeetingBoardsVM, MeetingBoards>(meetingBoardsVM);


                projectsApiDb.MeetingBoards.Add(meetingBoard);
                projectsApiDb.SaveChanges();
                return meetingBoard.MeetingBoardId;
            }
            catch (Exception)
            {
            }
            return 0;

        }

        public MeetingBoardsVM UpdateMeetingBoards(
              List<long> childsUsersIds,
            MeetingBoardsVM meetingBoardsVM)
        {
            var MeetingBoardId = meetingBoardsVM.MeetingBoardId;
            if (projectsApiDb.MeetingBoards.Where(x => childsUsersIds.Contains(x.UserIdCreator.Value))
                .Where(x => x.MeetingBoardId.Equals(MeetingBoardId)).Any())
            {
                try
                {
                    bool? isActivated = meetingBoardsVM.IsActivated.HasValue ? meetingBoardsVM.IsActivated.Value : (bool?)true;
                    bool? isDeleted = meetingBoardsVM.IsDeleted.HasValue ? meetingBoardsVM.IsDeleted.Value : (bool?)true;

                    MeetingBoards meetingBoard = projectsApiDb.MeetingBoards.Where(a => a.MeetingBoardId == MeetingBoardId).FirstOrDefault();
                    meetingBoard.MeetingBoardTitle = meetingBoardsVM.MeetingBoardTitle;
                    //meetingBoard.MeetingBoardFileExt = meetingBoardsVM.MeetingBoardFileExt;
                    //meetingBoard.MeetingBoardFilePath = meetingBoardsVM.MeetingBoardFilePath;
                    meetingBoard.MeetingBoardFileOrder = meetingBoardsVM.MeetingBoardFileOrder;
                    meetingBoard.MeetingBoardNumber = meetingBoardsVM.MeetingBoardNumber.Value;
                    meetingBoard.MeetingBoardDescription = meetingBoardsVM.MeetingBoardDescription;
                    meetingBoard.EditTime = meetingBoardsVM.EditTime;
                    meetingBoard.EditEnDate = meetingBoardsVM.EditEnDate;
                    meetingBoard.UserIdEditor = meetingBoardsVM.UserIdEditor;
                    meetingBoard.IsActivated = isActivated;
                    meetingBoard.IsDeleted = isDeleted;

                    projectsApiDb.Entry<MeetingBoards>(meetingBoard).State = EntityState.Modified;

                    projectsApiDb.SaveChanges();
                    return _mapper.Map<MeetingBoards, MeetingBoardsVM>(meetingBoard);
                }
                catch (Exception)
                {

                }
            }
            return null;
        }

        public bool ToggleActivationMeetingBoards(long meetingBoardId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var meetingBoards = (from c in projectsApiDb.MeetingBoards
                                     where c.MeetingBoardId == meetingBoardId &&
                                     childsUsersIds.Contains(c.UserIdCreator.Value)
                                     select c).FirstOrDefault();

                if (meetingBoards != null)
                {
                    meetingBoards.IsActivated = !meetingBoards.IsActivated;
                    meetingBoards.EditEnDate = DateTime.Now;
                    meetingBoards.EditTime = PersianDate.TimeNow;
                    meetingBoards.UserIdEditor = userId;

                    projectsApiDb.Entry<Entities.MeetingBoards>(meetingBoards).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool TemporaryDeleteMeetingBoards(long meetingBoardId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var meetingBoards = (from c in projectsApiDb.MeetingBoards
                                     where c.MeetingBoardId == meetingBoardId &&
                                     childsUsersIds.Contains(c.UserIdCreator.Value)
                                     select c).FirstOrDefault();

                if (meetingBoards != null)
                {
                    meetingBoards.IsDeleted = !meetingBoards.IsDeleted;
                    meetingBoards.EditEnDate = DateTime.Now;
                    meetingBoards.EditTime = PersianDate.TimeNow;
                    meetingBoards.UserIdEditor = userId;

                    projectsApiDb.Entry<Entities.MeetingBoards>(meetingBoards).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool? CompleteDeleteMeetingBoards(long meetingBoardId,
            long userId,
            List<long> childsUsersIds)
        {

            try
            {
                var haveAttachements = (from a in projectsApiDb.AttachementFiles
                                        where a.AttachementTableTitle == "MeetingBoards" &&
                                        a.AttachementParentId == meetingBoardId
                                        select a).Any();
                if (haveAttachements)
                {
                    return null;
                }
                var meetingBoard = (from c in projectsApiDb.MeetingBoards
                                    where c.MeetingBoardId == meetingBoardId &&
                                    childsUsersIds.Contains(c.UserIdCreator.Value)
                                    select c).FirstOrDefault();

                if (meetingBoard != null)
                {
                    projectsApiDb.MeetingBoards.Remove(meetingBoard);
                    projectsApiDb.SaveChanges();


                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;


        }

        public MeetingBoardsVM GetMeetingBoardsWithMeetingBoardId(long meetingBoardId,
            long userId,
            List<long> childsUsersIds)
        {
            MeetingBoardsVM meetingBoardsVM = new MeetingBoardsVM();

            try
            {
                meetingBoardsVM = _mapper.Map<MeetingBoards,
                    MeetingBoardsVM>(projectsApiDb.MeetingBoards
                    .Where(p => childsUsersIds.Contains(p.UserIdCreator.Value))
                     .Where(e => e.MeetingBoardId.Equals(meetingBoardId)).FirstOrDefault());

            }
            catch (Exception exc)
            { }

            return meetingBoardsVM;
        }
        #endregion

        #region Methods For Work With OperationsOnProperty

        #endregion

        #region Methods For Work With OperationOnPropertyProjects

        #endregion

        #region Methods For Work With PropertyProjects

        public List<PropertyProjectsVM> GetAllPropertyProjectsList(ref int listCount,
            List<long> childsUsersIds,
            long? propertyId = null,
            int? propertyProjectTypeId = null,
            bool? isPrivate = null,
            string jtSorting = null)
        {
            List<PropertyProjectsVM> propertyProjectsVMList = new List<PropertyProjectsVM>();

            try
            {
                var list = (from p in projectsApiDb.PropertyProjects
                            where childsUsersIds.Contains(p.UserIdCreator.Value) &&
                            p.IsActivated.Value.Equals(true) &&
                            p.IsDeleted.Value.Equals(false)
                            select new PropertyProjectsVM
                            {
                                PropertyProjectId = p.PropertyProjectId,
                                PropertyId = p.PropertyId,
                                CreateEnDate = p.CreateEnDate,
                                CreateTime = p.CreateTime,
                                EditEnDate = p.EditEnDate,
                                EditTime = p.EditTime,
                                IsActivated = p.IsActivated,
                                IsDeleted = p.IsDeleted,
                                RemoveEnDate = p.RemoveEnDate,
                                ParentPropertyProjectId = p.ParentPropertyProjectId,
                                PropertyProjectTypeId = p.PropertyProjectTypeId,
                                IsPrivate = p.IsPrivate,
                                RemoveTime = p.EditTime,
                                UserCreatorName = "",
                                UserIdCreator = p.UserIdCreator.Value,
                                UserIdEditor = p.UserIdEditor.Value,
                                UserIdRemover = p.UserIdRemover.Value,
                            }).OrderByDescending(p => p.PropertyId).AsQueryable();

                if (propertyId.HasValue)
                    if (propertyId.Value > 0)
                        list = list.Where(a => a.PropertyId.Equals(propertyId.Value));

                if (isPrivate.HasValue)
                    list = list.Where(a => a.IsPrivate.Equals(isPrivate.Value));

                if (propertyProjectTypeId.HasValue)
                    if (propertyProjectTypeId.Value > 0)
                        list = list.Where(a => a.PropertyProjectTypeId.Equals(propertyProjectTypeId.Value));

                listCount = list.Count();

                propertyProjectsVMList = list.ToList();
            }
            catch (Exception exc)
            { }

            return propertyProjectsVMList;
        }

        public List<PropertyProjectsVM> GetListOfPropertyProjects(int jtStartIndex,
              int jtPageSize,
              ref int listCount,
              List<long> childsUsersIds,
              long? propertyId = null,
              int? propertyProjectTypeId = null,
              bool? isPrivate = null,
              string jtSorting = null)
        {
            List<PropertyProjectsVM> propertyProjectsVMList = new List<PropertyProjectsVM>();

            try
            {
                var list = (from p in projectsApiDb.PropertyProjects
                            where childsUsersIds.Contains(p.UserIdCreator.Value) &&
                            p.IsDeleted.Value.Equals(false) &&
                            p.IsActivated.Value.Equals(true)
                            select new PropertyProjectsVM
                            {
                                PropertyProjectId = p.PropertyProjectId,
                                PropertyId = p.PropertyId,
                                CreateEnDate = p.CreateEnDate,
                                CreateTime = p.CreateTime,
                                EditEnDate = p.EditEnDate,
                                EditTime = p.EditTime,
                                IsActivated = p.IsActivated,
                                IsDeleted = p.IsDeleted,
                                RemoveEnDate = p.RemoveEnDate,
                                ParentPropertyProjectId = p.ParentPropertyProjectId,
                                PropertyProjectTypeId = p.PropertyProjectTypeId,
                                IsPrivate = p.IsPrivate,
                                RemoveTime = p.EditTime,
                                UserCreatorName = "",
                                UserIdCreator = p.UserIdCreator.Value,
                                UserIdEditor = p.UserIdEditor.Value,
                                UserIdRemover = p.UserIdRemover.Value,
                            }).OrderByDescending(p => p.PropertyId).AsQueryable();

                if (propertyId.HasValue)
                    if (propertyId.Value > 0)
                        list = list.Where(a => a.PropertyId.Equals(propertyId.Value));

                if (isPrivate.HasValue)
                    list = list.Where(a => a.IsPrivate.Equals(isPrivate.Value));

                if (propertyProjectTypeId.HasValue)
                    if (propertyProjectTypeId.Value > 0)
                        list = list.Where(a => a.PropertyProjectTypeId.Equals(propertyProjectTypeId.Value));

                if (string.IsNullOrEmpty(jtSorting))
                {
                    listCount = list.Count();

                    if (listCount > jtPageSize)
                    {
                        propertyProjectsVMList = list
                                 .Skip(jtStartIndex).Take(jtPageSize).ToList();
                    }
                    else
                    {
                        propertyProjectsVMList = list.ToList();
                    }
                }
                else
                {
                    listCount = list.Count();

                    if (listCount > jtPageSize)
                    {
                        switch (jtSorting)
                        {
                            case "PropertyProjectId ASC":
                                list = list.OrderBy(l => l.PropertyProjectId);
                                break;
                            case "PropertyProjectId DESC":
                                list = list.OrderByDescending(l => l.PropertyProjectId);
                                break;
                        }

                        if (string.IsNullOrEmpty(jtSorting))
                            propertyProjectsVMList = list
                                     .Skip(jtStartIndex).Take(jtPageSize).ToList();
                        else
                            propertyProjectsVMList = list.Skip(jtStartIndex).Take(jtPageSize).ToList();
                    }
                    else
                    {
                        propertyProjectsVMList = list.ToList();
                    }
                }

            }
            catch (Exception exc)
            { }

            return propertyProjectsVMList;
        }

        public int AddToPropertyProjects(PropertyProjectsVM propertyProjectsVM)
        {
            try
            {
                PropertyProjects propertyProject = _mapper.Map<PropertyProjectsVM, PropertyProjects>(propertyProjectsVM);

                projectsApiDb.PropertyProjects.Add(propertyProject);
                projectsApiDb.SaveChanges();

                return propertyProject.PropertyProjectId;
            }
            catch (Exception exc)
            { }
            return 0;
        }

        public PropertyProjectsVM GetPropertyProjectWithPropertyProjectId(int propertyProjectId,
            List<long> childsUsersIds)
        {
            PropertyProjectsVM propertyProjectsVM = new PropertyProjectsVM();

            try
            {
                if (projectsApiDb.PropertyProjects
                    .Where(e => e.PropertyProjectId.Equals(propertyProjectId) &&
                    childsUsersIds.Contains(e.UserIdCreator.Value)).Any())
                    propertyProjectsVM = _mapper.Map<PropertyProjects,
                        PropertyProjectsVM>(projectsApiDb.PropertyProjects
                        .Where(e => e.PropertyProjectId.Equals(propertyProjectId)).FirstOrDefault());
            }
            catch (Exception exc)
            { }

            return propertyProjectsVM;
        }

        public int UpdatePropertyProjects(ref PropertyProjectsVM propertyProjectsVM,
            List<long> childsUsersIds)
        {
            try
            {
                int propertyProjectId = propertyProjectsVM.PropertyProjectId;

                if (projectsApiDb.PropertyProjects.Where(p => p.PropertyProjectId.Equals(propertyProjectId) &&
                    childsUsersIds.Contains(p.UserIdCreator.Value)).Any())
                {
                    long PropertyId = propertyProjectsVM.PropertyId;
                    int parentPropertyProjectId = propertyProjectsVM.ParentPropertyProjectId.HasValue ?
                        propertyProjectsVM.ParentPropertyProjectId.Value :
                        0;
                    int propertyProjectTypeId = propertyProjectsVM.PropertyProjectTypeId;
                    bool isPrivate = propertyProjectsVM.IsPrivate;

                    PropertyProjects propertyProject = (from c in projectsApiDb.PropertyProjects
                                                        where c.PropertyProjectId == propertyProjectId
                                                        select c).FirstOrDefault();

                    propertyProject.EditEnDate = propertyProjectsVM.EditEnDate.Value;
                    propertyProject.EditTime = propertyProjectsVM.EditTime;
                    propertyProject.UserIdEditor = propertyProjectsVM.UserIdEditor;

                    propertyProject.ParentPropertyProjectId = parentPropertyProjectId;
                    propertyProject.PropertyProjectTypeId = propertyProjectTypeId;
                    propertyProject.IsPrivate = isPrivate;
                    propertyProject.IsActivated = propertyProjectsVM.IsActivated;
                    propertyProject.IsDeleted = propertyProjectsVM.IsDeleted;

                    projectsApiDb.Entry<PropertyProjects>(propertyProject).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    propertyProjectsVM.UserIdCreator = propertyProject.UserIdCreator.Value;

                    return propertyProject.PropertyProjectId;
                }
            }
            catch (Exception exc)
            { }

            return 0;
        }

        public bool ToggleActivationPropertyProjects(int propertyProjectId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var propertyProject = (from c in projectsApiDb.PropertyProjects
                                       where c.PropertyProjectId == propertyProjectId &&
                                       childsUsersIds.Contains(c.UserIdCreator.Value)
                                       select c).FirstOrDefault();

                if (propertyProject != null)
                {
                    propertyProject.IsActivated = !propertyProject.IsActivated;
                    propertyProject.EditEnDate = DateTime.Now;
                    propertyProject.EditTime = PersianDate.TimeNow;
                    propertyProject.UserIdEditor = userId;

                    projectsApiDb.Entry<PropertyProjects>(propertyProject).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool TemporaryDeletePropertyProjects(int propertyProjectId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var propertyProject = (from c in projectsApiDb.PropertyProjects
                                       where c.PropertyProjectId == propertyProjectId &&
                                       childsUsersIds.Contains(c.UserIdCreator.Value)
                                       select c).FirstOrDefault();

                if (propertyProject != null)
                {
                    propertyProject.IsDeleted = !propertyProject.IsDeleted;
                    propertyProject.EditEnDate = DateTime.Now;
                    propertyProject.EditTime = PersianDate.TimeNow;
                    propertyProject.UserIdEditor = userId;

                    projectsApiDb.Entry<PropertyProjects>(propertyProject).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool CompleteDeletePropertyProjects(int propertyProjectId,
            List<long> childsUsersIds)
        {
            try
            {
                var propertyProject = (from c in projectsApiDb.PropertyProjects
                                       where c.PropertyProjectId == propertyProjectId &&
                                       childsUsersIds.Contains(c.UserIdCreator.Value)
                                       select c).FirstOrDefault();

                if (propertyProject != null)
                {
                    projectsApiDb.PropertyProjects.Remove(propertyProject);
                    projectsApiDb.SaveChanges();
                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        #endregion

        #region Methods For Work With PropertyProjectTypes

        #endregion

        #region Methods For Work With PartnershipAgreements

        public List<PartnershipAgreementsVM> GetAllPartnershipAgreementsList(
              List<long> childsUsersIds,
            string? PartnershipAgreementTitle = "",
            long? ConstructionProjectId = 0)
        {

            List<PartnershipAgreementsVM> partnershipAgreementsVMList = new List<PartnershipAgreementsVM>();

            try
            {
                var list = (from p in projectsApiDb.PartnershipAgreements
                            where childsUsersIds.Contains(p.UserIdCreator.Value) &&
                            p.IsDeleted.Value.Equals(false) &&
                            p.IsActivated.Value.Equals(true)
                            select new PartnershipAgreementsVM
                            {
                                PartnershipAgreementId = p.PartnershipAgreementId,
                                PartnershipAgreementTitle = p.PartnershipAgreementTitle,
                                ConstructionProjectId = p.ConstructionProjectId,

                                //PartnershipAgreementDescription = p.PartnershipAgreementDescription,
                                //PartnershipAgreementFileExt = p.PartnershipAgreementFileExt,
                                //PartnershipAgreementFilePath = p.PartnershipAgreementFilePath,
                                //PartnershipAgreementNumber = p.PartnershipAgreementNumber,
                                //ParentPartnershipAgreementId = p.ParentPartnershipAgreementId,
                                //PartnershipAgreementFileOrder = p.PartnershipAgreementFileOrder,
                                //PartnershipAgreementFileType = p.PartnershipAgreementFileType,

                                UserIdCreator = p.UserIdCreator.Value,
                                CreateEnDate = p.CreateEnDate,
                                CreateTime = p.CreateTime,
                                EditEnDate = p.EditEnDate,
                                EditTime = p.EditTime,
                                UserIdEditor = p.UserIdEditor.Value,
                                RemoveEnDate = p.RemoveEnDate,
                                RemoveTime = p.EditTime,
                                UserIdRemover = p.UserIdRemover.Value,
                                IsActivated = p.IsActivated,
                                IsDeleted = p.IsDeleted,

                            })
                               .AsEnumerable();

                if (ConstructionProjectId.HasValue && ConstructionProjectId.Value != 0)
                {
                    list = list.Where(a => a.ConstructionProjectId == ConstructionProjectId);
                }

                if (!string.IsNullOrEmpty(PartnershipAgreementTitle))
                    list = list.Where(a => a.PartnershipAgreementTitle.Contains(PartnershipAgreementTitle));
                partnershipAgreementsVMList = list.OrderByDescending(s => s.PartnershipAgreementId).ToList();
            }
            catch (Exception exc)
            { }
            return partnershipAgreementsVMList;
        }



        public List<PartnershipAgreementsVM> GetListOfPartnershipAgreements(int jtStartIndex,
              int jtPageSize,
              ref int listCount,
              List<long> childsUsersIds,
              long? userId,
              string? PartnershipAgreementTitle = "",
              long? ConstructionProjectId = 0,
              string jtSorting = null)
        {

            List<PartnershipAgreementsVM> partnershipAgreementsVMList = new List<PartnershipAgreementsVM>();

            try
            {
                var list = (from p in projectsApiDb.PartnershipAgreements
                            where childsUsersIds.Contains(p.UserIdCreator.Value)
                            select new PartnershipAgreementsVM
                            {
                                PartnershipAgreementId = p.PartnershipAgreementId,
                                PartnershipAgreementTitle = p.PartnershipAgreementTitle,
                                ConstructionProjectId = p.ConstructionProjectId,

                                //PartnershipAgreementDescription = p.PartnershipAgreementDescription,
                                //PartnershipAgreementFileExt = p.PartnershipAgreementFileExt,
                                //PartnershipAgreementFilePath = p.PartnershipAgreementFilePath,
                                //PartnershipAgreementNumber = p.PartnershipAgreementNumber,
                                //ParentPartnershipAgreementId = p.ParentPartnershipAgreementId,
                                //PartnershipAgreementFileOrder = p.PartnershipAgreementFileOrder,
                                //PartnershipAgreementFileType = p.PartnershipAgreementFileType,

                                UserIdCreator = p.UserIdCreator.Value,
                                CreateEnDate = p.CreateEnDate,
                                CreateTime = p.CreateTime,
                                EditEnDate = p.EditEnDate,
                                EditTime = p.EditTime,
                                UserIdEditor = p.UserIdEditor.Value,
                                RemoveEnDate = p.RemoveEnDate,
                                RemoveTime = p.EditTime,
                                UserIdRemover = p.UserIdRemover.Value,
                                IsActivated = p.IsActivated,
                                IsDeleted = p.IsDeleted,
                                IsView = p.IsView
                            }).AsEnumerable();




                #region Comments

                var partnershipAgreementIds = list.Select(x => x.PartnershipAgreementId).ToList();

                if (list != null)
                {
                    if (list.Count() > 0)
                    {
                        List<FileStateLogStatusVM> fileState = new List<FileStateLogStatusVM>();


                        //درخواست ها فقط مکاتبه دارند
                        fileState = projectsApiDb.FileStatesLogs
                              .Where(x => partnershipAgreementIds.Contains(x.RecordId) &&
                              x.TableTitle.Equals("PartnershipAgreement") &&
                              (x.FileStateId == 3 || x.FileStateId == 4))
                              .GroupBy(x => x.RecordId)
                              .Select(group => new FileStateLogStatusVM
                              {
                                  AgreementId = group.Key,
                                  IsView = group.Any(x => x.FileStateId == 3),
                                  IsConfirm = group.Any(x => x.FileStateId == 4)
                              })
                              .ToList();


                        //var conversations = projectsApiDb.ConversationLogs.Where(f => partnershipAgreementIds.Contains(f.RecordId) && f.IsRead.Equals(false) && f.TableTitle.Equals("PartnershipAgreement") && f.UserIdCreator.Equals(userId)).ToList();

                        //foreach (var item in list)
                        //{
                        //    //var userId = list.Select(f => f.UserIdCreator).FirstOrDefault();

                        //    item.ConversationIsReadCount = conversations.Where(f => f.RecordId.Equals(item.PartnershipAgreementId)).Count();

                        //    //item.ConversationIsReadCount = projectsApiDb.ConversationLogs.Where(x => x.RecordId.Equals(item.PartnershipAgreementId) && x.IsRead == false && x.TableTitle.Equals("PartnershipAgreements") && x.UserIdCreator != userId).Count();
                        //}
                    }
                }

                #endregion



                if (ConstructionProjectId.HasValue && ConstructionProjectId.Value != 0)
                {
                    list = list.Where(a => a.ConstructionProjectId == ConstructionProjectId);
                }

                if (!string.IsNullOrEmpty(PartnershipAgreementTitle))
                    list = list.Where(a => a.PartnershipAgreementTitle.Contains(PartnershipAgreementTitle));

                listCount = list.Count();



                try
                {
                    if (string.IsNullOrEmpty(jtSorting))
                    {

                        if (listCount > jtPageSize)
                        {

                            partnershipAgreementsVMList = list.OrderByDescending(s => s.PartnershipAgreementId)
                                     .Skip(jtStartIndex).Take(jtPageSize).ToList();
                        }
                        else
                            partnershipAgreementsVMList = list.OrderByDescending(s => s.PartnershipAgreementId).ToList();
                    }
                    else
                    {


                        switch (jtSorting)
                        {
                            case "PartnershipAgreementTitle ASC":
                                list = list.OrderBy(l => l.PartnershipAgreementTitle);
                                break;
                            case "PartnershipAgreementTitle DESC":
                                list = list.OrderByDescending(l => l.PartnershipAgreementTitle);
                                break;
                        }
                        if (listCount > jtPageSize)
                        {
                            partnershipAgreementsVMList = list.Skip(jtStartIndex).Take(jtPageSize).ToList();
                        }
                        else
                        {

                            partnershipAgreementsVMList = list.ToList();
                        }
                    }
                }
                catch (Exception exc)
                { }

            }
            catch (Exception exc)
            { }
            return partnershipAgreementsVMList;
        }


        public long AddToPartnershipAgreements(PartnershipAgreementsVM partnershipAgreementsVM)
        {
            try
            {

                PartnershipAgreements partnershipAgreement = _mapper.Map<PartnershipAgreementsVM, PartnershipAgreements>(partnershipAgreementsVM);

                projectsApiDb.PartnershipAgreements.Add(partnershipAgreement);
                projectsApiDb.SaveChanges();

                return partnershipAgreement.PartnershipAgreementId;
            }
            catch (Exception)
            {
            }
            return 0;

        }



        public PartnershipAgreementsVM UpdatePartnershipAgreements(
              List<long> childsUsersIds,
            PartnershipAgreementsVM partnershipAgreementsVM)
        {
            var PartnershipAgreementId = partnershipAgreementsVM.PartnershipAgreementId;
            if (projectsApiDb.PartnershipAgreements.Where(x => childsUsersIds.Contains(x.UserIdCreator.Value))
                .Where(x => x.PartnershipAgreementId.Equals(PartnershipAgreementId)).Any())
            {
                try
                {
                    bool? isActivated = partnershipAgreementsVM.IsActivated.HasValue ? partnershipAgreementsVM.IsActivated.Value : (bool?)true;
                    bool? isDeleted = partnershipAgreementsVM.IsDeleted.HasValue ? partnershipAgreementsVM.IsDeleted.Value : (bool?)true;

                    PartnershipAgreements partnershipAgreement = projectsApiDb.PartnershipAgreements.Where(a => a.PartnershipAgreementId == PartnershipAgreementId).FirstOrDefault();
                    partnershipAgreement.PartnershipAgreementTitle = partnershipAgreementsVM.PartnershipAgreementTitle;

                    partnershipAgreement.EditTime = partnershipAgreementsVM.EditTime;
                    partnershipAgreement.EditEnDate = partnershipAgreementsVM.EditEnDate;
                    partnershipAgreement.UserIdEditor = partnershipAgreementsVM.UserIdEditor;
                    partnershipAgreement.IsActivated = isActivated;
                    partnershipAgreement.IsDeleted = isDeleted;

                    #region comments
                    //partnershipAgreement.PartnershipAgreementFileExt = partnershipAgreementsVM.PartnershipAgreementFileExt;
                    //partnershipAgreement.PartnershipAgreementFilePath = partnershipAgreementsVM.PartnershipAgreementFilePath;
                    //partnershipAgreement.PartnershipAgreementNumber = partnershipAgreementsVM.PartnershipAgreementNumber.Value;
                    //partnershipAgreement.PartnershipAgreementDescription = partnershipAgreementsVM.PartnershipAgreementDescription;
                    //partnershipAgreement.PartnershipAgreementFileOrder = partnershipAgreementsVM.PartnershipAgreementFileOrder;
                    //partnershipAgreement.PartnershipAgreementFileType = partnershipAgreementsVM.PartnershipAgreementFileType;
                    #endregion

                    projectsApiDb.Entry<PartnershipAgreements>(partnershipAgreement).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();


                    return _mapper.Map<PartnershipAgreements, PartnershipAgreementsVM>(partnershipAgreement);
                }
                catch (Exception)
                {

                }
            }
            return null;
        }

        public bool ToggleActivationPartnershipAgreements(long partnershipAgreementId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var partnershipAgreements = (from c in projectsApiDb.PartnershipAgreements
                                             where c.PartnershipAgreementId == partnershipAgreementId &&
                                             childsUsersIds.Contains(c.UserIdCreator.Value)
                                             select c).FirstOrDefault();

                if (partnershipAgreements != null)
                {
                    partnershipAgreements.IsActivated = !partnershipAgreements.IsActivated;
                    partnershipAgreements.EditEnDate = DateTime.Now;
                    partnershipAgreements.EditTime = PersianDate.TimeNow;
                    partnershipAgreements.UserIdEditor = userId;

                    projectsApiDb.Entry<Entities.PartnershipAgreements>(partnershipAgreements).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool TemporaryDeletePartnershipAgreements(long partnershipAgreementId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var partnershipAgreements = (from c in projectsApiDb.PartnershipAgreements
                                             where c.PartnershipAgreementId == partnershipAgreementId &&
                                             childsUsersIds.Contains(c.UserIdCreator.Value)
                                             select c).FirstOrDefault();

                if (partnershipAgreements != null)
                {
                    partnershipAgreements.IsDeleted = !partnershipAgreements.IsDeleted;
                    partnershipAgreements.EditEnDate = DateTime.Now;
                    partnershipAgreements.EditTime = PersianDate.TimeNow;
                    partnershipAgreements.UserIdEditor = userId;

                    projectsApiDb.Entry<Entities.PartnershipAgreements>(partnershipAgreements).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool? CompleteDeletePartnershipAgreements(long partnershipAgreementId,
            long userId,
            List<long> childsUsersIds)
        {

            try
            {

                #region Attachements
                var haveAttachements = (from a in projectsApiDb.AttachementFiles
                                        where a.AttachementTableTitle == "PartnershipAgreements" &&
                                        a.AttachementParentId == partnershipAgreementId
                                        select a).Any();

                if (haveAttachements)
                {
                    return null;
                }
                #endregion


                #region conversations

                var conversations = projectsApiDb.ConversationLogs.Where
                            (f => f.RecordId.Equals(partnershipAgreementId) && f.TableTitle.Equals("PartnershipAgreement")).ToList();



                if (conversations != null)
                {
                    projectsApiDb.ConversationLogs.RemoveRange(conversations);
                    projectsApiDb.SaveChanges();
                }


                #endregion

                #region PartnershipAgreement
                var partnershipAgreement = (from c in projectsApiDb.PartnershipAgreements
                                            where c.PartnershipAgreementId == partnershipAgreementId &&
                                            childsUsersIds.Contains(c.UserIdCreator.Value)
                                            select c).FirstOrDefault();

                if (partnershipAgreement != null)
                {
                    projectsApiDb.PartnershipAgreements.Remove(partnershipAgreement);
                    projectsApiDb.SaveChanges();


                    return true;
                }
                #endregion




            }
            catch (Exception exc)
            { }

            return false;


        }

        public PartnershipAgreementsVM GetPartnershipAgreementsWithPartnershipAgreementId(long partnershipAgreementId,
            long userId,
            List<long> childsUsersIds)
        {
            PartnershipAgreementsVM partnershipAgreementsVM = new PartnershipAgreementsVM();

            try
            {
                partnershipAgreementsVM = _mapper.Map<PartnershipAgreements,
                    PartnershipAgreementsVM>(projectsApiDb.PartnershipAgreements
                    .Where(p => childsUsersIds.Contains(p.UserIdCreator.Value))
                     .Where(e => e.PartnershipAgreementId.Equals(partnershipAgreementId)).FirstOrDefault());

            }
            catch (Exception exc)
            { }

            return partnershipAgreementsVM;
        }


        #region Comments
        //public List<PartnershipAgreementsVM> GetPartnershipAgreementsWithParentId(int parentId,
        //    long userId,
        //    List<long> childsUsersIds)
        //{
        //    List<PartnershipAgreementsVM> partnershipAgreementsVMList = new List<PartnershipAgreementsVM>();

        //    try
        //    {
        //        var list = (from p in projectsApiDb.PartnershipAgreements
        //                    select new PartnershipAgreementsVM
        //                    {
        //                        PartnershipAgreementId = p.PartnershipAgreementId,
        //                        PartnershipAgreementTitle = p.PartnershipAgreementTitle,
        //                        PartnershipAgreementDescription = p.PartnershipAgreementDescription,
        //                        PartnershipAgreementFileExt = p.PartnershipAgreementFileExt,
        //                        PartnershipAgreementFilePath = p.PartnershipAgreementFilePath,
        //                        PartnershipAgreementNumber = p.PartnershipAgreementNumber,
        //                        ConstructionProjectId = p.ConstructionProjectId,
        //                        ParentPartnershipAgreementId = p.ParentPartnershipAgreementId,
        //                        PartnershipAgreementFileOrder = p.PartnershipAgreementFileOrder,
        //                        PartnershipAgreementFileType = p.PartnershipAgreementFileType,
        //                        UserIdCreator = p.UserIdCreator.Value,
        //                        CreateEnDate = p.CreateEnDate,
        //                        CreateTime = p.CreateTime,
        //                        EditEnDate = p.EditEnDate,
        //                        EditTime = p.EditTime,
        //                        UserIdEditor = p.UserIdEditor.Value,
        //                        RemoveEnDate = p.RemoveEnDate,
        //                        RemoveTime = p.EditTime,
        //                        UserIdRemover = p.UserIdRemover.Value,
        //                        IsActivated = p.IsActivated,
        //                        IsDeleted = p.IsDeleted
        //                    })
        //                       .AsEnumerable();


        //            list = list.Where(a => a.ParentPartnershipAgreementId == parentId);


        //        partnershipAgreementsVMList = list.OrderByDescending(s => s.PartnershipAgreementId).ToList();
        //    }
        //    catch (Exception exc)
        //    { }
        //    return partnershipAgreementsVMList;
        //}
        #endregion


        #endregion

        #region Methods For Work With PitchDecks

        public List<PitchDecksVM> GetAllPitchDecksList(
              List<long> childsUsersIds,
            string? PitchDeckTitle = "",
            long? ConstructionProjectId = 0)
        {

            List<PitchDecksVM> pitchDecksVMList = new List<PitchDecksVM>();

            try
            {
                var list = (from p in projectsApiDb.PitchDecks
                            where childsUsersIds.Contains(p.UserIdCreator.Value) &&
                           p.IsDeleted.Value.Equals(false) &&
                           p.IsActivated.Value.Equals(true)
                            select new PitchDecksVM
                            {
                                PitchDeckId = p.PitchDeckId,
                                PitchDeckTitle = p.PitchDeckTitle,
                                PitchDeckDescription = p.PitchDeckDescription,
                                PitchDeckFileExt = p.PitchDeckFileExt,
                                PitchDeckFilePath = p.PitchDeckFilePath,
                                PitchDeckNumber = p.PitchDeckNumber,
                                ConstructionProjectId = p.ConstructionProjectId,
                                PitchDeckFileOrder = p.PitchDeckFileOrder,
                                PitchDeckFileType = p.PitchDeckFileType,
                                UserIdCreator = p.UserIdCreator.Value,
                                CreateEnDate = p.CreateEnDate,
                                CreateTime = p.CreateTime,
                                EditEnDate = p.EditEnDate,
                                EditTime = p.EditTime,
                                UserIdEditor = p.UserIdEditor.Value,
                                RemoveEnDate = p.RemoveEnDate,
                                RemoveTime = p.EditTime,
                                UserIdRemover = p.UserIdRemover.Value,
                                IsActivated = p.IsActivated,
                                IsDeleted = p.IsDeleted,

                            })
                               .AsEnumerable();

                if (ConstructionProjectId.HasValue && ConstructionProjectId.Value != 0)
                {
                    list = list.Where(a => a.ConstructionProjectId == ConstructionProjectId);
                }

                if (!string.IsNullOrEmpty(PitchDeckTitle))
                    list = list.Where(a => a.PitchDeckTitle.Contains(PitchDeckTitle));
                pitchDecksVMList = list.OrderByDescending(s => s.PitchDeckId).ToList();
            }
            catch (Exception exc)
            { }
            return pitchDecksVMList;
        }
        public List<PitchDecksVM> GetListOfPitchDecks(int jtStartIndex,
              int jtPageSize,
              ref int listCount,
              List<long> childsUsersIds,
              string? PitchDeckTitle = "",
              long? ConstructionProjectId = 0,
              string jtSorting = null)
        {

            List<PitchDecksVM> pitchDecksVMList = new List<PitchDecksVM>();

            try
            {
                var list = (from p in projectsApiDb.PitchDecks
                            where childsUsersIds.Contains(p.UserIdCreator.Value)
                            select new PitchDecksVM
                            {
                                PitchDeckId = p.PitchDeckId,
                                PitchDeckTitle = p.PitchDeckTitle,
                                PitchDeckDescription = p.PitchDeckDescription,
                                PitchDeckFileExt = p.PitchDeckFileExt,
                                PitchDeckFilePath = p.PitchDeckFilePath,
                                PitchDeckNumber = p.PitchDeckNumber,
                                ConstructionProjectId = p.ConstructionProjectId,
                                PitchDeckFileOrder = p.PitchDeckFileOrder,
                                PitchDeckFileType = p.PitchDeckFileType,
                                UserIdCreator = p.UserIdCreator.Value,
                                CreateEnDate = p.CreateEnDate,
                                CreateTime = p.CreateTime,
                                EditEnDate = p.EditEnDate,
                                EditTime = p.EditTime,
                                UserIdEditor = p.UserIdEditor.Value,
                                RemoveEnDate = p.RemoveEnDate,
                                RemoveTime = p.EditTime,
                                UserIdRemover = p.UserIdRemover.Value,
                                IsActivated = p.IsActivated,
                                IsDeleted = p.IsDeleted,

                            })
                               .AsEnumerable();

                if (ConstructionProjectId.HasValue && ConstructionProjectId.Value != 0)
                {
                    list = list.Where(a => a.ConstructionProjectId == ConstructionProjectId);
                }

                if (!string.IsNullOrEmpty(PitchDeckTitle))
                    list = list.Where(a => a.PitchDeckTitle.Contains(PitchDeckTitle));

                listCount = list.Count();
                try
                {
                    if (string.IsNullOrEmpty(jtSorting))
                    {

                        if (listCount > jtPageSize)
                        {

                            pitchDecksVMList = list.OrderByDescending(s => s.PitchDeckId)
                                     .Skip(jtStartIndex).Take(jtPageSize).ToList();
                        }
                        else
                            pitchDecksVMList = list.OrderByDescending(s => s.PitchDeckId).ToList();
                    }
                    else
                    {


                        switch (jtSorting)
                        {
                            case "PitchDeckTitle ASC":
                                list = list.OrderBy(l => l.PitchDeckTitle);
                                break;
                            case "PitchDeckTitle DESC":
                                list = list.OrderByDescending(l => l.PitchDeckTitle);
                                break;
                        }
                        if (listCount > jtPageSize)
                        {
                            pitchDecksVMList = list.Skip(jtStartIndex).Take(jtPageSize).ToList();
                        }
                        else
                        {

                            pitchDecksVMList = list.ToList();
                        }
                    }
                }
                catch (Exception exc)
                { }
            }
            catch (Exception exc)
            { }
            return pitchDecksVMList;
        }


        public long AddToPitchDecks(PitchDecksVM pitchDecksVM)
        {
            try
            {

                PitchDecks pitchDeck = _mapper.Map<PitchDecksVM, PitchDecks>(pitchDecksVM);


                projectsApiDb.PitchDecks.Add(pitchDeck);
                projectsApiDb.SaveChanges();
                return pitchDeck.PitchDeckId;
            }
            catch (Exception)
            {
            }
            return 0;

        }

        public PitchDecksVM UpdatePitchDecks(
              List<long> childsUsersIds,
            PitchDecksVM pitchDecksVM)
        {
            var PitchDeckId = pitchDecksVM.PitchDeckId;
            if (projectsApiDb.PitchDecks.Where(x => childsUsersIds.Contains(x.UserIdCreator.Value))
                .Where(x => x.PitchDeckId.Equals(PitchDeckId)).Any())
            {
                try
                {
                    bool? isActivated = pitchDecksVM.IsActivated.HasValue ? pitchDecksVM.IsActivated.Value : (bool?)true;
                    bool? isDeleted = pitchDecksVM.IsDeleted.HasValue ? pitchDecksVM.IsDeleted.Value : (bool?)true;

                    PitchDecks pitchDeck = projectsApiDb.PitchDecks.Where(a => a.PitchDeckId == PitchDeckId).FirstOrDefault();
                    pitchDeck.PitchDeckTitle = pitchDecksVM.PitchDeckTitle;
                    //pitchDeck.PitchDeckFileExt = pitchDecksVM.PitchDeckFileExt;
                    //pitchDeck.PitchDeckFilePath = pitchDecksVM.PitchDeckFilePath;
                    pitchDeck.PitchDeckFileOrder = pitchDecksVM.PitchDeckFileOrder;
                    pitchDeck.PitchDeckNumber = pitchDecksVM.PitchDeckNumber.Value;
                    pitchDeck.PitchDeckDescription = pitchDecksVM.PitchDeckDescription;
                    pitchDeck.EditTime = pitchDecksVM.EditTime;
                    pitchDeck.EditEnDate = pitchDecksVM.EditEnDate;
                    pitchDeck.UserIdEditor = pitchDecksVM.UserIdEditor;
                    pitchDeck.IsActivated = isActivated;
                    pitchDeck.IsDeleted = isDeleted;

                    projectsApiDb.Entry<PitchDecks>(pitchDeck).State = EntityState.Modified;

                    projectsApiDb.SaveChanges();
                    return _mapper.Map<PitchDecks, PitchDecksVM>(pitchDeck);
                }
                catch (Exception)
                {

                }
            }
            return null;
        }

        public bool ToggleActivationPitchDecks(long pitchDeckId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var pitchDecks = (from c in projectsApiDb.PitchDecks
                                  where c.PitchDeckId == pitchDeckId &&
                                  childsUsersIds.Contains(c.UserIdCreator.Value)
                                  select c).FirstOrDefault();

                if (pitchDecks != null)
                {
                    pitchDecks.IsActivated = !pitchDecks.IsActivated;
                    pitchDecks.EditEnDate = DateTime.Now;
                    pitchDecks.EditTime = PersianDate.TimeNow;
                    pitchDecks.UserIdEditor = userId;

                    projectsApiDb.Entry<Entities.PitchDecks>(pitchDecks).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool TemporaryDeletePitchDecks(long pitchDeckId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var pitchDecks = (from c in projectsApiDb.PitchDecks
                                  where c.PitchDeckId == pitchDeckId &&
                                  childsUsersIds.Contains(c.UserIdCreator.Value)
                                  select c).FirstOrDefault();

                if (pitchDecks != null)
                {
                    pitchDecks.IsDeleted = !pitchDecks.IsDeleted;
                    pitchDecks.EditEnDate = DateTime.Now;
                    pitchDecks.EditTime = PersianDate.TimeNow;
                    pitchDecks.UserIdEditor = userId;

                    projectsApiDb.Entry<Entities.PitchDecks>(pitchDecks).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool? CompleteDeletePitchDecks(long pitchDeckId,
            long userId,
            List<long> childsUsersIds)
        {

            try
            {
                var haveAttachements = (from a in projectsApiDb.AttachementFiles
                                        where a.AttachementTableTitle == "PitchDecks" &&
                                        a.AttachementParentId == pitchDeckId
                                        select a).Any();
                if (haveAttachements)
                {
                    return null;
                }
                var pitchDeck = (from c in projectsApiDb.PitchDecks
                                 where c.PitchDeckId == pitchDeckId &&
                                 childsUsersIds.Contains(c.UserIdCreator.Value)
                                 select c).FirstOrDefault();

                if (pitchDeck != null)
                {
                    projectsApiDb.PitchDecks.Remove(pitchDeck);
                    projectsApiDb.SaveChanges();


                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;


        }

        public PitchDecksVM GetPitchDecksWithPitchDeckId(long pitchDeckId,
            long userId,
            List<long> childsUsersIds)
        {
            PitchDecksVM pitchDecksVM = new PitchDecksVM();

            try
            {
                pitchDecksVM = _mapper.Map<PitchDecks,
                    PitchDecksVM>(projectsApiDb.PitchDecks
                    .Where(p => childsUsersIds.Contains(p.UserIdCreator.Value))
                     .Where(e => e.PitchDeckId.Equals(pitchDeckId)).FirstOrDefault());

            }
            catch (Exception exc)
            { }

            return pitchDecksVM;
        }
        #endregion

        #region Methods For Work With ProgressPictures

        public List<ProgressPicturesVM> GetAllProgressPicturesList(
              List<long> childsUsersIds,
            string? ProgressPictureTitle = "",
            long? ConstructionProjectId = 0)
        {

            List<ProgressPicturesVM> progressPicturesVMList = new List<ProgressPicturesVM>();

            try
            {
                var list = (from p in projectsApiDb.ProgressPictures
                            where //childsUsersIds.Contains(p.UserIdCreator.Value) &&
                           p.IsDeleted.Value.Equals(false) &&
                           p.IsActivated.Value.Equals(true)
                            select new ProgressPicturesVM
                            {
                                ProgressPictureId = p.ProgressPictureId,
                                ProgressPictureTitle = p.ProgressPictureTitle,
                                ProgressPictureDescription = p.ProgressPictureDescription,
                                ProgressPictureFileExt = p.ProgressPictureFileExt,
                                ProgressPictureFilePath = p.ProgressPictureFilePath,
                                ProgressPictureNumber = p.ProgressPictureNumber,
                                ConstructionProjectId = p.ConstructionProjectId,
                                ProgressPictureFileOrder = p.ProgressPictureFileOrder,
                                ProgressPictureFileType = p.ProgressPictureFileType,
                                UserIdCreator = p.UserIdCreator.Value,
                                CreateEnDate = p.CreateEnDate,
                                CreateTime = p.CreateTime,
                                EditEnDate = p.EditEnDate,
                                EditTime = p.EditTime,
                                UserIdEditor = p.UserIdEditor.Value,
                                RemoveEnDate = p.RemoveEnDate,
                                RemoveTime = p.EditTime,
                                UserIdRemover = p.UserIdRemover.Value,
                                IsActivated = p.IsActivated,
                                IsDeleted = p.IsDeleted,

                            })
                               .AsEnumerable();

                if (childsUsersIds != null)
                {
                    if (childsUsersIds.Count > 1)
                    {
                        list = list.Where(c => childsUsersIds.Contains(c.UserIdCreator.Value));
                    }
                    else
                    {
                        if (childsUsersIds.Count == 1)
                        {
                            if (childsUsersIds.FirstOrDefault() > 0)
                            {
                                list = list.Where(c => childsUsersIds.Contains(c.UserIdCreator.Value));
                            }
                        }
                    }
                }

                if (ConstructionProjectId.HasValue && ConstructionProjectId.Value != 0)
                {
                    list = list.Where(a => a.ConstructionProjectId == ConstructionProjectId);
                }

                if (!string.IsNullOrEmpty(ProgressPictureTitle))
                    list = list.Where(a => a.ProgressPictureTitle.Contains(ProgressPictureTitle));
                progressPicturesVMList = list.OrderByDescending(s => s.ProgressPictureId).ToList();
            }
            catch (Exception exc)
            { }
            return progressPicturesVMList;
        }
        public List<ProgressPicturesVM> GetListOfProgressPictures(int jtStartIndex,
              int jtPageSize,
              ref int listCount,
              List<long> childsUsersIds,
              string? ProgressPictureTitle = "",
              long? ConstructionProjectId = 0,
              string jtSorting = null)
        {

            List<ProgressPicturesVM> progressPicturesVMList = new List<ProgressPicturesVM>();

            try
            {
                var list = (from p in projectsApiDb.ProgressPictures
                            where childsUsersIds.Contains(p.UserIdCreator.Value)
                            select new ProgressPicturesVM
                            {
                                ProgressPictureId = p.ProgressPictureId,
                                ProgressPictureTitle = p.ProgressPictureTitle,
                                ProgressPictureDescription = p.ProgressPictureDescription,
                                ProgressPictureFileExt = p.ProgressPictureFileExt,
                                ProgressPictureFilePath = p.ProgressPictureFilePath,
                                ProgressPictureNumber = p.ProgressPictureNumber,
                                ConstructionProjectId = p.ConstructionProjectId,
                                ProgressPictureFileOrder = p.ProgressPictureFileOrder,
                                ProgressPictureFileType = p.ProgressPictureFileType,
                                UserIdCreator = p.UserIdCreator.Value,
                                CreateEnDate = p.CreateEnDate,
                                CreateTime = p.CreateTime,
                                EditEnDate = p.EditEnDate,
                                EditTime = p.EditTime,
                                UserIdEditor = p.UserIdEditor.Value,
                                RemoveEnDate = p.RemoveEnDate,
                                RemoveTime = p.EditTime,
                                UserIdRemover = p.UserIdRemover.Value,
                                IsActivated = p.IsActivated,
                                IsDeleted = p.IsDeleted,

                            })
                               .AsEnumerable();

                if (ConstructionProjectId.HasValue && ConstructionProjectId.Value != 0)
                {
                    list = list.Where(a => a.ConstructionProjectId == ConstructionProjectId);
                }

                if (!string.IsNullOrEmpty(ProgressPictureTitle))
                    list = list.Where(a => a.ProgressPictureTitle.Contains(ProgressPictureTitle));

                listCount = list.Count();
                try
                {
                    if (string.IsNullOrEmpty(jtSorting))
                    {

                        if (listCount > jtPageSize)
                        {

                            progressPicturesVMList = list.OrderByDescending(s => s.ProgressPictureId)
                                     .Skip(jtStartIndex).Take(jtPageSize).ToList();
                        }
                        else
                            progressPicturesVMList = list.OrderByDescending(s => s.ProgressPictureId).ToList();
                    }
                    else
                    {


                        switch (jtSorting)
                        {
                            case "ProgressPictureTitle ASC":
                                list = list.OrderBy(l => l.ProgressPictureTitle);
                                break;
                            case "ProgressPictureTitle DESC":
                                list = list.OrderByDescending(l => l.ProgressPictureTitle);
                                break;
                        }
                        if (listCount > jtPageSize)
                        {
                            progressPicturesVMList = list.Skip(jtStartIndex).Take(jtPageSize).ToList();
                        }
                        else
                        {

                            progressPicturesVMList = list.ToList();
                        }
                    }
                }
                catch (Exception exc)
                { }
            }
            catch (Exception exc)
            { }
            return progressPicturesVMList;
        }


        public long AddToProgressPictures(ProgressPicturesVM progressPicturesVM)
        {
            try
            {

                ProgressPictures progressPicture = _mapper.Map<ProgressPicturesVM, ProgressPictures>(progressPicturesVM);


                projectsApiDb.ProgressPictures.Add(progressPicture);
                projectsApiDb.SaveChanges();
                return progressPicture.ProgressPictureId;
            }
            catch (Exception)
            {
            }
            return 0;

        }

        public ProgressPicturesVM UpdateProgressPictures(
              List<long> childsUsersIds,
            ProgressPicturesVM progressPicturesVM)
        {
            var ProgressPictureId = progressPicturesVM.ProgressPictureId;
            if (projectsApiDb.ProgressPictures.Where(x => childsUsersIds.Contains(x.UserIdCreator.Value))
                .Where(x => x.ProgressPictureId.Equals(ProgressPictureId)).Any())
            {
                try
                {
                    bool? isActivated = progressPicturesVM.IsActivated.HasValue ? progressPicturesVM.IsActivated.Value : (bool?)true;
                    bool? isDeleted = progressPicturesVM.IsDeleted.HasValue ? progressPicturesVM.IsDeleted.Value : (bool?)true;

                    ProgressPictures progressPicture = projectsApiDb.ProgressPictures.Where(a => a.ProgressPictureId == ProgressPictureId).FirstOrDefault();
                    progressPicture.ProgressPictureTitle = progressPicturesVM.ProgressPictureTitle;
                    //progressPicture.ProgressPictureFileExt = progressPicturesVM.ProgressPictureFileExt;
                    //progressPicture.ProgressPictureFilePath = progressPicturesVM.ProgressPictureFilePath;
                    progressPicture.ProgressPictureFileOrder = progressPicturesVM.ProgressPictureFileOrder;
                    progressPicture.ProgressPictureNumber = progressPicturesVM.ProgressPictureNumber.Value;
                    progressPicture.ProgressPictureDescription = progressPicturesVM.ProgressPictureDescription;
                    progressPicture.EditTime = progressPicturesVM.EditTime;
                    progressPicture.EditEnDate = progressPicturesVM.EditEnDate;
                    progressPicture.UserIdEditor = progressPicturesVM.UserIdEditor;
                    progressPicture.IsActivated = isActivated;
                    progressPicture.IsDeleted = isDeleted;

                    projectsApiDb.Entry<ProgressPictures>(progressPicture).State = EntityState.Modified;

                    projectsApiDb.SaveChanges();
                    return _mapper.Map<ProgressPictures, ProgressPicturesVM>(progressPicture);
                }
                catch (Exception)
                {

                }
            }
            return null;
        }

        public bool ToggleActivationProgressPictures(long progressPictureId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var progressPictures = (from c in projectsApiDb.ProgressPictures
                                        where c.ProgressPictureId == progressPictureId &&
                                        childsUsersIds.Contains(c.UserIdCreator.Value)
                                        select c).FirstOrDefault();

                if (progressPictures != null)
                {
                    progressPictures.IsActivated = !progressPictures.IsActivated;
                    progressPictures.EditEnDate = DateTime.Now;
                    progressPictures.EditTime = PersianDate.TimeNow;
                    progressPictures.UserIdEditor = userId;

                    projectsApiDb.Entry<Entities.ProgressPictures>(progressPictures).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool TemporaryDeleteProgressPictures(long progressPictureId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var progressPictures = (from c in projectsApiDb.ProgressPictures
                                        where c.ProgressPictureId == progressPictureId &&
                                        childsUsersIds.Contains(c.UserIdCreator.Value)
                                        select c).FirstOrDefault();

                if (progressPictures != null)
                {
                    progressPictures.IsDeleted = !progressPictures.IsDeleted;
                    progressPictures.EditEnDate = DateTime.Now;
                    progressPictures.EditTime = PersianDate.TimeNow;
                    progressPictures.UserIdEditor = userId;

                    projectsApiDb.Entry<Entities.ProgressPictures>(progressPictures).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool? CompleteDeleteProgressPictures(long progressPictureId,
            long userId,
            List<long> childsUsersIds)
        {

            try
            {
                var progressPicture = (from c in projectsApiDb.ProgressPictures
                                       where c.ProgressPictureId == progressPictureId &&
                                       childsUsersIds.Contains(c.UserIdCreator.Value)
                                       select c).FirstOrDefault();

                if (progressPicture != null)
                {
                    projectsApiDb.ProgressPictures.Remove(progressPicture);
                    projectsApiDb.SaveChanges();


                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;


        }

        public ProgressPicturesVM GetProgressPicturesWithProgressPictureId(long progressPictureId,
            long userId,
            List<long> childsUsersIds)
        {
            ProgressPicturesVM progressPicturesVM = new ProgressPicturesVM();

            try
            {
                progressPicturesVM = _mapper.Map<ProgressPictures,
                    ProgressPicturesVM>(projectsApiDb.ProgressPictures
                    .Where(p => childsUsersIds.Contains(p.UserIdCreator.Value))
                     .Where(e => e.ProgressPictureId.Equals(progressPictureId)).FirstOrDefault());

            }
            catch (Exception exc)
            { }

            return progressPicturesVM;
        }
        #endregion

        #region Methods For Work With TimeTables
        public List<TimeTablesVM> GetAllTimeTablesList(
                List<long> childsUsersIds,
                long? constructionProjectId)
        {


            List<TimeTablesVM> timeTablesVMList = new List<TimeTablesVM>();

            try
            {
                var list = (from c in projectsApiDb.TimeTables
                                //where childsUsersIds.Contains(c.UserIdCreator.Value)
                            select new TimeTablesVM
                            {
                                TimeTableId = c.TimeTableId,
                                TimeTableDescription = c.TimeTableDescription,
                                ConstructionProjectId = c.ConstructionProjectId,
                                TimeTableFileExt = c.TimeTableFileExt,
                                TimeTableFileOrder = c.TimeTableFileOrder,
                                TimeTableFilePath = c.TimeTableFilePath,
                                TimeTableFileType = c.TimeTableFileType,
                                TimeTableTitle = c.TimeTableTitle,
                                UserIdCreator = c.UserIdCreator.Value,
                                CreateEnDate = c.CreateEnDate,
                                CreateTime = c.CreateTime,
                                EditEnDate = c.EditEnDate,
                                EditTime = c.EditTime,
                                UserIdEditor = c.UserIdEditor.Value,
                                RemoveEnDate = c.RemoveEnDate,
                                RemoveTime = c.EditTime,
                                UserIdRemover = c.UserIdRemover.Value,
                                IsActivated = c.IsActivated,
                                IsDeleted = c.IsDeleted
                            }
                        ).Where(c => c.ConstructionProjectId.Equals(constructionProjectId))
                        .AsEnumerable();


                //if (!string.IsNullOrEmpty(timeTableTitle))
                //    list = list.Where(a => a.TimeTableTitle.Contains(timeTableTitle));

                timeTablesVMList = list.OrderByDescending(s => s.TimeTableId).ToList();

            }
            catch (Exception exc)
            { }
            return timeTablesVMList;
        }
        public List<TimeTablesVM> GetListOfTimeTables(int jtStartIndex,
              int jtPageSize,
              ref int listCount,
              List<long> childsUsersIds,
              long? constructionProjectId,
              string jtSorting = null)
        {

            List<TimeTablesVM> timeTablesVMList = new List<TimeTablesVM>();

            try
            {
                var list = (from c in projectsApiDb.TimeTables
                                //where childsUsersIds.Contains(c.UserIdCreator.Value)
                            select new TimeTablesVM
                            {
                                TimeTableId = c.TimeTableId,
                                TimeTableDescription = c.TimeTableDescription,
                                ConstructionProjectId = c.ConstructionProjectId,
                                TimeTableFileExt = c.TimeTableFileExt,
                                TimeTableFileOrder = c.TimeTableFileOrder,
                                TimeTableFilePath = c.TimeTableFilePath,
                                TimeTableFileType = c.TimeTableFileType,
                                TimeTableTitle = c.TimeTableTitle,
                                UserIdCreator = c.UserIdCreator.Value,
                                CreateEnDate = c.CreateEnDate,
                                CreateTime = c.CreateTime,
                                EditEnDate = c.EditEnDate,
                                EditTime = c.EditTime,
                                UserIdEditor = c.UserIdEditor.Value,
                                RemoveEnDate = c.RemoveEnDate,
                                RemoveTime = c.EditTime,
                                UserIdRemover = c.UserIdRemover.Value,
                                IsActivated = c.IsActivated,
                                IsDeleted = c.IsDeleted
                            }
                        ).Where(c => c.ConstructionProjectId.Equals(constructionProjectId))
                        .AsEnumerable();


                //if (!string.IsNullOrEmpty(timeTableTitle))
                //    list = list.Where(a => a.TimeTableTitle.Contains(timeTableTitle));

                listCount = list.Count();

                try
                {
                    if (string.IsNullOrEmpty(jtSorting))
                    {

                        if (listCount > jtPageSize)
                        {

                            timeTablesVMList = list.OrderByDescending(s => s.TimeTableId)
                                     .Skip(jtStartIndex).Take(jtPageSize).ToList();
                        }
                        else
                            timeTablesVMList = list.OrderByDescending(s => s.TimeTableId).ToList();
                    }
                    else
                    {


                        switch (jtSorting)
                        {
                            case "TimeTableTitle ASC":
                                list = list.OrderBy(l => l.TimeTableTitle);
                                break;
                            case "TimeTableTitle DESC":
                                list = list.OrderByDescending(l => l.TimeTableTitle);
                                break;
                        }
                        if (listCount > jtPageSize)
                        {
                            timeTablesVMList = list.Skip(jtStartIndex).Take(jtPageSize).ToList();
                        }
                        else
                        {

                            timeTablesVMList = list.ToList();
                        }
                    }
                }
                catch (Exception exc)
                { }
            }
            catch (Exception exc)
            { }
            return timeTablesVMList;
        }


        public bool AddToTimeTables(
            TimeTablesVM timeTablesVM)
        {
            try
            {
                TimeTables timeTable = _mapper.Map<TimeTablesVM, TimeTables>(timeTablesVM);


                projectsApiDb.TimeTables.Add(timeTable);
                projectsApiDb.SaveChanges();

                return true;
            }
            catch (Exception)
            {

            }

            return false;

        }
        public bool UpdateTimeTables(
            List<long> childsUsersIds,
            TimeTablesVM timeTablesVM)
        {
            var TimeTableId = timeTablesVM.TimeTableId;

            if (projectsApiDb.TimeTables.Where(n => childsUsersIds.Contains(n.UserIdCreator.Value)).Where(x => x.TimeTableId.Equals(TimeTableId)).Any())
            {

                try
                {
                    bool? isActivated = timeTablesVM.IsActivated.HasValue ? timeTablesVM.IsActivated.Value : (bool?)true;
                    bool? isDeleted = timeTablesVM.IsDeleted.HasValue ? timeTablesVM.IsDeleted.Value : (bool?)true;


                    TimeTables timeTable = projectsApiDb.TimeTables.Where(a => a.TimeTableId == TimeTableId).FirstOrDefault();
                    timeTable.TimeTableDescription = timeTablesVM.TimeTableDescription;
                    timeTable.ConstructionProjectId = timeTablesVM.ConstructionProjectId;
                    timeTable.TimeTableFileExt = timeTablesVM.TimeTableFileExt;
                    timeTable.TimeTableFileOrder = timeTablesVM.TimeTableFileOrder;
                    timeTable.TimeTableFilePath = timeTablesVM.TimeTableFilePath;
                    timeTable.TimeTableFileType = timeTablesVM.TimeTableFileType;
                    timeTable.TimeTableTitle = timeTablesVM.TimeTableTitle;
                    timeTable.EditTime = timeTablesVM.EditTime;
                    timeTable.EditEnDate = timeTablesVM.EditEnDate;
                    timeTable.UserIdEditor = timeTablesVM.UserIdEditor;
                    timeTable.IsActivated = isActivated;
                    timeTable.IsDeleted = isDeleted;

                    projectsApiDb.Entry<TimeTables>(timeTable).State = EntityState.Modified;

                    projectsApiDb.SaveChanges();

                    return true;
                }
                catch (Exception)
                {

                }
            }
            return false;
        }

        public bool ToggleActivationTimeTables(long TimeTableId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var timeTables = (from c in projectsApiDb.TimeTables
                                  where c.TimeTableId == TimeTableId &&
                                  childsUsersIds.Contains(c.UserIdCreator.Value)
                                  select c).FirstOrDefault();

                if (timeTables != null)
                {
                    timeTables.IsActivated = !timeTables.IsActivated;
                    timeTables.EditEnDate = DateTime.Now;
                    timeTables.EditTime = PersianDate.TimeNow;
                    timeTables.UserIdEditor = userId;

                    projectsApiDb.Entry<Entities.TimeTables>(timeTables).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool TemporaryDeleteTimeTables(long TimeTableId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var timeTables = (from c in projectsApiDb.TimeTables
                                  where c.TimeTableId == TimeTableId &&
                                  childsUsersIds.Contains(c.UserIdCreator.Value)
                                  select c).FirstOrDefault();

                if (timeTables != null)
                {
                    timeTables.IsDeleted = !timeTables.IsDeleted;
                    timeTables.EditEnDate = DateTime.Now;
                    timeTables.EditTime = PersianDate.TimeNow;
                    timeTables.UserIdEditor = userId;

                    projectsApiDb.Entry<Entities.TimeTables>(timeTables).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }
        public bool CompleteDeleteTimeTables(long TimeTableId,
            long userId,
            List<long> childsUsersIds,
            ref string returnMessage)
        {
            returnMessage = "";

            try
            {
                if (projectsApiDb.TimeTableItems.Where(i => i.TimeTableId.Equals(TimeTableId)).Any())
                    returnMessage = "RemoveChildsFirst";

                var timeTable = (from c in projectsApiDb.TimeTables
                                 where c.TimeTableId == TimeTableId &&
                                 childsUsersIds.Contains(c.UserIdCreator.Value)
                                 select c).FirstOrDefault();

                if (timeTable != null)
                {
                    projectsApiDb.TimeTables.Remove(timeTable);
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;


        }
        #endregion

        #region Methods For Work With TimeTableItems
        public List<TimeTableItemsVM> GetAllTimeTableItemsList(List<long> childsUsersIds,
                long? TimeTableId,
                long? timeTableItemParentId,
                string? timeTableItemTitle = "")
        {


            List<TimeTableItemsVM> timeTableItemsVMList = new List<TimeTableItemsVM>();

            try
            {
                var list = (from c in projectsApiDb.TimeTableItems
                                //where childsUsersIds.Contains(c.UserIdCreator.Value)
                            select new TimeTableItemsVM
                            {
                                TimeTableItemId = c.TimeTableItemId,
                                TimeTableItemCurrentPath = c.TimeTableItemCurrentPath,
                                TimeTableItemEndDate = c.TimeTableItemEndDate,
                                TimeTableItemEndEnDate = c.TimeTableItemEndEnDate,
                                TimeTableItemStartEnDate = c.TimeTableItemStartEnDate,
                                TimeTableItemParentId = c.TimeTableItemId,
                                TimeTableItemParentPath = c.TimeTableItemParentPath,
                                TimeTableItemStartDate = c.TimeTableItemStartDate,
                                TimeTableItemTitle = c.TimeTableItemTitle,
                                TimeTableItemValue = c.TimeTableItemValue,
                                UserIdCreator = c.UserIdCreator.Value,
                                CreateEnDate = c.CreateEnDate,
                                CreateTime = c.CreateTime,
                                EditEnDate = c.EditEnDate,
                                EditTime = c.EditTime,
                                UserIdEditor = c.UserIdEditor.Value,
                                RemoveEnDate = c.RemoveEnDate,
                                RemoveTime = c.EditTime,
                                UserIdRemover = c.UserIdRemover.Value,
                                IsActivated = c.IsActivated,
                                IsDeleted = c.IsDeleted
                            }
                        //.Where(c => childsUsersIds.Contains(c.UserIdCreator.Value))
                        ).AsEnumerable();

                if (TimeTableId.HasValue)
                    if (TimeTableId.Value > 0)
                        list = list.Where(a => a.TimeTableItemId.Equals(TimeTableId.Value));

                if (timeTableItemParentId.HasValue)
                    if (timeTableItemParentId.Value > 0)
                        list = list.Where(a => a.TimeTableItemParentId.HasValue).Where(a => a.TimeTableItemParentId.Value.Equals(timeTableItemParentId.Value));


                if (!string.IsNullOrEmpty(timeTableItemTitle))
                    list = list.Where(a => a.TimeTableItemTitle.Contains(timeTableItemTitle));

                timeTableItemsVMList = list.OrderByDescending(s => s.TimeTableItemId).ToList();

            }
            catch (Exception exc)
            { }
            return timeTableItemsVMList;
        }

        public List<TimeTableItemsVM> GetListOfTimeTableItems(int jtStartIndex,
              int jtPageSize,
              ref int listCount,
              List<long> childsUsersIds,
              string jtSorting = null,
              long? TimeTableId = 0,
              long? timeTableItemParentId = 0,
              string? timeTableItemTitle = "")
        {

            List<TimeTableItemsVM> timeTableItemsVMList = new List<TimeTableItemsVM>();

            try
            {
                var list = (from c in projectsApiDb.TimeTableItems
                            select new TimeTableItemsVM
                            {
                                TimeTableItemId = c.TimeTableItemId,
                                TimeTableItemCurrentPath = c.TimeTableItemCurrentPath,
                                TimeTableItemEndDate = c.TimeTableItemEndDate,
                                TimeTableItemEndEnDate = c.TimeTableItemEndEnDate,
                                TimeTableItemStartEnDate = c.TimeTableItemStartEnDate,
                                TimeTableItemParentId = c.TimeTableItemId,
                                TimeTableItemParentPath = c.TimeTableItemParentPath,
                                TimeTableItemStartDate = c.TimeTableItemStartDate,
                                TimeTableItemTitle = c.TimeTableItemTitle,
                                TimeTableItemValue = c.TimeTableItemValue,
                                UserIdCreator = c.UserIdCreator.Value,
                                CreateEnDate = c.CreateEnDate,
                                CreateTime = c.CreateTime,
                                EditEnDate = c.EditEnDate,
                                EditTime = c.EditTime,
                                UserIdEditor = c.UserIdEditor.Value,
                                RemoveEnDate = c.RemoveEnDate,
                                RemoveTime = c.EditTime,
                                UserIdRemover = c.UserIdRemover.Value,
                                IsActivated = c.IsActivated,
                                IsDeleted = c.IsDeleted
                            }
                        //.Where(c => childsUsersIds.Contains(c.UserIdCreator.Value))
                        ).AsEnumerable();

                if (TimeTableId.HasValue)
                    if (TimeTableId.Value > 0)
                        list = list.Where(a => a.TimeTableItemId.Equals(TimeTableId.Value));

                if (timeTableItemParentId.HasValue)
                    if (timeTableItemParentId.Value > 0)
                        list = list.Where(a => a.TimeTableItemParentId.HasValue).Where(a => a.TimeTableItemParentId.Value.Equals(timeTableItemParentId.Value));

                listCount = list.Count();

                try
                {
                    if (string.IsNullOrEmpty(jtSorting))
                    {

                        if (listCount > jtPageSize)
                        {

                            timeTableItemsVMList = list.OrderByDescending(s => s.TimeTableItemId)
                                     .Skip(jtStartIndex).Take(jtPageSize).ToList();
                        }
                        else
                            timeTableItemsVMList = list.OrderByDescending(s => s.TimeTableItemId).ToList();
                    }
                    else
                    {


                        switch (jtSorting)
                        {
                            case "TimeTableItemTitle ASC":
                                list = list.OrderBy(l => l.TimeTableItemTitle);
                                break;
                            case "TimeTableItemTitle DESC":
                                list = list.OrderByDescending(l => l.TimeTableItemTitle);
                                break;
                        }
                        if (listCount > jtPageSize)
                        {
                            timeTableItemsVMList = list.Skip(jtStartIndex).Take(jtPageSize).ToList();
                        }
                        else
                        {

                            timeTableItemsVMList = list.ToList();
                        }
                    }
                }
                catch (Exception exc)
                { }
            }
            catch (Exception exc)
            { }
            return timeTableItemsVMList;
        }


        public bool AddToTimeTableItems(
            TimeTableItemsVM timeTableItemsVM)
        {
            try
            {
                TimeTableItems timeTableItem = _mapper.Map<TimeTableItemsVM, TimeTableItems>(timeTableItemsVM);


                projectsApiDb.TimeTableItems.Add(timeTableItem);
                projectsApiDb.SaveChanges();

                return true;
            }
            catch (Exception)
            {

            }
            return false;

        }
        public bool UpdateTimeTableItems(
            List<long> childsUsersIds,
            TimeTableItemsVM timeTableItemsVM)
        {
            var timeTableItemId = timeTableItemsVM.TimeTableItemId;

            if (projectsApiDb.TimeTableItems.Where(n => childsUsersIds.Contains(n.UserIdCreator.Value)).Where(x => x.TimeTableItemId.Equals(timeTableItemId)).Any())
            {

                try
                {
                    bool? isActivated = timeTableItemsVM.IsActivated.HasValue ? timeTableItemsVM.IsActivated.Value : (bool?)true;
                    bool? isDeleted = timeTableItemsVM.IsDeleted.HasValue ? timeTableItemsVM.IsDeleted.Value : (bool?)true;


                    TimeTableItems timeTableItem = projectsApiDb.TimeTableItems.Where(a => a.TimeTableItemId == timeTableItemId).FirstOrDefault();
                    timeTableItem.TimeTableItemCurrentPath = timeTableItem.TimeTableItemCurrentPath;
                    timeTableItem.TimeTableItemEndDate = timeTableItemsVM.TimeTableItemEndDate;

                    timeTableItem.TimeTableItemEndEnDate = timeTableItemsVM.TimeTableItemEndEnDate.HasValue ?
                        timeTableItemsVM.TimeTableItemEndEnDate.Value :
                        (DateTime?)null;

                    timeTableItem.TimeTableItemParentId = timeTableItemsVM.TimeTableItemParentId.HasValue ?
                        timeTableItemsVM.TimeTableItemParentId.Value :
                        0;

                    timeTableItem.TimeTableItemId = timeTableItemsVM.TimeTableItemId;
                    timeTableItem.TimeTableItemParentPath = timeTableItemsVM.TimeTableItemParentPath;
                    timeTableItem.TimeTableItemStartDate = timeTableItemsVM.TimeTableItemStartDate;

                    timeTableItem.TimeTableItemStartEnDate = timeTableItemsVM.TimeTableItemStartEnDate.HasValue ?
                        timeTableItemsVM.TimeTableItemStartEnDate.Value :
                        (DateTime?)null;

                    timeTableItem.TimeTableItemTitle = timeTableItemsVM.TimeTableItemTitle;

                    timeTableItem.TimeTableItemValue = timeTableItemsVM.TimeTableItemValue.HasValue ?
                        timeTableItemsVM.TimeTableItemValue.Value :
                        0;

                    timeTableItem.EditTime = timeTableItemsVM.EditTime;
                    timeTableItem.EditEnDate = timeTableItemsVM.EditEnDate;
                    timeTableItem.UserIdEditor = timeTableItemsVM.UserIdEditor;
                    timeTableItem.IsActivated = isActivated;
                    timeTableItem.IsDeleted = isDeleted;

                    projectsApiDb.Entry<TimeTableItems>(timeTableItem).State = EntityState.Modified;

                    projectsApiDb.SaveChanges();

                    return true;
                }
                catch (Exception)
                {

                }
            }

            return false;
        }

        public bool ToggleActivationTimeTableItems(long timeTableItemId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var timeTableItems = (from c in projectsApiDb.TimeTableItems
                                      where c.TimeTableItemId == timeTableItemId &&
                                      childsUsersIds.Contains(c.UserIdCreator.Value)
                                      select c).FirstOrDefault();

                if (timeTableItems != null)
                {
                    timeTableItems.IsActivated = !timeTableItems.IsActivated;
                    timeTableItems.EditEnDate = DateTime.Now;
                    timeTableItems.EditTime = PersianDate.TimeNow;
                    timeTableItems.UserIdEditor = userId;

                    projectsApiDb.Entry<Entities.TimeTableItems>(timeTableItems).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool TemporaryDeleteTimeTableItems(long timeTableItemId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var timeTableItems = (from c in projectsApiDb.TimeTableItems
                                      where c.TimeTableItemId == timeTableItemId &&
                                      childsUsersIds.Contains(c.UserIdCreator.Value)
                                      select c).FirstOrDefault();

                if (timeTableItems != null)
                {
                    timeTableItems.IsDeleted = !timeTableItems.IsDeleted;
                    timeTableItems.EditEnDate = DateTime.Now;
                    timeTableItems.EditTime = PersianDate.TimeNow;
                    timeTableItems.UserIdEditor = userId;

                    projectsApiDb.Entry<Entities.TimeTableItems>(timeTableItems).State = EntityState.Modified;
                    projectsApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }
        public string CompleteDeleteTimeTableItems(long timeTableItemId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var timeTableItem = (from c in projectsApiDb.TimeTableItems
                                     where c.TimeTableItemId == timeTableItemId &&
                                     childsUsersIds.Contains(c.UserIdCreator.Value)
                                     select c).FirstOrDefault();

                if (timeTableItem != null)
                {
                    projectsApiDb.TimeTableItems.Remove(timeTableItem);
                    projectsApiDb.SaveChanges();
                    return "OK";
                }
            }
            catch (Exception exc)
            { }

            return "ERROR";


        }
        #endregion

        #region Methods For Work With TeniacoSuggestionFiles

        public List<TeniacoSuggestionFilesVM> GetListOfTeniacoSuggestionFilesWithConstructionProjectId(long ConstructionProjectId,
             List<long> childsUsersIds)
        {

            List<TeniacoSuggestionFilesVM> TeniacoSuggestionFilesVMList = new List<TeniacoSuggestionFilesVM>();

            try
            {
                var teniacoSuggestionFiles = projectsApiDb.TeniacoSuggestionFiles.Where(tg => childsUsersIds.Contains(tg.UserIdCreator.Value) && tg.ConstructionProjectId == ConstructionProjectId).OrderBy(tg => tg.SuggestionFileOrder).ToList();

                if (teniacoSuggestionFiles != null)
                {
                    if (teniacoSuggestionFiles.Count() > 0)
                    {
                        TeniacoSuggestionFilesVMList = _mapper.Map<List<TeniacoSuggestionFiles>, List<TeniacoSuggestionFilesVM>>(teniacoSuggestionFiles);
                    }
                    else
                    {
                        TeniacoSuggestionFilesVMList = new List<TeniacoSuggestionFilesVM>();
                    }
                }
                else
                {
                    TeniacoSuggestionFilesVMList = new List<TeniacoSuggestionFilesVM>();
                }

            }
            catch (Exception exc)
            {
            }
            return TeniacoSuggestionFilesVMList;
        }



        public List<SuggestedConstructionProjectsVM> GetListOfTeniacoSuggestedProjects(
            int pageNumber,
            int pageSize,
            List<long> childsUsersIds,
            ITeniacoApiBusiness teniacoApiDb,
            IPublicApiBusiness publicApiDb)

        {

            List<SuggestedConstructionProjectsVM> SuggestedConstructionProjectsVMList = new List<SuggestedConstructionProjectsVM>();

            try
            {
                var suggestionFiles = projectsApiDb.TeniacoSuggestionFiles.ToList();
                var suggestedProjectsIds = suggestionFiles.Select(tg => tg.ConstructionProjectId).Distinct().ToList();
                var suggestedProjects = projectsApiDb.ConstructionProjects.Where(p => suggestedProjectsIds.Contains(p.ConstructionProjectId)).ToList();

                var propertyIds = suggestedProjects.Select(c => c.PropertyId).ToList().Distinct();
                var propertyAddress = teniacoApiDb.TeniacoApiDb.PropertyAddress.Where(c => propertyIds.Contains(c.PropertyId)).ToList();

                var constructionPriceHistories = projectsApiDb.ConstructionProjectPriceHistories.Where(c => suggestedProjectsIds.Contains(c.ConstructionProjectId)).ToList();

                var stateIds = propertyAddress.Select(c => c.StateId).ToList().Distinct();
                var cityIds = propertyAddress.Select(c => c.CityId).ToList().Distinct();
                var zoneIds = propertyAddress.Select(c => c.ZoneId).ToList().Distinct();
                var districtsIds = propertyAddress.Select(c => c.DistrictId).ToList().Distinct();


                var states = publicApiDb.PublicApiDb.States.Where(c => stateIds.Contains(c.StateId)).ToList();
                var cities = publicApiDb.PublicApiDb.Cities.Where(c => cityIds.Contains(c.CityId)).ToList();
                var zones = publicApiDb.PublicApiDb.Zones.Where(c => zoneIds.Contains(c.ZoneId)).ToList();
                var districts = publicApiDb.PublicApiDb.Districts.Where(c => districtsIds.Contains(c.DistrictId)).ToList();




                foreach (var project in suggestedProjects)
                {
                    #region Filling Address

                    var address = propertyAddress.Where(c => c.PropertyId.Equals(project.PropertyId)).FirstOrDefault();

                    var stateName = "";
                    var cityName = "";
                    var zoneName = "";
                    var districtName = "";
                    if (address != null)
                    {
                        stateName = states.Where(c => c.StateId.Equals(address.StateId)).Select(c => c.StateName).FirstOrDefault();
                        cityName = cities.Where(c => c.CityId.Equals(address.CityId)).Select(c => c.CityName).FirstOrDefault();
                        zoneName = zones.Where(c => c.ZoneId.Equals(address.ZoneId)).Select(c => c.ZoneName).FirstOrDefault();
                        districtName = districts.Where(c => c.DistrictId.Equals(address.DistrictId)).Select(c => c.DistrictName).FirstOrDefault();

                    }


                    var suggestionFilesVMList = new List<TeniacoSuggestionFilesVM>();
                    if (suggestionFiles.Where(tg => tg.ConstructionProjectId == project.ConstructionProjectId).Any())
                    {

                        var suggestionFilesList = suggestionFiles.Where(tg => tg.ConstructionProjectId == project.ConstructionProjectId).ToList();

                        suggestionFilesVMList = suggestionFilesList.Select(tg => new TeniacoSuggestionFilesVM
                        {
                            SuggestionFileId = tg.SuggestionFileId,
                            ConstructionProjectId = tg.ConstructionProjectId,
                            SuggestionFileDescription = tg.SuggestionFileDescription,
                            SuggestionFilePath = tg.SuggestionFilePath,
                            SuggestionFileExt = tg.SuggestionFileExt,
                            SuggestionFileOrder = tg.SuggestionFileOrder,
                            SuggestionFileTitle = tg.SuggestionFileTitle,
                            SuggestionFileType = tg.SuggestionFileType
                        }).ToList();
                    }


                    var constructionProjectPriceHistoriesVM = new ConstructionProjectPriceHistoriesVM();
                    if (constructionPriceHistories.Where(d => d.ConstructionProjectId.Equals(project.ConstructionProjectId)).Any())
                    {
                        var constructionPriceHistory = constructionPriceHistories.Where(d => d.ConstructionProjectId.Equals(project.ConstructionProjectId)).OrderByDescending(c => c.ConstructionProjectPriceHistoryId).ToList();


                        constructionProjectPriceHistoriesVM = constructionPriceHistory.Select(v => new ConstructionProjectPriceHistoriesVM()
                        {
                            ConstructionProjectPriceHistoryId = v.ConstructionProjectPriceHistoryId,
                            CurrentValueOfProject = v.CurrentValueOfProject,
                            ProjectEstimate = v.ProjectEstimate,
                            PrevisionOfCost = v.PrevisionOfCost,
                            ConstructionProjectId = v.ConstructionProjectId,

                        }).FirstOrDefault();
                    }

                    SuggestedConstructionProjectsVM suggestedConstructionProjectsVM = new()
                    {
                        ConstructionProjectId = project.ConstructionProjectId,
                        ConstructionProjectTitle = project.ConstructionProjectTitle,
                        StartDateEn = project.StartDateEn,
                        EndDateEn = (project.StartDateEn.HasValue && !string.IsNullOrEmpty(project.MonthsLeftUntilTheEnd)) ? project.StartDateEn.Value.AddMonths(int.Parse(project.MonthsLeftUntilTheEnd)).Date : null,
                        SuggestionTitle = project.SuggestionTitle,
                        MonthsLeftUntilTheEnd = project.MonthsLeftUntilTheEnd,
                        propertyAddressVM = new PropertyAddressVM()
                        {
                            StateName = !stateName.IsNullOrEmpty() ? stateName : "",
                            CityName = !cityName.IsNullOrEmpty() ? cityName : "",
                            ZoneName = !zoneName.IsNullOrEmpty() ? zoneName : "",
                            DistrictName = !districtName.IsNullOrEmpty() ? districtName : "",
                            LocationLat = address.LocationLat,
                            LocationLon = address.LocationLon,
                            Address = address.Address,
                        },
                        suggestionFiles = suggestionFilesVMList,
                        constructionProjectPriceHistoriesVM = constructionProjectPriceHistoriesVM

                    };


                    #endregion


                    SuggestedConstructionProjectsVMList.Add(suggestedConstructionProjectsVM);
                }
            }
            catch (Exception exc)
            {
            }
            return SuggestedConstructionProjectsVMList;
        }


        public long AddToTeniacoSuggestionFiles(
                List<TeniacoSuggestionFilesVM> teniacoSuggestionFilesVM,
                List<long> childsUsersIds,
                long userId,
                string? SuggestionPageTitle)
        {
            try
            {
                // بروزرسانی عنوان صفحه پیشنهادات تنیاکو
                var project = projectsApiDb.ConstructionProjects.Where(p => p.ConstructionProjectId == teniacoSuggestionFilesVM[0].ConstructionProjectId).FirstOrDefault();
                project.SuggestionTitle = SuggestionPageTitle;
                projectsApiDb.ConstructionProjects.Update(project);
                projectsApiDb.SaveChanges();


                foreach (var suggestionFile in teniacoSuggestionFilesVM)
                {

                    if (suggestionFile.SuggestionFileId > 0) // اگر ریکورد قبلا وجود داشت فقط ترتیب آن را آپدیت کند
                    {
                        var entity = projectsApiDb.TeniacoSuggestionFiles.Where(tg => tg.SuggestionFileId == suggestionFile.SuggestionFileId).FirstOrDefault();
                        entity.SuggestionFileOrder = suggestionFile.SuggestionFileOrder;
                        projectsApiDb.TeniacoSuggestionFiles.Update(entity);
                        projectsApiDb.SaveChanges();
                    }
                    else // اگر ریکورد جدید است کامل آن را اضافه کند
                    {
                        TeniacoSuggestionFiles teniacoSuggestionFiles = _mapper.Map<TeniacoSuggestionFilesVM, TeniacoSuggestionFiles>(suggestionFile);

                        teniacoSuggestionFiles.IsActivated = true;
                        teniacoSuggestionFiles.IsDeleted = false;
                        teniacoSuggestionFiles.UserIdCreator = userId;

                        projectsApiDb.TeniacoSuggestionFiles.Add(teniacoSuggestionFiles);
                        projectsApiDb.SaveChanges();
                    }

                }

                return 1;
            }
            catch (Exception exc)
            {

            }

            return 0;

        }



        public long DeleteTeniacoSuggestionFile(
                long SuggestionFileId,
                List<long> childsUsersIds)
        {
            try
            {
                TeniacoSuggestionFiles teniacoSuggestionFiles = projectsApiDb.TeniacoSuggestionFiles.Where(tg => tg.SuggestionFileId == SuggestionFileId && childsUsersIds.Contains(tg.UserIdCreator.Value)).FirstOrDefault();

                if (teniacoSuggestionFiles != null)
                {
                    projectsApiDb.TeniacoSuggestionFiles.Remove(teniacoSuggestionFiles);
                    projectsApiDb.SaveChanges();

                    // reordering rest files
                    var restFiles = projectsApiDb.TeniacoSuggestionFiles.Where(tg => tg.ConstructionProjectId == teniacoSuggestionFiles.ConstructionProjectId).OrderBy(tg => tg.SuggestionFileOrder).ToList();
                    for (int i = 0; i < restFiles.Count; i++)
                    {
                        restFiles[i].SuggestionFileOrder = i + 1;
                        projectsApiDb.TeniacoSuggestionFiles.Update(restFiles[i]);
                        projectsApiDb.SaveChanges();
                    }

                    return 1;
                }

                return teniacoSuggestionFiles.SuggestionFileId;
            }
            catch (Exception exc)
            {

            }

            return 0;

        }



        public long EditTeniacoSuggestionFile(
                List<TeniacoSuggestionFilesVM> teniacoSuggestionFilesVM,
                List<long> childsUsersIds)
        {
            try
            {
                foreach (var suggestionFile in teniacoSuggestionFilesVM)
                {
                    var entity = projectsApiDb.TeniacoSuggestionFiles.Where(tg => tg.SuggestionFileId == suggestionFile.SuggestionFileId).FirstOrDefault();
                    entity.SuggestionFileOrder = suggestionFile.SuggestionFileOrder;
                    entity.SuggestionFileDescription = suggestionFile.SuggestionFileDescription;

                    if (suggestionFile.SuggestionFilePath != null)
                    {
                        entity.SuggestionFilePath = suggestionFile.SuggestionFilePath;
                        entity.SuggestionFileTitle = suggestionFile.SuggestionFileTitle;
                        entity.SuggestionFileExt = suggestionFile.SuggestionFileExt;
                    }

                    projectsApiDb.TeniacoSuggestionFiles.Update(entity);
                    projectsApiDb.SaveChanges();
                }

                return 1;
            }
            catch (Exception exc)
            {

            }

            return 0;

        }


        #endregion

        #endregion
    }
}




