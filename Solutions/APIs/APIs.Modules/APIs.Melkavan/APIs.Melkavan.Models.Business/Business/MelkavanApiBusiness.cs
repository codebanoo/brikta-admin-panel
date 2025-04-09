using APIs.Melkavan.Models.Entities;
using APIs.Public.Models;
using APIs.Public.Models.Business;
using APIs.Teniaco.Models;
using APIs.Teniaco.Models.Business;
using APIs.Teniaco.Models.Entities;
using AutoMapper;
using Castle.Core.Internal;
using FrameWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Models.Business.ConsoleBusiness;
using Models.Entities.Console;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using VM.Console;
using VM.Melkavan;
using VM.Melkavan.VM.Melkavan;
using VM.Public;
using VM.PVM.Melkavan;
using VM.Teniaco;
using console = Models.Entities.Console;
using Properties = APIs.Teniaco.Models.Entities.Properties;


namespace APIs.Melkavan.Models.Business
{
    public class MelkavanApiBusiness : IMelkavanApiBusiness, IDisposable
    {
        private MelkavanApiContext melkavanApiDb = new MelkavanApiContext();


        private IMapper _mapper;

        private IHostEnvironment hostingEnvironment;

        public MelkavanApiContext MelkavanApiDb
        {
            get { return this.melkavanApiDb; }
            set { }
            //private set { }
        }


        public void Dispose()
        {
            melkavanApiDb.Dispose();

        }

        public MelkavanApiBusiness(IMapper mapper,
            MelkavanApiContext _melkavanApiDb,
            IHostEnvironment _hostingEnvironment)
        {
            try
            {
                _mapper = mapper;

                melkavanApiDb = _melkavanApiDb;

                MelkavanApiDb = melkavanApiDb;



                hostingEnvironment = _hostingEnvironment;
            }
            catch (Exception exc)
            {
            }
        }

        #region Melkavan

        #region Methods For Work With Advertisement




        public List<AdvertisementVM> GetAllAdvertisementList(
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
            string jtSorting = null)
        {
            List<AdvertisementVM> AdvertisementVMList = new List<AdvertisementVM>();

            try
            {

                var districts = (from s in publicApiDb.States
                                 join c in publicApiDb.Cities on s.StateId equals c.StateId
                                 join z in publicApiDb.Zones on c.CityId equals z.CityId
                                 join d in publicApiDb.Districts on z.ZoneId equals d.ZoneId
                                 select new DistrictsVM
                                 {
                                     CityId = c.CityId,
                                     StateId = s.StateId,
                                     ZoneId = z.ZoneId,
                                     DistrictId = d.DistrictId
                                 }).AsEnumerable();

                var list = (from p in melkavanApiDb.Advertisement
                            join pa in melkavanApiDb.AdvertisementAddress on p.AdvertisementId equals pa.AdvertisementId
                            join ad in melkavanApiDb.AdvertisementDetails on p.AdvertisementId equals ad.AdvertisementId
                            where childsUsersIds.Contains(p.UserIdCreator.Value) &&
                            p.IsActivated.Value.Equals(true) &&
                            p.IsDeleted.Value.Equals(false)
                            select new AdvertisementVM
                            {
                                Area = p.Area,
                                BuiltInYear = p.BuiltInYear.HasValue ? p.BuiltInYear.Value : (int?)0,
                                BuiltInYearFa = p.BuiltInYearFa.HasValue ? p.BuiltInYearFa.Value : (int?)0,
                                //IntermediaryPersonId = p.IntermediaryPersonId.HasValue ? p.IntermediaryPersonId.Value : (long?)null,
                                //OwnerPersonId = p.OwnerPersonId.HasValue ? p.OwnerPersonId.Value : (long?)null,
                                //Intermediary = p.Intermediary,
                                //IntermediaryPhone = p.IntermediaryPhone,
                                //IsPrivate = p.IsPrivate,
                                AdvertisementTitle = p.AdvertisementTitle,
                                AdvertisementId = p.AdvertisementId,
                                PropertyTypeId = p.PropertyTypeId,
                                RebuiltInYear = p.BuiltInYear.HasValue ? p.BuiltInYear.Value : (int?)0,
                                RebuiltInYearFa = p.RebuiltInYearFa.HasValue ? p.RebuiltInYearFa.Value : (int?)0,
                                TypeOfUseId = p.TypeOfUseId.HasValue ? p.TypeOfUseId.Value : (int?)0,
                                DocumentTypeId = p.DocumentOwnershipTypeId.HasValue ? p.DocumentOwnershipTypeId.Value : (int?)0,
                                DocumentOwnershipTypeId = p.DocumentOwnershipTypeId.HasValue ? p.DocumentOwnershipTypeId.Value : (int?)0,
                                DocumentRootTypeId = p.DocumentRootTypeId.HasValue ? p.DocumentRootTypeId.Value : (int?)0,
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
                                AdvertisementAddressVM = new AdvertisementAddressVM
                                {
                                    StateId = 0,
                                    CityId = 0,
                                    ZoneId = pa.ZoneId,
                                    DistrictId = pa.DistrictId,
                                    Address = pa.Address,
                                    LocationLat = pa.LocationLat,
                                    LocationLon = pa.LocationLon,
                                    AdvertisementId = pa.AdvertisementId,
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
                                },
                                AdvertisementDetailsVM = new AdvertisementDetailsVM
                                {
                                    AdvertisementId = p.AdvertisementId,
                                    AdvertisementDetailsId = ad.AdvertisementDetailsId,
                                    AdvertisementTypeId = ad.AdvertisementTypeId,
                                    Convertable = ad.Convertable,
                                    MaritalStatusId = ad.MaritalStatusId,
                                    //MaritalStatusTitle = ad.MaritalStatusTitle,
                                    ShowInSpecialSuggestion = ad.ShowInSpecialSuggestion,
                                    VerifyAdvertisement = ad.VerifyAdvertisement,
                                    BuildingLifeId = ad.BuildingLifeId,
                                    Foundation = ad.Foundation,
                                    UserIdCreator = ad.UserIdCreator.Value,
                                    CreateEnDate = ad.CreateEnDate,
                                    CreateTime = ad.CreateTime,
                                    EditEnDate = ad.EditEnDate,
                                    EditTime = ad.EditTime,
                                    UserIdEditor = ad.UserIdEditor.Value,
                                    RemoveEnDate = ad.RemoveEnDate,
                                    RemoveTime = ad.EditTime,
                                    UserIdRemover = ad.UserIdRemover.Value,
                                    IsActivated = ad.IsActivated,
                                    IsDeleted = ad.IsDeleted,
                                }
                            })
                            .AsEnumerable()
                            .Join(districts, pa => pa.AdvertisementAddressVM.DistrictId, z => z.DistrictId, (a, b) => new { a, b })
                            .Select(p => new AdvertisementVM
                            {
                                Area = p.a.Area,
                                BuiltInYear = p.a.BuiltInYear.HasValue ? p.a.BuiltInYear.Value : (int?)0,
                                BuiltInYearFa = p.a.BuiltInYearFa.HasValue ? p.a.BuiltInYearFa.Value : (int?)0,
                                //IntermediaryPersonId = p.a.IntermediaryPersonId.HasValue ? p.a.IntermediaryPersonId.Value : (long?)null,
                                //OwnerPersonId = p.a.OwnerPersonId.HasValue ? p.a.OwnerPersonId.Value : (long?)null,
                                //Intermediary = p.a.Intermediary,
                                //IntermediaryPhone = p.a.IntermediaryPhone,
                                //IsPrivate = p.a.IsPrivate,
                                AdvertisementTitle = p.a.AdvertisementTitle,
                                AdvertisementId = p.a.AdvertisementId,
                                PropertyTypeId = p.a.PropertyTypeId,
                                RebuiltInYear = p.a.BuiltInYear.HasValue ? p.a.BuiltInYear.Value : (int?)0,
                                RebuiltInYearFa = p.a.RebuiltInYearFa.HasValue ? p.a.RebuiltInYearFa.Value : (int?)0,
                                TypeOfUseId = p.a.TypeOfUseId.HasValue ? p.a.TypeOfUseId.Value : (int?)0,
                                DocumentTypeId = p.a.DocumentTypeId.HasValue ? p.a.DocumentTypeId.Value : (int?)0,
                                DocumentOwnershipTypeId = p.a.DocumentOwnershipTypeId.HasValue ? p.a.DocumentOwnershipTypeId.Value : (int?)0,
                                DocumentRootTypeId = p.a.DocumentRootTypeId.HasValue ? p.a.DocumentRootTypeId.Value : (int?)0,
                                UserIdCreator = p.a.UserIdCreator.Value,
                                CreateEnDate = p.a.CreateEnDate,
                                CreateTime = p.a.CreateTime,
                                EditEnDate = p.a.EditEnDate,
                                EditTime = p.a.EditTime,
                                UserIdEditor = p.a.UserIdEditor.Value,
                                RemoveEnDate = p.a.RemoveEnDate,
                                RemoveTime = p.a.EditTime,
                                UserIdRemover = p.a.UserIdRemover.Value,
                                IsActivated = p.a.IsActivated,
                                IsDeleted = p.a.IsDeleted,
                                AdvertisementAddressVM = new AdvertisementAddressVM
                                {
                                    StateId = p.b.StateId.Value,
                                    CityId = p.b.CityId.Value,
                                    ZoneId = p.b.ZoneId,
                                    DistrictId = p.b.DistrictId,
                                    //Abbreviation = p.a.AdvertisementAddressVM.Address,
                                    Address = p.a.AdvertisementAddressVM.Address,
                                    LocationLat = p.a.AdvertisementAddressVM.LocationLat,
                                    LocationLon = p.a.AdvertisementAddressVM.LocationLon,
                                    AdvertisementId = p.a.AdvertisementAddressVM.AdvertisementId,
                                    UserIdCreator = p.a.AdvertisementAddressVM.UserIdCreator.Value,
                                    CreateEnDate = p.a.AdvertisementAddressVM.CreateEnDate,
                                    CreateTime = p.a.AdvertisementAddressVM.CreateTime,
                                    EditEnDate = p.a.AdvertisementAddressVM.EditEnDate,
                                    EditTime = p.a.AdvertisementAddressVM.EditTime,
                                    UserIdEditor = p.a.AdvertisementAddressVM.UserIdEditor.Value,
                                    RemoveEnDate = p.a.AdvertisementAddressVM.RemoveEnDate,
                                    RemoveTime = p.a.AdvertisementAddressVM.EditTime,
                                    UserIdRemover = p.a.AdvertisementAddressVM.UserIdRemover.Value,
                                    IsActivated = p.a.AdvertisementAddressVM.IsActivated,
                                    IsDeleted = p.a.AdvertisementAddressVM.IsDeleted,
                                }
                            }).AsQueryable();



                if (advertisementTypeId.HasValue)
                    if (advertisementTypeId.Value > 0)
                        list = list.Where(a => a.AdvertisementDetailsVM.AdvertisementTypeId.Equals(advertisementTypeId.Value));


                if (propertyTypeId.HasValue)
                    if (propertyTypeId.Value > 0)
                        list = list.Where(a => a.PropertyTypeId.Equals(propertyTypeId.Value));



                if (typeOfUseId.HasValue)
                    if (typeOfUseId.Value > 0)
                        list = list.Where(a => a.TypeOfUseId.Equals(typeOfUseId.Value));


                if (documentTypeId.HasValue)
                    if (documentTypeId.Value > 0)
                        list = list.Where(a => a.DocumentTypeId.Equals(documentTypeId.Value));



                //if (documentOwnershipTypeId.HasValue)
                //    if (documentOwnershipTypeId.Value > 0)
                //        list = list.Where(a => a.DocumentOwnershipTypeId.Equals(documentOwnershipTypeId.Value));


                //if (documentRootTypeId.HasValue)
                //    if (documentRootTypeId.Value > 0)
                //        list = list.Where(a => a.DocumentRootTypeId.Equals(documentRootTypeId.Value));


                if (!string.IsNullOrEmpty(propertyCodeName))
                    list = list.Where(z => z.AdvertisementTitle.Contains(propertyCodeName));


                //if (!string.IsNullOrEmpty(intermediary))
                //    list = list.Where(z => z.Intermediary.Contains(intermediary) ||
                //            z.IntermediaryPhone.Contains(intermediary));


                if (stateId.HasValue)
                    if (stateId.Value > 0)
                        list = list.Where(a => a.AdvertisementAddressVM.StateId.Equals(stateId.Value));


                if (cityId.HasValue)
                    if (cityId.Value > 0)
                        list = list.Where(a => a.AdvertisementAddressVM.CityId.Equals(cityId.Value));


                if (zoneId.HasValue)
                    if (zoneId.Value > 0)
                        list = list.Where(a => a.AdvertisementAddressVM.ZoneId.Equals(zoneId.Value));


                if (districtId.HasValue)
                    if (districtId.Value > 0)
                        list = list.Where(a => a.AdvertisementAddressVM.DistrictId.Equals(districtId.Value));



                //if (intermediaryPersonId.HasValue)
                //    if (intermediaryPersonId.Value > 0)
                //        list = list.Where(a => a.IntermediaryPersonId.Value.Equals(intermediaryPersonId.Value));


                //if (ownerPersonId.HasValue)
                //    if (ownerPersonId.Value > 0)
                //        list = list.Where(a => a.OwnerPersonId.Value.Equals(ownerPersonId.Value));


                //if (isPrivate.HasValue)
                //    list = list.Where(a => a.IsPrivate.Equals(isPrivate.Value));

                //zonesVM = _mapper.Map<List<Zones>,
                //    List<ZonesVM>>(list.OrderByDescending(f => f.ZoneId).ToList());


                AdvertisementVMList = list.OrderByDescending(f => f.AdvertisementId).ToList();


            }
            catch (Exception exc)
            { }

            return AdvertisementVMList;
        }


        #region Sina's code

        //public List<AdvertisementAdvanceSearchVM> GetListOfAdverisementsAdvanceSearch(
        //                 int jtStartIndex,
        //                 int jtPageSize,
        //                 ref int listCount,
        //                 List<long> childsUsersIds,
        //                 PublicApiContext publicApiDb,
        //                 TeniacoApiContext teniacoApiDb,
        //                 string jtSorting = null,
        //                 string advertisementTitle = null,
        //                 bool? onlyFavorite = null,
        //                 long? userId = null, bool IsFiltered = false,
        //                 //parameters for advanced filters
        //                 int? AdvertisementTypeId = null,
        //                 int? PropertyTypeId = null,
        //                 string? TypeOfUseId = null,
        //                 long? StateId = null,
        //                 long? CityId = null,
        //                 long? ZoneId = null,
        //                 string? TownName = null,
        //                 string? FromArea = null,
        //                 string? ToArea = null,
        //                 int? FromFoundation = null,
        //                 int? ToFoundation = null,
        //                 long? FromPrice = 0,
        //                 long? ToPrice = 0,
        //                 string? BuildingLifeId = null,
        //                 int? FromRebuiltInYear = null,
        //                 int? ToRebuiltInYear = null,
        //                 int? DocumentTypeId = null,
        //                 int? DocumentRootTypeId = null,
        //                 int? DocumentOwnershipTypeId = null,
        //                 long? DepositFromPrice = 0,
        //                 long? DepositToPrice = 0,
        //                 long? RentFromPrice = 0,
        //                 long? RentToPrice = 0,
        //                 int? MaritalStatusId = null,
        //                 string? Convertable = null,
        //                 Dictionary<string, string>? Features = null)
        //{

        //    List<AdvertisementAdvanceSearchVM> advertisementAdvanceSearchVMList = new List<AdvertisementAdvanceSearchVM>();

        //    try
        //    {
        //        string sp = @"";


        //        sp = @"
        //                  SELECT DISTINCT * FROM (
        //                            SELECT 'Advertisement' AS RecordType,
        //                                    ad.StatusId,
        //                                    ad.RejectionReason,
        //                                    details.InstagramLink,
        //                                    details.AdvertisementTypeId,
        //                                    ad.AdvertisementId,
        //                                    '' AS PropertyTypeTitle,
        //                                    '' AS TypeUseTitle,
        //                                    '' AS DocumentTypeTitle,
        //                                    '' AS DocumentOwnershipTypeTitle,
        //                                    '' AS DocumentRootTypeTitle,
        //                                    '' AS StateName,
        //                                    '' AS CityName,
        //                                    '' AS ZoneName,
        //                                    '' AS DistrictName,
        //                                    '' AS UserCreatorName,
        //                                    ad.IsActivated,
        //                                    ad.IsDeleted,
        //                                    ad.ConsultantUserId,
        //                                    ad.EditEnDate,
        //                                    ad.RebuiltInYearFa,
        //                                    ad.CreateEnDate,
        //                                    ad.PropertyTypeId,
        //                                    ad.TypeOfUseId,
        //                                    ad.DocumentOwnershipTypeId,
        //                                    ad.DocumentTypeId,
        //                                    ad.DocumentRootTypeId,
        //                                    ad.AdvertisementTitle,
        //                                    ad.Area,
        //                                    ad.AdvertisementDescriptions,
        //                                    ad.UserIdCreator,
        //                                    '1' AS ShowInMelkavan,
        //                                    addr.CountryId,
        //                                    addr.StateId,
        //                                    addr.CityId,
        //                                    addr.ZoneId,
        //                                    addr.DistrictId,
        //                                    addr.TownName,
        //                                    priceHist.OfferPrice,
        //                                    priceHist.DepositPrice,
        //                                    priceHist.RentPrice,
        //                                    '' AS CurrentDate,
        //                                    details.TagId,
        //                                    CAST(CASE WHEN fav.AdvertisementId IS NOT NULL THEN 1 ELSE 0 END AS BIT) AS IsFavorite,
        //                                    ISNULL(viewers.ViewersCount, 0) AS ViewersCount
        //                                    FROM MelkavanDbHaghighi.dbo.Advertisement AS ad
        //                                    INNER JOIN MelkavanDbHaghighi.dbo.AdvertisementAddress AS addr ON ad.AdvertisementId = addr.AdvertisementId
        //                                    INNER JOIN (
        //                                        SELECT AdvertisementId, OfferPrice, DepositPrice, RentPrice
        //                                        FROM MelkavanDbHaghighi.dbo.AdvertisementPricesHistories AS aph
        //                                        WHERE aph.AdvertisementPriceHistoryId = (
        //                                            SELECT MAX(AdvertisementPriceHistoryId)
        //                                            FROM MelkavanDbHaghighi.dbo.AdvertisementPricesHistories
        //                                            WHERE AdvertisementId = aph.AdvertisementId
        //                                        )
        //                                    ) AS priceHist ON ad.AdvertisementId = priceHist.AdvertisementId
        //                                    INNER JOIN MelkavanDbHaghighi.dbo.AdvertisementDetails AS details ON ad.AdvertisementId = details.AdvertisementId
        //                                    LEFT JOIN MelkavanDbHaghighi.dbo.AdvertisementFavorites AS fav ON ad.AdvertisementId = fav.AdvertisementId
        //                                    LEFT JOIN (
        //                                        SELECT AdvertisementId, COUNT(*) AS ViewersCount
        //                                        FROM MelkavanDbHaghighi.dbo.AdvertisementViewers
        //                                        GROUP BY AdvertisementId
        //                                    ) AS viewers ON ad.AdvertisementId = viewers.AdvertisementId
        //                                    {JoinMelkavanFeatures} 
        //                                    {JoinAdvertisementFavorites}
        //                                    {whereCluaseMelkavan}
        //                                    UNION ALL
        //                                    SELECT 'Properties' AS RecordType,
        //                                    p.StatusId,
        //                                    p.RejectionReason,
        //                                    '' as InstagramLink,
        //                                    pd.AdvertisementTypeId,
        //                                    p.PropertyId AS AdvertisementId,
        //                                    '' AS PropertyTypeTitle,
        //                                    '' AS TypeUseTitle,
        //                                    '' AS DocumentTypeTitle,
        //                                    '' AS DocumentOwnershipTypeTitle,
        //                                    '' AS DocumentRootTypeTitle,
        //                                    '' AS StateName,
        //                                    '' AS CityName,
        //                                    '' AS ZoneName,
        //                                    '' AS DistrictName,
        //                                    '' AS UserCreatorName,
        //                                    p.IsActivated,
        //                                    p.IsDeleted,
        //                                    p.ConsultantUserId,
        //                                    p.EditEnDate,
        //                                    p.RebuiltInYearFa,
        //                                    p.CreateEnDate,
        //                                    p.PropertyTypeId,
        //                                    p.TypeOfUseId,
        //                                    p.DocumentOwnershipTypeId,
        //                                    p.DocumentTypeId,
        //                                    p.DocumentRootTypeId,
        //                                    p.PropertyCodeName AS AdvertisementTitle,
        //                                    p.Area,
        //                                    p.PropertyDescriptions AS AdvertisementDescriptions,
        //                                    p.UserIdCreator,
        //                                    p.ShowInMelkavan,
        //                                    propAddr.CountryId,
        //                                    propAddr.StateId,
        //                                    propAddr.CityId,
        //                                    propAddr.ZoneId,
        //                                    propAddr.DistrictId,
        //                                    propAddr.TownName,
        //                                    propPriceHist.OfferPrice,
        //                                    propPriceHist.DepositPrice,
        //                                    propPriceHist.RentPrice,
        //                                    '' AS CurrentDate,
        //                                    pd.TagId,
        //                                    CAST(CASE WHEN fav.PropertyId IS NOT NULL THEN 1 ELSE 0 END AS BIT) AS IsFavorite,
        //                                    ISNULL(viewers.ViewersCount, 0) AS ViewersCount
        //                                    FROM TeniacoDbHaghighi.dbo.Properties AS p
        //                                    INNER JOIN TeniacoDbHaghighi.dbo.PropertyAddress AS propAddr ON p.PropertyId = propAddr.PropertyId
        //                                    INNER JOIN (
        //                                        SELECT PropertyId, CASE OfferPriceType WHEN '0' THEN CalculatedOfferPrice ELSE OfferPrice END as OfferPrice, DepositPrice, RentPrice
        //                                        FROM TeniacoDbHaghighi.dbo.PropertiesPricesHistories AS pph
        //                                        WHERE pph.PropertyPriceHistoryId = (
        //                                            SELECT MAX(PropertyPriceHistoryId)
        //                                            FROM TeniacoDbHaghighi.dbo.PropertiesPricesHistories
        //                                            WHERE PropertyId = pph.PropertyId
        //                                        )
        //                                    ) AS propPriceHist ON p.PropertyId = propPriceHist.PropertyId
        //                                    INNER JOIN TeniacoDbHaghighi.dbo.PropertiesDetails AS pd ON p.PropertyId = pd.PropertyId
        //                                    LEFT JOIN TeniacoDbHaghighi.dbo.PropertiesFavorites AS fav ON p.PropertyId = fav.PropertyId
        //                                    LEFT JOIN (
        //                                        SELECT PropertyId, COUNT(*) AS ViewersCount
        //                                        FROM TeniacoDbHaghighi.dbo.PropertiesViewers
        //                                        GROUP BY PropertyId
        //                                    ) AS viewers ON p.PropertyId = viewers.PropertyId

        //                                    {JoinTeniacoFeatures}
        //                                    {JoinPropertiesFavorites}
        //                                    WHERE p.ShowInMelkavan = 1 {whereCluaseTeniaco}
        //                                    ) AS tmp
        //                                    ORDER BY CreateEnDate DESC;  ";


        //        string joinTeniacoFeatureValues = "";
        //        string joinMelkavanFeatureValues = "";
        //        string AdvancedFilters = "";

        //        string strWhereCluaseMelkavan = "where";
        //        string strWhereCluaseTeniaco = "";

        //        if (IsFiltered == true)
        //        {



        //            if (AdvertisementTypeId.HasValue && AdvertisementTypeId > 0)
        //                AdvancedFilters += " AND AdvertisementTypeId = " + AdvertisementTypeId;


        //            if (PropertyTypeId.HasValue && PropertyTypeId > 0)
        //                AdvancedFilters += " AND PropertyTypeId = " + PropertyTypeId;


        //            if (!String.IsNullOrEmpty(TypeOfUseId))
        //                AdvancedFilters += " AND TypeOfUseId IN (" + TypeOfUseId + ")";


        //            if (StateId.HasValue && StateId > 0)
        //                AdvancedFilters += " AND StateId = " + StateId;


        //            if (CityId.HasValue && CityId > 0)
        //                AdvancedFilters += " AND CityId = " + CityId;


        //            if (ZoneId.HasValue && ZoneId > 0)
        //                AdvancedFilters += " AND ZoneId = " + ZoneId;


        //            if (!String.IsNullOrEmpty(TownName))
        //                AdvancedFilters += " AND TownName like N'%" + TownName + "%' ";


        //            if (!String.IsNullOrEmpty(FromArea) && FromArea != "0")
        //                AdvancedFilters += " AND CAST(Area AS float) > " + FromArea;


        //            if (!String.IsNullOrEmpty(ToArea) && ToArea != "0")
        //                AdvancedFilters += " AND CAST(Area AS float) < " + ToArea;


        //            if (FromFoundation.HasValue && FromFoundation > 0)
        //                AdvancedFilters += " AND Foundation > " + FromFoundation;


        //            if (ToFoundation.HasValue && ToFoundation > 0)
        //                AdvancedFilters += " AND Foundation < " + ToFoundation;


        //            //if (FromPrice.HasValue && FromPrice>0)
        //            //    AdvancedFilters += " AND OfferPrice > " + FromPrice;


        //            //if (ToPrice.HasValue && ToPrice!=0 && ToPrice!=100000000000)
        //            //  AdvancedFilters += " AND OfferPrice < " + ToPrice;


        //            if (FromPrice.HasValue && FromPrice != 1000000000 && ToPrice.HasValue && ToPrice != 100000000000)
        //                AdvancedFilters += @" AND ((OfferPriceType = 0 and CAST(CalculatedOfferPrice AS bigint) >= " + FromPrice.Value.ToString() +
        //                                    @") and (OfferPriceType = 0 and CAST(CalculatedOfferPrice AS bigint)  <= " + ToPrice.Value.ToString() + @")) or 
        //                           ((OfferPriceType = 1 and CAST(OfferPrice AS bigint) >= " + FromPrice + ") and (OfferPriceType = 1 and CAST(OfferPrice AS bigint) <= " + ToPrice.Value.ToString() + ")) ";
        //            else
        //           if (FromPrice.HasValue && FromPrice != 1000000000 && ToPrice == 100000000000)
        //            {
        //                AdvancedFilters += @" AND (OfferPriceType = 0 and CAST(CalculatedOfferPrice AS bigint) >= " + FromPrice.Value.ToString() + @") or
        //               (OfferPriceType = 1 and CAST(OfferPrice AS bigint) >= " + FromPrice + ") ";
        //            }
        //            else
        //           if (FromPrice == 1000000000 && ToPrice.HasValue && ToPrice != 100000000000)
        //            {
        //                AdvancedFilters += @" AND (OfferPriceType = 0 and CAST(CalculatedOfferPrice AS bigint) <= " + ToPrice.Value.ToString() +
        //                    @") or
        //               (OfferPriceType = 1 and CAST(OfferPrice AS bigint) <= " + ToPrice + ") ";
        //            }


        //            if (!String.IsNullOrEmpty(BuildingLifeId))
        //                AdvancedFilters += " AND BuildingLifeId IN (" + BuildingLifeId + ")";


        //            if (FromRebuiltInYear.HasValue && FromRebuiltInYear > 0)
        //                AdvancedFilters += " AND RebuiltInYearFa > " + FromRebuiltInYear;


        //            if (ToRebuiltInYear.HasValue && ToRebuiltInYear > 0)
        //                AdvancedFilters += " AND RebuiltInYearFa < " + ToRebuiltInYear;


        //            if (DocumentTypeId.HasValue && DocumentTypeId > 0)
        //                AdvancedFilters += " AND DocumentTypeId = " + DocumentTypeId;


        //            if (DocumentRootTypeId.HasValue && DocumentRootTypeId > 0)
        //                AdvancedFilters += " AND DocumentRootTypeId = " + DocumentRootTypeId;


        //            if (DocumentOwnershipTypeId.HasValue && DocumentOwnershipTypeId > 0)
        //                AdvancedFilters += " AND DocumentOwnershipTypeId = " + DocumentOwnershipTypeId;


        //            if (DocumentOwnershipTypeId.HasValue && DocumentOwnershipTypeId > 0)
        //                AdvancedFilters += " AND DocumentOwnershipTypeId = " + DocumentOwnershipTypeId;


        //            if (DepositFromPrice.HasValue && DepositFromPrice > 0)
        //                AdvancedFilters += " AND DepositPrice > " + DepositFromPrice;


        //            if (DepositToPrice.HasValue && DepositToPrice != 0 && DepositToPrice != 50000000000)
        //                AdvancedFilters += " AND DepositPrice < " + DepositToPrice;


        //            if (RentFromPrice.HasValue && RentFromPrice > 0)
        //                AdvancedFilters += " AND RentPrice > " + RentFromPrice;


        //            if (RentToPrice.HasValue && RentToPrice != 0 && RentToPrice != 10000000000)
        //                AdvancedFilters += " AND RentPrice < " + RentToPrice;


        //            if (MaritalStatusId.HasValue && MaritalStatusId != 0)
        //                AdvancedFilters += " AND MaritalStatusId = " + MaritalStatusId;



        //            if (!String.IsNullOrEmpty(Convertable) && Convertable != "0")
        //                AdvancedFilters += " AND Convertable = " + Convertable;

        //            //Query for features
        //            if (Features != null && Features.Count > 0)
        //            {
        //                //Join features
        //                joinTeniacoFeatureValues = "inner join TeniacoDbHaghighi.dbo.FeaturesValues on p.PropertyId = TeniacoDbHaghighi.dbo.FeaturesValues.PropertyId";
        //                joinMelkavanFeatureValues = "inner join MelkavanDbHaghighi.dbo.AdvertisementFeaturesValues on ad.AdvertisementId = MelkavanDbHaghighi.dbo.AdvertisementFeaturesValues.AdvertisementId";


        //                foreach (string key in Features.Keys)
        //                {
        //                    AdvancedFilters += @" AND (ad.AdvertisementId in 
        //			                            (select distinct AdvertisementId from MelkavanDbHaghighi.dbo.AdvertisementFeaturesValues where AdvertisementId in 
        //			                            (select AdvertisementId from MelkavanDbHaghighi.dbo.AdvertisementFeaturesValues where FeatureValue IN (" + Features[key] + ") and FeatureId = " + key + ")))";
        //                }
        //            }


        //            strWhereCluaseTeniaco += AdvancedFilters
        //               .Replace("MelkavanDbHaghighi", "TeniacoDbHaghighi")
        //               .Replace("Advertisement.AdvertisementId", "Properties.PropertyId")
        //               .Replace("AdvertisementId", "PropertyId")
        //               .Replace("AdvertisementFeaturesValues", "FeaturesValues")
        //               .Replace("ad.PropertyId", "p.PropertyId");




        //            // To remove first AND in Advanced Filters
        //            int firstAnd = AdvancedFilters.IndexOf("AND");
        //            if (firstAnd != -1)
        //            {
        //                AdvancedFilters = AdvancedFilters.Remove(firstAnd, 3);
        //            }

        //            strWhereCluaseMelkavan += AdvancedFilters;


        //        }



        //        #region AdvertisementTitle

        //        //use advertisementTitle for searching
        //        if (!string.IsNullOrEmpty(advertisementTitle))
        //        {
        //            if (strWhereCluaseMelkavan.Equals("where"))
        //            {
        //                strWhereCluaseMelkavan += " AdvertisementTitle like N'%" + advertisementTitle + "%' ";

        //            }
        //            else
        //            {
        //                strWhereCluaseMelkavan += " and AdvertisementTitle like N'%" + advertisementTitle + "%' ";

        //            }

        //            strWhereCluaseTeniaco += "and PropertyCodeName  like N'%" + advertisementTitle + "'";
        //        }

        //        #endregion


        //        #region UserIdCreator

        //        //var strMelkavanUserIdCreator = "";
        //        //var strTeniacoUserIdCreator = "";


        //        //if (onlyFavorite.HasValue && userId.HasValue)
        //        //{
        //        //    if (onlyFavorite.Value == true && userId.Value > 0)
        //        //    {

        //        //        if (strWhereCluaseMelkavan.Contains("where"))
        //        //        {
        //        //            strMelkavanUserIdCreator += " and MelkavanDbHaghighi.dbo.Advertisement.UserIdCreator =" + userId.Value.ToString();
        //        //        }
        //        //        else
        //        //        {
        //        //            strMelkavanUserIdCreator += " where MelkavanDbHaghighi.dbo.Advertisement.UserIdCreator =" + userId.Value.ToString();
        //        //        }


        //        //        strTeniacoUserIdCreator = "and TeniacoDbHaghighi.dbo.Properties.UserIdCreator =" + userId.Value.ToString();

        //        //    }

        //        //}


        //        //if (!string.IsNullOrEmpty(strMelkavanUserIdCreator))
        //        //{
        //        //    sp = sp.Replace("{melkavanUserIdCreatorWhereCluase}", strMelkavanUserIdCreator);
        //        //}
        //        //else
        //        //{
        //        //    sp = sp.Replace("{melkavanUserIdCreatorWhereCluase}", "");
        //        //}



        //        //if (!string.IsNullOrEmpty(strTeniacoUserIdCreator))
        //        //{
        //        //    sp = sp.Replace("{teniacoUserIdCreatorWhereCluase}", strTeniacoUserIdCreator);
        //        //}
        //        //else
        //        //{
        //        //    sp = sp.Replace("{teniacoUserIdCreatorWhereCluase}", "");
        //        //}

        //        string joinAdvertisementFavorites = "";
        //        string joinPropertiesFavorites = "";

        //        if (onlyFavorite.HasValue && userId.HasValue)
        //        {
        //            if (onlyFavorite.Value == true && userId.Value > 0)
        //            {
        //                joinAdvertisementFavorites = "inner join MelkavanDbHaghighi.dbo.AdvertisementFavorites on ad.AdvertisementId = MelkavanDbHaghighi.dbo.AdvertisementFavorites.AdvertisementId";
        //                joinPropertiesFavorites = "inner join TeniacoDbHaghighi.dbo.PropertiesFavorites on p.PropertyId = TeniacoDbHaghighi.dbo.PropertiesFavorites.PropertyId";

        //                if (strWhereCluaseMelkavan.Equals("where"))
        //                {
        //                    strWhereCluaseMelkavan += " AdvertisementFavorites.AdvertisementId = ad.AdvertisementId AND AdvertisementFavorites.UserIdCreator=" + userId.Value.ToString();
        //                }
        //                else
        //                {
        //                    strWhereCluaseMelkavan += " AND AdvertisementFavorites.AdvertisementId = ad.AdvertisementId AND AdvertisementFavorites.UserIdCreator=" + userId.Value.ToString();
        //                }


        //                strWhereCluaseTeniaco = "AND PropertiesFavorites.PropertyId = p.PropertyId AND PropertiesFavorites.UserIdCreator = " + userId.Value.ToString();

        //            }

        //        }




        //        #endregion




        //        sp = sp.Replace("{JoinMelkavanFeatures}", joinMelkavanFeatureValues);
        //        sp = sp.Replace("{JoinTeniacoFeatures}", joinTeniacoFeatureValues);
        //        sp = sp.Replace("{JoinAdvertisementFavorites}", joinAdvertisementFavorites);
        //        sp = sp.Replace("{JoinPropertiesFavorites}", joinPropertiesFavorites);







        //        if (strWhereCluaseMelkavan.Equals("where"))
        //        {
        //            strWhereCluaseMelkavan = "";
        //        }



        //        if (!string.IsNullOrEmpty(strWhereCluaseMelkavan))
        //        {
        //            sp = sp.Replace("{whereCluaseMelkavan}", strWhereCluaseMelkavan);
        //        }
        //        else
        //        {
        //            sp = sp.Replace("{whereCluaseMelkavan}", "");
        //        }


        //        if (!string.IsNullOrEmpty(strWhereCluaseTeniaco))
        //        {
        //            sp = sp.Replace("{whereCluaseTeniaco}", strWhereCluaseTeniaco);
        //        }
        //        else
        //        {
        //            sp = sp.Replace("{whereCluaseTeniaco}", "");
        //        }


        //        //sp = string.Format(sp, whereClause, whereClause.Replace("AdvertisementTitle", "PropertyCodeName").Replace("AdvertisementDescriptions", "PropertyDescriptions"));

        //        var list1 = melkavanApiDb.AdvertisementAdvanceSearchVM.FromSqlRaw(sp).AsEnumerable().Distinct();


        //        #region ChildUsers
        //        //if (childsUsersIds != null)
        //        //{
        //        //    if (childsUsersIds.Count > 1)
        //        //    {
        //        //        list1 = list1.Where(p => p.UserIdCreator.HasValue).Where(p => childsUsersIds.Contains(p.UserIdCreator.Value));
        //        //    }
        //        //    else
        //        //    {
        //        //        if (childsUsersIds.Count == 1)
        //        //        {
        //        //            if (childsUsersIds.FirstOrDefault() > 0)
        //        //            {
        //        //                list1 = list1.Where(p => p.UserIdCreator.HasValue).Where(p => childsUsersIds.Contains(p.UserIdCreator.Value));
        //        //            }
        //        //        }
        //        //    }
        //        //}
        //        #endregion

        //        #region Pagination and Load extra title data

        //        try
        //        {
        //            if (string.IsNullOrEmpty(jtSorting))
        //            {
        //                listCount = list1.Count();

        //                //if (listCount > jtPageSize)
        //                //{
        //                list1 = list1.OrderByDescending(s => s.CreateEnDate)
        //                         .Skip(jtStartIndex).Take(jtPageSize).ToList();
        //                //}
        //                //else
        //                //{


        //                //    list1 = list1.OrderByDescending(s => s.AdvertisementId).ToList();
        //                //}
        //            }
        //            else
        //            {
        //                listCount = list1.Count();

        //                if (listCount > jtPageSize)
        //                {
        //                    switch (jtSorting)
        //                    {
        //                        case "AdvertisementTitle ASC":
        //                            list1 = list1.OrderBy(l => l.AdvertisementTitle);
        //                            break;
        //                        case "AdvertisementTitle DESC":
        //                            list1 = list1.OrderByDescending(l => l.AdvertisementTitle);
        //                            break;
        //                    }

        //                    if (string.IsNullOrEmpty(jtSorting))
        //                        advertisementAdvanceSearchVMList = list1.OrderByDescending(s => s.AdvertisementId)
        //                                 .Skip(jtStartIndex).Take(jtPageSize).ToList();
        //                    else
        //                        advertisementAdvanceSearchVMList = list1.Skip(jtStartIndex).Take(jtPageSize).ToList();
        //                }
        //                else
        //                {

        //                    advertisementAdvanceSearchVMList = list1.ToList();
        //                }
        //            }



        //            var stateIds = list1.Where(p => p.StateId.HasValue).Select(p => p.StateId.Value).ToList();
        //            var states = publicApiDb.States.Where(s => stateIds.Contains(s.StateId)).ToList();

        //            var cityIds = list1.Where(p => p.CityId.HasValue).Select(p => p.CityId.Value).ToList();
        //            var cities = publicApiDb.Cities.Where(s => cityIds.Contains(s.CityId)).ToList();

        //            var zoneIds = list1.Where(p => p.ZoneId.HasValue).Select(p => p.ZoneId.Value).ToList();
        //            var zones = publicApiDb.Zones.Where(s => zoneIds.Contains(s.ZoneId)).ToList();

        //            var districtIds = list1.Where(p => p.DistrictId.HasValue).Select(p => p.DistrictId.Value).ToList();
        //            var districts = publicApiDb.Districts.Where(s => districtIds.Contains(s.DistrictId)).ToList();

        //            var propertyIds = list1.Select(p => p.AdvertisementId).ToList().Distinct();

        //            var advertisementIds = list1.Select(c => c.AdvertisementId).ToList().Distinct();

        //            var advertisementFilesList = melkavanApiDb.AdvertisementFiles.Where(a => advertisementIds.Contains(a.AdvertisementId) && a.AdvertisementFileType.Equals("media")).ToList();

        //            var propertiesFilesList = teniacoApiDb.PropertyFiles.Where(z => advertisementIds.Contains(z.PropertyId) && z.PropertyFileType.Equals("media")).ToList();

        //            //var tagsIds = list1.Where(p => !p.TagId.IsNullOrEmpty()).Select(p => p.TagId).ToList();
        //            //var tags = melkavanApiDb.Tags.Where(a => tagsIds.Contains(a.TagId)).ToList();


        //            var tmpList = list1.Distinct();

        //            foreach (var item in tmpList)
        //            {

        //                //var tag = tags.Where(s => s.TagId.Equals(item.TagId)).FirstOrDefault();
        //                //if(tag != null)
        //                //{
        //                //    item.TagTitle = tag.TagTitle;
        //                //}


        //                var state = states.Where(s => s.StateId.Equals(item.StateId)).FirstOrDefault();
        //                if (state != null)
        //                {
        //                    item.StateName = state.StateName;
        //                }

        //                var city = cities.Where(c => c.CityId.Equals(item.CityId)).FirstOrDefault();
        //                if (city != null)
        //                {
        //                    item.CityName = city.CityName;
        //                }

        //                if (item.ZoneId.HasValue)
        //                {
        //                    var zone = zones.Where(z => z.ZoneId.Equals(item.ZoneId.Value)).FirstOrDefault();
        //                    if (zone != null)
        //                    {
        //                        item.ZoneName = zone.ZoneName;
        //                    }
        //                }

        //                if (item.DistrictId.HasValue)
        //                {
        //                    var district = districts.Where(z => z.DistrictId.Equals(item.DistrictId.Value)).FirstOrDefault();
        //                    if (district != null)
        //                    {
        //                        item.DistrictName = district.DistrictName;

        //                    }
        //                }

        //                if (userId.HasValue &&
        //                    melkavanApiDb.AdvertisementFavorites.Where(a => a.UserIdCreator == userId && a.AdvertisementId == item.AdvertisementId && a.IsDeleted != true).Any())
        //                {
        //                    item.IsFavorite = true;
        //                }
        //                else
        //                {
        //                    if (userId.HasValue &&
        //                        teniacoApiDb.PropertiesFavorites.Where(a => a.UserIdCreator == userId && a.PropertyId == item.AdvertisementId && a.IsDeleted != true).Any())
        //                    {
        //                        item.IsFavorite = true;
        //                    }
        //                }


        //                if (advertisementFilesList.Where(ad => ad.AdvertisementId == item.AdvertisementId).Any())
        //                {
        //                    try
        //                    {
        //                        item.AdvertisementFilesVM = advertisementFilesList.Where(ad => ad.AdvertisementId == item.AdvertisementId)
        //                            .Select(p => new AdvertisementFilesVM
        //                            {
        //                                AdvertisementId = p.AdvertisementId,
        //                                AdvertisementFilePath = p.AdvertisementFilePath,
        //                                AdvertisementFileExt = p.AdvertisementFileExt,
        //                                AdvertisementFileTitle = p.AdvertisementFileTitle,
        //                                AdvertisementFileId = p.AdvertisementFileId,
        //                                AdvertisementFileType = p.AdvertisementFileType,
        //                            }).OrderByDescending(f => f.AdvertisementFileId).ToList();
        //                    }
        //                    catch (Exception exc)
        //                    { }
        //                }
        //                else
        //                {
        //                    if (propertiesFilesList.Where(ad => ad.PropertyId == item.AdvertisementId).Any())
        //                    {
        //                        try
        //                        {
        //                            item.AdvertisementFilesVM = propertiesFilesList.Where(ad => ad.PropertyId == item.AdvertisementId)
        //                                .Select(p => new AdvertisementFilesVM
        //                                {
        //                                    AdvertisementId = p.PropertyId,
        //                                    AdvertisementFilePath = p.PropertyFilePath,
        //                                    AdvertisementFileExt = p.PropertyFileExt,
        //                                    AdvertisementFileTitle = p.PropertyFileTitle,
        //                                    AdvertisementFileId = p.PropertyFileId,
        //                                    AdvertisementFileType = p.PropertyFileType,
        //                                }).OrderByDescending(f => f.AdvertisementFileId).ToList();
        //                        }
        //                        catch (Exception exc)
        //                        { }
        //                    }
        //                }
        //            }



        //            advertisementAdvanceSearchVMList = tmpList.Distinct().ToList();

        //        }
        //        catch (Exception exc)
        //        { }

        //        #endregion


        //    }
        //    catch (Exception exc)
        //    { }



        //    return advertisementAdvanceSearchVMList;
        //}

        #endregion



        #region Ghaliany's Code

        //public List<AdvertisementAdvanceSearchVM> GetListOfAdverisementsAdvanceSearch(
        //          int jtStartIndex,
        //          int jtPageSize,
        //          ref int listCount,
        //          List<long> childsUsersIds,
        //          PublicApiContext publicApiDb,
        //          TeniacoApiContext teniacoApiDb,
        //          string jtSorting = null,
        //          string advertisementTitle = null,
        //          bool? onlyFavorite = null,
        //          long? userId = null, bool IsFiltered = false,
        //          //parameters for advanced filters
        //          int? AdvertisementTypeId = null,
        //          int? PropertyTypeId = null,
        //          string? TypeOfUseId = null,
        //          long? StateId = null,
        //          long? CityId = null,
        //          long? ZoneId = null,
        //          string? TownName = null,
        //          string? FromArea = null,
        //          string? ToArea = null,
        //          int? FromFoundation = null,
        //          int? ToFoundation = null,
        //          long? FromPrice = 0,
        //          long? ToPrice = 0,
        //          string? BuildingLifeId = null,
        //          int? FromRebuiltInYear = null,
        //          int? ToRebuiltInYear = null,
        //          int? DocumentTypeId = null,
        //          int? DocumentRootTypeId = null,
        //          int? DocumentOwnershipTypeId = null,
        //          long? DepositFromPrice = 0,
        //          long? DepositToPrice = 0,
        //          long? RentFromPrice = 0,
        //          long? RentToPrice = 0,
        //          int? MaritalStatusId = null,
        //          string? Convertable = null,
        //          Dictionary<string, string>? Features = null)
        //{

        //    List<AdvertisementAdvanceSearchVM> advertisementAdvanceSearchVMList = new List<AdvertisementAdvanceSearchVM>();

        //    try
        //    {
        //        string sp = @"";


        //        sp = @"
        //                  SELECT DISTINCT * FROM (
        //                            SELECT 'Advertisement' AS RecordType,
        //                                    ad.StatusId,
        //                                    ad.RejectionReason,
        //                                    details.InstagramLink,
        //                                    details.AdvertisementTypeId,
        //                                    ad.AdvertisementId,
        //                                    '' AS PropertyTypeTitle,
        //                                    '' AS TypeUseTitle,
        //                                    '' AS DocumentTypeTitle,
        //                                    '' AS DocumentOwnershipTypeTitle,
        //                                    '' AS DocumentRootTypeTitle,
        //                                    '' AS StateName,
        //                                    '' AS CityName,
        //                                    '' AS ZoneName,
        //                                    '' AS DistrictName,
        //                                    '' AS UserCreatorName,
        //                                    ad.IsActivated,
        //                                    ad.IsDeleted,
        //                                    ad.ConsultantUserId,
        //                                    ad.EditEnDate,
        //                                    ad.RebuiltInYearFa,
        //                                    ad.CreateEnDate,
        //                                    ad.PropertyTypeId,
        //                                    ad.TypeOfUseId,
        //                                    ad.DocumentOwnershipTypeId,
        //                                    ad.DocumentTypeId,
        //                                    ad.DocumentRootTypeId,
        //                                    ad.AdvertisementTitle,
        //                                    ad.Area,
        //                                    ad.AdvertisementDescriptions,
        //                                    ad.UserIdCreator,
        //                                    '1' AS ShowInMelkavan,
        //                                    addr.CountryId,
        //                                    addr.StateId,
        //                                    addr.CityId,
        //                                    addr.ZoneId,
        //                                    addr.DistrictId,
        //                                    addr.TownName,
        //                                    priceHist.OfferPrice,
        //                                    priceHist.DepositPrice,
        //                                    priceHist.RentPrice,
        //                                    '' AS CurrentDate,
        //                                    details.TagId,
        //                                    CAST(CASE WHEN fav.AdvertisementId IS NOT NULL THEN 1 ELSE 0 END AS BIT) AS IsFavorite,
        //                                    ISNULL(viewers.ViewersCount, 0) AS ViewersCount
        //                                    FROM MelkavanDbGhaliany.dbo.Advertisement AS ad
        //                                    INNER JOIN MelkavanDbGhaliany.dbo.AdvertisementAddress AS addr ON ad.AdvertisementId = addr.AdvertisementId
        //                                    INNER JOIN (
        //                                        SELECT AdvertisementId, OfferPrice, DepositPrice, RentPrice
        //                                        FROM MelkavanDbGhaliany.dbo.AdvertisementPricesHistories AS aph
        //                                        WHERE aph.AdvertisementPriceHistoryId = (
        //                                            SELECT MAX(AdvertisementPriceHistoryId)
        //                                            FROM MelkavanDbGhaliany.dbo.AdvertisementPricesHistories
        //                                            WHERE AdvertisementId = aph.AdvertisementId
        //                                        )
        //                                    ) AS priceHist ON ad.AdvertisementId = priceHist.AdvertisementId
        //                                    INNER JOIN MelkavanDbGhaliany.dbo.AdvertisementDetails AS details ON ad.AdvertisementId = details.AdvertisementId
        //                                    LEFT JOIN MelkavanDbGhaliany.dbo.AdvertisementFavorites AS fav ON ad.AdvertisementId = fav.AdvertisementId
        //                                    LEFT JOIN (
        //                                        SELECT AdvertisementId, COUNT(*) AS ViewersCount
        //                                        FROM MelkavanDbGhaliany.dbo.AdvertisementViewers
        //                                        GROUP BY AdvertisementId
        //                                    ) AS viewers ON ad.AdvertisementId = viewers.AdvertisementId
        //                                    {JoinMelkavanFeatures} 
        //                                    {JoinAdvertisementFavorites}
        //                                    {whereCluaseMelkavan}
        //                                    UNION ALL
        //                                    SELECT 'Properties' AS RecordType,
        //                                    p.StatusId,
        //                                    p.RejectionReason,
        //                                    '' as InstagramLink,
        //                                    pd.AdvertisementTypeId,
        //                                    p.PropertyId AS AdvertisementId,
        //                                    '' AS PropertyTypeTitle,
        //                                    '' AS TypeUseTitle,
        //                                    '' AS DocumentTypeTitle,
        //                                    '' AS DocumentOwnershipTypeTitle,
        //                                    '' AS DocumentRootTypeTitle,
        //                                    '' AS StateName,
        //                                    '' AS CityName,
        //                                    '' AS ZoneName,
        //                                    '' AS DistrictName,
        //                                    '' AS UserCreatorName,
        //                                    p.IsActivated,
        //                                    p.IsDeleted,
        //                                    p.ConsultantUserId,
        //                                    p.EditEnDate,
        //                                    p.RebuiltInYearFa,
        //                                    p.CreateEnDate,
        //                                    p.PropertyTypeId,
        //                                    p.TypeOfUseId,
        //                                    p.DocumentOwnershipTypeId,
        //                                    p.DocumentTypeId,
        //                                    p.DocumentRootTypeId,
        //                                    p.PropertyCodeName AS AdvertisementTitle,
        //                                    p.Area,
        //                                    p.PropertyDescriptions AS AdvertisementDescriptions,
        //                                    p.UserIdCreator,
        //                                    p.ShowInMelkavan,
        //                                    propAddr.CountryId,
        //                                    propAddr.StateId,
        //                                    propAddr.CityId,
        //                                    propAddr.ZoneId,
        //                                    propAddr.DistrictId,
        //                                    propAddr.TownName,
        //                                    propPriceHist.OfferPrice,
        //                                    propPriceHist.DepositPrice,
        //                                    propPriceHist.RentPrice,
        //                                    '' AS CurrentDate,
        //                                    pd.TagId,
        //                                    CAST(CASE WHEN fav.PropertyId IS NOT NULL THEN 1 ELSE 0 END AS BIT) AS IsFavorite,
        //                                    ISNULL(viewers.ViewersCount, 0) AS ViewersCount
        //                                    FROM TeniacoDbGhaliany.dbo.Properties AS p
        //                                    INNER JOIN TeniacoDbGhaliany.dbo.PropertyAddress AS propAddr ON p.PropertyId = propAddr.PropertyId
        //                                    INNER JOIN (
        //                                        SELECT PropertyId, CASE OfferPriceType WHEN '0' THEN CalculatedOfferPrice ELSE OfferPrice END as OfferPrice, DepositPrice, RentPrice
        //                                        FROM TeniacoDbGhaliany.dbo.PropertiesPricesHistories AS pph
        //                                        WHERE pph.PropertyPriceHistoryId = (
        //                                            SELECT MAX(PropertyPriceHistoryId)
        //                                            FROM TeniacoDbGhaliany.dbo.PropertiesPricesHistories
        //                                            WHERE PropertyId = pph.PropertyId
        //                                        )
        //                                    ) AS propPriceHist ON p.PropertyId = propPriceHist.PropertyId
        //                                    INNER JOIN TeniacoDbGhaliany.dbo.PropertiesDetails AS pd ON p.PropertyId = pd.PropertyId
        //                                    LEFT JOIN TeniacoDbGhaliany.dbo.PropertiesFavorites AS fav ON p.PropertyId = fav.PropertyId
        //                                    LEFT JOIN (
        //                                        SELECT PropertyId, COUNT(*) AS ViewersCount
        //                                        FROM TeniacoDbGhaliany.dbo.PropertiesViewers
        //                                        GROUP BY PropertyId
        //                                    ) AS viewers ON p.PropertyId = viewers.PropertyId

        //                                    {JoinTeniacoFeatures}
        //                                    {JoinPropertiesFavorites}
        //                                    WHERE p.ShowInMelkavan = 1 {whereCluaseTeniaco}
        //                                    ) AS tmp
        //                                    ORDER BY CreateEnDate DESC;  ";


        //        string joinTeniacoFeatureValues = "";
        //        string joinMelkavanFeatureValues = "";
        //        string AdvancedFilters = "";

        //        string strWhereCluaseMelkavan = "where";
        //        string strWhereCluaseTeniaco = "";

        //        if (IsFiltered == true)
        //        {



        //            if (AdvertisementTypeId.HasValue && AdvertisementTypeId > 0)
        //                AdvancedFilters += " AND AdvertisementTypeId = " + AdvertisementTypeId;


        //            if (PropertyTypeId.HasValue && PropertyTypeId > 0)
        //                AdvancedFilters += " AND PropertyTypeId = " + PropertyTypeId;


        //            if (!String.IsNullOrEmpty(TypeOfUseId))
        //                AdvancedFilters += " AND TypeOfUseId IN (" + TypeOfUseId + ")";


        //            if (StateId.HasValue && StateId > 0)
        //                AdvancedFilters += " AND StateId = " + StateId;


        //            if (CityId.HasValue && CityId > 0)
        //                AdvancedFilters += " AND CityId = " + CityId;


        //            if (ZoneId.HasValue && ZoneId > 0)
        //                AdvancedFilters += " AND ZoneId = " + ZoneId;


        //            if (!String.IsNullOrEmpty(TownName))
        //                AdvancedFilters += " AND TownName like N'%" + TownName + "%' ";


        //            if (!String.IsNullOrEmpty(FromArea) && FromArea != "0")
        //                AdvancedFilters += " AND CAST(Area AS float) > " + FromArea;


        //            if (!String.IsNullOrEmpty(ToArea) && ToArea != "0")
        //                AdvancedFilters += " AND CAST(Area AS float) < " + ToArea;


        //            if (FromFoundation.HasValue && FromFoundation > 0)
        //                AdvancedFilters += " AND Foundation > " + FromFoundation;


        //            if (ToFoundation.HasValue && ToFoundation > 0)
        //                AdvancedFilters += " AND Foundation < " + ToFoundation;


        //            //if (FromPrice.HasValue && FromPrice>0)
        //            //    AdvancedFilters += " AND OfferPrice > " + FromPrice;


        //            //if (ToPrice.HasValue && ToPrice!=0 && ToPrice!=100000000000)
        //            //  AdvancedFilters += " AND OfferPrice < " + ToPrice;


        //            if (FromPrice.HasValue && FromPrice != 1000000000 && ToPrice.HasValue && ToPrice != 100000000000)
        //                AdvancedFilters += @" AND ((OfferPriceType = 0 and CAST(CalculatedOfferPrice AS bigint) >= " + FromPrice.Value.ToString() +
        //                                    @") and (OfferPriceType = 0 and CAST(CalculatedOfferPrice AS bigint)  <= " + ToPrice.Value.ToString() + @")) or 
        //                           ((OfferPriceType = 1 and CAST(OfferPrice AS bigint) >= " + FromPrice + ") and (OfferPriceType = 1 and CAST(OfferPrice AS bigint) <= " + ToPrice.Value.ToString() + ")) ";
        //            else
        //           if (FromPrice.HasValue && FromPrice != 1000000000 && ToPrice == 100000000000)
        //            {
        //                AdvancedFilters += @" AND (OfferPriceType = 0 and CAST(CalculatedOfferPrice AS bigint) >= " + FromPrice.Value.ToString() + @") or
        //               (OfferPriceType = 1 and CAST(OfferPrice AS bigint) >= " + FromPrice + ") ";
        //            }
        //            else
        //           if (FromPrice == 1000000000 && ToPrice.HasValue && ToPrice != 100000000000)
        //            {
        //                AdvancedFilters += @" AND (OfferPriceType = 0 and CAST(CalculatedOfferPrice AS bigint) <= " + ToPrice.Value.ToString() +
        //                    @") or
        //               (OfferPriceType = 1 and CAST(OfferPrice AS bigint) <= " + ToPrice + ") ";
        //            }


        //            if (!String.IsNullOrEmpty(BuildingLifeId))
        //                AdvancedFilters += " AND BuildingLifeId IN (" + BuildingLifeId + ")";


        //            if (FromRebuiltInYear.HasValue && FromRebuiltInYear > 0)
        //                AdvancedFilters += " AND RebuiltInYearFa > " + FromRebuiltInYear;


        //            if (ToRebuiltInYear.HasValue && ToRebuiltInYear > 0)
        //                AdvancedFilters += " AND RebuiltInYearFa < " + ToRebuiltInYear;


        //            if (DocumentTypeId.HasValue && DocumentTypeId > 0)
        //                AdvancedFilters += " AND DocumentTypeId = " + DocumentTypeId;


        //            if (DocumentRootTypeId.HasValue && DocumentRootTypeId > 0)
        //                AdvancedFilters += " AND DocumentRootTypeId = " + DocumentRootTypeId;


        //            if (DocumentOwnershipTypeId.HasValue && DocumentOwnershipTypeId > 0)
        //                AdvancedFilters += " AND DocumentOwnershipTypeId = " + DocumentOwnershipTypeId;


        //            if (DocumentOwnershipTypeId.HasValue && DocumentOwnershipTypeId > 0)
        //                AdvancedFilters += " AND DocumentOwnershipTypeId = " + DocumentOwnershipTypeId;


        //            if (DepositFromPrice.HasValue && DepositFromPrice > 0)
        //                AdvancedFilters += " AND DepositPrice > " + DepositFromPrice;


        //            if (DepositToPrice.HasValue && DepositToPrice != 0 && DepositToPrice != 50000000000)
        //                AdvancedFilters += " AND DepositPrice < " + DepositToPrice;


        //            if (RentFromPrice.HasValue && RentFromPrice > 0)
        //                AdvancedFilters += " AND RentPrice > " + RentFromPrice;


        //            if (RentToPrice.HasValue && RentToPrice != 0 && RentToPrice != 10000000000)
        //                AdvancedFilters += " AND RentPrice < " + RentToPrice;


        //            if (MaritalStatusId.HasValue && MaritalStatusId != 0)
        //                AdvancedFilters += " AND MaritalStatusId = " + MaritalStatusId;



        //            if (!String.IsNullOrEmpty(Convertable) && Convertable != "0")
        //                AdvancedFilters += " AND Convertable = " + Convertable;

        //            //Query for features
        //            if (Features != null && Features.Count > 0)
        //            {
        //                //Join features
        //                joinTeniacoFeatureValues = "inner join TeniacoDbGhaliany.dbo.FeaturesValues on p.PropertyId = TeniacoDbGhaliany.dbo.FeaturesValues.PropertyId";
        //                joinMelkavanFeatureValues = "inner join MelkavanDbGhaliany.dbo.AdvertisementFeaturesValues on ad.AdvertisementId = MelkavanDbGhaliany.dbo.AdvertisementFeaturesValues.AdvertisementId";


        //                foreach (string key in Features.Keys)
        //                {
        //                    AdvancedFilters += @" AND (ad.AdvertisementId in 
        //			                            (select distinct AdvertisementId from MelkavanDbGhaliany.dbo.AdvertisementFeaturesValues where AdvertisementId in 
        //			                            (select AdvertisementId from MelkavanDbGhaliany.dbo.AdvertisementFeaturesValues where FeatureValue IN (" + Features[key] + ") and FeatureId = " + key + ")))";
        //                }
        //            }


        //            strWhereCluaseTeniaco += AdvancedFilters
        //               .Replace("MelkavanDbGhaliany", "TeniacoDbGhaliany")
        //               .Replace("Advertisement.AdvertisementId", "Properties.PropertyId")
        //               .Replace("AdvertisementId", "PropertyId")
        //               .Replace("AdvertisementFeaturesValues", "FeaturesValues")
        //               .Replace("ad.PropertyId", "p.PropertyId");




        //            // To remove first AND in Advanced Filters
        //            int firstAnd = AdvancedFilters.IndexOf("AND");
        //            if (firstAnd != -1)
        //            {
        //                AdvancedFilters = AdvancedFilters.Remove(firstAnd, 3);
        //            }

        //            strWhereCluaseMelkavan += AdvancedFilters;


        //        }



        //        #region AdvertisementTitle

        //        //use advertisementTitle for searching
        //        if (!string.IsNullOrEmpty(advertisementTitle))
        //        {
        //            if (strWhereCluaseMelkavan.Equals("where"))
        //            {
        //                strWhereCluaseMelkavan += " AdvertisementTitle like N'%" + advertisementTitle + "%' ";

        //            }
        //            else
        //            {
        //                strWhereCluaseMelkavan += " and AdvertisementTitle like N'%" + advertisementTitle + "%' ";

        //            }

        //            strWhereCluaseTeniaco += "and PropertyCodeName  like N'%" + advertisementTitle + "'";
        //        }

        //        #endregion


        //        #region UserIdCreator

        //        //var strMelkavanUserIdCreator = "";
        //        //var strTeniacoUserIdCreator = "";


        //        //if (onlyFavorite.HasValue && userId.HasValue)
        //        //{
        //        //    if (onlyFavorite.Value == true && userId.Value > 0)
        //        //    {

        //        //        if (strWhereCluaseMelkavan.Contains("where"))
        //        //        {
        //        //            strMelkavanUserIdCreator += " and MelkavanDbGhaliany.dbo.Advertisement.UserIdCreator =" + userId.Value.ToString();
        //        //        }
        //        //        else
        //        //        {
        //        //            strMelkavanUserIdCreator += " where MelkavanDbGhaliany.dbo.Advertisement.UserIdCreator =" + userId.Value.ToString();
        //        //        }


        //        //        strTeniacoUserIdCreator = "and TeniacoDbGhaliany.dbo.Properties.UserIdCreator =" + userId.Value.ToString();

        //        //    }

        //        //}


        //        //if (!string.IsNullOrEmpty(strMelkavanUserIdCreator))
        //        //{
        //        //    sp = sp.Replace("{melkavanUserIdCreatorWhereCluase}", strMelkavanUserIdCreator);
        //        //}
        //        //else
        //        //{
        //        //    sp = sp.Replace("{melkavanUserIdCreatorWhereCluase}", "");
        //        //}



        //        //if (!string.IsNullOrEmpty(strTeniacoUserIdCreator))
        //        //{
        //        //    sp = sp.Replace("{teniacoUserIdCreatorWhereCluase}", strTeniacoUserIdCreator);
        //        //}
        //        //else
        //        //{
        //        //    sp = sp.Replace("{teniacoUserIdCreatorWhereCluase}", "");
        //        //}

        //        string joinAdvertisementFavorites = "";
        //        string joinPropertiesFavorites = "";

        //        if (onlyFavorite.HasValue && userId.HasValue)
        //        {
        //            if (onlyFavorite.Value == true && userId.Value > 0)
        //            {
        //                joinAdvertisementFavorites = "inner join MelkavanDbGhaliany.dbo.AdvertisementFavorites on ad.AdvertisementId = MelkavanDbGhaliany.dbo.AdvertisementFavorites.AdvertisementId";
        //                joinPropertiesFavorites = "inner join TeniacoDbGhaliany.dbo.PropertiesFavorites on p.PropertyId = TeniacoDbGhaliany.dbo.PropertiesFavorites.PropertyId";

        //                if (strWhereCluaseMelkavan.Equals("where"))
        //                {
        //                    strWhereCluaseMelkavan += " AdvertisementFavorites.AdvertisementId = ad.AdvertisementId AND AdvertisementFavorites.UserIdCreator=" + userId.Value.ToString();
        //                }
        //                else
        //                {
        //                    strWhereCluaseMelkavan += " AND AdvertisementFavorites.AdvertisementId = ad.AdvertisementId AND AdvertisementFavorites.UserIdCreator=" + userId.Value.ToString();
        //                }


        //                strWhereCluaseTeniaco = "AND PropertiesFavorites.PropertyId = p.PropertyId AND PropertiesFavorites.UserIdCreator = " + userId.Value.ToString();

        //            }

        //        }




        //        #endregion




        //        sp = sp.Replace("{JoinMelkavanFeatures}", joinMelkavanFeatureValues);
        //        sp = sp.Replace("{JoinTeniacoFeatures}", joinTeniacoFeatureValues);
        //        sp = sp.Replace("{JoinAdvertisementFavorites}", joinAdvertisementFavorites);
        //        sp = sp.Replace("{JoinPropertiesFavorites}", joinPropertiesFavorites);







        //        if (strWhereCluaseMelkavan.Equals("where"))
        //        {
        //            strWhereCluaseMelkavan = "";
        //        }



        //        if (!string.IsNullOrEmpty(strWhereCluaseMelkavan))
        //        {
        //            sp = sp.Replace("{whereCluaseMelkavan}", strWhereCluaseMelkavan);
        //        }
        //        else
        //        {
        //            sp = sp.Replace("{whereCluaseMelkavan}", "");
        //        }


        //        if (!string.IsNullOrEmpty(strWhereCluaseTeniaco))
        //        {
        //            sp = sp.Replace("{whereCluaseTeniaco}", strWhereCluaseTeniaco);
        //        }
        //        else
        //        {
        //            sp = sp.Replace("{whereCluaseTeniaco}", "");
        //        }


        //        //sp = string.Format(sp, whereClause, whereClause.Replace("AdvertisementTitle", "PropertyCodeName").Replace("AdvertisementDescriptions", "PropertyDescriptions"));

        //        var list1 = melkavanApiDb.AdvertisementAdvanceSearchVM.FromSqlRaw(sp).AsEnumerable().Distinct();


        //        #region ChildUsers
        //        //if (childsUsersIds != null)
        //        //{
        //        //    if (childsUsersIds.Count > 1)
        //        //    {
        //        //        list1 = list1.Where(p => p.UserIdCreator.HasValue).Where(p => childsUsersIds.Contains(p.UserIdCreator.Value));
        //        //    }
        //        //    else
        //        //    {
        //        //        if (childsUsersIds.Count == 1)
        //        //        {
        //        //            if (childsUsersIds.FirstOrDefault() > 0)
        //        //            {
        //        //                list1 = list1.Where(p => p.UserIdCreator.HasValue).Where(p => childsUsersIds.Contains(p.UserIdCreator.Value));
        //        //            }
        //        //        }
        //        //    }
        //        //}
        //        #endregion

        //        #region Pagination and Load extra title data

        //        try
        //        {
        //            if (string.IsNullOrEmpty(jtSorting))
        //            {
        //                listCount = list1.Count();

        //                //if (listCount > jtPageSize)
        //                //{
        //                list1 = list1.OrderByDescending(s => s.CreateEnDate)
        //                         .Skip(jtStartIndex).Take(jtPageSize).ToList();
        //                //}
        //                //else
        //                //{


        //                //    list1 = list1.OrderByDescending(s => s.AdvertisementId).ToList();
        //                //}
        //            }
        //            else
        //            {
        //                listCount = list1.Count();

        //                if (listCount > jtPageSize)
        //                {
        //                    switch (jtSorting)
        //                    {
        //                        case "AdvertisementTitle ASC":
        //                            list1 = list1.OrderBy(l => l.AdvertisementTitle);
        //                            break;
        //                        case "AdvertisementTitle DESC":
        //                            list1 = list1.OrderByDescending(l => l.AdvertisementTitle);
        //                            break;
        //                    }

        //                    if (string.IsNullOrEmpty(jtSorting))
        //                        advertisementAdvanceSearchVMList = list1.OrderByDescending(s => s.AdvertisementId)
        //                                 .Skip(jtStartIndex).Take(jtPageSize).ToList();
        //                    else
        //                        advertisementAdvanceSearchVMList = list1.Skip(jtStartIndex).Take(jtPageSize).ToList();
        //                }
        //                else
        //                {

        //                    advertisementAdvanceSearchVMList = list1.ToList();
        //                }
        //            }



        //            var stateIds = list1.Where(p => p.StateId.HasValue).Select(p => p.StateId.Value).ToList();
        //            var states = publicApiDb.States.Where(s => stateIds.Contains(s.StateId)).ToList();

        //            var cityIds = list1.Where(p => p.CityId.HasValue).Select(p => p.CityId.Value).ToList();
        //            var cities = publicApiDb.Cities.Where(s => cityIds.Contains(s.CityId)).ToList();

        //            var zoneIds = list1.Where(p => p.ZoneId.HasValue).Select(p => p.ZoneId.Value).ToList();
        //            var zones = publicApiDb.Zones.Where(s => zoneIds.Contains(s.ZoneId)).ToList();

        //            var districtIds = list1.Where(p => p.DistrictId.HasValue).Select(p => p.DistrictId.Value).ToList();
        //            var districts = publicApiDb.Districts.Where(s => districtIds.Contains(s.DistrictId)).ToList();

        //            var propertyIds = list1.Select(p => p.AdvertisementId).ToList().Distinct();

        //            var advertisementIds = list1.Select(c => c.AdvertisementId).ToList().Distinct();

        //            var advertisementFilesList = melkavanApiDb.AdvertisementFiles.Where(a => advertisementIds.Contains(a.AdvertisementId) && a.AdvertisementFileType.Equals("media")).ToList();

        //            var propertiesFilesList = teniacoApiDb.PropertyFiles.Where(z => advertisementIds.Contains(z.PropertyId) && z.PropertyFileType.Equals("media")).ToList();

        //            //var tagsIds = list1.Where(p => !p.TagId.IsNullOrEmpty()).Select(p => p.TagId).ToList();
        //            //var tags = melkavanApiDb.Tags.Where(a => tagsIds.Contains(a.TagId)).ToList();


        //            var tmpList = list1.Distinct();

        //            foreach (var item in tmpList)
        //            {

        //                //var tag = tags.Where(s => s.TagId.Equals(item.TagId)).FirstOrDefault();
        //                //if(tag != null)
        //                //{
        //                //    item.TagTitle = tag.TagTitle;
        //                //}


        //                var state = states.Where(s => s.StateId.Equals(item.StateId)).FirstOrDefault();
        //                if (state != null)
        //                {
        //                    item.StateName = state.StateName;
        //                }

        //                var city = cities.Where(c => c.CityId.Equals(item.CityId)).FirstOrDefault();
        //                if (city != null)
        //                {
        //                    item.CityName = city.CityName;
        //                }

        //                if (item.ZoneId.HasValue)
        //                {
        //                    var zone = zones.Where(z => z.ZoneId.Equals(item.ZoneId.Value)).FirstOrDefault();
        //                    if (zone != null)
        //                    {
        //                        item.ZoneName = zone.ZoneName;
        //                    }
        //                }

        //                if (item.DistrictId.HasValue)
        //                {
        //                    var district = districts.Where(z => z.DistrictId.Equals(item.DistrictId.Value)).FirstOrDefault();
        //                    if (district != null)
        //                    {
        //                        item.DistrictName = district.DistrictName;

        //                    }
        //                }

        //                if (userId.HasValue &&
        //                    melkavanApiDb.AdvertisementFavorites.Where(a => a.UserIdCreator == userId && a.AdvertisementId == item.AdvertisementId && a.IsDeleted != true).Any())
        //                {
        //                    item.IsFavorite = true;
        //                }
        //                else
        //                {
        //                    if (userId.HasValue &&
        //                        teniacoApiDb.PropertiesFavorites.Where(a => a.UserIdCreator == userId && a.PropertyId == item.AdvertisementId && a.IsDeleted != true).Any())
        //                    {
        //                        item.IsFavorite = true;
        //                    }
        //                }


        //                if (advertisementFilesList.Where(ad => ad.AdvertisementId == item.AdvertisementId).Any())
        //                {
        //                    try
        //                    {
        //                        item.AdvertisementFilesVM = advertisementFilesList.Where(ad => ad.AdvertisementId == item.AdvertisementId)
        //                            .Select(p => new AdvertisementFilesVM
        //                            {
        //                                AdvertisementId = p.AdvertisementId,
        //                                AdvertisementFilePath = p.AdvertisementFilePath,
        //                                AdvertisementFileExt = p.AdvertisementFileExt,
        //                                AdvertisementFileTitle = p.AdvertisementFileTitle,
        //                                AdvertisementFileId = p.AdvertisementFileId,
        //                                AdvertisementFileType = p.AdvertisementFileType,
        //                            }).OrderByDescending(f => f.AdvertisementFileId).ToList();
        //                    }
        //                    catch (Exception exc)
        //                    { }
        //                }
        //                else
        //                {
        //                    if (propertiesFilesList.Where(ad => ad.PropertyId == item.AdvertisementId).Any())
        //                    {
        //                        try
        //                        {
        //                            item.AdvertisementFilesVM = propertiesFilesList.Where(ad => ad.PropertyId == item.AdvertisementId)
        //                                .Select(p => new AdvertisementFilesVM
        //                                {
        //                                    AdvertisementId = p.PropertyId,
        //                                    AdvertisementFilePath = p.PropertyFilePath,
        //                                    AdvertisementFileExt = p.PropertyFileExt,
        //                                    AdvertisementFileTitle = p.PropertyFileTitle,
        //                                    AdvertisementFileId = p.PropertyFileId,
        //                                    AdvertisementFileType = p.PropertyFileType,
        //                                }).OrderByDescending(f => f.AdvertisementFileId).ToList();
        //                        }
        //                        catch (Exception exc)
        //                        { }
        //                    }
        //                }
        //            }



        //            advertisementAdvanceSearchVMList = tmpList.Distinct().ToList();

        //        }
        //        catch (Exception exc)
        //        { }

        //        #endregion


        //    }
        //    catch (Exception exc)
        //    { }



        //    return advertisementAdvanceSearchVMList;
        //}
        #endregion



        public List<AdvertisementAdvanceSearchVM> GetListOfAdverisementsAdvanceSearch(
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
                  long? FromPrice = 0,
                  long? ToPrice = 0,
                  string? BuildingLifeId = null,
                  int? FromRebuiltInYear = null,
                  int? ToRebuiltInYear = null,
                  int? DocumentTypeId = null,
                  int? DocumentRootTypeId = null,
                  int? DocumentOwnershipTypeId = null,
                  long? DepositFromPrice = 0,
                  long? DepositToPrice = 0,
                  long? RentFromPrice = 0,
                  long? RentToPrice = 0,
                  int? MaritalStatusId = null,
                  string? Convertable = null,
                  Dictionary<string, string>? Features = null)
        {

            List<AdvertisementAdvanceSearchVM> advertisementAdvanceSearchVMList = new List<AdvertisementAdvanceSearchVM>();

            try
            {
                string sp = @"";


                sp = @"
                          SELECT DISTINCT * FROM (
                                    SELECT 'Advertisement' AS RecordType,
                                            ad.StatusId,
                                            ad.RejectionReason,
                                            details.InstagramLink,
                                            details.AdvertisementTypeId,
                                            ad.AdvertisementId,
                                            '' AS PropertyTypeTitle,
                                            '' AS TypeUseTitle,
                                            '' AS DocumentTypeTitle,
                                            '' AS DocumentOwnershipTypeTitle,
                                            '' AS DocumentRootTypeTitle,
                                            '' AS StateName,
                                            '' AS CityName,
                                            '' AS ZoneName,
                                            '' AS DistrictName,
                                            '' AS UserCreatorName,
                                            ad.IsActivated,
                                            ad.IsDeleted,
                                            ad.ConsultantUserId,
                                            ad.EditEnDate,
                                            ad.RebuiltInYearFa,
                                            ad.CreateEnDate,
                                            ad.PropertyTypeId,
                                            ad.TypeOfUseId,
                                            ad.DocumentOwnershipTypeId,
                                            ad.DocumentTypeId,
                                            ad.DocumentRootTypeId,
                                            ad.AdvertisementTitle,
                                            ad.Area,
                                            ad.AdvertisementDescriptions,
                                            ad.UserIdCreator,
                                            '1' AS ShowInMelkavan,
                                            addr.CountryId,
                                            addr.StateId,
                                            addr.CityId,
                                            addr.ZoneId,
                                            addr.DistrictId,
                                            addr.TownName,
                                            priceHist.OfferPrice,
                                            priceHist.DepositPrice,
                                            priceHist.RentPrice,
                                            '' AS CurrentDate,
                                            details.TagId,
                                            CAST(CASE WHEN fav.AdvertisementId IS NOT NULL THEN 1 ELSE 0 END AS BIT) AS IsFavorite,
                                            ISNULL(viewers.ViewersCount, 0) AS ViewersCount
                                            FROM MelkavanDb.dbo.Advertisement AS ad
                                            INNER JOIN MelkavanDb.dbo.AdvertisementAddress AS addr ON ad.AdvertisementId = addr.AdvertisementId
                                            INNER JOIN (
                                                SELECT AdvertisementId, OfferPrice, DepositPrice, RentPrice
                                                FROM MelkavanDb.dbo.AdvertisementPricesHistories AS aph
                                                WHERE aph.AdvertisementPriceHistoryId = (
                                                    SELECT MAX(AdvertisementPriceHistoryId)
                                                    FROM MelkavanDb.dbo.AdvertisementPricesHistories
                                                    WHERE AdvertisementId = aph.AdvertisementId
                                                )
                                            ) AS priceHist ON ad.AdvertisementId = priceHist.AdvertisementId
                                            INNER JOIN MelkavanDb.dbo.AdvertisementDetails AS details ON ad.AdvertisementId = details.AdvertisementId
                                            LEFT JOIN MelkavanDb.dbo.AdvertisementFavorites AS fav ON ad.AdvertisementId = fav.AdvertisementId
                                            LEFT JOIN (
                                                SELECT AdvertisementId, COUNT(*) AS ViewersCount
                                                FROM MelkavanDb.dbo.AdvertisementViewers
                                                GROUP BY AdvertisementId
                                            ) AS viewers ON ad.AdvertisementId = viewers.AdvertisementId
                                            {JoinMelkavanFeatures} 
                                            {JoinAdvertisementFavorites}
                                            {whereCluaseMelkavan}
                                            UNION ALL
                                            SELECT 'Properties' AS RecordType,
                                            p.StatusId,
                                            p.RejectionReason,
                                            '' as InstagramLink,
                                            pd.AdvertisementTypeId,
                                            p.PropertyId AS AdvertisementId,
                                            '' AS PropertyTypeTitle,
                                            '' AS TypeUseTitle,
                                            '' AS DocumentTypeTitle,
                                            '' AS DocumentOwnershipTypeTitle,
                                            '' AS DocumentRootTypeTitle,
                                            '' AS StateName,
                                            '' AS CityName,
                                            '' AS ZoneName,
                                            '' AS DistrictName,
                                            '' AS UserCreatorName,
                                            p.IsActivated,
                                            p.IsDeleted,
                                            p.ConsultantUserId,
                                            p.EditEnDate,
                                            p.RebuiltInYearFa,
                                            p.CreateEnDate,
                                            p.PropertyTypeId,
                                            p.TypeOfUseId,
                                            p.DocumentOwnershipTypeId,
                                            p.DocumentTypeId,
                                            p.DocumentRootTypeId,
                                            p.PropertyCodeName AS AdvertisementTitle,
                                            p.Area,
                                            p.PropertyDescriptions AS AdvertisementDescriptions,
                                            p.UserIdCreator,
                                            p.ShowInMelkavan,
                                            propAddr.CountryId,
                                            propAddr.StateId,
                                            propAddr.CityId,
                                            propAddr.ZoneId,
                                            propAddr.DistrictId,
                                            propAddr.TownName,
                                            propPriceHist.OfferPrice,
                                            propPriceHist.DepositPrice,
                                            propPriceHist.RentPrice,
                                            '' AS CurrentDate,
                                            pd.TagId,
                                            CAST(CASE WHEN fav.PropertyId IS NOT NULL THEN 1 ELSE 0 END AS BIT) AS IsFavorite,
                                            ISNULL(viewers.ViewersCount, 0) AS ViewersCount
                                            FROM TeniacoDb.dbo.Properties AS p
                                            INNER JOIN TeniacoDb.dbo.PropertyAddress AS propAddr ON p.PropertyId = propAddr.PropertyId
                                            INNER JOIN (
                                                SELECT PropertyId, CASE OfferPriceType WHEN '0' THEN CalculatedOfferPrice ELSE OfferPrice END as OfferPrice, DepositPrice, RentPrice
                                                FROM TeniacoDb.dbo.PropertiesPricesHistories AS pph
                                                WHERE pph.PropertyPriceHistoryId = (
                                                    SELECT MAX(PropertyPriceHistoryId)
                                                    FROM TeniacoDb.dbo.PropertiesPricesHistories
                                                    WHERE PropertyId = pph.PropertyId
                                                )
                                            ) AS propPriceHist ON p.PropertyId = propPriceHist.PropertyId
                                            INNER JOIN TeniacoDb.dbo.PropertiesDetails AS pd ON p.PropertyId = pd.PropertyId
                                            LEFT JOIN TeniacoDb.dbo.PropertiesFavorites AS fav ON p.PropertyId = fav.PropertyId
                                            LEFT JOIN (
                                                SELECT PropertyId, COUNT(*) AS ViewersCount
                                                FROM TeniacoDb.dbo.PropertiesViewers
                                                GROUP BY PropertyId
                                            ) AS viewers ON p.PropertyId = viewers.PropertyId

                                            {JoinTeniacoFeatures}
                                            {JoinPropertiesFavorites}
                                            WHERE p.ShowInMelkavan = 1 {whereCluaseTeniaco}
                                            ) AS tmp
                                            ORDER BY CreateEnDate DESC;  ";


                string joinTeniacoFeatureValues = "";
                string joinMelkavanFeatureValues = "";
                string AdvancedFilters = "";

                string strWhereCluaseMelkavan = "where";
                string strWhereCluaseTeniaco = "";

                if (IsFiltered == true)
                {



                    if (AdvertisementTypeId.HasValue && AdvertisementTypeId > 0)
                        AdvancedFilters += " AND AdvertisementTypeId = " + AdvertisementTypeId;


                    if (PropertyTypeId.HasValue && PropertyTypeId > 0)
                        AdvancedFilters += " AND PropertyTypeId = " + PropertyTypeId;


                    if (!String.IsNullOrEmpty(TypeOfUseId))
                        AdvancedFilters += " AND TypeOfUseId IN (" + TypeOfUseId + ")";


                    if (StateId.HasValue && StateId > 0)
                        AdvancedFilters += " AND StateId = " + StateId;


                    if (CityId.HasValue && CityId > 0)
                        AdvancedFilters += " AND CityId = " + CityId;


                    if (ZoneId.HasValue && ZoneId > 0)
                        AdvancedFilters += " AND ZoneId = " + ZoneId;


                    if (!String.IsNullOrEmpty(TownName))
                        AdvancedFilters += " AND TownName like N'%" + TownName + "%' ";


                    if (!String.IsNullOrEmpty(FromArea) && FromArea != "0")
                        AdvancedFilters += " AND CAST(Area AS float) > " + FromArea;


                    if (!String.IsNullOrEmpty(ToArea) && ToArea != "0")
                        AdvancedFilters += " AND CAST(Area AS float) < " + ToArea;


                    if (FromFoundation.HasValue && FromFoundation > 0)
                        AdvancedFilters += " AND Foundation > " + FromFoundation;


                    if (ToFoundation.HasValue && ToFoundation > 0)
                        AdvancedFilters += " AND Foundation < " + ToFoundation;


                    //if (FromPrice.HasValue && FromPrice>0)
                    //    AdvancedFilters += " AND OfferPrice > " + FromPrice;


                    //if (ToPrice.HasValue && ToPrice!=0 && ToPrice!=100000000000)
                    //  AdvancedFilters += " AND OfferPrice < " + ToPrice;


                    if (FromPrice.HasValue && FromPrice != 1000000000 && ToPrice.HasValue && ToPrice != 100000000000)
                        AdvancedFilters += @" AND ((OfferPriceType = 0 and CAST(CalculatedOfferPrice AS bigint) >= " + FromPrice.Value.ToString() +
                                            @") and (OfferPriceType = 0 and CAST(CalculatedOfferPrice AS bigint)  <= " + ToPrice.Value.ToString() + @")) or 
                                   ((OfferPriceType = 1 and CAST(OfferPrice AS bigint) >= " + FromPrice + ") and (OfferPriceType = 1 and CAST(OfferPrice AS bigint) <= " + ToPrice.Value.ToString() + ")) ";
                    else
                   if (FromPrice.HasValue && FromPrice != 1000000000 && ToPrice == 100000000000)
                    {
                        AdvancedFilters += @" AND (OfferPriceType = 0 and CAST(CalculatedOfferPrice AS bigint) >= " + FromPrice.Value.ToString() + @") or
                       (OfferPriceType = 1 and CAST(OfferPrice AS bigint) >= " + FromPrice + ") ";
                    }
                    else
                   if (FromPrice == 1000000000 && ToPrice.HasValue && ToPrice != 100000000000)
                    {
                        AdvancedFilters += @" AND (OfferPriceType = 0 and CAST(CalculatedOfferPrice AS bigint) <= " + ToPrice.Value.ToString() +
                            @") or
                       (OfferPriceType = 1 and CAST(OfferPrice AS bigint) <= " + ToPrice + ") ";
                    }


                    if (!String.IsNullOrEmpty(BuildingLifeId))
                        AdvancedFilters += " AND BuildingLifeId IN (" + BuildingLifeId + ")";


                    if (FromRebuiltInYear.HasValue && FromRebuiltInYear > 0)
                        AdvancedFilters += " AND RebuiltInYearFa > " + FromRebuiltInYear;


                    if (ToRebuiltInYear.HasValue && ToRebuiltInYear > 0)
                        AdvancedFilters += " AND RebuiltInYearFa < " + ToRebuiltInYear;


                    if (DocumentTypeId.HasValue && DocumentTypeId > 0)
                        AdvancedFilters += " AND DocumentTypeId = " + DocumentTypeId;


                    if (DocumentRootTypeId.HasValue && DocumentRootTypeId > 0)
                        AdvancedFilters += " AND DocumentRootTypeId = " + DocumentRootTypeId;


                    if (DocumentOwnershipTypeId.HasValue && DocumentOwnershipTypeId > 0)
                        AdvancedFilters += " AND DocumentOwnershipTypeId = " + DocumentOwnershipTypeId;


                    if (DocumentOwnershipTypeId.HasValue && DocumentOwnershipTypeId > 0)
                        AdvancedFilters += " AND DocumentOwnershipTypeId = " + DocumentOwnershipTypeId;


                    if (DepositFromPrice.HasValue && DepositFromPrice > 0)
                        AdvancedFilters += " AND DepositPrice > " + DepositFromPrice;


                    if (DepositToPrice.HasValue && DepositToPrice != 0 && DepositToPrice != 50000000000)
                        AdvancedFilters += " AND DepositPrice < " + DepositToPrice;


                    if (RentFromPrice.HasValue && RentFromPrice > 0)
                        AdvancedFilters += " AND RentPrice > " + RentFromPrice;


                    if (RentToPrice.HasValue && RentToPrice != 0 && RentToPrice != 10000000000)
                        AdvancedFilters += " AND RentPrice < " + RentToPrice;


                    if (MaritalStatusId.HasValue && MaritalStatusId != 0)
                        AdvancedFilters += " AND MaritalStatusId = " + MaritalStatusId;



                    if (!String.IsNullOrEmpty(Convertable) && Convertable != "0")
                        AdvancedFilters += " AND Convertable = " + Convertable;

                    //Query for features
                    if (Features != null && Features.Count > 0)
                    {
                        //Join features
                        joinTeniacoFeatureValues = "inner join TeniacoDb.dbo.FeaturesValues on p.PropertyId = TeniacoDb.dbo.FeaturesValues.PropertyId";
                        joinMelkavanFeatureValues = "inner join MelkavanDb.dbo.AdvertisementFeaturesValues on ad.AdvertisementId = MelkavanDb.dbo.AdvertisementFeaturesValues.AdvertisementId";


                        foreach (string key in Features.Keys)
                        {
                            AdvancedFilters += @" AND (ad.AdvertisementId in 
        			                            (select distinct AdvertisementId from MelkavanDb.dbo.AdvertisementFeaturesValues where AdvertisementId in 
        			                            (select AdvertisementId from MelkavanDb.dbo.AdvertisementFeaturesValues where FeatureValue IN (" + Features[key] + ") and FeatureId = " + key + ")))";
                        }
                    }


                    strWhereCluaseTeniaco += AdvancedFilters
                       .Replace("MelkavanDb", "TeniacoDb")
                       .Replace("Advertisement.AdvertisementId", "Properties.PropertyId")
                       .Replace("AdvertisementId", "PropertyId")
                       .Replace("AdvertisementFeaturesValues", "FeaturesValues")
                       .Replace("ad.PropertyId", "p.PropertyId");




                    // To remove first AND in Advanced Filters
                    int firstAnd = AdvancedFilters.IndexOf("AND");
                    if (firstAnd != -1)
                    {
                        AdvancedFilters = AdvancedFilters.Remove(firstAnd, 3);
                    }

                    strWhereCluaseMelkavan += AdvancedFilters;


                }



                #region AdvertisementTitle

                //use advertisementTitle for searching
                if (!string.IsNullOrEmpty(advertisementTitle))
                {
                    if (strWhereCluaseMelkavan.Equals("where"))
                    {
                        strWhereCluaseMelkavan += " AdvertisementTitle like N'%" + advertisementTitle + "%' ";

                    }
                    else
                    {
                        strWhereCluaseMelkavan += " and AdvertisementTitle like N'%" + advertisementTitle + "%' ";

                    }

                    strWhereCluaseTeniaco += "and PropertyCodeName  like N'%" + advertisementTitle + "'";
                }

                #endregion


                #region UserIdCreator

                //var strMelkavanUserIdCreator = "";
                //var strTeniacoUserIdCreator = "";


                //if (onlyFavorite.HasValue && userId.HasValue)
                //{
                //    if (onlyFavorite.Value == true && userId.Value > 0)
                //    {

                //        if (strWhereCluaseMelkavan.Contains("where"))
                //        {
                //            strMelkavanUserIdCreator += " and MelkavanDb.dbo.Advertisement.UserIdCreator =" + userId.Value.ToString();
                //        }
                //        else
                //        {
                //            strMelkavanUserIdCreator += " where MelkavanDb.dbo.Advertisement.UserIdCreator =" + userId.Value.ToString();
                //        }


                //        strTeniacoUserIdCreator = "and TeniacoDb.dbo.Properties.UserIdCreator =" + userId.Value.ToString();

                //    }

                //}


                //if (!string.IsNullOrEmpty(strMelkavanUserIdCreator))
                //{
                //    sp = sp.Replace("{melkavanUserIdCreatorWhereCluase}", strMelkavanUserIdCreator);
                //}
                //else
                //{
                //    sp = sp.Replace("{melkavanUserIdCreatorWhereCluase}", "");
                //}



                //if (!string.IsNullOrEmpty(strTeniacoUserIdCreator))
                //{
                //    sp = sp.Replace("{teniacoUserIdCreatorWhereCluase}", strTeniacoUserIdCreator);
                //}
                //else
                //{
                //    sp = sp.Replace("{teniacoUserIdCreatorWhereCluase}", "");
                //}

                string joinAdvertisementFavorites = "";
                string joinPropertiesFavorites = "";

                if (onlyFavorite.HasValue && userId.HasValue)
                {
                    if (onlyFavorite.Value == true && userId.Value > 0)
                    {
                        joinAdvertisementFavorites = "inner join MelkavanDb.dbo.AdvertisementFavorites on ad.AdvertisementId = MelkavanDb.dbo.AdvertisementFavorites.AdvertisementId";
                        joinPropertiesFavorites = "inner join TeniacoDb.dbo.PropertiesFavorites on p.PropertyId = TeniacoDb.dbo.PropertiesFavorites.PropertyId";

                        if (strWhereCluaseMelkavan.Equals("where"))
                        {
                            strWhereCluaseMelkavan += " AdvertisementFavorites.AdvertisementId = ad.AdvertisementId AND AdvertisementFavorites.UserIdCreator=" + userId.Value.ToString();
                        }
                        else
                        {
                            strWhereCluaseMelkavan += " AND AdvertisementFavorites.AdvertisementId = ad.AdvertisementId AND AdvertisementFavorites.UserIdCreator=" + userId.Value.ToString();
                        }


                        strWhereCluaseTeniaco = "AND PropertiesFavorites.PropertyId = p.PropertyId AND PropertiesFavorites.UserIdCreator = " + userId.Value.ToString();

                    }

                }




                #endregion




                sp = sp.Replace("{JoinMelkavanFeatures}", joinMelkavanFeatureValues);
                sp = sp.Replace("{JoinTeniacoFeatures}", joinTeniacoFeatureValues);
                sp = sp.Replace("{JoinAdvertisementFavorites}", joinAdvertisementFavorites);
                sp = sp.Replace("{JoinPropertiesFavorites}", joinPropertiesFavorites);







                if (strWhereCluaseMelkavan.Equals("where"))
                {
                    strWhereCluaseMelkavan = "";
                }



                if (!string.IsNullOrEmpty(strWhereCluaseMelkavan))
                {
                    sp = sp.Replace("{whereCluaseMelkavan}", strWhereCluaseMelkavan);
                }
                else
                {
                    sp = sp.Replace("{whereCluaseMelkavan}", "");
                }


                if (!string.IsNullOrEmpty(strWhereCluaseTeniaco))
                {
                    sp = sp.Replace("{whereCluaseTeniaco}", strWhereCluaseTeniaco);
                }
                else
                {
                    sp = sp.Replace("{whereCluaseTeniaco}", "");
                }


                //sp = string.Format(sp, whereClause, whereClause.Replace("AdvertisementTitle", "PropertyCodeName").Replace("AdvertisementDescriptions", "PropertyDescriptions"));

                var list1 = melkavanApiDb.AdvertisementAdvanceSearchVM.FromSqlRaw(sp).AsEnumerable().Distinct();


                #region ChildUsers
                //if (childsUsersIds != null)
                //{
                //    if (childsUsersIds.Count > 1)
                //    {
                //        list1 = list1.Where(p => p.UserIdCreator.HasValue).Where(p => childsUsersIds.Contains(p.UserIdCreator.Value));
                //    }
                //    else
                //    {
                //        if (childsUsersIds.Count == 1)
                //        {
                //            if (childsUsersIds.FirstOrDefault() > 0)
                //            {
                //                list1 = list1.Where(p => p.UserIdCreator.HasValue).Where(p => childsUsersIds.Contains(p.UserIdCreator.Value));
                //            }
                //        }
                //    }
                //}
                #endregion

                #region Pagination and Load extra title data

                try
                {
                    if (string.IsNullOrEmpty(jtSorting))
                    {
                        listCount = list1.Count();

                        //if (listCount > jtPageSize)
                        //{
                        list1 = list1.OrderByDescending(s => s.CreateEnDate)
                                 .Skip(jtStartIndex).Take(jtPageSize).ToList();
                        //}
                        //else
                        //{


                        //    list1 = list1.OrderByDescending(s => s.AdvertisementId).ToList();
                        //}
                    }
                    else
                    {
                        listCount = list1.Count();

                        if (listCount > jtPageSize)
                        {
                            switch (jtSorting)
                            {
                                case "AdvertisementTitle ASC":
                                    list1 = list1.OrderBy(l => l.AdvertisementTitle);
                                    break;
                                case "AdvertisementTitle DESC":
                                    list1 = list1.OrderByDescending(l => l.AdvertisementTitle);
                                    break;
                            }

                            if (string.IsNullOrEmpty(jtSorting))
                                advertisementAdvanceSearchVMList = list1.OrderByDescending(s => s.AdvertisementId)
                                         .Skip(jtStartIndex).Take(jtPageSize).ToList();
                            else
                                advertisementAdvanceSearchVMList = list1.Skip(jtStartIndex).Take(jtPageSize).ToList();
                        }
                        else
                        {

                            advertisementAdvanceSearchVMList = list1.ToList();
                        }
                    }



                    var stateIds = list1.Where(p => p.StateId.HasValue).Select(p => p.StateId.Value).ToList();
                    var states = publicApiDb.States.Where(s => stateIds.Contains(s.StateId)).ToList();

                    var cityIds = list1.Where(p => p.CityId.HasValue).Select(p => p.CityId.Value).ToList();
                    var cities = publicApiDb.Cities.Where(s => cityIds.Contains(s.CityId)).ToList();

                    var zoneIds = list1.Where(p => p.ZoneId.HasValue).Select(p => p.ZoneId.Value).ToList();
                    var zones = publicApiDb.Zones.Where(s => zoneIds.Contains(s.ZoneId)).ToList();

                    var districtIds = list1.Where(p => p.DistrictId.HasValue).Select(p => p.DistrictId.Value).ToList();
                    var districts = publicApiDb.Districts.Where(s => districtIds.Contains(s.DistrictId)).ToList();

                    var propertyIds = list1.Select(p => p.AdvertisementId).ToList().Distinct();

                    var advertisementIds = list1.Select(c => c.AdvertisementId).ToList().Distinct();

                    var advertisementFilesList = melkavanApiDb.AdvertisementFiles.Where(a => advertisementIds.Contains(a.AdvertisementId) && a.AdvertisementFileType.Equals("media")).ToList();

                    var propertiesFilesList = teniacoApiDb.PropertyFiles.Where(z => advertisementIds.Contains(z.PropertyId) && z.PropertyFileType.Equals("media")).ToList();

                    //var tagsIds = list1.Where(p => !p.TagId.IsNullOrEmpty()).Select(p => p.TagId).ToList();
                    //var tags = melkavanApiDb.Tags.Where(a => tagsIds.Contains(a.TagId)).ToList();


                    var tmpList = list1.Distinct();

                    foreach (var item in tmpList)
                    {

                        //var tag = tags.Where(s => s.TagId.Equals(item.TagId)).FirstOrDefault();
                        //if(tag != null)
                        //{
                        //    item.TagTitle = tag.TagTitle;
                        //}


                        var state = states.Where(s => s.StateId.Equals(item.StateId)).FirstOrDefault();
                        if (state != null)
                        {
                            item.StateName = state.StateName;
                        }

                        var city = cities.Where(c => c.CityId.Equals(item.CityId)).FirstOrDefault();
                        if (city != null)
                        {
                            item.CityName = city.CityName;
                        }

                        if (item.ZoneId.HasValue)
                        {
                            var zone = zones.Where(z => z.ZoneId.Equals(item.ZoneId.Value)).FirstOrDefault();
                            if (zone != null)
                            {
                                item.ZoneName = zone.ZoneName;
                            }
                        }

                        if (item.DistrictId.HasValue)
                        {
                            var district = districts.Where(z => z.DistrictId.Equals(item.DistrictId.Value)).FirstOrDefault();
                            if (district != null)
                            {
                                item.DistrictName = district.DistrictName;

                            }
                        }

                        if (userId.HasValue &&
                            melkavanApiDb.AdvertisementFavorites.Where(a => a.UserIdCreator == userId && a.AdvertisementId == item.AdvertisementId && a.IsDeleted != true).Any())
                        {
                            item.IsFavorite = true;
                        }
                        else
                        {
                            if (userId.HasValue &&
                                teniacoApiDb.PropertiesFavorites.Where(a => a.UserIdCreator == userId && a.PropertyId == item.AdvertisementId && a.IsDeleted != true).Any())
                            {
                                item.IsFavorite = true;
                            }
                        }


                        if (advertisementFilesList.Where(ad => ad.AdvertisementId == item.AdvertisementId).Any())
                        {
                            try
                            {
                                item.AdvertisementFilesVM = advertisementFilesList.Where(ad => ad.AdvertisementId == item.AdvertisementId)
                                    .Select(p => new AdvertisementFilesVM
                                    {
                                        AdvertisementId = p.AdvertisementId,
                                        AdvertisementFilePath = p.AdvertisementFilePath,
                                        AdvertisementFileExt = p.AdvertisementFileExt,
                                        AdvertisementFileTitle = p.AdvertisementFileTitle,
                                        AdvertisementFileId = p.AdvertisementFileId,
                                        AdvertisementFileType = p.AdvertisementFileType,
                                    }).OrderByDescending(f => f.AdvertisementFileId).ToList();
                            }
                            catch (Exception exc)
                            { }
                        }
                        else
                        {
                            if (propertiesFilesList.Where(ad => ad.PropertyId == item.AdvertisementId).Any())
                            {
                                try
                                {
                                    item.AdvertisementFilesVM = propertiesFilesList.Where(ad => ad.PropertyId == item.AdvertisementId)
                                        .Select(p => new AdvertisementFilesVM
                                        {
                                            AdvertisementId = p.PropertyId,
                                            AdvertisementFilePath = p.PropertyFilePath,
                                            AdvertisementFileExt = p.PropertyFileExt,
                                            AdvertisementFileTitle = p.PropertyFileTitle,
                                            AdvertisementFileId = p.PropertyFileId,
                                            AdvertisementFileType = p.PropertyFileType,
                                        }).OrderByDescending(f => f.AdvertisementFileId).ToList();
                                }
                                catch (Exception exc)
                                { }
                            }
                        }
                    }



                    advertisementAdvanceSearchVMList = tmpList.Distinct().ToList();

                }
                catch (Exception exc)
                { }

                #endregion


            }
            catch (Exception exc)
            { }



            return advertisementAdvanceSearchVMList;
        }


        public List<AdvertisementVM> GetListOfAdvertisement(
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
             string advertisementTitle = null)
        {


            List<AdvertisementVM> AdvertisementVMList = new List<AdvertisementVM>();


            var states = publicApiDb.States.ToList();
            var cities = publicApiDb.Cities.ToList();
            var zones = publicApiDb.Zones.ToList();
            var districts = publicApiDb.Districts.ToList();


            var list = (from p in melkavanApiDb.Advertisement
                        join pa in melkavanApiDb.AdvertisementAddress on p.AdvertisementId equals pa.AdvertisementId
                        join ad in melkavanApiDb.AdvertisementDetails on p.AdvertisementId equals ad.AdvertisementId
                        where
                        //childsUsersIds.Contains(p.UserIdCreator.Value) &&
                        p.IsDeleted.Value.Equals(false) &&
                        p.IsActivated.Value.Equals(true)
                        select new AdvertisementVM
                        {
                            Area = p.Area,
                            BuiltInYear = p.BuiltInYear.HasValue ? p.BuiltInYear.Value : (int?)0,
                            BuiltInYearFa = p.BuiltInYearFa.HasValue ? p.BuiltInYearFa.Value : (int?)0,
                            //IntermediaryPersonId = p.IntermediaryPersonId.HasValue ? p.IntermediaryPersonId.Value : (long?)null,
                            //OwnerPersonId = p.OwnerPersonId.HasValue ? p.OwnerPersonId.Value : (long?)null,
                            AdvertisementTitle = p.AdvertisementTitle,
                            AdvertisementId = p.AdvertisementId,
                            PropertyTypeId = p.PropertyTypeId,
                            RebuiltInYear = p.BuiltInYear.HasValue ? p.BuiltInYear.Value : (int?)0,
                            RebuiltInYearFa = p.RebuiltInYearFa.HasValue ? p.RebuiltInYearFa.Value : (int?)0,
                            TypeOfUseId = p.TypeOfUseId.HasValue ? p.TypeOfUseId.Value : (int?)0,
                            DocumentTypeId = p.DocumentTypeId.HasValue ? p.DocumentTypeId.Value : (int?)0,
                            DocumentOwnershipTypeId = p.DocumentOwnershipTypeId.HasValue ? p.DocumentOwnershipTypeId.Value : (int?)0,
                            DocumentRootTypeId = p.DocumentRootTypeId.HasValue ? p.DocumentRootTypeId.Value : (int?)0,
                            AdvertisementDescriptions = !string.IsNullOrEmpty(p.AdvertisementDescriptions) ? p.AdvertisementDescriptions : "",
                            CurrentDate = DateTime.Now,
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
                            AdvertisementAddressVM = new AdvertisementAddressVM
                            {
                                StateId = pa.StateId.Value,
                                TempStateId = pa.TempStateId.Value,
                                CityId = pa.CityId.Value,
                                TempCityId = pa.TempCityId.Value,
                                ZoneId = pa.ZoneId,
                                TempZoneId = pa.TempZoneId.Value,
                                DistrictId = pa.DistrictId,
                                //TempDistrictId = pa.DistrictId,
                                StateName = "",//x.StateName,
                                TempStateName = "",
                                CityName = "",//x.CityName,
                                TempCityName = "",
                                ZoneName = "",//x.ZoneName,
                                TempZoneName = "",
                                DistrictName = "",
                                TownName = pa.TownName,
                                //VillageName = "",//x.VillageName,
                                //Abbreviation = "",//x.Abbreviation,
                                Address = !string.IsNullOrEmpty(pa.Address) ? pa.Address : "",
                                //Address = pa.Address,
                                LocationLat = pa.LocationLat,
                                LocationLon = pa.LocationLon,
                                AdvertisementId = pa.AdvertisementId,
                                UserIdCreator = pa.UserIdCreator,
                                CreateEnDate = pa.CreateEnDate,
                                CreateTime = pa.CreateTime,
                                EditEnDate = pa.EditEnDate,
                                EditTime = pa.EditTime,
                                UserIdEditor = pa.UserIdEditor,
                                RemoveEnDate = pa.RemoveEnDate,
                                RemoveTime = pa.EditTime,
                                UserIdRemover = pa.UserIdRemover,
                                IsActivated = pa.IsActivated,
                                IsDeleted = pa.IsDeleted,
                            },
                            AdvertisementDetailsVM = new AdvertisementDetailsVM
                            {
                                AdvertisementId = p.AdvertisementId,
                                AdvertisementDetailsId = ad.AdvertisementDetailsId,
                                AdvertisementTypeId = ad.AdvertisementTypeId,
                                Convertable = ad.Convertable,
                                MaritalStatusId = ad.MaritalStatusId,
                                //MaritalStatusTitle = ad.MaritalStatusTitle,
                                ShowInSpecialSuggestion = ad.ShowInSpecialSuggestion,
                                VerifyAdvertisement = ad.VerifyAdvertisement,
                                BuildingLifeId = ad.BuildingLifeId,
                                Foundation = ad.Foundation,
                                UserIdCreator = ad.UserIdCreator,
                                CreateEnDate = ad.CreateEnDate,
                                CreateTime = ad.CreateTime,
                                EditEnDate = ad.EditEnDate,
                                EditTime = ad.EditTime,
                                UserIdEditor = ad.UserIdEditor,
                                RemoveEnDate = ad.RemoveEnDate,
                                RemoveTime = ad.EditTime,
                                UserIdRemover = ad.UserIdRemover,
                                IsActivated = ad.IsActivated,
                                IsDeleted = ad.IsDeleted,
                            },

                        })
                        .AsEnumerable();



            if (advertisementTypeId.HasValue)
                if (advertisementTypeId.Value > 0)
                    list = list.Where(a => a.AdvertisementDetailsVM.AdvertisementTypeId.Equals(advertisementTypeId.Value));


            if (propertyTypeId.HasValue)
                if (propertyTypeId.Value > 0)
                    list = list.Where(a => a.PropertyTypeId.Equals(propertyTypeId.Value));


            if (typeOfUseId.HasValue)
                if (typeOfUseId.Value > 0)
                    list = list.Where(a => a.TypeOfUseId.Equals(typeOfUseId.Value));


            if (documentTypeId.HasValue)
                if (documentTypeId.Value > 0)
                    list = list.Where(a => a.DocumentTypeId.Equals(documentTypeId.Value));


            if (!string.IsNullOrEmpty(propertyCodeName))
                list = list.Where(z => z.AdvertisementTitle.Contains(propertyCodeName));


            if (stateId.HasValue)
                if (stateId.Value > 0)
                    list = list.Where(a => a.AdvertisementAddressVM.StateId.Equals(stateId.Value));


            if (cityId.HasValue)
                if (cityId.Value > 0)
                    list = list.Where(a => a.AdvertisementAddressVM.CityId.Equals(cityId.Value));


            if (zoneId.HasValue)
                if (zoneId.Value > 0)
                    list = list.Where(a => a.AdvertisementAddressVM.ZoneId.Equals(zoneId.Value));


            if (districtId.HasValue)
                if (districtId.Value > 0)
                    list = list.Where(a => a.AdvertisementAddressVM.DistrictId.Equals(districtId.Value));

            if (onlyFavorite.HasValue && userId.HasValue)
                if (onlyFavorite.Value == true && userId.Value > 0)
                {
                    List<long> userFavorites = new List<long>();


                    if (melkavanApiDb.AdvertisementFavorites.Where(a => a.UserIdCreator == userId).Any())
                    {
                        userFavorites = melkavanApiDb.AdvertisementFavorites.Where(a => a.UserIdCreator == userId && a.IsDeleted != true).Select(a => a.AdvertisementId).ToList();
                    }

                    list = list.Where(a => userFavorites.Contains(a.AdvertisementId));
                }

            if (!string.IsNullOrEmpty(advertisementTitle))
                list = list.Where(a => a.AdvertisementTitle.Contains(advertisementTitle));

            //if (intermediaryPersonId.HasValue)
            //    if (intermediaryPersonId.Value > 0)
            //        list = list.Where(a => a.IntermediaryPersonId.Equals(intermediaryPersonId));

            //if (ownerPersonId.HasValue)
            //    if (ownerPersonId.Value > 0)
            //        list = list.Where(a => a.OwnerPersonId.Equals(ownerPersonId));


            try
            {
                if (string.IsNullOrEmpty(jtSorting))
                {
                    listCount = list.Count();

                    if (listCount >= jtPageSize)
                    {
                        AdvertisementVMList = list.OrderByDescending(s => s.AdvertisementId)
                                 .Skip(jtStartIndex).Take(jtPageSize).ToList();
                    }
                    else
                    {
                        AdvertisementVMList = list.OrderByDescending(s => s.AdvertisementId).ToList();
                    }
                }
                else
                {
                    listCount = list.Count();

                    if (listCount > jtPageSize)
                    {
                        switch (jtSorting)
                        {
                            case "AdvertisementTitle ASC":
                                list = list.OrderBy(l => l.AdvertisementTitle);
                                break;
                            case "AdvertisementTitle DESC":
                                list = list.OrderByDescending(l => l.AdvertisementTitle);
                                break;
                        }

                        if (string.IsNullOrEmpty(jtSorting))
                            AdvertisementVMList = list.OrderByDescending(s => s.AdvertisementId)
                                     .Skip(jtStartIndex).Take(jtPageSize).ToList();
                        else
                            AdvertisementVMList = list.Skip(jtStartIndex).Take(jtPageSize).ToList();
                    }
                    else
                    {

                        AdvertisementVMList = list.ToList();
                    }
                }

                var advertisementIds = AdvertisementVMList.Select(a => a.AdvertisementId).ToList();

                var advertisementPriceHistories = melkavanApiDb.AdvertisementPricesHistories.Where(a => advertisementIds.Contains(a.AdvertisementId)).ToList();
                var advertisementFilesList = melkavanApiDb.AdvertisementFiles.Where(a => advertisementIds.Contains(a.AdvertisementId)).ToList();

                foreach (var item in AdvertisementVMList)
                {
                    var state = states.Where(s => s.StateId.Equals(item.AdvertisementAddressVM.StateId)).FirstOrDefault();
                    if (state != null)
                    {
                        item.AdvertisementAddressVM.StateId = state.StateId;
                        item.AdvertisementAddressVM.StateName = state.StateName;
                    }

                    var city = cities.Where(c => c.CityId.Equals(item.AdvertisementAddressVM.CityId)).FirstOrDefault();
                    if (city != null)
                    {
                        item.AdvertisementAddressVM.CityId = city.CityId;
                        item.AdvertisementAddressVM.CityName = city.CityName;
                    }

                    if (item.AdvertisementAddressVM.ZoneId.HasValue)
                    {
                        var zone = zones.Where(z => z.ZoneId.Equals(item.AdvertisementAddressVM.ZoneId.Value)).FirstOrDefault();
                        if (zone != null)
                        {
                            item.AdvertisementAddressVM.ZoneId = zone.ZoneId;
                            item.AdvertisementAddressVM.ZoneName = zone.ZoneName;
                        }
                    }

                    if (item.AdvertisementAddressVM.DistrictId.HasValue)
                    {
                        var district = districts.Where(z => z.DistrictId.Equals(item.AdvertisementAddressVM.DistrictId.Value)).FirstOrDefault();
                        if (district != null)
                        {
                            item.AdvertisementAddressVM.DistrictId = district.DistrictId;
                            item.AdvertisementAddressVM.DistrictName = district.DistrictName;
                            //item.AdvertisementAddressVM.VillageName = district.VillageName;
                            item.AdvertisementAddressVM.TownName = district.TownName;
                        }
                    }

                    if (advertisementPriceHistories.Where(ad => ad.AdvertisementId == item.AdvertisementId).Any())
                    {
                        try
                        {
                            if (advertisementPriceHistories.Where(a => a.AdvertisementId.Equals(item.AdvertisementId)).Any())
                            {
                                var advertisementPricesHistoriesVM = advertisementPriceHistories.Where(ad => ad.AdvertisementId == item.AdvertisementId).
                                    OrderByDescending(a => a.CreateEnDate).ThenByDescending(a => a.CreateTime).FirstOrDefault();

                                item.AdvertisementPricesHistoriesVM = new AdvertisementPricesHistoriesVM();
                                item.AdvertisementPricesHistoriesVM.AdvertisementId = advertisementPricesHistoriesVM.AdvertisementId;
                                item.AdvertisementPricesHistoriesVM.OfferPrice = advertisementPricesHistoriesVM.OfferPrice;
                                item.AdvertisementPricesHistoriesVM.RentPrice = advertisementPricesHistoriesVM.RentPrice;
                                item.AdvertisementPricesHistoriesVM.DepositPrice = advertisementPricesHistoriesVM.DepositPrice;
                                item.AdvertisementPricesHistoriesVM.CalculatedOfferPrice = advertisementPricesHistoriesVM.CalculatedOfferPrice;
                                item.AdvertisementPricesHistoriesVM.OfferPriceType = advertisementPricesHistoriesVM.OfferPriceType;
                                item.AdvertisementPricesHistoriesVM.CreateEnDate = advertisementPricesHistoriesVM.CreateEnDate;
                                item.AdvertisementPricesHistoriesVM.CreateTime = advertisementPricesHistoriesVM.CreateTime;
                                //item.AdvertisementPricesHistoriesVM.EditEnDate = advertisementPricesHistoriesVM.EditEnDate;
                                //item.AdvertisementPricesHistoriesVM.EditTime = advertisementPricesHistoriesVM.EditTime;
                                //item.AdvertisementPricesHistoriesVM.UserIdEditor = advertisementPricesHistoriesVM.UserIdEditor.Value;
                                //item.AdvertisementPricesHistoriesVM.RemoveEnDate = advertisementPricesHistoriesVM.RemoveEnDate;
                                //item.AdvertisementPricesHistoriesVM.RemoveTime = advertisementPricesHistoriesVM.EditTime;
                                //item.AdvertisementPricesHistoriesVM.UserIdRemover = advertisementPricesHistoriesVM.UserIdRemover.Value;
                                item.AdvertisementPricesHistoriesVM.IsActivated = advertisementPricesHistoriesVM.IsActivated;
                                item.AdvertisementPricesHistoriesVM.IsDeleted = advertisementPricesHistoriesVM.IsDeleted;

                                //.Select(p => new AdvertisementPricesHistoriesVM
                                //{
                                //    AdvertisementId = p.AdvertisementId,
                                //    OfferPrice = p.OfferPrice,
                                //    RentPrice = p.RentPrice,
                                //    DepositPrice = p.DepositPrice,
                                //    CalculatedOfferPrice = p.CalculatedOfferPrice,
                                //    OfferPriceType = p.OfferPriceType,
                                //    CreateEnDate = p.CreateEnDate,
                                //    CreateTime = p.CreateTime,
                                //    EditEnDate = p.EditEnDate,
                                //    EditTime = p.EditTime,
                                //    UserIdEditor = p.UserIdEditor.Value,
                                //    RemoveEnDate = p.RemoveEnDate,
                                //    RemoveTime = p.EditTime,
                                //    UserIdRemover = p.UserIdRemover.Value,
                                //    IsActivated = p.IsActivated,
                                //    IsDeleted = p.IsDeleted,
                                //}).OrderByDescending(a => a.CreateEnDate).ThenByDescending(a => a.CreateTime).FirstOrDefault();
                            }
                        }
                        catch (Exception exc)
                        { }
                    }

                    if (advertisementFilesList.Where(ad => ad.AdvertisementId == item.AdvertisementId).Any())
                    {
                        try
                        {
                            item.AdvertisementFilesVM = advertisementFilesList.Where(ad => ad.AdvertisementId == item.AdvertisementId)
                                .Select(p => new AdvertisementFilesVM
                                {
                                    AdvertisementId = p.AdvertisementId,
                                    AdvertisementFilePath = p.AdvertisementFilePath,
                                    AdvertisementFileExt = p.AdvertisementFileExt,
                                    AdvertisementFileTitle = p.AdvertisementFileTitle,
                                    AdvertisementFileId = p.AdvertisementFileId,
                                    AdvertisementFileType = p.AdvertisementFileType,
                                    //CreateEnDate = p.CreateEnDate,
                                    //CreateTime = p.CreateTime,
                                    //EditEnDate = p.EditEnDate,
                                    //EditTime = p.EditTime,
                                    //UserIdEditor = p.UserIdEditor.Value,
                                    //RemoveEnDate = p.RemoveEnDate,
                                    //RemoveTime = p.EditTime,
                                    //UserIdRemover = p.UserIdRemover.Value,
                                    //IsActivated = p.IsActivated,
                                    //IsDeleted = p.IsDeleted,
                                }).OrderByDescending(f => f.AdvertisementFileId).ToList();
                        }
                        catch (Exception exc)
                        { }
                    }
                    if (userId.HasValue && melkavanApiDb.AdvertisementFavorites
                        .Where(a => a.UserIdCreator == userId && a.AdvertisementId == item.AdvertisementId && a.IsDeleted != true).Any())
                    {
                        item.IsFavorite = true;
                    }


                }
            }
            catch (Exception exc)
            { }

            return AdvertisementVMList;
        }



        public long AddToAdvertisement(AdvertisementVM advertisementVM,
             IPublicApiBusiness publicApiBusiness,
             IConsoleBusiness consoleBusiness)
        {
            using (var transaction = melkavanApiDb.Database.BeginTransaction())
            {
                try
                {

                    #region Advertisement


                    Advertisement Advertisement = _mapper.Map<AdvertisementVM, Advertisement>(advertisementVM);
                    Advertisement.AdvertiserId = Advertisement.UserIdCreator; //آگهی دهنده - کسی که لاگین کرده است.
                    Advertisement.StatusId = 3; // در انتظار تعیین وضعیت

                    #region Add ConsultantUserId
                    //مشاور

                    var userId = consoleBusiness.CmsDb.Users.Where(c => c.UserId.Equals(Advertisement.UserIdCreator)).Select(c => c.UserId).FirstOrDefault();
                    var LevelIds = consoleBusiness.CmsDb.UsersLevels.Where(c => c.UserId.Equals(userId)).Select(c => c.LevelId).ToList();
                    var levelNames = consoleBusiness.CmsDb.Levels.Where(c => LevelIds.Contains(c.LevelId)).Select(c => c.LevelName).ToList();


                    if (levelNames.Contains("مشاور"))
                    {
                        Advertisement.ConsultantUserId = Advertisement.UserIdCreator;

                    }

                    #endregion


                    melkavanApiDb.Advertisement.Add(Advertisement);
                    melkavanApiDb.SaveChanges();

                    #endregion

                    #region AdvertisementAddress 


                    if (advertisementVM.AdvertisementAddressVM != null)
                    {
                        AdvertisementAddress AdvertisementAddress = _mapper.Map<AdvertisementAddressVM, AdvertisementAddress>(advertisementVM.AdvertisementAddressVM);
                        AdvertisementAddress.AdvertisementId = Advertisement.AdvertisementId;
                        AdvertisementAddress.IsActivated = true;
                        AdvertisementAddress.IsDeleted = false;
                        AdvertisementAddress.UserIdCreator = Advertisement.UserIdCreator.Value;

                        melkavanApiDb.AdvertisementAddress.Add(AdvertisementAddress);
                        melkavanApiDb.SaveChanges();
                    }
                    else
                    {
                        AdvertisementAddress AdvertisementAddress = new AdvertisementAddress();
                        AdvertisementAddress.AdvertisementId = Advertisement.AdvertisementId;
                        AdvertisementAddress.CreateEnDate = Advertisement.CreateEnDate.Value;
                        AdvertisementAddress.CreateTime = Advertisement.CreateTime;
                        AdvertisementAddress.UserIdCreator = Advertisement.UserIdCreator.Value;
                        AdvertisementAddress.IsActivated = true;
                        AdvertisementAddress.IsDeleted = false;

                        melkavanApiDb.AdvertisementAddress.Add(AdvertisementAddress);
                        melkavanApiDb.SaveChanges();
                    }

                    #endregion

                    #region AdvertisementDetails 



                    if (advertisementVM.AdvertisementDetailsVM != null)
                    {
                        AdvertisementDetails AdvertisementDetails = _mapper.Map<AdvertisementDetailsVM, AdvertisementDetails>(advertisementVM.AdvertisementDetailsVM);
                        AdvertisementDetails.AdvertisementId = Advertisement.AdvertisementId;
                        AdvertisementDetails.UserIdCreator = Advertisement.UserIdCreator.Value;
                        AdvertisementDetails.IsActivated = true;
                        AdvertisementDetails.IsDeleted = false;

                        melkavanApiDb.AdvertisementDetails.Add(AdvertisementDetails);
                        melkavanApiDb.SaveChanges();
                    }
                    else
                    {
                        AdvertisementDetails AdvertisementDetails = new AdvertisementDetails();
                        AdvertisementDetails.AdvertisementId = Advertisement.AdvertisementId;
                        AdvertisementDetails.CreateEnDate = Advertisement.CreateEnDate.Value;
                        AdvertisementDetails.CreateTime = Advertisement.CreateTime;
                        AdvertisementDetails.UserIdCreator = Advertisement.UserIdCreator.Value;
                        AdvertisementDetails.IsActivated = true;
                        AdvertisementDetails.IsDeleted = false;

                        melkavanApiDb.AdvertisementDetails.Add(AdvertisementDetails);
                        melkavanApiDb.SaveChanges();
                    }

                    #endregion

                    #region AdvertisementPricesHistories 

                    var OfferPrice = (long)0;
                    var Area = Convert.ToInt64(advertisementVM.Area);


                    if (advertisementVM.AdvertisementPricesHistoriesVM != null)
                    {
                        if (advertisementVM.AdvertisementDetailsVM.AdvertisementTypeId.Equals(1)) // اجاره
                        {
                            //ثبت قیمت اجاره و ودیعه اجباری است
                            if ((advertisementVM.AdvertisementPricesHistoriesVM.RentPrice != null) &&
                                (advertisementVM.AdvertisementPricesHistoriesVM.DepositPrice != null))
                            {
                                if ((advertisementVM.AdvertisementPricesHistoriesVM.RentPrice.Value > 0) &&
                                     (advertisementVM.AdvertisementPricesHistoriesVM.DepositPrice.Value > 0))
                                {

                                    AdvertisementPricesHistories AdvertisementPricesHistories = _mapper.Map<AdvertisementPricesHistoriesVM, AdvertisementPricesHistories>(advertisementVM.AdvertisementPricesHistoriesVM);
                                    AdvertisementPricesHistories.OfferPrice = 0;
                                    AdvertisementPricesHistories.CalculatedOfferPrice = 0;
                                    AdvertisementPricesHistories.RentPrice = advertisementVM.AdvertisementPricesHistoriesVM.RentPrice.Value;
                                    AdvertisementPricesHistories.DepositPrice = advertisementVM.AdvertisementPricesHistoriesVM.DepositPrice.Value;
                                    AdvertisementPricesHistories.AdvertisementId = Advertisement.AdvertisementId;

                                    AdvertisementPricesHistories.CreateEnDate = Advertisement.CreateEnDate.Value;
                                    AdvertisementPricesHistories.CreateTime = Advertisement.CreateTime;
                                    AdvertisementPricesHistories.UserIdCreator = Advertisement.UserIdCreator.Value;
                                    AdvertisementPricesHistories.IsActivated = true;
                                    AdvertisementPricesHistories.IsDeleted = false;

                                    melkavanApiDb.AdvertisementPricesHistories.Add(AdvertisementPricesHistories);
                                    melkavanApiDb.SaveChanges();
                                }
                            }

                        }
                        else if (advertisementVM.AdvertisementDetailsVM.AdvertisementTypeId.Equals(2)) //فروش
                        {
                            //ثبت قیمت در فروش ملک اجباری نیست
                            if (advertisementVM.AdvertisementPricesHistoriesVM.OfferPrice != null)
                            {
                                if (advertisementVM.AdvertisementPricesHistoriesVM.OfferPrice.Value > 0) //قیمت را وارد کرده بود
                                {
                                    OfferPrice = Convert.ToInt64(advertisementVM.AdvertisementPricesHistoriesVM.OfferPrice.Value);

                                    AdvertisementPricesHistories AdvertisementPricesHistories = _mapper.Map<AdvertisementPricesHistoriesVM, AdvertisementPricesHistories>(advertisementVM.AdvertisementPricesHistoriesVM);
                                    AdvertisementPricesHistories.CalculatedOfferPrice = Convert.ToInt64(OfferPrice / Area);
                                    AdvertisementPricesHistories.OfferPriceType = 1;
                                    AdvertisementPricesHistories.OfferPrice = OfferPrice;
                                    AdvertisementPricesHistories.AdvertisementId = Advertisement.AdvertisementId;
                                    AdvertisementPricesHistories.CreateEnDate = Advertisement.CreateEnDate.Value;
                                    AdvertisementPricesHistories.CreateTime = Advertisement.CreateTime;
                                    AdvertisementPricesHistories.UserIdCreator = Advertisement.UserIdCreator.Value;
                                    AdvertisementPricesHistories.IsActivated = true;
                                    AdvertisementPricesHistories.IsDeleted = false;

                                    melkavanApiDb.AdvertisementPricesHistories.Add(AdvertisementPricesHistories);
                                    melkavanApiDb.SaveChanges();
                                }
                                else // قیمت را صفر وارد کرده بود
                                {
                                    AdvertisementPricesHistories AdvertisementPricesHistories = _mapper.Map<AdvertisementPricesHistoriesVM, AdvertisementPricesHistories>(advertisementVM.AdvertisementPricesHistoriesVM);
                                    AdvertisementPricesHistories.CalculatedOfferPrice = 0; //فروش قیمت ندارد
                                    AdvertisementPricesHistories.OfferPriceType = 1;
                                    AdvertisementPricesHistories.OfferPrice = 0; // فروش قیمت ندارد
                                    AdvertisementPricesHistories.AdvertisementId = Advertisement.AdvertisementId;
                                    AdvertisementPricesHistories.CreateEnDate = Advertisement.CreateEnDate.Value;
                                    AdvertisementPricesHistories.CreateTime = Advertisement.CreateTime;
                                    AdvertisementPricesHistories.UserIdCreator = Advertisement.UserIdCreator.Value;
                                    AdvertisementPricesHistories.IsActivated = true;
                                    AdvertisementPricesHistories.IsDeleted = false;

                                    melkavanApiDb.AdvertisementPricesHistories.Add(AdvertisementPricesHistories);
                                    melkavanApiDb.SaveChanges();
                                }
                            }
                            else // قیمت را وارد نکرده بود
                            {
                                AdvertisementPricesHistories AdvertisementPricesHistories = _mapper.Map<AdvertisementPricesHistoriesVM, AdvertisementPricesHistories>(advertisementVM.AdvertisementPricesHistoriesVM);
                                AdvertisementPricesHistories.CalculatedOfferPrice = 0; //فروش قیمت ندارد
                                AdvertisementPricesHistories.OfferPriceType = 1;
                                AdvertisementPricesHistories.OfferPrice = 0; // فروش قیمت ندارد
                                AdvertisementPricesHistories.AdvertisementId = Advertisement.AdvertisementId;
                                AdvertisementPricesHistories.CreateEnDate = Advertisement.CreateEnDate.Value;
                                AdvertisementPricesHistories.CreateTime = Advertisement.CreateTime;
                                AdvertisementPricesHistories.UserIdCreator = Advertisement.UserIdCreator.Value;
                                AdvertisementPricesHistories.IsActivated = true;
                                AdvertisementPricesHistories.IsDeleted = false;

                                melkavanApiDb.AdvertisementPricesHistories.Add(AdvertisementPricesHistories);
                                melkavanApiDb.SaveChanges();
                            }

                        }

                    }
                    else
                    {

                        AdvertisementPricesHistories AdvertisementPricesHistories = new AdvertisementPricesHistories();
                        AdvertisementPricesHistories.OfferPrice = advertisementVM.OfferPrice.HasValue ? advertisementVM.OfferPrice.Value : 0;
                        AdvertisementPricesHistories.OfferPriceType = advertisementVM.OfferPriceType.HasValue ? advertisementVM.OfferPriceType.Value : 0;
                        AdvertisementPricesHistories.CalculatedOfferPrice = advertisementVM.CalculatedOfferPrice.HasValue ? advertisementVM.CalculatedOfferPrice.Value : 0;
                        //AdvertisementPricesHistories.CalculatedOfferPrice = Convert.ToInt64(OfferPrice / Area);
                        AdvertisementPricesHistories.AdvertisementId = Advertisement.AdvertisementId;
                        AdvertisementPricesHistories.RentPrice = advertisementVM.RentPrice;
                        AdvertisementPricesHistories.DepositPrice = advertisementVM.DepositPrice;

                        AdvertisementPricesHistories.CreateEnDate = Advertisement.CreateEnDate.Value;
                        AdvertisementPricesHistories.CreateTime = Advertisement.CreateTime;
                        AdvertisementPricesHistories.UserIdCreator = Advertisement.UserIdCreator.Value;
                        AdvertisementPricesHistories.IsActivated = true;
                        AdvertisementPricesHistories.IsDeleted = false;

                        melkavanApiDb.AdvertisementPricesHistories.Add(AdvertisementPricesHistories);
                        melkavanApiDb.SaveChanges();
                    }
                    #endregion

                    #region AdvertisementFiles


                    if (advertisementVM.AdvertisementFilesVM != null)
                    {
                        if (advertisementVM.AdvertisementFilesVM.Count > 0)
                        {
                            var AdvertisementFilesList = _mapper.Map<List<AdvertisementFilesVM>, List<AdvertisementFiles>>(advertisementVM.AdvertisementFilesVM);

                            foreach (var item in AdvertisementFilesList)
                            {
                                item.AdvertisementId = Advertisement.AdvertisementId;
                                item.UserIdCreator = Advertisement.UserIdCreator.Value;
                                item.IsActivated = true;
                                item.IsDeleted = false;
                            }

                            melkavanApiDb.AdvertisementFiles.AddRange(AdvertisementFilesList);
                            melkavanApiDb.SaveChanges();
                        }

                    }

                    #endregion

                    #region AdvertisementOwners


                    if (advertisementVM.AdvertisementOwnersVM != null)
                    {
                        var AdvertisementOwners = _mapper.Map<AdvertisementOwnersVM, AdvertisementOwners>(advertisementVM.AdvertisementOwnersVM);
                        AdvertisementOwners.AdvertisementId = Advertisement.AdvertisementId;

                        AdvertisementOwners.IsActivated = true;
                        AdvertisementOwners.IsDeleted = false;
                        AdvertisementOwners.OwnerType = "users";

                        melkavanApiDb.AdvertisementOwners.Add(AdvertisementOwners);
                        melkavanApiDb.SaveChanges();


                    }
                    #endregion

                    #region AdvertisementFeaturesValues


                    if (advertisementVM.AdvertisementFeaturesValuesVM != null)
                    {
                        List<AdvertisementFeaturesValues> advertisementFeaturesValuesList = new List<AdvertisementFeaturesValues>();

                        if (advertisementVM.AdvertisementFeaturesValuesVM.Count > 0)
                        {


                            foreach (var item in advertisementVM.AdvertisementFeaturesValuesVM)
                            {

                                if (!item.FeatureValue.IsNullOrEmpty())
                                {
                                    AdvertisementFeaturesValues advertisementFeaturesValues = new AdvertisementFeaturesValues();


                                    advertisementFeaturesValues.AdvertisementId = Advertisement.AdvertisementId;
                                    advertisementFeaturesValues.CreateEnDate = Advertisement.CreateEnDate.Value;
                                    advertisementFeaturesValues.CreateTime = Advertisement.CreateTime;
                                    advertisementFeaturesValues.UserIdCreator = Advertisement.UserIdCreator.Value;
                                    advertisementFeaturesValues.IsActivated = true;
                                    advertisementFeaturesValues.IsDeleted = false;



                                    advertisementFeaturesValues.FeatureId = item.FeatureId;
                                    advertisementFeaturesValues.FeatureValue = item.FeatureValue;
                                    advertisementFeaturesValuesList.Add(advertisementFeaturesValues);
                                }

                            }

                            melkavanApiDb.AdvertisementFeaturesValues.AddRange(advertisementFeaturesValuesList);
                            melkavanApiDb.SaveChanges();
                        }
                    }
                    #endregion

                    #region AdvertisementSelectedCallers


                    var user = consoleBusiness.CmsDb.UsersProfile.Where(u => u.UserId == Advertisement.UserIdCreator.Value).FirstOrDefault();
                    if (user != null && Advertisement.AdvertisementId != 0)
                    {
                        AdvertisementSelectedCallers selectedCaller = new AdvertisementSelectedCallers()
                        {
                            CallerType = "Advertiser",
                            AdvertiserNumber = user.Mobile,
                            AdvertisementId = Advertisement.AdvertisementId,
                            UserIdCreator = Advertisement.UserIdCreator.Value,
                            CreateTime = PersianDate.TimeNow,
                            CreateEnDate = DateTime.Now,
                            IsActivated = true,
                            IsDeleted = false
                        };

                        melkavanApiDb.AdvertisementSelectedCallers.Add(selectedCaller);
                        melkavanApiDb.SaveChanges();

                    };



                    #endregion

                    transaction.Commit();


                    return Advertisement.AdvertisementId;

                }
                catch (Exception exc)
                {
                    transaction.Rollback();
                }
            }
            return 0;
        }



        public AdvertisementVM GetAdvertisementWithAdvertisementId(
            long advertisementId,
            List<long> childsUsersIds,
            IConsoleBusiness consoleBusiness,
            PublicApiContext publicApiDb,
            TeniacoApiContext teniacoApiDb,
            MelkavanApiContext melkavanApiDb,
            long? userId = 0,
            string type = "")
        {
            AdvertisementVM advertisementVM = new AdvertisementVM();

            try
            {

                if (type.Equals("Properties"))
                {

                    #region Property

                    advertisementVM = teniacoApiDb.Properties
                       .Where(e => e.PropertyId.Equals(advertisementId)).Select(a => new AdvertisementVM
                       {
                           AdvertisementDescriptions = a.PropertyDescriptions,
                           AdvertisementId = a.PropertyId,
                           AdvertisementTitle = a.PropertyCodeName,
                           Area = a.Area,
                           BuiltInYear = a.BuiltInYear,
                           BuiltInYearFa = a.BuiltInYearFa,
                           PropertyTypeId = a.PropertyTypeId,
                           TypeOfUseId = a.TypeOfUseId,
                           DocumentOwnershipTypeId = a.DocumentOwnershipTypeId,
                           DocumentRootTypeId = a.DocumentRootTypeId,
                           DocumentTypeId = a.DocumentTypeId,
                           StatusId = a.StatusId,
                           RejectionReason = a.RejectionReason,
                           RebuiltInYear = a.RebuiltInYear,
                           RebuiltInYearFa = a.RebuiltInYearFa,
                           CurrentDate = DateTime.Now,
                           UserIdCreator = a.UserIdCreator,
                           CreateEnDate = a.CreateEnDate,
                           CreateTime = a.CreateTime,
                           EditEnDate = a.EditEnDate,
                           EditTime = a.EditTime,
                           UserIdEditor = a.UserIdEditor.Value,
                           RemoveEnDate = a.RemoveEnDate,
                           RemoveTime = a.EditTime,
                           UserIdRemover = a.UserIdRemover.Value,
                           IsActivated = a.IsActivated,
                           IsDeleted = a.IsDeleted,
                       }).FirstOrDefault();



                    #region TypeOfUses

                    var typeOfUses = teniacoApiDb.TypeOfUses.Where(a => a.TypeOfUseId.Equals(advertisementVM.TypeOfUseId)).ToList();

                    if (typeOfUses != null)
                    {
                        advertisementVM.TypeUsesVM = typeOfUses.Select(a => new TypeOfUsesVM
                        {
                            TypeOfUseTitle = a.TypeOfUseTitle,

                        }).FirstOrDefault();
                        advertisementVM.TypeUseTitle = advertisementVM.TypeUsesVM.TypeOfUseTitle;
                    }

                    #endregion

                    #region DocumentRootTypes

                    var documentRootTypes = teniacoApiDb.DocumentRootTypes.Where(a => a.DocumentRootTypeId == advertisementVM.DocumentRootTypeId).ToList();

                    if (documentRootTypes != null)
                    {
                        advertisementVM.DocumentRootTypeTitle = documentRootTypes.Select(a => a.DocumentRootTypeTitle).FirstOrDefault();

                    }
                    #endregion

                    #region DocumentOwnerShipTypes

                    var documentOwnershipTypes = teniacoApiDb.DocumentOwnershipTypes.Where(a => a.DocumentOwnershipTypeId == advertisementVM.DocumentOwnershipTypeId);

                    if (documentOwnershipTypes != null)
                    {
                        advertisementVM.DocumentOwnershipTypeTitle = documentOwnershipTypes.Select(a => a.DocumentOwnershipTypeTitle).FirstOrDefault();

                    }
                    #endregion

                    #region DocumentTypes

                    var documentTypes = teniacoApiDb.DocumentTypes.Where(a => a.DocumentTypeId == advertisementVM.DocumentTypeId).ToList();

                    if (documentTypes != null)
                    {
                        advertisementVM.DocumentTypeTitle = documentTypes.Select(a => a.DocumentTypeTitle).FirstOrDefault();

                    }

                    #endregion

                    #region PropertyTypes

                    var propertyTypes = teniacoApiDb.PropertyTypes.Where(a => a.PropertyTypeId == advertisementVM.PropertyTypeId).ToList();
                    if (propertyTypes != null)
                    {

                        advertisementVM.PropertyTypeTilte = propertyTypes.Select(a => a.PropertyTypeTilte).FirstOrDefault();
                    }
                    #endregion


                    #endregion

                    #region PropertyAddress

                    if (teniacoApiDb.PropertyAddress
                        .Where(e => e.PropertyId.Equals(advertisementId)).Any())
                    {
                        advertisementVM.AdvertisementAddressVM = teniacoApiDb.PropertyAddress
                            .Where(e => e.PropertyId.Equals(advertisementId)).Select(a => new AdvertisementAddressVM
                            {
                                Address = a.Address,
                                StateId = a.StateId,
                                CityId = a.CityId,
                                ZoneId = a.ZoneId,
                                DistrictId = a.DistrictId,
                                TownName = a.TownName,
                                AdvertisementId = a.PropertyId,
                                LocationLat = a.LocationLat,
                                LocationLon = a.LocationLon,
                                UserIdCreator = a.UserIdCreator,
                                CreateEnDate = a.CreateEnDate,
                                CreateTime = a.CreateTime,
                                EditEnDate = a.EditEnDate,
                                EditTime = a.EditTime,
                                UserIdEditor = a.UserIdEditor.Value,
                                RemoveEnDate = a.RemoveEnDate,
                                RemoveTime = a.EditTime,
                                UserIdRemover = a.UserIdRemover.Value,
                                IsActivated = a.IsActivated,
                                IsDeleted = a.IsDeleted,
                            }).FirstOrDefault();
                    }


                    //States


                    var state = publicApiDb.States.Where(p => p.StateId.Equals(advertisementVM.AdvertisementAddressVM.StateId)).FirstOrDefault();

                    if (state != null)
                    {
                        advertisementVM.AdvertisementAddressVM.StateId = state.StateId;
                        advertisementVM.AdvertisementAddressVM.StateName = state.StateName;
                    }


                    //Cities

                    var city = publicApiDb.Cities.Where(p => p.CityId.Equals(advertisementVM.AdvertisementAddressVM.CityId)).FirstOrDefault();

                    if (city != null)
                    {
                        advertisementVM.AdvertisementAddressVM.CityId = city.CityId;
                        advertisementVM.AdvertisementAddressVM.CityName = city.CityName;
                    }



                    //Zones

                    var zone = publicApiDb.Zones.Where(p => p.ZoneId.Equals(advertisementVM.AdvertisementAddressVM.ZoneId)).FirstOrDefault();

                    if (zone != null)
                    {
                        advertisementVM.AdvertisementAddressVM.ZoneId = zone.ZoneId;
                        advertisementVM.AdvertisementAddressVM.ZoneName = zone.ZoneName;
                    }

                    #endregion

                    #region PropertySelectedCallerVM

                    #region old code
                    //if (consoleBusiness.CmsDb.Users.Where(f => f.UserId.Equals(advertisementVM.UserIdCreator)).Any())
                    //{

                    //    var usersProfile = consoleBusiness.CmsDb.UsersProfile.Where(u => u.UserId.Equals(advertisementVM.UserIdCreator)).ToList();


                    //    advertisementVM.AdvertisementOwnersVM = new AdvertisementOwnersVM
                    //    {
                    //        UsersProfileVM = usersProfile
                    //        .Select(a => new UsersProfileVM
                    //        {
                    //            Mobile = a.Mobile,
                    //        }).FirstOrDefault()
                    //    };
                    //}
                    #endregion

                    if (consoleBusiness.CmsDb.Users.Where(f => f.UserId.Equals(advertisementVM.UserIdCreator)).Any())
                    {

                        var selectedPropertyCaller = teniacoApiDb.PropertySelectedCallers
                            .Where(s => s.PropertyId == advertisementId).FirstOrDefault();

                        if (selectedPropertyCaller != null)
                        {
                            advertisementVM.AdvertisementSelectedCallersVM = new AdvertisementSelectedCallersVM()
                            {
                                CallerType = selectedPropertyCaller.CallerType,
                                AdvertiserNumber = selectedPropertyCaller.AdvertiserNumber,
                                AgencyNumber = selectedPropertyCaller.AgencyNumber
                            };
                        }

                    }

                    #endregion

                    #region PropertiesDetails

                    if (teniacoApiDb.PropertiesDetails
                        .Where(e => e.PropertyId.Equals(advertisementId)).Any())
                    {

                        advertisementVM.AdvertisementDetailsVM = teniacoApiDb.PropertiesDetails
                            .Where(e => e.PropertyId.Equals(advertisementId)).Select(a => new AdvertisementDetailsVM
                            {
                                AdvertisementId = a.PropertyId,
                                AdvertisementDetailsId = a.PropertiesDetailsId,
                                AdvertisementTypeId = a.AdvertisementTypeId,
                                Convertable = a.Convertable,
                                MaritalStatusId = a.MaritalStatusId,
                                VerifyAdvertisement = a.VerifyAdvertisement,
                                ShowInSpecialSuggestion = a.ShowInSpecialSuggestion,
                                BuildingLifeId = a.BuildingLifeId,
                                Foundation = a.Foundation,
                                UserIdCreator = a.UserIdCreator,
                                CreateEnDate = a.CreateEnDate,
                                CreateTime = a.CreateTime,
                                EditEnDate = a.EditEnDate,
                                EditTime = a.EditTime,
                                UserIdEditor = a.UserIdEditor.Value,
                                RemoveEnDate = a.RemoveEnDate,
                                RemoveTime = a.EditTime,
                                UserIdRemover = a.UserIdRemover.Value,
                                IsActivated = a.IsActivated,
                                IsDeleted = a.IsDeleted,
                                TagId = a.TagId,
                            }).FirstOrDefault();
                    }

                    if (advertisementVM.AdvertisementDetailsVM != null)
                    {
                        var buildingLife = melkavanApiDb.BuildingLifes.Where(c => c.BuildingLifeId.Equals(advertisementVM.AdvertisementDetailsVM.BuildingLifeId)).FirstOrDefault();
                        if (buildingLife != null)
                        {

                            advertisementVM.AdvertisementDetailsVM.BuildingLifesVM = new BuildingLifesVM
                            {
                                BuildingLifeTitle = buildingLife.BuildingLifeTitle,
                            };

                        }
                    }




                    #endregion

                    #region PropertiesFiles

                    advertisementVM.AdvertisementFilesVM = new List<AdvertisementFilesVM>();
                    if (teniacoApiDb.PropertyFiles
                       .Where(h => h.PropertyId.Equals(advertisementId)).Any())
                    {
                        advertisementVM.AdvertisementFilesVM.AddRange(teniacoApiDb.PropertyFiles
                       .Where(h => h.PropertyId.Equals(advertisementId) && h.PropertyFileType.Equals("Media")).Select(a => new AdvertisementFilesVM
                       {
                           AdvertisementId = a.PropertyId,
                           AdvertisementFileExt = a.PropertyFileExt,
                           AdvertisementFileId = a.PropertyFileId,
                           AdvertisementFileOrder = a.PropertyFileOrder,
                           AdvertisementFilePath = a.PropertyFilePath,
                           AdvertisementFileTitle = a.PropertyFileTitle,
                           AdvertisementFileType = a.PropertyFileType,
                           UserIdCreator = a.UserIdCreator,
                           CreateEnDate = a.CreateEnDate,
                           CreateTime = a.CreateTime,
                           EditEnDate = a.EditEnDate,
                           EditTime = a.EditTime,
                           UserIdEditor = a.UserIdEditor.Value,
                           RemoveEnDate = a.RemoveEnDate,
                           RemoveTime = a.EditTime,
                           UserIdRemover = a.UserIdRemover.Value,
                           IsActivated = a.IsActivated,
                           IsDeleted = a.IsDeleted,

                       }).OrderByDescending(a => a.AdvertisementFileId).ToList());
                    }
                    #endregion

                    #region PropertyPriceHistories

                    advertisementVM.AdvertisementPricesHistoriesVM = new AdvertisementPricesHistoriesVM();
                    if (teniacoApiDb.PropertiesPricesHistories
                       .Where(h => h.PropertyId.Equals(advertisementId)).Any())
                    {
                        advertisementVM.AdvertisementPricesHistoriesVM = (teniacoApiDb.PropertiesPricesHistories
                       .Where(h => h.PropertyId.Equals(advertisementId)).OrderByDescending(c => c.CreateEnDate)
                        .ThenByDescending(c => c.CreateTime).Select(a => new AdvertisementPricesHistoriesVM
                        {
                            AdvertisementId = a.PropertyId,
                            AdvertisementPriceHistoryId = a.PropertyPriceHistoryId,
                            CalculatedOfferPrice = a.CalculatedOfferPrice,
                            OfferPrice = a.OfferPrice,
                            RentPrice = a.RentPrice,
                            DepositPrice = a.DepositPrice,
                            OfferPriceType = a.OfferPriceType,
                            UserIdCreator = a.UserIdCreator,
                            CreateEnDate = a.CreateEnDate,
                            CreateTime = a.CreateTime,
                            EditEnDate = a.EditEnDate,
                            EditTime = a.EditTime,
                            UserIdEditor = a.UserIdEditor.Value,
                            RemoveEnDate = a.RemoveEnDate,
                            RemoveTime = a.EditTime,
                            UserIdRemover = a.UserIdRemover.Value,
                            IsActivated = a.IsActivated,
                            IsDeleted = a.IsDeleted,

                        }).FirstOrDefault());
                    }
                    #endregion

                    #region PropertyFavorites

                    if (userId.HasValue && userId.Value > 0)
                    {
                        if (teniacoApiDb.PropertiesFavorites
                            .Where(a => a.UserIdCreator == userId && a.PropertyId == advertisementVM.AdvertisementId && a.IsDeleted != true).Any())
                        {
                            advertisementVM.IsFavorite = true;
                        }
                    }
                    #endregion

                    #region PropertyFeaturesValues


                    try
                    {

                        if (teniacoApiDb.FeaturesValues.Where(a => a.PropertyId == advertisementVM.AdvertisementId).Any())
                        {
                            advertisementVM.AdvertisementFeaturesValuesVM = new List<AdvertisementFeaturesValuesVM>();
                            var advertisementFeaturesValues = teniacoApiDb.FeaturesValues.Where(a => a.PropertyId == advertisementVM.AdvertisementId).ToList();

                            var featureIds = advertisementFeaturesValues.Select(c => c.FeatureId).ToList();
                            var features = teniacoApiDb.Features.Where(a => featureIds.Contains(a.FeatureId)).ToList();

                            var featureOptions = teniacoApiDb.FeaturesOptions.Where(a => featureIds.Contains(a.FeatureId)).ToList();


                            foreach (var item in advertisementFeaturesValues)
                            {
                                var advertisementFeaturesValuesVM = new AdvertisementFeaturesValuesVM();
                                var feature = _mapper.Map<Features, FeaturesVM>(features.Where(a => a.FeatureId == item.FeatureId).FirstOrDefault());
                                if (featureOptions.Where(c => c.FeatureId.Equals(item.FeatureId)).Any())
                                {
                                    var options = featureOptions.Where(c => c.FeatureId.Equals(item.FeatureId)).ToList();
                                    feature.FeaturesOptionsVM = _mapper.Map<List<FeaturesOptions>, List<FeaturesOptionsVM>>
                                   (options);
                                }

                                var featureValue = "";
                                switch (feature.ElementTypeId)
                                {
                                    case 1:
                                    case 4:
                                        featureValue += item.FeatureValue;
                                        break;
                                    case 2:
                                        if (!string.IsNullOrEmpty(item.FeatureValue))
                                        {
                                            featureValue += feature.FeaturesOptionsVM.Where(a => a.FeatureOptionValue == int.Parse(item.FeatureValue)).Select(a => a.FeatureOptionText).FirstOrDefault();
                                            featureValue += ';';
                                        }
                                        break;
                                    case 3:
                                        if (!string.IsNullOrEmpty(item.FeatureValue))
                                        {
                                            var selectedOptionValues = item.FeatureValue.Split(',');
                                            foreach (var selectedOptionValue in selectedOptionValues)
                                            {
                                                if (!string.IsNullOrEmpty(selectedOptionValue))
                                                {
                                                    featureValue += feature.FeaturesOptionsVM.Where(a => a.FeatureOptionValue == int.Parse(selectedOptionValue)).Select(a => a.FeatureOptionText).FirstOrDefault();
                                                    featureValue += ';';
                                                }
                                            }
                                        }
                                        break;
                                }

                                // این کد بدون توجه به عواقب کامنت شده و به جای آن کد پایینی نوشته شده است برای عملکرد صحیح امکانات اضافی در صفحه ویرایش آگهی
                                //advertisementFeaturesValuesVM.FeatureValue = featureValue;
                                advertisementFeaturesValuesVM.FeatureValue = item.FeatureValue;

                                advertisementFeaturesValuesVM.FeaturesVMList.Add(new FeaturesVM
                                {
                                    FeatureTitle = feature.FeatureTitle,
                                    FeatureId = feature.FeatureId,
                                });


                                advertisementVM.AdvertisementFeaturesValuesVM.Add(advertisementFeaturesValuesVM);
                            }

                        }
                    }
                    catch (Exception exc)
                    { }


                    #endregion

                    advertisementVM.RecordType = "Properties";


                }
                else if (type.Equals("Advertisement"))
                {
                    #region AdvertisementVM

                    advertisementVM = melkavanApiDb.Advertisement
                       .Where(e => e.AdvertisementId.Equals(advertisementId)).Select(a => new AdvertisementVM
                       {
                           PublishType = a.PublishType,
                           ConsultantUserId = a.ConsultantUserId,
                           OwnerId = a.AdvertisementOwners.Where(ao => ao.AdvertisementId.Equals(advertisementId)).Select(ao => ao.OwnerId).FirstOrDefault(),
                           AdvertisementDescriptions = a.AdvertisementDescriptions,
                           AdvertisementId = a.AdvertisementId,
                           AdvertisementTitle = a.AdvertisementTitle,
                           Area = a.Area,
                           BuiltInYear = a.BuiltInYear,
                           BuiltInYearFa = a.BuiltInYearFa,
                           PropertyTypeId = a.PropertyTypeId,
                           TypeOfUseId = a.TypeOfUseId,
                           DocumentOwnershipTypeId = a.DocumentOwnershipTypeId,
                           DocumentRootTypeId = a.DocumentRootTypeId,
                           DocumentTypeId = a.DocumentTypeId,
                           StatusId = a.StatusId,
                           RejectionReason = a.RejectionReason,
                           //IntermediaryPersonId = a.IntermediaryPersonId,
                           RebuiltInYear = a.RebuiltInYear,
                           RebuiltInYearFa = a.RebuiltInYearFa,
                           CurrentDate = DateTime.Now,
                           UserIdCreator = a.UserIdCreator,
                           CreateEnDate = a.CreateEnDate,
                           CreateTime = a.CreateTime,
                           EditEnDate = a.EditEnDate,
                           EditTime = a.EditTime,
                           UserIdEditor = a.UserIdEditor.Value,
                           RemoveEnDate = a.RemoveEnDate,
                           RemoveTime = a.EditTime,
                           UserIdRemover = a.UserIdRemover.Value,
                           IsActivated = a.IsActivated,
                           IsDeleted = a.IsDeleted,
                       }).FirstOrDefault();


                    #region TypeOfUses

                    var typeOfUses = teniacoApiDb.TypeOfUses.Where(a => a.TypeOfUseId.Equals(advertisementVM.TypeOfUseId)).ToList();

                    if (typeOfUses != null)
                    {
                        advertisementVM.TypeUsesVM = typeOfUses.Select(a => new TypeOfUsesVM
                        {
                            TypeOfUseTitle = a.TypeOfUseTitle,

                        }).FirstOrDefault();
                        advertisementVM.TypeUseTitle = advertisementVM.TypeUsesVM.TypeOfUseTitle;
                    }

                    #endregion

                    #region DocumentRootTypes

                    var documentRootTypes = teniacoApiDb.DocumentRootTypes.Where(a => a.DocumentRootTypeId == advertisementVM.DocumentRootTypeId).ToList();

                    if (documentRootTypes != null)
                    {
                        advertisementVM.DocumentRootTypeTitle = documentRootTypes.Select(a => a.DocumentRootTypeTitle).FirstOrDefault();

                    }
                    #endregion

                    #region DocumentOwnerShipTypes

                    var documentOwnershipTypes = teniacoApiDb.DocumentOwnershipTypes.Where(a => a.DocumentOwnershipTypeId == advertisementVM.DocumentOwnershipTypeId);

                    if (documentOwnershipTypes != null)
                    {
                        advertisementVM.DocumentOwnershipTypeTitle = documentOwnershipTypes.Select(a => a.DocumentOwnershipTypeTitle).FirstOrDefault();

                    }
                    #endregion

                    #region DocumentTypes

                    var documentTypes = teniacoApiDb.DocumentTypes.Where(a => a.DocumentTypeId == advertisementVM.DocumentTypeId).ToList();

                    if (documentTypes != null)
                    {
                        advertisementVM.DocumentTypeTitle = documentTypes.Select(a => a.DocumentTypeTitle).FirstOrDefault();

                    }

                    #endregion

                    #region PropertyTypes

                    var propertyTypes = teniacoApiDb.PropertyTypes.Where(a => a.PropertyTypeId == advertisementVM.PropertyTypeId).ToList();
                    if (propertyTypes != null)
                    {

                        advertisementVM.PropertyTypeTilte = propertyTypes.Select(a => a.PropertyTypeTilte).FirstOrDefault();
                    }
                    #endregion

                    #endregion

                    #region AdvertisementAddressVM

                    if (melkavanApiDb.AdvertisementAddress
                        .Where(e => e.AdvertisementId.Equals(advertisementId)).Any())
                    {
                        advertisementVM.AdvertisementAddressVM = melkavanApiDb.AdvertisementAddress
                            .Where(e => e.AdvertisementId.Equals(advertisementId)).Select(a => new AdvertisementAddressVM
                            {
                                Address = a.Address,
                                StateId = a.StateId,
                                CityId = a.CityId,
                                ZoneId = a.ZoneId,
                                DistrictId = a.DistrictId,
                                TownName = a.TownName,
                                AdvertisementId = a.AdvertisementId,
                                LocationLat = a.LocationLat,
                                LocationLon = a.LocationLon,
                                UserIdCreator = a.UserIdCreator,
                                CreateEnDate = a.CreateEnDate,
                                CreateTime = a.CreateTime,
                                EditEnDate = a.EditEnDate,
                                EditTime = a.EditTime,
                                UserIdEditor = a.UserIdEditor.Value,
                                RemoveEnDate = a.RemoveEnDate,
                                RemoveTime = a.EditTime,
                                UserIdRemover = a.UserIdRemover.Value,
                                IsActivated = a.IsActivated,
                                IsDeleted = a.IsDeleted,
                            }).FirstOrDefault();
                    }


                    //States


                    var state = publicApiDb.States.Where(p => p.StateId.Equals(advertisementVM.AdvertisementAddressVM.StateId)).FirstOrDefault();

                    if (state != null)
                    {
                        advertisementVM.AdvertisementAddressVM.StateId = state.StateId;
                        advertisementVM.AdvertisementAddressVM.StateName = state.StateName;
                    }


                    //Cities

                    var city = publicApiDb.Cities.Where(p => p.CityId.Equals(advertisementVM.AdvertisementAddressVM.CityId)).FirstOrDefault();

                    if (city != null)
                    {
                        advertisementVM.AdvertisementAddressVM.CityId = city.CityId;
                        advertisementVM.AdvertisementAddressVM.CityName = city.CityName;
                    }



                    //Zones

                    var zone = publicApiDb.Zones.Where(p => p.ZoneId.Equals(advertisementVM.AdvertisementAddressVM.ZoneId)).FirstOrDefault();

                    if (zone != null)
                    {
                        advertisementVM.AdvertisementAddressVM.ZoneId = zone.ZoneId;
                        advertisementVM.AdvertisementAddressVM.ZoneName = zone.ZoneName;
                    }


                    //districts
                    //var districtIds = publicApiDb.Districts.Select(s => s.DistrictId).ToList();
                    //var districts = publicApiDb.Districts.Where(p => districtIds.Contains(p.DistrictId)).ToList();

                    //if (publicApiDb.Districts.Where(c => c.DistrictId.Equals(advertisementVM.AdvertisementAddressVM.DistrictId)).Any())
                    //{
                    //    var district = publicApiDb.Districts.Where(c => c.DistrictId.Equals(advertisementVM.AdvertisementAddressVM.DistrictId)).FirstOrDefault();
                    //    advertisementVM.AdvertisementAddressVM.DistrictId = district.DistrictId;
                    //    advertisementVM.AdvertisementAddressVM.DistrictName = district.DistrictName;
                    //}


                    #endregion

                    #region AdvertisementSelectedCallerVM 

                    #region old code


                    //if (publicApiDb.Persons.Where(a => a.UserIdCreator == advertisementVM.UserIdCreator).Any())
                    //{

                    //    var persons = publicApiDb.Persons.Where(p => p.UserIdCreator.Equals(advertisementVM.UserIdCreator)).ToList();


                    //    advertisementVM.AdvertisementOwnerPersonsVM = new AdvertisementOwnerPersonsVM
                    //    {
                    //        OwnerPersons = persons
                    //        .Select(a => new PersonsVM
                    //        {
                    //            //Name = a.Name,
                    //            //Family = a.Family,
                    //            Mobail = a.Mobail,
                    //        }).FirstOrDefault()
                    //    };
                    //}

                    #endregion


                    #region old code 2
                    //if (consoleBusiness.CmsDb.Users.Where(f => f.UserId.Equals(advertisementVM.UserIdCreator)).Any())
                    //{
                    //    //var users = consoleBusiness.CmsDb.Users.Where(u => u.UserId.Equals(advertisementVM.UserIdCreator)).ToList();

                    //    var usersProfile = consoleBusiness.CmsDb.UsersProfile.Where(u => u.UserId.Equals(advertisementVM.UserIdCreator)).ToList();


                    //    advertisementVM.AdvertisementOwnersVM = new AdvertisementOwnersVM
                    //    {
                    //        UsersProfileVM = usersProfile
                    //        .Select(a => new UsersProfileVM
                    //        {
                    //            Mobile = a.Mobile,
                    //        }).FirstOrDefault()
                    //    };
                    //}
                    #endregion


                    if (consoleBusiness.CmsDb.Users.Where(f => f.UserId.Equals(advertisementVM.UserIdCreator)).Any())
                    {

                        var selectedAdvertisementCaller = melkavanApiDb.AdvertisementSelectedCallers
                            .Where(s => s.AdvertisementId == advertisementId).FirstOrDefault();

                        if (selectedAdvertisementCaller != null)
                        {
                            advertisementVM.AdvertisementSelectedCallersVM = new AdvertisementSelectedCallersVM()
                            {
                                CallerType = selectedAdvertisementCaller.CallerType,
                                AdvertiserNumber = selectedAdvertisementCaller.AdvertiserNumber,
                                AgencyNumber = selectedAdvertisementCaller.AgencyNumber
                            };
                        }

                    }

                    #endregion

                    #region AdvertisementDetails

                    if (melkavanApiDb.AdvertisementDetails
                        .Where(e => e.AdvertisementId.Equals(advertisementId)).Any())
                    {

                        advertisementVM.AdvertisementDetailsVM = melkavanApiDb.AdvertisementDetails
                            .Where(e => e.AdvertisementId.Equals(advertisementId)).Select(a => new AdvertisementDetailsVM
                            {
                                AdvertisementId = a.AdvertisementId,
                                AdvertisementDetailsId = a.AdvertisementDetailsId,
                                AdvertisementTypeId = a.AdvertisementTypeId,
                                Convertable = a.Convertable,
                                MaritalStatusId = a.MaritalStatusId,
                                VerifyAdvertisement = a.VerifyAdvertisement,
                                ShowInSpecialSuggestion = a.ShowInSpecialSuggestion,
                                BuildingLifeId = a.BuildingLifeId,
                                Foundation = a.Foundation,
                                UserIdCreator = a.UserIdCreator,
                                CreateEnDate = a.CreateEnDate,
                                CreateTime = a.CreateTime,
                                EditEnDate = a.EditEnDate,
                                EditTime = a.EditTime,
                                UserIdEditor = a.UserIdEditor.Value,
                                RemoveEnDate = a.RemoveEnDate,
                                RemoveTime = a.EditTime,
                                UserIdRemover = a.UserIdRemover.Value,
                                IsActivated = a.IsActivated,
                                IsDeleted = a.IsDeleted,
                                TagId = a.TagId,
                                InstagramLink = a.InstagramLink
                            }).FirstOrDefault();
                    }



                    var buildingLife = melkavanApiDb.BuildingLifes.Where(c => c.BuildingLifeId.Equals(advertisementVM.AdvertisementDetailsVM.BuildingLifeId)).FirstOrDefault();
                    if (buildingLife != null)
                    {

                        advertisementVM.AdvertisementDetailsVM.BuildingLifesVM = new BuildingLifesVM
                        {
                            BuildingLifeTitle = buildingLife.BuildingLifeTitle,
                        };


                        //advertisementVM.AdvertisementDetailsVM.BuildingLifesVM.BuildingLifeTitle = buildingLife.BuildingLifeTitle;
                    }


                    #endregion

                    #region AdvertisementFilesVM 

                    advertisementVM.AdvertisementFilesVM = new List<AdvertisementFilesVM>();
                    if (melkavanApiDb.AdvertisementFiles
                       .Where(h => h.AdvertisementId.Equals(advertisementId)).Any())
                    {
                        advertisementVM.AdvertisementFilesVM.AddRange(melkavanApiDb.AdvertisementFiles
                       .Where(h => h.AdvertisementId.Equals(advertisementId) && h.AdvertisementFileType.Equals("Media")).Select(a => new AdvertisementFilesVM
                       {
                           AdvertisementId = a.AdvertisementId,
                           AdvertisementFileExt = a.AdvertisementFileExt,
                           AdvertisementFileId = a.AdvertisementFileId,
                           AdvertisementFileOrder = a.AdvertisementFileOrder,
                           AdvertisementFilePath = a.AdvertisementFilePath,
                           AdvertisementFileTitle = a.AdvertisementFileTitle,
                           AdvertisementFileType = a.AdvertisementFileType,

                           UserIdCreator = a.UserIdCreator,
                           CreateEnDate = a.CreateEnDate,
                           CreateTime = a.CreateTime,
                           EditEnDate = a.EditEnDate,
                           EditTime = a.EditTime,
                           UserIdEditor = a.UserIdEditor.Value,
                           RemoveEnDate = a.RemoveEnDate,
                           RemoveTime = a.EditTime,
                           UserIdRemover = a.UserIdRemover.Value,
                           IsActivated = a.IsActivated,
                           IsDeleted = a.IsDeleted,

                       }).OrderByDescending(a => a.AdvertisementFileId).ToList());
                    }
                    #endregion

                    #region AdvertisementPricesHistoriesVM 

                    advertisementVM.AdvertisementPricesHistoriesVM = new AdvertisementPricesHistoriesVM();
                    if (melkavanApiDb.AdvertisementPricesHistories
                       .Where(h => h.AdvertisementId.Equals(advertisementId)).Any())
                    {
                        advertisementVM.AdvertisementPricesHistoriesVM = (melkavanApiDb.AdvertisementPricesHistories
                       .Where(h => h.AdvertisementId.Equals(advertisementId)).OrderByDescending(c => c.CreateEnDate)
                        .ThenByDescending(c => c.CreateTime).Select(a => new AdvertisementPricesHistoriesVM
                        {
                            AdvertisementId = a.AdvertisementId,
                            AdvertisementPriceHistoryId = a.AdvertisementPriceHistoryId,
                            CalculatedOfferPrice = a.CalculatedOfferPrice,
                            OfferPrice = a.OfferPrice,
                            RentPrice = a.RentPrice,
                            DepositPrice = a.DepositPrice,
                            OfferPriceType = a.OfferPriceType,
                            UserIdCreator = a.UserIdCreator,
                            CreateEnDate = a.CreateEnDate,
                            CreateTime = a.CreateTime,
                            EditEnDate = a.EditEnDate,
                            EditTime = a.EditTime,
                            UserIdEditor = a.UserIdEditor.Value,
                            RemoveEnDate = a.RemoveEnDate,
                            RemoveTime = a.EditTime,
                            UserIdRemover = a.UserIdRemover.Value,
                            IsActivated = a.IsActivated,
                            IsDeleted = a.IsDeleted,

                        }).FirstOrDefault());
                    }
                    #endregion

                    #region AdvertisementFavorites

                    if (userId.HasValue && userId.Value > 0)
                    {
                        if (melkavanApiDb.AdvertisementFavorites
                            .Where(a => a.UserIdCreator == userId && a.AdvertisementId == advertisementVM.AdvertisementId && a.IsDeleted != true).Any())
                        {
                            advertisementVM.IsFavorite = true;
                        }
                    }
                    #endregion

                    #region AdvertisementFeaturesValues


                    try
                    {

                        if (melkavanApiDb.AdvertisementFeaturesValues.Where(a => a.AdvertisementId == advertisementVM.AdvertisementId).Any())
                        {
                            advertisementVM.AdvertisementFeaturesValuesVM = new List<AdvertisementFeaturesValuesVM>();
                            var advertisementFeaturesValues = melkavanApiDb.AdvertisementFeaturesValues.Where(a => a.AdvertisementId == advertisementVM.AdvertisementId).ToList();

                            var featureIds = advertisementFeaturesValues.Select(c => c.FeatureId).ToList();
                            var features = teniacoApiDb.Features.Where(a => featureIds.Contains(a.FeatureId)).ToList();

                            var featureOptions = teniacoApiDb.FeaturesOptions.Where(a => featureIds.Contains(a.FeatureId)).ToList();


                            //advertisementFeaturesValuesVM.FeaturesVMList = new List<FeaturesVM>();

                            foreach (var item in advertisementFeaturesValues)
                            {
                                var advertisementFeaturesValuesVM = new AdvertisementFeaturesValuesVM();
                                var feature = _mapper.Map<Features, FeaturesVM>(features.Where(a => a.FeatureId == item.FeatureId).FirstOrDefault());
                                if (featureOptions.Where(c => c.FeatureId.Equals(item.FeatureId)).Any())
                                {
                                    var options = featureOptions.Where(c => c.FeatureId.Equals(item.FeatureId)).ToList();
                                    feature.FeaturesOptionsVM = _mapper.Map<List<FeaturesOptions>, List<FeaturesOptionsVM>>
                                   (options);
                                }


                                //feature.FeaturesOptionsVM = _mapper.Map<List<FeaturesOptions>, List<FeaturesOptionsVM>>
                                //    (featureOptions);

                                var featureValue = "";
                                switch (feature.ElementTypeId)
                                {
                                    case 1:
                                    case 4:
                                        featureValue += item.FeatureValue;
                                        break;
                                    case 2:
                                        if (!string.IsNullOrEmpty(item.FeatureValue))
                                        {
                                            featureValue += feature.FeaturesOptionsVM.Where(a => a.FeatureOptionValue == int.Parse(item.FeatureValue)).Select(a => a.FeatureOptionText).FirstOrDefault();
                                            featureValue += ';';
                                        }
                                        break;
                                    case 3:
                                        var selectedOptionValues = item.FeatureValue.Split(',');
                                        foreach (var selectedOptionValue in selectedOptionValues)
                                        {
                                            if (!string.IsNullOrEmpty(selectedOptionValue))
                                            {
                                                featureValue += feature.FeaturesOptionsVM.Where(a => a.FeatureOptionValue == int.Parse(selectedOptionValue)).Select(a => a.FeatureOptionText).FirstOrDefault();
                                                featureValue += ';';
                                            }
                                        }
                                        break;
                                }

                                // این کد بدون توجه به عواقب کامنت شده و به جای آن کد پایینی نوشته شده است برای عملکرد صحیح امکانات اضافی در صفحه ویرایش آگهی
                                // advertisementFeaturesValuesVM.FeatureValue = featureValue;
                                advertisementFeaturesValuesVM.FeatureValue = item.FeatureValue;

                                advertisementFeaturesValuesVM.FeaturesVMList.Add(new FeaturesVM
                                {
                                    FeatureTitle = feature.FeatureTitle,
                                    FeatureId = feature.FeatureId,
                                });


                                advertisementVM.AdvertisementFeaturesValuesVM.Add(advertisementFeaturesValuesVM);
                            }

                        }
                    }
                    catch (Exception exc)
                    {
                        //advertisementVM.AdvertisementFeaturesValuesVM = null;
                    }


                    #endregion


                    advertisementVM.RecordType = "Advertisement";
                }




            }
            catch (Exception exc)
            { }

            return advertisementVM;
        }




        public long UpdateAdvertisement(
              ref AdvertisementVM AdvertisementVM,
              List<long> childsUsersIds,
              TeniacoApiContext teniacoApiDb)
        {
            long AdvertisementId = AdvertisementVM.AdvertisementId;



            if (AdvertisementVM.RecordType == "Advertisement")
            {
                if (melkavanApiDb.Advertisement.Where(n => childsUsersIds.Contains(n.UserIdCreator.Value))
                     .Where(x => x.AdvertisementId.Equals(AdvertisementId)).Any())
                {
                    using (var transaction = melkavanApiDb.Database.BeginTransaction())
                    {
                        try
                        {

                            #region UpdateAdvertisement
                            Advertisement Advertisement = (from c in melkavanApiDb.Advertisement
                                                           where c.AdvertisementId == AdvertisementId
                                                           select c).FirstOrDefault();

                            Advertisement.PropertyTypeId = AdvertisementVM.PropertyTypeId;
                            Advertisement.AdvertisementTitle = AdvertisementVM.AdvertisementTitle;
                            Advertisement.TypeOfUseId = AdvertisementVM.TypeOfUseId.HasValue ? AdvertisementVM.TypeOfUseId.Value : (int?)0;
                            Advertisement.DocumentOwnershipTypeId = AdvertisementVM.DocumentOwnershipTypeId.HasValue ? AdvertisementVM.DocumentOwnershipTypeId.Value : (int?)0;
                            Advertisement.DocumentRootTypeId = AdvertisementVM.DocumentRootTypeId.HasValue ? AdvertisementVM.DocumentRootTypeId.Value : (int?)0;
                            Advertisement.DocumentTypeId = AdvertisementVM.DocumentTypeId.HasValue ? AdvertisementVM.DocumentTypeId.Value : (int?)0;
                            Advertisement.Area = AdvertisementVM.Area;
                            Advertisement.RebuiltInYearFa = AdvertisementVM.RebuiltInYearFa;

                            if (AdvertisementVM.PublishType == "Quick")
                            {
                                Advertisement.ConsultantUserId = AdvertisementVM.ConsultantUserId;
                            }

                            Advertisement.EditEnDate = DateTime.Now;
                            Advertisement.EditTime = PersianDate.TimeNow;
                            Advertisement.UserIdEditor = AdvertisementVM.UserIdEditor.Value;
                            Advertisement.IsActivated = Advertisement.IsActivated.HasValue ? Advertisement.IsActivated.Value : (bool?)true;
                            Advertisement.IsDeleted = Advertisement.IsDeleted.HasValue ? Advertisement.IsDeleted.Value : (bool?)false;
                            Advertisement.AdvertisementDescriptions = AdvertisementVM.NewAdvertisementDescriptions;

                            melkavanApiDb.Entry<Advertisement>(Advertisement).State = EntityState.Modified;
                            melkavanApiDb.SaveChanges();


                            #region AdvertisementOwners
                            if (AdvertisementVM.PublishType == "Quick" && AdvertisementVM.OwnerId > 0)
                            {
                                var advertisementOwner = melkavanApiDb.AdvertisementOwners.Where(ao => ao.AdvertisementId == AdvertisementId).FirstOrDefault();
                                if (advertisementOwner != null)
                                {
                                    advertisementOwner.OwnerId = (long)AdvertisementVM.OwnerId;
                                    melkavanApiDb.Entry<AdvertisementOwners>(advertisementOwner).State = EntityState.Modified;
                                    melkavanApiDb.SaveChanges();
                                }
                                else
                                {
                                    advertisementOwner = new AdvertisementOwners();
                                    advertisementOwner.OwnerId = AdvertisementVM.OwnerId.Value;
                                    advertisementOwner.Share = AdvertisementVM.AdvertisementOwnersVM.Share;
                                    advertisementOwner.SharePercent = AdvertisementVM.AdvertisementOwnersVM.SharePercent;
                                    advertisementOwner.OwnerType = AdvertisementVM.AdvertisementOwnersVM.OwnerType;
                                    advertisementOwner.AdvertisementId = AdvertisementId;
                                    advertisementOwner.UserIdCreator = Advertisement.UserIdCreator.Value;
                                    advertisementOwner.CreateEnDate = DateTime.Now;
                                    advertisementOwner.CreateTime = PersianDate.TimeNow;
                                    advertisementOwner.IsActivated = true;
                                    advertisementOwner.IsDeleted = false;

                                    melkavanApiDb.AdvertisementOwners.Add(advertisementOwner);
                                    melkavanApiDb.SaveChanges();
                                }
                            }
                            else if (AdvertisementVM.PublishType == "Quick" && AdvertisementVM.OwnerId == null)
                            {
                                var advertisementOwner = melkavanApiDb.AdvertisementOwners.Where(ao => ao.AdvertisementId == AdvertisementId).FirstOrDefault();
                                if (advertisementOwner != null)
                                {
                                    melkavanApiDb.Entry<AdvertisementOwners>(advertisementOwner).State = EntityState.Deleted;
                                    melkavanApiDb.SaveChanges();
                                }
                            }
                            #endregion


                            #region AdvertisementPricesHistories

                            // ثبت قیمت جدید
                            AdvertisementPricesHistories AdvertisementPricesHistories = new AdvertisementPricesHistories();
                            AdvertisementPricesHistories.DepositPrice = AdvertisementVM.AdvertisementPricesHistoriesVM.DepositPrice;
                            AdvertisementPricesHistories.RentPrice = AdvertisementVM.AdvertisementPricesHistoriesVM.RentPrice;
                            AdvertisementPricesHistories.OfferPrice = AdvertisementVM.AdvertisementPricesHistoriesVM.OfferPrice.HasValue ? AdvertisementVM.AdvertisementPricesHistoriesVM.OfferPrice.Value : 0;
                            AdvertisementPricesHistories.OfferPriceType = AdvertisementVM.AdvertisementPricesHistoriesVM.OfferPriceType.HasValue ? AdvertisementVM.AdvertisementPricesHistoriesVM.OfferPriceType.Value : 1;
                            AdvertisementPricesHistories.CalculatedOfferPrice = AdvertisementVM.AdvertisementPricesHistoriesVM.CalculatedOfferPrice.HasValue ? AdvertisementVM.AdvertisementPricesHistoriesVM.CalculatedOfferPrice.Value : 0;
                            AdvertisementPricesHistories.PriceTypeRegister = AdvertisementVM.PriceTypeRegister;
                            AdvertisementPricesHistories.AdvertisementId = Advertisement.AdvertisementId;

                            AdvertisementPricesHistories.CreateEnDate = DateTime.Now;
                            AdvertisementPricesHistories.CreateTime = PersianDate.TimeNow;

                            AdvertisementPricesHistories.UserIdCreator = Advertisement.UserIdCreator.Value;
                            AdvertisementPricesHistories.IsActivated = true;
                            AdvertisementPricesHistories.IsDeleted = false;

                            melkavanApiDb.AdvertisementPricesHistories.Add(AdvertisementPricesHistories);
                            melkavanApiDb.SaveChanges();

                            //if (AdvertisementVM.PriceTypeRegister == 0)//اصلاح قیمت قبلی
                            //{
                            //    AdvertisementPricesHistories AdvertisementPricesHistories = (
                            //        from h in melkavanApiDb.AdvertisementPricesHistories
                            //        where h.AdvertisementId == AdvertisementId
                            //        select h).OrderByDescending(c => c.CreateEnDate)
                            //             .ThenByDescending(c => c.CreateTime).FirstOrDefault();



                            //    AdvertisementPricesHistories.DepositPrice = AdvertisementVM.AdvertisementPricesHistoriesVM.DepositPrice;
                            //    AdvertisementPricesHistories.RentPrice = AdvertisementVM.AdvertisementPricesHistoriesVM.RentPrice;
                            //    AdvertisementPricesHistories.OfferPrice = AdvertisementVM.OfferPrice;
                            //    AdvertisementPricesHistories.OfferPriceType = AdvertisementVM.OfferPriceType;
                            //    AdvertisementPricesHistories.CalculatedOfferPrice = AdvertisementVM.CalculatedOfferPrice;
                            //    AdvertisementPricesHistories.PriceTypeRegister = AdvertisementVM.PriceTypeRegister;
                            //    AdvertisementPricesHistories.EditEnDate = DateTime.Now;
                            //    AdvertisementPricesHistories.EditTime = PersianDate.TimeNow;
                            //    AdvertisementPricesHistories.IsActivated = true;
                            //    AdvertisementPricesHistories.IsDeleted = false;


                            //    melkavanApiDb.Entry<AdvertisementPricesHistories>(AdvertisementPricesHistories).State = EntityState.Modified;
                            //    melkavanApiDb.SaveChanges();
                            //}
                            //else//ثبت قیمت جدید
                            //{

                            //    AdvertisementPricesHistories AdvertisementPricesHistories = new AdvertisementPricesHistories();
                            //    AdvertisementPricesHistories.DepositPrice = AdvertisementVM.AdvertisementPricesHistoriesVM.DepositPrice;
                            //    AdvertisementPricesHistories.RentPrice = AdvertisementVM.AdvertisementPricesHistoriesVM.RentPrice;
                            //    AdvertisementPricesHistories.OfferPrice = AdvertisementVM.AdvertisementPricesHistoriesVM.OfferPrice.HasValue ? AdvertisementVM.AdvertisementPricesHistoriesVM.OfferPrice.Value : 0;
                            //    AdvertisementPricesHistories.OfferPriceType = AdvertisementVM.AdvertisementPricesHistoriesVM.OfferPriceType.HasValue ? AdvertisementVM.AdvertisementPricesHistoriesVM.OfferPriceType.Value : 0;
                            //    AdvertisementPricesHistories.CalculatedOfferPrice = AdvertisementVM.AdvertisementPricesHistoriesVM.CalculatedOfferPrice.HasValue ? AdvertisementVM.AdvertisementPricesHistoriesVM.CalculatedOfferPrice.Value : 0;
                            //    AdvertisementPricesHistories.PriceTypeRegister = AdvertisementVM.PriceTypeRegister;
                            //    AdvertisementPricesHistories.AdvertisementId = Advertisement.AdvertisementId;

                            //    AdvertisementPricesHistories.CreateEnDate = DateTime.Now;
                            //    AdvertisementPricesHistories.CreateTime = PersianDate.TimeNow;

                            //    AdvertisementPricesHistories.UserIdCreator = Advertisement.UserIdCreator.Value;
                            //    AdvertisementPricesHistories.IsActivated = true;
                            //    AdvertisementPricesHistories.IsDeleted = false;

                            //    melkavanApiDb.AdvertisementPricesHistories.Add(AdvertisementPricesHistories);
                            //    melkavanApiDb.SaveChanges();


                            //}
                            #endregion

                            #region AdvertisementDetails

                            if (AdvertisementVM.AdvertisementDetailsVM != null)
                            {
                                AdvertisementDetails AdvertisementDetails = melkavanApiDb.AdvertisementDetails.Where(a => a.AdvertisementId == AdvertisementId).FirstOrDefault();

                                AdvertisementDetails.AdvertisementId = AdvertisementId;
                                AdvertisementDetails.AdvertisementTypeId = AdvertisementVM.AdvertisementDetailsVM.AdvertisementTypeId;
                                AdvertisementDetails.VerifyAdvertisement = AdvertisementVM.AdvertisementDetailsVM.VerifyAdvertisement;
                                AdvertisementDetails.Convertable = AdvertisementVM.AdvertisementDetailsVM.Convertable;
                                AdvertisementDetails.ShowInSpecialSuggestion = AdvertisementVM.AdvertisementDetailsVM.ShowInSpecialSuggestion;
                                AdvertisementDetails.MaritalStatusId = AdvertisementVM.AdvertisementDetailsVM.MaritalStatusId;
                                AdvertisementDetails.BuildingLifeId = AdvertisementVM.AdvertisementDetailsVM.BuildingLifeId;
                                AdvertisementDetails.Foundation = AdvertisementVM.AdvertisementDetailsVM.Foundation;
                                AdvertisementDetails.TagId = AdvertisementVM.AdvertisementDetailsVM.TagId;
                                AdvertisementDetails.InstagramLink = AdvertisementVM.AdvertisementDetailsVM.InstagramLink;


                                melkavanApiDb.Entry<AdvertisementDetails>(AdvertisementDetails).State = EntityState.Modified;
                                melkavanApiDb.SaveChanges();

                            }
                            else
                            {
                                AdvertisementDetails AdvertisementDetails = new AdvertisementDetails();

                                AdvertisementDetails.AdvertisementId = Advertisement.AdvertisementId;

                                AdvertisementDetails.AdvertisementTypeId = AdvertisementVM.AdvertisementDetailsVM.AdvertisementTypeId.HasValue ? AdvertisementVM.AdvertisementDetailsVM.AdvertisementTypeId.Value : 0;
                                AdvertisementDetails.VerifyAdvertisement = AdvertisementVM.AdvertisementDetailsVM.VerifyAdvertisement;
                                AdvertisementDetails.Convertable = AdvertisementVM.AdvertisementDetailsVM.Convertable;
                                AdvertisementDetails.ShowInSpecialSuggestion = AdvertisementVM.AdvertisementDetailsVM.ShowInSpecialSuggestion;
                                AdvertisementDetails.MaritalStatusId = AdvertisementVM.AdvertisementDetailsVM.MaritalStatusId.HasValue ? AdvertisementVM.AdvertisementDetailsVM.MaritalStatusId.Value : 0;
                                AdvertisementDetails.BuildingLifeId = AdvertisementVM.AdvertisementDetailsVM.BuildingLifeId.HasValue ? AdvertisementVM.AdvertisementDetailsVM.BuildingLifeId.Value : 0;
                                AdvertisementDetails.Foundation = AdvertisementVM.AdvertisementDetailsVM.Foundation;
                                AdvertisementDetails.TagId = AdvertisementVM.AdvertisementDetailsVM.TagId;
                                AdvertisementDetails.InstagramLink = AdvertisementVM.AdvertisementDetailsVM.InstagramLink;

                                AdvertisementDetails.CreateEnDate = DateTime.Now;
                                AdvertisementDetails.CreateTime = PersianDate.TimeNow;

                                AdvertisementDetails.UserIdCreator = Advertisement.UserIdCreator.Value;
                                AdvertisementDetails.IsActivated = true;
                                AdvertisementDetails.IsDeleted = false;


                                melkavanApiDb.AdvertisementDetails.Add(AdvertisementDetails);
                                melkavanApiDb.SaveChanges();
                            }
                            #endregion

                            #region AdvertisementAddress

                            if (AdvertisementVM.AdvertisementAddressVM != null)
                            {

                                AdvertisementAddress AdvertisementAddress = (from ad in melkavanApiDb.AdvertisementAddress
                                                                             where ad.AdvertisementId == AdvertisementId
                                                                             select ad).FirstOrDefault();


                                AdvertisementAddress.AdvertisementId = AdvertisementId;
                                AdvertisementAddress.Address = AdvertisementVM.AdvertisementAddressVM.Address;
                                AdvertisementAddress.StateId = AdvertisementVM.AdvertisementAddressVM.StateId;
                                AdvertisementAddress.TempStateId = AdvertisementVM.AdvertisementAddressVM.TempStateId;
                                AdvertisementAddress.CityId = AdvertisementVM.AdvertisementAddressVM.CityId;
                                AdvertisementAddress.TempCityId = AdvertisementVM.AdvertisementAddressVM.TempCityId;
                                AdvertisementAddress.ZoneId = AdvertisementVM.AdvertisementAddressVM.ZoneId;
                                AdvertisementAddress.TempZoneId = AdvertisementVM.AdvertisementAddressVM.TempZoneId;
                                AdvertisementAddress.DistrictId = AdvertisementVM.AdvertisementAddressVM.DistrictId;
                                AdvertisementAddress.LocationLat = AdvertisementVM.AdvertisementAddressVM.LocationLat;
                                AdvertisementAddress.LocationLon = AdvertisementVM.AdvertisementAddressVM.LocationLon;
                                AdvertisementAddress.TownName = AdvertisementVM.AdvertisementAddressVM.TownName;
                                //AdvertisementAddress.TempDistrictId = AdvertisementVM.AdvertisementAddressVM.TempDistrictId;

                                melkavanApiDb.Entry<AdvertisementAddress>(AdvertisementAddress).State = EntityState.Modified;
                                melkavanApiDb.SaveChanges();

                            }
                            else if (AdvertisementVM.PublishType == "Melkavan" && AdvertisementVM.AdvertisementAddressVM != null)
                            {
                                AdvertisementAddress AdvertisementAddress = new AdvertisementAddress();

                                AdvertisementAddress.AdvertisementId = Advertisement.AdvertisementId;

                                AdvertisementAddress.Address = AdvertisementVM.AdvertisementAddressVM.Address;
                                AdvertisementAddress.StateId = AdvertisementVM.AdvertisementAddressVM.StateId.HasValue ? AdvertisementVM.AdvertisementAddressVM.StateId.Value : 0;
                                AdvertisementAddress.TempStateId = AdvertisementVM.AdvertisementAddressVM.TempStateId.HasValue ? AdvertisementVM.AdvertisementAddressVM.TempStateId.Value : 0;
                                AdvertisementAddress.CityId = AdvertisementVM.AdvertisementAddressVM.CityId.HasValue ? AdvertisementVM.AdvertisementAddressVM.CityId.Value : 0;
                                AdvertisementAddress.TempCityId = AdvertisementVM.AdvertisementAddressVM.TempCityId.HasValue ? AdvertisementVM.AdvertisementAddressVM.TempCityId.Value : 0;
                                AdvertisementAddress.ZoneId = AdvertisementVM.AdvertisementAddressVM.ZoneId.HasValue ? AdvertisementVM.AdvertisementAddressVM.ZoneId.Value : 0;
                                AdvertisementAddress.TempZoneId = AdvertisementVM.AdvertisementAddressVM.TempZoneId.HasValue ? AdvertisementVM.AdvertisementAddressVM.TempZoneId.Value : 0;
                                AdvertisementAddress.DistrictId = AdvertisementVM.AdvertisementAddressVM.DistrictId.HasValue ? AdvertisementVM.AdvertisementAddressVM.DistrictId.Value : 0;
                                AdvertisementAddress.LocationLat = AdvertisementVM.AdvertisementAddressVM.LocationLat;
                                AdvertisementAddress.LocationLon = AdvertisementVM.AdvertisementAddressVM.LocationLon;
                                //AdvertisementAddress.TempDistrictId = AdvertisementVM.AdvertisementAddressVM.TempDistrictId.HasValue ? AdvertisementVM.AdvertisementAddressVM.TempDistrictId.Value : 0;

                                AdvertisementAddress.CreateEnDate = DateTime.Now;
                                AdvertisementAddress.CreateTime = PersianDate.TimeNow;

                                AdvertisementAddress.UserIdCreator = Advertisement.UserIdCreator.Value;
                                AdvertisementAddress.IsActivated = true;
                                AdvertisementAddress.IsDeleted = false;


                                melkavanApiDb.AdvertisementAddress.Add(AdvertisementAddress);
                                melkavanApiDb.SaveChanges();
                            }

                            #endregion

                            #region AdvertisementFiles
                            //for adding files
                            if (AdvertisementVM.AdvertisementFilesVM != null)
                            {
                                if (AdvertisementVM.AdvertisementFilesVM.Count > 0)
                                {

                                    //main photo is last record in files table
                                    var MainFile = melkavanApiDb.AdvertisementFiles
                                        .Where(a => a.AdvertisementId == AdvertisementId)
                                     .OrderByDescending(a => a.AdvertisementFileId)
                                     .FirstOrDefault();

                                    var AdvertisementFilesList = _mapper.Map<List<AdvertisementFilesVM>, List<AdvertisementFiles>>(AdvertisementVM.AdvertisementFilesVM);

                                    foreach (var item in AdvertisementFilesList)
                                    {
                                        item.AdvertisementId = Advertisement.AdvertisementId;
                                        item.UserIdCreator = Advertisement.UserIdCreator.Value;
                                        item.IsActivated = true;
                                        item.IsDeleted = false;
                                    }

                                    melkavanApiDb.AdvertisementFiles.AddRange(AdvertisementFilesList);
                                    melkavanApiDb.SaveChanges();




                                    if (MainFile != null && AdvertisementVM.IsMainChanged == false)
                                    {
                                        melkavanApiDb.AdvertisementFiles.Remove(MainFile);
                                        melkavanApiDb.SaveChanges();

                                        MainFile.AdvertisementFileId = 0;
                                        melkavanApiDb.AdvertisementFiles.Add(MainFile);
                                        melkavanApiDb.SaveChanges();
                                    }
                                }
                            }

                            //for removing files
                            if (AdvertisementVM.DeletedPhotosIDs != null)
                            {
                                foreach (int id in AdvertisementVM.DeletedPhotosIDs)
                                {
                                    var FileToDelete = melkavanApiDb.AdvertisementFiles
                                        .Where(f => f.AdvertisementFileId == id).FirstOrDefault();

                                    melkavanApiDb.AdvertisementFiles.Remove(FileToDelete);
                                    melkavanApiDb.SaveChanges();
                                }
                            }
                            #endregion

                            #region AdvertisementFeatures

                            if (AdvertisementVM.AdvertisementFeaturesValuesVM != null)
                            {

                                if (melkavanApiDb.AdvertisementFeaturesValues.Where(fv => fv.AdvertisementId.Equals(AdvertisementId)).Any())
                                {
                                    var oldFeaturesValues = melkavanApiDb.AdvertisementFeaturesValues.Where(fv => fv.AdvertisementId.Equals(AdvertisementId)).ToList();
                                    melkavanApiDb.AdvertisementFeaturesValues.RemoveRange(oldFeaturesValues);
                                    melkavanApiDb.SaveChanges();
                                }


                                foreach (var item in AdvertisementVM.AdvertisementFeaturesValuesVM)
                                {

                                    if (!item.FeatureValue.IsNullOrEmpty())
                                    {
                                        AdvertisementFeaturesValues advertisementFeaturesValues = melkavanApiDb.AdvertisementFeaturesValues
                                            .Where(a => a.AdvertisementId == AdvertisementId && a.FeatureId == item.FeatureId).FirstOrDefault();

                                        if (advertisementFeaturesValues == null)
                                        {
                                            advertisementFeaturesValues = new AdvertisementFeaturesValues();

                                            advertisementFeaturesValues.AdvertisementId = Advertisement.AdvertisementId;
                                            advertisementFeaturesValues.CreateEnDate = Advertisement.CreateEnDate.Value;
                                            advertisementFeaturesValues.CreateTime = Advertisement.CreateTime;
                                            advertisementFeaturesValues.UserIdCreator = Advertisement.UserIdCreator.Value;
                                            advertisementFeaturesValues.IsActivated = true;
                                            advertisementFeaturesValues.IsDeleted = false;

                                            advertisementFeaturesValues.FeatureId = item.FeatureId;
                                            advertisementFeaturesValues.FeatureValue = item.FeatureValue;
                                            melkavanApiDb.AdvertisementFeaturesValues.Add(advertisementFeaturesValues);
                                            melkavanApiDb.SaveChanges();
                                        }
                                        else
                                        {
                                            advertisementFeaturesValues.UserIdEditor = AdvertisementVM.UserIdEditor;

                                            advertisementFeaturesValues.FeatureId = item.FeatureId;
                                            advertisementFeaturesValues.FeatureValue = item.FeatureValue;
                                            melkavanApiDb.AdvertisementFeaturesValues.Update(advertisementFeaturesValues);
                                            melkavanApiDb.SaveChanges();
                                        }



                                    }

                                }
                            }

                            #endregion

                            #endregion

                            transaction.Commit();

                            AdvertisementVM.UserIdCreator = Advertisement.UserIdCreator.Value;

                            return Advertisement.AdvertisementId;
                        }
                        catch (Exception exc)
                        {
                            transaction.Rollback();
                        }
                    }
                }
            }
            else if (AdvertisementVM.RecordType == "Properties")
            {
                if (teniacoApiDb.Properties.Where(n => childsUsersIds.Contains(n.UserIdCreator.Value))
                    .Where(x => x.PropertyId.Equals(AdvertisementId)).Any())
                {
                    using (var transaction = teniacoApiDb.Database.BeginTransaction())
                    {
                        try
                        {


                            #region UpdateProperty

                            Properties Property = (from c in teniacoApiDb.Properties
                                                   where c.PropertyId == AdvertisementId
                                                   select c).FirstOrDefault();

                            Property.PropertyTypeId = AdvertisementVM.PropertyTypeId;
                            Property.PropertyCodeName = AdvertisementVM.AdvertisementTitle;
                            Property.TypeOfUseId = AdvertisementVM.TypeOfUseId.HasValue ? AdvertisementVM.TypeOfUseId.Value : (int?)0;
                            Property.DocumentOwnershipTypeId = AdvertisementVM.DocumentOwnershipTypeId.HasValue ? AdvertisementVM.DocumentOwnershipTypeId.Value : (int?)0;
                            Property.DocumentRootTypeId = AdvertisementVM.DocumentRootTypeId.HasValue ? AdvertisementVM.DocumentRootTypeId.Value : (int?)0;
                            Property.DocumentTypeId = AdvertisementVM.DocumentTypeId.HasValue ? AdvertisementVM.DocumentTypeId.Value : (int?)0;
                            Property.Area = AdvertisementVM.Area;
                            Property.RebuiltInYearFa = AdvertisementVM.RebuiltInYearFa;


                            Property.EditEnDate = DateTime.Now;
                            Property.EditTime = PersianDate.TimeNow;
                            Property.UserIdEditor = AdvertisementVM.UserIdEditor.Value;
                            Property.IsActivated = Property.IsActivated.HasValue ? Property.IsActivated.Value : (bool?)true;
                            Property.IsDeleted = Property.IsDeleted.HasValue ? Property.IsDeleted.Value : (bool?)false;
                            Property.PropertyDescriptions = AdvertisementVM.NewAdvertisementDescriptions;

                            teniacoApiDb.Entry<Teniaco.Models.Entities.Properties>(Property).State = EntityState.Modified;
                            teniacoApiDb.SaveChanges();



                            #region PropertyPricesHistories
                            if (AdvertisementVM.PriceTypeRegister == 0)//اصلاح قیمت قبلی
                            {
                                PropertiesPricesHistories PropertyPricesHistories = (
                                    from h in teniacoApiDb.PropertiesPricesHistories
                                    where h.PropertyId == AdvertisementId
                                    select h).OrderByDescending(c => c.CreateEnDate)
                                         .ThenByDescending(c => c.CreateTime).FirstOrDefault();



                                PropertyPricesHistories.DepositPrice = AdvertisementVM.AdvertisementPricesHistoriesVM.DepositPrice;
                                PropertyPricesHistories.RentPrice = AdvertisementVM.AdvertisementPricesHistoriesVM.RentPrice;
                                PropertyPricesHistories.OfferPrice = AdvertisementVM.OfferPrice;
                                PropertyPricesHistories.OfferPriceType = AdvertisementVM.OfferPriceType;
                                PropertyPricesHistories.CalculatedOfferPrice = AdvertisementVM.CalculatedOfferPrice;
                                PropertyPricesHistories.PriceTypeRegister = AdvertisementVM.PriceTypeRegister;
                                PropertyPricesHistories.EditEnDate = DateTime.Now;
                                PropertyPricesHistories.EditTime = PersianDate.TimeNow;
                                PropertyPricesHistories.IsActivated = true;
                                PropertyPricesHistories.IsDeleted = false;


                                teniacoApiDb.Entry<PropertiesPricesHistories>(PropertyPricesHistories).State = EntityState.Modified;
                                teniacoApiDb.SaveChanges();
                            }
                            else//ثبت قیمت جدید
                            {

                                PropertiesPricesHistories PropertyPricesHistories = new PropertiesPricesHistories();
                                PropertyPricesHistories.DepositPrice = AdvertisementVM.AdvertisementPricesHistoriesVM.DepositPrice;
                                PropertyPricesHistories.RentPrice = AdvertisementVM.AdvertisementPricesHistoriesVM.RentPrice;
                                PropertyPricesHistories.OfferPrice = AdvertisementVM.AdvertisementPricesHistoriesVM.OfferPrice.HasValue ? AdvertisementVM.AdvertisementPricesHistoriesVM.OfferPrice.Value : 0;
                                PropertyPricesHistories.OfferPriceType = AdvertisementVM.AdvertisementPricesHistoriesVM.OfferPriceType.HasValue ? AdvertisementVM.AdvertisementPricesHistoriesVM.OfferPriceType.Value : 0;
                                PropertyPricesHistories.CalculatedOfferPrice = AdvertisementVM.AdvertisementPricesHistoriesVM.CalculatedOfferPrice.HasValue ? AdvertisementVM.AdvertisementPricesHistoriesVM.CalculatedOfferPrice.Value : 0;
                                PropertyPricesHistories.PriceTypeRegister = AdvertisementVM.PriceTypeRegister;
                                PropertyPricesHistories.PropertyId = Property.PropertyId;

                                PropertyPricesHistories.CreateEnDate = DateTime.Now;
                                PropertyPricesHistories.CreateTime = PersianDate.TimeNow;

                                PropertyPricesHistories.UserIdCreator = Property.UserIdCreator.Value;
                                PropertyPricesHistories.IsActivated = true;
                                PropertyPricesHistories.IsDeleted = false;

                                teniacoApiDb.PropertiesPricesHistories.Add(PropertyPricesHistories);
                                teniacoApiDb.SaveChanges();


                            }
                            #endregion

                            #region PropertyDetails

                            if (AdvertisementVM.AdvertisementDetailsVM != null)
                            {
                                PropertiesDetails PropertyDetails = (from ad in teniacoApiDb.PropertiesDetails
                                                                     where ad.PropertyId == AdvertisementId
                                                                     select ad).FirstOrDefault();

                                PropertyDetails.PropertyId = AdvertisementId;
                                PropertyDetails.AdvertisementTypeId = AdvertisementVM.AdvertisementDetailsVM.AdvertisementTypeId;
                                PropertyDetails.VerifyAdvertisement = AdvertisementVM.AdvertisementDetailsVM.VerifyAdvertisement;
                                PropertyDetails.Convertable = AdvertisementVM.AdvertisementDetailsVM.Convertable;
                                PropertyDetails.ShowInSpecialSuggestion = AdvertisementVM.AdvertisementDetailsVM.ShowInSpecialSuggestion;
                                PropertyDetails.MaritalStatusId = AdvertisementVM.AdvertisementDetailsVM.MaritalStatusId;
                                PropertyDetails.BuildingLifeId = AdvertisementVM.AdvertisementDetailsVM.BuildingLifeId;
                                PropertyDetails.Foundation = AdvertisementVM.AdvertisementDetailsVM.Foundation;
                                PropertyDetails.TagId = AdvertisementVM.AdvertisementDetailsVM.TagId;


                                teniacoApiDb.Entry<PropertiesDetails>(PropertyDetails).State = EntityState.Modified;
                                teniacoApiDb.SaveChanges();

                            }
                            else
                            {
                                PropertiesDetails PropertyDetails = new PropertiesDetails();

                                PropertyDetails.PropertyId = Property.PropertyId;

                                PropertyDetails.AdvertisementTypeId = AdvertisementVM.AdvertisementDetailsVM.AdvertisementTypeId.HasValue ? AdvertisementVM.AdvertisementDetailsVM.AdvertisementTypeId.Value : 0;
                                PropertyDetails.VerifyAdvertisement = AdvertisementVM.AdvertisementDetailsVM.VerifyAdvertisement;
                                PropertyDetails.Convertable = AdvertisementVM.AdvertisementDetailsVM.Convertable;
                                PropertyDetails.ShowInSpecialSuggestion = AdvertisementVM.AdvertisementDetailsVM.ShowInSpecialSuggestion;
                                PropertyDetails.MaritalStatusId = AdvertisementVM.AdvertisementDetailsVM.MaritalStatusId.HasValue ? AdvertisementVM.AdvertisementDetailsVM.MaritalStatusId.Value : 0;
                                PropertyDetails.BuildingLifeId = AdvertisementVM.AdvertisementDetailsVM.BuildingLifeId.HasValue ? AdvertisementVM.AdvertisementDetailsVM.BuildingLifeId.Value : 0;
                                PropertyDetails.Foundation = AdvertisementVM.AdvertisementDetailsVM.Foundation;
                                PropertyDetails.TagId = AdvertisementVM.AdvertisementDetailsVM.TagId;


                                PropertyDetails.CreateEnDate = DateTime.Now;
                                PropertyDetails.CreateTime = PersianDate.TimeNow;

                                PropertyDetails.UserIdCreator = Property.UserIdCreator.Value;
                                PropertyDetails.IsActivated = true;
                                PropertyDetails.IsDeleted = false;


                                teniacoApiDb.PropertiesDetails.Add(PropertyDetails);
                                teniacoApiDb.SaveChanges();
                            }
                            #endregion

                            #region PropertyAddress

                            if (AdvertisementVM.AdvertisementAddressVM != null)
                            {

                                PropertyAddress PropertyAddress = (from ad in teniacoApiDb.PropertyAddress
                                                                   where ad.PropertyId == AdvertisementId
                                                                   select ad).FirstOrDefault();


                                PropertyAddress.PropertyId = AdvertisementId;
                                PropertyAddress.Address = AdvertisementVM.AdvertisementAddressVM.Address;
                                PropertyAddress.StateId = AdvertisementVM.AdvertisementAddressVM.StateId;
                                PropertyAddress.CityId = AdvertisementVM.AdvertisementAddressVM.CityId;
                                PropertyAddress.ZoneId = AdvertisementVM.AdvertisementAddressVM.ZoneId;
                                PropertyAddress.DistrictId = AdvertisementVM.AdvertisementAddressVM.DistrictId;
                                PropertyAddress.LocationLat = AdvertisementVM.AdvertisementAddressVM.LocationLat;
                                PropertyAddress.LocationLon = AdvertisementVM.AdvertisementAddressVM.LocationLon;

                                teniacoApiDb.Entry<PropertyAddress>(PropertyAddress).State = EntityState.Modified;
                                teniacoApiDb.SaveChanges();

                            }
                            else
                            {
                                PropertyAddress PropertyAddress = new PropertyAddress();

                                PropertyAddress.PropertyId = Property.PropertyId;

                                PropertyAddress.Address = AdvertisementVM.AdvertisementAddressVM.Address;
                                PropertyAddress.StateId = AdvertisementVM.AdvertisementAddressVM.StateId.HasValue ? AdvertisementVM.AdvertisementAddressVM.StateId.Value : 0;
                                PropertyAddress.CityId = AdvertisementVM.AdvertisementAddressVM.CityId.HasValue ? AdvertisementVM.AdvertisementAddressVM.CityId.Value : 0;
                                PropertyAddress.ZoneId = AdvertisementVM.AdvertisementAddressVM.ZoneId.HasValue ? AdvertisementVM.AdvertisementAddressVM.ZoneId.Value : 0;
                                PropertyAddress.DistrictId = AdvertisementVM.AdvertisementAddressVM.DistrictId.HasValue ? AdvertisementVM.AdvertisementAddressVM.DistrictId.Value : 0;
                                PropertyAddress.LocationLat = AdvertisementVM.AdvertisementAddressVM.LocationLat;
                                PropertyAddress.LocationLon = AdvertisementVM.AdvertisementAddressVM.LocationLon;

                                PropertyAddress.CreateEnDate = DateTime.Now;
                                PropertyAddress.CreateTime = PersianDate.TimeNow;

                                PropertyAddress.UserIdCreator = Property.UserIdCreator.Value;
                                PropertyAddress.IsActivated = true;
                                PropertyAddress.IsDeleted = false;


                                teniacoApiDb.PropertyAddress.Add(PropertyAddress);
                                teniacoApiDb.SaveChanges();
                            }

                            #endregion

                            #region PropertyFiles
                            //for adding files
                            if (AdvertisementVM.AdvertisementFilesVM != null)
                            {
                                if (AdvertisementVM.AdvertisementFilesVM.Count > 0)
                                {

                                    //main photo is last record in files table
                                    var MainFile = teniacoApiDb.PropertyFiles
                                        .Where(a => a.PropertyId == AdvertisementId)
                                     .OrderByDescending(a => a.PropertyFileId)
                                     .FirstOrDefault();


                                    //var PropertyFilesList = _mapper.Map<List<AdvertisementFilesVM>, List<AdvertisementFiles>>(AdvertisementVM.AdvertisementFilesVM);
                                    // به علت متفاوت بودن نام پراپرتی ها امکان استفاده از اوتو مپر وجود نداشت
                                    List<PropertyFiles> PropertyFilesList = new List<PropertyFiles>();
                                    foreach (var File in AdvertisementVM.AdvertisementFilesVM)
                                    {
                                        PropertyFiles propFile = new PropertyFiles()
                                        {
                                            PropertyFileTitle = File.AdvertisementFileTitle,
                                            PropertyFilePath = File.AdvertisementFilePath,
                                            PropertyFileExt = File.AdvertisementFileExt,
                                            PropertyFileType = File.AdvertisementFileType,
                                            PropertyFileOrder = File.AdvertisementFileOrder,
                                            UserIdCreator = Property.UserIdCreator.Value,
                                            CreateEnDate = DateTime.Now,
                                            CreateTime = PersianDate.TimeNow,
                                        };
                                        PropertyFilesList.Add(propFile);
                                    }
                                    //اتمام

                                    foreach (var item in PropertyFilesList)
                                    {
                                        item.PropertyId = Property.PropertyId;
                                        item.UserIdCreator = Property.UserIdCreator.Value;
                                        item.IsActivated = true;
                                        item.IsDeleted = false;
                                    }

                                    teniacoApiDb.PropertyFiles.AddRange(PropertyFilesList);
                                    teniacoApiDb.SaveChanges();




                                    if (MainFile != null && AdvertisementVM.IsMainChanged == false)
                                    {
                                        teniacoApiDb.PropertyFiles.Remove(MainFile);
                                        teniacoApiDb.SaveChanges();

                                        MainFile.PropertyFileId = 0;
                                        teniacoApiDb.PropertyFiles.Add(MainFile);
                                        teniacoApiDb.SaveChanges();
                                    }
                                }
                            }

                            //for removing files
                            if (AdvertisementVM.DeletedPhotosIDs != null)
                            {
                                foreach (int id in AdvertisementVM.DeletedPhotosIDs)
                                {
                                    var FileToDelete = teniacoApiDb.PropertyFiles
                                        .Where(f => f.PropertyFileId == id).FirstOrDefault();

                                    teniacoApiDb.PropertyFiles.Remove(FileToDelete);
                                    teniacoApiDb.SaveChanges();
                                }
                            }
                            #endregion

                            #region PropertyFeatures

                            if (AdvertisementVM.AdvertisementFeaturesValuesVM != null)
                            {

                                if (teniacoApiDb.FeaturesValues.Where(fv => fv.PropertyId.Equals(AdvertisementId)).Any())
                                {
                                    var oldFeaturesValues = teniacoApiDb.FeaturesValues.Where(fv => fv.PropertyId.Equals(AdvertisementId)).ToList();
                                    teniacoApiDb.FeaturesValues.RemoveRange(oldFeaturesValues);
                                    melkavanApiDb.SaveChanges();
                                }


                                foreach (var item in AdvertisementVM.AdvertisementFeaturesValuesVM)
                                {

                                    if (!item.FeatureValue.IsNullOrEmpty())
                                    {
                                        FeaturesValues propertyFeaturesValues = teniacoApiDb.FeaturesValues
                                            .Where(a => a.PropertyId == AdvertisementId && a.FeatureId == item.FeatureId).FirstOrDefault();

                                        if (propertyFeaturesValues == null)
                                        {
                                            propertyFeaturesValues = new FeaturesValues();

                                            propertyFeaturesValues.PropertyId = Property.PropertyId;
                                            propertyFeaturesValues.CreateEnDate = Property.CreateEnDate.Value;
                                            propertyFeaturesValues.CreateTime = Property.CreateTime;
                                            propertyFeaturesValues.UserIdCreator = Property.UserIdCreator.Value;
                                            propertyFeaturesValues.IsActivated = true;
                                            propertyFeaturesValues.IsDeleted = false;

                                            propertyFeaturesValues.FeatureId = item.FeatureId;
                                            propertyFeaturesValues.FeatureValue = item.FeatureValue;
                                            teniacoApiDb.FeaturesValues.Add(propertyFeaturesValues);
                                            teniacoApiDb.SaveChanges();
                                        }
                                        else
                                        {
                                            propertyFeaturesValues.UserIdEditor = AdvertisementVM.UserIdEditor;

                                            propertyFeaturesValues.FeatureId = item.FeatureId;
                                            propertyFeaturesValues.FeatureValue = item.FeatureValue;
                                            teniacoApiDb.FeaturesValues.Update(propertyFeaturesValues);
                                            teniacoApiDb.SaveChanges();
                                        }



                                    }

                                }
                            }

                            #endregion

                            #endregion


                            transaction.Commit();

                            AdvertisementVM.UserIdCreator = Property.UserIdCreator.Value;

                            return Property.PropertyId;
                        }
                        catch (Exception exc)
                        {
                            transaction.Rollback();
                        }
                    }
                }
            }



            return 0;
        }





        public bool ToggleActivationAdvertisement(
            long AdvertisementId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var Advertisement = (from c in melkavanApiDb.Advertisement
                                     where c.AdvertisementId == AdvertisementId &&
                                     childsUsersIds.Contains(c.UserIdCreator.Value)
                                     select c).FirstOrDefault();


                if (Advertisement != null)
                {
                    Advertisement.IsActivated = !Advertisement.IsActivated;
                    Advertisement.EditEnDate = DateTime.Now;
                    Advertisement.EditTime = PersianDate.TimeNow;
                    Advertisement.UserIdEditor = userId;

                    melkavanApiDb.Entry<Advertisement>(Advertisement).State = EntityState.Modified;
                    melkavanApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }


        public bool TemporaryDeleteAdvertisement(
            long AdvertisementId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var Advertisement = (from c in melkavanApiDb.Advertisement
                                     where c.AdvertisementId == AdvertisementId &&
                                     childsUsersIds.Contains(c.UserIdCreator.Value)
                                     select c).FirstOrDefault();

                if (Advertisement != null)
                {
                    Advertisement.IsDeleted = !Advertisement.IsDeleted;
                    Advertisement.EditEnDate = DateTime.Now;
                    Advertisement.EditTime = PersianDate.TimeNow;
                    Advertisement.UserIdEditor = userId;


                    melkavanApiDb.Entry<Advertisement>(Advertisement).State = EntityState.Modified;
                    melkavanApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }


        public bool CompleteDeleteAdvertisement(
          long AdvertisementId,
          List<long> childsUsersIds,
          long UserId,
          string type,
            TeniacoApiContext teniacoApiDb)
        {
            if (type.Equals("Advertisement"))
            {
                using (var transaction = melkavanApiDb.Database.BeginTransaction())
                {
                    try
                    {
                        //var Advertisement = melkavanApiDb.Advertisement.Where(
                        //    a => a.AdvertisementId == AdvertisementId &&
                        //    a.UserIdCreator == UserId).
                        //    Include(a => a.AdvertisementPricesHistories)
                        //    .Include(a => a.AdvertisementAddress)
                        //    .Include(a => a.AdvertisementDetails)
                        //    .Include(a => a.AdvertisementFeaturesValues)
                        //    .Include(a => a.AdvertisementCallers)
                        //    .Include(a => a.AdvertisementFiles)
                        //    .Include(a => a.AdvertisementOwners)
                        //    .Include(a => a.AdvertisementViewers)
                        //    .Include(a => a.AdvertisementFavourite)
                        //    .Include(a => a.AdvertisementSelectedCaller).FirstOrDefault();

                        var Advertisement = melkavanApiDb.Advertisement.Where(
            a => a.AdvertisementId == AdvertisementId &&
            a.UserIdCreator == UserId).FirstOrDefault();

                        if (Advertisement != null)
                        {

                            #region AdvertisementAddress 

                            try
                            {
                                var AdvertisementAddress = melkavanApiDb.AdvertisementAddress.Where(
                                    a => a.AdvertisementId == AdvertisementId).FirstOrDefault();


                                if (AdvertisementAddress != null)
                                {
                                    melkavanApiDb.AdvertisementAddress.Remove(AdvertisementAddress);
                                    melkavanApiDb.SaveChanges();
                                }

                            }
                            catch (Exception exc)
                            {
                                return false;
                            }

                            #endregion

                            #region AdvertisementDetails 


                            try
                            {
                                var AdvertisementDetails = melkavanApiDb.AdvertisementDetails.Where(
                                    ad => ad.AdvertisementId == AdvertisementId).FirstOrDefault();

                                if (AdvertisementDetails != null)
                                {
                                    melkavanApiDb.AdvertisementDetails.Remove(AdvertisementDetails);
                                    melkavanApiDb.SaveChanges();
                                }


                            }
                            catch (Exception exc)
                            {
                                return false;
                            }



                            #endregion

                            #region AdvertisementPriceHistories


                            var AdvertisementPriceHistories = melkavanApiDb.AdvertisementPricesHistories.Where(
                                p => p.AdvertisementId == AdvertisementId).ToList();

                            try
                            {
                                if (AdvertisementPriceHistories != null && AdvertisementPriceHistories.Count > 0)
                                {
                                    melkavanApiDb.AdvertisementPricesHistories.RemoveRange(AdvertisementPriceHistories);
                                    melkavanApiDb.SaveChanges();
                                }

                            }
                            catch (Exception)
                            {
                                return false;
                            }

                            #endregion

                            #region AdvertisementFavourite 


                            try
                            {
                                var AdvertisementFavourite = melkavanApiDb.AdvertisementFavorites.Where(
                                    a => a.AdvertisementId == AdvertisementId).FirstOrDefault();

                                if (AdvertisementFavourite != null)
                                {
                                    melkavanApiDb.AdvertisementFavorites.Remove(AdvertisementFavourite);
                                    melkavanApiDb.SaveChanges();
                                }

                            }
                            catch (Exception exc)
                            {
                                return false;
                            }

                            #endregion

                            #region AdvertisementSelectedCaller


                            try
                            {
                                var AdvertisementSelectedCaller = melkavanApiDb.AdvertisementSelectedCallers.Where(
                                    a => a.AdvertisementId == AdvertisementId).FirstOrDefault();


                                if (AdvertisementSelectedCaller != null)
                                {
                                    melkavanApiDb.AdvertisementSelectedCallers.Remove(AdvertisementSelectedCaller);
                                    melkavanApiDb.SaveChanges();
                                }


                            }
                            catch (Exception exc)
                            {
                                return false;
                            }

                            #endregion

                            #region AdvertisementCaller


                            var AdvertisementCaller = melkavanApiDb.AdvertisementCallers.Where(
                                p => p.AdvertisementId == AdvertisementId).ToList();

                            try
                            {
                                if (AdvertisementCaller != null && AdvertisementCaller.Count > 0)
                                {
                                    melkavanApiDb.AdvertisementCallers.RemoveRange(AdvertisementCaller);
                                    melkavanApiDb.SaveChanges();
                                }


                            }
                            catch (Exception)
                            {
                                return false;
                            }


                            #endregion

                            #region AdvertisementFeaturesValues



                            var AdvertisementFeaturesValue = melkavanApiDb.AdvertisementFeaturesValues.Where(
                                p => p.AdvertisementId == AdvertisementId).ToList();

                            try
                            {
                                if (AdvertisementFeaturesValue != null && AdvertisementFeaturesValue.Count > 0)
                                {
                                    melkavanApiDb.AdvertisementFeaturesValues.RemoveRange(AdvertisementFeaturesValue);
                                    melkavanApiDb.SaveChanges();
                                }


                            }
                            catch (Exception)
                            {
                                return false;
                            }


                            #endregion

                            #region AdvertisementFiles



                            var AdvertisementFile = melkavanApiDb.AdvertisementFiles.Where(
                                p => p.AdvertisementId == AdvertisementId).ToList();

                            try
                            {
                                if (AdvertisementFile != null && AdvertisementFile.Count > 0)
                                {
                                    melkavanApiDb.AdvertisementFiles.RemoveRange(AdvertisementFile);
                                    melkavanApiDb.SaveChanges();
                                }


                            }
                            catch (Exception)
                            {
                                return false;
                            }

                            #endregion

                            #region AdvertisementViewers



                            var AdvertisementViewer = melkavanApiDb.AdvertisementViewers.Where(
                                p => p.AdvertisementId == AdvertisementId).ToList();

                            try
                            {
                                if (AdvertisementViewer != null && AdvertisementViewer.Count > 0)
                                {
                                    melkavanApiDb.AdvertisementViewers.RemoveRange(AdvertisementViewer);
                                    melkavanApiDb.SaveChanges();
                                }

                            }
                            catch (Exception)
                            {
                                return false;
                            }


                            #endregion

                            #region AdvertisementOwners



                            var AdvertisementOwner = melkavanApiDb.AdvertisementOwners.Where(
                                p => p.AdvertisementId == AdvertisementId).ToList();

                            try
                            {
                                if (AdvertisementOwner != null && AdvertisementOwner.Count > 0)
                                {
                                    melkavanApiDb.AdvertisementOwners.RemoveRange(AdvertisementOwner);
                                    melkavanApiDb.SaveChanges();
                                }

                            }
                            catch (Exception)
                            {
                                return false;
                            }


                            #endregion

                            #region Advertisement

                            try
                            {
                                melkavanApiDb.Advertisement.Remove(Advertisement);
                                melkavanApiDb.SaveChanges();
                            }
                            catch (Exception exc)
                            {
                                return false;
                            }

                            #endregion


                            transaction.Commit();
                        }
                    }
                    catch (Exception exc)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }

            else if (type.Equals("Properties"))
            {
                using (var transaction = teniacoApiDb.Database.BeginTransaction())
                {
                    try
                    {
                        //var Properties = teniacoApiDb.Properties.Where(
                        //    a => a.PropertyId == AdvertisementId &&
                        //    a.UserIdCreator == UserId).
                        //    Include(a => a.PropertiesPricesHistories)
                        //    .Include(a => a.PropertyAddress)
                        //    .Include(a => a.PropertyDetails)
                        //    .Include(a => a.FeaturesValues)
                        //    .Include(a => a.PropertyCallers)
                        //    .Include(a => a.PropertyFiles)
                        //    .Include(a => a.PropertyOwners)
                        //    .Include(a => a.PropertyViewers)
                        //    .Include(a => a.PropertyFavorites)
                        //    .Include(a => a.SelectedPropertyCallers).FirstOrDefault();

                        var Properties = teniacoApiDb.Properties.Where(
                            a => a.PropertyId == AdvertisementId &&
                            a.UserIdCreator == UserId).FirstOrDefault();

                        if (Properties != null)
                        {
                            #region Codes for complete delete all property tables
                            //#region PropertiesAddress 


                            //    try
                            //    {
                            //        var PropertiesAddress = teniacoApiDb.PropertyAddress.Where(
                            //            a => a.PropertyId == AdvertisementId).FirstOrDefault();

                            //    if(PropertiesAddress != null)
                            //    {

                            //        teniacoApiDb.PropertyAddress.Remove(PropertiesAddress);
                            //        teniacoApiDb.SaveChanges();
                            //    }

                            //    }
                            //    catch (Exception exc)
                            //    {
                            //        return false;
                            //    }

                            //#endregion

                            //#region PropertiesDetails 


                            //    try
                            //    {
                            //        var PropertiesDetails = teniacoApiDb.PropertiesDetails.Where(
                            //            ad => ad.PropertyId == AdvertisementId).FirstOrDefault();

                            //    if (PropertiesDetails != null)
                            //    {
                            //        teniacoApiDb.PropertiesDetails.Remove(PropertiesDetails);
                            //        teniacoApiDb.SaveChanges();
                            //    }



                            //    }
                            //    catch (Exception exc)
                            //    {
                            //        return false;
                            //    }



                            //#endregion

                            //#region PropertiesPriceHistories



                            //    var PropertiesPriceHistories = teniacoApiDb.PropertiesPricesHistories.Where(
                            //        p => p.PropertyId == AdvertisementId).ToList();

                            //    try
                            //    {

                            //    if (PropertiesPriceHistories != null && PropertiesPriceHistories.Count > 0)
                            //    {
                            //        teniacoApiDb.PropertiesPricesHistories.RemoveRange(PropertiesPriceHistories);
                            //        teniacoApiDb.SaveChanges();
                            //    }

                            //    }
                            //    catch (Exception)
                            //    {
                            //        return false;
                            //    }


                            //#endregion

                            //#region PropertiesFavourite 



                            //    try
                            //    {
                            //        var PropertiesFavourite = teniacoApiDb.PropertiesFavorites.Where(
                            //            a => a.PropertyId == AdvertisementId).FirstOrDefault();

                            //    if (PropertiesFavourite != null)
                            //    {

                            //        teniacoApiDb.PropertiesFavorites.Remove(PropertiesFavourite);
                            //        teniacoApiDb.SaveChanges();
                            //    }

                            //    }
                            //    catch (Exception exc)
                            //    {
                            //        return false;
                            //    }

                            //#endregion

                            //#region PropertiesSelectedCaller



                            //    try
                            //    {
                            //        var PropertiesSelectedCaller = teniacoApiDb.PropertySelectedCallers.Where(
                            //            a => a.PropertyId == AdvertisementId).FirstOrDefault();

                            //    if (PropertiesSelectedCaller != null)
                            //    {
                            //        teniacoApiDb.PropertySelectedCallers.Remove(PropertiesSelectedCaller);
                            //        teniacoApiDb.SaveChanges();
                            //    }


                            //    }
                            //    catch (Exception exc)
                            //    {
                            //        return false;
                            //    }

                            //#endregion

                            //#region PropertiesCaller



                            //    var PropertiesCaller = teniacoApiDb.PropertiesCallers.Where(
                            //        p => p.PropertyId == AdvertisementId).ToList();

                            //    try
                            //    {
                            //    if (PropertiesCaller != null && PropertiesCaller.Count > 0)
                            //    {
                            //        teniacoApiDb.PropertiesCallers.RemoveRange(PropertiesCaller);
                            //        teniacoApiDb.SaveChanges();
                            //    }


                            //    }
                            //    catch (Exception)
                            //    {
                            //        return false;
                            //    }

                            //#endregion

                            //#region PropertiesFeaturesValues



                            //    var PropertiesFeaturesValue = teniacoApiDb.FeaturesValues.Where(
                            //        p => p.PropertyId == AdvertisementId).ToList();

                            //    try
                            //    {
                            //    if (PropertiesFeaturesValue != null && PropertiesFeaturesValue.Count > 0)
                            //    {
                            //        teniacoApiDb.FeaturesValues.RemoveRange(PropertiesFeaturesValue);
                            //        teniacoApiDb.SaveChanges();
                            //    }


                            //    }
                            //    catch (Exception)
                            //    {
                            //        return false;
                            //    }


                            //#endregion

                            //#region PropertiesFiles



                            //    var PropertiesFile = teniacoApiDb.PropertyFiles.Where(
                            //        p => p.PropertyId == AdvertisementId).ToList();

                            //    try
                            //    {
                            //    if (PropertiesFile != null && PropertiesFile.Count > 0)
                            //    {

                            //        teniacoApiDb.PropertyFiles.RemoveRange(PropertiesFile);
                            //        teniacoApiDb.SaveChanges();
                            //    }


                            //    }
                            //    catch (Exception)
                            //    {
                            //        return false;
                            //    }


                            //#endregion

                            //#region PropertiesViewers



                            //    var PropertiesViewer = teniacoApiDb.PropertiesViewers.Where(
                            //        p => p.PropertyId == AdvertisementId).ToList();

                            //    try
                            //    {
                            //    if (PropertiesViewer != null && PropertiesViewer.Count > 0)
                            //    {
                            //        teniacoApiDb.PropertiesViewers.RemoveRange(PropertiesViewer);
                            //        teniacoApiDb.SaveChanges();
                            //    }


                            //    }
                            //    catch (Exception)
                            //    {
                            //        return false;
                            //    }


                            //#endregion

                            //#region PropertiesOwners



                            //    var PropertiesOwner = teniacoApiDb.PropertyOwners.Where(
                            //        p => p.PropertyId == AdvertisementId).ToList();

                            //    try
                            //    {
                            //    if (PropertiesOwner != null && PropertiesOwner.Count > 0)
                            //    {
                            //        teniacoApiDb.PropertyOwners.RemoveRange(PropertiesOwner);
                            //        teniacoApiDb.SaveChanges();
                            //    }


                            //    }
                            //    catch (Exception)
                            //    {
                            //        return false;
                            //    }

                            //#endregion

                            //#region Properties

                            //try
                            //{
                            //    teniacoApiDb.Properties.Remove(Properties);
                            //    teniacoApiDb.SaveChanges();
                            //}
                            //catch (Exception exc)
                            //{
                            //    return false;
                            //}

                            //#endregion


                            #endregion


                            Properties.ShowInMelkavan = false;
                            teniacoApiDb.Properties.Update(Properties);
                            teniacoApiDb.SaveChanges();

                            transaction.Commit();
                        }
                    }
                    catch (Exception exc)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }


            return true;
        }



        public bool TemporaryDeleteAdvertisementWithChild(
            long AdvertisementId,
           List<long> childsUsersIds)
        {
            using (var transaction = melkavanApiDb.Database.BeginTransaction())
            {
                try
                {

                    if (melkavanApiDb.AdvertisementFiles.Where(a => a.AdvertisementId == AdvertisementId && childsUsersIds.Contains(a.UserIdCreator.Value)).Any())
                    {
                        var advertisemnetFilesList = melkavanApiDb.AdvertisementFiles.Where(a => a.AdvertisementId == AdvertisementId && childsUsersIds.Contains(a.UserIdCreator.Value)).ToList();

                        foreach (var item in advertisemnetFilesList)
                        {
                            item.IsDeleted = true;
                        }
                        melkavanApiDb.AdvertisementFiles.UpdateRange(advertisemnetFilesList);
                    }


                    if (melkavanApiDb.AdvertisementPricesHistories.Where(a => a.AdvertisementId == AdvertisementId && childsUsersIds.Contains(a.UserIdCreator.Value)).Any())
                    {
                        var advertisemenPriceHistoriesList = melkavanApiDb.AdvertisementPricesHistories.Where(a => a.AdvertisementId == AdvertisementId && childsUsersIds.Contains(a.UserIdCreator.Value)).ToList();

                        foreach (var item in advertisemenPriceHistoriesList)
                        {
                            item.IsDeleted = true;
                        }
                        melkavanApiDb.AdvertisementPricesHistories.UpdateRange(advertisemenPriceHistoriesList);
                    }


                    if (melkavanApiDb.Advertisement.Where(a => a.AdvertisementId.Equals(AdvertisementId)).Any())
                    {
                        var advertise = melkavanApiDb.Advertisement.Where(a => a.AdvertisementId.Equals(AdvertisementId)).FirstOrDefault();
                        advertise.IsDeleted = true;
                    }


                    melkavanApiDb.SaveChanges();
                    transaction.Commit();


                    return true;
                }
                catch
                { }
            }
            return false;
        }



        #region Sina's code


        //public List<AdvertisementAdvanceSearchVM> GetAdvertisementsWithOwnerId(
        //        int jtStartIndex,
        //        int jtPageSize,
        //        ref int listCount,
        //        List<long> childsUsersIds,
        //        PublicApiContext publicApiDb,
        //        TeniacoApiContext teniacoApiDb,
        //        string jtSorting = null,
        //        long? owenerId = null)
        //{

        //    List<AdvertisementAdvanceSearchVM> advertisementAdvanceSearchVMList = new List<AdvertisementAdvanceSearchVM>();

        //    try
        //    {
        //        string sp = @"";

        //        sp = @"
        //                SELECT DISTINCT * FROM (
        //                            SELECT 'Advertisement' AS RecordType,
        //                            details.InstagramLink,
        //                            details.AdvertisementTypeId,
        //                            ad.AdvertisementId,
        //                            ad.StatusId,
        //                            ad.RejectionReason,
        //                            '' AS PropertyTypeTitle,
        //                            '' AS TypeUseTitle,
        //                            '' AS DocumentTypeTitle,
        //                            '' AS DocumentOwnershipTypeTitle,
        //                            '' AS DocumentRootTypeTitle,
        //                            '' AS StateName,
        //                            '' AS CityName,
        //                            '' AS ZoneName,
        //                            '' AS DistrictName,
        //                            '' AS UserCreatorName,
        //                            ad.IsActivated,
        //                            ad.IsDeleted,
        //                            ad.ConsultantUserId,
        //                            ad.EditEnDate,
        //                            ad.RebuiltInYearFa,
        //                            ad.CreateEnDate,
        //                            ad.PropertyTypeId,
        //                            ad.TypeOfUseId,
        //                            ad.DocumentOwnershipTypeId,
        //                            ad.DocumentTypeId,
        //                            ad.DocumentRootTypeId,
        //                            ad.AdvertisementTitle,
        //                            ad.Area,
        //                            ad.AdvertisementDescriptions,
        //                            ad.UserIdCreator,
        //                            '1' AS ShowInMelkavan,
        //                            addr.CountryId,
        //                            addr.StateId,
        //                            addr.CityId,
        //                            addr.ZoneId,
        //                            addr.DistrictId,
        //                            addr.TownName,
        //                            priceHist.OfferPrice,
        //                            priceHist.DepositPrice,
        //                            priceHist.RentPrice,
        //                            '' AS CurrentDate,
        //                            details.TagId,
        //                            CAST('false' AS BIT) AS IsFavorite
        //                                FROM MelkavanDbHaghighi.dbo.Advertisement AS ad
        //                                INNER JOIN MelkavanDbHaghighi.dbo.AdvertisementOwners on ad.AdvertisementId = MelkavanDbHaghighi.dbo.AdvertisementOwners.AdvertisementId
        //                                INNER JOIN MelkavanDbHaghighi.dbo.AdvertisementAddress AS addr ON ad.AdvertisementId = addr.AdvertisementId
        //                                INNER JOIN (
        //                                        SELECT AdvertisementId, OfferPrice, DepositPrice, RentPrice
        //                                        FROM MelkavanDbHaghighi.dbo.AdvertisementPricesHistories AS aph
        //                                        WHERE aph.AdvertisementPriceHistoryId = (
        //                                            SELECT MAX(AdvertisementPriceHistoryId)
        //                                            FROM MelkavanDbHaghighi.dbo.AdvertisementPricesHistories
        //                                            WHERE AdvertisementId = aph.AdvertisementId
        //                                        )
        //                                ) AS priceHist ON ad.AdvertisementId = priceHist.AdvertisementId
        //                                INNER JOIN MelkavanDbHaghighi.dbo.AdvertisementDetails AS details ON ad.AdvertisementId = details.AdvertisementId
        //                                where ad.UserIdCreator = {owenerId} or ad.AdvertiserId = {owenerId} or AdvertisementOwners.OwnerId={owenerId} or ad.ConsultantUserId={owenerId}
        //                                UNION ALL
        //                                        SELECT 'Properties' AS RecordType,
        //                                                '' AS InstagramLink,
        //                                                pd.AdvertisementTypeId,
        //                                                p.PropertyId AS AdvertisementId,
        //                                                p.StatusId,
        //                                                p.RejectionReason,
        //                                                '' AS PropertyTypeTitle,
        //                                                '' AS TypeUseTitle,
        //                                                '' AS DocumentTypeTitle,
        //                                                '' AS DocumentOwnershipTypeTitle,
        //                                                '' AS DocumentRootTypeTitle,
        //                                                '' AS StateName,
        //                                                '' AS CityName,
        //                                                '' AS ZoneName,
        //                                                '' AS DistrictName,
        //                                                '' AS UserCreatorName,
        //                                                p.IsActivated,
        //                                                p.IsDeleted,
        //                                                p.ConsultantUserId,
        //                                                p.EditEnDate,
        //                                                p.RebuiltInYearFa,
        //                                                p.CreateEnDate,
        //                                                p.PropertyTypeId,
        //                                                p.TypeOfUseId,
        //                                                p.DocumentOwnershipTypeId,
        //                                                p.DocumentTypeId,
        //                                                p.DocumentRootTypeId,
        //                                                p.PropertyCodeName AS AdvertisementTitle,
        //                                                p.Area,
        //                                                p.PropertyDescriptions AS AdvertisementDescriptions,
        //                                                p.UserIdCreator,
        //                                                p.ShowInMelkavan,
        //                                                propAddr.CountryId,
        //                                                propAddr.StateId,
        //                                                propAddr.CityId,
        //                                                propAddr.ZoneId,
        //                                                propAddr.DistrictId,
        //                                                propAddr.TownName,
        //                                                propPriceHist.OfferPrice,
        //                                                propPriceHist.DepositPrice,
        //                                                propPriceHist.RentPrice,
        //                                                '' AS CurrentDate,
        //                                                pd.TagId,
        //                                                CAST('false' AS BIT) AS IsFavorite
        //                                           FROM TeniacoDbHaghighi.dbo.Properties AS p
        //                                                INNER JOIN  TeniacoDbHaghighi.dbo.PropertyOwners on p.PropertyId = TeniacoDbHaghighi.dbo.PropertyOwners.PropertyId
        //                                                INNER JOIN TeniacoDbHaghighi.dbo.PropertyAddress AS propAddr ON p.PropertyId = propAddr.PropertyId
        //                                                INNER JOIN (
        //                                                    SELECT PropertyId, CASE OfferPriceType WHEN '0' THEN CalculatedOfferPrice ELSE OfferPrice END as OfferPrice, DepositPrice, RentPrice
        //                                                            FROM TeniacoDbHaghighi.dbo.PropertiesPricesHistories AS pph
        //                                                                    WHERE pph.PropertyPriceHistoryId = (
        //                                                                        SELECT MAX(PropertyPriceHistoryId)
        //                                                                        FROM TeniacoDbHaghighi.dbo.PropertiesPricesHistories
        //                                                                        WHERE PropertyId = pph.PropertyId
        //                                                                    )
        //                                                ) AS propPriceHist ON p.PropertyId = propPriceHist.PropertyId
        //                                                INNER JOIN TeniacoDbHaghighi.dbo.PropertiesDetails AS pd ON p.PropertyId = pd.PropertyId
        //                                                WHERE p.ShowInMelkavan = 1 and (p.UserIdCreator = {owenerId} or p.AdvertiserId = {owenerId} or PropertyOwners.OwnerId={owenerId} or p.ConsultantUserId={owenerId})
        //                                            ) AS tmp
        //                                            ORDER BY CreateEnDate DESC;";

        //        sp = sp.Replace("{owenerId}", owenerId.ToString());

        //        var list1 = melkavanApiDb.AdvertisementAdvanceSearchVM.FromSqlRaw(sp).AsEnumerable().Distinct();


        //        #region ChildUsers
        //        //if (childsUsersIds != null)
        //        //{
        //        //    if (childsUsersIds.Count > 1)
        //        //    {
        //        //        list1 = list1.Where(p => p.UserIdCreator.HasValue).Where(p => childsUsersIds.Contains(p.UserIdCreator.Value));
        //        //    }
        //        //    else
        //        //    {
        //        //        if (childsUsersIds.Count == 1)
        //        //        {
        //        //            if (childsUsersIds.FirstOrDefault() > 0)
        //        //            {
        //        //                list1 = list1.Where(p => p.UserIdCreator.HasValue).Where(p => childsUsersIds.Contains(p.UserIdCreator.Value));
        //        //            }
        //        //        }
        //        //    }
        //        //}
        //        #endregion

        //        #region Pagination and Load extra title data

        //        try
        //        {
        //            if (string.IsNullOrEmpty(jtSorting))
        //            {
        //                listCount = list1.Count();

        //                //if (listCount > jtPageSize)
        //                //{
        //                list1 = list1.OrderByDescending(s => s.AdvertisementId)
        //                         .Skip(jtStartIndex).Take(jtPageSize).ToList();
        //                //}
        //                //else
        //                //{


        //                //    list1 = list1.OrderByDescending(s => s.AdvertisementId).ToList();
        //                //}
        //            }
        //            else
        //            {
        //                listCount = list1.Count();

        //                if (listCount > jtPageSize)
        //                {
        //                    switch (jtSorting)
        //                    {
        //                        case "AdvertisementTitle ASC":
        //                            list1 = list1.OrderBy(l => l.AdvertisementTitle);
        //                            break;
        //                        case "AdvertisementTitle DESC":
        //                            list1 = list1.OrderByDescending(l => l.AdvertisementTitle);
        //                            break;
        //                    }

        //                    if (string.IsNullOrEmpty(jtSorting))
        //                        advertisementAdvanceSearchVMList = list1.OrderByDescending(s => s.AdvertisementId)
        //                                 .Skip(jtStartIndex).Take(jtPageSize).ToList();
        //                    else
        //                        advertisementAdvanceSearchVMList = list1.Skip(jtStartIndex).Take(jtPageSize).ToList();
        //                }
        //                else
        //                {

        //                    advertisementAdvanceSearchVMList = list1.ToList();
        //                }
        //            }





        //            var stateIds = list1.Where(p => p.StateId.HasValue).Select(p => p.StateId.Value).ToList();
        //            var states = publicApiDb.States.Where(s => stateIds.Contains(s.StateId)).ToList();

        //            var cityIds = list1.Where(p => p.CityId.HasValue).Select(p => p.CityId.Value).ToList();
        //            var cities = publicApiDb.Cities.Where(s => cityIds.Contains(s.CityId)).ToList();

        //            var zoneIds = list1.Where(p => p.ZoneId.HasValue).Select(p => p.ZoneId.Value).ToList();
        //            var zones = publicApiDb.Zones.Where(s => zoneIds.Contains(s.ZoneId)).ToList();

        //            var districtIds = list1.Where(p => p.DistrictId.HasValue).Select(p => p.DistrictId.Value).ToList();
        //            var districts = publicApiDb.Districts.Where(s => districtIds.Contains(s.DistrictId)).ToList();

        //            var propertyIds = list1.Select(p => p.AdvertisementId).ToList().Distinct();

        //            var advertisementIds = list1.Select(c => c.AdvertisementId).ToList().Distinct();

        //            var advertisementFilesList = melkavanApiDb.AdvertisementFiles.Where(a => advertisementIds.Contains(a.AdvertisementId) && a.AdvertisementFileType.Equals("media")).ToList();

        //            var propertiesFilesList = teniacoApiDb.PropertyFiles.Where(z => advertisementIds.Contains(z.PropertyId) && z.PropertyFileType.Equals("media")).ToList();


        //            var tmpList = list1.Distinct();

        //            foreach (var item in tmpList)
        //            {


        //                var state = states.Where(s => s.StateId.Equals(item.StateId)).FirstOrDefault();
        //                if (state != null)
        //                {
        //                    item.StateName = state.StateName;
        //                }

        //                var city = cities.Where(c => c.CityId.Equals(item.CityId)).FirstOrDefault();
        //                if (city != null)
        //                {
        //                    item.CityName = city.CityName;
        //                }

        //                if (item.ZoneId.HasValue)
        //                {
        //                    var zone = zones.Where(z => z.ZoneId.Equals(item.ZoneId.Value)).FirstOrDefault();
        //                    if (zone != null)
        //                    {
        //                        item.ZoneName = zone.ZoneName;
        //                    }
        //                }

        //                if (item.DistrictId.HasValue)
        //                {
        //                    var district = districts.Where(z => z.DistrictId.Equals(item.DistrictId.Value)).FirstOrDefault();
        //                    if (district != null)
        //                    {
        //                        item.DistrictName = district.DistrictName;

        //                    }
        //                }

        //                if (owenerId != 0 &&
        //                    melkavanApiDb.AdvertisementFavorites.Where(a => a.UserIdCreator == owenerId && a.AdvertisementId == item.AdvertisementId && a.IsDeleted != true).Any())
        //                {
        //                    item.IsFavorite = true;
        //                }
        //                else
        //                {
        //                    if (owenerId != 0 &&
        //                        teniacoApiDb.PropertiesFavorites.Where(a => a.UserIdCreator == owenerId && a.PropertyId == item.AdvertisementId && a.IsDeleted != true).Any())
        //                    {
        //                        item.IsFavorite = true;
        //                    }
        //                }


        //                if (advertisementFilesList.Where(ad => ad.AdvertisementId == item.AdvertisementId).Any())
        //                {
        //                    try
        //                    {
        //                        item.AdvertisementFilesVM = advertisementFilesList.Where(ad => ad.AdvertisementId == item.AdvertisementId)
        //                            .Select(p => new AdvertisementFilesVM
        //                            {
        //                                AdvertisementId = p.AdvertisementId,
        //                                AdvertisementFilePath = p.AdvertisementFilePath,
        //                                AdvertisementFileExt = p.AdvertisementFileExt,
        //                                AdvertisementFileTitle = p.AdvertisementFileTitle,
        //                                AdvertisementFileId = p.AdvertisementFileId,
        //                                AdvertisementFileType = p.AdvertisementFileType,
        //                            }).OrderByDescending(f => f.AdvertisementFileId).ToList();
        //                    }
        //                    catch (Exception exc)
        //                    { }
        //                }
        //                else
        //                {
        //                    if (propertiesFilesList.Where(ad => ad.PropertyId == item.AdvertisementId).Any())
        //                    {
        //                        try
        //                        {
        //                            item.AdvertisementFilesVM = propertiesFilesList.Where(ad => ad.PropertyId == item.AdvertisementId)
        //                                .Select(p => new AdvertisementFilesVM
        //                                {
        //                                    AdvertisementId = p.PropertyId,
        //                                    AdvertisementFilePath = p.PropertyFilePath,
        //                                    AdvertisementFileExt = p.PropertyFileExt,
        //                                    AdvertisementFileTitle = p.PropertyFileTitle,
        //                                    AdvertisementFileId = p.PropertyFileId,
        //                                    AdvertisementFileType = p.PropertyFileType,
        //                                }).OrderByDescending(f => f.AdvertisementFileId).ToList();
        //                        }
        //                        catch (Exception exc)
        //                        { }
        //                    }
        //                }
        //            }



        //            advertisementAdvanceSearchVMList = tmpList.Distinct().ToList();

        //        }
        //        catch (Exception exc)
        //        { }

        //        #endregion


        //    }
        //    catch (Exception exc)
        //    { }



        //    return advertisementAdvanceSearchVMList;
        //}

        #endregion



        public List<AdvertisementAdvanceSearchVM> GetAdvertisementsWithOwnerId(
               int jtStartIndex,
               int jtPageSize,
               ref int listCount,
               List<long> childsUsersIds,
               PublicApiContext publicApiDb,
               TeniacoApiContext teniacoApiDb,
               string jtSorting = null,
               long? owenerId = null)
        {

            List<AdvertisementAdvanceSearchVM> advertisementAdvanceSearchVMList = new List<AdvertisementAdvanceSearchVM>();

            try
            {
                string sp = @"";

                sp = @"
                        SELECT DISTINCT * FROM (
                                    SELECT 'Advertisement' AS RecordType,
                                    details.InstagramLink,
                                    details.AdvertisementTypeId,
                                    ad.AdvertisementId,
                                    ad.StatusId,
                                    ad.RejectionReason,
                                    '' AS PropertyTypeTitle,
                                    '' AS TypeUseTitle,
                                    '' AS DocumentTypeTitle,
                                    '' AS DocumentOwnershipTypeTitle,
                                    '' AS DocumentRootTypeTitle,
                                    '' AS StateName,
                                    '' AS CityName,
                                    '' AS ZoneName,
                                    '' AS DistrictName,
                                    '' AS UserCreatorName,
                                    ad.IsActivated,
                                    ad.IsDeleted,
                                    ad.ConsultantUserId,
                                    ad.EditEnDate,
                                    ad.RebuiltInYearFa,
                                    ad.CreateEnDate,
                                    ad.PropertyTypeId,
                                    ad.TypeOfUseId,
                                    ad.DocumentOwnershipTypeId,
                                    ad.DocumentTypeId,
                                    ad.DocumentRootTypeId,
                                    ad.AdvertisementTitle,
                                    ad.Area,
                                    ad.AdvertisementDescriptions,
                                    ad.UserIdCreator,
                                    '1' AS ShowInMelkavan,
                                    addr.CountryId,
                                    addr.StateId,
                                    addr.CityId,
                                    addr.ZoneId,
                                    addr.DistrictId,
                                    addr.TownName,
                                    priceHist.OfferPrice,
                                    priceHist.DepositPrice,
                                    priceHist.RentPrice,
                                    '' AS CurrentDate,
                                    details.TagId,
                                    CAST('false' AS BIT) AS IsFavorite
                                        FROM MelkavanDb.dbo.Advertisement AS ad
                                        INNER JOIN MelkavanDb.dbo.AdvertisementOwners on ad.AdvertisementId = MelkavanDb.dbo.AdvertisementOwners.AdvertisementId
                                        INNER JOIN MelkavanDb.dbo.AdvertisementAddress AS addr ON ad.AdvertisementId = addr.AdvertisementId
                                        INNER JOIN (
                                                SELECT AdvertisementId, OfferPrice, DepositPrice, RentPrice
                                                FROM MelkavanDb.dbo.AdvertisementPricesHistories AS aph
                                                WHERE aph.AdvertisementPriceHistoryId = (
                                                    SELECT MAX(AdvertisementPriceHistoryId)
                                                    FROM MelkavanDb.dbo.AdvertisementPricesHistories
                                                    WHERE AdvertisementId = aph.AdvertisementId
                                                )
                                        ) AS priceHist ON ad.AdvertisementId = priceHist.AdvertisementId
                                        INNER JOIN MelkavanDb.dbo.AdvertisementDetails AS details ON ad.AdvertisementId = details.AdvertisementId
                                        where ad.UserIdCreator = {owenerId} or ad.AdvertiserId = {owenerId} or AdvertisementOwners.OwnerId={owenerId} or ad.ConsultantUserId={owenerId}
                                        UNION ALL
                                                SELECT 'Properties' AS RecordType,
                                                        '' AS InstagramLink,
                                                        pd.AdvertisementTypeId,
                                                        p.PropertyId AS AdvertisementId,
                                                        p.StatusId,
                                                        p.RejectionReason,
                                                        '' AS PropertyTypeTitle,
                                                        '' AS TypeUseTitle,
                                                        '' AS DocumentTypeTitle,
                                                        '' AS DocumentOwnershipTypeTitle,
                                                        '' AS DocumentRootTypeTitle,
                                                        '' AS StateName,
                                                        '' AS CityName,
                                                        '' AS ZoneName,
                                                        '' AS DistrictName,
                                                        '' AS UserCreatorName,
                                                        p.IsActivated,
                                                        p.IsDeleted,
                                                        p.ConsultantUserId,
                                                        p.EditEnDate,
                                                        p.RebuiltInYearFa,
                                                        p.CreateEnDate,
                                                        p.PropertyTypeId,
                                                        p.TypeOfUseId,
                                                        p.DocumentOwnershipTypeId,
                                                        p.DocumentTypeId,
                                                        p.DocumentRootTypeId,
                                                        p.PropertyCodeName AS AdvertisementTitle,
                                                        p.Area,
                                                        p.PropertyDescriptions AS AdvertisementDescriptions,
                                                        p.UserIdCreator,
                                                        p.ShowInMelkavan,
                                                        propAddr.CountryId,
                                                        propAddr.StateId,
                                                        propAddr.CityId,
                                                        propAddr.ZoneId,
                                                        propAddr.DistrictId,
                                                        propAddr.TownName,
                                                        propPriceHist.OfferPrice,
                                                        propPriceHist.DepositPrice,
                                                        propPriceHist.RentPrice,
                                                        '' AS CurrentDate,
                                                        pd.TagId,
                                                        CAST('false' AS BIT) AS IsFavorite
                                                   FROM TeniacoDb.dbo.Properties AS p
                                                        INNER JOIN  TeniacoDb.dbo.PropertyOwners on p.PropertyId = TeniacoDb.dbo.PropertyOwners.PropertyId
                                                        INNER JOIN TeniacoDb.dbo.PropertyAddress AS propAddr ON p.PropertyId = propAddr.PropertyId
                                                        INNER JOIN (
                                                            SELECT PropertyId, CASE OfferPriceType WHEN '0' THEN CalculatedOfferPrice ELSE OfferPrice END as OfferPrice, DepositPrice, RentPrice
                                                                    FROM TeniacoDb.dbo.PropertiesPricesHistories AS pph
                                                                            WHERE pph.PropertyPriceHistoryId = (
                                                                                SELECT MAX(PropertyPriceHistoryId)
                                                                                FROM TeniacoDb.dbo.PropertiesPricesHistories
                                                                                WHERE PropertyId = pph.PropertyId
                                                                            )
                                                        ) AS propPriceHist ON p.PropertyId = propPriceHist.PropertyId
                                                        INNER JOIN TeniacoDb.dbo.PropertiesDetails AS pd ON p.PropertyId = pd.PropertyId
                                                        WHERE p.ShowInMelkavan = 1 and (p.UserIdCreator = {owenerId} or p.AdvertiserId = {owenerId} or PropertyOwners.OwnerId={owenerId} or p.ConsultantUserId={owenerId})
                                                    ) AS tmp
                                                    ORDER BY CreateEnDate DESC;";

                sp = sp.Replace("{owenerId}", owenerId.ToString());

                var list1 = melkavanApiDb.AdvertisementAdvanceSearchVM.FromSqlRaw(sp).AsEnumerable().Distinct();


                #region ChildUsers
                //if (childsUsersIds != null)
                //{
                //    if (childsUsersIds.Count > 1)
                //    {
                //        list1 = list1.Where(p => p.UserIdCreator.HasValue).Where(p => childsUsersIds.Contains(p.UserIdCreator.Value));
                //    }
                //    else
                //    {
                //        if (childsUsersIds.Count == 1)
                //        {
                //            if (childsUsersIds.FirstOrDefault() > 0)
                //            {
                //                list1 = list1.Where(p => p.UserIdCreator.HasValue).Where(p => childsUsersIds.Contains(p.UserIdCreator.Value));
                //            }
                //        }
                //    }
                //}
                #endregion

                #region Pagination and Load extra title data

                try
                {
                    if (string.IsNullOrEmpty(jtSorting))
                    {
                        listCount = list1.Count();

                        //if (listCount > jtPageSize)
                        //{
                        list1 = list1.OrderByDescending(s => s.AdvertisementId)
                                 .Skip(jtStartIndex).Take(jtPageSize).ToList();
                        //}
                        //else
                        //{


                        //    list1 = list1.OrderByDescending(s => s.AdvertisementId).ToList();
                        //}
                    }
                    else
                    {
                        listCount = list1.Count();

                        if (listCount > jtPageSize)
                        {
                            switch (jtSorting)
                            {
                                case "AdvertisementTitle ASC":
                                    list1 = list1.OrderBy(l => l.AdvertisementTitle);
                                    break;
                                case "AdvertisementTitle DESC":
                                    list1 = list1.OrderByDescending(l => l.AdvertisementTitle);
                                    break;
                            }

                            if (string.IsNullOrEmpty(jtSorting))
                                advertisementAdvanceSearchVMList = list1.OrderByDescending(s => s.AdvertisementId)
                                         .Skip(jtStartIndex).Take(jtPageSize).ToList();
                            else
                                advertisementAdvanceSearchVMList = list1.Skip(jtStartIndex).Take(jtPageSize).ToList();
                        }
                        else
                        {

                            advertisementAdvanceSearchVMList = list1.ToList();
                        }
                    }





                    var stateIds = list1.Where(p => p.StateId.HasValue).Select(p => p.StateId.Value).ToList();
                    var states = publicApiDb.States.Where(s => stateIds.Contains(s.StateId)).ToList();

                    var cityIds = list1.Where(p => p.CityId.HasValue).Select(p => p.CityId.Value).ToList();
                    var cities = publicApiDb.Cities.Where(s => cityIds.Contains(s.CityId)).ToList();

                    var zoneIds = list1.Where(p => p.ZoneId.HasValue).Select(p => p.ZoneId.Value).ToList();
                    var zones = publicApiDb.Zones.Where(s => zoneIds.Contains(s.ZoneId)).ToList();

                    var districtIds = list1.Where(p => p.DistrictId.HasValue).Select(p => p.DistrictId.Value).ToList();
                    var districts = publicApiDb.Districts.Where(s => districtIds.Contains(s.DistrictId)).ToList();

                    var propertyIds = list1.Select(p => p.AdvertisementId).ToList().Distinct();

                    var advertisementIds = list1.Select(c => c.AdvertisementId).ToList().Distinct();

                    var advertisementFilesList = melkavanApiDb.AdvertisementFiles.Where(a => advertisementIds.Contains(a.AdvertisementId) && a.AdvertisementFileType.Equals("media")).ToList();

                    var propertiesFilesList = teniacoApiDb.PropertyFiles.Where(z => advertisementIds.Contains(z.PropertyId) && z.PropertyFileType.Equals("media")).ToList();


                    var tmpList = list1.Distinct();

                    foreach (var item in tmpList)
                    {


                        var state = states.Where(s => s.StateId.Equals(item.StateId)).FirstOrDefault();
                        if (state != null)
                        {
                            item.StateName = state.StateName;
                        }

                        var city = cities.Where(c => c.CityId.Equals(item.CityId)).FirstOrDefault();
                        if (city != null)
                        {
                            item.CityName = city.CityName;
                        }

                        if (item.ZoneId.HasValue)
                        {
                            var zone = zones.Where(z => z.ZoneId.Equals(item.ZoneId.Value)).FirstOrDefault();
                            if (zone != null)
                            {
                                item.ZoneName = zone.ZoneName;
                            }
                        }

                        if (item.DistrictId.HasValue)
                        {
                            var district = districts.Where(z => z.DistrictId.Equals(item.DistrictId.Value)).FirstOrDefault();
                            if (district != null)
                            {
                                item.DistrictName = district.DistrictName;

                            }
                        }

                        if (owenerId != 0 &&
                            melkavanApiDb.AdvertisementFavorites.Where(a => a.UserIdCreator == owenerId && a.AdvertisementId == item.AdvertisementId && a.IsDeleted != true).Any())
                        {
                            item.IsFavorite = true;
                        }
                        else
                        {
                            if (owenerId != 0 &&
                                teniacoApiDb.PropertiesFavorites.Where(a => a.UserIdCreator == owenerId && a.PropertyId == item.AdvertisementId && a.IsDeleted != true).Any())
                            {
                                item.IsFavorite = true;
                            }
                        }


                        if (advertisementFilesList.Where(ad => ad.AdvertisementId == item.AdvertisementId).Any())
                        {
                            try
                            {
                                item.AdvertisementFilesVM = advertisementFilesList.Where(ad => ad.AdvertisementId == item.AdvertisementId)
                                    .Select(p => new AdvertisementFilesVM
                                    {
                                        AdvertisementId = p.AdvertisementId,
                                        AdvertisementFilePath = p.AdvertisementFilePath,
                                        AdvertisementFileExt = p.AdvertisementFileExt,
                                        AdvertisementFileTitle = p.AdvertisementFileTitle,
                                        AdvertisementFileId = p.AdvertisementFileId,
                                        AdvertisementFileType = p.AdvertisementFileType,
                                    }).OrderByDescending(f => f.AdvertisementFileId).ToList();
                            }
                            catch (Exception exc)
                            { }
                        }
                        else
                        {
                            if (propertiesFilesList.Where(ad => ad.PropertyId == item.AdvertisementId).Any())
                            {
                                try
                                {
                                    item.AdvertisementFilesVM = propertiesFilesList.Where(ad => ad.PropertyId == item.AdvertisementId)
                                        .Select(p => new AdvertisementFilesVM
                                        {
                                            AdvertisementId = p.PropertyId,
                                            AdvertisementFilePath = p.PropertyFilePath,
                                            AdvertisementFileExt = p.PropertyFileExt,
                                            AdvertisementFileTitle = p.PropertyFileTitle,
                                            AdvertisementFileId = p.PropertyFileId,
                                            AdvertisementFileType = p.PropertyFileType,
                                        }).OrderByDescending(f => f.AdvertisementFileId).ToList();
                                }
                                catch (Exception exc)
                                { }
                            }
                        }
                    }



                    advertisementAdvanceSearchVMList = tmpList.Distinct().ToList();

                }
                catch (Exception exc)
                { }

                #endregion


            }
            catch (Exception exc)
            { }



            return advertisementAdvanceSearchVMList;
        }


        public List<AdvertisementVM> GetListOfAdvertisementWithMoreDetail(
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
            )
        {

            List<AdvertisementVM> AdvertisementVMList = new List<AdvertisementVM>();
            var states = publicApiDb.States.ToList();
            var cities = publicApiDb.Cities.ToList();
            var zones = publicApiDb.Zones.ToList();
            var districts = publicApiDb.Districts.ToList();




            var list = (from p in melkavanApiDb.Advertisement
                        join pe in melkavanApiDb.AdvertisementOwners on p.AdvertisementId equals pe.AdvertisementId
                        where
                        //childsUsersIds.Contains(p.UserIdCreator.Value) &&
                        p.IsDeleted.Value.Equals(false) &&
                        p.IsActivated.Value.Equals(true)
                        select new AdvertisementVM
                        {
                            Area = p.Area,
                            BuiltInYear = p.BuiltInYear.HasValue ? p.BuiltInYear.Value : (int?)0,
                            BuiltInYearFa = p.BuiltInYearFa.HasValue ? p.BuiltInYearFa.Value : (int?)0,
                            //IntermediaryPersonId = p.IntermediaryPersonId.HasValue ? p.IntermediaryPersonId.Value : (long?)null,
                            OwnerId = pe.OwnerId != null ? pe.OwnerId : (long?)null,
                            AdvertisementTitle = p.AdvertisementTitle,
                            AdvertisementId = p.AdvertisementId,
                            PropertyTypeId = p.PropertyTypeId,
                            RebuiltInYear = p.BuiltInYear.HasValue ? p.BuiltInYear.Value : (int?)0,
                            RebuiltInYearFa = p.RebuiltInYearFa.HasValue ? p.RebuiltInYearFa.Value : (int?)0,
                            TypeOfUseId = p.TypeOfUseId.HasValue ? p.TypeOfUseId.Value : (int?)0,
                            DocumentTypeId = p.DocumentTypeId.HasValue ? p.DocumentTypeId.Value : (int?)0,
                            DocumentOwnershipTypeId = p.DocumentOwnershipTypeId.HasValue ? p.DocumentOwnershipTypeId.Value : (int?)0,
                            DocumentRootTypeId = p.DocumentRootTypeId.HasValue ? p.DocumentRootTypeId.Value : (int?)0,
                            AdvertisementDescriptions = !string.IsNullOrEmpty(p.AdvertisementDescriptions) ? p.AdvertisementDescriptions : "",
                            CurrentDate = DateTime.Now,
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

            if (advertisementTypeId.HasValue)
                if (advertisementTypeId.Value > 0)
                    list = list.Where(a => a.AdvertisementDetailsVM.AdvertisementTypeId.Equals(advertisementTypeId.Value));


            if (propertyTypeId.HasValue)
                if (propertyTypeId.Value > 0)
                    list = list.Where(a => a.PropertyTypeId.Equals(propertyTypeId.Value));


            if (typeOfUseId.HasValue)
                if (typeOfUseId.Value > 0)
                    list = list.Where(a => a.TypeOfUseId.Equals(typeOfUseId.Value));


            if (documentTypeId.HasValue)
                if (documentTypeId.Value > 0)
                    list = list.Where(a => a.DocumentTypeId.Equals(documentTypeId.Value));


            if (!string.IsNullOrEmpty(propertyCodeName))
                list = list.Where(z => z.AdvertisementTitle.Contains(propertyCodeName));


            if (stateId.HasValue)
                if (stateId.Value > 0)
                    list = list.Where(a => a.AdvertisementAddressVM.StateId.Equals(stateId.Value));


            if (cityId.HasValue)
                if (cityId.Value > 0)
                    list = list.Where(a => a.AdvertisementAddressVM.CityId.Equals(cityId.Value));


            if (zoneId.HasValue)
                if (zoneId.Value > 0)
                    list = list.Where(a => a.AdvertisementAddressVM.ZoneId.Equals(zoneId.Value));


            if (districtId.HasValue)
                if (districtId.Value > 0)
                    list = list.Where(a => a.AdvertisementAddressVM.DistrictId.Equals(districtId.Value));




            if (!string.IsNullOrEmpty(advertisementTitle))
                list = list.Where(a => a.AdvertisementTitle.Contains(advertisementTitle));

            try
            {
                if (string.IsNullOrEmpty(jtSorting))
                {
                    listCount = list.Count();

                    if (listCount >= jtPageSize)
                    {
                        AdvertisementVMList = list.OrderByDescending(s => s.AdvertisementId)
                        .Skip(jtStartIndex).Take(jtPageSize).ToList();
                    }
                    else
                    {
                        AdvertisementVMList = list.OrderByDescending(s => s.AdvertisementId).ToList();
                    }
                }
                else
                {
                    listCount = list.Count();

                    if (listCount > jtPageSize)
                    {
                        switch (jtSorting)
                        {
                            case "AdvertisementTitle ASC":
                                list = list.OrderBy(l => l.AdvertisementTitle);
                                break;
                            case "AdvertisementTitle DESC":
                                list = list.OrderByDescending(l => l.AdvertisementTitle);
                                break;
                        }

                        if (string.IsNullOrEmpty(jtSorting))
                            AdvertisementVMList = list.OrderByDescending(s => s.AdvertisementId)
                                     .Skip(jtStartIndex).Take(jtPageSize).ToList();
                        else
                            AdvertisementVMList = list.Skip(jtStartIndex).Take(jtPageSize).ToList();
                    }
                    else
                    {
                        AdvertisementVMList = list.ToList();
                    }
                }

                var advertisementIds = AdvertisementVMList.Select(a => a.AdvertisementId).ToList();

                var advertisementPriceHistories = melkavanApiDb.AdvertisementPricesHistories.Where(a => advertisementIds.Contains(a.AdvertisementId)).ToList();
                if (HaveFavorit && userId.Value > 0)
                {
                    List<long> userFavoritesList = new List<long>();


                    if (melkavanApiDb.AdvertisementFavorites.Where(a => a.UserIdCreator == userId).Any())
                    {
                        userFavoritesList = melkavanApiDb.AdvertisementFavorites.Where(a => a.UserIdCreator == userId && a.IsDeleted != true).Select(a => a.AdvertisementId).ToList();
                    }

                    list = list.Where(a => userFavoritesList.Contains(a.AdvertisementId));
                }
                List<AdvertisementDetailsVM> advertisementDetailsList = new List<AdvertisementDetailsVM>();

                if (HaveDetails)
                {
                    advertisementDetailsList = _mapper.Map<List<AdvertisementDetails>, List<AdvertisementDetailsVM>>(
                        melkavanApiDb.AdvertisementDetails.Where(a => advertisementIds.Contains(a.AdvertisementId)).ToList());
                }

                List<AdvertisementFiles> advertisementFilesList = new List<AdvertisementFiles>();
                if (HaveFiles)
                {
                    advertisementFilesList = melkavanApiDb.AdvertisementFiles.Where(a => advertisementIds.Contains(a.AdvertisementId)).ToList();
                }
                if (HaveCallers)
                {
                    // TODO
                }
                if (HaveTags)
                {
                    // TODO
                }
                if (HaveViewers)
                {
                    // TODO
                }

                //documentTypeIds
                var documentTypeIds = AdvertisementVMList.Select(a => a.DocumentTypeId).Distinct().ToList();
                var documentTypes = teniacoApiContext.DocumentTypes.Where(d => documentTypeIds.Contains(d.DocumentTypeId)).ToList();
                //typeOfUsesIds
                var typeOfUsesIds = AdvertisementVMList.Select(a => a.TypeOfUseId).Distinct().ToList();
                var typeOfUses = teniacoApiContext.TypeOfUses.Where(d => typeOfUsesIds.Contains(d.TypeOfUseId)).ToList();
                //propertyTypes
                var propertyTypesIds = AdvertisementVMList.Select(a => a.PropertyTypeId).Distinct().ToList();
                var propertyTypes = teniacoApiContext.PropertyTypes.Where(d => propertyTypesIds.Contains(d.PropertyTypeId)).ToList();
                //documentRootTypes
                var documentRootTypesIds = AdvertisementVMList.Select(a => a.DocumentRootTypeId).Distinct().ToList();
                var documentRootTypes = teniacoApiContext.DocumentRootTypes.Where(d => documentRootTypesIds.Contains(d.DocumentRootTypeId)).ToList();
                //documentOwnershipTypes
                var documentOwnershipTypesIds = AdvertisementVMList.Select(a => a.DocumentOwnershipTypeId).Distinct().ToList();
                var documentOwnershipTypes = teniacoApiContext.DocumentOwnershipTypes.Where(d => documentOwnershipTypesIds.Contains(d.DocumentOwnershipTypeId)).ToList();

                string sp = @"select ROW_NUMBER() OVER (ORDER BY (SELECT 1)) as AdvertisementDataTypeCountId, AdvertisementId, DataType, Count
from

(
SELECT AdvertisementId, AdvertisementFileType as DataType, COUNT(AdvertisementFileType) as Count
  FROM AdvertisementFiles
  group by AdvertisementId, AdvertisementFileType
  union all
  select AdvertisementId, 'price' DataType, count(AdvertisementPriceHistoryId) as Count
  from AdvertisementPricesHistories
    group by AdvertisementId
  union all
  select AdvertisementId, 'view' DataType, count(AdvertisementViewersId) as Count
  from AdvertisementViewers
    group by AdvertisementId
) as Counts";

                var advertisementDataTypeCounts = melkavanApiDb.AdvertisementDataTypeCounts.FromSqlRaw(sp).Where(p => advertisementIds.Contains(p.AdvertisementId)).ToList();

                foreach (var item in AdvertisementVMList)
                {
                    if (advertisementDataTypeCounts.Where(p => p.AdvertisementId.Equals(item.AdvertisementId)).Any())
                    {
                        var advertisementDataTypeCount = advertisementDataTypeCounts.Where(p => p.AdvertisementId.Equals(item.AdvertisementId)).ToList();

                        item.CountOfDocs = advertisementDataTypeCount.Where(p => p.DataType.Equals("docs")).FirstOrDefault()?.Count;
                        item.CountOfMaps = advertisementDataTypeCount.Where(p => p.DataType.Equals("maps")).FirstOrDefault()?.Count;
                        item.CountOfMedia = advertisementDataTypeCount.Where(p => p.DataType.Equals("media")).FirstOrDefault()?.Count;
                        item.CountOfPrices = advertisementDataTypeCount.Where(p => p.DataType.Equals("price")).FirstOrDefault()?.Count;
                        item.ViewersTotalCount = advertisementDataTypeCount.Where(p => p.DataType.Equals("view")).FirstOrDefault()?.Count;
                    }

                    if (HaveAddress)
                    {
                        try
                        {
                            var AdvertisementAddress = melkavanApiDb.AdvertisementAddress
                           .Where(a => a.AdvertisementId == item.AdvertisementId)
                           .Select(pa => new AdvertisementAddressVM
                           {
                               StateId = pa.StateId,
                               TempStateId = pa.TempStateId,
                               CityId = pa.CityId,
                               TempCityId = pa.TempCityId,
                               ZoneId = pa.ZoneId,
                               TempZoneId = pa.TempZoneId,
                               DistrictId = pa.DistrictId,
                               StateName = "",
                               TempStateName = "",
                               CityName = "",
                               TempCityName = "",
                               ZoneName = "",
                               TempZoneName = "",
                               DistrictName = "",
                               TownName = pa.TownName,
                               Address = !string.IsNullOrEmpty(pa.Address) ? pa.Address : "",
                               LocationLat = pa.LocationLat,
                               LocationLon = pa.LocationLon,
                               AdvertisementId = pa.AdvertisementId,
                               UserIdCreator = pa.UserIdCreator,
                               CreateEnDate = pa.CreateEnDate,
                               CreateTime = pa.CreateTime,
                               EditEnDate = pa.EditEnDate,
                               EditTime = pa.EditTime,
                               UserIdEditor = pa.UserIdEditor,
                               RemoveEnDate = pa.RemoveEnDate,
                               RemoveTime = pa.RemoveTime,
                               UserIdRemover = pa.UserIdRemover,
                               IsActivated = pa.IsActivated,
                               IsDeleted = pa.IsDeleted,
                           })
                           .FirstOrDefault();

                            if (AdvertisementAddress != null)
                            {
                                var state = states.Where(s => s.StateId.Equals(AdvertisementAddress.StateId)).FirstOrDefault();
                                if (state != null)
                                {
                                    AdvertisementAddress.StateId = state.StateId;
                                    AdvertisementAddress.StateName = state.StateName;
                                }

                                var city = cities.Where(c => c.CityId.Equals(AdvertisementAddress.CityId)).FirstOrDefault();
                                if (city != null)
                                {
                                    AdvertisementAddress.CityId = city.CityId;
                                    AdvertisementAddress.CityName = city.CityName;
                                }

                                if (AdvertisementAddress.ZoneId.HasValue)
                                {
                                    var zone = zones.Where(z => z.ZoneId.Equals(AdvertisementAddress.ZoneId.Value)).FirstOrDefault();
                                    if (zone != null)
                                    {
                                        AdvertisementAddress.ZoneId = zone.ZoneId;
                                        AdvertisementAddress.ZoneName = zone.ZoneName;
                                    }
                                }

                                if (AdvertisementAddress.DistrictId.HasValue)
                                {
                                    var district = districts.Where(z => z.DistrictId.Equals(AdvertisementAddress.DistrictId.Value)).FirstOrDefault();
                                    if (district != null)
                                    {
                                        AdvertisementAddress.DistrictId = district.DistrictId;
                                        AdvertisementAddress.DistrictName = district.DistrictName;
                                        AdvertisementAddress.TownName = district.TownName;
                                    }
                                }
                                item.AdvertisementAddressVM = AdvertisementAddress;
                            }
                        }
                        catch (Exception exc)
                        { }
                    }
                    if (HaveDetails)
                    {
                        if (advertisementDetailsList.Where(ad => ad.AdvertisementId.Equals(item.AdvertisementId)).Any())
                        {
                            try
                            {
                                item.AdvertisementDetailsVM = advertisementDetailsList.Where(ad => ad.AdvertisementId.Equals(item.AdvertisementId)).FirstOrDefault();
                            }

                            catch (Exception exc)
                            {
                                item.AdvertisementDetailsVM = new AdvertisementDetailsVM();
                            }
                        }
                        else
                        {
                            item.AdvertisementDetailsVM = new AdvertisementDetailsVM();
                        }
                    }
                    if (advertisementPriceHistories.Where(ad => ad.AdvertisementId == item.AdvertisementId).Any())
                    {
                        try
                        {
                            var advertisementPricesHistoriesVM = advertisementPriceHistories.Where(ad => ad.AdvertisementId == item.AdvertisementId).
                                OrderByDescending(a => a.CreateEnDate).ThenByDescending(a => a.CreateTime).FirstOrDefault();

                            item.AdvertisementPricesHistoriesVM = new AdvertisementPricesHistoriesVM();
                            item.AdvertisementPricesHistoriesVM.AdvertisementId = advertisementPricesHistoriesVM.AdvertisementId;
                            item.AdvertisementPricesHistoriesVM.OfferPrice = advertisementPricesHistoriesVM.OfferPrice;
                            item.AdvertisementPricesHistoriesVM.RentPrice = advertisementPricesHistoriesVM.RentPrice;
                            item.AdvertisementPricesHistoriesVM.DepositPrice = advertisementPricesHistoriesVM.DepositPrice;
                            item.AdvertisementPricesHistoriesVM.CalculatedOfferPrice = advertisementPricesHistoriesVM.CalculatedOfferPrice;
                            item.AdvertisementPricesHistoriesVM.OfferPriceType = advertisementPricesHistoriesVM.OfferPriceType;
                            item.AdvertisementPricesHistoriesVM.CreateEnDate = advertisementPricesHistoriesVM.CreateEnDate;
                            item.AdvertisementPricesHistoriesVM.CreateTime = advertisementPricesHistoriesVM.CreateTime;
                            //item.AdvertisementPricesHistoriesVM.EditEnDate = advertisementPricesHistoriesVM.EditEnDate;
                            //item.AdvertisementPricesHistoriesVM.EditTime = advertisementPricesHistoriesVM.EditTime;
                            //item.AdvertisementPricesHistoriesVM.UserIdEditor = advertisementPricesHistoriesVM.UserIdEditor.Value;
                            //item.AdvertisementPricesHistoriesVM.RemoveEnDate = advertisementPricesHistoriesVM.RemoveEnDate;
                            //item.AdvertisementPricesHistoriesVM.RemoveTime = advertisementPricesHistoriesVM.EditTime;
                            //item.AdvertisementPricesHistoriesVM.UserIdRemover = advertisementPricesHistoriesVM.UserIdRemover.Value;
                            item.AdvertisementPricesHistoriesVM.IsActivated = advertisementPricesHistoriesVM.IsActivated;
                            item.AdvertisementPricesHistoriesVM.IsDeleted = advertisementPricesHistoriesVM.IsDeleted;
                            //.Select(p => new AdvertisementPricesHistoriesVM
                            //{
                            //    AdvertisementId = p.AdvertisementId,
                            //    OfferPrice = p.OfferPrice,
                            //    RentPrice = p.RentPrice,
                            //    DepositPrice = p.DepositPrice,
                            //    CalculatedOfferPrice = p.CalculatedOfferPrice,
                            //    OfferPriceType = p.OfferPriceType,
                            //    CreateEnDate = p.CreateEnDate,
                            //    CreateTime = p.CreateTime,
                            //    EditEnDate = p.EditEnDate,
                            //    EditTime = p.EditTime,
                            //    UserIdEditor = p.UserIdEditor.Value,
                            //    RemoveEnDate = p.RemoveEnDate,
                            //    RemoveTime = p.EditTime,
                            //    UserIdRemover = p.UserIdRemover.Value,
                            //    IsActivated = p.IsActivated,
                            //    IsDeleted = p.IsDeleted,
                            //}).OrderByDescending(a => a.CreateEnDate).ThenByDescending(a => a.CreateTime).FirstOrDefault();
                        }
                        catch (Exception exc)
                        { }
                    }
                    if (HaveFiles)
                    {
                        if (advertisementFilesList.Where(ad => ad.AdvertisementId == item.AdvertisementId).Any())
                        {
                            try
                            {
                                item.AdvertisementFilesVM = advertisementFilesList.Where(ad => ad.AdvertisementId == item.AdvertisementId)
                               .Select(p => new AdvertisementFilesVM
                               {
                                   AdvertisementId = p.AdvertisementId,
                                   AdvertisementFilePath = p.AdvertisementFilePath,
                                   AdvertisementFileExt = p.AdvertisementFileExt,
                                   AdvertisementFileTitle = p.AdvertisementFileTitle,
                                   AdvertisementFileId = p.AdvertisementFileId,
                                   AdvertisementFileType = p.AdvertisementFileType,
                                   //CreateEnDate = p.CreateEnDate,
                                   //CreateTime = p.CreateTime,
                                   //EditEnDate = p.EditEnDate,
                                   //EditTime = p.EditTime,
                                   //UserIdEditor = p.UserIdEditor.Value,
                                   //RemoveEnDate = p.RemoveEnDate,
                                   //RemoveTime = p.EditTime,
                                   //UserIdRemover = p.UserIdRemover.Value,
                                   //IsActivated = p.IsActivated,
                                   //IsDeleted = p.IsDeleted,
                               }).OrderByDescending(f => f.AdvertisementFileId).ToList();
                            }
                            catch (Exception exc)
                            { }
                        }
                        else
                        {
                            item.AdvertisementFilesVM = new List<AdvertisementFilesVM>();
                        }
                    }

                    if (item.DocumentTypeId.HasValue)
                    {
                        if (documentTypes.Where(ad => ad.DocumentTypeId == item.DocumentTypeId).Any())
                        {
                            try
                            {
                                item.DocumentTypeTitle = documentTypes
                                                       .Where(a => a.DocumentTypeId == item.DocumentTypeId).Select(a => a.DocumentTypeTitle).FirstOrDefault();
                            }
                            catch (Exception exc)
                            { }
                        }
                        else
                        {
                            item.DocumentTypeTitle = String.Empty;
                        }

                    }

                    if (item.DocumentOwnershipTypeId.HasValue)
                    {
                        if (documentOwnershipTypes.Where(ad => ad.DocumentOwnershipTypeId == item.DocumentOwnershipTypeId).Any())
                        {
                            try
                            {
                                item.DocumentOwnershipTypeTitle = documentOwnershipTypes
                                      .Where(a => a.DocumentOwnershipTypeId == item.DocumentOwnershipTypeId).Select(a => a.DocumentOwnershipTypeTitle).FirstOrDefault();

                            }
                            catch (Exception exc)
                            { }
                        }
                        else
                        {
                            item.DocumentOwnershipTypeTitle = String.Empty;
                        }
                    }

                    if (item.DocumentRootTypeId.HasValue)
                    {

                        if (documentOwnershipTypes.Where(ad => ad.DocumentOwnershipTypeId == item.DocumentOwnershipTypeId).Any())
                        {
                            try
                            {
                                item.DocumentRootTypeTitle = documentRootTypes
                               .Where(a => a.DocumentRootTypeId == item.DocumentRootTypeId).Select(a => a.DocumentRootTypeTitle).FirstOrDefault();
                            }
                            catch (Exception exc)
                            { }
                        }
                        else
                        {
                            item.DocumentRootTypeTitle = String.Empty;
                        }
                    }

                    //item.ViewersTotalCount = melkavanApiDb.AdvertisementViewers.Where(x => x.AdvertisementId == item.AdvertisementId).Count();

                    if (propertyTypes.Where(ad => ad.PropertyTypeId == item.PropertyTypeId).Any())
                    {
                        try
                        {
                            var resultOfPropertyTypeTilte = propertyTypes.FirstOrDefault(x => x.PropertyTypeId == item.PropertyTypeId);
                            item.PropertyTypeTilte = resultOfPropertyTypeTilte.PropertyTypeTilte;
                        }
                        catch (Exception exc)
                        { }
                    }
                    else
                    {
                        item.PropertyTypeTilte = String.Empty;
                    }

                    if (item.TypeOfUseId.HasValue)
                    {
                        if (typeOfUses.Where(ad => ad.TypeOfUseId == item.TypeOfUseId).Any())
                        {
                            try
                            {
                                var resultOfTypeUseTitle = typeOfUses.FirstOrDefault(x => x.TypeOfUseId == item.TypeOfUseId);
                                item.TypeUseTitle = resultOfTypeUseTitle.TypeOfUseTitle;
                            }
                            catch (Exception exc)
                            { }
                        }
                        else
                        {
                            item.TypeUseTitle = String.Empty;
                        }
                    }

                    if (item.AdvertisementDetailsVM.BuildingLifeId.HasValue)
                    {
                        try
                        {
                            var resultOfBuildingLifeTitle = melkavanApiDb.BuildingLifes.First(x => x.BuildingLifeId == item.AdvertisementDetailsVM.BuildingLifeId);
                            item.AdvertisementDetailsVM.BuildingLifesVM = new BuildingLifesVM
                            {
                                BuildingLifeTitle = resultOfBuildingLifeTitle.BuildingLifeTitle,
                            };
                        }
                        catch (Exception exc)
                        { }
                    }

                    if (HaveFeature)
                    {

                        try
                        {
                            item.AdvertisementFeaturesValuesVM = new List<AdvertisementFeaturesValuesVM>();
                            var advertisementFeaturesValues = melkavanApiDb.AdvertisementFeaturesValues.Where(a => a.AdvertisementId == item.AdvertisementId).ToList();

                            var featureIds = advertisementFeaturesValues.Select(c => c.FeatureId).ToList();
                            var features = teniacoApiContext.Features.Where(a => featureIds.Contains(a.FeatureId)).ToList();

                            var featureOptions = teniacoApiContext.FeaturesOptions.Where(a => featureIds.Contains(a.FeatureId)).ToList();

                            foreach (var featuresValuesItem in advertisementFeaturesValues)
                            {
                                var advertisementFeaturesValuesVM = new AdvertisementFeaturesValuesVM();
                                var feature = _mapper.Map<Features, FeaturesVM>(features.Where(a => a.FeatureId == featuresValuesItem.FeatureId).FirstOrDefault());
                                if (featureOptions.Where(c => c.FeatureId.Equals(featuresValuesItem.FeatureId)).Any())
                                {
                                    var options = featureOptions.Where(c => c.FeatureId.Equals(featuresValuesItem.FeatureId)).ToList();
                                    feature.FeaturesOptionsVM = _mapper.Map<List<FeaturesOptions>, List<FeaturesOptionsVM>>
                                   (options);
                                }
                                var featureValue = "";
                                switch (feature.ElementTypeId)
                                {
                                    case 1:
                                    case 4:
                                        featureValue += featuresValuesItem.FeatureValue;
                                        break;
                                    case 2:
                                        if (!string.IsNullOrEmpty(featuresValuesItem.FeatureValue))
                                        {
                                            featureValue += feature.FeaturesOptionsVM.Where(a => a.FeatureOptionValue == int.Parse(featuresValuesItem.FeatureValue)).Select(a => a.FeatureOptionText).FirstOrDefault();
                                            featureValue += ';';
                                        }
                                        break;
                                    case 3:
                                        var selectedOptionValues = featuresValuesItem.FeatureValue.Split(',');
                                        foreach (var selectedOptionValue in selectedOptionValues)
                                        {
                                            if (!string.IsNullOrEmpty(selectedOptionValue))
                                            {
                                                featureValue += feature.FeaturesOptionsVM.Where(a => a.FeatureOptionValue == int.Parse(selectedOptionValue)).Select(a => a.FeatureOptionText).FirstOrDefault();
                                                featureValue += ';';
                                            }
                                        }
                                        break;
                                }
                                advertisementFeaturesValuesVM.FeatureValue = featureValue;

                                advertisementFeaturesValuesVM.FeaturesVMList.Add(new FeaturesVM
                                {
                                    FeatureTitle = feature.FeatureTitle,
                                    FeatureId = feature.FeatureId,
                                });
                                item.AdvertisementFeaturesValuesVM.Add(advertisementFeaturesValuesVM);
                            }
                        }
                        catch (Exception exc)
                        { }
                    }
                    if (HaveFavorit)
                    {
                        try
                        {
                            if (userId.HasValue && melkavanApiDb.AdvertisementFavorites.Where(a => a.UserIdCreator == userId && a.AdvertisementId == item.AdvertisementId && a.IsDeleted != true).Any())
                            {
                                item.IsFavorite = true;
                            }
                            else
                            {
                                item.IsFavorite = null;
                            }
                        }
                        catch (Exception exc)
                        { }
                    }

                }
            }
            catch (Exception exc)
            { }

            return AdvertisementVMList;
        }



        public long AddOwnerOrConcultant(
            AddOwnerOrConcultantPVM addOwnerOrConcultantPVM,
             IConsoleBusiness consoleBusiness)
        {


            var userName = addOwnerOrConcultantPVM.Phone;
            long userId = 0;

            int LevelId = consoleBusiness.CmsDb.Levels.Where(l => l.LevelName.Equals(addOwnerOrConcultantPVM.Type)).Select(l => l.LevelId).FirstOrDefault();
            int domainSettingId = consoleBusiness.CmsDb.DomainsSettings.Where(d => d.DomainName.Equals("melkavan.com")).Select(d => d.DomainSettingId).FirstOrDefault();
            long parentUserId = consoleBusiness.CmsDb.Users.Where(u => u.UserName == "اعضای ملکاوان").Select(u => u.UserId).FirstOrDefault();



            if (userName != null)
            {
                if (!consoleBusiness.CmsDb.Users.Where(u => u.UserName.Equals(userName)).Any())   //اگر این کاربر در ملکاوان وجود نداشته باشد
                {

                    using (var transaction = consoleBusiness.CmsDb.Database.BeginTransaction())
                    {
                        try
                        {
                            #region Add User

                            Users users = new Users();
                            users.UserName = addOwnerOrConcultantPVM.Phone;
                            users.Password = FrameWork.MD5Hash.GetMD5Hash(addOwnerOrConcultantPVM.Phone);
                            users.DomainSettingId = domainSettingId;
                            users.UserIdCreator = addOwnerOrConcultantPVM.UserIdCreator; //this.userId
                            users.IsActivated = true;
                            users.IsDeleted = false;
                            users.CreateEnDate = DateTime.Now;
                            users.CreateTime = PersianDate.TimeNow;
                            users.ParentUserId = parentUserId; //والد


                            consoleBusiness.CmsDb.Users.Add(users);
                            consoleBusiness.CmsDb.SaveChanges();


                            userId = users.UserId;

                            melkavanApiDb.SaveChanges();

                            #endregion

                            #region Add UserProfile
                            UsersProfile usersProfile = new UsersProfile()
                            {
                                UserId = userId,
                                Address = "",
                                Age = 0,
                                BirthDateTimeEn = DateTime.Now,
                                CertificateId = "" +
                                "",
                                CreateEnDate = DateTime.Now,
                                CreateTime = PersianDate.TimeNow,
                                CreditCardNumber = "",
                                Email = "",
                                Family = addOwnerOrConcultantPVM.Family != null ? addOwnerOrConcultantPVM.Family : "",
                                HasModified = false,
                                IsActivated = true,
                                IsDeleted = false,
                                Mobile = addOwnerOrConcultantPVM.Phone,
                                Name = addOwnerOrConcultantPVM.Name != null ? addOwnerOrConcultantPVM.Name : "",
                                NationalCode = "",
                                Phone = addOwnerOrConcultantPVM.Phone,
                                Picture = "",
                                PostalCode = "",
                                Sexuality = false,
                                SocialNetworkAddress = "",
                                UniqueKey = "",
                                UserIdCreator = addOwnerOrConcultantPVM.UserIdCreator.Value,
                            };

                            consoleBusiness.CmsDb.UsersProfile.Add(usersProfile);
                            consoleBusiness.CmsDb.SaveChanges();

                            #endregion

                            #region Add UsersLeveles


                            UsersLevels usersLevels = new UsersLevels()
                            {
                                LevelId = LevelId,
                                UserId = userId,
                                CreateEnDate = DateTime.Now,
                                CreateTime = PersianDate.TimeNow,
                                UserIdCreator = addOwnerOrConcultantPVM.UserIdCreator.Value,
                                IsActivated = true,
                                IsDeleted = false,
                            };

                            consoleBusiness.CmsDb.UsersLevels.Add(usersLevels);
                            consoleBusiness.CmsDb.SaveChanges();


                            #endregion

                            #region Add UserRoles

                            var roleId = consoleBusiness.CmsDb.Roles.Where(r => r.RoleName.Equals("Users")).FirstOrDefault().RoleId;

                            UsersRoles usersRoles = new UsersRoles()
                            {
                                RoleId = roleId,
                                UserId = userId,
                                CreateEnDate = DateTime.Now,
                                CreateTime = PersianDate.TimeNow,
                                UserIdCreator = addOwnerOrConcultantPVM.UserIdCreator.Value,
                                IsActivated = true,
                                IsDeleted = false,
                            };

                            consoleBusiness.CmsDb.UsersRoles.Add(usersRoles);
                            consoleBusiness.CmsDb.SaveChanges();

                            #endregion

                            transaction.Commit();

                            return userId;
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                        }
                    }

                }
                else
                {
                    //اگر این کاربر وجود داشته باشد  
                    var user = consoleBusiness.CmsDb.Users.Where(c => c.UserName.Equals(userName)).FirstOrDefault();

                    var SpecificuserId = user.UserId;
                    var levelIds = consoleBusiness.CmsDb.UsersLevels.Where(c => c.UserId.Equals(SpecificuserId)).ToList().Select(c => c.LevelId).Distinct();
                    var levelNames = consoleBusiness.CmsDb.Levels.Where(c => levelIds.Contains(c.LevelId)).ToList().Select(c => c.LevelName);


                    if (user != null && user.UserId > 0)
                    {
                        if (addOwnerOrConcultantPVM.Type.Equals("مالک"))
                        {
                            if (levelNames.Contains("مشاور"))
                            {
                                //اگر این شماره موبایل مشاور تعریف شده باشد در پلتفرم دیگر نمیتواند به عنوان مالک تعریف شود
                                return -2;
                            }
                            else if (!levelNames.Contains("مالک")) //اگر کاربر دسترسی مالک نداشته باشد
                            {

                                #region Update Users

                                user.UserIdEditor = addOwnerOrConcultantPVM.UserIdCreator.Value; //this.userId
                                user.EditEnDate = DateTime.Now;
                                user.EditTime = PersianDate.TimeNow;

                                consoleBusiness.CmsDb.Entry<Users>(user).State = EntityState.Modified;
                                consoleBusiness.CmsDb.SaveChanges();

                                #endregion

                                #region Add  UsersLeveles

                                var levelId = consoleBusiness.GetLevelId("مالک");

                                UsersLevels usersLevels = new UsersLevels()
                                {
                                    LevelId = levelId,
                                    UserId = user.UserId,
                                    CreateEnDate = DateTime.Now,
                                    CreateTime = PersianDate.TimeNow,
                                    UserIdCreator = addOwnerOrConcultantPVM.UserIdCreator.Value,
                                    IsActivated = true,
                                    IsDeleted = false,
                                };

                                consoleBusiness.CmsDb.UsersLevels.Add(usersLevels);
                                consoleBusiness.CmsDb.SaveChanges();

                                #endregion

                                #region Add UsersRoles

                                var roleIds = consoleBusiness.GetRoleIdsWithUserId(user.UserId);
                                var roleNames = consoleBusiness.GetRolesWithRoleIds(roleIds).Select(r => r.RoleName);

                                if (roleNames != null)
                                {
                                    if (!roleNames.Contains("Users"))
                                    {
                                        ////اضافه کردن نقش Users
                                        var roleId = consoleBusiness.CmsDb.Roles.Where(r => r.RoleName.Equals("Users")).FirstOrDefault().RoleId;

                                        UsersRoles usersRoles = new UsersRoles()
                                        {
                                            RoleId = roleId,
                                            UserId = user.UserId,
                                            CreateEnDate = DateTime.Now,
                                            CreateTime = PersianDate.TimeNow,
                                            UserIdCreator = addOwnerOrConcultantPVM.UserIdCreator.Value,
                                            IsActivated = true,
                                            IsDeleted = false,
                                        };


                                        consoleBusiness.CmsDb.UsersRoles.Add(usersRoles);
                                        consoleBusiness.CmsDb.SaveChanges();
                                    }


                                }
                                #endregion

                                #region Update UsersProfile

                                var userProfile = consoleBusiness.CmsDb.UsersProfile.Where(p => p.UserId.Equals(user.UserId)).FirstOrDefault();
                                if (userProfile != null)
                                {
                                    userProfile.Name = addOwnerOrConcultantPVM.Name != null ? addOwnerOrConcultantPVM.Name : "";
                                    userProfile.Family = addOwnerOrConcultantPVM.Family != null ? addOwnerOrConcultantPVM.Family : "";
                                    userProfile.Mobile = addOwnerOrConcultantPVM.Phone;
                                    userProfile.Phone = addOwnerOrConcultantPVM.Phone;

                                    userProfile.UserIdEditor = addOwnerOrConcultantPVM.UserIdCreator.Value; //this.userId
                                    userProfile.EditEnDate = DateTime.Now;
                                    userProfile.EditTime = PersianDate.TimeNow;

                                    consoleBusiness.CmsDb.Entry<UsersProfile>(userProfile).State = EntityState.Modified;
                                    consoleBusiness.CmsDb.SaveChanges();
                                }


                                #endregion

                                return user.UserId;
                            }
                            else
                            {
                                //این فرد به عنوان مالک تعریف شده است
                                // امکان ثبت دوباره آن وجود ندارد
                                return -3;
                            }

                        }
                        else if (addOwnerOrConcultantPVM.Type.Equals("مشاور"))
                        {
                            if (levelNames.Contains("مالک"))
                            {
                                var levelId = consoleBusiness.GetLevelId("مالک");
                                var userLevel = consoleBusiness.CmsDb.UsersLevels.Where(c => c.LevelId.Equals(levelId) && c.UserId.Equals(user.UserId)).FirstOrDefault();

                                //دسترسی مالک حذف میشود
                                consoleBusiness.CmsDb.UsersLevels.Remove(userLevel);
                                consoleBusiness.CmsDb.SaveChanges();

                                // دسترسی مشاور اضافه میشود
                                #region Add  UsersLeveles
                                var levelId2 = consoleBusiness.GetLevelId("مشاور");

                                UsersLevels usersLevels2 = new UsersLevels()
                                {
                                    LevelId = levelId2,
                                    UserId = user.UserId,
                                    CreateEnDate = DateTime.Now,
                                    CreateTime = PersianDate.TimeNow,
                                    UserIdCreator = addOwnerOrConcultantPVM.UserIdCreator.Value,
                                    IsActivated = true,
                                    IsDeleted = false,
                                };

                                consoleBusiness.CmsDb.UsersLevels.Add(usersLevels2);
                                consoleBusiness.CmsDb.SaveChanges();

                                #endregion

                                return user.UserId;
                            }
                            else if ((levelNames.Contains("مشاور املاک - کاربر و زیر گروه ها")) ||
                                (levelNames.Contains("مشاور املاک - فقط خود کاربر")) ||
                                (levelNames.Contains("مشاور املاک - فقط زیر گروه ها")) ||
                                (levelNames.Contains("مشاور املاک - والد و زیر گروه ها")) ||
                                (levelNames.Contains("مدیر املاک")))
                            {
                                //این کاربر به عنوان مشاور در پلتفرم ثبت نام کرده است.
                                //امکان ثبت مجدد وجود ندارد
                                return -5;

                            }
                            else if (!levelNames.Contains("مشاور"))
                            {
                                //دسترسی مشاور اضافه میشود

                                #region Update Users

                                user.UserIdEditor = addOwnerOrConcultantPVM.UserIdCreator.Value; //this.userId
                                user.EditEnDate = DateTime.Now;
                                user.EditTime = PersianDate.TimeNow;

                                consoleBusiness.CmsDb.Entry<Users>(user).State = EntityState.Modified;
                                consoleBusiness.CmsDb.SaveChanges();

                                #endregion

                                #region Add  UsersLeveles

                                var levelId = consoleBusiness.GetLevelId("مشاور");

                                UsersLevels usersLevels = new UsersLevels()
                                {
                                    LevelId = levelId,
                                    UserId = user.UserId,
                                    CreateEnDate = DateTime.Now,
                                    CreateTime = PersianDate.TimeNow,
                                    UserIdCreator = addOwnerOrConcultantPVM.UserIdCreator.Value,
                                    IsActivated = true,
                                    IsDeleted = false,
                                };

                                consoleBusiness.CmsDb.UsersLevels.Add(usersLevels);
                                consoleBusiness.CmsDb.SaveChanges();

                                #endregion

                                #region Add UsersRoles

                                var roleIds = consoleBusiness.GetRoleIdsWithUserId(user.UserId);
                                var roleNames = consoleBusiness.GetRolesWithRoleIds(roleIds).Select(r => r.RoleName);

                                if (roleNames != null)
                                {
                                    if (!roleNames.Contains("Users"))
                                    {
                                        ////اضافه کردن نقش Users
                                        var roleId = consoleBusiness.CmsDb.Roles.Where(r => r.RoleName.Equals("Users")).FirstOrDefault().RoleId;

                                        UsersRoles usersRoles = new UsersRoles()
                                        {
                                            RoleId = roleId,
                                            UserId = user.UserId,
                                            CreateEnDate = DateTime.Now,
                                            CreateTime = PersianDate.TimeNow,
                                            UserIdCreator = addOwnerOrConcultantPVM.UserIdCreator.Value,
                                            IsActivated = true,
                                            IsDeleted = false,
                                        };


                                        consoleBusiness.CmsDb.UsersRoles.Add(usersRoles);
                                        consoleBusiness.CmsDb.SaveChanges();
                                    }


                                }
                                #endregion

                                #region Update UsersProfile

                                var userProfile = consoleBusiness.CmsDb.UsersProfile.Where(p => p.UserId.Equals(user.UserId)).FirstOrDefault();
                                if (userProfile != null)
                                {
                                    userProfile.Name = addOwnerOrConcultantPVM.Name != null ? addOwnerOrConcultantPVM.Name : "";
                                    userProfile.Family = addOwnerOrConcultantPVM.Family != null ? addOwnerOrConcultantPVM.Family : "";
                                    userProfile.Mobile = addOwnerOrConcultantPVM.Phone;
                                    userProfile.Phone = addOwnerOrConcultantPVM.Phone;

                                    userProfile.UserIdEditor = addOwnerOrConcultantPVM.UserIdCreator.Value; //this.userId
                                    userProfile.EditEnDate = DateTime.Now;
                                    userProfile.EditTime = PersianDate.TimeNow;

                                    consoleBusiness.CmsDb.Entry<UsersProfile>(userProfile).State = EntityState.Modified;
                                    consoleBusiness.CmsDb.SaveChanges();
                                }


                                #endregion

                                return user.UserId;

                            }


                        }

                    }

                    return -1;
                }
            }

            return 0;
        }


        #endregion

        #region Methods For Work With AdvertisementQuickRegistration

        public long AdvertisementQuickRegistration(
                    AdvertisementVM advertisementVM,
                    IPublicApiBusiness publicApiBusiness,
                    IConsoleBusiness consoleBusiness)
        {
            using (var transaction = melkavanApiDb.Database.BeginTransaction())
            {
                try
                {

                    #region Advertisement


                    Advertisement Advertisement = _mapper.Map<AdvertisementVM, Advertisement>(advertisementVM);
                    Advertisement.AdvertiserId = Advertisement.UserIdCreator; //آگهی دهنده - کسی که لاگین کرده است.
                    Advertisement.StatusId = 3; //در انتظار تعیین وضعیت

                    #region Add ConsultantUserId
                    //مشاور

                    var userId = consoleBusiness.CmsDb.Users.Where(c => c.UserId.Equals(Advertisement.UserIdCreator)).Select(c => c.UserId).FirstOrDefault();
                    var LevelIds = consoleBusiness.CmsDb.UsersLevels.Where(c => c.UserId.Equals(userId)).Select(c => c.LevelId).ToList();
                    var levelNames = consoleBusiness.CmsDb.Levels.Where(c => LevelIds.Contains(c.LevelId)).Select(c => c.LevelName).ToList();


                    if (levelNames.Contains("مشاور"))
                    {
                        Advertisement.ConsultantUserId = Advertisement.UserIdCreator;

                    }

                    #endregion


                    melkavanApiDb.Advertisement.Add(Advertisement);
                    melkavanApiDb.SaveChanges();

                    #endregion

                    #region AdvertisementAddress 


                    if (advertisementVM.AdvertisementAddressVM != null)
                    {
                        AdvertisementAddress AdvertisementAddress = _mapper.Map<AdvertisementAddressVM, AdvertisementAddress>(advertisementVM.AdvertisementAddressVM);
                        AdvertisementAddress.AdvertisementId = Advertisement.AdvertisementId;
                        AdvertisementAddress.IsActivated = true;
                        AdvertisementAddress.IsDeleted = false;
                        AdvertisementAddress.UserIdCreator = Advertisement.UserIdCreator.Value;

                        melkavanApiDb.AdvertisementAddress.Add(AdvertisementAddress);
                        melkavanApiDb.SaveChanges();
                    }
                    else
                    {
                        AdvertisementAddress AdvertisementAddress = new AdvertisementAddress();
                        AdvertisementAddress.AdvertisementId = Advertisement.AdvertisementId;
                        AdvertisementAddress.CreateEnDate = Advertisement.CreateEnDate.Value;
                        AdvertisementAddress.CreateTime = Advertisement.CreateTime;
                        AdvertisementAddress.UserIdCreator = Advertisement.UserIdCreator.Value;
                        AdvertisementAddress.IsActivated = true;
                        AdvertisementAddress.IsDeleted = false;

                        melkavanApiDb.AdvertisementAddress.Add(AdvertisementAddress);
                        melkavanApiDb.SaveChanges();
                    }

                    #endregion

                    #region AdvertisementDetails 



                    if (advertisementVM.AdvertisementDetailsVM != null)
                    {
                        AdvertisementDetails AdvertisementDetails = _mapper.Map<AdvertisementDetailsVM, AdvertisementDetails>(advertisementVM.AdvertisementDetailsVM);
                        AdvertisementDetails.AdvertisementId = Advertisement.AdvertisementId;
                        AdvertisementDetails.UserIdCreator = Advertisement.UserIdCreator.Value;
                        AdvertisementDetails.IsActivated = true;
                        AdvertisementDetails.IsDeleted = false;

                        melkavanApiDb.AdvertisementDetails.Add(AdvertisementDetails);
                        melkavanApiDb.SaveChanges();
                    }
                    else
                    {
                        AdvertisementDetails AdvertisementDetails = new AdvertisementDetails();
                        AdvertisementDetails.AdvertisementId = Advertisement.AdvertisementId;
                        AdvertisementDetails.CreateEnDate = Advertisement.CreateEnDate.Value;
                        AdvertisementDetails.CreateTime = Advertisement.CreateTime;
                        AdvertisementDetails.UserIdCreator = Advertisement.UserIdCreator.Value;
                        AdvertisementDetails.IsActivated = true;
                        AdvertisementDetails.IsDeleted = false;

                        melkavanApiDb.AdvertisementDetails.Add(AdvertisementDetails);
                        melkavanApiDb.SaveChanges();
                    }

                    #endregion

                    #region AdvertisementPricesHistories 

                    var OfferPrice = (long)0;
                    var Area = Convert.ToInt64(advertisementVM.Area);


                    if (advertisementVM.AdvertisementPricesHistoriesVM != null)
                    {
                        if (advertisementVM.AdvertisementDetailsVM.AdvertisementTypeId.Equals(1)) // اجاره
                        {
                            //ثبت قیمت اجاره و ودیعه اجباری است
                            if ((advertisementVM.AdvertisementPricesHistoriesVM.RentPrice != null) &&
                                (advertisementVM.AdvertisementPricesHistoriesVM.DepositPrice != null))
                            {
                                if ((advertisementVM.AdvertisementPricesHistoriesVM.RentPrice.Value > 0) &&
                                     (advertisementVM.AdvertisementPricesHistoriesVM.DepositPrice.Value > 0))
                                {

                                    AdvertisementPricesHistories AdvertisementPricesHistories = _mapper.Map<AdvertisementPricesHistoriesVM, AdvertisementPricesHistories>(advertisementVM.AdvertisementPricesHistoriesVM);
                                    AdvertisementPricesHistories.OfferPrice = 0;
                                    AdvertisementPricesHistories.CalculatedOfferPrice = 0;
                                    AdvertisementPricesHistories.RentPrice = advertisementVM.AdvertisementPricesHistoriesVM.RentPrice.Value;
                                    AdvertisementPricesHistories.DepositPrice = advertisementVM.AdvertisementPricesHistoriesVM.DepositPrice.Value;
                                    AdvertisementPricesHistories.AdvertisementId = Advertisement.AdvertisementId;

                                    AdvertisementPricesHistories.CreateEnDate = Advertisement.CreateEnDate.Value;
                                    AdvertisementPricesHistories.CreateTime = Advertisement.CreateTime;
                                    AdvertisementPricesHistories.UserIdCreator = Advertisement.UserIdCreator.Value;
                                    AdvertisementPricesHistories.IsActivated = true;
                                    AdvertisementPricesHistories.IsDeleted = false;

                                    melkavanApiDb.AdvertisementPricesHistories.Add(AdvertisementPricesHistories);
                                    melkavanApiDb.SaveChanges();
                                }
                            }

                        }
                        else if (advertisementVM.AdvertisementDetailsVM.AdvertisementTypeId.Equals(2)) //فروش
                        {
                            //ثبت قیمت در فروش ملک اجباری نیست
                            if (advertisementVM.AdvertisementPricesHistoriesVM.OfferPrice != null)
                            {
                                if (advertisementVM.AdvertisementPricesHistoriesVM.OfferPrice.Value > 0) //قیمت را وارد کرده بود
                                {
                                    OfferPrice = Convert.ToInt64(advertisementVM.AdvertisementPricesHistoriesVM.OfferPrice.Value);

                                    AdvertisementPricesHistories AdvertisementPricesHistories = _mapper.Map<AdvertisementPricesHistoriesVM, AdvertisementPricesHistories>(advertisementVM.AdvertisementPricesHistoriesVM);
                                    AdvertisementPricesHistories.CalculatedOfferPrice = Convert.ToInt64(OfferPrice / Area);
                                    AdvertisementPricesHistories.OfferPrice = OfferPrice;
                                    AdvertisementPricesHistories.AdvertisementId = Advertisement.AdvertisementId;
                                    AdvertisementPricesHistories.CreateEnDate = Advertisement.CreateEnDate.Value;
                                    AdvertisementPricesHistories.CreateTime = Advertisement.CreateTime;
                                    AdvertisementPricesHistories.UserIdCreator = Advertisement.UserIdCreator.Value;
                                    AdvertisementPricesHistories.IsActivated = true;
                                    AdvertisementPricesHistories.IsDeleted = false;

                                    melkavanApiDb.AdvertisementPricesHistories.Add(AdvertisementPricesHistories);
                                    melkavanApiDb.SaveChanges();
                                }
                                else // قیمت را صفر وارد کرده بود
                                {
                                    AdvertisementPricesHistories AdvertisementPricesHistories = _mapper.Map<AdvertisementPricesHistoriesVM, AdvertisementPricesHistories>(advertisementVM.AdvertisementPricesHistoriesVM);
                                    AdvertisementPricesHistories.CalculatedOfferPrice = 0; //فروش قیمت ندارد
                                    AdvertisementPricesHistories.OfferPrice = 0; // فروش قیمت ندارد
                                    AdvertisementPricesHistories.AdvertisementId = Advertisement.AdvertisementId;
                                    AdvertisementPricesHistories.CreateEnDate = Advertisement.CreateEnDate.Value;
                                    AdvertisementPricesHistories.CreateTime = Advertisement.CreateTime;
                                    AdvertisementPricesHistories.UserIdCreator = Advertisement.UserIdCreator.Value;
                                    AdvertisementPricesHistories.IsActivated = true;
                                    AdvertisementPricesHistories.IsDeleted = false;

                                    melkavanApiDb.AdvertisementPricesHistories.Add(AdvertisementPricesHistories);
                                    melkavanApiDb.SaveChanges();
                                }
                            }
                            else // قیمت را وارد نکرده بود
                            {
                                AdvertisementPricesHistories AdvertisementPricesHistories = _mapper.Map<AdvertisementPricesHistoriesVM, AdvertisementPricesHistories>(advertisementVM.AdvertisementPricesHistoriesVM);
                                AdvertisementPricesHistories.CalculatedOfferPrice = 0; //فروش قیمت ندارد
                                AdvertisementPricesHistories.OfferPrice = 0; // فروش قیمت ندارد
                                AdvertisementPricesHistories.AdvertisementId = Advertisement.AdvertisementId;
                                AdvertisementPricesHistories.CreateEnDate = Advertisement.CreateEnDate.Value;
                                AdvertisementPricesHistories.CreateTime = Advertisement.CreateTime;
                                AdvertisementPricesHistories.UserIdCreator = Advertisement.UserIdCreator.Value;
                                AdvertisementPricesHistories.IsActivated = true;
                                AdvertisementPricesHistories.IsDeleted = false;

                                melkavanApiDb.AdvertisementPricesHistories.Add(AdvertisementPricesHistories);
                                melkavanApiDb.SaveChanges();
                            }

                        }

                    }
                    else
                    {

                        AdvertisementPricesHistories AdvertisementPricesHistories = new AdvertisementPricesHistories();
                        AdvertisementPricesHistories.OfferPrice = advertisementVM.OfferPrice.HasValue ? advertisementVM.OfferPrice.Value : 0;
                        AdvertisementPricesHistories.OfferPriceType = advertisementVM.OfferPriceType.HasValue ? advertisementVM.OfferPriceType.Value : 0;
                        AdvertisementPricesHistories.CalculatedOfferPrice = advertisementVM.CalculatedOfferPrice.HasValue ? advertisementVM.CalculatedOfferPrice.Value : 0;
                        //AdvertisementPricesHistories.CalculatedOfferPrice = Convert.ToInt64(OfferPrice / Area);
                        AdvertisementPricesHistories.AdvertisementId = Advertisement.AdvertisementId;
                        AdvertisementPricesHistories.RentPrice = advertisementVM.RentPrice;
                        AdvertisementPricesHistories.DepositPrice = advertisementVM.DepositPrice;

                        AdvertisementPricesHistories.CreateEnDate = Advertisement.CreateEnDate.Value;
                        AdvertisementPricesHistories.CreateTime = Advertisement.CreateTime;
                        AdvertisementPricesHistories.UserIdCreator = Advertisement.UserIdCreator.Value;
                        AdvertisementPricesHistories.IsActivated = true;
                        AdvertisementPricesHistories.IsDeleted = false;

                        melkavanApiDb.AdvertisementPricesHistories.Add(AdvertisementPricesHistories);
                        melkavanApiDb.SaveChanges();
                    }
                    #endregion

                    #region AdvertisementFiles


                    if (advertisementVM.AdvertisementFilesVM != null)
                    {
                        if (advertisementVM.AdvertisementFilesVM.Count > 0)
                        {
                            var AdvertisementFilesList = _mapper.Map<List<AdvertisementFilesVM>, List<AdvertisementFiles>>(advertisementVM.AdvertisementFilesVM);

                            foreach (var item in AdvertisementFilesList)
                            {
                                item.AdvertisementId = Advertisement.AdvertisementId;
                                item.UserIdCreator = Advertisement.UserIdCreator.Value;
                                item.IsActivated = true;
                                item.IsDeleted = false;
                            }

                            melkavanApiDb.AdvertisementFiles.AddRange(AdvertisementFilesList);
                            melkavanApiDb.SaveChanges();
                        }

                    }

                    #endregion

                    #region AdvertisementOwners


                    if (advertisementVM.AdvertisementOwnersVM != null)
                    {
                        var AdvertisementOwners = _mapper.Map<AdvertisementOwnersVM, AdvertisementOwners>(advertisementVM.AdvertisementOwnersVM);
                        AdvertisementOwners.AdvertisementId = Advertisement.AdvertisementId;

                        AdvertisementOwners.IsActivated = true;
                        AdvertisementOwners.IsDeleted = false;
                        AdvertisementOwners.OwnerType = "users";

                        melkavanApiDb.AdvertisementOwners.Add(AdvertisementOwners);
                        melkavanApiDb.SaveChanges();


                    }
                    #endregion

                    #region AdvertisementFeaturesValues


                    if (advertisementVM.AdvertisementFeaturesValuesVM != null)
                    {
                        List<AdvertisementFeaturesValues> advertisementFeaturesValuesList = new List<AdvertisementFeaturesValues>();

                        if (advertisementVM.AdvertisementFeaturesValuesVM.Count > 0)
                        {


                            foreach (var item in advertisementVM.AdvertisementFeaturesValuesVM)
                            {

                                if (!item.FeatureValue.IsNullOrEmpty())
                                {
                                    AdvertisementFeaturesValues advertisementFeaturesValues = new AdvertisementFeaturesValues();


                                    advertisementFeaturesValues.AdvertisementId = Advertisement.AdvertisementId;
                                    advertisementFeaturesValues.CreateEnDate = Advertisement.CreateEnDate.Value;
                                    advertisementFeaturesValues.CreateTime = Advertisement.CreateTime;
                                    advertisementFeaturesValues.UserIdCreator = Advertisement.UserIdCreator.Value;
                                    advertisementFeaturesValues.IsActivated = true;
                                    advertisementFeaturesValues.IsDeleted = false;



                                    advertisementFeaturesValues.FeatureId = item.FeatureId;
                                    advertisementFeaturesValues.FeatureValue = item.FeatureValue;
                                    advertisementFeaturesValuesList.Add(advertisementFeaturesValues);
                                }

                            }

                            melkavanApiDb.AdvertisementFeaturesValues.AddRange(advertisementFeaturesValuesList);
                            melkavanApiDb.SaveChanges();
                        }
                    }
                    #endregion

                    #region AdvertisementSelectedCallers

                    // کد های صبا
                    //var user = consoleBusiness.CmsDb.UsersProfile.Where(u => u.UserId == Advertisement.UserIdCreator.Value).FirstOrDefault();
                    //if (user != null && Advertisement.AdvertisementId != 0)
                    //{
                    //    AdvertisementSelectedCallers selectedCaller = new AdvertisementSelectedCallers()
                    //    {
                    //        CallerType = "Advertiser",
                    //        AdvertiserNumber = user.Mobile,
                    //        AdvertisementId = Advertisement.AdvertisementId,
                    //        UserIdCreator = Advertisement.UserIdCreator.Value,
                    //        CreateTime = PersianDate.TimeNow,
                    //        CreateEnDate = DateTime.Now,
                    //        IsActivated = true,
                    //        IsDeleted = false
                    //    };

                    //    melkavanApiDb.AdvertisementSelectedCallers.Add(selectedCaller);
                    //    melkavanApiDb.SaveChanges();
                    //};

                    // کد های سینا
                    if (advertisementVM.OwnerId != null && advertisementVM.OwnerId > 0) // اگر مالک بود
                    {
                        var user = consoleBusiness.CmsDb.UsersProfile.Where(u => u.UserId == advertisementVM.OwnerId.Value).FirstOrDefault();
                        if (user != null && Advertisement.AdvertisementId != 0)
                        {
                            AdvertisementSelectedCallers selectedCaller = new AdvertisementSelectedCallers()
                            {
                                CallerType = "Owner",
                                AdvertiserNumber = user.Mobile,
                                AdvertisementId = Advertisement.AdvertisementId,
                                UserIdCreator = Advertisement.UserIdCreator.Value,
                                CreateTime = PersianDate.TimeNow,
                                CreateEnDate = DateTime.Now,
                                IsActivated = true,
                                IsDeleted = false
                            };

                            melkavanApiDb.AdvertisementSelectedCallers.Add(selectedCaller);
                            melkavanApiDb.SaveChanges();
                        };
                    }
                    else if (advertisementVM.ConsultantUserId != null && advertisementVM.ConsultantUserId > 0) // اگر مشاور بود
                    {
                        var user = consoleBusiness.CmsDb.UsersProfile.Where(u => u.UserId == advertisementVM.ConsultantUserId.Value).FirstOrDefault();
                        if (user != null && Advertisement.AdvertisementId != 0)
                        {
                            AdvertisementSelectedCallers selectedCaller = new AdvertisementSelectedCallers()
                            {
                                CallerType = "Advertiser",
                                AdvertiserNumber = user.Mobile,
                                AdvertisementId = Advertisement.AdvertisementId,
                                UserIdCreator = Advertisement.UserIdCreator.Value,
                                CreateTime = PersianDate.TimeNow,
                                CreateEnDate = DateTime.Now,
                                IsActivated = true,
                                IsDeleted = false
                            };

                            melkavanApiDb.AdvertisementSelectedCallers.Add(selectedCaller);
                            melkavanApiDb.SaveChanges();
                        };
                    }



                    #endregion

                    transaction.Commit();


                    return Advertisement.AdvertisementId;

                }
                catch (Exception exc)
                {
                    transaction.Rollback();
                }
            }
            return 0;
        }

        #endregion

        #region Methods For Work With Advertisementlocation

        public bool UpdateAdvertisementlocation(long userId,
            long advertisementId,
            int stateId,
            int cityId,
            int zoneId,
            string townName,
            string address,
            double locationLat,
            double locationLon,
            List<long> childsUsersIds)
        {
            try
            {

                AdvertisementAddress? advertisementAddress = melkavanApiDb.AdvertisementAddress.Where(ad => ad.AdvertisementId.Equals(advertisementId) && childsUsersIds.Contains(ad.UserIdCreator.Value)).FirstOrDefault();
                if (advertisementAddress == null)
                {
                    return false; //اگر نال باشد یعنی کاربر دسترسی لازم برای بروزرسانی موقعیت این آگهی را ندارد
                }

                advertisementAddress.LocationLat = locationLat;
                advertisementAddress.LocationLon = locationLon;
                advertisementAddress.StateId = stateId;
                advertisementAddress.CityId = cityId;
                advertisementAddress.ZoneId = zoneId;
                advertisementAddress.TownName = townName;
                advertisementAddress.Address = address;
                advertisementAddress.EditEnDate = DateTime.Now;
                advertisementAddress.EditTime = PersianDate.TimeNow;
                advertisementAddress.UserIdEditor = userId;


                melkavanApiDb.SaveChanges();
                return true;
            }
            catch (Exception exc)
            { }

            return false;
        }

        #endregion

        #region Methods For Work With AdvertisementStatus

        #region Sina's Code


        //public List<AdvertisementStatusListVM> GetListOfAdvertisementsForStatus(int jtStartIndex,
        //            int jtPageSize,
        //            ref int listCount,
        //            List<long> childsUsersIds,
        //            MelkavanApiContext melkavanApiDb,
        //            TeniacoApiContext teniacoApiDb,
        //            IConsoleBusiness consoleBusiness,
        //            string jtSorting = null,
        //            long? ThisUserId = null)
        //{
        //    List<AdvertisementStatusListVM> advertisementStatusList = new List<AdvertisementStatusListVM>();

        //    try
        //    {
        //        string sp = @"";


        //        sp = @"

        //                      SELECT DISTINCT * FROM (
        //                                SELECT 'Advertisement' AS RecordType, 
        //                                        0 CountOfMedia,
        //                                        ad.UserIdCreator,
        //                                        ad.ConsultantUserId,
        //					        ad.AdvertisementId,
        //                            ad.RejectionReason,
        //							st.StatusId,
        //                                        st.StatusTitle,

        //                                        details.AdvertisementTypeId,
        //                                        ad.AdvertisementTitle,
        //                                        ad.CreateEnDate,
        //							ad.AdvertisementDescriptions,
        //							us.Mobile,
        //							us.Name,
        //							us.Family,
        //							details.TagId,
        //							priceHist.OfferPrice AS 'LastPrice'

        //                                        FROM MelkavanDbHaghighi.dbo.Advertisement AS ad
        //                                        INNER JOIN MelkavanDbHaghighi.dbo.AdvertisementAddress AS addr ON ad.AdvertisementId = addr.AdvertisementId
        //                                        INNER JOIN (
        //                                            SELECT AdvertisementId, OfferPrice, DepositPrice, RentPrice
        //                                            FROM MelkavanDbHaghighi.dbo.AdvertisementPricesHistories AS aph
        //                                            WHERE aph.AdvertisementPriceHistoryId = (
        //                                                SELECT MAX(AdvertisementPriceHistoryId)
        //                                                FROM MelkavanDbHaghighi.dbo.AdvertisementPricesHistories
        //                                                WHERE AdvertisementId = aph.AdvertisementId
        //                                            )
        //                                        ) AS priceHist ON ad.AdvertisementId = priceHist.AdvertisementId
        //                                        INNER JOIN MelkavanDbHaghighi.dbo.AdvertisementDetails AS details ON ad.AdvertisementId = details.AdvertisementId
        //							LEFT JOIN MelkavanDbHaghighi.dbo.StatusTypes AS st ON ad.StatusId = st.StatusId
        //							INNER JOIN NewArashCmsDbHaghighi.dbo.UsersProfile AS us ON ad.AdvertiserId = us.UserId 



        //                                        UNION ALL
        //                                        SELECT 'Properties' AS RecordType,
        //                                        0 CountOfMedia,
        //                                        p.UserIdCreator,
        //                                        p.ConsultantUserId,
        //							p.PropertyId AS 'AdvertisementId',
        //                            p.RejectionReason,
        //							st.StatusId,
        //                                        st.StatusTitle,
        //                                        pd.AdvertisementTypeId,
        //                                        p.PropertyCodeName AS AdvertisementTitle,
        //                                        p.CreateEnDate,
        //							pd.SecondPropertyDescriptions as 'AdvertisementDescriptions',
        //							us.Mobile,
        //							us.Name,
        //							us.Family,
        //							pd.TagId,
        //							propPriceHist.OfferPrice AS 'LastPrice'

        //                                        FROM TeniacoDbHaghighi.dbo.Properties AS p
        //                                        INNER JOIN TeniacoDbHaghighi.dbo.PropertyAddress AS propAddr ON p.PropertyId = propAddr.PropertyId
        //                                        INNER JOIN (
        //                                            SELECT PropertyId, CASE OfferPriceType WHEN '0' THEN CalculatedOfferPrice ELSE OfferPrice END as OfferPrice, DepositPrice, RentPrice
        //                                            FROM TeniacoDbHaghighi.dbo.PropertiesPricesHistories AS pph
        //                                            WHERE pph.PropertyPriceHistoryId = (
        //                                                SELECT MAX(PropertyPriceHistoryId)
        //                                                FROM TeniacoDbHaghighi.dbo.PropertiesPricesHistories
        //                                                WHERE PropertyId = pph.PropertyId
        //                                            )
        //                                        ) AS propPriceHist ON p.PropertyId = propPriceHist.PropertyId
        //                                        INNER JOIN TeniacoDbHaghighi.dbo.PropertiesDetails AS pd ON p.PropertyId = pd.PropertyId
        //							LEFT JOIN MelkavanDbHaghighi.dbo.StatusTypes AS st ON p.StatusId = st.StatusId
        //                                        INNER JOIN NewArashCmsDbHaghighi.dbo.UsersProfile AS us ON p.AdvertiserId = us.UserId 

        //                                        WHERE p.ShowInMelkavan = 1 
        //                                        ) AS tmp
        //                                        ORDER BY CreateEnDate DESC;    ";


        //        var list = melkavanApiDb.AdvertisementStatusListVM.FromSqlRaw(sp).AsEnumerable().Distinct();

        //        //سطح دسترسی
        //        list = list.Where(a => childsUsersIds.Contains(a.UserIdCreator.Value)).ToList();


        //        //تعداد عکس ها
        //        string countsp = @"select distinct * from (

        //           					select ROW_NUMBER() OVER (ORDER BY (SELECT 1)) as PropertyDataTypeCountId, PropertyId, DataType, 'Properties' RecordType, Count
        //              						 from           
        //                 							  (
        //                 							  SELECT PropertyId, PropertyFileType as DataType, COUNT(PropertyFileType) as Count
        //                    								FROM [TeniacoDbHaghighi].[dbo].[PropertyFiles]
        //                    								group by PropertyId, PropertyFileType



        //                 							  ) as Counts


        //              						union all


        //select ROW_NUMBER() OVER (ORDER BY (SELECT 1)) as AdvertisementDataTypeCountId, AdvertisementId, DataType, 'Advertisement' RecordType, Count
        //              						from

        //                 							  (
        //                 							  SELECT AdvertisementId, AdvertisementFileType as DataType, COUNT(AdvertisementFileType) as Count
        //                    								FROM [MelkavanDbHaghighi].[dbo].[AdvertisementFiles]
        //                    								group by AdvertisementId, AdvertisementFileType


        //       ) as Counts
        //        				) as tmp";
        //        var propertyIds = list.Select(p => p.AdvertisementId).ToList().Distinct();
        //        var propertyDataTypeCounts = teniacoApiDb.PropertyDataTypeCounts.FromSqlRaw(countsp).Where(p => propertyIds.Contains(p.PropertyId)).ToList();
        //        foreach (var item in list)
        //        {
        //            if (propertyDataTypeCounts.Where(p => p.PropertyId.Equals(item.AdvertisementId) && item.RecordType.Equals(p.RecordType)).Any())
        //            {
        //                var propertyDataTypeCount = propertyDataTypeCounts.Where(p => p.PropertyId.Equals(item.AdvertisementId)).ToList();
        //                item.CountOfMedia = propertyDataTypeCount.Where(p => p.DataType.Equals("media")).FirstOrDefault()?.Count;
        //            }
        //        }


        //        #region Pagination

        //        if (string.IsNullOrEmpty(jtSorting))
        //        {
        //            listCount = list.Count();

        //            if (listCount > jtPageSize)
        //            {
        //                list = list.OrderByDescending(s => s.AdvertisementId)
        //                         .Skip(jtStartIndex).Take(jtPageSize).ToList();
        //            }
        //            else
        //            {
        //                list = list.OrderByDescending(s => s.AdvertisementId).ToList();
        //            }
        //            advertisementStatusList = list.ToList();
        //        }
        //        else
        //        {
        //            listCount = list.Count();

        //            if (listCount > jtPageSize)
        //            {
        //                switch (jtSorting)
        //                {
        //                    case "AdvertisementTitle ASC":
        //                        list = list.OrderBy(l => l.AdvertisementTitle);
        //                        break;
        //                    case "AdvertisementTitle DESC":
        //                        list = list.OrderByDescending(l => l.AdvertisementTitle);
        //                        break;
        //                }

        //                if (string.IsNullOrEmpty(jtSorting))
        //                    advertisementStatusList = list.OrderByDescending(s => s.AdvertisementId)
        //                             .Skip(jtStartIndex).Take(jtPageSize).ToList();
        //                else
        //                    advertisementStatusList = list.Skip(jtStartIndex).Take(jtPageSize).ToList();
        //            }
        //            else
        //            {

        //                advertisementStatusList = list.ToList();
        //            }
        //        }

        //        #endregion


        //    }
        //    catch (Exception exc)
        //    { }



        //    return advertisementStatusList;
        //}

        #endregion


        public List<AdvertisementStatusListVM> GetListOfAdvertisementsForStatus(int jtStartIndex,
                  int jtPageSize,
                  ref int listCount,
                  List<long> childsUsersIds,
                  MelkavanApiContext melkavanApiDb,
                  TeniacoApiContext teniacoApiDb,
                  IConsoleBusiness consoleBusiness,
                  string jtSorting = null,
                  long? ThisUserId = null)
        {
            List<AdvertisementStatusListVM> advertisementStatusList = new List<AdvertisementStatusListVM>();

            try
            {
                string sp = @"";


                sp = @"

                              SELECT DISTINCT * FROM (
                                        SELECT 'Advertisement' AS RecordType, 
                                                0 CountOfMedia,
                                                ad.UserIdCreator,
                                                ad.ConsultantUserId,
        					        ad.AdvertisementId,
                                    ad.RejectionReason,
        							st.StatusId,
                                                st.StatusTitle,

                                                details.AdvertisementTypeId,
                                                ad.AdvertisementTitle,
                                                ad.CreateEnDate,
        							ad.AdvertisementDescriptions,
        							us.Mobile,
        							us.Name,
        							us.Family,
        							details.TagId,
        							priceHist.OfferPrice AS 'LastPrice'

                                                FROM MelkavanDb.dbo.Advertisement AS ad
                                                INNER JOIN MelkavanDb.dbo.AdvertisementAddress AS addr ON ad.AdvertisementId = addr.AdvertisementId
                                                INNER JOIN (
                                                    SELECT AdvertisementId, OfferPrice, DepositPrice, RentPrice
                                                    FROM MelkavanDb.dbo.AdvertisementPricesHistories AS aph
                                                    WHERE aph.AdvertisementPriceHistoryId = (
                                                        SELECT MAX(AdvertisementPriceHistoryId)
                                                        FROM MelkavanDb.dbo.AdvertisementPricesHistories
                                                        WHERE AdvertisementId = aph.AdvertisementId
                                                    )
                                                ) AS priceHist ON ad.AdvertisementId = priceHist.AdvertisementId
                                                INNER JOIN MelkavanDb.dbo.AdvertisementDetails AS details ON ad.AdvertisementId = details.AdvertisementId
        							LEFT JOIN MelkavanDb.dbo.StatusTypes AS st ON ad.StatusId = st.StatusId
        							INNER JOIN NewArashCmsDb.dbo.UsersProfile AS us ON ad.AdvertiserId = us.UserId 



                                                UNION ALL
                                                SELECT 'Properties' AS RecordType,
                                                0 CountOfMedia,
                                                p.UserIdCreator,
                                                p.ConsultantUserId,
        							p.PropertyId AS 'AdvertisementId',
                                    p.RejectionReason,
        							st.StatusId,
                                                st.StatusTitle,
                                                pd.AdvertisementTypeId,
                                                p.PropertyCodeName AS AdvertisementTitle,
                                                p.CreateEnDate,
        							pd.SecondPropertyDescriptions as 'AdvertisementDescriptions',
        							us.Mobile,
        							us.Name,
        							us.Family,
        							pd.TagId,
        							propPriceHist.OfferPrice AS 'LastPrice'

                                                FROM TeniacoDb.dbo.Properties AS p
                                                INNER JOIN TeniacoDb.dbo.PropertyAddress AS propAddr ON p.PropertyId = propAddr.PropertyId
                                                INNER JOIN (
                                                    SELECT PropertyId, CASE OfferPriceType WHEN '0' THEN CalculatedOfferPrice ELSE OfferPrice END as OfferPrice, DepositPrice, RentPrice
                                                    FROM TeniacoDb.dbo.PropertiesPricesHistories AS pph
                                                    WHERE pph.PropertyPriceHistoryId = (
                                                        SELECT MAX(PropertyPriceHistoryId)
                                                        FROM TeniacoDb.dbo.PropertiesPricesHistories
                                                        WHERE PropertyId = pph.PropertyId
                                                    )
                                                ) AS propPriceHist ON p.PropertyId = propPriceHist.PropertyId
                                                INNER JOIN TeniacoDb.dbo.PropertiesDetails AS pd ON p.PropertyId = pd.PropertyId
        							LEFT JOIN MelkavanDb.dbo.StatusTypes AS st ON p.StatusId = st.StatusId
                                                INNER JOIN NewArashCmsDb.dbo.UsersProfile AS us ON p.AdvertiserId = us.UserId 

                                                WHERE p.ShowInMelkavan = 1 
                                                ) AS tmp
                                                ORDER BY CreateEnDate DESC;    ";


                var list = melkavanApiDb.AdvertisementStatusListVM.FromSqlRaw(sp).AsEnumerable().Distinct();

                //سطح دسترسی
                list = list.Where(a => childsUsersIds.Contains(a.UserIdCreator.Value)).ToList();


                //تعداد عکس ها
                string countsp = @"select distinct * from (

                   					select ROW_NUMBER() OVER (ORDER BY (SELECT 1)) as PropertyDataTypeCountId, PropertyId, DataType, 'Properties' RecordType, Count
                      						 from           
                         							  (
                         							  SELECT PropertyId, PropertyFileType as DataType, COUNT(PropertyFileType) as Count
                            								FROM [TeniacoDb].[dbo].[PropertyFiles]
                            								group by PropertyId, PropertyFileType



                         							  ) as Counts


                      						union all


        select ROW_NUMBER() OVER (ORDER BY (SELECT 1)) as AdvertisementDataTypeCountId, AdvertisementId, DataType, 'Advertisement' RecordType, Count
                      						from

                         							  (
                         							  SELECT AdvertisementId, AdvertisementFileType as DataType, COUNT(AdvertisementFileType) as Count
                            								FROM [MelkavanDb].[dbo].[AdvertisementFiles]
                            								group by AdvertisementId, AdvertisementFileType


               ) as Counts
                				) as tmp";
                var propertyIds = list.Select(p => p.AdvertisementId).ToList().Distinct();
                var propertyDataTypeCounts = teniacoApiDb.PropertyDataTypeCounts.FromSqlRaw(countsp).Where(p => propertyIds.Contains(p.PropertyId)).ToList();
                foreach (var item in list)
                {
                    if (propertyDataTypeCounts.Where(p => p.PropertyId.Equals(item.AdvertisementId) && item.RecordType.Equals(p.RecordType)).Any())
                    {
                        var propertyDataTypeCount = propertyDataTypeCounts.Where(p => p.PropertyId.Equals(item.AdvertisementId)).ToList();
                        item.CountOfMedia = propertyDataTypeCount.Where(p => p.DataType.Equals("media")).FirstOrDefault()?.Count;
                    }
                }


                #region Pagination

                if (string.IsNullOrEmpty(jtSorting))
                {
                    listCount = list.Count();

                    if (listCount > jtPageSize)
                    {
                        list = list.OrderByDescending(s => s.AdvertisementId)
                                 .Skip(jtStartIndex).Take(jtPageSize).ToList();
                    }
                    else
                    {
                        list = list.OrderByDescending(s => s.AdvertisementId).ToList();
                    }
                    advertisementStatusList = list.ToList();
                }
                else
                {
                    listCount = list.Count();

                    if (listCount > jtPageSize)
                    {
                        switch (jtSorting)
                        {
                            case "AdvertisementTitle ASC":
                                list = list.OrderBy(l => l.AdvertisementTitle);
                                break;
                            case "AdvertisementTitle DESC":
                                list = list.OrderByDescending(l => l.AdvertisementTitle);
                                break;
                        }

                        if (string.IsNullOrEmpty(jtSorting))
                            advertisementStatusList = list.OrderByDescending(s => s.AdvertisementId)
                                     .Skip(jtStartIndex).Take(jtPageSize).ToList();
                        else
                            advertisementStatusList = list.Skip(jtStartIndex).Take(jtPageSize).ToList();
                    }
                    else
                    {

                        advertisementStatusList = list.ToList();
                    }
                }

                #endregion


            }
            catch (Exception exc)
            { }



            return advertisementStatusList;
        }



        public long UpdateAdvertisementStatusId(
                    long AdvertisementId,
                    string type,
                    int? StatusId,
                    string? RejectionReason,
                    List<long> childsUsersIds,
                    TeniacoApiContext teniacoApiDb)
        {
            if (type == "Advertisement")
            {
                var advertisement = melkavanApiDb.Advertisement.Where(a => a.AdvertisementId == AdvertisementId && childsUsersIds.Contains(a.UserIdCreator.Value)).FirstOrDefault();
                if (advertisement != null)
                {
                    advertisement.StatusId = StatusId;
                    advertisement.RejectionReason = RejectionReason;
                    melkavanApiDb.Advertisement.Update(advertisement);
                    melkavanApiDb.SaveChanges();
                    return AdvertisementId;
                }
            }
            else if (type == "Properties")
            {
                var property = teniacoApiDb.Properties.Where(p => p.PropertyId == AdvertisementId && childsUsersIds.Contains(p.UserIdCreator.Value)).FirstOrDefault();
                if (property != null)
                {
                    property.StatusId = StatusId;
                    property.RejectionReason = RejectionReason;
                    teniacoApiDb.Properties.Update(property);
                    teniacoApiDb.SaveChanges();
                    return AdvertisementId;
                }
            }

            return 0;


        }


        public long UpdateAdvertisementTagId(
                long AdvertisementId,
                string type,
                string? TagId,
                List<long> childsUsersIds,
                TeniacoApiContext teniacoApiDb)
        {
            if (type == "Advertisement")
            {
                var advertisementDetails = melkavanApiDb.AdvertisementDetails.Where(a => a.AdvertisementId == AdvertisementId && childsUsersIds.Contains(a.UserIdCreator.Value)).FirstOrDefault();
                if (advertisementDetails != null)
                {
                    advertisementDetails.TagId = TagId;
                    melkavanApiDb.AdvertisementDetails.Update(advertisementDetails);
                    melkavanApiDb.SaveChanges();
                    return AdvertisementId;
                }
            }
            else if (type == "Properties")
            {
                var propertyDetails = teniacoApiDb.PropertiesDetails.Where(p => p.PropertyId == AdvertisementId && childsUsersIds.Contains(p.UserIdCreator.Value)).FirstOrDefault();
                if (propertyDetails != null)
                {
                    propertyDetails.TagId = TagId;
                    teniacoApiDb.PropertiesDetails.Update(propertyDetails);
                    teniacoApiDb.SaveChanges();
                    return AdvertisementId;
                }
            }

            return 0;


        }

        #endregion

        #region Methods For Work With AdvertisementFiles

        public List<AdvertisementFilesVM> GetAllAdvertisementFilesList(
            ref int listCount,
            List<long> childsUsersIds,
            long? advertisementId = null,
            string advertisementFileTitle = "",
            string advertisementFileType = "",
            string jtSorting = null)
        {
            List<AdvertisementFilesVM> AdvertisementFilesVMList = new List<AdvertisementFilesVM>();

            try
            {
                var list = (from pf in melkavanApiDb.AdvertisementFiles
                            where childsUsersIds.Contains(pf.UserIdCreator.Value) &&
                            pf.IsDeleted.Value.Equals(false) &&
                            pf.IsActivated.Value.Equals(true) &&
                            pf.AdvertisementId.Equals(advertisementId)
                            select pf).AsQueryable();



                if (!string.IsNullOrEmpty(advertisementFileTitle))
                    list = list.Where(a => a.AdvertisementFileTitle.Contains(advertisementFileTitle));



                if (!string.IsNullOrEmpty(advertisementFileType))
                    list = list.Where(a => a.AdvertisementFileType.Contains(advertisementFileType));


                listCount = list.Count();

                AdvertisementFilesVMList = _mapper.Map<List<AdvertisementFiles>, List<AdvertisementFilesVM>>(list.OrderByDescending(pf => pf.AdvertisementFileId).ToList());
            }
            catch (Exception exc)
            { }

            return AdvertisementFilesVMList;
        }

        public List<AdvertisementFilesVM> GetListOfAdvertisementFiles(int jtStartIndex,
              int jtPageSize,
              ref int listCount,
              List<long> childsUsersIds,
              long? advertisementId = null,
              string advertisementFileTitle = "",
              string advertisementFileType = "",
              string jtSorting = null)
        {
            List<AdvertisementFilesVM> AdvertisementFilesVMList = new List<AdvertisementFilesVM>();

            var list = (from pf in melkavanApiDb.AdvertisementFiles
                        where childsUsersIds.Contains(pf.UserIdCreator.Value) &&
                        pf.AdvertisementId.Equals(advertisementId) && pf.IsDeleted.Value.Equals(false) &&
                        pf.IsActivated.Value.Equals(true)
                        select pf).AsQueryable();

            if (!string.IsNullOrEmpty(advertisementFileTitle))
                list = list.Where(a => a.AdvertisementFileTitle.Contains(advertisementFileTitle));

            if (!string.IsNullOrEmpty(advertisementFileType))
                list = list.Where(a => a.AdvertisementFileType.Contains(advertisementFileType));

            listCount = list.Count();

            try
            {
                if (string.IsNullOrEmpty(jtSorting))
                {
                    if (listCount > jtPageSize)
                    {
                        //zonesVMList = _mapper.Map<List<Zones>, List<ZonesVM>>(list.OrderByDescending(s => s.ZoneId)
                        //         .Skip(jtStartIndex).Take(jtPageSize).ToList());

                        AdvertisementFilesVMList = _mapper.Map<List<AdvertisementFiles>, List<AdvertisementFilesVM>>(list.OrderByDescending(s => s.AdvertisementId)
                                 .Skip(jtStartIndex).Take(jtPageSize).ToList());
                    }
                    else
                    {
                        //zonesVMList = _mapper.Map<List<Zones>,
                        //    List<ZonesVM>>(list.OrderByDescending(s => s.ZoneId).ToList());

                        AdvertisementFilesVMList = _mapper.Map<List<AdvertisementFiles>, List<AdvertisementFilesVM>>(list.OrderByDescending(s => s.AdvertisementId).ToList());
                    }
                }
                else
                {
                    if (listCount > jtPageSize)
                    {
                        switch (jtSorting)
                        {
                            case "AdvertisementFileTitle ASC":
                                list = list.OrderBy(l => l.AdvertisementFileTitle);
                                break;
                            case "AdvertisementFileTitle DESC":
                                list = list.OrderByDescending(l => l.AdvertisementFileTitle);
                                break;
                        }

                        if (string.IsNullOrEmpty(jtSorting))
                            AdvertisementFilesVMList = _mapper.Map<List<AdvertisementFiles>, List<AdvertisementFilesVM>>(list.OrderByDescending(s => s.AdvertisementId)
                                     .Skip(jtStartIndex).Take(jtPageSize).ToList());
                        else
                            AdvertisementFilesVMList = _mapper.Map<List<AdvertisementFiles>, List<AdvertisementFilesVM>>(list.Skip(jtStartIndex).Take(jtPageSize).ToList());
                    }
                    else
                    {
                        AdvertisementFilesVMList = _mapper.Map<List<AdvertisementFiles>, List<AdvertisementFilesVM>>(list.ToList());
                    }
                }

            }
            catch (Exception exc)
            { }
            return AdvertisementFilesVMList;
        }

        public bool AddToAdvertisementFiles(List<AdvertisementFilesVM> AdvertisementFilesVMList,
            List<int>? DeletedPhotosIDs,
            bool? IsMainChanged,
            long? AdvertisementId,
            List<long> childsUsersIds)
        {
            bool result = false;
            try
            {

                var advertisement = melkavanApiDb.Advertisement.Where(a => a.AdvertisementId == AdvertisementId && childsUsersIds.Contains(a.UserIdCreator.Value)).FirstOrDefault(); // برای بررسی سطح دسترسی
                if (advertisement == null)
                {
                    return false;
                }

                //for removing files
                if (DeletedPhotosIDs != null && DeletedPhotosIDs.Count > 0)
                {
                    foreach (int id in DeletedPhotosIDs)
                    {
                        var FileToDelete = melkavanApiDb.AdvertisementFiles
                            .Where(f => f.AdvertisementFileId == id).FirstOrDefault();

                        if (FileToDelete != null)
                        {
                            melkavanApiDb.AdvertisementFiles.Remove(FileToDelete);
                            melkavanApiDb.SaveChanges();
                        }

                    }

                    result = true;
                }


                if (AdvertisementFilesVMList != null)
                    if (AdvertisementFilesVMList.Count > 0)
                    {
                        //main photo is last record in files table
                        var MainFile = melkavanApiDb.AdvertisementFiles
                            .Where(a => a.AdvertisementId == AdvertisementId)
                         .OrderByDescending(a => a.AdvertisementFileId)
                         .FirstOrDefault();


                        var advertisementFilesList = _mapper.Map<List<AdvertisementFilesVM>, List<AdvertisementFiles>>(AdvertisementFilesVMList);

                        melkavanApiDb.AdvertisementFiles.AddRange(advertisementFilesList);
                        melkavanApiDb.SaveChanges();


                        if (MainFile != null && IsMainChanged == false)
                        {
                            melkavanApiDb.AdvertisementFiles.Remove(MainFile);
                            melkavanApiDb.SaveChanges();

                            MainFile.AdvertisementFileId = 0;
                            melkavanApiDb.AdvertisementFiles.Add(MainFile);
                            melkavanApiDb.SaveChanges();
                        }

                        return true;
                    }

            }
            catch (Exception exc)
            {
            }
            return result;
        }


        public AdvertisementFilesVM GetAdvertisementFileWithAdvertisementFileId(int AdvertisementFileId,
            List<long> childsUsersIds)
        {
            AdvertisementFilesVM AdvertisementFilesVM = new AdvertisementFilesVM();

            try
            {
                AdvertisementFilesVM = _mapper.Map<AdvertisementFiles,
                    AdvertisementFilesVM>(melkavanApiDb.AdvertisementFiles
                    .Where(p => childsUsersIds.Contains(p.UserIdCreator.Value))
                    .Where(e => e.AdvertisementFileId.Equals(AdvertisementFileId)).FirstOrDefault());
            }
            catch (Exception exc)
            { }

            return AdvertisementFilesVM;
        }


        public bool UpdateAdvertisementFiles(ref AdvertisementFilesVM AdvertisementFilesVM,
            List<long> childsUsersIds)
        {
            int AdvertisementFileId = AdvertisementFilesVM.AdvertisementFileId;
            long AdvertisementId = AdvertisementFilesVM.AdvertisementId;
            string AdvertisementFileExt = AdvertisementFilesVM.AdvertisementFileExt;
            string AdvertisementFilePath = AdvertisementFilesVM.AdvertisementFilePath;
            string AdvertisementFileTitle = AdvertisementFilesVM.AdvertisementFileTitle;
            string AdvertisementFileType = AdvertisementFilesVM.AdvertisementFileType;

            try
            {
                AdvertisementFiles AdvertisementFiles = (from c in melkavanApiDb.AdvertisementFiles
                                                         where c.AdvertisementFileId == AdvertisementFileId
                                                         select c).FirstOrDefault();

                AdvertisementFiles.AdvertisementId = AdvertisementId;
                AdvertisementFiles.AdvertisementFileExt = AdvertisementFileExt;
                AdvertisementFiles.AdvertisementFilePath = AdvertisementFilePath;
                AdvertisementFiles.AdvertisementFileTitle = AdvertisementFileTitle;
                AdvertisementFiles.AdvertisementFileType = AdvertisementFileType;

                AdvertisementFiles.EditEnDate = AdvertisementFilesVM.EditEnDate.Value;
                AdvertisementFiles.EditTime = AdvertisementFilesVM.EditTime;
                AdvertisementFiles.UserIdEditor = AdvertisementFilesVM.UserIdEditor;
                AdvertisementFiles.IsActivated = AdvertisementFilesVM.IsActivated;
                AdvertisementFiles.IsDeleted = AdvertisementFilesVM.IsDeleted;

                melkavanApiDb.Entry<AdvertisementFiles>(AdvertisementFiles).State = EntityState.Modified;
                melkavanApiDb.SaveChanges();

                return true;
            }
            catch (Exception exc)
            {
            }

            return false;
        }


        public bool ToggleActivationAdvertisementFiles(int AdvertisementFileId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var AdvertisementFiles = (from c in melkavanApiDb.AdvertisementFiles
                                          where c.AdvertisementFileId == AdvertisementFileId &&
                                          childsUsersIds.Contains(c.UserIdCreator.Value)
                                          select c).FirstOrDefault();

                if (AdvertisementFiles != null)
                {
                    AdvertisementFiles.IsActivated = !AdvertisementFiles.IsActivated;
                    AdvertisementFiles.EditEnDate = DateTime.Now;
                    AdvertisementFiles.EditTime = PersianDate.TimeNow;
                    AdvertisementFiles.UserIdEditor = userId;

                    melkavanApiDb.Entry<AdvertisementFiles>(AdvertisementFiles).State = EntityState.Modified;
                    melkavanApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }


        public bool TemporaryDeleteAdvertisementFiles(int AdvertisementFileId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var AdvertisementFiles = (from c in melkavanApiDb.AdvertisementFiles
                                          where c.AdvertisementFileId == AdvertisementFileId &&
                                          childsUsersIds.Contains(c.UserIdCreator.Value)
                                          select c).FirstOrDefault();

                if (AdvertisementFiles != null)
                {
                    AdvertisementFiles.IsDeleted = !AdvertisementFiles.IsDeleted;
                    AdvertisementFiles.EditEnDate = DateTime.Now;
                    AdvertisementFiles.EditTime = PersianDate.TimeNow;
                    AdvertisementFiles.UserIdEditor = userId;

                    melkavanApiDb.Entry<AdvertisementFiles>(AdvertisementFiles).State = EntityState.Modified;
                    melkavanApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }


        public bool CompleteDeleteAdvertisementFiles(int AdvertisementFileId,
            List<long> childsUsersIds)
        {
            try
            {
                var AdvertisementFiles = (from c in melkavanApiDb.AdvertisementFiles
                                          where c.AdvertisementFileId == AdvertisementFileId &&
                                          childsUsersIds.Contains(c.UserIdCreator.Value)
                                          select c).FirstOrDefault();

                if (AdvertisementFiles != null)
                {
                    melkavanApiDb.AdvertisementFiles.Remove(AdvertisementFiles);
                    melkavanApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }



        //public PropertyFeaturesValuesVM GetPropertyFeaturesValues(int propertyId,
        //   int propertyTypeId)
        //{
        //    PropertyFeaturesValuesVM propertyFeaturesValuesVM = new PropertyFeaturesValuesVM();

        //    try
        //    {
        //        //propertyFeaturesValuesVM.ElementTypesVMList = _mapper.Map<List<ElementTypes>,
        //        //            List<ElementTypesTeniacoVM>>(teniacoApiDb.ElementTypes.ToList());

        //        if (teniacoApiDb.Features.Where(f => f.PropertyTypeId.Equals(propertyTypeId)).Any())
        //        {
        //            propertyFeaturesValuesVM.FeaturesVMList = _mapper.Map<List<Features>,
        //                List<FeaturesVM>>(teniacoApiDb.Features.Where(f => f.PropertyTypeId.Equals(propertyTypeId)).ToList());

        //            List<int> featureIds = propertyFeaturesValuesVM.FeaturesVMList.Select(f => f.FeatureId).ToList();

        //            if (teniacoApiDb.FeaturesOptions.Where(fo => featureIds.Contains(fo.FeatureId)).Any())
        //            {
        //                propertyFeaturesValuesVM.FeaturesOptionsVMList = _mapper.Map<List<FeaturesOptions>,
        //                    List<FeaturesOptionsVM>>(teniacoApiDb.FeaturesOptions.Where(fo => featureIds.Contains(fo.FeatureId)).ToList());
        //            }

        //            if (teniacoApiDb.FeaturesValues.Where(fv => fv.PropertyId.Equals(propertyId)).Any())
        //            {
        //                propertyFeaturesValuesVM.FeaturesValuesVMList = propertyFeaturesValuesVM.FeaturesValuesVMList = _mapper.Map<List<FeaturesValues>,
        //                    List<FeaturesValuesVM>>(teniacoApiDb.FeaturesValues.Where(fv => fv.PropertyId.Equals(propertyId)).ToList());
        //            }
        //        }
        //    }
        //    catch (Exception exc)
        //    { }

        //    return propertyFeaturesValuesVM;
        //}



        #endregion

        #region Methods For Work With AdvertisementFeaturesValues

        public AdvertisementFeaturesValuesVM GetAdvertisementFeaturesValues(long AdvertisementId,
              int propertyTypeId,
              TeniacoApiContext teniacoApiDb)
        {
            AdvertisementFeaturesValuesVM advertisementFeaturesValuesVM = new AdvertisementFeaturesValuesVM();

            try
            {

                if (teniacoApiDb.Features.Where(f => f.PropertyTypeId.Equals(propertyTypeId)).Any())
                {
                    var advertisementFeatures = melkavanApiDb.AdvertisementFeatures.Select(af => af.FeatureId).ToList();


                    advertisementFeaturesValuesVM.FeaturesVMList = _mapper.Map<List<Features>,
                        List<FeaturesVM>>(teniacoApiDb.Features.Where(f => f.PropertyTypeId.Equals(propertyTypeId) && advertisementFeatures.Contains(f.FeatureId)).ToList());

                    // codes by sina

                    List<int> featuresCategoriesIds = teniacoApiDb.Features.Where(c => c.PropertyTypeId == propertyTypeId).Select(c => c.FeatureCategoryId).Distinct().ToList();
                    List<int> featureIds = teniacoApiDb.Features.Where(c => featuresCategoriesIds.Contains(c.FeatureCategoryId)).Select(c => c.FeatureId).Distinct().ToList();


                    advertisementFeaturesValuesVM.FeaturesCategoriesVMList = _mapper.Map<List<FeaturesCategories>, List<FeaturesCategoriesVM>>(teniacoApiDb.FeaturesCategories.Where(f => featuresCategoriesIds.Contains(f.FeatureCategoryId)).ToList());


                    if (teniacoApiDb.FeaturesOptions.Where(fo => featureIds.Contains(fo.FeatureId)).Any())
                    {
                        advertisementFeaturesValuesVM.FeaturesOptionsVMList = _mapper.Map<List<FeaturesOptions>,
                            List<FeaturesOptionsVM>>(teniacoApiDb.FeaturesOptions.Where(fo => featureIds.Contains(fo.FeatureId)).ToList());
                    }

                    if (melkavanApiDb.AdvertisementFeaturesValues.Where(fv => fv.AdvertisementId.Equals(AdvertisementId)).Any())
                    {
                        advertisementFeaturesValuesVM.AdvertisementFeaturesValuesVMList = advertisementFeaturesValuesVM.AdvertisementFeaturesValuesVMList = _mapper.Map<List<AdvertisementFeaturesValues>,
                            List<AdvertisementFeaturesValuesVM>>(melkavanApiDb.AdvertisementFeaturesValues.Where(fv => fv.AdvertisementId.Equals(AdvertisementId)).ToList());
                    }
                }
            }
            catch (Exception exc)
            { }

            return advertisementFeaturesValuesVM;
        }



        public AdvertisementFeaturesValuesVM GetAllAdvertisementFeatureValues(
            int advertisementTypeId,
            int propertyTypeId,
            TeniacoApiContext teniacoApiDb)
        {
            AdvertisementFeaturesValuesVM advertisementFeaturesValuesVM = new AdvertisementFeaturesValuesVM();

            try
            {
                List<int> advertisementFeatures = new List<int>();
                if (melkavanApiDb.AdvertisementFeatures.Where(f => f.AdvertisementTypeId.Equals(advertisementTypeId) || f.AdvertisementTypeId == 0).Any())
                {
                    advertisementFeaturesValuesVM.AdvertisementFeaturesVMList = _mapper.Map<List<AdvertisementFeatures>,
                        List<AdvertisementFeaturesVM>>(melkavanApiDb.AdvertisementFeatures.Where(f => f.AdvertisementTypeId.Equals(advertisementTypeId) || f.AdvertisementTypeId == 0).ToList());

                    advertisementFeatures = advertisementFeaturesValuesVM.AdvertisementFeaturesVMList.Select(af => af.FeatureId).ToList();
                }

                // codes by sina
                List<int> featuresCategoriesIds = teniacoApiDb.Features.Where(c => c.PropertyTypeId == propertyTypeId).Select(c => c.FeatureCategoryId).Distinct().ToList();
                List<int> featureIds = teniacoApiDb.Features.Where(c => featuresCategoriesIds.Contains(c.FeatureCategoryId)).Select(c => c.FeatureId).Distinct().ToList();


                if (teniacoApiDb.Features.Where(f => featureIds.Contains(f.FeatureId) && f.PropertyTypeId.Equals(propertyTypeId)).Any())
                {

                    advertisementFeaturesValuesVM.FeaturesVMList = _mapper.Map<List<Features>,
                        List<FeaturesVM>>(teniacoApiDb.Features.Where(f => featureIds.Contains(f.FeatureId) && f.PropertyTypeId.Equals(propertyTypeId) && advertisementFeatures.Contains(f.FeatureId)).ToList());


                    advertisementFeaturesValuesVM.FeaturesCategoriesVMList = _mapper.Map<List<FeaturesCategories>, List<FeaturesCategoriesVM>>(teniacoApiDb.FeaturesCategories.Where(f => featuresCategoriesIds.Contains(f.FeatureCategoryId)).ToList());


                }

                if (teniacoApiDb.FeaturesOptions.Where(fo => featureIds.Contains(fo.FeatureId)).Any())
                {
                    advertisementFeaturesValuesVM.FeaturesOptionsVMList = _mapper.Map<List<FeaturesOptions>,
                        List<FeaturesOptionsVM>>(teniacoApiDb.FeaturesOptions.Where(f => featureIds.Contains(f.FeatureId)).ToList());
                }


                if (melkavanApiDb.AdvertisementFeaturesValues.Where(f => featureIds.Contains(f.FeatureId)).Any())
                {
                    advertisementFeaturesValuesVM.AdvertisementFeaturesValuesVMList = _mapper.Map<List<AdvertisementFeaturesValues>,
                       List<AdvertisementFeaturesValuesVM>>(melkavanApiDb.AdvertisementFeaturesValues.Where(f => featureIds.Contains(f.FeatureId)).ToList());
                }

            }
            catch (Exception exc)
            { }
            return advertisementFeaturesValuesVM;
        }


        public bool UpdateAdvertisementFeatureValues(
           long advertisementId,
           List<AdvertisementFeaturesValuesVM> advertisementFeaturesValuesVMList,
           List<long> childsUsersIds)
        {
            try
            {
                var advertisement = melkavanApiDb.Advertisement.Where(a => a.AdvertisementId == advertisementId && childsUsersIds.Contains(a.UserIdCreator.Value)).FirstOrDefault(); // برای سطح دسترسی
                if (advertisement == null)
                {
                    return false;
                }

                if (melkavanApiDb.AdvertisementFeaturesValues.Where(fv => fv.AdvertisementId.Equals(advertisementId)).Any())
                {
                    var oldFeaturesValues = melkavanApiDb.AdvertisementFeaturesValues.Where(fv => fv.AdvertisementId.Equals(advertisementId)).ToList();
                    melkavanApiDb.AdvertisementFeaturesValues.RemoveRange(oldFeaturesValues);
                    melkavanApiDb.SaveChanges();
                }

                var featuresValuesList = _mapper.Map<List<AdvertisementFeaturesValuesVM>, List<AdvertisementFeaturesValues>>(advertisementFeaturesValuesVMList);

                melkavanApiDb.AdvertisementFeaturesValues.AddRange(featuresValuesList);
                melkavanApiDb.SaveChanges();

                return true;
            }
            catch (Exception exc)
            { }

            return false;
        }



        #endregion

        #region Methods For Work With AdvertisementTypes

        public List<AdvertisementTypesVM> GetAllAdvertisementTypesList(
            ref int listCount,
            List<long> childsUsersIds)
        {
            List<AdvertisementTypesVM> advertisementTypesVMList = new List<AdvertisementTypesVM>();

            try
            {
                var list = (from p in melkavanApiDb.AdvertisementTypes
                                //where childsUsersIds.Contains(p.UserIdCreator.Value) &&
                            where p.IsActivated.Value.Equals(true) &&
                            p.IsDeleted.Value.Equals(false)
                            select new AdvertisementTypesVM
                            {
                                AdvertisementTypeId = p.AdvertisementTypeId,
                                AdvertisementTypeTilte = p.AdvertisementTypeTilte,
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
                                IsDeleted = p.IsDeleted
                            })
                            .AsEnumerable();

                advertisementTypesVMList = list.OrderByDescending(s => s.AdvertisementTypeId).ToList();

            }
            catch (Exception ex)
            { }

            return advertisementTypesVMList;
        }

        #endregion

        #region Methods for Work With AdvertisementPricesHistories
        public List<AdvertisementPricesHistoriesVM> GetListOfAdvertisementPricesHistories(
           int jtStartIndex,
           int jtPageSize,
           ref int listCount,
           List<long> childsUsersIds,
           long advertisementId,
           string jtSorting = null)
        {
            List<AdvertisementPricesHistoriesVM> advertisementPricesHistoriesVMList = new List<AdvertisementPricesHistoriesVM>();


            var list = (from p in melkavanApiDb.AdvertisementPricesHistories
                        where childsUsersIds.Contains(p.UserIdCreator.Value) &&
                        p.IsDeleted.Value.Equals(false) &&
                        p.IsActivated.Value.Equals(true)
                        select new AdvertisementPricesHistoriesVM
                        {
                            AdvertisementPriceHistoryId = p.AdvertisementPriceHistoryId,
                            CalculatedOfferPrice = p.CalculatedOfferPrice,
                            OfferPrice = p.OfferPrice,
                            RentPrice = p.RentPrice,
                            DepositPrice = p.DepositPrice,
                            AdvertisementId = p.AdvertisementId,
                            OfferPriceType = p.OfferPriceType,
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
                        }).AsQueryable();




            if (advertisementId != null)
                if (advertisementId > 0)
                    list = list.Where(a => a.AdvertisementId.Equals(advertisementId));

            try
            {
                if (string.IsNullOrEmpty(jtSorting))
                {
                    listCount = list.Count();

                    if (listCount > jtPageSize)
                    {

                        advertisementPricesHistoriesVMList = list.OrderByDescending(s => s.AdvertisementPriceHistoryId)
                                 .Skip(jtStartIndex).Take(jtPageSize).ToList();
                    }
                    else
                        advertisementPricesHistoriesVMList = list.OrderByDescending(s => s.AdvertisementPriceHistoryId).ToList();
                }
                else
                {
                    listCount = list.Count();

                    if (listCount > jtPageSize)
                    {

                        if (string.IsNullOrEmpty(jtSorting))
                            advertisementPricesHistoriesVMList = list.OrderByDescending(s => s.AdvertisementPriceHistoryId)
                                     .Skip(jtStartIndex).Take(jtPageSize).ToList();
                        else
                            advertisementPricesHistoriesVMList = list.Skip(jtStartIndex).Take(jtPageSize).ToList();
                    }
                    else
                    {

                        advertisementPricesHistoriesVMList = list.ToList();
                    }
                }
            }
            catch (Exception exc)
            { }
            return advertisementPricesHistoriesVMList;
        }

        #endregion

        #region Methods For Work With AdvertisementFeatures

        public List<AdvertisementFeaturesVM> GetAllAdvertisementFeatures(
            List<int> allFeaturesVMIds,
            ref int listCount,
            int? advertisementType)
        {

            List<AdvertisementFeaturesVM> advertisementFeaturesVMs = new List<AdvertisementFeaturesVM>();

            try
            {

                //var list = melkavanApiDb.AdvertisementFeatures.Where(c => c.IsDeleted.Value.Equals(false) &&
                //            c.IsActivated.Value.Equals(true)).AsEnumerable();




                var list = melkavanApiDb.AdvertisementFeatures.Where(c => c.IsActivated.Value.Equals(true) &&
                            c.IsDeleted.Value.Equals(false))
                    .AsEnumerable().Select(a => new AdvertisementFeaturesVM
                    {
                        AdvertisementFeaturesId = a.AdvertisementFeaturesId,
                        AdvertisementFeatureOrder = a.AdvertisementFeatureOrder,
                        AdvertisementTypeId = a.AdvertisementTypeId,
                        FeatureId = a.FeatureId,
                        CreateEnDate = a.CreateEnDate,
                        CreateTime = a.CreateTime,
                        EditEnDate = a.EditEnDate,
                        EditTime = a.EditTime,
                        IsActivated = a.IsActivated,
                        IsDeleted = a.IsDeleted,
                        RemoveEnDate = a.RemoveEnDate,
                        RemoveTime = a.RemoveTime,
                        UserCreatorName = a.UserCreatorName,
                        UserIdCreator = a.UserIdCreator,
                        UserIdEditor = a.UserIdEditor,
                        UserIdRemover = a.UserIdRemover
                    });

                if (allFeaturesVMIds.Count > 0)
                {

                    list = list.Where(a => allFeaturesVMIds.Contains(a.FeatureId));
                }


                if (advertisementType.HasValue && advertisementType != 0)
                {
                    list = list.Where(a => a.AdvertisementTypeId == advertisementType);
                }
                advertisementFeaturesVMs = list.OrderByDescending(a => a.AdvertisementFeatureOrder).ToList();
                listCount = advertisementFeaturesVMs.Count;

                return advertisementFeaturesVMs;
            }
            catch (Exception exc)
            { }

            return null;
        }



        public ManageAdvertisementFeaturesValuesVM GetListOfFeaturesByPropertyTypeId(
            int propertyTypeId,
            TeniacoApiContext teniacoApiDb)
        {

            ManageAdvertisementFeaturesValuesVM manageAdvertisementFeaturesValuesVM = new ManageAdvertisementFeaturesValuesVM();

            List<AdvertisementFeaturesVM> selectedAdvertisementFeaturesVMList = new List<AdvertisementFeaturesVM>();

            try
            {

                if (teniacoApiDb.Features.Where(c => c.PropertyTypeId.Equals(propertyTypeId)).Any())
                {
                    var featureWithPropertyIds = teniacoApiDb.Features.Where(c => c.PropertyTypeId.Equals(propertyTypeId)).ToList();


                    var features = featureWithPropertyIds.Select(a => new FeaturesVM
                    {
                        FeatureId = a.FeatureId,
                        FeatureTitle = a.FeatureTitle,
                        FeatureCategoryId = a.FeatureCategoryId
                    }).ToList();

                    foreach (var feature in features)
                    {
                        feature.FeatureCategoryName = teniacoApiDb.FeaturesCategories.Where(fc => fc.FeatureCategoryId == feature.FeatureCategoryId).Select(fc => fc.FeatureCategoryTitle).FirstOrDefault();
                    }

                    var featureIds = features.Select(f => f.FeatureId).ToList();



                    var advertisementFeaturesList = melkavanApiDb.AdvertisementFeatures.ToList();

                    if (featureIds.Count > 0)
                    {
                        advertisementFeaturesList.Where(c => featureIds.Contains(c.FeatureId));


                        selectedAdvertisementFeaturesVMList = advertisementFeaturesList.Select(a => new AdvertisementFeaturesVM
                        {
                            FeatureId = a.FeatureId,
                            AdvertisementFeatureOrder = a.AdvertisementFeatureOrder,
                            AdvertisementTypeId = a.AdvertisementTypeId,
                            CreateEnDate = a.CreateEnDate,
                            CreateTime = a.CreateTime,
                            EditEnDate = a.EditEnDate,
                            EditTime = a.EditTime,
                            IsActivated = a.IsActivated,
                            IsDeleted = a.IsDeleted,
                            RemoveEnDate = a.RemoveEnDate,
                            RemoveTime = a.RemoveTime,
                            UserCreatorName = a.UserCreatorName,
                            UserIdCreator = a.UserIdCreator,
                            UserIdEditor = a.UserIdEditor,
                            UserIdRemover = a.UserIdRemover
                        }).ToList();
                    }


                    manageAdvertisementFeaturesValuesVM.FeaturesVMList = features;

                    manageAdvertisementFeaturesValuesVM.AdvertisementFeaturesVMList = selectedAdvertisementFeaturesVMList;
                }


                return manageAdvertisementFeaturesValuesVM;

            }
            catch (Exception exc)
            { }

            return null;
        }



        public bool UpdateAdvertisementFeatures(List<int> FeatureIds,
            List<AdvertisementFeaturesVM> advertisementFeaturesVMs,
            List<long> childsUsersIds)
        {

            using (var transaction = melkavanApiDb.Database.BeginTransaction())
            {
                try
                {
                    if (advertisementFeaturesVMs != null && advertisementFeaturesVMs.Count > 0)
                        foreach (var item in advertisementFeaturesVMs)
                        {
                            if (!FeatureIds.Contains(item.FeatureId))
                            {
                                return false;
                            }
                        }

                    var advertisementFeatures = (from c in melkavanApiDb.AdvertisementFeatures
                                                 where FeatureIds.Contains(c.FeatureId)
                                                 select c).ToList();

                    if (advertisementFeatures != null && advertisementFeatures.Count > 0)
                    {
                        melkavanApiDb.AdvertisementFeatures.RemoveRange(advertisementFeatures.ToList());
                    }

                    if (advertisementFeaturesVMs != null && advertisementFeaturesVMs.Count > 0)
                        foreach (var item in advertisementFeaturesVMs)
                        {

                            melkavanApiDb.AdvertisementFeatures.Add(new AdvertisementFeatures
                            {
                                FeatureId = item.FeatureId,
                                AdvertisementFeatureOrder = item.AdvertisementFeatureOrder,
                                AdvertisementTypeId = item.AdvertisementTypeId,
                                CreateEnDate = DateTime.Now,
                                CreateTime = PersianDate.TimeNow,
                            });
                        }
                    transaction.Commit();
                    melkavanApiDb.SaveChanges();
                    return true;
                }
                catch (Exception exc)
                {
                    transaction.Rollback();
                }
            }

            return false;
        }
        #endregion

        #region Methods For Work With AdvertisementViewers


        public List<AdvertisementViewers> GetAdvertisementViewersByAdvertisementId(int? AdvertisementId, string Type, TeniacoApiContext teniacoApiDb)
        {
            if (Type.Equals("Advertisement"))
            {
                List<AdvertisementViewers> advertisementViewers = melkavanApiDb.AdvertisementViewers.Where(v => v.AdvertisementId == AdvertisementId).ToList();
                return advertisementViewers;
            }
            else if (Type.Equals("Properties"))
            {
                List<PropertiesViewers> propertiesViewers = teniacoApiDb.PropertiesViewers.Where(v => v.PropertyId == AdvertisementId).ToList();
                List<AdvertisementViewers> advertisementViewers = new List<AdvertisementViewers>();
                if (propertiesViewers.Count != 0)
                {
                    foreach (var viewer in propertiesViewers)
                    {
                        AdvertisementViewers viewers1 = new AdvertisementViewers()
                        {
                            AdvertisementViewersId = viewer.PropertiesViewersId,
                            AdvertisementId = viewer.PropertyId,
                            UserIdCreator = viewer.UserIdCreator,
                            CreateEnDate = viewer.CreateEnDate,
                            CreateTime = viewer.CreateTime
                        };
                        advertisementViewers.Add(viewers1);
                    }
                }

                return advertisementViewers;
            }
            else
            {
                return null;
            }
        }


        #region Sina's code

        //public List<AdvertisementAdvanceSearchVM> GetWatchedAdvertisementsWithOwnerId(
        //           int jtStartIndex,
        //           int jtPageSize,
        //           ref int listCount,
        //           List<long> childsUsersIds,
        //           PublicApiContext publicApiDb,
        //           TeniacoApiContext teniacoApiDb,
        //           string jtSorting = null,
        //           long? owenerId = null)
        //{

        //    List<AdvertisementAdvanceSearchVM> advertisementAdvanceSearchVMList = new List<AdvertisementAdvanceSearchVM>();

        //    try
        //    {
        //        string sp = @"";

        //        sp = @"
        //            select distinct * from (select 'Advertisement' RecordType,
        //                     MelkavanDbHaghighi.dbo.Advertisement.AdvertisementId,
        //                     '' PropertyTypeTilte,
        //                     '' TypeUseTitle,
        //                     '' DocumentTypeTitle ,
        //                     '' DocumentOwnershipTypeTitle ,
        //                     '' DocumentRootTypeTitle ,
        //                     '' StateName,
        //                     '' CityName,
        //                     '' ZoneName,
        //                     '' DistrictName ,
        //                     '' UserCreatorName,
        //                     MelkavanDbHaghighi.dbo.Advertisement.IsActivated,
        //                     MelkavanDbHaghighi.dbo.Advertisement.IsDeleted,
        //                     MelkavanDbHaghighi.dbo.Advertisement.ConsultantUserId,
        //                     MelkavanDbHaghighi.dbo.Advertisement.EditEnDate,
        //                     MelkavanDbHaghighi.dbo.Advertisement.RebuiltInYearFa,
        //                     MelkavanDbHaghighi.dbo.AdvertisementViewers.CreateEnDate,
        //                     MelkavanDbHaghighi.dbo.Advertisement.PropertyTypeId,
        //                     MelkavanDbHaghighi.dbo.Advertisement.TypeOfUseId,
        //                     MelkavanDbHaghighi.dbo.Advertisement.DocumentOwnershipTypeId,
        //                     MelkavanDbHaghighi.dbo.Advertisement.DocumentTypeId,
        //                     MelkavanDbHaghighi.dbo.Advertisement.DocumentRootTypeId,
        //                     MelkavanDbHaghighi.dbo.Advertisement.AdvertisementTitle,
        //                     MelkavanDbHaghighi.dbo.Advertisement.Area,
        //                     MelkavanDbHaghighi.dbo.Advertisement.AdvertisementDescriptions,
        //                     MelkavanDbHaghighi.dbo.Advertisement.UserIdCreator,
        //                     '1' ShowInMelkavan,
        //                     MelkavanDbHaghighi.dbo.AdvertisementAddress.CountryId,
        //                     MelkavanDbHaghighi.dbo.AdvertisementAddress.StateId,
        //                     MelkavanDbHaghighi.dbo.AdvertisementAddress.CityId,
        //                     MelkavanDbHaghighi.dbo.AdvertisementAddress.ZoneId,
        //                     MelkavanDbHaghighi.dbo.AdvertisementAddress.DistrictId,
        //                     MelkavanDbHaghighi.dbo.AdvertisementAddress.TownName,
        //                     MelkavanDbHaghighi.dbo.AdvertisementDetails.BuildingLifeId,
        //                     MelkavanDbHaghighi.dbo.AdvertisementDetails.Foundation,
        //                     MelkavanDbHaghighi.dbo.AdvertisementDetails.AdvertisementTypeId,
        //                     MelkavanDbHaghighi.dbo.AdvertisementDetails.TagId,
        //                     MelkavanDbHaghighi.dbo.AdvertisementDetails.InstagramLink,
        //                     priceHist.OfferPrice,
        //                     priceHist.DepositPrice,
        //                     priceHist.RentPrice,
        //                     '' CurrentDate,
        //                     '' RejectionReason,
        //                  0  statusId ,
        //                     CAST('false' as bit) IsFavorite
        //                         from MelkavanDbHaghighi.dbo.Advertisement
        //                             inner join MelkavanDbHaghighi.dbo.AdvertisementAddress on Advertisement.AdvertisementId = MelkavanDbHaghighi.dbo.AdvertisementAddress.AdvertisementId
        //                             inner join MelkavanDbHaghighi.dbo.AdvertisementDetails on Advertisement.AdvertisementId = MelkavanDbHaghighi.dbo.AdvertisementDetails.AdvertisementId
        //                             INNER JOIN (
        //                             SELECT AdvertisementId, OfferPrice, DepositPrice, RentPrice
        //                             FROM MelkavanDbHaghighi.dbo.AdvertisementPricesHistories AS aph
        //                             WHERE aph.AdvertisementPriceHistoryId = (
        //                                 SELECT MAX(AdvertisementPriceHistoryId)
        //                                 FROM MelkavanDbHaghighi.dbo.AdvertisementPricesHistories
        //                                 WHERE AdvertisementId = aph.AdvertisementId
        //                             )
        //                         ) AS priceHist ON MelkavanDbHaghighi.dbo.Advertisement.AdvertisementId = priceHist.AdvertisementId
        //        inner join MelkavanDbHaghighi.dbo.AdvertisementOwners on Advertisement.AdvertisementId = MelkavanDbHaghighi.dbo.AdvertisementOwners.AdvertisementId
        //        inner join MelkavanDbHaghighi.dbo.AdvertisementViewers on Advertisement.AdvertisementId = MelkavanDbHaghighi.dbo.AdvertisementViewers.AdvertisementId

        //                           where AdvertisementViewers.UserIdCreator = {owenerId}
        //        union all


        //    select 'Properties' RecordType,
        //            TeniacoDbHaghighi.dbo.Properties.PropertyId as AdvertisementId,
        //            '' PropertyTypeTilte,
        //            '' TypeUseTitle,
        //            '' DocumentTypeTitle ,
        //            '' DocumentOwnershipTypeTitle ,
        //            '' DocumentRootTypeTitle ,
        //            '' StateName,
        //            '' CityName,
        //            '' ZoneName,
        //            '' DistrictName,
        //            '' UserCreatorName,
        //            TeniacoDbHaghighi.dbo.Properties.IsActivated,
        //            TeniacoDbHaghighi.dbo.Properties.IsDeleted,
        //            TeniacoDbHaghighi.dbo.Properties.ConsultantUserId,
        //            TeniacoDbHaghighi.dbo.Properties.EditEnDate,
        //            TeniacoDbHaghighi.dbo.Properties.RebuiltInYearFa,
        //            TeniacoDbHaghighi.dbo.PropertiesViewers.CreateEnDate,
        //            TeniacoDbHaghighi.dbo.Properties.PropertyTypeId,
        //            TeniacoDbHaghighi.dbo.Properties.TypeOfUseId,
        //            TeniacoDbHaghighi.dbo.Properties.DocumentOwnershipTypeId,
        //            TeniacoDbHaghighi.dbo.Properties.DocumentTypeId,
        //            TeniacoDbHaghighi.dbo.Properties.DocumentRootTypeId,
        //            TeniacoDbHaghighi.dbo.Properties.PropertyCodeName as AdvertisementTitle,
        //            TeniacoDbHaghighi.dbo.Properties.Area,
        //            TeniacoDbHaghighi.dbo.Properties.PropertyDescriptions as AdvertisementDescriptions,
        //            TeniacoDbHaghighi.dbo.Properties.UserIdCreator,
        //            TeniacoDbHaghighi.dbo.Properties.ShowInMelkavan,
        //            TeniacoDbHaghighi.dbo.PropertyAddress.CountryId,
        //            TeniacoDbHaghighi.dbo.PropertyAddress.StateId,
        //            TeniacoDbHaghighi.dbo.PropertyAddress.CityId,
        //            TeniacoDbHaghighi.dbo.PropertyAddress.ZoneId,
        //            TeniacoDbHaghighi.dbo.PropertyAddress.DistrictId,
        //            TeniacoDbHaghighi.dbo.PropertyAddress.TownName,
        //            TeniacoDbHaghighi.dbo.PropertiesDetails.BuildingLifeId,
        //            TeniacoDbHaghighi.dbo.PropertiesDetails.Foundation,
        //            TeniacoDbHaghighi.dbo.PropertiesDetails.AdvertisementTypeId,
        //            TeniacoDbHaghighi.dbo.PropertiesDetails.TagId,
        //            '' AS InstagramLink,
        //            propPriceHist.OfferPrice,
        //            propPriceHist.DepositPrice,
        //            propPriceHist.RentPrice,
        //            '' CurrentDate,
        //            '' RejectionReason,
        //         0 statusId ,
        //            CAST('false' as bit) IsFavorite
        //                    from TeniacoDbHaghighi.dbo.Properties

        //        inner join TeniacoDbHaghighi.dbo.PropertyAddress on Properties.PropertyId = TeniacoDbHaghighi.dbo.PropertyAddress.PropertyId
        //        inner join TeniacoDbHaghighi.dbo.PropertiesDetails on Properties.PropertyId = TeniacoDbHaghighi.dbo.PropertiesDetails.PropertyId

        //        INNER JOIN (
        //        SELECT PropertyId, CASE OfferPriceType WHEN '0' THEN CalculatedOfferPrice ELSE OfferPrice END as OfferPrice, DepositPrice, RentPrice
        //        FROM TeniacoDbHaghighi.dbo.PropertiesPricesHistories AS pph
        //        WHERE pph.PropertyPriceHistoryId = (
        //            SELECT MAX(PropertyPriceHistoryId)
        //            FROM TeniacoDbHaghighi.dbo.PropertiesPricesHistories
        //            WHERE PropertyId = pph.PropertyId
        //        )
        //    ) AS propPriceHist ON TeniacoDbHaghighi.dbo.Properties.PropertyId = propPriceHist.PropertyId
        //        inner join TeniacoDbHaghighi.dbo.PropertyOwners on Properties.PropertyId = TeniacoDbHaghighi.dbo.PropertyOwners.PropertyId
        //        inner join TeniacoDbHaghighi.dbo.PropertiesViewers on Properties.PropertyId = TeniacoDbHaghighi.dbo.PropertiesViewers.PropertyId

        //                where PropertiesViewers.UserIdCreator = {owenerId}
        //        ) as tmp
        //           order by CreateEnDate desc";

        //        sp = sp.Replace("{owenerId}", owenerId.ToString());

        //        var list1 = melkavanApiDb.AdvertisementAdvanceSearchVM.FromSqlRaw(sp).AsEnumerable().Distinct();


        //        #region ChildUsers
        //        //if (childsUsersIds != null)
        //        //{
        //        //    if (childsUsersIds.Count > 1)
        //        //    {
        //        //        list1 = list1.Where(p => p.UserIdCreator.HasValue).Where(p => childsUsersIds.Contains(p.UserIdCreator.Value));
        //        //    }
        //        //    else
        //        //    {
        //        //        if (childsUsersIds.Count == 1)
        //        //        {
        //        //            if (childsUsersIds.FirstOrDefault() > 0)
        //        //            {
        //        //                list1 = list1.Where(p => p.UserIdCreator.HasValue).Where(p => childsUsersIds.Contains(p.UserIdCreator.Value));
        //        //            }
        //        //        }
        //        //    }
        //        //}
        //        #endregion

        //        #region Pagination and Load extra title data

        //        try
        //        {
        //            if (string.IsNullOrEmpty(jtSorting))
        //            {
        //                listCount = list1.Count();

        //                //if (listCount > jtPageSize)
        //                //{
        //                list1 = list1.OrderByDescending(s => s.CreateEnDate)
        //                         .Skip(jtStartIndex).Take(jtPageSize).ToList();
        //                //}
        //                //else
        //                //{


        //                //list1 = list1.OrderByDescending(s => s.CreateEnDate).ToList();
        //                //}
        //            }
        //            else
        //            {
        //                listCount = list1.Count();

        //                if (listCount > jtPageSize)
        //                {
        //                    switch (jtSorting)
        //                    {
        //                        case "AdvertisementTitle ASC":
        //                            list1 = list1.OrderBy(l => l.AdvertisementTitle);
        //                            break;
        //                        case "AdvertisementTitle DESC":
        //                            list1 = list1.OrderByDescending(l => l.AdvertisementTitle);
        //                            break;
        //                    }

        //                    if (string.IsNullOrEmpty(jtSorting))
        //                        advertisementAdvanceSearchVMList = list1.OrderByDescending(s => s.CreateEnDate)
        //                                 .Skip(jtStartIndex).Take(jtPageSize).ToList();
        //                    else
        //                        advertisementAdvanceSearchVMList = list1.Skip(jtStartIndex).Take(jtPageSize).ToList();
        //                }
        //                else
        //                {

        //                    advertisementAdvanceSearchVMList = list1.ToList();
        //                }
        //            }





        //            var stateIds = list1.Where(p => p.StateId.HasValue).Select(p => p.StateId.Value).ToList();
        //            var states = publicApiDb.States.Where(s => stateIds.Contains(s.StateId)).ToList();

        //            var cityIds = list1.Where(p => p.CityId.HasValue).Select(p => p.CityId.Value).ToList();
        //            var cities = publicApiDb.Cities.Where(s => cityIds.Contains(s.CityId)).ToList();

        //            var zoneIds = list1.Where(p => p.ZoneId.HasValue).Select(p => p.ZoneId.Value).ToList();
        //            var zones = publicApiDb.Zones.Where(s => zoneIds.Contains(s.ZoneId)).ToList();

        //            var districtIds = list1.Where(p => p.DistrictId.HasValue).Select(p => p.DistrictId.Value).ToList();
        //            var districts = publicApiDb.Districts.Where(s => districtIds.Contains(s.DistrictId)).ToList();

        //            var propertyIds = list1.Select(p => p.AdvertisementId).ToList().Distinct();

        //            var advertisementIds = list1.Select(c => c.AdvertisementId).ToList().Distinct();

        //            var advertisementFilesList = melkavanApiDb.AdvertisementFiles.Where(a => advertisementIds.Contains(a.AdvertisementId) && a.AdvertisementFileType.Equals("media")).ToList();

        //            var propertiesFilesList = teniacoApiDb.PropertyFiles.Where(z => advertisementIds.Contains(z.PropertyId) && z.PropertyFileType.Equals("media")).ToList();


        //            var tmpList = list1.Distinct();

        //            foreach (var item in tmpList)
        //            {


        //                var state = states.Where(s => s.StateId.Equals(item.StateId)).FirstOrDefault();
        //                if (state != null)
        //                {
        //                    item.StateName = state.StateName;
        //                }

        //                var city = cities.Where(c => c.CityId.Equals(item.CityId)).FirstOrDefault();
        //                if (city != null)
        //                {
        //                    item.CityName = city.CityName;
        //                }

        //                if (item.ZoneId.HasValue)
        //                {
        //                    var zone = zones.Where(z => z.ZoneId.Equals(item.ZoneId.Value)).FirstOrDefault();
        //                    if (zone != null)
        //                    {
        //                        item.ZoneName = zone.ZoneName;
        //                    }
        //                }

        //                if (item.DistrictId.HasValue)
        //                {
        //                    var district = districts.Where(z => z.DistrictId.Equals(item.DistrictId.Value)).FirstOrDefault();
        //                    if (district != null)
        //                    {
        //                        item.DistrictName = district.DistrictName;

        //                    }
        //                }

        //                if (owenerId != 0 &&
        //                    melkavanApiDb.AdvertisementFavorites.Where(a => a.UserIdCreator == owenerId && a.AdvertisementId == item.AdvertisementId && a.IsDeleted != true).Any())
        //                {
        //                    item.IsFavorite = true;
        //                }
        //                else
        //                {
        //                    if (owenerId != 0 &&
        //                        teniacoApiDb.PropertiesFavorites.Where(a => a.UserIdCreator == owenerId && a.PropertyId == item.AdvertisementId && a.IsDeleted != true).Any())
        //                    {
        //                        item.IsFavorite = true;
        //                    }
        //                }


        //                if (advertisementFilesList.Where(ad => ad.AdvertisementId == item.AdvertisementId).Any())
        //                {
        //                    try
        //                    {
        //                        item.AdvertisementFilesVM = advertisementFilesList.Where(ad => ad.AdvertisementId == item.AdvertisementId)
        //                            .Select(p => new AdvertisementFilesVM
        //                            {
        //                                AdvertisementId = p.AdvertisementId,
        //                                AdvertisementFilePath = p.AdvertisementFilePath,
        //                                AdvertisementFileExt = p.AdvertisementFileExt,
        //                                AdvertisementFileTitle = p.AdvertisementFileTitle,
        //                                AdvertisementFileId = p.AdvertisementFileId,
        //                                AdvertisementFileType = p.AdvertisementFileType,
        //                            }).OrderByDescending(f => f.AdvertisementFileId).ToList();
        //                    }
        //                    catch (Exception exc)
        //                    { }
        //                }
        //                else
        //                {
        //                    if (propertiesFilesList.Where(ad => ad.PropertyId == item.AdvertisementId).Any())
        //                    {
        //                        try
        //                        {
        //                            item.AdvertisementFilesVM = propertiesFilesList.Where(ad => ad.PropertyId == item.AdvertisementId)
        //                                .Select(p => new AdvertisementFilesVM
        //                                {
        //                                    AdvertisementId = p.PropertyId,
        //                                    AdvertisementFilePath = p.PropertyFilePath,
        //                                    AdvertisementFileExt = p.PropertyFileExt,
        //                                    AdvertisementFileTitle = p.PropertyFileTitle,
        //                                    AdvertisementFileId = p.PropertyFileId,
        //                                    AdvertisementFileType = p.PropertyFileType,
        //                                }).OrderByDescending(f => f.AdvertisementFileId).ToList();
        //                        }
        //                        catch (Exception exc)
        //                        { }
        //                    }
        //                }
        //            }



        //            advertisementAdvanceSearchVMList = tmpList.Distinct().ToList();

        //        }
        //        catch (Exception exc)
        //        { }

        //        #endregion


        //    }
        //    catch (Exception exc)
        //    { }



        //    return advertisementAdvanceSearchVMList;
        //}


        #endregion




        #region Ghaliany's Code


        //public List<AdvertisementAdvanceSearchVM> GetWatchedAdvertisementsWithOwnerId(
        //           int jtStartIndex,
        //           int jtPageSize,
        //           ref int listCount,
        //           List<long> childsUsersIds,
        //           PublicApiContext publicApiDb,
        //           TeniacoApiContext teniacoApiDb,
        //           string jtSorting = null,
        //           long? owenerId = null)
        //{

        //    List<AdvertisementAdvanceSearchVM> advertisementAdvanceSearchVMList = new List<AdvertisementAdvanceSearchVM>();

        //    try
        //    {
        //        string sp = @"";

        //        sp = @"
        //            select distinct * from (select 'Advertisement' RecordType,
        //                     MelkavanDbGhaliany.dbo.Advertisement.AdvertisementId,
        //                     '' PropertyTypeTilte,
        //                     '' TypeUseTitle,
        //                     '' DocumentTypeTitle ,
        //                     '' DocumentOwnershipTypeTitle ,
        //                     '' DocumentRootTypeTitle ,
        //                     '' StateName,
        //                     '' CityName,
        //                     '' ZoneName,
        //                     '' DistrictName ,
        //                     '' UserCreatorName,
        //                     MelkavanDbGhaliany.dbo.Advertisement.IsActivated,
        //                     MelkavanDbGhaliany.dbo.Advertisement.IsDeleted,
        //                     MelkavanDbGhaliany.dbo.Advertisement.ConsultantUserId,
        //                     MelkavanDbGhaliany.dbo.Advertisement.EditEnDate,
        //                     MelkavanDbGhaliany.dbo.Advertisement.RebuiltInYearFa,
        //                     MelkavanDbGhaliany.dbo.AdvertisementViewers.CreateEnDate,
        //                     MelkavanDbGhaliany.dbo.Advertisement.PropertyTypeId,
        //                     MelkavanDbGhaliany.dbo.Advertisement.TypeOfUseId,
        //                     MelkavanDbGhaliany.dbo.Advertisement.DocumentOwnershipTypeId,
        //                     MelkavanDbGhaliany.dbo.Advertisement.DocumentTypeId,
        //                     MelkavanDbGhaliany.dbo.Advertisement.DocumentRootTypeId,
        //                     MelkavanDbGhaliany.dbo.Advertisement.AdvertisementTitle,
        //                     MelkavanDbGhaliany.dbo.Advertisement.Area,
        //                     MelkavanDbGhaliany.dbo.Advertisement.AdvertisementDescriptions,
        //                     MelkavanDbGhaliany.dbo.Advertisement.UserIdCreator,
        //                     '1' ShowInMelkavan,
        //                     MelkavanDbGhaliany.dbo.AdvertisementAddress.CountryId,
        //                     MelkavanDbGhaliany.dbo.AdvertisementAddress.StateId,
        //                     MelkavanDbGhaliany.dbo.AdvertisementAddress.CityId,
        //                     MelkavanDbGhaliany.dbo.AdvertisementAddress.ZoneId,
        //                     MelkavanDbGhaliany.dbo.AdvertisementAddress.DistrictId,
        //                     MelkavanDbGhaliany.dbo.AdvertisementAddress.TownName,
        //                     MelkavanDbGhaliany.dbo.AdvertisementDetails.BuildingLifeId,
        //                     MelkavanDbGhaliany.dbo.AdvertisementDetails.Foundation,
        //                     MelkavanDbGhaliany.dbo.AdvertisementDetails.AdvertisementTypeId,
        //                     MelkavanDbGhaliany.dbo.AdvertisementDetails.TagId,
        //                     MelkavanDbGhaliany.dbo.AdvertisementDetails.InstagramLink,
        //                     priceHist.OfferPrice,
        //                     priceHist.DepositPrice,
        //                     priceHist.RentPrice,
        //                     '' CurrentDate,
        //                     '' RejectionReason,
        //                  0  statusId ,
        //                     CAST('false' as bit) IsFavorite
        //                         from MelkavanDbGhaliany.dbo.Advertisement
        //                             inner join MelkavanDbGhaliany.dbo.AdvertisementAddress on Advertisement.AdvertisementId = MelkavanDbGhaliany.dbo.AdvertisementAddress.AdvertisementId
        //                             inner join MelkavanDbGhaliany.dbo.AdvertisementDetails on Advertisement.AdvertisementId = MelkavanDbGhaliany.dbo.AdvertisementDetails.AdvertisementId
        //                             INNER JOIN (
        //                             SELECT AdvertisementId, OfferPrice, DepositPrice, RentPrice
        //                             FROM MelkavanDbGhaliany.dbo.AdvertisementPricesHistories AS aph
        //                             WHERE aph.AdvertisementPriceHistoryId = (
        //                                 SELECT MAX(AdvertisementPriceHistoryId)
        //                                 FROM MelkavanDbGhaliany.dbo.AdvertisementPricesHistories
        //                                 WHERE AdvertisementId = aph.AdvertisementId
        //                             )
        //                         ) AS priceHist ON MelkavanDbGhaliany.dbo.Advertisement.AdvertisementId = priceHist.AdvertisementId
        //        inner join MelkavanDbGhaliany.dbo.AdvertisementOwners on Advertisement.AdvertisementId = MelkavanDbGhaliany.dbo.AdvertisementOwners.AdvertisementId
        //        inner join MelkavanDbGhaliany.dbo.AdvertisementViewers on Advertisement.AdvertisementId = MelkavanDbGhaliany.dbo.AdvertisementViewers.AdvertisementId

        //                           where AdvertisementViewers.UserIdCreator = {owenerId}
        //        union all


        //    select 'Properties' RecordType,
        //            TeniacoDbGhaliany.dbo.Properties.PropertyId as AdvertisementId,
        //            '' PropertyTypeTilte,
        //            '' TypeUseTitle,
        //            '' DocumentTypeTitle ,
        //            '' DocumentOwnershipTypeTitle ,
        //            '' DocumentRootTypeTitle ,
        //            '' StateName,
        //            '' CityName,
        //            '' ZoneName,
        //            '' DistrictName,
        //            '' UserCreatorName,
        //            TeniacoDbGhaliany.dbo.Properties.IsActivated,
        //            TeniacoDbGhaliany.dbo.Properties.IsDeleted,
        //            TeniacoDbGhaliany.dbo.Properties.ConsultantUserId,
        //            TeniacoDbGhaliany.dbo.Properties.EditEnDate,
        //            TeniacoDbGhaliany.dbo.Properties.RebuiltInYearFa,
        //            TeniacoDbGhaliany.dbo.PropertiesViewers.CreateEnDate,
        //            TeniacoDbGhaliany.dbo.Properties.PropertyTypeId,
        //            TeniacoDbGhaliany.dbo.Properties.TypeOfUseId,
        //            TeniacoDbGhaliany.dbo.Properties.DocumentOwnershipTypeId,
        //            TeniacoDbGhaliany.dbo.Properties.DocumentTypeId,
        //            TeniacoDbGhaliany.dbo.Properties.DocumentRootTypeId,
        //            TeniacoDbGhaliany.dbo.Properties.PropertyCodeName as AdvertisementTitle,
        //            TeniacoDbGhaliany.dbo.Properties.Area,
        //            TeniacoDbGhaliany.dbo.Properties.PropertyDescriptions as AdvertisementDescriptions,
        //            TeniacoDbGhaliany.dbo.Properties.UserIdCreator,
        //            TeniacoDbGhaliany.dbo.Properties.ShowInMelkavan,
        //            TeniacoDbGhaliany.dbo.PropertyAddress.CountryId,
        //            TeniacoDbGhaliany.dbo.PropertyAddress.StateId,
        //            TeniacoDbGhaliany.dbo.PropertyAddress.CityId,
        //            TeniacoDbGhaliany.dbo.PropertyAddress.ZoneId,
        //            TeniacoDbGhaliany.dbo.PropertyAddress.DistrictId,
        //            TeniacoDbGhaliany.dbo.PropertyAddress.TownName,
        //            TeniacoDbGhaliany.dbo.PropertiesDetails.BuildingLifeId,
        //            TeniacoDbGhaliany.dbo.PropertiesDetails.Foundation,
        //            TeniacoDbGhaliany.dbo.PropertiesDetails.AdvertisementTypeId,
        //            TeniacoDbGhaliany.dbo.PropertiesDetails.TagId,
        //            '' AS InstagramLink,
        //            propPriceHist.OfferPrice,
        //            propPriceHist.DepositPrice,
        //            propPriceHist.RentPrice,
        //            '' CurrentDate,
        //            '' RejectionReason,
        //         0 statusId ,
        //            CAST('false' as bit) IsFavorite
        //                    from TeniacoDbGhaliany.dbo.Properties

        //        inner join TeniacoDbGhaliany.dbo.PropertyAddress on Properties.PropertyId = TeniacoDbGhaliany.dbo.PropertyAddress.PropertyId
        //        inner join TeniacoDbGhaliany.dbo.PropertiesDetails on Properties.PropertyId = TeniacoDbGhaliany.dbo.PropertiesDetails.PropertyId

        //        INNER JOIN (
        //        SELECT PropertyId, CASE OfferPriceType WHEN '0' THEN CalculatedOfferPrice ELSE OfferPrice END as OfferPrice, DepositPrice, RentPrice
        //        FROM TeniacoDbGhaliany.dbo.PropertiesPricesHistories AS pph
        //        WHERE pph.PropertyPriceHistoryId = (
        //            SELECT MAX(PropertyPriceHistoryId)
        //            FROM TeniacoDbGhaliany.dbo.PropertiesPricesHistories
        //            WHERE PropertyId = pph.PropertyId
        //        )
        //    ) AS propPriceHist ON TeniacoDbGhaliany.dbo.Properties.PropertyId = propPriceHist.PropertyId
        //        inner join TeniacoDbGhaliany.dbo.PropertyOwners on Properties.PropertyId = TeniacoDbGhaliany.dbo.PropertyOwners.PropertyId
        //        inner join TeniacoDbGhaliany.dbo.PropertiesViewers on Properties.PropertyId = TeniacoDbGhaliany.dbo.PropertiesViewers.PropertyId

        //                where PropertiesViewers.UserIdCreator = {owenerId}
        //        ) as tmp
        //           order by CreateEnDate desc";

        //        sp = sp.Replace("{owenerId}", owenerId.ToString());

        //        var list1 = melkavanApiDb.AdvertisementAdvanceSearchVM.FromSqlRaw(sp).AsEnumerable().Distinct();


        //        #region ChildUsers
        //        //if (childsUsersIds != null)
        //        //{
        //        //    if (childsUsersIds.Count > 1)
        //        //    {
        //        //        list1 = list1.Where(p => p.UserIdCreator.HasValue).Where(p => childsUsersIds.Contains(p.UserIdCreator.Value));
        //        //    }
        //        //    else
        //        //    {
        //        //        if (childsUsersIds.Count == 1)
        //        //        {
        //        //            if (childsUsersIds.FirstOrDefault() > 0)
        //        //            {
        //        //                list1 = list1.Where(p => p.UserIdCreator.HasValue).Where(p => childsUsersIds.Contains(p.UserIdCreator.Value));
        //        //            }
        //        //        }
        //        //    }
        //        //}
        //        #endregion

        //        #region Pagination and Load extra title data

        //        try
        //        {
        //            if (string.IsNullOrEmpty(jtSorting))
        //            {
        //                listCount = list1.Count();

        //                //if (listCount > jtPageSize)
        //                //{
        //                list1 = list1.OrderByDescending(s => s.CreateEnDate)
        //                         .Skip(jtStartIndex).Take(jtPageSize).ToList();
        //                //}
        //                //else
        //                //{


        //                //list1 = list1.OrderByDescending(s => s.CreateEnDate).ToList();
        //                //}
        //            }
        //            else
        //            {
        //                listCount = list1.Count();

        //                if (listCount > jtPageSize)
        //                {
        //                    switch (jtSorting)
        //                    {
        //                        case "AdvertisementTitle ASC":
        //                            list1 = list1.OrderBy(l => l.AdvertisementTitle);
        //                            break;
        //                        case "AdvertisementTitle DESC":
        //                            list1 = list1.OrderByDescending(l => l.AdvertisementTitle);
        //                            break;
        //                    }

        //                    if (string.IsNullOrEmpty(jtSorting))
        //                        advertisementAdvanceSearchVMList = list1.OrderByDescending(s => s.CreateEnDate)
        //                                 .Skip(jtStartIndex).Take(jtPageSize).ToList();
        //                    else
        //                        advertisementAdvanceSearchVMList = list1.Skip(jtStartIndex).Take(jtPageSize).ToList();
        //                }
        //                else
        //                {

        //                    advertisementAdvanceSearchVMList = list1.ToList();
        //                }
        //            }





        //            var stateIds = list1.Where(p => p.StateId.HasValue).Select(p => p.StateId.Value).ToList();
        //            var states = publicApiDb.States.Where(s => stateIds.Contains(s.StateId)).ToList();

        //            var cityIds = list1.Where(p => p.CityId.HasValue).Select(p => p.CityId.Value).ToList();
        //            var cities = publicApiDb.Cities.Where(s => cityIds.Contains(s.CityId)).ToList();

        //            var zoneIds = list1.Where(p => p.ZoneId.HasValue).Select(p => p.ZoneId.Value).ToList();
        //            var zones = publicApiDb.Zones.Where(s => zoneIds.Contains(s.ZoneId)).ToList();

        //            var districtIds = list1.Where(p => p.DistrictId.HasValue).Select(p => p.DistrictId.Value).ToList();
        //            var districts = publicApiDb.Districts.Where(s => districtIds.Contains(s.DistrictId)).ToList();

        //            var propertyIds = list1.Select(p => p.AdvertisementId).ToList().Distinct();

        //            var advertisementIds = list1.Select(c => c.AdvertisementId).ToList().Distinct();

        //            var advertisementFilesList = melkavanApiDb.AdvertisementFiles.Where(a => advertisementIds.Contains(a.AdvertisementId) && a.AdvertisementFileType.Equals("media")).ToList();

        //            var propertiesFilesList = teniacoApiDb.PropertyFiles.Where(z => advertisementIds.Contains(z.PropertyId) && z.PropertyFileType.Equals("media")).ToList();


        //            var tmpList = list1.Distinct();

        //            foreach (var item in tmpList)
        //            {


        //                var state = states.Where(s => s.StateId.Equals(item.StateId)).FirstOrDefault();
        //                if (state != null)
        //                {
        //                    item.StateName = state.StateName;
        //                }

        //                var city = cities.Where(c => c.CityId.Equals(item.CityId)).FirstOrDefault();
        //                if (city != null)
        //                {
        //                    item.CityName = city.CityName;
        //                }

        //                if (item.ZoneId.HasValue)
        //                {
        //                    var zone = zones.Where(z => z.ZoneId.Equals(item.ZoneId.Value)).FirstOrDefault();
        //                    if (zone != null)
        //                    {
        //                        item.ZoneName = zone.ZoneName;
        //                    }
        //                }

        //                if (item.DistrictId.HasValue)
        //                {
        //                    var district = districts.Where(z => z.DistrictId.Equals(item.DistrictId.Value)).FirstOrDefault();
        //                    if (district != null)
        //                    {
        //                        item.DistrictName = district.DistrictName;

        //                    }
        //                }

        //                if (owenerId != 0 &&
        //                    melkavanApiDb.AdvertisementFavorites.Where(a => a.UserIdCreator == owenerId && a.AdvertisementId == item.AdvertisementId && a.IsDeleted != true).Any())
        //                {
        //                    item.IsFavorite = true;
        //                }
        //                else
        //                {
        //                    if (owenerId != 0 &&
        //                        teniacoApiDb.PropertiesFavorites.Where(a => a.UserIdCreator == owenerId && a.PropertyId == item.AdvertisementId && a.IsDeleted != true).Any())
        //                    {
        //                        item.IsFavorite = true;
        //                    }
        //                }


        //                if (advertisementFilesList.Where(ad => ad.AdvertisementId == item.AdvertisementId).Any())
        //                {
        //                    try
        //                    {
        //                        item.AdvertisementFilesVM = advertisementFilesList.Where(ad => ad.AdvertisementId == item.AdvertisementId)
        //                            .Select(p => new AdvertisementFilesVM
        //                            {
        //                                AdvertisementId = p.AdvertisementId,
        //                                AdvertisementFilePath = p.AdvertisementFilePath,
        //                                AdvertisementFileExt = p.AdvertisementFileExt,
        //                                AdvertisementFileTitle = p.AdvertisementFileTitle,
        //                                AdvertisementFileId = p.AdvertisementFileId,
        //                                AdvertisementFileType = p.AdvertisementFileType,
        //                            }).OrderByDescending(f => f.AdvertisementFileId).ToList();
        //                    }
        //                    catch (Exception exc)
        //                    { }
        //                }
        //                else
        //                {
        //                    if (propertiesFilesList.Where(ad => ad.PropertyId == item.AdvertisementId).Any())
        //                    {
        //                        try
        //                        {
        //                            item.AdvertisementFilesVM = propertiesFilesList.Where(ad => ad.PropertyId == item.AdvertisementId)
        //                                .Select(p => new AdvertisementFilesVM
        //                                {
        //                                    AdvertisementId = p.PropertyId,
        //                                    AdvertisementFilePath = p.PropertyFilePath,
        //                                    AdvertisementFileExt = p.PropertyFileExt,
        //                                    AdvertisementFileTitle = p.PropertyFileTitle,
        //                                    AdvertisementFileId = p.PropertyFileId,
        //                                    AdvertisementFileType = p.PropertyFileType,
        //                                }).OrderByDescending(f => f.AdvertisementFileId).ToList();
        //                        }
        //                        catch (Exception exc)
        //                        { }
        //                    }
        //                }
        //            }



        //            advertisementAdvanceSearchVMList = tmpList.Distinct().ToList();

        //        }
        //        catch (Exception exc)
        //        { }

        //        #endregion


        //    }
        //    catch (Exception exc)
        //    { }



        //    return advertisementAdvanceSearchVMList;
        //}
        #endregion




        public List<AdvertisementAdvanceSearchVM> GetWatchedAdvertisementsWithOwnerId(
                    int jtStartIndex,
                    int jtPageSize,
                    ref int listCount,
                    List<long> childsUsersIds,
                    PublicApiContext publicApiDb,
                    TeniacoApiContext teniacoApiDb,
                    string jtSorting = null,
                    long? owenerId = null)
        {

            List<AdvertisementAdvanceSearchVM> advertisementAdvanceSearchVMList = new List<AdvertisementAdvanceSearchVM>();

            try
            {
                string sp = @"";

                sp = @"
                    select distinct * from (select 'Advertisement' RecordType,
                             MelkavanDb.dbo.Advertisement.AdvertisementId,
                             '' PropertyTypeTilte,
                             '' TypeUseTitle,
                             '' DocumentTypeTitle ,
                             '' DocumentOwnershipTypeTitle ,
                             '' DocumentRootTypeTitle ,
                             '' StateName,
                             '' CityName,
                             '' ZoneName,
                             '' DistrictName ,
                             '' UserCreatorName,
                             MelkavanDb.dbo.Advertisement.IsActivated,
                             MelkavanDb.dbo.Advertisement.IsDeleted,
                             MelkavanDb.dbo.Advertisement.ConsultantUserId,
                             MelkavanDb.dbo.Advertisement.EditEnDate,
                             MelkavanDb.dbo.Advertisement.RebuiltInYearFa,
                             MelkavanDb.dbo.AdvertisementViewers.CreateEnDate,
                             MelkavanDb.dbo.Advertisement.PropertyTypeId,
                             MelkavanDb.dbo.Advertisement.TypeOfUseId,
                             MelkavanDb.dbo.Advertisement.DocumentOwnershipTypeId,
                             MelkavanDb.dbo.Advertisement.DocumentTypeId,
                             MelkavanDb.dbo.Advertisement.DocumentRootTypeId,
                             MelkavanDb.dbo.Advertisement.AdvertisementTitle,
                             MelkavanDb.dbo.Advertisement.Area,
                             MelkavanDb.dbo.Advertisement.AdvertisementDescriptions,
                             MelkavanDb.dbo.Advertisement.UserIdCreator,
                             '1' ShowInMelkavan,
                             MelkavanDb.dbo.AdvertisementAddress.CountryId,
                             MelkavanDb.dbo.AdvertisementAddress.StateId,
                             MelkavanDb.dbo.AdvertisementAddress.CityId,
                             MelkavanDb.dbo.AdvertisementAddress.ZoneId,
                             MelkavanDb.dbo.AdvertisementAddress.DistrictId,
                             MelkavanDb.dbo.AdvertisementAddress.TownName,
                             MelkavanDb.dbo.AdvertisementDetails.BuildingLifeId,
                             MelkavanDb.dbo.AdvertisementDetails.Foundation,
                             MelkavanDb.dbo.AdvertisementDetails.AdvertisementTypeId,
                             MelkavanDb.dbo.AdvertisementDetails.TagId,
                             MelkavanDb.dbo.AdvertisementDetails.InstagramLink,
                             priceHist.OfferPrice,
                             priceHist.DepositPrice,
                             priceHist.RentPrice,
                             '' CurrentDate,
                             '' RejectionReason,
	                         0  statusId ,
                             CAST('false' as bit) IsFavorite
                                 from MelkavanDb.dbo.Advertisement
                                     inner join MelkavanDb.dbo.AdvertisementAddress on Advertisement.AdvertisementId = MelkavanDb.dbo.AdvertisementAddress.AdvertisementId
                                     inner join MelkavanDb.dbo.AdvertisementDetails on Advertisement.AdvertisementId = MelkavanDb.dbo.AdvertisementDetails.AdvertisementId
                                     INNER JOIN (
                                     SELECT AdvertisementId, OfferPrice, DepositPrice, RentPrice
                                     FROM MelkavanDb.dbo.AdvertisementPricesHistories AS aph
                                     WHERE aph.AdvertisementPriceHistoryId = (
                                         SELECT MAX(AdvertisementPriceHistoryId)
                                         FROM MelkavanDb.dbo.AdvertisementPricesHistories
                                         WHERE AdvertisementId = aph.AdvertisementId
                                     )
                                 ) AS priceHist ON MelkavanDb.dbo.Advertisement.AdvertisementId = priceHist.AdvertisementId
                inner join MelkavanDb.dbo.AdvertisementOwners on Advertisement.AdvertisementId = MelkavanDb.dbo.AdvertisementOwners.AdvertisementId
                inner join MelkavanDb.dbo.AdvertisementViewers on Advertisement.AdvertisementId = MelkavanDb.dbo.AdvertisementViewers.AdvertisementId

                                   where AdvertisementViewers.UserIdCreator = {owenerId}
                union all


            select 'Properties' RecordType,
                    TeniacoDb.dbo.Properties.PropertyId as AdvertisementId,
                    '' PropertyTypeTilte,
                    '' TypeUseTitle,
                    '' DocumentTypeTitle ,
                    '' DocumentOwnershipTypeTitle ,
                    '' DocumentRootTypeTitle ,
                    '' StateName,
                    '' CityName,
                    '' ZoneName,
                    '' DistrictName,
                    '' UserCreatorName,
                    TeniacoDb.dbo.Properties.IsActivated,
                    TeniacoDb.dbo.Properties.IsDeleted,
                    TeniacoDb.dbo.Properties.ConsultantUserId,
                    TeniacoDb.dbo.Properties.EditEnDate,
                    TeniacoDb.dbo.Properties.RebuiltInYearFa,
                    TeniacoDb.dbo.PropertiesViewers.CreateEnDate,
                    TeniacoDb.dbo.Properties.PropertyTypeId,
                    TeniacoDb.dbo.Properties.TypeOfUseId,
                    TeniacoDb.dbo.Properties.DocumentOwnershipTypeId,
                    TeniacoDb.dbo.Properties.DocumentTypeId,
                    TeniacoDb.dbo.Properties.DocumentRootTypeId,
                    TeniacoDb.dbo.Properties.PropertyCodeName as AdvertisementTitle,
                    TeniacoDb.dbo.Properties.Area,
                    TeniacoDb.dbo.Properties.PropertyDescriptions as AdvertisementDescriptions,
                    TeniacoDb.dbo.Properties.UserIdCreator,
                    TeniacoDb.dbo.Properties.ShowInMelkavan,
                    TeniacoDb.dbo.PropertyAddress.CountryId,
                    TeniacoDb.dbo.PropertyAddress.StateId,
                    TeniacoDb.dbo.PropertyAddress.CityId,
                    TeniacoDb.dbo.PropertyAddress.ZoneId,
                    TeniacoDb.dbo.PropertyAddress.DistrictId,
                    TeniacoDb.dbo.PropertyAddress.TownName,
                    TeniacoDb.dbo.PropertiesDetails.BuildingLifeId,
                    TeniacoDb.dbo.PropertiesDetails.Foundation,
                    TeniacoDb.dbo.PropertiesDetails.AdvertisementTypeId,
                    TeniacoDb.dbo.PropertiesDetails.TagId,
                    '' AS InstagramLink,
                    propPriceHist.OfferPrice,
                    propPriceHist.DepositPrice,
                    propPriceHist.RentPrice,
                    '' CurrentDate,
                    '' RejectionReason,
	                0 statusId ,
                    CAST('false' as bit) IsFavorite
                            from TeniacoDb.dbo.Properties

                inner join TeniacoDb.dbo.PropertyAddress on Properties.PropertyId = TeniacoDb.dbo.PropertyAddress.PropertyId
                inner join TeniacoDb.dbo.PropertiesDetails on Properties.PropertyId = TeniacoDb.dbo.PropertiesDetails.PropertyId

                INNER JOIN (
                SELECT PropertyId, CASE OfferPriceType WHEN '0' THEN CalculatedOfferPrice ELSE OfferPrice END as OfferPrice, DepositPrice, RentPrice
                FROM TeniacoDb.dbo.PropertiesPricesHistories AS pph
                WHERE pph.PropertyPriceHistoryId = (
                    SELECT MAX(PropertyPriceHistoryId)
                    FROM TeniacoDb.dbo.PropertiesPricesHistories
                    WHERE PropertyId = pph.PropertyId
                )
            ) AS propPriceHist ON TeniacoDb.dbo.Properties.PropertyId = propPriceHist.PropertyId
                inner join TeniacoDb.dbo.PropertyOwners on Properties.PropertyId = TeniacoDb.dbo.PropertyOwners.PropertyId
                inner join TeniacoDb.dbo.PropertiesViewers on Properties.PropertyId = TeniacoDb.dbo.PropertiesViewers.PropertyId

                        where PropertiesViewers.UserIdCreator = {owenerId}
                ) as tmp
                   order by CreateEnDate desc";

                sp = sp.Replace("{owenerId}", owenerId.ToString());

                var list1 = melkavanApiDb.AdvertisementAdvanceSearchVM.FromSqlRaw(sp).AsEnumerable().Distinct();


                #region ChildUsers
                //if (childsUsersIds != null)
                //{
                //    if (childsUsersIds.Count > 1)
                //    {
                //        list1 = list1.Where(p => p.UserIdCreator.HasValue).Where(p => childsUsersIds.Contains(p.UserIdCreator.Value));
                //    }
                //    else
                //    {
                //        if (childsUsersIds.Count == 1)
                //        {
                //            if (childsUsersIds.FirstOrDefault() > 0)
                //            {
                //                list1 = list1.Where(p => p.UserIdCreator.HasValue).Where(p => childsUsersIds.Contains(p.UserIdCreator.Value));
                //            }
                //        }
                //    }
                //}
                #endregion

                #region Pagination and Load extra title data

                try
                {
                    if (string.IsNullOrEmpty(jtSorting))
                    {
                        listCount = list1.Count();

                        //if (listCount > jtPageSize)
                        //{
                        list1 = list1.OrderByDescending(s => s.CreateEnDate)
                                 .Skip(jtStartIndex).Take(jtPageSize).ToList();
                        //}
                        //else
                        //{


                        //list1 = list1.OrderByDescending(s => s.CreateEnDate).ToList();
                        //}
                    }
                    else
                    {
                        listCount = list1.Count();

                        if (listCount > jtPageSize)
                        {
                            switch (jtSorting)
                            {
                                case "AdvertisementTitle ASC":
                                    list1 = list1.OrderBy(l => l.AdvertisementTitle);
                                    break;
                                case "AdvertisementTitle DESC":
                                    list1 = list1.OrderByDescending(l => l.AdvertisementTitle);
                                    break;
                            }

                            if (string.IsNullOrEmpty(jtSorting))
                                advertisementAdvanceSearchVMList = list1.OrderByDescending(s => s.CreateEnDate)
                                         .Skip(jtStartIndex).Take(jtPageSize).ToList();
                            else
                                advertisementAdvanceSearchVMList = list1.Skip(jtStartIndex).Take(jtPageSize).ToList();
                        }
                        else
                        {

                            advertisementAdvanceSearchVMList = list1.ToList();
                        }
                    }





                    var stateIds = list1.Where(p => p.StateId.HasValue).Select(p => p.StateId.Value).ToList();
                    var states = publicApiDb.States.Where(s => stateIds.Contains(s.StateId)).ToList();

                    var cityIds = list1.Where(p => p.CityId.HasValue).Select(p => p.CityId.Value).ToList();
                    var cities = publicApiDb.Cities.Where(s => cityIds.Contains(s.CityId)).ToList();

                    var zoneIds = list1.Where(p => p.ZoneId.HasValue).Select(p => p.ZoneId.Value).ToList();
                    var zones = publicApiDb.Zones.Where(s => zoneIds.Contains(s.ZoneId)).ToList();

                    var districtIds = list1.Where(p => p.DistrictId.HasValue).Select(p => p.DistrictId.Value).ToList();
                    var districts = publicApiDb.Districts.Where(s => districtIds.Contains(s.DistrictId)).ToList();

                    var propertyIds = list1.Select(p => p.AdvertisementId).ToList().Distinct();

                    var advertisementIds = list1.Select(c => c.AdvertisementId).ToList().Distinct();

                    var advertisementFilesList = melkavanApiDb.AdvertisementFiles.Where(a => advertisementIds.Contains(a.AdvertisementId) && a.AdvertisementFileType.Equals("media")).ToList();

                    var propertiesFilesList = teniacoApiDb.PropertyFiles.Where(z => advertisementIds.Contains(z.PropertyId) && z.PropertyFileType.Equals("media")).ToList();


                    var tmpList = list1.Distinct();

                    foreach (var item in tmpList)
                    {


                        var state = states.Where(s => s.StateId.Equals(item.StateId)).FirstOrDefault();
                        if (state != null)
                        {
                            item.StateName = state.StateName;
                        }

                        var city = cities.Where(c => c.CityId.Equals(item.CityId)).FirstOrDefault();
                        if (city != null)
                        {
                            item.CityName = city.CityName;
                        }

                        if (item.ZoneId.HasValue)
                        {
                            var zone = zones.Where(z => z.ZoneId.Equals(item.ZoneId.Value)).FirstOrDefault();
                            if (zone != null)
                            {
                                item.ZoneName = zone.ZoneName;
                            }
                        }

                        if (item.DistrictId.HasValue)
                        {
                            var district = districts.Where(z => z.DistrictId.Equals(item.DistrictId.Value)).FirstOrDefault();
                            if (district != null)
                            {
                                item.DistrictName = district.DistrictName;

                            }
                        }

                        if (owenerId != 0 &&
                            melkavanApiDb.AdvertisementFavorites.Where(a => a.UserIdCreator == owenerId && a.AdvertisementId == item.AdvertisementId && a.IsDeleted != true).Any())
                        {
                            item.IsFavorite = true;
                        }
                        else
                        {
                            if (owenerId != 0 &&
                                teniacoApiDb.PropertiesFavorites.Where(a => a.UserIdCreator == owenerId && a.PropertyId == item.AdvertisementId && a.IsDeleted != true).Any())
                            {
                                item.IsFavorite = true;
                            }
                        }


                        if (advertisementFilesList.Where(ad => ad.AdvertisementId == item.AdvertisementId).Any())
                        {
                            try
                            {
                                item.AdvertisementFilesVM = advertisementFilesList.Where(ad => ad.AdvertisementId == item.AdvertisementId)
                                    .Select(p => new AdvertisementFilesVM
                                    {
                                        AdvertisementId = p.AdvertisementId,
                                        AdvertisementFilePath = p.AdvertisementFilePath,
                                        AdvertisementFileExt = p.AdvertisementFileExt,
                                        AdvertisementFileTitle = p.AdvertisementFileTitle,
                                        AdvertisementFileId = p.AdvertisementFileId,
                                        AdvertisementFileType = p.AdvertisementFileType,
                                    }).OrderByDescending(f => f.AdvertisementFileId).ToList();
                            }
                            catch (Exception exc)
                            { }
                        }
                        else
                        {
                            if (propertiesFilesList.Where(ad => ad.PropertyId == item.AdvertisementId).Any())
                            {
                                try
                                {
                                    item.AdvertisementFilesVM = propertiesFilesList.Where(ad => ad.PropertyId == item.AdvertisementId)
                                        .Select(p => new AdvertisementFilesVM
                                        {
                                            AdvertisementId = p.PropertyId,
                                            AdvertisementFilePath = p.PropertyFilePath,
                                            AdvertisementFileExt = p.PropertyFileExt,
                                            AdvertisementFileTitle = p.PropertyFileTitle,
                                            AdvertisementFileId = p.PropertyFileId,
                                            AdvertisementFileType = p.PropertyFileType,
                                        }).OrderByDescending(f => f.AdvertisementFileId).ToList();
                                }
                                catch (Exception exc)
                                { }
                            }
                        }
                    }



                    advertisementAdvanceSearchVMList = tmpList.Distinct().ToList();

                }
                catch (Exception exc)
                { }

                #endregion


            }
            catch (Exception exc)
            { }



            return advertisementAdvanceSearchVMList;
        }

        public List<AdvertisementViewersVM> GetAdvertisementViewersWithAdvertisementId(int? advertisementId)
        {
            List<AdvertisementViewersVM> advertisementViewersVMList = new List<AdvertisementViewersVM>();
            try
            {
                if (advertisementId.HasValue)
                    if (advertisementId.Value > 0)
                    {
                        if (melkavanApiDb.AdvertisementViewers.Where(v => v.AdvertisementId.Equals(advertisementId.Value)).Any())
                        {
                            advertisementViewersVMList = _mapper.Map<List<AdvertisementViewers>, List<AdvertisementViewersVM>>(
                                melkavanApiDb.AdvertisementViewers.Where(v => v.AdvertisementId.Equals(advertisementId.Value)).ToList());
                        }
                    }

            }
            catch (Exception exc)
            { }
            return advertisementViewersVMList;
        }


        public int AddToAdvertisementViewers(AdvertisementViewersVM advertisementViewersVM, TeniacoApiContext teniacoApiDb)
        {

            try
            {

                if (advertisementViewersVM.UserIdCreator.HasValue)
                    if (advertisementViewersVM.UserIdCreator.Value > 0)

                    {


                        //If type is properties then add to PropertiesViewers
                        if (advertisementViewersVM.Type == "Properties")
                        {

                            PropertiesViewers pv = new PropertiesViewers()
                            {
                                PropertyId = advertisementViewersVM.AdvertisementId,
                                UserIdCreator = advertisementViewersVM.UserIdCreator,
                                CreateEnDate = advertisementViewersVM.CreateEnDate,
                                CreateTime = advertisementViewersVM.CreateTime,
                                IsActivated = true,
                                IsDeleted = false
                            };
                            teniacoApiDb.PropertiesViewers.Add(pv);
                            teniacoApiDb.SaveChanges();

                            return pv.PropertiesViewersId;
                        }
                        //If type is advertisement then add to AdvertisementsViewers
                        else if (advertisementViewersVM.Type == "Advertisement")
                        {
                            AdvertisementViewers av = new AdvertisementViewers()
                            {
                                AdvertisementId = advertisementViewersVM.AdvertisementId,
                                UserIdCreator = advertisementViewersVM.UserIdCreator,
                                CreateEnDate = advertisementViewersVM.CreateEnDate,
                                CreateTime = advertisementViewersVM.CreateTime,
                                IsActivated = true,
                                IsDeleted = false
                            };

                            melkavanApiDb.AdvertisementViewers.Add(av);
                            melkavanApiDb.SaveChanges();



                            return av.AdvertisementViewersId;
                        }

                    }

            }
            catch (Exception exc)
            { }

            return 0;
        }

        public List<AdvertisementViewersReportPerMonthVM> GetAdvertisementsViewersReportWithIdAndFilterType(GetAdvertisementsViewersReportWithIdAndFilterTypePVM getAdvertisementsViewersReportWithIdAndFilterType, TeniacoApiContext teniacoApiDb)
        {
            var persianCalendar = new PersianCalendar();
            DateTime dayValue = DateTime.Now;
            IEnumerable<DateTime> consecutiveDays = null;
            switch (getAdvertisementsViewersReportWithIdAndFilterType.FilterType)
            {
                case "60days":
                    dayValue = DateTime.Now.AddDays(-58);
                    consecutiveDays = Enumerable.Range(0, 60)
                   .Select(offset => dayValue.AddDays(offset))
                   .Select(d => d.Date);
                    break;
                case "30days":
                    dayValue = DateTime.Now.AddDays(-29);
                    consecutiveDays = Enumerable.Range(0, 30)
                   .Select(offset => dayValue.AddDays(offset))
                   .Select(d => d.Date);
                    break;
                case "15days":
                    dayValue = DateTime.Now.AddDays(-14);
                    consecutiveDays = Enumerable.Range(0, 15)
                     .Select(offset => dayValue.AddDays(offset))
                     .Select(d => d.Date);
                    break;
                default:
                    dayValue = DateTime.Now.AddDays(-6);
                    consecutiveDays = Enumerable.Range(0, 7)
                   .Select(offset => dayValue.AddDays(offset))
                   .Select(d => d.Date);
                    break;
            }


            List <AdvertisementViewersReportPerMonthVM> viewsPerDayForAdvertisementsList = new();
            if (getAdvertisementsViewersReportWithIdAndFilterType.RecordType == "Advertisement")
            {
                viewsPerDayForAdvertisementsList = melkavanApiDb.AdvertisementViewers
                    .Where(x => x.AdvertisementId == getAdvertisementsViewersReportWithIdAndFilterType.AdvertisementId
                                && x.CreateEnDate != null
                                && x.CreateEnDate.Value >= dayValue)
                    .GroupBy(g => g.CreateEnDate.Value.Date)
                    .Select(g => new AdvertisementViewersReportPerMonthVM
                    {
                        Count = g.Count(),
                        Day = g.Key.ToString("yyyy/MM/dd")
                    })
                    .ToList();
            }
            else if (getAdvertisementsViewersReportWithIdAndFilterType.RecordType == "Properties")
            {

                viewsPerDayForAdvertisementsList = teniacoApiDb.PropertiesViewers
                    .Where(x => x.PropertyId == getAdvertisementsViewersReportWithIdAndFilterType.AdvertisementId
                                && x.CreateEnDate != null
                                && x.CreateEnDate.Value >= dayValue)
                    .GroupBy(g => g.CreateEnDate.Value.Date)
                    .Select(g => new AdvertisementViewersReportPerMonthVM
                    {
                        Count = g.Count(),
                        Day = g.Key.ToString("yyyy/MM/dd")
                    })
                    .ToList();
            }





            var result = consecutiveDays
        .GroupJoin(
        viewsPerDayForAdvertisementsList,
        day => day,
        view => DateTime.ParseExact(view.Day, "yyyy/MM/dd", CultureInfo.InvariantCulture).Date,
        (day, views) => new AdvertisementViewersReportPerMonthVM
        {
            Count = views.Any() ? views.Sum(v => v.Count) : 0,
            Day = day.ToString("yyyy/MM/dd")
        })
        .Select(item => new AdvertisementViewersReportPerMonthVM
        {
            Count = item.Count,
            Day = persianCalendar.GetYear(DateTime.ParseExact(item.Day, "yyyy/MM/dd", CultureInfo.InvariantCulture)).ToString("0000") +
                  "/" +
                  persianCalendar.GetMonth(DateTime.ParseExact(item.Day, "yyyy/MM/dd", CultureInfo.InvariantCulture)).ToString("00") +
                  "/" +
                  persianCalendar.GetDayOfMonth(DateTime.ParseExact(item.Day, "yyyy/MM/dd", CultureInfo.InvariantCulture)).ToString("00")
        })
        .ToList();

            return result;

        }

        #endregion

        #region Methods For Work With AdvertisementCallers
        public List<AdvertisementCallersVM> GetAdvertisementCallersWithAdvertisementId(int? advertisementId)
        {
            List<AdvertisementCallersVM> advertisementCallersVMList = new List<AdvertisementCallersVM>();
            try
            {

                if (advertisementId.HasValue)
                    if (advertisementId.Value > 0)
                    {
                        if (melkavanApiDb.AdvertisementCallers.Where(c => c.AdvertisementId.Equals(advertisementId.Value)).Any())
                        {
                            advertisementCallersVMList = _mapper.Map<List<AdvertisementCallers>, List<AdvertisementCallersVM>>(
                                melkavanApiDb.AdvertisementCallers.Where(v => v.AdvertisementId.Equals(advertisementId.Value)).ToList());
                        }
                    }

            }
            catch (Exception exc)
            { }
            return advertisementCallersVMList;
        }


        public int AddToAdvertisementCallers(AdvertisementCallersVM advertisementCallersVM)
        {

            try
            {

                if (advertisementCallersVM.UserIdCreator.HasValue)
                    if (advertisementCallersVM.UserIdCreator.Value > 0)

                    {
                        AdvertisementCallers advertisementCallers = _mapper.Map<AdvertisementCallersVM, AdvertisementCallers>(advertisementCallersVM);

                        advertisementCallers.IsActivated = true;
                        advertisementCallers.IsDeleted = false;

                        melkavanApiDb.AdvertisementCallers.Add(advertisementCallers);
                        melkavanApiDb.SaveChanges();

                        return advertisementCallers.AdvertisementCallersId;
                    }

            }
            catch (Exception exc)
            { }

            return 0;
        }


        #endregion

        #region Methods For Work With AdvertisementFavorites


        public long AddToAdvertisementFavorites(AdvertisementFavoritesVM advertisementFavoritesVM, TeniacoApiContext teniacoApiDb)
        {

            try
            {

                if (advertisementFavoritesVM.UserIdCreator.HasValue)
                    if (advertisementFavoritesVM.UserIdCreator.Value > 0)
                    {

                        if (advertisementFavoritesVM.Type.Equals("Advertisement"))
                        {

                            AdvertisementFavorites advertisementFavorites = _mapper.Map<AdvertisementFavoritesVM, AdvertisementFavorites>(advertisementFavoritesVM);


                            if (melkavanApiDb.AdvertisementFavorites.Where(c => c.AdvertisementId.Equals(advertisementFavorites.AdvertisementId)).Any())
                            {
                                advertisementFavorites = melkavanApiDb.AdvertisementFavorites.Where(c => c.AdvertisementId.Equals(advertisementFavorites.AdvertisementId)).FirstOrDefault();


                                melkavanApiDb.AdvertisementFavorites.Remove(advertisementFavorites);
                                melkavanApiDb.SaveChanges();


                            }
                            else
                            {

                                advertisementFavorites.IsActivated = true;
                                advertisementFavorites.IsDeleted = false;
                                melkavanApiDb.AdvertisementFavorites.Add(advertisementFavorites);
                                melkavanApiDb.SaveChanges();

                            }

                        }
                        else if (advertisementFavoritesVM.Type.Equals("Properties"))
                        {

                            PropertiesFavorites propertyFavorites = new PropertiesFavorites()
                            {
                                PropertyId = advertisementFavoritesVM.AdvertisementId,
                                UserIdCreator = advertisementFavoritesVM.UserIdCreator,
                                CreateEnDate = DateTime.Now,
                                CreateTime = PersianDate.TimeNow,
                            };


                            if (teniacoApiDb.PropertiesFavorites.Where(c => c.PropertyId.Equals(propertyFavorites.PropertyId)).Any())
                            {
                                propertyFavorites = teniacoApiDb.PropertiesFavorites.Where(c => c.PropertyId.Equals(propertyFavorites.PropertyId)).FirstOrDefault();


                                teniacoApiDb.PropertiesFavorites.Remove(propertyFavorites);
                                teniacoApiDb.SaveChanges();


                            }
                            else
                            {

                                propertyFavorites.IsActivated = true;
                                propertyFavorites.IsDeleted = false;
                                teniacoApiDb.PropertiesFavorites.Add(propertyFavorites);
                                teniacoApiDb.SaveChanges();

                            }

                        }



                        return advertisementFavoritesVM.AdvertisementId;

                        //return advertisementFavorites.AdvertisementFavoriteId;
                    }

            }
            catch (Exception exc)
            { }

            return 0;
        }


        public bool CompleteDeleteAdvertisementFavorites(long advertisementId, long userId)
        {
            try
            {
                var advertisementFavorite = (from c in melkavanApiDb.AdvertisementFavorites
                                             where c.AdvertisementId == advertisementId &&
                                             c.UserIdCreator == userId
                                             select c).FirstOrDefault();

                if (advertisementFavorite != null)
                {
                    melkavanApiDb.AdvertisementFavorites.Remove(advertisementFavorite);
                    melkavanApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }
        #endregion

        #region Methods For Work With AdvertisementUserProfile


        #region old code
        //public bool UpdateUserProfile(
        //    PersonsVM personsVM,
        //    string? email,
        //    List<long> childsUsersIds,
        //    IPublicApiBusiness publicApiBusiness,
        //    IConsoleBusiness consoleBusiness)
        //{
        //    long personsId = personsVM.PersonId;
        //    bool? isActivated = personsVM.IsActivated.HasValue ? personsVM.IsActivated.Value : (bool?)true;
        //    bool? isDeleted = personsVM.IsDeleted.HasValue ? personsVM.IsDeleted.Value : (bool?)true;
        //    if (publicApiBusiness.PublicApiDb.Persons.Where(n => childsUsersIds.Contains(n.UserIdCreator.Value)).Where(x => x.PersonId.Equals(personsId)).Any())
        //    {
        //        using (var transaction = publicApiBusiness.PublicApiDb.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                Persons persons = (from a in publicApiBusiness.PublicApiDb.Persons
        //                                   where a.PersonId == personsId
        //                                   select a).FirstOrDefault();
        //                persons.StateId = personsVM.StateId;
        //                persons.CityId = personsVM.CityId;
        //                persons.Name = personsVM.Name;
        //                persons.Family = personsVM.Family;
        //                persons.PersonTypeId = personsVM.PersonTypeId;
        //                persons.EditEnDate = personsVM.EditEnDate;
        //                persons.UserIdEditor = personsVM.UserIdEditor;
        //                persons.EditTime = personsVM.EditTime;
        //                publicApiBusiness.PublicApiDb.Entry<Persons>(persons).State = EntityState.Modified;
        //                publicApiBusiness.PublicApiDb.SaveChanges();
        //                if (consoleBusiness.CmsDb.Users.Where(u => u.UserId.Equals(persons.UserIdCreator)).Any())
        //                {
        //                    var user = consoleBusiness.CmsDb.Users.Where(u => u.UserId.Equals(persons.UserIdCreator)).FirstOrDefault();
        //                    user.Email = email;
        //                    user.EditEnDate = DateTime.Now;
        //                    user.EditTime = PersianDate.TimeNow;
        //                    user.UserIdEditor = personsVM.UserIdEditor;
        //                    consoleBusiness.CmsDb.Entry<Users>(user).State = EntityState.Modified;
        //                    consoleBusiness.CmsDb.SaveChanges();
        //                }
        //                if (consoleBusiness.CmsDb.UsersProfile.Where(u => u.UserId.Equals(persons.UserIdCreator)).Any())
        //                {
        //                    var userProfile = consoleBusiness.CmsDb.UsersProfile.Where(u => u.UserId.Equals(persons.UserIdCreator)).FirstOrDefault();
        //                    userProfile.Email = email;
        //                    userProfile.Name = persons.Name;
        //                    userProfile.Family = persons.Family;
        //                    userProfile.Phone = persons.Phone;
        //                    userProfile.Mobile = persons.Mobail;
        //                    userProfile.EditEnDate = DateTime.Now;
        //                    userProfile.EditTime = PersianDate.TimeNow;
        //                    userProfile.UserIdEditor = personsVM.UserIdEditor;
        //                    consoleBusiness.CmsDb.Entry<UsersProfile>(userProfile).State = EntityState.Modified;
        //                    consoleBusiness.CmsDb.SaveChanges();
        //                }
        //                transaction.Commit();
        //                return true;
        //            }
        //            catch (Exception ex)
        //            {
        //                transaction.Rollback();
        //            }
        //        }
        //    }
        //    return false;
        //}
        #endregion


        public bool UpdateUserProfile(
            long userId,
            long? stateId,
            long? cityId,
            string? name,
            string? family,
            string? email,
            int? personTypeId,
            List<long> childsUsersIds,
            IConsoleBusiness consoleBusiness,
            IPublicApiBusiness publicApiBusiness)
        {


            if (consoleBusiness.CmsDb.Users.Where(f => f.UserId.Equals(userId)).Any())
            {
                using (var transaction = consoleBusiness.CmsDb.Database.BeginTransaction())
                {
                    try
                    {

                        #region User

                        var user = consoleBusiness.CmsDb.Users.Where(u => u.UserId == userId).FirstOrDefault();



                        user.Email = email != null || email != "" ? email : "";


                        user.EditEnDate = DateTime.Now;
                        user.UserIdEditor = userId;
                        user.EditTime = PersianDate.TimeNow;
                        user.IsActivated = true;
                        user.IsDeleted = false;


                        consoleBusiness.CmsDb.Entry<Users>(user).State = EntityState.Modified;
                        consoleBusiness.CmsDb.SaveChanges();
                        #endregion

                        if (consoleBusiness.CmsDb.UsersProfile.Where(f => f.UserId.Equals(userId)).Any())
                        {
                            #region update UserProfile

                            var userProfile = consoleBusiness.CmsDb.UsersProfile.Where(f => f.UserId.Equals(userId)).FirstOrDefault();

                            userProfile.StateId = stateId != null || stateId > 0 ? stateId : 0;
                            userProfile.CityId = cityId != null || cityId > 0 ? cityId : 0;
                            userProfile.Name = name != null || name != "" ? name : "";
                            userProfile.Family = family != null || family != "" ? family : "";
                            userProfile.Email = email != null || email != "" ? email : "";

                            userProfile.EditEnDate = DateTime.Now;
                            userProfile.UserIdEditor = userId;
                            userProfile.EditTime = PersianDate.TimeNow;
                            userProfile.IsActivated = true;
                            userProfile.IsDeleted = false;


                            consoleBusiness.CmsDb.Entry<console.UsersProfile>(userProfile).State = EntityState.Modified;
                            consoleBusiness.CmsDb.SaveChanges();

                            #endregion
                        }
                        else
                        {
                            #region add UserProfile

                            console.UsersProfile usersProfile = new console.UsersProfile()
                            {
                                StateId = stateId != null || stateId > 0 ? stateId : 0,
                                CityId = cityId != null || cityId > 0 ? cityId : 0,
                                Name = name != null || name != "" ? name : "",
                                Family = family != null || family != "" ? family : "",
                                Email = email != null || email != "" ? email : "",
                                EditEnDate = DateTime.Now,
                                UserIdEditor = userId,
                                EditTime = PersianDate.TimeNow,
                                IsActivated = true,
                                IsDeleted = false,
                            };

                            consoleBusiness.CmsDb.UsersProfile.Add(usersProfile);
                            consoleBusiness.CmsDb.SaveChanges();

                            #endregion
                        }

                        #region change level

                        if (personTypeId.HasValue)
                            if (personTypeId.Value > 0)
                            {
                                switch (personTypeId.Value)
                                {
                                    //اگر کاربر مالک انتخاب کرد دسترسی مشاور حذف شود و دسترسی مالک اضافه شود
                                    case 1:
                                        var consultantUsersLevels = (from l in consoleBusiness.CmsDb.Levels
                                                                     join ul in consoleBusiness.CmsDb.UsersLevels on l.LevelId equals ul.LevelId
                                                                     where l.LevelName.Equals("مشاور")
                                                                     select ul).FirstOrDefault();
                                        if (consultantUsersLevels != null)
                                        {
                                            //consoleBusiness.CmsDb.Entry<UsersLevels>(usersLevels).State = EntityState.Modified;
                                            consoleBusiness.CmsDb.UsersLevels.Remove(consultantUsersLevels);
                                            consoleBusiness.CmsDb.SaveChanges();
                                        }

                                        var ownerLevelId = (from l in consoleBusiness.CmsDb.Levels
                                                            where l.LevelName.Equals("مالک")
                                                            select l).FirstOrDefault().LevelId;

                                        consoleBusiness.CmsDb.UsersLevels.Add(new UsersLevels()
                                        {
                                            LevelId = ownerLevelId,
                                            CreateEnDate = DateTime.Now,
                                            CreateTime = PersianDate.TimeNow,
                                            IsActivated = true,
                                            IsDeleted = false,
                                            UserId = userId,
                                            UserIdCreator = userId,
                                        });

                                        consoleBusiness.CmsDb.SaveChanges();
                                        break;
                                    //اگر کاربر مشاور انتخاب کرد دسترسی مالک حذف شود و دسترسی مشاور اضافه شود
                                    case 2:
                                        var ownerUsersLevels = (from l in consoleBusiness.CmsDb.Levels
                                                                join ul in consoleBusiness.CmsDb.UsersLevels on l.LevelId equals ul.LevelId
                                                                where l.LevelName.Equals("مالک")
                                                                select ul).FirstOrDefault();
                                        if (ownerUsersLevels != null)
                                        {
                                            //consoleBusiness.CmsDb.Entry<UsersLevels>(usersLevels).State = EntityState.Modified;
                                            consoleBusiness.CmsDb.UsersLevels.Remove(ownerUsersLevels);
                                            consoleBusiness.CmsDb.SaveChanges();
                                        }

                                        var consultantLevelId = (from l in consoleBusiness.CmsDb.Levels
                                                                 where l.LevelName.Equals("مشاور")
                                                                 select l).FirstOrDefault().LevelId;

                                        consoleBusiness.CmsDb.UsersLevels.Add(new UsersLevels()
                                        {
                                            LevelId = consultantLevelId,
                                            CreateEnDate = DateTime.Now,
                                            CreateTime = PersianDate.TimeNow,
                                            IsActivated = true,
                                            IsDeleted = false,
                                            UserId = userId,
                                            UserIdCreator = userId,
                                        });

                                        consoleBusiness.CmsDb.SaveChanges();
                                        break;
                                }
                            }

                        #endregion


                        #region comment Person

                        //if (publicApiBusiness.PublicApiDb.Persons.Where(f => f.Phone.Equals(user.UserName)).Any())
                        //{
                        //    var person = publicApiBusiness.PublicApiDb.Persons.Where(f => f.Phone.Equals(user.UserName)).FirstOrDefault();

                        //    person.Name = name != null || name != "" ? name : "";
                        //    person.Family = family != null || family != "" ? family : "";


                        //    person.EditEnDate = DateTime.Now;
                        //    person.UserIdEditor = userId;
                        //    person.EditTime = PersianDate.TimeNow;
                        //    person.IsActivated = true;
                        //    person.IsActivated = false;


                        //    publicApiBusiness.PublicApiDb.Entry<Persons>(person).State = EntityState.Modified;
                        //    publicApiBusiness.PublicApiDb.SaveChanges();
                        //}
                        #endregion

                        transaction.Commit();


                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                }
            }
            return false;
        }



        public MelkavanPropertiesUsersProfileVM GetAdvertiserProfileWithAdvertiserId(
                long advertiserId,
                List<long> childsUsersIds,
                IConsoleBusiness consoleBusiness,
                PublicApiContext publicApiDb,
                MelkavanApiContext melkavanApiDb)
        {
            MelkavanPropertiesUsersProfileVM melkavanPropertiesUsersProfileVM = new();
            UsersProfile usersProfile = consoleBusiness.CmsDb.UsersProfile.Where(u => u.UserId == advertiserId).FirstOrDefault();

            try
            {
                if (advertiserId > 0)
                {
                    melkavanPropertiesUsersProfileVM.UserId = advertiserId;
                    melkavanPropertiesUsersProfileVM.Name = usersProfile.Name; 
                    melkavanPropertiesUsersProfileVM.Family = usersProfile.Family;
                    melkavanPropertiesUsersProfileVM.Mobile = usersProfile.Mobile;
                    melkavanPropertiesUsersProfileVM.Email = usersProfile.Email;
                    melkavanPropertiesUsersProfileVM.StateName = publicApiDb.States.Where(s => s.StateId == usersProfile.StateId).Select(s => s.StateName).FirstOrDefault();
                    melkavanPropertiesUsersProfileVM.CityName = publicApiDb.Cities.Where(s => s.CityId == usersProfile.CityId).Select(s => s.CityName).FirstOrDefault();

                }
            }
            catch (Exception exc)
            {
            }

            return melkavanPropertiesUsersProfileVM;
        }



        #endregion

        #region Methods For Work With Banners

        public List<BannersVM> GetAllBannersList(string bannerTitle)
        {
            List<BannersVM> bannersVM = new List<BannersVM>();

            try
            {
                var list = (from b in melkavanApiDb.Banners
                            where b.IsActivated.Value.Equals(true) &&
                             b.IsDeleted.Value.Equals(false)
                            select new BannersVM
                            {
                                BannerId = b.BannerId,
                                BannerTitle = b.BannerTitle,
                                BannerFileExt = b.BannerFileExt,
                                BannerFileOrder = b.BannerFileOrder,
                                BannerFilePath = b.BannerFilePath,
                                BannerLink = b.BannerLink,
                                IsDeleted = b.IsDeleted,
                                IsActivated = b.IsActivated,
                                UserIdCreator = b.UserIdCreator,
                                UserIdEditor = b.UserIdEditor,
                                UserIdRemover = b.UserIdRemover,
                                CreateEnDate = b.CreateEnDate,
                                CreateTime = b.CreateTime,
                                EditEnDate = b.EditEnDate,
                                EditTime = b.EditTime,
                                RemoveTime = b.RemoveTime,
                                RemoveEnDate = b.RemoveEnDate,
                                UserCreatorName = b.UserCreatorName,

                            }).AsEnumerable();

                if (!string.IsNullOrEmpty(bannerTitle))
                {
                    list = list.Where(a => a.BannerTitle.Contains(bannerTitle));
                }
                bannersVM = list.OrderByDescending(s => s.BannerId).ToList();

                return bannersVM;

            }
            catch (Exception exc)
            { }
            return bannersVM;
        }

        public List<BannersVM> GetListOfBanners(int jtStartIndex,
            int jtPageSize,
            ref int listCount,
            string? jtSorting = null,
            string bannerTitle = "")
        {
            List<BannersVM> bannersVM = new List<BannersVM>();
            try
            {
                var list = (from b in melkavanApiDb.Banners
                            where b.IsActivated.Value.Equals(true) &&
                             b.IsDeleted.Value.Equals(false)
                            select new BannersVM
                            {
                                BannerId = b.BannerId,
                                BannerTitle = b.BannerTitle,
                                BannerFileExt = b.BannerFileExt,
                                BannerFileOrder = b.BannerFileOrder,
                                BannerFilePath = b.BannerFilePath,
                                BannerLink = b.BannerLink,
                                IsDeleted = b.IsDeleted,
                                IsActivated = b.IsActivated,
                                UserIdCreator = b.UserIdCreator,
                                UserIdEditor = b.UserIdEditor,
                                UserIdRemover = b.UserIdRemover,
                                CreateEnDate = b.CreateEnDate,
                                CreateTime = b.CreateTime,
                                EditEnDate = b.EditEnDate,
                                EditTime = b.EditTime,
                                RemoveTime = b.RemoveTime,
                                RemoveEnDate = b.RemoveEnDate,
                                UserCreatorName = b.UserCreatorName,

                            }).AsEnumerable();

                if (!string.IsNullOrEmpty(bannerTitle))
                {
                    list = list.Where(z => z.BannerTitle.Contains(bannerTitle));
                }

                try
                {
                    if (string.IsNullOrEmpty(jtSorting))
                    {
                        listCount = list.Count();

                        if (listCount > jtPageSize)
                        {

                            bannersVM = list.OrderByDescending(s => s.BannerId)
                                     .Skip(jtStartIndex).Take(jtPageSize).ToList();
                        }
                        else
                            bannersVM = list.OrderByDescending(s => s.BannerId).ToList();
                    }
                    else
                    {
                        listCount = list.Count();

                        if (listCount > jtPageSize)
                        {
                            switch (jtSorting)
                            {
                                case "BannerTitle ASC":
                                    list = list.OrderBy(l => l.BannerTitle);
                                    break;
                                case "BannerTitle DESC":
                                    list = list.OrderByDescending(l => l.BannerTitle);
                                    break;
                            }


                            if (string.IsNullOrEmpty(jtSorting))
                                bannersVM = list.OrderByDescending(s => s.BannerId)
                                         .Skip(jtStartIndex).Take(jtPageSize).ToList();
                            else
                                bannersVM = list.Skip(jtStartIndex).Take(jtPageSize).ToList();
                        }
                        else
                        {

                            bannersVM = list.ToList();
                        }
                    }


                }
                catch (Exception exc)
                { }




                return bannersVM;

            }
            catch (Exception exc)
            { }
            return bannersVM;
        }

        public int AddToBanners(BannersVM bannersVM)
        {
            try
            {

                Banners banners = _mapper.Map<BannersVM, Banners>(bannersVM);


                melkavanApiDb.Banners.Add(banners);
                melkavanApiDb.SaveChanges();
                return banners.BannerId;
            }
            catch (Exception)
            {
            }
            return 0;
        }
        public bool UpdateBanners(BannersVM bannersVM, List<long> childsUsersIds)
        {
            var bannerId = bannersVM.BannerId;
            if (melkavanApiDb.Banners.Where(n => childsUsersIds.Contains(n.UserIdCreator.Value)).Where(x => x.BannerId.Equals(bannerId)).Any())
            {
                try
                {
                    Banners banner = melkavanApiDb.Banners.Where(a => a.BannerId == bannerId).FirstOrDefault();
                    banner.BannerLink = bannersVM.BannerLink;
                    banner.BannerTitle = bannersVM.BannerTitle;
                    banner.BannerFileExt = bannersVM.BannerFileExt;
                    banner.BannerFileOrder = bannersVM.BannerFileOrder;
                    banner.BannerFilePath = bannersVM.BannerFilePath;
                    banner.EditTime = bannersVM.EditTime;
                    banner.EditEnDate = bannersVM.EditEnDate;
                    banner.UserIdEditor = bannersVM.UserIdEditor;
                    banner.IsActivated = bannersVM.IsActivated;
                    banner.IsDeleted = bannersVM.IsDeleted;

                    melkavanApiDb.Entry<Banners>(banner).State = EntityState.Modified;

                    melkavanApiDb.SaveChanges();
                    return true;
                }
                catch (Exception)
                {

                }
            }
            return false;
        }

        public bool ToggleActivationBanners(int bannerId, long userId, List<long> childsUsersIds)
        {
            try
            {
                var banner = (from c in melkavanApiDb.Banners
                              where c.BannerId == bannerId &&
                              childsUsersIds.Contains(c.UserIdCreator.Value)
                              select c).FirstOrDefault();

                if (banner != null)
                {
                    banner.IsActivated = !banner.IsActivated;
                    banner.EditEnDate = DateTime.Now;
                    banner.EditTime = PersianDate.TimeNow;
                    banner.UserIdEditor = userId;

                    melkavanApiDb.Entry<Banners>(banner).State = EntityState.Modified;
                    melkavanApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool TemporaryDeleteBanners(int bannerId, long userId, List<long> childsUsersIds)
        {
            try
            {
                var banner = (from c in melkavanApiDb.Banners
                              where c.BannerId == bannerId &&
                              childsUsersIds.Contains(c.UserIdCreator.Value)
                              select c).FirstOrDefault();

                if (banner != null)
                {
                    banner.IsDeleted = !banner.IsDeleted;
                    banner.RemoveEnDate = DateTime.Now;
                    banner.RemoveTime = PersianDate.TimeNow;
                    banner.UserIdRemover = userId;

                    melkavanApiDb.Entry<Banners>(banner).State = EntityState.Modified;
                    melkavanApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool CompleteDeleteBanners(int bannerId, long userId, List<long> childsUsersIds)
        {
            try
            {
                var banner = (from c in melkavanApiDb.Banners
                              where c.BannerId == bannerId &&
                              childsUsersIds.Contains(c.UserIdCreator.Value)
                              select c).FirstOrDefault();

                if (banner != null)
                {
                    melkavanApiDb.Banners.Remove(banner);
                    melkavanApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public BannersVM GetBannersByBannerId(int bannerId, List<long> childsUsersIds)
        {
            BannersVM bannerVM = null;
            try
            {
                var banner = melkavanApiDb.Banners
                    .Where(a => a.BannerId.Equals(bannerId))
                    .Where(p => childsUsersIds.Contains(p.UserIdCreator.Value))
                    .FirstOrDefault();
                bannerVM = _mapper.Map<Banners, BannersVM>(banner);
            }
            catch (Exception)
            {
            }
            return bannerVM;
        }


        #endregion

        #region Methods For Work With BuildingLifes

        public List<BuildingLifesVM> GetAllBuildingLifesList(
            ref int listCount,
            List<long> childsUsersIds)
        {
            List<BuildingLifesVM> buildingLifesVMList = new List<BuildingLifesVM>();

            try
            {
                var list = (from p in melkavanApiDb.BuildingLifes
                            where p.IsActivated.Value.Equals(true) &&
                            p.IsDeleted.Value.Equals(false)
                            select new BuildingLifesVM
                            {
                                BuildingLifeId = p.BuildingLifeId,
                                BuildingLifeTitle = p.BuildingLifeTitle,
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
                                IsDeleted = p.IsDeleted
                            })
                            .AsEnumerable();

                buildingLifesVMList = list.OrderByDescending(s => s.BuildingLifeId).ToList();

            }
            catch (Exception ex)
            { }

            return buildingLifesVMList;
        }

        #endregion

        #region Methods For Work With Tags

        public List<TagsVM> GetAllTagsList(
         ref int listCount,
         List<long> childsUsersIds)
        {
            List<TagsVM> tagsVMList = new List<TagsVM>();

            try
            {
                var list = (from t in melkavanApiDb.Tags
                            where t.IsActivated.Value.Equals(true) &&
                            t.IsDeleted.Value.Equals(false)
                            select new TagsVM
                            {
                                TagId = t.TagId,
                                TagTitle = t.TagTitle,
                                UserIdCreator = t.UserIdCreator.Value,
                                CreateEnDate = t.CreateEnDate,
                                CreateTime = t.CreateTime,
                                EditEnDate = t.EditEnDate,
                                EditTime = t.EditTime,
                                UserIdEditor = t.UserIdEditor.Value,
                                RemoveEnDate = t.RemoveEnDate,
                                RemoveTime = t.EditTime,
                                UserIdRemover = t.UserIdRemover.Value,
                                IsActivated = t.IsActivated,
                                IsDeleted = t.IsDeleted
                            })
                            .AsEnumerable();

                tagsVMList = list.OrderByDescending(s => s.TagId).ToList();

            }
            catch (Exception ex)
            { }

            return tagsVMList;
        }

        #endregion

        #region Methods For Work With Smses

        public List<SmsesVM> GetListOfSmses(
            List<long> childsUsersIds,
            int jtStartIndex,
            int jtPageSize,
            ref int listCount,
            string searchText)
        {
            List<SmsesVM> SmsesVMList = new List<SmsesVM>();

            var list = melkavanApiDb.Smses
                    .Where(c => childsUsersIds.Contains(c.UserIdCreator.Value) && c.IsDeleted.Value.Equals(false) && c.IsActivated.Value.Equals(true))
                    .AsQueryable();

            //if (!string.IsNullOrEmpty(Lang))
            //{
            //    list = list.Where(x => x.Lang.Equals(Lang));
            //}

            //if (!string.IsNullOrEmpty(searchText))
            //    list = list.Where(a => a.SmsSenderId.Contains(searchText) || a.Family.Contains(searchText) || a.Phone.Contains(searchText));

            try
            {
                listCount = list.Count();

                if (listCount > jtPageSize)
                {
                    SmsesVMList = _mapper.Map<List<Smses>, List<SmsesVM>>(list.OrderByDescending(s => s.SmsId)
                             .Skip(jtStartIndex).Take(jtPageSize).ToList());

                }
                else
                {
                    SmsesVMList = _mapper.Map<List<Smses>,
                        List<SmsesVM>>(list.OrderByDescending(s => s.SmsId).ToList());
                }
            }
            catch (Exception exc)
            { }
            return SmsesVMList;
        }

        public List<SmsesVM> GetAllSmsesList(
            List<long> childsUsersIds,
            ref int listCount,
            string searchText)
        {
            List<SmsesVM> smsesVM = new List<SmsesVM>();

            try
            {
                var list = melkavanApiDb.Smses.Where(p => childsUsersIds.Contains(p.UserIdCreator.Value)).
                    Where(f => f.IsActivated.Equals(true) &&
                    f.IsDeleted.Equals(false)).AsQueryable();

                //if (!string.IsNullOrEmpty(searchText))
                //    list = list.Where(a => a.Name.Contains(searchText) || a.Family.Contains(searchText) || a.Phone.Contains(searchText));

                listCount = list.Count();

                smsesVM = _mapper.Map<List<Smses>, List<SmsesVM>>(list.OrderByDescending(f => f.SmsId).ToList());
            }
            catch (Exception exc)
            { }

            return smsesVM;
        }

        public long AddToSmses(
            SmsesVM smsesVM,
            List<long> childsUsersIds,
            IPublicApiBusiness publicApiBusiness,
            IConsoleBusiness consoleBusiness)
        {
            var smsSender = publicApiBusiness.PublicApiDb.SmsSenders.Where(c => c.SmsSenderTitle.Contains("سفیر آنلاین")).ToList();
            smsesVM.SmsSenderId = smsSender[0].SmsSenderId;

            try
            {
                if (smsesVM != null)
                {
                    Smses smses = _mapper.Map<SmsesVM, Smses>(smsesVM);
                    melkavanApiDb.Smses.Add(smses);
                    melkavanApiDb.SaveChanges();
                    return smses.SmsId;
                }
                else
                    return -1;
                //}
            }
            catch (Exception exc)
            {

                var msg = exc.Message;
                var msg2 = exc.Data.Values;
            }
            return 0;
        }

        public long UpdateSmses(ref SmsesVM smsesVM,
            //long userId,
            List<long> childsUsersIds)
        {
            try
            {
                long smsId = smsesVM.SmsId;

                if (melkavanApiDb.Smses.Where(p =>
                    childsUsersIds.Contains(p.UserIdCreator.Value) &&
                    !p.SmsId.Equals(smsId)).Any())
                {
                    return -1;
                }

                Smses sms = (from c in melkavanApiDb.Smses
                             where c.SmsId == smsId
                             select c).FirstOrDefault();

                sms.EditEnDate = DateTime.Now;
                sms.EditTime = PersianDate.TimeNow;
                sms.UserIdEditor = smsesVM.UserIdEditor;

                sms.IsActivated = smsesVM.IsActivated;
                sms.IsDeleted = smsesVM.IsDeleted;
                melkavanApiDb.Entry<Smses>(sms).State = EntityState.Modified;
                melkavanApiDb.SaveChanges();

                smsesVM.UserIdCreator = sms.UserIdCreator.Value;

                #region rewrite inside module

                //long userIdCreator = smsesVM.UserIdCreator.Value;
                //if (smsesVM.UserIdCreator.HasValue)
                //{
                //    var user = publicApiDb.Users.FirstOrDefault(u => u.UserId.Equals(userIdCreator));
                //    var userDetails = publicApiDb.UsersProfile.FirstOrDefault(up => up.UserId.Equals(userIdCreator));
                //    smsesVM.UserCreatorName = user.UserName;

                //    if (!string.IsNullOrEmpty(userDetails.Name))
                //        smsesVM.UserCreatorName += " - " + userDetails.Name;

                //    if (!string.IsNullOrEmpty(userDetails.Family))
                //        smsesVM.UserCreatorName += " - " + userDetails.Family;
                //}

                #endregion

                return sms.SmsId;
                //}
            }
            catch (Exception exc)
            { }
            return 0;
        }

        public bool ToggleActivationSmses(long smsId, long userId, List<long> childsUsersIds)
        {
            try
            {
                var sms = (from c in melkavanApiDb.Smses
                           where c.SmsId == smsId &&
                           childsUsersIds.Contains(c.UserIdCreator.Value)
                           select c).FirstOrDefault();

                if (sms != null)
                {
                    sms.IsActivated = !sms.IsActivated;
                    sms.EditEnDate = DateTime.Now;
                    sms.EditTime = PersianDate.TimeNow;
                    sms.UserIdEditor = userId;
                    melkavanApiDb.Entry<Smses>(sms).State = EntityState.Modified;
                    melkavanApiDb.SaveChanges();
                    return true;
                }
            }
            catch (Exception exc)
            { }
            return false;
        }

        public bool TemporaryDeleteSmses(long smsId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var sms = (from c in melkavanApiDb.Smses
                           where c.SmsId == smsId &&
                           childsUsersIds.Contains(c.UserIdCreator.Value)
                           select c).FirstOrDefault();

                if (sms != null)
                {
                    sms.IsDeleted = !sms.IsDeleted;
                    sms.RemoveEnDate = DateTime.Now;
                    sms.RemoveTime = PersianDate.TimeNow;
                    sms.UserIdRemover = userId;
                    melkavanApiDb.Entry<Smses>(sms).State = EntityState.Modified;
                    melkavanApiDb.SaveChanges();
                    return true;
                }
            }
            catch (Exception exc)
            { }
            return false;
        }

        public bool CompleteDeleteSmses(long smsId,
            //long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var sms = (from c in melkavanApiDb.Smses
                           where c.SmsId == smsId &&
                           childsUsersIds.Contains(c.UserIdCreator.Value)
                           select c).FirstOrDefault();

                if (sms != null)
                {
                    melkavanApiDb.Smses.Remove(sms);
                    melkavanApiDb.SaveChanges();
                    return true;
                }
            }
            catch (Exception exc)
            { }
            return false;
        }



        #endregion

        #region Methods For Work With MelkavanLogin

        public long MelkavanLogin(
            ref MelkavanLoginVM melkavanLoginVM,
            IConsoleBusiness consoleBusiness,
            IPublicApiBusiness publicApiDb)
        {
            #region Add User


            var userName = melkavanLoginVM.UserName;
            long userId = 0;


            if (userName != null)
            {
                if (!consoleBusiness.CmsDb.Users.Where(u => u.UserName.Equals(userName)).Any())   //اگر این کاربر در ملکاوان وجود نداشته باشد
                {

                    using (var transaction = consoleBusiness.CmsDb.Database.BeginTransaction())
                    {
                        try
                        {
                            var domainSettingId = consoleBusiness.CmsDb.DomainsSettings.Where(c => c.DomainName.Equals("melkavan.com")).Select(c => c.DomainSettingId).FirstOrDefault();
                            var parentUserId = consoleBusiness.CmsDb.Users.Where(c => c.UserName.Equals("اعضای ملکاوان")).Select(c => c.UserId).FirstOrDefault();

                            #region Add User

                            Users users = new Users();
                            users.UserName = userName;
                            users.Password = FrameWork.MD5Hash.GetMD5Hash(melkavanLoginVM.Password); ;
                            //users.DomainSettingId = melkavanLoginVM.DomainSettingId;
                            users.DomainSettingId = domainSettingId;
                            users.Email = "";
                            //users.ParentUserId = melkavanLoginVM.ParentUserId;
                            users.ParentUserId = parentUserId;
                            users.RegisterEnDate = DateTime.Now;
                            users.RegisterTime = PersianDate.TimeNow;


                            //users.UserIdCreator = melkavanLoginVM.UserIdCreator; //this.userId.value
                            users.IsActivated = true;
                            users.IsDeleted = false;
                            users.CreateEnDate = DateTime.Now;
                            users.CreateTime = PersianDate.TimeNow;

                            consoleBusiness.CmsDb.Users.Add(users);
                            consoleBusiness.CmsDb.SaveChanges();

                            userId = users.UserId;


                            users.UserIdCreator = userId;
                            consoleBusiness.CmsDb.SaveChanges();

                            #endregion

                            #region Add UserProfile

                            console.UsersProfile usersProfile = new console.UsersProfile()
                            {
                                UserId = userId,
                                Address = "",
                                Age = 0,
                                BirthDateTimeEn = null,
                                CertificateId = "" +
                                "",
                                CreateEnDate = DateTime.Now,
                                CreateTime = PersianDate.TimeNow,
                                CreditCardNumber = "",
                                Email = "",
                                Name = "",
                                Family = "",
                                Mobile = userName,
                                Phone = userName,
                                HasModified = false,
                                IsActivated = true,
                                IsDeleted = false,
                                NationalCode = "",
                                Picture = "",
                                PostalCode = "",
                                Sexuality = false,
                                SocialNetworkAddress = "",
                                UniqueKey = "",
                                UserIdCreator = userId
                            };

                            consoleBusiness.CmsDb.UsersProfile.Add(usersProfile);
                            consoleBusiness.CmsDb.SaveChanges();

                            #endregion

                            #region comment

                            //var person = publicApiDb.PublicApiDb.Persons.Where(p => p.Phone.Equals(userName)).FirstOrDefault();

                            //if (person != null)
                            //{

                            //    //شخصی با همین شماره موبایل وجود دارد

                            //    #region Add UserProfile

                            //    UsersProfile usersProfile = new UsersProfile()
                            //    {
                            //        UserId = userId,
                            //        Address = "",
                            //        Age = 0,
                            //        BirthDateTimeEn = null,
                            //        CertificateId = "" +
                            //        "",
                            //        CreateEnDate = DateTime.Now,
                            //        CreateTime = PersianDate.TimeNow,
                            //        CreditCardNumber = "",
                            //        Email = "",
                            //        Name = person.Name != "" ? person.Name : "",
                            //        Family = person.Family != "" ? person.Family : "",
                            //        Mobile = person.Phone != "" ? person.Phone : "",
                            //        Phone = person.Phone != "" ? person.Phone : "",
                            //        HasModified = false,
                            //        IsActivated = true,
                            //        IsDeleted = false,
                            //        NationalCode = "",
                            //        Picture = "",
                            //        PostalCode = "",
                            //        Sexuality = false,
                            //        SocialNetworkAddress = "",
                            //        UniqueKey = "",
                            //        UserIdCreator = userId
                            //    };

                            //    consoleBusiness.CmsDb.UsersProfile.Add(usersProfile);
                            //    consoleBusiness.CmsDb.SaveChanges();


                            //    #endregion
                            //}
                            //else
                            //{
                            //    //شخصی با همین شماره موبایل وجود ندارد

                            //    #region Add UserProfile

                            //    UsersProfile usersProfile = new UsersProfile()
                            //    {
                            //        UserId = userId,
                            //        Address = "",
                            //        Age = 0,
                            //        BirthDateTimeEn = null,
                            //        CertificateId = "" +
                            //        "",
                            //        CreateEnDate = DateTime.Now,
                            //        CreateTime = PersianDate.TimeNow,
                            //        CreditCardNumber = "",
                            //        Email = "",
                            //        Name = userName,
                            //        Family = userName,
                            //        Mobile = userName,
                            //        Phone = userName,
                            //        HasModified = false,
                            //        IsActivated = true,
                            //        IsDeleted = false,
                            //        NationalCode = "",
                            //        Picture = "",
                            //        PostalCode = "",
                            //        Sexuality = false,
                            //        SocialNetworkAddress = "",
                            //        UniqueKey = "",
                            //        UserIdCreator = userId,
                            //    };

                            //    consoleBusiness.CmsDb.UsersProfile.Add(usersProfile);
                            //    consoleBusiness.CmsDb.SaveChanges();
                            //    #endregion

                            //}

                            #endregion

                            #region Add UserLEvels

                            int levelId = 0;

                            //levelId = consoleBusiness.GetLevelId("آگهی دهنده");
                            levelId = consoleBusiness.GetLevelId("مالک");


                            UsersLevels usersLevels = new UsersLevels()
                            {
                                LevelId = levelId,
                                UserId = userId,
                                CreateEnDate = DateTime.Now,
                                CreateTime = PersianDate.TimeNow,
                                UserIdCreator = userId,
                                IsActivated = true,
                                IsDeleted = false,
                            };

                            consoleBusiness.CmsDb.UsersLevels.Add(usersLevels);
                            consoleBusiness.CmsDb.SaveChanges();

                            #endregion

                            #region Add UserRoles

                            var roleId = consoleBusiness.CmsDb.Roles.Where(r => r.RoleName.Equals("Users")).FirstOrDefault().RoleId;

                            UsersRoles usersRoles = new UsersRoles()
                            {
                                RoleId = roleId,
                                UserId = userId,
                                CreateEnDate = DateTime.Now,
                                CreateTime = PersianDate.TimeNow,
                                UserIdCreator = userId,
                                IsActivated = true,
                                IsDeleted = false,
                            };

                            consoleBusiness.CmsDb.UsersRoles.Add(usersRoles);
                            consoleBusiness.CmsDb.SaveChanges();

                            #endregion

                            transaction.Commit();

                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                        }
                    }

                    #region comments - old codes
                    //if (!consoleBusiness.CmsDb.UsersProfile.Where(f => f.Mobile.Equals(userName) || f.Phone.Equals(userName)).Any()) //اگر این کاربر در سیستم وجود نداشته باشد
                    //{
                    //    using (var transaction = consoleBusiness.CmsDb.Database.BeginTransaction())
                    //    {
                    //        try
                    //        {


                    //            #region Add User

                    //            Users users = new Users();
                    //            users.UserName = userName;
                    //            users.Password = FrameWork.MD5Hash.GetMD5Hash(melkavanLoginVM.Password); ;
                    //            users.DomainSettingId = melkavanLoginVM.DomainSettingId;
                    //            users.Email = "";
                    //            users.ParentUserId = melkavanLoginVM.ParentUserId;
                    //            users.RegisterEnDate = DateTime.Now;
                    //            users.RegisterTime = PersianDate.TimeNow;


                    //            //users.UserIdCreator = melkavanLoginVM.UserIdCreator; //this.userId.value
                    //            users.IsActivated = true;
                    //            users.IsDeleted = false;
                    //            users.CreateEnDate = DateTime.Now;
                    //            users.CreateTime = PersianDate.TimeNow;

                    //            consoleBusiness.CmsDb.Users.Add(users);
                    //            consoleBusiness.CmsDb.SaveChanges();

                    //            userId = users.UserId;


                    //            users.UserIdCreator = userId;
                    //            consoleBusiness.CmsDb.SaveChanges();

                    //            #endregion

                    //            #region Add UserProfile

                    //            console.UsersProfile usersProfile = new console.UsersProfile()
                    //            {
                    //                UserId = userId,
                    //                Address = "",
                    //                Age = 0,
                    //                BirthDateTimeEn = null,
                    //                CertificateId = "" +
                    //                "",
                    //                CreateEnDate = DateTime.Now,
                    //                CreateTime = PersianDate.TimeNow,
                    //                CreditCardNumber = "",
                    //                Email = "",
                    //                Name = "",
                    //                Family = "",
                    //                Mobile = userName,
                    //                Phone = userName,
                    //                HasModified = false,
                    //                IsActivated = true,
                    //                IsDeleted = false,
                    //                NationalCode = "",
                    //                Picture = "",
                    //                PostalCode = "",
                    //                Sexuality = false,
                    //                SocialNetworkAddress = "",
                    //                UniqueKey = "",
                    //                UserIdCreator = userId
                    //            };

                    //            consoleBusiness.CmsDb.UsersProfile.Add(usersProfile);
                    //            consoleBusiness.CmsDb.SaveChanges();

                    //            #endregion

                    //            #region comment

                    //            //var person = publicApiDb.PublicApiDb.Persons.Where(p => p.Phone.Equals(userName)).FirstOrDefault();

                    //            //if (person != null)
                    //            //{

                    //            //    //شخصی با همین شماره موبایل وجود دارد

                    //            //    #region Add UserProfile

                    //            //    UsersProfile usersProfile = new UsersProfile()
                    //            //    {
                    //            //        UserId = userId,
                    //            //        Address = "",
                    //            //        Age = 0,
                    //            //        BirthDateTimeEn = null,
                    //            //        CertificateId = "" +
                    //            //        "",
                    //            //        CreateEnDate = DateTime.Now,
                    //            //        CreateTime = PersianDate.TimeNow,
                    //            //        CreditCardNumber = "",
                    //            //        Email = "",
                    //            //        Name = person.Name != "" ? person.Name : "",
                    //            //        Family = person.Family != "" ? person.Family : "",
                    //            //        Mobile = person.Phone != "" ? person.Phone : "",
                    //            //        Phone = person.Phone != "" ? person.Phone : "",
                    //            //        HasModified = false,
                    //            //        IsActivated = true,
                    //            //        IsDeleted = false,
                    //            //        NationalCode = "",
                    //            //        Picture = "",
                    //            //        PostalCode = "",
                    //            //        Sexuality = false,
                    //            //        SocialNetworkAddress = "",
                    //            //        UniqueKey = "",
                    //            //        UserIdCreator = userId
                    //            //    };

                    //            //    consoleBusiness.CmsDb.UsersProfile.Add(usersProfile);
                    //            //    consoleBusiness.CmsDb.SaveChanges();


                    //            //    #endregion
                    //            //}
                    //            //else
                    //            //{
                    //            //    //شخصی با همین شماره موبایل وجود ندارد

                    //            //    #region Add UserProfile

                    //            //    UsersProfile usersProfile = new UsersProfile()
                    //            //    {
                    //            //        UserId = userId,
                    //            //        Address = "",
                    //            //        Age = 0,
                    //            //        BirthDateTimeEn = null,
                    //            //        CertificateId = "" +
                    //            //        "",
                    //            //        CreateEnDate = DateTime.Now,
                    //            //        CreateTime = PersianDate.TimeNow,
                    //            //        CreditCardNumber = "",
                    //            //        Email = "",
                    //            //        Name = userName,
                    //            //        Family = userName,
                    //            //        Mobile = userName,
                    //            //        Phone = userName,
                    //            //        HasModified = false,
                    //            //        IsActivated = true,
                    //            //        IsDeleted = false,
                    //            //        NationalCode = "",
                    //            //        Picture = "",
                    //            //        PostalCode = "",
                    //            //        Sexuality = false,
                    //            //        SocialNetworkAddress = "",
                    //            //        UniqueKey = "",
                    //            //        UserIdCreator = userId,
                    //            //    };

                    //            //    consoleBusiness.CmsDb.UsersProfile.Add(usersProfile);
                    //            //    consoleBusiness.CmsDb.SaveChanges();
                    //            //    #endregion

                    //            //}

                    //            #endregion

                    //            #region Add UserLEvels

                    //            int levelId = 0;

                    //            //levelId = consoleBusiness.GetLevelId("آگهی دهنده");
                    //            levelId = consoleBusiness.GetLevelId("مالک");


                    //            UsersLevels usersLevels = new UsersLevels()
                    //            {
                    //                LevelId = levelId,
                    //                UserId = userId,
                    //                CreateEnDate = DateTime.Now,
                    //                CreateTime = PersianDate.TimeNow,
                    //                UserIdCreator = userId,
                    //                IsActivated = true,
                    //                IsDeleted = false,
                    //            };

                    //            consoleBusiness.CmsDb.UsersLevels.Add(usersLevels);
                    //            consoleBusiness.CmsDb.SaveChanges();

                    //            #endregion

                    //            #region Add UserRoles

                    //            var roleId = consoleBusiness.CmsDb.Roles.Where(r => r.RoleName.Equals("Users")).FirstOrDefault().RoleId;

                    //            UsersRoles usersRoles = new UsersRoles()
                    //            {
                    //                RoleId = roleId,
                    //                UserId = userId,
                    //                CreateEnDate = DateTime.Now,
                    //                CreateTime = PersianDate.TimeNow,
                    //                UserIdCreator = userId,
                    //                IsActivated = true,
                    //                IsDeleted = false,
                    //            };

                    //            consoleBusiness.CmsDb.UsersRoles.Add(usersRoles);
                    //            consoleBusiness.CmsDb.SaveChanges();

                    //            #endregion

                    //            transaction.Commit();

                    //        }
                    //        catch (Exception)
                    //        {
                    //            transaction.Rollback();
                    //        }
                    //    }
                    //}
                    //else
                    //{

                    //    var userProfile = consoleBusiness.CmsDb.UsersProfile.Where(f => f.Mobile.Equals(userName) || f.Phone.Equals(userName)).FirstOrDefault();
                    //    var user = consoleBusiness.CmsDb.Users.Where(f => f.UserId.Equals(userProfile.UserId)).FirstOrDefault();

                    //    if (user != null)
                    //    {
                    //        if (user.UserId > 0)
                    //        {
                    //            using (var transaction = consoleBusiness.CmsDb.Database.BeginTransaction())
                    //            {
                    //                try
                    //                {
                    //                    #region comment

                    //                    //var person = publicApiDb.PublicApiDb.Persons.Where(p => p.Phone.Equals(userName)).FirstOrDefault();

                    //                    //if (person != null)
                    //                    //{
                    //                    //    //اگر اسامی متفاوت بودند

                    //                    //    if ((person.Name != userProfile.Name) ||
                    //                    //        (person.Family != userProfile.Family))
                    //                    //    {
                    //                    //        #region Edit UserProfile

                    //                    //        userProfile.Name = person.Name != "" ? person.Name : "";
                    //                    //        userProfile.Mobile = person.Phone != "" ? person.Phone : ""; //هنگام تعریف شخص فیلد موبایل همیشه خالی است و فقط فیلد تلفن پر میشود
                    //                    //        userProfile.Phone = person.Phone != "" ? person.Phone : "";
                    //                    //        userProfile.Email = "";
                    //                    //        userProfile.IsActivated = true;
                    //                    //        userProfile.IsDeleted = false;

                    //                    //        userProfile.UserIdEditor = user.UserId;
                    //                    //        userProfile.EditEnDate = DateTime.Now;
                    //                    //        userProfile.EditTime = PersianDate.TimeNow;

                    //                    //        consoleBusiness.CmsDb.Entry<UsersProfile>(userProfile).State = EntityState.Modified;
                    //                    //        consoleBusiness.CmsDb.SaveChanges();

                    //                    //        #endregion
                    //                    //    }

                    //                    //}

                    //                    #endregion

                    //                    #region UserLevel

                    //                    var levelIds = consoleBusiness.GetMultiLevelsUserWithUserId(user.UserId).LevelIds;
                    //                    var levelNames = consoleBusiness.GetLevelsWithLevelIds(levelIds).Select(l => l.LevelName);

                    //                    //اگر دسترسی  مالک نداشته باشد
                    //                    //اگر نقش users نداشته باشد
                    //                    if (levelNames != null)
                    //                    {
                    //                        //اضافه کردن دسترسی مالک
                    //                        //if (!levelNames.Contains("آگهی دهنده"))
                    //                        if (!levelNames.Contains("مالک"))
                    //                        {
                    //                            //var levelId = consoleBusiness.GetLevelId("آگهی دهنده");
                    //                            var levelId = consoleBusiness.GetLevelId("مالک");

                    //                            UsersLevels usersLevels = new UsersLevels()
                    //                            {
                    //                                LevelId = levelId,
                    //                                UserId = user.UserId,
                    //                                CreateEnDate = DateTime.Now,
                    //                                CreateTime = PersianDate.TimeNow,
                    //                                UserIdCreator = user.UserId,
                    //                                IsActivated = true,
                    //                                IsDeleted = false,
                    //                            };

                    //                            consoleBusiness.CmsDb.UsersLevels.Add(usersLevels);
                    //                            consoleBusiness.CmsDb.SaveChanges();
                    //                        }
                    //                    }

                    //                    #endregion

                    //                    #region UserRole

                    //                    var roleIds = consoleBusiness.GetRoleIdsWithUserId(user.UserId);
                    //                    var roleNames = consoleBusiness.GetRolesWithRoleIds(roleIds).Select(r => r.RoleName);

                    //                    if (roleNames != null)
                    //                    {
                    //                        if (!roleNames.Contains("Users"))
                    //                        {
                    //                            //اضافه کردن نقش Users
                    //                            var roleId = consoleBusiness.CmsDb.Roles.Where(r => r.RoleName.Equals("Users")).FirstOrDefault().RoleId;

                    //                            UsersRoles usersRoles = new UsersRoles()
                    //                            {
                    //                                RoleId = roleId,
                    //                                UserId = user.UserId,
                    //                                CreateEnDate = DateTime.Now,
                    //                                CreateTime = PersianDate.TimeNow,
                    //                                UserIdCreator = user.UserId,
                    //                                IsActivated = true,
                    //                                IsDeleted = false,
                    //                            };


                    //                            consoleBusiness.CmsDb.UsersRoles.Add(usersRoles);
                    //                            consoleBusiness.CmsDb.SaveChanges();
                    //                        }
                    //                    }

                    //                    #endregion

                    //                    #region Edit the User

                    //                    user.IsActivated = true;
                    //                    user.IsDeleted = false;

                    //                    consoleBusiness.CmsDb.Entry<Users>(user).State = EntityState.Modified;
                    //                    consoleBusiness.CmsDb.SaveChanges();

                    //                    #endregion

                    //                    transaction.Commit();
                    //                }
                    //                catch (Exception exc)
                    //                {
                    //                    transaction.Rollback();
                    //                }
                    //            }
                    //        }
                    //    }
                    //    userId = user.UserId;
                    //}
                    #endregion





                }
                else
                {
                    //اگر این کاربر وجود داشته باشد  
                    var userProfile = consoleBusiness.CmsDb.UsersProfile.Where(f => f.Mobile.Equals(userName) || f.Phone.Equals(userName)).FirstOrDefault();

                    if (userProfile != null)
                    {
                        var user = consoleBusiness.CmsDb.Users.Where(u => u.UserId.Equals(userProfile.UserId)).FirstOrDefault();

                        if (user != null)
                        {
                            if (user.UserId > 0)
                            {
                                using (var transaction = consoleBusiness.CmsDb.Database.BeginTransaction())
                                {
                                    try
                                    {
                                        #region comment

                                        //var person = publicApiDb.PublicApiDb.Persons.Where(p => p.Phone.Equals(userName)).FirstOrDefault();

                                        //if (person != null)
                                        //{
                                        //    //اگر اسامی متفاوت بودند

                                        //    if ((person.Name != userProfile.Name) ||
                                        //        (person.Family != userProfile.Family))
                                        //    {
                                        //        #region Edit UserProfile

                                        //        userProfile.Name = person.Name != "" ? person.Name : "";
                                        //        userProfile.Mobile = person.Phone != "" ? person.Phone : ""; //هنگام تعریف شخص فیلد موبایل همیشه خالی است و فقط فیلد تلفن پر میشود
                                        //        userProfile.Phone = person.Phone != "" ? person.Phone : "";
                                        //        userProfile.Email = "";
                                        //        userProfile.IsActivated = true;
                                        //        userProfile.IsDeleted = false;

                                        //        userProfile.UserIdEditor = user.UserId;
                                        //        userProfile.EditEnDate = DateTime.Now;
                                        //        userProfile.EditTime = PersianDate.TimeNow;

                                        //        consoleBusiness.CmsDb.Entry<UsersProfile>(userProfile).State = EntityState.Modified;
                                        //        consoleBusiness.CmsDb.SaveChanges();

                                        //        #endregion
                                        //    }

                                        //}

                                        #endregion

                                        #region UserLevel

                                        var levelIds = consoleBusiness.GetMultiLevelsUserWithUserId(user.UserId).LevelIds;
                                        var levelNames = consoleBusiness.GetLevelsWithLevelIds(levelIds).Select(l => l.LevelName);

                                        //اگر دسترسی  مالک نداشته باشد
                                        //اگر نقش users نداشته باشد
                                        if (levelNames != null)
                                        {
                                            //اضافه کردن دسترسی مالک
                                            if (!levelNames.Contains("مالک")/* || !levelNames.Contains("مشاور")*/)
                                            {
                                                if (!levelNames.Contains("مشاور"))
                                                {
                                                    var levelId = consoleBusiness.GetLevelId("مالک");

                                                    UsersLevels usersLevels = new UsersLevels()
                                                    {
                                                        LevelId = levelId,
                                                        UserId = user.UserId,
                                                        CreateEnDate = DateTime.Now,
                                                        CreateTime = PersianDate.TimeNow,
                                                        UserIdCreator = user.UserId,
                                                        IsActivated = true,
                                                        IsDeleted = false,
                                                    };


                                                    consoleBusiness.CmsDb.UsersLevels.Add(usersLevels);
                                                    consoleBusiness.CmsDb.SaveChanges();
                                                }
                                            }
                                        }

                                        #endregion

                                        #region UserRole


                                        var roleIds = consoleBusiness.GetRoleIdsWithUserId(user.UserId);
                                        var roleNames = consoleBusiness.GetRolesWithRoleIds(roleIds).Select(r => r.RoleName);

                                        if (roleNames != null)
                                        {
                                            if (!roleNames.Contains("Users"))
                                            {
                                                //اضافه کردن نقش Users
                                                var roleId = consoleBusiness.CmsDb.Roles.Where(r => r.RoleName.Equals("Users")).FirstOrDefault().RoleId;

                                                UsersRoles usersRoles = new UsersRoles()
                                                {
                                                    RoleId = roleId,
                                                    UserId = user.UserId,
                                                    CreateEnDate = DateTime.Now,
                                                    CreateTime = PersianDate.TimeNow,
                                                    UserIdCreator = user.UserId,
                                                    IsActivated = true,
                                                    IsDeleted = false,
                                                };


                                                consoleBusiness.CmsDb.UsersRoles.Add(usersRoles);
                                                consoleBusiness.CmsDb.SaveChanges();
                                            }
                                        }
                                        #endregion

                                        transaction.Commit();
                                    }
                                    catch (Exception xc)
                                    {
                                        transaction.Rollback();
                                    }
                                }
                            }
                        }
                        userId = user.UserId;


                    }
                }

                return userId;
            }
            #endregion

            return 0;
        }


        #endregion

        #region Methods For Work With AboutUs
        public AboutUsVM AboutUs(
            IConsoleBusiness consoleBusiness,
            IPublicApiBusiness publicApiDb,
            ITeniacoApiBusiness tenicoApiDb)
        {

            AboutUsVM aboutUsVM = new AboutUsVM();
            var countOfAgencies = tenicoApiDb.TeniacoApiDb.Agencies.Count();
            var countOfAgencyStaffs = tenicoApiDb.TeniacoApiDb.AgencyStaffs.Count();
            var countOfOwnersInpersons = publicApiDb.PublicApiDb.Persons.Where(c => c.PersonTypeId.Equals(2)).Count();
            var countOfOwnersInUsers = consoleBusiness.CmsDb.UsersLevels.Where(c => c.LevelId.Equals(20)).Count();
            var countOfProperties = tenicoApiDb.TeniacoApiDb.Properties.Where(c => c.ShowInMelkavan.Equals(true)).Count();
            var countofAdvertisements = melkavanApiDb.Advertisement.Count();

            aboutUsVM.countOfAgencies = countOfAgencies;
            aboutUsVM.countOfAgencyStaffs = countOfAgencyStaffs;
            aboutUsVM.countOfAdvertisements = countOfProperties + countofAdvertisements;
            aboutUsVM.countOfOwners = countOfOwnersInpersons + countOfOwnersInUsers;

            return aboutUsVM;
        }

        #endregion

        #endregion
    }

}
