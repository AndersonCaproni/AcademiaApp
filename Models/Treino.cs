namespace AcademiaApp.Models
{
    public class Treino
    {
        public long Id { get; set; }
        public string AlunoId { get; set; }
        public Aluno Aluno { get; set; }
        public string PersonalId { get; set; }
        public Personal Personal { get; set; }
        public DateTime DataTreino { get; set; }
        public bool Status { get; set; }  
        public List<TreinosExercicios> TreinosExercicios { get; set; }

        public Treino()
        {
            TreinosExercicios = new List<TreinosExercicios>();
        }
    }
}
