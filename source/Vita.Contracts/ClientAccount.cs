namespace Vita.Contracts
{
    public class ClientAccount : ValueObject
    {
        public ClientId ClientId { get; set; }
        public AccountId AccountId { get; set; }

        public ClientAccount(ClientId clientId, AccountId accountId)
        {
            ClientId = clientId;
            AccountId = accountId;
        }
    }
}