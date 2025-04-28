using AcademiaApp.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AcademiaApp.Repositorys.Configurations
{
    public class TreinosExerciciosConfiguration : IEntityTypeConfiguration<TreinosExercicios>
    {
        public void Configure(EntityTypeBuilder<TreinosExercicios> builder)
        {
            builder.ToTable("TreinosExercicios").HasKey(nameof(TreinosExercicios.Id));

            builder.Property(nameof(TreinosExercicios.Id)).HasColumnName("Id").IsRequired();
            builder.Property(nameof(TreinosExercicios.TreinoId)).HasColumnName("TreinoId").IsRequired();
            builder.Property(nameof(TreinosExercicios.ExercicioId)).HasColumnName("ExercicioId").IsRequired();
            builder.Property(nameof(TreinosExercicios.qtdRepeticoes)).HasColumnName("qtdRepeticoes").IsRequired();
            builder.Property(nameof(TreinosExercicios.qtdSeries)).HasColumnName("qtdSeries").IsRequired();

            builder.HasOne(x => x.Treino)
                .WithMany(y => y.TreinosExercicios)
                .HasForeignKey(x => x.TreinoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Exercicio)
                .WithMany(y => y.TreinosExercicios)
                .HasForeignKey(x => x.ExercicioId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
