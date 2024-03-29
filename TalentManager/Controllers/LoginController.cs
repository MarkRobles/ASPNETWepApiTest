﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;

namespace TalentManager.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index(string returnurl)
        {
            return View(); // present the login page to the user
        }
        // Login page gets posted to this action method
        [System.Web.Mvc.HttpPost]
        public ActionResult Index(string userId, string password)
        {
            if (userId.Equals(password)) // dumb check for illustration
            {
                // Create the ticket and stuff it in a cookie
                FormsAuthentication.SetAuthCookie("Badri", false);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}