using PaymentSystem.Data;
using PaymentSystem.Types;
using System.Configuration;
using PaymentSystem.Core;
using System;

namespace PaymentSystem.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountDataStore _accountDataStore;
        private readonly IValidatePaymentFactory _validatePaymentFactory;
        public PaymentService(IAccountDataStore accountDataStore, IValidatePaymentFactory validatePaymentFactory)
        {
            _accountDataStore = accountDataStore;
            _validatePaymentFactory = validatePaymentFactory;
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            Account account = _accountDataStore.GetAccount(request.DebtorAccountNumber);

            if (account == null)
                return new MakePaymentResult() { Success = false };
           
            var validationProcess = _validatePaymentFactory.CreateValidationProcess(request.PaymentScheme);

            if (validationProcess == null)
                return new MakePaymentResult() { Success = false };
         
            var isValid = validationProcess.ValidatePayment(account,request);

            var result = new MakePaymentResult();

            if (isValid)
            {
                account.Balance -= request.Amount;

                //if data store update fails then we should return false 
                //and reset the balance to the amount that was before deduction,in other words we need a transaction
                //but skipping for brevity
                _accountDataStore.UpdateAccount(account);
                result.Success = true;
            }

            return result;
        }
    }
}
