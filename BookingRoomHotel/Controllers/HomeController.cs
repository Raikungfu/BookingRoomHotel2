using BookingRoomHotel.Models;
using BookingRoomHotel.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingRoomHotel.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context) 
        {
            _context = context;
        }
        // GET: HomeController
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        // GET: HomeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HomeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HomeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HomeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        public async Task<IActionResult> RoomTypes()
        {
            var listRoomTypes = await _context.RoomTypes.ToListAsync();
            return View(listRoomTypes);
        }

        public async Task<IActionResult> RoomTypeDetail(string id)
        {
            var roomType = await _context.RoomTypes.Include(rt => rt.Media).FirstOrDefaultAsync(rt => rt.RoomTypeID == int.Parse(id));
            RoomTypeDetail roomTypeDetail = new RoomTypeDetail();
            roomTypeDetail.ListMedia = roomType.Media.ToList();
            roomTypeDetail.RoomType = roomType;
            return View(roomTypeDetail);
        }
    }
}
