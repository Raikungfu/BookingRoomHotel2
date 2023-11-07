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
        private readonly IUploadFileService _uploadFileService;

        public ProfileCustomerController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, IUploadFileService uploadFileService)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _uploadFileService = uploadFileService;
        }

        // GET: ProfileCustomer
        public async Task<IActionResult> Index()
        {
            var CustomerId = HttpContext.Session.GetString("CustomerId");
            var Customer = await _context.Customers.FindAsync(CustomerId);
            return (Customer == null) ?
                RedirectToAction("Error","Home") : View(CustomerToProfileView(Customer));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ProfileCustomerViewModel model)
        {
            var CustomerId = HttpContext.Session.GetString("CustomerId");
            var CustomertoUpdate = await _context.Customers.FindAsync(CustomerId);

            if (ModelState.IsValid)
            {
                string profileFileName = _uploadFileService.uploadImage(model.ImgAvt, "images/customer/profile");
                string imgidentify1 = _uploadFileService.uploadImage(model.ImgIdentify1, "images/customer/imgIdentify1");
                string imgidentify2 = _uploadFileService.uploadImage(model.ImgIdentify2, "images/customer/imgIdentify2");
                CustomertoUpdate.Name = model.Name;
                CustomertoUpdate.Email = model.Email;
                CustomertoUpdate.Phone = model.Phone;
                CustomertoUpdate.Address = model.Address;
                CustomertoUpdate.DateOfBirth = model.DateOfBirth;
                CustomertoUpdate.ImgAvt = profileFileName == null? CustomertoUpdate.ImgAvt: profileFileName;
                CustomertoUpdate.ImgIdentify1 = imgidentify1 == null ? CustomertoUpdate.ImgIdentify1 : imgidentify1;
                CustomertoUpdate.ImgIdentify2 = imgidentify2 == null ? CustomertoUpdate.ImgIdentify2 : imgidentify2;
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

    }
}