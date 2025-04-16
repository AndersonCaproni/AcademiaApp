using AcademiaApp.Models;
using AcademiaApp.Repositorys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcademiaApp.Controllers
{
    [Authorize(Roles = "Aluno")]
    public class AlunoController : Controller
    {
        private readonly Context _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;

        public AlunoController(UserManager<Usuario> userManager, Context context, SignInManager<Usuario> signInManager)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
        }
        [HttpGet]
        [Authorize(Roles = "Aluno")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Aluno")]
        public async Task<IActionResult> Detail(string id)
        {
            var usuario = await _context.Set<Aluno>().FirstOrDefaultAsync(x => x.Id == id);
            return View(usuario);
        }

        [HttpGet]
        [Authorize(Roles = "Aluno")]
        public async Task<IActionResult> Edit(string id)
        {
            var aluno = await _context.Set<Aluno>().FirstOrDefaultAsync(p => p.Id == id);
            if (aluno == null)
            {
                return NotFound();
            }

            return View(aluno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Aluno")]
        public async Task<IActionResult> Edit(string id, Aluno model)
        {
            if (id != model.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            var aluno = await _context.Set<Aluno>().FirstOrDefaultAsync(p => p.Id == id);
            if (aluno == null)
                return NotFound();

            aluno.Nome = model.Nome;
            aluno.DataNascimento = model.DataNascimento;
            aluno.Telefone = model.Telefone;
            aluno.CPF = model.CPF;

            _context.Update(aluno);
            await _context.SaveChangesAsync();

            return RedirectToAction("Detail", new { id = aluno.Id });
        }

        [HttpGet]
        [Authorize(Roles = "Aluno")]
        public async Task<IActionResult> Delete(string id)
        {
            var aluno = await _context.Set<Aluno>().FirstOrDefaultAsync(p => p.Id == id);
            if (aluno == null)
                return NotFound();

            return View(aluno); // Mostra tela de confirmação
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Aluno")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var aluno = await _context.Set<Aluno>().FirstOrDefaultAsync(p => p.Id == id);
            if (aluno == null)
                return NotFound();

            var treinos = await _context.Treinos.Where(t => t.AlunoId == id).ToListAsync();
            _context.Treinos.RemoveRange(treinos);

            await _context.SaveChangesAsync();

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
