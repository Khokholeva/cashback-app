using cashback_domain.DB;
using cashback_domain.Entities;
using cashback_domain.Entities.Bank;
using cashback_domain.Entities.Cards.Bank_Card;
using cashback_domain.Entities.MCC;
using cashback_infrastructure.DB.LocalDB;
using System.Reflection;

namespace cashback_application
{
    public static class Application
    {
        static BankRepository bankRepository;
        static IMCCRepository mCCRepository;
        static IBankCardRepository bankCardRepository;
        static IDiscountCardRepository discountCardRepository;

        //при первом запуске - создать бд и сохранить пути к ним
        public static void Initialize()
        {
            bankRepository = new BankRepository();
            mCCRepository = new MCCRepository();
            bankCardRepository = new BankCardRepository();
            discountCardRepository = new DiscountCardRepository();
        }

        /// <summary>
        /// из формы заполненной в ui (из метода типа OnClickSubmit) передать все нужные строки
        /// </summary>
        public static void AddBankCard() //аргументы каких типов?
        {
            var card = new BankCardEntity(); // аргументы?
            card.SaveToDB(bankCardRepository); // разобраться с ошибками в файлах domain
        }

        public static void DeleteBankCard(int id) // что будет передаваться? дб нужен в итоге id
        {
            bankCardRepository.DeleteByIDAsync(id);
        }

        public static async Task<IEnumerable<IBankCardEntity>> GetBankCards()
        {
            return await bankCardRepository.GetAllEntriesAsync();
        }

        public static async void EditBankCard(int id, string fieldName, object value) // возможны другие варианты аргументов, предлагайте
        {
            var card = await bankCardRepository.FindByIDAsync(id);
            Type type = card.GetType();
            FieldInfo field = type.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(card, value);
            }
            card.SaveToDB(bankCardRepository);

        }

        public static void AddDiscountCard()
        {
            var card = new DiscountCardEntity(); // аргументы?
            card.SaveToDB(discountCardRepository); // разобраться с ошибками в файлах domain
        }

        public static void DeleteDiscountCard(int id) // что будет передаваться? дб нужен в итоге id
        {
            discountCardRepository.DeleteByIDAsync(id);
        }

        public static async Task<IEnumerable<IDiscountCardEntity>> GetDiscountCards()
        {
            return await discountCardRepository.GetAllEntriesAsync();
        }

        public static async void EditDiscountCard(int id, string fieldName, object value) // возможны другие варианты аргументов, предлагайте
        {
            var card = await discountCardRepository.FindByIDAsync(id);
            Type type = card.GetType();
            FieldInfo field = type.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(card, value);
            }
            card.SaveToDB(discountCardRepository);
        }
    }
}
