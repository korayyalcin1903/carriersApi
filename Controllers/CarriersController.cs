using carriersApi.Models;
using CarriersAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace carriersApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class CarriersController: Controller 
    {
        private readonly CarriersContext _context;

        public CarriersController(CarriersContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCarriers()
        {
            var carriers = await _context.Carriers.ToListAsync();
            return Ok(carriers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarriers(int? id)
        {
            if(id == null){
                return NotFound();
            }

            var carrier = await _context.Carriers.Where(c => c.CarrierID == id).FirstOrDefaultAsync();
            if(carrier == null){
                return NotFound();
            } else {
                return Ok(new {
                    message = $"{id} numaralı kargo firması listelendi",
                    data = carrier
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostCarrier(Carrier entity)
        {
            _context.Carriers.Add(entity);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCarriers), new {id = entity.CarrierID}, new {
                message = "Kargo firması eklendi",
                data = entity
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCarrier(int id, [FromBody] Carrier entity)
        {
            if(id != entity.CarrierID){
                return BadRequest();
            }

            var carrier = await _context.Carriers.FirstOrDefaultAsync(c => c.CarrierID == id);

            if(carrier == null){
                return NotFound();
            }

            carrier.CarrierName = entity.CarrierName;
            carrier.CarrierIsActive = entity.CarrierIsActive;
            carrier.CarrierPlusDesiCost = entity.CarrierPlusDesiCost;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return NotFound();
            }
            
            return Ok(new {
                message = $"{carrier.CarrierName} Kargo firması düzenlendi",
                data = carrier
            });
        }
    
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarrier(int? id)
        {
            if(id == null){
                return NotFound();
            }

            var carrier = await _context.Carriers.FirstOrDefaultAsync(c => c.CarrierID == id);

            if( carrier != null){
                _context.Carriers.Remove(carrier);
            } else {
                return BadRequest("Aradığınız öğe bulunamadı");
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return NotFound();
            }

            return Ok(new {
                message = $"{carrier.CarrierID} numaralı kargo firması silindi.",
                data = carrier
            });
        }
    }
}