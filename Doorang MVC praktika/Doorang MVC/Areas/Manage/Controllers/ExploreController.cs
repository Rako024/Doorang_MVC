using Business.Exceptions;
using Business.Services.Abstracts;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Composition;
using System.Data;

namespace Doorang_MVC.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]
    public class ExploreController : Controller
    {
        IExploreService _exploreService;

        public ExploreController(IExploreService exploreService)
        {
            _exploreService = exploreService;
        }

        public IActionResult Index()
        {
            List<Explore> explores = _exploreService.GetAllExplores();
            return View(explores);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Explore explore)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if(explore.PhotoFile == null)
            {
                ModelState.AddModelError("PhotoFile", "Photo file is null!");
                return View();
            }
            try
            {
                _exploreService.CreateExplore(explore);
            }
            catch (NotFoundExploreException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (PhotoFileFormatException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            
            }catch(Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
                return View();
            }
            return RedirectToAction("index");
        }


        public IActionResult Delete(int id)
        {

            try
            {
                _exploreService.DeleteExplore(id);
            }catch (NotFoundExploreException ex)
            {
                ModelState.AddModelError("", "Explore is null referance");
                return RedirectToAction("index");
            }
            return RedirectToAction("index");
        }

        public IActionResult Update(int id)
        {
            Explore explore = _exploreService.GetExplore(x => x.Id == id);
            if(explore == null)
            {
                ModelState.AddModelError("", "Explore is null referance");
                return RedirectToAction("index");
            }
            return View(explore);
        }
        [HttpPost]
        public IActionResult Update(Explore explore)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                _exploreService.UpdateExplore(explore.Id, explore);
            }catch(NotFoundExploreException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }catch(PhotoFileFormatException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
            return RedirectToAction("index");
        }

    }
}
