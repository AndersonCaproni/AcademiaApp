using AcademiaApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class AccountController : Controller
{
    private readonly UserManager<Usuario> _userManager;
    private readonly SignInManager<Usuario> _signInManager;

    public AccountController(UserManager<Usuario> userManager,
                             SignInManager<Usuario> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult RegisterAluno() => View();

    [HttpPost]
    public async Task<IActionResult> RegisterAluno(RegisterAlunoView model)
    {
        // Validação do ModelState
        if (!ModelState.IsValid)
            return View("RegisterAluno", model);

        var aluno = new Aluno
        {
            UserName = model.Email,
            Email = model.Email,
            Nome = model.Nome,
            EmailConfirmed = true,
            CPF = model.CPF,
            DataNascimento = model.DataNascimento,
            Telefone = model.Telefone,
        };

        var result = await _userManager.CreateAsync(aluno, model.Senha);

        if (result.Succeeded)
        {
            // Adição do usuário à Role "Aluno"
            await _userManager.AddToRoleAsync(aluno, "Aluno");

            await _signInManager.SignInAsync(aluno, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        // Adição dos erros ao ModelState
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View("RegisterAluno", model);
    }

    [HttpGet]
    public IActionResult RegisterPersonal() => View();

    [HttpPost]
    public async Task<IActionResult> RegisterPersonal(RegisterPersonalView model)
    {
        // Validação do ModelState
        if (!ModelState.IsValid)
            return View("RegisterPersonal", model);

        var personal = new Personal
        {
            UserName = model.Email,
            Email = model.Email,
            Nome = model.Nome,
            CPF = model.CPF,
            EmailConfirmed = true,
            DataNascimento = model.DataNascimento,
            Telefone = model.Telefone,
            CREF = model.CREF
        };

        var result = await _userManager.CreateAsync(personal, model.Senha);

        if (result.Succeeded)
        {
            // Adição do usuário à Role "Personal"
            await _userManager.AddToRoleAsync(personal, "Personal");

            await _signInManager.SignInAsync(personal, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        // Adição dos erros ao ModelState
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View("RegisterPersonal", model);
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

        [HttpPost]
        public async Task<IActionResult> Login(LoginView model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Usuário não encontrado.");
                    return View(model);
                }

                if (!user.EmailConfirmed)
                {
                    ModelState.AddModelError(string.Empty, "E-mail ainda não confirmado.");
                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Senha, model.LembrarMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Contains("Aluno"))
                    {
                        return RedirectToAction("Index", "Aluno");
                    }
                    else if (roles.Contains("Personal"))
                    {
                        return RedirectToAction("Index", "Personal");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Conta bloqueada. Tente novamente mais tarde.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "E-mail ou senha inválidos.");
                }
            }

            return View(model);
        }


    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
