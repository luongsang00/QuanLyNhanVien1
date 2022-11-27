using static QuanLyNhanVien.Controllers.NhanViensController;

namespace QuanLyNhanVien.Models
{
    public class ChiTietNhanVien
    {
        
        public int Id { get; set; }
        public string Ten { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
        public int IdLoaiNv { get; set; }
        
        //public string Skill { get; set; }
        public List<DanhSachKyNang> Skill { get; set; }
        public string ChucVu { get; set; }
       

    }
}
