using ApiCallers.BaseApiCaller;
using VM.Melkavan.PVM.Melkavan.Advertisement;
using VM.Melkavan.PVM.Melkavan.Tags;
using VM.PVM.Melkavan;

namespace ApiCallers.MelkavanApiCaller
{
    public class MelkavanApiCaller : BaseCaller, IMelkavanApiCaller
    {
        public MelkavanApiCaller() : base()
        {
        }

        public MelkavanApiCaller(string _serviceUrl) : base(_serviceUrl)
        {
            serviceUrl = _serviceUrl;
        }

        public MelkavanApiCaller(string _serviceUrl,
            string _accessToken) :
            base(_serviceUrl,
                _accessToken)
        {
            serviceUrl = _serviceUrl;
        }


        #region AdvertisementManagement
        public ResponseApiCaller GetAllAdvertisementList(GetAllAdvertisementListPVM getAllAdvertisementsListPVM)
        {
            return GetRecords(getAllAdvertisementsListPVM);
        }

        public ResponseApiCaller GetListOfAdvertisement(GetListOfAdvertisementPVM getListOfAdvertisementPVM)
        {
            return GetRecords(getListOfAdvertisementPVM);
        }

        public ResponseApiCaller GetListOfAdverisementsAdvanceSearch(GetListOfAdvertisementAdvanceSearchPVM getListOfAdvertisementAdvanceSearchPVM)
        {
            return GetRecords(getListOfAdvertisementAdvanceSearchPVM);
        }

        public ResponseApiCaller AddToAdvertisement(AddToAdvertisementPVM addToAdvertisementPVM)
        {
            //try
            //{
            //    inputJson = JsonConvert.SerializeObject(addToAdvertisementPVM);
            //    inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
            //    response = client.PostAsync(serviceUrl, inputContent).Result;
            //    if (response.IsSuccessStatusCode)
            //    {
            //        responseApiCaller.IsSuccessStatusCode = response.IsSuccessStatusCode;
            //        byte[] bytes = response.Content.ReadAsByteArrayAsync().Result;
            //        string utfString = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            //        responseApiCaller.JsonResultWithRecordsObjectVM = JsonConvert.DeserializeObject<JsonResultWithRecordsObjectVM>(
            //                utfString, new JsonSerializerSettings
            //                {
            //                    NullValueHandling = NullValueHandling.Ignore
            //                });
            //    }
            //}
            //catch (Exception exc)
            //{ }
            //return responseApiCaller;


            return GetRecord(addToAdvertisementPVM);
        }

        public ResponseApiCaller GetAdvertisementWithAdvertisementId(GetAdvertisementWithAdvertisementIdPVM getAdvertisementWithAdvertisementIdPVM)
        {
            return GetRecord(getAdvertisementWithAdvertisementIdPVM);
        }

        public ResponseApiCaller GetAdvertisementDetailsWithAdvertisementId(GetAdvertisementWithAdvertisementIdPVM getAdvertisementWithAdvertisementIdPVM)
        {
            return GetRecord(getAdvertisementWithAdvertisementIdPVM);
        }

        public ResponseApiCaller UpdateAdvertisement(UpdateAdvertisementPVM updateAdvertisementPVM)
        {
            return GetRecord(updateAdvertisementPVM);
        }

        public ResponseApiCaller UpdateAdvertisementStatus(UpdateAdvertisementStatusPVM updateAdvertisementStatusPVM)
        {
            return GetRecord(updateAdvertisementStatusPVM);
        }

        public ResponseApiCaller UpdateAdvertisementTagId(UpdateAdvertisementTagIdPVM updateAdvertisementTagId)
        {
            return GetRecord(updateAdvertisementTagId);
        }

        public ResponseApiCaller ToggleActivationAdvertisement(ToggleActivationAdvertisementPVM toggleActivationAdvertisementPVM)
        {
            return GetResult(toggleActivationAdvertisementPVM);
        }

        public ResponseApiCaller TemporaryDeleteAdvertisement(TemporaryDeleteAdvertisementPVM temporaryDeleteAdvertisementPVM)
        {
            return GetResult(temporaryDeleteAdvertisementPVM);
        }

        public ResponseApiCaller TemporaryDeleteAdvertisementWithChild(TemporaryDeleteAdvertisementWithChildPVM temporaryDeleteAdvertisementWithChildPVM)
        {
            return GetResult(temporaryDeleteAdvertisementWithChildPVM);
        }
        public ResponseApiCaller CompleteDeleteAdvertisement(CompleteDeleteAdvertisementPVM completeDeleteAdvertisementPVM)
        {
            return GetResult(completeDeleteAdvertisementPVM);
        }


        public ResponseApiCaller GetAdvertisementsWithOwnerId(GetAdvertisementsWithOwnerIdPVM getAdvertisementsWithOwnerIdPVM)
        {
            return GetRecords(getAdvertisementsWithOwnerIdPVM);
        }

        public ResponseApiCaller GetWatchedAdvertisementsWithOwnerId(GetAdvertisementsWithOwnerIdPVM getAdvertisementsWithOwnerIdPVM)
        {
            return GetRecords(getAdvertisementsWithOwnerIdPVM);
        }

        public ResponseApiCaller GetListOfAdvertisementWithMoreDetail(GetListOfAdvertisementWithMoreDetailPVM getListOfAdvertisementWithMoreDetailPVM)
        {
            return GetRecords(getListOfAdvertisementWithMoreDetailPVM);
        }

     


        #endregion

        #region AdvertisementLocationManagement

        public ResponseApiCaller UpdateAdvertisementLocation(UpdateAdvertisementLocationPVM updateAdvertisementlocationPVM)
        {
            return GetResult(updateAdvertisementlocationPVM);
        }

        #endregion

        #region AdvertisementFilesManagement

        public ResponseApiCaller GetAllAdvertisementFilesList(GetAllAdvertisementFilesListPVM getAllAdvertisementFilesListPVM)
        {
            return GetRecords(getAllAdvertisementFilesListPVM);
        }

        public ResponseApiCaller GetListOfAdvertisementFiles(GetListOfAdvertisementFilesPVM getListOfAdvertisementFilesPVM)
        {
            return GetRecords(getListOfAdvertisementFilesPVM);
        }

        public ResponseApiCaller AddToAdvertisementFiles(AddToAdvertisementFilesPVM addToAdvertisementFilesPVM)
        {
            return GetRecord(addToAdvertisementFilesPVM);
        }

        public ResponseApiCaller GetAdvertisementFileWithAdvertisementFileId(GetAdvertisementFileWithAdvertisementFileIdPVM getAdvertisementFileWithAdvertisementFileIdPVM)
        {
            return GetRecord(getAdvertisementFileWithAdvertisementFileIdPVM);
        }

        public ResponseApiCaller UpdateAdvertisementFiles(UpdateAdvertisementFilesPVM updateAdvertisementFilesPVM)
        {
            return GetRecord(updateAdvertisementFilesPVM);
        }

        public ResponseApiCaller ToggleActivationAdvertisementFiles(ToggleActivationAdvertisementFilesPVM toggleActivationPAdvertisementFilesPVM)
        {
            return GetResult(toggleActivationPAdvertisementFilesPVM);
        }

        public ResponseApiCaller TemporaryDeleteAdvertisementFiles(TemporaryDeleteAdvertisementFilesPVM temporaryDeleteAdvertisementFilesPVM)
        {
            return GetResult(temporaryDeleteAdvertisementFilesPVM);
        }

        public ResponseApiCaller CompleteDeleteAdvertisementFiles(CompleteDeleteAdvertisementFilesPVM completeDeleteAdvertisementFilesPVM)
        {
            return GetResult(completeDeleteAdvertisementFilesPVM);
        }

        #endregion

        #region AdvertisementsPricesHistoriesManagement

        public ResponseApiCaller GetListOfAdvertisementsPricesHistories(GetListOfAdvertisementPricesHistoriesPVM getListOfAdvertisementPricesHistoriesPVM)
        {
            return GetRecords(getListOfAdvertisementPricesHistoriesPVM);
        }
        #endregion

        #region AdvertisementTypesManagement

        public ResponseApiCaller GetAllAdvertisementTypesList(GetAllAdvertisementTypesListPVM getAllAdvertisementTypesListPVM)
        {
            return GetRecords(getAllAdvertisementTypesListPVM);
        }

        #endregion

        #region AdvertisementFeaturesManagement
        public ResponseApiCaller UpdateAdvertisementFeatures(UpdateAdvertisementFeaturesPVM updateAdvertisementFeaturesPVM)
        {
            return GetResult(updateAdvertisementFeaturesPVM);
        }
        public ResponseApiCaller GetListOfFeaturesByPropertyTypeId(GetListOfFeaturesByPropertyTypeIdPVM getListOfFeaturesByPropertyTypeIdPVM)
        {
            return GetRecord(getListOfFeaturesByPropertyTypeIdPVM);
        }
        #endregion

        #region AdvertisementFeaturesValues

        public ResponseApiCaller GetAdvertisementFeaturesValues(GetAdvertisementFeaturesValuesPVM getAdvertisementFeaturesValuesPVM)
        {
            return GetRecord(getAdvertisementFeaturesValuesPVM);
        }
        public ResponseApiCaller UpdateAdvertisementFeatureValues(UpdateAdvertisementFeatureValuesPVM updateAdvertisementFeatureValuesPVM)
        {
            return GetRecord(updateAdvertisementFeatureValuesPVM);
        }


        public ResponseApiCaller GetAllAdvertisementFeatureValues(GetAllAdvertisementFeatureValuesPVM getAllAdvertisementFeatureValuesPVM)
        {
            return GetRecord(getAllAdvertisementFeatureValuesPVM);
        }


        #endregion

        #region AdvertisementViewers
        public ResponseApiCaller GetAdvertisementViewersWithAdvertisementId(GetAdvertisementViewersWithAdvertisementIdPVM getAdvertisementViewersWithAdvertisementIdPVM)
        {
            return GetRecords(getAdvertisementViewersWithAdvertisementIdPVM);
        }
        public ResponseApiCaller AddToAdvertisementViewers(AddToAdvertisementViewersPVM addToAdvertisementViewersPVM)
        {
            return GetRecord(addToAdvertisementViewersPVM);
        }
        public ResponseApiCaller GetAdvertisementsViewersReportWithIdAndFilterType(GetAdvertisementsViewersReportWithIdAndFilterTypePVM getAdvertisementsViewersReportWithIdAndFilterTypePVM)
        {
            return GetRecords(getAdvertisementsViewersReportWithIdAndFilterTypePVM);

        }
        #endregion

        #region AdvertisementCallers
        public ResponseApiCaller GetAdvertisementCallersWithAdvertisementId(GetAdvertisementCallersWithAdvertisementIdPVM getAdvertisementCallersWithAdvertisementIdPVM)
        {
            return GetRecords(getAdvertisementCallersWithAdvertisementIdPVM);
        }
        public ResponseApiCaller AddToAdvertisementCallers(AddToAdvertisementCallersPVM addToAdvertisementCallersPVM)
        {
            return GetRecord(addToAdvertisementCallersPVM);
        }
        #endregion

        #region AdvertisementFavorites
        public ResponseApiCaller AddToAdvertisementFavorites(AddToAdvertisementFavoritesPVM addToAdvertisementFavoritesPVM)
        {
            return GetRecord(addToAdvertisementFavoritesPVM);
        }
        public ResponseApiCaller CompleteDeleteAdvertisementFavorites(CompleteDeleteAdvertisementFavoritesPVM completeDeleteAdvertisementFavoritesPVM)
        {
            return GetResult(completeDeleteAdvertisementFavoritesPVM);
        }
        #endregion

        #region BannersManagement

        public ResponseApiCaller GetAllBannersList(GetAllBannersListPVM getAllBannersListPVM)
        {
            return GetRecords(getAllBannersListPVM);
        }
        public ResponseApiCaller GetListOfBanners(GetListOfBannersPVM getListOfBanners)
        {
            return GetRecords(getListOfBanners);
        }

        public ResponseApiCaller AddToBanners(AddToBannersPVM addToBannersPVM)
        {
            return GetRecord(addToBannersPVM);
        }

        public ResponseApiCaller UpdateBanners(UpdateBannersPVM updateBannersPVM)
        {
            return GetRecord(updateBannersPVM);
        }
        public ResponseApiCaller ToggleActivationBanners(ToggleActivationBannersPVM toggleActivationBannersPVM)
        {
            return GetResult(toggleActivationBannersPVM);
        }
        public ResponseApiCaller TemporaryDeleteBanners(TemporaryDeleteBannersPVM temporaryDeleteBannersPVM)
        {
            return GetResult(temporaryDeleteBannersPVM);
        }
        public ResponseApiCaller CompleteDeleteBanners(CompleteDeleteBannersPVM completeDeleteBannersPVM)
        {
            return GetResult(completeDeleteBannersPVM);
        }
        public ResponseApiCaller GetBannersWithBannerId(GetBannersWithBannerIdPVM getBannerWithBannerIdPVM)
        {
            return GetRecord(getBannerWithBannerIdPVM);
        }



        #endregion

        #region BuildingLifesManagement

        public ResponseApiCaller GetAllBuildingLifesList(GetAllBuildingLifesListPVM getAllBuildingLifesListPVM)
        {
            return GetRecords(getAllBuildingLifesListPVM);
        }

        #endregion

        #region TagsManagement

        public ResponseApiCaller GetAllTagsList(GetAllTagsListPVM getAllTagsListPVM)
        {
            return GetRecords(getAllTagsListPVM);
        }

        #endregion

        #region SmsesManagement


        public ResponseApiCaller GetAllSmsesList(GetAllSmsesListPVM getAllSmsesListPVM)
        {
            return GetRecords(getAllSmsesListPVM);
        }


        public ResponseApiCaller GetListOfSmses(GetListOfSmsesPVM getListOfSmsesPVM)
        {
            return GetRecords(getListOfSmsesPVM);
        }

        public ResponseApiCaller AddToSmses(AddToSmsesPVM addToSmsesPVM)
        {
            return GetRecord(addToSmsesPVM);
        }

        public ResponseApiCaller UpdateSmses(UpdateSmsesPVM updateSmsesPVM)
        {
            return GetRecord(updateSmsesPVM);
        }

        public ResponseApiCaller ToggleActivationSmses(ToggleActivationSmsesPVM toggleActivationSmsesPVM)
        {
            return GetResult(toggleActivationSmsesPVM);
        }

        public ResponseApiCaller TemporaryDeleteSmses(TemporaryDeleteSmsesPVM temporaryDeleteSmsesPVM)
        {
            return GetResult(temporaryDeleteSmsesPVM);
        }

        public ResponseApiCaller CompleteDeleteSmses(CompleteDeleteSmsesPVM completeDeleteSmsesPVM)
        {
            return GetResult(completeDeleteSmsesPVM);
        }



        #endregion

        #region UserProfileManagement
        public ResponseApiCaller UpdateUserProfile(UpdateUserProfilePVM updateUserProfilePVM)
        {
            return GetRecord(updateUserProfilePVM);
        }

        public ResponseApiCaller GetAdvertiserProfileWithAdvertiserId(GetAdvertiserProfileWithAdvertiserIdPVM getAdvertiserProfileWithAdvertiserIdPVM)
        {
            return GetRecord(getAdvertiserProfileWithAdvertiserIdPVM);
        }
        #endregion

        #region MelkavanLogin


        public ResponseApiCaller MelkavanLogin(MelkavanLoginPVM melkavanLoginPVM)
        {
            return GetRecord(melkavanLoginPVM);
        }

        #endregion

        #region SendSMSservice

        public ResponseApiCaller SendSMSservice(MelkavanLoginPVM melkavanLoginPVM)
        {
            return GetRecord(melkavanLoginPVM);
        }

        #endregion

        #region AddOwnerOrConcultant

        public ResponseApiCaller AddOwnerOrConcultant(AddOwnerOrConcultantPVM addOwnerOrConcultantPVM)
        {
            return GetRecord(addOwnerOrConcultantPVM);
        }

        #endregion


    }
}
