using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelExercise.Models
{
    public class HotelSpecials
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Special { get; set; } = string.Empty;

        public List<Hotel> Hotels { get; set; } = new();
    }
}