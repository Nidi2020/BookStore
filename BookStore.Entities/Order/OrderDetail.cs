namespace BookStore.Entities;

public class OrderDetail : BaseEntity
{
    public int OrderId { get; set; }
    public Order Order { get; set; }
    public int BookId { get; set; }
    public Book Book { get; set; }
    public int Count { get; set; }
    public int Price { get; set; }
    public int? Score { get; set; }

}

