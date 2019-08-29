using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EnergyGeneration.Domain.SeedWork
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Elementses any ns.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="localName">Name of the local.</param>
        /// <returns></returns>
        public static IEnumerable<XElement> ElementsAnyNS(this XElement source, string localName)
        {
            return source.Elements().Where(e => e.Name.LocalName.Equals(localName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Elements any ns.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="localName">Name of the local.</param>
        /// <returns></returns>
        public static XElement ElementAnyNS(this XElement source, string localName)
        {
            return source.Elements().Where(e => e.Name.LocalName.Equals(localName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        }
    }
}
