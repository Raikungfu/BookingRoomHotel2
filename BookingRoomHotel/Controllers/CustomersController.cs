using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingRoomHotel.Models;
using BookingRoomHotel.ViewModels;
using BookingRoomHotel.Models.ModelsInterface;
using Microsoft.AspNetCore.Authorization;

namespace BookingRoomHotel.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public CustomersController(ApplicationDbContext context, IEmailService emailService, ITokenService tokenService, IConfiguration configuration)
        {
            _context = context;
            _emailService = emailService;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        // GET: Customers
        [Authorize(Policy = "AdminAndReceptPolicy")]
        public async Task<IActionResult> Index()
         {
            ListCustomerViewModel listCustomerViewModel = new ListCustomerViewModel();
            listCustomerViewModel.ListCus = await _context.Customers.OrderByDescending(x => x.Status).Take(6).ToListAsync();
            int total = await _context.Customers.CountAsync();
            listCustomerViewModel.Count = total % 6 == 0? total /6 : total / 6 + 1;
            return _context.Customers != null ?
                        PartialView(listCustomerViewModel):
                        Problem("Entity set 'ApplicationDbContext.Customers'  is null.");
        }

        [HttpPost, ActionName("Index")]
        [Authorize(Policy = "AdminAndReceptPolicy")]
        public async Task<IActionResult> Index(string id)
        {
            ListCustomerViewModel listCustomerViewModel = new ListCustomerViewModel();
            listCustomerViewModel.ListCus = await _context.Customers.OrderByDescending(x => x.Status).Skip(6 * (int.Parse(id) - 1)).Take(6).ToListAsync();
            int total = await _context.Customers.CountAsync();
            listCustomerViewModel.Count = total % 6 == 0 ? total / 6 : total / 6 + 1;
            return _context.Customers != null ?
                        PartialView(listCustomerViewModel) :
                        Problem("Entity set 'ApplicationDbContext.Customers'  is null.");
        }

        // GET: Customers/Details/5
        [Authorize(Policy = "AdminAndReceptPolicy")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return PartialView(customer);
        }

        // GET: Customers/Create
        [HttpGet]
        [Authorize(Policy = "AdminAndReceptPolicy")]
        public IActionResult Create()
        {
            return PartialView();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "AdminAndReceptPolicy")]
        public async Task<IActionResult> Create([FromForm][Bind("Id,Name,Email,Phone,DateOfBirth,Address,Pw")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return PartialView(customer);
        }

        // GET: Customers/Edit/5
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return PartialView(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Edit(string id, [FromForm][Bind("Id,Name,Email,Phone,DateOfBirth,Address,Pw,Status,ImgIdentify1,ImgIdentify2,ImgAvt")] Customer customer)
        {
            if (ModelState.IsValid )
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
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
            return PartialView(customer);
        }

        // GET: Customers/Delete/5
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Delete(string id)
        {
            if (_context.Customers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Customers'  is null.");
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(string id)
        {
            return (_context.Customers?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] CusRegisterViewModel model)
        {
            try
            {
                if (ModelState.IsValid && model.Pw.Equals(model.PwCf))
                {
                    if (await _context.Customers.FindAsync(model.Id) != null)
                    {
                        throw new Exception("ID already existed!");
                    }
                    else
                    {
                        _emailService.SendRegisterMail(model.Email, model.Name, model.Id, model.Pw);
                        var cus = new Customer
                        {
                            Id = model.Id,
                            Pw = model.Pw,
                            Name = model.Name,
                            Email = model.Email,
                            Address = model.Address,
                            DateOfBirth = model.DateOfBirth,
                            Phone = model.Phone,
                            Status = "Unverified"
                        };
                        await _context.Customers.AddAsync(cus);
                        await _context.SaveChangesAsync();
                        return Json(new { success = true, message = "Register Successful! Please check your email!", accessToken = _tokenService.GenerateAccessToken(cus.Id, cus.Name, "customer"), role = "customer", name = cus.Name });
                    }
                }
                throw new Exception("Your password does not match!");
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = "Register Failed! Error: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] CusLoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var cus = await _context.Customers.FindAsync(model.UserName);
                    if (cus != null && cus.Pw.Equals(model.Password))
                    {
                        HttpContext.Session.SetString("CustomerId", model.UserName);
                        return Json(new { success = true, message = "Login Successful!", accessToken = _tokenService.GenerateAccessToken(cus.Id, cus.Name, "customer"), role = "customer", name = cus.Name, avt = cus.ImgAvt });
                    }
                    else
                    {
                        throw new Exception("ID or Password not correct!");
                    }
                }
                else
                {
                    throw new Exception("Your input not correct!");
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = "Login Failed! Error: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromForm] CusChangePwViewModel model)
        {
            try
            {
                if (ModelState.IsValid && model.NewPw.Equals(model.ConfirmNewPw))
                {
                    var cus = _context.Customers.Find(model.Id);
                    if (cus != null && cus.Pw.Equals(model.OldPw))
                    {
                        cus.Pw = model.NewPw;
                        await _context.SaveChangesAsync();
                        _emailService.SendChangePasswordMail(cus.Email, cus.Name, cus.Pw);
                        return Json(new { success = true, message = "Change Password successful! Please check your email!" });
                    }
                    else
                    {
                        throw new Exception("Your information not correctly!");
                    }
                }
                else
                {
                    throw new Exception("Your input not correct!");
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = "Change Password Failed! Error: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromForm] CusForgotPasswordViewModel model)
        {
            try
            {
                var cus = await _context.Customers.FindAsync(model.Id);
                if (cus != null && cus.Email.Equals(model.Email))
                {
                    _emailService.SendForgotPasswordMail(cus.Email, cus.Name, cus.Pw);
                    return Json(new { success = true, message = "Your password has been sent via email. Please check your email!" });
                    }
                else
                {
                    throw new Exception("Your ID or Email not correct!");
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = "Get password Failed! Error: " + ex.Message });
            }
        }
    }
}