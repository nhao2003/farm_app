using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vn.com.SE310.P11.WebApplication.Models;

namespace vn.com.SE310.P11.WebApplication.Controllers
{
    public class CatalogController : Controller
    {
        // GET: Catalog
        public ActionResult Index()
        {
            string connection = "Data Source=localhost;Initial Catalog=QuanLySanPham;User ID=SA;Password=Admin@123";
            QuanLySanPhamDataContext context = new QuanLySanPhamDataContext(connection);
            List<Catalog> dsCatalog = context.Catalogs.ToList();
            return View(dsCatalog);
        }
    }
}