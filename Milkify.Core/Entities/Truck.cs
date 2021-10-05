using System.ComponentModel.DataAnnotations;
using Milkify.Core.Entities.Membership;

namespace Milkify.Core.Entities
{
    public class Truck
    {
        [Key]
        public long Id { get; set; }

        public string SerialNumber { get; set; }

        public int LiftingWeight { get; set; }
    }
}