using BLH_BancaPersona_api.Models;

namespace BLH_BancaPersona_api.Services
{
    public interface IClienteService
    {
        Task<Cliente> CrearCliente(string nombre, DateTime fechaNacimiento, string sexo, decimal ingresos);
        Task<IReadOnlyList<Cliente>> ObtenerTodosClientes();
        Task<Cliente> ObtenerClientePorId(Guid id);
    }
}
