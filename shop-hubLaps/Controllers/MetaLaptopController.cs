using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shop_hubLaps.Models;
using System.Linq;

namespace shop_hubLaps.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MetaLaptopController : Controller
    {
        private readonly DataModel _context;

        public MetaLaptopController(DataModel context)
        {
            _context = context;
        }
       

        // GET: MetaLaptop/Index
        public async Task<IActionResult> Index()
        {
            var metaLaptops = await _context.MetaLaptops.Include(m => m.Laptop).ToListAsync();
            return View(metaLaptops);
        }


        // GET: MetaLaptop/Create
        public IActionResult Create()
        {
            ViewBag.LaptopList = _context.Laptops.ToList();

            var laptopList = _context.Laptops.ToList();

            if (laptopList == null || !laptopList.Any())
            {
                TempData["ErrorMessage"] = "Không có laptop nào trong hệ thống.";
            }

            return View(new MetaLaptop());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("mameta,keymeta,valuemeta,malaptop")] MetaLaptop metaLaptop)
        {
            // Populate the laptop list in ViewBag in case of error
            ViewBag.LaptopList = _context.Laptops.ToList();

            // Validate malaptop (it should not be null or 0)
            if (metaLaptop.malaptop == 0 || !_context.Laptops.Any(l => l.malaptop == metaLaptop.malaptop))
            {
                TempData["ErrorMessage"] = "Laptop không hợp lệ!";
                return RedirectToAction(nameof(Index));  // Redirect to Index page after error
            }

            try
            {
                if (ModelState.IsValid)
                {
                    // Ensure malaptop is a valid value
                    if (!_context.Laptops.Any(l => l.malaptop == metaLaptop.malaptop))
                    {
                        TempData["ErrorMessage"] = "Laptop không hợp lệ!";
                        return RedirectToAction(nameof(Index));
                    }

                    // Save the MetaLaptop object to the database
                    _context.Add(metaLaptop);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Thêm MetaLaptop thành công!";
                }
                else
                {
                    var validationErrors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    TempData["ErrorMessage"] = "Dữ liệu không hợp lệ! Lỗi: " + validationErrors;
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra: " + ex.ToString();
            }

            // Redirect to the Index page
            return RedirectToAction("Index", "MetaLaptop");
        }


        // GET: MetaLaptop/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "ID không hợp lệ!";
                return RedirectToAction(nameof(Index));
            }

            var metaLaptop = await _context.MetaLaptops
                .Include(m => m.Laptop)
                .FirstOrDefaultAsync(m => m.mameta == id);

            if (metaLaptop == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy meta laptop với ID này!";
                return RedirectToAction(nameof(Index));
            }

            return View(metaLaptop);
        }
        // GET: MetaLaptop/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "ID không hợp lệ!";
                return RedirectToAction(nameof(Index));
            }

            var metaLaptop = await _context.MetaLaptops.FindAsync(id);
            if (metaLaptop == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy meta laptop cần sửa!";
                return RedirectToAction(nameof(Index));
            }

            var laptopList = await _context.Laptops.ToListAsync();  // Fetch laptop list asynchronously
            if (laptopList == null || !laptopList.Any())
            {
                TempData["ErrorMessage"] = "Không có laptop nào trong hệ thống.";
            }

            ViewData["LaptopList"] = new SelectList(laptopList, "malaptop", "tenlaptop", metaLaptop.malaptop);
            return View(metaLaptop);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("mameta,keymeta,valuemeta,malaptop")] MetaLaptop metaLaptop)
        {
            if (id != metaLaptop.mameta)
            {
                TempData["ErrorMessage"] = "ID không khớp!";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(metaLaptop);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật meta laptop thành công!";
                    return RedirectToAction(nameof(Index));  // Redirect to index after success
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MetaLaptopExists(metaLaptop.mameta))
                    {
                        TempData["ErrorMessage"] = "Không tìm thấy meta laptop để cập nhật!";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Có lỗi xảy ra khi cập nhật meta laptop!";
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi cập nhật: " + ex.Message;
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Dữ liệu không hợp lệ!";
            }

            // Repopulate the dropdown list if there is an error
            ViewData["LaptopList"] = new SelectList(_context.Laptops, "malaptop", "tenlaptop", metaLaptop.malaptop);
            return View(metaLaptop);  // Return to the form with error message
        }


        // GET: MetaLaptop/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "ID không hợp lệ!";
                return RedirectToAction(nameof(Index));
            }

            var metaLaptop = await _context.MetaLaptops
                .Include(m => m.Laptop)
                .FirstOrDefaultAsync(m => m.mameta == id);

            if (metaLaptop == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy meta laptop cần xóa!";
                return RedirectToAction(nameof(Index));
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

            return RedirectToAction(nameof(Index));  // Always return to the Index action
        }

        private bool MetaLaptopExists(int id)
        {
            return _context.MetaLaptops.Any(e => e.mameta == id);
        }
    }
}
