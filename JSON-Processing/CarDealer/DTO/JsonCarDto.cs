using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.DTO
{
    public class JsonCarDto
    {
        public string Make { get; set; }

        public string Model { get; set; }

        public long TravelledDistance { get; set; }

        public int[] PartsId { get; set; }
    }
}
