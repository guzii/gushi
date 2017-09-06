using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Domain.Entity.SystemManage;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web.Controllers
{
    public class SpecialsController : Controller
    {
        SpecialApp specialApp = new SpecialApp();
        SpecialArticleApp specialArticleApp = new SpecialArticleApp();
        public ActionResult Index()
        {
            List<SpecialEntity> list = specialApp.GetTreeList(a => a.F_EnabledMark == true);
            return View(list);
        }

        public ActionResult List(string id, int pageIndex = 1)
        {
            SpecialEntity entity = specialApp.GetEntityByEnCode(id, a => a.F_EnabledMark == true);
            if (entity == null || !Validate.IsGuid(entity.F_Id))
                return RedirectToAction("index", "Specials");
            ViewBag.entity = entity;
            return View();
        }

    }
}
