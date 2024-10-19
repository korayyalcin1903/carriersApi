using carriersApi.Models;
using CarriersAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace carriersApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarrierConfigurationController: Controller
    {
        private readonly CarriersContext _context;

        public CarrierConfigurationController(CarriersContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCarriersConfiguration()
        {
            var carriers = await _context.CarrierConfigurations.ToListAsync();

            return Ok(carriers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarrierConfiguration(int? id)
        {
            if(id == null){
                return BadRequest();
            }

            var carrier = await _context.CarrierConfigurations.Where(c => c.CarrierConfigurationId == id).FirstOrDefaultAsync();
            if(carrier == null){
                return BadRequest();
            } else {
                return Ok(carrier);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostCarrierConfiguration(CarrierConfiguration entity)
        {
            var carrierExists = await _context.Carriers.AnyAsync(c => c.CarrierID == entity.CarrierId);

            if(!carrierExists){
                return BadRequest("Kargo firması bulunamıyor");
            }

            _context.CarrierConfigurations.Add(entity);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Veritabanı hatası: {ex.Message}");
            }

            return CreatedAtAction(nameof(GetCarrierConfiguration), new {id = entity.CarrierConfigurationId}, new {
                message = $"{entity.CarrierConfigurationId} numaralı yapılandırma eklendi",
                data= entity
            });
        }
    
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCarrierConfiguration(int? id, CarrierConfiguration entity)
        {
            if(id == null){
                return BadRequest();
            }

            var carrierConfiguration = await _context.CarrierConfigurations.FirstOrDefaultAsync(c => c.CarrierConfigurationId == id);

            if(carrierConfiguration == null){
                return BadRequest();
            } 

            carrierConfiguration.CarrierId = entity.CarrierId;
            carrierConfiguration.CarrierMaxDesi = entity.CarrierMaxDesi;
            carrierConfiguration.CarrierMinDesi = entity.CarrierMinDesi;
            carrierConfiguration.CarrierCost = entity.CarrierCost;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(new {
                message = $"{carrierConfiguration.CarrierConfigurationId} numaralı carrierConfiguration güncellendi.",
                data = carrierConfiguration
            });
        }
    
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarrierConfiguration(int? id)
        {
            if(id == null){
                return BadRequest();
            }

            var carrierConfiguration = await _context.CarrierConfigurations.FirstOrDefaultAsync(c => c.CarrierConfigurationId == id);

            if( carrierConfiguration != null){
                _context.CarrierConfigurations.Remove(carrierConfiguration);
            } else {
                return BadRequest("Aradığınız öğe bulunamadı");
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(new {
                message = $"{carrierConfiguration.CarrierConfigurationId} numaralı öğe silindi.",
                data = carrierConfiguration
            });
        }
    }
}