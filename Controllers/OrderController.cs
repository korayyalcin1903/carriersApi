using carriersApi.Models;
using CarriersAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace carriersApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class OrderController: Controller
    {
        private readonly CarriersContext _context;

        public OrderController(CarriersContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _context.Orders.ToListAsync();
            if(orders.Count > 0){
                return Ok(orders);
            }
            return BadRequest();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int? id)
        {
            if(id == null){
                return BadRequest();
            }

            var order = await _context.Orders.FirstOrDefaultAsync(c => c.OrderID == id);
            if(order == null){
                return NotFound();
            }

            return Ok(new {
                    message = $"{id} numaralı kargo firması listelendi",
                    data = order
                });
        }
    
        [HttpPost]
        public async Task<IActionResult> PostOrder(Orders entity)
        {
            var carriersConfiguration = await _context.CarrierConfigurations.FirstOrDefaultAsync(c => c.CarrierId == entity.CarrierId);
            var carrier = await _context.Carriers.FirstOrDefaultAsync(c => c.CarrierID == entity.CarrierId);

            if (carriersConfiguration != null && carrier != null){
                decimal orderCarrierCost = carriersConfiguration.CarrierCost;

                if (entity.OrderDesi > carriersConfiguration.CarrierMaxDesi){
                    orderCarrierCost += carrier.CarrierPlusDesiCost * (entity.OrderDesi - carriersConfiguration.CarrierMaxDesi);
                }

                entity.OrderCarrierCost = orderCarrierCost;

                _context.Orders.Add(entity);
                
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetOrder), new { id = entity.OrderID }, new {
                    message = "Sipariş eklendi",
                    data = entity
                });
            }
            
            return BadRequest();
        }
    
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int? id, Orders entity)
        {
            if(id == null){
                return BadRequest();
            }

            var order = await _context.Orders.FirstOrDefaultAsync(c => c.OrderID == id);

            if(order != null){
                var carriersConfiguration = await _context.CarrierConfigurations.FirstOrDefaultAsync(c => c.CarrierId == entity.CarrierId);
                var carrier = await _context.Carriers.FirstOrDefaultAsync(c => c.CarrierID == entity.CarrierId);

                if (carriersConfiguration != null && carrier != null){
                    decimal orderCarrierCost = carriersConfiguration.CarrierCost;

                    if (entity.OrderDesi > carriersConfiguration.CarrierMaxDesi){
                        orderCarrierCost += carrier.CarrierPlusDesiCost * (entity.OrderDesi - carriersConfiguration.CarrierMaxDesi);
                    }

                    entity.OrderCarrierCost = orderCarrierCost;

                    order.CarrierId = entity.CarrierId;
                    order.OrderDesi = entity.OrderDesi;
                    order.OrderCarrierCost = entity.OrderCarrierCost;

                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                    return Ok(new {
                        message = $"{order.OrderID} numaralı order güncellendi.",
                        data = order
                    });
                }
            }
        
            return BadRequest();
        }
    
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int? id)
        {
            if (id == null)
            {
                return BadRequest("Geçersiz ID");
            }

            var order = await _context.Orders.FirstOrDefaultAsync(c => c.OrderID == id);
            if (order == null)
            {
                return NotFound("Aradığınız sipariş bulunamadı.");
            }

            _context.Orders.Remove(order);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest($"Bir hata oluştu: {ex.Message}");
            }

            return Ok(new
            {
                message = $"{order.OrderID} numaralı sipariş başarıyla silindi.",
                data = order
            });
        }

    }
}