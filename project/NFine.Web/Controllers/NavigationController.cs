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
    public class NavigationController : Controller
    {
        NavigationApp navigationApp = new NavigationApp();
        SpecialApp specialApp = new SpecialApp();
        ArticleApp articleApp = new ArticleApp();

        [HttpGet]
        public ActionResult index(string id, int pageIndex = 1)
        {
            //分类实体
            NavigationEntity navigationEntity = navigationApp.GetForm(a => a.F_EnCode.Equals(id, StringComparison.OrdinalIgnoreCase));
            if (navigationEntity != null && !string.IsNullOrEmpty(navigationEntity.F_Id))
            {
                ViewBag.NavigationEntity = navigationEntity;
            }
            //获取相关的分类
            if (navigationApp.ExistChild(navigationEntity.F_Id))
            {
                ViewBag.NavigationList = navigationApp.GetChildList(navigationEntity.F_Id, false);
            }
            else
            {
                ViewBag.NavigationList = navigationApp.GetChildList(navigationEntity.F_ParentId, false);
            }
            //相关推荐专题
            ViewBag.SpecialList = specialApp.GetSpecialList(navigationEntity.F_Id, true);
            //文章
            string arrNav = navigationApp.GetChildList(navigationEntity.F_Id, true).Select(a => a.F_Id).ToJson(",").TrimStart('[').TrimEnd(']').Replace("\"", "'");

            IPagedList<ArticleEntity> ArticleList = articleApp.getMvcPageDataList(a => arrNav.Contains(a.F_NavID), a => a.F_CreatorTime, pageIndex, 10);
            if (ArticleList.Count == 0)
            {
                ArticleList = articleApp.getMvcPageDataList(a => a.F_NavID == navigationEntity.F_Id, a => a.F_SortCode, 1, 10);
            }
            //相关专题
            ViewBag.RecommandSpecialList = specialApp.GetSpecialList(navigationEntity.F_Id);
            return View(ArticleList);
        }

    }
}
