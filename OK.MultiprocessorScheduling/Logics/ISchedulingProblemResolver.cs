using OK.MultiprocessorScheduling.Models;
using System;

namespace OK.MultiprocessorScheduling.Logics
{
    internal interface ISchedulingProblemResolver
    {
        string AlgorithmName { get; }

        int Result { get; }
        TimeSpan ExecutionTime { get; }

        int Resolve(SchedulingProblem schedulingProblem);
    }
}