using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shop_hubLaps.Models;

namespace shop_hubLaps.Controllers
{
    public class MetaLaptopController : Controller
    {
        private readonly DataModel _context;

        public MetaLaptopController(DataModel context)
        {
            _context = context;
        }

        // GET: MetaLaptop
        //public IActionResult Create()
        //{
        //    // Include Hang (brand) when retrieving laptops
        //    var laptopsWithBrands = _context.Laptops.Include(l => l.Hang).ToList();
        //    ViewData["LaptopList"] = new SelectList(laptopsWithBrands, "malaptop", "tenlaptop", "Hang.tenhang");
        //    return View(new MetaLaptop());
        //}
        // GET: MetaLaptop/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metaLaptop = await _context.MetaLaptops
                .Include(m => m.Laptop)
                .FirstOrDefaultAsync(m => m.mameta == id);

            if (metaLaptop == null)
            {
                return NotFound();
            }

            return View(metaLaptop);
        }

        // GET: MetaLaptop/Create
        public IActionResult Create()
        {
            // Pass laptop list to the view
            ViewData["LaptopList"] = new SelectList(_context.Laptops, "malaptop", "tenlaptop");
            return View(new MetaLaptop());
        }

        // POST: MetaLaptop/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("mameta,keymeta,valuemeta,malaptop")] MetaLaptop metaLaptop)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(metaLaptop);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Thêm meta laptop thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra: " + ex.Message;
                }
            }

            // Ensure the laptop list is passed back in case of error
            ViewData["LaptopList"] = new SelectList(_context.Laptops, "malaptop", "tenlaptop", metaLaptop.malaptop);
            return View(metaLaptop);
        }

        // GET: MetaLaptop/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metaLaptop = await _context.MetaLaptops.FindAsync(id);
            if (metaLaptop == null)
            {
                return NotFound();
            }

            ViewData["LaptopList"] = new SelectList(_context.Laptops, "malaptop", "tenlaptop", metaLaptop.malaptop);
            return View(metaLaptop);
        }

        // POST: MetaLaptop/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("mameta,keymeta,valuemeta,malaptop")] MetaLaptop metaLaptop)
        {
            if (id != metaLaptop.mameta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(metaLaptop);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật meta laptop thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MetaLaptopExists(metaLaptop.mameta))
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

            ViewData["LaptopList"] = new SelectList(_context.Laptops, "malaptop", "tenlaptop", metaLaptop.malaptop);
            return View(metaLaptop);
        }

        // GET: MetaLaptop/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metaLaptop = await _context.MetaLaptops
                .Include(m => m.Laptop)
                .FirstOrDefaultAsync(m => m.mameta == id);

            if (metaLaptop == null)
            {
                return NotFound();
            }

            return View(metaLaptop);
        }

        // POST: MetaLaptop/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var metaLaptop = await _context.MetaLaptops.FindAsync(id);
                if (metaLaptop != null)
                {
                    _context.MetaLaptops.Remove(metaLaptop);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Xóa meta laptop thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Không tìm thấy meta laptop cần xóa.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        private bool MetaLaptopExists(int id)
        {
            return _context.MetaLaptops.Any(e => e.mameta == id);
        }
    }
}
