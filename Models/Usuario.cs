using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AcademiaApp.Models
{
    public class Usuario : IdentityUser
    {
        [Required]
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Telefone { get; set; }
        public string CPF { get; set; }
        public List<Treino> Treinos { get; set; }

        public Usuario()
        {
            Treinos = new List<Treino>();
        }
    }
}
