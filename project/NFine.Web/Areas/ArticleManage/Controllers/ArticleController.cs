using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Domain.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web.Areas.ArticleManage.Controllers
{
    public class ArticleController : ControllerBase
    {

        private NavigationApp navApp = new NavigationApp();
        private ArticleApp articleApp = new ArticleApp();
        private SpecialArticleApp specialArticleApp = new SpecialArticleApp();
        #region 文章
        [HttpGet]
        [HandlerAuthorize]
        public ActionResult List()
        {
            return View();
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            articleApp.DeleteFormForged(keyValue);
            return Success("删除成功。");
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            ArticleEntity articleEntity = articleApp.GetForm(keyValue);
            if (!articleEntity.F_SaveStyle)
            {
                string filePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath
                    + "\\" + articleEntity.F_ContentLink;
                //文本
                articleEntity.F_Content = FileHelper.ReaderFile(filePath);
            }
            return Content(articleEntity.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string navId, string keywords)
        {
            List<NavigationEntity> ChildNav = navApp.GetChildList(navId, true);
            string navIds = ChildNav.Select(a => a.F_Id).ToJson(",").Replace("\"", "'").TrimStart('[').TrimEnd(']');
            var data = articleApp.GetList(pagination, navIds, keywords, a => a.F_DeleteMark == false);
            var returndata = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(returndata.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetListGridJson(Pagination pagination, string navId, string SpId, string keywords)
        {
            var allData = specialArticleApp.GetList();
            string specialArticleIds = allData.Where(a => a.F_SpecialId == SpId)
                .Select(a => a.F_ArticleId).ToJson(",").Replace("\"", "'").TrimStart('[').TrimEnd(']');
            //同一文章出现在3个主题就不在添加
            string RepeatIds = allData.Where(d => allData.Count(d2 => d2.F_ArticleId == d.F_ArticleId) >= 3)
                 .Select(a => a.F_ArticleId).ToJson(",").Replace("\"", "'").TrimStart('[').TrimEnd(']');
            if (RepeatIds != "")
            {
                specialArticleIds += "," + RepeatIds;
            }
            specialArticleIds = specialArticleIds.Trim(',');

            List<NavigationEntity> ChildNav = navApp.GetChildList(navId, true);
            string navIds = ChildNav.Select(a => a.F_Id).ToJson(",").Replace("\"", "'").TrimStart('[').TrimEnd(']');
            var data = articleApp.GetList(pagination, navIds, specialArticleIds, keywords, a => a.F_DeleteMark == false);
            var returndata = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(returndata.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult SubmitForm(ArticleEntity articleEntity, string keyValue)
        {
            if (string.IsNullOrEmpty(articleEntity.F_EnCode))
                articleEntity.F_EnCode = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff");

            if (!articleEntity.F_SaveStyle)
            {
                //文本存储
                articleEntity.F_Content = WebHelper.PagerHtml(articleEntity.F_Content);

                string filePath = string.Empty;
                if (!StringHelper.IsNullOrEmptyNoHtml(articleEntity.F_ContentLink))
                    filePath = articleEntity.F_Link;
                else
                    filePath = "Article/" + articleEntity.F_EnCode + ".txt";
                filePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\" + filePath;
                FileHelper.AsynWrite(filePath, articleEntity.F_Content);
                articleEntity.F_Content = "";
                articleEntity.F_ContentLink = "Article/" + articleEntity.F_EnCode + ".txt";
            }
            else
            {
                //数据存储
                string path = "Article/" + articleEntity.F_EnCode + ".txt";
                FileHelper.DeleteFile(path);
                articleEntity.F_ContentLink = "";

            }
            if (StringHelper.IsNullOrEmptyNoHtml(articleEntity.F_Zhaiyao))
            {
                string zhaiyao = WebHelper.RemoveEmpty(WebHelper.NoHtml(articleEntity.F_Content));
                if (zhaiyao.Length > 130)
                    articleEntity.F_Zhaiyao = zhaiyao.Substring(0, 120) + "...";
                else
                    articleEntity.F_Zhaiyao = zhaiyao;
            }
            if (StringHelper.IsNullOrEmptyNoHtml(articleEntity.F_SEOTitle))
                articleEntity.F_SEOTitle = WebHelper.NoHtml(articleEntity.F_Title);
            if (StringHelper.IsNullOrEmptyNoHtml(articleEntity.F_SEOKeywords))
                articleEntity.F_SEOKeywords = WebHelper.NoHtml(articleEntity.F_Title);
            if (StringHelper.IsNullOrEmptyNoHtml(articleEntity.F_SEOdescription))
                articleEntity.F_SEOdescription = articleEntity.F_Zhaiyao;

            articleApp.SubmitForm(articleEntity, keyValue);
            return Success("操作成功。");
        }
        #endregion

        #region 文章分类管理
        [HttpGet]
        [HandlerAuthorize]
        public ActionResult Navigation()
        {
            return View();
        }
        [HttpGet]
        [HandlerAuthorize]
        public ActionResult NavForm()
        {
            return View();
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteNavForm(string keyValue)
        {
            navApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitNavForm(NavigationEntity navigationEntity, string keyValue)
        {
            navApp.SubmitForm(navigationEntity, keyValue);
            return Success("操作成功。");
        }


        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetNavTreeJson()
        {
            var data = navApp.GetList();
            var treeList = new List<TreeViewModel>();
            foreach (NavigationEntity item in data)
            {
                TreeViewModel tree = new TreeViewModel();
                bool hasChildren = data.Count(t => t.F_ParentId == item.F_Id) == 0 ? false : true;
                tree.id = item.F_Id;
                tree.text = item.F_Name;
                tree.value = item.F_EnCode;
                tree.parentId = item.F_ParentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChildren;
                treeList.Add(tree);
            }
            return Content(treeList.TreeViewJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetNavTreeGridJson()
        {
            var data = navApp.GetList();
            var treeList = new List<TreeGridModel>();
            foreach (NavigationEntity item in data)
            {
                TreeGridModel treeModel = new TreeGridModel();
                bool hasChildren = data.Count(t => t.F_ParentId == item.F_Id) == 0 ? false : true;
                treeModel.id = item.F_Id;
                treeModel.isLeaf = hasChildren;
                treeModel.parentId = item.F_ParentId;
                treeModel.expanded = hasChildren;
                treeModel.entityJson = item.ToJson();
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeGridJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetNavFormJson(string keyValue)
        {
            var data = navApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetNavTreeSelectJson()
        {
            var data = navApp.GetList();
            var treeList = new List<TreeSelectModel>();
            foreach (NavigationEntity item in data)
            {
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.F_Id;
                treeModel.text = item.F_Name;
                treeModel.parentId = item.F_ParentId;
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }

        #endregion
    }
}
