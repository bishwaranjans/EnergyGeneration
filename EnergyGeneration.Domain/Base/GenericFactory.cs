namespace EnergyGeneration.Domain.Base
{
    /// <summary>
    /// GenericFactory
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public abstract class GenericFactory<TEntity>
    {
        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public abstract TEntity GetObject(string type);
    }
}
