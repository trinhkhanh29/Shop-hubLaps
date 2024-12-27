using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shop_hubLaps.Models;
using System;
using System.Linq;

namespace shop_hubLaps.Controllers
{
    public class QuangCaoController : Controller
    {
        private readonly DataModel _context;

        public QuangCaoController(DataModel context)
        {
            _context = context;
        }
        
        // GET: QuangCao/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: QuangCao/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(QuangCao quangCao)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    quangCao.trangthai = false; // Mặc định trạng thái chưa kích hoạt
                    _context.QuangCaos.Add(quangCao);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Đăng ký quảng cáo thành công!";
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    // Ghi lỗi chi tiết và hiển thị cho người dùng
                    ModelState.AddModelError(string.Empty, $"Có lỗi xảy ra: {ex.Message}");
                }
            }

            // Trả về view với lỗi từ ModelState
            return View(quangCao);
        }


        [Authorize(Roles = "Admin")]
        // GET: QuangCao/Index
        public IActionResult Index()
        {
            var quangCaoList = _context.QuangCaos.ToList();
            return View(quangCaoList);
        }

        // GET: QuangCao/Create
        [Authorize(Roles = "Admin")]
        public IActionResult CreateAdmin()
        {
            return View();
        }

        // POST: QuangCao/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAdmin(QuangCao quangCao)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    quangCao.trangthai = false; // Mặc định trạng thái chưa kích hoạt
                    _context.QuangCaos.Add(quangCao);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Đăng ký quảng cáo thành công!";
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    // Ghi lỗi chi tiết và hiển thị cho người dùng
                    ModelState.AddModelError(string.Empty, $"Có lỗi xảy ra: {ex.Message}");
                }
            }

            // Trả về view với lỗi từ ModelState
            return View(Index);
        }

        [Authorize(Roles = "Admin")]
        // GET: QuangCao/Edit/{id}
        public IActionResult Edit(int id)
        {
            var quangCao = _context.QuangCaos.FirstOrDefault(q => q.maqc == id);
            if (quangCao == null)
            {
                return NotFound();
            }
            return View(quangCao);
        }

        // POST: QuangCao/Edit/{id}
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(QuangCao quangCao)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.QuangCaos.Update(quangCao);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Cập nhật quảng cáo thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Lỗi: {ex.Message}");
                }
            }
            return View(quangCao);
        }





        [Authorize(Roles = "Admin")]
        // GET: QuangCao/Details/{id}
        public IActionResult Details(int id)
        {
            var quangCao = _context.QuangCaos.FirstOrDefault(q => q.maqc == id);
            if (quangCao == null)
            {
                return NotFound();
            }
            return View(quangCao);
        }

        [Authorize(Roles = "Admin")]
        // GET: QuangCao/Delete/{id}
        public IActionResult Delete(int id)
        {
            var quangCao = _context.QuangCaos.FirstOrDefault(q => q.maqc == id);
            if (quangCao == null)
            {
                return NotFound();
            }
            return View(quangCao);
        }

        // POST: QuangCao/Delete/{id}
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var quangCao = _context.QuangCaos.FirstOrDefault(q => q.maqc == id);
            if (quangCao != null)
            {
                try
                {
                    _context.QuangCaos.Remove(quangCao);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Xóa quảng cáo thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Lỗi: {ex.Message}");
                }
            }
            return View(quangCao);
        }
    }
}
