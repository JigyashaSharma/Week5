using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IndustryConnect_Week5_WebApi.Models;
using IndustryConnect_Week5_WebApi.Dtos;
using IndustryConnect_Week5_WebApi.Mappers;

namespace IndustryConnect_Week5_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly IndustryConnectWeek2Context _context;

        public SaleController(IndustryConnectWeek2Context context)
        {
            _context = context;
        }

        // GET: api/Sale
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SaleDto>>> GetSales()
        {
            try
            {
                var sale = await _context.Sales.Include(p => p.Product)
                .Include(c => c.Customer)
                .Include(st => st.Store)
                .Select(s => SaleMapper.EntityToSaleDto(s))
                .ToListAsync();

                return Ok(sale);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Sale/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SaleDto>> GetSale(int id)
        {
            try
            {
                var sale = await _context.Sales
                .Include(p => p.Product)
                .Include(c => c.Customer)
                .Include(st => st.Store)
                .FirstOrDefaultAsync(s => s.Id == id);

                if (sale == null)
                {
                    return NotFound();
                }

                return Ok(SaleMapper.EntityToSaleDto(sale));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Sale/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<SaleDto>> PutSale(int id, SaleDto saleDto)
        {
            if (id != saleDto.Id)
            {
                return BadRequest();
            }

            var sale = SaleMapper.SaleDtoToEntity(saleDto);
            _context.Entry(sale).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var createdSale = await _context.Sales
                   .Include(s => s.Customer)
                   .Include(s => s.Product)
                   .Include(s => s.Store)
                   .FirstOrDefaultAsync(s => s.Id == sale.Id);

            if (createdSale == null)
            {
                return BadRequest();
            }
            saleDto = SaleMapper.EntityToSaleDto(createdSale);

            return Ok(saleDto);
        }

        // POST: api/Sale
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SaleDto>> PostSale(SaleDto saleDto)
        {
            try
            {
                var sale = SaleMapper.SaleDtoToEntity(saleDto);
                _context.Sales.Add(sale);
                await _context.SaveChangesAsync();

                var createdSale = await _context.Sales
                    .Include(s => s.Customer)
                    .Include(s => s.Product)
                    .Include(s => s.Store)
                    .FirstOrDefaultAsync(s => s.Id == sale.Id);

                if (createdSale == null)
                {
                    return BadRequest();
                }
                saleDto = SaleMapper.EntityToSaleDto(createdSale);

                return CreatedAtAction("GetSale", new { id = sale.Id }, saleDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        // DELETE: api/Sale/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteSale(int id)
        {
            try
            {
                var sale = await _context.Sales.FindAsync(id);
                if (sale == null)
                {
                    return NotFound();
                }

                _context.Sales.Remove(sale);
                await _context.SaveChangesAsync();

                return $"Sale with ID: {id} deleted successfully!!!!";
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private bool SaleExists(int id)
        {
            return _context.Sales.Any(e => e.Id == id);
        }
    }
}
