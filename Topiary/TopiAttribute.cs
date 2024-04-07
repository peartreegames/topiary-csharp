using System;

namespace PeartreeGames.Topiary
{
    /// <summary>
    /// Represents an attribute that declares a method as an extern topi function.
    /// Can only be used on static methods.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class TopiAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the function in the topi file.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Declare the function as an extern topi function
        /// Can only be used on static methods
        /// </summary>
        /// <param name="name">Name of the function in the topi file</param>
        public TopiAttribute(string name) => Name = name;
    }
}