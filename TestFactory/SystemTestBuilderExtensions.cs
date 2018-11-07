using System;
using TestFactory.TestSteps;

namespace TestFactory
{
    public static class SystemTestBuilderExtensions
    {
        public static SystemTestBuilder AddTestStep(this SystemTestBuilder systemTestBuilder, Action actionTestStep)
        {
            systemTestBuilder.AddTestStep(new ActionTestStep(actionTestStep));
            return systemTestBuilder;
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