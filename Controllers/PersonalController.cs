using AcademiaApp.Models;
using AcademiaApp.Repositorys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcademiaApp.Controllers
{
    [Authorize(Roles = "Personal")]
    public class PersonalController : Controller
    {
        private readonly Context _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;

        public PersonalController(UserManager<Usuario> userManager, Context context, SignInManager<Usuario> signInManager)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
        }
        [HttpGet]
        [Authorize(Roles = "Personal")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize (Roles = "Personal")]
        public async Task<IActionResult> Detail (string id)
        {
            var usuario = await _context.Set<Personal>().FirstOrDefaultAsync(x => x.Id == id);
            return View(usuario);
        }

        [HttpGet]
        [Authorize(Roles = "Personal")]
        public async Task<IActionResult> Edit(string id)
        {
            var personal = await _context.Set<Personal>().FirstOrDefaultAsync(p => p.Id == id);
            if (personal == null)
            {
                return NotFound();
            }

            return View(personal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Personal")]
        public async Task<IActionResult> Edit(string id, Personal model)
        {
            if (id != model.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            var personal = await _context.Set<Personal>().FirstOrDefaultAsync(p => p.Id == id);
            if (personal == null)
                return NotFound();

            personal.Nome = model.Nome;
            personal.CREF = model.CREF;
            personal.DataNascimento = model.DataNascimento;
            personal.Telefone = model.Telefone;
            personal.CPF = model.CPF;

            _context.Update(personal);
            await _context.SaveChangesAsync();

            return RedirectToAction("Detail", new { id = personal.Id });
        }

        [HttpGet]
        [Authorize(Roles = "Personal")]
        public async Task<IActionResult> Delete(string id)
        {
            var personal = await _context.Set<Personal>().FirstOrDefaultAsync(p => p.Id == id);
            if (personal == null)
                return NotFound();

            return View(personal); // Mostra tela de confirmação
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Personal")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var personal = await _context.Set<Personal>().FirstOrDefaultAsync(p => p.Id == id);
            if (personal == null)
                return NotFound();

            var treinos = await _context.Treinos.Where(t => t.PersonalId == id).ToListAsync();
            _context.Treinos.RemoveRange(treinos);

            await _context.SaveChangesAsync();

            // Remover o personal do Identity
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }

            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
