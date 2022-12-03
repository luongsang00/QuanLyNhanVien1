using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyNhanVien.Models;
using static QuanLyNhanVien.Controllers.NhanViensController;

namespace QuanLyNhanVien.Controllers
{
    public class ChiTietKyNangsController : Controller
    {
        private readonly quanlynhanvienContext _context;

        public ChiTietKyNangsController(quanlynhanvienContext context)
        {
            _context = context;
        }
        public class Kynang
        {

            
            public string TenNhanVien { get; set; }
            public string TenKyNang { get; set; }
           


        }
        // GET: ChiTietKyNangs
        public async Task<IActionResult> Index()
        {
            List<Kynang> Kynang = await (from N in _context.NhanViens
                                               join C in _context.ChiTietKyNangs on N.Id equals C.IdNhanVien
                                               join K in _context.KyNangs on C.IdKyNang equals K.IdKyNang
                                               
                                               select new Kynang
                                               {
                                                   TenNhanVien=N.Ten,
                                                   TenKyNang = K.TenLoaiKn,
                                               }).ToListAsync();
            return View(Kynang);
        }

        // GET: ChiTietKyNangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ChiTietKyNangs == null)
            {
                return NotFound();
            }

            var chiTietKyNang = await _context.ChiTietKyNangs
                .FirstOrDefaultAsync(m => m.IdNhanVien == id);
            if (chiTietKyNang == null)
            {
                return NotFound();
            }

            return View(chiTietKyNang);
        }

        // GET: ChiTietKyNangs/Create
        public IActionResult Create()
        {
            //tạo select cho kỹ năng
            List<KyNang> dep = _context.KyNangs.ToList();
            SelectList ListKyNang = new SelectList(dep, "IdKyNang", "TenLoaiKn");
            ViewBag.LoaiKnlist = ListKyNang;
            //tạo select cho id
            List<NhanVien> Nv = _context.NhanViens.ToList();
            SelectList ListNhanVien = new SelectList(Nv, "Id", "Ten");
            ViewBag.LoaiNvlist = ListNhanVien;
            return View();
        }

        // POST: ChiTietKyNangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdNhanVien,IdKyNang")] ChiTietKyNang chiTietKyNang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chiTietKyNang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(chiTietKyNang);
        }

        // GET: ChiTietKyNangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ChiTietKyNangs == null)
            {
                return NotFound();
            }

            var chiTietKyNang = await _context.ChiTietKyNangs.FindAsync(id);
            if (chiTietKyNang == null)
            {
                return NotFound();
            }
            return View(chiTietKyNang);
        }

        // POST: ChiTietKyNangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdNhanVien,IdKyNang")] ChiTietKyNang chiTietKyNang)
        {
            if (id != chiTietKyNang.IdNhanVien)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chiTietKyNang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChiTietKyNangExists(chiTietKyNang.IdNhanVien))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(chiTietKyNang);
        }

        // GET: ChiTietKyNangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ChiTietKyNangs == null)
            {
                return NotFound();
            }

            var chiTietKyNang = await _context.ChiTietKyNangs
                .FirstOrDefaultAsync(m => m.IdNhanVien == id);
            if (chiTietKyNang == null)
            {
                return NotFound();
            }

            return View(chiTietKyNang);
        }

        // POST: ChiTietKyNangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ChiTietKyNangs == null)
            {
                return Problem("Entity set 'quanlynhanvienContext.ChiTietKyNangs'  is null.");
            }
            var chiTietKyNang = await _context.ChiTietKyNangs.FindAsync(id);
            if (chiTietKyNang != null)
            {
                _context.ChiTietKyNangs.Remove(chiTietKyNang);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool ChiTietKyNangExists(int id)
        {
          return _context.ChiTietKyNangs.Any(e => e.IdNhanVien == id);
        }
        private void AddNewEmp(NhanVien nhanVien)
        {
            //Keyword transaction
            // Model taoj moi nhan vien: thong tin nhan vien, skill
            try
            {
                // Add new NV
                var nv = new NhanVien();
                _context.Add(nv);
                _context.SaveChanges();

                var idnv = nv.Id;


                // Add Skill
                List<ChiTietKyNang> kyNangs = new List<ChiTietKyNang>();

                foreach (var item in kyNangs)
                {
                    //var skill = new ChiTietKyNang()
                    //{
                    //    // idnv  = idnv
                    //}
                }
                // Cach viet dung lambar ex
                //kyNangs.ForEach(item => {
                //    _context.Add(nv);
                //    _context.SaveChanges();
                //});
            }
            catch(Exception ex)
            {

            }



        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Ten,DiaChi,SoDienThoai,IdLoaiNv")] NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nhanVien);
                await _context.SaveChangesAsync();
                var a = _context.NhanViens.Where(x => x.Ten == nhanVien.Ten).FirstOrDefault();
                var idn = a.Id;
                //var idnv = nhanVien.Id;
                ////await _context.SaveChangesAsync();

                //// Add Skill
                //List<KyNang> kyNangs = new List<KyNang>();

                //foreach (var item in kyNangs)
                //{
                //    var skill = new ChiTietKyNang()
                //    {
                //        IdNhanVien = idn,
                //        IdKyNang = item.IdKyNang,
                //    };
                //    _context.Add(skill);
                //    await _context.SaveChangesAsync();

                //    //    //{
                //    //    //    // idnv  = idnv
                //    //    //}
                //}
                async Task<IActionResult> Create([Bind("IdNhanVien,IdKyNang")] ChiTietKyNang chiTietKyNang)
                {
                    if (ModelState.IsValid)
                    {
                        chiTietKyNang.IdNhanVien= idn;
                        _context.Add(chiTietKyNang);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    return View(chiTietKyNang);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(nhanVien);
        }
    }
}
