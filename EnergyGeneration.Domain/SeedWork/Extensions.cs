using System;
using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="substring">The substring.</param>
        /// <param name="comp">The comp.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified substring]; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">substring - substring cannot be null.</exception>
        /// <exception cref="System.ArgumentException">comp is not a member of StringComparison - comp</exception>
        public static bool Contains(this string str, string substring, StringComparison comp)
        {
            if (substring == null)
                throw new ArgumentNullException("substring", "substring cannot be null.");
            else if (!Enum.IsDefined(typeof(StringComparison), comp))
                throw new ArgumentException("comp is not a member of StringComparison", "comp");

            return str.IndexOf(substring, comp) >= 0;
        }
    }
}
