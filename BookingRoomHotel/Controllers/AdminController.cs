using BookingRoomHotel.Models;
using BookingRoomHotel.Models.ModelsInterface;
using BookingRoomHotel.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BookingRoomHotel.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        public AdminController(ApplicationDbContext context, ITokenService tokenService, IConfiguration configuration){ 
            _context = context;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            string message = TempData["Message"] as string;
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message;
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromForm] StaffLoginViewModel model)
        {
            var staff = _context.Staffs.Find(model.UserName);
            if (ModelState.IsValid && staff != null)
            {
                if (staff.Pw.Equals(model.Password))
                {
                    TempData["Message"] = "Login Successful!";
                    return Json(new { success = true, accessToken = _tokenService.GenerateAccessToken(staff.Id, staff.Name, staff.Role), role = staff.Role, name = staff.Name}); ;
                }
            }
            TempData["Message"] = "Login Failed!";
            return Json(new { success = false });
        }

        public ViewResult Dashboard()
        {
            return View();
        }
    }
}
