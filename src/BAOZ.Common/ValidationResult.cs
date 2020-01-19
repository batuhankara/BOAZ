using System.Collections.Generic;
using System.Linq;

namespace BAOZ.Common
{
    public class ValidationResult
    {
        private readonly IList<ValidationFailure> _errors;

        /// <summary>
        /// Whether validation succeeded
        /// </summary>
        public virtual bool IsValid => Errors.Count == 0;

        /// <summary>
        /// A collection of errors
        /// </summary>
        public IList<ValidationFailure> Errors => _errors;

        /// <summary>
        /// Creates a new validationResult
        /// </summary>
        public ValidationResult()
        {
            _errors = new List<ValidationFailure>();
        }

        /// <summary>
        /// Creates a new ValidationResult from a collection of failures
        /// </summary>
        /// <param name="failures">List of <see cref="ValidationFailure"/> which is later available through <see cref="Errors"/>. This list gets copied.</param>
        /// <remarks>
        /// Every caller is responsible for not adding <c>null</c> to the list.
        /// </remarks>
        public ValidationResult(IEnumerable<ValidationFailure> failures)
        {
            _errors = failures.Where(failure => failure != null).ToList();
        }
    }
}
