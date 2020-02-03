using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BAOZ.Common.ValidationHelpers
{
    public static class PhoneNumberValidation
    {

        public static bool IsNumeric(string phone)
        {
            var regex = new Regex(@"(?<=\s|^)\d+(?=\s|$)");
            return regex.IsMatch(phone);

        }
    }

}
