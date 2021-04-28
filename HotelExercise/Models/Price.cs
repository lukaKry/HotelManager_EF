using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelExercise.Models
{
    public class Price
    {
        [ForeignKey("RoomType")]
        public int Id { get; set; }
        public RoomType RoomType { get; set; }
        
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidUntil { get; set; }

        [Column(TypeName ="decimal(6,2)")]
        public decimal PriceEur { get; set; }

    }
}