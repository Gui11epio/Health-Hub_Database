using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Health_Hub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Health_Hub.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        public DbSet<Usuario> Usuario { get; set; }

        public DbSet<Questionario> Questionario { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.HasDefaultSchema("RM557982");

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("USUARIO");

                entity.Property(e => e.Id)
                    .HasColumnName("ID");

                entity.Property(e => e.EmailCorporativo)
                    .HasColumnName("EMAIL_CORPORATIVO");

                entity.Property(e => e.Nome)
                    .HasColumnName("NOME");

                entity.Property(e => e.Senha)
                    .HasColumnName("SENHA");

                entity.Property(e => e.Tipo)
                    .HasColumnName("TIPO");
            });

            modelBuilder.Entity<Questionario>(entity =>
            {
                entity.ToTable("QUESTIONARIO");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd(); // usa o trigger SEQ_QUESTIONARIO

                entity.Property(e => e.DataPreenchimento)
                    .HasColumnName("DATA_PREENCHIMENTO")
                    .HasColumnType("DATE");

                entity.Property(e => e.NivelEstresse)
                    .HasColumnName("NIVEL_ESTRESSE");

                entity.Property(e => e.QualidadeSono)
                    .HasColumnName("QUALIDADE_SONO");

                entity.Property(e => e.Ansiedade)
                    .HasColumnName("ANSIEDADE");

                entity.Property(e => e.Sobrecarga)
                    .HasColumnName("SOBRECARGA");

                entity.Property(e => e.Avaliacao)
                    .HasColumnName("AVALIACAO")
                    .HasMaxLength(300);

                entity.Property(e => e.UsuarioId)
                    .HasColumnName("USUARIO_ID");

                entity.HasOne(e => e.Usuario)
                    .WithMany() // se sua entidade Usuario não tiver List<Questionario>
                    .HasForeignKey(e => e.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

        }

    }
}
