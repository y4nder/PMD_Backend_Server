using Newtonsoft.Json;

namespace PMD_Backend.models
{
    public class VehicleType
    {
        public int Id { get; set; } 
        public string Name { get; set; } = null!;
        public double Flagdown { get; set; }
        public double AdditionalFee { get; set; }


        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
