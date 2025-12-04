using System.Security.Principal;

namespace BLH_BancaPersona_api.Models
{
    public sealed class Cliente
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = null!;
        public DateTime FechaNacimiento { get; set; }
        public string Sexo { get; set; } = null!;
        public decimal Ingresos { get; set; }

        public ICollection<Cuenta> Cuentas { get; set; } = new List<Cuenta>();
    }
}
