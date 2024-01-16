﻿using System.ComponentModel.DataAnnotations.Schema;

namespace BigBasketApplication.Model
{
    public class OrderItem
    {
        public int Id { get; set; }
        //public int OrderId { get; set; }
        //public Order Order { get; set; }
        public int ProductId { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}