using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentSystem.Types;
using Moq;
using PaymentSystem.Validation;
using PaymentSystem.Data;
using PaymentSystem.Core;

namespace PaymentSystem.Services.Tests
{
    [TestClass()]
    public class PaymentServiceTests
    {
      
              
        [TestMethod()]
        public void Balance_Should_Be_Deducted_After_Successfull_Payment()
        {

            var testAccount = GetTestAccount();
            var dataStore = new Mock<IAccountDataStore>();

            dataStore.Setup<Account>(d => d.GetAccount(It.IsAny<string>())).Returns
                ((string accountNo) =>
                {
                    return testAccount;
                });

            dataStore.Setup(d => d.UpdateAccount(It.IsAny<Account>())).Callback((Account ac) =>
            { testAccount = ac; });

           var paymentValidationFactory = GetValidatePaymentFactory(true);

            var paymentService = new PaymentService(dataStore.Object, paymentValidationFactory.Object);

            MakePaymentRequest request = new MakePaymentRequest() { Amount = 500, PaymentScheme = PaymentScheme.Bacs };

            var result = paymentService.MakePayment(request);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(testAccount.Balance, 1000);//balance should be 1000 after deduction of 500


        }

        private Mock<ValidatePaymentFactory> GetValidatePaymentFactory(bool returnValidation)
        {
            var validatePayment = new Mock<ValidatePaymentBase>();
            validatePayment.Setup<bool>(v => v.ValidatePayment(It.IsAny<Account>(), It.IsAny<MakePaymentRequest>())).Returns(returnValidation);

            var paymentValidationFactory = new Mock<ValidatePaymentFactory>();//we will be using a different factory if we are adding new payment schemes

            paymentValidationFactory.Setup<Core.ValidatePaymentBase>(v => v.CreateValidationProcess(It.IsAny<PaymentScheme>())).
              Returns(validatePayment.Object);
            return paymentValidationFactory;
        }

        [TestMethod()]
        public void Balance_Should_Not_Be_Deducted_After_Failed_Payment()
        {
            Account testAccount = GetTestAccount();

            var dataStore = new Mock<IAccountDataStore>();
            dataStore.Setup<Account>(d => d.GetAccount(It.IsAny<string>())).Returns
                (testAccount);

            dataStore.Setup(d => d.UpdateAccount(It.IsAny<Account>())).Callback((Account ac) =>
            { testAccount = ac; });

            var paymentValidationFactory = GetValidatePaymentFactory(false);

            var paymentService = new PaymentService(dataStore.Object, paymentValidationFactory.Object);

            MakePaymentRequest request = new MakePaymentRequest() { Amount = 500, PaymentScheme = PaymentScheme.Bacs };

            var result = paymentService.MakePayment(request);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(testAccount.Balance, 1500);//balance should stay at 1500 after failed payment no deduction

        }

        private static Account GetTestAccount()
        {
            return new Account()
            {
                AccountNumber = "",
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs,
                Balance = 1500,
                Status = AccountStatus.Live
            };
        }
    }
}