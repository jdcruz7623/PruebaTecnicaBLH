using BLH_BancaPersona_api.Data;
using BLH_BancaPersona_api.Models;
using Microsoft.EntityFrameworkCore;

namespace BLH_BancaPersona_api.Services
{
    public class ClienteService: IClienteService
    {
        private readonly BancaPersonaDbContext bdBancaPersona;
        private readonly ILogger<ClienteService> log;

        public ClienteService(BancaPersonaDbContext bdbp, ILogger<ClienteService> logger)
        {
            bdBancaPersona = bdbp;
            log = logger;
        }

        public async Task<Cliente> CrearCliente(string nombre, DateTime fechaNacimiento, string sexo, decimal ingresos)
        {
            var cliente = new Cliente
            {
                Id = Guid.NewGuid(),
                Nombre = nombre,
                FechaNacimiento = fechaNacimiento,
                Sexo = sexo,
                Ingresos = ingresos
            };

            bdBancaPersona.Clientes.Add(cliente);
            await bdBancaPersona.SaveChangesAsync();
            log.LogInformation("Cliente creado: {Id Cliente}", cliente.Id);

            return cliente;
        }

        public async Task<IReadOnlyList<Cliente>> ObtenerTodosClientes()
        {
            return await bdBancaPersona.Clientes.Include(c=>c.Cuentas).AsNoTracking().ToListAsync();
        }

        public async Task<Cliente> ObtenerClientePorId(Guid id)
        {
            var cliente = await bdBancaPersona.Clientes.Include(c => c.Cuentas).AsNoTracking().FirstOrDefaultAsync(c => c.Id == id)
                ?? throw new KeyNotFoundException("Cliente no encontrado");
            return cliente;
        }
    }
}
