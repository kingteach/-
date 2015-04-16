using Ams;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModel;

namespace Web.Controllers
{
    public class AuditController : Controller
    {
        //
        // GET: /Audit/

        public AuditService AuditService { get { return new AuditService(); } }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(TransMainInfo condition, PagerInfo<TransMainInfo> pgInfo)
        {
            return Json(AuditService.Get(condition, pgInfo), JsonRequestBehavior.AllowGet);
        }


    }
}
