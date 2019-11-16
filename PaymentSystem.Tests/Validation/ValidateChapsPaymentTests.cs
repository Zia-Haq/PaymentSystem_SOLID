using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentSystem.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentSystem.Types;


namespace PaymentSystem.Validation.Tests
{
    [TestClass()]
    public class ValidateChapsPaymentTests
    {
        private ValidateChapsPayment _validateChapsPayment;
        private Account _account;
        private MakePaymentRequest _request;

        [TestInitialize]
        public void Initialize()
        {
            _validateChapsPayment = new ValidateChapsPayment();
            _account = new Account();
            _request = new MakePaymentRequest();
        }

        [TestMethod()]
        public void Validation_ShouldPass_When_RequestAndAccount_SupportChaps()
        {
            _account.AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps | AllowedPaymentSchemes.Bacs;
            _request.PaymentScheme = PaymentScheme.Chaps;
            Assert.IsTrue(_validateChapsPayment.ValidatePayment(_account, _request));
        }

        [TestMethod()]
        public void Validation_ShouldFail_When_Request_IsNotChaps()
        {
            _account.AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps;
            _request.PaymentScheme = PaymentScheme.FasterPayments;
            Assert.IsFalse(_validateChapsPayment.ValidatePayment(_account, _request));
        }

        [TestMethod()]
        public void Validation_ShouldFail_When_Account_DestNot_Support_Chaps()
        {
            _account.AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs;
            _request.PaymentScheme = PaymentScheme.Chaps;
            Assert.IsFalse(_validateChapsPayment.ValidatePayment(_account, _request));
        }

        [TestMethod()]
        public void Validation_ShouldFail_When_Account_Is_Null()
        {
            _request.PaymentScheme = PaymentScheme.Chaps;
            Assert.IsFalse(_validateChapsPayment.ValidatePayment(null, _request));
        }

        [TestMethod()]
        public void Validation_ShouldFail_When_Account_IsNot_Live()
        {

            _account.AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps;
            _request.PaymentScheme = PaymentScheme.Chaps;
            _account.Status = AccountStatus.Disabled;
            Assert.IsFalse(_validateChapsPayment.ValidatePayment(_account, _request));
        }

        [TestMethod()]
        public void Validation_ShouldPass_When_Account_IsLive()
        {
            _account.AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps;
            _request.PaymentScheme = PaymentScheme.Chaps;
            _account.Status = AccountStatus.Live;

            Assert.IsTrue(_validateChapsPayment.ValidatePayment(_account, _request));
        }

    }
}