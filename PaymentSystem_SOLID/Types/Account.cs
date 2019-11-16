using System;

namespace PaymentSystem.Types
{
    public class Account 
    {
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public  AccountStatus Status { get; set; }
        public Enum AllowedPaymentSchemes { get; set; }//changed to Enum to support future schmeses
    }
}
