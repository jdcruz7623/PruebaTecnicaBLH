using BLH_BancaPersona_api.DTOs;
using BLH_BancaPersona_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BLH_BancaPersona_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : Controller
    {
        private readonly IClienteService clienteService;

        public ClienteController(IClienteService iclienteService)
        {
            clienteService = iclienteService;
        }

        [HttpPost]
        public async Task<IActionResult> CrearCliente([FromBody] ClienteDto clienteDto)
        {
            var cliente = await clienteService.CrearCliente(clienteDto.Nombre, clienteDto.FechaNacimiento, clienteDto.Sexo, clienteDto.Ingresos);
            return CreatedAtAction(nameof(ObtenerClientePorId), new { idCliente = cliente.Id }, cliente);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodosClientes()
        {
            var clientes = await clienteService.ObtenerTodosClientes();
            return Ok(clientes);
        }

        [HttpGet("{idCliente}")]
        public async Task<IActionResult> ObtenerClientePorId(Guid idCliente)
        {
            try
            {
                var cliente = await clienteService.ObtenerClientePorId(idCliente);
                return Ok(cliente);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
