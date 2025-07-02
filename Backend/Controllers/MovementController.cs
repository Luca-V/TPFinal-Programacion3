using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_Programacion3.Models;

namespace ProyectoFinal_Programacion3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovementController : ControllerBase
    {
        private readonly AppDbContext _context;
        public MovementController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movement>>> GetMovements()
        {
            var movements = await _context.Movements.Include(x => x.client).ToListAsync();

            return movements;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Movement>> GetMovement(int id)
        {
            var movement = await _context.Movements.Include(x => x.client).FirstOrDefaultAsync(x => x.id == id);

            if (movement == null) return NotFound();

            return movement;
        }

        private async Task<bool> TieneSaldoSuficiente(Movement movimiento)
        {
            var compras = await _context.Movements.Where(m => m.clientID == movimiento.clientID && m.cryptoCode == movimiento.cryptoCode && m.action.ToLower() == "purchase" && m.id != movimiento.id).SumAsync(m => m.cryptoAmount ?? 0);
            var ventas = await _context.Movements.Where(m => m.clientID == movimiento.clientID && m.cryptoCode == movimiento.cryptoCode && m.action.ToLower() == "sale" && m.id != movimiento.id).SumAsync(m => m.cryptoAmount ?? 0);

            var disponible = compras - ventas;

            return (movimiento.cryptoAmount ?? 0) <= disponible;
        }

        [HttpPost]
        public async Task<ActionResult<Movement>> PostMovement(Movement movement)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (movement.action.ToLower() == "sale" && !await TieneSaldoSuficiente(movement))
            {
                return BadRequest("El cliente no tiene suficiente saldo para realizar esta venta.");
            }


            _context.Movements.Add(movement);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMovement), new { id = movement.id }, movement);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovement(int id, Movement movement)
        {
            if (id != movement.id) return BadRequest();

            if (movement.action.ToLower() == "sale" && !await TieneSaldoSuficiente(movement))
            {
                return BadRequest("El cliente no tiene suficiente saldo para realizar esta venta.");
            }

            _context.Entry(movement).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovement(int id)
        {
            var movement = await _context.Movements.FindAsync(id);

            if (movement == null) return NotFound();

            _context.Movements.Remove(movement);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
