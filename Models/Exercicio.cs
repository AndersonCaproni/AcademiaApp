namespace AcademiaApp.Models
{
    public class Exercicio
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Categoria { get; set; }
        public string Descricao { get; set; }
        public List<TreinosExercicios> TreinosExercicios { get; set; }

        public Exercicio()
        {
            TreinosExercicios = new List<TreinosExercicios>();
        }
    }
}
