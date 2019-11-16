using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentSystem.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentSystem.Core;
using PaymentSystem.Types;
using Moq;

namespace PaymentSystem.Validation.Tests
{
    [TestClass()]
    public class ValidatePaymentFactoryTests
    {
        private IValidatePaymentFactory _validatePaymentFactory;

        [TestInitialize]
        public void Initialize()
        {
            _validatePaymentFactory = new ValidatePaymentFactory();
          
        }


        [TestMethod()]
        public void Requesting_ChapsValidator_Factory_Should_Return_ChapValdiator()
        {

            var chapsValidator = _validatePaymentFactory.CreateValidationProcess(PaymentScheme.Chaps);
           Assert.IsInstanceOfType(chapsValidator,typeof( ValidateChapsPayment));
        }

        [TestMethod()]
        public void Requesting_BacsValidator_Factory_Should_Return_BacsValdiator()
        {

            var chapsValidator = _validatePaymentFactory.CreateValidationProcess(PaymentScheme.Bacs);
            Assert.IsInstanceOfType(chapsValidator, typeof(ValidateBacsPayment));
        }


        [TestMethod()]
        public void Requesting_FasterPaymentValidator_Factory_Should_Return_FasterPaymentValdiator()
        {
            var chapsValidator = _validatePaymentFactory.CreateValidationProcess(PaymentScheme.FasterPayments);
            Assert.IsInstanceOfType(chapsValidator, typeof(ValidateFasterPayment));
        }

        enum TestPaymentScheme
        {
            UltraFastPaymentScheme=1
        }

        [TestMethod()]
        public void Requesting_NotSupportedPaymentValidator_Factory_Should_Return_Null()
        {
             var ultraFastSchemeValidator = _validatePaymentFactory.CreateValidationProcess(TestPaymentScheme.UltraFastPaymentScheme);
             Assert.AreEqual(ultraFastSchemeValidator, null);
        }

    }
}