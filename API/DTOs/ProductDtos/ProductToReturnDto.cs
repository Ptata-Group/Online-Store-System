﻿using System;
using System.Collections.Generic;

namespace API.DTOs.ProductDtos
{
    public class ProductToReturnDto
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string CategoryName { get; set; }
        public double Price { get; set; }
        public bool IsSold { get; set; }
        public DateTime DateAdded { get; set; }
        public IReadOnlyList<string> ImagesUrl { get; set; }
    }
}