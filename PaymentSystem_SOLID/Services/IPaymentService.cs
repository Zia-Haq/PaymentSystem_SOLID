using PaymentSystem.Types;

namespace PaymentSystem.Services
{
    public interface IPaymentService
    {
        MakePaymentResult MakePayment(MakePaymentRequest request);
    }
}
