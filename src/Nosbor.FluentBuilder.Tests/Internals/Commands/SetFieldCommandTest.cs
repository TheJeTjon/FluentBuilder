﻿using Nosbor.FluentBuilder.Internals.Commands;
using Nosbor.FluentBuilder.Exceptions;
using NUnit.Framework;

namespace Nosbor.FluentBuilder.Tests.Internals.Commands
{
    [TestFixture]
    public class SetFieldCommandTest
    {
        private SampleTypeWithFields _object;

        [SetUp]
        public void SetUp()
        {
            _object = new SampleTypeWithFields();
        }

        [Test]
        public void Should_set_a_public_field()
        {
            var fieldName = "field";
            var newValue = 1;
            var command = new SetFieldCommand(_object, fieldName, newValue);

            command.Execute();

            Assert.AreEqual(newValue, _object.field);
        }

        [Test]
        public void Should_set_a_private_field()
        {
            var fieldName = "privateField";
            var newValue = 1;
            var command = new SetFieldCommand(_object, fieldName, newValue);

            command.Execute();

            Assert.AreEqual(newValue, _object.PropertyOnlyForTestingPurpose);
        }

        [Test]
        public void Should_set_a_field_when_value_inherits_from_field_type()
        {
            var fieldName = "abstractField";
            var newValue = new ConcreteSampleType();
            var command = new SetFieldCommand(_object, fieldName, newValue);

            command.Execute();

            Assert.AreEqual(newValue, _object.abstractField);
        }

        [TestCase("field", null, Description = "When value is null")]
        [TestCase("field", "invalidType", Description = "When field type is different from value type")]
        [TestCase("NonExistentField", "dummyValue", Description = "When field was not found")]
        [TestCase(null, 10, Description = "When field name is null")]
        public void Should_not_create_invalid_command_when(string fieldName, object newValue)
        {
            TestDelegate testAction = () => new SetFieldCommand(_object, fieldName, newValue);

            Assert.Throws<FluentBuilderException>(testAction);
        }

        [Test]
        public void Should_not_create_invalid_command_when_destination_object_is_null()
        {
            SampleTypeWithFields @object = null;

            TestDelegate testAction = () => new SetFieldCommand(@object, "dummyField", "dummyValue");

            Assert.Throws<FluentBuilderException>(testAction);
        }
    }

    internal class SampleTypeWithFields
    {
        public int field;
        private int privateField;

        public AbstractSampleType abstractField;

        public int PropertyOnlyForTestingPurpose { get { return privateField; } }
    }
}
