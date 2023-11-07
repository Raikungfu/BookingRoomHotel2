using AutoMapper;
using BookingRoomHotel.Models;
using BookingRoomHotel.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingRoomHotel.Controllers
{
    public class RoomsController : Controller
    {
        // GET: RoomsController
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUploadFileService _uploadFileService;
        public RoomsController(ApplicationDbContext context, IMapper mapper, IUploadFileService uploadFileService)
        {
            _context = context;
            _mapper = mapper;
            _uploadFileService = uploadFileService;
        }

        // GET: Rooms
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Index()
        {
            return _context.Rooms != null ?
                          PartialView(await getListViewRoom("1")) :
                          Problem("Entity set 'ApplicationDbContext.Rooms'  is null.");
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Index(string id)
        {
            return _context.Rooms != null ?
                          PartialView(await getListViewRoom(id)) :
                          Problem("Entity set 'ApplicationDbContext.Rooms'  is null.");
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .FirstOrDefaultAsync(m => m.RoomID == int.Parse(id));
            if (room == null)
            {
                return NotFound();
            }

            return PartialView(room);
        }

        // GET: Rooms/Create
        public IActionResult Create()
        {
            return PartialView();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create([FromForm][Bind("RoomNumber,RoomTypeID,Price,Describe,HotelID,Image")] CreateRoomViewModel roomC)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Room room = _mapper.Map<Room>(roomC);
                    string roomImg = _uploadFileService.uploadImage(roomC.Image, "images/Admin/Rooms");
                    room.RoomImage = roomImg == null ? room.RoomImage : roomImg;
                    _context.Add(room);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }catch(Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return PartialView(roomC);
            }
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            return PartialView(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(string id, [FromForm][Bind("Id,Name,Email,Phone,DateOfBirth,Address,Pw,Role")] Room room)
        {
            if (int.Parse(id) != room.RoomID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.RoomID.ToString()))
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
            return PartialView(room);
        }

        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .FirstOrDefaultAsync(m => m.RoomID == int.Parse(id));
            if (room == null)
            {
                return NotFound();
            }

            return PartialView(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed([FromForm] string id)
        {
            if (_context.Rooms == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Rooms'  is null.");
            }
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(string id)
        {
            return (_context.Rooms?.Any(e => e.RoomID == int.Parse(id))).GetValueOrDefault();
        }

        public async Task<RoomViewModel> getListViewRoom(string id)
        {
            RoomViewModel listRoomViewModel = new RoomViewModel();
            listRoomViewModel.ListRoom = await _context.Rooms.OrderByDescending(x => x.RoomID).Skip(6 * (int.Parse(id) - 1)).Take(6).ToListAsync();
            int total = await _context.Rooms.CountAsync();
            listRoomViewModel.Count = total % 6 == 0 ? total / 6 : total / 6 + 1;
            return listRoomViewModel;
        }

        
    }
}
