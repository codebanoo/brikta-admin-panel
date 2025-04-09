using APIs.Automation.Models.Entities;
using Models.Business.ConsoleBusiness;
using System.Collections.Generic;
using VM.Automation;
using VM.Public;
using System.Linq;
using VM.Teniaco;
using VM.Console;

namespace APIs.Automation.Models.Business
{
    public interface IAutomationApiBusiness
    {
        #region Automation

        #region Methods For Work With BoardMembers

        List<BoardMembersVM> GetAllBoardMembersList(int orgChartNodeId);

        bool EditBoardMembers(List<BoardMembersVM> boardMembersVMList,
            int orgChartNodeId);

        #endregion

        #region Methods For Work With DepartmentsStaff
        List<DepartmentsStaffVM> GetListOfDepartmentsStaff(int jtStartIndex,
            int jtPageSize,
            ref int listCount,
            List<long> childsUsersIds,
            string jtSorting = null);

        int AddToDepartmentsStaff(DepartmentsStaffVM departmentsStaffVM,
            List<long> childsUsersIds);

        bool UpdateDepartmentsStaff(ref DepartmentsStaffVM departmentsStaffVM,
            //long userId,
            List<long> childsUsersIds);

        bool ToggleActivationDepartmentsStaff(int departmentStaffId,
            long userId,
            List<long> childsUsersIds);

        bool TemporaryDeleteDepartmentsStaff(int departmentStaffId,
            long userId,
            List<long> childsUsersIds);

        bool CompleteDeleteDepartmentsStaff(int departmentStaffId,
            //long userId,
            List<long> childsUsersIds,
            ref string returnMessage);

        List<DepartmentsStaffVM> GetAllDepartmentsStaffList(//long userId,
            List<long> childsUsersIds);

        DepartmentsStaffVM GetDepartmentStaffWithDepartmentStaffId(int departmentStaffId,
            List<long> childsUsersIds);

        #endregion

        #region Methods For Work With Forms

        List<FormsVM> GetListOfForms(
             int jtStartIndex,
           int jtPageSize,
           ref int listCount,
           string formName);

        List<FormsVM> GetAllFormsList(ref int listCount,
            string formName);

        int AddToForms(FormsVM FormsVM, List<long> childsUsersIds);

        FormsVM GetFormWithFormId(int formId);

        FormsVM GetFormAndFieldsWithFormId(int formId);

        int UpdateForms(ref FormsVM FormsVM,
            //long userId,
            List<long> childsUsersIds);

        bool ToggleActivationForms(long formId, long userId, List<long> childsUsersIds);

        bool TemporaryDeleteForms(long formId, long userId, List<long> childsUsersIds);

        bool CompleteDeleteForms(long formId,
           List<long> childsUsersIds);

        #endregion

        #region Methods For Work With FormElements

        List<FormElementsVM> GetListOfFormElements(
            int jtStartIndex,
            int jtPageSize,
            ref int listCount,
            int? formId = null,
            string formElementTitle = "");

        List<FormElementsVM> GetAllFormElementsList(ref int listCount,
            int? formId = null,
            string formElementTitle = "");

        int AddToFormElements(FormElementsVM formElementsVM, List<long> childsUsersIds);

        FormElementsVM GetFormElementWithFormElementId(int formElementId);

        int UpdateFormElements(ref FormElementsVM formElementsVM,
            //long userId,
            List<long> childsUsersIds);

        bool ToggleActivationFormElements(int formElementId, long userId, List<long> childsUsersIds);

        bool TemporaryDeleteFormElements(int formElementId, long userId, List<long> childsUsersIds);

        bool CompleteDeleteFormElements(int formElementId,
            //long userId,
            List<long> childsUsersIds);

        #endregion

        #region Methods For Work With FormElementOptions

        List<FormElementOptionsVM> GetListOfFormElementOptions(
            int jtStartIndex,
            int jtPageSize,
            ref int listCount,
            int formElementId = 0);

        List<FormElementOptionsVM> GetAllFormElementOptionsList(ref int listCount,
            int formElementId = 0);

        int AddToFormElementOptions(FormElementOptionsVM formElementOptionsVM);

        FormElementOptionsVM GetFormElementOptionWithFormElementOptionId(int formElementOptionId);

        int UpdateFormElementOptions(ref FormElementOptionsVM formElementOptionsVM);

        bool ToggleActivationFormElementOptions(int formElementOptionId, long userId);

        bool TemporaryDeleteFormElementOptions(int formElementOptionId, long userId);

        bool CompleteDeleteFormElementOptions(int formElementOptionId);

        #endregion

        #region Methods For Work With MyCompanies

        List<MyCompaniesVM> GetMyCompaniesList(int jtStartIndex,
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
            long? StateId);

        List<MyCompaniesVM> GetAllMyCompaniesList(List<long> childsUsersIds);

        bool ExistCompanyWithCompanyName(List<long> childsUsersIds,
            MyCompaniesVM myCompaniesVM);

        int AddToMyCompanies(MyCompaniesVM myCompaniesVM);

        int AddToMyCompanies(MyCompaniesVM myCompaniesVM, List<long> childsUsersIds);

        MyCompaniesVM GetMyCompaniesWithMyCompanyId(List<long> childsUsersIds,
            int myCompanyId);

        string GetCompanyPictures(long userId,
            int myCompanyId,
            List<long> childsUsersIds,
            ref string companyMapPicture,
            ref string companyLogoPicture,
            ref string waterMarkImagePicture);
        bool UpdateCompanyPictures(long userId,
            int myCompanyId,
            string companyMapPicture,
            string companyLogoPicture,
            string waterMarkImagePicture);

        bool ToggleActivationMyCompanies(int myCompanyId, long userId, List<long> childsUsersIds);

        bool TemporaryDeleteMyCompanies(int myCompanyId, long userId, List<long> childsUsersIds);

        //bool ChangeCentralOffice(int myCompanyId, long userId);

        bool CompleteDeleteMyCompanies(int myCompanyId, List<long> childsUsersIds, ref string returnMessage);

        bool UpdateMyCompany(MyCompaniesVM myCompaniesVM, ref string returnMessage);

        bool UpdateMyCompany(ref MyCompaniesVM myCompaniesVM, List<long> childsUsersIds);

        bool CheckLimitationOfDefinedCompany(long userId, DomainLimitationsVM limitation, List<long> childsUsersIds);

        List<MyCompaniesVM> GetAllMyCompaniesList(long userId, List<long> childsUsersIds);

        bool ToggleCentralOfficeCompanies(int myCompanyId, long userId);

        bool ToggleHasLimitedCompanies(int myCompanyId, long userId);

        DomainsSettingsVM GetCompanyDomain(int myCompanyId);

        GetMyCompaniesImagesVM GetMyCompaniesImages(List<long> childsUsersIds, int MyCompanyId);

        #endregion

        #region Methods For Work With MyDepartments

        List<MyDepartmentsVM> GetListOfMyDepartments(int jtStartIndex,
            int jtPageSize,
            ref int listCount,
            string jtSorting,
            List<long> childsUsersIds,
            long userId = 0,
            string myDepartmentNameSearch = null,
            int myCompanyIdSearch = 0);

        List<MyDepartmentsVM> GetMyDepartmentsList(int DomainSettingId, List<long> childsUsersIds);

        bool ExistDepartmentWithDepartmentName(List<long> childsUsersIds, MyDepartmentsVM myDepartmentsVM);

        int AddToMyDepartments(MyDepartmentsVM myDepartmentsVM, List<long> childsUsersIds);

        bool UpdateMyDepartments(MyDepartmentsVM myDepartmentsVM, List<long> childsUsersIds, ref string returnMessage);

        bool TemporaryDeleteMyDepartments(List<long> childsUsersIds, int MyDepartmentId, long userId);

        bool ToggleActivationMyDepartment(int myDepartmentId, long userId, List<long> childsUsersIds);

        bool CompleteDeleteMyDepartments(int myDepartmentId, List<long> childsUsersIds, ref string returnMessage);

        List<MyDepartmentsVM> GetAllMyDepartmentsList(List<long> childsUsersIds, int myCompanyId);

        List<MyDepartmentsVM> GetFreeMyDepartmentsList(long userId, List<long> childsUsersIds);

        #endregion

        #region Methods For Work With MyDepartmentsDirectors

        //List<MyDepartmentsDirectorsVM> GetMyDepartmentsDirectorsList(int jtStartIndex,
        //    int jtPageSize,
        //    ref int listCount,
        //    string jtSorting,
        //    List<long> childsUsersIds,
        //    long userId = 0,
        //    string userNameSearch = null,
        //    string nameFamilySearch = null,
        //    string mobilePhoneSearch = null,
        //    string nationalCodeSearch = null);

        //List<MyDepartmentsDirectorsVM> GetAllMyDepartmentsDirectorsList(List<long> childsUsersIds);

        //List<MyDepartmentsDirectorsVM> GetMyDepartmentsDirectorsList(List<long> childsUsersIds,
        //    int directorRoleId,
        //    List<UsersRolesVM> usersRolesVMList,
        //    long userId = 0,
        //    int domainSettingId = 0);

        //bool AddToMyDepartmentsDirectors(MyDepartmentsDirectorsVM myDepartmentsDirectorsVM,
        //    List<long> childsUsersIds);

        //bool UpdateMyDepartmentDirector(ref MyDepartmentsDirectorsVM myDepartmentsDirectorsVM,
        //    List<long> childsUsersIds);

        //bool ToggleActivationMyDepartmentsDirectors(List<long> childsUsersIds,
        //    int directorId,
        //    long userId);

        //bool TemporaryDeleteMyDepartmentsDirectors(List<long> childsUsersIds,
        //    int directorId,
        //    long userId);

        //MyDepartmentsDirectorsVM GetMyDepartmentsDirectorsWithMyDepartmentDirectorId(List<long> childsUsersIds, 
        //    int directorId, 
        //    long userId);

        //bool ToggleLimitedMyDepartmentsDirectors(List<long> childsUsersIds, 
        //    int directorId, 
        //    long userId);

        //bool CompleteDeleteMyDepartmentsDirectors(List<long> childsUsersIds, 
        //    int directorId, 
        //    long userId,
        //    ref string returnMessage);

        #endregion

        #region Methods For Work With NodeTypes

        List<NodeTypesVM> GetAllNodeTypesList(bool? isShow);

        #endregion

        #region Methods For Work With OrganizationalPositions
        List<OrganizationalPositionsVM> GetAllOrganizationalPositionsList(
            ref int listCount,
            List<long> childsUsersIds,
            string organizationalPositionName = "");

        List<OrganizationalPositionsVM> GetListOfOrganizationalPositions(
            int jtStartIndex,
            int jtPageSize,
            ref int listCount,
            string organizationalPositionName = "",
            string jtSorting = null);

        int AddToOrganizationalPositions(OrganizationalPositionsVM organizationalPositionsVM);

        int UpdateOrganizationalPositions(ref OrganizationalPositionsVM organizationalPositionsVM, List<long> childsUsersIds);

        bool ToggleActivationOrganizationalPositions(int organizationalPositionId,
           long userId,
           List<long> childsUsersIds);

        bool TemporaryDeleteOrganizationalPositions(int organizationalPositionId,
          long userId,
          List<long> childsUsersIds);

        bool CompleteDeleteOrganizationalPositions(int organizationalPositionId,
          List<long> childsUsersIds);

        OrganizationalPositionsVM GetOrganizationalPositionWithOrganizationalPositionId(int? organizationalPositionId,
           List<long> childsUsersIds);


        #endregion

        #region Methods For Work With OrgChartNodes

        OrgChartNodesVM GetFirstOrgChartNode(long userId);

        List<OrgChartNodes> GetHierarchyOfOrgChartNodeIds(List<long> childsUsersIds,
            int orgChartNodeId,
            bool addSelf);

        List<OrgChartNodesVM> GetHierarchyOfOrgChartNodes(List<long> childsUsersIds,
            int orgChartNodeId,
            long userId,
            ref OrgChartNodesVM orgChartNodesVM);

        List<OrgChartNodesVM> GetAllOrgChartNodesList(List<long> childsUsersIds);

        List<OrgChartNodesVM> GetListOfOrgChartNodes(int jtStartIndex,
            int jtPageSize,
            ref int listCount,
            List<long> childsUsersIds,
            string jtSorting = null);

        bool AddToOrgChartNodes(OrgChartNodesVM orgChartNodesVM,/*,
            List<long> childsUsersIds*/
            ref string returnMessage,
            IConsoleBusiness consoleBusiness);

        bool ExistOrgChartNodeWithUserId(long userId);

        OrgChartNodesVM GetOrgChartNodeWithOrgChartNodeId(int orgChartNodeId,
            List<long> childsUsersIds);

        bool UpdateOrgChartNodes(OrgChartNodesVM orgChartNodesVM,
            ref string returnMessage,
            List<long> childsUsersIds,
            IConsoleBusiness consoleBusiness);

        bool ToggleActivationOrgChartNodes(int orgChartNodeId,
            long userId,
            List<long> childsUsersIds);

        bool TemporaryDeleteOrgChartNodes(int orgChartNodeId,
            long userId,
            List<long> childsUsersIds);

        bool CompleteDeleteOrgChartNodes(int orgChartNodeId,
            List<long> childsUsersIds,
            long userId,
            IConsoleBusiness consoleBusiness);

        #endregion

        #region Methods For Work With Staff

        List<StaffVM> GetListOfStaff(int jtStartIndex,
            int jtPageSize,
            ref int listCount,
            List<long> childsUsersIds,
            string jtSorting = null);

        long AddToStaff(StaffVM staffVM,
            List<long> childsUsersIds);

        bool UpdateStaff(ref StaffVM staffVM,
            //long userId,
            List<long> childsUsersIds);

        bool ToggleActivationStaff(long staffId,
            long userId,
            List<long> childsUsersIds);

        bool TemporaryDeleteStaff(long staffId,
            long userId,
            List<long> childsUsersIds);

        bool CompleteDeleteStaff(long staffId,
            //long userId,
            List<long> childsUsersIds,
            ref string returnMessage);

        List<StaffVM> GetAllStaffList(/*long userId,*/List<long> childsUsersIds);

        StaffVM GetStaffWithUserId(long userId, List<long> childsUsersIds);

        StaffVM GetStaffWithStaffId(int staffId, List<long> childsUsersIds);

        GetStaffImagesVM GetStaffImages(List<long> childsUsersIds, int staffId);

        bool UpdateStaffImages(long userId, int staffId, string contractImage, string certificateImage, string nationalCodeImage);

        #endregion

        #endregion
    }
}
