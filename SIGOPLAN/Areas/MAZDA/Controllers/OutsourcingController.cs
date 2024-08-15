using Core.DTO.MAZDA;
using Core.Enum.MAZDA;
using Data.Factory.MAZDA;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.MAZDA.Controllers
{
    public class OutsourcingController : BaseController
    {
        // GET: MAZDA/Outsourcing
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Outsourcing()
        {
            return View();
        }
    }
}