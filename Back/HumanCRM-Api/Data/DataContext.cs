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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações básicas
            modelBuilder.Entity<Clientes>(entity =>
            {
                entity.Property(c => c.Codigo)
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
        }
    }
}