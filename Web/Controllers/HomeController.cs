using Core.Abstract;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDataService _dataService;

        public HomeController(IDataService dataService)
        {
            _dataService = dataService;
        }

        // GET: Home
        public ActionResult Index()
        {
            List<PersonViewModel> persons = _dataService.GetPersons();
            return View(persons);
        }
    }
}