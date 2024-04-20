using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedData.Models
{
    public class Destinations
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public DestinationsType Type { get; set; }

        public string Code { get; set; } = string.Empty;

        public double Price { get; set; }
    }
}
