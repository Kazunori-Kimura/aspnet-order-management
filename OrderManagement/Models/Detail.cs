namespace OrderManagement.Models
{
    public class Detail
    {
        public int Id { get; set; }

        public string MenuName { get; set; }

        public decimal Price { get; set; }

        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}