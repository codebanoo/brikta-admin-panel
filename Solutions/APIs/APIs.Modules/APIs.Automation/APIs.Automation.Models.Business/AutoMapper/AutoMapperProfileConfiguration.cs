using APIs.Automation.Models.Entities;
using VM.Automation;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIs.Automation.Models.Business.AutoMapper
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
        {
            #region Automation

            CreateMap<MyCompanies, MyCompaniesVM>();
            CreateMap<MyCompaniesVM, MyCompanies>();

            //CreateMap<MyDepartmentsDirectors, MyDepartmentsDirectorsVM>();
            //CreateMap<MyDepartmentsDirectorsVM, MyDepartmentsDirectors>();

            CreateMap<MyDepartments, MyDepartmentsVM>()
                /*.ForMember(x => x.EstablishedDateTime, opt => opt.Ignore())
                .ForMember(x => x.DirectorName, opt => opt.Ignore())
                .ForMember(x => x.MyCompaniesVM, opt => opt.Ignore())*/;
            CreateMap<MyDepartmentsVM, MyDepartments>();

            CreateMap<Staff, StaffVM>();
            CreateMap<StaffVM, Staff>();

            CreateMap<DepartmentsStaff, DepartmentsStaffVM>();
            CreateMap<DepartmentsStaffVM, DepartmentsStaff>();

            CreateMap<NodeTypes, NodeTypesVM>();
            CreateMap<NodeTypesVM, NodeTypes>();

            CreateMap<OrgChartNodes, OrgChartNodesVM>();
            CreateMap<OrgChartNodesVM, OrgChartNodes>();

            CreateMap<BoardMembers, BoardMembersVM>();
            CreateMap<BoardMembersVM, BoardMembers>();

            CreateMap<MyDepartmentsDirectors, MyDepartmentsDirectorsVM>();
            CreateMap<MyDepartmentsDirectorsVM, MyDepartmentsDirectors>();

            CreateMap<MyCompaniesDirectors, MyCompaniesDirectorsVM>();
            CreateMap<MyCompaniesDirectorsVM, MyCompaniesDirectors>();

            CreateMap<Forms, FormsVM>();
            CreateMap<FormsVM, Forms>();

            CreateMap<FormElements, FormElementsVM>();
            CreateMap<FormElementsVM, FormElements>();

            CreateMap<FormElementOptions, FormElementOptionsVM>();
            CreateMap<FormElementOptionsVM, FormElementOptions>();

            CreateMap<FormElementValues, FormElementValuesVM>();
            CreateMap<FormElementValuesVM, FormElementValues>();

            CreateMap<OrganizationalPositions, OrganizationalPositionsVM>();
            CreateMap<OrganizationalPositionsVM, OrganizationalPositions>();


            #endregion
        }
    }
}
