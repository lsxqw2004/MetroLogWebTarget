using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MetroLogWebTarget.Service;

namespace MetroLogWebTarget.Web.Controllers
{
    public class ViewerController:Controller
    {
        private readonly ILogEnvironmentService _logEnvironmentService;

        public ViewerController(ILogEnvironmentService logEnvironmentService)
        {
            _logEnvironmentService = logEnvironmentService;
        }

        public ActionResult List(int page, int pageSize, int envId)
        {
            return View();
        }
    }
}