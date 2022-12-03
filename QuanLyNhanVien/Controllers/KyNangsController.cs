using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyNhanVien.Models;
using X.PagedList;

namespace QuanLyNhanVien.Controllers
{
    public class KyNangsController : Controller
    {
        private readonly quanlynhanvienContext _context;

        public KyNangsController(quanlynhanvienContext context)
        {
            _context = context;
        }

        // GET: KyNangs
        public IActionResult Index(int ? page)
        {
            if (page == null) page = 1;
            
            
            //var books = _context.KyNangs.Include(b => b.TenLoaiKn).OrderBy(b => b.IdKyNang);
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            var kynang = _context.KyNangs.ToPagedList(pageNumber,pageSize);
            return View(kynang);
        }

        // GET: KyNangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.KyNangs == null)
            {
                return NotFound();
            }

            var kyNang = await _context.KyNangs
                .FirstOrDefaultAsync(m => m.IdKyNang == id);
            if (kyNang == null)
            {
                return NotFound();
            }

            return View(kyNang);
        }

        // GET: KyNangs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: KyNangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdKyNang,TenLoaiKn")] KyNang kyNang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kyNang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kyNang);
        }

        // GET: KyNangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.KyNangs == null)
            {
                return NotFound();
            }

            var kyNang = await _context.KyNangs.FindAsync(id);
            if (kyNang == null)
            {
                return NotFound();
            }
            return View(kyNang);
        }

        // POST: KyNangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdKyNang,TenLoaiKn")] KyNang kyNang)
        {
            if (id != kyNang.IdKyNang)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kyNang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KyNangExists(kyNang.IdKyNang))
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
            return View(kyNang);
        }

        // GET: KyNangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.KyNangs == null)
            {
                return NotFound();
            }

            var kyNang = await _context.KyNangs
                .FirstOrDefaultAsync(m => m.IdKyNang == id);
            if (kyNang == null)
            {
                return NotFound();
            }

            return View(kyNang);
        }

        // POST: KyNangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.KyNangs == null)
            {
                return Problem("Entity set 'quanlynhanvienContext.KyNangs'  is null.");
            }
            var kyNang = await _context.KyNangs.FindAsync(id);
            if (kyNang != null)
            {
                _context.KyNangs.Remove(kyNang);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KyNangExists(int id)
        {
          return _context.KyNangs.Any(e => e.IdKyNang == id);
        }
    }
}
