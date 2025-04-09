using ApiCallers.BaseApiCaller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Automation;

namespace ApiCallers.AutomationApiCaller
{
    public interface IAutomationApiCaller
    {

        #region BoardMembersManagement

        ResponseApiCaller EditBoardMembers(EditBoardMembersPVM editBoardMembersPVM);

        #endregion

        #region FormsManagement

        ResponseApiCaller GetAllFormsList(GetAllFormsListPVM getAllFormsListPVM);

        ResponseApiCaller GetListOfForms(GetListOfFormsPVM getListOfFormsPVM);

        ResponseApiCaller AddToForms(AddToFormsPVM addToFormsPVM);

        ResponseApiCaller GetFormWithFormId(GetFormWithFormIdPVM getFormWithFormIdPVM);

        ResponseApiCaller UpdateForms(UpdateFormsPVM updateFormsPVM);

        ResponseApiCaller ToggleActivationForms(ToggleActivationFormsPVM toggleActivationFormsPVM);

        ResponseApiCaller TemporaryDeleteForms(TemporaryDeleteFormsPVM temporaryDeleteFormsPVM);

        ResponseApiCaller CompleteDeleteForms(CompleteDeleteFormsPVM completeDeleteFormsPVM);

        #endregion

        #region FormElementsManagement

        ResponseApiCaller GetAllFormElementsList(GetAllFormElementsListPVM getAllFormElementsListPVM);

        ResponseApiCaller GetListOfFormElements(GetListOfFormElementsPVM getListOfFormElementsPVM);

        ResponseApiCaller AddToFormElements(AddToFormElementsPVM addToFormElementsPVM);

        ResponseApiCaller GetFormElementWithFormElementId(GetFormElementWithFormElementIdPVM getFormElementWithFormElementIdPVM);

        ResponseApiCaller UpdateFormElements(UpdateFormElementsPVM updateFormElementsPVM);

        ResponseApiCaller ToggleActivationFormElements(ToggleActivationFormElementsPVM toggleActivationFormElementsPVM);

        ResponseApiCaller TemporaryDeleteFormElements(TemporaryDeleteFormElementsPVM temporaryDeleteFormElementsPVM);

        ResponseApiCaller CompleteDeleteFormElements(CompleteDeleteFormElementsPVM completeDeleteFormElementsPVM);

        #endregion

        #region FormElementOptionsManagement

        ResponseApiCaller GetAllFormElementOptionsList(GetAllFormElementOptionsListPVM getAllFormElementOptionsListPVM);

        ResponseApiCaller GetListOfFormElementOptions(GetListOfFormElementOptionsPVM getListOfFormElementOptionsPVM);

        ResponseApiCaller AddToFormElementOptions(AddToFormElementOptionsPVM addToFormElementOptionsPVM);

        ResponseApiCaller GetFormElementOptionWithFormElementOptionId(GetFormElementOptionWithFormElementOptionIdPVM getFormElementOptionWithFormElementOptionIdPVM);

        ResponseApiCaller UpdateFormElementOptions(UpdateFormElementOptionsPVM updateFormElementOptionsPVM);

        ResponseApiCaller ToggleActivationFormElementOptions(ToggleActivationFormElementOptionsPVM toggleActivationFormElementOptionsPVM);

        ResponseApiCaller TemporaryDeleteFormElementOptions(TemporaryDeleteFormElementOptionsPVM temporaryDeleteFormElementOptionsPVM);

        ResponseApiCaller CompleteDeleteFormElementOptions(CompleteDeleteFormElementOptionsPVM completeDeleteFormElementOptionsPVM);

        ResponseApiCaller GetFormAndFieldsWithFormId(GetFormAndFieldsWithFormIdPVM getFormAndFieldsWithFormIdPVM);

        #endregion

        #region MyDepartmentsManagement

        ResponseApiCaller GetAllMyDepartmentsList(GetAllMyDepartmentsListPVM getAllMyDepartmentsListPVM);

        ResponseApiCaller GetListOfMyDepartments(GetListOfMyDepartmentsPVM getListOfMyDepartmentsPVM);

        ResponseApiCaller AddToMyDepartments(AddToMyDepartmentsPVM addToMyDepartmentsPVM);

        ResponseApiCaller UpdateMyDepartments(UpdateMyDepartmentsPVM updateMyDepartmentsPVM);

        ResponseApiCaller ToggleActivationMyDepartments(ToggleActivationMyDepartmentsPVM toggleActivationMyDepartmentsPVM);

        ResponseApiCaller TemporaryDeleteMyDepartments(TemporaryDeleteMyDepartmentsPVM temporaryDeleteMyDepartmentsPVM);

        ResponseApiCaller CompleteDeleteMyDepartments(CompleteDeleteMyDepartmentsPVM completeDeleteMyDepartmentsPVM);

        #endregion

        #region MyDepartmentsDirectorsManagement

        ResponseApiCaller GetListOfMyDepartmentsDirectors(
            GetListOfMyDepartmentsDirectorsPVM getListOfMyDepartmentsDirectorsPVM);

        ResponseApiCaller GetAllMyDepartmentsDirectorsList(
            GetAllMyDepartmentsDirectorsListPVM getAllMyDepartmentsDirectorsListPVM);

        ResponseApiCaller AddToMyDepartmentsDirectors(
            AddToMyDepartmentsDirectorsPVM addToMyDepartmentsDirectorsPVM);

        ResponseApiCaller GetMyDepartmentDirectorWithMyDepartmentDirectorId(
            GetMyDepartmentDirectorWithMyDepartmentDirectorIdPVM getMyDepartmentDirectorWithMyDepartmentDirectorIdPVM);

        ResponseApiCaller UpdateMyDepartmentsDirectors(
            UpdateMyDepartmentsDirectorsPVM updateMyDepartmentsDirectorsPVM);

        ResponseApiCaller ToggleActivationMyDepartmentsDirectors(
            ToggleActivationMyDepartmentsDirectorsPVM toggleActivationMyDepartmentsDirectorsPVM);

        ResponseApiCaller TemporaryDeleteMyDepartmentsDirectors(
            TemporaryDeleteMyDepartmentsDirectorsPVM temporaryDeleteMyDepartmentsDirectorsPVM);

        ResponseApiCaller CompleteDeleteMyDepartmentsDirectors(
            CompleteDeleteMyDepartmentsDirectorsPVM completeDeleteMyDepartmentsDirectorsPVM);

        #endregion

        #region MyCompaniesManagement

        //ResponseApiCaller GetListOfMyCompanies(GetListOfMyCompaniesPVM getListOfMyCompaniesPVM);

        //ResponseApiCaller GetAllMyCompaniesList(GetAllMyCompaniesListPVM getAllMyCompaniesListPVM);

        //ResponseApiCaller AddToMyCompanies(AddToMyCompaniesPVM addToMyCompaniesPVM);

        //ResponseApiCaller GetMyCompanyWithMyCompanyId(GetMyCompanyWithMyCompanyIdPVM getMyCompanyWithMyCompanyIdPVM);

        //ResponseApiCaller UpdateMyCompanies(UpdateMyCompaniesPVM updateMyCompaniesPVM);

        //ResponseApiCaller ToggleActivationMyCompanies(ToggleActivationMyCompaniesPVM toggleActivationMyCompaniesPVM);

        //ResponseApiCaller TemporaryDeleteMyCompanies(TemporaryDeleteMyCompaniesPVM temporaryDeleteMyCompaniesPVM);

        //ResponseApiCaller CompleteDeleteMyCompanies(CompleteDeleteMyCompaniesPVM completeDeleteMyCompaniesPVM);

        //ResponseApiCaller GetMyCompaniesImages(GetMyCompaniesImagesPVM getMyCompaniesImagesPVM);

        //ResponseApiCaller UpdateCompanyPictures(UpdateCompanyPicturesPVM updateCompanyPicturesPVM);

        #endregion

        #region NodeTypesManagement

        ResponseApiCaller GetAllNodeTypesList(GetAllNodeTypesListPVM getAllNodeTypesListPVM);

        #endregion

        #region OrgChartNodesManagement

        ResponseApiCaller GetHierarchyOfOrgChartNodes(GetHierarchyOfOrgChartNodesPVM getHierarchyOfOrgChartNodesPVM);

        ResponseApiCaller GetHierarchyOfOrgChartNodesForTreeView(GetHierarchyOfOrgChartNodesForTreeViewPVM getHierarchyOfOrgChartNodesForTreeViewPVM);

        ResponseApiCaller GetAllOrgChartNodesList(GetAllOrgChartNodesListPVM getAllOrgChartNodesListPVM);

        ResponseApiCaller GetListOfOrgChartNodes(GetListOfOrgChartNodesPVM getListOfOrgChartNodesPVM);

        ResponseApiCaller AddToOrgChartNodes(AddToOrgChartNodesPVM addToOrgChartNodesPVM);

        ResponseApiCaller ExistOrgChartNodeWithUserId(ExistOrgChartNodeWithUserIdPVM existOrgChartNodeWithUserIdPVM);

        ResponseApiCaller GetOrgChartNodeWithOrgChartNodeId(GetOrgChartNodeWithOrgChartNodeIdPVM getOrgChartNodeWithOrgChartNodeIdPVM);

        ResponseApiCaller UpdateOrgChartNodes(UpdateOrgChartNodesPVM updateOrgChartNodesPVM);

        ResponseApiCaller ToggleActivationOrgChartNodes(ToggleActivationOrgChartNodesPVM toggleActivationOrgChartNodesPVM);

        ResponseApiCaller TemporaryDeleteOrgChartNodes(TemporaryDeleteOrgChartNodesPVM temporaryDeleteOrgChartNodesPVM);

        ResponseApiCaller CompleteDeleteOrgChartNodes(CompleteDeleteOrgChartNodesPVM completeDeleteOrgChartNodesPVM);

        #endregion

        #region OrganizationalPositionsManagement
        ResponseApiCaller GetAllOrganizationalPositionsList(GetAllOrganizationalPositionsListPVM getAllOrganizationalPositionsListPVM);
        ResponseApiCaller GetListOfOrganizationalPositions(GetListOfOrganizationalPositionsPVM getListOfOrganizationalPositionsPVM);
        ResponseApiCaller AddToOrganizationalPositions(AddToOrganizationalPositionsPVM addToOrganizationalPositionsPVM);
        ResponseApiCaller UpdateOrganizationalPositions(UpdateOrganizationalPositionsPVM updateOrganizationalPositionsPVM);
        ResponseApiCaller ToggleActivationOrganizationalPositions(ToggleActivationOrganizationalPositionsPVM toggleActivationOrganizationalPositionsPVM);
        ResponseApiCaller TemporaryDeleteOrganizationalPositions(TemporaryDeleteOrganizationalPositionsPVM temporaryDeleteOrganizationalPositionsPVM);
        ResponseApiCaller CompleteDeleteOrganizationalPositions(CompleteDeleteOrganizationalPositionsPVM completeDeleteOrganizationalPositionsPVM);
        ResponseApiCaller GetOrganizationalPositionWithOrganizationalPositionId(GetOrganizationalPositionWithOrganizationalPositionIdPVM getOrganizationalPositionWithOrganizationalPositionIdPVM);

        #endregion

    }
}
