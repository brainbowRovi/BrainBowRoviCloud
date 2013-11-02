using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace BrainBowTestWebAPI.Controllers
{
    public class MoviesController : Controller
    {
        //
        // GET: /Movies/

        public ActionResult Index()
        {
            return View();
        }

        public  ActionResult GetMovieByKeyword(string keyword)
        {
            return View();
        }

    }
}
