using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanCRM_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HumanCRM_Api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Clientes> Clientes { get; set; }
        public DbSet<ProspeccaoCliente> ProspeccoesClientes { get; set; }
        public DbSet<ContratoCliente> ContratosClientes { get; set; }
        public DbSet<ClienteDocumento> ClienteDocumentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações básicas
            modelBuilder.Entity<Clientes>(entity =>
            {
                entity.Property(c => c.Id)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(c => c.Nome)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(c => c.TipoPessoa)
                      .IsRequired()
                      .HasMaxLength(20);
            });

            modelBuilder.Entity<ProspeccaoCliente>(entity =>
            {
                entity.HasOne(p => p.Cliente)
                      .WithMany(c => c.Prospeccoes)
                      .HasForeignKey(p => p.ClienteId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ClienteDocumento>(entity =>
            {
                entity.ToTable("ClienteDocumentos");
                entity.HasKey(x => x.Id);

                entity.Property(x => x.NomeArquivo).HasMaxLength(255).IsRequired();
                entity.Property(x => x.ContentType).HasMaxLength(80).IsRequired();
                entity.Property(x => x.Arquivo).IsRequired();
                entity.HasIndex(x => x.ClienteId);
            });
        }
    }
}