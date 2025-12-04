using BLH_BancaPersona_api.Models;
using System.Security.Principal;

namespace BLH_BancaPersona_api.Services
{
    public interface ICuentaService
    {
        Task<Cuenta> CrearCuenta(Guid idCLiente, decimal saldoInicial);
        Task<IReadOnlyList<Cuenta>> ObtenerTodosCuentas();
        Task<decimal> ObtenerSaldoActual(string numeroCuenta);
        Task<Transaccion> RealizarDeposito(string numeroCuenta, decimal monto);
        Task<Transaccion> RealizarRetiro(string numeroCuenta, decimal monto);
        Task<Cuenta> ObtenerCuentaPorNumCuenta(string numeroCuenta);
        Task<Transaccion> AplicarIntereses(string numeroCuenta, decimal tasa);
        Task<IReadOnlyList<Transaccion>> ObtenerTransacciones(string numeroCuenta);
    }
}
