using OK.MultiprocessorScheduling.Models;
using System;
using System.Diagnostics;
using System.Linq;

namespace OK.MultiprocessorScheduling.Logics
{
    internal class BranchAndBoundResolver : ISchedulingProblemResolver
    {
        public string AlgorithmName { get { return "Algorytm brute force (Branch and Bound)"; } }

        public int Result { get; private set; }
        public TimeSpan ExecutionTime { get; private set; }

        public int Resolve(SchedulingProblem schedulingProblem)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var tasks = schedulingProblem.Tasks.OrderByDescending(task => task.Duration).ThenBy(task => task.Id).ToArray();
            var result = int.MaxValue;

            do
            {
                var processors = new Processor[schedulingProblem.Processors.Count];
                for (int i = 0; i < processors.Length; i++) processors[i] = new Processor() { Id = i };
                var totalDuration = new int[processors.Length];
                var maxDuration = int.MinValue;

                for (int i = 0; i < tasks.Length; i++)
                {
                    // Branch And Bound
                    if (maxDuration > result)
                    {
                        for (int j = i + 1, k = tasks.Length - 1; j < k; j++, k--)
                            Algorithm.Swap(ref tasks[j], ref tasks[k]);

                        break;
                    }

                    var index = Algorithm.IndexOfMin(totalDuration);

                    processors[index].CompletedTasks.Add(tasks[i]);
                    totalDuration[index] += tasks[i].Duration;

                    if (totalDuration[index] > maxDuration)
                        maxDuration = totalDuration[index];
                }

                if (maxDuration < result)
                {
                    result = maxDuration;
                    schedulingProblem.Processors = processors;
                }
            }
            while (Algorithm.NextPermutation(tasks, (t1, t2) => t1.Duration > t2.Duration));

            stopwatch.Stop();
            this.ExecutionTime = stopwatch.Elapsed;

            return this.Result = result;
        }
    }
}