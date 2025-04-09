using ApiCallers.BaseApiCaller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Automation;
using Newtonsoft.Json;
using VM.Base;

namespace ApiCallers.AutomationApiCaller
{
    public class AutomationApiCaller : BaseCaller, IAutomationApiCaller
    {
        public AutomationApiCaller() : base()
        {
        }

        public AutomationApiCaller(string _serviceUrl) : base(_serviceUrl)
        {
            serviceUrl = _serviceUrl;
        }

        public AutomationApiCaller(string _serviceUrl,
            string _accessToken) :
            base(_serviceUrl,
                _accessToken)
        {
            serviceUrl = _serviceUrl;
        }


        #region BoardMembersManagement

        public ResponseApiCaller EditBoardMembers(EditBoardMembersPVM editBoardMembersPVM)
        {
            return GetResult(editBoardMembersPVM);
        }

        #endregion

        #region FormsManagement

        public ResponseApiCaller GetAllFormsList(GetAllFormsListPVM getAllFormsListPVM)
        {
            return GetRecords(getAllFormsListPVM);
        }

        public ResponseApiCaller GetListOfForms(GetListOfFormsPVM getListOfFormsPVM)
        {
            return GetRecords(getListOfFormsPVM);
        }

        public ResponseApiCaller AddToForms(AddToFormsPVM addToFormsPVM)
        {
            return GetRecord(addToFormsPVM);
        }

        public ResponseApiCaller GetFormWithFormId(GetFormWithFormIdPVM getFormWithFormIdPVM)
        {
            return GetRecord(getFormWithFormIdPVM);
        }

        public ResponseApiCaller UpdateForms(UpdateFormsPVM updateFormsPVM)
        {
            return GetRecord(updateFormsPVM);
        }

        public ResponseApiCaller ToggleActivationForms(ToggleActivationFormsPVM toggleActivationFormsPVM)
        {
            return GetResult(toggleActivationFormsPVM);
        }

        public ResponseApiCaller TemporaryDeleteForms(TemporaryDeleteFormsPVM temporaryDeleteFormsPVM)
        {
            return GetResult(temporaryDeleteFormsPVM);
        }

        public ResponseApiCaller CompleteDeleteForms(CompleteDeleteFormsPVM completeDeleteFormsPVM)
        {
            return GetResult(completeDeleteFormsPVM);
        }

        public ResponseApiCaller GetFormAndFieldsWithFormId(GetFormAndFieldsWithFormIdPVM getFormAndFieldsWithFormIdPVM)
        {
            return GetRecord(getFormAndFieldsWithFormIdPVM);
        }

        #endregion

        #region FormElementsManagement

        public ResponseApiCaller GetAllFormElementsList(GetAllFormElementsListPVM getAllFormElementsListPVM)
        {
            return GetRecords(getAllFormElementsListPVM);
        }

        public ResponseApiCaller GetListOfFormElements(GetListOfFormElementsPVM getListOfFormElementsPVM)
        {
            return GetRecords(getListOfFormElementsPVM);
        }

        public ResponseApiCaller AddToFormElements(AddToFormElementsPVM addToFormElementsPVM)
        {
            return GetRecord(addToFormElementsPVM);
        }

        public ResponseApiCaller GetFormElementWithFormElementId(GetFormElementWithFormElementIdPVM getFormElementWithFormElementIdPVM)
        {
            return GetRecord(getFormElementWithFormElementIdPVM);
        }

        public ResponseApiCaller UpdateFormElements(UpdateFormElementsPVM updateFormElementsPVM)
        {
            return GetRecord(updateFormElementsPVM);
        }

        public ResponseApiCaller ToggleActivationFormElements(ToggleActivationFormElementsPVM toggleActivationFormElementsPVM)
        {
            return GetResult(toggleActivationFormElementsPVM);
        }

        public ResponseApiCaller TemporaryDeleteFormElements(TemporaryDeleteFormElementsPVM temporaryDeleteFormElementsPVM)
        {
            return GetResult(temporaryDeleteFormElementsPVM);
        }

        public ResponseApiCaller CompleteDeleteFormElements(CompleteDeleteFormElementsPVM completeDeleteFormElementsPVM)
        {
            return GetResult(completeDeleteFormElementsPVM);
        }

        #endregion

        #region FormElementOptionsManagement

        public ResponseApiCaller GetAllFormElementOptionsList(GetAllFormElementOptionsListPVM getAllFormElementOptionsListPVM)
        {
            return GetRecords(getAllFormElementOptionsListPVM);
        }

        public ResponseApiCaller GetListOfFormElementOptions(GetListOfFormElementOptionsPVM getListOfFormElementOptionsPVM)
        {
            return GetRecords(getListOfFormElementOptionsPVM);
        }

        public ResponseApiCaller AddToFormElementOptions(AddToFormElementOptionsPVM addToFormElementOptionsPVM)
        {
            return GetRecord(addToFormElementOptionsPVM);
        }

        public ResponseApiCaller GetFormElementOptionWithFormElementOptionId(GetFormElementOptionWithFormElementOptionIdPVM getFormElementOptionWithFormElementOptionIdPVM)
        {
            return GetRecord(getFormElementOptionWithFormElementOptionIdPVM);
        }

        public ResponseApiCaller UpdateFormElementOptions(UpdateFormElementOptionsPVM updateFormElementOptionsPVM)
        {
            return GetRecord(updateFormElementOptionsPVM);
        }

        public ResponseApiCaller ToggleActivationFormElementOptions(ToggleActivationFormElementOptionsPVM toggleActivationFormElementOptionsPVM)
        {
            return GetResult(toggleActivationFormElementOptionsPVM);
        }

        public ResponseApiCaller TemporaryDeleteFormElementOptions(TemporaryDeleteFormElementOptionsPVM temporaryDeleteFormElementOptionsPVM)
        {
            return GetResult(temporaryDeleteFormElementOptionsPVM);
        }

        public ResponseApiCaller CompleteDeleteFormElementOptions(CompleteDeleteFormElementOptionsPVM completeDeleteFormElementOptionsPVM)
        {
            return GetResult(completeDeleteFormElementOptionsPVM);
        }

        #endregion

        #region MyDepartmentsManagement

        public ResponseApiCaller GetAllMyDepartmentsList(GetAllMyDepartmentsListPVM getAllMyDepartmentsListPVM)
        {
            return GetRecords(getAllMyDepartmentsListPVM);
        }

        public ResponseApiCaller GetListOfMyDepartments(GetListOfMyDepartmentsPVM getListOfMyDepartmentsPVM)
        {
            return GetRecords(getListOfMyDepartmentsPVM);
        }

        public ResponseApiCaller AddToMyDepartments(AddToMyDepartmentsPVM addToMyDepartmentsPVM)
        {
            return GetRecord(addToMyDepartmentsPVM);
        }

        public ResponseApiCaller UpdateMyDepartments(UpdateMyDepartmentsPVM updateMyDepartmentsPVM)
        {
            return GetRecord(updateMyDepartmentsPVM);
        }

        public ResponseApiCaller ToggleActivationMyDepartments(ToggleActivationMyDepartmentsPVM toggleActivationMyDepartmentsPVM)
        {
            return GetResult(toggleActivationMyDepartmentsPVM);
        }

        public ResponseApiCaller TemporaryDeleteMyDepartments(TemporaryDeleteMyDepartmentsPVM temporaryDeleteMyDepartmentsPVM)
        {
            return GetResult(temporaryDeleteMyDepartmentsPVM);
        }

        public ResponseApiCaller CompleteDeleteMyDepartments(CompleteDeleteMyDepartmentsPVM completeDeleteMyDepartmentsPVM)
        {
            return GetResult(completeDeleteMyDepartmentsPVM);
        }

        #endregion

        #region MyDepartmentsDirectorsManagement

        public ResponseApiCaller GetListOfMyDepartmentsDirectors(
            GetListOfMyDepartmentsDirectorsPVM getListOfMyDepartmentsDirectorsPVM)
        {
            return GetRecords(getListOfMyDepartmentsDirectorsPVM);
        }

        public ResponseApiCaller GetAllMyDepartmentsDirectorsList(
            GetAllMyDepartmentsDirectorsListPVM getAllMyDepartmentsDirectorsListPVM)
        {
            return GetRecords(getAllMyDepartmentsDirectorsListPVM);
        }

        public ResponseApiCaller AddToMyDepartmentsDirectors(
            AddToMyDepartmentsDirectorsPVM addToMyDepartmentsDirectorsPVM)
        {
            return GetRecord(addToMyDepartmentsDirectorsPVM);
        }

        public ResponseApiCaller GetMyDepartmentDirectorWithMyDepartmentDirectorId(
            GetMyDepartmentDirectorWithMyDepartmentDirectorIdPVM getMyDepartmentDirectorWithMyDepartmentDirectorIdPVM)
        {
            return GetRecord(getMyDepartmentDirectorWithMyDepartmentDirectorIdPVM);
        }

        public ResponseApiCaller UpdateMyDepartmentsDirectors(
            UpdateMyDepartmentsDirectorsPVM updateMyDepartmentsDirectorsPVM)
        {
            return GetRecord(updateMyDepartmentsDirectorsPVM);
        }

        public ResponseApiCaller ToggleActivationMyDepartmentsDirectors(
            ToggleActivationMyDepartmentsDirectorsPVM toggleActivationMyDepartmentsDirectorsPVM)
        {
            return GetResult(toggleActivationMyDepartmentsDirectorsPVM);
        }

        public ResponseApiCaller TemporaryDeleteMyDepartmentsDirectors(
            TemporaryDeleteMyDepartmentsDirectorsPVM temporaryDeleteMyDepartmentsDirectorsPVM)
        {
            return GetResult(temporaryDeleteMyDepartmentsDirectorsPVM);
        }

        public ResponseApiCaller CompleteDeleteMyDepartmentsDirectors(
            CompleteDeleteMyDepartmentsDirectorsPVM completeDeleteMyDepartmentsDirectorsPVM)
        {
            return GetResult(completeDeleteMyDepartmentsDirectorsPVM);
        }

        #endregion

        #region MyCompaniesManagement

        public ResponseApiCaller GetListOfMyCompanies(GetListOfMyCompaniesPVM getListOfMyCompaniesPVM)
        {
            return GetRecords(getListOfMyCompaniesPVM);
        }

        public ResponseApiCaller GetAllMyCompaniesList(GetAllMyCompaniesListPVM getAllMyCompaniesListPVM)
        {
            return GetRecords(getAllMyCompaniesListPVM);
        }

        public ResponseApiCaller AddToMyCompanies(AddToMyCompaniesPVM addToMyCompaniesPVM)
        {
            return GetRecord(addToMyCompaniesPVM);
        }

        public ResponseApiCaller GetMyCompanyWithMyCompanyId(GetMyCompanyWithMyCompanyIdPVM getMyCompanyWithMyCompanyIdPVM)
        {
            return GetRecord(getMyCompanyWithMyCompanyIdPVM);
        }

        public ResponseApiCaller UpdateMyCompanies(UpdateMyCompaniesPVM updateMyCompaniesPVM)
        {
            return GetRecord(updateMyCompaniesPVM);
        }

        public ResponseApiCaller ToggleActivationMyCompanies(ToggleActivationMyCompaniesPVM toggleActivationMyCompaniesPVM)
        {
            return GetResult(toggleActivationMyCompaniesPVM);
        }

        public ResponseApiCaller TemporaryDeleteMyCompanies(TemporaryDeleteMyCompaniesPVM temporaryDeleteMyCompaniesPVM)
        {
            return GetResult(temporaryDeleteMyCompaniesPVM);
        }

        public ResponseApiCaller CompleteDeleteMyCompanies(CompleteDeleteMyCompaniesPVM completeDeleteMyCompaniesPVM)
        {
            return GetResult(completeDeleteMyCompaniesPVM);
        }

        public ResponseApiCaller GetMyCompaniesImages(GetMyCompaniesImagesPVM getMyCompaniesImagesPVM)
        {
            return GetRecord(getMyCompaniesImagesPVM);
        }

        public ResponseApiCaller UpdateCompanyPictures(UpdateCompanyPicturesPVM updateCompanyPicturesPVM)
        {
            return GetRecord(updateCompanyPicturesPVM);
        }

        #endregion

        #region NodeTypesManagement

        public ResponseApiCaller GetAllNodeTypesList(GetAllNodeTypesListPVM getAllNodeTypesListPVM)
        {
            return GetRecords(getAllNodeTypesListPVM);
        }

        #endregion

        #region OrgChartNodesManagement

        public ResponseApiCaller GetHierarchyOfOrgChartNodes(GetHierarchyOfOrgChartNodesPVM getHierarchyOfOrgChartNodesPVM)
        {
            return GetRecord(getHierarchyOfOrgChartNodesPVM);
        }

        public ResponseApiCaller GetHierarchyOfOrgChartNodesForTreeView(GetHierarchyOfOrgChartNodesForTreeViewPVM getHierarchyOfOrgChartNodesForTreeViewPVM)
        {
            return GetRecord(getHierarchyOfOrgChartNodesForTreeViewPVM);
        }

        public ResponseApiCaller GetAllOrgChartNodesList(GetAllOrgChartNodesListPVM getAllOrgChartNodesListPVM)
        {
            return GetRecords(getAllOrgChartNodesListPVM);
        }

        public ResponseApiCaller GetListOfOrgChartNodes(GetListOfOrgChartNodesPVM getListOfOrgChartNodesPVM)
        {
            return GetRecords(getListOfOrgChartNodesPVM);
        }

        public ResponseApiCaller AddToOrgChartNodes(AddToOrgChartNodesPVM addToOrgChartNodesPVM)
        {
            return GetRecord(addToOrgChartNodesPVM);
        }

        public ResponseApiCaller ExistOrgChartNodeWithUserId(ExistOrgChartNodeWithUserIdPVM existOrgChartNodeWithUserIdPVM)
        {
            return GetResult(existOrgChartNodeWithUserIdPVM);
        }

        public ResponseApiCaller GetOrgChartNodeWithOrgChartNodeId(GetOrgChartNodeWithOrgChartNodeIdPVM getOrgChartNodeWithOrgChartNodeIdPVM)
        {
            return GetRecord(getOrgChartNodeWithOrgChartNodeIdPVM);
        }

        public ResponseApiCaller UpdateOrgChartNodes(UpdateOrgChartNodesPVM updateOrgChartNodesPVM)
        {
            return GetRecord(updateOrgChartNodesPVM);
        }

        public ResponseApiCaller ToggleActivationOrgChartNodes(ToggleActivationOrgChartNodesPVM toggleActivationOrgChartNodesPVM)
        {
            return GetResult(toggleActivationOrgChartNodesPVM);
        }

        public ResponseApiCaller TemporaryDeleteOrgChartNodes(TemporaryDeleteOrgChartNodesPVM temporaryDeleteOrgChartNodesPVM)
        {
            return GetResult(temporaryDeleteOrgChartNodesPVM);
        }

        public ResponseApiCaller CompleteDeleteOrgChartNodes(CompleteDeleteOrgChartNodesPVM completeDeleteOrgChartNodesPVM)
        {
            return GetResult(completeDeleteOrgChartNodesPVM);
        }

        #endregion

        #region OrganizationalPositionsManagement

        public ResponseApiCaller GetAllOrganizationalPositionsList(GetAllOrganizationalPositionsListPVM getAllOrganizationalPositionsListPVM)
        {
            return GetRecords(getAllOrganizationalPositionsListPVM);
        }

        public ResponseApiCaller GetListOfOrganizationalPositions(GetListOfOrganizationalPositionsPVM getListOfOrganizationalPositionsPVM)
        {
            return GetRecords(getListOfOrganizationalPositionsPVM);
        }

        public ResponseApiCaller AddToOrganizationalPositions(AddToOrganizationalPositionsPVM addToOrganizationalPositionsPVM)
        {
            return GetRecord(addToOrganizationalPositionsPVM);
        }

        public ResponseApiCaller UpdateOrganizationalPositions(UpdateOrganizationalPositionsPVM updateOrganizationalPositionsPVM)
        {
            return GetRecord(updateOrganizationalPositionsPVM);
        }

        public ResponseApiCaller ToggleActivationOrganizationalPositions(ToggleActivationOrganizationalPositionsPVM toggleActivationOrganizationalPositionsPVM)
        {
            return GetResult(toggleActivationOrganizationalPositionsPVM);
        }

        public ResponseApiCaller TemporaryDeleteOrganizationalPositions(TemporaryDeleteOrganizationalPositionsPVM temporaryDeleteOrganizationalPositionsPVM)
        {
            return GetResult(temporaryDeleteOrganizationalPositionsPVM);
        }

        public ResponseApiCaller CompleteDeleteOrganizationalPositions(CompleteDeleteOrganizationalPositionsPVM completeDeleteOrganizationalPositionsPVM)
        {
            return GetResult(completeDeleteOrganizationalPositionsPVM);
        }

        public ResponseApiCaller GetOrganizationalPositionWithOrganizationalPositionId(GetOrganizationalPositionWithOrganizationalPositionIdPVM getOrganizationalPositionWithOrganizationalPositionIdPVM)
        {
            return GetRecord(getOrganizationalPositionWithOrganizationalPositionIdPVM);
        }

        #endregion       

    }
}
