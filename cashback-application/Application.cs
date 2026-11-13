using System.Reflection;
using cashback_domain.Entities;
using cashback_infrastructure.DB;
using Ninject;

namespace cashback_application
{
    public static class Application
    {
        static IBankRepository bankRepository;
        static IMCCRepository mCCRepository;
        static IBankCardRepository bankCardRepository;
        static IDiscountCardRepository discountCardRepository;

        //при первом запуске - создать бд и сохранить пути к ним
        public static void Initialize(string dataSource)
        {
            var container = ConfigureContainer();

            bankRepository = container.Get<IBankRepository>();
            mCCRepository = container.Get<IMCCRepository>();
            bankCardRepository = container.Get<IBankCardRepository>();
            discountCardRepository = container.Get<IDiscountCardRepository>();
        }

        public static StandardKernel ConfigureContainer()
        {
            var container = new StandardKernel();

            container.Bind<IBankRepository>().To<BankRepository>().InSingletonScope();
            container.Bind<IBankCardRepository>().To<BankCardRepository>().InSingletonScope();
            container.Bind<IMCCRepository>().To<MCCRepository>().InSingletonScope();
            container.Bind<IDiscountCardRepository>().To<DiscountCardRepository>().InSingletonScope();

            return container;
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
            EditDBEntry(bankCardRepository, id, fieldName, value);

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
            EditDBEntry(discountCardRepository, id, fieldName, value);
        }

        public static async void EditDBEntry<TEntity, TID>(
            IDBRepository<TEntity, TID> dBRepository, TID id, string fieldName, object value)
            where TEntity : IEntity<TID>
        {
            var entity = await dBRepository.FindByIDAsync(id);
            Type type = entity.GetType();
            FieldInfo field = type.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(entity, value);
            }
            entity.SaveToDB(dBRepository);
        }
    }
}
