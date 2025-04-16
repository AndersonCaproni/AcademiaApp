using AcademiaApp.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AcademiaApp.Repositorys.Configurations
{
    public class ExercicioConfiguration : IEntityTypeConfiguration<Exercicio>
    {
        public void Configure(EntityTypeBuilder<Exercicio> builder)
        {
            builder.ToTable("Exercicios").HasKey(nameof(Exercicio.Id));

            builder.Property(nameof(Exercicio.Id)).HasColumnName("Id").IsRequired();
            builder.Property(nameof(Exercicio.Nome)).HasColumnName("Nome").IsRequired().HasMaxLength(125);
            builder.Property(nameof(Exercicio.Categoria)).HasColumnName("Categoria").IsRequired().HasMaxLength(125);
            builder.Property(nameof(Exercicio.Descricao)).HasColumnName("Descricao").IsRequired();
        }
    }
}
