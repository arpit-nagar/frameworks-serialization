﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavisca.Frameworks.Serialization.Configuration
{
    internal static class Helpers
    {
        public static bool MatchTypeName(this string typeName, Type type)
        {
            return FormatTypeName(type.FullName).Equals(FormatTypeName(typeName),
                                                                     StringComparison.InvariantCultureIgnoreCase);
        }

        private static string FormatTypeName(string typeName)
        {
            return typeName.Trim().Replace(" ", "");
        }

        public static IEnumerable<IAssemblyElement> GetAssembliesFromType(
            this IApplicationSerializationConfiguration configuration, Type type)
        {
            if (configuration.TypeElements == null)
                yield break;

            ICollection<IAssemblyElement> targetAssemblies = null;

            var typeConfig = configuration.GetTypeConfigurationByTypeFromConfig(type);

            if (typeConfig != null)
                targetAssemblies = typeConfig.LookupAssemblies;

            if (targetAssemblies == null)
                targetAssemblies = new List<IAssemblyElement>();

            foreach (var obj in targetAssemblies)
            {
                var targetAssembly = obj;

                if (targetAssembly == null)
                    continue;

                yield return targetAssembly;
            }
        }

        public static ITypeElement GetTypeConfigurationByTypeFromConfig(
            this IApplicationSerializationConfiguration configuration, Type type)
        {
            return configuration.TypeElements == null ? null :
                configuration.TypeElements.FirstOrDefault(typeElement => typeElement.Match(type));
        }
    }
}
