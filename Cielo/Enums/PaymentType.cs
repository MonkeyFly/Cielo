using System.ComponentModel;

namespace Cielo.API.Enums
{
    public enum PaymentType
    {
        [Description("Cartão de Crédito")]
        CreditCard,
        [Description("Cartão de Débito")]
        DebitCard,
        [Description("Boleto")]
        Boleto,
        [Description("Transferência Eletrônica")]
        EletronicTransfer
    }
}