using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentSystem.Core;
using PaymentSystem.Types;

namespace PaymentSystem.Validation
{
    /// <summary>
    /// This factory only supports Bacs, Chaps and Fasterpayments.
    /// Any other new payment schemes, this class can be inherited to support
    /// new scheme, The payments services then can work with new factory. 
    /// Hence we can avoid modifying this class for new payment schemes.
    /// </summary>
   public class ValidatePaymentFactory : IValidatePaymentFactory
    {
        public virtual ValidatePaymentBase CreateValidationProcess(Enum paymentType)
        {
            if (paymentType is PaymentScheme)
            {
                switch (paymentType)
                {
                    case PaymentScheme.Bacs:
                        return new ValidateBacsPayment();

                    case PaymentScheme.Chaps:
                        return new ValidateChapsPayment();

                    case PaymentScheme.FasterPayments:
                        return new ValidateFasterPayment();

                }

            }

            return null;
        }
    }
}
