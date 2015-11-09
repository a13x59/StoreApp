using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreApp.Models;

namespace StoreApp.Controllers
{
    public class StatisticsController : Controller
    {
        // GET: Statistics
        public ActionResult Index()
        {
            List<StatisticsViewModel> result = new List<StatisticsViewModel>();

            for (int i = 0; i < 3; i++)
            {
                result.Add(new StatisticsViewModel() { productName = "Продукт " + i, count = i });
            }

            return View(result);
        }
    }
}