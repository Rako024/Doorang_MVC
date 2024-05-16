using Business.Services.Abstracts;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Doorang_MVC.Controllers
{
    public class HomeController : Controller
    {
        IExploreService _exploreService;

        public HomeController(IExploreService exploreService)
        {
            _exploreService = exploreService;
        }

        public IActionResult Index()
        {
            List<Explore> explores = _exploreService.GetAllExplores();
            return View(explores);
        }
    }
}
