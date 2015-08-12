using ErrH.Tools.FileSystemShims;

namespace ErrH.Tools.DataAttributes
{

    /// <summary>
    /// Executes validations based on property attributes.
    /// </summary>
    /// <example>
    /// For implementing IDataErrorInfo:
    /// <code>
    /// class MyClass : IDataErrorInfo
    /// {
    ///     [Int(Min = 12345)]
    ///     public int Number { get; set; }
    ///     
    ///     
    ///     public string Error => DataError.Info(this);
    /// 
    ///     public string this[string col] 
    ///         => DataError.Info(this, col);
    /// }
    /// </code>
    /// </example>
    public class DataError
    {

        /// <summary>
        /// Validate all public properties with ValidationAttribute.
        /// </summary>
        public static string Info<T>(T objWithAttributes)
            => Validator().GetAllErrors(objWithAttributes);

        public static string Info<T>(T objWithAttributes, string columnName)
            => Validator().GetErrorMessage(objWithAttributes, columnName);



        public static string Info<T>(T objWithAttributes, IFileSystemShim fsShim)
            => Validator(fsShim).GetAllErrors(objWithAttributes);

        public static string Info<T>(T objWithAttributes, string columnName, IFileSystemShim fsShim)
            => Validator(fsShim).GetErrorMessage(objWithAttributes, columnName);




        private static AttributeValidator Validator() 
            => new AttributeValidator();

        private static AttributeValidator Validator(IFileSystemShim fsShim)
            => new AttributeValidator { FsShim = fsShim };
    }
}
