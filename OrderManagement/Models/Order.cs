using System;
using System.Collections.Generic;

namespace OrderManagement.Models
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }

        public int TableNumber { get; set; }

        public virtual IList<Detail> Details { get; set; }
    }
}