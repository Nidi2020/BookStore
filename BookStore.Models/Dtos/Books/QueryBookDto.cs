namespace BookStore.Models.Dtos;

public class QueryBookDto : BasePaging
{
    public string? Title { get; set; }
    public int? CategoryId { get; set; }
}

