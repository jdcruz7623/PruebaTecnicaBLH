using BLH_BancaPersona_api.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace BLH_BancaPersona_api.Data
{
    public class BancaPersonaDbContext: DbContext
    {
        public BancaPersonaDbContext(DbContextOptions<BancaPersonaDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<Cuenta> Cuentas => Set<Cuenta>();
        public DbSet<Transaccion> Transacciones => Set<Transaccion>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cuenta>()
                .HasIndex(a => a.NumeroCuenta)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
