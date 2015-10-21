using System.Collections.Generic;

namespace OK.MultiprocessorScheduling.Models
{
    internal class Processor
    {
        public Processor()
        {
            this.CompletedTasks = new List<Task>();
        }

        public int Id { get; set; }
        public IList<Task> CompletedTasks { get; set; }
    }
}