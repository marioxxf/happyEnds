using Business;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gioiaflix.Controllers
{
    public class ActivityController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetUserActivity(string userAccountLoggedId, string userLoggedId, string bookCategory, string contentsType)
        {
            var userOfUserAccountLogged = new User();
            userOfUserAccountLogged.PostActivity(userAccountLoggedId, userLoggedId, bookCategory, contentsType);

            JsonResult result = new JsonResult();
            result = this.Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
            return result;
        }
    }
}