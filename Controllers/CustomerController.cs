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
using Microsoft.AspNetCore.JsonPatch;

//Task 1: Extend the solution to have a customer controller
//Used the New Scaffolded Feature to generate this file

namespace IndustryConnect_Week5_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IndustryConnectWeek2Context _context;

        public CustomerController(IndustryConnectWeek2Context context)
        {
            _context = context;
        }

        // GET: api/Customer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
        {
            var customersDto = await _context.Customers.Select(c => CustomerMapper.EntityToCustomerDto(c)).ToListAsync();

            if (customersDto.Count > 0)
            {
                return Ok(customersDto);
            }
            
            return BadRequest("There are no customer record to display");
        }

        // GET: api/Customer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return CustomerMapper.EntityToCustomerDto(customer);
        }

        // PUT: api/Customer/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerDto>> PutCustomer(int id, CustomerDto customerDto)
        {
            if (id != customerDto.Id)
            {
                return BadRequest();
            }

            var customer = CustomerMapper.CustomerDtoToEntity(customerDto);

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(CustomerMapper.EntityToCustomerDto(customer));
        }

        // POST: api/Customer
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CustomerDto>> PostCustomer(CustomerDto customerDto)
        {
            var customer = CustomerMapper.CustomerDtoToEntity(customerDto);

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            var newCustomerDto = CustomerMapper.EntityToCustomerDto(customer);
            return CreatedAtAction("GetCustomer", new { id = customer.Id }, newCustomerDto);
        }

        // DELETE: api/Customer/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return $"Customer with ID: {id} deleted successfully!!!";
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<CustomerDto>> PatchCustomerDetails(int id, [FromBody] JsonPatchDocument<CustomerDto> patchDto)
        {
            try
            {
                if (patchDto == null)
                {
                    return BadRequest("No values were send to change");
                }

                var customer = await _context.Customers.FindAsync(id);
                if (customer == null)
                {
                    return BadRequest($"No customer with ID: {id}");
                }

                var customerDto = CustomerMapper.EntityToCustomerDto(customer);
                patchDto.ApplyTo(customerDto);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _context.Entry(customer).State = EntityState.Detached;

                customer = CustomerMapper.CustomerDtoToEntity(customerDto);
                _context.Entry(customer).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return Ok(customerDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
             
        }
        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
