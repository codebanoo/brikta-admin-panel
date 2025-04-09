using ApiCallers.BaseApiCaller;
using VM.PVM.Melkavan;

namespace ApiCallers.MelkavanApiCaller
{
    public interface IMelkavanApiCaller
    {
        #region AdvertisementManagement
        ResponseApiCaller GetAllAdvertisementList(GetAllAdvertisementListPVM getAllAdvertisementsListPVM);

        ResponseApiCaller GetListOfAdvertisement(GetListOfAdvertisementPVM getListOfAdvertisementPVM);

        ResponseApiCaller GetListOfAdverisementsAdvanceSearch(GetListOfAdvertisementAdvanceSearchPVM getListOfAdvertisementAdvanceSearchPVM);
        ResponseApiCaller AddToAdvertisement(AddToAdvertisementPVM addToAdvertisementPVM);

        ResponseApiCaller UpdateAdvertisement(UpdateAdvertisementPVM updateAdvertisementPVM);

        ResponseApiCaller ToggleActivationAdvertisement(ToggleActivationAdvertisementPVM toggleActivationAdvertisementPVM);

        ResponseApiCaller TemporaryDeleteAdvertisement(TemporaryDeleteAdvertisementPVM temporaryDeleteAdvertisementPVM);

        ResponseApiCaller TemporaryDeleteAdvertisementWithChild(TemporaryDeleteAdvertisementWithChildPVM temporaryDeleteAdvertisementWithChildPVM);

        ResponseApiCaller CompleteDeleteAdvertisement(CompleteDeleteAdvertisementPVM completeDeleteAdvertisementPVM);

        ResponseApiCaller GetAdvertisementWithAdvertisementId(GetAdvertisementWithAdvertisementIdPVM getAdvertisementWithAdvertisementIdPVM);

        ResponseApiCaller GetAdvertisementDetailsWithAdvertisementId(GetAdvertisementWithAdvertisementIdPVM getAdvertisementWithAdvertisementIdPVM);

        ResponseApiCaller GetAdvertisementsWithOwnerId(GetAdvertisementsWithOwnerIdPVM getAdvertisementsWithOwnerIdPVM);

        ResponseApiCaller GetWatchedAdvertisementsWithOwnerId(GetAdvertisementsWithOwnerIdPVM getAdvertisementsWithOwnerIdPVM);

        ResponseApiCaller GetListOfAdvertisementWithMoreDetail(GetListOfAdvertisementWithMoreDetailPVM getListOfAdvertisementWithMoreDetailPVM);

        #endregion

        #region AdvertisementLocationManagement

        ResponseApiCaller UpdateAdvertisementLocation(UpdateAdvertisementLocationPVM updateAdvertisementlocationPVM);
        #endregion

        #region AdvertisementFilesManagement

        ResponseApiCaller GetAllAdvertisementFilesList(GetAllAdvertisementFilesListPVM getAllAdvertisementFilesListPVM);

        ResponseApiCaller GetListOfAdvertisementFiles(GetListOfAdvertisementFilesPVM getListOfAdvertisementFilesPVM);

        ResponseApiCaller AddToAdvertisementFiles(AddToAdvertisementFilesPVM addToAdvertisementFilesPVM);

        ResponseApiCaller GetAdvertisementFileWithAdvertisementFileId(GetAdvertisementFileWithAdvertisementFileIdPVM getAdvertisementFileWithAdvertisementFileIdPVM);

        ResponseApiCaller UpdateAdvertisementFiles(UpdateAdvertisementFilesPVM updateAdvertisementFilesPVM);
        ResponseApiCaller ToggleActivationAdvertisementFiles(ToggleActivationAdvertisementFilesPVM toggleActivationPAdvertisementFilesPVM);

        ResponseApiCaller TemporaryDeleteAdvertisementFiles(TemporaryDeleteAdvertisementFilesPVM temporaryDeleteAdvertisementFilesPVM);

        ResponseApiCaller CompleteDeleteAdvertisementFiles(CompleteDeleteAdvertisementFilesPVM completeDeleteAdvertisementFilesPVM);

        #endregion

        #region AdvertisementsPricesHistoriesManagement

        ResponseApiCaller GetListOfAdvertisementsPricesHistories(GetListOfAdvertisementPricesHistoriesPVM getListOfAdvertisementPricesHistoriesPVM);
        #endregion

        #region AdvertisementTypesManagement

        ResponseApiCaller GetAllAdvertisementTypesList(GetAllAdvertisementTypesListPVM getAllAdvertisementTypesListPVM);
        #endregion

        #region AdvertisementFeaturesManagement
        ResponseApiCaller UpdateAdvertisementFeatures(UpdateAdvertisementFeaturesPVM updateAdvertisementFeaturesPVM);
        ResponseApiCaller GetListOfFeaturesByPropertyTypeId(GetListOfFeaturesByPropertyTypeIdPVM getListOfFeaturesByPropertyTypeIdPVM);
        #endregion

        #region AdvertisementFeaturesValues
        ResponseApiCaller GetAdvertisementFeaturesValues(GetAdvertisementFeaturesValuesPVM getAdvertisementFeaturesValuesPVM);
        ResponseApiCaller UpdateAdvertisementFeatureValues(UpdateAdvertisementFeatureValuesPVM updateAdvertisementFeatureValuesPVM);
        ResponseApiCaller GetAllAdvertisementFeatureValues(GetAllAdvertisementFeatureValuesPVM getAllAdvertisementFeatureValuesPVM);
        #endregion

        #region AdvertisementViewers
        ResponseApiCaller GetAdvertisementViewersWithAdvertisementId(GetAdvertisementViewersWithAdvertisementIdPVM getAdvertisementViewersWithAdvertisementIdPVM);
        ResponseApiCaller AddToAdvertisementViewers(AddToAdvertisementViewersPVM addToAdvertisementViewersPVM);
        ResponseApiCaller GetAdvertisementsViewersReportWithIdAndFilterType(GetAdvertisementsViewersReportWithIdAndFilterTypePVM getAdvertisementsViewersReportWithIdAndFilterTypePVM);
        #endregion

        #region AdvertisementCallers
        ResponseApiCaller GetAdvertisementCallersWithAdvertisementId(GetAdvertisementCallersWithAdvertisementIdPVM getAdvertisementCallersWithAdvertisementIdPVM);
        ResponseApiCaller AddToAdvertisementCallers(AddToAdvertisementCallersPVM addToAdvertisementCallersPVM);
        #endregion

        #region AdvertisementFavorites
        ResponseApiCaller AddToAdvertisementFavorites(AddToAdvertisementFavoritesPVM addToAdvertisementFavoritesPVM);
        ResponseApiCaller CompleteDeleteAdvertisementFavorites(CompleteDeleteAdvertisementFavoritesPVM completeDeleteAdvertisementFavoritesPVM);
        #endregion

        #region BannersManagement

        ResponseApiCaller GetAllBannersList(GetAllBannersListPVM getAllBannersListPVM);
        ResponseApiCaller GetListOfBanners(GetListOfBannersPVM getListOfBanners);
        ResponseApiCaller AddToBanners(AddToBannersPVM addToBannersPVM);
        ResponseApiCaller UpdateBanners(UpdateBannersPVM updateBannersPVM);
        ResponseApiCaller TemporaryDeleteBanners(TemporaryDeleteBannersPVM temporaryDeleteBannersPVM);
        ResponseApiCaller ToggleActivationBanners(ToggleActivationBannersPVM toggleActivationBannersPVM);
        ResponseApiCaller CompleteDeleteBanners(CompleteDeleteBannersPVM completeDeleteBannersPVM);
        ResponseApiCaller GetBannersWithBannerId(GetBannersWithBannerIdPVM getBannerWithBannerIdPVM);

        #endregion

        #region BuildingLifesManagement

        ResponseApiCaller GetAllBuildingLifesList(GetAllBuildingLifesListPVM getAllBuildingLifesListPVM);

        #endregion

        #region SmsesManagement

        ResponseApiCaller GetListOfSmses(GetListOfSmsesPVM getListOfSmsesPVM);

        ResponseApiCaller AddToSmses(AddToSmsesPVM addToSmsesPVM);

        ResponseApiCaller UpdateSmses(UpdateSmsesPVM updateSmsesPVM);

        ResponseApiCaller ToggleActivationSmses(ToggleActivationSmsesPVM toggleActivationSmsesPVM);

        ResponseApiCaller TemporaryDeleteSmses(TemporaryDeleteSmsesPVM temporaryDeleteSmsesPVM);

        ResponseApiCaller CompleteDeleteSmses(CompleteDeleteSmsesPVM completeDeleteSmsesPVM);

        ResponseApiCaller GetAllSmsesList(GetAllSmsesListPVM getAllSmsesListPVM);

        #endregion

        #region UserProfileManagement
        ResponseApiCaller UpdateUserProfile(UpdateUserProfilePVM updateUserProfilePVM);
        #endregion

        #region MelkavanLogin
        ResponseApiCaller MelkavanLogin(MelkavanLoginPVM melkavanLoginPVM);
        #endregion

        #region SendSMSservice

        ResponseApiCaller SendSMSservice(MelkavanLoginPVM melkavanLoginPVM);

        #endregion

        #region AddOwnerOrConcultant

        ResponseApiCaller AddOwnerOrConcultant(AddOwnerOrConcultantPVM addOwnerOrConcultantPVM);

        #endregion

    }
}
