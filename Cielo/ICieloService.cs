using System;
using Cielo.API.Requests.Entites;
using Cielo.API.Responses;

namespace Cielo.API
{
    public interface ICieloService
    {
        NewTransactionResponse CreateTransaction(TransactionRequest request);
        EletronicTransferResponse CreateEletronicTransfer(TransactionRequest request);
        TransactionResponse CancelTransaction(Guid? paymentId = null, string merchantOrderId = null, decimal amount = 0);
        TransactionResponse CaptureTransaction(Guid paymentId, decimal amount = 0);
        CheckTransactionResponse CheckTransaction(Guid? paymentId = null, string merchantOrderId = null);
        CardResponse SaveCard(CreditCardRequest request);
    }
}
