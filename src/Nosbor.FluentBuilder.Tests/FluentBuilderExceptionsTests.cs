﻿using Nosbor.FluentBuilder.Exceptions;
using Nosbor.FluentBuilder.Lib;
using Nosbor.FluentBuilder.Tests.SampleClasses;
using NUnit.Framework;

namespace Nosbor.FluentBuilder.Tests
{
    [TestFixture]
    public class FluentBuilderExceptionsTests
    {
        [Test]
        public void Throws_exception_when_property_is_read_only()
        {
            const string dummy = "";

            Assert.Throws<FluentBuilderException>(() => FluentBuilder<ComplexType>
                .New()
                .With(newObject => newObject.ReadOnlyPropertyWithBackingField, dummy)
                .Build());
        }

        [Test]
        public void Throws_exception_when_property_is_not_informed()
        {
            var dummy = (ComplexType)null;

            Assert.Throws<FluentBuilderException>(() => FluentBuilder<ComplexType>
                .New()
                .With(justAnObjectWithNoPropInformed => justAnObjectWithNoPropInformed, dummy)
                .Build());
        }

        [Test]
        public void Throws_exception_when_underlying_field_for_collection_is_not_found()
        {
            const int dummy = 0;

            Assert.Throws<FluentBuilderException>(() => FluentBuilder<ComplexType>
                .New()
                .AddingTo(newObject => newObject.CollectionWithFieldNotFollowingNameConvention, dummy)
                .Build());
        }

        [Test]
        public void Throws_exception_when_trying_to_set_property_of_child_object()
        {
            Assert.Throws<FluentBuilderException>(() => FluentBuilder<ComplexType>
                .New()
                .With(newObject => newObject.AnotherComplexType.Name, "dummy")
                .Build());
        }

        [Test]
        public void Throws_exception_when_member_was_not_found()
        {
            var newValue = new StandAloneComplexType();

            Assert.Throws<FluentBuilderException>(() => FluentBuilder<ComplexType>
                .New()
                .With(newValue)
                .Build());
        }

        [Test]
        public void Throws_exception_when_member_was_found_but_is_of_another_type()
        {
            var newValue = new OneMoreComplexType();

            Assert.Throws<FluentBuilderException>(() => FluentBuilder<ComplexType>
                .New()
                .With(newValue)
                .Build());
        }
    }
}
