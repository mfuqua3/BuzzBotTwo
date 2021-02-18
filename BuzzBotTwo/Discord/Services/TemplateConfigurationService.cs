using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace BuzzBotTwo.Discord.Services
{
    public interface ITemplateConfigurationService
    {
        void UpdateTemplate(object configObject, int key, int value);
        void UpdateTemplate(object configObject, int key, string value);
        IEnumerable<ITemplatePropertyInfo> GetPropertyData(object configObject);
    }

    public interface ITemplatePropertyInfo
    {
        int Key { get; }
        string Name { get; }
        object Value { get; }
    }
    public class TemplateConfigurationService : ITemplateConfigurationService
    {
        public void UpdateTemplate(object configObject, int key, int value)
        {
            var property = GetConfigurationProperty(configObject, key);
            if (property.PropertyType != typeof(int))
            {
                throw new InvalidOperationException($"Invalid use of {nameof(UpdateTemplate)}, {property.Name} is not of type {nameof(Int32)}");
            }
            property.SetValue(configObject, value);
        }
        public void UpdateTemplate(object configObject, int key, string value)
        {
            var property = GetConfigurationProperty(configObject, key);
            if (property.PropertyType == typeof(int))
            {
                var valueAsInt = int.Parse(value);
                UpdateTemplate(configObject, key, valueAsInt);
                return;
            }
            if (property.PropertyType != typeof(string))
            {
                throw new InvalidOperationException($"Invalid use of {nameof(UpdateTemplate)}, {property.Name} is not of type {nameof(String)}");
            }
            property.SetValue(configObject, value);
        }
        public IEnumerable<ITemplatePropertyInfo> GetPropertyData(object configObject)
        {
            var configProperties = configObject.GetType().GetProperties()
                .Where(pi => pi.GetCustomAttribute<ConfigurationKeyAttribute>() != null);
            foreach (var configProperty in configProperties)
            {
                var name = configProperty.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ??
                           configProperty.Name;
                var key = configProperty.GetCustomAttribute<ConfigurationKeyAttribute>().Key;
                var value = configProperty.GetValue(configObject);
                yield return new TemplatePropertyInfo(key, name, value);
            }
        }
        private PropertyInfo GetConfigurationProperty(object propertyObject, int key)
        {
            var properties = propertyObject.GetType().GetProperties()
                .Where(pi => Attribute.IsDefined(pi, typeof(ConfigurationKeyAttribute))).ToList();
            foreach (var propertyInfo in properties)
            {
                var attribute = propertyInfo.GetCustomAttribute<ConfigurationKeyAttribute>();
                if (attribute.Key == key) return propertyInfo;
            }
            throw new ArgumentException($"Unable to find a property in {propertyObject.GetType().Name} with configuration key {key}");
        }

        private class TemplatePropertyInfo : ITemplatePropertyInfo
        {
            public TemplatePropertyInfo(int key, string name, object value)
            {
                Key = key;
                Name = name;
                Value = value;
            }

            public int Key { get; }
            public string Name { get; }
            public object Value { get; }
        }
    }



    [AttributeUsage(AttributeTargets.Property)]
    public class ConfigurationKeyAttribute : Attribute
    {
        public ConfigurationKeyAttribute(int key)
        {
            Key = key;
        }

        public int Key { get; }
    }
}