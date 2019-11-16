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
    public class PaymentServiceIntegrationTests
    {
        private IValidatePaymentFactory _validatePaymentFactory;
        private Account _account;
        private MakePaymentRequest _request;

        [TestInitialize]
        public void Initialize()
        {
            _validatePaymentFactory = new ValidatePaymentFactory();
            _account = new Account();
            _request = new MakePaymentRequest();
        }

        [TestMethod()]
        public void Balance_Should_Be_Deducted_After_Successfull_Payment()
        {

            var testAccount = GetTestAccount();
            testAccount.AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs;

            var dataStore = new Mock<IAccountDataStore>();

            dataStore.Setup<Account>(d => d.GetAccount(It.IsAny<string>())).Returns
                ((string accountNo) =>
                {
                    return testAccount;
                });

            dataStore.Setup(d => d.UpdateAccount(It.IsAny<Account>())).Callback((Account ac) =>
            { testAccount = ac; });

            var paymentService = new PaymentService(dataStore.Object, _validatePaymentFactory);

            MakePaymentRequest request = new MakePaymentRequest() { Amount = 500, PaymentScheme = PaymentScheme.Bacs };

            var result = paymentService.MakePayment(request);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(testAccount.Balance, 1000);//balance should be 1000 after deduction of 500


        }

        [TestMethod()]
        public void FasterPayment_Should_Fail_When_Balance_IsLess_Than_PaymentAmount()
        {

            var testAccount = GetTestAccount();
            testAccount.AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments;

            var dataStore = new Mock<IAccountDataStore>();

            dataStore.Setup<Account>(d => d.GetAccount(It.IsAny<string>())).Returns
                ((string accountNo) =>
                {
                    return testAccount;
                });

            dataStore.Setup(d => d.UpdateAccount(It.IsAny<Account>())).Callback((Account ac) =>
            { testAccount = ac; });

            var paymentService = new PaymentService(dataStore.Object, _validatePaymentFactory);

            MakePaymentRequest request = new MakePaymentRequest() { Amount = 2500, PaymentScheme = PaymentScheme.FasterPayments };

            var result = paymentService.MakePayment(request);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(testAccount.Balance, 1500);//balance should stay same as payment should fail


        }

        [TestMethod()]
        public void ChapsPayment_Should_Fail_When_Account_IsNot_Live()
        {

            var testAccount = GetTestAccount();
            testAccount.AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps;
            testAccount.Status = AccountStatus.Disabled;

            var dataStore = new Mock<IAccountDataStore>();

            dataStore.Setup<Account>(d => d.GetAccount(It.IsAny<string>())).Returns
                ((string accountNo) =>
                {
                    return testAccount;
                });

            dataStore.Setup(d => d.UpdateAccount(It.IsAny<Account>())).Callback((Account ac) =>
            { testAccount = ac; });

            var paymentService = new PaymentService(dataStore.Object, _validatePaymentFactory);

            MakePaymentRequest request = new MakePaymentRequest() { Amount = 500, PaymentScheme = PaymentScheme.Chaps };

            var result = paymentService.MakePayment(request);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(testAccount.Balance, 1500);//balance should stay same as payment should fail


        }


        [TestMethod()]
        public void Balance_Should_Not_Be_Deducted_After_Failed_Payment()
        {
            Account testAccount = GetTestAccount();
            testAccount.AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs;

            var dataStore = new Mock<IAccountDataStore>();
            dataStore.Setup<Account>(d => d.GetAccount(It.IsAny<string>())).Returns
                (testAccount);

            dataStore.Setup(d => d.UpdateAccount(It.IsAny<Account>())).Callback((Account ac) =>
            { testAccount = ac; });

          
            var paymentService = new PaymentService(dataStore.Object, _validatePaymentFactory);

            MakePaymentRequest request = new MakePaymentRequest() { Amount = 500, PaymentScheme = PaymentScheme.FasterPayments };

            var result = paymentService.MakePayment(request);
            //payment should fail as account only support Bacs whereas payment request is for fasterpayments
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