using Microsoft.EntityFrameworkCore;
using Projeto04.AspNet.WebAPI.BackEnd.Models.Entities;

namespace Projeto04.AspNet.WebAPI.BackEnd.Models
{
    // praticar o mecanismo de herança com  a superclasse DbContext
    public class MeuDbContext: DbContext
    {
        // definir o construtor da classe para fazer referencia direta ao construtor da superclasse
        public MeuDbContext(DbContextOptions<MeuDbContext> options): base(options) { }

        // fazer referencia as entities - ja definidas -  na aplicação
        public DbSet<Entities.Estudante> Estudante {  get; set; }
        public DbSet<Entities.Cursos> Cursos { get; set; }

        
        // referenciar/implementar o método OnModelCreating()
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // definir o "mapeamento" das relações entre as entities
            modelBuilder.Entity<Estudante>(entity =>
            {
                entity.HasKey(e => e.Estudante_Id); // entity possui uma PK
                entity.HasMany(e => e.Curso) // 1 Estudante para varios Cursos
                      .WithOne(c => c.Estudante) // estabelece que um curso pode ter mais de 1 Estudant
                      .HasForeignKey(c => c.Estudante_Id); // aqui está evidenciado o relacionamente entre as entities - a partir das propriedades PK/FK - evitando a interpretação de duplicidade
            });

            modelBuilder.Entity<Cursos>(entity =>
            {
                entity.HasKey(c => c.Curso_Id);
                entity.HasOne(c => c.Estudante)
                      .WithMany(e => e.Curso)
                      .HasForeignKey(c => c.Estudante_Id);
            });
        }
       
    }
}
