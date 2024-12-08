using System.Diagnostics;
using Login.Areas.Identity.Data;
using Login.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Login.Controllers
{
    public class CompetitionsController : Controller
    {
        private readonly ApplicationUserDbContext faqsDb;
        private readonly IWebHostEnvironment env;

        public CompetitionsController(ApplicationUserDbContext faqsDb, IWebHostEnvironment env)
        {
            this.faqsDb = faqsDb;
            this.env = env;
        }

        // Competition List
        public async Task<IActionResult> Index()
        {
            var com_Data = await faqsDb.Competitions.ToListAsync();
            return View(com_Data);
        }

        public IActionResult Create()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Create(Compviewmodel new_comp)
        {
            if (ModelState.IsValid)
            {
                if (new_comp.Photo != null && new_comp.Photo.Length > 0)
                {
                    // Define folder to save images
                    string folder = Path.Combine(env.WebRootPath, "images");

                    // Create the folder if it doesn't exist
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    // Generate a unique filename
                    string fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(new_comp.Photo.FileName);
                    string filePath = Path.Combine(folder, fileName);

                    try
                    {
                        // Save the file
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await new_comp.Photo.CopyToAsync(stream);
                        }

                        // Save record to the database
                        Competition competition = new Competition
                        {
                            Name = new_comp.Name,
                            Date = new_comp.Date,
                            Deadline = new_comp.Deadline,
                            Image = fileName
                        };

                        faqsDb.Competitions.Add(competition);
                        await faqsDb.SaveChangesAsync();

                        TempData["Success"] = "Competition created successfully!";
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        TempData["Error"] = "Error uploading the image: " + ex.Message;
                    }
                }
                else
                {
                    TempData["Error"] = "Please upload a valid image.";
                }
            }
            return View(new_comp);
        }




        // Edit Competition
        public async Task<IActionResult> Edit(int id)
        {
            var competition = await faqsDb.Competitions.FindAsync(id);
            if (competition == null) return NotFound();

            var viewModel = new Compviewmodel
            {
                Com_Id = competition.Com_Id,
                Name = competition.Name,
                Date = competition.Date,
                Deadline = competition.Deadline,
                Photo = null
            };
            TempData["Photo"] = competition.Image;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Compviewmodel editedComp)
        {
            var competition = await faqsDb.Competitions.FindAsync(editedComp.Com_Id);
            if (competition == null) return NotFound();

            competition.Name = editedComp.Name;
            competition.Date = editedComp.Date;
            competition.Deadline = editedComp.Deadline;

            if (editedComp.Photo != null && editedComp.Photo.Length > 0)
            {
                string oldFilePath = Path.Combine(env.WebRootPath, "images", competition.Image);
                if (System.IO.File.Exists(oldFilePath)) System.IO.File.Delete(oldFilePath);

                string folder = Path.Combine(env.WebRootPath, "images");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(editedComp.Photo.FileName);
                string filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await editedComp.Photo.CopyToAsync(stream);
                }

                competition.Image = fileName;
            }

            faqsDb.Competitions.Update(competition);
            await faqsDb.SaveChangesAsync();

            TempData["Success"] = "Competition updated successfully!";
            return RedirectToAction("Index");
        }

        // Delete Competition
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var competition = await faqsDb.Competitions.FindAsync(id);
            if (competition == null) return NotFound();

            return View(competition);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var competition = await faqsDb.Competitions.FindAsync(id);
            if (competition != null)
            {
                faqsDb.Competitions.Remove(competition);
                await faqsDb.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}
