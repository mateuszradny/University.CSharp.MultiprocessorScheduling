using System;
using System.Collections.Generic;

namespace OK.MultiprocessorScheduling.Models
{
    internal class SchedulingProblem
    {
        public SchedulingProblem(int processorCount, params int[] tasks)
        {
            this.Processors = new List<Processor>();
            for (int i = 0; i < processorCount; i++)
                this.Processors.Add(new Processor() { Id = i });

            this.Tasks = new List<Task>();
            for (int i = 0; i < tasks.Length; i++)
                this.Tasks.Add(new Task() { Id = i, Duration = tasks[i] });
        }

        public IList<Processor> Processors { get; set; }
        public IList<Task> Tasks { get; set; }

        public static SchedulingProblem Generate(int processorCount, int taskCount, int minDuration, int maxDuration)
        {
            return Generate(processorCount, processorCount, taskCount, taskCount, minDuration, maxDuration);
        }

        public static SchedulingProblem Generate(int minProcessorCount, int maxProcessorCount, int minTaskCount, int maxTaskCount, int minDuration, int maxDuration)
        {
            var random = new Random();

            int processorCount = random.Next(minProcessorCount, maxProcessorCount + 1);
            int taskCount = random.Next(minTaskCount, maxTaskCount + 1);

            int[] tasks = new int[taskCount];
            for (int i = 0; i < taskCount; i++)
                tasks[i] = random.Next(minDuration, maxDuration + 1);

            return new SchedulingProblem(processorCount, tasks);
        }
    }
}