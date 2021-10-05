using System.ComponentModel.DataAnnotations;

namespace Milkify.Core.Entities
{
    public class Location
    {
        [Key]
        public long Id { get; set; }

        public string Address { get; set; }
        
        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}