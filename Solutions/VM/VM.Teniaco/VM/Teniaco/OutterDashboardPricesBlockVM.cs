using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Teniaco
{
    [NotMapped]
    public class OutterDashboardPricesBlockVM
    {
        //مجموع سرمایه گذاری اولیه املاک
        public double SumOfFirstPropertiesPrices { get; set; }

        //مجموع آخرین سرمایه گذاری ملک
        public double SumOfLastPropertiesPrices { get; set; }

        //رشد قیمت املاک
        public double PropertiesGrowth { get; set; }

        //سود املاک
        public double PropertiesProfit { get; set; }

        //مجموع سرمایه گذاری اولیه پروژه های ساخت
        public double SumOfFirstConstructionProjectsPrices { get; set; }

        //مجموع آخرین سرمایه گذاری پروژه های ساخت
        public double SumOfLastConstructionProjectsPrices { get; set; }

        //رشد قیمت پروژه های ساخت
        public double ConstructionProjectsGrowth { get; set; }

        //سود پروژه های ساخت
        public double ConstructionProjectsProfit { get; set; }

        //سرمایه گذاری کل
        public double TotalInvestment { get; set; }

        //جمع کل پرداختی ها
        public double SumOfPayments { get; set; }

        #region date of Property

        //تاریخ اولین سرمایه گذاری ملک
        //قدیمی ترین تاریخ قیمت ملک
        public DateTime OldestEnDateOfPropertyInvestment { get; set; }

        //تاریخ اولین سرمایه گذاری ملک
        //قدیمی ترین تاریخ قیمت ملک
        public string OldestFaDateOfPropertyInvestment { get; set; }

        //تاریخ آخرین سرمایه گذاری ملک
        //جدید ترین تاریخ قیمت ملک
        public DateTime NewestEnDateOfPropertyInvestment { get; set; }

        //تاریخ آخرین سرمایه گذاری ملک
        //جدید ترین تاریخ قیمت ملک
        public string NewestFaDateOfPropertyInvestment { get; set; }

        #endregion

        #region date of ConstructionProject

        //تاریخ اولین سرمایه گذاری پروژه ساخت
        //قدیمی ترین تاریخ قیمت پروژه ساخت
        public DateTime OldestEnDateOfConstructionProjectInvestment { get; set; }

        //تاریخ اولین سرمایه گذاری پروژه ساخت
        //قدیمی ترین تاریخ قیمت پروژه ساخت
        public string OldestFaDateOfConstructionProjectInvestment { get; set; }

        //تاریخ آخرین سرمایه گذاری پروژه ساخت
        //جدید ترین تاریخ قیمت پروژه ساخت
        public DateTime NewestEnDateOfConstructionProjectInvestment { get; set; }

        //تاریخ آخرین سرمایه گذاری پروژه ساخت
        //جدید ترین تاریخ قیمت پروژه ساخت
        public string NewestFaDateOfConstructionProjectInvestment { get; set; }

        #endregion

        //تاریخ آخرین بروز رسانی
        public double LastDaysOfLastUpdate { get; set; }
    }
}
