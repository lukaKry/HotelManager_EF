using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelExercise.Models
{
   

    public class Hotel
    {
        public int Id { get; set; }

        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        public Address Address { get; set; }

        public List<HotelSpecials> HotelSpecials { get; set; } = new();
        public List<RoomType> RoomTypes { get; set; } = new();
    }
}
