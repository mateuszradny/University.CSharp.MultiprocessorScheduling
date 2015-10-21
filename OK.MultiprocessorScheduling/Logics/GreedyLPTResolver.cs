using OK.MultiprocessorScheduling.Models;
using System;
using System.Diagnostics;
using System.Linq;

namespace OK.MultiprocessorScheduling.Logics
{
    internal class GreedyLPTResolver : ISchedulingProblemResolver
    {
        public string AlgorithmName { get { return "Algorytm zachłanny (Longest Processing Time)"; } }

        public int Result { get; private set; }
        public TimeSpan ExecutionTime { get; private set; }

        public int Resolve(SchedulingProblem schedulingProblem)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var processors = new Processor[schedulingProblem.Processors.Count];
            for (int i = 0; i < processors.Length; i++) processors[i] = new Processor() { Id = i };

            var tasks = schedulingProblem.Tasks.OrderByDescending(task => task.Duration).ToArray();
            var totalDuration = new int[processors.Length];

            for (int i = 0; i < tasks.Length; i++)
            {
                int index = Algorithm.IndexOfMin(totalDuration);

                processors[index].CompletedTasks.Add(tasks[i]);
                totalDuration[index] += tasks[i].Duration;
            }

            stopwatch.Stop();
            this.ExecutionTime = stopwatch.Elapsed;

            schedulingProblem.Processors = processors;
            return this.Result = processors.Max(processor => processor.CompletedTasks.Sum(t => t.Duration));
        }
    }
}