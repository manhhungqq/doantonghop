using System.Linq;
using System.Web.Mvc;
using qldoansvhutech.Models;
using PagedList;
using System.Web;
using System.IO;
using qldoansvhutech.Areas.Admin.Controllers.CustomAttributes;
using System.EnterpriseServices;

namespace qldoansvhutech.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        QLDADataContext db = new QLDADataContext();
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SinhVien(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.Sinhviens.ToList().OrderBy(n => n.Id).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Delete()
        {

            return View();
        }
        public ActionResult Details(int? id)
        {
            Sinhvien sv = db.Sinhviens.SingleOrDefault(n => n.Id == id);
            ViewBag.Id = sv.Id;
            if (sv == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sv);
        }
        public ActionResult Edit(int? id)
        {
            Sinhvien sv = db.Sinhviens.SingleOrDefault(p => p.Id == id);
            var dsGiangVien = db.Gvhds.ToList();
            ViewBag.DSGV = dsGiangVien;
            ViewBag.Id = sv.Id_gv;
            ViewBag.Id = sv.Id;
            if (sv == null)
            {
                Response.StatusCode = 404;
                return null;
            }
          
            return View();
        }
        [AdminAuthorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Sinhvien sv, HttpPostedFileBase fileUpload)
        {
            if (ModelState.IsValid)
            {
                var obj = db.Sinhviens.SingleOrDefault(p => p.Id == sv.Id);
                obj.Hoten = sv.Hoten;
                obj.NgaySinh = sv.NgaySinh;
                obj.Gioitinh = sv.Gioitinh;
                obj.Lop = sv.Lop;
                obj.Id_gv = int.Parse(sv.Id_gv.ToString());
                //obj.Id_gv = sv.Id_gv;
                db.SubmitChanges();
                return RedirectToAction("SinhVien");
            }
            return View(sv);
        }
        public ActionResult DoAn(int? page)
        {
            {
                int pageNumber = (page ?? 1);
                int pageSize = 7;
                return View(db.Doans.ToList().Where(n => n.Trangthai == true).ToPagedList(pageNumber, pageSize));
            }
        }
        public ActionResult XoaDA(int? id)
        {
            Doan da = db.Doans.SingleOrDefault(n => n.Id == id);
            ViewBag.Id = da.Id;
            if (da == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(da);
        }
        [HttpPost, ActionName("XoaDA")]
        public ActionResult XacNhanXoa(int? id)
        {
            Doan da = db.Doans.SingleOrDefault(n => n.Id == id);
            ViewBag.Id = da.Id;
            if (da == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.Doans.DeleteOnSubmit(da);
            db.SubmitChanges();
            return RedirectToAction("DoAn");
        }
        public ActionResult GiangVien(int? page)
        {
            {
                int pageNumber = (page ?? 1);
                int pageSize = 7;
                return View(db.Gvhds.ToList().OrderBy(n => n.Id).ToPagedList(pageNumber, pageSize));
            }
        }
    }
}