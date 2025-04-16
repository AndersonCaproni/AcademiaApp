namespace AcademiaApp.Models
{
    public class TreinosExercicios
    {
        public long Id { get; set; }
        public long TreinoId { get; set; }
        public Treino Treino { get; set;}
        public long ExercicioId { get; set; }
        public Exercicio Exercicio { get; set; }
    }
}
