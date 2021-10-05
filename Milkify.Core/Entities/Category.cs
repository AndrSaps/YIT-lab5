using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Milkify.Core.Entities
{
    public class Category
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public string CategoryDescription { get; set; }
    }
}
