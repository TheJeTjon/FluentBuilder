﻿using Nosbor.FluentBuilder.Exceptions;
using System;
using System.Reflection;

namespace Nosbor.FluentBuilder.Commands
{
    internal class SetPropertyCommand : ICommand
    {
        private object _object;
        private object _newValue;
        private PropertyInfo _propertyInfo;

        private string _errorMessage = "Can't set value";

        internal SetPropertyCommand(object @object, string propertyName, object newValue)
        {
            ValidateArguments(@object, propertyName, newValue);
            _propertyInfo = @object.GetType().GetProperty(propertyName);
            _object = @object;
            _newValue = newValue;
            ValidateProperty();
        }

        private void ValidateArguments(object @object, string propertyName, object newValue)
        {
            if (@object == null)
                throw new FluentBuilderException(AppendErrorMessage("Destination object is null"), new ArgumentNullException("@object"));

            if (propertyName == null)
                throw new FluentBuilderException(AppendErrorMessage("Property name is null"), new ArgumentNullException("propertyName"));

            if (newValue == null)
                throw new FluentBuilderException(AppendErrorMessage("Value is null"), new ArgumentNullException("newValue"));
        }

        private void ValidateProperty()
        {
            if (_propertyInfo == null)
                throw new FluentBuilderException(AppendErrorMessage("Property not found"));

            if (!_propertyInfo.CanWrite)
                throw new FluentBuilderException(AppendErrorMessage("Property must have a settter"));

            if (!_propertyInfo.PropertyType.IsAssignableFrom(_newValue.GetType()))
                throw new FluentBuilderException(AppendErrorMessage("Value must be of the same type of the property"));
        }

        public void Execute()
        {
            _propertyInfo.SetValue(_object, _newValue, null);
        }

        private string AppendErrorMessage(string aditionalMessage)
        {
            return string.Format("{0} - {1}", _errorMessage, aditionalMessage);
        }
    }
}
