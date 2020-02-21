using System.Web;
using System.Web.Mvc;

namespace Test
{
    /// <summary>
    /// FilterConfig class
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// Register filters
        /// </summary>
        /// <param name="filters">GlobalFilterCollection</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
