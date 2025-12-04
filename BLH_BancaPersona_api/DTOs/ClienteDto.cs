namespace BLH_BancaPersona_api.DTOs
{
    public class ClienteDto
    {
        public string Nombre { get; set; } = null!;
        public DateTime FechaNacimiento { get; set; }
        public string Sexo { get; set; } = null!;
        public decimal Ingresos { get; set; }
    }
}
