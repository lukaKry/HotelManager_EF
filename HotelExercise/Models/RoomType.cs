using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelExercise.Models
{
    public class RoomType
    {
        public int Id { get; set; }
        public Hotel Hotel { get; set; }
        public int HotelId { get; set; }

        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(250)]
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(4,1)")]
        public float Size { get; set; }

        public bool DisabilityAccess { get; set; }
        public int Amount { get; set; }

        public Price Price { get; set; }

       
    }
}