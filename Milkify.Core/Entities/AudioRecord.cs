using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Milkify.Core.Entities
{
    public class AudioRecord
    {
        [Key]
        public long Id { get; set; }

        public string AudioRecordUrl { get; set; }
    }
}