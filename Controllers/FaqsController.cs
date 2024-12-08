using Login.Models;
using Login.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Login.Controllers
{
    public class FaqsController : Controller
    {
        private readonly ApplicationUserDbContext faqsDb;

        public FaqsController(ApplicationUserDbContext faqsDb)
        {
            this.faqsDb = faqsDb;
        }

        // FAQ List
        public async Task<IActionResult> Index()
        {
            var faqData = await faqsDb.FaQss.ToListAsync();
            return View(faqData);
        }

        // Create FAQ
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Faqs new_D)
        {
            if (ModelState.IsValid)
            {
                await faqsDb.FaQss.AddAsync(new_D);
                await faqsDb.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(new_D);
        }

        // Edit FAQ
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var faqData = await faqsDb.FaQss.FindAsync(id);
            if (faqData == null) return NotFound();

            return View(faqData);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int? id, Faqs new_E)
        {
            if (id != new_E.Id) return NotFound();

            if (ModelState.IsValid)
            {
                faqsDb.FaQss.Update(new_E);
                await faqsDb.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(new_E);
        }

        // Delete FAQ
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var faqData = await faqsDb.FaQss.FirstOrDefaultAsync(x => x.Id == id);
            if (faqData == null) return NotFound();

            return View(faqData);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var faqData = await faqsDb.FaQss.FindAsync(id);
            if (faqData != null)
            {
                faqsDb.FaQss.Remove(faqData);
                await faqsDb.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}
