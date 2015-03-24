﻿using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using FluentAssertions.Common;
using FluentAssertions.Equivalency;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref="object"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class ObjectAssertions : ReferenceTypeAssertions<object, ObjectAssertions>
    {
        public ObjectAssertions(object value)
        {
            Subject = value;
        }

        /// <summary>
        /// Asserts that an object equals another object using its <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<ObjectAssertions> Be(object expected, string because = "", params object[] reasonArgs)
        {
            Execute.Assertion
                .BecauseOf(because, reasonArgs)
                .ForCondition(Subject.IsSameOrEqualTo(expected))
                .FailWith("Expected {context:object} to be {0}{reason}, but found {1}.", expected,
                    Subject);

            return new AndConstraint<ObjectAssertions>(this);
        }

        /// <summary>
        /// Asserts that an object does not equal another object using its <see cref="object.Equals(object)" /> method.
        /// </summary>
        /// <param name="unexpected">The unexpected value</param>
        /// <param name="because">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])" /> compatible placeholders.
        /// </param>
        public AndConstraint<ObjectAssertions> NotBe(object unexpected, string because = "", params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.IsSameOrEqualTo(unexpected))
                .BecauseOf(because, reasonArgs)
                .FailWith("Did not expect {context:object} to be equal to {0}{reason}.", unexpected);

            return new AndConstraint<ObjectAssertions>(this);
        }

        /// <summary>
        /// Asserts that an object is an enum and has a specified flag
        /// </summary>
        /// <param name="expectedFlag">The expected flag.</param>
        /// <param name="because">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])" /> compatible placeholders.
        /// </param>
        public AndConstraint<ObjectAssertions> HaveFlag(Enum expectedFlag, string because = "", 
            params object[] reasonArgs)
        {
            Enum subject = Subject as Enum;

            Subject.Should().BeOfType(expectedFlag.GetType(), because, reasonArgs);

            Execute.Assertion
                .ForCondition(subject != null && subject.HasFlag(expectedFlag))
                .BecauseOf(because, reasonArgs)
                .FailWith("The enum was expected to have flag {0} but found {1}{reason}.", expectedFlag, subject);

            return new AndConstraint<ObjectAssertions>(this);
        }

        /// <summary>
        /// Asserts that an object is an enum and does not have a specified flag
        /// </summary>
        /// <param name="unexpectedFlag">The unexpected flag.</param>
        /// <param name="because">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])" /> compatible placeholders.
        /// </param>
        public AndConstraint<ObjectAssertions> NotHaveFlag(Enum unexpectedFlag, string because = "", 
            params object[] reasonArgs)
        {
            Enum subject = Subject as Enum;

            Subject.Should().BeOfType(unexpectedFlag.GetType(), because, reasonArgs);

            Execute.Assertion
                .ForCondition(subject != null && !subject.HasFlag(unexpectedFlag))
                .BecauseOf(because, reasonArgs)
                .FailWith("Did not expect the enum to have flag {0}{reason}.", unexpectedFlag, subject);

            return new AndConstraint<ObjectAssertions>(this);
        }

        /// <summary>
        /// Returns the type of the subject the assertion applies on.
        /// </summary>
        protected override string Context
        {
            get { return "object"; }
        }
    }
}