using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AcademiaApp.Models
{
    public class TreinoAlunoEditViewModel
    {
        [Required(ErrorMessage = "Selecione um personal.")]
        public string PersonalId { get; set; }

        [Required(ErrorMessage = "A data é obrigatória.")]
        public DateTime? DataTreino { get; set; }

        [Required(ErrorMessage = "É necessário selecionar pelo menos 4 exercícios.")]
        public List<ListaExercicios> ExerciciosSelecionados { get; set; }

        [ValidateNever]
        public List<SelectListItem> ExerciciosDisponiveis { get; set; }
    }

}
