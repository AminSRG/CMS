namespace AS.BaseModels.BaseEntitys.Abstracts
{
    public abstract class EntityID : BaseEntity, Interfaces.IEntityID
    {
        public string ID { get; set; } = Guid.NewGuid().ToString();
    }
}
