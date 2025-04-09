using APIs.Melkavan.Models.Entities;
using APIs.Public.Models;
using APIs.Public.Models.Business;
using APIs.Teniaco.Models;
using APIs.Teniaco.Models.Business;
using Models.Business.ConsoleBusiness;
using System.Collections.Generic;
using VM.Console;
using VM.Melkavan;
using VM.Melkavan.VM.Melkavan;
using VM.PVM.Melkavan;

namespace APIs.Melkavan.Models.Business
{
    public interface IMelkavanApiBusiness
    {
        MelkavanApiContext MelkavanApiDb { get; set; }

        #region Melkavan

        #region Methods For Work With Advertisement


        List<AdvertisementViewers> GetAdvertisementViewersByAdvertisementId(int? AdvertisementId, string Type, TeniacoApiContext teniacoApiDb);

        List<AdvertisementVM> GetAllAdvertisementList(
           ref int listCount,
           List<long> childsUsersIds,
           PublicApiContext publicApiDb,
           int? advertisementTypeId = null,
           int? propertyTypeId = null,
           int? typeOfUseId = null,
           int? documentTypeId = null,
           string propertyCodeName = null,
           long? stateId = null,
           long? cityId = null,
           long? zoneId = null,
           long? districtId = null,
           string jtSorting = null);




        List<AdvertisementAdvanceSearchVM> GetListOfAdverisementsAdvanceSearch(
              int jtStartIndex,
              int jtPageSize,
              ref int listCount,
              List<long> childsUsersIds,
              PublicApiContext publicApiDb,
              TeniacoApiContext teniacoApiDb,
              string jtSorting = null,
             string advertisementTitle = null,
             bool? onlyFavorite = null,
             long? userId = null, bool IsFiltered = false,
             //parameters for advanced filters
             int? AdvertisementTypeId = null,
             int? PropertyTypeId = null,
             string? TypeOfUseId = null,
             long? StateId = null,
             long? CityId = null,
             long? ZoneId = null,
             string? TownName = null,
             string? FromArea = null,
             string? ToArea = null,
             int? FromFoundation = null,
             int? ToFoundation = null,
             long? FromPrice = null,
             long? ToPrice = null,
             string? BuildingLifeId = null,
             int? FromRebuiltInYear = null,
             int? ToRebuiltInYear = null,
             int? DocumentTypeId = null,
             int? DocumentRootTypeId = null,
             int? DocumentOwnershipTypeId = null,
             long? DepositFromPrice = null,
             long? DepositToPrice = null,
             long? RentFromPrice = null,
             long? RentToPrice = null,
             int? MaritalStatusId = null,
             string? Convertable = null,
             Dictionary<string, string>? Features = null);



        List<AdvertisementVM> GetListOfAdvertisement(
             int jtStartIndex,
             int jtPageSize,
             ref int listCount,
             List<long> childsUsersIds,
             PublicApiContext publicApiDb,
             int? advertisementTypeId = null,
             int? propertyTypeId = null,
             int? typeOfUseId = null,
             int? documentTypeId = null,
             string propertyCodeName = null,
             long? stateId = null,
             long? cityId = null,
             long? zoneId = null,
             long? districtId = null,
             string jtSorting = null,
             bool? onlyFavorite = null,
             long? userId = null,
             string advertisementTitle = null);


        long AddToAdvertisement(AdvertisementVM advertisementVM,
            IPublicApiBusiness publicApiBusiness,
            IConsoleBusiness consoleBusiness);



        AdvertisementVM GetAdvertisementWithAdvertisementId(
             long advertisementId,
             List<long> childsUsersIds,
             IConsoleBusiness consoleBusiness,
             PublicApiContext publicApiDb,
             TeniacoApiContext teniacoApiDb,
             MelkavanApiContext melkavanApiDb,
             long? userId = 0,
             string type = "");



        long UpdateAdvertisement(
           ref AdvertisementVM AdvertisementVM,
           List<long> childsUsersIds, TeniacoApiContext teniacoApiDb);




        bool ToggleActivationAdvertisement(
           long advertisementId,
           long userId,
           List<long> childsUsersIds);


        bool TemporaryDeleteAdvertisement(
           long advertisementId,
           long userId,
           List<long> childsUsersIds);

        bool CompleteDeleteAdvertisement(
           long advertisementId,
           List<long> childsUsersIds,
           long UserId,
           string type, TeniacoApiContext teniacoApiDb);


        bool TemporaryDeleteAdvertisementWithChild(
           long advertisementId,
          List<long> childsUsersIds);


        List<AdvertisementAdvanceSearchVM> GetAdvertisementsWithOwnerId(
            int jtStartIndex,
              int jtPageSize,
              ref int listCount,
              List<long> childsUsersIds,
              PublicApiContext publicApiDb,
              TeniacoApiContext teniacoApiDb,
              string jtSorting = null,
             long? owenerId = null);

        List<AdvertisementAdvanceSearchVM> GetWatchedAdvertisementsWithOwnerId(
    int jtStartIndex,
      int jtPageSize,
      ref int listCount,
      List<long> childsUsersIds,
      PublicApiContext publicApiDb,
      TeniacoApiContext teniacoApiDb,
      string jtSorting = null,
     long? owenerId = null);

        List<AdvertisementVM> GetListOfAdvertisementWithMoreDetail(
             int jtStartIndex,
             int jtPageSize,
             ref int listCount,
             List<long> childsUsersIds,
             PublicApiContext publicApiDb,
             TeniacoApiContext teniacoApiContext,
             bool HaveCallers,
             bool HaveAddress,
             bool HaveFeature,
             bool HaveFavorit,
             bool HaveViewers,
             bool HaveDetails,
             bool HaveTags,
             bool HaveFiles,
             int? advertisementTypeId = null,
             int? propertyTypeId = null,
             int? typeOfUseId = null,
             int? documentTypeId = null,
             string propertyCodeName = null,
             long? stateId = null,
             long? cityId = null,
             long? zoneId = null,
             long? districtId = null,
             string jtSorting = null,
             long? userId = null,
             string advertisementTitle = null
           );


        long AddOwnerOrConcultant(
            AddOwnerOrConcultantPVM addToUsersPVM,
            IConsoleBusiness consoleBusiness);



        long AdvertisementQuickRegistration(AdvertisementVM advertisementVM,
                IPublicApiBusiness publicApiBusiness,
                IConsoleBusiness consoleBusiness);


        #endregion

        #region Methods For Work With AdvertisementFiles

        List<AdvertisementFilesVM> GetAllAdvertisementFilesList(
           ref int listCount,
           List<long> childsUsersIds,
           long? advertisementId = null,
           string advertisementFileTitle = "",
           string advertisementFileType = "",
           string jtSorting = null);

        List<AdvertisementFilesVM> GetListOfAdvertisementFiles(int jtStartIndex,
             int jtPageSize,
             ref int listCount,
             List<long> childsUsersIds,
             long? advertisementId = null,
             string advertisementFileTitle = "",
             string advertisementFileType = "",
             string jtSorting = null);

        bool AddToAdvertisementFiles(List<AdvertisementFilesVM> AdvertisementFilesVMList,
                 List<int>? DeletedPhotosIDs,
                 bool? IsMainChanged,
                 long? AdvertisementId,
                 List<long> childsUsersIds);


        AdvertisementFilesVM GetAdvertisementFileWithAdvertisementFileId(int AdvertisementFileId,
           List<long> childsUsersIds);


        bool UpdateAdvertisementFiles(ref AdvertisementFilesVM AdvertisementFilesVM,
           List<long> childsUsersIds);


        bool ToggleActivationAdvertisementFiles(int AdvertisementFileId,
           long userId,
           List<long> childsUsersIds);


        bool TemporaryDeleteAdvertisementFiles(int AdvertisementFileId,
           long userId,
           List<long> childsUsersIds);

        bool CompleteDeleteAdvertisementFiles(int AdvertisementFileId,
           List<long> childsUsersIds);






        #endregion

        #region Methods For Work With AdvertisementFeaturesValues

        AdvertisementFeaturesValuesVM GetAdvertisementFeaturesValues(long AdvertisementId,
               int propertyTypeId,
               TeniacoApiContext teniacoApiDb);


        bool UpdateAdvertisementFeatureValues(long AdvertisementId,
            List<AdvertisementFeaturesValuesVM> advertisementFeaturesValuesVMList,
            List<long> childsUsersIds);


        AdvertisementFeaturesValuesVM GetAllAdvertisementFeatureValues(
              int advertisementTypeId,
              int propertyTypeId,
              TeniacoApiContext teniacoApiDb);
        #endregion

        #region Methods For Work With AdvertisementTypes

        List<AdvertisementTypesVM> GetAllAdvertisementTypesList(
           ref int listCount,
           List<long> childsUsersIds);

        #endregion

        #region Methods for Work With AdvertisementPricesHistories
        List<AdvertisementPricesHistoriesVM> GetListOfAdvertisementPricesHistories(
          int jtStartIndex,
          int jtPageSize,
          ref int listCount,
          List<long> childsUsersIds,
          long AdvertisementId,
          string jtSorting = null);

        #endregion

        #region Methods For Work With AdvertisementFeatures

        ManageAdvertisementFeaturesValuesVM GetListOfFeaturesByPropertyTypeId(
            int propertyTypeId,
            TeniacoApiContext teniacoApiDb);


        public bool UpdateAdvertisementFeatures(List<int> FeatureIds,
            List<AdvertisementFeaturesVM> advertisementFeaturesVMs,
            List<long> childsUsersIds);

        #endregion

        #region Methods For Work With AdvertisementViewers

        //List<AdvertisementViewersVM> GetAdvertisementViewersWithAdvertisementId(int? advertisementId);


        int AddToAdvertisementViewers(AdvertisementViewersVM advertisementViewersVM, TeniacoApiContext teniacoApiDb);

        List<AdvertisementViewersReportPerMonthVM> GetAdvertisementsViewersReportWithIdAndFilterType(GetAdvertisementsViewersReportWithIdAndFilterTypePVM getAdvertisementsViewersReportWithIdAndFilterType, TeniacoApiContext teniacoApiDb);


        #endregion

        #region Methods For Work With AdvertisementCallers
        List<AdvertisementCallersVM> GetAdvertisementCallersWithAdvertisementId(int? advertisementId);


        int AddToAdvertisementCallers(AdvertisementCallersVM advertisementCallersVM);


        #endregion

        #region Methods For Work With AdvertisementFavorites


        long AddToAdvertisementFavorites(AdvertisementFavoritesVM advertisementFavoritesVM, TeniacoApiContext teniacoApiDb);


        bool CompleteDeleteAdvertisementFavorites(long AdvertisementId, long userId);
        #endregion

        #region Methods For Work With AdvertisementUserProfile
        bool UpdateUserProfile(
            long userId,
            long? stateId,
            long? cityId,
            string? name,
            string? family,
            string? email,
            int? personTypeId,
            List<long> childsUsersIds,
            IConsoleBusiness consoleBusiness,
            IPublicApiBusiness publicApiBusiness);


        MelkavanPropertiesUsersProfileVM GetAdvertiserProfileWithAdvertiserId(
    long advertiserId,
    List<long> childsUsersIds,
    IConsoleBusiness consoleBusiness,
    PublicApiContext publicApiDb,
    MelkavanApiContext melkavanApiDb);


        #endregion

        #region Methods For Work With Banners

        List<BannersVM> GetAllBannersList(string bannerTitle);
        List<BannersVM> GetListOfBanners(int jtStartIndex, int jtPageSize, ref int listCount, string? jtSorting = null, string bannerTitle = "");
        int AddToBanners(BannersVM bannersVM);
        bool UpdateBanners(BannersVM bannersVM, List<long> childsUsersIds);
        bool ToggleActivationBanners(int bannerId, long userId, List<long> childsUsersIds);
        bool TemporaryDeleteBanners(int bannerId, long userId, List<long> childsUsersIds);
        bool CompleteDeleteBanners(int bannerId, long userId, List<long> childsUsersIds);
        BannersVM GetBannersByBannerId(int bannerId, List<long> childsUsersIds);

        #endregion

        #region Methods for Work With BuildingLifes

        List<BuildingLifesVM> GetAllBuildingLifesList(
            ref int listCount,
            List<long> childsUsersIds);

        #endregion

        #region Methods for Work With Tags

        List<TagsVM> GetAllTagsList(
            ref int listCount,
            List<long> childsUsersIds);

        #endregion

        #region Methods For Work With Smses

        List<SmsesVM> GetListOfSmses(
            List<long> childsUsersIds,
            int jtStartIndex,
            int jtPageSize,
            ref int listCount,
            string searchText);

        List<SmsesVM> GetAllSmsesList(
            List<long> childsUsersIds,
            ref int listCount,
            string searchText);

        long AddToSmses(
            SmsesVM smsesVM,
            List<long> childsUsersIds,
            IPublicApiBusiness publicApiBusiness,
            IConsoleBusiness consoleBusiness);

        long UpdateSmses(ref SmsesVM smsesVM,
            //long userId,
            List<long> childsUsersIds);

        bool ToggleActivationSmses(long smsId,
            long userId,
            List<long> childsUsersIds);

        bool TemporaryDeleteSmses(long smsId,
            long userId,
            List<long> childsUsersIds);

        bool CompleteDeleteSmses(long smsId,
            //long userId,
            List<long> childsUsersIds);

        #endregion

        #region Methods For Work With MelkavanLogin
        long MelkavanLogin(
            ref MelkavanLoginVM melkavanLoginVM,
            IConsoleBusiness consoleBusiness,
            IPublicApiBusiness publicApiDb);

        #endregion

        #region Methods For Work With Advertisementlocation


        bool UpdateAdvertisementlocation(long userId,
            long advertisementId,
            int stateId,
            int cityId,
            int zoneId,
            string townName,
            string address,
            double locationLat,
            double locationLon,
            List<long> childsUsersIds);
        #endregion

        #region Methods For Work With AdvertisementStatus

        List<AdvertisementStatusListVM> GetListOfAdvertisementsForStatus(int jtStartIndex,
int jtPageSize,
ref int listCount,
List<long> childsUsersIds,
MelkavanApiContext melkavanApiDb,
TeniacoApiContext teniacoApiDb,
IConsoleBusiness consoleBusiness,
string jtSorting = null,
long? ThisUserId = null);

        long UpdateAdvertisementStatusId(
      long AdvertisementId,
      string type,
      int? StatusId,
      string? RejectionReason,
      List<long> childsUsersIds,
      TeniacoApiContext teniacoApiDb);

        long UpdateAdvertisementTagId(
long AdvertisementId,
string type,
string? TagId,
List<long> childsUsersIds,
TeniacoApiContext teniacoApiDb);


        #endregion

        #region Methods For Work With AboutUs

        public AboutUsVM AboutUs(
          IConsoleBusiness consoleBusiness,
          IPublicApiBusiness publicApiDb,
          ITeniacoApiBusiness tenicoApiDb);
        #endregion

        #endregion
    }
}
