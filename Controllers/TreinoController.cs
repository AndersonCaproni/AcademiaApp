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
using Microsoft.AspNetCore.Identity;

namespace AcademiaApp.Controllers
{
    public class TreinoController : Controller
    {
        private readonly Context _context;
        private readonly UserManager<Usuario> _userManager;

        public TreinoController(UserManager<Usuario> userManager, Context context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var context = _context.Treinos.Include(t => t.Aluno).Include(t => t.Personal);
            return View(await context.ToListAsync());
        }

        [Authorize(Roles = "Personal")]
        public async Task<IActionResult> ListarPorPersonal(string id)
        {
            var context = _context.Treinos.Where(x => x.PersonalId == id).Include(t => t.Aluno).Include(t => t.Personal);
            return View(await context.ToListAsync());
        }

        [Authorize(Roles = "Aluno")]
        public async Task<IActionResult> ListarPorAluno(string id)
        {
            var context = _context.Treinos.Where(x => x.AlunoId == id).Include(t => t.Aluno).Include(t => t.Personal);
            return View(await context.ToListAsync());
        }

        [Authorize(Roles = "Aluno")]
        public async Task<IActionResult> ListarPorAlunoData(string id)
        {
            var hoje = DateTime.Today;
            var context = _context.Treinos.Where(x => x.AlunoId == id && x.DataTreino.Date == hoje).Include(t => t.Aluno).Include(t => t.Personal);
            return View(await context.ToListAsync());
        }

        [Authorize(Roles = "Personal")]
        public async Task<IActionResult> DetailPersonal(int id)
        {
            var treino = await _context.Treinos
                .Include(t => t.Aluno)
                .Include(t => t.TreinosExercicios)
                .ThenInclude(te => te.Exercicio)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (treino == null)
            {
                return NotFound();
            }

            ViewBag.TreinoCompleto = treino;


            var model = new TreinoEditViewModel
            {
                AlunoId = treino.AlunoId,
                DataTreino = treino.DataTreino,
                ExerciciosSelecionados = treino.TreinosExercicios.Select(x => new ListaExercicios
                {
                    Id = x.ExercicioId,
                    qtdSeries = x.qtdSeries,
                    qtdRepeticoes = x.qtdRepeticoes
                }
                ).ToList(),
                ExerciciosDisponiveis = _context.Exercicios
                    .Select(e => new SelectListItem
                    {
                        Value = e.Id.ToString(),
                        Text = e.Nome
                    }).ToList()
            };

            return View(model);
        }

        [Authorize(Roles = "Aluno")]
        public async Task<IActionResult> DetailAluno(int id)
        {
            var treino = await _context.Treinos
                .Include(t => t.Personal)
                .Include(t => t.TreinosExercicios)
                .ThenInclude(te => te.Exercicio)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (treino == null)
            {
                return NotFound();
            }

            ViewBag.TreinoCompleto = treino;

            var model = new TreinoAlunoEditViewModel
            {
                PersonalId = treino.PersonalId,
                DataTreino = treino.DataTreino,
                ExerciciosSelecionados = treino.TreinosExercicios.Select(x => new ListaExercicios
                {
                    Id = x.ExercicioId,
                    qtdSeries = x.qtdSeries,
                    qtdRepeticoes = x.qtdRepeticoes
                }
                ).ToList(),
                ExerciciosDisponiveis = _context.Exercicios
                    .Select(e => new SelectListItem
                    {
                        Value = e.Id.ToString(),
                        Text = e.Nome
                    }).ToList()
            };

            return View(model);
        }

        [Authorize(Roles = "Personal")]
        public IActionResult Create()
        {
            var model = new TreinoCreateViewModel
            {
                ExerciciosDisponiveis = _context.Exercicios
                    .Select(e => new SelectListItem
                    {
                        Value = e.Id.ToString(),
                        Text = e.Nome
                    }).ToList()
            };

            ViewBag.Alunos = _context.Set<Aluno>()
                .Select(a => new { a.Id, a.Nome })
                .ToList();

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Personal")]
        public async Task<IActionResult> Create(TreinoCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var exerciciosSelecionadosIds = model.ExerciciosSelecionados?.Select(e => e.Id).ToList() ?? new List<long>();

                model.ExerciciosDisponiveis = _context.Exercicios
                    .Select(e => new SelectListItem
                    {
                        Value = e.Id.ToString(),
                        Text = e.Nome,
                        Selected = exerciciosSelecionadosIds.Contains(e.Id)
                    }).ToList();

                ViewBag.Alunos = _context.Set<Aluno>()
                    .Select(a => new { a.Id, a.Nome })
                    .ToList();

                return View(model);
            }

            if (model.ExerciciosSelecionados == null || model.ExerciciosSelecionados.Count < 4)
            {
                ModelState.AddModelError(nameof(model.ExerciciosSelecionados), "É necessário selecionar pelo menos 4 exercícios.");

                model.ExerciciosDisponiveis = _context.Exercicios
                    .Select(e => new SelectListItem
                    {
                        Value = e.Id.ToString(),
                        Text = e.Nome
                    }).ToList();

                ViewBag.Alunos = _context.Set<Aluno>()
                    .Select(a => new { a.Id, a.Nome })
                    .ToList();

                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);

            var treino = new Treino
            {
                AlunoId = model.AlunoId,
                DataTreino = (DateTime)model.DataTreino,
                PersonalId = user.Id,
                Status = false,
                TreinosExercicios = model.ExerciciosSelecionados.Select(x => new TreinosExercicios
                {
                    ExercicioId = x.Id,
                    qtdSeries = x.qtdSeries,
                    qtdRepeticoes = x.qtdRepeticoes
                }).ToList()
            };

            _context.Treinos.Add(treino);
            await _context.SaveChangesAsync();

            return RedirectToAction("ListarPorPersonal", new { id = user.Id });
        }

        [Authorize(Roles = "Personal")]
        public async Task<IActionResult> Edit(int id)
        {
            var treino = await _context.Treinos
                .Include(t => t.TreinosExercicios)
                .ThenInclude(te => te.Exercicio)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (treino == null)
            {
                return NotFound();
            }

            var model = new TreinoEditViewModel
            {
                AlunoId = treino.AlunoId,
                DataTreino = treino.DataTreino,
                ExerciciosSelecionados = treino.TreinosExercicios.Select(x => new ListaExercicios
                {
                    Id = x.ExercicioId,
                    qtdSeries = x.qtdSeries,
                    qtdRepeticoes = x.qtdRepeticoes
                }
                ).ToList(),
                ExerciciosDisponiveis = _context.Exercicios
                    .Select(e => new SelectListItem
                    {
                        Value = e.Id.ToString(),
                        Text = e.Nome
                    }).ToList()
            };

            ViewBag.Alunos = _context.Set<Aluno>()
                .Select(a => new { a.Id, a.Nome })
                .ToList();

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Personal")]
        public async Task<IActionResult> Edit(int id, TreinoEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var exerciciosSelecionadosIds = model.ExerciciosSelecionados?.Select(e => e.Id).ToList() ?? new List<long>();

                model.ExerciciosDisponiveis = _context.Exercicios
                    .Select(e => new SelectListItem
                    {
                        Value = e.Id.ToString(),
                        Text = e.Nome,
                        Selected = exerciciosSelecionadosIds.Contains(e.Id)
                    }).ToList();

                ViewBag.Alunos = _context.Set<Aluno>()
                    .Select(a => new { a.Id, a.Nome })
                    .ToList();

                return View(model);
            }

            if (model.ExerciciosSelecionados == null || model.ExerciciosSelecionados.Count < 4)
            {
                ModelState.AddModelError(nameof(model.ExerciciosSelecionados), "É necessário selecionar pelo menos 4 exercícios.");

                model.ExerciciosDisponiveis = _context.Exercicios
                    .Select(e => new SelectListItem
                    {
                        Value = e.Id.ToString(),
                        Text = e.Nome
                    }).ToList();

                ViewBag.Alunos = _context.Set<Aluno>()
                    .Select(a => new { a.Id, a.Nome })
                    .ToList();

                return View(model);
            }

            var treino = await _context.Treinos
                .Include(t => t.TreinosExercicios)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (treino == null)
            {
                return NotFound();
            }

            treino.AlunoId = model.AlunoId;
            treino.DataTreino = (DateTime)model.DataTreino;
            treino.TreinosExercicios.Clear();

            foreach (var exercicio in model.ExerciciosSelecionados)
            {
                treino.TreinosExercicios.Add(new TreinosExercicios
                {
                    ExercicioId = exercicio.Id,
                    qtdRepeticoes = exercicio.qtdRepeticoes,
                    qtdSeries = exercicio.qtdSeries
                });
            }

            _context.Update(treino);
            await _context.SaveChangesAsync();

            return RedirectToAction("ListarPorPersonal", new { id = treino.PersonalId });
        }



        [Authorize(Roles = "Aluno")]
        public async Task<IActionResult> MarcarComoFeito(int id)
        {
            var treino = await _context.Treinos
                .Include(t => t.Personal)
                .Include(t => t.TreinosExercicios)
                .ThenInclude(te => te.Exercicio)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (treino == null)
            {
                return NotFound();
            }

            ViewBag.TreinoCompleto = treino;
            ViewBag.Treino = id;

            var model = new TreinoAlunoEditViewModel
            {
                PersonalId = treino.PersonalId,
                DataTreino = treino.DataTreino,
                ExerciciosSelecionados = treino.TreinosExercicios.Select(x => new ListaExercicios
                {
                    Id = x.ExercicioId,
                    qtdSeries = x.qtdSeries,
                    qtdRepeticoes = x.qtdRepeticoes
                }
                ).ToList(),
                ExerciciosDisponiveis = _context.Exercicios
                    .Select(e => new SelectListItem
                    {
                        Value = e.Id.ToString(),
                        Text = e.Nome
                    }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Aluno")]
        public async Task<IActionResult> MarcarComoFeitoAcao(long id)
        {
            var treino = await _context.Treinos.FindAsync(id);

            if (treino == null)
            {
                return NotFound();
            }
            treino.Status = true;
            _context.Treinos.Update(treino);
            await _context.SaveChangesAsync();
            var user = await _userManager.GetUserAsync(User);
            return RedirectToAction("ListarPorAlunoData", new { id = user.Id });
        }

        [Authorize(Roles = "Personal")]
        public async Task<IActionResult> Delete(int id)
        {
            var treino = await _context.Treinos
                .Include(t => t.Aluno)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (treino == null)
            {
                return NotFound();
            }

            ViewBag.Aluno = treino.Aluno;

            return View(treino);
        }

        [HttpPost]
        [Authorize(Roles = "Personal")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var treino = await _context.Treinos.FindAsync(id);

            if (treino == null)
            {
                return NotFound();
            }

            // Remove o treino e os exercícios relacionados
            _context.Treinos.Remove(treino);
            await _context.SaveChangesAsync();
            var user = await _userManager.GetUserAsync(User);
            return RedirectToAction("ListarPorPersonal", new { id = user.Id });
        }


        [Authorize(Roles = "Aluno")]
        public async Task<IActionResult> DeleteAluno(int id)
        {
            var treino = await _context.Treinos
                .Include(t => t.Personal)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (treino == null)
            {
                return NotFound();
            }

            ViewBag.Personal = treino.Personal;

            return View(treino);
        }

        [HttpPost]
        [Authorize(Roles = "Aluno")]
        public async Task<IActionResult> DeleteConfirmedAluno(long id)
        {
            var treino = await _context.Treinos.FindAsync(id);

            if (treino == null)
            {
                return NotFound();
            }

            _context.Treinos.Remove(treino);
            await _context.SaveChangesAsync();
            var user = await _userManager.GetUserAsync(User);
            return RedirectToAction("ListarPorAluno", new { id = user.Id });
        }
        private bool TreinoExists(long id)
        {
            return _context.Treinos.Any(e => e.Id == id);
        }
    }
}
