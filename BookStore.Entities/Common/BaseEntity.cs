using System.ComponentModel;

namespace BookStore.Entities;

public interface IEntity
{
}

public abstract class BaseEntity<Tkey> : IEntity  
{
    public Tkey Id { get; set; }
    public DateTime? CreateAt { get; set; }
    public int? CreateBy { get; set; }
    public DateTime? UpdateAt { get; set; }
    public int? UpdateBy { get; set; }

    [DefaultValue(false)]
    public bool IsDeleted { get; set; }
}

public abstract class BaseEntity : BaseEntity<int>
{
}
