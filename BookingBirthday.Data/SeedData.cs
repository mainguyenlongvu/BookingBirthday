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
        private static readonly string GUEST_FILE_PATH = "Resources/guest.txt";
        private static readonly string HOST_FILE_PATH = "Resources/host.txt";
        private static readonly string PACKAGE_TYPES_FILE_PATH = "Resources/package.txt";
        private static readonly string CART_FILE_PATH = "Resources/cart.txt";
        private static readonly string BILL_FILE_PATH = "Resources/bill.txt";
        private static readonly string BOOKING_FILE_PATH = "Resources/booking.txt";
        private static readonly string SERVICE_FILE_PATH = "Resources/service.txt";
        private static readonly string PROMOTION_FILE_PATH = "Resources/promotion.txt";
        private static readonly string PAYMENT_FILE_PATH = "Resources/payment.txt";
        private static readonly string PACKAGE_SERVICE_FILE_PATH = "Resources/package-service.txt";
        private static readonly string CART_SERVICE_FILE_PATH = "Resources/cart-service.txt";
        private static readonly string CART_PACKAGE_FILE_PATH = "Resources/cart-package.txt";
        private static readonly string BOOKING_PACKAGE_FILE_PATH = "Resources/booking-package.txt";
        private static readonly string BOOKING_SERVICE_FILE_PATH = "Resources/booking-service.txt";

        


        public static void Initialize(ModelBuilder modelBuilder)
        {
            InitializeGuest(modelBuilder);
            InitializeHost(modelBuilder);
            InitializePackage(modelBuilder);
            InitializeCart(modelBuilder);
            InitializeBill(modelBuilder);
            InitializeBooking(modelBuilder);
            InitializeService(modelBuilder);
            InitializePromotion(modelBuilder);
            InitializePayment(modelBuilder);
            InitializePackageService(modelBuilder);
            InitializeCartService(modelBuilder);
            InitializeCartPackage(modelBuilder);
            InitializeBookingPackage(modelBuilder);

        }

        private static void InitializeGuest(ModelBuilder modelBuilder)
        {
            var guest = new List<Guest>();

            if (File.Exists(GUEST_FILE_PATH))
            {
                using (StreamReader sr = new StreamReader(GUEST_FILE_PATH))
                {
                    string? guestLine;
                    while ((guestLine = sr.ReadLine()) != null)
                    {
                        string[]? guestData = guestLine!.Split(',');

                        guest.Add(new Guest
                        {
                            Id = int.Parse(guestData[0].Trim()),
                            Username = guestData[1].Trim(),
                            Password = guestData[2].Trim(),
                            Name = guestData[3].Trim(),
                            Gender = (guestData[4].Trim() == "Male") ? Gender.Male : Gender.Female,
                            DateOfBirth = DateTime.ParseExact(guestData[5].Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            Email = guestData[6].Trim() + "@gmail.com",
                            Address = guestData[7].Trim(),
                            Phone = guestData[8].Trim(),
                        });
                        modelBuilder.Entity<Guest>().HasData(guest);

                    }
                }
            }
        }

        private static void InitializeHost(ModelBuilder modelBuilder)
        {
            var host = new List<Host>();

            if (File.Exists(HOST_FILE_PATH))
            {
                using (StreamReader sr = new StreamReader(HOST_FILE_PATH))
                {
                    string? hostLine;

                    while ((hostLine = sr.ReadLine()) != null)
                    {
                        string[]? hostData = hostLine!.Split(',');

                        host.Add(new Host
                        {
                            Id = int.Parse(hostData[0].Trim()),
                            Username = hostData[1].Trim(),
                            Password = hostData[2].Trim(),
                            Name = hostData[3].Trim(),
                            Gender = (hostData[4].Trim() == "Male") ? Gender.Male : Gender.Female,
                            DateOfBirth = DateTime.ParseExact(hostData[5].Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            Email = hostData[6].Trim() + "@gmail.com",
                            Address = hostData[7].Trim(),
                            Phone = hostData[8].Trim(),
                        });
                    }

                    modelBuilder.Entity<Host>().HasData(host);
                }
            }
        }

        private static void InitializePackage(ModelBuilder modelBuilder)
        {
            var package = new List<Package>();

            if (File.Exists(PACKAGE_TYPES_FILE_PATH))
            {
                using (StreamReader sr = new StreamReader(PACKAGE_TYPES_FILE_PATH))
                {
                    string? packageLine;

                    while ((packageLine = sr.ReadLine()) != null)
                    {
                        string[]? packageeData = packageLine!.Split(',');

                        package.Add(new Package
                        {
                            Id = int.Parse(packageeData[0].Trim()),
                            Name= packageeData[1].Trim(),
                            Price= double.Parse(packageeData[2].Trim()),
                            Venue= packageeData[3].Trim(),
                            Detail= packageeData[4].Trim(),
                            PromotionId= int.Parse(packageeData[5].Trim()),
                        });
                    }

                    modelBuilder.Entity<Package>().HasData(package); ;
                }
            }
        }

        private static void InitializeCart(ModelBuilder modelBuilder)
        {
            var cart = new List<Cart>();

            if (File.Exists(CART_FILE_PATH))
            {
                using (StreamReader sr = new StreamReader(CART_FILE_PATH))
                {
                    string? cartLine;

                    while ((cartLine = sr.ReadLine()) != null)
                    {
                        string[]? cartData = cartLine!.Split(',');

                        cart.Add(new Cart
                        {
                            Id = int.Parse(cartData[0].Trim()),
                            Total = double.Parse(cartData[1].Trim()),
                            GuestId = int.Parse(cartData[2].Trim()),
                        });
                    }

                    modelBuilder.Entity<Cart>().HasData(cart);
                }
            }
        }

        private static void InitializeBill(ModelBuilder modelBuilder)
        {
            var bill = new List<Bill>();

            if (File.Exists(BILL_FILE_PATH))
            {
                using (StreamReader sr = new StreamReader(BILL_FILE_PATH))
                {
                    string? billLine;

                    while ((billLine = sr.ReadLine()) != null)
                    {
                        string[]? billData = billLine!.Split(',');

                        bill.Add(new Bill
                        {
                            Id = int.Parse(billData[0].Trim()),
                            Date = DateTime.ParseExact(billData[1].Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            Discount = double.Parse(billData[2].Trim()),
                            Total = double.Parse(billData[3].Trim()),
                            BookingId = int.Parse(billData[4].Trim()),
                        });
                    }

                    modelBuilder.Entity<Bill>().HasData(bill);
                }
            }
        }

        public static void InitializeBooking(ModelBuilder modelBuilder)
        {
            var booking = new List<Booking>();

            if (File.Exists(BOOKING_FILE_PATH))
            {
                using (StreamReader sr = new StreamReader(BOOKING_FILE_PATH))
                {
                    string? bookingLine;

                    while ((bookingLine = sr.ReadLine()) != null)
                    {
                        string[]? bookingData = bookingLine!.Split(',');

                        BookingStatus bookingStatus = BookingStatus.Accepted;
                        if (int.Parse(bookingData[2].Trim()) == 2)
                        {
                            bookingStatus = BookingStatus.Declined;
                        }
                        else if (int.Parse(bookingData[2].Trim()) == 3)
                        {
                            bookingStatus = BookingStatus.Processing;
                        }

                        booking.Add(new Booking
                        {
                            Id = int.Parse(bookingData[0].Trim()),
                            Date = DateTime.ParseExact(bookingData[1].Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            BookingStatus = bookingStatus,
                            Total= double.Parse(bookingData[3].Trim()),
                            GuestId = int.Parse(bookingData[4].Trim()),
                            HostId = int.Parse(bookingData[5].Trim()),
                            PaymentId = int.Parse(bookingData[6].Trim()),
                            BillId = int.Parse(bookingData[7].Trim()),
                        });
                    }

                    modelBuilder.Entity<Booking>().HasData(booking);
                }
            }
        }

        public static void InitializeService(ModelBuilder modelBuilder)
        {
            var service = new List<Service>();

            if (File.Exists(SERVICE_FILE_PATH))
            {
                using (StreamReader sr = new StreamReader(SERVICE_FILE_PATH))
                {
                    string? serviceLine;

                    while ((serviceLine = sr.ReadLine()) != null)
                    {
                        string[]? serviceData = serviceLine!.Split(',');

                        service.Add(new Service
                        {
                            Id = int.Parse(serviceData[0].Trim()),
                            Name = serviceData[1].Trim(),
                            Price = double.Parse(serviceData[2].Trim()),
                            Detail = serviceData[3].Trim(),
                        });
                    }

                    modelBuilder.Entity<Service>().HasData(service);
                }
            }
        }

        public static void InitializePromotion(ModelBuilder modelBuilder)
        {
            var promotion = new List<Promotion>();

            if (File.Exists(PROMOTION_FILE_PATH))
            {
                using (StreamReader sr = new StreamReader(PROMOTION_FILE_PATH))
                {
                    string? promotionLine;

                    while ((promotionLine = sr.ReadLine()) != null)
                    {
                        string[]? promotionData = promotionLine!.Split(',');

                        promotion.Add(new Promotion
                        {
                            Id = int.Parse(promotionData[0].Trim()),
                            Name = promotionData[1].Trim(),
                            FromDate = DateTime.ParseExact(promotionData[2].Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            ToDate = DateTime.ParseExact(promotionData[3].Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            DiscountPercent= double.Parse(promotionData[4].Trim()),
                            Status = (promotionData[5].Trim() == "Accept") ? Status.Accept : Status.Decline,
                            HostId = int.Parse(promotionData[6].Trim()),
                        });
                    }

                    modelBuilder.Entity<Promotion>().HasData(promotion);
                }
            }
        }

        public static void InitializePayment(ModelBuilder modelBuilder)
        {
            var payment = new List<Payment>();

            if (File.Exists(PAYMENT_FILE_PATH))
            {
                using (StreamReader sr = new StreamReader(PAYMENT_FILE_PATH))
                {
                    string? paymentLine;

                    while ((paymentLine = sr.ReadLine()) != null)
                    {
                        string[] paymentData = paymentLine!.Split(",");

                        Types types = Types.Momo;
                        if (int.Parse(paymentData[3].Trim()) == 2)
                        {
                            types = Types.Banking;
                        }
                        else if (int.Parse(paymentData[3].Trim()) == 3)
                        {
                            types = Types.ByCast;
                        }

                        payment.Add(new Payment
                        {
                            Id = int.Parse(paymentData[0].Trim()),
                            Amount = double.Parse(paymentData[1].Trim()),
                            Date= DateTime.ParseExact(paymentData[2].Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            Types = types,
                            BookingId= int.Parse(paymentData[4].Trim()),
                        });
                    }

                    modelBuilder.Entity<Payment>().HasData(payment);
                }
            }
        }

        public static void InitializePackageService(ModelBuilder modelBuilder)
        {
            var packageService = new List<PackageService>();

            if (File.Exists(PACKAGE_SERVICE_FILE_PATH))
            {
                using (StreamReader sr = new StreamReader(PACKAGE_SERVICE_FILE_PATH))
                {
                    int subjectId = 1;
                    string? packageServiceLine;

                    while ((packageServiceLine = sr.ReadLine()) != null)
                    {
                        string[] packageServiceData = packageServiceLine!.Split(",");

                        packageService.Add(new PackageService
                        {
                            PackageId= int.Parse(packageServiceData[0].Trim()),
                            ServiceId = int.Parse(packageServiceData[1].Trim()),
                        });
                    }

                    modelBuilder.Entity<PackageService>().HasData(packageService);
                }
            }
        }

        public static void InitializeCartService(ModelBuilder modelBuilder)
        {
            var cartService = new List<CartService>();

            if (File.Exists(CART_SERVICE_FILE_PATH))
            {
                using (StreamReader sr = new StreamReader(CART_SERVICE_FILE_PATH))
                {
                    string? cartServiceLine;

                    while ((cartServiceLine = sr.ReadLine()) != null)
                    {
                        string[]? cartServiceData = cartServiceLine!.Split(',');

                        cartService.Add(new CartService
                        {
                            CartId= int.Parse(cartServiceData[0].Trim()),
                            ServiceId = int.Parse(cartServiceData[1].Trim()),
                        });
                    }

                    modelBuilder.Entity<CartService>().HasData(cartService);
                }
            }
        }

        public static void InitializeCartPackage(ModelBuilder modelBuilder)
        {
            var cartPackage = new List<CartPackage>();

            if (File.Exists(CART_PACKAGE_FILE_PATH))
            {
                using (StreamReader sr = new StreamReader(CART_PACKAGE_FILE_PATH))
                {
                    string? cartPackageLine;

                    while ((cartPackageLine = sr.ReadLine()) != null)
                    {
                        string[]? cartPackageData = cartPackageLine!.Split(',');

                        cartPackage.Add(new CartPackage
                        {
                            CartId = int.Parse(cartPackageData[0].Trim()),
                            PackageId = int.Parse(cartPackageData[1].Trim()),


                        });
                    }

                    modelBuilder.Entity<CartPackage>().HasData(cartPackage);
                }
            }
        }

        public static void InitializeBookingService(ModelBuilder modelBuilder)
        {
            var bookingService = new List<BookingService>();

            if (File.Exists(BOOKING_SERVICE_FILE_PATH))
            {
                using (StreamReader sr = new StreamReader(BOOKING_SERVICE_FILE_PATH))
                {
                    string? bookingServiceLine;

                    while ((bookingServiceLine = sr.ReadLine()) != null)
                    {
                        string[]? bookingServiceData = bookingServiceLine!.Split(',');

                        bookingService.Add(new BookingService
                        {
                            BookingId= int.Parse(bookingServiceData[0].Trim()),
                            ServiceId = int.Parse(bookingServiceData[1].Trim()),
                        });
                    }

                    modelBuilder.Entity<BookingService>().HasData(bookingService);
                }
            }
        }

        public static void InitializeBookingPackage(ModelBuilder modelBuilder)
        {
            var bookingPackage = new List<BookingPackage>();

            if (File.Exists(BOOKING_PACKAGE_FILE_PATH))
            {
                using (StreamReader sr = new StreamReader(BOOKING_PACKAGE_FILE_PATH))
                {
                    string? bookingPackageLine;

                    while ((bookingPackageLine = sr.ReadLine()) != null)
                    {
                        string[]? bookingPackageData = bookingPackageLine!.Split(',');

                        bookingPackage.Add(new BookingPackage
                        {
                            BookingId = int.Parse(bookingPackageData[0].Trim()),
                            PackageId = int.Parse(bookingPackageData[1].Trim()),
                        });
                    }

                    modelBuilder.Entity<BookingPackage>().HasData(bookingPackage);
                }
            }
        }





    }
}
