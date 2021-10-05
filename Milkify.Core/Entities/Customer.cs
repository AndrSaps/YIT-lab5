using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Milkify.Core.Entities
{
    public class Customer
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public long LocationId { get; set; }

        public virtual Location Location { get; set; }
    }
}
