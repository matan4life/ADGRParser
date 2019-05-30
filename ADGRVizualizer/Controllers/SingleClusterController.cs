using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ADGRVizualizer.Controllers
{
    public class SingleClusterController : Controller
    {
        public static int AgeClusterIndexId { get; set; } = 1;
        public static int AgeClusterPieId { get; set; } = 1;
        public static DateTime AgeClusterDate { get; set; } = new DateTime(2018, 1, 1);
        public static bool CheatClusterIndexId { get; set; } = true;
        public static bool CheatClusterPieId { get; set; } = true;
        public static DateTime CheatClusterDate { get; set; } = new DateTime(2018, 1, 1);
        public static string CountryClusterIndexId { get; set; } = "Afghanistan";
        public static string CountryClusterPieId { get; set; } = "Afghanistan";
        public static DateTime CountryClusterDate { get; set; } = new DateTime(2018, 1, 1);
        public static string SexClusterIndexId { get; set; } = "male";
        public static string SexClusterPieId { get; set; } = "male";
        public static DateTime SexClusterDate { get; set; } = new DateTime(2018, 1, 1);
        public static int ProfitClusterIndexId { get; set; } = 1;
        public static int ProfitClusterPieId { get; set; } = 1;
        public static DateTime ProfitClusterDate { get; set; } = new DateTime(2018, 1, 1);
        public async Task<IActionResult> AgeClusterIndex()
        {
            ViewBag.Ages = Enumerable.Range(1, 6).Select(x => new SelectListItem() { Value = x.ToString(), Text = x.ToString() }).ToList();
            var data = await Data.GetSingleClusterStatistics("AgeClusterStatistics", AgeClusterIndexId, "AgeClusterId");
            var predictions = await Data.GetSingleClusterPredictions("AgeClusterPrediction", AgeClusterIndexId, "AgeClusterId");
            return View((new SelectListItem()
            {
                Value = AgeClusterIndexId.ToString(),
                Text = AgeClusterIndexId.ToString()
            }, data, predictions));
        }

        public async Task<IActionResult> AgeClusterPie()
        {
            ViewBag.Ages = Enumerable.Range(1, 6).Select(x => new SelectListItem() { Value = x.ToString(), Text = x.ToString() }).ToList();
            ViewBag.Dates = Enumerable.Range(1, 30).Select(x => new DateTime(2018, 1, x)).ToList().
                Select(y => new SelectListItem() { Value = y.ToShortDateString(), Text = y.ToShortDateString() }).ToList();
            var data = await Data.GetProfits("AgeClusters", "UserId", AgeClusterPieId, "ClusterId", AgeClusterDate.ToString("yyyy-MM-dd"));
            return View((new SelectListItem()
            {
                Value = AgeClusterPieId.ToString(),
                Text = AgeClusterPieId.ToString()
            }, new SelectListItem()
            {
                Value = AgeClusterDate.ToShortDateString(),
                Text = AgeClusterDate.ToShortDateString()
            }, data));
        }

        public async Task<IActionResult> AgeClusterCount()
        {
            var data = await Data.GetClusterCount("AgeClusters", "UserId", "ClusterId");
            var normalized = Data.NormalizeCount(data);
            return View((data.OrderByDescending(x=>x.Quantity).ToList(), normalized.OrderByDescending(x=>x.Quantity).ToList()));
        }

        public async Task<IActionResult> CheatClusterIndex()
        {
            ViewBag.Cheats = new List<bool>() { true, false }.Select(x=>new SelectListItem() { Value = x.ToString(), Text = x.ToString() }).ToList();
            var data = await Data.GetSingleClusterStatistics("CheatsClusterStatistics", CheatClusterIndexId, "Cheat");
            var predictions = await Data.GetSingleClusterPredictions("CheatsClusterPredictions", CheatClusterIndexId, "Cheat");
            return View((new SelectListItem()
            {
                Value = CheatClusterIndexId.ToString(),
                Text = CheatClusterIndexId.ToString()
            }, data, predictions));
        }

        public async Task<IActionResult> CheatClusterPie()
        {
            ViewBag.Cheats = new List<bool>() { true, false }.Select(x => new SelectListItem() { Value = x.ToString(), Text = x.ToString() }).ToList();
            ViewBag.Dates = Enumerable.Range(1, 30).Select(x => new DateTime(2018, 1, x)).ToList().
                Select(y => new SelectListItem() { Value = y.ToShortDateString(), Text = y.ToShortDateString() }).ToList();
            var data = await Data.GetProfits("Cheats", "UserId", CheatClusterPieId, "Cheat", CheatClusterDate.ToString("yyyy-MM-dd"));
            return View((new SelectListItem()
            {
                Value = CheatClusterPieId.ToString(),
                Text = CheatClusterPieId.ToString()
            }, new SelectListItem()
            {
                Value = CheatClusterDate.ToShortDateString(),
                Text = CheatClusterDate.ToShortDateString()
            }, data));
        }

        public async Task<IActionResult> CheatClusterCount()
        {
            var data = await Data.GetClusterCount("Cheats", "UserId", "Cheat");
            var normalized = Data.NormalizeCount(data);
            return View((data.OrderByDescending(x => x.Quantity).ToList(), normalized.OrderByDescending(x => x.Quantity).ToList()));
        }

        public async Task<IActionResult> CountryClusterIndex()
        {
            ViewBag.Countries = (await Data.GetAllCountries()).Select(x => new SelectListItem() { Value = x, Text = x }).ToList();
            var data = await Data.GetSingleClusterStatistics("CountryClusterStatistics", CountryClusterIndexId, "Country");
            var predictions = await Data.GetSingleClusterPredictions("CountryClusterPredictions", CountryClusterIndexId, "Country");
            return View((new SelectListItem()
            {
                Value = CountryClusterIndexId.ToString(),
                Text = CountryClusterIndexId.ToString()
            }, data, predictions));
        }

        public async Task<IActionResult> CountryClusterPie()
        {
            ViewBag.Countries = (await Data.GetAllCountries()).Select(x=>new SelectListItem() { Value = x, Text = x }).ToList();
            ViewBag.Dates = Enumerable.Range(1, 30).Select(x => new DateTime(2018, 1, x)).ToList().
                Select(y => new SelectListItem() { Value = y.ToShortDateString(), Text = y.ToShortDateString() }).ToList();
            var data = await Data.GetProfits("Users", "Id", CountryClusterPieId, "Country", CountryClusterDate.ToString("yyyy-MM-dd"));
            return View((new SelectListItem()
            {
                Value = CountryClusterPieId.ToString(),
                Text = CountryClusterPieId.ToString()
            }, new SelectListItem()
            {
                Value = CountryClusterDate.ToShortDateString(),
                Text = CountryClusterDate.ToShortDateString()
            }, data));
        }

        public async Task<IActionResult> CountryClusterCount()
        {
            var data = await Data.GetClusterCount("Users", "Id", "Country");
            var normalized = Data.NormalizeCount(data);
            return View((data.OrderByDescending(x => x.Quantity).ToList(), normalized.OrderByDescending(x => x.Quantity).ToList()));
        }

        public async Task<IActionResult> SexClusterIndex()
        {
            ViewBag.Genders = new List<string>() { "male", "female" }.Select(x=>new SelectListItem() { Value = x, Text = x }).ToList();
            var data = await Data.GetSingleClusterStatistics("SexClusterStatistics", SexClusterIndexId, "Sex");
            var predictions = await Data.GetSingleClusterPredictions("SexClusterPredictions", SexClusterIndexId, "Sex");
            return View((new SelectListItem()
            {
                Value = SexClusterIndexId.ToString(),
                Text = SexClusterIndexId.ToString()
            }, data, predictions));
        }

        public async Task<IActionResult> SexClusterPie()
        {
            ViewBag.Genders = new List<string>() { "male", "female" }.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();
            ViewBag.Dates = Enumerable.Range(1, 30).Select(x => new DateTime(2018, 1, x)).ToList().
                Select(y => new SelectListItem() { Value = y.ToShortDateString(), Text = y.ToShortDateString() }).ToList();
            var data = await Data.GetProfits("Users", "Id", SexClusterPieId, "Gender", SexClusterDate.ToString("yyyy-MM-dd"));
            return View((new SelectListItem()
            {
                Value = SexClusterPieId.ToString(),
                Text = SexClusterPieId.ToString()
            }, new SelectListItem()
            {
                Value = SexClusterDate.ToShortDateString(),
                Text = SexClusterDate.ToShortDateString()
            }, data));
        }

        public async Task<IActionResult> SexClusterCount()
        {
            var data = await Data.GetClusterCount("Users", "Id", "Gender");
            var normalized = Data.NormalizeCount(data);
            return View((data.OrderByDescending(x => x.Quantity).ToList(), normalized.OrderByDescending(x => x.Quantity).ToList()));
        }

        public async Task<IActionResult> ProfitClusterIndex()
        {
            ViewBag.Profits = Enumerable.Range(1, 4).Select(x => new SelectListItem() { Value = x.ToString(), Text = x.ToString() }).ToList();
            var data = await Data.GetSingleClusterStatistics("ProfitClusterStatistics", ProfitClusterIndexId, "ProfitClusterId");
            var predictions = await Data.GetSingleClusterPredictions("ProfitClusterPredictions", ProfitClusterIndexId, "ProfitClusterId");
            return View((new SelectListItem()
            {
                Value = ProfitClusterIndexId.ToString(),
                Text = ProfitClusterIndexId.ToString()
            }, data, predictions));
        }

        public async Task<IActionResult> ProfitClusterPie()
        {
            ViewBag.Profits = Enumerable.Range(1, 4).Select(x => new SelectListItem() { Value = x.ToString(), Text = x.ToString() }).ToList();
            ViewBag.Dates = Enumerable.Range(1, 30).Select(x => new DateTime(2018, 1, x)).ToList().
                Select(y => new SelectListItem() { Value = y.ToShortDateString(), Text = y.ToShortDateString() }).ToList();
            var data = await Data.GetProfits("RevenueClusters", "UserId", ProfitClusterPieId, "ClusterId", ProfitClusterDate.ToString("yyyy-MM-dd"));
            return View((new SelectListItem()
            {
                Value = ProfitClusterPieId.ToString(),
                Text = ProfitClusterPieId.ToString()
            }, new SelectListItem()
            {
                Value = ProfitClusterDate.ToShortDateString(),
                Text = ProfitClusterDate.ToShortDateString()
            }, data));
        }

        public async Task<IActionResult> ProfitClusterCount()
        {
            var data = await Data.GetClusterCount("RevenueClusters", "UserId", "ClusterId");
            var normalized = Data.NormalizeCount(data);
            return View((data.OrderByDescending(x => x.Quantity).ToList(), normalized.OrderByDescending(x => x.Quantity).ToList()));
        }

        public IActionResult AgeIndexIdChange(string clusterId, string returnUrl)
        {
            AgeClusterIndexId = int.Parse(clusterId);
            return RedirectToAction("AgeClusterIndex");
        }
        public IActionResult AgePieIdChange(string clusterId, string returnUrl)
        {
            AgeClusterPieId = int.Parse(clusterId);
            return RedirectToAction("AgeClusterPie");
        }
        public IActionResult AgePieDateChange(string clusterId, string returnUrl)
        {
            AgeClusterDate = DateTime.Parse(clusterId);
            return RedirectToAction("AgeClusterPie");
        }
        public IActionResult CheatIndexIdChange(string clusterId, string returnUrl)
        {
            CheatClusterIndexId = bool.Parse(clusterId);
            return RedirectToAction("CheatClusterIndex");
        }
        public IActionResult CheatPieIdChange(string clusterId, string returnUrl)
        {
            CheatClusterPieId = bool.Parse(clusterId);
            return RedirectToAction("CheatClusterPie");
        }
        public IActionResult CheatPieDateChange(string clusterId, string returnUrl)
        {
            CheatClusterDate = DateTime.Parse(clusterId);
            return RedirectToAction("CheatClusterPie");
        }

        public IActionResult CountryIndexIdChange(string clusterId, string returnUrl)
        {
            CountryClusterIndexId = clusterId;
            return RedirectToAction("CountryClusterIndex");
        }
        public IActionResult CountryPieIdChange(string clusterId, string returnUrl)
        {
            CountryClusterPieId = clusterId;
            return RedirectToAction("CountryClusterPie");
        }
        public IActionResult CountryPieDateChange(string clusterId, string returnUrl)
        {
            CountryClusterDate = DateTime.Parse(clusterId);
            return RedirectToAction("CountryClusterPie");
        }

        public IActionResult SexIndexIdChange(string clusterId, string returnUrl)
        {
            SexClusterIndexId = clusterId;
            return RedirectToAction("SexClusterIndex");
        }
        public IActionResult SexPieIdChange(string clusterId, string returnUrl)
        {
            SexClusterPieId = clusterId;
            return RedirectToAction("SexClusterPie");
        }
        public IActionResult SexPieDateChange(string clusterId, string returnUrl)
        {
            SexClusterDate = DateTime.Parse(clusterId);
            return RedirectToAction("SexClusterPie");
        }

        public IActionResult ProfitIndexIdChange(string clusterId, string returnUrl)
        {
            ProfitClusterIndexId = int.Parse(clusterId);
            return RedirectToAction("ProfitClusterIndex");
        }
        public IActionResult ProfitPieIdChange(string clusterId, string returnUrl)
        {
            ProfitClusterPieId = int.Parse(clusterId);
            return RedirectToAction("ProfitClusterPie");
        }
        public IActionResult ProfitPieDateChange(string clusterId, string returnUrl)
        {
            ProfitClusterDate = DateTime.Parse(clusterId);
            return RedirectToAction("ProfitClusterPie");
        }

    }
}