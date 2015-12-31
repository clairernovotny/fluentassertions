﻿using System;

#if !OLD_MSTEST && !NUNIT
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#elif NUNIT
using TestClassAttribute = NUnit.Framework.TestFixtureAttribute;
using TestMethodAttribute = NUnit.Framework.TestCaseAttribute;
using AssertFailedException = NUnit.Framework.AssertionException;
using TestInitializeAttribute = NUnit.Framework.SetUpAttribute;
using Assert = NUnit.Framework.Assert;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace FluentAssertions.Specs
{
    [TestClass]
    public class GuidAssertionSpecs
    {
        #region BeEmpty / NotBeEmpty

        [TestMethod]
        public void Should_succeed_when_asserting_empty_guid_is_empty()
        {
            Guid.Empty.Should().BeEmpty();
        }

        [TestMethod]
        public void Should_fail_when_asserting_non_empty_guid_is_empty()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var guid = new Guid("12345678-1234-1234-1234-123456789012");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => guid.Should().BeEmpty("because we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected empty Guid because we want to test the failure message, but found {12345678-1234-1234-1234-123456789012}.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_non_empty_guid_is_not_empty()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var guid = new Guid("12345678-1234-1234-1234-123456789012");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => guid.Should().NotBeEmpty();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void Should_fail_when_asserting_empty_guid_is_not_empty()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => Guid.Empty.Should().NotBeEmpty("because we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Did not expect empty Guid because we want to test the failure message.");
        }

        #endregion

        #region Be / NotBe

        [TestMethod]
        public void Should_succeed_when_asserting_guid_equals_the_same_guid()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var guid = new Guid("11111111-aaaa-bbbb-cccc-999999999999");
            var sameGuid = new Guid("11111111-aaaa-bbbb-cccc-999999999999");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => guid.Should().Be(sameGuid);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void Should_succeed_when_asserting_guid_equals_the_same_guid_in_string_format()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var guid = new Guid("11111111-aaaa-bbbb-cccc-999999999999");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => guid.Should().Be("11111111-aaaa-bbbb-cccc-999999999999");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void Should_fail_when_asserting_guid_equals_a_different_guid()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var guid = new Guid("11111111-aaaa-bbbb-cccc-999999999999");
            var differentGuid = new Guid("55555555-ffff-eeee-dddd-444444444444");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => guid.Should().Be(differentGuid, "because we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected Guid to be {55555555-ffff-eeee-dddd-444444444444} because we want to test the failure message, but found {11111111-aaaa-bbbb-cccc-999999999999}.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_guid_does_not_equal_a_different_guid()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var guid = new Guid("11111111-aaaa-bbbb-cccc-999999999999");
            var differentGuid = new Guid("55555555-ffff-eeee-dddd-444444444444");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () =>
                guid.Should().NotBe(differentGuid);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void Should_succeed_when_asserting_guid_does_not_equal_the_same_guid()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var guid = new Guid("11111111-aaaa-bbbb-cccc-999999999999");
            var sameGuid = new Guid("11111111-aaaa-bbbb-cccc-999999999999");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => guid.Should().NotBe(sameGuid, "because we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Did not expect Guid to be {11111111-aaaa-bbbb-cccc-999999999999} because we want to test the failure message.");
        }

        #endregion

        [TestMethod]
        public void Should_support_chaining_constraints_with_and()
        {
            Guid guid = Guid.NewGuid();
            guid.Should()
                .NotBeEmpty()
                .And.Be(guid);
        }
    }
}