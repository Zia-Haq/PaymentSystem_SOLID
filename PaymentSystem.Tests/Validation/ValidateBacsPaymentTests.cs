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
    public class ValidateBacsPaymentTests
    {
        private ValidateBacsPayment _validateBackPayment;
        private Account _account;
        private MakePaymentRequest _request;

        [TestInitialize]
        public void Initialize()
        {
            _validateBackPayment = new ValidateBacsPayment();
            _account = new Account();
            _request = new MakePaymentRequest();
        }

        [TestMethod()]
        public void Validation_ShouldPass_When_RequestAndAccount_SupportBacs()
        {
            _account.AllowedPaymentSchemes =AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.Chaps;
            _request.PaymentScheme =PaymentScheme.Bacs;
            Assert.IsTrue(_validateBackPayment.ValidatePayment(_account, _request));
        }

        [TestMethod()]
        public void Validation_ShouldFail_When_Request_IsNotBacs()
        {
            _account.AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs;
            _request.PaymentScheme = PaymentScheme.FasterPayments;
            Assert.IsFalse(_validateBackPayment.ValidatePayment(_account, _request));
        }

        [TestMethod()]
        public void Validation_ShouldFail_When_Account_DestNot_Support_Bacs()
        {
            _account.AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps;
            _request.PaymentScheme = PaymentScheme.Bacs;
            Assert.IsFalse(_validateBackPayment.ValidatePayment(_account, _request));
        }

        [TestMethod()]
        public void Validation_ShouldFail_When_Account_Is_Null()
        {
             _request.PaymentScheme = PaymentScheme.Bacs;
            Assert.IsFalse(_validateBackPayment.ValidatePayment(null, _request));
        }


       
    }
}