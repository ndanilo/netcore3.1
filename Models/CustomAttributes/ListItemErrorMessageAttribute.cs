using System;

namespace Models.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public class ListItemErrorMessageAttribute : Attribute
    {
        public string DefaultErrorMessage { get; set; }
        public string ItemErrorMessage { get; set; }
    }
}
