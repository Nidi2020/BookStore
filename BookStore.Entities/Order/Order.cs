using System.ComponentModel;

namespace BookStore.Entities;

public class Order : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; }
    public int TotalPrice { get; set; }
    public int? TotalScore { get; set; }

    [DefaultValue(true)]
    public bool IsVerify { get; set; }
    public List<OrderDetail> OrderDetails { get; set; }
}

