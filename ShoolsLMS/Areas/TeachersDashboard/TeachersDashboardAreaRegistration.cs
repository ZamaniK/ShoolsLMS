using System.Web.Mvc;

namespace ShoolsLMS.Areas.TeachersDashboard
{
    public class TeachersDashboardAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "TeachersDashboard";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "TeachersDashboard_default",
                "TeachersDashboard/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}