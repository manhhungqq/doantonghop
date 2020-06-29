using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using qldoansvhutech.Models;

namespace qldoansvhutech.Controllers
{
    public class SinhVienController : Controller
    {
        QLDADataContext db = new QLDADataContext();

        // GET: SinhVien
        public ActionResult ThongTinSinhVien()
        {  
            return View();
        }
        [HttpPost]
        public ActionResult SuaSV(FormCollection f)
        {
            var id = f["txtId"].ToString();
            Sinhvien a = db.Sinhviens.Where(m => m.Id == int.Parse(id)).SingleOrDefault();
            a.Hoten = f["txtHoTen"].ToString();
            a.Email = f["txtEmail"].ToString();
            a.Sdt = f["txtSDT"].ToString();
            string gt = f["chkGT"].ToString();
            bool gtinh = false;
            if (gt.CompareTo("1") == 0)
            {
                gtinh = true;
            }
            a.Gioitinh = gtinh;
            db.SubmitChanges();
            return RedirectToAction("ThongTinSinhVien");
        }
      
        public ActionResult Dangkydoan()
        {
            var dsMaLoai = db.LoaiDAs.ToList();
            ViewBag.DSDA = dsMaLoai;
            Sinhvien sv = (Sinhvien) Session["Taikhoan"];
            ViewBag.Id = sv.Id;
            return View();
        }
        [HttpPost]
        public ActionResult Dangkydoan(FormCollection f)
        {
            Sinhvien sv = (Sinhvien)Session["Taikhoan"];
            Doan da = new Doan();
            if(da !=null)
            {
                    da.Tenda = f["txtTenDoAn"].ToString();
                    da.Mota = f["txtMoTa"].ToString();
                    //da.Id_gv = int.Parse(f["radMaGV"].ToString());
                    da.Id_lda = int.Parse(f["radMaLoai"].ToString());
                    da.Id_sv = sv.Id;
                da.Id_gv = sv.Id_gv;
                    db.Doans.InsertOnSubmit(da);
                    db.SubmitChanges();
                    return RedirectToAction("Dangkydoan");
              }
            return View();
        }
        public ActionResult ThongTinDoAn()
        {
            Sinhvien sv = (Sinhvien)Session["Taikhoan"];
            var doAn = db.Doans.Where(n => n.Id_sv == sv.Id);
            return View(doAn);
        }
        public ActionResult Chitietda(int ? id)
        {
            Doan da = db.Doans.SingleOrDefault(o => o.Id == id);
            return View(da);
        }
        public ActionResult Chitietcv(int?id)
        {
            Congviectuan cv = db.Congviectuans.SingleOrDefault(w => w.Id == id);
            return View(cv);
        }
   
        public ActionResult Congviecduocgiao()
        {
            Sinhvien sv = (Sinhvien)Session["Taikhoan"];
            var dsCongViec = db.Congviectuans.Where(n => n.Id_sv == sv.Id).ToList();
            return View(dsCongViec);
        }
        public ActionResult Baocaotuan()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Baocaotuan(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/fileUploaded/"), fileName);
                file.SaveAs(path);
            }
            return RedirectToAction("ThongTinDoAn");
        }
    }
}