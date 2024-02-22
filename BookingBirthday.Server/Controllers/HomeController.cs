if (ServiceId != 0)
            {
                filteredProducts = filteredProducts.Where(x => x.a.ServiceId == ServiceId);
            }
            if (filteredProducts != null)
            {
                var lstProducts = filteredProducts.Select(x => new PackageModel()
                {
                    Id = x.a.Id,
                    Name = x.a.Name,
                    Detail = x.a.Detail,
                    Venue = x.a.Venue,
                    ServiceId = x.a.ServiceId,
                    PromotionId = x.a.PromotionId,
                    Service_Name = x.a.Service!.Name,
                    Price = x.a.Price,
                    image_url = x.a.image_url
                }).ToList();
                return PartialView("ProductList", lstProducts);
            }
            return PartialView("ProductList", null);
        }
        public IActionResult Search(string keyword)
        {
            var filteredProducts = from a in _dbContext.Packages.Include(x => x.Service)
                                   select new { a };

            filteredProducts = filteredProducts.Where(x => x.a.Name!.Contains(keyword));
            if (filteredProducts != null)
            {
                var lstProducts = filteredProducts.Select(x => new PackageModel()
                {
                    Id = x.a.Id,
                    Name = x.a.Name,
                    Detail = x.a.Detail,
                    Venue = x.a.Venue,
                    ServiceId = x.a.ServiceId,
                    PromotionId = x.a.PromotionId,
                    Service_Name = x.a.Service!.Name,
                    Price = x.a.Price,
                    image_url = x.a.image_url
                }).ToList();
                return View(lstProducts);
            }
            return View(filteredProducts);
        }
    }
}