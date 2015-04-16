using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ams.Web;

namespace Web.Controllers
{
    public class ApplyController : Controller
    {
        //
        // GET: /Apply/
        public EmployeeService EmployeeService { get { return new EmployeeService(); } }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Commit()
        {
            return View();
        }

        public ActionResult ChooseShUser()
        {
            string unitId = "3400008888";
            var list = EmployeeService.GetByUnitId(unitId, "");
            var s = this.RenderViewAsString("_ChooseUser", list);
            return Json(s, JsonRequestBehavior.AllowGet);
        }

    }
}
