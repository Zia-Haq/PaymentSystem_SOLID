using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentSystem.Core;
using PaymentSystem.Types;

namespace PaymentSystem.Validation
{
 public   class ValidateChapsPayment : ValidatePaymentBase
    {
        public override bool ValidatePayment(Account account, MakePaymentRequest paymentRequest)
        {

            if (account == null)
                return false;
            else if (!IsPaymentSchemeAllowed(account.AllowedPaymentSchemes, AllowedPaymentSchemes.Chaps))
                return false;
            else if (!(paymentRequest.PaymentScheme is PaymentScheme.Chaps))
                return false;
            else if (account.Status != AccountStatus.Live)
                return false;

            return true;

        }
    }
}
