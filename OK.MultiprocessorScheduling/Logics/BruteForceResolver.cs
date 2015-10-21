using OK.MultiprocessorScheduling.Models;
using System;
using System.Diagnostics;
using System.Linq;

namespace OK.MultiprocessorScheduling.Logics
{
    internal class BruteForceResolver : ISchedulingProblemResolver
    {
        public string AlgorithmName { get { return "Algorytm brute force"; } }

        public TimeSpan ExecutionTime { get; private set; }
        public int Result { get; private set; }

        public int Resolve(SchedulingProblem schedulingProblem)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var tasks = schedulingProblem.Tasks.OrderBy(task => task.Id).ToArray();
            var result = int.MaxValue;

            do
            {
                var processors = new Processor[schedulingProblem.Processors.Count];
                for (int i = 0; i < processors.Length; i++) processors[i] = new Processor() { Id = i };
                var totalDuration = new int[processors.Length];

                for (int i = 0; i < tasks.Length; i++)
                {
                    var index = Algorithm.IndexOfMin(totalDuration);

                    processors[index].CompletedTasks.Add(tasks[i]);
                    totalDuration[index] += tasks[i].Duration;
                }

                var maxDuration = totalDuration.Max();
                if (maxDuration < result)
                {
                    result = maxDuration;
                    schedulingProblem.Processors = processors;
                }
            }
            while (Algorithm.NextPermutation(tasks, (t1, t2) => t1.Id < t2.Id));

            stopwatch.Stop();
            this.ExecutionTime = stopwatch.Elapsed;

            return this.Result = result;
        }
    }
}