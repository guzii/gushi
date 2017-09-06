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
    public class ArticlesController : Controller
    {
        ArticleApp articleApp = new ArticleApp();
        NavigationApp navigationApp = new NavigationApp();

        [HttpGet]
        public ActionResult Index(string id, int pageIndex = 1)
        {
            ArticleEntity articleEntity = articleApp.GetFormByEnCode(id);
            if (!articleEntity.F_SaveStyle)
            {
                string filePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath
                    + "\\" + articleEntity.F_ContentLink;
                //文本
                articleEntity.F_Content = FileHelper.ReaderFile(filePath);
            }

            articleEntity.NavEntity = navigationApp.GetForm(a => a.F_Id == articleEntity.F_NavID);
            //文章处理
            string[] split = { "_ueditor_page_break_tag_" };
            string[] Content = articleEntity.F_Content.Split(split, StringSplitOptions.RemoveEmptyEntries);
            articleEntity.F_Content = Content[pageIndex - 1];
            IPagedList<string> contentList = Content.ToList().ToPagedList(pageIndex, 1);
            ViewBag.contentList = contentList;
            //精华推荐
            ViewBag.Isrecommend = articleApp.GetList(a => a.F_Isrecommend == true && a.F_EnabledMark == true && !string.IsNullOrEmpty(a.F_ImgUrl),5);
            return View(articleEntity);
        }

    }
}
