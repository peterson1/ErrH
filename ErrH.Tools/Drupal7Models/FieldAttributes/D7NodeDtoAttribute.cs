using System;

namespace ErrH.Tools.Drupal7Models.FieldAttributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class D7NodeDtoAttribute : Attribute
    {

        /// <summary>
        /// Concrete type of the DTO object.
        /// Will be used to create an instance of this type using Activator.
        /// Type should have a parameterless constructor.
        /// </summary>
        public Type    DtoType     { get; }


        /// <summary>
        /// The machine name of the content type.
        /// </summary>
        public string  MachineName { get; }


        /// <summary>
        /// Defines how to create an instance of the outgoing DTO for a class.
        /// </summary>
        /// <param name="contentTypeMachineName">The machine name of the content type.</param>
        /// <param name="nodeDtoType">Concrete type of the DTO object. Will be used to create an instance of this type using Activator. Type should have a parameterless constructor.</param>
        public D7NodeDtoAttribute
            (string contentTypeMachineName, Type nodeDtoType)
        {
            DtoType     = nodeDtoType;
            MachineName = contentTypeMachineName;
        }
    }
}
