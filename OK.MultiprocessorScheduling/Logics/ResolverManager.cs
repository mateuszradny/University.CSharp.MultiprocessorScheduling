using System;
using System.Collections.Generic;
using System.Linq;

namespace OK.MultiprocessorScheduling.Logics
{
    internal class ResolverManager
    {
        static ResolverManager()
        {
            Resolvers = new List<ISchedulingProblemResolver>();

            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type != typeof(ISchedulingProblemResolver) && typeof(ISchedulingProblemResolver).IsAssignableFrom(type))
                .ToArray();

            foreach (var type in types)
                Resolvers.Add(Activator.CreateInstance(type) as ISchedulingProblemResolver);
        }

        public static IList<ISchedulingProblemResolver> Resolvers { get; private set; }
    }
}