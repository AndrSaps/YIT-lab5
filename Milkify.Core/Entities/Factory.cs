using System.ComponentModel.DataAnnotations;

namespace Milkify.Core.Entities
{
    public class Factory
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public int Performance { get; set; }

        public long LocationId { get; set; }

        public virtual Location Location { get; set; }
    }
}