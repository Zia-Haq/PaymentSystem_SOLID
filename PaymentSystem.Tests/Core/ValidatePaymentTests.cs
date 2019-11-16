using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using PaymentSystem.Types;

namespace PaymentSystem.Core.Tests
{
    [TestClass()]
    public class ValidatePaymentTests
    {
      

        private ValidatePaymentBase _validatePayment;

        [TestInitialize()]
        public void Intialiaze()
        {
            var mock = new Mock<ValidatePaymentBase>();
             mock.CallBase = true;
            _validatePayment = mock.Object;
           
        }

        [TestMethod()]
        public void Validation_Should_Pass_When_AllowedSchemes_Have_Chaps_Flag ()
        {
            Assert.IsTrue( _validatePayment.IsPaymentSchemeAllowed(AllowedPaymentSchemes.Chaps | AllowedPaymentSchemes.FasterPayments, AllowedPaymentSchemes.Chaps));
        }

        [TestMethod()]
        public void Validation_Should_Fail_When_AllowedSchemes_DoesNot_Have_Chaps_Flag()
        {
            Assert.IsFalse(_validatePayment.IsPaymentSchemeAllowed(AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.FasterPayments, AllowedPaymentSchemes.Chaps));
        }
    }
}