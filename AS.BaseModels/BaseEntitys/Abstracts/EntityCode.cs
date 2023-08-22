namespace AS.BaseModels.BaseEntitys.Abstracts
{
    public abstract class EntityCode : BaseEntity, Interfaces.IEntityCode
    {
        public byte Code { get; set; }
    }
}