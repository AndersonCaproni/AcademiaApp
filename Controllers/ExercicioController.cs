using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AcademiaApp.Models;
using AcademiaApp.Repositorys;
using Microsoft.AspNetCore.Authorization;

namespace AcademiaApp.Controllers
{
    public class ExercicioController : Controller
    {
        private readonly Context _context;

        public ExercicioController(Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Exercicios.ToListAsync());
        }

        [Authorize(Roles = "Personal")]
        public async Task<IActionResult> ListarExerciciosPersonal()
        {
            return View(await _context.Exercicios.ToListAsync());
        }

        [Authorize (Roles = "Personal")]
        public async Task<IActionResult> DetailsPersonal(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercicio = await _context.Exercicios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exercicio == null)
            {
                return NotFound();
            }

            return View(exercicio);
        }

        [Authorize(Roles = "Personal")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Personal")]
        public async Task<IActionResult> Create(Exercicio exercicio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(exercicio);
                await _context.SaveChangesAsync();
                return RedirectToAction("ListarExerciciosPersonal");
            }
            return View(exercicio);
        }

        [Authorize(Roles = "Personal")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercicio = await _context.Exercicios.FindAsync(id);
            if (exercicio == null)
            {
                return NotFound();
            }
            return View(exercicio);
        }

        [HttpPost]
        [Authorize(Roles = "Personal")]
        public async Task<IActionResult> Edit(long id, Exercicio exercicio)
        {
            if (id != exercicio.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exercicio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExercicioExists(exercicio.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("ListarExerciciosPersonal");
            }
            return View(exercicio);
        }

        [Authorize(Roles = "Personal")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercicio = await _context.Exercicios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exercicio == null)
            {
                return NotFound();
            }

            return View(exercicio);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Personal")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var exercicio = await _context.Exercicios.FindAsync(id);
            if (exercicio != null)
            {
                _context.Exercicios.Remove(exercicio);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("ListarExerciciosPersonal");
        }

        private bool ExercicioExists(long id)
        {
            return _context.Exercicios.Any(e => e.Id == id);
        }
    }
}
