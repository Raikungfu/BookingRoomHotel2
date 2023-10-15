using Microsoft.AspNetCore.Mvc;
using BookingRoomHotel.Models;
using System.Data;
using BookingRoomHotel.ViewModels;

namespace BookingRoomHotel.Controllers
{
    public class ProfileCustomerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProfileCustomerController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: ProfileCustomer
        public async Task<IActionResult> Index()
        {
            var CustomerId = HttpContext.Session.GetString("CustomerId");
            var Customer = await _context.Customers.FindAsync(CustomerId);
            return (Customer == null) ?
				Problem("Entity set 'ApplicationDbContext.Customers'  is null.") : View(CustomerToProfileView(Customer));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ProfileCustomerViewModel model)
        {
            var CustomerId = HttpContext.Session.GetString("CustomerId");
            var CustomertoUpdate = await _context.Customers.FindAsync(CustomerId);

            if (ModelState.IsValid)
            {
                string profileFileName = UploadedFileProfile(model);
                string imgidentify1 = UploadedFileImgIdentify1(model);
                string imgidentify2 = UploadedFileImgIdentify2(model);

                CustomertoUpdate.Name = model.Name;
                CustomertoUpdate.Email = model.Email;
                CustomertoUpdate.Phone = model.Phone;
                CustomertoUpdate.Address = model.Address;
                CustomertoUpdate.DateOfBirth = model.DateOfBirth;
                CustomertoUpdate.ImgAvt = profileFileName;
                CustomertoUpdate.ImgIdentify1 = imgidentify1;
                CustomertoUpdate.ImgIdentify2 = imgidentify2;
                _context.Customers.Update(CustomertoUpdate);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(CustomerToProfileView(CustomertoUpdate));
        }

        private ProfileView CustomerToProfileView(Customer Customer)
        {
            ProfileView cus = new ProfileView();
            cus.Name = Customer.Name;
            cus.Email = Customer.Email;
            cus.Phone = Customer.Phone;
            cus.Address = Customer.Address;
            cus.DateOfBirth = Customer.DateOfBirth;
            cus.ImgAvt = Customer.ImgAvt;
            cus.ImgIdentify1 = Customer.ImgIdentify1;
            cus.ImgIdentify2 = Customer.ImgIdentify2;
            cus.Status = Customer.Status;
            return cus;
        }

        private string UploadedFileProfile(ProfileCustomerViewModel model)
        {
            string profileFileName = null;

            if (model.ImgAvt != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/customer/profile");
                profileFileName = Guid.NewGuid().ToString() + "_" + model.ImgAvt.FileName;
                string filePath = Path.Combine(uploadsFolder, profileFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ImgAvt.CopyTo(fileStream);
                }
            }
            return profileFileName;
        }

        private string UploadedFileImgIdentify1(ProfileCustomerViewModel model)
        {
            string ImgIdentify1FileName = null;

            if (model.ImgIdentify1 != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/customer/imgIdentify1");
                ImgIdentify1FileName = Guid.NewGuid().ToString() + "_" + model.ImgIdentify1.FileName;
                string filePath = Path.Combine(uploadsFolder, ImgIdentify1FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ImgIdentify1.CopyTo(fileStream);
                }
            }
            return ImgIdentify1FileName;
        }

        private string UploadedFileImgIdentify2(ProfileCustomerViewModel model)
        {
            string ImgIdentify2FileName = null;

            if (model.ImgIdentify2 != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/customer/imgIdentify2");
                ImgIdentify2FileName = Guid.NewGuid().ToString() + "_" + model.ImgIdentify2.FileName;
                string filePath = Path.Combine(uploadsFolder, ImgIdentify2FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ImgIdentify2.CopyTo(fileStream);
                }
            }
            return ImgIdentify2FileName;
        }
    }
}