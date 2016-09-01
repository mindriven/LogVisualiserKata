using System.Web.Mvc;
using LogChart.Models;

namespace LogChart.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string filePath = Server.MapPath("~/input.txt");
            var lines = System.IO.File.ReadAllLines(filePath);
            return View(lines.ToDrawingInfos());
        }
    }
}