using System.Web.Mvc;

namespace NFine.Web.Areas.MasterManage
{
    public class MasterManageAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "MasterManage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "MasterManage_default",
                "MasterManage/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
