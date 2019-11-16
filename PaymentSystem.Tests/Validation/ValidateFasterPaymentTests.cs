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
    public class ValidateFasterPaymentTests
    {
        private ValidateFasterPayment _validateChapsPayment;
        private Account _account;
        private MakePaymentRequest _request;

        [TestInitialize]
        public void Initialize()
        {
            _validateChapsPayment = new ValidateFasterPayment();
            _account = new Account();
            _request = new MakePaymentRequest();
        }

        [TestMethod()]
        public void Validation_ShouldPass_When_RequestAndAccount_SupportFasterPayment()
        {
            _account.AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments | AllowedPaymentSchemes.Bacs;
            _request.PaymentScheme = PaymentScheme.FasterPayments;
            Assert.IsTrue(_validateChapsPayment.ValidatePayment(_account, _request));
        }

        [TestMethod()]
        public void Validation_ShouldFail_When_Request_IsNotFasterPayment()
        {
            _account.AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments;
            _request.PaymentScheme = PaymentScheme.Bacs;
            Assert.IsFalse(_validateChapsPayment.ValidatePayment(_account, _request));
        }

        [TestMethod()]
        public void Validation_ShouldFail_When_Account_DestNot_Support_FasterPayment()
        {
            _account.AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs;
            _request.PaymentScheme = PaymentScheme.FasterPayments;
            Assert.IsFalse(_validateChapsPayment.ValidatePayment(_account, _request));
        }

        [TestMethod()]
        public void Validation_ShouldFail_When_Account_Is_Null()
        {
            _request.PaymentScheme = PaymentScheme.FasterPayments;
            Assert.IsFalse(_validateChapsPayment.ValidatePayment(null, _request));
        }

        [TestMethod()]
        public void Validation_ShouldFail_When_Account_Balance_IsLessThan_PaymentAmout()
        {

            _account.AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments | AllowedPaymentSchemes.Bacs;
            _request.PaymentScheme = PaymentScheme.FasterPayments;
            _account.Balance = 100;
            _request.Amount = 150;
            Assert.IsFalse(_validateChapsPayment.ValidatePayment(_account, _request));
        }

        [TestMethod()]
        public void Validation_ShouldPass_When_Account_Balance_IsGreaterThan_PaymentAmount()
        {
            _account.AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments;
            _request.PaymentScheme = PaymentScheme.FasterPayments;
            _account.Balance = 200;
            _request.Amount = 150;
            Assert.IsTrue(_validateChapsPayment.ValidatePayment(_account, _request));
        }


    }
}