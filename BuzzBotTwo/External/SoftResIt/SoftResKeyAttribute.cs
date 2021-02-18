using System;

namespace BuzzBotTwo.External.SoftResIt
{
    [AttributeUsage(AttributeTargets.Field)]
    public class SoftResKeyAttribute : Attribute
    {
        public SoftResKeyAttribute(string key)
        {
            Key = key;
        }

        public string Key { get; }
    }
}