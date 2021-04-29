using HotelExercise.DataAccess;
using HotelExercise.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelExercise
{
    class Program
    {
        private static HotelSpecials[] specialsArray = new HotelSpecials[] {
            new() { Special = "Spa" },
            new() { Special = "Sauna" },
            new() { Special = "Dog friendly" },
            new() { Special = "Indoor pool" },
            new() { Special = "Outdoor pool" },
            new() { Special = "Bike rental" },
            new() { Special = "eCar charging station" },
            new() { Special = "Vegetarian cuisine" },
            new() { Special = "Organic food" },
        };


        static async Task Main(string[] args)
        {
            //entry point to the application
            Console.WriteLine("Welcome to the Hotel Manager Console App!");

            //create a database context instance
            var factory = new HotelsContextFactory();
            using var dbContext = factory.CreateDbContext();

            //check if HotelSpecials list is empty; if not add some predefined data
            if (!await dbContext.HotelSpecials.AnyAsync())
            {
                Console.WriteLine("Checking the database content");
                Console.WriteLine("The list of specials was empty");
                await AddHotelSpecialsAsync(dbContext);
            }

            //for testing purposes - Adds one hotel to the database 
            //await AddFirstHotelAsync(dbContext);

            await LoadMenuAsync(dbContext);

            //ending point of the application
            Console.ReadLine();
            Console.WriteLine("Everything is done. Bye bye.");
        }

        private static async Task LoadMenuAsync(HotelsContext dbContext)
        {
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("0 - add a new hotel");
            Console.WriteLine("1 - show a hotel by Id");
            Console.WriteLine("2 - show hotels list");
            Console.WriteLine("Press ESC to exit");
            var input = Console.ReadKey();

            switch (input.Key)
            {
                case ConsoleKey.D0:
                    Console.Clear();
                    await AddHotelAsync(dbContext);
                    break;
                case ConsoleKey.NumPad0:
                    Console.Clear();
                    await AddHotelAsync(dbContext);
                    break;
                case ConsoleKey.D1:
                    Console.Clear();
                    Console.WriteLine("Id: ");
                    int id = int.Parse(Console.ReadLine());
                     await ShowHotelById(id, dbContext);
                    break;
                case ConsoleKey.NumPad1:
                    Console.Clear();
                    Console.WriteLine("Id: ");
                    int id2 = int.Parse(Console.ReadLine());
                    await ShowHotelById(id2, dbContext);
                    break;
                case ConsoleKey.NumPad2:
                    await ShowHotels(dbContext);
                    break;
                case ConsoleKey.D2:
                    await ShowHotels(dbContext);

                    break;

                case ConsoleKey.Escape:
                    break;
            }
        }

        private static async Task ShowHotelById(int id, HotelsContext dbContext)
        {
            var hotel = await dbContext.Hotels
                .Include(va => va.Address)
                .Include(va => va.HotelSpecials)
                .Include(va => va.RoomTypes)
                .ThenInclude(va => va.Price)
                .FirstOrDefaultAsync(q => q.Id == id);

            StringBuilder sb = new();
            sb.Append($"# {hotel.Name}\n\n")
              .AppendLine("## Location\n")
              .AppendLine($"{hotel.Address.Street} \n{hotel.Address.ZipCode} {hotel.Address.City}\n")
              .AppendLine("## Specials\n");
            foreach(var item in hotel.HotelSpecials)
            {
                sb.AppendLine($"* {item.Special}");
            }

            AppendTableToStringBuilder(sb, hotel);
            
            Console.WriteLine(sb.ToString());
            Console.WriteLine("");
        }

        private static void AppendTableToStringBuilder(StringBuilder sb, Hotel hotel)
        {
            string room = "Room Type";
            string size = "Size";
            string from = "Price Valid From";
            string to = "Price Valid To";
            string price = "Price in";

            sb.AppendLine()
             .AppendLine("# Room Types\n")
             .AppendLine(String.Format("| {0,25} | {1,5} | {2,20} | {3,20} | {4,10} EUR |", room, size, from, to, price))
             .AppendLine("| ------------------------- | ----- | -------------------- | -------------------- | -------------- |");


            foreach (var item in hotel.RoomTypes)
            {
                var line = String.Format("| {0,25} | {1,5} | {2,20} | {3,20} | {4,10} EUR |", item.Title, item.Size, item.Price.ValidFrom, item.Price.ValidUntil, item.Price.PriceEur);
                sb.AppendLine(line);
            }
           
        }

        private static async Task ShowHotels(HotelsContext dbContext)
        {
            Console.Clear();
            var items = await dbContext.Hotels.ToArrayAsync();
            foreach(var item in items)
            {
                Console.WriteLine($"{item.Id}: {item.Name}");
            }
            await LoadMenuAsync(dbContext);
        }

        private static async Task AddHotelAsync(HotelsContext dbContext)
        {
            Address address = new();



            Console.WriteLine("Welcome in the Add Hotel Menu. Please follow the instructions. Each one confirm by pressing the enter button");

            //Hotel name property
            Console.WriteLine("Enter a hotel name:");
            var inputName = Console.ReadLine();
            string hotelName = (string.IsNullOrEmpty(inputName)) ? "no name" : inputName;
            Console.Clear();


            //Address - street
            Console.WriteLine("Address:");
            Console.WriteLine("street(with house number):");
            var inputStreet = Console.ReadLine();
            address.Street = (string.IsNullOrEmpty(inputStreet)) ? "no name" : inputStreet;

            //Address - ZipCode
            Console.WriteLine("Zip Code:");
            var inputZipCode = Console.ReadLine();
            address.ZipCode = (string.IsNullOrEmpty(inputZipCode)) ? "no name" : inputZipCode;

            //Address - City
            Console.WriteLine("City:");
            var inputCity = Console.ReadLine();
            address.City = (string.IsNullOrEmpty(inputCity)) ? "no name" : inputCity;
            Console.Clear();


            //Hotel Specials
            Console.WriteLine("Which of the following Hotel specials you choose:");
            var specials = dbContext.HotelSpecials;
            var chosenSpecials = new List<HotelSpecials>();
            foreach (var item in specials)
            {
                Console.WriteLine($"{item.Id} - {item.Special}");
            }

            bool repeat;
            do
            {
                repeat = false;
                var correctInput = int.TryParse(Console.ReadLine(), out int result);
                if (correctInput == false)
                {
                    Console.WriteLine("Wrong input. Try typing only one digit and press enter");
                    repeat = true;
                }
                else
                {
                    if (!specials.Any(p => p.Id == result))
                    {
                        Console.WriteLine("Wrong digit. Choose one of the list above");
                        repeat = true;
                    }
                    else
                    {
                        chosenSpecials.Add(specials.FirstOrDefault(p => p.Id == result));
                        Console.WriteLine("You have already chosen: ");
                        chosenSpecials.ForEach(s => Console.WriteLine(s.Special));
                    }
                }
                Console.WriteLine("Wanna add more? (y/n)");
                if (Console.ReadLine() == "y")
                { repeat = true; }
                else
                { repeat = false; };
                
            } while (repeat);



            //Room Types:
            var roomTypes = new List<RoomType>();
            Console.Clear();
            Console.WriteLine("Would you like to add a Room Type? (y/n)");
            if (Console.ReadLine() == "y")
            {
                bool repeat2;
                do
                {
                    repeat2 = false;
                    roomTypes.Add(AddRoomType());

                    Console.WriteLine("Wanna add more? (y/n)");
                    if (Console.ReadLine() == "y")
                    { repeat2 = true; }
                    else
                    { repeat2 = false; };
                    Console.Clear();
                } while (repeat2);
            }
            else
            {
                Console.WriteLine("exited");
            }



            //Create Hotel instance to add to the database
            Hotel hotel = new()
            {
                Name = hotelName,
                Address = address,
                HotelSpecials = chosenSpecials,
                RoomTypes = roomTypes
            };




            //Add object to database and save it asynchronously
            await dbContext.AddAsync(hotel);
            await dbContext.SaveChangesAsync();

            Console.Clear();
            Console.WriteLine($"Hotel {hotel.Name} added to the database with id: {hotel.Id}");
        }

        private static RoomType AddRoomType()
        {
            string title, description;
            float size;
            bool disabilityAccess;
            int amount;
            decimal priceEur;
            DateTime? validFrom;
            DateTime? validUntil;


            Console.WriteLine("Type room title:");
            title = Console.ReadLine();
            Console.WriteLine("Type description for the room");
            description = Console.ReadLine();
            Console.WriteLine("Type size of the room ( ex.:  12,2  )");
            size = float.TryParse(Console.ReadLine(), out float result) ? result : 0;
            Console.WriteLine("Is the room accessible for disabled people? (y/n)");
            string answer = Console.ReadLine();
            disabilityAccess = (answer == "y") ? true : false;
            Console.WriteLine("Type amount of available rooms ( ex.: 5 )");
            amount = int.TryParse(Console.ReadLine(), out int result2) ? result2 : 0;
            Console.WriteLine("Type desired price ( ex.: 123,45 )");
            priceEur = decimal.TryParse(Console.ReadLine(), out decimal result3) ? result3 : 0;
            Console.WriteLine("Type starting date for the price you entered or leave empty ( ex.: 08/18/2018 )");
            validFrom = DateTime.TryParse(Console.ReadLine(), out DateTime result4) ? result4 : null;
            Console.WriteLine("Type ending date for the price you entered or leave empty ( ex.: 08/18/2018 )");
            validUntil = DateTime.TryParse(Console.ReadLine(), out DateTime result5) ? result5 : null;


            RoomType typeOne = new()
            {
                Title = title,
                Description = description,
                Size = size,
                DisabilityAccess = disabilityAccess,
                Amount = amount,
                Price = new()
                {
                    PriceEur = priceEur,
                    ValidFrom = validFrom,
                    ValidUntil = validUntil
                }
            };

            return typeOne;
        }

        async static Task AddFirstHotelAsync(HotelsContext dbContext)
        {

            var specialOne = await dbContext.HotelSpecials.FirstOrDefaultAsync(q => q.Id == 8);
            var specialTwo = await dbContext.HotelSpecials.FirstOrDefaultAsync(q => q.Id == 7);
            var specialThree = await dbContext.HotelSpecials.FirstOrDefaultAsync(q => q.Id == 6);
            var specialFour = await dbContext.HotelSpecials.FirstOrDefaultAsync(q => q.Id == 1);
            var roomTypeOne = new RoomType()
            {
                Title = "Cudowny pokoik dla dwojga z widokiem na basen",
                Description = "Wszystko fajnie w tym pokoiku. Nic dodać nic ująć. Nic tylko przyjeżdżać na wypoczynek.",
                Size = 20.1f,
                DisabilityAccess = false,
                Amount = 10,
                Price = new()
                {
                    PriceEur = 39m,
                }
            };
            var roomTypeTwo = new RoomType()
            {
                Title = "Apartament królewski z bezpośrednim wyjściem na taras widokowy",
                Description = "Jeśli szukasz iście królewskiego odpoczynku, to ten apartament jest właśnie dla ciebie!",
                Size = 41f,
                DisabilityAccess = true,
                Amount = 2,
                Price = new()
                {
                    PriceEur = 189m,
                    ValidFrom = new DateTime(2020, 1, 1),
                    ValidUntil = new DateTime(2021, 1, 1)
                }
            };

            Hotel hotelOne = new()
            {
                Name = "Hotel pod Dębem",
                Address = new Address()
                {
                    Street = "Czarnieckiego",
                    ZipCode = "44-800",
                    City = "Zabrze"
                },
                HotelSpecials = new()
                {
                    specialOne,
                    specialTwo,
                    specialThree,
                    specialFour
                },
                RoomTypes = new()
                {
                    roomTypeOne,
                    roomTypeTwo
                }
            };
            await dbContext.AddAsync(hotelOne);

            await dbContext.SaveChangesAsync();

            Console.WriteLine($"Hotel {hotelOne.Name} added to the database with id: {hotelOne.Id}");


        }

        async static Task AddHotelSpecialsAsync(HotelsContext dbContext)
        {
            await dbContext.AddRangeAsync(specialsArray);
            await dbContext.SaveChangesAsync();
            Console.WriteLine("Hotel specials list populated successfully");
        }
    }
}
