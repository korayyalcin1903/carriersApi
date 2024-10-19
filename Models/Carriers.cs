using System.Text.Json.Serialization;

namespace carriersApi.Models
{
    public class Carrier
    {
        public int CarrierID { get; set; }
        public string? CarrierName { get; set; }
        public bool CarrierIsActive { get; set; }
        public int CarrierPlusDesiCost { get; set; }

        [JsonIgnore]
        public ICollection<CarrierConfiguration>? CarrierConfigurations{ get; set; }
        [JsonIgnore]
        public ICollection<Orders>? Orders{ get; set; }
    }
}