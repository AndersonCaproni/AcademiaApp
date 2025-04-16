using AcademiaApp.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AcademiaApp.Repositorys.Configurations
{
    public class TreinoConfiguration : IEntityTypeConfiguration<Treino>
    {
        public void Configure(EntityTypeBuilder<Treino> builder)
        {
            builder.ToTable("Treinos").HasKey(nameof(Treino.Id));

            builder.Property(nameof(Treino.Id)).HasColumnName("Id").IsRequired();
            builder.Property(nameof(Treino.AlunoId)).HasColumnName("AlunoId").IsRequired();
            builder.Property(nameof(Treino.PersonalId)).HasColumnName("PersonalId").IsRequired();
            builder.Property(nameof(Treino.DataTreino)).HasColumnName("DataTreino").IsRequired();
            builder.Property(nameof(Treino.Status)).HasColumnName("Status").IsRequired();

            builder.HasOne(treino => treino.Personal)
                .WithMany(usuario => usuario.Treinos)
                .HasForeignKey(treino => treino.PersonalId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(treino => treino.Aluno)
                .WithMany(usuario => usuario.Treinos)
                .HasForeignKey(treino => treino.AlunoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
