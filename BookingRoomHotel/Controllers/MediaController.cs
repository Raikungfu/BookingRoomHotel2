using Microsoft.AspNetCore.Mvc;

namespace BookingRoomHotel.Controllers
{
    public class MediaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
