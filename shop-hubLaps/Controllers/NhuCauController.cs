using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shop_hubLaps.Models;

namespace shop_hubLaps.Controllers
{
    public class NhuCauController : Controller
    {
        private readonly DataModel _context;

        public NhuCauController(DataModel context)
        {
            _context = context;
        }

        // GET: NhuCau
        public async Task<IActionResult> Index()
        {
            var nhuCaus = await _context.NhuCaus.ToListAsync();
            return View(nhuCaus);
        }

        // GET: NhuCau/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NhuCau/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("manhucau,tennhucau")] NhuCau nhuCau)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(nhuCau);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Thêm mới nhu cầu thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra: " + ex.Message;
                }
            }
            return View(nhuCau);
        }

        // GET: NhuCau/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhuCau = await _context.NhuCaus.FindAsync(id);
            if (nhuCau == null)
            {
                return NotFound();
            }
            return View(nhuCau);
        }

        // POST: NhuCau/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("manhucau,tennhucau")] NhuCau nhuCau)
        {
            if (id != nhuCau.manhucau)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nhuCau);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật nhu cầu thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NhuCauExists(nhuCau.manhucau))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra: " + ex.Message;
                }
            }
            return View(nhuCau);
        }

        // GET: NhuCau/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "ID không hợp lệ.";
                return RedirectToAction(nameof(Index));
            }

            var nhuCau = await _context.NhuCaus
                .FirstOrDefaultAsync(m => m.manhucau == id);
            if (nhuCau == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy nhu cầu.";
                return RedirectToAction(nameof(Index));
            }

            return View(nhuCau);
        }

        // POST: NhuCau/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var nhuCau = await _context.NhuCaus.FindAsync(id);
                if (nhuCau != null)
                {
                    _context.NhuCaus.Remove(nhuCau);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Xóa nhu cầu thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Không tìm thấy nhu cầu cần xóa.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        private bool NhuCauExists(int id)
        {
            return _context.NhuCaus.Any(e => e.manhucau == id);
        }
    }
}
