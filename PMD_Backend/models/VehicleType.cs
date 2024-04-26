using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            string parsed = $"Id            : {Id}\n" +
                            $"Name          : {Name}\n" +
                            $"Flagdown      : {Flagdown} \n" +
                            $"Additional Fee: {AdditionalFee} \n";
            
            return parsed;
        }
    }
}
