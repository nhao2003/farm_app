using System.Web;
using System.Web.Mvc;

namespace vn.com.SE310.P11.WebApplication
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
