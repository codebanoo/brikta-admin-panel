using APIs.Public.Models.Entities;
using AutoMapper;
using FrameWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Models.Business.ConsoleBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using VM.Public;



namespace APIs.Public.Models.Business
{
    public class PublicApiBusiness : IPublicApiBusiness, IDisposable
    {
        private PublicApiContext publicApiDb = new PublicApiContext();

        private IMapper _mapper;

        private IHostEnvironment hostingEnvironment;

        public PublicApiContext PublicApiDb
        {
            get { return this.publicApiDb; }
            set { }
            //private set { }
        }

        public void Dispose()
        {
            publicApiDb.Dispose();
        }

        public PublicApiBusiness(IMapper mapper,
            PublicApiContext _publicApiDb,
            IHostEnvironment _hostingEnvironment)
        {
            try
            {
                _mapper = mapper;

                publicApiDb = _publicApiDb;

                PublicApiDb = publicApiDb;

                hostingEnvironment = _hostingEnvironment;
            }
            catch (Exception exc)
            {
            }
        }

        #region Public

        #region Methods For Work With Countries

        public int AddToCountries(string countryName, string countryLatinName, string countryAbbreviationName, string countryCode, string countryFlagPath)
        {
            try
            {
                if (!publicApiDb.Countries.Any(c => c.CountryLatinName.Equals(countryLatinName)))
                {
                    Countries country = new Countries()
                    {
                        CountryAbbreviationName = countryAbbreviationName,
                        CountryCode = countryCode,
                        CountryLatinName = countryLatinName,
                        CountryName = countryName,
                        CountryFlagPath = countryFlagPath
                    };

                    publicApiDb.Countries.Add(country);
                    publicApiDb.SaveChanges();
                    return country.CountryId;
                }
            }
            catch (Exception exc)
            { }
            return 0;
        }

        public List<CountriesVM> GetListOfCountries()
        {
            try
            {
                var countries = publicApiDb.Countries.Where(c => c.IsActivated.Value.Equals(true) && c.IsDeleted.Value.Equals(false)).AsQueryable();

                if (countries.Any())
                    return _mapper.Map<List<Countries>, List<CountriesVM>>(countries.ToList());
            }
            catch (Exception exc)
            { }
            return new List<CountriesVM>();
        }

        #endregion

        #region Methods For Work With ConstructionItems

        public List<ConstructionItemsVM> GetAllConstructionItemsList(ref int listCount,
            long? ConstructionItemParentId,
            string ConstructionItemTitle,
            DateTime? dateTimeTo)
        {
            List<ConstructionItemsVM> constructionItemsVMList = new List<ConstructionItemsVM>();

            try
            {
                var list = publicApiDb.ConstructionItems.Where(f => f.IsActivated.Value.Equals(true) && f.IsDeleted.Value.Equals(false)).AsQueryable();

                if (ConstructionItemParentId.HasValue)
                    if (ConstructionItemParentId.Value > 0)
                        list = list.Where(c => c.ConstructionItemParentId.HasValue).Where(c => c.ConstructionItemParentId.Value.Equals(ConstructionItemParentId.Value));

                if (!string.IsNullOrEmpty(ConstructionItemTitle))
                    list = list.Where(c => c.ConstructionItemTitle.Contains(ConstructionItemTitle));

                listCount = list.Count();

                constructionItemsVMList = _mapper.Map<List<ConstructionItems>, List<ConstructionItemsVM>>(list.ToList());

                var constructionItemIds = constructionItemsVMList.Select(i => i.ConstructionItemId).ToList();

                List<ConstructionItemPricesHistories> pricesHistorieslist = new List<ConstructionItemPricesHistories>();

                if (!dateTimeTo.HasValue)
                    pricesHistorieslist = (from ph in publicApiDb.ConstructionItemPricesHistories
                                           where constructionItemIds.Contains(ph.ParentId) &&
                                           ph.ParentType.Equals("item")
                                           group ph by ph.ParentId into g
                                           select g.OrderByDescending(t => t.CreateEnDate.Value).FirstOrDefault()).ToList();
                else
                    pricesHistorieslist = (from ph in publicApiDb.ConstructionItemPricesHistories
                                           where constructionItemIds.Contains(ph.ParentId) &&
                                           ph.ParentType.Equals("item") &&
                                           ph.CreateEnDate.Value <= dateTimeTo.Value
                                           group ph by ph.ParentId into g
                                           select g.OrderByDescending(t => t.CreateEnDate.Value).FirstOrDefault()).ToList();

                foreach (var constructionItemsVM in constructionItemsVMList)
                {
                    var pricesHistory = pricesHistorieslist.Where(ph => ph.ParentId.Equals(constructionItemsVM.ConstructionItemId)).FirstOrDefault();
                    if (pricesHistory != null)
                    {
                        constructionItemsVM.LastItemValue = pricesHistory.ItemValue.Value;
                        constructionItemsVM.LastCreateEnDateItemValue = pricesHistory.CreateEnDate.Value;
                    }
                }
            }
            catch (Exception exc)
            { }

            return constructionItemsVMList;
        }

        public List<ConstructionItemsVM> GetListOfConstructionItems(int jtStartIndex,
            int jtPageSize,
            ref int listCount,
            long? ConstructionItemParentId,
            string ConstructionItemTitle,
            DateTime? dateTimeTo,
            string jtSorting = null)
        {
            List<ConstructionItemsVM> constructionItemsVMList = new List<ConstructionItemsVM>();

            try
            {
                var list = publicApiDb.ConstructionItems.Where(f => f.IsActivated.Value.Equals(true) && f.IsDeleted.Value.Equals(false)).AsQueryable();

                if (ConstructionItemParentId.HasValue)
                    if (ConstructionItemParentId.Value > 0)
                        list = list.Where(c => c.ConstructionItemParentId.HasValue).Where(c => c.ConstructionItemParentId.Value.Equals(ConstructionItemParentId.Value));

                if (!string.IsNullOrEmpty(ConstructionItemTitle))
                    list = list.Where(c => c.ConstructionItemTitle.Contains(ConstructionItemTitle));

                if (string.IsNullOrEmpty(jtSorting))
                {
                    listCount = list.Count();

                    if (listCount > jtPageSize)
                    {
                        //zonesVMList = _mapper.Map<List<Zones>, List<ZonesVM>>(list.OrderByDescending(s => s.ZoneId)
                        //         .Skip(jtStartIndex).Take(jtPageSize).ToList());

                        constructionItemsVMList = _mapper.Map<List<ConstructionItems>, List<ConstructionItemsVM>>(list.OrderByDescending(s => s.ConstructionItemId)
                                 .Skip(jtStartIndex).Take(jtPageSize).ToList());
                    }
                    else
                    {
                        //zonesVMList = _mapper.Map<List<Zones>,
                        //    List<ZonesVM>>(list.OrderByDescending(s => s.ZoneId).ToList());

                        constructionItemsVMList = _mapper.Map<List<ConstructionItems>, List<ConstructionItemsVM>>(list.OrderByDescending(s => s.ConstructionItemId).ToList());
                    }
                }
                else
                {
                    listCount = list.Count();

                    if (listCount > jtPageSize)
                    {
                        switch (jtSorting)
                        {
                            case "ConstructionItemTitle ASC":
                                list = list.OrderBy(l => l.ConstructionItemTitle);
                                break;
                            case "ConstructionItemTitle DESC":
                                list = list.OrderByDescending(l => l.ConstructionItemTitle);
                                break;
                        }
                        //zonesVMList = _mapper.Map<List<Zones>,
                        //    List<ZonesVM>>(list.OrderByDescending(s => s.ZoneId).Skip(jtStartIndex).Take(jtPageSize).ToList());

                        if (string.IsNullOrEmpty(jtSorting))
                            constructionItemsVMList = _mapper.Map<List<ConstructionItems>, List<ConstructionItemsVM>>(list.OrderByDescending(s => s.ConstructionItemId)
                                     .Skip(jtStartIndex).Take(jtPageSize).ToList());
                        else
                            constructionItemsVMList = _mapper.Map<List<ConstructionItems>, List<ConstructionItemsVM>>(list.Skip(jtStartIndex).Take(jtPageSize).ToList());
                    }
                    else
                    {
                        //zonesVMList = _mapper.Map<List<Zones>,
                        //    List<ZonesVM>>(list.ToList());

                        constructionItemsVMList = _mapper.Map<List<ConstructionItems>, List<ConstructionItemsVM>>(list.ToList());
                    }
                }

                var constructionItemIds = constructionItemsVMList.Select(i => i.ConstructionItemId).ToList();

                List<ConstructionItemPricesHistories> pricesHistorieslist = new List<ConstructionItemPricesHistories>();

                if (!dateTimeTo.HasValue)
                    pricesHistorieslist = (from ph in publicApiDb.ConstructionItemPricesHistories
                                           where constructionItemIds.Contains(ph.ParentId) &&
                                           ph.ParentType.Equals("item")
                                           group ph by ph.ParentId into g
                                           select g.OrderByDescending(t => t.CreateEnDate.Value).FirstOrDefault()).ToList();
                else
                    pricesHistorieslist = (from ph in publicApiDb.ConstructionItemPricesHistories
                                           where constructionItemIds.Contains(ph.ParentId) &&
                                           ph.ParentType.Equals("item") &&
                                           ph.CreateEnDate.Value <= dateTimeTo.Value
                                           group ph by ph.ParentId into g
                                           select g.OrderByDescending(t => t.CreateEnDate.Value).FirstOrDefault()).ToList();

                foreach (var constructionItemsVM in constructionItemsVMList)
                {
                    var pricesHistory = pricesHistorieslist.Where(ph => ph.ParentId.Equals(constructionItemsVM.ConstructionItemId)).FirstOrDefault();
                    if (pricesHistory != null)
                    {
                        constructionItemsVM.LastItemValue = pricesHistory.ItemValue.Value;
                        constructionItemsVM.LastCreateEnDateItemValue = pricesHistory.CreateEnDate.Value;
                    }
                }
            }
            catch (Exception exc)
            { }

            return constructionItemsVMList;
        }

        public long AddToConstructionItems(ConstructionItemsVM constructionItemsVM/*,
            List<long> childsUsersIds*/)
        {
            using (var transaction = publicApiDb.Database.BeginTransaction())
            {
                try
                {

                    if (publicApiDb.ConstructionItems//.Where(n => childsUsersIds.Contains(n.UserIdCreator.Value))
                                        .Where(x => x.ConstructionItemTitle.Equals(constructionItemsVM.ConstructionItemTitle)).Any())
                        return -1;
                    else
                    {
                        ConstructionItems constructionItems = _mapper.Map<ConstructionItemsVM, ConstructionItems>(constructionItemsVM);

                        publicApiDb.ConstructionItems.Add(constructionItems);
                        publicApiDb.SaveChanges();

                        if (constructionItemsVM.LastItemValue.HasValue)
                            if (constructionItemsVM.LastItemValue.Value > 0)
                            {
                                ConstructionItemPricesHistories constructionItemPricesHistories = new ConstructionItemPricesHistories();

                                constructionItemPricesHistories.CreateEnDate = DateTime.Now;
                                constructionItemPricesHistories.CreateTime = PersianDate.TimeNow;
                                constructionItemPricesHistories.UserIdCreator = constructionItemsVM.UserIdCreator.Value;
                                constructionItemPricesHistories.ItemValue = constructionItemsVM.LastItemValue.Value;
                                constructionItemPricesHistories.ItemPercentValue = 0;
                                constructionItemPricesHistories.IsDeleted = false;
                                constructionItemPricesHistories.IsActivated = true;
                                constructionItemPricesHistories.ParentId = constructionItems.ConstructionItemId;
                                constructionItemPricesHistories.ParentType = "item";

                                publicApiDb.ConstructionItemPricesHistories.Add(constructionItemPricesHistories);
                                publicApiDb.SaveChanges();
                            }

                        transaction.Commit();

                        return constructionItems.ConstructionItemId;
                    }

                }
                catch (Exception exc)
                {
                    transaction.Rollback();
                }
            }
            return 0;
        }

        public ConstructionItemsVM GetConstructionItemWithConstructionItemId(long constructionItemId/*,
            List<long> childsUsersIds*/)
        {
            ConstructionItemsVM ConstructionItemsVM = new ConstructionItemsVM();
            try
            {
                if (publicApiDb.ConstructionItems.
                    Where(l => l.ConstructionItemId.Equals(constructionItemId)).
                    Any())
                    ConstructionItemsVM = _mapper.Map<ConstructionItems, ConstructionItemsVM>(publicApiDb.ConstructionItems
                        .Where(l => l.ConstructionItemId.Equals(constructionItemId)).FirstOrDefault());
            }
            catch (Exception exc)
            { }
            return ConstructionItemsVM;
        }

        public long UpdateConstructionItems(ref ConstructionItemsVM constructionItemsVM/*,
            List<long> childsUsersIds*/)
        {
            using (var transaction = publicApiDb.Database.BeginTransaction())
            {
                try
                {
                    long constructionItemId = constructionItemsVM.ConstructionItemId;
                    int unitOfMeasurementId = constructionItemsVM.UnitOfMeasurementId.HasValue ? constructionItemsVM.UnitOfMeasurementId.Value : 0;
                    long constructionItemParentId = constructionItemsVM.ConstructionItemParentId.HasValue ? constructionItemsVM.ConstructionItemParentId.Value : 0;
                    string constructionItemTitle = constructionItemsVM.ConstructionItemTitle;
                    string constructionItemDesc = constructionItemsVM.ConstructionItemDesc;

                    if (!publicApiDb.ConstructionItems//.Where(n => childsUsersIds.Contains(n.UserIdCreator.Value))
                                .Where(x => x.ConstructionItemTitle.Equals(constructionItemTitle) &&
                                x.ConstructionItemId != constructionItemId).Any())
                    {
                        ConstructionItems constructionItems = (from c in publicApiDb.ConstructionItems
                                                               where c.ConstructionItemId == constructionItemId
                                                               select c).FirstOrDefault();

                        constructionItems.EditEnDate = constructionItemsVM.EditEnDate.Value;
                        constructionItems.EditTime = constructionItemsVM.EditTime;
                        constructionItems.UserIdEditor = constructionItemsVM.UserIdEditor;
                        constructionItems.UnitOfMeasurementId = unitOfMeasurementId;
                        constructionItems.ConstructionItemParentId = constructionItemParentId;
                        constructionItems.ConstructionItemTitle = constructionItemTitle;
                        constructionItems.ConstructionItemDesc = constructionItemDesc;
                        constructionItems.IsActivated = constructionItemsVM.IsActivated;
                        constructionItems.IsDeleted = constructionItemsVM.IsDeleted;

                        publicApiDb.Entry<ConstructionItems>(constructionItems).State = EntityState.Modified;
                        publicApiDb.SaveChanges();

                        ConstructionItemPricesHistories constructionItemPricesHistories = new ConstructionItemPricesHistories();

                        if (constructionItemsVM.LastItemValue.HasValue)
                            if (constructionItemsVM.LastItemValue.Value > 0)
                            {
                                constructionItemPricesHistories.CreateEnDate = DateTime.Now;
                                constructionItemPricesHistories.CreateTime = PersianDate.TimeNow;
                                constructionItemPricesHistories.UserIdCreator = constructionItemsVM.UserIdEditor.Value;
                                constructionItemPricesHistories.ItemValue = constructionItemsVM.LastItemValue.Value;
                                constructionItemPricesHistories.ItemPercentValue = 0;
                                constructionItemPricesHistories.IsDeleted = false;
                                constructionItemPricesHistories.IsActivated = true;
                                constructionItemPricesHistories.ParentId = constructionItems.ConstructionItemId;
                                constructionItemPricesHistories.ParentType = "item";

                                publicApiDb.ConstructionItemPricesHistories.Add(constructionItemPricesHistories);
                                publicApiDb.SaveChanges();
                            }

                        transaction.Commit();

                        constructionItemsVM.UserIdCreator = constructionItems.UserIdCreator.Value;
                        constructionItemsVM.LastCreateEnDateItemValue = constructionItemPricesHistories.CreateEnDate.Value;

                        return constructionItems.ConstructionItemId;
                    }
                    else
                        return -1;
                }
                catch (Exception exc)
                {
                    transaction.Rollback();
                }
            }

            return 0;
        }

        public bool ToggleActivationConstructionItems(long constructionItemId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var constructionItems = (from c in publicApiDb.ConstructionItems
                                         where c.ConstructionItemId == constructionItemId &&
                                         childsUsersIds.Contains(c.UserIdCreator.Value)
                                         select c).FirstOrDefault();

                if (constructionItems != null)
                {
                    constructionItems.IsActivated = !constructionItems.IsActivated;
                    constructionItems.EditEnDate = DateTime.Now;
                    constructionItems.EditTime = PersianDate.TimeNow;
                    constructionItems.UserIdEditor = userId;

                    publicApiDb.Entry<ConstructionItems>(constructionItems).State = EntityState.Modified;
                    publicApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool TemporaryDeleteConstructionItems(long constructionItemId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var constructionItems = (from c in publicApiDb.ConstructionItems
                                         where c.ConstructionItemId == constructionItemId &&
                                         childsUsersIds.Contains(c.UserIdCreator.Value)
                                         select c).FirstOrDefault();

                if (constructionItems != null)
                {
                    constructionItems.IsDeleted = !constructionItems.IsDeleted;
                    constructionItems.EditEnDate = DateTime.Now;
                    constructionItems.EditTime = PersianDate.TimeNow;
                    constructionItems.UserIdEditor = userId;

                    publicApiDb.Entry<ConstructionItems>(constructionItems).State = EntityState.Modified;
                    publicApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool CompleteDeleteConstructionItems(long constructionItemId,
            List<long> childsUsersIds,
            ref string returnMessage)
        {
            returnMessage = "";

            try
            {
                var constructionItems = (from c in publicApiDb.ConstructionItems
                                         where c.ConstructionItemId == constructionItemId &&
                                         childsUsersIds.Contains(c.UserIdCreator.Value)
                                         select c).FirstOrDefault();

                if (constructionItems != null)
                {
                    if (publicApiDb.ConstructionItems.Where(ci => ci.ConstructionItemParentId.HasValue).Where(ci => ci.ConstructionItemParentId.Value.Equals(constructionItems.ConstructionItemId)).Any())
                    {
                        returnMessage = "RemoveChildItemFirst";
                        return true;
                    }
                    else
                        if (publicApiDb.ConstructionSubItems.Where(ci => ci.ConstructionItemId.Equals(constructionItems.ConstructionItemId)).Any())
                    {
                        returnMessage = "RemoveSubItemFirst";
                        return true;
                    }

                    publicApiDb.ConstructionItems.Remove(constructionItems);
                    publicApiDb.SaveChanges();
                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        #endregion

        #region Methods For Work With ConstructionSubItems

        public List<ConstructionSubItemsVM> GetAllConstructionSubItemsList(ref int listCount,
            long? ConstructionItemId,
            string ConstructionSubItemTitle)
        {
            List<ConstructionSubItemsVM> ConstructionSubItemsVMList = new List<ConstructionSubItemsVM>();

            try
            {
                var list = publicApiDb.ConstructionSubItems.Where(f => f.IsActivated.Value.Equals(true) && f.IsDeleted.Value.Equals(false)).AsQueryable();

                if (ConstructionItemId.HasValue)
                    if (ConstructionItemId.Value > 0)
                        list = list.Where(c => c.ConstructionItemId.Equals(ConstructionItemId.Value));

                if (!string.IsNullOrEmpty(ConstructionSubItemTitle))
                    list = list.Where(c => c.ConstructionSubItemTitle.Contains(ConstructionSubItemTitle));

                listCount = list.Count();

                ConstructionSubItemsVMList = _mapper.Map<List<ConstructionSubItems>, List<ConstructionSubItemsVM>>(list.ToList());
            }
            catch (Exception exc)
            { }

            return ConstructionSubItemsVMList;
        }

        public List<ConstructionSubItemsVM> GetListOfConstructionSubItems(int jtStartIndex,
            int jtPageSize,
            ref int listCount,
            long? ConstructionItemId,
            string ConstructionSubItemTitle,
            string jtSorting = null)
        {
            List<ConstructionSubItemsVM> ConstructionSubItemsVMList = new List<ConstructionSubItemsVM>();

            try
            {
                var list = publicApiDb.ConstructionSubItems.Where(f => f.IsActivated.Value.Equals(true) && f.IsDeleted.Value.Equals(false)).AsQueryable();

                if (ConstructionItemId.HasValue)
                    if (ConstructionItemId.Value > 0)
                        list = list.Where(c => c.ConstructionItemId.Equals(ConstructionItemId.Value));

                if (!string.IsNullOrEmpty(ConstructionSubItemTitle))
                    list = list.Where(c => c.ConstructionSubItemTitle.Contains(ConstructionSubItemTitle));

                if (string.IsNullOrEmpty(jtSorting))
                {
                    listCount = list.Count();

                    if (listCount > jtPageSize)
                    {
                        //zonesVMList = _mapper.Map<List<Zones>, List<ZonesVM>>(list.OrderByDescending(s => s.ZoneId)
                        //         .Skip(jtStartIndex).Take(jtPageSize).ToList());

                        ConstructionSubItemsVMList = _mapper.Map<List<ConstructionSubItems>, List<ConstructionSubItemsVM>>(list.OrderByDescending(s => s.ConstructionSubItemId)
                                 .Skip(jtStartIndex).Take(jtPageSize).ToList());
                    }
                    else
                    {
                        //zonesVMList = _mapper.Map<List<Zones>,
                        //    List<ZonesVM>>(list.OrderByDescending(s => s.ZoneId).ToList());

                        ConstructionSubItemsVMList = _mapper.Map<List<ConstructionSubItems>, List<ConstructionSubItemsVM>>(list.OrderByDescending(s => s.ConstructionSubItemId).ToList());
                    }
                }
                else
                {
                    listCount = list.Count();

                    if (listCount > jtPageSize)
                    {
                        switch (jtSorting)
                        {
                            case "ConstructionSubItemTitle ASC":
                                list = list.OrderBy(l => l.ConstructionSubItemTitle);
                                break;
                            case "ConstructionSubItemTitle DESC":
                                list = list.OrderByDescending(l => l.ConstructionSubItemTitle);
                                break;
                        }
                        //zonesVMList = _mapper.Map<List<Zones>,
                        //    List<ZonesVM>>(list.OrderByDescending(s => s.ZoneId).Skip(jtStartIndex).Take(jtPageSize).ToList());

                        if (string.IsNullOrEmpty(jtSorting))
                            ConstructionSubItemsVMList = _mapper.Map<List<ConstructionSubItems>, List<ConstructionSubItemsVM>>(list.OrderByDescending(s => s.ConstructionSubItemId)
                                     .Skip(jtStartIndex).Take(jtPageSize).ToList());
                        else
                            ConstructionSubItemsVMList = _mapper.Map<List<ConstructionSubItems>, List<ConstructionSubItemsVM>>(list.Skip(jtStartIndex).Take(jtPageSize).ToList());
                    }
                    else
                    {
                        //zonesVMList = _mapper.Map<List<Zones>,
                        //    List<ZonesVM>>(list.ToList());

                        ConstructionSubItemsVMList = _mapper.Map<List<ConstructionSubItems>, List<ConstructionSubItemsVM>>(list.ToList());
                    }
                }
            }
            catch (Exception exc)
            { }

            return ConstructionSubItemsVMList;
        }

        public long AddToConstructionSubItems(ConstructionSubItemsVM ConstructionSubItemsVM/*,
            List<long> childsUsersIds*/)
        {
            try
            {

                if (publicApiDb.ConstructionSubItems//.Where(n => childsUsersIds.Contains(n.UserIdCreator.Value))
                                    .Where(x => x.ConstructionSubItemTitle.Equals(ConstructionSubItemsVM.ConstructionSubItemTitle)).Any())
                    return -1;
                else
                {
                    ConstructionSubItems ConstructionSubItems = _mapper.Map<ConstructionSubItemsVM, ConstructionSubItems>(ConstructionSubItemsVM);

                    publicApiDb.ConstructionSubItems.Add(ConstructionSubItems);
                    publicApiDb.SaveChanges();

                    return ConstructionSubItems.ConstructionSubItemId;
                }

            }
            catch (Exception exc)
            { }
            return 0;
        }

        public ConstructionSubItemsVM GetConstructionSubItemWithConstructionSubItemId(long ConstructionSubItemId/*,
            List<long> childsUsersIds*/)
        {
            ConstructionSubItemsVM ConstructionSubItemsVM = new ConstructionSubItemsVM();
            try
            {
                if (publicApiDb.ConstructionSubItems.
                    Where(l => l.ConstructionSubItemId.Equals(ConstructionSubItemId)).
                    Any())
                    ConstructionSubItemsVM = _mapper.Map<ConstructionSubItems, ConstructionSubItemsVM>(publicApiDb.ConstructionSubItems
                        .Where(l => l.ConstructionSubItemId.Equals(ConstructionSubItemId)).FirstOrDefault());
            }
            catch (Exception exc)
            { }
            return ConstructionSubItemsVM;
        }

        public long UpdateConstructionSubItems(ref ConstructionSubItemsVM ConstructionSubItemsVM/*,
            List<long> childsUsersIds*/)
        {
            try
            {
                long ConstructionSubItemId = ConstructionSubItemsVM.ConstructionSubItemId;
                int UnitOfMeasurementId = ConstructionSubItemsVM.UnitOfMeasurementId.HasValue ? ConstructionSubItemsVM.UnitOfMeasurementId.Value : 0;
                long ConstructionItemId = ConstructionSubItemsVM.ConstructionItemId;
                string ConstructionSubItemTitle = ConstructionSubItemsVM.ConstructionSubItemTitle;
                string ConstructionSubItemDesc = ConstructionSubItemsVM.ConstructionSubItemDesc;

                if (!publicApiDb.ConstructionSubItems//.Where(n => childsUsersIds.Contains(n.UserIdCreator.Value))
                            .Where(x => x.ConstructionSubItemTitle.Equals(ConstructionSubItemTitle) &&
                            x.ConstructionSubItemId != ConstructionSubItemId).Any())
                {
                    ConstructionSubItems ConstructionSubItems = (from c in publicApiDb.ConstructionSubItems
                                                                 where c.ConstructionSubItemId == ConstructionSubItemId
                                                                 select c).FirstOrDefault();

                    ConstructionSubItems.EditEnDate = ConstructionSubItemsVM.EditEnDate.Value;
                    ConstructionSubItems.EditTime = ConstructionSubItemsVM.EditTime;
                    ConstructionSubItems.UserIdEditor = ConstructionSubItemsVM.UserIdEditor;
                    ConstructionSubItems.ConstructionItemId = ConstructionItemId;
                    ConstructionSubItems.UnitOfMeasurementId = UnitOfMeasurementId;
                    ConstructionSubItems.ConstructionSubItemId = ConstructionSubItemId;
                    ConstructionSubItems.ConstructionSubItemTitle = ConstructionSubItemTitle;
                    ConstructionSubItems.ConstructionSubItemDesc = ConstructionSubItemDesc;
                    ConstructionSubItems.IsActivated = ConstructionSubItemsVM.IsActivated;
                    ConstructionSubItems.IsDeleted = ConstructionSubItemsVM.IsDeleted;

                    publicApiDb.Entry<ConstructionSubItems>(ConstructionSubItems).State = EntityState.Modified;
                    publicApiDb.SaveChanges();

                    ConstructionSubItemsVM.UserIdCreator = ConstructionSubItems.UserIdCreator.Value;

                    return ConstructionSubItems.ConstructionSubItemId;
                }
                else
                    return -1;
            }
            catch (Exception exc)
            { }

            return 0;
        }

        public bool ToggleActivationConstructionSubItems(long ConstructionSubItemId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var ConstructionSubItems = (from c in publicApiDb.ConstructionSubItems
                                            where c.ConstructionSubItemId == ConstructionSubItemId &&
                                            childsUsersIds.Contains(c.UserIdCreator.Value)
                                            select c).FirstOrDefault();

                if (ConstructionSubItems != null)
                {
                    ConstructionSubItems.IsActivated = !ConstructionSubItems.IsActivated;
                    ConstructionSubItems.EditEnDate = DateTime.Now;
                    ConstructionSubItems.EditTime = PersianDate.TimeNow;
                    ConstructionSubItems.UserIdEditor = userId;

                    publicApiDb.Entry<ConstructionSubItems>(ConstructionSubItems).State = EntityState.Modified;
                    publicApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool TemporaryDeleteConstructionSubItems(long ConstructionSubItemId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var ConstructionSubItems = (from c in publicApiDb.ConstructionSubItems
                                            where c.ConstructionSubItemId == ConstructionSubItemId &&
                                            childsUsersIds.Contains(c.UserIdCreator.Value)
                                            select c).FirstOrDefault();

                if (ConstructionSubItems != null)
                {
                    ConstructionSubItems.IsDeleted = !ConstructionSubItems.IsDeleted;
                    ConstructionSubItems.EditEnDate = DateTime.Now;
                    ConstructionSubItems.EditTime = PersianDate.TimeNow;
                    ConstructionSubItems.UserIdEditor = userId;

                    publicApiDb.Entry<ConstructionSubItems>(ConstructionSubItems).State = EntityState.Modified;
                    publicApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool CompleteDeleteConstructionSubItems(long ConstructionSubItemId,
            List<long> childsUsersIds)
        {
            try
            {
                var ConstructionSubItems = (from c in publicApiDb.ConstructionSubItems
                                            where c.ConstructionSubItemId == ConstructionSubItemId &&
                                            childsUsersIds.Contains(c.UserIdCreator.Value)
                                            select c).FirstOrDefault();

                if (ConstructionSubItems != null)
                {
                    publicApiDb.ConstructionSubItems.Remove(ConstructionSubItems);
                    publicApiDb.SaveChanges();
                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        #endregion

        #region Methods For Work With ConstructionItemPricesHistories

        public List<ConstructionItemPricesHistoriesVM> GetAllConstructionItemPricesHistoriesList(ref int listCount,
            string parentType,
            long parentId)
        {
            List<ConstructionItemPricesHistoriesVM> ConstructionItemPricesHistoriesVMList = new List<ConstructionItemPricesHistoriesVM>();

            try
            {
                var list = publicApiDb.ConstructionItemPricesHistories.Where(f => f.IsActivated.Value.Equals(true) && f.IsDeleted.Value.Equals(false)).AsQueryable();

                if (parentId > 0)
                    list = list.Where(c => c.ParentId.Equals(parentId));

                if (!string.IsNullOrEmpty(parentType))
                    list = list.Where(c => c.ParentType.Contains(parentType));

                listCount = list.Count();

                ConstructionItemPricesHistoriesVMList = _mapper.Map<List<ConstructionItemPricesHistories>, List<ConstructionItemPricesHistoriesVM>>(list.ToList());
            }
            catch (Exception exc)
            { }

            return ConstructionItemPricesHistoriesVMList;
        }

        public List<ConstructionItemPricesHistoriesVM> GetListOfConstructionItemPricesHistories(int jtStartIndex,
            int jtPageSize,
            ref int listCount,
            string parentType,
            long parentId,
            DateTime? dateTimeTo,
            string jtSorting = null)
        {
            List<ConstructionItemPricesHistoriesVM> ConstructionItemPricesHistoriesVMList = new List<ConstructionItemPricesHistoriesVM>();

            try
            {
                var list = publicApiDb.ConstructionItemPricesHistories.Where(f => f.IsActivated.Value.Equals(true) && f.IsDeleted.Value.Equals(false)).AsQueryable();

                if (parentId > 0)
                    list = list.Where(c => c.ParentId.Equals(parentId));

                if (!string.IsNullOrEmpty(parentType))
                    list = list.Where(c => c.ParentType.Contains(parentType));

                if (dateTimeTo.HasValue)
                    list = list.Where(c => c.CreateEnDate.Value <= dateTimeTo.Value);

                if (string.IsNullOrEmpty(jtSorting))
                {
                    listCount = list.Count();

                    if (listCount > jtPageSize)
                    {
                        //zonesVMList = _mapper.Map<List<Zones>, List<ZonesVM>>(list.OrderByDescending(s => s.ZoneId)
                        //         .Skip(jtStartIndex).Take(jtPageSize).ToList());

                        ConstructionItemPricesHistoriesVMList = _mapper.Map<List<ConstructionItemPricesHistories>, List<ConstructionItemPricesHistoriesVM>>(list.OrderByDescending(s => s.CreateEnDate.Value)
                                 .Skip(jtStartIndex).Take(jtPageSize).ToList());
                    }
                    else
                    {
                        //zonesVMList = _mapper.Map<List<Zones>,
                        //    List<ZonesVM>>(list.OrderByDescending(s => s.ZoneId).ToList());

                        ConstructionItemPricesHistoriesVMList = _mapper.Map<List<ConstructionItemPricesHistories>, List<ConstructionItemPricesHistoriesVM>>(list.OrderByDescending(s => s.CreateEnDate.Value).ToList());
                    }
                }
                else
                {
                    listCount = list.Count();

                    if (listCount > jtPageSize)
                    {
                        //switch (jtSorting)
                        //{
                        //    case "ConstructionItemTitle ASC":
                        //        list = list.OrderBy(l => l.ConstructionItemTitle);
                        //        break;
                        //    case "ConstructionItemTitle DESC":
                        //        list = list.OrderByDescending(l => l.ConstructionItemTitle);
                        //        break;
                        //}
                        //zonesVMList = _mapper.Map<List<Zones>,
                        //    List<ZonesVM>>(list.OrderByDescending(s => s.ZoneId).Skip(jtStartIndex).Take(jtPageSize).ToList());

                        if (string.IsNullOrEmpty(jtSorting))
                            ConstructionItemPricesHistoriesVMList = _mapper.Map<List<ConstructionItemPricesHistories>, List<ConstructionItemPricesHistoriesVM>>(list.OrderByDescending(s => s.CreateEnDate.Value)
                                     .Skip(jtStartIndex).Take(jtPageSize).ToList());
                        else
                            ConstructionItemPricesHistoriesVMList = _mapper.Map<List<ConstructionItemPricesHistories>, List<ConstructionItemPricesHistoriesVM>>(list.Skip(jtStartIndex).Take(jtPageSize).ToList());
                    }
                    else
                    {
                        //zonesVMList = _mapper.Map<List<Zones>,
                        //    List<ZonesVM>>(list.ToList());

                        ConstructionItemPricesHistoriesVMList = _mapper.Map<List<ConstructionItemPricesHistories>, List<ConstructionItemPricesHistoriesVM>>(list.ToList());
                    }
                }
            }
            catch (Exception exc)
            { }

            return ConstructionItemPricesHistoriesVMList;
        }

        public long AddToConstructionItemPricesHistories(ConstructionItemPricesHistoriesVM ConstructionItemPricesHistoriesVM/*,
            List<long> childsUsersIds*/)
        {
            try
            {

                ConstructionItemPricesHistories ConstructionItemPricesHistories = _mapper.Map<ConstructionItemPricesHistoriesVM, ConstructionItemPricesHistories>(ConstructionItemPricesHistoriesVM);

                publicApiDb.ConstructionItemPricesHistories.Add(ConstructionItemPricesHistories);
                publicApiDb.SaveChanges();

                return ConstructionItemPricesHistories.ConstructionItemPricesHistoryId;

            }
            catch (Exception exc)
            { }
            return 0;
        }

        public ConstructionItemPricesHistoriesVM GetConstructionItemHistoryWithConstructionItemHistoryId(long constructionItemPricesHistoryId/*,
            List<long> childsUsersIds*/)
        {
            ConstructionItemPricesHistoriesVM ConstructionItemPricesHistoriesVM = new ConstructionItemPricesHistoriesVM();
            try
            {
                if (publicApiDb.ConstructionItemPricesHistories.
                    Where(l => l.ConstructionItemPricesHistoryId.Equals(constructionItemPricesHistoryId)).
                    Any())
                    ConstructionItemPricesHistoriesVM = _mapper.Map<ConstructionItemPricesHistories, ConstructionItemPricesHistoriesVM>(publicApiDb.ConstructionItemPricesHistories
                        .Where(l => l.ConstructionItemPricesHistoryId.Equals(constructionItemPricesHistoryId)).FirstOrDefault());
            }
            catch (Exception exc)
            { }
            return ConstructionItemPricesHistoriesVM;
        }

        public long UpdateConstructionItemPricesHistories(ref ConstructionItemPricesHistoriesVM ConstructionItemPricesHistoriesVM/*,
            List<long> childsUsersIds*/)
        {
            try
            {
                long constructionItemPricesHistoryId = ConstructionItemPricesHistoriesVM.ConstructionItemPricesHistoryId;
                string parentType = ConstructionItemPricesHistoriesVM.ParentType;
                long parentId = ConstructionItemPricesHistoriesVM.ParentId;
                double itemPercentValue = ConstructionItemPricesHistoriesVM.ItemPercentValue.HasValue ? ConstructionItemPricesHistoriesVM.ItemPercentValue.Value : 0;
                long itemValue = ConstructionItemPricesHistoriesVM.ItemValue.HasValue ? ConstructionItemPricesHistoriesVM.ItemValue.Value : 0;

                ConstructionItemPricesHistories ConstructionItemPricesHistories = (from c in publicApiDb.ConstructionItemPricesHistories
                                                                                   where c.ConstructionItemPricesHistoryId == constructionItemPricesHistoryId
                                                                                   select c).FirstOrDefault();

                ConstructionItemPricesHistories.EditEnDate = ConstructionItemPricesHistoriesVM.EditEnDate.Value;
                ConstructionItemPricesHistories.EditTime = ConstructionItemPricesHistoriesVM.EditTime;
                ConstructionItemPricesHistories.UserIdEditor = ConstructionItemPricesHistoriesVM.UserIdEditor;
                ConstructionItemPricesHistories.ParentType = parentType;
                ConstructionItemPricesHistories.ParentId = parentId;
                ConstructionItemPricesHistories.ItemPercentValue = itemPercentValue;
                ConstructionItemPricesHistories.ItemValue = itemValue;
                ConstructionItemPricesHistories.IsActivated = ConstructionItemPricesHistoriesVM.IsActivated;
                ConstructionItemPricesHistories.IsDeleted = ConstructionItemPricesHistoriesVM.IsDeleted;

                publicApiDb.Entry<ConstructionItemPricesHistories>(ConstructionItemPricesHistories).State = EntityState.Modified;
                publicApiDb.SaveChanges();

                ConstructionItemPricesHistoriesVM.UserIdCreator = ConstructionItemPricesHistories.UserIdCreator.Value;

                return ConstructionItemPricesHistories.ConstructionItemPricesHistoryId;
            }
            catch (Exception exc)
            { }

            return 0;
        }

        public bool ToggleActivationConstructionItemPricesHistories(long constructionItemPricesHistoryId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var ConstructionItemPricesHistories = (from c in publicApiDb.ConstructionItemPricesHistories
                                                       where c.ConstructionItemPricesHistoryId == constructionItemPricesHistoryId &&
                                                       childsUsersIds.Contains(c.UserIdCreator.Value)
                                                       select c).FirstOrDefault();

                if (ConstructionItemPricesHistories != null)
                {
                    ConstructionItemPricesHistories.IsActivated = !ConstructionItemPricesHistories.IsActivated;
                    ConstructionItemPricesHistories.EditEnDate = DateTime.Now;
                    ConstructionItemPricesHistories.EditTime = PersianDate.TimeNow;
                    ConstructionItemPricesHistories.UserIdEditor = userId;

                    publicApiDb.Entry<ConstructionItemPricesHistories>(ConstructionItemPricesHistories).State = EntityState.Modified;
                    publicApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool TemporaryDeleteConstructionItemPricesHistories(long constructionItemPricesHistoryId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var ConstructionItemPricesHistories = (from c in publicApiDb.ConstructionItemPricesHistories
                                                       where c.ConstructionItemPricesHistoryId == constructionItemPricesHistoryId &&
                                                       childsUsersIds.Contains(c.UserIdCreator.Value)
                                                       select c).FirstOrDefault();

                if (ConstructionItemPricesHistories != null)
                {
                    ConstructionItemPricesHistories.IsDeleted = !ConstructionItemPricesHistories.IsDeleted;
                    ConstructionItemPricesHistories.EditEnDate = DateTime.Now;
                    ConstructionItemPricesHistories.EditTime = PersianDate.TimeNow;
                    ConstructionItemPricesHistories.UserIdEditor = userId;

                    publicApiDb.Entry<ConstructionItemPricesHistories>(ConstructionItemPricesHistories).State = EntityState.Modified;
                    publicApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool CompleteDeleteConstructionItemPricesHistories(long constructionItemPricesHistoryId,
            List<long> childsUsersIds,
            ref string returnMessage)
        {
            returnMessage = "";

            try
            {
                var ConstructionItemPricesHistories = (from c in publicApiDb.ConstructionItemPricesHistories
                                                       where c.ConstructionItemPricesHistoryId == constructionItemPricesHistoryId &&
                                                       childsUsersIds.Contains(c.UserIdCreator.Value)
                                                       select c).FirstOrDefault();

                if (ConstructionItemPricesHistories != null)
                {
                    publicApiDb.ConstructionItemPricesHistories.Remove(ConstructionItemPricesHistories);
                    publicApiDb.SaveChanges();
                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        #endregion

        #region Methods For Work With Cities

        public List<CitiesVM> GetAllCitiesList(ref int listCount,
            long? StateId,
            string cityName,
            string cityCode)
        {
            List<CitiesVM> citiesVMList = new List<CitiesVM>();

            try
            {
                var list = publicApiDb.Cities.Where(f => f.IsActivated.Value.Equals(true) && f.IsDeleted.Value.Equals(false)).AsQueryable();

                if (StateId.HasValue)
                    if (StateId.Value > 0)
                        list = list.Where(c => c.StateId.HasValue).Where(c => c.StateId.Value.Equals(StateId.Value));

                if (!string.IsNullOrEmpty(cityName))
                    list = list.Where(c => c.CityName.Contains(cityName));

                if (!string.IsNullOrEmpty(cityCode))
                    list = list.Where(c => c.CityCode.Contains(cityCode));

                listCount = list.Count();

                citiesVMList = _mapper.Map<List<Cities>, List<CitiesVM>>(list.ToList());
            }
            catch (Exception exc)
            { }

            return citiesVMList;
        }

        public List<CitiesVM> GetAllCitiesListWithOutStrPolygon(ref int listCount,
            long? StateId,
            string cityName,
            string cityCode)
        {
            List<CitiesVM> citiesVMList = new List<CitiesVM>();

            try
            {
                var list = publicApiDb.Cities.Where(f => f.IsActivated.Value.Equals(true) && f.IsDeleted.Value.Equals(false)).AsQueryable();

                if (StateId.HasValue)
                    if (StateId.Value > 0)
                        list = list.Where(c => c.StateId.HasValue).Where(c => c.StateId.Value.Equals(StateId.Value));

                if (!string.IsNullOrEmpty(cityName))
                    list = list.Where(c => c.CityName.Contains(cityName));

                if (!string.IsNullOrEmpty(cityCode))
                    list = list.Where(c => c.CityCode.Contains(cityCode));

                listCount = list.Count();

                //citiesVMList = _mapper.Map<List<Cities>, List<CitiesVM>>(list.ToList());

                citiesVMList = list.Select(c => new CitiesVM
                {
                    CityCode = c.CityCode,
                    CityId = c.CityId,
                    CityName = c.CityName,
                    CreateEnDate = c.CreateEnDate.Value,
                    CreateTime = c.CreateTime,
                    EditEnDate = c.EditEnDate.Value,
                    EditTime = c.EditTime,
                    IsActivated = c.IsActivated,
                    IsDeleted = c.IsDeleted,
                    RemoveEnDate = c.RemoveEnDate.Value,
                    RemoveTime = c.RemoveTime,
                    StateId = c.StateId.Value,
                    UserIdCreator = c.UserIdCreator,
                    UserIdEditor = c.UserIdEditor,
                    UserIdRemover = c.UserIdRemover,
                }).ToList(); ;
            }
            catch (Exception exc)
            { }

            return citiesVMList;
        }

        public List<CitiesVM> GetListOfCities(int jtStartIndex,
            int jtPageSize,
            ref int listCount,
            long? StateId,
            string cityName,
            string cityCode,
            string jtSorting = null)
        {
            List<CitiesVM> citiesVMList = new List<CitiesVM>();

            try
            {
                var list = publicApiDb.Cities.Where(f => f.IsActivated.Value.Equals(true) && f.IsDeleted.Value.Equals(false)).AsQueryable();

                if (StateId.HasValue)
                    if (StateId.Value > 0)
                        list = list.Where(c => c.StateId.HasValue).Where(c => c.StateId.Value.Equals(StateId.Value));

                if (!string.IsNullOrEmpty(cityName))
                    list = list.Where(c => c.CityName.Contains(cityName));

                if (!string.IsNullOrEmpty(cityCode))
                    list = list.Where(c => c.CityCode.Contains(cityCode));

                if (string.IsNullOrEmpty(jtSorting))
                {
                    listCount = list.Count();

                    if (listCount > jtPageSize)
                    {
                        //zonesVMList = _mapper.Map<List<Zones>, List<ZonesVM>>(list.OrderByDescending(s => s.ZoneId)
                        //         .Skip(jtStartIndex).Take(jtPageSize).ToList());

                        citiesVMList = _mapper.Map<List<Cities>, List<CitiesVM>>(list.OrderByDescending(s => s.CityId)
                                 .Skip(jtStartIndex).Take(jtPageSize).ToList());
                    }
                    else
                    {
                        //zonesVMList = _mapper.Map<List<Zones>,
                        //    List<ZonesVM>>(list.OrderByDescending(s => s.ZoneId).ToList());

                        citiesVMList = _mapper.Map<List<Cities>, List<CitiesVM>>(list.OrderByDescending(s => s.CityId).ToList());
                    }
                }
                else
                {
                    listCount = list.Count();

                    if (listCount > jtPageSize)
                    {
                        switch (jtSorting)
                        {
                            case "CityName ASC":
                                list = list.OrderBy(l => l.CityName);
                                break;
                            case "CityName DESC":
                                list = list.OrderByDescending(l => l.CityName);
                                break;
                        }
                        //zonesVMList = _mapper.Map<List<Zones>,
                        //    List<ZonesVM>>(list.OrderByDescending(s => s.ZoneId).Skip(jtStartIndex).Take(jtPageSize).ToList());

                        if (string.IsNullOrEmpty(jtSorting))
                            citiesVMList = _mapper.Map<List<Cities>, List<CitiesVM>>(list.OrderByDescending(s => s.CityId)
                                     .Skip(jtStartIndex).Take(jtPageSize).ToList());
                        else
                            citiesVMList = _mapper.Map<List<Cities>, List<CitiesVM>>(list.Skip(jtStartIndex).Take(jtPageSize).ToList());
                    }
                    else
                    {
                        //zonesVMList = _mapper.Map<List<Zones>,
                        //    List<ZonesVM>>(list.ToList());

                        citiesVMList = _mapper.Map<List<Cities>, List<CitiesVM>>(list.ToList());
                    }
                }
            }
            catch (Exception exc)
            { }

            return citiesVMList;
        }

        public List<CitiesVM> GetListOfCitiesWithOutStrPolygon(int jtStartIndex,
            int jtPageSize,
            ref int listCount,
            long? StateId,
            string cityName,
            string cityCode,
            string jtSorting = null)
        {
            List<CitiesVM> citiesVMList = new List<CitiesVM>();

            try
            {
                var list = publicApiDb.Cities.Where(f => f.IsActivated.Value.Equals(true) && f.IsDeleted.Value.Equals(false)).AsQueryable();

                if (StateId.HasValue)
                    if (StateId.Value > 0)
                        list = list.Where(c => c.StateId.HasValue).Where(c => c.StateId.Value.Equals(StateId.Value));

                if (!string.IsNullOrEmpty(cityName))
                    list = list.Where(c => c.CityName.Contains(cityName));

                if (!string.IsNullOrEmpty(cityCode))
                    list = list.Where(c => c.CityCode.Contains(cityCode));

                if (string.IsNullOrEmpty(jtSorting))
                {
                    listCount = list.Count();

                    if (listCount > jtPageSize)
                    {
                        //zonesVMList = _mapper.Map<List<Zones>, List<ZonesVM>>(list.OrderByDescending(s => s.ZoneId)
                        //         .Skip(jtStartIndex).Take(jtPageSize).ToList());

                        //citiesVMList = _mapper.Map<List<Cities>, List<CitiesVM>>(list.OrderByDescending(s => s.CityId)
                        //         .Skip(jtStartIndex).Take(jtPageSize).ToList());

                        citiesVMList = list.OrderByDescending(s => s.CityId).
                                 Skip(jtStartIndex).Take(jtPageSize).Select(c => new CitiesVM
                                 {
                                     CityCode = c.CityCode,
                                     CityId = c.CityId,
                                     CityName = c.CityName,
                                     CreateEnDate = c.CreateEnDate.Value,
                                     CreateTime = c.CreateTime,
                                     EditEnDate = c.EditEnDate.Value,
                                     EditTime = c.EditTime,
                                     IsActivated = c.IsActivated,
                                     IsDeleted = c.IsDeleted,
                                     RemoveEnDate = c.RemoveEnDate.Value,
                                     RemoveTime = c.RemoveTime,
                                     StateId = c.StateId.Value,
                                     UserIdCreator = c.UserIdCreator,
                                     UserIdEditor = c.UserIdEditor,
                                     UserIdRemover = c.UserIdRemover,
                                 }).ToList();
                    }
                    else
                    {
                        //zonesVMList = _mapper.Map<List<Zones>,
                        //    List<ZonesVM>>(list.OrderByDescending(s => s.ZoneId).ToList());

                        //citiesVMList = _mapper.Map<List<Cities>, List<CitiesVM>>(list.OrderByDescending(s => s.CityId).ToList());

                        citiesVMList = list.OrderByDescending(s => s.CityId).Select(c => new CitiesVM
                        {
                            CityCode = c.CityCode,
                            CityId = c.CityId,
                            CityName = c.CityName,
                            CreateEnDate = c.CreateEnDate.Value,
                            CreateTime = c.CreateTime,
                            EditEnDate = c.EditEnDate.Value,
                            EditTime = c.EditTime,
                            IsActivated = c.IsActivated,
                            IsDeleted = c.IsDeleted,
                            RemoveEnDate = c.RemoveEnDate.Value,
                            RemoveTime = c.RemoveTime,
                            StateId = c.StateId.Value,
                            UserIdCreator = c.UserIdCreator,
                            UserIdEditor = c.UserIdEditor,
                            UserIdRemover = c.UserIdRemover,
                        }).ToList();
                    }
                }
                else
                {
                    listCount = list.Count();

                    if (listCount > jtPageSize)
                    {
                        switch (jtSorting)
                        {
                            case "CityName ASC":
                                list = list.OrderBy(l => l.CityName);
                                break;
                            case "CityName DESC":
                                list = list.OrderByDescending(l => l.CityName);
                                break;
                        }
                        //zonesVMList = _mapper.Map<List<Zones>,
                        //    List<ZonesVM>>(list.OrderByDescending(s => s.ZoneId).Skip(jtStartIndex).Take(jtPageSize).ToList());

                        if (string.IsNullOrEmpty(jtSorting))
                        {
                            //citiesVMList = _mapper.Map<List<Cities>, List<CitiesVM>>(list.OrderByDescending(s => s.CityId)
                            //         .Skip(jtStartIndex).Take(jtPageSize).ToList());

                            citiesVMList = list.OrderByDescending(s => s.CityId).
                                     Skip(jtStartIndex).Take(jtPageSize).Select(c => new CitiesVM
                                     {
                                         CityCode = c.CityCode,
                                         CityId = c.CityId,
                                         CityName = c.CityName,
                                         CreateEnDate = c.CreateEnDate.Value,
                                         CreateTime = c.CreateTime,
                                         EditEnDate = c.EditEnDate.Value,
                                         EditTime = c.EditTime,
                                         IsActivated = c.IsActivated,
                                         IsDeleted = c.IsDeleted,
                                         RemoveEnDate = c.RemoveEnDate.Value,
                                         RemoveTime = c.RemoveTime,
                                         StateId = c.StateId.Value,
                                         UserIdCreator = c.UserIdCreator,
                                         UserIdEditor = c.UserIdEditor,
                                         UserIdRemover = c.UserIdRemover,
                                     }).ToList();
                        }
                        else
                        {
                            citiesVMList = list.Skip(jtStartIndex).Take(jtPageSize).Select(c => new CitiesVM
                            {
                                CityCode = c.CityCode,
                                CityId = c.CityId,
                                CityName = c.CityName,
                                CreateEnDate = c.CreateEnDate.Value,
                                CreateTime = c.CreateTime,
                                EditEnDate = c.EditEnDate.Value,
                                EditTime = c.EditTime,
                                IsActivated = c.IsActivated,
                                IsDeleted = c.IsDeleted,
                                RemoveEnDate = c.RemoveEnDate.Value,
                                RemoveTime = c.RemoveTime,
                                StateId = c.StateId.Value,
                                UserIdCreator = c.UserIdCreator,
                                UserIdEditor = c.UserIdEditor,
                                UserIdRemover = c.UserIdRemover,
                            }).ToList();

                            //citiesVMList = _mapper.Map<List<Cities>, List<CitiesVM>>(list.Skip(jtStartIndex).Take(jtPageSize).ToList());
                        }
                    }
                    else
                    {
                        //zonesVMList = _mapper.Map<List<Zones>,
                        //    List<ZonesVM>>(list.ToList());

                        //citiesVMList = _mapper.Map<List<Cities>, List<CitiesVM>>(list.ToList());

                        citiesVMList = list.Select(c => new CitiesVM
                        {
                            CityCode = c.CityCode,
                            CityId = c.CityId,
                            CityName = c.CityName,
                            CreateEnDate = c.CreateEnDate.Value,
                            CreateTime = c.CreateTime,
                            EditEnDate = c.EditEnDate.Value,
                            EditTime = c.EditTime,
                            IsActivated = c.IsActivated,
                            IsDeleted = c.IsDeleted,
                            RemoveEnDate = c.RemoveEnDate.Value,
                            RemoveTime = c.RemoveTime,
                            StateId = c.StateId.Value,
                            UserIdCreator = c.UserIdCreator,
                            UserIdEditor = c.UserIdEditor,
                            UserIdRemover = c.UserIdRemover,
                        }).ToList();
                    }
                }
            }
            catch (Exception exc)
            { }

            return citiesVMList;
        }

        public long AddToCities(CitiesVM citysVM)
        {
            try
            {

                if (publicApiDb.Cities//.Where(n => childsUsersIds.Contains(n.UserIdCreator.Value))
                                    .Where(x => x.StateId == citysVM.StateId &&
                                        x.CityName.Equals(citysVM.CityName)).Any())
                    return -1;
                else
                {
                    Cities city = _mapper.Map<CitiesVM, Cities>(citysVM);

                    publicApiDb.Cities.Add(city);
                    publicApiDb.SaveChanges();

                    return city.CityId;
                }

            }
            catch (Exception exc)
            { }
            return 0;
        }

        public CitiesVM GetCityWithCityId(long CityId/*,
            List<long> childsUsersIds*/)
        {
            CitiesVM citiesVM = new CitiesVM();
            try
            {
                if (publicApiDb.Cities.
                    Where(l => l.CityId.Equals(CityId)).
                    Any())
                    citiesVM = _mapper.Map<Cities, CitiesVM>(publicApiDb.Cities
                        .Where(l => l.CityId.Equals(CityId)).FirstOrDefault());
            }
            catch (Exception exc)
            { }
            return citiesVM;
        }

        public long UpdateCities(ref CitiesVM citysVM/*,
            List<long> childsUsersIds*/)
        {
            try
            {
                long cityId = citysVM.CityId;
                long stateId = citysVM.StateId;
                string cityName = citysVM.CityName;
                string cityCode = citysVM.CityCode;
                string strPolygon = citysVM.StrPolygon;

                if (!publicApiDb.Cities//.Where(n => childsUsersIds.Contains(n.UserIdCreator.Value))
                            .Where(x => x.CityName.Equals(cityName) &&
                            x.StateId == stateId &&
                            !x.CityId.Equals(cityId) &&
                            x.CityName.Equals(cityName)).Any())
                {
                    Cities city = (from c in publicApiDb.Cities
                                   where c.CityId == cityId
                                   select c).FirstOrDefault();

                    city.StateId = citysVM.StateId;
                    city.EditEnDate = citysVM.EditEnDate.Value;
                    city.EditTime = citysVM.EditTime;
                    city.UserIdEditor = citysVM.UserIdEditor;
                    city.CityId = citysVM.CityId;
                    city.CityName = citysVM.CityName;
                    city.CityCode = citysVM.CityCode;
                    city.StrPolygon = citysVM.StrPolygon;
                    city.IsActivated = citysVM.IsActivated;
                    city.IsDeleted = citysVM.IsDeleted;

                    publicApiDb.Entry<Cities>(city).State = EntityState.Modified;
                    publicApiDb.SaveChanges();

                    citysVM.UserIdCreator = city.UserIdCreator.Value;

                    return city.CityId;
                }
                else
                    return -1;
            }
            catch (Exception exc)
            { }

            return 0;
        }

        public bool ToggleActivationCities(long cityId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var City = (from c in publicApiDb.Cities
                            where c.CityId == cityId &&
                            childsUsersIds.Contains(c.UserIdCreator.Value)
                            select c).FirstOrDefault();

                if (City != null)
                {
                    City.IsActivated = !City.IsActivated;
                    City.EditEnDate = DateTime.Now;
                    City.EditTime = PersianDate.TimeNow;
                    City.UserIdEditor = userId;

                    publicApiDb.Entry<Cities>(City).State = EntityState.Modified;
                    publicApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool TemporaryDeleteCities(long cityId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var City = (from c in publicApiDb.Cities
                            where c.CityId == cityId &&
                            childsUsersIds.Contains(c.UserIdCreator.Value)
                            select c).FirstOrDefault();

                if (City != null)
                {
                    City.IsDeleted = !City.IsDeleted;
                    City.EditEnDate = DateTime.Now;
                    City.EditTime = PersianDate.TimeNow;
                    City.UserIdEditor = userId;

                    publicApiDb.Entry<Cities>(City).State = EntityState.Modified;
                    publicApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool CompleteDeleteCities(long cityId,
            List<long> childsUsersIds)
        {
            try
            {
                var City = (from c in publicApiDb.Cities
                            where c.CityId == cityId &&
                            childsUsersIds.Contains(c.UserIdCreator.Value)
                            select c).FirstOrDefault();

                if (City != null)
                {
                    publicApiDb.Cities.Remove(City);
                    publicApiDb.SaveChanges();
                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        #endregion

        #region Methods For Work With Districts

        public List<DistrictsVM> GetAllDistrictsList(
            long? stateId = null,
            long? cityId = null,
            long? zoneId = null,
           string? districtName = "")
        {
            List<DistrictsVM> districtsVMList = new List<DistrictsVM>();

            try
            {
                var list = (from s in publicApiDb.States
                            join c in publicApiDb.Cities on s.StateId equals c.StateId
                            join z in publicApiDb.Zones on c.CityId equals z.CityId
                            join d in publicApiDb.Districts on z.ZoneId equals d.ZoneId
                            where s.IsActivated.Value.Equals(true) &&
                            s.IsDeleted.Value.Equals(false)
                            select new DistrictsVM
                            {
                                DistrictId = d.DistrictId,
                                DistrictName = d.DistrictName,
                                TownName = d.TownName,
                                VillageName = d.VillageName,
                                ZoneId = z.ZoneId,
                                ZoneName = z.ZoneName,
                                CityId = c.CityId,
                                CityName = c.CityName,
                                StateId = s.StateId,
                                StateName = s.StateName,
                                StrPolygon = d.StrPolygon,
                                CreateEnDate = d.CreateEnDate,
                                CreateTime = d.CreateTime,
                                EditEnDate = d.EditEnDate,
                                EditTime = d.EditTime,
                                IsActivated = d.IsActivated,
                                IsDeleted = d.IsDeleted,
                                RemoveEnDate = d.RemoveEnDate,
                                RemoveTime = d.EditTime,
                                UserCreatorName = "",
                                UserIdCreator = d.UserIdCreator.Value,
                                UserIdEditor = d.UserIdEditor.Value,
                                UserIdRemover = d.UserIdRemover.Value,
                            }).AsQueryable();

                if (zoneId.HasValue)
                    if (zoneId.Value > 0)
                        list = list.Where(a => a.ZoneId.Equals(zoneId.Value));


                if (cityId.HasValue)
                    if (cityId.Value > 0)
                        list = list.Where(a => a.CityId.Equals(cityId.Value));

                if (stateId.HasValue)
                    if (stateId.Value > 0)
                        list = list.Where(a => a.StateId.Equals(stateId.Value));

                if (!string.IsNullOrEmpty(districtName))
                    list = list.Where(z => z.DistrictName.Contains(districtName));

                districtsVMList = list.OrderByDescending(f => f.DistrictId).ToList();


            }
            catch (Exception exc)
            { }


            return districtsVMList;
        }

        public List<DistrictsVM> GetAllDistrictsListWithOutStrPolygon(
            long? stateId = null,
            long? cityId = null,
            long? zoneId = null,
           string? districtName = "")
        {
            List<DistrictsVM> districtsVMList = new List<DistrictsVM>();

            try
            {
                var list = (from s in publicApiDb.States
                            join c in publicApiDb.Cities on s.StateId equals c.StateId
                            join z in publicApiDb.Zones on c.CityId equals z.CityId
                            join d in publicApiDb.Districts on z.ZoneId equals d.ZoneId
                            where s.IsActivated.Value.Equals(true) &&
                            s.IsDeleted.Value.Equals(false)
                            select new DistrictsVM
                            {
                                DistrictId = d.DistrictId,
                                DistrictName = d.DistrictName,
                                TownName = d.TownName,
                                VillageName = d.VillageName,
                                ZoneId = z.ZoneId,
                                ZoneName = z.ZoneName,
                                CityId = c.CityId,
                                CityName = c.CityName,
                                StateId = s.StateId,
                                StateName = s.StateName,
                                Description = !string.IsNullOrEmpty(d.Description) ? d.Description : "",
                                StrPolygon = d.StrPolygon,
                                CreateEnDate = d.CreateEnDate,
                                CreateTime = d.CreateTime,
                                EditEnDate = d.EditEnDate,
                                EditTime = d.EditTime,
                                IsActivated = d.IsActivated,
                                IsDeleted = d.IsDeleted,
                                RemoveEnDate = d.RemoveEnDate,
                                RemoveTime = d.EditTime,
                                UserCreatorName = "",
                                UserIdCreator = d.UserIdCreator.Value,
                                UserIdEditor = d.UserIdEditor.Value,
                                UserIdRemover = d.UserIdRemover.Value,
                            }).AsQueryable();

                if (zoneId.HasValue)
                    if (zoneId.Value > 0)
                        list = list.Where(a => a.ZoneId.Equals(zoneId.Value));


                if (cityId.HasValue)
                    if (cityId.Value > 0)
                        list = list.Where(a => a.CityId.Equals(cityId.Value));

                if (stateId.HasValue)
                    if (stateId.Value > 0)
                        list = list.Where(a => a.StateId.Equals(stateId.Value));

                if (!string.IsNullOrEmpty(districtName))
                    list = list.Where(z => z.DistrictName.Contains(districtName));

                districtsVMList = list.OrderByDescending(f => f.DistrictId).ToList();


            }
            catch (Exception exc)
            { }


            return districtsVMList;
        }


        public List<DistrictsVM> GetListOfDistricts(int jtStartIndex,
             int jtPageSize,
             ref int listCount,
             long? stateId = null,
             long? cityId = null,
             long? zoneId = null,
             string? districtName = "",
             string jtSorting = null)
        {
            List<DistrictsVM> districtsVMList = new List<DistrictsVM>();

            try
            {
                var list = (from s in publicApiDb.States
                            join c in publicApiDb.Cities on s.StateId equals c.StateId
                            join z in publicApiDb.Zones on c.CityId equals z.CityId
                            join d in publicApiDb.Districts on z.ZoneId equals d.ZoneId
                            where s.IsActivated.Value.Equals(true) &&
                           s.IsDeleted.Value.Equals(false)
                            select new DistrictsVM
                            {
                                DistrictId = d.DistrictId,
                                DistrictName = d.DistrictName,
                                VillageName = d.VillageName,
                                TownName = d.TownName,
                                ZoneId = z.ZoneId,
                                ZoneName = z.ZoneName,
                                CityId = c.CityId,
                                CityName = c.CityName,
                                StateId = s.StateId,
                                StateName = s.StateName,
                                Description = !string.IsNullOrEmpty(d.Description) ? d.Description : "",
                                StrPolygon = d.StrPolygon,
                                CreateEnDate = d.CreateEnDate,
                                CreateTime = d.CreateTime,
                                EditEnDate = d.EditEnDate,
                                EditTime = d.EditTime,
                                IsActivated = d.IsActivated,
                                IsDeleted = d.IsDeleted,
                                RemoveEnDate = d.RemoveEnDate,
                                RemoveTime = d.EditTime,
                                UserCreatorName = "",
                                UserIdCreator = d.UserIdCreator.Value,
                                UserIdEditor = d.UserIdEditor.Value,
                                UserIdRemover = d.UserIdRemover.Value,
                            }).AsQueryable();

                if (stateId.HasValue)
                    if (stateId.Value > 0)
                        list = list.Where(a => a.StateId.Equals(stateId.Value));


                if (cityId.HasValue)
                    if (cityId.Value > 0)
                        list = list.Where(a => a.CityId.Equals(cityId.Value));


                if (zoneId.HasValue)
                    if (zoneId.Value > 0)
                        list = list.Where(a => a.ZoneId.Equals(zoneId.Value));

                if (!string.IsNullOrEmpty(districtName))
                    list = list.Where(z => z.DistrictName.Contains(districtName));

                if (string.IsNullOrEmpty(jtSorting))
                {
                    listCount = list.Count();

                    if (listCount > jtPageSize)
                    {
                        districtsVMList = list.OrderByDescending(s => s.DistrictId)
                                 .Skip(jtStartIndex).Take(jtPageSize).ToList();
                    }
                    else
                    {
                        districtsVMList = list.OrderByDescending(s => s.DistrictId).ToList();
                    }
                }
                else
                {
                    listCount = list.Count();

                    if (listCount > jtPageSize)
                    {


                        if (string.IsNullOrEmpty(jtSorting))
                            districtsVMList = list.OrderByDescending(s => s.DistrictId)
                                     .Skip(jtStartIndex).Take(jtPageSize).ToList();
                        else
                            districtsVMList = list.Skip(jtStartIndex).Take(jtPageSize).ToList();
                    }
                    else
                    {
                        districtsVMList = list.ToList();
                    }
                }

            }
            catch (Exception exc)
            { }

            return districtsVMList;
        }


        public List<DistrictsVM> GetListOfDistrictsWithOutStrPolygon(
             int jtStartIndex,
             int jtPageSize,
             ref int listCount,
             long? stateId = null,
             long? cityId = null,
             long? zoneId = null,
             string? districtName = "",
             string jtSorting = null)
        {

            List<DistrictsVM> districtsVMList = new List<DistrictsVM>();

            try
            {
                var list = (from s in publicApiDb.States
                            join c in publicApiDb.Cities on s.StateId equals c.StateId
                            join z in publicApiDb.Zones on c.CityId equals z.CityId
                            join d in publicApiDb.Districts on z.ZoneId equals d.ZoneId
                            where s.IsActivated.Value.Equals(true) &&
                           s.IsDeleted.Value.Equals(false)
                            select new DistrictsVM
                            {
                                DistrictId = d.DistrictId,
                                DistrictName = d.DistrictName,
                                TownName = d.TownName,
                                VillageName = d.VillageName,
                                ZoneId = z.ZoneId,
                                ZoneName = z.ZoneName,
                                CityId = c.CityId,
                                CityName = c.CityName,
                                StateId = s.StateId,
                                StateName = s.StateName,
                                Description = !string.IsNullOrEmpty(d.Description) ? d.Description : "",
                                StrPolygon = "",
                                CreateEnDate = d.CreateEnDate,
                                CreateTime = d.CreateTime,
                                EditEnDate = d.EditEnDate,
                                EditTime = d.EditTime,
                                IsActivated = d.IsActivated,
                                IsDeleted = d.IsDeleted,
                                RemoveEnDate = d.RemoveEnDate,
                                RemoveTime = d.EditTime,
                                UserCreatorName = "",
                                UserIdCreator = d.UserIdCreator.Value,
                                UserIdEditor = d.UserIdEditor.Value,
                                UserIdRemover = d.UserIdRemover.Value,
                            }).AsQueryable();

                if (stateId.HasValue)
                    if (stateId.Value > 0)
                        list = list.Where(a => a.StateId.Equals(stateId.Value));


                if (cityId.HasValue)
                    if (cityId.Value > 0)
                        list = list.Where(a => a.CityId.Equals(cityId.Value));


                if (zoneId.HasValue)
                    if (zoneId.Value > 0)
                        list = list.Where(a => a.ZoneId.Equals(zoneId.Value));

                if (!string.IsNullOrEmpty(districtName))
                    list = list.Where(z => z.DistrictName.Contains(districtName));

                if (string.IsNullOrEmpty(jtSorting))
                {
                    listCount = list.Count();

                    if (listCount > jtPageSize)
                    {
                        districtsVMList = list.OrderByDescending(s => s.DistrictId)
                                 .Skip(jtStartIndex).Take(jtPageSize).ToList();
                    }
                    else
                    {
                        districtsVMList = list.OrderByDescending(s => s.DistrictId).ToList();
                    }
                }
                else
                {
                    listCount = list.Count();

                    if (listCount > jtPageSize)
                    {


                        if (string.IsNullOrEmpty(jtSorting))
                            districtsVMList = list.OrderByDescending(s => s.DistrictId)
                                     .Skip(jtStartIndex).Take(jtPageSize).ToList();
                        else
                            districtsVMList = list.Skip(jtStartIndex).Take(jtPageSize).ToList();
                    }
                    else
                    {
                        districtsVMList = list.ToList();
                    }
                }

            }
            catch (Exception exc)
            { }

            return districtsVMList;
        }


        public long AddToDistricts(DistrictsVM districtsVM)
        {
            try
            {
                Districts districts = _mapper.Map<DistrictsVM, Districts>(districtsVM);

                publicApiDb.Districts.Add(districts);
                publicApiDb.SaveChanges();

                return districts.DistrictId;


            }
            catch (Exception exc)
            { }

            return 0;
        }

        public DistrictsVM GetDistrictWithDistrictId(long districtId)
        {
            DistrictsVM districtsVM = new DistrictsVM();

            try
            {
                districtsVM = (from d in publicApiDb.Districts
                               join z in publicApiDb.Zones on d.ZoneId equals z.ZoneId
                               join c in publicApiDb.Cities on z.CityId equals c.CityId
                               join s in publicApiDb.States on c.StateId equals s.StateId
                               where d.DistrictId == districtId
                               select new DistrictsVM
                               {
                                   CityId = c.CityId,
                                   CityName = c.CityName,
                                   CreateEnDate = d.CreateEnDate.Value,
                                   CreateTime = d.CreateTime,
                                   DistrictId = d.DistrictId,
                                   DistrictName = d.DistrictName,
                                   EditEnDate = d.EditEnDate.HasValue ? d.EditEnDate.Value : (DateTime?)null,
                                   EditTime = d.EditTime,
                                   IsActivated = d.IsActivated.Value,
                                   IsDeleted = d.IsDeleted.Value,
                                   RemoveEnDate = d.EditEnDate.HasValue ? d.EditEnDate.Value : (DateTime?)null,
                                   RemoveTime = d.RemoveTime,
                                   StateId = s.StateId,
                                   StateName = s.StateName,
                                   StrPolygon = d.StrPolygon,
                                   TownName = d.TownName,
                                   UserIdCreator = d.UserIdCreator.Value,
                                   VillageName = d.VillageName,
                                   ZoneId = z.ZoneId,
                                   ZoneName = z.ZoneName
                               }).FirstOrDefault();
            }
            catch (Exception exc)
            { }

            return districtsVM;
        }

        public long UpdateDistricts(ref DistrictsVM districtsVM,
            List<long> childsUsersIds)
        {
            long districtId = districtsVM.DistrictId;
            bool? isActivated = districtsVM.IsActivated.HasValue ? districtsVM.IsActivated.Value : (bool?)true;
            bool? isDeleted = districtsVM.IsDeleted.HasValue ? districtsVM.IsDeleted.Value : (bool?)true;

            if (publicApiDb.Districts/*.Where(n => childsUsersIds.Contains(n.UserIdCreator.Value))*/.Where(x => x.DistrictId.Equals(districtId)).Any())
            {
                try
                {
                    Districts districts = (from a in publicApiDb.Districts
                                           where a.DistrictId == districtId
                                           select a).FirstOrDefault();


                    districts.DistrictName = districtsVM.DistrictName;
                    districts.VillageName = districtsVM.VillageName;
                    districts.TownName = districtsVM.TownName;
                    districts.StrPolygon = districtsVM.StrPolygon;
                    districts.ZoneId = districtsVM.ZoneId;
                    districts.Description = districtsVM.Description;

                    districts.EditEnDate = DateTime.Now;
                    districts.EditTime = PersianDate.TimeNow;
                    districts.UserIdEditor = districtsVM.UserIdEditor.Value;
                    districts.IsActivated = isActivated.Value;
                    districts.IsDeleted = isDeleted.Value;


                    publicApiDb.Entry<Districts>(districts).State = EntityState.Modified;
                    publicApiDb.SaveChanges();

                    districtsVM.UserIdCreator = districts.UserIdCreator.Value;

                    return districts.DistrictId;
                }
                catch (Exception ex)
                { }

            }
            return 0;

        }

        public bool ToggleActivationDistricts(long districtId,
           long userId,
           List<long> childsUsersIds)
        {
            try
            {
                var district = (from c in publicApiDb.Districts
                                where c.DistrictId == districtId &&
                                childsUsersIds.Contains(c.UserIdCreator.Value)
                                select c).FirstOrDefault();

                if (district != null)
                {
                    district.IsActivated = !district.IsActivated;
                    district.EditEnDate = DateTime.Now;
                    district.EditTime = PersianDate.TimeNow;
                    district.UserIdEditor = userId;

                    publicApiDb.Entry<Districts>(district).State = EntityState.Modified;
                    publicApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }


        public bool TemporaryDeleteDistricts(long districtId,
           long userId,
           List<long> childsUsersIds)
        {
            try
            {
                var district = (from c in publicApiDb.Districts
                                where c.DistrictId == districtId &&
                                childsUsersIds.Contains(c.UserIdCreator.Value)
                                select c).FirstOrDefault();

                if (district != null)
                {
                    district.IsDeleted = !district.IsDeleted;
                    district.EditEnDate = DateTime.Now;
                    district.EditTime = PersianDate.TimeNow;
                    district.UserIdEditor = userId;

                    publicApiDb.Entry<Districts>(district).State = EntityState.Modified;
                    publicApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool CompleteDeleteDistricts(long districtId,
           List<long> childsUsersIds)
        {
            try
            {
                var District = (from c in publicApiDb.Districts
                                where c.DistrictId == districtId &&
                                childsUsersIds.Contains(c.UserIdCreator.Value)
                                select c).FirstOrDefault();

                if (District != null)
                {
                    publicApiDb.Districts.Remove(District);
                    publicApiDb.SaveChanges();
                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        #endregion

        #region Methods For Work With DistrictFiles

        public List<DistrictFilesVM> GetListOfDistrictFiles(int jtStartIndex,
             int jtPageSize,
             ref int listCount,
             List<long> childsUsersIds,
             long? districtId = null,
             string districtFileTitle = "",
             string districtFileType = "",
             string jtSorting = null)
        {
            List<DistrictFilesVM> districtFilesVMList = new List<DistrictFilesVM>();

            var list = (from df in publicApiDb.DistrictFiles
                        where childsUsersIds.Contains(df.UserIdCreator.Value) &&
                        df.IsActivated.Value.Equals(true) &&
                        df.IsDeleted.Value.Equals(false) &&
                        df.DistrictId.Equals(districtId)
                        select df).AsQueryable();

            if (!string.IsNullOrEmpty(districtFileTitle))
                list = list.Where(a => a.DistrictFileTitle.Contains(districtFileTitle));

            if (!string.IsNullOrEmpty(districtFileType))
                list = list.Where(a => a.DistrictFileType.Contains(districtFileType));

            listCount = list.Count();

            try
            {
                if (string.IsNullOrEmpty(jtSorting))
                {
                    if (listCount > jtPageSize)
                    {

                        districtFilesVMList = _mapper.Map<List<DistrictFiles>, List<DistrictFilesVM>>(list.OrderByDescending(s => s.DistrictId)
                                 .Skip(jtStartIndex).Take(jtPageSize).ToList());
                    }
                    else
                    {

                        districtFilesVMList = _mapper.Map<List<DistrictFiles>, List<DistrictFilesVM>>(list.OrderByDescending(s => s.DistrictId).ToList());
                    }
                }
                else
                {
                    if (listCount > jtPageSize)
                    {
                        switch (jtSorting)
                        {
                            case "DistrictFileTitle ASC":
                                list = list.OrderBy(l => l.DistrictFileTitle);
                                break;
                            case "DistrictFileTitle DESC":
                                list = list.OrderByDescending(l => l.DistrictFileTitle);
                                break;
                        }

                        if (string.IsNullOrEmpty(jtSorting))
                            districtFilesVMList = _mapper.Map<List<DistrictFiles>, List<DistrictFilesVM>>(list.OrderByDescending(s => s.DistrictId)
                                     .Skip(jtStartIndex).Take(jtPageSize).ToList());
                        else
                            districtFilesVMList = _mapper.Map<List<DistrictFiles>, List<DistrictFilesVM>>(list.Skip(jtStartIndex).Take(jtPageSize).ToList());
                    }
                    else
                    {
                        districtFilesVMList = _mapper.Map<List<DistrictFiles>, List<DistrictFilesVM>>(list.ToList());
                    }
                }

            }
            catch (Exception exc)
            { }
            return districtFilesVMList;
        }



        public bool AddToDistrictFiles(List<DistrictFilesVM> districtFilesVMList)
        {
            try
            {
                if (districtFilesVMList != null)
                    if (districtFilesVMList.Count > 0)
                    {
                        var districtFilesList = _mapper.Map<List<DistrictFilesVM>, List<DistrictFiles>>(districtFilesVMList);

                        publicApiDb.DistrictFiles.AddRange(districtFilesList);
                        publicApiDb.SaveChanges();

                        return true;
                    }
            }
            catch (Exception exc)
            {
            }
            return false;
        }

        public DistrictFilesVM GetDistrictFileWithDistrictFileId(int districtFileId,
            List<long> childsUsersIds)
        {
            DistrictFilesVM districtFilesVM = new DistrictFilesVM();

            try
            {
                districtFilesVM = _mapper.Map<DistrictFiles,
                    DistrictFilesVM>(publicApiDb.DistrictFiles
                    .Where(p => childsUsersIds.Contains(p.UserIdCreator.Value))
                    .Where(e => e.DistrictFileId.Equals(districtFileId)).FirstOrDefault());
            }
            catch (Exception exc)
            { }

            return districtFilesVM;
        }

        public bool UpdateDistrictFiles(ref DistrictFilesVM districtFilesVM,
            List<long> childsUsersIds)
        {
            int districtFileId = districtFilesVM.DistrictFileId;
            bool? isActivated = districtFilesVM.IsActivated.HasValue ? districtFilesVM.IsActivated.Value : (bool?)true;
            bool? isDeleted = districtFilesVM.IsDeleted.HasValue ? districtFilesVM.IsDeleted.Value : (bool?)true;

            try
            {
                DistrictFiles districtFiles = (from c in publicApiDb.DistrictFiles
                                               where c.DistrictFileId == districtFileId
                                               select c).FirstOrDefault();

                districtFiles.DistrictId = districtFilesVM.DistrictId;
                districtFiles.DistrictFileExt = districtFilesVM.DistrictFileExt;
                districtFiles.DistrictFilePath = districtFilesVM.DistrictFilePath;
                districtFiles.DistrictFileTitle = districtFilesVM.DistrictFileTitle;
                districtFiles.DistrictFileType = districtFilesVM.DistrictFileType;

                districtFiles.EditEnDate = DateTime.Now;
                districtFiles.EditTime = PersianDate.TimeNow;
                districtFiles.UserIdEditor = districtFiles.UserIdEditor;
                districtFiles.IsActivated = isActivated.Value;
                districtFiles.IsDeleted = isDeleted.Value;




                publicApiDb.Entry<DistrictFiles>(districtFiles).State = EntityState.Modified;
                publicApiDb.SaveChanges();

                districtFilesVM.UserIdCreator = districtFiles.UserIdCreator.Value;

                return true;
            }
            catch (Exception exc)
            {
            }

            return false;
        }

        public bool ToggleActivationDistrictFiles(int districtFileId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var districtFiles = (from c in publicApiDb.DistrictFiles
                                     where c.DistrictFileId == districtFileId &&
                                     childsUsersIds.Contains(c.UserIdCreator.Value)
                                     select c).FirstOrDefault();

                if (districtFiles != null)
                {
                    districtFiles.IsActivated = !districtFiles.IsActivated;
                    districtFiles.EditEnDate = DateTime.Now;
                    districtFiles.EditTime = PersianDate.TimeNow;
                    districtFiles.UserIdEditor = userId;

                    publicApiDb.Entry<DistrictFiles>(districtFiles).State = EntityState.Modified;
                    publicApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool TemporaryDeleteDistrictFiles(int districtFileId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var districtFiles = (from c in publicApiDb.DistrictFiles
                                     where c.DistrictFileId == districtFileId &&
                                     childsUsersIds.Contains(c.UserIdCreator.Value)
                                     select c).FirstOrDefault();

                if (districtFiles != null)
                {
                    districtFiles.IsDeleted = !districtFiles.IsDeleted;
                    districtFiles.EditEnDate = DateTime.Now;
                    districtFiles.EditTime = PersianDate.TimeNow;
                    districtFiles.UserIdEditor = userId;

                    publicApiDb.Entry<DistrictFiles>(districtFiles).State = EntityState.Modified;
                    publicApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool CompleteDeleteDistrictFiles(int districtFileId,
            List<long> childsUsersIds)
        {
            try
            {
                var districtFiles = (from c in publicApiDb.DistrictFiles
                                     where c.DistrictFileId == districtFileId &&
                                     childsUsersIds.Contains(c.UserIdCreator.Value)
                                     select c).FirstOrDefault();

                if (districtFiles != null)
                {
                    publicApiDb.DistrictFiles.Remove(districtFiles);
                    publicApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        #endregion

        #region Methods For Work With ElementTypesPublic
        public List<ElementTypesVM> GetAllElementTypesList()
        {
            try
            {
                var documentTypes = publicApiDb.ElementTypes.Where(c => c.IsActivated.Value.Equals(true) && c.IsDeleted.Value.Equals(false)).AsQueryable();

                return _mapper.Map<List<ElementTypes>, List<ElementTypesVM>>(documentTypes.ToList());
            }
            catch (Exception exc)
            { }
            return new List<ElementTypesVM>();
        }
        #endregion

        #region Methods For Work With FormUsages

        public List<FormUsagesVM> GetAllFormUsagesList()
        {
            List<FormUsagesVM> formUsagesVMList = new List<FormUsagesVM>();

            try
            {
                var list = publicApiDb.FormUsages.Where(c => c.IsActivated.Value.Equals(true) && c.IsDeleted.Value.Equals(false)).AsQueryable();

                formUsagesVMList = _mapper.Map<List<FormUsages>, List<FormUsagesVM>>(list.ToList());
            }
            catch (Exception exc)
            { }

            return formUsagesVMList;
        }

        #endregion

        #region Methods For Work With FormUsages

        public List<FormUsagesVM> GetAllFormUsagesList(
           string? formUsageTitle = "")
        {
            List<FormUsagesVM> formUsagesVMList = new List<FormUsagesVM>();

            try
            {
                var list = (from u in publicApiDb.FormUsages
                            where u.IsActivated.Value.Equals(true) &&
                            u.IsDeleted.Value.Equals(false)
                            select new FormUsagesVM
                            {
                                FormUsageId = u.FormUsageId,
                                FormUsageTitle = u.FormUsageTitle,
                                CreateEnDate = u.CreateEnDate,
                                CreateTime = u.CreateTime,
                                EditEnDate = u.EditEnDate,
                                EditTime = u.EditTime,
                                IsActivated = u.IsActivated,
                                IsDeleted = u.IsDeleted,
                                RemoveEnDate = u.RemoveEnDate,
                                RemoveTime = u.EditTime,
                                UserCreatorName = "",
                                UserIdCreator = u.UserIdCreator.Value,
                                UserIdEditor = u.UserIdEditor.Value,
                                UserIdRemover = u.UserIdRemover.Value,
                            }).AsQueryable();


                if (!string.IsNullOrEmpty(formUsageTitle))
                    list = list.Where(z => z.FormUsageTitle.Contains(formUsageTitle));

                formUsagesVMList = list.OrderByDescending(f => f.FormUsageId).ToList();


            }
            catch (Exception exc)
            { }


            return formUsagesVMList;
        }



        public List<FormUsagesVM> GetListOfFormUsages(int jtStartIndex,
             int jtPageSize,
             ref int listCount,
             string? formUsageTitle = "",
             string jtSorting = null)
        {
            List<FormUsagesVM> formUsagesVMList = new List<FormUsagesVM>();

            try
            {
                var list = (from u in publicApiDb.FormUsages
                            where u.IsActivated.Value.Equals(true) &&
                            u.IsDeleted.Value.Equals(false)
                            select new FormUsagesVM
                            {
                                FormUsageId = u.FormUsageId,
                                FormUsageTitle = u.FormUsageTitle,
                                CreateEnDate = u.CreateEnDate,
                                CreateTime = u.CreateTime,
                                EditEnDate = u.EditEnDate,
                                EditTime = u.EditTime,
                                IsActivated = u.IsActivated,
                                IsDeleted = u.IsDeleted,
                                RemoveEnDate = u.RemoveEnDate,
                                RemoveTime = u.EditTime,
                                UserCreatorName = "",
                                UserIdCreator = u.UserIdCreator.Value,
                                UserIdEditor = u.UserIdEditor.Value,
                                UserIdRemover = u.UserIdRemover.Value,
                            }).AsQueryable();



                if (!string.IsNullOrEmpty(formUsageTitle))
                    list = list.Where(z => z.FormUsageTitle.Contains(formUsageTitle));

                if (string.IsNullOrEmpty(jtSorting))
                {
                    listCount = list.Count();

                    if (listCount > jtPageSize)
                    {
                        formUsagesVMList = list.OrderByDescending(s => s.FormUsageId)
                                 .Skip(jtStartIndex).Take(jtPageSize).ToList();
                    }
                    else
                    {
                        formUsagesVMList = list.OrderByDescending(s => s.FormUsageId).ToList();
                    }
                }
                else
                {
                    listCount = list.Count();

                    if (listCount > jtPageSize)
                    {


                        if (string.IsNullOrEmpty(jtSorting))
                            formUsagesVMList = list.OrderByDescending(s => s.FormUsageId)
                                     .Skip(jtStartIndex).Take(jtPageSize).ToList();
                        else
                            formUsagesVMList = list.Skip(jtStartIndex).Take(jtPageSize).ToList();
                    }
                    else
                    {
                        formUsagesVMList = list.ToList();
                    }
                }

            }
            catch (Exception exc)
            { }

            return formUsagesVMList;
        }


        public int AddToFormUsages(FormUsagesVM formUsagesVM)
        {
            try
            {
                FormUsages formUsages = _mapper.Map<FormUsagesVM, FormUsages>(formUsagesVM);

                publicApiDb.FormUsages.Add(formUsages);
                publicApiDb.SaveChanges();

                return formUsages.FormUsageId;


            }
            catch (Exception exc)
            { }

            return 0;
        }


        public FormUsagesVM GetFormUsageWithFormUsageId(int formUsageId)
        {
            FormUsagesVM formUsagesVM = new FormUsagesVM();

            try
            {
                formUsagesVM = _mapper.Map<FormUsages,
                    FormUsagesVM>(publicApiDb.FormUsages
                    .Where(e => e.FormUsageId.Equals(formUsageId)).FirstOrDefault());
            }
            catch (Exception exc)
            { }

            return formUsagesVM;
        }


        public int UpdateFormUsages(ref FormUsagesVM formUsagesVM,
           List<long> childsUsersIds)
        {
            int formUsageId = formUsagesVM.FormUsageId;
            bool? isActivated = formUsagesVM.IsActivated.HasValue ? formUsagesVM.IsActivated.Value : (bool?)true;
            bool? isDeleted = formUsagesVM.IsDeleted.HasValue ? formUsagesVM.IsDeleted.Value : (bool?)true;

            if (publicApiDb.FormUsages.Where(n => childsUsersIds.Contains(n.UserIdCreator.Value)).Where(x => x.FormUsageId.Equals(formUsageId)).Any())
            {
                try
                {
                    var usage = (from a in publicApiDb.FormUsages
                                 where a.FormUsageId == formUsageId
                                 select a).FirstOrDefault();


                    usage.FormUsageTitle = formUsagesVM.FormUsageTitle;


                    usage.EditEnDate = DateTime.Now;
                    usage.EditTime = PersianDate.TimeNow;
                    usage.UserIdEditor = usage.UserIdEditor.Value;
                    usage.IsActivated = isActivated.Value;
                    usage.IsDeleted = isDeleted.Value;


                    publicApiDb.Entry<FormUsages>(usage).State = EntityState.Modified;
                    publicApiDb.SaveChanges();

                    formUsagesVM.UserIdCreator = usage.UserIdCreator.Value;

                    return usage.FormUsageId;
                }
                catch (Exception ex)
                { }

            }
            return 0;

        }

        public bool ToggleActivationFormUsages(int formUsageId,
           long userId,
           List<long> childsUsersIds)
        {
            try
            {
                var usage = (from c in publicApiDb.FormUsages
                             where c.FormUsageId == formUsageId &&
                             childsUsersIds.Contains(c.UserIdCreator.Value)
                             select c).FirstOrDefault();

                if (usage != null)
                {
                    usage.IsActivated = !usage.IsActivated;
                    usage.EditEnDate = DateTime.Now;
                    usage.EditTime = PersianDate.TimeNow;
                    usage.UserIdEditor = userId;

                    publicApiDb.Entry<FormUsages>(usage).State = EntityState.Modified;
                    publicApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }


        public bool TemporaryDeleteFormUsages(int formUsageId,
           long userId,
           List<long> childsUsersIds)
        {
            try
            {
                var usage = (from c in publicApiDb.FormUsages
                             where c.FormUsageId == formUsageId &&
                             childsUsersIds.Contains(c.UserIdCreator.Value)
                             select c).FirstOrDefault();

                if (usage != null)
                {
                    usage.IsDeleted = !usage.IsDeleted;
                    usage.EditEnDate = DateTime.Now;
                    usage.EditTime = PersianDate.TimeNow;
                    usage.UserIdEditor = userId;

                    publicApiDb.Entry<FormUsages>(usage).State = EntityState.Modified;
                    publicApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool CompleteDeleteFormUsages(int formUsageId,
           List<long> childsUsersIds)
        {
            try
            {
                var usage = (from c in publicApiDb.FormUsages
                             where c.FormUsageId == formUsageId &&
                             childsUsersIds.Contains(c.UserIdCreator.Value)
                             select c).FirstOrDefault();

                if (usage != null)
                {
                    publicApiDb.FormUsages.Remove(usage);
                    publicApiDb.SaveChanges();
                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }
        #endregion

        #region Methods For Work With GuildCategories
        public List<GuildCategoriesVM> GetAllGuildCategoriesList(
            long? guildCategoryId = null)
        {
            List<GuildCategoriesVM> guildCategoriesList = new List<GuildCategoriesVM>();

            try
            {
                var list = (from c in publicApiDb.GuildCategories
                            where c.IsActivated.Value.Equals(true) &&
                            c.IsDeleted.Value.Equals(false)
                            select new GuildCategoriesVM
                            {
                                GuildCategoryId = c.GuildCategoryId,
                                GuildCategoryName = c.GuildCategoryName,
                                CreateEnDate = c.CreateEnDate,
                                CreateTime = c.CreateTime,
                                EditEnDate = c.EditEnDate,
                                EditTime = c.EditTime,
                                IsActivated = c.IsActivated,
                                IsDeleted = c.IsDeleted,
                                RemoveEnDate = c.RemoveEnDate,
                                RemoveTime = c.EditTime,
                                UserCreatorName = "",
                                UserIdCreator = c.UserIdCreator.Value,
                                UserIdEditor = c.UserIdEditor.Value,
                                UserIdRemover = c.UserIdRemover.Value,
                            }).AsQueryable();



                if (guildCategoryId.HasValue)
                    if (guildCategoryId.Value > 0)
                        list = list.Where(a => a.GuildCategoryId.Equals(guildCategoryId.Value));


                guildCategoriesList = list.OrderByDescending(f => f.GuildCategoryId).ToList();


            }
            catch (Exception exc)
            { }


            return guildCategoriesList;
        }

        #endregion

        #region Methods For Work with Indices

        public List<IndicesVM> GetAllIndicesList(int? IndiceId)
        {
            List<IndicesVM> result = new List<IndicesVM>();
            try
            {
                var list = publicApiDb.Indices.Where(c => c.IsActivated.Value.Equals(true) && c.IsDeleted.Value.Equals(false)).Select(a => new IndicesVM
                {
                    CreateEnDate = a.CreateEnDate,
                    CreateTime = a.CreateTime,
                    EditEnDate = a.EditEnDate,
                    EditTime = a.EditTime,
                    IndiceId = a.IndiceId,
                    IsActivated = a.IsActivated,
                    IsDeleted = a.IsDeleted,
                    Name = a.Name,
                    RemoveEnDate = a.RemoveEnDate,
                    RemoveTime = a.RemoveTime,
                    UserCreatorName = a.UserCreatorName,
                    UserIdCreator = a.UserIdCreator,
                    UserIdEditor = a.UserIdEditor
                }).AsQueryable();
                if (IndiceId.HasValue && IndiceId > 0)
                    list.Where(a => a.IndiceId == IndiceId);
                result = list.ToList();

            }
            catch (Exception exx)
            {
                throw;
            }
            return result;

        }

        public List<IndicesVM> GetListOfIndices(int jtStartIndex, int jtPageSize, ref int listCount, List<long> childsUsersIds, string? jtSorting)
        {
            List<IndicesVM> result = new List<IndicesVM>();
            var list = publicApiDb.Indices.Where(c => c.IsActivated.Value.Equals(true) && c.IsDeleted.Value.Equals(false)).Select(a => new IndicesVM
            {
                CreateEnDate = a.CreateEnDate,
                CreateTime = a.CreateTime,
                EditEnDate = a.EditEnDate,
                EditTime = a.EditTime,
                IndiceId = a.IndiceId,
                IsActivated = a.IsActivated,
                IsDeleted = a.IsDeleted,
                Name = a.Name,
                RemoveEnDate = a.RemoveEnDate,
                RemoveTime = a.RemoveTime,
                UserCreatorName = a.UserCreatorName,
                UserIdCreator = a.UserIdCreator,
                UserIdEditor = a.UserIdEditor
            }).Skip(jtStartIndex).Take(jtPageSize).AsQueryable();
            listCount = publicApiDb.Indices.AsQueryable().Count();// list.Count();
            try
            {
                switch (jtSorting)
                {
                    case "Name ASC":
                        list = list.OrderBy(l => l.Name);
                        break;
                    case "Name DESC":
                        list = list.OrderByDescending(l => l.Name);
                        break;
                    default:
                        list = list.OrderBy(l => l.IndiceId);
                        break;
                }
                result = list.ToList();
            }
            catch (Exception exc)
            {
                throw;
            }
            return result;
        }

        public int AddToIndices(IndicesVM indicesVM)
        {
            Indices _Indices = _mapper.Map<IndicesVM, Indices>(indicesVM);
            publicApiDb.Indices.Add(_Indices);
            publicApiDb.SaveChanges();
            return _Indices.IndiceId;

        }

        public int UpdateIndices(IndicesVM indicesVM, List<long> childsUsersIds)
        {
            bool? isActivated = indicesVM.IsActivated.HasValue ? indicesVM.IsActivated.Value : (bool?)true;
            bool? isDeleted = indicesVM.IsDeleted.HasValue ? indicesVM.IsDeleted.Value : (bool?)true;
            if (publicApiDb.Indices.Where(n => childsUsersIds.Contains(n.UserIdCreator.Value) && n.IndiceId == indicesVM.IndiceId).Any())
            {
                try
                {
                    Indices? _Indices = publicApiDb.Indices.Find(indicesVM.IndiceId);

                    ArgumentNullException.ThrowIfNull(_Indices);

                    _Indices.Name = indicesVM.Name;
                    _Indices.EditEnDate = DateTime.Now;
                    _Indices.EditTime = PersianDate.TimeNow;
                    _Indices.UserIdEditor = indicesVM.UserIdEditor.Value;
                    _Indices.IsActivated = isActivated.Value;
                    _Indices.IsDeleted = isDeleted.Value;

                    publicApiDb.SaveChanges();
                    return _Indices.IndiceId;
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
            return 0;
        }

        public bool ToggleActivationIndices(int IndiceId, long userId, List<long> childsUsersIds)
        {
            try
            {
                var Indices = (from c in publicApiDb.Indices
                               where c.IndiceId == IndiceId &&
                               childsUsersIds.Contains(c.UserIdCreator.Value)
                               select c).FirstOrDefault();

                ArgumentNullException.ThrowIfNull(Indices);
                Indices.IsActivated = !Indices.IsActivated;
                Indices.EditEnDate = DateTime.Now;
                Indices.EditTime = PersianDate.TimeNow;
                Indices.UserIdEditor = userId;
                //teniacoApiDb.Entry<Funds>(Funds).State = EntityState.Modified;
                publicApiDb.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool TemporaryDeleteIndices(int IndiceId, long userId, List<long> childsUsersIds)
        {
            try
            {
                var Indices = (from c in publicApiDb.Indices
                               where c.IndiceId == IndiceId &&
                               childsUsersIds.Contains(c.UserIdCreator.Value)
                               select c).FirstOrDefault();
                ArgumentNullException.ThrowIfNull(Indices);

                Indices.IsDeleted = !Indices.IsDeleted;
                Indices.EditEnDate = DateTime.Now;
                Indices.EditTime = PersianDate.TimeNow;
                Indices.UserIdEditor = userId;
                /// teniacoApiDb.Entry<Funds>(_funds).State = EntityState.Modified;
                publicApiDb.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public bool CompleteDeleteIndices(int IndiceId, List<long> childsUsersIds)
        {
            try
            {
                var Indices = (from c in publicApiDb.Indices
                               where c.IndiceId == IndiceId &&
                               childsUsersIds.Contains(c.UserIdCreator.Value)
                               select c).FirstOrDefault();
                ArgumentNullException.ThrowIfNull(Indices);
                publicApiDb.Indices.Remove(Indices);
                publicApiDb.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Methods For Work With Persons

        public List<PersonsVM> GetListOfPersons(
            List<long> childsUsersIds,
            int jtStartIndex,
            int jtPageSize,
            ref int listCount,
            string searchText/*,
           List<long> childsUsersIds,
           string Lang = null,
           string jtSorting = null
           /*long userId = 0*/)
        {
            List<PersonsVM> personsVMList = new List<PersonsVM>();

            var list = publicApiDb.Persons
                    .Where(c => childsUsersIds.Contains(c.UserIdCreator.Value) && c.IsActivated.Value.Equals(true) && c.IsDeleted.Value.Equals(false))
                    .AsQueryable();

            //if (!string.IsNullOrEmpty(Lang))
            //{
            //    list = list.Where(x => x.Lang.Equals(Lang));
            //}

            if (!string.IsNullOrEmpty(searchText))
                list = list.Where(a => a.Name.Contains(searchText) || a.Family.Contains(searchText) || a.Phone.Contains(searchText));

            try
            {
                listCount = list.Count();

                if (listCount > jtPageSize)
                {
                    personsVMList = _mapper.Map<List<Persons>, List<PersonsVM>>(list.OrderByDescending(s => s.PersonId)
                             .Skip(jtStartIndex).Take(jtPageSize).ToList());

                }
                else
                {
                    personsVMList = _mapper.Map<List<Persons>,
                        List<PersonsVM>>(list.OrderByDescending(s => s.PersonId).ToList());
                }
            }
            catch (Exception exc)
            { }
            return personsVMList;
        }

        public List<PersonsVM> GetBuyersList(
    List<long> childsUsersIds)
        {
            List<PersonsVM> personsVM = new List<PersonsVM>();

            try
            {
                var list = publicApiDb.Persons.//Where(p => childsUsersIds.Contains(p.UserIdCreator.Value)).
                    Where(f => f.PersonTypeId==3).AsQueryable();

                if (childsUsersIds != null)
                {
                    if (childsUsersIds.Count > 0)
                    {
                        list = list.Where(p => childsUsersIds.Contains(p.UserIdCreator.Value));
                    }
                    if (childsUsersIds.Count == 0)
                        if (childsUsersIds.FirstOrDefault() > 0)
                        {
                            list = list.Where(p => childsUsersIds.Contains(p.UserIdCreator.Value));
                        }
                }

                personsVM = _mapper.Map<List<Persons>, List<PersonsVM>>(list.OrderByDescending(f => f.PersonId).ToList());
            }
            catch (Exception exc)
            { }

            return personsVM;
        }

        public List<PersonsVM> GetAllPersonsList(
            List<long> childsUsersIds,
            ref int listCount,
            string searchText)
        {
            List<PersonsVM> personsVM = new List<PersonsVM>();

            try
            {
                var list = publicApiDb.Persons.//Where(p => childsUsersIds.Contains(p.UserIdCreator.Value)).
                    Where(f => f.IsActivated.Equals(true) &&
                    f.IsDeleted.Equals(false) && f.PersonTypeId.Equals(2)).AsQueryable();

                if (childsUsersIds != null)
                {
                    if (childsUsersIds.Count > 0)
                    {
                        list = list.Where(p => childsUsersIds.Contains(p.UserIdCreator.Value));
                    }
                    if (childsUsersIds.Count == 0)
                        if (childsUsersIds.FirstOrDefault() > 0)
                        {
                            list = list.Where(p => childsUsersIds.Contains(p.UserIdCreator.Value));
                        }
                }

                if (!string.IsNullOrEmpty(searchText))
                    list = list.Where(a => a.Name.Contains(searchText) || a.Family.Contains(searchText) || a.Phone.Contains(searchText));

                listCount = list.Count();

                personsVM = _mapper.Map<List<Persons>, List<PersonsVM>>(list.OrderByDescending(f => f.PersonId).ToList());
            }
            catch (Exception exc)
            { }

            return personsVM;
        }

        public long AddToPersons(PersonsVM personsVM, List<long> childsUsersIds)
        {
            try
            {
                if (!publicApiDb.Persons.Where(p => /*p.Name.Equals(personsVM.Name) && 
                    p.Family.Equals(personsVM.Family) && */
                    p.Phone.Equals(personsVM.Phone) &&
                    childsUsersIds.Contains(p.UserIdCreator.Value)).Any())
                {
                    Persons persons = _mapper.Map<PersonsVM, Persons>(personsVM);
                    publicApiDb.Persons.Add(persons);
                    publicApiDb.SaveChanges();
                    return persons.PersonId;
                }
                else
                    return -1;
                //}
            }
            catch (Exception exc)
            { }
            return 0;
        }

        public long UpdatePersons(ref PersonsVM personsVM,
            //long userId,
            List<long> childsUsersIds)
        {
            try
            {
                long personId = personsVM.PersonId;
                string name = personsVM.Name;
                string family = personsVM.Family;
                string phone = personsVM.Phone;

                if (publicApiDb.Persons.Where(p => /*p.Family.Equals(family) ||*/
                    p.Phone.Equals(phone) &&
                    childsUsersIds.Contains(p.UserIdCreator.Value) &&
                    !p.PersonId.Equals(personId)).Any())
                {
                    return -1;
                }

                Persons person = (from c in publicApiDb.Persons
                                  where c.PersonId == personId
                                  select c).FirstOrDefault();

                person.EditEnDate = DateTime.Now;
                person.EditTime = PersianDate.TimeNow;
                person.UserIdEditor = personsVM.UserIdEditor;
                person.Name = name;
                person.Family = family;
                person.BirthDateTimeEn = personsVM.BirthDateTimeEn.HasValue ? personsVM.BirthDateTimeEn.Value : (DateTime?)null;
                person.Sexuality = personsVM.Sexuality;
                person.NationalCode = personsVM.NationalCode;
                person.Phone = personsVM.Phone;
                person.Mobail = personsVM.Mobail;
                person.PostalCode = personsVM.PostalCode;
                person.CountryId = personsVM.CountryId;
                person.StateId = personsVM.StateId;
                person.CityId = personsVM.CityId;
                person.IsActivated = personsVM.IsActivated;
                person.IsDeleted = personsVM.IsDeleted;
                publicApiDb.Entry<Persons>(person).State = EntityState.Modified;
                publicApiDb.SaveChanges();

                personsVM.UserIdCreator = person.UserIdCreator.Value;

                #region rewrite inside module

                //long userIdCreator = PersonsVM.UserIdCreator.Value;
                //if (PersonsVM.UserIdCreator.HasValue)
                //{
                //    var user = publicApiDb.Users.FirstOrDefault(u => u.UserId.Equals(userIdCreator));
                //    var userDetails = publicApiDb.UsersProfile.FirstOrDefault(up => up.UserId.Equals(userIdCreator));
                //    PersonsVM.UserCreatorName = user.UserName;

                //    if (!string.IsNullOrEmpty(userDetails.Name))
                //        PersonsVM.UserCreatorName += " - " + userDetails.Name;

                //    if (!string.IsNullOrEmpty(userDetails.Family))
                //        PersonsVM.UserCreatorName += " - " + userDetails.Family;
                //}

                #endregion

                return person.PersonId;
                //}
            }
            catch (Exception exc)
            { }
            return 0;
        }

        public bool ToggleActivationPersons(long personId, long userId, List<long> childsUsersIds)
        {
            try
            {
                //List<long> childsUsersIds = new List<long>();
                //childsUsersIds = GetChildUserIds(ref childsUsersIds, userId).Distinct().ToList();
                var person = (from c in publicApiDb.Persons
                              where c.PersonId == personId &&
                              childsUsersIds.Contains(c.UserIdCreator.Value)
                              select c).FirstOrDefault();

                if (person != null)
                {
                    person.IsActivated = !person.IsActivated;
                    person.EditEnDate = DateTime.Now;
                    person.EditTime = PersianDate.TimeNow;
                    person.UserIdEditor = userId;
                    publicApiDb.Entry<Persons>(person).State = EntityState.Modified;
                    publicApiDb.SaveChanges();
                    return true;
                }
            }
            catch (Exception exc)
            { }
            return false;
        }

        public bool TemporaryDeletePersons(long personId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                //List<long> childsUsersIds = new List<long>();
                //childsUsersIds = GetChildUserIds(ref childsUsersIds, userId).Distinct().ToList();
                var person = (from c in publicApiDb.Persons
                              where c.PersonId == personId &&
                              childsUsersIds.Contains(c.UserIdCreator.Value)
                              select c).FirstOrDefault();

                if (person != null)
                {
                    person.IsDeleted = !person.IsDeleted;
                    person.RemoveEnDate = DateTime.Now;
                    person.RemoveTime = PersianDate.TimeNow;
                    person.UserIdRemover = userId;
                    publicApiDb.Entry<Persons>(person).State = EntityState.Modified;
                    publicApiDb.SaveChanges();
                    return true;
                }
            }
            catch (Exception exc)
            { }
            return false;
        }

        public bool CompleteDeletePersons(long personId,
            //long userId,
            List<long> childsUsersIds)
        {
            try
            {
                //List<long> childsUsersIds = new List<long>();
                //childsUsersIds = GetChildUserIds(ref childsUsersIds, userId).Distinct().ToList();
                var person = (from c in publicApiDb.Persons
                              where c.PersonId == personId &&
                              childsUsersIds.Contains(c.UserIdCreator.Value)
                              select c).FirstOrDefault();

                if (person != null)
                {
                    publicApiDb.Persons.Remove(person);
                    publicApiDb.SaveChanges();
                    return true;
                }
            }
            catch (Exception exc)
            { }
            return false;
        }

        public PersonsVM GetPersonWithMobileNumber(string mobileNumber,
           List<long> childsUsersIds)
        {
            PersonsVM personsVM = new PersonsVM();

            try
            {

                personsVM = _mapper.Map<Entities.Persons,
                    PersonsVM>(publicApiDb.Persons
                    .Where(e => e.Mobail.Equals(mobileNumber)).FirstOrDefault());

                //States

                if (personsVM.StateId != null)
                {
                    var stateIds = publicApiDb.States.Select(s => s.StateId).ToList();
                    var states = publicApiDb.States.Where(p => stateIds.Contains(p.StateId)).ToList();
                    if (states.Where(p => p.StateId.Equals(personsVM.StateId)).Any())
                    {
                        var state = states.Where(p => p.StateId.Equals(personsVM.StateId)).FirstOrDefault();
                        personsVM.StateId = state.StateId;
                        personsVM.StateName = state.StateName;
                    }
                }

                //Cities

                if (personsVM.CityId != null)
                {
                    var cityIds = publicApiDb.Cities.Select(s => s.CityId).ToList();
                    var cities = publicApiDb.Cities.Where(p => cityIds.Contains(p.CityId)).ToList();

                    if (cities.Where(c => c.CityId.Equals(personsVM.CityId)).Any())
                    {
                        var city = cities.Where(c => c.CityId.Equals(personsVM.CityId)).FirstOrDefault();
                        personsVM.CityId = city.CityId;
                        personsVM.CityName = city.CityName;
                    }
                }



            }
            catch (Exception exc)
            { }

            return personsVM;
        }

        public PersonsVM GetPersonWithUserId(long userId,
          List<long> childsUsersIds)
        {
            PersonsVM personsVM = new PersonsVM();

            try
            {

                personsVM = _mapper.Map<Entities.Persons,
                    PersonsVM>(publicApiDb.Persons
                    .Where(e => e.UserIdCreator.Equals(userId)).FirstOrDefault());

                //States

                if (personsVM.StateId != null)
                {
                    var stateIds = publicApiDb.States.Select(s => s.StateId).ToList();
                    var states = publicApiDb.States.Where(p => stateIds.Contains(p.StateId)).ToList();
                    if (states.Where(p => p.StateId.Equals(personsVM.StateId)).Any())
                    {
                        var state = states.Where(p => p.StateId.Equals(personsVM.StateId)).FirstOrDefault();
                        personsVM.StateId = state.StateId;
                        personsVM.StateName = state.StateName;
                    }
                }

                //Cities

                if (personsVM.CityId != null)
                {
                    var cityIds = publicApiDb.Cities.Select(s => s.CityId).ToList();
                    var cities = publicApiDb.Cities.Where(p => cityIds.Contains(p.CityId)).ToList();

                    if (cities.Where(c => c.CityId.Equals(personsVM.CityId)).Any())
                    {
                        var city = cities.Where(c => c.CityId.Equals(personsVM.CityId)).FirstOrDefault();
                        personsVM.CityId = city.CityId;
                        personsVM.CityName = city.CityName;
                    }
                }



            }
            catch (Exception exc)
            { }

            return personsVM;
        }

        public List<PersonsVM> GetAllPersonsListWithUsers(
            List<long> childsUsersIds,
            IConsoleBusiness consoleBusiness)
        {
            List<PersonsVM> personsVM = new List<PersonsVM>();

            try
            {
                var userNames = consoleBusiness.CmsDb.Users.Select(u => u.UserName).ToList();

                var userIds = consoleBusiness.CmsDb.Users.Select(u => u.UserId).ToList();

                if (publicApiDb.Persons.Where(p => userIds.Contains(p.UserIdCreator.Value)).Any())
                {
                    var personsList = publicApiDb.Persons.Where(p => userIds.Contains(p.UserIdCreator.Value) && !userNames.Contains(p.Phone)).ToList();

                    personsVM = _mapper.Map<List<Persons>, List<PersonsVM>>(personsList.OrderByDescending(f => f.PersonId).ToList());

                }

                return personsVM;
            }
            catch (Exception exc)
            { }

            return personsVM;
        }

        #endregion

        #region Methods For Work With PersonTypes

        public List<PersonTypesVM> GetAllPersonTypesList()
        {
            List<PersonTypesVM> personTypesVMList = new List<PersonTypesVM>();

            try
            {
                var list = publicApiDb.PersonTypes.Where(c => c.IsActivated.Value.Equals(true) && c.IsDeleted.Value.Equals(false)).AsQueryable();

                personTypesVMList = _mapper.Map<List<PersonTypes>, List<PersonTypesVM>>(list.ToList());
            }
            catch (Exception exc)
            { }

            return personTypesVMList;
        }

        #endregion

        #region Methods For Work with PricesListIndices

        public List<PricesListIndicesVM> GetAllPricesListIndicesList(int? PricesListIndicesId)
        {

            List<PricesListIndicesVM> result = new List<PricesListIndicesVM>();
            try
            {
                var list = publicApiDb.PricesListIndices.Where(c => c.IsActivated.Value.Equals(true) && c.IsDeleted.Value.Equals(false)).Select(a => new PricesListIndicesVM
                {
                    CreateEnDate = a.CreateEnDate,
                    CreateTime = a.CreateTime,
                    EditEnDate = a.EditEnDate,
                    EditTime = a.EditTime,
                    PricesListIndicesId = a.PricesListIndicesId,
                    IsActivated = a.IsActivated,
                    IsDeleted = a.IsDeleted,
                    Change = a.Change,
                    Date = a.Date,
                    IndicesId = a.IndicesId,
                    RemoveEnDate = a.RemoveEnDate,
                    RemoveTime = a.RemoveTime,
                    UserCreatorName = a.UserCreatorName,
                    UserIdCreator = a.UserIdCreator,
                    UserIdEditor = a.UserIdEditor
                }).AsQueryable();
                if (PricesListIndicesId.HasValue && PricesListIndicesId > 0)
                    list.Where(a => a.PricesListIndicesId == PricesListIndicesId);
                result = list.ToList();

            }
            catch (Exception exx)
            {
                throw;
            }
            return result;

        }

        public List<PricesListIndicesVM> GetListOfPricesListIndices(int IndicesId, DateTime? PBDate, DateTime? PEDate,
            int jtStartIndex, int jtPageSize, ref int listCount, List<long> childsUsersIds, string jtSorting)
        {
            List<PricesListIndicesVM> result = new List<PricesListIndicesVM>();
            var query = from a in publicApiDb.PricesListIndices.AsQueryable()
                        join b in publicApiDb.Indices.AsQueryable() on a.IndicesId equals b.IndiceId
                        join c in publicApiDb.Persons.AsQueryable() on a.UserIdCreator equals c.PersonId
                        where a.IsActivated.Value.Equals(true) &&
                        a.IsDeleted.Value.Equals(false)
                        select new PricesListIndicesVM
                        {
                            CreateEnDate = a.CreateEnDate,
                            CreateTime = a.CreateTime,
                            EditEnDate = a.EditEnDate,
                            EditTime = a.EditTime,
                            PricesListIndicesId = a.PricesListIndicesId,
                            IsActivated = a.IsActivated,
                            IsDeleted = a.IsDeleted,
                            Price = a.Price,
                            IndicesId = a.IndicesId,
                            IndicesName = b.Name,
                            Change = a.Change,
                            Date = a.Date,
                            CreatorName = c.Name + " " + c.Family,
                            RemoveEnDate = a.RemoveEnDate,
                            RemoveTime = a.RemoveTime,
                            UserCreatorName = a.UserCreatorName,
                            UserIdCreator = a.UserIdCreator,
                            UserIdEditor = a.UserIdEditor
                        };
            if (IndicesId > 0)
                query = query.Where(a => a.IndicesId == IndicesId);
            if (PBDate.HasValue)
                query = query.Where(x => x.Date >= PBDate.Value);
            if (PEDate.HasValue)
                query = query.Where(x => x.Date <= PEDate.Value);
            query = query.Skip(jtStartIndex).Take(jtPageSize).AsQueryable();
            listCount = query.Count();// list.Count();
            try
            {
                switch (jtSorting)
                {
                    case "PDate DESC":
                        query = query.OrderByDescending(l => l.Date);
                        break;
                    case "PDate ASC":
                        query = query.OrderBy(l => l.Date);
                        break;
                    case "Price DESC":
                        query = query.OrderByDescending(l => l.Price);
                        break;
                    case "Price ASC":
                        query = query.OrderBy(l => l.Price);
                        break;
                    default:
                        query = query.OrderBy(a => a.PricesListIndicesId);
                        break;
                }
                result = query.ToList();

            }
            catch (Exception exc)
            {
                throw;
            }
            return result;
        }

        public int AddToPricesListIndices(PricesListIndicesVM pricesListIndicesVM)
        {
            PricesListIndices _PricesListIndices = _mapper.Map<PricesListIndicesVM, PricesListIndices>(pricesListIndicesVM);
            publicApiDb.PricesListIndices.Add(_PricesListIndices);
            publicApiDb.SaveChanges();
            return pricesListIndicesVM.PricesListIndicesId = _PricesListIndices.PricesListIndicesId;
        }

        public int UpdatePricesListIndices(PricesListIndicesVM pricesListIndicesVM, List<long> childsUsersIds)
        {
            bool? isActivated = pricesListIndicesVM.IsActivated.HasValue ? pricesListIndicesVM.IsActivated.Value : (bool?)true;
            bool? isDeleted = pricesListIndicesVM.IsDeleted.HasValue ? pricesListIndicesVM.IsDeleted.Value : (bool?)true;
            if (publicApiDb.PricesListIndices.Where(n => childsUsersIds.Contains(n.UserIdCreator.Value) && n.PricesListIndicesId == pricesListIndicesVM.PricesListIndicesId).Any())
            {
                try
                {
                    PricesListIndices _pricesListIndices = publicApiDb.PricesListIndices.Find(pricesListIndicesVM.PricesListIndicesId);

                    ArgumentNullException.ThrowIfNull(_pricesListIndices);

                    _pricesListIndices.IndicesId = pricesListIndicesVM.IndicesId;
                    _pricesListIndices.Date = pricesListIndicesVM.Date;
                    _pricesListIndices.Price = pricesListIndicesVM.Price;
                    _pricesListIndices.EditEnDate = DateTime.Now;
                    _pricesListIndices.EditTime = PersianDate.TimeNow;
                    _pricesListIndices.UserIdEditor = pricesListIndicesVM.UserIdEditor.Value;
                    _pricesListIndices.IsActivated = isActivated.Value;
                    _pricesListIndices.IsDeleted = isDeleted.Value;

                    publicApiDb.SaveChanges();
                    return _pricesListIndices.PricesListIndicesId;
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
            return 0;
        }

        public bool ToggleActivationPricesListIndices(int PricesListIndicesId, long userId, List<long> childsUsersIds)
        {
            try
            {
                var PricesListIndices = (from c in publicApiDb.PricesListIndices
                                         where c.PricesListIndicesId == PricesListIndicesId &&
                                         childsUsersIds.Contains(c.UserIdCreator.Value)
                                         select c).FirstOrDefault();

                ArgumentNullException.ThrowIfNull(PricesListIndices);
                PricesListIndices.IsActivated = !PricesListIndices.IsActivated;
                PricesListIndices.EditEnDate = DateTime.Now;
                PricesListIndices.EditTime = PersianDate.TimeNow;
                PricesListIndices.UserIdEditor = userId;
                //teniacoApiDb.Entry<Funds>(Funds).State = EntityState.Modified;
                publicApiDb.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool TemporaryDeletePricesListIndices(int PricesListIndicesId, long userId, List<long> childsUsersIds)
        {
            try
            {
                var PricesListIndices = (from c in publicApiDb.PricesListIndices
                                         where c.PricesListIndicesId == PricesListIndicesId &&
                                         childsUsersIds.Contains(c.UserIdCreator.Value)
                                         select c).FirstOrDefault();
                ArgumentNullException.ThrowIfNull(PricesListIndices);

                PricesListIndices.IsDeleted = !PricesListIndices.IsDeleted;
                PricesListIndices.EditEnDate = DateTime.Now;
                PricesListIndices.EditTime = PersianDate.TimeNow;
                PricesListIndices.UserIdEditor = userId;
                /// teniacoApiDb.Entry<Funds>(_funds).State = EntityState.Modified;
                publicApiDb.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public bool CompleteDeletePricesListIndices(int PricesListIndicesId, List<long> childsUsersIds)
        {
            try
            {
                var PricesListIndices = (from c in publicApiDb.PricesListIndices
                                         where c.PricesListIndicesId == PricesListIndicesId &&
                               childsUsersIds.Contains(c.UserIdCreator.Value)
                                         select c).FirstOrDefault();
                ArgumentNullException.ThrowIfNull(PricesListIndices);
                publicApiDb.PricesListIndices.Remove(PricesListIndices);
                publicApiDb.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Methods For Work With States

        public List<StatesVM> GetListOfStates(int? countryId)
        {
            try
            {
                var states = publicApiDb.States.Where(s => s.IsActivated.Value.Equals(true) && s.IsDeleted.Value.Equals(false)).AsQueryable();

                if (countryId.HasValue)
                    if (countryId.Value > 0)
                        states = states.Where(s => s.CountryId.HasValue).Where(s => s.CountryId.Value.Equals(countryId));

                return _mapper.Map<List<States>, List<StatesVM>>(states.ToList());
            }
            catch (Exception exc)
            { }
            return new List<StatesVM>();
        }

        public StatesVM GetStateWithStateId(long StateId, List<long> childsUsersIds)
        {
            StatesVM StatesVM = new StatesVM();
            try
            {
                if (publicApiDb.States.
                    Where(l => l.StateId.Equals(StateId)).
                    Any())
                    StatesVM = _mapper.Map<States, StatesVM>(publicApiDb.States
                        .Where(l => l.StateId.Equals(StateId)).FirstOrDefault());
            }
            catch (Exception exc)
            { }
            return StatesVM;
        }

        #endregion

        #region Methods For Work With SmsSenders
        public SmsSendersVM GetDefaultSmsSender()
        {
            var smsSender = publicApiDb.SmsSenders.Where(c => c.IsDefault == true && c.IsActivated.Value.Equals(true) && c.IsDeleted.Value.Equals(false)).FirstOrDefault();
            var smsSenderVm = new SmsSendersVM();
            smsSenderVm = _mapper.Map<SmsSenders, SmsSendersVM>(smsSender);

            return smsSenderVm;
        }
        public List<SmsSendersVM> GetListOfSmsSenders(
            List<long> childsUsersIds,
            int jtStartIndex,
            int jtPageSize,
            ref int listCount,
            string searchText/*,
           List<long> childsUsersIds,
           string Lang = null,
           string jtSorting = null
           /*long userId = 0*/)
        {
            List<SmsSendersVM> smsSendersVMList = new List<SmsSendersVM>();

            var list = publicApiDb.SmsSenders
                    .Where(c => childsUsersIds.Contains(c.UserIdCreator.Value) && c.IsActivated.Value.Equals(true) &&
                     c.IsDeleted.Value.Equals(false)).AsQueryable();

            //if (!string.IsNullOrEmpty(Lang))
            //{
            //    list = list.Where(x => x.Lang.Equals(Lang));
            //}

            if (!string.IsNullOrEmpty(searchText))
                list = list.Where(a => a.SmsSenderTitle.Contains(searchText) || a.SmsSenderNumber.Contains(searchText));

            try
            {
                listCount = list.Count();

                if (listCount > jtPageSize)
                {
                    smsSendersVMList = _mapper.Map<List<SmsSenders>, List<SmsSendersVM>>(list.OrderByDescending(s => s.SmsSenderId)
                             .Skip(jtStartIndex).Take(jtPageSize).ToList());

                }
                else
                {
                    smsSendersVMList = _mapper.Map<List<SmsSenders>,
                        List<SmsSendersVM>>(list.OrderByDescending(s => s.SmsSenderId).ToList());
                }
            }
            catch (Exception exc)
            { }
            return smsSendersVMList;
        }

        public List<SmsSendersVM> GetAllSmsSendersList(
            List<long> childsUsersIds,
            ref int listCount,
            string searchText)
        {
            List<SmsSendersVM> smsSendersVM = new List<SmsSendersVM>();

            try
            {
                var list = publicApiDb.SmsSenders.Where(p => childsUsersIds.Contains(p.UserIdCreator.Value)).
                    Where(f => f.IsActivated.Equals(true) &&
                    f.IsDeleted.Equals(false)).AsQueryable();

                if (!string.IsNullOrEmpty(searchText))
                    list = list.Where(a => a.SmsSenderTitle.Contains(searchText) || a.SmsSenderNumber.Contains(searchText));

                listCount = list.Count();

                smsSendersVM = _mapper.Map<List<SmsSenders>, List<SmsSendersVM>>(list.OrderByDescending(f => f.SmsSenderId).ToList());
            }
            catch (Exception exc)
            { }

            return smsSendersVM;
        }

        public long AddToSmsSenders(SmsSendersVM smsSendersVM, List<long> childsUsersIds)
        {
            try
            {
                if (!publicApiDb.SmsSenders.Where(p =>
                    p.SmsSenderNumber.Equals(smsSendersVM.SmsSenderNumber) &&
                    childsUsersIds.Contains(p.UserIdCreator.Value)).Any())
                {
                    SmsSenders smsSenders = _mapper.Map<SmsSendersVM, SmsSenders>(smsSendersVM);
                    publicApiDb.SmsSenders.Add(smsSenders);
                    publicApiDb.SaveChanges();
                    return smsSenders.SmsSenderId;
                }
                else
                    return -1;
                //}
            }
            catch (Exception exc)
            { }
            return 0;
        }

        public long UpdateSmsSenders(ref SmsSendersVM smsSendersVM,
            //long userId,
            List<long> childsUsersIds)
        {
            try
            {
                long smsSenderId = smsSendersVM.SmsSenderId;
                string userName = smsSendersVM.UserName;
                string password = smsSendersVM.Password;
                string title = smsSendersVM.SmsSenderTitle;
                string number = smsSendersVM.SmsSenderNumber;

                if (publicApiDb.SmsSenders.Where(p =>
                    p.SmsSenderNumber.Equals(number) &&
                    childsUsersIds.Contains(p.UserIdCreator.Value) &&
                    !p.SmsSenderId.Equals(smsSenderId)).Any())
                {
                    return -1;
                }

                SmsSenders smsSender = (from c in publicApiDb.SmsSenders
                                        where c.SmsSenderId == smsSenderId
                                        select c).FirstOrDefault();

                smsSender.EditEnDate = DateTime.Now;
                smsSender.EditTime = PersianDate.TimeNow;
                smsSender.UserIdEditor = smsSendersVM.UserIdEditor;
                smsSender.SmsSenderTitle = title;
                smsSender.SmsSenderNumber = number;
                smsSender.UserName = userName;
                smsSender.Password = password;
                smsSender.IsActivated = smsSendersVM.IsActivated;
                smsSender.IsDeleted = smsSendersVM.IsDeleted;
                publicApiDb.Entry<SmsSenders>(smsSender).State = EntityState.Modified;
                publicApiDb.SaveChanges();

                smsSendersVM.UserIdCreator = smsSender.UserIdCreator.Value;

                #region rewrite inside module

                //long userIdCreator = SmsSendersVM.UserIdCreator.Value;
                //if (SmsSendersVM.UserIdCreator.HasValue)
                //{
                //    var user = publicApiDb.Users.FirstOrDefault(u => u.UserId.Equals(userIdCreator));
                //    var userDetails = publicApiDb.UsersProfile.FirstOrDefault(up => up.UserId.Equals(userIdCreator));
                //    SmsSendersVM.UserCreatorName = user.UserName;

                //    if (!string.IsNullOrEmpty(userDetails.Name))
                //        SmsSendersVM.UserCreatorName += " - " + userDetails.Name;

                //    if (!string.IsNullOrEmpty(userDetails.Family))
                //        SmsSendersVM.UserCreatorName += " - " + userDetails.Family;
                //}

                #endregion

                return smsSender.SmsSenderId;
                //}
            }
            catch (Exception exc)
            { }
            return 0;
        }

        public bool ToggleActivationSmsSenders(long smsSenderId, long userId, List<long> childsUsersIds)
        {
            try
            {
                var smsSender = (from c in publicApiDb.SmsSenders
                                 where c.SmsSenderId == smsSenderId &&
                                 childsUsersIds.Contains(c.UserIdCreator.Value)
                                 select c).FirstOrDefault();

                if (smsSender != null)
                {
                    smsSender.IsActivated = !smsSender.IsActivated;
                    smsSender.EditEnDate = DateTime.Now;
                    smsSender.EditTime = PersianDate.TimeNow;
                    smsSender.UserIdEditor = userId;
                    publicApiDb.Entry<SmsSenders>(smsSender).State = EntityState.Modified;
                    publicApiDb.SaveChanges();
                    return true;
                }
            }
            catch (Exception exc)
            { }
            return false;
        }

        public bool TemporaryDeleteSmsSenders(long smsSenderId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var smsSender = (from c in publicApiDb.SmsSenders
                                 where c.SmsSenderId == smsSenderId &&
                                 childsUsersIds.Contains(c.UserIdCreator.Value)
                                 select c).FirstOrDefault();

                if (smsSender != null)
                {
                    smsSender.IsDeleted = !smsSender.IsDeleted;
                    smsSender.RemoveEnDate = DateTime.Now;
                    smsSender.RemoveTime = PersianDate.TimeNow;
                    smsSender.UserIdRemover = userId;
                    publicApiDb.Entry<SmsSenders>(smsSender).State = EntityState.Modified;
                    publicApiDb.SaveChanges();
                    return true;
                }
            }
            catch (Exception exc)
            { }
            return false;
        }

        public bool CompleteDeleteSmsSenders(long smsSenderId,
            //long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var smsSender = (from c in publicApiDb.SmsSenders
                                 where c.SmsSenderId == smsSenderId &&
                                 childsUsersIds.Contains(c.UserIdCreator.Value)
                                 select c).FirstOrDefault();

                if (smsSender != null)
                {
                    publicApiDb.SmsSenders.Remove(smsSender);
                    publicApiDb.SaveChanges();
                    return true;
                }
            }
            catch (Exception exc)
            { }
            return false;
        }

        #endregion

        #region Methods For Work With TypeOfUseLayers
        public List<TypeOfUseLayersVM> GetAllTypeOfUseLayersList(
          ref int listCount,
          List<long> childsUsersIds,
          int? typeOfUseLayersId)
        {
            List<TypeOfUseLayersVM> typeOfUseLayers = new List<TypeOfUseLayersVM>();

            try
            {
                var list = (from d in publicApiDb.TypeOfUseLayers
                            where d.IsActivated.Value.Equals(true) &&
                            d.IsDeleted.Value.Equals(false)
                            select new TypeOfUseLayersVM
                            {
                                TypeOfUseLayersId = d.TypeOfUseLayersId,
                                TypeOfUseLayersName = d.TypeOfUseLayersName,
                                UserIdCreator = d.UserIdCreator.Value,
                                CreateEnDate = d.CreateEnDate,
                                CreateTime = d.CreateTime,
                                EditEnDate = d.EditEnDate,
                                EditTime = d.EditTime,
                                UserIdEditor = d.UserIdEditor.Value,
                                RemoveEnDate = d.RemoveEnDate,
                                RemoveTime = d.EditTime,
                                UserIdRemover = d.UserIdRemover.Value,
                                IsActivated = d.IsActivated,
                                IsDeleted = d.IsDeleted,
                            }).AsQueryable();


                if (typeOfUseLayersId.HasValue)
                    if (typeOfUseLayersId.Value > 0)
                        list = list.Where(a => a.TypeOfUseLayersId.Equals(typeOfUseLayersId.Value));

                typeOfUseLayers = list.OrderByDescending(s => s.TypeOfUseLayersId).ToList();

            }
            catch (Exception)
            { }

            return typeOfUseLayers;
        }

        #endregion

        #region Methods For Work With UnitsOfMeasurement

        public List<UnitsOfMeasurementVM> GetAllUnitsOfMeasurementList(ref int listCount)
        {
            List<UnitsOfMeasurementVM> unitsOfMeasurementVMList = new List<UnitsOfMeasurementVM>();

            try
            {
                var list = publicApiDb.UnitsOfMeasurement.Where(c => c.IsActivated.Value.Equals(true) && c.IsDeleted.Value.Equals(false)).AsQueryable();

                listCount = list.Count();

                unitsOfMeasurementVMList = _mapper.Map<List<UnitsOfMeasurement>, List<UnitsOfMeasurementVM>>(list.ToList());
            }
            catch (Exception exc)
            { }

            return unitsOfMeasurementVMList;
        }

        #endregion

        #region Methods For Work With Zones

        public List<ZonesVM> GetAllZonesList(ref int listCount,
            long? stateId = null,
            long? cityId = null,
            string searchTitle = null,
            string jtSorting = null)
        {
            List<ZonesVM> zonesVMList = new List<ZonesVM>();

            try
            {
                var list = (from s in publicApiDb.States
                            join c in publicApiDb.Cities on s.StateId equals c.StateId
                            join z in publicApiDb.Zones on c.CityId equals z.CityId
                            where s.IsActivated.Value.Equals(true) &&
                                  s.IsDeleted.Value.Equals(false)
                            select new ZonesVM
                            {
                                CityId = c.CityId,
                                CityName = c.CityName,
                                CreateEnDate = z.CreateEnDate,
                                CreateTime = z.CreateTime,
                                EditEnDate = z.EditEnDate,
                                EditTime = z.EditTime,
                                IsActivated = z.IsActivated,
                                IsDeleted = z.IsDeleted,
                                RemoveEnDate = z.RemoveEnDate,
                                StateId = s.StateId,
                                StateName = s.StateName,
                                StrPolygon = z.StrPolygon,
                                Description = z.Description,
                                RemoveTime = z.EditTime,
                                UserCreatorName = "",
                                UserIdCreator = z.UserIdCreator.Value,
                                UserIdEditor = z.UserIdEditor.Value,
                                UserIdRemover = z.UserIdRemover.Value,
                                ZoneId = z.ZoneId,
                                ZoneName = z.ZoneName
                            }).OrderBy(z => z.ZoneName).AsQueryable();

                if (stateId.HasValue)
                    if (stateId.Value > 0)
                        list = list.Where(a => a.StateId.Equals(stateId.Value));

                if (cityId.HasValue)
                    if (cityId.Value > 0)
                        list = list.Where(a => a.CityId.Equals(cityId.Value));

                if (!string.IsNullOrEmpty(searchTitle))
                    list = list.Where(z => /*z.Abbreviation.Contains(searchTitle) ||*/
                            z.ZoneName.Contains(searchTitle) /*||*/
                            //z.VillageName.Contains(searchTitle) ||
                            /*z.TownName.Contains(searchTitle)*/);

                listCount = list.Count();

                //zonesVM = _mapper.Map<List<Zones>,
                //    List<ZonesVM>>(list.OrderByDescending(f => f.ZoneId).ToList());

                zonesVMList = list/*.OrderByDescending(f => f.ZoneId)*/.ToList();

                //try
                //{
                //    var cityIds = zonesVM.Select(f => f.CityId).ToList();
                //    if (cityIds != null)
                //        if (cityIds.Count > 0)
                //        {
                //            var cities = publicApiDb.Cities.Where(c => cityIds.Contains(c.CityId)).ToList();

                //            foreach (var zone in zonesVM)
                //            {
                //                if (zone.CityId > 0)
                //                {
                //                    zone.CityName = cities.
                //                        Where(fc => fc.CityId.Equals(zone.CityId)).
                //                        FirstOrDefault().
                //                        CityName;
                //                }
                //            }
                //        }
                //}
                //catch (Exception exc)
                //{ }
            }
            catch (Exception exc)
            { }

            return zonesVMList;
        }

        public List<ZonesVM> GetAllZonesListWithOutStrPolygon(ref int listCount,
            long? stateId = null,
            long? cityId = null,
            string searchTitle = null,
            string jtSorting = null)
        {
            List<ZonesVM> zonesVMList = new List<ZonesVM>();

            try
            {
                var list = (from s in publicApiDb.States
                            join c in publicApiDb.Cities on s.StateId equals c.StateId
                            join z in publicApiDb.Zones on c.CityId equals z.CityId
                            where s.IsActivated.Value.Equals(true) &&
                            s.IsDeleted.Value.Equals(false)
                            select new ZonesVM
                            {
                                CityId = c.CityId,
                                CityName = c.CityName,
                                CreateEnDate = z.CreateEnDate,
                                CreateTime = z.CreateTime,
                                EditEnDate = z.EditEnDate,
                                EditTime = z.EditTime,
                                IsActivated = z.IsActivated,
                                IsDeleted = z.IsDeleted,
                                RemoveEnDate = z.RemoveEnDate,
                                StateId = s.StateId,
                                StateName = s.StateName,
                                //StrPolygon = z.StrPolygon,
                                RemoveTime = z.EditTime,
                                UserCreatorName = "",
                                UserIdCreator = z.UserIdCreator.Value,
                                UserIdEditor = z.UserIdEditor.Value,
                                UserIdRemover = z.UserIdRemover.Value,
                                ZoneId = z.ZoneId,
                                ZoneName = z.ZoneName
                            }).OrderBy(z => z.ZoneName).AsQueryable();

                if (stateId.HasValue)
                    if (stateId.Value > 0)
                        list = list.Where(a => a.StateId.Equals(stateId.Value));

                if (cityId.HasValue)
                    if (cityId.Value > 0)
                        list = list.Where(a => a.CityId.Equals(cityId.Value));

                if (!string.IsNullOrEmpty(searchTitle))
                    list = list.Where(z => /*z.Abbreviation.Contains(searchTitle) ||*/
                            z.ZoneName.Contains(searchTitle) /*||*/
                            //z.VillageName.Contains(searchTitle) ||
                            /*z.TownName.Contains(searchTitle)*/);

                listCount = list.Count();

                //zonesVM = _mapper.Map<List<Zones>,
                //    List<ZonesVM>>(list.OrderByDescending(f => f.ZoneId).ToList());

                zonesVMList = list/*.OrderByDescending(f => f.ZoneId)*/.ToList();

                //try
                //{
                //    var cityIds = zonesVM.Select(f => f.CityId).ToList();
                //    if (cityIds != null)
                //        if (cityIds.Count > 0)
                //        {
                //            var cities = publicApiDb.Cities.Where(c => cityIds.Contains(c.CityId)).ToList();

                //            foreach (var zone in zonesVM)
                //            {
                //                if (zone.CityId > 0)
                //                {
                //                    zone.CityName = cities.
                //                        Where(fc => fc.CityId.Equals(zone.CityId)).
                //                        FirstOrDefault().
                //                        CityName;
                //                }
                //            }
                //        }
                //}
                //catch (Exception exc)
                //{ }
            }
            catch (Exception exc)
            { }

            return zonesVMList;
        }

        public List<ZonesVM> GetListOfZones(int jtStartIndex,
              int jtPageSize,
              ref int listCount,
              //List<long> childsUsersIds,
              long? stateId = null,
              long? cityId = null,
              string searchTitle = null,
              string jtSorting = null)
        {
            List<ZonesVM> zonesVMList = new List<ZonesVM>();

            try
            {
                var list = (from s in publicApiDb.States
                            join c in publicApiDb.Cities on s.StateId equals c.StateId
                            join z in publicApiDb.Zones on c.CityId equals z.CityId
                            where s.IsActivated.Value.Equals(true) &&
                            s.IsDeleted.Value.Equals(false)
                            select new ZonesVM
                            {
                                CityId = c.CityId,
                                CityName = c.CityName,
                                CreateEnDate = z.CreateEnDate,
                                CreateTime = z.CreateTime,
                                EditEnDate = z.EditEnDate,
                                EditTime = z.EditTime,
                                IsActivated = z.IsActivated,
                                IsDeleted = z.IsDeleted,
                                RemoveEnDate = z.RemoveEnDate,
                                StateId = s.StateId,
                                StateName = s.StateName,
                                StrPolygon = z.StrPolygon,
                                Description = z.Description,
                                RemoveTime = z.EditTime,
                                UserCreatorName = "",
                                UserIdCreator = z.UserIdCreator.Value,
                                UserIdEditor = z.UserIdEditor.Value,
                                UserIdRemover = z.UserIdRemover.Value,
                                ZoneId = z.ZoneId,
                                ZoneName = z.ZoneName
                            }).OrderBy(z => z.ZoneName).AsQueryable();

                if (stateId.HasValue)
                    if (stateId.Value > 0)
                        list = list.Where(a => a.StateId.Equals(stateId.Value));

                if (cityId.HasValue)
                    if (cityId.Value > 0)
                        list = list.Where(a => a.CityId.Equals(cityId.Value));

                if (!string.IsNullOrEmpty(searchTitle))
                    list = list.Where(z => /*z.Abbreviation.Contains(searchTitle) ||*/
                                z.ZoneName.Contains(searchTitle)
                                //z.VillageName.Contains(searchTitle) ||
                                /*z.TownName.Contains(searchTitle)*/);

                if (string.IsNullOrEmpty(jtSorting))
                {
                    listCount = list.Count();

                    if (listCount > jtPageSize)
                    {
                        //zonesVMList = _mapper.Map<List<Zones>, List<ZonesVM>>(list.OrderByDescending(s => s.ZoneId)
                        //         .Skip(jtStartIndex).Take(jtPageSize).ToList());

                        zonesVMList = list/*.OrderByDescending(s => s.ZoneId)*/
                                 .Skip(jtStartIndex).Take(jtPageSize).ToList();
                    }
                    else
                    {
                        //zonesVMList = _mapper.Map<List<Zones>,
                        //    List<ZonesVM>>(list.OrderByDescending(s => s.ZoneId).ToList());

                        zonesVMList = list/*.OrderByDescending(s => s.ZoneId)*/.ToList();
                    }
                }
                else
                {
                    listCount = list.Count();

                    if (listCount > jtPageSize)
                    {
                        switch (jtSorting)
                        {
                            //case "Abbreviation ASC":
                            //    list = list.OrderBy(l => l.Abbreviation);
                            //    break;
                            //case "Abbreviation DESC":
                            //    list = list.OrderByDescending(l => l.Abbreviation);
                            //    break;
                            //case "VillageName ASC":
                            //    list = list.OrderBy(l => l.VillageName);
                            //    break;
                            //case "VillageName DESC":
                            //    list = list.OrderByDescending(l => l.VillageName);
                            //    break;
                            case "ZoneName ASC":
                                list = list.OrderBy(l => l.ZoneName);
                                break;
                            case "ZoneName DESC":
                                list = list.OrderByDescending(l => l.ZoneName);
                                break;
                                //case "TownName ASC":
                                //    list = list.OrderBy(l => l.TownName);
                                //    break;
                                //case "TownName DESC":
                                //    list = list.OrderByDescending(l => l.TownName);
                                //    break;
                        }
                        //zonesVMList = _mapper.Map<List<Zones>,
                        //    List<ZonesVM>>(list.OrderByDescending(s => s.ZoneId).Skip(jtStartIndex).Take(jtPageSize).ToList());

                        if (string.IsNullOrEmpty(jtSorting))
                            zonesVMList = list/*.OrderByDescending(s => s.ZoneId)*/
                                     .Skip(jtStartIndex).Take(jtPageSize).ToList();
                        else
                            zonesVMList = list.Skip(jtStartIndex).Take(jtPageSize).ToList();
                    }
                    else
                    {
                        //zonesVMList = _mapper.Map<List<Zones>,
                        //    List<ZonesVM>>(list.ToList());

                        zonesVMList = list.ToList();
                    }
                }

            }
            catch (Exception exc)
            { }

            return zonesVMList;
        }



        public List<ZonesVM> GetListOfZonesWithOutStrPolygon(int jtStartIndex,
             int jtPageSize,
             ref int listCount,
             long? stateId = null,
             long? cityId = null,
             string searchTitle = null,
             string jtSorting = null)
        {
            List<ZonesVM> zonesVMList = new List<ZonesVM>();

            try
            {
                var list = (from s in publicApiDb.States
                            join c in publicApiDb.Cities on s.StateId equals c.StateId
                            join z in publicApiDb.Zones on c.CityId equals z.CityId
                            where s.IsActivated.Value.Equals(true) &&
                            s.IsDeleted.Value.Equals(false)
                            select new ZonesVM
                            {
                                CityId = c.CityId,
                                CityName = c.CityName,
                                CreateEnDate = z.CreateEnDate,
                                CreateTime = z.CreateTime,
                                EditEnDate = z.EditEnDate,
                                EditTime = z.EditTime,
                                IsActivated = z.IsActivated,
                                IsDeleted = z.IsDeleted,
                                RemoveEnDate = z.RemoveEnDate,
                                StateId = s.StateId,
                                StateName = s.StateName,
                                Description = z.Description,
                                //ParentZoneId = z.ParentZoneId.HasValue ? z.ParentZoneId.Value : null,
                                RemoveTime = z.EditTime,
                                UserCreatorName = "",
                                UserIdCreator = z.UserIdCreator.Value,
                                UserIdEditor = z.UserIdEditor.Value,
                                UserIdRemover = z.UserIdRemover.Value,
                                ZoneId = z.ZoneId,
                                ZoneName = z.ZoneName
                            }).OrderBy(z => z.ZoneName).AsQueryable();

                if (stateId.HasValue)
                    if (stateId.Value > 0)
                        list = list.Where(a => a.StateId.Equals(stateId.Value));

                if (cityId.HasValue)
                    if (cityId.Value > 0)
                        list = list.Where(a => a.CityId.Equals(cityId.Value));

                if (!string.IsNullOrEmpty(searchTitle))
                    list = list.Where(z => /*z.Abbreviation.Contains(searchTitle) ||*/
                                z.ZoneName.Contains(searchTitle)
                                //z.VillageName.Contains(searchTitle) ||
                                /*z.TownName.Contains(searchTitle)*/);

                if (string.IsNullOrEmpty(jtSorting))
                {
                    listCount = list.Count();

                    if (listCount > jtPageSize)
                    {
                        zonesVMList = list/*.OrderByDescending(s => s.ZoneId)*/
                                 .Skip(jtStartIndex).Take(jtPageSize).ToList();
                    }
                    else
                    {
                        zonesVMList = list/*.OrderByDescending(s => s.ZoneId)*/.ToList();
                    }
                }
                else
                {
                    listCount = list.Count();

                    if (listCount > jtPageSize)
                    {
                        switch (jtSorting)
                        {
                            //case "Abbreviation ASC":
                            //    list = list.OrderBy(l => l.Abbreviation);
                            //    break;
                            //case "Abbreviation DESC":
                            //    list = list.OrderByDescending(l => l.Abbreviation);
                            //    break;
                            //case "VillageName ASC":
                            //    list = list.OrderBy(l => l.VillageName);
                            //    break;
                            //case "VillageName DESC":
                            //    list = list.OrderByDescending(l => l.VillageName);
                            //    break;
                            case "ZoneName ASC":
                                list = list.OrderBy(l => l.ZoneName);
                                break;
                            case "ZoneName DESC":
                                list = list.OrderByDescending(l => l.ZoneName);
                                break;
                                //case "TownName ASC":
                                //    list = list.OrderBy(l => l.TownName);
                                //    break;
                                //case "TownName DESC":
                                //    list = list.OrderByDescending(l => l.TownName);
                                //    break;
                        }


                        if (string.IsNullOrEmpty(jtSorting))
                            zonesVMList = list.OrderByDescending(s => s.ZoneId)
                                     .Skip(jtStartIndex).Take(jtPageSize).ToList();
                        else
                            zonesVMList = list.Skip(jtStartIndex).Take(jtPageSize).ToList();
                    }
                    else
                    {


                        zonesVMList = list.ToList();
                    }
                }

            }
            catch (Exception exc)
            { }

            return zonesVMList;
        }

        public long AddToZones(ZonesVM zonesVM/*,
            List<long> childsUsersIds*/)
        {
            try
            {

                Zones zone = _mapper.Map<ZonesVM, Zones>(zonesVM);

                publicApiDb.Zones.Add(zone);
                publicApiDb.SaveChanges();

                return zone.ZoneId;



                if (publicApiDb.Zones.Where(x => x.CityId == zonesVM.CityId &&
                        //x.VillageName.Equals(zonesVM.VillageName) &&
                        x.ZoneName.Equals(zonesVM.ZoneName)
                        /*x.TownName.Equals(zonesVM.TownName)*/).Any())
                    return -1;
                else
                {
                    Zones zone2 = _mapper.Map<ZonesVM, Zones>(zonesVM);

                    publicApiDb.Zones.Add(zone2);
                    publicApiDb.SaveChanges();

                    return zone2.ZoneId;
                }
            }
            catch (Exception exc)
            { }
            return 0;
        }

        public ZonesVM GetZoneWithZoneId(long zoneId)
        {
            ZonesVM zonesVM = new ZonesVM();

            try
            {
                zonesVM = _mapper.Map<Zones,
                    ZonesVM>(publicApiDb.Zones
                    .Where(e => e.ZoneId.Equals(zoneId)).FirstOrDefault());
            }
            catch (Exception exc)
            { }

            return zonesVM;
        }

        public long UpdateZones(ref ZonesVM zonesVM/*,
            List<long> childsUsersIds*/)
        {
            try
            {
                //string abbreviation = zonesVM.Abbreviation;
                long zoneId = zonesVM.ZoneId;
                //long? parentZoneId = zonesVM.ParentZoneId.HasValue ? zonesVM.ParentZoneId.Value : 0;
                long cityId = zonesVM.CityId;
                //string villageName = zonesVM.VillageName;
                string zoneName = zonesVM.ZoneName;
                //string townName = zonesVM.TownName;
                string strPolygon = zonesVM.StrPolygon;



                Zones zone = (from c in publicApiDb.Zones
                              where c.ZoneId == zoneId
                              select c).FirstOrDefault();

                //zone.Abbreviation = zonesVM.Abbreviation;
                zone.EditEnDate = zonesVM.EditEnDate.Value;
                zone.EditTime = zonesVM.EditTime;
                zone.UserIdEditor = zonesVM.UserIdEditor;
                zone.CityId = zonesVM.CityId;
                //zone.VillageName = zonesVM.VillageName;
                zone.ZoneName = zonesVM.ZoneName;
                //zone.TownName = zonesVM.TownName;
                zone.StrPolygon = zonesVM.StrPolygon;
                zone.IsActivated = zonesVM.IsActivated;
                zone.IsDeleted = zonesVM.IsDeleted;

                publicApiDb.Entry<Zones>(zone).State = EntityState.Modified;
                publicApiDb.SaveChanges();

                zonesVM.UserIdCreator = zone.UserIdCreator.Value;

                return zone.ZoneId;




                if (!publicApiDb.Zones//.Where(n => childsUsersIds.Contains(n.UserIdCreator.Value))
                    .Where(x => /*x.Abbreviation.Equals(abbreviation) &&*/
                    x.CityId == cityId &&
                    !x.ZoneId.Equals(zoneId) &&
                    //x.VillageName.Equals(villageName) &&
                    x.ZoneName.Equals(zoneName)
                    /*x.TownName.Equals(townName)*/).Any())
                {
                    Zones zone2 = (from c in publicApiDb.Zones
                                   where c.ZoneId == zoneId
                                   select c).FirstOrDefault();

                    //zone.Abbreviation = zonesVM.Abbreviation;
                    zone2.EditEnDate = zonesVM.EditEnDate.Value;
                    zone2.EditTime = zonesVM.EditTime;
                    zone2.UserIdEditor = zonesVM.UserIdEditor;
                    zone2.CityId = zonesVM.CityId;
                    //zone.VillageName = zonesVM.VillageName;
                    zone2.ZoneName = zonesVM.ZoneName;
                    //zone.TownName = zonesVM.TownName;
                    zone2.StrPolygon = zonesVM.StrPolygon;
                    zone2.IsActivated = zonesVM.IsActivated;
                    zone2.IsDeleted = zonesVM.IsDeleted;

                    publicApiDb.Entry<Zones>(zone).State = EntityState.Modified;
                    publicApiDb.SaveChanges();

                    zonesVM.UserIdCreator = zone.UserIdCreator.Value;

                    return zone.ZoneId;
                }
                else
                    return -1;
            }
            catch (Exception exc)
            { }

            return 0;
        }

        public bool ToggleActivationZones(long zoneId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var Zone = (from c in publicApiDb.Zones
                            where c.ZoneId == zoneId &&
                            childsUsersIds.Contains(c.UserIdCreator.Value)
                            select c).FirstOrDefault();

                if (Zone != null)
                {
                    Zone.IsActivated = !Zone.IsActivated;
                    Zone.EditEnDate = DateTime.Now;
                    Zone.EditTime = PersianDate.TimeNow;
                    Zone.UserIdEditor = userId;

                    publicApiDb.Entry<Zones>(Zone).State = EntityState.Modified;
                    publicApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool TemporaryDeleteZones(long zoneId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var Zone = (from c in publicApiDb.Zones
                            where c.ZoneId == zoneId &&
                            childsUsersIds.Contains(c.UserIdCreator.Value)
                            select c).FirstOrDefault();

                if (Zone != null)
                {
                    Zone.IsDeleted = !Zone.IsDeleted;
                    Zone.EditEnDate = DateTime.Now;
                    Zone.EditTime = PersianDate.TimeNow;
                    Zone.UserIdEditor = userId;

                    publicApiDb.Entry<Zones>(Zone).State = EntityState.Modified;
                    publicApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool CompleteDeleteZones(long zoneId,
            List<long> childsUsersIds)
        {
            try
            {
                var Zone = (from c in publicApiDb.Zones
                            where c.ZoneId == zoneId &&
                            childsUsersIds.Contains(c.UserIdCreator.Value)
                            select c).FirstOrDefault();

                if (Zone != null)
                {
                    publicApiDb.Zones.Remove(Zone);
                    publicApiDb.SaveChanges();
                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        #endregion

        #region Methods For Work With ZoneFiles

        public List<ZoneFilesVM> GetListOfZoneFiles(int jtStartIndex,
             int jtPageSize,
             ref int listCount,
             List<long> childsUsersIds,
             long? zoneId = null,
             string zoneFileTitle = "",
             string zoneFileType = "",
             string jtSorting = null)
        {
            List<ZoneFilesVM> zoneFilesVMList = new List<ZoneFilesVM>();

            var list = (from df in publicApiDb.ZoneFiles
                        where childsUsersIds.Contains(df.UserIdCreator.Value) &&
                        df.IsActivated.Value.Equals(true) &&
                        df.IsDeleted.Value.Equals(false) &&
                        df.ZoneId.Equals(zoneId)
                        select df).AsQueryable();

            if (!string.IsNullOrEmpty(zoneFileTitle))
                list = list.Where(a => a.ZoneFileTitle.Contains(zoneFileTitle));

            if (!string.IsNullOrEmpty(zoneFileType))
                list = list.Where(a => a.ZoneFileType.Contains(zoneFileType));

            listCount = list.Count();

            try
            {
                if (string.IsNullOrEmpty(jtSorting))
                {
                    if (listCount > jtPageSize)
                    {

                        zoneFilesVMList = _mapper.Map<List<ZoneFiles>, List<ZoneFilesVM>>(list.OrderByDescending(s => s.ZoneId)
                                 .Skip(jtStartIndex).Take(jtPageSize).ToList());
                    }
                    else
                    {

                        zoneFilesVMList = _mapper.Map<List<ZoneFiles>, List<ZoneFilesVM>>(list.OrderByDescending(s => s.ZoneId).ToList());
                    }
                }
                else
                {
                    if (listCount > jtPageSize)
                    {
                        switch (jtSorting)
                        {
                            case "ZoneFileTitle ASC":
                                list = list.OrderBy(l => l.ZoneFileTitle);
                                break;
                            case "ZoneFileTitle DESC":
                                list = list.OrderByDescending(l => l.ZoneFileTitle);
                                break;
                        }

                        if (string.IsNullOrEmpty(jtSorting))
                            zoneFilesVMList = _mapper.Map<List<ZoneFiles>, List<ZoneFilesVM>>(list.OrderByDescending(s => s.ZoneId)
                                     .Skip(jtStartIndex).Take(jtPageSize).ToList());
                        else
                            zoneFilesVMList = _mapper.Map<List<ZoneFiles>, List<ZoneFilesVM>>(list.Skip(jtStartIndex).Take(jtPageSize).ToList());
                    }
                    else
                    {
                        zoneFilesVMList = _mapper.Map<List<ZoneFiles>, List<ZoneFilesVM>>(list.ToList());
                    }
                }

            }
            catch (Exception exc)
            { }
            return zoneFilesVMList;
        }



        public bool AddToZoneFiles(List<ZoneFilesVM> zoneFilesVMList)
        {
            try
            {
                if (zoneFilesVMList != null)
                    if (zoneFilesVMList.Count > 0)
                    {
                        var zoneFilesList = _mapper.Map<List<ZoneFilesVM>, List<ZoneFiles>>(zoneFilesVMList);

                        publicApiDb.ZoneFiles.AddRange(zoneFilesList);
                        publicApiDb.SaveChanges();

                        return true;
                    }
            }
            catch (Exception exc)
            {
            }
            return false;
        }

        public ZoneFilesVM GetZoneFileWithZoneFileId(int zoneFileId,
            List<long> childsUsersIds)
        {
            ZoneFilesVM zoneFilesVM = new ZoneFilesVM();

            try
            {
                zoneFilesVM = _mapper.Map<ZoneFiles,
                    ZoneFilesVM>(publicApiDb.ZoneFiles
                    .Where(p => childsUsersIds.Contains(p.UserIdCreator.Value))
                    .Where(e => e.ZoneFileId.Equals(zoneFileId)).FirstOrDefault());
            }
            catch (Exception exc)
            { }

            return zoneFilesVM;
        }

        public bool UpdateZoneFiles(ref ZoneFilesVM zoneFilesVM,
            List<long> childsUsersIds)
        {
            int zoneFileId = zoneFilesVM.ZoneFileId;
            bool? isActivated = zoneFilesVM.IsActivated.HasValue ? zoneFilesVM.IsActivated.Value : (bool?)true;
            bool? isDeleted = zoneFilesVM.IsDeleted.HasValue ? zoneFilesVM.IsDeleted.Value : (bool?)true;

            try
            {
                ZoneFiles zoneFiles = (from c in publicApiDb.ZoneFiles
                                       where c.ZoneFileId == zoneFileId
                                       select c).FirstOrDefault();

                zoneFiles.ZoneId = zoneFilesVM.ZoneId;
                zoneFiles.ZoneFileExt = zoneFilesVM.ZoneFileExt;
                zoneFiles.ZoneFilePath = zoneFilesVM.ZoneFilePath;
                zoneFiles.ZoneFileTitle = zoneFilesVM.ZoneFileTitle;
                zoneFiles.ZoneFileType = zoneFilesVM.ZoneFileType;

                zoneFiles.EditEnDate = DateTime.Now;
                zoneFiles.EditTime = PersianDate.TimeNow;
                zoneFiles.UserIdEditor = zoneFiles.UserIdEditor;
                zoneFiles.IsActivated = isActivated.Value;
                zoneFiles.IsDeleted = isDeleted.Value;




                publicApiDb.Entry<ZoneFiles>(zoneFiles).State = EntityState.Modified;
                publicApiDb.SaveChanges();

                zoneFilesVM.UserIdCreator = zoneFiles.UserIdCreator.Value;

                return true;
            }
            catch (Exception exc)
            {
            }

            return false;
        }

        public bool ToggleActivationZoneFiles(int zoneFileId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var zoneFiles = (from c in publicApiDb.ZoneFiles
                                 where c.ZoneFileId == zoneFileId &&
                                 childsUsersIds.Contains(c.UserIdCreator.Value)
                                 select c).FirstOrDefault();

                if (zoneFiles != null)
                {
                    zoneFiles.IsActivated = !zoneFiles.IsActivated;
                    zoneFiles.EditEnDate = DateTime.Now;
                    zoneFiles.EditTime = PersianDate.TimeNow;
                    zoneFiles.UserIdEditor = userId;

                    publicApiDb.Entry<ZoneFiles>(zoneFiles).State = EntityState.Modified;
                    publicApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool TemporaryDeleteZoneFiles(int zoneFileId,
            long userId,
            List<long> childsUsersIds)
        {
            try
            {
                var zoneFiles = (from c in publicApiDb.ZoneFiles
                                 where c.ZoneFileId == zoneFileId &&
                                 childsUsersIds.Contains(c.UserIdCreator.Value)
                                 select c).FirstOrDefault();

                if (zoneFiles != null)
                {
                    zoneFiles.IsDeleted = !zoneFiles.IsDeleted;
                    zoneFiles.EditEnDate = DateTime.Now;
                    zoneFiles.EditTime = PersianDate.TimeNow;
                    zoneFiles.UserIdEditor = userId;

                    publicApiDb.Entry<ZoneFiles>(zoneFiles).State = EntityState.Modified;
                    publicApiDb.SaveChanges();

                    return true;
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public bool CompleteDeleteZoneFiles(int zoneFileId,
            List<long> childsUsersIds)
        {
            try
            {
                var zoneFiles = (from c in publicApiDb.ZoneFiles
                                 where c.ZoneFileId == zoneFileId &&
                                 childsUsersIds.Contains(c.UserIdCreator.Value)
                                 select c).FirstOrDefault();

                if (zoneFiles != null)
                {
                    publicApiDb.ZoneFiles.Remove(zoneFiles);
                    publicApiDb.SaveChanges();

                    return true;
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
