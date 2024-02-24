using BookingBirthday.Data.Entities;
using BookingBirthday.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookingBirthday.Data
{
    public static class SeedData
    {
        private static readonly string PACKAGE_FILE_PATH = "Resources/packages.txt";
        private static readonly string CART_FILE_PATH = "Resources/carts.txt";
        private static readonly string BOOKING_FILE_PATH = "Resources/bookings.txt";
        private static readonly string PROMOTION_FILE_PATH = "Resources/promotions.txt";
        private static readonly string PAYMENT_FILE_PATH = "Resources/payments.txt";
        private static readonly string CART_PACKAGE_FILE_PATH = "Resources/cart-package.txt";
        private static readonly string BOOKING_PACKAGE_FILE_PATH = "Resources/booking-package.txt";
        private static readonly string USER_FILE_PATH = "Resources/users.txt";

        public static void Initialize(ModelBuilder modelBuilder)
        {
            InitializePackage(modelBuilder);
            InitializeCart(modelBuilder);
            InitializeBooking(modelBuilder);
            InitializePromotion(modelBuilder);
            InitializePayment(modelBuilder);
            InitializeCartPackage(modelBuilder);
            InitializeBookingPackage(modelBuilder);
            InitializeUser(modelBuilder);
        }

        private static void InitializePackage(ModelBuilder modelBuilder)
        {
            var package = new List<Package>();

            if (File.Exists(PACKAGE_FILE_PATH))
            {
                using StreamReader sr = new(PACKAGE_FILE_PATH);
                int packageId = 1;
                string? packageLine;

                while ((packageLine = sr.ReadLine()) != null)
                {
                    string[]? packageeData = packageLine!.Split('|');

                    package.Add(new Package
                    {
                        Id = packageId++,
                        Name = packageeData[0].Trim(),
                        Price = Double.Parse(packageeData[1].Trim()),
                        Venue = packageeData[2].Trim(),
                        Detail = packageeData[3].Trim(),
                        PromotionId = string.IsNullOrEmpty(packageeData[4].Trim()) ? (int?)null : int.Parse(packageeData[4].Trim())
                    });
                }
                modelBuilder.Entity<Package>().HasData(package); ;
            }
        }

        private static void InitializeCart(ModelBuilder modelBuilder)
        {
            var cart = new List<Cart>();

            if (File.Exists(CART_FILE_PATH))
            {
                using StreamReader sr = new(CART_FILE_PATH);
                int cartID = 1;
                string? cartLine;

                while ((cartLine = sr.ReadLine()) != null)
                {
                    string[]? cartData = cartLine!.Split('|');

                    cart.Add(new Cart
                    {
                        Id = cartID++,
                        Total = double.Parse(cartData[0].Trim()),
                    });
                }
                modelBuilder.Entity<Cart>().HasData(cart);
            }
        }


        public static void InitializeBooking(ModelBuilder modelBuilder)
        {
            var booking = new List<Booking>();

            if (File.Exists(BOOKING_FILE_PATH))
            {
                using StreamReader sr = new(BOOKING_FILE_PATH);
                int bookingId = 1;
                string? bookingLine;

                while ((bookingLine = sr.ReadLine()) != null)
                {
                    string[]? bookingData = bookingLine!.Split('|');

                    BookingStatus bookingStatus = BookingStatus.Accepted;
                    if (int.Parse(bookingData[1].Trim()) == 2)
                    {
                        bookingStatus = BookingStatus.Declined;
                    }
                    else if (int.Parse(bookingData[1].Trim()) == 3)
                    {
                        bookingStatus = BookingStatus.Processing;
                    }

                    booking.Add(new Booking
                    {
                        Id = bookingId++,
                        Date_order = DateTime.ParseExact(bookingData[0].Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        BookingStatus = bookingStatus,
                        Total = double.Parse(bookingData[2].Trim()),
                        UserId = int.Parse(bookingData[3].Trim()),
                        PaymentId = int.Parse(bookingData[4].Trim()),
                    });
                }
                modelBuilder.Entity<Booking>().HasData(booking);
            }
        }


        public static void InitializePromotion(ModelBuilder modelBuilder)
        {
            var promotion = new List<Promotion>();

            if (File.Exists(PROMOTION_FILE_PATH))
            {
                using StreamReader sr = new(PROMOTION_FILE_PATH);
                int promotionId = 1;
                string? promotionLine;

                while ((promotionLine = sr.ReadLine()) != null)
                {
                    string[]? promotionData = promotionLine!.Split('|');

                    promotion.Add(new Promotion
                    {
                        Id = promotionId++,
                        Name = promotionData[0].Trim(),
                        FromDate = DateTime.ParseExact(promotionData[1].Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        ToDate = DateTime.ParseExact(promotionData[2].Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        DiscountPercent = double.Parse(promotionData[3].Trim()),
                        Status = (int.Parse(promotionData[4].Trim()) == 0) ? Status.Active : Status.Inactive,
                        //UserId = int.Parse(promotionData[5].Trim()),
                    });
                }
                modelBuilder.Entity<Promotion>().HasData(promotion);
            }
        }

        public static void InitializePayment(ModelBuilder modelBuilder)
        {
            var payment = new List<Payment>();

            if (File.Exists(PAYMENT_FILE_PATH))
            {
                using StreamReader sr = new(PAYMENT_FILE_PATH);

                int paymentId = 1;
                string? paymentLine;

                while ((paymentLine = sr.ReadLine()) != null)
                {
                    string[] paymentData = paymentLine!.Split("|");

                    Types types = Types.Banking;
                    if (int.Parse(paymentData[2].Trim()) == 1)
                    {
                        types = Types.ByCast;
                    }
                    else if (int.Parse(paymentData[2].Trim()) == 2)
                    {
                        types = Types.Installment;
                    }

                    payment.Add(new Payment
                    {
                        Id = paymentId++,
                        Amount = double.Parse(paymentData[0].Trim()),
                        Date = DateTime.ParseExact(paymentData[1].Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        Types = types,
                        BookingId = int.Parse(paymentData[3].Trim())
                    });
                }
                modelBuilder.Entity<Payment>().HasData(payment);
            }
        }



        public static void InitializeCartPackage(ModelBuilder modelBuilder)
        {
            var cartPackage = new List<CartPackage>();

            if (File.Exists(CART_PACKAGE_FILE_PATH))
            {
                using StreamReader sr = new(CART_PACKAGE_FILE_PATH);
                string? cartPackageLine;

                while ((cartPackageLine = sr.ReadLine()) != null)
                {
                    string[]? cartPackageData = cartPackageLine!.Split('|');

                    cartPackage.Add(new CartPackage
                    {
                        CartId = int.Parse(cartPackageData[0].Trim()),
                        PackageId = int.Parse(cartPackageData[1].Trim())
                    });
                }
                modelBuilder.Entity<CartPackage>().HasData(cartPackage);
            }
        }


        public static void InitializeBookingPackage(ModelBuilder modelBuilder)
        {
            var bookingPackage = new List<BookingPackage>();

            if (File.Exists(BOOKING_PACKAGE_FILE_PATH))
            {
                using StreamReader sr = new(BOOKING_PACKAGE_FILE_PATH);
                string? bookingPackageLine;

                while ((bookingPackageLine = sr.ReadLine()) != null)
                {
                    string[]? bookingPackageData = bookingPackageLine!.Split('|');

                    bookingPackage.Add(new BookingPackage
                    {
                        BookingId = int.Parse(bookingPackageData[0].Trim()),
                        PackageId = int.Parse(bookingPackageData[1].Trim())
                    });
                }
                modelBuilder.Entity<BookingPackage>().HasData(bookingPackage);
            }
        }

        public static void InitializeUser(ModelBuilder modelBuilder)
        {
            var users = new List<User>();

            if (File.Exists(USER_FILE_PATH))
            {
                using StreamReader sr = new(USER_FILE_PATH);
                
                int userId = 1;
                string? userLine;

                while ((userLine = sr.ReadLine()) != null)
                {
                    string[]? userData = userLine!.Split('|');


                    users.Add(new User
                    {
                        Id = userId++,
                        Username = userData[0].Trim(),
                        Password = userData[1].Trim(),
                        Email = userData[2].Trim(),
                        Role = userData[3].Trim()
                    });
                }

                modelBuilder.Entity<User>().HasData(users);
            }
        }
    }
}
