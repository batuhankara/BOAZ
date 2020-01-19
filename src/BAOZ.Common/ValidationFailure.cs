namespace BAOZ.Common
{
    public class ValidationFailure
    {
        private ValidationFailure()
        {

        }

        /// <summary>
        /// Creates a new validation failure.
        /// </summary>
        public ValidationFailure(string propertyName, string errorMessage, string errorCode) : this(propertyName, errorMessage, errorCode, null)
        {
        }

        /// <summary>
        /// Creates a new ValidationFailure.
        /// </summary>
        public ValidationFailure(string propertyName, string errorMessage, string errorCode, object attemptedValue)
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
            AttemptedValue = attemptedValue;
            ErrorCode = errorCode;
        }

        /// <summary>
        /// The name of the property.
        /// </summary>
        public string PropertyName { get; private set; }

        /// <summary>
        /// The error message
        /// </summary>
        public string ErrorMessage { get; private set; }

        public string ErrorCode { get; private set; }

        /// <summary>
        /// The property value that caused the failure.
        /// </summary>
        public object AttemptedValue { get; private set; }
    }

}
