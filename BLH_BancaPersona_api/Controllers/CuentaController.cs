using BLH_BancaPersona_api.DTOs;
using BLH_BancaPersona_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BLH_BancaPersona_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CuentaController : Controller
    {
        private readonly ICuentaService cuentaService;

        public CuentaController(ICuentaService icuentaService) => cuentaService = icuentaService;

        [HttpPost]
        public async Task<IActionResult> CrearCuenta([FromBody] CuentaDto cuentaDto)
        {
            var cuenta = await cuentaService.CrearCuenta(cuentaDto.idCliente,cuentaDto.saldoInicial);
            return CreatedAtAction(nameof(ObtenerCuentaPorNumCuenta), new { numeroCuenta = cuenta.NumeroCuenta }, cuenta);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodosCuentas()
        {
            var cuentas = await cuentaService.ObtenerTodosCuentas();
            return Ok(cuentas);
        }

        [HttpGet("saldo-actual/{numeroCuenta}")]
        public async Task<IActionResult> ObtenerSaldoActual(string numeroCuenta)
        {
            var saldo = await cuentaService.ObtenerSaldoActual(numeroCuenta);
            return Ok(new { numeroCuenta, saldo });
        }

        [HttpPost("transaccion/deposito")]
        public async Task<IActionResult> RealizarDeposito([FromBody] TransaccionDto transaccionDto)
        {
            var identificadorTransaccion = await cuentaService.RealizarDeposito(transaccionDto.numeroCuenta, transaccionDto.monto);
            return Ok(identificadorTransaccion);
        }

        [HttpPost("transaccion/retiro")]
        public async Task<IActionResult> RealizarRetiro([FromBody] TransaccionDto transaccionDto)
        {
            try
            {
                var identificadorTransaccion = await cuentaService.RealizarRetiro(transaccionDto.numeroCuenta, transaccionDto.monto);
                return Ok(identificadorTransaccion);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{numeroCuenta}")]
        public async Task<IActionResult> ObtenerCuentaPorNumCuenta(string numeroCuenta)
        {
            try
            {
                var cuenta = await cuentaService.ObtenerCuentaPorNumCuenta(numeroCuenta);
                return Ok(cuenta);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("transaccion/interes")]
        public async Task<IActionResult> AplicarIntereses([FromBody] InteresDto interesDto)
        {
            var interes = await cuentaService.AplicarIntereses(interesDto.numeroCuenta, interesDto.tasa);
            return Ok(interes);
        }

        [HttpGet("transacciones/{numeroCuenta}")]
        public async Task<IActionResult> ObtenerTransacciones(string numeroCuenta)
        {
            var txs = await cuentaService.ObtenerTransacciones(numeroCuenta);
            return Ok(txs);
        }
    }
}
