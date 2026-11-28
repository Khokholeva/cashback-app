using cashback_domain.Entities;
using cashback_infrastructure.DB;
using Ninject;

namespace cashback_application
{
    public static class Application
    {
        static IBankLocalRepository bankLocalRepository;
        static IMCCLocalRepository mCCLocalRepository;
        static IBankCardLocalRepository bankCardLocalRepository;
        static IDiscountCardLocalRepository discountCardLocalRepository;
        static IShopLocalRepository shopLocalRepository;
        static IPromotionLocalRepository promotionLocalRepository;


        //при первом запуске - создать бд и сохранить пути к ним
        public static async void Initialize(string localDataSource, string remoteDataSource)
        {
            var container = ConfigureContainer();

            bankLocalRepository = container.Get<IBankLocalRepository>();
            mCCLocalRepository = container.Get<IMCCLocalRepository>();
            bankCardLocalRepository = container.Get<IBankCardLocalRepository>();
            discountCardLocalRepository = container.Get<IDiscountCardLocalRepository>();
            shopLocalRepository = container.Get<IShopLocalRepository>();
            promotionLocalRepository = container.Get<IPromotionLocalRepository>();

            await bankLocalRepository.InitializeDBAsync(localDataSource);
            await mCCLocalRepository.InitializeDBAsync(localDataSource);
            await bankCardLocalRepository.InitializeDBAsync(localDataSource);
            await discountCardLocalRepository.InitializeDBAsync(localDataSource);
            await shopLocalRepository.InitializeDBAsync(localDataSource);
            await promotionLocalRepository.InitializeDBAsync(localDataSource);
        }

        public static StandardKernel ConfigureContainer()
        {
            var container = new StandardKernel();

            container.Bind<IBankLocalRepository>().To<BankLocalRepository>().InSingletonScope();
            container.Bind<IBankCardLocalRepository>().To<BankCardLocalRepository>().InSingletonScope();
            container.Bind<IMCCLocalRepository>().To<MCCLocalRepository>().InSingletonScope();
            container.Bind<IDiscountCardLocalRepository>().To<DiscountCardLocalRepository>().InSingletonScope();
            container.Bind<IShopLocalRepository>().To<ShopLocalRepository>().InSingletonScope();
            container.Bind<IPromotionLocalRepository>().To<PromotionLocalRepository>().InSingletonScope();

            return container;
        }

        ///// <summary>
        ///// из формы заполненной в ui (из метода типа OnClickSubmit) передать все нужные строки
        ///// </summary>
        //public static void AddBankCard() //аргументы каких типов?
        //{
        //    var card = new BankCardEntity(); // аргументы?
        //    card.SaveToDB(bankCardRepository); // разобраться с ошибками в файлах domain
        //}

        //public static void DeleteBankCard(int id) // что будет передаваться? дб нужен в итоге id
        //{
        //    bankCardRepository.DeleteByIDAsync(id);
        //}

        //public static async IAsyncEnumerable<IBankCardEntity> GetBankCards()
        //{
        //    return bankCardRepository.GetAllEntriesAsync();
        //}

        //public static async void EditBankCard(int id, string fieldName, object value) // возможны другие варианты аргументов, предлагайте
        //{
        //    EditDBEntry(bankCardRepository, id, fieldName, value);

        //}

        //public static void AddDiscountCard()
        //{
        //    var card = new DiscountCardEntity(); // аргументы?
        //    card.SaveToDB(discountCardRepository); // разобраться с ошибками в файлах domain
        //}

        //public static void DeleteDiscountCard(int id) // что будет передаваться? дб нужен в итоге id
        //{
        //    discountCardRepository.DeleteByIDAsync(id);
        //}

        //public static async Task<IEnumerable<IDiscountCardEntity>> GetDiscountCards()
        //{
        //    return await discountCardRepository.GetAllEntriesAsync();
        //}

        //public static async void EditDiscountCard(int id, string fieldName, object value) // возможны другие варианты аргументов, предлагайте
        //{
        //    EditDBEntry(discountCardRepository, id, fieldName, value);
        //}

        //public static async void EditDBEntry<TEntity, TID>(
        //    IDBRepository<TEntity, TID> dBRepository, TID id, string fieldName, object value)
        //    where TEntity : IEntity<TID>
        //{
        //    var entity = await dBRepository.FindByIDAsync(id);
        //    Type type = entity.GetType();
        //    FieldInfo field = type.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        //    if (field != null)
        //    {
        //        field.SetValue(entity, value);
        //    }
        //    entity.SaveToDB(dBRepository);
        //}
    }
}
