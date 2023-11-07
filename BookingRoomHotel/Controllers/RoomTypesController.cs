using AutoMapper;
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
		private readonly IMapper _mapper;
		public RoomTypesController(ApplicationDbContext context, IUploadFileService uploadFileService, IMapper mapper)
		{
			_context = context;
			_uploadFileService = uploadFileService;
			_mapper = mapper;
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
		public async Task<IActionResult> Create([FromForm][Bind("TypeName,Max,Bed,Size,View,Description1,Description2,Description3,PriceFrom,PriceTo,Images,VideoID")] CreateRoomTypeViewModel type)
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

			var staff = await _context.RoomTypes.FindAsync(int.Parse(id));
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
        public async Task<IActionResult> Edit(string id, [FromForm][Bind("RoomTypeID,TypeName,Max,Bed,Size,View,Description1,Description2,Description3,PriceFrom,PriceTo,Images,VideoID")] CreateRoomTypeViewModel roomType)
		{
			if (int.Parse(id) != roomType.RoomTypeID)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
                    RoomType type = ConvertCreateRoomTypeViewModelToRoomType(roomType);
                    _context.Update(type);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					
				}
				return RedirectToAction(nameof(Index));
			}
			return PartialView(roomType);
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
            if (model.Images != null)
            {
                listMedia = _uploadFileService.uploadListImage(model.Images, "images/Admin/RoomTypes");
            }

            if (model.VideoID != null)
			{
                Media video = new Media
                {
                    For = "RoomType",
                    Type = "video",
                    URL = model.VideoID
                };
                listMedia.Add(video);
            }
			
            RoomType roomType = _mapper.Map<RoomType>(model);
			if (model.RoomTypeID != null) roomType.RoomTypeID = (int) model.RoomTypeID;
			if (listMedia.Count > 0)
			{
				roomType.Media = listMedia;
			}
			return roomType;
        }

    }

	
}
