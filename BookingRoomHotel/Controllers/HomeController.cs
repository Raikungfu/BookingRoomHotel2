using AutoMapper;
using BookingRoomHotel.Models;
using BookingRoomHotel.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace BookingRoomHotel.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private CustomerViewModel customerViewModel;
        public HomeController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // GET: HomeController
        public async Task<ActionResult> Index()
        {
            customerViewModel = new CustomerViewModel();
            HomeViewModel homeViewModel = new HomeViewModel();
            try
            {
                homeViewModel.ListRoomsRandom = await _context.Rooms.OrderBy(x => new Guid()).Take(3).ToListAsync();
                customerViewModel.HomeViewModel = homeViewModel;
                return View(customerViewModel);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error");
            }
        }

        public ActionResult Contact()
        {
            return View();
        }
        public IActionResult Error()
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

        [HttpPost]
        public async Task<IActionResult> Index([FromForm][Bind("CheckInDate,CheckOutDate,RoomType,AreaFrom,PriceFrom")] SearchRoomViewModel search)
        {
            if (!ModelState.IsValid)
            {
                customerViewModel = new CustomerViewModel();
                HomeViewModel homeViewModel = new HomeViewModel();
                try
                {
                    homeViewModel.ListRoomsRandom = await _context.Rooms.OrderBy(x => new Guid()).Take(3).ToListAsync();
                    customerViewModel.HomeViewModel = homeViewModel;
                    customerViewModel.SearchRoomViewModel = search;
                    return View(customerViewModel);
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Error");
                }
            }
            return RedirectToAction("Rooms", search);
        }

        public async Task<RoomViewModel> searchListRoom(SearchRoomViewModel search)
        {
            RoomViewModel listRoomViewModel = new RoomViewModel();
            var searchCheckInDate = search.CheckInDate == null ? search.CheckOutDate : search.CheckInDate;
            var searchCheckOutDate = search.CheckOutDate == null ? search.CheckInDate : search.CheckOutDate;

            var bookedRoomIds = _context.Bookings.Where(booking => booking.Status == "Booked" && (searchCheckInDate <= booking.CheckOutDate && searchCheckOutDate >= booking.CheckInDate)).Select(booking => booking.RoomID).Distinct();

            var query = _context.Rooms.Where(room => !bookedRoomIds.Contains(room.RoomID));

            if (search.RoomType.HasValue)
            {
                query = query.Include(r => r.RoomType).Where(room => room.RoomTypeID == search.RoomType);
                if (search.Adult.HasValue)
                {
                    query = query.Where(room => room.RoomType.Max >= search.Adult.Value);
                }

                if (search.AreaFrom.HasValue)
                {
                    query = query.Where(room => room.RoomType.Size >= search.AreaFrom.Value);
                }

                if (search.AreaTo.HasValue)
                {
                    query = query.Where(room => room.RoomType.Size <= search.AreaTo.Value);
                }
            }

            if (search.PriceFrom.HasValue)
            {
                query = query.Where(room => room.Price >= search.PriceFrom.Value);
            }

            if (search.PriceTo.HasValue)
            {
                query = query.Where(room => room.Price <= search.PriceTo.Value);
            }

            listRoomViewModel.ListRoom = query.Take(6).ToList();
            int total = query.Count();
            listRoomViewModel.Count = total % 6 == 0 ? total / 6 : total / 6 + 1;

            return listRoomViewModel;
        }

        public async Task<IActionResult> SearchRoom([FromForm] SearchRoomViewModel search)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    RoomViewModel roomViewModel = await searchListRoom(search);
                    return PartialView(roomViewModel);
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Error");
                }
            }
            return BadRequest(search);
        }
        public async Task<IActionResult> Rooms(SearchRoomViewModel search)
        {
            SearchListRoomViewModel roomViewModel = new SearchListRoomViewModel
            {
                roomViewModel = await searchListRoom(search),
                searchRoomViewModel = search
        };
            return View(roomViewModel);
        }

        public async Task<IActionResult> Booking(BookViewModel booking)
        {
             if (ModelState.IsValid)
            {
                try
                {
                    Booking book = _mapper.Map<Booking>(booking);
                    Room room = _context.Rooms.Find(booking.RoomID);
                    book.Status = "Pending";
                    book.TotalPrice = room.Price;
                    await _context.AddAsync(book);
                    CustomerNotification noti = new CustomerNotification
                    {
                        CreatedAt = DateTime.Now,
                        Title = "Booking Room Successful!",
                        Content = $"Room number : {room.RoomNumber}, Total Price: {book.TotalPrice}$",
                        CustomerId = booking.CustomerId
                    };
                    await _context.AddAsync(noti);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = "Booking Successful! Check your notification...", noti = ObjectToJson(noti)});
                }catch (Exception ex) {
                    return Json(new { BookingError = "Network was not OK!" });
                }
            }
            return BadRequest(ModelState);
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
