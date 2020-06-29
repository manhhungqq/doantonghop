using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using qldoansvhutech.Models;

namespace qldoansvhutech.Controllers
{
    public class GiangVienController : Controller
    {
        private QLDADataContext db = new QLDADataContext();
        // GET: GiangVien
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult QuanLySinhVien()
        {
            // Lấy giảng viên đang đăng nhập
            Gvhd gv = (Gvhd)Session["GiangVien"];
            // Lấy ra danh sách sinh viên hướng dẫn
            var dsSV = db.Sinhviens.Where(m => m.Id_gv == gv.Id).ToList();
            return View(dsSV);
        }
        public ActionResult  SinhVien()
        {
            Gvhd gv = (Gvhd)Session["GiangVien"];
            var dsSV = from s in db.Sinhviens where !(from a in db.Sinhviens where a.Id == gv.Id select a.Id).Contains(s.Id) select s; 
            return View(dsSV.ToList());
        }

        [HttpPost]
        public ActionResult ThemSinhVien(FormCollection f)
        {
            string listNew = f["id"].ToString();
            string[] dsId = listNew.Split(';');
            Gvhd gv = (Gvhd)Session["GiangVien"];
            foreach (string item in dsId)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    Sinhvien sv = db.Sinhviens.Where(n => n.Id == int.Parse(item)).SingleOrDefault();
                    sv.Id = gv.Id;
                    db.SubmitChanges();
                }
            }
            return RedirectToAction("QuanLySinhVien");
        }
        public ActionResult QLDoAn()
        {
            Gvhd gv = (Gvhd)Session["GiangVien"];
            var dsDA = db.Doans.Where(n=>n.Id_gv == gv.Id).ToList();
            return View(dsDA);
        }
        public ActionResult Chitietda(int ?id)
        {
            Doan da = db.Doans.SingleOrDefault(o => o.Id == id);
            return View(da);
        }
        public ActionResult Congviectuan(int ? id)
        {
            Doan da = db.Doans.SingleOrDefault(n => n.Id == id);
            Gvhd gv = (Gvhd)Session["GiangVien"];
            Sinhvien sv = new Sinhvien();
            da.Id_sv = sv.Id;
            ViewBag.Id = da.Id;
            return View();
        }
        [HttpPost]
        public ActionResult Congviectuan(FormCollection f, Congviectuan cv , int? id)
        {
            Gvhd gv = (Gvhd)Session["GiangVien"];
            Doan da = db.Doans.SingleOrDefault(n => n.Id == id);
            ViewBag.Id = da.Id;
            cv.Id_da = da.Id;
            cv.Id_sv = da.Id_sv;
            cv.Mota = f["txtMota"].ToString();
            cv.Tuan = f["txtTuan"].ToString();
            DateTime ngaybd = DateTime.Parse(f["NgayBd"].ToString());
            cv.NgayBD = ngaybd;
            DateTime ngaykt = DateTime.Parse(f["NgayKt"].ToString());
            cv.NgayKT = ngaykt;
            cv.Id_gv = gv.Id;
            db.Congviectuans.InsertOnSubmit(cv);
            db.SubmitChanges();
            return RedirectToAction("QLDoan");
        }
        public ActionResult Chitietcv(int? id)
        {
            Congviectuan cv = db.Congviectuans.SingleOrDefault(w => w.Id == id);
            return View(cv);
        }
        public ActionResult Lichcongviecsv(Sinhvien sv)
        {
            Gvhd gv = (Gvhd)Session["GiangVien"];
            sv.Id_gv = gv.Id;
            var dsCongViec = db.Congviectuans.Where(n => n.Id_sv == sv.Id).ToList();
            return View(dsCongViec);
        }
    }
}
