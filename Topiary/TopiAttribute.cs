using System;

namespace Topiary
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TopiAttribute : Attribute
    {
        public string Name { get; private set; }

        /// <summary>
        /// Declare the function as an extern topi function
        /// Can only be used on static methods
        /// </summary>
        /// <param name="name">Name of the function in the topi file</param>
        public TopiAttribute(string name) => Name = name;
    }
}