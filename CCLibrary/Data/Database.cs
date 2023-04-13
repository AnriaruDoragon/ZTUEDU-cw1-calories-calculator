using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using CCLibrary.Products;

namespace CCLibrary.Data
{
    public class Database
    {
        internal static readonly string DbSource = @"Data Source=CaloriesCalculator.db";
        internal SqliteConnection Connection;

        public DataContext Context;

        public Database()
        {
            Connection = new SqliteConnection(DbSource);

            var optionsBuilder = new DbContextOptionsBuilder<DataContext>().UseSqlite(Connection);
            Context = new DataContext(optionsBuilder.Options, Connection);

            Initialize();
        }

        /// <summary>
        /// Check and keep connection open.
        /// </summary>
        internal static void ReopenConnection(SqliteConnection connection)
        {
            switch (connection.State)
            {
                case System.Data.ConnectionState.Closed:
                    connection.Open();
                    break;
                case System.Data.ConnectionState.Broken:
                    connection = new SqliteConnection(DbSource);
                    connection.Open();
                    break;
            }
        }

        private void Initialize()
        {
            if (!Context.Database.CanConnect())
            {
                Context.Database.Migrate();

                ReopenConnection(Connection);
                new SqliteCommand(@"
                    CREATE TABLE IF NOT EXISTS ProfileConsumedProducts (
                        rowid INTEGER PRIMARY KEY,
                        ProfileID INTEGER NOT NULL,
                        ProductID INTEGER NOT NULL,
                        ProductMass FLOAT NOT NULL,
                        ConsumedDate DATE NOT NULL
                    );", Connection).ExecuteNonQuery();
                Connection.Close();
            }

            if (!Context.Products.Any())
                CreateDefaultProducts();
        }

        private void CreateDefaultProducts()
        {
            // Data from https://www.tablycjakalorijnosti.com.ua/
            Product[] products =
            {
                new Food("Яблуко", 63, FoodTypes.Fruit, new Nutrition(0.4, 0.37, 12.95, 3.14))
                {
                    Description = "Яблука містять корисну для організму клітковину й унікальний компонент – пектин. Разом вони позитивно впливають на травлення.",
                    NetMassInGrams = 200
                },
                new Food("Банан", 94, FoodTypes.Fruit, new Nutrition(0.2, 1.2, 22, 2, 0.001))
                {
                    Description = "Банан містить в собі вітаміни групи С, В, вітамін А, магній, кальцій та фосфор.",
                    NetMassInGrams = 170
                },
                new Food("Лохина", 39, FoodTypes.Fruit, new Nutrition(0.5, 1, 6.6, 2.5, 0.02))
                {
                    Description = "Свіжа лохина це болотна ягода, яка росте в основному в північних районах, в прохолодному кліматі. Має темно-синє забарвлення, з білуватим нальотом.",
                    NetMassInGrams = 120
                },
                new Food("Помідор ", 20, FoodTypes.Vegetable, new Nutrition(0.2, 0.9, 3.9, 1))
                {
                    Description = "Помідор містить в собі 92% води та вітаміни A, C, D, F, K, також всі вітаміни групи В. Також містить в собі кальцій, калій, залізо та магній.",
                    NetMassInGrams = 100
                },
                new Food("Огірок ", 16, FoodTypes.Vegetable, new Nutrition(0.18, 0.82, 2.28, 0.93))
                {
                    Description = "Свіжий огірок багатий вітамінами, особливо вітаміном С, В1, В2 та Е, а також білком та бета-каротином. Також багатий калієм, який сприяє виведенню зайвої рідини з організму.",
                    NetMassInGrams = 100
                },
                new Food("Перець червоний солодкий", 35, FoodTypes.Vegetable, new Nutrition(0.3, 1, 6, 1.7))
                {
                    Description = "Червоний болгарський перець – один з лідерів по вмісту вітаміну С. Він містить більше вітаміну С, ніж апельсини, лимони та ягоди. Щоб отримати денну норму цього вітаміну, достатньо додати у салат 70 г солодкого перцю.",
                    NetMassInGrams = 100
                },
                new Food("Помідори Черрі", 21, FoodTypes.Vegetable, new Nutrition(0.2, 0.9, 3.6, 1.2))
                {
                    Description = "Маленькі помідори, солодкуваті на смак.",
                    NetMassInGrams = 100
                },
                new Food("Кукурудза", 101, FoodTypes.Vegetable, new Nutrition(1.1, 3.1, 18.5, 2.5))
                {
                    Description = "Варена.",
                    NetMassInGrams = 100
                },
                new Food("Листя салату", 16, FoodTypes.Vegetable, new Nutrition(0.3, 1.2, 1.3, 2.1))
                {
                    NetMassInGrams = 100
                },
                new Food("Куряче філе", 165, FoodTypes.Meat, new Nutrition(3.5, 30, 0, 0, 0.19))
                {
                    Description = "Куряче філе, запечене у духовці з додаванням невеликої кількості олії для змащення.",
                    NetMassInGrams = 200
                },
                new Food("Куряче філе", 179, FoodTypes.Meat, new Nutrition(7, 29, 0.6))
                {
                    Description = "Смажене.",
                    NetMassInGrams = 200
                },
                new Food("Куряча грудка", 151, FoodTypes.Meat, new Nutrition(3, 29, 0, 0, 0.15))
                {
                    Description = "Варене куряче філе.",
                    NetMassInGrams = 200
                },
                new Food("Відбивна з курячої грудки", 178, FoodTypes.Meat, new Nutrition(10.98, 18.93, 7))
                {
                    Description = "Відбивна з курячої грудки, підсмажена на мінімальній кількості олії або масла.",
                    NetMassInGrams = 200
                },
                new Food("Котлета куряча", 222, FoodTypes.Meat, new Nutrition(12, 21, 13.8, 0.9, 2.25))
                {
                    Description = "Куряча котлета, у складі якої куряче філе, білий хліб, молоко, масло, сіль (пропорції в грамах: 1000 / 250 / 350 / 30 / 7).",
                    NetMassInGrams = 200
                },
                new Food("Котлети зі свинини смажені", 310, FoodTypes.Meat, new Nutrition(28, 14, 5.8, 0.8))
                {
                    Description = "Котлети можна запікати в духовці, обсмажувати, тушкувати, готувати на пару і навіть відварювати у воді. Все це впливає на смакові якості і відбивається на калорійності.",
                    NetMassInGrams = 200
                },
                new Food("Телятина запечена", 106, FoodTypes.Meat, new Nutrition(2.5, 19.7, 1.1))
                {
                    Description = "Телятина, запечена у духовці. Телятина містить дуже багато корисних компонентів, зокрема: багаті на амінокислоти білки, вітаміни В1, В2, В3, В4, В5, В6 і В9, РР, Е. Багато у ній заліза, необхідного для нормального стану крові, і цинку, що пришвидшує обмін речовин, позитивно впливає на волосся та нігті.",
                    NetMassInGrams = 200
                },
                new Food("Бекон смажений", 485, FoodTypes.Meat, new Nutrition(37, 35, 1.6, 0, 4.82))
                {
                    NetMassInGrams = 200
                },
                new Food("Тушонка свиняча", 367, FoodTypes.Meat, new Nutrition(35, 13, 0, 0, 1.25))
                {
                    NetMassInGrams = 200
                },
                new Food("Лосось слабосолений", 203, FoodTypes.Fish, new Nutrition(13, 21.18, 1.35, 0, 4))
                {
                    Description = "Сирий лосось, який приготували методом соління протягом 12+ годин.",
                    NetMassInGrams = 200
                },
                new Food("Тунець у власному соку", 101, FoodTypes.Fish, new Nutrition(1, 23, 0.1, 0, 1.2))
                {
                    Description = "Тунець, консервований у власному соці, воді і солі, без додавання олії.",
                    NetMassInGrams = 200
                },
                new Food("Хліб білий", 253, FoodTypes.Baking, new Nutrition(4, 11, 48, 4, 2))
                {
                    Description = "Білий хліб – це несолодка випічка, основним інгредієнтом якої є пшеничне борошно. Фактично, такий хліб можна поділити на корисний і не дуже корисний. До першого варіанту відноситься білий хліб з борошна першого сорту або із додаванням висівок, а до не дуже корисного – з борошна вищого сорту.",
                    NetMassInGrams = 150
                },
                new Food("Хліб чорний", 201, FoodTypes.Baking, new Nutrition(1.1, 6.6, 41, 6.4, 1))
                {
                    Description = "Чорний хліб – це несолодка випічка з суміші житнього та пшеничного борошна. Енергетична цінність чорного хліба представлена вуглеводами, більшість з яких – корисні (проте, це залежить від складу).",
                    NetMassInGrams = 150
                },
                new Food("Лаваш тонкий", 274, FoodTypes.Baking, new Nutrition(0.7, 8.4, 57.1, 2, 1))
                {
                    Description = "Дуже тонкі листки білого хлібу без дріжджів (схожі на млинці). У складі лише біле борошно, вода і сіль.",
                    NetMassInGrams = 150
                },
                new Food("Круасан", 406, FoodTypes.Baking, new Nutrition(21, 8.2, 46, 2.6, 0.85))
                {
                    Description = "Класичні круасани, виготовлені з білого пшеничного борошна, в які додаються інші інгредієнти (дріжджі, молоко, цукор, яйце, жир, сіль і т.д.). Часто додаються інші хімічні речовини.",
                    NetMassInGrams = 250
                },
                new Food("Булка здобна", 339, FoodTypes.Baking, new Nutrition(9.4, 7.9, 55.5, 2.1, 0.68))
                {
                    Description = "Мова йде про хлібо-булочний виріб з пшеничної муки. Поживних речовин практично там не міститься. Білий хліб сприяє різкому підвищенню глюкози в крові, так же швидко і знижується що сприяє легкому голоду, краще вживати продукти з жита це корисніше.",
                    NetMassInGrams = 200
                },
                new Food("Ватрушка з сиром", 318, FoodTypes.Baking, new Nutrition(12, 10, 40, 1.5))
                {
                    NetMassInGrams = 200
                },
                new Food("Вівсяне печиво", 450, FoodTypes.Baking, new Nutrition(18, 5.51, 69, 2.8, 0.1))
                {
                    Description = "Вівсяне печиво – це солодке печиво, в складі якого присутнє вівсяне борошно. Енергетична цінність вівсяного печива висока, і в основному складається з вуглеводів та жирів.",
                    NetMassInGrams = 100
                },
                new Food("Вівсяні пластівці", 400, FoodTypes.Cereals, new Nutrition(6.87, 13.14, 68.09, 7.24))
                {
                    NetMassInGrams = 120
                },
                new Food("Кукурудзяні пластівці", 380, FoodTypes.Cereals, new Nutrition(1.4, 7.4, 82.5, 4.2, 1.9))
                {
                    NetMassInGrams = 120
                },
                new Food("Гранола", 430, FoodTypes.Cereals, new Nutrition(18, 14.8, 70, 6))
                {
                    NetMassInGrams = 120
                },
                new Food("Батончик Fitness", 370, FoodTypes.Cereals, new Nutrition(5.5, 6, 71.4, 7.1, 0.76))
                {
                    Description = "Злаковий батончик зі смаком полуниці від Nestlé.",
                    NetMassInGrams = 75
                },
                new Drink("Питна вода", 0)
                {
                    Description = "Звичайна питна вода.",
                    NetMassInGrams = 200
                },
                new Drink("CocaCola", 105, DrinkTypes.Soda)
                {
                    Description = "Напій безалкогольний сильногазований на ароматизаторах Кока-Кола.",
                    IsCarbonated = true,
                    NetMassInGrams = 200,
                    ServingSizeInGrams = 250
                },
                new Drink("RedBull", 46, DrinkTypes.Energy)
                {
                    Description = "Безалкогольний середньогазований енергетичний напій. Пастеризований.\nНіацин і вітамін B6, що входять до складу напою, сприяють зменшенню втоми, а також нормальному енергетичному обміну ручовин.",
                    IsCarbonated = true,
                    NetMassInGrams = 355
                },
                new Drink("Martini Fiero", 155, DrinkTypes.Alcohol)
                {
                    Description = "Вермут червоний десертний.",
                    NetMassInGrams = 50
                },
                new Drink("Grante", 64, DrinkTypes.Juice)
                {
                    Description = "Гранатовий сік прямого віджиму.",
                    NetMassInGrams = 150
                },
                new Drink("Латте", 45, DrinkTypes.Coffee)
                {
                    Description = "Суміш молока та кави.",
                    NetMassInGrams = 150
                },
                new Drink("Зелений чай", 0.4, DrinkTypes.Tea)
                {
                    Description = "Без цукру.",
                    NetMassInGrams = 150
                },
                new Drink("Молоко 2.5%", 51, DrinkTypes.Milk)
                {
                    Description = "Знежирене молоко.",
                    NetMassInGrams = 200
                }
            };

            Context.Products.AddRange(products);
            Context.SaveChanges();
        }
    }
}
