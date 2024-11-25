using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SinqiaMVC.Models;


public class AppDbContext : DbContext

     
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<ProdutoCosif> ProdutoCosifs { get; set; }
    public DbSet<MovimentoManual> MovimentosManuais { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Produto
        modelBuilder.Entity<Produto>(entity =>
        {
            entity.ToTable("PRODUTO");
            entity.HasKey(e => e.COD_PRODUTO);
            entity.Property(e => e.COD_PRODUTO).HasMaxLength(4).IsRequired();
            entity.Property(e => e.DES_PRODUTO).HasMaxLength(30).IsRequired();
            entity.Property(e => e.STA_STATUS).HasMaxLength(1).IsRequired();
        });

        // ProdutoCosif
        modelBuilder.Entity<ProdutoCosif>(entity =>
        {
            entity.ToTable("PRODUTO_COSIF");
            entity.HasKey(e => new { e.COD_PRODUTO, e.COD_COSIF });
            entity.Property(e => e.COD_PRODUTO).HasMaxLength(4).IsRequired();
            entity.Property(e => e.COD_COSIF).HasMaxLength(11).IsRequired();
            entity.Property(e => e.COD_CLASSIFICACAO).HasMaxLength(6).IsRequired();
            entity.Property(e => e.STA_STATUS).HasMaxLength(1).IsRequired();

            entity.HasOne(e => e.Produto)
                  .WithMany(p => p.ProdutoCosifs)
                  .HasForeignKey(e => e.COD_PRODUTO);
        });

        // MovimentoManual
        modelBuilder.Entity<MovimentoManual>(entity =>
        {
            entity.ToTable("MOVIMENTO_MANUAL");
            entity.HasKey(e => new { e.DAT_MES, e.DAT_ANO, e.NUM_LANCAMENTO });
            entity.Property(e => e.DAT_MES).IsRequired();
            entity.Property(e => e.DAT_ANO).IsRequired();
            entity.Property(e => e.NUM_LANCAMENTO).IsRequired();
            entity.Property(e => e.COD_PRODUTO).HasMaxLength(4).IsRequired();
            entity.Property(e => e.COD_COSIF).HasMaxLength(11);
            entity.Property(e => e.DES_DESCRICAO).HasMaxLength(50).IsRequired();
            entity.Property(e => e.DAT_MOVIMENTO).IsRequired();
            entity.Property(e => e.COD_USUARIO).HasMaxLength(15);
            entity.Property(e => e.VAL_VALOR).IsRequired();

            entity.HasOne(e => e.ProdutoCosif)
                  .WithMany(p => p.MovimentosManuais)
                  .HasForeignKey(e => new { e.COD_PRODUTO, e.COD_COSIF });
        });
    }
}
