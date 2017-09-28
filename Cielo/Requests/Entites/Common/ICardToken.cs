namespace Cielo.API.Requests.Entites.Common
{
    public interface ICardToken
    {
        string CardToken { get; }
        bool SaveCard { get;  }
    }
}
