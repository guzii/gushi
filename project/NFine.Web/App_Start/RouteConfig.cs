using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Domain.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NFine.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.AppendTrailingSlash = true;
            routes.LowercaseUrls = true;

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");



            routes.Add(new ArticleUrlProvider());//文章Url规则

            routes.Add(new SpecialUrlProvider());//专题 Url 配置

            routes.Add(new NavigationUrlProvider());//分类Url规则


            routes.MapRoute(
                 "Home_Default",
                 "{controller}/{action}",
                 new { controller = "Main", action = "Index" }
            );
        }
    }

    #region Navigation  URL  配置
    public class NavigationUrlProvider : RouteBase
    {
        NavigationApp navigationApp = new NavigationApp();

        public override RouteData GetRouteData(System.Web.HttpContextBase httpContext)
        {
            var virtualPath = httpContext.Request.AppRelativeCurrentExecutionFilePath + httpContext.Request.PathInfo;//获取相对路径
            //  ~/mingrenmingyan/page-3
            virtualPath = virtualPath.Substring(2).Trim('/');//此时URL
            //判断是否是我们需要处理的URL，不是则返回null，匹配将会继续进行。
            if (!Validate.IsNavUrl(virtualPath))
            {
                return null;
            }
            var PageIndex = virtualPath.Split('/').Last().Split('-').Last();
            var Navigationname = virtualPath.Split('/').First();

            //尝试根据分类名称获取相应分类，忽略大小写
            var Navigation = navigationApp.GetEntityByEnCode(Navigationname, a => a.F_EnabledMark == true);

            if (Navigation == null)//如果分类是null，可能不是我们要处理的URL，返回null，让匹配继续进行
                return null;

            //至此可以肯定是我们要处理的URL了
            var data = new RouteData(this, new MvcRouteHandler());//声明一个RouteData，添加相应的路由值
            data.Values.Add("controller", "Navigation");
            data.Values.Add("action", "index");
            data.Values.Add("id", Navigation.F_EnCode);
            data.Values.Add("pageIndex", PageIndex);

            return data;//返回这个路由值将调用NavigationController.Index(Navigation.CategoeyID)方法。匹配终止
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            //判断请求是否来源于NavigationController.Index(string id),不是则返回null,让匹配继续
            var NavigationId = values["id"] as string;
            var Page = values["pageIndex"];
            if (values["pageIndex"] != null)
            {
                Page = values["pageIndex"].ToString();
            }
            if (Page == null)
                Page = "1";

            //请求不是NavigationController发起的，不是我们要处理的请求，返回null
            if (!values.ContainsKey("controller") || !values["controller"].ToString().Equals("Navigation", StringComparison.OrdinalIgnoreCase))
                return null;
            //请求不是NavigationController.Index(string id)发起的，不是我们要处理的请求，返回null
            if (!values.ContainsKey("action") || !values["action"].ToString().Equals("Index", StringComparison.OrdinalIgnoreCase))
                return null;

            //至此，我们可以确定请求是NavigationController.Index(string id)发起的，生成相应的URL并返回

            var Navigation = navigationApp.GetEntityByEnCode(NavigationId, a => a.F_EnabledMark == true);


            if (Navigation == null)
                throw new ArgumentNullException("Navigation");//找不到分类抛出异常

            var path = Navigation.F_EnCode.Trim();//生成URL
            if (Page + "" != "1")
                path = path + "/page-" + Page;

            return new VirtualPathData(this, path.ToLowerInvariant());
        }
    }
    #endregion

    #region Article URL 配置
    public class ArticleUrlProvider : RouteBase
    {
        ArticleApp articleApp = new ArticleApp();
        NavigationApp navigationApp = new NavigationApp();
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var virtualPath = httpContext.Request.AppRelativeCurrentExecutionFilePath + httpContext.Request.PathInfo;//获取相对路径
            //~/mingrenmingyan/154564654654/page-3
            virtualPath = virtualPath.Substring(2).Trim('/');
            if (!Validate.IsArticleUrl(virtualPath))
            {
                return null;
            }
            //mingrenmingyan/154564654654
            string[] virtualPathStr = virtualPath.Split('/');
            var PageIndex = "1"; var ArticleId = ""; var NavName = "";
            if (virtualPathStr.Length == 3)
            {
                NavName = virtualPathStr[0].Trim();
                ArticleId = virtualPathStr[1].Trim();
                PageIndex = virtualPathStr[2].Split('-').Last();
            }
            else if (virtualPathStr.Length == 2)
            {
                NavName = virtualPathStr[0].Trim();
                ArticleId = virtualPathStr[1].Trim();
            }
            else
                return null;

            NavigationEntity navigationEntity = navigationApp.GetEntityByEnCode(NavName, a => a.F_EnabledMark == true);

            ArticleEntity articleEntity = articleApp.GetFormByEnCode(ArticleId);

            if (navigationEntity == null || !Validate.IsGuid(navigationEntity.F_Id)
                || articleEntity == null || !Validate.IsGuid(articleEntity.F_Id)
                || !articleEntity.F_NavID.Equals(navigationEntity.F_Id, StringComparison.OrdinalIgnoreCase))
                throw new ArgumentNullException("Article");

            var data = new RouteData(this, new MvcRouteHandler());//声明一个RouteData，添加相应的路由值
            data.Values.Add("controller", "Articles");
            data.Values.Add("action", "index");
            data.Values.Add("id", articleEntity.F_EnCode);
            data.Values.Add("pageIndex", PageIndex);

            return data;//返回这个路由值将调用NavigationController.Index(Navigation.CategoeyID)方法。匹配终止
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            var ArticleId = values["id"] as string;
            var Page = values["pageIndex"];
            if (values["pageIndex"] != null)
            {
                Page = values["pageIndex"].ToString();
            }
            if (Page == null)
                Page = "1";

            //请求不是NavigationController发起的，不是我们要处理的请求，返回null
            if (!values.ContainsKey("controller") || !values["controller"].ToString().Equals("Articles", StringComparison.OrdinalIgnoreCase))
                return null;
            //请求不是NavigationController.Index(string id)发起的，不是我们要处理的请求，返回null
            if (!values.ContainsKey("action") || !values["action"].ToString().Equals("Index", StringComparison.OrdinalIgnoreCase))
                return null;

            ArticleEntity articleEntity = articleApp.GetFormByEnCode(ArticleId);


            if (articleEntity == null || !Validate.IsGuid(articleEntity.F_Id))
                throw new ArgumentNullException("Article");

            NavigationEntity navigationEntity = navigationApp.GetForm(articleEntity.F_NavID);

            var path = navigationEntity.F_EnCode + "/" + articleEntity.F_EnCode.Trim();//生成URL

            if (Page + "" != "1")
                path = path + "/page-" + Page;

            return new VirtualPathData(this, path.ToLowerInvariant());
        }

    }
    #endregion

    #region 专题 Url 配置
    public class SpecialUrlProvider : RouteBase
    {
        SpecialApp specialApp = new SpecialApp();
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var virtualPath = httpContext.Request.AppRelativeCurrentExecutionFilePath + httpContext.Request.PathInfo;//获取相对路径
            //~/zt   /mingren    /pagr-2
            virtualPath = virtualPath.Substring(2).Trim('/');
            if (!Validate.IsSpecialUrl(virtualPath))
            {
                return null;
            }
            string[] virtualPathStr = virtualPath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var PageIndex = "1"; var ztId = "";
            if (virtualPathStr.Length == 1)
            {
                var data = new RouteData(this, new MvcRouteHandler());//声明一个RouteData，添加相应的路由值
                data.Values.Add("controller", "Specials");
                data.Values.Add("action", "index");
                return data;
            }
            else if (virtualPathStr.Length == 3 || virtualPathStr.Length == 2)
            {
                if (virtualPathStr.Length == 3)
                {
                    ztId = virtualPathStr[1];
                    PageIndex = virtualPathStr[2].Split('-').Last();
                }
                else if (virtualPathStr.Length == 2)
                {
                    ztId = virtualPathStr[1];
                }
                SpecialEntity entity = specialApp.GetEntityByEnCode(ztId, a => a.F_EnabledMark == true);
                if (entity == null || !Validate.IsGuid(entity.F_Id))
                    throw new ArgumentNullException("Specials");

                var data = new RouteData(this, new MvcRouteHandler());//声明一个RouteData，添加相应的路由值
                data.Values.Add("controller", "Specials");
                data.Values.Add("action", "list");
                data.Values.Add("id", entity.F_EnCode);
                data.Values.Add("pageIndex", PageIndex);
                return data;
            }
            else
                return null;



        }
        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            var ztId = values["id"] as string;
            var Page = values["pageIndex"];
            if (values["pageIndex"] != null)
            {
                Page = values["pageIndex"].ToString();
            }
            if (Page == null)
                Page = "1";
            if (!values.ContainsKey("controller") || !values["controller"].ToString().Equals("Specials", StringComparison.OrdinalIgnoreCase))
                return null;
            if (!values.ContainsKey("action") || 
                (!values["action"].ToString().Equals("Index", StringComparison.OrdinalIgnoreCase)
                && !values["action"].ToString().Equals("List", StringComparison.OrdinalIgnoreCase)))
                return null;

            if (!string.IsNullOrEmpty(ztId))
            {
                SpecialEntity specialEntity = specialApp.GetEntityByEnCode(ztId, a => a.F_EnabledMark == true);
                if (specialEntity == null || !Validate.IsGuid(specialEntity.F_Id))
                    throw new ArgumentNullException("Specials");
                var path = "zt/" + specialEntity.F_EnCode;
                if (Page + "" != "1")
                    path = path + "/page-" + Page;
                return new VirtualPathData(this, path.ToLowerInvariant());
            }
            else
            {
                var path = "zt";
                return new VirtualPathData(this, path.ToLowerInvariant());
            }
        }
    }
    #endregion
}