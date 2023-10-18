using BookingRoomHotel.Models;
using BookingRoomHotel.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingRoomHotel.Controllers
{
    public class RoomTypesController : Controller
    {
		private readonly ApplicationDbContext _context;
		private readonly IUploadFileService _uploadFileService;
		public RoomTypesController(ApplicationDbContext context, IUploadFileService uploadFileService)
		{
			_context = context;
			_uploadFileService = uploadFileService;
		}

		// GET: RoomTypes
		[Authorize(Policy = "AdminPolicy")]
		public async Task<IActionResult> Index()
		{
			ListRoomTypeViewModel listRoomTypeViewModel = new ListRoomTypeViewModel();
			listRoomTypeViewModel.ListRoomTypes = await _context.RoomTypes.Take(6).ToListAsync();
			int total = await _context.RoomTypes.CountAsync();
			listRoomTypeViewModel.Count = total % 6 == 0 ? total / 6 : total / 6 + 1;
			return _context.RoomTypes != null ?
						  PartialView(listRoomTypeViewModel) :
						  Problem("Entity set 'ApplicationDbContext.RoomTypes'  is null.");
		}

		[HttpPost]
		[Authorize(Policy = "AdminPolicy")]
		public async Task<IActionResult> Index(string id)
		{
			ListRoomTypeViewModel listRoomTypeViewModel = new ListRoomTypeViewModel();
			listRoomTypeViewModel.ListRoomTypes = await _context.RoomTypes.Skip(6 * (int.Parse(id) - 1)).Take(6).ToListAsync();
			int total = await _context.RoomTypes.CountAsync();
			listRoomTypeViewModel.Count = total % 6 == 0 ? total / 6 : total / 6 + 1;
			return _context.RoomTypes != null ?
						  PartialView(listRoomTypeViewModel) :
						  Problem("Entity set 'ApplicationDbContext.RoomTypes'  is null.");
		}

		// GET: RoomTypes/Details/5
		public async Task<IActionResult> Details(string id)
		{
			if (id == null || _context.RoomTypes == null)
			{
				return NotFound();
			}

			var type = await _context.RoomTypes
				.FirstOrDefaultAsync(m => m.RoomTypeID == int.Parse(id));
			if (type == null)
			{
				return NotFound();
			}

			return PartialView(type);
		}

		// GET: RoomTypes/Create
		public IActionResult Create()
		{
			return PartialView();
		}

		// POST: RoomTypes/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<IActionResult> Create([FromForm][Bind("TypeName,Max,Bed,Size,View,Description1,Description2,Description3,Price,Images,VideoUrl")] CreateRoomTypeViewModel type)
		{
			if (ModelState.IsValid)
			{
				RoomType roomType = ConvertCreateRoomTypeViewModelToRoomType(type);
				_context.Add(roomType);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return PartialView(type);
		}

		// GET: RoomTypes/Edit/5
		public async Task<IActionResult> Edit(string id)
		{
			if (id == null || _context.RoomTypes == null)
			{
				return NotFound();
			}

			var staff = await _context.RoomTypes.FindAsync(id);
			if (staff == null)
			{
				return NotFound();
			}
			return PartialView(staff);
		}

		// POST: RoomTypes/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<IActionResult> Edit(string id, [FromForm][Bind("Id,Name,Email,Phone,DateOfBirth,Address,Pw,Role")] Staff staff)
		{
			if (id != staff.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(staff);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					
				}
				return RedirectToAction(nameof(Index));
			}
			return PartialView(staff);
		}

		// GET: RoomTypes/Delete/5
		public async Task<IActionResult> Delete(string id)
		{
			if (id == null || _context.RoomTypes == null)
			{
				return NotFound();
			}

			var staff = await _context.RoomTypes
				.FirstOrDefaultAsync(m => m.RoomTypeID == int.Parse(id));
			if (staff == null)
			{
				return NotFound();
			}

			return PartialView(staff);
		}

		// POST: RoomTypes/Delete/5
		[HttpPost, ActionName("Delete")]
		public async Task<IActionResult> DeleteConfirmed([FromForm] string id)
		{
			if (_context.RoomTypes == null)
			{
				return Problem("Entity set 'ApplicationDbContext.RoomTypes'  is null.");
			}
			var staff = await _context.RoomTypes.FindAsync(id);
			if (staff != null)
			{
				_context.RoomTypes.Remove(staff);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
        public RoomType ConvertCreateRoomTypeViewModelToRoomType(CreateRoomTypeViewModel model)
        {
            List<Media> listMedia = new List<Media>();
			listMedia = _uploadFileService.uploadListImage(model.Images, "images/Admin/RoomTypes");
            Media video = new Media
            {
				For = "RoomType",
				Type = "video",
				URL = model.VideoUrl
            };
			listMedia.Add(video);
            RoomType roomType = new RoomType
			{
				TypeName = model.TypeName,
				Max = model.Max,
				Size = model.Size,
				Price = model.Price,
				Bed = model.Bed,
				Description1 = model.Description1,
				Description2 = model.Description2,
				Description3 = model.Description3,
				View = model.View,
				Media = listMedia
        };
			return roomType;
        }

    }

	
}
