using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace qldoansvhutech.Areas.Admin.Models
{
    public class AdminViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        public string Taikhoan { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Matkhau { get; set; }
    }
}