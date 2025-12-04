using BLH_BancaPersona_api.Data;
using BLH_BancaPersona_api.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace BLH_BancaPersona_api.Services
{
    public class CuentaService: ICuentaService
    {
        private readonly BancaPersonaDbContext bdBancaPersona;
        private readonly ILogger<CuentaService> log;

        public CuentaService(BancaPersonaDbContext bdBancaPersona, ILogger<CuentaService> log)
        {
            this.bdBancaPersona = bdBancaPersona;
            this.log = log;
        }

        public async Task<Cuenta> CrearCuenta(Guid idCliente, decimal saldoInicial)
        {
            var cliente = await bdBancaPersona.Clientes.FindAsync(idCliente) ?? throw new KeyNotFoundException("Cliente no encontrado");

            var cuenta = new Cuenta
            {
                Id= Guid.NewGuid(),
                ClienteId = idCliente,
                NumeroCuenta = GenerarNumeroCuenta(),
                Saldo = saldoInicial
            };

            bdBancaPersona.Cuentas.Add(cuenta);

            var transaccion = new Transaccion
            {
                Id = Guid.NewGuid(),
                CuentaId = cuenta.Id,
                Fecha= DateTime.UtcNow,
                Tipo = "Deposito",
                Monto = saldoInicial,
                SaldoFinal = saldoInicial
            };
            bdBancaPersona.Transacciones.Add(transaccion);

            await bdBancaPersona.SaveChangesAsync();
            return cuenta;
        }

        public async Task<IReadOnlyList<Cuenta>> ObtenerTodosCuentas()
        {
            return await bdBancaPersona.Cuentas.AsNoTracking().ToListAsync();
        }

        public async Task<decimal> ObtenerSaldoActual(string numeroCuenta)
        {
            var cuenta = await bdBancaPersona.Cuentas.AsNoTracking().FirstOrDefaultAsync(a => a.NumeroCuenta == numeroCuenta)
                ?? throw new KeyNotFoundException("Cuenta no encontrada");
            return cuenta.Saldo;
        }

        public async Task<Transaccion> RealizarDeposito(string numeroCuenta, decimal monto)
        {
            if (monto <= 0) throw new ArgumentException("El monto debe ser positivo", nameof(monto));
            var cuenta = await bdBancaPersona.Cuentas.FirstOrDefaultAsync(a => a.NumeroCuenta == numeroCuenta)
                ?? throw new KeyNotFoundException("Cuenta no encontrada");

            cuenta.Saldo += monto;
            var tx = new Transaccion
            {
                Id = Guid.NewGuid(),
                CuentaId = cuenta.Id,
                Fecha = DateTime.UtcNow,
                Tipo = "Deposito",
                Monto = monto,
                SaldoFinal = cuenta.Saldo
            };
            bdBancaPersona.Transacciones.Add(tx);
            await bdBancaPersona.SaveChangesAsync();
            return tx;
        }

        public async Task<Transaccion> RealizarRetiro(string numeroCuenta, decimal monto)
        {
            if (monto <= 0) throw new ArgumentException("El monto debe ser positivo", nameof(monto));
            var cuenta = await bdBancaPersona.Cuentas.FirstOrDefaultAsync(a => a.NumeroCuenta == numeroCuenta)
                ?? throw new KeyNotFoundException("Cuenta no encontrada");

            if (cuenta.Saldo < monto)
                throw new InvalidOperationException("Fondos insuficientes");

            cuenta.Saldo -= monto;
            var tx = new Transaccion
            {
                Id = Guid.NewGuid(),
                CuentaId = cuenta.Id,
                Fecha = DateTime.UtcNow,
                Tipo = "Retiro",
                Monto = monto,
                SaldoFinal = cuenta.Saldo
            };
            bdBancaPersona.Transacciones.Add(tx);
            await bdBancaPersona.SaveChangesAsync();
            return tx;
        }

        public async Task<Cuenta> ObtenerCuentaPorNumCuenta(string numeroCuenta)
        {
            var cuenta = await bdBancaPersona.Cuentas.Include(c=>c.Cliente).AsNoTracking().FirstOrDefaultAsync(c => c.NumeroCuenta == numeroCuenta)
                ?? throw new KeyNotFoundException("Cuenta no encontrado");
            return cuenta;
        }

        public async Task<Transaccion> AplicarIntereses(string numeroCuenta, decimal tasa)
        {
            if (tasa <= 0)
                throw new ArgumentException("La tasa de interés debe ser positiva", nameof(tasa));

            var cuenta = await bdBancaPersona.Cuentas
                .FirstOrDefaultAsync(c => c.NumeroCuenta == numeroCuenta)
                ?? throw new KeyNotFoundException("Cuenta no encontrada");

            decimal interes = cuenta.Saldo * (tasa / 100);

            cuenta.Saldo += interes;

            var tx = new Transaccion
            {
                Id = Guid.NewGuid(),
                CuentaId = cuenta.Id,
                Fecha = DateTime.UtcNow,
                Tipo = "Interes",
                Monto = interes,
                SaldoFinal = cuenta.Saldo
            };

            bdBancaPersona.Transacciones.Add(tx);

            await bdBancaPersona.SaveChangesAsync();

            return tx;
        }

        public async Task<IReadOnlyList<Transaccion>> ObtenerTransacciones(string numeroCuenta)
        {
            var cuenta = await bdBancaPersona.Cuentas.Include(a => a.Transacciones).FirstOrDefaultAsync(a => a.NumeroCuenta == numeroCuenta)
                ?? throw new KeyNotFoundException("Cuenta no encontrada");

            return cuenta.Transacciones.OrderBy(t => t.Fecha).ToList();
        }

        private string GenerarNumeroCuenta()
        {
            return DateTime.UtcNow.Ticks.ToString();
        }

    }
}
