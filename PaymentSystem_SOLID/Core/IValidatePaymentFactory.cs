using System;

namespace PaymentSystem.Core
{
  public  interface IValidatePaymentFactory
    {
        ValidatePaymentBase CreateValidationProcess(Enum paymentType);
    }
}