using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Event.Net.Server.Data;
using AutoMapper;
using Event.Net.Shared;
using Event.Net.Server.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Event.Net.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ReviewsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviews()
        {
            var entities = await _context.Reviews.ToListAsync();

            return Ok(_mapper.Map<IEnumerable<ReviewDto>>(entities));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewDto>> GetReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ReviewDto>(review));
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutReview(int id, ReviewDto reviewDto)
        {
            var review = _mapper.Map<Review>(reviewDto);

            if (id != review.Id)
            {
                return BadRequest();
            }

            _context.Entry(review).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
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
        public async Task<ActionResult<ReviewDto>> PostReview(ReviewDto reviewDto)
        {
            var review = _mapper.Map<Review>(reviewDto);
            review.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReview", new { id = review.Id }, _mapper.Map<ReviewDto>(review));
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
    }
}
