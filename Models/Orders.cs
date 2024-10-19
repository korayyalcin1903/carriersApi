using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace carriersApi.Models
{
    public class Orders
    {
        [Key]
        public int OrderID { get; set; }
        public int CarrierId { get; set; }
        public int OrderDesi { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        [JsonIgnore]
        public decimal OrderCarrierCost { get; set; }
        [JsonIgnore]
        public Carrier? Carrier { get; set; } 
    }
}