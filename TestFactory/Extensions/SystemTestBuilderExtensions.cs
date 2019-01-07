using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestFactory
{
    public static class SystemTestBuilderExtensions
    {
        public static SystemTestBuilder AddTestStep(this SystemTestBuilder systemTestBuilder, Action actionTestStep)
        {
            systemTestBuilder.AddTestStep(new ActionTestStep(actionTestStep));
            return systemTestBuilder;
        }

        public static SystemTestBuilder AddTestStepAsync(this SystemTestBuilder systemTestBuilder, params Func<Task>[] tasks)
        {
            foreach (var task in tasks)
            {
                systemTestBuilder.AddTestStepAsync(new TaskTestStep(task));
            }

            return systemTestBuilder;
        }

        public static SystemTestBuilder AddTestStepAsync(this SystemTestBuilder systemTestBuilder, params TaskTestStep[] taskTestSteps)
        {
            foreach (var taskTestStep in taskTestSteps)
            {
                systemTestBuilder.AddTestStep(taskTestStep);
            }

            return systemTestBuilder;
        }

        /// <summary>
        /// Adds <see cref="parallelTasks"/> to run in parallel against each other. Each Task is wrapped into a new TaskTestStep.
        /// </summary>
        public static SystemTestBuilder AddParallelTestStep(this SystemTestBuilder systemTestBuilder, params Func<Task>[] parallelTasks)
        {
            return systemTestBuilder.AddParallelTestStep(parallelTasks.Select(task => new TaskTestStep(task)));
        }

        public static SystemTestBuilder AddParallelTestStep(this SystemTestBuilder systemTestBuilder, params ITestStep[] parallelTestSteps)
        {
            return systemTestBuilder.AddParallelTestStep(parallelTestSteps.ToList());
        }

        public static SystemTestBuilder AddParallelTestStep(this SystemTestBuilder systemTestBuilder, IEnumerable<ITestStep> parallelTestSteps)
        {
            return systemTestBuilder.AddTestStep(new ParallelTestStep(parallelTestSteps));
        }

        public static SystemTestBuilder Given(this SystemTestBuilder systemTestBuilder, IGivenTestStep step)
        {
            systemTestBuilder.AddTestStep(step);
            return systemTestBuilder;
        }

        public static SystemTestBuilder When(this SystemTestBuilder systemTestBuilder, IWhenTestStep step)
        {
            systemTestBuilder.AddTestStep(step);
            return systemTestBuilder;
        }

        public static SystemTestBuilder Then(this SystemTestBuilder systemTestBuilder, IThenTestStep step)
        {
            systemTestBuilder.AddTestStep(step);
            return systemTestBuilder;
        }

        public static SystemTestBuilder Then(this SystemTestBuilder systemTestBuilder, Action step)
        {
            systemTestBuilder.AddTestStep(step);
            return systemTestBuilder;
        }
    }
}