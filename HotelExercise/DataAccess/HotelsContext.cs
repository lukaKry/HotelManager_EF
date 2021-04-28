using HotelExercise.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelExercise.DataAccess
{
    class HotelsContext : DbContext
    {
        public DbSet<Hotel> Hotels { get; set; }      

        public DbSet<Address> Addresses { get; set; }     

        public DbSet<HotelSpecials> HotelSpecials { get; set; } 

        public DbSet<Price> Prices { get; set; }      

        public DbSet<RoomType> RoomTypes { get; set; }      



#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public HotelsContext(DbContextOptions<HotelsContext> options) : base(options)     //required contructor 
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        { }

    }
}
