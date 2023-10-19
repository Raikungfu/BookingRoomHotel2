using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookingRoomHotel.Models;
using Microsoft.AspNetCore.Authorization;
using BookingRoomHotel.Models.ModelsInterface;
using BookingRoomHotel.ViewModels;

namespace BookingRoomHotel.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public QuestionsController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // GET: Questions
        [Authorize(Policy = "AdminAndReceptPolicy")]
        public async Task<IActionResult> Index()
        {
            return _context.Question != null ?
                          PartialView(await getListViewQuestion("1")) :
                          Problem("Entity set 'ApplicationDbContext.Question'  is null.");
        }

        [HttpPost]
        [Authorize(Policy = "AdminAndReceptPolicy")]
        public async Task<IActionResult> Index(string id)
        {
            return _context.Question != null ?
                          PartialView(await getListViewQuestion(id)) :
                          Problem("Entity set 'ApplicationDbContext.Question'  is null.");
        }

        // GET: Questions/Details/5
        [Authorize(Policy = "AdminAndReceptPolicy")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Question == null)
            {
                return NotFound();
            }

            var question = await _context.Question
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            return PartialView(question);
        }

        // GET: Questions/Create

        [Authorize(Policy = "AdminAndReceptPolicy")]
        public IActionResult Create()
        {
            return PartialView();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create([FromForm][Bind("Name,Email,Subject,Message")] Question question)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    question.Status = "Pending";
                    _context.Add(question);
                    await _context.SaveChangesAsync();
                    _emailService.SendConfirmQ(question.Email, question.Name, question.Subject);
                    return RedirectToAction(nameof(Index));
                }else
                {
                    throw new Exception("Input not valid!");
                }
            }catch (Exception ex)
            {
                return Json(new { error = true, message = ex.Message});
            }

        }

        // GET: Questions/Edit/5
        [Authorize(Policy = "AdminAndReceptPolicy")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Question == null)
            {
                return NotFound();
            }

            var question = await _context.Question.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            return PartialView(question);
        }

        [Authorize(Policy = "AdminAndReceptPolicy")]
        public async Task<IActionResult> ListEdit(string id)
        {
            return _context.Question != null ?
                          PartialView(await getListViewQuestion(id)) :
                          Problem("Entity set 'ApplicationDbContext.Question'  is null.");
        }

        [HttpPost]
        [Authorize(Policy = "AdminAndReceptPolicy")]
        public async Task<IActionResult> ListEdit(int id, [Bind("Id,Email,Name,Subject,Message,Response,Status")] Question question)
        {
            if (id != question.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(question);
                    await _context.SaveChangesAsync();
                    if (question.Status.Equals("Complete"))
                    {
                        _emailService.SendResponseQ(question.Email, question.Name, question.Subject, question.Message, question.Response);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(question.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return _context.Question != null ?
                          PartialView(await getListViewQuestion("1")) :
                          Problem("Entity set 'ApplicationDbContext.Question'  is null.");
            }
            return PartialView(question);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "AdminAndReceptPolicy")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Name,Subject,Message,Response,Status")] Question question)
        {
            if (id != question.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(question);
                    await _context.SaveChangesAsync();
                    if (question.Status.Equals("Complete"))
                    {
                        _emailService.SendResponseQ(question.Email, question.Name, question.Subject, question.Message, question.Response);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(question.Id))
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
            return PartialView(question);
        }

        [Authorize(Policy = "AdminAndReceptPolicy")]
        public async Task<IActionResult> Delete(string id)
         {
            if (_context.Question == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Question'  is null.");
            }
            var question = await _context.Question.FindAsync(id);
            if (question != null)
            {
                _context.Question.Remove(question);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionExists(int id)
        {
          return (_context.Question?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<ListQuestionViewModel> getListViewQuestion(string id)
        {
            ListQuestionViewModel listQuestionViewModel = new ListQuestionViewModel();
            listQuestionViewModel.ListQuestion = await _context.Question.OrderBy(x => x.Status == "Pending" ? 1 : x.Status == "Processing" ? 2 : x.Status == "Complete" ? 3 : 4).Skip(6 * (int.Parse(id) - 1)).Take(6).ToListAsync();
            int total = await _context.Question.CountAsync();
            listQuestionViewModel.Count = total % 6 == 0 ? total / 6 : total / 6 + 1;
            return listQuestionViewModel;
        }
    }
}
