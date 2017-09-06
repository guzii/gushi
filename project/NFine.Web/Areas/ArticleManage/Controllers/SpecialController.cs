using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Domain.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web.Areas.ArticleManage.Controllers
{
    public class SpecialController : ControllerBase
    {
        SpecialApp specialApp = new SpecialApp();
        ArticleApp articleApp = new ArticleApp();
        SpecialArticleApp specialArticleApp = new SpecialArticleApp();
        SpecialNavigationApp specialNavigationApp = new SpecialNavigationApp();


        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeJson()
        {
            var data = specialApp.GetList();
            var treeList = new List<TreeViewModel>();
            foreach (SpecialEntity item in data)
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
        public ActionResult GetGridJson(Pagination pagination, string spId, string keywords)
        {
            string ArticleIds = string.Empty;
            if (!string.IsNullOrEmpty(spId))
            {
                List<SpecialEntity> ChildSpecial = specialApp.GetChildList(spId, true);
                string ChildSpecialIds = ChildSpecial.Select(a => a.F_Id).ToJson(",").Replace("\"", "'").TrimStart('[').TrimEnd(']');
                ArticleIds = specialArticleApp.GetList(a => ChildSpecialIds.Contains(a.F_SpecialId)).Select(a => a.F_ArticleId).ToJson(",").Replace("\"", "'").TrimStart('[').TrimEnd(']');
            }
            else
            {
                ArticleIds = specialArticleApp.GetList().Select(a => a.F_ArticleId).Distinct()
                    .ToJson(",").Replace("\"", "'").TrimStart('[').TrimEnd(']');
            }
            var data = articleApp.GetListSpecial(pagination, ArticleIds, keywords, a => a.F_DeleteMark == false).Distinct();
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
        public ActionResult GetTreeGridJson()
        {
            var data = specialApp.GetList();
            var treeList = new List<TreeGridModel>();
            foreach (SpecialEntity item in data)
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
        public ActionResult GetFormJson(string keyValue)
        {
            var data = specialApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeSelectJson()
        {
            var data = specialApp.GetList();
            var treeList = new List<TreeSelectModel>();
            foreach (SpecialEntity item in data)
            {
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.F_Id;
                treeModel.text = item.F_Name;
                treeModel.parentId = item.F_ParentId;
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(SpecialEntity specialEntity, string keyValue)
        {
            specialApp.SubmitForm(specialEntity, keyValue);
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitArticleForm(string SpId, string ArticleIds)
        {
            specialApp.SubmitArticleForm(SpId, ArticleIds);
            return Success("操作成功。");
        }
        [HttpGet]
        [HandlerAuthorize]
        public ActionResult Special()
        {
            return View();
        }
        [HttpGet]
        [HandlerAuthorize]
        public ActionResult List()
        {
            return View();
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetListGridJson()
        {
            var data = specialApp.GetList(a => a.F_ParentId != "0");

            return Content(data.ToJson());
        }
        public ActionResult GetListGridTree(string NavigationId)
        {
            var moduledata = specialApp.GetList();
            var specialdata = new List<SpecialNavigationEntity>();
            if (!string.IsNullOrEmpty(NavigationId))
            {
                specialdata = specialNavigationApp.GetList(a => a.F_NavigationId == NavigationId);
            }
            var treeList = new List<TreeViewModel>();
            foreach (SpecialEntity item in moduledata)
            {
                TreeViewModel tree = new TreeViewModel();
                bool hasChildren = moduledata.Count(t => t.F_ParentId == item.F_Id) == 0 ? false : true;
                tree.id = item.F_Id;
                tree.text = item.F_Name;
                tree.value = item.F_EnCode;
                tree.parentId = item.F_ParentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.showcheck = true;
                tree.checkstate = specialdata.Count(t => t.F_SpecialId == item.F_Id);
                tree.hasChildren = true;
                tree.img = "";
                treeList.Add(tree);
            }

            return Content(treeList.TreeViewJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitNavigationForm(string NavigationId, string SpecialIds)
        {
            specialApp.SubmitNavigationForm(NavigationId, SpecialIds);
            return Success("操作成功。");
        }

        [HttpGet]
        [HandlerAuthorize]
        public ActionResult ReList()
        {
            return View();
        }
        public ActionResult GetReListGridTree(string NavigationId)
        {
            var moduledata = specialApp.GetReSpecialList(NavigationId);
            var specialNavigationList = specialNavigationApp.GetList(a => a.F_NavigationId == NavigationId);

            var treeList = new List<TreeViewModel>();
            foreach (SpecialEntity item in moduledata)
            {
                TreeViewModel tree = new TreeViewModel();
                bool hasChildren = moduledata.Count(t => t.F_ParentId == item.F_Id) == 0 ? false : true;
                tree.id = item.F_Id;
                tree.text = item.F_Name;
                tree.value = item.F_EnCode;
                tree.parentId = item.F_ParentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.showcheck = true;
                tree.checkstate = specialNavigationList.Count(t => t.F_SpecialId == item.F_Id && t.F_IsRecommand == true);
                tree.hasChildren = true;
                tree.img = "";
                treeList.Add(tree);
            }

            return Content(treeList.TreeViewJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitReNavigationForm(string NavigationId, string SpecialIds)
        {
            specialApp.SubmitReNavigationForm(NavigationId, SpecialIds);
            return Success("操作成功。");
        }

    }
}
