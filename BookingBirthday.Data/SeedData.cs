﻿using BookingBirthday.Data.Entities;
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
        private static readonly string PACKAGE_FILE_PATH = "Resources/package.txt";
        private static readonly string CART_FILE_PATH = "Resources/cart.txt";
        private static readonly string BOOKING_FILE_PATH = "Resources/booking.txt";
        private static readonly string PROMOTION_FILE_PATH = "Resources/promotion.txt";
        private static readonly string PAYMENT_FILE_PATH = "Resources/payment.txt";
        private static readonly string CART_PACKAGE_FILE_PATH = "Resources/cart-packages.txt";
        private static readonly string BOOKING_PACKAGE_FILE_PATH = "Resources/booking-packages.txt";
        private static readonly string USER_FILE_PATH = "Resources/user.txt";

        public static void Initialize(ModelBuilder modelBuilder)
        {
            InitializePackage(modelBuilder);
            InitializeCart(modelBuilder);
            InitializeBooking(modelBuilder);
            InitializePayment(modelBuilder);
            InitializeCartPackage(modelBuilder);
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
                modelBuilder.Entity<Package>().HasData(package); 
            }
        }

        private static void InitializeCart(ModelBuilder modelBuilder)
        {
            var cart = new List<Categories>();

            if (File.Exists(CART_FILE_PATH))
            {
                using StreamReader sr = new(CART_FILE_PATH);
                int cartID = 1;
                string? cartLine;

                while ((cartLine = sr.ReadLine()) != null)
                {
                    string[]? cartData = cartLine!.Split('|');

                    cart.Add(new Categories
                    {
                        //Id = cartID++,
                        //Total = double.Parse(cartData[0].Trim()),
                    });
                }
                modelBuilder.Entity<Categories>().HasData(cart);
            }
        }


        public static void InitializeBooking(ModelBuilder modelBuilder)
        {
            var booking = new List<Booking>();

            if (File.Exists(BOOKING_FILE_PATH))
            {
                using StreamReader sr = new(BOOKING_FILE_PATH);
                string? bookingLine;

                while ((bookingLine = sr.ReadLine()) != null)
                {
                    string[]? bookingData = bookingLine!.Split('|');

                    booking.Add(new Booking
                    {
                        Date_order = DateTime.ParseExact(bookingData[0].Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        BookingStatus = bookingData[2].Trim(),
                        Total = double.Parse(bookingData[3].Trim()),
                        UserId = int.Parse(bookingData[4].Trim()),
                        DepositPaymentId = int.Parse(bookingData[5].Trim()),
                    });
                }
                modelBuilder.Entity<Booking>().HasData(booking);
            }
        }

        public static void InitializePayment(ModelBuilder modelBuilder)
        {
            var payment = new List<DepositPayment>();

            if (File.Exists(PAYMENT_FILE_PATH))
            {
                using StreamReader sr = new(PAYMENT_FILE_PATH);

                int paymentId = 1;
                string? paymentLine;

                while ((paymentLine = sr.ReadLine()) != null)
                {
                    string[] paymentData = paymentLine!.Split("|");

                    payment.Add(new DepositPayment
                    {
                        Id = paymentId++,
                        Date = DateTime.ParseExact(paymentData[1].Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        Success = bool.Parse(paymentData[2].Trim()),
                        Token = paymentData[3].Trim(),
                        VnPayResponseCode = paymentData[4].Trim(),
                        OrderDescription = paymentData[5].Trim(),
                        BookingId = int.Parse(paymentData[6].Trim())
                    });
                }
                modelBuilder.Entity<DepositPayment>().HasData(payment);
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
                        Name = userData[0],
                        //Gender = (Gender)Enum.Parse(typeof(Gender), userData[1]),
                        DateOfBirth = DateTime.Parse(userData[2]),
                        Username = userData[3],
                        Password = userData[4],
                        Email = userData[5],
                        Phone = userData[6],
                        Address = userData[7],
                        Image_url = userData[8],
                        Role = userData[9]
                    });
                }

                modelBuilder.Entity<User>().HasData(users);
            }
        }
    }
}
