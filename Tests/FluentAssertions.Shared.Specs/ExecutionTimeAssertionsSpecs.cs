﻿using System;
using System.Threading;

#if !OLD_MSTEST && !NUNIT
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#elif NUNIT
using TestClassAttribute = NUnit.Framework.TestFixtureAttribute;
using TestMethodAttribute = NUnit.Framework.TestCaseAttribute;
using AssertFailedException = NUnit.Framework.AssertionException;
using TestInitializeAttribute = NUnit.Framework.SetUpAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

#if !PORTABLE && !SILVERLIGHT

namespace FluentAssertions.Specs
{
    [TestClass]
    public class ExecutionTimeAssertionsSpecs
    {
        [TestMethod]
        public void When_the_execution_time_of_a_member_exceeds_the_maximum_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new SleepingClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ExecutionTimeOf(s => s.Sleep(610)).ShouldNotExceed(500.Milliseconds(), "we like speed");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act
                .ShouldThrow<AssertFailedException>()
                .And.Message.Should().StartWith(
                    "Execution of (s.Sleep(610)) should not exceed 0.500s because we like speed, but it required 0.6");
        }

        [TestMethod]
        public void When_the_execution_time_of_a_meber_stays_within_the_maximum_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new SleepingClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ExecutionTimeOf(s => s.Sleep(0)).ShouldNotExceed(500.Milliseconds());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_the_execution_time_of_an_action_exceeds_the_maximum_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
#if !WINRT && !WINDOWS_PHONE_APP && !CORE_CLR
            Action someAction = () => Thread.Sleep(510);
#else
            Action someAction = () => System.Threading.Tasks.Task.Delay(510).Wait();
#endif
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => someAction.ExecutionTime().ShouldNotExceed(100.Milliseconds());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act
                .ShouldThrow<AssertFailedException>()
                .And.Message.Should().StartWith(
                    "Execution of the action should not exceed 0.100s, but it required 0.5");
        }

        [TestMethod]
        public void When_the_execution_time_of_an_action_stays_within_the_limits_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
#if !WINRT && !WINDOWS_PHONE_APP && !CORE_CLR
            Action someAction = () => Thread.Sleep(100);
#else
            Action someAction = async () => await System.Threading.Tasks.Task.Delay(100);
#endif

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => someAction.ExecutionTime().ShouldNotExceed(1.Seconds());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        internal class SleepingClass
        {
            public void Sleep(int milliseconds)
            {
#if !WINRT && !WINDOWS_PHONE_APP && !CORE_CLR
                Thread.Sleep(milliseconds);
#else
                System.Threading.Tasks.Task.Delay(milliseconds).Wait();
#endif
            }
        }
    }
}
#endif