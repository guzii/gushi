using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Domain.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web.Controllers
{
    public class PartialController : Controller
    {
        private NavigationApp navApp = new NavigationApp();
        private ArticleApp articleApp = new ArticleApp();
        /// <summary>
        /// 首页头部
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public PartialViewResult Header()
        {
            var gushiEntity = navApp.GetEntityByEnCode("gushi", a => a.F_EnabledMark == true);

            List<NavigationEntity> NavgushiList = navApp.GetChildList(gushiEntity.F_Id, false).Where(a => a.F_IsNav == true).ToList();
            foreach (NavigationEntity item in NavgushiList)
            {
                NavigationEntity ParentNav = navApp.GetForm(item.F_ParentId);
                if (ParentNav.F_Id == gushiEntity.F_Id)
                {
                    item.PatentEnCode = "";
                }
                else
                {
                    item.PatentEnCode = ParentNav.F_EnCode;
                }
            }
            ViewBag.Navgushi = NavgushiList;
            var wenxueEntity = navApp.GetEntityByEnCode("wenxue", a => a.F_EnabledMark == true);
            ViewBag.Navwenxue = navApp.GetChildList(wenxueEntity.F_Id, false).Where(a => a.F_IsNav == true).ToList();
            return PartialView();
        }
        /// <summary>
        /// 其他页头部
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public PartialViewResult OtherHeader()
        {
            ViewBag.OtherNav = navApp.GetList(a => a.F_IsHeader == true);

            var ParentList = navApp.GetList(a => a.F_ParentId == "0" && a.F_EnabledMark == true).Select(a => a.F_Id);
            var PartentNav = navApp.GetList(
                 a => ParentList.Contains(a.F_ParentId) && a.F_IsRecommend == true);
            ViewBag.PartentNav = PartentNav;
            return PartialView();
        }
        [ChildActionOnly]
        public PartialViewResult AboutRecommend(string NavId)
        {
            NavigationEntity navigationEntity = navApp.GetForm(a => a.F_EnCode.Equals(NavId, StringComparison.OrdinalIgnoreCase));
            if (navigationEntity != null && !string.IsNullOrEmpty(navigationEntity.F_Id))
            {
                ViewBag.NavigationEntity = navigationEntity;
            }
            string arrNav = navApp.GetChildList(navigationEntity.F_Id, true).Select(a => a.F_Id).ToJson(",").TrimStart('[').TrimEnd(']').Replace("\"", "'");
            //网友推荐
            ViewBag.RecommandArticle = articleApp.GetList(arrNav, "", "", " F_LikeCount DESC,F_CreatorTime DESC ", 10);
            //排行榜
            ViewBag.RankArticle = articleApp.GetList(arrNav, "", "", " F_ReadCount DESC,F_CreatorTime DESC ", 10);
            return PartialView();
        }

        [ChildActionOnly]
        public PartialViewResult Footer()
        {
            return PartialView();
        }

        [HandlerAuthorizeAttribute]
        public ActionResult ImgForm()
        {
            return View();
        }

        [HttpPost]
        [HandlerAuthorizeAttribute]
        public JsonResult Upload(HttpPostedFileBase upImg, string DicPath)
        {
            if (string.IsNullOrEmpty(DicPath))
            {
                DicPath = "OtherPath/" + DateTime.Now.ToString("yyyyMMdd");
            }
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssyyy") + "_" + System.IO.Path.GetFileName(upImg.FileName);
            string Path = "/Upload/" + DicPath;
            string DirectoryPath = Server.MapPath(Path);
            if (!Directory.Exists(DirectoryPath))
            {
                //不存在  创建
                Directory.CreateDirectory(DirectoryPath);
            }
            string filePhysicalPath = DirectoryPath + "/" + fileName;
            string pic = "", error = "";
            try
            {
                upImg.SaveAs(filePhysicalPath);
                pic = Path + "/" + fileName;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return Json(new
            {
                pic = pic,
                error = error
            });
        }
    }
}
