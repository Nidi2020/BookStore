namespace BookStore.Models.Dtos;

public class OrderListDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Family { get; set; }
    public List<BookListDto> Books { get; set; }
    public int? Score { get; set; }
}

