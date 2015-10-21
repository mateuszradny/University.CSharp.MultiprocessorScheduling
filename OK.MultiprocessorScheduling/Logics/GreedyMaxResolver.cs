using OK.MultiprocessorScheduling.Models;
using System;
using System.Diagnostics;
using System.Linq;

namespace OK.MultiprocessorScheduling.Logics
{
    internal class GreedyMaxResolver : ISchedulingProblemResolver
    {
        public string AlgorithmName { get { return "Algorytm zachłanny (Dopasuj do max)"; } }

        public TimeSpan ExecutionTime { get; private set; }
        public int Result { get; private set; }

        public int Resolve(SchedulingProblem schedulingProblem)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var processors = new Processor[schedulingProblem.Processors.Count];
            for (int i = 0; i < processors.Length; i++) processors[i] = new Processor() { Id = i };

            var tasks = schedulingProblem.Tasks.OrderByDescending(task => task.Duration).ToArray();

            var isFull = new bool[processors.Length];
            var totalDuration = new int[processors.Length];

            for (int i = 0; i < tasks.Length;)
            {
                int maxIndex = Algorithm.IndexOfMax(totalDuration);
                int minIndex = Algorithm.IndexOfMin(totalDuration);

                while (i < tasks.Length && totalDuration[minIndex] + tasks[i].Duration <= totalDuration[maxIndex])
                {
                    processors[minIndex].CompletedTasks.Add(tasks[i]);
                    totalDuration[minIndex] += tasks[i++].Duration;
                    isFull[minIndex] = false;
                }

                if (!isFull[minIndex]) isFull[minIndex] = true;
                else
                {
                    processors[minIndex].CompletedTasks.Add(tasks[i]);
                    totalDuration[minIndex] += tasks[i++].Duration;
                    isFull[minIndex] = false;
                }
            }

            stopwatch.Stop();
            this.ExecutionTime = stopwatch.Elapsed;

            schedulingProblem.Processors = processors;
            return this.Result = processors.Max(processor => processor.CompletedTasks.Sum(task => task.Duration));
        }
    }
}