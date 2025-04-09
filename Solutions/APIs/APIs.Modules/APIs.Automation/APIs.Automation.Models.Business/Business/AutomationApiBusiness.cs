using APIs.Automation.Models;
using AutoMapper;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using VM.Automation;
using System.Linq;
using APIs.Automation.Models.Entities;
using FrameWork;
using VM.Console;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using VM.Public;
using System.Reflection;
using System.Runtime;
using System.Resources;
using Microsoft.EntityFrameworkCore.Storage;
using Models.Business.ConsoleBusiness;
using Models.Entities.Console;
using VM.Teniaco;
using System.Security.Cryptography;

namespace APIs.Automation.Models.Business
{
    public class AutomationApiBusiness : IAutomationApiBusiness, IDisposable
    {
        private AutomationApiContext automationApiDb = new AutomationApiContext();

        private IMapper _mapper;

        private IHostEnvironment hostEnvironment;

        private AutomationApiContext AutomationApiDb
        {
            get { return this.automationApiDb; }
            set { }
            //private set { }
        }

        public void Dispose()
        {
            automationApiDb.Dispose();
        }

        public AutomationApiBusiness(IMapper mapper,
            AutomationApiContext _AutomationApiDb,
            IHostEnvironment _hostEnvironment)
        {
            try
            {
                _mapper = mapper;

                AutomationApiDb = _AutomationApiDb;

                AutomationApiDb = AutomationApiDb;

                hostEnvironment = _hostEnvironment;
            }
            catch (Exception exc)
            {
            }
        }

        #region Automation

        #region Methods For Work With BoardMembers

        public List<BoardMembersVM> GetAllBoardMembersList(int orgChartNodeId)
        {
            List<BoardMembersVM> boardMembersList = new List<BoardMembersVM>();

            try
            {
                var list = automationApiDb.BoardMembers.Where(m => m.OrgChartNodeId.Equals(orgChartNodeId) && m.IsDeleted.Value.Equals(false) && m.IsActivated.Value.Equals(true)).AsQueryable();

                boardMembersList = _mapper.Map<List<BoardMembers>, List<BoardMembersVM>>(list.ToList());
            }
            catch (Exception exc)
            { }

            return boardMembersList;
        }

        public bool EditBoardMembers(List<BoardMembersVM> boardMembersVMList,
            int orgChartNodeId)
        {
            try
            {
                if (automationApiDb.BoardMembers.Where(m => m.OrgChartNodeId.Equals(orgChartNodeId)).Any())
                {
                    if (boardMembersVMList.Count > 0)
                    {
                        using (var dbContextTransaction = automationApiDb.Database.BeginTransaction())
                        {
                            try
                            {
                                #region remvoe old BoardMembers

                                var oldBoardMembers = automationApiDb.BoardMembers.Where(m => m.OrgChartNodeId.Equals(orgChartNodeId)).ToList();
                                automationApiDb.BoardMembers.RemoveRange(oldBoardMembers);
                                automationApiDb.SaveChanges();

                                #endregion

                                #region new BoardMembers

                                var newBoardMembers = new List<BoardMembers>();
                                foreach (var item in boardMembersVMList)
                                {
                                    newBoardMembers.Add(new BoardMembers()
                                    {
                                        CreateEnDate = DateTime.Now,
                                        CreateTime = PersianDate.TimeNow,
                                        IsActivated = true,
                                        IsCeo = item.IsCeo,
                                        IsDeleted = false,
                                        UserId = item.UserId,
                                        OrgChartNodeId = orgChartNodeId
                                    });
                                }

                                automationApiDb.BoardMembers.AddRange(newBoardMembers);
                                automationApiDb.SaveChanges();

                                #endregion

                                dbContextTransaction.Commit();

                                return true;
                            }
                            catch (Exception excx)
                            {

                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        #endregion

        #region Methods For Work With DepartmentsStaff

        public List<DepartmentsStaffVM> GetListOfDepartmentsStaff(int jtStartIndex,
            int jtPageSize,
            ref int listCount,
            List<long> childsUsersIds,
            string jtSorting = null)
        {
            List<DepartmentsStaffVM> departmentsStaffVMList = new List<DepartmentsStaffVM>();
            try
            {
                var list = automationApiDb.DepartmentsStaff
                    .Where(c => childsUsersIds.Contains(c.UserIdCreator.Value) && c.IsDeleted.Value.Equals(false) && c.IsActivated.Value.Equals(true))
                    .AsQueryable();

                listCount = list.Count();
                if (string.IsNullOrEmpty(jtSorting))
                {
                    if (listCount > jtPageSize)
                    {
                        departmentsStaffVMList = _mapper.Map<List<DepartmentsStaff>,
                            List<DepartmentsStaffVM>>(list
                            .OrderByDescending(s => s.DepartmentStaffId)
                            .Skip(jtStartIndex).Take(jtPageSize).ToList());
                    }
                    else
                    {
                        departmentsStaffVMList = _mapper.Map<List<DepartmentsStaff>,
                            List<DepartmentsStaffVM>>(list
                            .OrderByDescending(s => s.DepartmentStaffId).ToList());
                    }
                }
                else
                {
                    //switch (jtSorting)
                    //{
                    //    case "DepartmentStaffName ASC":
                    //        list = list.OrderBy(l => l.DepartmentStaffName);
                    //        break;
                    //    case "DepartmentStaffName DESC":
                    //        list = list.OrderByDescending(l => l.DepartmentStaffName);
                    //        break;
                    //}

                    if (listCount > jtPageSize)
                    {
                        departmentsStaffVMList = _mapper.Map<List<DepartmentsStaff>,
                            List<DepartmentsStaffVM>>(list.Skip(jtStartIndex).Take(jtPageSize).ToList());
                    }
                    else
                    {
                        departmentsStaffVMList = _mapper.Map<List<DepartmentsStaff>,
                            List<DepartmentsStaffVM>>(list.ToList());
                    }
                }

                #region rewrite inside module

                //foreach (var departmentsStaffVM in departmentsStaffVMList)
                //{
                //    if (departmentsStaffVM.UserIdCreator.HasValue)
                //    {
                //        var user = automationApiDb.Users.FirstOrDefault(u => u.UserId.Equals(departmentsStaffVM.UserIdCreator.Value));
                //        var userDetails = automationApiDb.UsersProfile.FirstOrDefault(up => up.UserId.Equals(departmentsStaffVM.UserIdCreator.Value));
                //        departmentsStaffVM.UserCreatorName = user.UserName;

                //        if (!string.IsNullOrEmpty(userDetails.Name))
                //            departmentsStaffVM.UserCreatorName += " - " + userDetails.Name;

                //        if (!string.IsNullOrEmpty(userDetails.Family))
                //            departmentsStaffVM.UserCreatorName += " - " + userDetails.Family;
                //    }
                //}

                #endregion
            }
            catch (Exception exc)
            { }
            return departmentsStaffVMList;
        }

        public int AddToDepartmentsStaff(DepartmentsStaffVM departmentsStaffVM,
            List<long> childsUsersIds)
        {
            try
            {
                //if (!automationApiDb.DepartmentsStaff.Any(c =>
                //    childsUsersIds.Contains(c.UserIdCreator.Value) &&
                //    //c.UserIdCreator.Value.Equals(departmentsStaffVM.UserIdCreator.Value) &&
                //    c.DepartmentStaffName.Equals(departmentsStaffVM.DepartmentStaffName)))
                //{
                DepartmentsStaff departmentStaff = _mapper.Map<DepartmentsStaffVM, DepartmentsStaff>(departmentsStaffVM);
                automationApiDb.DepartmentsStaff.Add(departmentStaff);
                automationApiDb.SaveChanges();
                return departmentStaff.DepartmentStaffId;
                //}
            }
            catch (Exception exc)
            { }
            return 0;
        }

        public bool UpdateDepartmentsStaff(ref DepartmentsStaffVM departmentsStaffVM,
            //long userId,
            List<long> childsUsersIds)
        {
            try
            {
                int departmentStaffId = departmentsStaffVM.DepartmentStaffId;
                //string departmentStaffName = departmentsStaffVM.DepartmentStaffName;

                //List<long> childsUsersIds = new List<long>();
                //childsUsersIds = GetChildUserIds(ref childsUsersIds, userId).Distinct().ToList();

                //if (!automationApiDb.DepartmentsStaff.Any(c =>
                //        childsUsersIds.Contains(c.UserIdCreator.Value) &&
                //        //c.UserIdCreator.Value.Equals(userId) &&
                //        !c.DepartmentStaffId.Equals(departmentStaffId) &&
                //        c.DepartmentStaffName.Equals(departmentStaffName)))
                //{
                DepartmentsStaff oldDepartmentStaff = (from c in automationApiDb.DepartmentsStaff
                                                       where c.DepartmentStaffId == departmentStaffId
                                                       select c).FirstOrDefault();

                //oldDepartmentStaff.DepartmentStaffName = departmentsStaffVM.DepartmentStaffName;
                //oldDepartmentStaff.DeviceParentCategoryId = departmentsStaffVM.DeviceParentCategoryId.HasValue ?
                //    departmentsStaffVM.DeviceParentCategoryId.Value :
                //    (int?)null;
                oldDepartmentStaff.EditEnDate = departmentsStaffVM.EditEnDate.Value;
                oldDepartmentStaff.EditTime = departmentsStaffVM.EditTime;
                //oldDepartmentStaff.DepartmentStaffDesc = departmentsStaffVM.DepartmentStaffDesc;
                oldDepartmentStaff.IsActivated = departmentsStaffVM.IsActivated;
                oldDepartmentStaff.IsDeleted = departmentsStaffVM.IsDeleted;
                oldDepartmentStaff.UserIdEditor = departmentsStaffVM.UserIdEditor;

                automationApiDb.Entry<DepartmentsStaff>(oldDepartmentStaff).State = EntityState.Modified;
                automationApiDb.SaveChanges();

                departmentsStaffVM.UserIdCreator = oldDepartmentStaff.UserIdCreator.Value;

                #region rewrite inside module

                //long userIdCreator = departmentsStaffVM.UserIdCreator.Value;
                //if (departmentsStaffVM.UserIdCreator.HasValue)
                //{
                //    var user = automationApiDb.Users.FirstOrDefault(u => u.UserId.Equals(userIdCreator));
                //    var userDetails = automationApiDb.UsersProfile.FirstOrDefault(up => up.UserId.Equals(userIdCreator));
                //    departmentsStaffVM.UserCreatorName = user.UserName;

                //    if (!string.IsNullOrEmpty(userDetails.Name))
                //        departmentsStaffVM.UserCreatorName += " - " + userDetails.Name;

                //    if (!string.IsNullOrEmpty(userDetails.Family))
                //        departmentsStaffVM.UserCreatorName += " - " + userDetails.Family;
                //}

                #endregion

                return true;
                //}
            }
            catch (Exception exc)
            { }
            return false;
        }

        public bool ToggleActivationDepartmentsStaff(int departmentStaffId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                //List<long> childsUsersIds = new List<long>();
                //childsUsersIds = GetChildUserIds(ref childsUsersIds, userId).Distinct().ToList();
                var departmentStaff = (from c in automationApiDb.DepartmentsStaff
                                       where c.DepartmentStaffId == departmentStaffId &&
                                       childsUsersIds.Contains(c.UserIdCreator.Value)
                                       select c).FirstOrDefault();

                if (departmentStaff != null)
                {
                    departmentStaff.IsActivated = !departmentStaff.IsActivated;
                    departmentStaff.EditEnDate = DateTime.Now;
                    departmentStaff.EditTime = PersianDate.TimeNow;
                    departmentStaff.UserIdEditor = userId;
                    automationApiDb.Entry<DepartmentsStaff>(departmentStaff).State = EntityState.Modified;
                    automationApiDb.SaveChanges();
                    return true;
                }
            }
            catch (Exception exc)
            { }
            return false;
        }

        public bool TemporaryDeleteDepartmentsStaff(int departmentStaffId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                //List<long> childsUsersIds = new List<long>();
                //childsUsersIds = GetChildUserIds(ref childsUsersIds, userId).Distinct().ToList();
                var departmentStaff = (from c in automationApiDb.DepartmentsStaff
                                       where c.DepartmentStaffId == departmentStaffId &&
                                       childsUsersIds.Contains(c.UserIdCreator.Value)
                                       select c).FirstOrDefault();

                if (departmentStaff != null)
                {
                    departmentStaff.IsDeleted = !departmentStaff.IsDeleted;
                    departmentStaff.RemoveEnDate = DateTime.Now;
                    departmentStaff.RemoveTime = PersianDate.TimeNow;
                    departmentStaff.UserIdRemover = userId;
                    automationApiDb.Entry<DepartmentsStaff>(departmentStaff).State = EntityState.Modified;
                    automationApiDb.SaveChanges();
                    return true;
                }
            }
            catch (Exception exc)
            { }
            return false;
        }

        public bool CompleteDeleteDepartmentsStaff(int departmentStaffId,
            //long userId,
            List<long> childsUsersIds,
            ref string returnMessage)
        {
            returnMessage = "";

            try
            {
                //List<long> childsUsersIds = new List<long>();
                //childsUsersIds = GetChildUserIds(ref childsUsersIds, userId).Distinct().ToList();
                var departmentStaff = (from c in automationApiDb.DepartmentsStaff
                                       where c.DepartmentStaffId == departmentStaffId &&
                                       childsUsersIds.Contains(c.UserIdCreator.Value)
                                       select c).FirstOrDefault();

                if (departmentStaff != null)
                {
                    automationApiDb.DepartmentsStaff.Remove(departmentStaff);
                    automationApiDb.SaveChanges();
                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public List<DepartmentsStaffVM> GetAllDepartmentsStaffList(//long userId,
            List<long> childsUsersIds)
        {
            List<DepartmentsStaffVM> departmentsStaffVMList = new List<DepartmentsStaffVM>();
            try
            {
                var list = automationApiDb.DepartmentsStaff
                                .Where(a => a.IsDeleted.Value.Equals(false))
                                .Where(a => a.IsActivated.Value.Equals(true))
                                .Where(c => childsUsersIds.Contains(c.UserIdCreator.Value))
                                .AsQueryable();

                departmentsStaffVMList = _mapper.Map<List<DepartmentsStaff>,
                                List<DepartmentsStaffVM>>(list
                                .OrderByDescending(s => s.DepartmentStaffId).ToList());
            }
            catch (Exception exc)
            { }
            return departmentsStaffVMList;
        }

        public DepartmentsStaffVM GetDepartmentStaffWithDepartmentStaffId(int departmentStaffId,
            List<long> childsUsersIds)
        {
            DepartmentsStaffVM departmentsStaffVM = new DepartmentsStaffVM();

            try
            {
                departmentsStaffVM = _mapper.Map<DepartmentsStaff,
                                DepartmentsStaffVM>(automationApiDb.DepartmentsStaff
                                .Where(a => a.DepartmentStaffId.Equals(departmentStaffId))
                                .Where(a => childsUsersIds.Contains(a.UserIdCreator.Value))
                                .FirstOrDefault());
            }
            catch (Exception exc)
            { }

            return departmentsStaffVM;
        }

        #endregion

        #region Methods For Work With Forms

        public List<FormsVM> GetListOfForms(
             int jtStartIndex,
           int jtPageSize,
           ref int listCount,
           string formName/*,
           List<long> childsUsersIds,
           string Lang = null,
           string jtSorting = null
           /*long userId = 0*/)
        {
            List<FormsVM> formsVMList = new List<FormsVM>();

            var list = automationApiDb.Forms.Where(c => c.IsDeleted.Value.Equals(false) && c.IsActivated.Value.Equals(true))
                    //.Where(c => childsUsersIds.Contains(c.UserIdCreator.Value))
                    .AsQueryable();

            //if (!string.IsNullOrEmpty(Lang))
            //{
            //    list = list.Where(x => x.Lang.Equals(Lang));
            //}

            if (!string.IsNullOrEmpty(formName))
                list = list.Where(a => a.FormName.Contains(formName));

            try
            {
                listCount = list.Count();

                if (listCount > jtPageSize)
                {
                    formsVMList = _mapper.Map<List<Forms>, List<FormsVM>>(list.OrderByDescending(s => s.FormId)
                             .Skip(jtStartIndex).Take(jtPageSize).ToList());

                }
                else
                {
                    formsVMList = _mapper.Map<List<Forms>,
                        List<FormsVM>>(list.OrderByDescending(s => s.FormId).ToList());
                }
            }
            catch (Exception exc)
            { }
            return formsVMList;
        }

        public List<FormsVM> GetAllFormsList(ref int listCount,
            string formName)
        {
            List<FormsVM> formsVM = new List<FormsVM>();

            try
            {
                var list = automationApiDb.Forms.Where(f => f.IsActivated.Equals(true) &&
                        f.IsDeleted.Equals(false)).AsQueryable();

                if (!string.IsNullOrEmpty(formName))
                    list = list.Where(a => a.FormName.Contains(formName));

                listCount = list.Count();

                formsVM = _mapper.Map<List<Forms>, List<FormsVM>>(list.OrderByDescending(f => f.FormId).ToList());
            }
            catch (Exception exc)
            { }

            return formsVM;
        }

        public int AddToForms(FormsVM formsVM, List<long> childsUsersIds)
        {
            try
            {
                if (!automationApiDb.Forms.Where(p => p.FormName.Equals(formsVM.FormName) && childsUsersIds.Contains(p.UserIdCreator.Value)).Any())
                {
                    Forms forms = _mapper.Map<FormsVM, Forms>(formsVM);
                    automationApiDb.Forms.Add(forms);
                    automationApiDb.SaveChanges();
                    return forms.FormId;
                }
                else
                    return -1;
                //}
            }
            catch (Exception exc)
            { }
            return 0;
        }

        public FormsVM GetFormWithFormId(int formId)
        {
            FormsVM formsVM = new FormsVM();

            try
            {
                formsVM = _mapper.Map<Forms,
                    FormsVM>(automationApiDb.Forms
                    .Where(e => e.FormId.Equals(formId)).FirstOrDefault());
            }
            catch (Exception exc)
            { }

            return formsVM;
        }

        public FormsVM GetFormAndFieldsWithFormId(int formId)
        {
            FormsVM formsVM = new FormsVM();

            try
            {
                formsVM = _mapper.Map<Forms,
                    FormsVM>(automationApiDb.Forms
                    .Where(e => e.FormId.Equals(formId)).FirstOrDefault());

                if (automationApiDb.FormElements.Where(e => e.FormId.Equals(formId)).Any())
                {
                    formsVM.FormElementsVM = new List<FormElementsVM>();

                    formsVM.FormElementsVM = _mapper.Map<List<FormElements>,
                        List<FormElementsVM>>(automationApiDb.FormElements
                        .Where(e => e.FormId.Equals(formId)).OrderBy(e => e.FormElementOrder).ToList());

                    foreach (var formElementsVM in formsVM.FormElementsVM)
                    {
                        if (formElementsVM.ElementTypeId.Equals(2) ||//تک انتخابی
                            formElementsVM.ElementTypeId.Equals(3))//چند انتخابی
                        {
                            if (automationApiDb.FormElementOptions.Where(o => o.FormElementId.Equals(formElementsVM.FormElementId)).Any())
                            {
                                formElementsVM.FormElementOptionsVM = new List<FormElementOptionsVM>();

                                formElementsVM.FormElementOptionsVM = _mapper.Map<List<FormElementOptions>,
                                    List<FormElementOptionsVM>>(automationApiDb.FormElementOptions
                                    .Where(e => e.FormElementId.Equals(formElementsVM.FormElementId)).ToList());
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            { }

            return formsVM;
        }

        public int UpdateForms(ref FormsVM formsVM,
            //long userId,
            List<long> childsUsersIds)
        {
            try
            {
                int formId = formsVM.FormId;
                string formName = formsVM.FormName;

                if (!automationApiDb.Forms.Where(p => p.FormName.Equals(formName) ||
                    !p.FormId.Equals(formId)).Any())
                {
                    return -1;
                }

                Forms form = (from c in automationApiDb.Forms
                              where c.FormId == formId
                              select c).FirstOrDefault();

                form.EditEnDate = formsVM.EditEnDate.Value;
                form.EditTime = formsVM.EditTime;
                form.UserIdEditor = formsVM.UserIdEditor;
                form.FormName = formName;
                form.IsActivated = formsVM.IsActivated;
                form.IsDeleted = formsVM.IsDeleted;
                automationApiDb.Entry<Forms>(form).State = EntityState.Modified;
                automationApiDb.SaveChanges();

                formsVM.UserIdCreator = form.UserIdCreator.Value;

                #region rewrite inside module

                //long userIdCreator = FormsVM.UserIdCreator.Value;
                //if (FormsVM.UserIdCreator.HasValue)
                //{
                //    var user = automationApiDb.Users.FirstOrDefault(u => u.UserId.Equals(userIdCreator));
                //    var userDetails = automationApiDb.UsersProfile.FirstOrDefault(up => up.UserId.Equals(userIdCreator));
                //    FormsVM.UserCreatorName = user.UserName;

                //    if (!string.IsNullOrEmpty(userDetails.Name))
                //        FormsVM.UserCreatorName += " - " + userDetails.Name;

                //    if (!string.IsNullOrEmpty(userDetails.Family))
                //        FormsVM.UserCreatorName += " - " + userDetails.Family;
                //}

                #endregion

                return form.FormId;
                //}
            }
            catch (Exception exc)
            { }
            return 0;
        }

        public bool ToggleActivationForms(long formId, long userId, List<long> childsUsersIds)
        {
            try
            {
                //List<long> childsUsersIds = new List<long>();
                //childsUsersIds = GetChildUserIds(ref childsUsersIds, userId).Distinct().ToList();
                var form = (from c in automationApiDb.Forms
                            where c.FormId == formId &&
                            childsUsersIds.Contains(c.UserIdCreator.Value)
                            select c).FirstOrDefault();

                if (form != null)
                {
                    form.IsActivated = !form.IsActivated;
                    form.EditEnDate = DateTime.Now;
                    form.EditTime = PersianDate.TimeNow;
                    form.UserIdEditor = userId;
                    automationApiDb.Entry<Forms>(form).State = EntityState.Modified;
                    automationApiDb.SaveChanges();
                    return true;
                }
            }
            catch (Exception exc)
            { }
            return false;
        }

        public bool TemporaryDeleteForms(long formId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                //List<long> childsUsersIds = new List<long>();
                //childsUsersIds = GetChildUserIds(ref childsUsersIds, userId).Distinct().ToList();
                var form = (from c in automationApiDb.Forms
                            where c.FormId == formId &&
                            childsUsersIds.Contains(c.UserIdCreator.Value)
                            select c).FirstOrDefault();

                if (form != null)
                {
                    form.IsDeleted = !form.IsDeleted;
                    form.RemoveEnDate = DateTime.Now;
                    form.RemoveTime = PersianDate.TimeNow;
                    form.UserIdRemover = userId;
                    automationApiDb.Entry<Forms>(form).State = EntityState.Modified;
                    automationApiDb.SaveChanges();
                    return true;
                }
            }
            catch (Exception exc)
            { }
            return false;
        }

        public bool CompleteDeleteForms(long formId,
            //long userId,
            List<long> childsUsersIds)
        {
            using (var transaction = automationApiDb.Database.BeginTransaction())
            {
                try
                {
                    //List<long> childsUsersIds = new List<long>();
                    //childsUsersIds = GetChildUserIds(ref childsUsersIds, userId).Distinct().ToList();
                    var form = (from c in automationApiDb.Forms
                                where c.FormId == formId &&
                                childsUsersIds.Contains(c.UserIdCreator.Value)
                                select c).FirstOrDefault();

                    if (form != null)
                    {
                        #region check in use form

                        //returnMessage = "InUseForm";
                        //return false;

                        #endregion

                        #region remove FormElements

                        var formElements = automationApiDb.FormElements.Where(e => e.FormId.Equals(form.FormId)).ToList();

                        if (formElements != null)
                        {
                            var formElementids = formElements.Select(e => e.FormElementId).ToList();

                            #region remove FormElementOptions

                            var formElementOptions = automationApiDb.FormElementOptions.Where(o => formElementids.Contains(o.FormElementId)).ToList();

                            if (formElementOptions != null)
                            {
                                if (formElementOptions.Count > 0)
                                {
                                    automationApiDb.FormElementOptions.RemoveRange(formElementOptions);
                                    automationApiDb.SaveChanges();
                                }
                            }

                            #endregion

                            #region remove FormElementValues

                            var formElementValues = automationApiDb.FormElementValues.Where(v => formElementids.Contains(v.FormElementId)).ToList();

                            if (formElementValues != null)
                            {
                                if (formElementValues.Count > 0)
                                {
                                    automationApiDb.FormElementValues.RemoveRange(formElementValues);
                                    automationApiDb.SaveChanges();
                                }
                            }

                            #endregion

                            automationApiDb.FormElements.RemoveRange(formElements);
                            automationApiDb.SaveChanges();
                        }

                        #endregion

                        automationApiDb.Forms.Remove(form);
                        automationApiDb.SaveChanges();

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

        #endregion

        #region Methods For Work With FormElements

        public List<FormElementsVM> GetListOfFormElements(
            int jtStartIndex,
            int jtPageSize,
            ref int listCount,
            int? formId = null,
            string formElementTitle = "")
        {
            List<FormElementsVM> formElementsVMList = new List<FormElementsVM>();

            try
            {
                var list = automationApiDb.FormElements.Where(c => c.IsDeleted.Value.Equals(false) && c.IsActivated.Value.Equals(true))
                        //.Where(c => childsUsersIds.Contains(c.UserIdCreator.Value))
                        .AsQueryable();

                if (formId.HasValue)
                    list = list.Where(l => l.FormId.Equals(formId.Value));

                if (!string.IsNullOrEmpty(formElementTitle))
                    list = list.Where(a => a.FormElementTitle.Contains(formElementTitle));

                listCount = list.Count();

                if (listCount > jtPageSize)
                {
                    formElementsVMList = _mapper.Map<List<FormElements>, List<FormElementsVM>>(list.OrderByDescending(s => s.FormElementOrder)
                             .Skip(jtStartIndex).Take(jtPageSize).ToList());

                }
                else
                {
                    formElementsVMList = _mapper.Map<List<FormElements>,
                        List<FormElementsVM>>(list.OrderByDescending(s => s.FormElementOrder).ToList());
                }
            }
            catch (Exception exc)
            { }
            return formElementsVMList;
        }

        public List<FormElementsVM> GetAllFormElementsList(ref int listCount,
            int? formId = null,
            string formElementTitle = "")
        {
            List<FormElementsVM> formElementsVM = new List<FormElementsVM>();

            try
            {
                var list = automationApiDb.FormElements.Where(f => f.IsActivated.Equals(true) &&
                        f.IsDeleted.Equals(false)).AsQueryable();

                if (formId.HasValue)
                    list = list.Where(l => l.FormId.Equals(formId.Value));

                if (!string.IsNullOrEmpty(formElementTitle))
                    list = list.Where(a => a.FormElementTitle.Contains(formElementTitle));

                listCount = list.Count();

                formElementsVM = _mapper.Map<List<FormElements>, List<FormElementsVM>>(list.OrderByDescending(f => f.FormElementOrder).ToList());
            }
            catch (Exception exc)
            { }

            return formElementsVM;
        }

        public int AddToFormElements(FormElementsVM formElementsVM, List<long> childsUsersIds)
        {
            try
            {
                if (!automationApiDb.FormElements.Where(p => p.FormElementTitle.Equals(formElementsVM.FormElementTitle) &&
                    p.FormId.Equals(formElementsVM.FormId)).Any())
                {
                    FormElements formElements = _mapper.Map<FormElementsVM, FormElements>(formElementsVM);
                    automationApiDb.FormElements.Add(formElements);
                    automationApiDb.SaveChanges();
                    return formElements.FormElementId;
                }
                else
                    return -1;
                //}
            }
            catch (Exception exc)
            { }
            return 0;
        }

        public FormElementsVM GetFormElementWithFormElementId(int formElementId)
        {
            FormElementsVM formElementsVM = new FormElementsVM();

            try
            {
                formElementsVM = _mapper.Map<FormElements,
                    FormElementsVM>(automationApiDb.FormElements
                    .Where(e => e.FormElementId.Equals(formElementId)).FirstOrDefault());
            }
            catch (Exception exc)
            { }

            return formElementsVM;
        }

        public int UpdateFormElements(ref FormElementsVM formElementsVM,
            //long userId,
            List<long> childsUsersIds)
        {
            try
            {
                int formElementId = formElementsVM.FormElementId;
                int formId = formElementsVM.FormId;
                string formElementTitle = formElementsVM.FormElementTitle;
                int elementTypeId = formElementsVM.ElementTypeId;
                bool isRequired = formElementsVM.IsRequired;
                string defaultValue = formElementsVM.DefaultValue;
                int formElementOrder = formElementsVM.FormElementOrder;
                int formElementSize = formElementsVM.FormElementSize;

                if (automationApiDb.FormElements.Where(p => p.FormElementTitle.Equals(formElementTitle) &&
                    p.FormId.Equals(formId) &&
                    !p.FormElementId.Equals(formElementId)).Any())
                {
                    return -1;
                }

                FormElements formElement = (from c in automationApiDb.FormElements
                                            where c.FormElementId == formElementId
                                            select c).FirstOrDefault();

                formElement.EditEnDate = formElementsVM.EditEnDate.Value;
                formElement.EditTime = formElementsVM.EditTime;
                formElement.UserIdEditor = formElementsVM.UserIdEditor;
                formElement.FormId = formId;
                formElement.FormElementTitle = formElementTitle;
                formElement.ElementTypeId = elementTypeId;
                formElement.IsRequired = isRequired;
                formElement.DefaultValue = defaultValue;
                formElement.FormElementOrder = formElementOrder;
                formElement.FormElementSize = formElementSize;

                formElement.IsActivated = formElementsVM.IsActivated;
                formElement.IsDeleted = formElementsVM.IsDeleted;
                automationApiDb.Entry<FormElements>(formElement).State = EntityState.Modified;
                automationApiDb.SaveChanges();

                formElementsVM.UserIdCreator = formElement.UserIdCreator.Value;

                #region rewrite inside module

                //long userIdCreator = FormElementsVM.UserIdCreator.Value;
                //if (FormElementsVM.UserIdCreator.HasValue)
                //{
                //    var user = automationApiDb.Users.FirstOrDefault(u => u.UserId.Equals(userIdCreator));
                //    var userDetails = automationApiDb.UsersProfile.FirstOrDefault(up => up.UserId.Equals(userIdCreator));
                //    FormElementsVM.UserCreatorName = user.UserName;

                //    if (!string.IsNullOrEmpty(userDetails.Name))
                //        FormElementsVM.UserCreatorName += " - " + userDetails.Name;

                //    if (!string.IsNullOrEmpty(userDetails.Family))
                //        FormElementsVM.UserCreatorName += " - " + userDetails.Family;
                //}

                #endregion

                return formElement.FormElementId;
                //}
            }
            catch (Exception exc)
            { }
            return 0;
        }

        public bool ToggleActivationFormElements(int formElementId, long userId, List<long> childsUsersIds)
        {
            try
            {
                //List<long> childsUsersIds = new List<long>();
                //childsUsersIds = GetChildUserIds(ref childsUsersIds, userId).Distinct().ToList();
                var formElement = (from c in automationApiDb.FormElements
                                   where c.FormElementId == formElementId &&
                                   childsUsersIds.Contains(c.UserIdCreator.Value)
                                   select c).FirstOrDefault();

                if (formElement != null)
                {
                    formElement.IsActivated = !formElement.IsActivated;
                    formElement.EditEnDate = DateTime.Now;
                    formElement.EditTime = PersianDate.TimeNow;
                    formElement.UserIdEditor = userId;
                    automationApiDb.Entry<FormElements>(formElement).State = EntityState.Modified;
                    automationApiDb.SaveChanges();
                    return true;
                }
            }
            catch (Exception exc)
            { }
            return false;
        }

        public bool TemporaryDeleteFormElements(int formElementId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                //List<long> childsUsersIds = new List<long>();
                //childsUsersIds = GetChildUserIds(ref childsUsersIds, userId).Distinct().ToList();
                var formElement = (from c in automationApiDb.FormElements
                                   where c.FormElementId == formElementId &&
                                   childsUsersIds.Contains(c.UserIdCreator.Value)
                                   select c).FirstOrDefault();

                if (formElement != null)
                {
                    formElement.IsDeleted = !formElement.IsDeleted;
                    formElement.RemoveEnDate = DateTime.Now;
                    formElement.RemoveTime = PersianDate.TimeNow;
                    formElement.UserIdRemover = userId;
                    automationApiDb.Entry<FormElements>(formElement).State = EntityState.Modified;
                    automationApiDb.SaveChanges();
                    return true;
                }
            }
            catch (Exception exc)
            { }
            return false;
        }

        public bool CompleteDeleteFormElements(int formElementId,
            //long userId,
            List<long> childsUsersIds)
        {
            try
            {
                //List<long> childsUsersIds = new List<long>();
                //childsUsersIds = GetChildUserIds(ref childsUsersIds, userId).Distinct().ToList();
                var formElement = (from c in automationApiDb.FormElements
                                   where c.FormElementId == formElementId &&
                                   childsUsersIds.Contains(c.UserIdCreator.Value)
                                   select c).FirstOrDefault();

                if (formElement != null)
                {
                    automationApiDb.FormElements.Remove(formElement);
                    automationApiDb.SaveChanges();
                    return true;
                }
            }
            catch (Exception exc)
            { }
            return false;
        }

        #endregion

        #region Methods For Work With FormElementOptions

        public List<FormElementOptionsVM> GetListOfFormElementOptions(
            int jtStartIndex,
            int jtPageSize,
            ref int listCount,
            int formElementId = 0)
        {
            List<FormElementOptionsVM> formElementOptionsVMList = new List<FormElementOptionsVM>();

            var list = automationApiDb.FormElementOptions.Where(c => c.IsDeleted.Value.Equals(false) && c.IsActivated.Value.Equals(true))
                    .AsQueryable();

            if (formElementId > 0)
                list = list.Where(l => l.FormElementId.Equals(formElementId));

            try
            {
                listCount = list.Count();

                if (listCount > jtPageSize)
                {
                    formElementOptionsVMList = _mapper.Map<List<FormElementOptions>, List<FormElementOptionsVM>>(list.OrderByDescending(s => s.FormElementOptionId)
                             .Skip(jtStartIndex).Take(jtPageSize).ToList());

                }
                else
                {
                    formElementOptionsVMList = _mapper.Map<List<FormElementOptions>,
                        List<FormElementOptionsVM>>(list.OrderByDescending(s => s.FormElementOptionId).ToList());
                }
            }
            catch (Exception exc)
            { }

            return formElementOptionsVMList;
        }

        public List<FormElementOptionsVM> GetAllFormElementOptionsList(ref int listCount,
            int formElementId = 0)
        {
            List<FormElementOptionsVM> formElementOptionsVMList = new List<FormElementOptionsVM>();

            try
            {
                var list = automationApiDb.FormElementOptions.Where(c => c.IsDeleted.Value.Equals(false) && c.IsActivated.Value.Equals(true))
                        .AsQueryable();

                if (formElementId > 0)
                    list = list.Where(l => l.FormElementId.Equals(formElementId));

                listCount = list.Count();

                formElementOptionsVMList = _mapper.Map<List<FormElementOptions>,
                    List<FormElementOptionsVM>>(list.OrderByDescending(s => s.FormElementOptionId).ToList());
            }
            catch (Exception exc)
            { }

            return formElementOptionsVMList;
        }

        public int AddToFormElementOptions(FormElementOptionsVM formElementOptionsVM)
        {
            try
            {
                if (!automationApiDb.FormElementOptions.
                    Where(p => p.FormElementOptionText.Equals(formElementOptionsVM.FormElementOptionText) &&
                        p.FormElementId.Equals(formElementOptionsVM.FormElementId)).Any())
                {
                    FormElementOptions formElementOptions = _mapper.Map<FormElementOptionsVM, FormElementOptions>(formElementOptionsVM);
                    automationApiDb.FormElementOptions.Add(formElementOptions);
                    automationApiDb.SaveChanges();
                    return formElementOptions.FormElementOptionId;
                }
                else
                    return -1;
                //}
            }
            catch (Exception exc)
            { }
            return 0;
        }

        public FormElementOptionsVM GetFormElementOptionWithFormElementOptionId(int formElementOptionId)
        {
            FormElementOptionsVM formElementOptionsVM = new FormElementOptionsVM();

            try
            {
                formElementOptionsVM = _mapper.Map<FormElementOptions,
                    FormElementOptionsVM>(automationApiDb.FormElementOptions
                    .Where(e => e.FormElementId.Equals(formElementOptionId)).FirstOrDefault());
            }
            catch (Exception exc)
            { }

            return formElementOptionsVM;
        }

        public int UpdateFormElementOptions(ref FormElementOptionsVM formElementOptionsVM)
        {
            try
            {
                int formElementOptionId = formElementOptionsVM.FormElementOptionId;
                int formElementId = formElementOptionsVM.FormElementId;
                int formElementOptionValue = formElementOptionsVM.FormElementOptionValue;
                string formElementOptionText = formElementOptionsVM.FormElementOptionText;

                if (automationApiDb.FormElementOptions.Where(p => p.FormElementOptionText.Equals(formElementOptionText) &&
                    p.FormElementId.Equals(formElementId) &&
                    !p.FormElementOptionId.Equals(formElementOptionId)).Any())
                {
                    return -1;
                }

                FormElementOptions formElementOptions = (from c in automationApiDb.FormElementOptions
                                                         where c.FormElementOptionId == formElementOptionId
                                                         select c).FirstOrDefault();

                formElementOptions.EditEnDate = formElementOptionsVM.EditEnDate.Value;
                formElementOptions.EditTime = formElementOptionsVM.EditTime;
                formElementOptions.UserIdEditor = formElementOptionsVM.UserIdEditor;
                formElementOptions.FormElementId = formElementId;
                formElementOptions.FormElementOptionValue = formElementOptionValue;
                formElementOptions.FormElementOptionText = formElementOptionText;

                formElementOptions.IsActivated = formElementOptionsVM.IsActivated;
                formElementOptions.IsDeleted = formElementOptionsVM.IsDeleted;
                automationApiDb.Entry<FormElementOptions>(formElementOptions).State = EntityState.Modified;
                automationApiDb.SaveChanges();

                formElementOptionsVM.UserIdCreator = formElementOptions.UserIdCreator.Value;

                #region rewrite inside module

                //long userIdCreator = FeaturesVM.UserIdCreator.Value;
                //if (FeaturesVM.UserIdCreator.HasValue)
                //{
                //    var user = automationApiDb.Users.FirstOrDefault(u => u.UserId.Equals(userIdCreator));
                //    var userDetails = automationApiDb.UsersProfile.FirstOrDefault(up => up.UserId.Equals(userIdCreator));
                //    FeaturesVM.UserCreatorName = user.UserName;

                //    if (!string.IsNullOrEmpty(userDetails.Name))
                //        FeaturesVM.UserCreatorName += " - " + userDetails.Name;

                //    if (!string.IsNullOrEmpty(userDetails.Family))
                //        FeaturesVM.UserCreatorName += " - " + userDetails.Family;
                //}

                #endregion

                return formElementOptions.FormElementOptionId;
                //}
            }
            catch (Exception exc)
            { }
            return 0;
        }

        public bool ToggleActivationFormElementOptions(int formElementOptionId, long userId)
        {
            try
            {
                //List<long> childsUsersIds = new List<long>();
                //childsUsersIds = GetChildUserIds(ref childsUsersIds, userId).Distinct().ToList();
                var formElementOptions = (from c in automationApiDb.FormElementOptions
                                          where c.FormElementOptionId == formElementOptionId
                                          select c).FirstOrDefault();

                if (formElementOptions != null)
                {
                    formElementOptions.IsActivated = !formElementOptions.IsActivated;
                    formElementOptions.EditEnDate = DateTime.Now;
                    formElementOptions.EditTime = PersianDate.TimeNow;
                    formElementOptions.UserIdEditor = userId;
                    automationApiDb.Entry<FormElementOptions>(formElementOptions).State = EntityState.Modified;
                    automationApiDb.SaveChanges();
                    return true;
                }
            }
            catch (Exception exc)
            { }
            return false;
        }

        public bool TemporaryDeleteFormElementOptions(int formElementOptionId, long userId)
        {
            try
            {
                //List<long> childsUsersIds = new List<long>();
                //childsUsersIds = GetChildUserIds(ref childsUsersIds, userId).Distinct().ToList();
                var formElementOptions = (from c in automationApiDb.FormElementOptions
                                          where c.FormElementOptionId == formElementOptionId
                                          select c).FirstOrDefault();

                if (formElementOptions != null)
                {
                    formElementOptions.IsDeleted = !formElementOptions.IsDeleted;
                    formElementOptions.EditEnDate = DateTime.Now;
                    formElementOptions.EditTime = PersianDate.TimeNow;
                    formElementOptions.UserIdEditor = userId;
                    automationApiDb.Entry<FormElementOptions>(formElementOptions).State = EntityState.Modified;
                    automationApiDb.SaveChanges();
                    return true;
                }
            }
            catch (Exception exc)
            { }
            return false;
        }

        public bool CompleteDeleteFormElementOptions(int formElementOptionId)
        {
            try
            {
                //List<long> childsUsersIds = new List<long>();
                //childsUsersIds = GetChildUserIds(ref childsUsersIds, userId).Distinct().ToList();
                var formElementOptions = (from c in automationApiDb.FormElementOptions
                                          where c.FormElementOptionId == formElementOptionId
                                          select c).FirstOrDefault();

                if (formElementOptions != null)
                {
                    automationApiDb.FormElementOptions.Remove(formElementOptions);
                    automationApiDb.SaveChanges();
                    return true;
                }
            }
            catch (Exception exc)
            { }
            return false;
        }

        #endregion

        #region Methods For Work With MyCompanies

        public List<MyCompaniesVM> GetMyCompaniesList(int jtStartIndex,
            int jtPageSize,
            ref int listCount,
            string jtSorting,
            //long userId = 0,
            List<long> childsUsersIds,
            string Lang,
            string Address,
            string CommercialCode,
            string MyCompanyName,
            string Phones,
            string MyCompanyRealName,
            string PostalCode,
            string Faxes,
            string RegisterNumber,
            string NationalCode,
            long? CityId,
            long? StateId)
        {
            List<MyCompaniesVM> myCompaniesVMList = new List<MyCompaniesVM>();
            try
            {
                //List<long> childsUsersIds = new List<long>();
                //userIds = /*GetChildUserIds(ref userIds, userId)*/base.GetChildUserIdsWithStoredProcedure(userId);

                if (string.IsNullOrEmpty(Address) &&
                    string.IsNullOrEmpty(CommercialCode) &&
                    string.IsNullOrEmpty(MyCompanyName) &&
                    string.IsNullOrEmpty(Phones) &&
                    string.IsNullOrEmpty(MyCompanyRealName) &&
                    string.IsNullOrEmpty(PostalCode) &&
                    string.IsNullOrEmpty(Faxes) &&
                    string.IsNullOrEmpty(RegisterNumber) &&
                    string.IsNullOrEmpty(NationalCode) &&
                    CityId == null && StateId == null &&
                    string.IsNullOrEmpty(Lang))
                {
                    if (string.IsNullOrEmpty(jtSorting))
                    {
                        listCount = automationApiDb.MyCompanies.Count();
                        if (listCount > jtPageSize)
                        {
                            myCompaniesVMList = _mapper.Map<List<MyCompanies>,
                                List<MyCompaniesVM>>(automationApiDb.MyCompanies.Where(c => childsUsersIds.Contains(c.UserIdCreator.Value))
                                    .OrderByDescending(s => s.MyCompanyId)
                                    .Skip(jtStartIndex).Take(jtPageSize).ToList());
                        }
                        else
                        {
                            myCompaniesVMList = _mapper.Map<List<MyCompanies>,
                                List<MyCompaniesVM>>(automationApiDb.MyCompanies.Where(c => childsUsersIds.Contains(c.UserIdCreator.Value))
                                    .OrderByDescending(s => s.MyCompanyId).ToList());
                        }
                    }
                    else
                    {
                        var list = automationApiDb.MyCompanies.Where(c => childsUsersIds.Contains(c.UserIdCreator.Value)).AsQueryable();
                        listCount = list.Count();
                        switch (jtSorting)
                        {
                            case "MyCompanyName ASC":
                                list = list.OrderBy(l => l.MyCompanyName);
                                break;
                            case "MyCompanyName DESC":
                                list = list.OrderByDescending(l => l.MyCompanyName);
                                break;
                        }

                        if (listCount > jtPageSize)
                        {
                            myCompaniesVMList = _mapper.Map<List<MyCompanies>,
                                List<MyCompaniesVM>>(list.Skip(jtStartIndex).Take(jtPageSize).ToList());
                        }
                        else
                        {
                            myCompaniesVMList = _mapper.Map<List<MyCompanies>,
                                List<MyCompaniesVM>>(list.ToList());
                        }
                    }
                }
                else
                {
                    var list = automationApiDb.MyCompanies.Where(c => childsUsersIds.Contains(c.UserIdCreator.Value)).OrderByDescending(x => x.MyCompanyId).AsQueryable();

                    //if (!string.IsNullOrEmpty(Lang))
                    //    list = list.Where(l => l.Lang.Equals(Lang));

                    if (!string.IsNullOrEmpty(Address))
                        list = list.Where(d => d.Address.Contains(Address));

                    if (!string.IsNullOrEmpty(CommercialCode))
                        list = list.Where(d => d.CommercialCode1.Contains(CommercialCode) || d.CommercialCode2.Contains(CommercialCode) ||
                                               d.RegisterNumber.Contains(CommercialCode) || d.PostalCode.Contains(CommercialCode));

                    if (!string.IsNullOrEmpty(MyCompanyName))
                        list = list.Where(d => d.MyCompanyName.Contains(MyCompanyName) || d.MyCompanyRealName.Contains(MyCompanyName));

                    if (!string.IsNullOrEmpty(Phones))
                        list = list.Where(d => d.Phones.Contains(Phones) || d.Faxes.Contains(Phones));

                    if (!string.IsNullOrEmpty(MyCompanyRealName))
                        list = list.Where(d => d.MyCompanyRealName.Contains(MyCompanyRealName));

                    if (!string.IsNullOrEmpty(PostalCode))
                        list = list.Where(d => d.PostalCode.Contains(PostalCode));

                    if (!string.IsNullOrEmpty(Faxes))
                        list = list.Where(d => d.Faxes.Contains(Faxes));

                    if (!string.IsNullOrEmpty(RegisterNumber))
                        list = list.Where(d => d.RegisterNumber.Contains(RegisterNumber));

                    if (!string.IsNullOrEmpty(NationalCode))
                        list = list.Where(d => d.NationalCode.Contains(NationalCode));

                    if (StateId > 0)
                        list = list.Where(d => d.StateId.Value == StateId);

                    if (CityId > 0)
                        list = list.Where(d => d.CityId.Value == CityId);

                    listCount = list.Count();
                    if (string.IsNullOrEmpty(jtSorting))
                    {
                        if (listCount > jtPageSize)
                        {
                            myCompaniesVMList = _mapper.Map<List<MyCompanies>,
                                List<MyCompaniesVM>>(list.Skip(jtStartIndex).Take(jtPageSize).ToList());
                        }
                        else
                        {
                            myCompaniesVMList = _mapper.Map<List<MyCompanies>,
                                List<MyCompaniesVM>>(list.ToList());
                        }
                    }
                    else
                    {
                        switch (jtSorting)
                        {
                            case "MyCompanyName ASC":
                                list = list.OrderBy(l => l.MyCompanyName);
                                break;
                            case "MyCompanyName DESC":
                                list = list.OrderByDescending(l => l.MyCompanyName);
                                break;
                        }

                        if (listCount > jtPageSize)
                        {
                            myCompaniesVMList = _mapper.Map<List<MyCompanies>,
                                List<MyCompaniesVM>>(list.Skip(jtStartIndex).Take(jtPageSize).ToList());
                        }
                        else
                        {
                            myCompaniesVMList = _mapper.Map<List<MyCompanies>,
                                List<MyCompaniesVM>>(list.ToList());
                        }
                    }
                }

                #region rewrite inside module

                //if (myCompaniesVMList.Count > 0)
                //{
                //    foreach (var myCompaniesVM in myCompaniesVMList)
                //    {
                //        string domainName = "";
                //        string countryName = "";
                //        string stateName = "";
                //        string cityName = "";
                //        try
                //        {
                //            if (myCompaniesVM.DomainSettingId.HasValue)
                //                domainName = automationApiDb.DomainsSettings.Where(d =>
                //                                d.DomainSettingId.Equals(myCompaniesVM.DomainSettingId.Value))
                //                                .FirstOrDefault().DomainName;

                //            if (myCompaniesVM.CountryId.HasValue)
                //                countryName = automationApiDb.Countries.Where(d =>
                //                                d.CountryId.Equals(myCompaniesVM.CountryId.Value))
                //                                .FirstOrDefault().CountryName;

                //            if (myCompaniesVM.StateId.HasValue)
                //                stateName = automationApiDb.States.Where(d =>
                //                                d.StateId.Equals(myCompaniesVM.StateId.Value))
                //                                .FirstOrDefault().StateName;

                //            if (myCompaniesVM.CityId.HasValue)
                //                cityName = automationApiDb.Cities.Where(d =>
                //                                d.CityId.Equals(myCompaniesVM.CityId.Value))
                //                                .FirstOrDefault().CityName;
                //        }
                //        catch (Exception exc)
                //        { }
                //        myCompaniesVM.DomainName = domainName;
                //        myCompaniesVM.CountryName = countryName;
                //        myCompaniesVM.StateName = stateName;
                //        myCompaniesVM.CityName = cityName;
                //    }
                //}

                #endregion
            }
            catch (Exception exc)
            { }
            return myCompaniesVMList;
        }

        public List<MyCompaniesVM> GetAllMyCompaniesList(List<long> childsUsersIds)
        {
            List<MyCompaniesVM> myCompaniesVMList = new List<MyCompaniesVM>();

            try
            {
                if (automationApiDb.MyCompanies.Where(c => childsUsersIds.Contains(c.UserIdCreator.Value) && c.IsDeleted.Value.Equals(false) && c.IsActivated.Value.Equals(true)).Any())
                {
                    myCompaniesVMList = _mapper.Map<List<MyCompanies>,
                        List<MyCompaniesVM>>(automationApiDb.MyCompanies.Where(c => childsUsersIds.Contains(c.UserIdCreator.Value) && c.IsDeleted.Value.Equals(false) && c.IsActivated.Value.Equals(true)).ToList());
                }
            }
            catch (Exception exc)
            { }

            return myCompaniesVMList;
        }

        public bool ExistCompanyWithCompanyName(List<long> childsUsersIds, MyCompaniesVM myCompaniesVM)
        {
            try
            {
                //List<long> childsUsersIds = new List<long>();
                //childsUsersIds = GetChildUserIds(ref childsUsersIds, myCompaniesVM.UserIdCreator.Value).Distinct().ToList();

                //List<long> childsUsersIds = base.GetChildUserIdsWithStoredProcedure(myCompaniesVM.UserIdCreator.Value);

                if (!automationApiDb.MyCompanies.Any(c => c.MyCompanyName.Equals(myCompaniesVM.MyCompanyName) &&
                        !childsUsersIds.Contains(c.UserIdCreator.Value)))
                    return true;
            }
            catch (Exception exc)
            { }
            return false;
        }

        public int AddToMyCompanies(MyCompaniesVM myCompaniesVM)
        {
            try
            {
                if (myCompaniesVM.CentralOffice)
                {
                    if (automationApiDb.MyCompanies.Any(c => c.CentralOffice.Equals(true)))
                    {
                        var myCompanies = automationApiDb.MyCompanies.Where(c => c.CentralOffice.Equals(true)).ToList();

                        foreach (var tmpMyCompany in myCompanies)
                        {
                            tmpMyCompany.CentralOffice = false;
                            tmpMyCompany.EditEnDate = DateTime.Now;
                            tmpMyCompany.EditTime = PersianDate.TimeNow;
                            tmpMyCompany.UserIdEditor = myCompaniesVM.UserIdCreator.Value;
                        }

                        if (myCompanies.Count > 0)
                            automationApiDb.SaveChanges();
                    }
                }

                #region rewrite inside module

                //if (myCompaniesVM.DomainSettingId.HasValue)
                //{
                //    var domain = automationApiDb.DomainsSettings.FirstOrDefault(d => d.DomainSettingId.Equals(myCompaniesVM.DomainSettingId.Value));
                //    domain.IsFree = false;
                //    automationApiDb.SaveChanges();
                //}

                #endregion

                MyCompanies myCompany = _mapper.Map<MyCompaniesVM, MyCompanies>(myCompaniesVM);
                automationApiDb.MyCompanies.Add(myCompany);
                automationApiDb.SaveChanges();
                return myCompany.MyCompanyId;
            }
            catch (Exception exc)
            { }
            return 0;
        }

        public int AddToMyCompanies(MyCompaniesVM myCompaniesVM, List<long> childsUsersIds)
        {
            try
            {
                if (myCompaniesVM.CentralOffice)
                {
                    if (automationApiDb.MyCompanies.Where(c => childsUsersIds.Contains(c.UserIdCreator.Value) &&
                        c.UserIdCreator.Value.Equals(myCompaniesVM.UserIdCreator.Value)).Where(c => c.CentralOffice.Equals(true)).Any())
                    {
                        var myCompanies = automationApiDb.MyCompanies.Where(c => childsUsersIds.Contains(c.UserIdCreator.Value) &&
                        c.UserIdCreator.Value.Equals(myCompaniesVM.UserIdCreator.Value)).Where(c => c.CentralOffice.Equals(true)).ToList();

                        foreach (var tmpMyCompany in myCompanies)
                        {
                            tmpMyCompany.CentralOffice = false;
                            tmpMyCompany.EditEnDate = DateTime.Now;
                            tmpMyCompany.EditTime = PersianDate.TimeNow;
                            tmpMyCompany.UserIdEditor = myCompaniesVM.UserIdCreator.Value;
                        }

                        if (myCompanies.Count > 0)
                            automationApiDb.SaveChanges();
                    }
                }

                #region rewrite inside module

                //if (myCompaniesVM.DomainSettingId.HasValue)
                //{
                //    var domain = automationApiDb.DomainsSettings.FirstOrDefault(d => d.DomainSettingId.Equals(myCompaniesVM.DomainSettingId.Value));
                //    domain.IsFree = false;
                //    automationApiDb.SaveChanges();
                //}

                #endregion

                MyCompanies myCompany = _mapper.Map<MyCompaniesVM, MyCompanies>(myCompaniesVM);
                automationApiDb.MyCompanies.Add(myCompany);
                automationApiDb.SaveChanges();
                return myCompany.MyCompanyId;
            }
            catch (Exception exc)
            { }
            return 0;
        }

        public MyCompaniesVM GetMyCompaniesWithMyCompanyId(List<long> childsUsersIds, int myCompanyId)
        {
            try
            {
                //List<long> childsUsersIds = new List<long>();
                //childsUsersIds = GetChildUserIds(ref childsUsersIds, userId).Distinct().ToList();

                if (automationApiDb.MyCompanies.Any(c => c.MyCompanyId.Equals(myCompanyId) && childsUsersIds.Contains(c.UserIdCreator.Value)))
                    return _mapper.Map<MyCompanies, MyCompaniesVM>(automationApiDb.MyCompanies.FirstOrDefault(c => c.MyCompanyId.Equals(myCompanyId) &&
                        childsUsersIds.Contains(c.UserIdCreator.Value)));
            }
            catch (Exception exc)
            { }
            return new MyCompaniesVM();
        }

        public string GetCompanyPictures(long userId,
            int myCompanyId,
            List<long> childsUsersIds,
            ref string companyMapPicture,
            ref string companyLogoPicture,
            ref string waterMarkImagePicture)
        {
            try
            {
                //List<long> childsUsersIds = new List<long>();
                //childsUsersIds = GetChildUserIds(ref childsUsersIds, userId).Distinct().ToList();

                if (automationApiDb.MyCompanies.Any(c => c.MyCompanyId.Equals(myCompanyId) && childsUsersIds.Contains(c.UserIdCreator.Value)))
                {
                    var myCompany = automationApiDb.MyCompanies.FirstOrDefault(c => c.MyCompanyId.Equals(myCompanyId) &&
                        childsUsersIds.Contains(c.UserIdCreator.Value));

                    companyMapPicture = myCompany.CompanyMap;
                    companyLogoPicture = myCompany.CompanyLogo;
                    waterMarkImagePicture = myCompany.WaterMarkImage;
                }
            }
            catch (Exception exc)
            { }
            return "";
        }

        public bool UpdateCompanyPictures(long userId, int myCompanyId, string companyMapPicture, string companyLogoPicture, string waterMarkImagePicture)
        {
            try
            {
                if (automationApiDb.MyCompanies.Any(c => c.MyCompanyId.Equals(myCompanyId)))
                {
                    var myCompany = automationApiDb.MyCompanies.FirstOrDefault(c => c.MyCompanyId.Equals(myCompanyId));

                    if (!string.IsNullOrEmpty(companyMapPicture))
                        myCompany.CompanyMap = companyMapPicture;

                    if (!string.IsNullOrEmpty(companyLogoPicture))
                        myCompany.CompanyLogo = companyLogoPicture;

                    if (!string.IsNullOrEmpty(waterMarkImagePicture))
                        myCompany.WaterMarkImage = waterMarkImagePicture;

                    if ((!string.IsNullOrEmpty(companyMapPicture)) ||
                        (!string.IsNullOrEmpty(companyLogoPicture)) ||
                        (!string.IsNullOrEmpty(waterMarkImagePicture)))
                    {
                        myCompany.UserIdEditor = userId;
                        myCompany.EditEnDate = DateTime.Now;
                        myCompany.EditTime = PersianDate.TimeNow;
                        automationApiDb.SaveChanges();
                        return true;
                    }
                }
            }
            catch (Exception exc)
            { }
            return false;
        }

        public bool ToggleActivationMyCompanies(int myCompanyId, long userId, List<long> childsUsersIds)
        {
            try
            {
                if (automationApiDb.MyCompanies.Where(c => c.MyCompanyId.Equals(myCompanyId) &&
                        childsUsersIds.Contains(c.UserIdCreator.Value)).Any())
                {
                    var myCompany = automationApiDb.MyCompanies.FirstOrDefault(c => c.MyCompanyId.Equals(myCompanyId));
                    myCompany.IsActivated = !myCompany.IsActivated;
                    myCompany.EditEnDate = DateTime.Now;
                    myCompany.EditTime = PersianDate.TimeNow;
                    myCompany.UserIdEditor = userId;
                    automationApiDb.SaveChanges();
                    return true;
                }
            }
            catch (Exception exc)
            { }
            return false;
        }

        public bool TemporaryDeleteMyCompanies(int myCompanyId, long userId, List<long> childsUsersIds)
        {
            try
            {
                if (automationApiDb.MyCompanies.Where(c => c.MyCompanyId.Equals(myCompanyId) &&
                        childsUsersIds.Contains(c.UserIdCreator.Value)).Any())
                {
                    var myCompany = automationApiDb.MyCompanies.FirstOrDefault(c => c.MyCompanyId.Equals(myCompanyId));
                    myCompany.IsDeleted = !myCompany.IsDeleted;
                    myCompany.EditEnDate = DateTime.Now;
                    myCompany.EditTime = PersianDate.TimeNow;
                    myCompany.UserIdEditor = userId;
                    automationApiDb.SaveChanges();
                    return true;
                }
            }
            catch (Exception exc)
            { }
            return false;
        }

        //public bool ChangeCentralOffice(int myCompanyId, long userId)
        //{
        //    try
        //    {
        //        if (automationApiDb.MyCompanies.Any(c => c.MyCompanyId.Equals(myCompanyId)))
        //        {
        //            if (automationApiDb.MyCompanies.Any(c => !c.MyCompanyId.Equals(myCompanyId) && c.CentralOffice.Equals(true)))
        //            {
        //                var myCompanies = automationApiDb.MyCompanies.Where(c => !c.MyCompanyId.Equals(myCompanyId) &&
        //                        c.CentralOffice.Equals(true)).ToList();

        //                foreach (var tmpMyCompany in myCompanies)
        //                {
        //                    tmpMyCompany.CentralOffice = false;
        //                    tmpMyCompany.EditEnDate = DateTime.Now;
        //                    tmpMyCompany.EditTime = PersianDate.TimeNow;
        //                    tmpMyCompany.UserIdEditor = userId;
        //                }

        //                if (myCompanies.Count > 0)
        //                    automationApiDb.SaveChanges();
        //            }

        //            var myCompany = automationApiDb.MyCompanies.FirstOrDefault(c => c.MyCompanyId.Equals(myCompanyId));
        //            myCompany.CentralOffice = true;
        //            myCompany.EditEnDate = DateTime.Now;
        //            myCompany.EditTime = PersianDate.TimeNow;
        //            myCompany.UserIdEditor = userId;
        //            automationApiDb.SaveChanges();
        //            return true;
        //        }
        //    }
        //    catch (Exception exc)
        //    { }
        //    return false;
        //}

        public bool CompleteDeleteMyCompanies(int myCompanyId, List<long> childsUsersIds, ref string returnMessage)
        {
            returnMessage = "";

            try
            {
                if (automationApiDb.MyCompanies.Where(c => c.MyCompanyId.Equals(myCompanyId) && childsUsersIds.Contains(c.UserIdCreator.Value)).Any())
                {
                    var myCompany = automationApiDb.MyCompanies.FirstOrDefault(c => c.MyCompanyId.Equals(myCompanyId));
                    automationApiDb.MyCompanies.Remove(myCompany);
                    automationApiDb.SaveChanges();
                    return true;
                }
            }
            catch (Exception exc)
            { }
            return false;
        }

        public bool UpdateMyCompany(MyCompaniesVM myCompaniesVM, ref string returnMessage)
        {
            try
            {
                if (!automationApiDb.MyCompanies.Any(c => c.MyCompanyName.Equals(myCompaniesVM.MyCompanyName) &&
                     !c.MyCompanyId.Equals(myCompaniesVM.MyCompanyId)))
                {
                    #region rewrite inside module

                    //if (myCompaniesVM.DomainSettingId.HasValue)
                    //{
                    //    var domain = automationApiDb.DomainsSettings.FirstOrDefault(d => d.DomainSettingId.Equals(myCompaniesVM.DomainSettingId.Value));
                    //    domain.IsFree = false;
                    //    automationApiDb.SaveChanges();
                    //}

                    #endregion

                    #region Update MyCompany

                    var myCompany = automationApiDb.MyCompanies.Where(c => c.MyCompanyId.Equals(myCompaniesVM.MyCompanyId)).FirstOrDefault();

                    myCompany.Address = myCompaniesVM.Address;
                    myCompany.CityId = myCompaniesVM.CityId.HasValue ? myCompaniesVM.CityId.Value : (long?)null;
                    myCompany.CommercialCode1 = myCompaniesVM.CommercialCode1;
                    myCompany.CommercialCode2 = myCompaniesVM.CommercialCode2;
                    myCompany.CountryId = myCompaniesVM.CountryId.HasValue ? myCompaniesVM.CountryId.Value : (int?)null;
                    myCompany.DomainSettingId = myCompaniesVM.DomainSettingId;
                    //myCompany.MyCompanyDirectorId = myCompaniesVM.MyCompanyDirectorId;
                    myCompany.EditEnDate = myCompaniesVM.EditEnDate.Value;
                    myCompany.EditTime = myCompaniesVM.EditTime;
                    myCompany.Faxes = myCompaniesVM.Faxes;
                    myCompany.Footer = myCompaniesVM.Footer;
                    myCompany.Header = myCompaniesVM.Header;
                    myCompany.IsActivated = myCompaniesVM.IsActivated;
                    myCompany.CentralOffice = myCompaniesVM.CentralOffice;
                    myCompany.MyCompanyId = myCompaniesVM.MyCompanyId;
                    myCompany.MyCompanyName = myCompaniesVM.MyCompanyName;
                    myCompany.MyCompanyRealName = myCompaniesVM.MyCompanyRealName;
                    myCompany.Phones = myCompaniesVM.Phones;
                    myCompany.PositionX = myCompaniesVM.PositionX;
                    myCompany.PositionY = myCompaniesVM.PositionY;
                    myCompany.PostalCode = myCompaniesVM.PostalCode;
                    myCompany.RealFooter = myCompaniesVM.RealFooter;
                    myCompany.RealHeader = myCompaniesVM.RealHeader;
                    myCompany.RegisterNumber = myCompaniesVM.RegisterNumber;
                    myCompany.StateId = myCompaniesVM.StateId.HasValue ? myCompaniesVM.StateId.Value : (long?)null;
                    myCompany.UserIdEditor = myCompaniesVM.UserIdEditor.HasValue ? myCompaniesVM.UserIdEditor.Value : (long?)null;
                    myCompany.WaterMarkText = myCompaniesVM.WaterMarkText;

                    automationApiDb.SaveChanges();

                    #endregion

                    return true;
                }
                else
                {
                    returnMessage = "DuplicateCompanyName";
                    return false;
                }
            }
            catch (Exception exc)
            { }
            //returnMessage = "ErrorInEnteredValues";
            return false;
        }

        public bool UpdateMyCompany(ref MyCompaniesVM myCompaniesVM, List<long> childsUsersIds)
        {
            try
            {
                string myCompanyName = myCompaniesVM.MyCompanyName;
                int myCompanyId = myCompaniesVM.MyCompanyId;

                if (!automationApiDb.MyCompanies.Any(c => c.MyCompanyName.Equals(myCompanyName) && !c.MyCompanyId.Equals(myCompanyId)))
                {
                    #region rewrite inside module

                    //if (myCompaniesVM.DomainSettingId.HasValue)
                    //{
                    //    var domain = automationApiDb.DomainsSettings.FirstOrDefault(d => d.DomainSettingId.Equals(myCompaniesVM.DomainSettingId.Value));
                    //    domain.IsFree = false;
                    //    automationApiDb.SaveChanges();
                    //}

                    #endregion

                    if (myCompaniesVM.CentralOffice)
                    {
                        if (automationApiDb.MyCompanies.Any(c => c.CentralOffice.Equals(true)))
                        {
                            var myCompanies = automationApiDb.MyCompanies.Where(c => c.CentralOffice.Equals(true)).ToList();

                            foreach (var tmpMyCompany in myCompanies)
                            {
                                tmpMyCompany.CentralOffice = false;
                                tmpMyCompany.EditEnDate = DateTime.Now;
                                tmpMyCompany.EditTime = PersianDate.TimeNow;
                                tmpMyCompany.UserIdEditor = myCompaniesVM.UserIdCreator.Value;
                            }

                            if (myCompanies.Count > 0)
                                automationApiDb.SaveChanges();
                        }
                    }

                    #region Update MyCompany

                    var myCompany = automationApiDb.MyCompanies.Where(c => c.MyCompanyId.Equals(myCompanyId)).FirstOrDefault();

                    myCompany.Address = myCompaniesVM.Address;
                    myCompany.CityId = myCompaniesVM.CityId.HasValue ? myCompaniesVM.CityId.Value : (long?)null;
                    myCompany.CommercialCode1 = myCompaniesVM.CommercialCode1;
                    myCompany.CommercialCode2 = myCompaniesVM.CommercialCode2;
                    myCompany.CountryId = myCompaniesVM.CountryId.HasValue ? myCompaniesVM.CountryId.Value : (int?)null;
                    myCompany.DomainSettingId = myCompaniesVM.DomainSettingId;
                    myCompany.EditEnDate = myCompaniesVM.EditEnDate.Value;
                    myCompany.EditTime = myCompaniesVM.EditTime;
                    myCompany.Faxes = myCompaniesVM.Faxes;
                    myCompany.Footer = myCompaniesVM.Footer;
                    myCompany.Header = myCompaniesVM.Header;
                    myCompany.IsActivated = myCompaniesVM.IsActivated;
                    myCompany.CentralOffice = myCompaniesVM.CentralOffice;
                    myCompany.MyCompanyId = myCompaniesVM.MyCompanyId;
                    myCompany.MyCompanyName = myCompaniesVM.MyCompanyName;
                    myCompany.MyCompanyRealName = myCompaniesVM.MyCompanyRealName;
                    myCompany.Phones = myCompaniesVM.Phones;
                    myCompany.PositionX = myCompaniesVM.PositionX;
                    myCompany.PositionY = myCompaniesVM.PositionY;
                    myCompany.PostalCode = myCompaniesVM.PostalCode;
                    myCompany.RealFooter = myCompaniesVM.RealFooter;
                    myCompany.RealHeader = myCompaniesVM.RealHeader;
                    myCompany.RegisterNumber = myCompaniesVM.RegisterNumber;
                    myCompany.StateId = myCompaniesVM.StateId.HasValue ? myCompaniesVM.StateId.Value : (long?)null;
                    myCompany.UserIdEditor = myCompaniesVM.UserIdEditor.HasValue ? myCompaniesVM.UserIdEditor.Value : (long?)null;
                    myCompany.WaterMarkText = myCompaniesVM.WaterMarkText;

                    automationApiDb.SaveChanges();

                    #endregion

                    return true;
                }
            }
            catch (Exception exc)
            { }
            //returnMessage = "ErrorInEnteredValues";
            return false;
        }

        public bool CheckLimitationOfDefinedCompany(long userId,
            DomainLimitationsVM limitation,
            List<long> childsUsersIds
            /*int domainSettingId*/)
        {
            try
            {
                //var domainSetting = automationApiDb.DomainsSettings.FirstOrDefault(d => d.UserIdCreator.Value.Equals(userId));

                #region rewrite inside module

                //var domainSetting = automationApiDb.DomainsSettings.FirstOrDefault(d => d.DomainSettingId.Equals(domainSettingId));

                //var limitation = automationApiDb.DomainLimitations.Where(dl =>
                //        dl.DomainSettingId.Equals(domainSetting.DomainSettingId) && dl.ActionName.Equals("AddToMyCompanies")).FirstOrDefault();

                #endregion

                if (limitation != null)
                {
                    //List<long> childsUsersIds = new List<long>();
                    //userIds = /*GetChildUserIds(ref userIds, userId)*/base.GetChildUserIdsWithStoredProcedure(userId);

                    var numberOfDefinedCompanies = automationApiDb.MyCompanies.Where(c => childsUsersIds.Contains(c.UserIdCreator.Value)).Count();
                    if (int.Parse(limitation.Limit) > numberOfDefinedCompanies)
                        return true;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception exc)
            { }
            return false;
        }

        public List<MyCompaniesVM> GetAllMyCompaniesList(long userId, List<long> childsUsersIds)
        {
            List<MyCompaniesVM> myCompaniesVMList = new List<MyCompaniesVM>();

            try
            {
                //List<long> childsUsersIds = new List<long>();
                //userIds = /*GetChildUserIds(ref userIds, userId)*/base.GetChildUserIdsWithStoredProcedure(userId);

                myCompaniesVMList = _mapper.Map<List<MyCompanies>,
                    List<MyCompaniesVM>>(automationApiDb.MyCompanies.Where(c => childsUsersIds.Contains(c.UserIdCreator.Value))
                        .OrderByDescending(s => s.MyCompanyId).ToList());

                #region rewrite inside module

                //var userIdCreators = myCompaniesVMList.Where(i => i.UserIdCreator.HasValue).Select(u => u.UserIdCreator.Value).ToList();
                //var customUsers = (from u in automationApiDb.Users
                //                   join up in automationApiDb.UsersProfile
                //                   on u.UserId equals up.UserId
                //                   where userIdCreators.Contains(u.UserId)
                //                   select new CustomUsersVM
                //                   {
                //                       UserId = u.UserId,
                //                       Name = up.Name,
                //                       Family = up.Family,
                //                       DomainSettingId = u.DomainSettingId,
                //                       UserName = u.UserName
                //                   }).ToList();

                //foreach (var myCompaniesVM in myCompaniesVMList)
                //{
                //    if (myCompaniesVM.UserIdCreator.HasValue)
                //    {
                //        var customUser = customUsers.FirstOrDefault(c => c.UserId.Equals(myCompaniesVM.UserIdCreator.Value));
                //        if (customUser != null)
                //        {
                //            myCompaniesVM.UserCreatorName = customUser.UserName;

                //            if (!string.IsNullOrEmpty(customUser.Name))
                //                myCompaniesVM.UserCreatorName += " " + customUser.Name;

                //            if (!string.IsNullOrEmpty(customUser.Family))
                //                myCompaniesVM.UserCreatorName += " " + customUser.Family;
                //        }
                //    }
                //}

                #endregion
            }
            catch (Exception exc)
            { }

            return myCompaniesVMList;
        }

        public bool ToggleCentralOfficeCompanies(int myCompanyId, long userId)
        {
            try
            {
                if (automationApiDb.MyCompanies.Any(c => c.MyCompanyId.Equals(myCompanyId)))
                {
                    if (automationApiDb.MyCompanies.Any(c => !c.MyCompanyId.Equals(myCompanyId) && c.CentralOffice.Equals(true)))
                    {
                        var myCompanies = automationApiDb.MyCompanies.Where(c => !c.MyCompanyId.Equals(myCompanyId) &&
                                c.CentralOffice.Equals(true)).ToList();

                        foreach (var tmpMyCompany in myCompanies)
                        {
                            tmpMyCompany.CentralOffice = false;
                            tmpMyCompany.EditEnDate = DateTime.Now;
                            tmpMyCompany.EditTime = PersianDate.TimeNow;
                            tmpMyCompany.UserIdEditor = userId;
                        }

                        if (myCompanies.Count > 0)
                            automationApiDb.SaveChanges();
                    }

                    var myCompany = automationApiDb.MyCompanies.FirstOrDefault(c => c.MyCompanyId.Equals(myCompanyId));
                    myCompany.CentralOffice = true;
                    myCompany.EditEnDate = DateTime.Now;
                    myCompany.EditTime = PersianDate.TimeNow;
                    myCompany.UserIdEditor = userId;
                    automationApiDb.SaveChanges();
                    return true;
                }
            }
            catch (Exception exc)
            { }
            return false;
        }

        public bool ToggleHasLimitedCompanies(int myCompanyId, long userId)
        {
            try
            {
                if (automationApiDb.MyCompanies.Any(c => c.MyCompanyId.Equals(myCompanyId)))
                {
                    var myCompany = automationApiDb.MyCompanies.FirstOrDefault(c => c.MyCompanyId.Equals(myCompanyId));
                    myCompany.HasLimited = !myCompany.HasLimited;
                    myCompany.EditEnDate = DateTime.Now;
                    myCompany.EditTime = PersianDate.TimeNow;
                    myCompany.UserIdEditor = userId;
                    automationApiDb.SaveChanges();
                    return true;
                }
            }
            catch (Exception exc)
            { }
            return false;
        }

        public DomainsSettingsVM GetCompanyDomain(int myCompanyId)
        {
            try
            {
                #region rewrite inside module

                //var domainsSetting = (from i in automationApiDb.MyCompanies
                //                      join d in automationApiDb.DomainsSettings
                //                      on i.DomainSettingId equals d.DomainSettingId
                //                      where i.MyCompanyId.Equals(myCompanyId)
                //                      select d).FirstOrDefault();
                //if (domainsSetting != null)
                //    return _mapper.Map<DomainsSettings, DomainsSettingsVM>(domainsSetting);

                #endregion
            }
            catch (Exception exc)
            { }
            return null;
        }

        public GetMyCompaniesImagesVM GetMyCompaniesImages(List<long> childsUsersIds, int MyCompanyId)
        {
            GetMyCompaniesImagesVM getMyCompaniesImagesVM = new GetMyCompaniesImagesVM();

            try
            {
                if (automationApiDb.MyCompanies.Where(ed => childsUsersIds.Contains(ed.UserIdCreator.Value) && ed.MyCompanyId.Equals(MyCompanyId)).Any())
                {
                    var MyCompany = automationApiDb.MyCompanies.Where(ed => childsUsersIds.Contains(ed.UserIdCreator.Value) &&
                                                                        ed.MyCompanyId.Equals(MyCompanyId)).FirstOrDefault();

                    getMyCompaniesImagesVM.companyLogoPath = MyCompany.CompanyLogo;
                    getMyCompaniesImagesVM.waterMarkImagePath = MyCompany.WaterMarkImage;
                }
            }
            catch (Exception exc)
            { }

            return getMyCompaniesImagesVM;
        }

        #endregion

        #region Methods For Work With MyDepartments

        public List<MyDepartmentsVM> GetListOfMyDepartments(int jtStartIndex,
            int jtPageSize,
            ref int listCount,
            string jtSorting,
            List<long> childsUsersIds,
            long userId = 0,
            string myDepartmentNameSearch = null,
            int myCompanyIdSearch = 0)
        {
            List<MyDepartmentsVM> myDepartmentsVMList = new List<MyDepartmentsVM>();
            try
            {
                //List<long> childsUsersIds = new List<long>();
                //userIds = /*GetChildUserIds(ref userIds, userId)*/base.GetChildUserIdsWithStoredProcedure(userId);

                if (string.IsNullOrEmpty(myDepartmentNameSearch) && myCompanyIdSearch != 0)
                {
                    if (string.IsNullOrEmpty(jtSorting))
                    {
                        listCount = automationApiDb.MyDepartments.Where(c => childsUsersIds.Contains(c.UserIdCreator.Value)).Count();
                        if (listCount > jtPageSize)
                        {
                            myDepartmentsVMList = _mapper.Map<List<MyDepartments>,
                                List<MyDepartmentsVM>>(automationApiDb.MyDepartments.Where(c => childsUsersIds.Contains(c.UserIdCreator.Value) && c.IsDeleted.Value.Equals(false) && c.IsActivated.Value.Equals(true))
                                    .OrderByDescending(s => s.MyDepartmentId)
                                    .Skip(jtStartIndex).Take(jtPageSize).ToList());
                        }
                        else
                        {
                            myDepartmentsVMList = _mapper.Map<List<MyDepartments>,
                                List<MyDepartmentsVM>>(automationApiDb.MyDepartments.Where(c => childsUsersIds.Contains(c.UserIdCreator.Value) && c.IsDeleted.Value.Equals(false) && c.IsActivated.Value.Equals(true))
                                    .OrderByDescending(s => s.MyDepartmentId).ToList());
                        }
                    }
                    else
                    {
                        var list = automationApiDb.MyDepartments.Where(c => childsUsersIds.Contains(c.UserIdCreator.Value) && c.IsDeleted.Value.Equals(false) && c.IsActivated.Value.Equals(true)).AsQueryable();
                        listCount = list.Count();

                        switch (jtSorting)
                        {
                            case "MyDepartmentName ASC":
                                list = list.OrderBy(l => l.MyDepartmentName);
                                break;
                            case "MyDepartmentName DESC":
                                list = list.OrderByDescending(l => l.MyDepartmentName);
                                break;
                        }

                        if (listCount > jtPageSize)
                        {
                            myDepartmentsVMList = _mapper.Map<List<MyDepartments>,
                                List<MyDepartmentsVM>>(list.Skip(jtStartIndex).Take(jtPageSize).ToList());
                        }
                        else
                        {
                            myDepartmentsVMList = _mapper.Map<List<MyDepartments>,
                                List<MyDepartmentsVM>>(list.ToList());
                        }
                    }
                }
                else
                {
                    var list = automationApiDb.MyDepartments.Where(c => childsUsersIds.Contains(c.UserIdCreator.Value)).OrderByDescending(x => x.MyDepartmentId).AsQueryable();

                    if (!string.IsNullOrEmpty(myDepartmentNameSearch))
                        list = list.Where(d => d.MyDepartmentName.Contains(myDepartmentNameSearch) ||
                                    d.MyDepartmentRealName.Contains(myDepartmentNameSearch));

                    if (myCompanyIdSearch != 0)
                        list = list.Where(c => c.ParentId.Equals(myCompanyIdSearch) && c.ParentType.Equals("company"));

                    listCount = list.Count();
                    if (string.IsNullOrEmpty(jtSorting))
                    {
                        if (listCount > jtPageSize)
                        {
                            myDepartmentsVMList = _mapper.Map<List<MyDepartments>,
                                List<MyDepartmentsVM>>(list.Skip(jtStartIndex).Take(jtPageSize).ToList());
                        }
                        else
                        {
                            myDepartmentsVMList = _mapper.Map<List<MyDepartments>,
                                List<MyDepartmentsVM>>(list.ToList());
                        }
                    }
                    else
                    {
                        switch (jtSorting)
                        {
                            case "MyDepartmentName ASC":
                                list = list.OrderBy(l => l.MyDepartmentName);
                                break;
                            case "MyDepartmentName DESC":
                                list = list.OrderByDescending(l => l.MyDepartmentName);
                                break;
                        }

                        if (listCount > jtPageSize)
                        {
                            myDepartmentsVMList = _mapper.Map<List<MyDepartments>,
                                List<MyDepartmentsVM>>(list.Skip(jtStartIndex).Take(jtPageSize).ToList());
                        }
                        else
                        {
                            myDepartmentsVMList = _mapper.Map<List<MyDepartments>,
                                List<MyDepartmentsVM>>(list.ToList());
                        }
                    }
                }

                if (myDepartmentsVMList.Count > 0)
                {
                    #region rewrite inside module

                    //var usersProfile = automationApiDb.UsersProfile.Where(up => myDepartmentsVMList
                    //    .Select(d => d.MyDepartmentDirectorId).ToList().Contains(up.UserId)).ToList();

                    #endregion

                    var myCompanyIds = myDepartmentsVMList.Where(d => d.ParentType.Equals("company")).Select(d => d.ParentId).ToList();

                    var myCompanies = automationApiDb.MyCompanies.Where(c => myCompanyIds.Contains(c.MyCompanyId)).ToList();

                    //var companies = (from c in automationApiDb.MyCompanies
                    //                 join cd in automationApiDb.CompaniesDepartments on c.MyCompanyId equals cd.MyCompanyId
                    //                 join d in automationApiDb.MyDepartments on cd.MyDepartmentId equals d.MyDepartmentId
                    //                 where myDepartmentsIds.Contains(d.MyDepartmentId)
                    //                 select c).ToList();

                    foreach (var myDepartmentsVM in myDepartmentsVMList)
                    {
                        #region rewrite inside module

                        //try
                        //{
                        //    var userProfile = usersProfile.FirstOrDefault(up => up.UserId.Equals(myDepartmentsVM.MyDepartmentDirectorId));
                        //    if (userProfile != null)
                        //    {
                        //        myDepartmentsVM.DirectorName = !string.IsNullOrEmpty(userProfile.Name) ? userProfile.Name : " ";
                        //        myDepartmentsVM.DirectorName += !string.IsNullOrEmpty(userProfile.Family) ? userProfile.Family : "";
                        //    }
                        //}
                        //catch (Exception exc)
                        //{ }

                        #endregion

                        try
                        {
                            var myCompany = myCompanies.Where(c => c.MyCompanyId.Equals(myDepartmentsVM.ParentId)).FirstOrDefault();

                            if (myCompany != null)
                            {
                                //myDepartmentsVM.MyCompaniesVM = _mapper.Map<MyCompanies, MyCompaniesVM>(myCompany);
                            }
                        }
                        catch (Exception exc)
                        {
                        }
                    }
                }
            }
            catch (Exception exc)
            { }
            return myDepartmentsVMList;
        }

        public List<MyDepartmentsVM> GetMyDepartmentsList(int DomainSettingId, List<long> childsUsersIds)
        {
            List<MyDepartmentsVM> myDepartmentsVMList = new List<MyDepartmentsVM>();
            try
            {
                //var domain = automationApiDb.DomainsSettings.Where(d => d.DomainSettingId.Equals(DomainSettingId)).FirstOrDefault();

                //long adminId = domain.UserIdCreator.Value;

                //List<long> childsUsersIds = base.GetChildUserIdsWithStoredProcedure(adminId);

                var company = automationApiDb.MyCompanies.Where(c => c.DomainSettingId.Equals(DomainSettingId) &&
                        childsUsersIds.Contains(c.UserIdCreator.Value) && c.IsDeleted.Value.Equals(false) && c.IsActivated.Value.Equals(true)).FirstOrDefault();

                myDepartmentsVMList = _mapper.Map<List<MyDepartments>,
                    List<MyDepartmentsVM>>(automationApiDb.MyDepartments.Where(c => childsUsersIds.Contains(c.UserIdCreator.Value) &&
                    c.ParentId.Equals(company.MyCompanyId) && c.ParentType.Equals("company")).
                    OrderByDescending(s => s.MyDepartmentId).ToList());

                //if (myDepartmentsVMList.Count > 0)
                //{
                //    var myDepartmentDirectorId = myDepartmentsVMList.Select(d => d.MyDepartmentDirectorId).ToList();

                //    #region rewrite inside module

                //    //var users = automationApiDb.Users.Where(up => myDepartmentDirectorId.Contains(up.UserId)).ToList();

                //    //var usersProfile = automationApiDb.UsersProfile.Where(up => myDepartmentDirectorId.Contains(up.UserId)).ToList();

                //    #endregion

                //    var myCompanyIds = myDepartmentsVMList.Where(d => d.ParentType.Equals("company")).Select(d => d.ParentId).ToList();

                //    //var myCompanies = automationApiDb.MyCompanies.Where(c => myCompanyIds.Contains(c.MyCompanyId)).ToList();

                //    //var companies = (from c in automationApiDb.MyCompanies
                //    //                 join cd in automationApiDb.CompaniesDepartments on c.MyCompanyId equals cd.MyCompanyId
                //    //                 join d in automationApiDb.MyDepartments on cd.MyDepartmentId equals d.MyDepartmentId
                //    //                 where myDepartmentsIds.Contains(d.MyDepartmentId)
                //    //                 select c).ToList();

                //    foreach (var myDepartmentsVM in myDepartmentsVMList)
                //    {
                //        #region rewrite inside module

                //        //try
                //        //{
                //        //    var user = users.FirstOrDefault(up => up.UserId.Equals(myDepartmentsVM.MyDepartmentDirectorId));
                //        //    var userProfile = usersProfile.FirstOrDefault(up => up.UserId.Equals(myDepartmentsVM.MyDepartmentDirectorId));

                //        //    if ((user != null) && (userProfile != null))
                //        //    {
                //        //        myDepartmentsVM.DirectorName = !string.IsNullOrEmpty(user.UserName) ? user.UserName : " ";
                //        //        myDepartmentsVM.DirectorName += !string.IsNullOrEmpty(userProfile.Name) ? userProfile.Name : " ";
                //        //        myDepartmentsVM.DirectorName += !string.IsNullOrEmpty(userProfile.Family) ? userProfile.Family : "";
                //        //    }
                //        //}
                //        //catch (Exception exc)
                //        //{ }

                //        #endregion

                //        try
                //        {
                //            //myDepartmentsVM.MyCompaniesVM = _mapper.Map<MyCompanies, MyCompaniesVM>(company);
                //            //var myCompany = myCompanies.Where(c => c.MyCompanyId.Equals(myDepartmentsVM.MyCompanyId)).FirstOrDefault();

                //            //if (myCompany != null)
                //            //{
                //            //    myDepartmentsVM.MyCompaniesVM = _mapper.Map<MyCompanies, MyCompaniesVM>(myCompany);
                //            //}
                //        }
                //        catch (Exception exc)
                //        {
                //        }
                //    }
                //}
            }
            catch (Exception exc)
            { }
            return myDepartmentsVMList;
        }

        public bool ExistDepartmentWithDepartmentName(List<long> childsUsersIds, MyDepartmentsVM myDepartmentsVM)
        {
            try
            {
                //List<long> childsUsersIds = new List<long>();
                //childsUsersIds = GetChildUserIds(ref childsUsersIds, myDepartmentsVM.UserIdCreator.Value).Distinct().ToList();

                if (!automationApiDb.MyDepartments.Any(c => c.MyDepartmentName.Equals(myDepartmentsVM.MyDepartmentName) &&
                        !c.ParentId.Equals(myDepartmentsVM.ParentId) &&
                        c.ParentType.Equals("company") &&
                        !childsUsersIds.Contains(c.UserIdCreator.Value)))
                    return true;
            }
            catch (Exception exc)
            { }
            return false;
        }

        public int AddToMyDepartments(MyDepartmentsVM myDepartmentsVM, List<long> childsUsersIds)
        {
            try
            {
                if (myDepartmentsVM.ParentId > 0)
                {
                    if (automationApiDb.MyDepartments.Where(n => childsUsersIds.Contains(n.UserIdCreator.Value))
                                        .Where(x => x.ParentId == myDepartmentsVM.ParentId &&
                                            x.ParentType.Equals("company") &&
                                            x.MyDepartmentName.Trim().Equals(myDepartmentsVM.MyDepartmentName.Trim())).Any())
                        return -1;
                    else
                    {
                        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                        {
                            MyDepartments myDepartment = _mapper.Map<MyDepartmentsVM, MyDepartments>(myDepartmentsVM);
                            automationApiDb.MyDepartments.Add(myDepartment);
                            automationApiDb.SaveChanges();

                            //MyDepartmentsDirectors myDepartmentsDirectors = new MyDepartmentsDirectors()
                            //{
                            //    CreateEnDate = myDepartment.CreateEnDate.Value,
                            //    CreateTime = myDepartment.CreateTime,
                            //    Credit = 0,
                            //    HasLimited = true,//TODO
                            //    IsActivated = true,
                            //    UserId = myDepartment.MyDepartmentDirectorId.Value,//مدیر بخش
                            //    MyDepartmentId = myDepartment.MyDepartmentId,//واحد
                            //    UserIdCreator = myDepartment.UserIdCreator.Value,

                            //};

                            //automationApiDb.MyDepartmentsDirectors.Add(myDepartmentsDirectors);
                            //automationApiDb.SaveChanges();

                            //myDepartment.MyDepartmentDirectorId = myDepartmentsDirectors.MyDepartmentsDirectorId;
                            //automationApiDb.MyDepartments.Update(myDepartment);
                            //automationApiDb.SaveChanges();

                            //The Transaction will be completed
                            scope.Complete();

                            return myDepartment.MyDepartmentId;

                        }
                    }
                }
                else
                {
                    if (!automationApiDb.MyDepartments.Any(c => childsUsersIds.Contains(c.UserIdCreator.Value) && c.MyDepartmentName.Equals(myDepartmentsVM.MyDepartmentName)))
                    {
                        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                        {
                            MyDepartments myDepartment = _mapper.Map<MyDepartmentsVM, MyDepartments>(myDepartmentsVM);
                            automationApiDb.MyDepartments.Add(myDepartment);
                            automationApiDb.SaveChanges();

                            //MyDepartmentsDirectors myDepartmentsDirectors = new MyDepartmentsDirectors()
                            //{
                            //    CreateEnDate = myDepartment.CreateEnDate.Value,
                            //    CreateTime = myDepartment.CreateTime,
                            //    Credit = 0,
                            //    HasLimited = true,//TODO
                            //    IsActivated = true,
                            //    UserId = myDepartment.MyDepartmentDirectorId.Value,//مدیر بخش
                            //    MyDepartmentId = myDepartment.MyDepartmentId,//واحد
                            //    UserIdCreator = myDepartment.UserIdCreator.Value,

                            //};

                            //automationApiDb.MyDepartmentsDirectors.Add(myDepartmentsDirectors);
                            //automationApiDb.SaveChanges();

                            //myDepartment.MyDepartmentDirectorId = myDepartmentsDirectors.MyDepartmentsDirectorId;
                            //automationApiDb.MyDepartments.Update(myDepartment);
                            //automationApiDb.SaveChanges();

                            //The Transaction will be completed
                            scope.Complete();

                            return myDepartment.MyDepartmentId;

                        }
                    }
                    else
                        return -1;
                }
            }


            catch (Exception exc)
            { }
            return 0;
        }

        public bool UpdateMyDepartments(MyDepartmentsVM myDepartmentsVM, List<long> childsUsersIds, ref string returnMessage)
        {
            try
            {
                //List<long> childsUsersIds = new List<long>();
                //childsUsersIds = GetChildUserIds(ref childsUsersIds, myDepartmentsVM.UserIdCreator.Value).Distinct().ToList();

                if (!automationApiDb.MyDepartments.Any(c => c.MyDepartmentName.Equals(myDepartmentsVM.MyDepartmentName) &&
                        c.ParentId.Equals(myDepartmentsVM.ParentId) &&
                        c.ParentType.Equals("company") &&
                        !c.MyDepartmentId.Equals(myDepartmentsVM.MyDepartmentId) &&
                        !childsUsersIds.Contains(c.UserIdCreator.Value)))
                {
                    #region Update MyDepartment
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                    {
                        var myDepartment = automationApiDb.MyDepartments.Where(c => c.MyDepartmentId.Equals(myDepartmentsVM.MyDepartmentId)).FirstOrDefault();

                        //var myDepartmentsDirector = automationApiDb.MyDepartmentsDirectors.Where(c => c.MyDepartmentsDirectorId == myDepartment.MyDepartmentDirectorId).FirstOrDefault();


                        myDepartment.EditEnDate = myDepartmentsVM.EditEnDate.Value;
                        myDepartment.EditTime = myDepartmentsVM.EditTime;
                        myDepartment.UserIdEditor = myDepartmentsVM.UserIdEditor.HasValue ? myDepartmentsVM.UserIdEditor.Value : (long?)null;
                        myDepartment.DutiesDescriptions = myDepartmentsVM.DutiesDescriptions;
                        myDepartment.EstablishedEnDate = myDepartmentsVM.EstablishedEnDate.HasValue ?
                        myDepartmentsVM.EstablishedEnDate.Value : (DateTime?)null;
                        myDepartment.IsActivated = myDepartmentsVM.IsActivated;
                        //myDepartment.MyDepartmentDirectorId = myDepartmentsVM.MyDepartmentDirectorId.HasValue ?
                        //myDepartmentsVM.MyDepartmentDirectorId.Value : (long?)null;
                        myDepartment.MyDepartmentName = myDepartmentsVM.MyDepartmentName;
                        myDepartment.MyDepartmentRealName = myDepartmentsVM.MyDepartmentRealName;


                        //if (!myDepartmentsDirector.UserId.Equals(myDepartmentsVM.MyDepartmentDirectorId))
                        //{
                        //    MyDepartmentsDirectors myDepartmentsDirectors = new MyDepartmentsDirectors()
                        //    {
                        //        CreateEnDate = DateTime.Now,
                        //        Credit = 0,
                        //        HasLimited = true,//TODO
                        //        IsActivated = true,
                        //        UserId = myDepartmentsVM.MyDepartmentDirectorId.Value,//مدیر بخش
                        //        MyDepartmentId = myDepartmentsVM.MyDepartmentId,//واحد
                        //        UserIdCreator = myDepartmentsVM.UserIdCreator.Value,

                        //    };

                        //    automationApiDb.MyDepartmentsDirectors.Add(myDepartmentsDirectors);
                        //    automationApiDb.SaveChanges();

                        //    myDepartment.MyDepartmentDirectorId = myDepartmentsDirectors.MyDepartmentsDirectorId;

                        //}

                        automationApiDb.SaveChanges();

                        scope.Complete();
                    }
                    #endregion

                    return true;
                }
                else
                {
                    returnMessage = "DuplicateDepartmentName";
                    return false;
                }
            }
            catch (Exception exc)
            { }
            return false;
        }

        public bool ToggleActivationMyDepartment(int myDepartmentId, long userId, List<long> childsUsersIds)
        {
            try
            {
                //List<long> childsUsersIds = base.GetChildUserIdsWithStoredProcedure(userId);

                if (automationApiDb.MyDepartments.Any(c => c.MyDepartmentId.Equals(myDepartmentId) &&
                        childsUsersIds.Contains(c.UserIdCreator.Value)))
                {
                    var myDepartment = automationApiDb.MyDepartments.FirstOrDefault(c => c.MyDepartmentId.Equals(myDepartmentId));
                    myDepartment.IsActivated = !myDepartment.IsActivated;
                    myDepartment.EditEnDate = DateTime.Now;
                    myDepartment.EditTime = PersianDate.TimeNow;
                    myDepartment.UserIdEditor = userId;
                    automationApiDb.SaveChanges();
                    return true;
                }
            }
            catch (Exception exc)
            { }
            return false;
        }

        public bool TemporaryDeleteMyDepartments(List<long> childsUsersIds, int MyDepartmentId, long userId)
        {
            try
            {
                //List<long> childUserIds = new List<long>();
                //childUserIds = GetChildUserIds(ref childUserIds, userId);

                //List<long> childUserIds = base.GetChildUserIdsWithStoredProcedure(userId);

                if (automationApiDb.MyDepartments.Any(gd => gd.MyDepartmentId.Equals(MyDepartmentId) &&
                        childsUsersIds.Contains(gd.UserIdCreator.Value)))
                {
                    var myDepartments = automationApiDb.MyDepartments.Where(gd => gd.MyDepartmentId.Equals(MyDepartmentId) &&
                             childsUsersIds.Contains(gd.UserIdCreator.Value)).FirstOrDefault();

                    myDepartments.IsDeleted = !myDepartments.IsDeleted;
                    myDepartments.EditEnDate = DateTime.Now;
                    myDepartments.EditTime = PersianDate.TimeNow;
                    myDepartments.UserIdEditor = userId;
                    automationApiDb.SaveChanges();
                    return true;
                }
            }
            catch (Exception exc)
            { }
            return false;
        }

        public bool CompleteDeleteMyDepartments(int myDepartmentId, List<long> childsUsersIds, ref string returnMessage)
        {
            try
            {
                if (automationApiDb.MyDepartments.Where(c => c.MyDepartmentId.Equals(myDepartmentId) && childsUsersIds.Contains(c.UserIdCreator.Value)).Any())
                {
                    var myDepartment = automationApiDb.MyDepartments.FirstOrDefault(c => c.MyDepartmentId.Equals(myDepartmentId));
                    automationApiDb.MyDepartments.Remove(myDepartment);
                    automationApiDb.SaveChanges();
                    return true;
                }
            }
            catch (Exception exc)
            { }
            return false;
        }

        public List<MyDepartmentsVM> GetAllMyDepartmentsList(List<long> childsUsersIds, int myCompanyId)
        {
            List<MyDepartmentsVM> myDepartmentsVMList = new List<MyDepartmentsVM>();
            try
            {
                //List<long> childsUsersIds = new List<long>();
                //childsUsersIds = GetChildUserIds(ref childsUsersIds, userId).Distinct().ToList();

                //List<long> childsUsersIds = base.GetChildUserIdsWithStoredProcedure(userId);

                var myDepartments = automationApiDb.MyDepartments.Where(c => childsUsersIds.Contains(c.UserIdCreator.Value) && c.IsDeleted.Value.Equals(false) && c.IsActivated.Value.Equals(true)).AsQueryable();

                if (myCompanyId > 0)
                    myDepartments = myDepartments.Where(c => c.ParentId.Equals(myCompanyId) && c.ParentType.Equals("company"));

                myDepartmentsVMList = _mapper.Map<List<MyDepartments>,
                                List<MyDepartmentsVM>>(myDepartments.ToList());

                #region rewrite inside module

                //var userIdCreators = myDepartmentsVMList.Where(i => i.UserIdCreator.HasValue).Select(u => u.UserIdCreator.Value).ToList();
                //var customUsers = (from u in automationApiDb.Users
                //                   join up in automationApiDb.UsersProfile
                //                   on u.UserId equals up.UserId
                //                   where userIdCreators.Contains(u.UserId)
                //                   select new CustomUsersVM
                //                   {
                //                       UserId = u.UserId,
                //                       Name = up.Name,
                //                       Family = up.Family,
                //                       UserName = u.UserName
                //                   }).ToList();

                //foreach (var myDepartmentsVM in myDepartmentsVMList)
                //{
                //    if (myDepartmentsVM.UserIdCreator.HasValue)
                //    {
                //        var customUser = customUsers.FirstOrDefault(c => c.UserId.Equals(myDepartmentsVM.UserIdCreator.Value));
                //        if (customUser != null)
                //        {
                //            myDepartmentsVM.UserCreatorName = customUser.UserName;

                //            if (!string.IsNullOrEmpty(customUser.Name))
                //                myDepartmentsVM.UserCreatorName += " " + customUser.Name;

                //            if (!string.IsNullOrEmpty(customUser.Family))
                //                myDepartmentsVM.UserCreatorName += " " + customUser.Family;
                //        }
                //    }
                //}

                #endregion
            }
            catch (Exception exc)
            { }
            return myDepartmentsVMList;
        }

        public List<MyDepartmentsVM> GetFreeMyDepartmentsList(long userId, List<long> childsUsersIds)
        {
            try
            {
                //List<long> childsUsersIds = base.GetChildUserIdsWithStoredProcedure(userId);

                //return _mapper.Map<List<MyDepartments>,
                //                List<MyDepartmentsVM>>((from eg in automationApiDb.MyDepartments
                //                                        where !eg.MyDepartmentDirectorId.HasValue &&
                //                                        childsUsersIds.Contains(eg.UserIdCreator.Value)
                //                                        select eg).ToList());



                //List<long> childsUsersIds = new List<long>();
                //childsUsersIds = GetChildUserIds(ref childsUsersIds, userId).Distinct().ToList();

                //return _mapper.Map<List<EducationalGroups>,
                //                List<EducationalGroupsVM>>((from eg in automationApiDb.EducationalGroups
                //                                                   where !automationApiDb.EducationalGroupsDirectorsEducationalGroups.Any(
                //                                                       gd => gd.EducationalGroupId == eg.EducationalGroupId) &&
                //                                                       childsUsersIds.Contains(eg.UserIdCreator.Value)
                //                                                   select eg).ToList());

                //return _mapper.Map<List<EducationalGroups>,
                //                List<EducationalGroupsVM>>(automationApiDb.EducationalGroups.Where(c => childsUsersIds.Contains(c.UserIdCreator.Value))
                //                    .OrderByDescending(s => s.EducationalGroupId).ToList());
            }
            catch (Exception exc)
            { }
            return new List<MyDepartmentsVM>();
        }

        #endregion

        #region Methods For Work With MyDepartmentsDirectors

        //public List<MyDepartmentsDirectorsVM> GetMyDepartmentsDirectorsList(int jtStartIndex,
        //    int jtPageSize,
        //    ref int listCount,
        //    string jtSorting,
        //    List<long> childsUsersIds,
        //    long userId = 0,
        //    string userNameSearch = null,
        //    string nameFamilySearch = null,
        //    string mobilePhoneSearch = null,
        //    string nationalCodeSearch = null)
        //{
        //    List<MyDepartmentsDirectorsVM> myDepartmentsDirectorsVMList = new List<MyDepartmentsDirectorsVM>();

        //    #region rewrite inside module

        //    //try
        //    //{
        //    //    //List<long> childUserIds = new List<long>();
        //    //    //childUserIds = base.GetChildUserIdsWithStoredProcedure(userId);

        //    //    var list = (from u in automationApiDb.Users
        //    //                join up in automationApiDb.UsersProfile on u.UserId equals up.UserId
        //    //                join d in automationApiDb.MyDepartmentsDirectors on u.UserId equals d.UserId
        //    //                where childsUsersIds.Contains(d.UserIdCreator.Value)
        //    //                select new MyDepartmentsDirectorsVM
        //    //                {
        //    //                    CreateEnDate = d.CreateEnDate.Value,
        //    //                    CreateTime = d.CreateTime,
        //    //                    Credit = d.Credit.HasValue ? d.Credit.Value : 0,
        //    //                    HasLimited = d.HasLimited,
        //    //                    IsActivated = d.IsActivated,
        //    //                    EditEnDate = d.EditEnDate.HasValue ?
        //    //                        d.EditEnDate.Value : (DateTime?)null,
        //    //                    EditTime = d.EditTime,
        //    //                    UserId = d.UserId,
        //    //                    UserIdCreator = d.UserIdCreator.Value,
        //    //                    UserIdEditor = d.UserIdEditor.HasValue ?
        //    //                        d.UserIdEditor.Value : (long?)null,
        //    //                    CustomUsersVM = new CustomUsersVM
        //    //                    {
        //    //                        UserName = u.UserName,
        //    //                        Name = up.Name,
        //    //                        Family = up.Family,
        //    //                        DomainSettingId = u.DomainSettingId
        //    //                    }
        //    //                }).AsQueryable();

        //    //    if (string.IsNullOrEmpty(userNameSearch) &&
        //    //        string.IsNullOrEmpty(nameFamilySearch) &&
        //    //        string.IsNullOrEmpty(mobilePhoneSearch) &&
        //    //        string.IsNullOrEmpty(nationalCodeSearch))
        //    //    {

        //    //    }
        //    //    else
        //    //    {
        //    //        if (!string.IsNullOrEmpty(userNameSearch))
        //    //            list = list.Where(l => l.CustomUsersVM.UserName.Contains(userNameSearch));

        //    //        if (!string.IsNullOrEmpty(nameFamilySearch))
        //    //            list = list.Where(l => l.CustomUsersVM.Name.Contains(nameFamilySearch) ||
        //    //                l.CustomUsersVM.Family.Contains(nameFamilySearch));

        //    //        if (!string.IsNullOrEmpty(mobilePhoneSearch))
        //    //            list = list.Where(l => l.CustomUsersVM.Mobile.Contains(mobilePhoneSearch) ||
        //    //                l.CustomUsersVM.Phone.Contains(mobilePhoneSearch));

        //    //        if (!string.IsNullOrEmpty(nationalCodeSearch))
        //    //            list = list.Where(l => l.CustomUsersVM.NationalCode.Contains(nationalCodeSearch) ||
        //    //                l.CustomUsersVM.NationalCode.Contains(nationalCodeSearch));
        //    //    }

        //    //    listCount = list.Count();
        //    //    if (listCount > jtPageSize)
        //    //        list = list.Skip(jtStartIndex).Take(jtPageSize);

        //    //    myDepartmentsDirectorsVMList = list.ToList();

        //    //    List<long> myDepartmentsDirectorIds = myDepartmentsDirectorsVMList.Select(g => g.UserId).ToList();
        //    //    var myDepartments = automationApiDb.MyDepartments.Where(g => g.MyDepartmentDirectorId.HasValue).
        //    //            Where(g => myDepartmentsDirectorIds.Contains(g.MyDepartmentDirectorId.Value)).ToList();

        //    //    List<int> myCompanyIds = myDepartments.Select(g => g.MyCompanyId).ToList();
        //    //    var myCompanies = automationApiDb.MyCompanies.Where(i => myCompanyIds.Contains(i.MyCompanyId)).ToList();

        //    //    foreach (var director in myDepartmentsDirectorsVMList)
        //    //    {
        //    //        var tmpMyDepartments = myDepartments.Where(g => g.MyDepartmentDirectorId.Value.Equals(director.UserId)).ToList();
        //    //        if (tmpMyDepartments.Count > 0)
        //    //        {
        //    //            foreach (var tmpMyDepartment in tmpMyDepartments)
        //    //            {
        //    //                director.MyDepartmentsIds.Add(tmpMyDepartment.MyDepartmentId);

        //    //                var myDepartmentsVM = _mapper.Map<MyDepartments,
        //    //                    MyDepartmentsVM>(tmpMyDepartment);

        //    //                director.MyDepartmentsVM.Add(myDepartmentsVM);
        //    //            }
        //    //        }
        //    //    }
        //    //}
        //    //catch (Exception exc)
        //    //{ }

        //    #endregion

        //    return myDepartmentsDirectorsVMList;
        //}

        //public List<MyDepartmentsDirectorsVM> GetAllMyDepartmentsDirectorsList(List<long> childsUsersIds)
        //{
        //    List<MyDepartmentsDirectorsVM> myDepartmentsDirectorsVMList = new List<MyDepartmentsDirectorsVM>();

        //    try
        //    {
        //        if (automationApiDb.MyDepartmentsDirectors.Where(d => childsUsersIds.Contains(d.UserIdCreator.Value)).Any())
        //        {
        //            var list = automationApiDb.MyDepartmentsDirectors.Where(d => childsUsersIds.Contains(d.UserIdCreator.Value)).AsQueryable();



        //            myDepartmentsDirectorsVMList = _mapper.Map<List<MyDepartmentsDirectors>,
        //                List<MyDepartmentsDirectorsVM>>(list.ToList());
        //        }
        //    }
        //    catch (Exception exe)
        //    { }

        //    return myDepartmentsDirectorsVMList;
        //}

        //public List<MyDepartmentsDirectorsVM> GetMyDepartmentsDirectorsList(List<long> childsUsersIds,
        //    int directorRoleId,
        //    List<UsersRolesVM> usersRolesVMList,
        //    long userId = 0,
        //    int domainSettingId = 0)
        //{
        //    List<MyDepartmentsDirectorsVM> myDepartmentsDirectorsVMList = new List<MyDepartmentsDirectorsVM>();
        //    try
        //    {
        //        //List<long> childUserIds = new List<long>();
        //        //childUserIds = base.GetChildUserIdsWithStoredProcedure(userId);

        //        #region rewrite inside module

        //        //var directorRole = automationApiDb.Roles.Where(r => r.RoleName.Equals("Admins")).FirstOrDefault();

        //        //var notHasdirectoreProfileUserIds = automationApiDb.UsersRoles.Where(ur => ur.RoleId.Equals(directorRole.RoleId))
        //        //    .Where(ur => childUserIds.Contains(ur.UserId))
        //        //    .Where(ur => !automationApiDb.MyDepartmentsDirectors
        //        //    .Any(d => d.UserId == ur.UserId)).Select(x => x.UserId).ToList();

        //        #endregion

        //        var notHasdirectoreProfileUserIds = usersRolesVMList.Where(ur => ur.RoleId.Equals(directorRoleId))
        //            .Where(ur => childsUsersIds.Contains(ur.UserId))
        //            .Where(ur => !automationApiDb.MyDepartmentsDirectors
        //            .Any(d => d.UserId == ur.UserId)).Select(x => x.UserId).ToList();

        //        if (notHasdirectoreProfileUserIds.Count > 0)
        //        {
        //            List<MyDepartmentsDirectors> newMyDepartmentsDirectorsList = new List<MyDepartmentsDirectors>();
        //            foreach (var newDirectorUserId in notHasdirectoreProfileUserIds)
        //            {
        //                MyDepartmentsDirectors myDepartmentsDirectors = new MyDepartmentsDirectors()
        //                {
        //                    CreateEnDate = DateTime.Now,
        //                    CreateTime = PersianDate.TimeNow,
        //                    Credit = 0,
        //                    HasLimited = false,
        //                    IsActivated = true,
        //                    UserId = newDirectorUserId,
        //                    UserIdCreator = userId
        //                };
        //                newMyDepartmentsDirectorsList.Add(myDepartmentsDirectors);
        //            }

        //            automationApiDb.MyDepartmentsDirectors.AddRange(newMyDepartmentsDirectorsList);
        //            automationApiDb.SaveChanges();
        //        }

        //        #region rewrite inside module

        //        //var list = (from u in automationApiDb.Users
        //        //            join up in automationApiDb.UsersProfile on u.UserId equals up.UserId
        //        //            join ed in automationApiDb.MyDepartmentsDirectors on u.UserId equals ed.UserId
        //        //            join d in automationApiDb.DomainsSettings on u.DomainSettingId equals d.DomainSettingId
        //        //            select new MyDepartmentsDirectorsVM
        //        //            {
        //        //                //UserId = u.UserId,
        //        //                CreateEnDate = ed.CreateEnDate.Value,
        //        //                CreateTime = ed.CreateTime,
        //        //                Credit = ed.Credit.HasValue ? ed.Credit.Value : 0,
        //        //                HasLimited = ed.HasLimited,
        //        //                IsActivated = ed.IsActivated,
        //        //                EditEnDate = ed.EditEnDate.HasValue ?
        //        //                    ed.EditEnDate.Value : (DateTime?)null,
        //        //                EditTime = ed.EditTime,
        //        //                UserId = ed.UserId,
        //        //                UserIdCreator = ed.UserIdCreator.Value,
        //        //                UserIdEditor = ed.UserIdEditor.HasValue ?
        //        //                    ed.UserIdEditor.Value : (long?)null,
        //        //                CustomUsersVM = new CustomUsersVM
        //        //                {
        //        //                    UserName = u.UserName,
        //        //                    Name = up.Name,
        //        //                    Family = up.Family,
        //        //                    DomainSettingId = d.DomainSettingId
        //        //                }
        //        //            }).AsQueryable();

        //        //if (userId != 0)
        //        //{
        //        //    list = list.Where(l => childUserIds.Contains(l.UserId));
        //        //}

        //        //if (domainSettingId != 0)
        //        //{
        //        //    list = list.Where(l => l.CustomUsersVM.DomainSettingId.Equals(domainSettingId));
        //        //}

        //        //myDepartmentsDirectorsVMList = list.ToList();

        //        //List<long> myDepartmentsDirectorsIds = myDepartmentsDirectorsVMList.Select(d => d.UserId).ToList();

        //        //var myDepartments = automationApiDb.MyDepartments.Where(g => g.MyDepartmentDirectorId.HasValue)
        //        //    .Where(g => myDepartmentsDirectorsIds.Contains(g.MyDepartmentDirectorId.Value)).ToList();

        //        //foreach (var myDepartmentsDirectorsVM in myDepartmentsDirectorsVMList)
        //        //{
        //        //    if (!myDepartments.Any(g => g.MyDepartmentDirectorId.Value.Equals(myDepartmentsDirectorsVM.UserId)))
        //        //        myDepartmentsDirectorsVM.IsFree = true;
        //        //    else
        //        //        myDepartmentsDirectorsVM.IsFree = false;
        //        //}

        //        #endregion
        //    }
        //    catch (Exception exc)
        //    {
        //    }
        //    return myDepartmentsDirectorsVMList;
        //}

        //public bool AddToMyDepartmentsDirectors(MyDepartmentsDirectorsVM myDepartmentsDirectorsVM,
        //    List<long> childsUsersIds)
        //{
        //    try
        //    {
        //        MyDepartmentsDirectors myDepartmentsDirectors = new MyDepartmentsDirectors();
        //        if (automationApiDb.MyDepartmentsDirectors.Where(d => d.UserId.Equals(myDepartmentsDirectorsVM.UserId) &&
        //                childsUsersIds.Contains(d.UserIdCreator.Value)).Any())
        //        {
        //            myDepartmentsDirectors = automationApiDb.MyDepartmentsDirectors.Where(d =>
        //                    d.UserId.Equals(myDepartmentsDirectorsVM.UserId)).FirstOrDefault();

        //            myDepartmentsDirectors.EditEnDate = DateTime.Now;
        //            myDepartmentsDirectors.EditTime = PersianDate.TimeNow;
        //            myDepartmentsDirectors.Credit = myDepartmentsDirectorsVM.Credit.HasValue ?
        //                myDepartmentsDirectorsVM.Credit.Value : 0;
        //            myDepartmentsDirectors.HasLimited = myDepartmentsDirectorsVM.HasLimited;
        //            myDepartmentsDirectors.IsActivated = myDepartmentsDirectorsVM.IsActivated;
        //            myDepartmentsDirectors.UserId = myDepartmentsDirectorsVM.UserId;
        //            myDepartmentsDirectors.UserIdEditor = myDepartmentsDirectorsVM.UserIdCreator.Value;

        //            automationApiDb.SaveChanges();
        //        }
        //        else
        //        {

        //            myDepartmentsDirectors = new MyDepartmentsDirectors()
        //            {
        //                CreateEnDate = myDepartmentsDirectorsVM.CreateEnDate.Value,
        //                CreateTime = myDepartmentsDirectorsVM.CreateTime,
        //                Credit = myDepartmentsDirectorsVM.Credit.HasValue ?
        //                    myDepartmentsDirectorsVM.Credit.Value : 0,
        //                HasLimited = myDepartmentsDirectorsVM.HasLimited,
        //                IsActivated = myDepartmentsDirectorsVM.IsActivated,
        //                UserId = myDepartmentsDirectorsVM.UserId,
        //                UserIdCreator = myDepartmentsDirectorsVM.UserIdCreator.Value
        //            };

        //            automationApiDb.MyDepartmentsDirectors.Add(myDepartmentsDirectors);
        //            automationApiDb.SaveChanges();
        //        }

        //        long myDepartmentsDirectorId = myDepartmentsDirectors.UserId;

        //        if (myDepartmentsDirectorsVM.MyDepartmentsIds != null)
        //        {
        //            if (myDepartmentsDirectorsVM.MyDepartmentsIds.Count > 0)
        //            {
        //                //var oldEducationalGroups = automationApiDb.EducationalGroups.Where(g =>
        //                //        educationalGroupsDirectorsVM.EducationalGroupIds.Contains(g.EducationalGroupId)).ToList();

        //                foreach (var myDepartmentsId in myDepartmentsDirectorsVM.MyDepartmentsIds)
        //                {
        //                    if (automationApiDb.MyDepartments.Any(g => g.MyDepartmentId.Equals(myDepartmentsId) /*&&
        //                            !g.EducationalGroupDirectorId.Value.Equals(educationalGroupDirectorId)*/))
        //                    {
        //                        var myDepartment = automationApiDb.MyDepartments.Where(g =>
        //                                g.MyDepartmentId.Equals(myDepartmentsId)).FirstOrDefault();

        //                        myDepartment.MyDepartmentDirectorId = myDepartmentsDirectorId;
        //                        myDepartment.UserIdEditor = myDepartmentsDirectorsVM.UserIdCreator.HasValue ?
        //                            myDepartmentsDirectorsVM.UserIdCreator.Value : (long?)null;
        //                        myDepartment.EditEnDate = DateTime.Now;
        //                        myDepartment.EditTime = PersianDate.TimeNow;

        //                        automationApiDb.SaveChanges();
        //                    }
        //                    //else
        //                    //{

        //                    //}
        //                    //educationalGroupsDirectorsEducationalGroupsList.Add(new EducationalGroupsDirectorsEducationalGroups()
        //                    //{
        //                    //    UserId = educationalGroupsDirectorsVM.UserId,
        //                    //    EducationalGroupId = educationalGroupId,
        //                    //    CreateEnDate = DateTime.Now,
        //                    //    CreateTime = PersianDate.DateNow,
        //                    //    UserIdCreator = educationalGroupsDirectorsVM.UserIdCreator.Value
        //                    //});
        //                }

        //                if (automationApiDb.MyDepartments.Where(g => g.MyDepartmentDirectorId.HasValue).
        //                    Any(g => g.MyDepartmentDirectorId.Value.Equals(myDepartmentsDirectorId) &&
        //                    !myDepartmentsDirectorsVM.MyDepartmentsIds.Contains(g.MyDepartmentId)))
        //                {
        //                    var oldMyDepartments = automationApiDb.MyDepartments.Where(g => g.MyDepartmentDirectorId.HasValue).
        //                            Where(g => g.MyDepartmentDirectorId.Value.Equals(myDepartmentsDirectorId) &&
        //                            !myDepartmentsDirectorsVM.MyDepartmentsIds.Contains(g.MyDepartmentId)).ToList();

        //                    foreach (var oldMyDepartment in oldMyDepartments)
        //                    {
        //                        oldMyDepartment.MyDepartmentDirectorId = (long?)null;
        //                        oldMyDepartment.UserIdEditor = myDepartmentsDirectorsVM.UserIdCreator.HasValue ?
        //                            myDepartmentsDirectorsVM.UserIdCreator.Value : (long?)null;
        //                        oldMyDepartment.EditEnDate = DateTime.Now;
        //                        oldMyDepartment.EditTime = PersianDate.TimeNow;
        //                    }
        //                    automationApiDb.SaveChanges();
        //                }

        //                //automationApiDb.EducationalGroupsDirectorsEducationalGroups.AddRange(educationalGroupsDirectorsEducationalGroupsList);
        //                //automationApiDb.SaveChanges();
        //            }
        //        }

        //        return true;
        //    }
        //    catch (Exception exc)
        //    { }
        //    return false;
        //}

        //public bool UpdateMyDepartmentDirector(ref MyDepartmentsDirectorsVM myDepartmentsDirectorsVM,
        //    List<long> childsUsersIds)
        //{
        //    try
        //    {

        //        //if (automationApiDb.MyDepartmentsDirectors.Any(d => d.UserId.Equals(myDepartmentsDirectorsVM.UserId)))
        //        //{
        //        //    var director = automationApiDb.MyDepartmentsDirectors.FirstOrDefault(d => d.UserId.Equals(myDepartmentsDirectorsVM.UserId));

        //        //    director.Credit = myDepartmentsDirectorsVM.Credit.HasValue ? myDepartmentsDirectorsVM.Credit.Value : 0;
        //        //    director.EditEnDate = myDepartmentsDirectorsVM.EditEnDate.Value;
        //        //    director.EditTime = myDepartmentsDirectorsVM.EditTime;
        //        //    director.HasLimited = myDepartmentsDirectorsVM.HasLimited;
        //        //    director.IsActivated = myDepartmentsDirectorsVM.IsActivated;
        //        //    director.UserIdEditor = myDepartmentsDirectorsVM.UserIdEditor.Value;
        //        //    automationApiDb.SaveChanges();

        //        //    if (myDepartmentsDirectorsVM.MyDepartmentsIds.Count > 0)
        //        //    {
        //        //        List<MyDepartments> oldMyDepartments = automationApiDb.MyDepartments.Where(g =>
        //        //                myDepartmentsDirectorsVM.MyDepartmentsIds.Contains(g.MyDepartmentId)).ToList();

        //        //        foreach (var directorsGroups in oldMyDepartments)
        //        //        {
        //        //            directorsGroups.MyDepartmentDirectorId = director.UserId;
        //        //            directorsGroups.EditEnDate = DateTime.Now;
        //        //            directorsGroups.EditTime = PersianDate.TimeNow;
        //        //            directorsGroups.UserIdEditor = myDepartmentsDirectorsVM.UserIdEditor.HasValue ?
        //        //                myDepartmentsDirectorsVM.UserIdEditor.Value : (long?)null;
        //        //        }

        //        //        automationApiDb.SaveChanges();

        //        //        if (automationApiDb.MyDepartments.Any(g => !myDepartmentsDirectorsVM.MyDepartmentsIds.Contains(g.MyDepartmentId)))
        //        //        {
        //        //            List<MyDepartments> deleteDirectorsGroups = automationApiDb.MyDepartments.
        //        //                Where(g => !myDepartmentsDirectorsVM.MyDepartmentsIds.Contains(g.MyDepartmentId) &&
        //        //                g.MyDepartmentDirectorId.Equals(myDepartmentsDirectorsVM.UserId)).ToList();

        //        //            if (deleteDirectorsGroups.Count > 0)
        //        //            {
        //        //                foreach (var deleteDirectorsGroup in deleteDirectorsGroups)
        //        //                {
        //        //                    deleteDirectorsGroup.MyDepartmentDirectorId = (long?)null;
        //        //                    deleteDirectorsGroup.EditEnDate = DateTime.Now;
        //        //                    deleteDirectorsGroup.EditTime = PersianDate.TimeNow;
        //        //                    deleteDirectorsGroup.UserIdEditor = myDepartmentsDirectorsVM.UserIdEditor.HasValue ?
        //        //                        myDepartmentsDirectorsVM.UserIdEditor.Value : (long?)null;
        //        //                }

        //        //                automationApiDb.SaveChanges();
        //        //            }
        //        //        }
        //        //    }

        //        //    return true;
        //        //}
        //    }
        //    catch (Exception exc)
        //    { }
        //    return false;
        //}

        //public MyDepartmentsDirectorsVM GetMyDepartmentsDirectorsWithMyDepartmentDirectorId(List<long> childsUsersIds,
        //    int directorId,
        //    long userId)
        //{
        //    MyDepartmentsDirectorsVM myDepartmentsDirectorsVM = new MyDepartmentsDirectorsVM();

        //    try
        //    {
        //        //List<long> childUserIds = new List<long>();
        //        //childUserIds = GetChildUserIds(ref childUserIds, userId);

        //        //List<long> childUserIds = base.GetChildUserIdsWithStoredProcedure(userId);

        //        if (automationApiDb.MyDepartmentsDirectors.Where(gd => gd.UserId.Equals(directorId) &&
        //                gd.UserId.Equals(userId) &&
        //                childsUsersIds.Contains(gd.UserIdCreator.Value)).Any())
        //        {

        //            myDepartmentsDirectorsVM = _mapper.Map<MyDepartmentsDirectors,
        //                MyDepartmentsDirectorsVM>(automationApiDb.MyDepartmentsDirectors.Where(gd => gd.UserId.Equals(directorId) &&
        //                gd.UserId.Equals(userId) &&
        //                childsUsersIds.Contains(gd.UserIdCreator.Value)).FirstOrDefault());
        //        }
        //    }
        //    catch (Exception exc)
        //    { }
        //    return myDepartmentsDirectorsVM;
        //}

        //public bool ToggleActivationMyDepartmentsDirectors(List<long> childsUsersIds,
        //    int directorId,
        //    long userId)
        //{
        //    try
        //    {
        //        //List<long> childUserIds = new List<long>();
        //        //childUserIds = GetChildUserIds(ref childUserIds, userId);

        //        //List<long> childUserIds = base.GetChildUserIdsWithStoredProcedure(userId);

        //        if (automationApiDb.MyDepartmentsDirectors.Any(gd => gd.MyDepartmentId.Equals(directorId) &&
        //                gd.UserId.Equals(userId) &&
        //                childsUsersIds.Contains(gd.UserIdCreator.Value)))
        //        {
        //            var myDepartmentsDirectors = automationApiDb.MyDepartmentsDirectors.FirstOrDefault(gd => gd.UserId.Equals(directorId) &&
        //                    childsUsersIds.Contains(gd.UserIdCreator.Value));

        //            myDepartmentsDirectors.IsActivated = !myDepartmentsDirectors.IsActivated;
        //            myDepartmentsDirectors.EditEnDate = DateTime.Now;
        //            myDepartmentsDirectors.EditTime = PersianDate.TimeNow;
        //            myDepartmentsDirectors.UserIdEditor = userId;
        //            automationApiDb.SaveChanges();
        //            return true;
        //        }
        //    }
        //    catch (Exception exc)
        //    { }
        //    return false;
        //}

        //public bool TemporaryDeleteMyDepartmentsDirectors(List<long> childsUsersIds,
        //    int directorId,
        //    long userId)
        //{
        //    try
        //    {
        //        //List<long> childUserIds = new List<long>();
        //        //childUserIds = GetChildUserIds(ref childUserIds, userId);

        //        //List<long> childUserIds = base.GetChildUserIdsWithStoredProcedure(userId);

        //        if (automationApiDb.MyDepartmentsDirectors.Any(gd => gd.MyDepartmentId.Equals(directorId) &&
        //                gd.UserId.Equals(userId) &&
        //                childsUsersIds.Contains(gd.UserIdCreator.Value)))
        //        {
        //            var myDepartmentsDirectors = automationApiDb.MyDepartmentsDirectors.Where(gd => gd.UserId.Equals(directorId) &&
        //                    gd.UserId.Equals(userId) &&
        //                    childsUsersIds.Contains(gd.UserIdCreator.Value)).FirstOrDefault();

        //            myDepartmentsDirectors.IsDeleted = !myDepartmentsDirectors.IsDeleted;
        //            myDepartmentsDirectors.EditEnDate = DateTime.Now;
        //            myDepartmentsDirectors.EditTime = PersianDate.TimeNow;
        //            myDepartmentsDirectors.UserIdEditor = userId;
        //            automationApiDb.SaveChanges();
        //            return true;
        //        }
        //    }
        //    catch (Exception exc)
        //    { }
        //    return false;
        //}

        //public bool ToggleLimitedMyDepartmentsDirectors(List<long> childsUsersIds,
        //    int directorId,
        //    long userId)
        //{
        //    try
        //    {
        //        //List<long> childUserIds = new List<long>();
        //        //childUserIds = GetChildUserIds(ref childUserIds, userId);

        //        if (automationApiDb.MyDepartmentsDirectors.Any(gd => gd.UserId.Equals(directorId) &&
        //                childsUsersIds.Contains(gd.UserIdCreator.Value)))
        //        {
        //            var myDepartmentsDirectors = automationApiDb.MyDepartmentsDirectors.
        //                Where(gd => gd.UserId.Equals(directorId) &&
        //                childsUsersIds.Contains(gd.UserIdCreator.Value)).FirstOrDefault();

        //            myDepartmentsDirectors.HasLimited = !myDepartmentsDirectors.HasLimited;
        //            automationApiDb.SaveChanges();
        //            return true;
        //        }
        //    }
        //    catch (Exception exc)
        //    { }
        //    return false;
        //}

        //public bool CompleteDeleteMyDepartmentsDirectors(List<long> childsUsersIds,
        //    int directorId,
        //    long userId,
        //    ref string returnMessage)
        //{
        //    returnMessage = "";

        //    try
        //    {
        //        //List<long> childUserIds = base.GetChildUserIdsWithStoredProcedure(userId);

        //        if (automationApiDb.MyDepartmentsDirectors.Any(gd => gd.UserId.Equals(directorId) &&
        //                childsUsersIds.Contains(gd.UserIdCreator.Value)))
        //        {
        //            var myDepartmentsDirectors = automationApiDb.MyDepartmentsDirectors.FirstOrDefault(gd => gd.UserId.Equals(directorId));

        //            var directorsGroups = automationApiDb.MyDepartments
        //                .Where(g => g.MyDepartmentDirectorId.HasValue)
        //                .Where(g => g.MyDepartmentDirectorId.Value.Equals(directorId)).ToList();
        //            if (directorsGroups.Count > 0)
        //            {
        //                foreach (var directorsGroup in directorsGroups)
        //                {
        //                    directorsGroup.MyDepartmentDirectorId = (long?)null;
        //                }

        //                automationApiDb.SaveChanges();
        //            }

        //            automationApiDb.MyDepartmentsDirectors.Remove(myDepartmentsDirectors);
        //            automationApiDb.SaveChanges();
        //            return true;
        //        }
        //    }
        //    catch (Exception exc)
        //    { }
        //    return false;
        //}

        #endregion

        #region Methods For Work With NodeTypes

        public List<NodeTypesVM> GetAllNodeTypesList(bool? isShow)
        {
            List<NodeTypesVM> nodeTypesVMList = new List<NodeTypesVM>();

            try
            {
                var list = automationApiDb.NodeTypes.AsQueryable();

                if (isShow.HasValue)
                    list = list.Where(s => s.IsShow.Equals(isShow.Value));

                nodeTypesVMList = _mapper.Map<List<NodeTypes>, List<NodeTypesVM>>(list.ToList());
            }
            catch (Exception exc)
            { }
            return nodeTypesVMList;
        }

        #endregion

        #region Methods For Work With OrgChartNodes

        public OrgChartNodesVM GetFirstOrgChartNode(long userId)
        {
            OrgChartNodesVM orgChartNodesVM = new OrgChartNodesVM();

            try
            {
                if (automationApiDb.OrgChartNodes.Where(n => n.UserIdCreator.Value.Equals(userId)).Any())
                {
                    if (!automationApiDb.BoardMembers.Where(m => m.UserIdCreator.Value.Equals(userId)).Any())
                    {
                        orgChartNodesVM = _mapper.Map<OrgChartNodes, OrgChartNodesVM>(
                            automationApiDb.OrgChartNodes.Where(n => n.UserIdCreator.Value.Equals(userId)).FirstOrDefault());
                    }
                    else
                    {
                        orgChartNodesVM = _mapper.Map<OrgChartNodes, OrgChartNodesVM>(
                            automationApiDb.OrgChartNodes.FirstOrDefault());
                    }
                }
            }
            catch (Exception exc)
            { }

            return orgChartNodesVM;
        }

        public List<OrgChartNodes> GetHierarchyOfOrgChartNodeIds(List<long> childsUsersIds,
            int orgChartNodeId,
            bool addSelf)
        {
            List<OrgChartNodes> orgChartNodesList = new List<OrgChartNodes>();

            try
            {
                if (automationApiDb.OrgChartNodes.Where(o => childsUsersIds.Contains(o.UserIdCreator.Value) &&
                    o.OrgChartNodeId.Equals(orgChartNodeId)).Any())
                {
                    List<int> orgChartNodeIds = new List<int>();

                    ResourceManager rm = new ResourceManager("FrameWork.Resources.SpResources",
                                    Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                        "\\FrameWork.dll"));

                    String sp = rm.GetString("GetNodeIdsWithParentNodeId");

                    sp = sp.Replace("@ParentOrgChartNodeId", orgChartNodeId.ToString());

                    orgChartNodeIds = automationApiDb.OrgChartNodeIds.FromSqlRaw(sp).AsEnumerable().Select(u => u.OrgChartNodeId).ToList();

                    if (addSelf)
                        orgChartNodeIds.Add(orgChartNodeId);

                    var list = automationApiDb.OrgChartNodes.Where(o => orgChartNodeIds.Contains(o.OrgChartNodeId)).AsQueryable();

                    orgChartNodesList = list.ToList();
                }
            }
            catch (Exception exc)
            { }

            return orgChartNodesList;
        }

        public List<OrgChartNodesVM> GetHierarchyOfOrgChartNodes(List<long> childsUsersIds,
            int orgChartNodeId,
            long userId,
            ref OrgChartNodesVM orgChartNodesVM)
        {
            List<OrgChartNodesVM> orgChartNodesVMList = new List<OrgChartNodesVM>();

            try
            {
                if (automationApiDb.OrgChartNodes.Where(o => childsUsersIds.Contains(o.UserIdCreator.Value)/* &&
                    o.OrgChartNodeId.Equals(orgChartNodeId)*/).Any())
                {
                    List<int> orgChartNodeIds = new List<int>();

                    int parentOrgChartNodeId = 0;

                    if (orgChartNodeId == 0)
                    {
                        orgChartNodesVM = _mapper.Map<OrgChartNodes, OrgChartNodesVM>(automationApiDb.OrgChartNodes.Where(n => n.UserIdCreator.Value.Equals(userId)).FirstOrDefault());
                        parentOrgChartNodeId = orgChartNodesVM.OrgChartNodeId;
                    }
                    else
                    {
                        orgChartNodesVM = _mapper.Map<OrgChartNodes, OrgChartNodesVM>(automationApiDb.OrgChartNodes.Where(o => o.OrgChartNodeId.Equals(orgChartNodeId)).FirstOrDefault());
                        parentOrgChartNodeId = orgChartNodeId;
                    }

                    ResourceManager rm = new ResourceManager("FrameWork.Resources.SpResources",
                                    Assembly.LoadFile(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                        "\\FrameWork.dll"));

                    String sp = rm.GetString("GetNodeIdsWithParentNodeId");

                    sp = sp.Replace("@ParentOrgChartNodeId", parentOrgChartNodeId.ToString());

                    orgChartNodeIds = automationApiDb.OrgChartNodeIds.FromSqlRaw(sp).AsEnumerable().Select(u => u.OrgChartNodeId).ToList();

                    var list = automationApiDb.OrgChartNodes.Where(o => orgChartNodeIds.Contains(o.OrgChartNodeId)).AsQueryable();

                    if (!automationApiDb.BoardMembers.Where(m => m.UserIdCreator.Value.Equals(userId)).Any())
                    {
                        orgChartNodesVMList = _mapper.Map<List<OrgChartNodes>, List<OrgChartNodesVM>>(list.Where(a => childsUsersIds.Contains(a.UserIdCreator.Value)).ToList());
                    }
                    else
                    {
                        orgChartNodesVMList = _mapper.Map<List<OrgChartNodes>, List<OrgChartNodesVM>>(list.ToList());
                    }
                }
            }
            catch (Exception exc)
            { }

            return orgChartNodesVMList;
        }

        public List<OrgChartNodesVM> GetAllOrgChartNodesList(List<long> childsUsersIds)
        {
            List<OrgChartNodesVM> orgChartNodesVMList = new List<OrgChartNodesVM>();

            try
            {
                var list = automationApiDb.OrgChartNodes.Where(c => c.IsDeleted.Value.Equals(false) && c.IsActivated.Value.Equals(true)).AsQueryable();

                orgChartNodesVMList = _mapper.Map<List<OrgChartNodes>, List<OrgChartNodesVM>>(list.OrderByDescending(s => s.OrgChartNodeId).ToList());
            }
            catch (Exception exc)
            { }

            return orgChartNodesVMList;
        }

        public List<OrgChartNodesVM> GetListOfOrgChartNodes(int jtStartIndex,
            int jtPageSize,
            ref int listCount,
            List<long> childsUsersIds,
            string jtSorting = null)
        {
            List<OrgChartNodesVM> orgChartNodesVMList = new List<OrgChartNodesVM>();

            try
            {
                var list = automationApiDb.OrgChartNodes.Where(c => c.IsDeleted.Value.Equals(false) && c.IsActivated.Value.Equals(true)).AsQueryable();

                if (string.IsNullOrEmpty(jtSorting))
                {
                    listCount = list.Count();

                    if (listCount > jtPageSize)
                    {
                        //zonesVMList = _mapper.Map<List<Zones>, List<ZonesVM>>(list.OrderByDescending(s => s.ZoneId)
                        //         .Skip(jtStartIndex).Take(jtPageSize).ToList());

                        orgChartNodesVMList = _mapper.Map<List<OrgChartNodes>, List<OrgChartNodesVM>>(list.OrderByDescending(s => s.OrgChartNodeId)
                                 .Skip(jtStartIndex).Take(jtPageSize).ToList());
                    }
                    else
                    {
                        //zonesVMList = _mapper.Map<List<Zones>,
                        //    List<ZonesVM>>(list.OrderByDescending(s => s.ZoneId).ToList());

                        orgChartNodesVMList = _mapper.Map<List<OrgChartNodes>, List<OrgChartNodesVM>>(list.OrderByDescending(s => s.OrgChartNodeId).ToList());
                    }
                }
                else
                {
                    listCount = list.Count();

                    if (listCount > jtPageSize)
                    {
                        switch (jtSorting)
                        {
                            case "NodeTitle ASC":
                                list = list.OrderBy(l => l.NodeTitle);
                                break;
                            case "NodeTitle DESC":
                                list = list.OrderByDescending(l => l.NodeTitle);
                                break;
                        }
                        //zonesVMList = _mapper.Map<List<Zones>,
                        //    List<ZonesVM>>(list.OrderByDescending(s => s.ZoneId).Skip(jtStartIndex).Take(jtPageSize).ToList());

                        if (string.IsNullOrEmpty(jtSorting))
                            orgChartNodesVMList = _mapper.Map<List<OrgChartNodes>, List<OrgChartNodesVM>>(list.OrderByDescending(s => s.OrgChartNodeId)
                                     .Skip(jtStartIndex).Take(jtPageSize).ToList());
                        else
                            orgChartNodesVMList = _mapper.Map<List<OrgChartNodes>, List<OrgChartNodesVM>>(list.Skip(jtStartIndex).Take(jtPageSize).ToList());
                    }
                    else
                    {
                        //zonesVMList = _mapper.Map<List<Zones>,
                        //    List<ZonesVM>>(list.ToList());

                        orgChartNodesVMList = _mapper.Map<List<OrgChartNodes>, List<OrgChartNodesVM>>(list.ToList());
                    }
                }
            }
            catch (Exception exc)
            { }

            return orgChartNodesVMList;
        }

        public bool AddToOrgChartNodes(OrgChartNodesVM orgChartNodesVM,/*,
            List<long> childsUsersIds*/
            ref string returnMessage,
            IConsoleBusiness consoleBusiness)
        {
            returnMessage = "";
            using (var dbContextTransaction = automationApiDb.Database.BeginTransaction())
            {
                try
                {
                    long parentUserId = 0;
                    OrgChartNodes orgChartNodes = new OrgChartNodes();

                    if (orgChartNodesVM.UserIds != null)
                        if (orgChartNodesVM.UserIds.Count > 0)
                        {
                            if (orgChartNodesVM.ParentOrgChartNodeId.HasValue)
                                if (orgChartNodesVM.ParentOrgChartNodeId.Value > 0)
                                {
                                    if (orgChartNodesVM.NodeTypeId.Equals(8))
                                    {
                                        var parentOrgChartNode = automationApiDb.OrgChartNodes.Where(n => n.OrgChartNodeId.Equals(orgChartNodesVM.ParentOrgChartNodeId.Value)).FirstOrDefault();
                                        if (parentOrgChartNode != null)
                                        {
                                            int myDepartmentId = 0;

                                            if (parentOrgChartNode.NodeTypeId.Equals(6) ||
                                                parentOrgChartNode.NodeTypeId.Equals(7) ||
                                                parentOrgChartNode.NodeTypeId.Equals(9))
                                            {
                                                var parentMyDepartments = automationApiDb.MyDepartments.Where(d => d.OrgChartNodeId.Equals(parentOrgChartNode.OrgChartNodeId)).FirstOrDefault();
                                                if (parentMyDepartments != null)
                                                {
                                                    parentUserId = parentMyDepartments.UserIdCreator.Value;
                                                    myDepartmentId = parentMyDepartments.MyDepartmentId;
                                                }
                                            }
                                            else
                                                if (parentOrgChartNode.NodeTypeId.Equals(8))
                                            {
                                                var parentOfParentOrgChartNode = automationApiDb.OrgChartNodes.Where(n => n.OrgChartNodeId.
                                                        Equals(parentOrgChartNode.ParentOrgChartNodeId.Value)).FirstOrDefault();
                                                if (parentOfParentOrgChartNode != null)
                                                {
                                                    if (parentOfParentOrgChartNode.NodeTypeId.Equals(8))
                                                    {
                                                        returnMessage = "این تعریف مجاز نیست";
                                                        dbContextTransaction.Rollback();

                                                        return false;
                                                    }

                                                    var parentOfparentMyDepartments = automationApiDb.MyDepartments.Where(d => d.OrgChartNodeId.Equals(parentOfParentOrgChartNode.OrgChartNodeId)).FirstOrDefault();
                                                    if (parentOfparentMyDepartments != null)
                                                    {
                                                        myDepartmentId = parentOfparentMyDepartments.MyDepartmentId;
                                                    }
                                                }

                                                parentUserId = parentOrgChartNode.UserIdCreator.Value;
                                            }

                                            foreach (var userId in orgChartNodesVM.UserIds)
                                            {
                                                orgChartNodes = new OrgChartNodes();
                                                orgChartNodes = _mapper.Map<OrgChartNodesVM, OrgChartNodes>(orgChartNodesVM);

                                                orgChartNodes.UserIdCreator = userId;

                                                automationApiDb.OrgChartNodes.Add(orgChartNodes);
                                                automationApiDb.SaveChanges();

                                                #region create DepartmentsStaff

                                                DepartmentsStaff departmentsStaff = new DepartmentsStaff();

                                                departmentsStaff.MyDepartmentId = myDepartmentId;
                                                departmentsStaff.UserId = userId;

                                                departmentsStaff.CreateEnDate = DateTime.Now;
                                                departmentsStaff.CreateTime = PersianDate.TimeNow;
                                                departmentsStaff.UserIdCreator = userId;

                                                departmentsStaff.IsActivated = true;
                                                departmentsStaff.IsDeleted = false;

                                                automationApiDb.DepartmentsStaff.Add(departmentsStaff);
                                                automationApiDb.SaveChanges();

                                                #endregion

                                                if (parentUserId > 0)
                                                {
                                                    var curentUser = consoleBusiness.CmsDb.Users.Where(u => u.UserId.Equals(orgChartNodes.UserIdCreator.Value)).FirstOrDefault();
                                                    curentUser.ParentUserId = parentUserId;
                                                    consoleBusiness.CmsDb.Entry<Users>(curentUser).State = EntityState.Modified;
                                                    consoleBusiness.CmsDb.SaveChanges();
                                                }
                                            }

                                            dbContextTransaction.Commit();

                                            return true;

                                        }
                                    }
                                }

                            returnMessage = "خطا در داده های ورودی";

                            dbContextTransaction.Rollback();

                            return false;
                        }

                    orgChartNodes = _mapper.Map<OrgChartNodesVM, OrgChartNodes>(orgChartNodesVM);

                    automationApiDb.OrgChartNodes.Add(orgChartNodes);
                    automationApiDb.SaveChanges();

                    parentUserId = 0;

                    switch (orgChartNodes.NodeTypeId)
                    {
                        case 1://ساختار سازمانی
                            break;
                        case 2://هیئت مدیره
                            break;
                        case 4://گروه
                        case 5://شرکت
                            if (orgChartNodesVM.ParentOrgChartNodeId.HasValue)
                                if (orgChartNodesVM.ParentOrgChartNodeId.Value > 0)
                                {
                                    if (!automationApiDb.OrgChartNodes.Where(n => n.ParentOrgChartNodeId.HasValue).
                                        Where(n => n.ParentOrgChartNodeId.Value.Equals(orgChartNodesVM.ParentOrgChartNodeId.Value) &&
                                        !n.OrgChartNodeId.Equals(orgChartNodes.OrgChartNodeId) &&
                                        n.NodeTitle.Equals(orgChartNodesVM.NodeTitle)).Any())
                                    {
                                        int parentMyCompanyId = 0;

                                        var parentOrgChartNode = automationApiDb.OrgChartNodes.Where(n => n.OrgChartNodeId.Equals(orgChartNodesVM.ParentOrgChartNodeId.Value)).FirstOrDefault();
                                        if (parentOrgChartNode != null)
                                        {
                                            switch (parentOrgChartNode.NodeTypeId)
                                            {
                                                case 1:
                                                    returnMessage = "خطا در داده های ورودی";

                                                    dbContextTransaction.Rollback();

                                                    return false;
                                                    break;
                                                //case 2:

                                                case 4:
                                                case 5:
                                                    var parentMyCompanies = automationApiDb.MyCompanies.Where(c => c.OrgChartNodeId.Equals(parentOrgChartNode.OrgChartNodeId)).FirstOrDefault();
                                                    if (parentMyCompanies != null)
                                                    {
                                                        parentMyCompanyId = parentMyCompanies.MyCompanyId;
                                                        parentUserId = parentMyCompanies.UserIdCreator.Value;
                                                    }
                                                    break;
                                                case 6:
                                                case 7:
                                                case 8:
                                                case 9:
                                                    returnMessage = "خطا در داده های ورودی";

                                                    dbContextTransaction.Rollback();

                                                    return false;
                                                    break;
                                            }
                                        }

                                        #region create MyCompanies

                                        MyCompanies myCompanies = new MyCompanies();

                                        myCompanies.CreateEnDate = DateTime.Now;
                                        myCompanies.CreateTime = PersianDate.TimeNow;
                                        myCompanies.UserIdCreator = orgChartNodes.UserIdCreator.Value;

                                        myCompanies.ParentMyCompanyId = parentMyCompanyId;
                                        myCompanies.MyCompanyName = orgChartNodesVM.NodeTitle;
                                        myCompanies.OrgChartNodeId = orgChartNodes.OrgChartNodeId;
                                        myCompanies.IsActivated = true;
                                        myCompanies.IsDeleted = false;
                                        myCompanies.HasLimited = false;
                                        myCompanies.CentralOffice = false;

                                        automationApiDb.MyCompanies.Add(myCompanies);
                                        automationApiDb.SaveChanges();

                                        #endregion

                                        #region create MyCompaniesDirectors

                                        MyCompaniesDirectors myCompaniesDirectors = new MyCompaniesDirectors();

                                        myCompaniesDirectors.CreateEnDate = DateTime.Now;
                                        myCompaniesDirectors.CreateTime = PersianDate.TimeNow;
                                        myCompaniesDirectors.UserIdCreator = orgChartNodes.UserIdCreator.Value;

                                        myCompaniesDirectors.MyCompanyId = myCompanies.MyCompanyId;
                                        myCompaniesDirectors.UserId = orgChartNodes.UserIdCreator.Value;

                                        myCompaniesDirectors.IsActivated = true;
                                        myCompaniesDirectors.IsDeleted = false;
                                        myCompaniesDirectors.HasLimited = false;

                                        automationApiDb.MyCompaniesDirectors.Add(myCompaniesDirectors);
                                        automationApiDb.SaveChanges();

                                        #endregion
                                    }
                                    else
                                    {
                                        dbContextTransaction.Rollback();

                                        returnMessage = "نام تکراری است";
                                        return false;
                                    }
                                }
                            break;
                        case 6://معاونت
                        case 7://واحد سازمانی
                        case 9://پروژه
                            if (orgChartNodesVM.ParentOrgChartNodeId.HasValue)
                                if (orgChartNodesVM.ParentOrgChartNodeId.Value > 0)
                                {
                                    if (!automationApiDb.OrgChartNodes.Where(n => n.ParentOrgChartNodeId.HasValue).
                                        Where(n => n.ParentOrgChartNodeId.Value.Equals(orgChartNodesVM.ParentOrgChartNodeId.Value) &&
                                        !n.OrgChartNodeId.Equals(orgChartNodes.OrgChartNodeId) &&
                                        n.NodeTitle.Equals(orgChartNodesVM.NodeTitle)).Any())
                                    {
                                        int parentId = 0;
                                        string parentType = "";

                                        var parentOrgChartNode = automationApiDb.OrgChartNodes.Where(n => n.OrgChartNodeId.Equals(orgChartNodesVM.ParentOrgChartNodeId.Value)).FirstOrDefault();
                                        if (parentOrgChartNode != null)
                                        {
                                            switch (parentOrgChartNode.NodeTypeId)
                                            {
                                                case 1:
                                                case 2:
                                                    returnMessage = "خطا در داده های ورودی";

                                                    dbContextTransaction.Rollback();

                                                    return false;
                                                    break;
                                                case 4:
                                                case 5:
                                                    var parentMyCompanies = automationApiDb.MyCompanies.Where(c => c.OrgChartNodeId.Equals(parentOrgChartNode.OrgChartNodeId)).FirstOrDefault();
                                                    if (parentMyCompanies != null)
                                                    {
                                                        parentId = parentMyCompanies.MyCompanyId;
                                                        parentUserId = parentMyCompanies.UserIdCreator.Value;
                                                        parentType = "company";
                                                    }
                                                    break;
                                                case 6:
                                                case 7:
                                                case 9:
                                                    var parentMyDepartments = automationApiDb.MyDepartments.Where(d => d.OrgChartNodeId.Equals(parentOrgChartNode.OrgChartNodeId)).FirstOrDefault();
                                                    if (parentMyDepartments != null)
                                                    {
                                                        parentId = parentMyDepartments.MyDepartmentId;
                                                        parentUserId = parentMyDepartments.UserIdCreator.Value;
                                                        parentType = "department";
                                                    }
                                                    break;
                                                case 8:
                                                    returnMessage = "خطا در داده های ورودی";

                                                    dbContextTransaction.Rollback();

                                                    return false;
                                                    break;
                                            }
                                        }

                                        #region create MyDepartments

                                        MyDepartments myDepartments = new MyDepartments();

                                        myDepartments.CreateEnDate = DateTime.Now;
                                        myDepartments.CreateTime = PersianDate.TimeNow;
                                        myDepartments.UserIdCreator = orgChartNodes.UserIdCreator.Value;

                                        myDepartments.ParentId = parentId;
                                        myDepartments.ParentType = parentType;
                                        myDepartments.MyDepartmentName = orgChartNodesVM.NodeTitle;
                                        myDepartments.OrgChartNodeId = orgChartNodes.OrgChartNodeId;
                                        myDepartments.IsActivated = true;
                                        myDepartments.IsDeleted = false;

                                        automationApiDb.MyDepartments.Add(myDepartments);
                                        automationApiDb.SaveChanges();

                                        #endregion

                                        #region create MyDepartmentsDirectors

                                        MyDepartmentsDirectors myDepartmentsDirectors = new MyDepartmentsDirectors();

                                        myDepartmentsDirectors.CreateEnDate = DateTime.Now;
                                        myDepartmentsDirectors.CreateTime = PersianDate.TimeNow;
                                        myDepartmentsDirectors.UserIdCreator = orgChartNodes.UserIdCreator.Value;

                                        myDepartmentsDirectors.MyDepartmentId = myDepartments.MyDepartmentId;
                                        myDepartmentsDirectors.UserId = orgChartNodes.UserIdCreator.Value;

                                        myDepartmentsDirectors.IsActivated = true;
                                        myDepartmentsDirectors.IsDeleted = false;
                                        myDepartmentsDirectors.HasLimited = false;

                                        automationApiDb.MyDepartmentsDirectors.Add(myDepartmentsDirectors);
                                        automationApiDb.SaveChanges();

                                        #endregion
                                    }
                                    else
                                    {
                                        dbContextTransaction.Rollback();

                                        returnMessage = "نام تکراری است";
                                        return false;
                                    }
                                }
                            break;
                    }

                    dbContextTransaction.Commit();

                    if (parentUserId > 0)
                    {
                        var curentUser = consoleBusiness.CmsDb.Users.Where(u => u.UserId.Equals(orgChartNodes.UserIdCreator.Value)).FirstOrDefault();
                        curentUser.ParentUserId = parentUserId;
                        consoleBusiness.CmsDb.Entry<Users>(curentUser).State = EntityState.Modified;
                        consoleBusiness.CmsDb.SaveChanges();
                    }

                    return true;
                }
                catch (Exception exc)
                {
                    dbContextTransaction.Rollback();
                }
            }

            return false;
        }

        public bool ExistOrgChartNodeWithUserId(long userId)
        {
            try
            {
                if (automationApiDb.OrgChartNodes.Where(o => o.UserIdCreator.Value.Equals(userId)).Any())
                    return true;
            }
            catch (Exception exc)
            { }

            return false;
        }

        public OrgChartNodesVM GetOrgChartNodeWithOrgChartNodeId(int orgChartNodeId,
            List<long> childsUsersIds)
        {
            OrgChartNodesVM orgChartNodesVM = new OrgChartNodesVM();
            try
            {
                if (automationApiDb.OrgChartNodes.
                    Where(l => childsUsersIds.Contains(l.UserIdCreator.Value) &&
                    l.OrgChartNodeId.Equals(orgChartNodeId)).
                    Any())
                    orgChartNodesVM = _mapper.Map<OrgChartNodes, OrgChartNodesVM>(automationApiDb.OrgChartNodes
                        .Where(l => l.OrgChartNodeId.Equals(orgChartNodeId)).FirstOrDefault());
            }
            catch (Exception exc)
            { }
            return orgChartNodesVM;
        }

        public bool UpdateOrgChartNodes(OrgChartNodesVM orgChartNodesVM,
            ref string returnMessage,
            List<long> childsUsersIds,
            IConsoleBusiness consoleBusiness)
        {
            returnMessage = "";

            int orgChartNodeId = orgChartNodesVM.OrgChartNodeId;
            //int nodeTypeId = orgChartNodesVM.NodeTypeId;
            string nodeTitle = orgChartNodesVM.NodeTitle;
            string nodeDescription = orgChartNodesVM.NodeDescription;
            //int parentOrgChartNodeId = orgChartNodesVM.ParentOrgChartNodeId.HasValue ? orgChartNodesVM.ParentOrgChartNodeId.Value : 0;

            if (automationApiDb.OrgChartNodes.Where(n => n.OrgChartNodeId.Equals(orgChartNodeId) &&
                childsUsersIds.Contains(n.UserIdCreator.Value)).Any())
            {
                using (var dbContextTransaction = automationApiDb.Database.BeginTransaction())
                {
                    try
                    {
                        OrgChartNodes orgChartNode = (from c in automationApiDb.OrgChartNodes
                                                      where c.OrgChartNodeId == orgChartNodeId
                                                      select c).FirstOrDefault();

                        int parentOrgChartNodeId = orgChartNode.ParentOrgChartNodeId.HasValue ? orgChartNode.ParentOrgChartNodeId.Value : 0;

                        if (automationApiDb.OrgChartNodes.Where(n => n.ParentOrgChartNodeId.HasValue).
                            Where(n => n.ParentOrgChartNodeId.Value.Equals(orgChartNodesVM.ParentOrgChartNodeId.Value) &&
                            !n.OrgChartNodeId.Equals(orgChartNodeId) &&
                            n.NodeTitle.Equals(orgChartNodesVM.NodeTitle)).Any())
                        {
                            returnMessage = "نام تکراری است";

                            return false;
                        }

                        long oldUserIdCreator = orgChartNode.UserIdCreator.Value;

                        int nodeTypeId = orgChartNode.NodeTypeId;

                        //orgChartNode.NodeTypeId = nodeTypeId;
                        orgChartNode.NodeTitle = nodeTitle;
                        orgChartNode.NodeDescription = nodeDescription;
                        //orgChartNode.ParentOrgChartNodeId = parentOrgChartNodeId;

                        orgChartNode.EditEnDate = orgChartNodesVM.EditEnDate.Value;
                        orgChartNode.EditTime = orgChartNodesVM.EditTime;

                        if (oldUserIdCreator != orgChartNodesVM.UserIdEditor)
                            orgChartNode.UserIdCreator = orgChartNodesVM.UserIdEditor;

                        orgChartNode.UserIdEditor = orgChartNodesVM.UserIdEditor;
                        orgChartNode.IsActivated = orgChartNodesVM.IsActivated;
                        orgChartNode.IsDeleted = orgChartNodesVM.IsDeleted;

                        automationApiDb.Entry<OrgChartNodes>(orgChartNode).State = EntityState.Modified;
                        automationApiDb.SaveChanges();

                        switch (nodeTypeId)
                        {
                            case 1://ساختار سازمانی
                                break;
                            case 2://هیئت مدیره
                                break;
                            case 4://گروه
                            case 5://شرکت

                                MyCompanies myCompanies = automationApiDb.MyCompanies.Where(c => c.OrgChartNodeId.HasValue).Where(c => c.OrgChartNodeId.Value.Equals(orgChartNodeId)).FirstOrDefault();
                                if (myCompanies != null)
                                {
                                    List<long> childUserIds = new List<long>();

                                    if (oldUserIdCreator != orgChartNodesVM.UserIdEditor)
                                    {
                                        var childMyCompanies = automationApiDb.MyCompanies.Where(c => c.ParentMyCompanyId.HasValue).Where(c => c.ParentMyCompanyId.Value.Equals(myCompanies.MyCompanyId)).ToList();
                                        if (childMyCompanies != null)
                                            if (childMyCompanies.Count > 0)
                                                childUserIds.AddRange(childMyCompanies.Select(c => c.UserIdCreator.Value).ToList());

                                        var childMyDepartments = automationApiDb.MyDepartments.Where(d => d.ParentId.Equals(myCompanies.MyCompanyId) && d.ParentType.Equals("company")).ToList();
                                        if (childMyDepartments != null)
                                            if (childMyDepartments.Count > 0)
                                                childUserIds.AddRange(childMyDepartments.Select(c => c.UserIdCreator.Value).ToList());
                                    }

                                    myCompanies.MyCompanyName = nodeTitle;

                                    if (oldUserIdCreator != orgChartNodesVM.UserIdEditor)
                                        myCompanies.UserIdCreator = orgChartNode.UserIdCreator.Value;

                                    automationApiDb.Entry<MyCompanies>(myCompanies).State = EntityState.Modified;
                                    automationApiDb.SaveChanges();

                                    MyCompaniesDirectors myCompaniesDirectors = new MyCompaniesDirectors();

                                    myCompaniesDirectors.CreateEnDate = DateTime.Now;
                                    myCompaniesDirectors.CreateTime = PersianDate.TimeNow;
                                    myCompaniesDirectors.UserIdCreator = orgChartNode.UserIdCreator.Value;

                                    myCompaniesDirectors.MyCompanyId = myCompanies.MyCompanyId;
                                    myCompaniesDirectors.UserId = orgChartNode.UserIdCreator.Value;

                                    myCompaniesDirectors.IsActivated = true;
                                    myCompaniesDirectors.IsDeleted = false;
                                    myCompaniesDirectors.HasLimited = false;

                                    automationApiDb.MyCompaniesDirectors.Add(myCompaniesDirectors);
                                    automationApiDb.SaveChanges();

                                    try
                                    {
                                        if (oldUserIdCreator != orgChartNodesVM.UserIdEditor)
                                        {
                                            var parentOrgChartNode = automationApiDb.OrgChartNodes.Where(n => n.OrgChartNodeId.Equals(orgChartNode.ParentOrgChartNodeId.Value)).FirstOrDefault();
                                            if (parentOrgChartNode != null)
                                            {
                                                if (parentOrgChartNode.NodeTypeId.Equals(4) ||
                                                    parentOrgChartNode.NodeTypeId.Equals(5))
                                                {
                                                    var user = consoleBusiness.CmsDb.Users.Where(u => u.UserId.Equals(orgChartNode.UserIdCreator.Value)).FirstOrDefault();

                                                    if (!user.UserId.Equals(parentOrgChartNode.UserIdCreator.Value))
                                                    {
                                                        user.ParentUserId = parentOrgChartNode.UserIdCreator.Value;
                                                        consoleBusiness.CmsDb.SaveChanges();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception exc)
                                    {
                                        dbContextTransaction.Rollback();
                                    }

                                    using (var consoleDbContextTransaction = consoleBusiness.CmsDb.Database.BeginTransaction())
                                    {
                                        try
                                        {
                                            if (childUserIds.Count > 0)
                                            {
                                                childUserIds.Remove(orgChartNode.UserIdCreator.Value);

                                                var users = consoleBusiness.CmsDb.Users.Where(u => childUserIds.Contains(u.UserId)).ToList();
                                                if (users != null)
                                                    if (users.Count > 0)
                                                    {
                                                        foreach (var user in users)
                                                        {
                                                            user.ParentUserId = orgChartNode.UserIdCreator.Value;
                                                        }

                                                        consoleBusiness.CmsDb.SaveChanges();
                                                        consoleDbContextTransaction.Commit();
                                                    }
                                            }
                                        }
                                        catch (Exception exc)
                                        {
                                            consoleDbContextTransaction.Rollback();
                                            dbContextTransaction.Rollback();

                                            return false;
                                        }
                                    }

                                    dbContextTransaction.Commit();
                                }

                                break;
                            case 6://معاونت
                            case 7://واحد سازمانی
                            case 9://پروژه
                                MyDepartments myDepartments = automationApiDb.MyDepartments.Where(c => c.OrgChartNodeId.HasValue).Where(c => c.OrgChartNodeId.Value.Equals(orgChartNodeId)).FirstOrDefault();
                                if (myDepartments != null)
                                {
                                    List<long> childUserIds = new List<long>();

                                    if (oldUserIdCreator != orgChartNodesVM.UserIdEditor)
                                    {
                                        var childMyDepartments = automationApiDb.MyDepartments.Where(d => d.ParentId.Equals(myDepartments.MyDepartmentId) && d.ParentType.Equals("department")).ToList();
                                        if (childMyDepartments != null)
                                            if (childMyDepartments.Count > 0)
                                                childUserIds.AddRange(childMyDepartments.Select(c => c.UserIdCreator.Value).ToList());

                                        var childNodes = automationApiDb.OrgChartNodes.Where(n => n.ParentOrgChartNodeId.HasValue).
                                            Where(n => n.ParentOrgChartNodeId.Value.Equals(orgChartNodeId) && n.NodeTypeId.Equals(8)).ToList();
                                        if (childNodes != null)
                                            if (childNodes.Count > 0)
                                                childUserIds.AddRange(childNodes.Select(c => c.UserIdCreator.Value).ToList());
                                    }

                                    myDepartments.MyDepartmentName = nodeTitle;

                                    if (oldUserIdCreator != orgChartNodesVM.UserIdEditor)
                                        myDepartments.UserIdCreator = orgChartNode.UserIdCreator.Value;

                                    automationApiDb.Entry<MyDepartments>(myDepartments).State = EntityState.Modified;
                                    automationApiDb.SaveChanges();

                                    MyDepartmentsDirectors myDepartmentsDirectors = new MyDepartmentsDirectors();

                                    myDepartmentsDirectors.CreateEnDate = DateTime.Now;
                                    myDepartmentsDirectors.CreateTime = PersianDate.TimeNow;
                                    myDepartmentsDirectors.UserIdCreator = orgChartNode.UserIdCreator.Value;

                                    myDepartmentsDirectors.MyDepartmentId = myDepartments.MyDepartmentId;
                                    myDepartmentsDirectors.UserId = orgChartNode.UserIdCreator.Value;

                                    myDepartmentsDirectors.IsActivated = true;
                                    myDepartmentsDirectors.IsDeleted = false;
                                    myDepartmentsDirectors.HasLimited = false;

                                    automationApiDb.MyDepartmentsDirectors.Add(myDepartmentsDirectors);
                                    automationApiDb.SaveChanges();

                                    try
                                    {
                                        if (oldUserIdCreator != orgChartNodesVM.UserIdEditor)
                                        {
                                            var parentOrgChartNode = automationApiDb.OrgChartNodes.Where(n => n.OrgChartNodeId.Equals(orgChartNode.ParentOrgChartNodeId.Value)).FirstOrDefault();
                                            if (parentOrgChartNode != null)
                                            {
                                                if (parentOrgChartNode.NodeTypeId.Equals(4) ||
                                                    parentOrgChartNode.NodeTypeId.Equals(5) ||
                                                    parentOrgChartNode.NodeTypeId.Equals(6) ||
                                                    parentOrgChartNode.NodeTypeId.Equals(7) ||
                                                    parentOrgChartNode.NodeTypeId.Equals(9))
                                                {
                                                    var user = consoleBusiness.CmsDb.Users.Where(u => u.UserId.Equals(orgChartNode.UserIdCreator.Value)).FirstOrDefault();

                                                    if (!user.UserId.Equals(parentOrgChartNode.UserIdCreator.Value))
                                                    {
                                                        user.ParentUserId = parentOrgChartNode.UserIdCreator.Value;
                                                        consoleBusiness.CmsDb.SaveChanges();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception exc)
                                    {
                                        dbContextTransaction.Rollback();
                                        return false;
                                    }

                                    using (var consoleDbContextTransaction = consoleBusiness.CmsDb.Database.BeginTransaction())
                                    {
                                        try
                                        {
                                            if (childUserIds.Count > 0)
                                            {
                                                childUserIds.Remove(orgChartNode.UserIdCreator.Value);

                                                try
                                                {
                                                    var users = consoleBusiness.CmsDb.Users.Where(u => childUserIds.Contains(u.UserId)).ToList();
                                                    if (users != null)
                                                        if (users.Count > 0)
                                                        {
                                                            foreach (var user in users)
                                                            {
                                                                user.ParentUserId = orgChartNode.UserIdCreator.Value;
                                                            }

                                                            consoleBusiness.CmsDb.SaveChanges();
                                                            consoleDbContextTransaction.Commit();

                                                            //return false;
                                                        }
                                                }
                                                catch (Exception exc)
                                                {
                                                    consoleDbContextTransaction.Rollback();
                                                    dbContextTransaction.Rollback();

                                                    return false;
                                                }
                                            }
                                        }
                                        catch (Exception exc)
                                        {
                                            consoleDbContextTransaction.Rollback();
                                            dbContextTransaction.Rollback();

                                            return false;
                                        }
                                    }

                                    dbContextTransaction.Commit();
                                }

                                break;
                            case 8://شخص
                                dbContextTransaction.Commit();
                                break;
                        }

                        return true;
                    }
                    catch (Exception exc)
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }

            return false;
        }

        public bool ToggleActivationOrgChartNodes(int orgChartNodeId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var orgChartNodes = (from c in automationApiDb.OrgChartNodes
                                     where c.OrgChartNodeId == orgChartNodeId &&
                                     childsUsersIds.Contains(c.UserIdCreator.Value)
                                     select c).FirstOrDefault();

                if (orgChartNodes != null)
                {
                    orgChartNodes.IsActivated = !orgChartNodes.IsActivated;
                    orgChartNodes.EditEnDate = DateTime.Now;
                    orgChartNodes.EditTime = PersianDate.TimeNow;
                    orgChartNodes.UserIdEditor = userId;

                    automationApiDb.Entry<OrgChartNodes>(orgChartNodes).State = EntityState.Modified;
                    automationApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool TemporaryDeleteOrgChartNodes(int orgChartNodeId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var orgChartNodes = (from c in automationApiDb.OrgChartNodes
                                     where c.OrgChartNodeId == orgChartNodeId &&
                                     childsUsersIds.Contains(c.UserIdCreator.Value)
                                     select c).FirstOrDefault();

                if (orgChartNodes != null)
                {
                    orgChartNodes.IsDeleted = !orgChartNodes.IsDeleted;
                    orgChartNodes.EditEnDate = DateTime.Now;
                    orgChartNodes.EditTime = PersianDate.TimeNow;
                    orgChartNodes.UserIdEditor = userId;

                    automationApiDb.Entry<OrgChartNodes>(orgChartNodes).State = EntityState.Modified;
                    automationApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool CompleteDeleteOrgChartNodes(int orgChartNodeId,
            List<long> childsUsersIds,
            long userId,
            IConsoleBusiness consoleBusiness)
        {
            using (var dbContextTransaction = automationApiDb.Database.BeginTransaction())
            {
                try
                {
                    var orgChartNodes = GetHierarchyOfOrgChartNodeIds(childsUsersIds, orgChartNodeId, true);

                    if (orgChartNodes != null)
                        if (orgChartNodes.Count > 0)
                        {
                            List<int> orgChartNodeIds = orgChartNodes.Select(n => n.OrgChartNodeId).ToList();
                            List<long> childNodeUserIds = orgChartNodes.Select(n => n.UserIdCreator.Value).ToList();

                            var myDepartmentsNodes = orgChartNodes.Where(n => n.NodeTypeId.Equals(6) || n.NodeTypeId.Equals(7) || n.NodeTypeId.Equals(9)).ToList();

                            #region remove MyDepartments

                            var myDepartmentsNodeIds = myDepartmentsNodes.Select(n => n.OrgChartNodeId).ToList();

                            var myDepartments = automationApiDb.MyDepartments.Where(d => d.OrgChartNodeId.HasValue).Where(d => myDepartmentsNodeIds.Contains(d.OrgChartNodeId.Value)).ToList();
                            if (myDepartments != null)
                                if (myDepartments.Count > 0)
                                {
                                    var myDepartmentIds = myDepartments.Select(d => d.MyDepartmentId).ToList();

                                    if (myDepartmentIds != null)
                                        if (myDepartmentIds.Count > 0)
                                        {
                                            #region remove MyDepartmentsDirectors

                                            var myDepartmentsDirectors = automationApiDb.MyDepartmentsDirectors.Where(dd => myDepartmentIds.Contains(dd.MyDepartmentId)).ToList();
                                            automationApiDb.MyDepartmentsDirectors.RemoveRange(myDepartmentsDirectors);
                                            automationApiDb.SaveChanges();

                                            #endregion

                                            #region remove DepartmentsStaff

                                            var departmentsStaff = automationApiDb.DepartmentsStaff.Where(dd => myDepartmentIds.Contains(dd.MyDepartmentId)).ToList();
                                            automationApiDb.DepartmentsStaff.RemoveRange(departmentsStaff);
                                            automationApiDb.SaveChanges();

                                            #endregion
                                        }

                                    automationApiDb.MyDepartments.RemoveRange(myDepartments);
                                    automationApiDb.SaveChanges();
                                }

                            #endregion

                            var MyCompaniesNodes = orgChartNodes.Where(n => n.NodeTypeId.Equals(4) || n.NodeTypeId.Equals(5)).ToList();

                            #region remove MyCompanies

                            var myCompaniesNodeIds = MyCompaniesNodes.Select(n => n.OrgChartNodeId).ToList();

                            var myCompanies = automationApiDb.MyCompanies.Where(d => d.OrgChartNodeId.HasValue).Where(d => myCompaniesNodeIds.Contains(d.OrgChartNodeId.Value)).ToList();
                            if (myCompanies != null)
                                if (myCompanies.Count > 0)
                                {
                                    var myCompanyIds = myCompanies.Select(d => d.MyCompanyId).ToList();

                                    if (myCompanyIds != null)
                                        if (myCompanyIds.Count > 0)
                                        {
                                            #region remove MyCompaniesDirectors

                                            var myCompaniesDirectors = automationApiDb.MyCompaniesDirectors.Where(cd => myCompanyIds.Contains(cd.MyCompanyId)).ToList();
                                            automationApiDb.MyCompaniesDirectors.RemoveRange(myCompaniesDirectors);
                                            automationApiDb.SaveChanges();

                                            #endregion
                                        }

                                    automationApiDb.MyCompanies.RemoveRange(myCompanies);
                                    automationApiDb.SaveChanges();
                                }

                            #endregion

                            automationApiDb.OrgChartNodes.RemoveRange(orgChartNodes);
                            automationApiDb.SaveChanges();

                            using (var consoleDbContextTransaction = consoleBusiness.CmsDb.Database.BeginTransaction())
                            {
                                if (childNodeUserIds.Count > 0)
                                {
                                    childNodeUserIds.Remove(userId);

                                    try
                                    {
                                        var users = consoleBusiness.CmsDb.Users.Where(u => childNodeUserIds.Contains(u.UserId)).ToList();
                                        foreach (var user in users)
                                        {
                                            user.ParentUserId = userId;
                                        }

                                        consoleBusiness.CmsDb.SaveChanges();
                                        consoleDbContextTransaction.Commit();
                                    }
                                    catch (Exception exc)
                                    {
                                        consoleDbContextTransaction.Rollback();
                                        dbContextTransaction.Rollback();

                                        return false;
                                    }
                                }
                            }

                            dbContextTransaction.Commit();

                            return true;
                        }
                }
                catch (Exception exc)
                {
                    dbContextTransaction.Rollback();
                }
            }

            return false;
        }

        #endregion

        #region Methods For Work With OrganizationalPositions

        public List<OrganizationalPositionsVM> GetAllOrganizationalPositionsList(
            ref int listCount,
            List<long> childsUsersIds,
            string organizationalPositionName = "")
        {
            List<OrganizationalPositionsVM> organizationalPositionsList = new List<OrganizationalPositionsVM>();

            try
            {
                var list = (from o in automationApiDb.OrganizationalPositions
                            where childsUsersIds.Contains(o.UserIdCreator.Value) &&
                            o.IsActivated.Value.Equals(true) &&
                            o.IsActivated.Value.Equals(false)
                            select new OrganizationalPositionsVM
                            {
                                OrganizationalPositionId = o.OrganizationalPositionId,
                                OrganizationalPositionName = o.OrganizationalPositionName,
                                UserIdCreator = o.UserIdCreator.Value,
                                CreateEnDate = o.CreateEnDate,
                                CreateTime = o.CreateTime,
                                EditEnDate = o.EditEnDate,
                                EditTime = o.EditTime,
                                UserIdEditor = o.UserIdEditor.Value,
                                RemoveEnDate = o.RemoveEnDate,
                                RemoveTime = o.EditTime,
                                UserIdRemover = o.UserIdRemover.Value,
                                IsActivated = o.IsActivated,
                                IsDeleted = o.IsDeleted
                            })
                      .AsEnumerable();



                if (!string.IsNullOrEmpty(organizationalPositionName))
                {
                    list = list.Where(a => a.OrganizationalPositionName.Contains(organizationalPositionName));
                }


                organizationalPositionsList = list.OrderByDescending(s => s.OrganizationalPositionId).ToList();



            }
            catch (Exception ex)
            { }

            return organizationalPositionsList;
        }

        public List<OrganizationalPositionsVM> GetListOfOrganizationalPositions(
            int jtStartIndex,
            int jtPageSize,
            ref int listCount,
            string organizationalPositionName = "",
            string jtSorting = null)
        {
            List<OrganizationalPositionsVM> organizationalPositionsList = new List<OrganizationalPositionsVM>();


            var list = (from o in automationApiDb.OrganizationalPositions
                        where o.IsActivated.Value.Equals(true) &&
                              o.IsDeleted.Value.Equals(false)
                        select new OrganizationalPositionsVM
                        {
                            OrganizationalPositionId = o.OrganizationalPositionId,
                            OrganizationalPositionName = o.OrganizationalPositionName,
                            UserIdCreator = o.UserIdCreator.Value,
                            CreateEnDate = o.CreateEnDate,
                            CreateTime = o.CreateTime,
                            EditEnDate = o.EditEnDate,
                            EditTime = o.EditTime,
                            UserIdEditor = o.UserIdEditor.Value,
                            RemoveEnDate = o.RemoveEnDate,
                            RemoveTime = o.EditTime,
                            UserIdRemover = o.UserIdRemover.Value,
                            IsActivated = o.IsActivated,
                            IsDeleted = o.IsDeleted
                        })
                        .AsEnumerable();




            if (!string.IsNullOrEmpty(organizationalPositionName))
                list = list.Where(z => z.OrganizationalPositionName.Contains(organizationalPositionName));


            try
            {
                if (string.IsNullOrEmpty(jtSorting))
                {
                    listCount = list.Count();

                    if (listCount > jtPageSize)
                    {

                        organizationalPositionsList = list.OrderByDescending(s => s.OrganizationalPositionId)
                                 .Skip(jtStartIndex).Take(jtPageSize).ToList();
                    }
                    else
                        organizationalPositionsList = list.OrderByDescending(s => s.OrganizationalPositionId).ToList();
                }
                else
                {
                    listCount = list.Count();

                    if (listCount > jtPageSize)
                    {
                        switch (jtSorting)
                        {
                            case "OrganizationalPositionName ASC":
                                list = list.OrderBy(l => l.OrganizationalPositionName);
                                break;
                            case "OrganizationalPositionName DESC":
                                list = list.OrderByDescending(l => l.OrganizationalPositionName);
                                break;
                        }


                        if (string.IsNullOrEmpty(jtSorting))
                            organizationalPositionsList = list.OrderByDescending(s => s.OrganizationalPositionId)
                                     .Skip(jtStartIndex).Take(jtPageSize).ToList();
                        else
                            organizationalPositionsList = list.Skip(jtStartIndex).Take(jtPageSize).ToList();
                    }
                    else
                    {

                        organizationalPositionsList = list.ToList();
                    }
                }


            }
            catch (Exception exc)
            { }
            return organizationalPositionsList;
        }

        public int AddToOrganizationalPositions(OrganizationalPositionsVM organizationalPositionsVM)
        {
            try
            {
                OrganizationalPositions organizationalPositions = _mapper.Map<OrganizationalPositionsVM, OrganizationalPositions>(organizationalPositionsVM);

                organizationalPositionsVM.UserIdCreator = organizationalPositions.UserIdCreator.Value;

                automationApiDb.OrganizationalPositions.Add(organizationalPositions);
                automationApiDb.SaveChanges();

                return organizationalPositions.OrganizationalPositionId;

            }
            catch (Exception exc)
            { }

            return 0;
        }

        public int UpdateOrganizationalPositions(ref OrganizationalPositionsVM organizationalPositionsVM,
           List<long> childsUsersIds)
        {
            int organizationalPositionId = organizationalPositionsVM.OrganizationalPositionId;
            string organizationalPositionName = organizationalPositionsVM.OrganizationalPositionName;

            bool? isActivated = organizationalPositionsVM.IsActivated.HasValue ? organizationalPositionsVM.IsActivated.Value : (bool?)true;
            bool? isDeleted = organizationalPositionsVM.IsDeleted.HasValue ? organizationalPositionsVM.IsDeleted.Value : (bool?)true;



            if (automationApiDb.OrganizationalPositions.Where(n => childsUsersIds.Contains(n.UserIdCreator.Value)).Where(o => o.OrganizationalPositionId.Equals(organizationalPositionId)).Any())
            {
                try
                {
                    OrganizationalPositions organizationalPositions = (from o in automationApiDb.OrganizationalPositions
                                                                       where o.OrganizationalPositionId == organizationalPositionId
                                                                       select o).FirstOrDefault();

                    organizationalPositions.OrganizationalPositionName = organizationalPositionName;

                    organizationalPositions.EditEnDate = DateTime.Now;
                    organizationalPositions.EditTime = PersianDate.TimeNow;
                    organizationalPositions.UserIdEditor = organizationalPositions.UserIdEditor.Value;
                    organizationalPositions.IsActivated = isActivated.Value;
                    organizationalPositions.IsDeleted = isDeleted.Value;


                    automationApiDb.Entry<OrganizationalPositions>(organizationalPositions).State = EntityState.Modified;
                    automationApiDb.SaveChanges();




                    return organizationalPositions.OrganizationalPositionId;
                }
                catch (Exception ex)
                { }

            }
            return 0;
        }

        public bool ToggleActivationOrganizationalPositions(int organizationalPositionId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var organizationalPositions = (from o in automationApiDb.OrganizationalPositions
                                               where o.OrganizationalPositionId == organizationalPositionId &&
                                               childsUsersIds.Contains(o.UserIdCreator.Value)
                                               select o).FirstOrDefault();

                if (organizationalPositions != null)
                {
                    organizationalPositions.IsActivated = !organizationalPositions.IsActivated;
                    organizationalPositions.EditEnDate = DateTime.Now;
                    organizationalPositions.EditTime = PersianDate.TimeNow;
                    organizationalPositions.UserIdEditor = userId;

                    automationApiDb.Entry<OrganizationalPositions>(organizationalPositions).State = EntityState.Modified;
                    automationApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception)
            { }

            return false;
        }

        public bool TemporaryDeleteOrganizationalPositions(int organizationalPositionId,
           long userId,
           List<long> childsUsersIds)
        {
            try
            {
                var organizationalPositions = (from o in automationApiDb.OrganizationalPositions
                                               where o.OrganizationalPositionId == organizationalPositionId &&
                                               childsUsersIds.Contains(o.UserIdCreator.Value)
                                               select o).FirstOrDefault();


                if (organizationalPositions != null)
                {
                    organizationalPositions.IsDeleted = !organizationalPositions.IsDeleted;
                    organizationalPositions.EditEnDate = DateTime.Now;
                    organizationalPositions.EditTime = PersianDate.TimeNow;
                    organizationalPositions.UserIdEditor = userId;

                    automationApiDb.Entry<OrganizationalPositions>(organizationalPositions).State = EntityState.Modified;
                    automationApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            { }
            return false;
        }

        public bool CompleteDeleteOrganizationalPositions(int organizationalPositionId,
           List<long> childsUsersIds)
        {
            try
            {
                var organizationalPositions = (from o in automationApiDb.OrganizationalPositions
                                               where o.OrganizationalPositionId == organizationalPositionId &&
                                               childsUsersIds.Contains(o.UserIdCreator.Value)
                                               select o).FirstOrDefault();

                if (organizationalPositions != null)
                {
                    using (var transaction = automationApiDb.Database.BeginTransaction())
                    {
                        try
                        {
                            automationApiDb.OrganizationalPositions.Remove(organizationalPositions);
                            automationApiDb.SaveChanges();

                            transaction.Commit();

                            return true;
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                        }
                    }
                }
            }
            catch (Exception ex)
            { }

            return false;
        }

        public OrganizationalPositionsVM GetOrganizationalPositionWithOrganizationalPositionId(
           int? organizationalPositionId,
           List<long> childsUsersIds)
        {
            OrganizationalPositionsVM organizationalPositionsVM = new OrganizationalPositionsVM();

            try
            {
                organizationalPositionsVM = _mapper.Map<OrganizationalPositions,
                    OrganizationalPositionsVM>(automationApiDb.OrganizationalPositions
                     .Where(p => childsUsersIds.Contains(p.UserIdCreator.Value))
                     .Where(e => e.OrganizationalPositionId.Equals(organizationalPositionId)).FirstOrDefault());
            }
            catch (Exception exc)
            { }

            return organizationalPositionsVM;
        }

        #endregion

        #region Methods For Work With Staff

        public List<StaffVM> GetListOfStaff(int jtStartIndex,
            int jtPageSize,
            ref int listCount,
            List<long> childsUsersIds,
            string jtSorting = null)
        {
            List<StaffVM> staffVMList = new List<StaffVM>();
            try
            {
                var list = automationApiDb.Staff
                    .Where(c => childsUsersIds.Contains(c.UserIdCreator.Value) && c.IsDeleted.Value.Equals(false) && c.IsActivated.Value.Equals(true)).OrderByDescending(x => x.StaffId)
                    .AsQueryable();

                listCount = list.Count();
                if (string.IsNullOrEmpty(jtSorting))
                {
                    if (listCount > jtPageSize)
                    {
                        staffVMList = _mapper.Map<List<Staff>,
                            List<StaffVM>>(list
                            .OrderByDescending(s => s.UserId)
                            .Skip(jtStartIndex).Take(jtPageSize).ToList());
                    }
                    else
                    {
                        staffVMList = _mapper.Map<List<Staff>,
                            List<StaffVM>>(list
                            .OrderByDescending(s => s.UserId).ToList());
                    }
                }
                else
                {
                    //switch (jtSorting)
                    //{
                    //    case "StaffName ASC":
                    //        list = list.OrderBy(l => l.StaffName);
                    //        break;
                    //    case "StaffName DESC":
                    //        list = list.OrderByDescending(l => l.StaffName);
                    //        break;
                    //}

                    if (listCount > jtPageSize)
                    {
                        staffVMList = _mapper.Map<List<Staff>,
                            List<StaffVM>>(list.Skip(jtStartIndex).Take(jtPageSize).ToList());
                    }
                    else
                    {
                        staffVMList = _mapper.Map<List<Staff>,
                            List<StaffVM>>(list.ToList());
                    }
                }

                #region rewrite inside module

                //foreach (var staffVM in staffVMList)
                //{
                //    if (staffVM.UserIdCreator.HasValue)
                //    {
                //        var user = automationApiDb.Users.FirstOrDefault(u => u.UserId.Equals(staffVM.UserIdCreator.Value));
                //        var userDetails = automationApiDb.UsersProfile.FirstOrDefault(up => up.UserId.Equals(staffVM.UserIdCreator.Value));
                //        staffVM.UserCreatorName = user.UserName;

                //        if (!string.IsNullOrEmpty(userDetails.Name))
                //            staffVM.UserCreatorName += " - " + userDetails.Name;

                //        if (!string.IsNullOrEmpty(userDetails.Family))
                //            staffVM.UserCreatorName += " - " + userDetails.Family;
                //    }
                //}

                #endregion
            }
            catch (Exception exc)
            { }
            return staffVMList;
        }

        public long AddToStaff(StaffVM staffVM, List<long> childsUsersIds)
        {
            try
            {
                if (automationApiDb.Staff.Any(c =>
                    childsUsersIds.Contains(c.UserIdCreator.Value) &&
                    c.UserIdCreator.Value.Equals(staffVM.UserIdCreator.Value) &&
                    c.UserId.Equals(staffVM.UserId)))
                {
                    Staff staff = _mapper.Map<StaffVM, Staff>(staffVM);
                    automationApiDb.Staff.Add(staff);
                    automationApiDb.SaveChanges();
                    return staff.StaffId;
                }
            }
            catch (Exception exc)
            { }
            return 0;
        }

        public bool UpdateStaff(ref StaffVM staffVM,
            //long userId,
            List<long> childsUsersIds)
        {
            try
            {
                long staffId = staffVM.StaffId;
                long userId = staffVM.UserId;
                //string staffName = staffVM.StaffName;

                //List<long> childsUsersIds = new List<long>();
                //childsUsersIds = GetChildUserIds(ref childsUsersIds, userId).Distinct().ToList();

                //if (!automationApiDb.Staff.Any(c =>
                //        childsUsersIds.Contains(c.UserIdCreator.Value) &&
                //        //c.UserIdCreator.Value.Equals(userId) &&
                //        !c.StaffId.Equals(staffId) &&
                //        c.StaffName.Equals(staffName)))
                //{
                Staff oldStaff = (from c in automationApiDb.Staff
                                  where c.StaffId == staffId
                                  select c).FirstOrDefault();

                //oldStaff.StaffName = staffVM.StaffName;
                //oldStaff.DeviceParentCategoryId = staffVM.DeviceParentCategoryId.HasValue ?
                //    staffVM.DeviceParentCategoryId.Value :
                //    (int?)null;


                if (oldStaff.UserId != staffVM.UserId)
                {
                    var staff = automationApiDb.Staff.Any(c => childsUsersIds.Contains(c.UserIdCreator.Value) && c.UserId.Equals(userId));
                    return false;
                }

                oldStaff.EditEnDate = staffVM.EditEnDate.Value;
                oldStaff.EditTime = staffVM.EditTime;
                oldStaff.UserId = staffVM.UserId;
                // oldStaff.ContractImage = staffVM.ContractImage;
                oldStaff.PersonalCode = staffVM.PersonalCode;
                //oldStaff.CertificateImage = staffVM.CertificateImage;
                //oldStaff.NationalCodeImage = staffVM.NationalCodeImage;
                oldStaff.JobTitle = staffVM.JobTitle;
                oldStaff.Skill = staffVM.Skill;
                oldStaff.IsMarried = staffVM.IsMarried;
                oldStaff.Dependants = staffVM.Dependants;
                //oldStaff.StaffDesc = staffVM.StaffDesc;
                oldStaff.IsActivated = staffVM.IsActivated;
                oldStaff.IsDeleted = staffVM.IsDeleted;
                oldStaff.UserIdEditor = staffVM.UserIdEditor;

                automationApiDb.Entry<Staff>(oldStaff).State = EntityState.Modified;
                automationApiDb.SaveChanges();

                staffVM.UserIdCreator = oldStaff.UserIdCreator.Value;

                #region rewrite inside module

                //long userIdCreator = staffVM.UserIdCreator.Value;
                //if (staffVM.UserIdCreator.HasValue)
                //{
                //    var user = automationApiDb.Users.FirstOrDefault(u => u.UserId.Equals(userIdCreator));
                //    var userDetails = automationApiDb.UsersProfile.FirstOrDefault(up => up.UserId.Equals(userIdCreator));
                //    staffVM.UserCreatorName = user.UserName;

                //    if (!string.IsNullOrEmpty(userDetails.Name))
                //        staffVM.UserCreatorName += " - " + userDetails.Name;

                //    if (!string.IsNullOrEmpty(userDetails.Family))
                //        staffVM.UserCreatorName += " - " + userDetails.Family;
                //}

                #endregion

                return true;
                //}
            }
            catch (Exception exc)
            { }
            return false;
        }

        public bool ToggleActivationStaff(long staffId, long userId, List<long> childsUsersIds)
        {
            try
            {
                //List<long> childsUsersIds = new List<long>();
                //childsUsersIds = GetChildUserIds(ref childsUsersIds, userId).Distinct().ToList();
                var staff = (from c in automationApiDb.Staff
                             where c.StaffId == staffId &&
                             childsUsersIds.Contains(c.UserIdCreator.Value)
                             select c).FirstOrDefault();

                if (staff != null)
                {
                    staff.IsActivated = !staff.IsActivated;
                    staff.EditEnDate = DateTime.Now;
                    staff.EditTime = PersianDate.TimeNow;
                    staff.UserIdEditor = userId;
                    automationApiDb.Entry<Staff>(staff).State = EntityState.Modified;
                    automationApiDb.SaveChanges();
                    return true;
                }
            }
            catch (Exception exc)
            { }
            return false;
        }

        public bool TemporaryDeleteStaff(long staffId, long userId, List<long> childsUsersIds)
        {
            try
            {
                //List<long> childsUsersIds = new List<long>();
                //childsUsersIds = GetChildUserIds(ref childsUsersIds, userId).Distinct().ToList();
                var staff = (from c in automationApiDb.Staff
                             where c.StaffId == staffId &&
                             childsUsersIds.Contains(c.UserIdCreator.Value)
                             select c).FirstOrDefault();

                if (staff != null)
                {
                    staff.IsDeleted = !staff.IsDeleted;
                    staff.RemoveEnDate = DateTime.Now;
                    staff.RemoveTime = PersianDate.TimeNow;
                    staff.UserIdRemover = userId;
                    automationApiDb.Entry<Staff>(staff).State = EntityState.Modified;
                    automationApiDb.SaveChanges();
                    return true;
                }
            }
            catch (Exception exc)
            { }
            return false;
        }

        public bool CompleteDeleteStaff(long staffId,
            //long userId,
            List<long> childsUsersIds,
            ref string returnMessage)
        {
            returnMessage = "";

            try
            {
                //List<long> childsUsersIds = new List<long>();
                //childsUsersIds = GetChildUserIds(ref childsUsersIds, userId).Distinct().ToList();
                var staff = (from c in automationApiDb.Staff
                             where c.StaffId == staffId &&
                             childsUsersIds.Contains(c.UserIdCreator.Value)
                             select c).FirstOrDefault();

                if (staff != null)
                {
                    automationApiDb.Staff.Remove(staff);
                    automationApiDb.SaveChanges();
                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public List<StaffVM> GetAllStaffList(/*long userId,*/List<long> childsUsersIds)
        {
            List<StaffVM> staffVMList = new List<StaffVM>();
            try
            {
                var list = automationApiDb.Staff
                                .Where(a => a.IsDeleted.Value.Equals(false))
                                .Where(a => a.IsActivated.Value.Equals(true))
                                .Where(c => childsUsersIds.Contains(c.UserIdCreator.Value))
                                .AsQueryable();

                staffVMList = _mapper.Map<List<Staff>,
                                List<StaffVM>>(list
                                .OrderByDescending(s => s.UserId).ToList());
            }
            catch (Exception exc)
            { }
            return staffVMList;
        }

        public StaffVM GetStaffWithUserId(long userId, List<long> childsUsersIds)
        {
            StaffVM staffVM = new StaffVM();

            try
            {
                staffVM = _mapper.Map<Staff,
                                StaffVM>(automationApiDb.Staff
                                .Where(a => a.UserId.Equals(userId))
                                .Where(a => childsUsersIds.Contains(a.UserIdCreator.Value))
                                .FirstOrDefault());
            }
            catch (Exception exc)
            { }

            return staffVM;
        }

        public StaffVM GetStaffWithStaffId(int staffId, List<long> childsUsersIds)
        {
            StaffVM staffVM = new StaffVM();

            try
            {
                staffVM = _mapper.Map<Staff,
                                StaffVM>(automationApiDb.Staff
                                .Where(a => a.StaffId.Equals(staffId))
                                .Where(a => childsUsersIds.Contains(a.UserIdCreator.Value))
                                .FirstOrDefault());
            }
            catch (Exception exc)
            { }

            return staffVM;
        }

        public GetStaffImagesVM GetStaffImages(List<long> childsUsersIds, int staffId)
        {
            GetStaffImagesVM getStaffImagesVM = new GetStaffImagesVM();

            try
            {
                if (automationApiDb.Staff.Where(ed => childsUsersIds.Contains(ed.UserIdCreator.Value) && ed.StaffId.Equals(staffId)).Any())
                {
                    var MyStaff = automationApiDb.Staff.Where(ed => childsUsersIds.Contains(ed.UserIdCreator.Value) &&
                                                                        ed.StaffId.Equals(staffId)).FirstOrDefault();

                    getStaffImagesVM.NationalCodeImage = MyStaff.NationalCodeImage;
                    getStaffImagesVM.ContractImage = MyStaff.ContractImage;
                    getStaffImagesVM.CertificateImage = MyStaff.CertificateImage;
                }
            }
            catch (Exception exc)
            { }

            return getStaffImagesVM;
        }

        public bool UpdateStaffImages(long userId, int staffId, string contractImage, string certificateImage, string nationalCodeImage)
        {
            try
            {
                if (automationApiDb.Staff.Any(c => c.StaffId.Equals(staffId)))
                {
                    var myStaff = automationApiDb.Staff.FirstOrDefault(c => c.StaffId.Equals(staffId));

                    if (!string.IsNullOrEmpty(contractImage))
                        myStaff.ContractImage = contractImage;

                    if (!string.IsNullOrEmpty(certificateImage))
                        myStaff.CertificateImage = certificateImage;

                    if (!string.IsNullOrEmpty(nationalCodeImage))
                        myStaff.NationalCodeImage = nationalCodeImage;

                    if ((!string.IsNullOrEmpty(contractImage)) ||
                        (!string.IsNullOrEmpty(certificateImage)) ||
                        (!string.IsNullOrEmpty(nationalCodeImage)))
                    {
                        myStaff.UserIdEditor = userId;
                        myStaff.EditEnDate = DateTime.Now;
                        myStaff.EditTime = PersianDate.TimeNow;
                        automationApiDb.SaveChanges();
                        return true;
                    }
                }
            }
            catch (Exception exc)
            { }
            return false;
        }

        #endregion

        #endregion

    }
}