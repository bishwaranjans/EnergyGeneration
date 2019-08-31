using EnergyGeneration.Domain.Base;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace EnergyGeneration.Infrastructure.Factories
{
    /// <summary>
    /// ParserFactory
    /// </summary>
    /// <seealso cref="EnergyGeneration.Domain.Base.GenericFactory{EnergyGeneration.Domain.Base.BaseParser}" />
    public class ParserFactory : GenericFactory<BaseParser>
    {
        Dictionary<string, BaseParser> parsers = new Dictionary<string, BaseParser>();

        /// <summary>
        /// Load the assembly dynamically which are inheriting from baseparser class.
        /// We can add any number of classes which inherit from baseparser
        /// </summary>
        public ParserFactory()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();
            foreach (Type type in types)
            {
                if (type.IsSubclassOf(typeof(BaseParser)) && (!type.IsAbstract))
                {
                    BaseParser parser = Activator.CreateInstance(type) as BaseParser;
                    parsers.Add(parser.ToString(), parser);
                }
            }
        }

        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public override BaseParser GetObject(string type)
        {
            return parsers[type];
        }
    }
}
