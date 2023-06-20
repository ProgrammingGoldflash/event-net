using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Event.Net.Server.Data;
using AutoMapper;
using Event.Net.Shared;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Event.Net.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class EventsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public EventsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        
        public async Task<ActionResult<IEnumerable<EventDto>>> GetEvents()
        {
            if (_context.Events == null)
            {
                return NotFound();
            }

            var entities = await _context.Events.Include(c => c.Reviews).AsNoTracking().ToListAsync();

            return Ok(_mapper.Map<IEnumerable<EventDto>>(entities));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventDetailsDto>> GetEvent(int id)
        {
            if (_context.Events == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.Include(c => c.Reviews).FirstOrDefaultAsync(e => e.Id == id);

            if (@event == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<EventDetailsDto>(@event));
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutEvent(int id, EventDto eventDto)
        {
            var @event = _mapper.Map<Server.Models.Event>(eventDto);

            if (id != @event.Id)
            {
                return BadRequest();
            }

            _context.Entry(@event).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<EventDto>> PostEvent(EventDto eventDto)
        {
            if (_context.Events == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Events' is null.");
            }

            var @event = _mapper.Map<Server.Models.Event>(eventDto);
            @event.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _context.Events.Add(@event);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvent", new { id = @event.Id }, _mapper.Map<EventDto>(@event));
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            if (_context.Events == null)
            {
                return NotFound();
            }
            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventExists(int id)
        {
            return (_context.Events?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
