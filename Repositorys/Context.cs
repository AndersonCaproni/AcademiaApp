using AcademiaApp.Models;
using AcademiaApp.Repositorys.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AcademiaApp.Repositorys
{
    public class Context : IdentityDbContext
    {
        public DbSet<Treino> Treinos { get; set; }
        public DbSet<Exercicio> Exercicios { get; set; }
        public DbSet<TreinosExercicios> TreinosExercicios { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>()
                .HasDiscriminator<string>("TipoUsuario")
                .HasValue<Aluno>("Aluno")
                .HasValue<Personal>("Personal");

            modelBuilder.ApplyConfiguration(new TreinoConfiguration());
            modelBuilder.ApplyConfiguration(new ExercicioConfiguration());
            modelBuilder.ApplyConfiguration(new TreinosExerciciosConfiguration());
        }
    }
}
