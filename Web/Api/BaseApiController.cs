using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Api
{
    public class BaseApiController : Controller
    {
        private JsonResult JsonResponse(bool flagSuccess, object data = null, string message = "")
        {
            var response = new { success = flagSuccess, data = data, message = message };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        protected JsonResult SuccessJsonResponse(object data = null, string message = "")
        {
            return JsonResponse(true, data, message);
        }

        protected JsonResult FailJsonResponse(string message = "", object data = null)
        {
            return JsonResponse(false, data, message);
        }
    }
}