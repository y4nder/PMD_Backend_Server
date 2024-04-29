using Newtonsoft.Json;

namespace PMD_Backend.models
{
    public class HistoryLog
    {
        public int HistoryPK { get; set; }
        public TimeSpan Duration { get; set; }
        public string ParsedDuration { get; set; } = null!;
        public int VehicleFK { get; set; }
        public double TotalAmount { get; set; }
        public DateTime? TransactionDateTime { get; set; } 
        public int IssuerID { get; set; }
        public string IssuerName { get; set; } = null!;
        public string IssuerEmail { get; set; } = null!;
        public string Notes { get; set; } = null!;
        public int VehiclePK { get; set; }
        public string LicensePlate { get; set; } = null!;
        public int TypeAndBrandFK { get; set; }
        public string FloorLevel { get; set; } = null!;
        public DateTime ParkInDateTime { get; set; }
        public DateTime ParkOutDateTime { get; set; }
        public string OwnerFirstName { get; set; } = null!;
        public string OwnerLastName { get; set; } = null!;
        public string CreationCode { get; set; } = null!;
        public int TypeAndBrandPK { get; set; }
        public int VehicleTypeFK { get; set; }
        public int VehicleBrandFK { get; set; }
        public int VehicleTypePK { get; set; }
        public string VehicleTypeName { get; set; } = null!;
        public double Flagdown { get; set; }
        public double AdditionalFee { get; set; }
        public int BrandPK { get; set; }
        public string BrandName { get; set; } = null!;

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
