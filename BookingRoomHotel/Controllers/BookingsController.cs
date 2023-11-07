using BookingRoomHotel.Models;
using BookingRoomHotel.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace BookingRoomHotel.Controllers
{
    public class BookingsController : Controller
    {
        // GET: BookingsController
        private readonly ApplicationDbContext _context;
        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bookings
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Index()
        {
            return _context.Bookings != null ?
                          PartialView(await getListViewBooking("1")) :
                          Problem("Entity set 'ApplicationDbContext.Bookings'  is null.");
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Index(string id)
        {
            return _context.Bookings != null ?
                          PartialView(await getListViewBooking(id)) :
                          Problem("Entity set 'ApplicationDbContext.Bookings'  is null.");
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .FirstOrDefaultAsync(m => m.BookingID == int.Parse(id));
            if (booking == null)
            {
                return NotFound();
            }

            return PartialView(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            return PartialView();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create([FromForm][Bind("Id,Name,Email,Phone,DateOfBirth,Address,Pw,Role")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                booking.Status = "Action";
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return PartialView(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return PartialView(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(string id, [FromForm][Bind("Id,Name,Email,Phone,DateOfBirth,Address,Pw,Role")] Booking booking)
        {
            if (int.Parse(id) != booking.BookingID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!bookingExists(booking.BookingID.ToString()))
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
            return PartialView(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .FirstOrDefaultAsync(m => m.BookingID == int.Parse(id));
            if (booking == null)
            {
                return NotFound();
            }

            return PartialView(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed([FromForm] string id)
        {
            if (_context.Bookings == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Bookings'  is null.");
            }
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool bookingExists(string id)
        {
            return (_context.Bookings?.Any(e => e.BookingID == int.Parse(id))).GetValueOrDefault();
        }

        public async Task<BookingViewModel> getListViewBooking(string id)
        {
            BookingViewModel listBooking = new BookingViewModel();
            listBooking.ListBooking = await _context.Bookings.OrderByDescending(x => x.BookingID).Skip(6 * (int.Parse(id) - 1)).Take(6).ToListAsync();
            int total = await _context.Bookings.CountAsync();
            listBooking.Count = total % 6 == 0 ? total / 6 : total / 6 + 1;
            return listBooking;
        }

        [HttpGet]
		public async Task<IActionResult> GetListBookingJson()
		{
			try
			{
				var bookings = await _context.Bookings.Include(r => r.Room.RoomType).Select(booking => new
				{
					BookingID = booking.BookingID,
					CheckInDate = booking.CheckInDate,
					CheckOutDate = booking.CheckOutDate,
					RoomID = booking.Room.RoomNumber,
					CustomerId = booking.CustomerId,
					TotalPrice = booking.TotalPrice,
					Status = booking.Status,
                    RoomType = booking.Room.RoomType.TypeName
				}).ToListAsync();

				JsonSerializerOptions options = new JsonSerializerOptions
				{
					ReferenceHandler = ReferenceHandler.IgnoreCycles,
					WriteIndented = true
				};

				var json = JsonSerializer.Serialize(bookings, options);

				return Content(json, "application/json");
			}
			catch (Exception ex)
			{
				return BadRequest("An error occurred while fetching bookings.");
			}
		}


		public string ObjectToJson(Object obj)
        {
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };
            return JsonSerializer.Serialize(obj, options);
        }

    }
}
