﻿namespace Bike.Test
{
    using System;
    using Interpreter;
    using Interpreter.Builtin;
    using NUnit.Framework;
    using System.IO;

    public abstract class BaseInterpreterTest : BaseTest
    {
        private InterpretationContext context;

        [SetUp]
        protected void Initialize()
        {
            string home = System.IO.Path.GetFullPath("../../../../");
            context = InterpretationContext.StartInterpretation(home, null, null);
		}

        protected void InterpretFail(string errorType, params string[] sources)
        {
            foreach (var source in sources)
            {
                ExpectFail(errorType, source, () => Interpret(source));
            }
        }

        protected InterpretationContext Interpret(string source)
        {
            ParseAndWalk(source);
            context.Interpreter.Execute(source);
            return context;
        }

        protected void ExpectFail(string errorType, string source, Func<InterpretationContext> func)
        {
            try
            {
                func();
                Assert.Fail(source);
            }
            catch (BikeObject bo)
            {
                if (!ErrorFactory.IsErrorOfType(bo, errorType))
                {
                    Assert.Fail(source);
                }
            }
        }
    }
}
