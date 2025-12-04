using System.Security.Principal;

namespace BLH_BancaPersona_api.Models
{
    public sealed class Transaccion
    {
        public Guid Id { get; set; }
        public Guid CuentaId { get; set; }
        public Cuenta? Cuenta { get; set; }
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; } = null!;
        public decimal Monto { get; set; }
        public decimal SaldoFinal { get; set; }
    }
}
