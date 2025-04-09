using APIs.Teniaco.Models.Entities;
using AutoMapper;
using VM.Public;
using VM.Teniaco;
using Properties = APIs.Teniaco.Models.Entities.Properties;

namespace APIs.Teniaco.Models.Business.AutoMapper
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
        {
            #region Teniaco

            CreateMap<ElementTypes, ElementTypesVM>();
            CreateMap<ElementTypesVM, ElementTypes>();

            CreateMap<TypeOfUses, TypeOfUsesVM>();
            CreateMap<TypeOfUsesVM, TypeOfUses>();

            CreateMap<ProposedProjects, ProposedProjectsVM>();
            CreateMap<ProposedProjectsVM, ProposedProjects>();

            CreateMap<PropertyTypes, PropertyTypesVM>();
            CreateMap<PropertyTypesVM, PropertyTypes>();

            CreateMap<PropertyTypes, MyPropertyTypesVM>();
            CreateMap<MyPropertyTypesVM, PropertyTypes>();

            CreateMap<PropertyStates, PropertyStatesVM>();
            CreateMap<PropertyStatesVM, PropertyStates>();

            CreateMap<PropertyFiles, PropertyFilesVM>();
            CreateMap<PropertyFilesVM, PropertyFiles>();

            CreateMap<PropertyFiles, MyPropertyFilesVM>();
            CreateMap<MyPropertyFilesVM, PropertyFiles>();

            CreateMap<PropertyAddress, PropertyAddressVM>();
            CreateMap<PropertyAddressVM, PropertyAddress>();

            CreateMap<PropertyAddress, MyPropertyAddressVM>();
            CreateMap<MyPropertyAddressVM, PropertyAddress>();

            CreateMap<PropertiesPricesHistories, PropertiesPricesHistoriesVM>();
            CreateMap<PropertiesPricesHistoriesVM, PropertiesPricesHistories>();

            CreateMap<PropertiesPricesHistories, MyPropertiesPricesHistoriesVM>();
            CreateMap<MyPropertiesPricesHistoriesVM, PropertiesPricesHistories>();

            CreateMap<Properties, PropertiesVM>();
            CreateMap<PropertiesVM, Properties>();

            CreateMap<Properties, MyPropertiesVM>();
            CreateMap<MyPropertiesVM, Properties>();

            //CreateMap<ProjectTypes, ProjectTypesVM>();
            //CreateMap<ProjectTypesVM, ProjectTypes>();

            CreateMap<ProjectStates, ProjectStatesVM>();
            CreateMap<ProjectStatesVM, ProjectStates>();

            //CreateMap<Projects, ProjectsVM>();
            //CreateMap<ProjectsVM, Projects>();

            CreateMap<InvestorsRequests, InvestorsRequestsVM>();
            CreateMap<InvestorsRequestsVM, InvestorsRequests>();

            CreateMap<InvestorsFavorites, InvestorsFavoritesVM>();
            CreateMap<InvestorsFavoritesVM, InvestorsFavorites>();

            CreateMap<Investors, InvestorsVM>();
            CreateMap<InvestorsVM, Investors>();

            CreateMap<IntroductionMethods, IntroductionMethodsVM>();
            CreateMap<IntroductionMethodsVM, IntroductionMethods>();

            CreateMap<FeaturesValues, FeaturesValuesVM>();
            CreateMap<FeaturesValuesVM, FeaturesValues>();

            CreateMap<FeaturesOptions, FeaturesOptionsVM>();
            CreateMap<FeaturesOptionsVM, FeaturesOptions>();

            CreateMap<Features, FeaturesVM>();
            CreateMap<FeaturesVM, Features>();

            CreateMap<FeaturesCategories, FeaturesCategoriesVM>();
            CreateMap<FeaturesCategoriesVM, FeaturesCategories>();

            CreateMap<Funds, FundsVM>();
            CreateMap<FundsVM, Funds>();
            
            CreateMap<DocumentOwnershipTypes, DocumentOwnershipTypesVM>();
            CreateMap<DocumentOwnershipTypesVM, DocumentOwnershipTypes>();

            CreateMap<DocumentRootTypes, DocumentRootTypesVM>();
            CreateMap<DocumentRootTypesVM, DocumentRootTypes>();

            CreateMap<DocumentTypes, DocumentTypesVM>();
            CreateMap<DocumentTypesVM, DocumentTypes>();

            CreateMap<Agencies, AgenciesVM>();
            CreateMap<AgenciesVM, Agencies>();

            CreateMap<AgencyStaffs, AgencyStaffsVM>();
            CreateMap<AgencyStaffsVM, AgencyStaffs>();

            CreateMap<Contractors, ContractorsVM>();
            CreateMap<ContractorsVM, Contractors>();

            CreateMap<MapLayerCategories, MapLayerCategoriesVM>();
            CreateMap<MapLayerCategoriesVM, MapLayerCategories>();

            CreateMap<MapLayers, MapLayersVM>();
            CreateMap<MapLayersVM, MapLayers>();

            CreateMap<EvaluationCategories, EvaluationCategoriesVM>();
            CreateMap<EvaluationCategoriesVM, EvaluationCategories>();

            CreateMap<EvaluationQuestions, EvaluationQuestionsVM>();
            CreateMap<EvaluationQuestionsVM, EvaluationQuestions>();

            CreateMap<EvaluationItems, EvaluationItemsVM>();
            CreateMap<EvaluationItemsVM, EvaluationItems>();

            CreateMap<Evaluations, EvaluationsVM>();
            CreateMap<EvaluationsVM, Evaluations>();

            CreateMap<PropertyOwners, PropertyOwnersVM>();
            CreateMap<PropertyOwnersVM, PropertyOwners>();

            CreateMap<EvaluationItemValues, EvaluationItemValuesVM>();
            CreateMap<EvaluationItemValuesVM, EvaluationItemValues>();

            CreateMap<PropertiesDetails, PropertiesDetailsVM>();
            CreateMap<PropertiesDetailsVM, PropertiesDetails>();


            CreateMap<PropertySelectedCallers, PropertySelectedCallersVM>();
            CreateMap<PropertySelectedCallersVM, PropertySelectedCallers>();
            #endregion
        }
    }
}
