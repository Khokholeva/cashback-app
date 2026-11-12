namespace cashback_domain.Entities
{
    public class BankCardEntity : IBankCardEntity
    {
        public IMCCEntity[] CashBackCategories { get; init; }
        public int BankId {  get; init; }
        public string AdditionalInfo { get; init; }
        public DateTime NextNotification { get; set; }
        public int Id { get; set; }

        public void SaveToDB(IDBRepository<BankCardEntity, int> dBRepository)
        {
            throw new NotImplementedException();
        }
    }
}
