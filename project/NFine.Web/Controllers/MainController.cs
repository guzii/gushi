using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Domain.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web.Controllers
{
    public class MainController : Controller
    {
        private NavigationApp navApp = new NavigationApp();
        private ArticleApp articleApp = new ArticleApp();
        private SpecialApp specialApp = new SpecialApp();
        public ActionResult Index()
        {
            //最新更新
            List<ArticleEntity> NewestArticle= articleApp.GetList(a => a.F_EnabledMark == true && a.F_CreatorTime <= DateTime.Now);
            foreach (var item in NewestArticle)
            {
                item.NavEntity = navApp.GetForm(item.F_NavID);
                item.F_Link = "/" + item.NavEntity.F_EnCode + "/" + item.F_EnCode;
            }
            ViewBag.NewestArticle = NewestArticle.ToJson();

            //热门排行
            ViewBag.HotArticle = articleApp.GetHotList();

            var gushiEntity = navApp.GetEntityByEnCode("gushi", a => a.F_EnabledMark == true);
            var ParentGushi = navApp.GetList(a => a.F_ParentId == gushiEntity.F_Id && a.F_IsRecommend == true);
            foreach (var item in ParentGushi)
            {
                List<NavigationEntity> ChildNavigationList = navApp.GetChildList(item.F_Id, false);
                List<NavigationEntity> ReChildNavigationList = ChildNavigationList.FindAll(a => a.F_IsRecommend == true);
                string navIds = ChildNavigationList.Select(a => a.F_Id).ToJson(",").Replace("\"", "'").TrimStart('[').TrimEnd(']');
                List<ArticleEntity> ChildArticleList = articleApp.GetList(navIds, "", " F_EnabledMark=1 ", "F_CreatorTime DESC,F_SortCode ASC ").Take(12).ToList();
                foreach (var itemArticle in ChildArticleList)
                {
                    itemArticle.NavEntity = navApp.GetForm(itemArticle.F_NavID);
                }
                item.ChildArticleList = ChildArticleList;
                item.ChildNavigationList = ReChildNavigationList;
                //推荐专题
                item.RecommendSpecial = specialApp.GetSpecialList(item.F_Id, true);
            }
            ViewBag.RecommendGushi = ParentGushi;

            return View();
        }


    }
}
