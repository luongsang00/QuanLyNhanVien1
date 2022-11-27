using System;
using System.Collections.Generic;

namespace QuanLyNhanVien.Models
{
    public partial class NhanVien
    {
        public int Id { get; set; }
        public string Ten { get; set; } = null!;
        public string DiaChi { get; set; } = null!;
        public string SoDienThoai { get; set; } = null!;
        public int IdLoaiNv { get; set; }
    }
}
