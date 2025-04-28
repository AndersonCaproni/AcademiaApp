using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AcademiaApp.Models
{
    public class TreinoCreateViewModel
    {
        [Required(ErrorMessage = "Selecione um aluno.")]
        public string AlunoId { get; set; }

        [Required(ErrorMessage = "A data é obrigatória.")]
        public DateTime? DataTreino { get; set; }

        [Required(ErrorMessage = "É necessário selecionar pelo menos 4 exercícios.")]
        public List<ListaExercicios> ExerciciosSelecionados { get; set; }

        [ValidateNever]
        public List<SelectListItem> ExerciciosDisponiveis { get; set; }
    }

    public class ListaExercicios
    { 
        public long Id { get; set; }
        [Required(ErrorMessage = "A quantidade de séries é obrigatória.")]
        public int qtdSeries { get; set; }
        [Required(ErrorMessage = "A quantidade de repetições é obrigatória.")]
        public int qtdRepeticoes { get; set; }
    }
}
