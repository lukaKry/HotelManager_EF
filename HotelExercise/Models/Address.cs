using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelExercise.Models
{
    public class Address
    {
        
        public int Id { get; set; }

        [MaxLength(250)]
        public string Street { get; set; } = string.Empty;

        [MaxLength(20)]
        public string ZipCode { get; set; } = string.Empty;

        [MaxLength(100)]
        public string City { get; set; } = string.Empty;

        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }

    }
}