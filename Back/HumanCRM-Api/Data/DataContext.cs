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
        public DbSet<Schedule> Schedules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações básicas
            modelBuilder.Entity<Clientes>(entity =>
            {
                entity.Property(c => c.Id)
                      .IsRequired();


                entity.Property(c => c.Nome)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(c => c.TipoPessoa)
                      .IsRequired()
                      .HasMaxLength(20);

                entity.Property(c => c.Celular).IsRequired(false);

                modelBuilder.Entity<Clientes>()
                        .Property(x => x.DataNascimento)
                        .HasColumnType("date");

            });

            modelBuilder.Entity<ProspeccaoCliente>(entity =>
            {
                entity.HasOne(p => p.Cliente)
                      .WithMany(c => c.Prospeccoes)
                      .HasForeignKey(p => p.ClienteId)
                      .OnDelete(DeleteBehavior.Cascade);

                    modelBuilder.Entity<ProspeccaoCliente>()
                            .Property(x => x.DataProximoContato)
                            .HasColumnType("date");


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

            modelBuilder.Entity<Schedule>(entity =>
                {
                    entity.HasKey(e => e.AgendamentoId);

                    entity.Property(e => e.AgendamentoId)
                        .ValueGeneratedOnAdd();

                    entity.Property(e => e.ClienteId)
                        .IsRequired();

                    entity.Property(e => e.DataAgendamento)
                        .IsRequired();

                    entity.Property(e => e.Descricao)
                        .IsRequired()
                        .HasMaxLength(255);

                    entity.Property(e => e.ResponsavelAgendamento)
                        .IsRequired()
                        .HasMaxLength(150);
                });
        }
    }
}