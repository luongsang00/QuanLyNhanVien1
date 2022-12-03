using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyNhanVien.Models;

namespace QuanLyNhanVien.Controllers
{
    public class NhanViensController : Controller
    {
        private readonly quanlynhanvienContext _context;

        public NhanViensController(quanlynhanvienContext context)
        {
            _context = context;
        }
        
        // GET: NhanViens
        public async Task<IActionResult> Index()
        {
            
            List<ChiTietNhanVien> nhanVien = await (from Nv in _context.NhanViens
                                        join LNv in _context.LoaiNhanViens on Nv.IdLoaiNv equals LNv.IdLoaiNv
                                        //join CtKn in _context.ChiTietKyNangs on Nv.Id equals CtKn.IdNhanVien
                                        //join Kn in _context.KyNangs on CtKn.IdKyNang equals Kn.IdKyNang

                                        select new ChiTietNhanVien
                                        {
                                            Id = Nv.Id,
                                            Ten = Nv.Ten,
                                            ChucVu = LNv.TenLoaiNv,
                                            SoDienThoai = Nv.SoDienThoai,
                                            //Skill = Kn.TenLoaiKn,
                                            DiaChi = Nv.DiaChi
                                        }).ToListAsync();
                
            //return View(await _context.NhanViens.ToListAsync());
            return View(nhanVien);
        }
        public class DanhSachKyNang
        {
            public int Id { get; set; }
            public string TenKyNang { get; set; }
        }
        // GET: NhanViens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NhanViens == null)
            {
                return NotFound();
            }

            //var nhanVien = await _context.NhanViens
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //ChiTietNhanVien nhanVien = await (from Nv in _context.NhanViens
            //                                  join LNv in _context.LoaiNhanViens on Nv.IdLoaiNv equals LNv.IdLoaiNv
            //                                  join CtKn in _context.ChiTietKyNangs on Nv.Id equals CtKn.IdNhanVien
            //                                  join Kn in _context.KyNangs on CtKn.IdKyNang equals Kn.IdKyNang

            //                                  select new ChiTietNhanVien
            //                                  {
            //                                      Id = Nv.Id,
            //                                      Ten = Nv.Ten,
            //                                      ChucVu = LNv.TenLoaiNv,
            //                                      SoDienThoai = Nv.SoDienThoai,
            //                                      skill = kn.tenloaikn,
            //                                      DiaChi = Nv.DiaChi
            //                                  })
            //    .FirstOrDefaultAsync(m => m.Id == id);
            List<DanhSachKyNang> a = await (from N in _context.NhanViens
                     join C in _context.ChiTietKyNangs on N.Id equals C.IdNhanVien
                     join K in _context.KyNangs on C.IdKyNang equals K.IdKyNang
                     where N.Id == id
                     select new DanhSachKyNang
                     {
                         Id = K.IdKyNang,
                         TenKyNang = K.TenLoaiKn,
                     }).ToListAsync();


            ChiTietNhanVien nhanVien = await (from LNv in _context.LoaiNhanViens
                                              join Nv in _context.NhanViens on LNv.IdLoaiNv equals Nv.IdLoaiNv
                                              join CtKn in _context.ChiTietKyNangs on Nv.Id equals CtKn.IdNhanVien
                                              join Kn in _context.KyNangs on CtKn.IdKyNang equals Kn.IdKyNang

                                              select new ChiTietNhanVien
                                              {
                                                  Id = Nv.Id,
                                                  Ten = Nv.Ten,
                                                  ChucVu = LNv.TenLoaiNv,
                                                  SoDienThoai = Nv.SoDienThoai,
                                                  //Skill = Kn.TenLoaiKn,
                                                  Skill = a,
                                                  DiaChi = Nv.DiaChi
                                              })
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nhanVien == null)
            {
                return NotFound();
            }

            return View(nhanVien);
        }

        // GET: NhanViens/Create
        public IActionResult Create()
        {
            //lấy danh sách chức vụ nhân viên
            List<LoaiNhanVien> dep = _context.LoaiNhanViens.ToList();
            SelectList ListChucVu = new SelectList(dep, "IdLoaiNv", "TenLoaiNv");
            ViewBag.LoaiNvlist = ListChucVu;
            //Lấy danh sách kỹ năng
            List<KyNang> Kn = _context.KyNangs.ToList();
            SelectList ListKyNang = new SelectList(Kn, "IdKyNang", "TenLoaiKn");
            ViewBag.LoaiKnlist = ListKyNang;
            return View();
        }

        // POST: NhanViens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Create([Bind("Id,Ten,DiaChi,SoDienThoai,IdLoaiNv")] NhanVien nhanVien, List<int> IdKyNang)
        {
            if (ModelState.IsValid)
            {     
                _context.NhanViens.Add(nhanVien);
                await _context.SaveChangesAsync();
                var id = nhanVien.Id;
                //var idn = a.Id;
                foreach (var item in IdKyNang)
                {
                    
                    _context.ChiTietKyNangs.Add(new ChiTietKyNang { 
                        IdKyNang = item, 
                        IdNhanVien = id
                    });
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nhanVien);
        }

        // GET: NhanViens/Edit/5
        public async Task<IActionResult> Edit(int? id, ChiTietKyNang chiTietKyNang)

        {
            //tạo list chức vụ
            List<LoaiNhanVien> dep = _context.LoaiNhanViens.ToList();
            SelectList ListChucVu = new SelectList(dep, "IdLoaiNv", "TenLoaiNv");
            ViewBag.LoaiNvlist = ListChucVu;
            //tạo list kỹ năng
            List<KyNang> kn = _context.KyNangs.ToList();
            SelectList ListKyNang = new SelectList(kn, "IdKyNang", "TenLoaiKn");
            ViewBag.LoaiKnlist = ListKyNang;



            if (id == null || _context.NhanViens == null)
            {
                return NotFound();
            }



            var nhanVien = await _context.NhanViens.FindAsync(id);
            if (nhanVien == null)
            {
                return NotFound();
            }
            return View(nhanVien);
        }

        // POST: NhanViens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ten,DiaChi,SoDienThoai,IdLoaiNv")] NhanVien nhanVien, List<int> IdKyNang)
        {
            if (id != nhanVien.Id )
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nhanVien);

                    foreach (var item in IdKyNang)
                    {

                        _context.ChiTietKyNangs.Add(new ChiTietKyNang
                        {
                            IdKyNang = item,
                            IdNhanVien = id
                        });
                    }
                    //foreach (var item in IdKyNang)
                    //{

                    //    _context.ChiTietKyNangs.Update( IdKyNang);
                    //}

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NhanVienExists(nhanVien.Id))
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
            return View(nhanVien);
        }

        // GET: NhanViens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            
            if (id == null || _context.NhanViens == null)
            {
                return NotFound();
            }
            List<DanhSachKyNang> a = await (from N in _context.NhanViens
                                            join C in _context.ChiTietKyNangs on N.Id equals C.IdNhanVien
                                            join K in _context.KyNangs on C.IdKyNang equals K.IdKyNang
                                            where N.Id == id
                                            select new DanhSachKyNang
                                            {
                                                Id = K.IdKyNang,
                                                TenKyNang = K.TenLoaiKn,
                                            }).ToListAsync();


            ChiTietNhanVien nhanVien = await (from LNv in _context.LoaiNhanViens
                                              join Nv in _context.NhanViens on LNv.IdLoaiNv equals Nv.IdLoaiNv
                                              join CtKn in _context.ChiTietKyNangs on Nv.Id equals CtKn.IdNhanVien
                                              join Kn in _context.KyNangs on CtKn.IdKyNang equals Kn.IdKyNang

                                              select new ChiTietNhanVien
                                              {
                                                  Id = Nv.Id,
                                                  Ten = Nv.Ten,
                                                  ChucVu = LNv.TenLoaiNv,
                                                  SoDienThoai = Nv.SoDienThoai,
                                                  //Skill = Kn.TenLoaiKn,
                                                  Skill = a,
                                                  DiaChi = Nv.DiaChi
                                              })
                .FirstOrDefaultAsync(m => m.Id == id);
            //var nhanVien = await _context.NhanViens
            //    .FirstOrDefaultAsync(m => m.Id == id);
            if (nhanVien == null)
            {
                return NotFound();
            }

            return View(nhanVien);
        }

        // POST: NhanViens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NhanViens == null)
            {
                return Problem("Entity set 'quanlynhanvienContext.NhanViens'  is null.");
            }
            var nhanVien = await _context.NhanViens.FindAsync(id);
            if (nhanVien != null)
            {
                _context.NhanViens.Remove(nhanVien);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NhanVienExists(int id)
        {
          return _context.NhanViens.Any(e => e.Id == id);
        }
    }
}
