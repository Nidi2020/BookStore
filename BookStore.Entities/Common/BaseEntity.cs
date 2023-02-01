using System.ComponentModel;

namespace BookStore.Entities;

//vase mark kardan class haei ke az in impelement kardan... vase sakhtan table ha
// bedon tarif DB SET dar DBContext
// ba estefadeh az reflaction
//har classi ke az in interface ersbari karde be ezash to database table sakhte mishe
//hamchenin baraye create table haeii ke nemikham hatman ID dashte bashe mesle settings
public interface IEntity
{
}

///abstract hatman tarif mikonim chon khode base entity az khodesh chizi nadare va ghabele new shodan nist!
public abstract class BaseEntity<Tkey> : IEntity  // the type of key
{
    public Tkey Id { get; set; }
    public DateTime? CreateAt { get; set; }
    public int? CreateBy { get; set; }
    public DateTime? UpdateAt { get; set; }
    public int? UpdateBy { get; set; }

    [DefaultValue(false)]
    public bool IsDeleted { get; set; }
}

//halati ke az base entity gheyre generic estefadeh konim , int ro call mikone dige baghiye class ha kafiye az hamin
//BaseEntity inheritance konan!
// in default int -- mage gheure int bekhaym ke type moshkhas mikonim
public abstract class BaseEntity : BaseEntity<int>
{
}
