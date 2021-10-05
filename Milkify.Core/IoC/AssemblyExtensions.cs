using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Milkify.Core.IoC
{
    public static class AssemblyExtensions
    {
        public static Type[] GetDefinedConcreteTypes(this Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException exception)
            {
                return exception.Types;
            }
        }
        /// <summary>
        /// Get all defined classes in provided list of assemblies, except abstract classes and interfaces and ignore classes
        /// </summary>
        /// <param name="assemblies">List of assemblies to get classes</param>
        /// <returns>Array of classes</returns>
        public static Type[] GetDefinedConcreteTypes(this IEnumerable<Assembly> assemblies)
        {
            var assembliesArray = assemblies as Assembly[] ?? assemblies.ToArray();


            return assembliesArray.SelectMany(x => x.GetDefinedConcreteTypes())
                                  .Where(type => type != null && !type.IsInterface && !type.IsAbstract)
                                  .ToArray();
        }

    }
}