﻿using System.Web.Mvc;

namespace MVCTestTools.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }

        public ActionResult Error()
        {
            Response.StatusCode = 500;
            return View();
        }
    }
}