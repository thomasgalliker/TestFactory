using System;
using System.Collections.Generic;
using System.Text;
using TestFactory.TestSteps;

namespace TestFactory.Tests.Testdata
{
    public class EmptyActionTestStep : ActionTestStep
    {
        public EmptyActionTestStep() : base(() => { })
        {
        }
    }
}
