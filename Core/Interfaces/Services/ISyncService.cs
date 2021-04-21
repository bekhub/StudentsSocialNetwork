namespace Core.Interfaces.Services
{
    public interface ISyncService<out TEntity, in TModel> where TEntity: BaseEntity
    {
        TEntity Synchronize(TModel model);
    }
}
