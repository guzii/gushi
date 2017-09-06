using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Domain.Entity.SystemManage;
using NPOI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace NFine.Web.Areas.ArticleManage.Controllers
{
    public class TagsController : ControllerBase
    {
        private TagsApp tagsApp = new TagsApp();

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            string[] arr = keyValue.Split(',');
            List<String> listS = new List<String>(arr);
            tagsApp.DeleteForge(listS);
            return Success("删除成功。");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult SubmitForm(TagsEntity tagsEntity, string keyValue)
        {
            if (tagsApp.ExistTitle(tagsEntity.F_Title))
            {
                return Error("标签名称已存在");
            }
            if (tagsEntity.F_DeleteMark == null)
                tagsEntity.F_DeleteMark = false;
            tagsApp.SubmitForm(tagsEntity, keyValue);
            return Success("操作成功。");
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(string navId, string keywords)
        {
            var data = tagsApp.GetList();
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            TagsEntity tagsEntity = tagsApp.GetForm(keyValue);
            return Content(tagsEntity.ToJson());
        }
    }
}
