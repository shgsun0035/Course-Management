﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Assignment.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult NoPermission()
        {
            return View();
        }

        public ActionResult PageNotFound() 
        {
            return View();
        }
    }
}