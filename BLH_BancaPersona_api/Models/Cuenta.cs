namespace BLH_BancaPersona_api.Models
{
    public sealed class Cuenta
    {
        public Guid Id { get; set; }
        public string NumeroCuenta { get; set; } = null!;
        public Guid ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
        public decimal Saldo { get; set; }
        public ICollection<Transaccion> Transacciones { get; set; } = new List<Transaccion>();
    }
}
