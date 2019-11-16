using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentSystem.Types;

namespace PaymentSystem.Core
{

    //This is base abtract class to validate  the payments, current as well as for future payments schemes
    public abstract class ValidatePaymentBase
    {
        public abstract bool ValidatePayment(Account account, MakePaymentRequest paymentRequest);

        public virtual bool IsPaymentSchemeAllowed(Enum allowedPaymentScheme, Enum paymentScheme)
        {
           return allowedPaymentScheme.HasFlag(paymentScheme);
        }

    }
}
