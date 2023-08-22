namespace AS.BaseModels.BaseEntitys.Abstracts
{
    public abstract class BaseEntity : Interfaces.IBaseEntitys
    {
        public DateTime InsertDateTime { get; set; } = DateTime.Now;
        public DateTime UpdateDateTime { get; set; } = DateTime.Now;
    }
}
