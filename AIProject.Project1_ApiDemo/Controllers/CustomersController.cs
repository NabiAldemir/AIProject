using AIProject.Project1_ApiDemo.Context;
using AIProject.Project1_ApiDemo.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AIProject.Project1_ApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ApiContext _context;

        public CustomersController(ApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetCustomers()
        {
            var customers = _context.Customers.ToList();
            return Ok(customers);
        }

        [HttpPost]
        public IActionResult CreateCustomer(Customer customer) 
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
            return Ok("Musteri Ekleme Basarili");
        }

        [HttpDelete]
        public IActionResult DeleteCustomer(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null)
            {
                return NotFound("Musteri Bulunamadi");
            }

            _context.Customers.Remove(customer);
            _context.SaveChanges();
            return Ok("Musteri Silme Basarili");
        }

        [HttpGet("GetCustomerById")]
        public IActionResult GetCustomerById(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null)
            {
                return NotFound("Musteri Bulunamadi");
            }

            return Ok(customer);
        }

        [HttpPut]
        public IActionResult UpdateCustomer(Customer customer)
        {
            _context.Customers.Update(customer);
            _context.SaveChanges();
            return Ok("Musteri Basariyla Guncellendi");
        }
    }
}
