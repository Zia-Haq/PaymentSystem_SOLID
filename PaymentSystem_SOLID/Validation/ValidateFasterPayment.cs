using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentSystem.Core;
using PaymentSystem.Types;

namespace PaymentSystem.Validation
{
   public class ValidateFasterPayment : ValidatePaymentBase
    {
        public override bool ValidatePayment(Account account, MakePaymentRequest paymentRequest)
        {
            if (account == null)
                return false;
            else if (!IsPaymentSchemeAllowed(account.AllowedPaymentSchemes, AllowedPaymentSchemes.FasterPayments))
                return false;
            else if (!(paymentRequest.PaymentScheme is PaymentScheme.FasterPayments))
                return false;
            else if (account.Balance < paymentRequest.Amount)
                return false;

            return true;

        }
    }
}

