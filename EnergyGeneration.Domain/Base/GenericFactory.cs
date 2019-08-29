using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyGeneration.Domain.Base
{
    /// <summary>
    /// GenericFactory
    /// </summary>
    /// <typeparam name="TEnity">The type of the enity.</typeparam>
    public abstract class GenericFactory<TEnity>
    {
        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public abstract TEnity GetObject(string type);
    }
}
