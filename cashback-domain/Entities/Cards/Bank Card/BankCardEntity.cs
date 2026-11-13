namespace cashback_domain.Entities
{
    public class BankCardEntity : IBankCardEntity
    {
        public int[] CashBackCategoriesIDs { get; set; }
        public int BankId {  get; init; }
        private string addInfo;
        public string AdditionalInfo { get => addInfo ??= ""; set => addInfo = value; }
        public DateTime NextNotification { get; set; }
        public int? Id { get; set; }
        public int? Limit { get; set; }
        private string lastDigits;
        public string LastDigits { get => lastDigits ??= ""; set => lastDigits = value; }
        private string expires;
        public string Expires { get => expires ??= ""; set => expires = value; }
    }
}
