# Курсова робота на тему: Віконний додаток "Калькулятор калорій"

|Курс, група|1, ІПЗ-22-2 \[2]|
|:-|:-|
|Розробник|Шевцов М. С.|
|Керівник|Чижмотря О. В.|

---

---

# Структура класів та документація бібліотеки `CCLibrary`

- [Data](#user-content-cclibrarydata)
	- [Database](#user-content-database-class)
	- [ProfileContext](#user-content-profilecontext-class)
	- [ProductContext](#user-content-productcontext-class)
- [User](#user-content-cclibraryuser)
	- [Profile](#user-content-profile-class)
	- [DailyConsumption](#user-content-dailyconsumption-class)
- [Products](#user-content-cclibraryproducts)
	- [Product](#user-content-product-class)
		- [Dish](#user-content-dish-class)
		- [Consumable](#user-content-consumable-class)
			- [Food](#user-content-food-class)
			- [Drink](#user-content-drink-class)
				- [EnergyDrink](#user-content-energydrink-class)
				- [AlcoholDrink](#user-content-alcoholdrink-class)
- [Exceptions](#user-content-cclibraryexceptions)
	- [ProfileAlreadyExistsException](#user-content-profilealreadyexistsexception)
	- [ProfiletNotFoundException](#user-content-profilenotfoundexception)
	- [WrongPasswordException](#user-content-wrongpasswordexception)
	- [ProductNotFoundException](#user-content-productnotfoundexception)
	- [ValueOutOfRangeException](#user-content-valueoutofrangeexception)
	- [VitaminAlreadyExistsException](#user-content-vitaminalreadyexistsexception)
	- [VitaminNotFoundException](#user-content-vitaminnotfoundexception)

## CCLibrary.Data

### Database class
Namespace: CCLibrary.Data

#### Означення

Клас що ініціалізує та надає доступ до локальної бази даних, у якій зберігаються продукти,
профілі користувачів та інша інформація.

```cs
public class Database
```

#### Конструктори

- `public Database()`

#### Поля

- `internal static readonly string _dbSource`
- `internal SqliteConnection _connection`

#### Методи

##### CheckConnection

Перевіряє чи відкрита база даних, якщо ні то відкриває її.
```cs
internal void CheckConnection();
```

##### Initialize

Ініціалізує базу даних, створює файл для неї та таблиці, якщо ті не існують.
```cs
private void Initialize();
```

---

### ProfileContext class
Namespace: CCLibrary.Data

#### Означення

Контекст для керуванням профілями.

```cs
public class ProfileContext : DbContext
```

#### Конструктори

- `public ProfileContext()`

#### Властивості

- `public DbSet<Profile> Profiles`

#### Методи

##### CreateProfile

Створює у базі даних профіль з вказаним логіном та паролем. Якщо профіль вже існує - то виникне
помилка `ProfileAlreadyExistsException`.
```cs
public Profile CreateProfile(string login, string password);
```

##### GetProfile

Повертає профіль, якщо його знайдено у базі та вказаний пароль співпадає.
Якщо профіль не знайдено то виникне помилка `ProfiletNotFoundException`, а якщо не співпадають
паролі то - `WrongPasswordException`.
```cs
public Profile GetProfile(string login, string password);
```

#### Виключення

- `ProfileAlreadyExistsException`
- `ProfiletNotFoundException`
- `WrongPasswordException`

---

### ProductContext class
Namespace: CCLibrary.Data

#### Означення

Контекст для керуванням продуктами.

```cs
public class ProductContext : DbContext
```

#### Конструктори

- `public ProductContext()`

#### Властивості

- `public DbSet<Product> Products`

#### Методи

##### GetProduct

Повертає продукт з вказаним ID.
```cs
public Product? GetProduct(ulong id);
```

##### AddProduct

Додає продукт до бази даних.
```cs
public void AddProduct(Product product);
```

##### DeleteProduct

Видаляє вказаний продукт з бази даних.
```cs
public void DeleteProduct(ulong id);
```

#### Виключення

- `ProductNotFoundException`

## CCLibrary.User

### Profile class
Namespace: CCLibrary.User

#### Означення

Клас представляє профіль користувача.

```cs
public class Profile
```

#### Конструктори

- `public Profile(string login, string password)`

#### Поля

- `internal string _login`
- `private string _password`
- `public ulong Id`
- `public string Name`
- `public byte[]? Image`
- `public DateTime BirthDay`
- `protected float _height`
- `protected float _weight`

#### Властивості

- `public int Age`
- `public float HeightInCm`
- `public float WeightInKg`

#### Методи

##### CheckPassword

Перевіряє вказаний пароль на дійсність.
```cs
public bool CheckPassword(string password);
```

##### UpdatePassword

Оновлює пароль якщо старий пароль вказано вірно.
```cs
public void UpdatePassword(string oldPassword, string newPassword);
```

#### Виключення

- `ValueOutOfRangeException`
- `WrongPasswordException`

---

### DailyConsumption class
Namespace: CCLibrary.User

#### Означення

Представляє собою збірку продкутів, що вжив певний профіль в певний день.

```cs
public class DailyConsumption
```

#### Конструктори

- `public DailyConsumption(Profile profile, DateTime date)`

#### Поля

- `public Profile LinkedProfile`
- `public DateTime Date`
- `private List<Product> _consumedProducts`

#### Методи

##### CalculateCalories

Підраховує загальну кількість вжитих калорій за день.
```cs
public double CalculateCalories();
```

##### CalculateEnergy

Переводить калорії в енергію.
```cs
public double CalculateEnergy();
```

##### GetProducts

Повертає колекцію з продуктів, що вживалися.
```cs
public List<Product> GetProducts();
```

##### Consume

Додає продукт до списку вживаних.
```cs
public void Consume(Product product);
```

##### UpdateConsumed

Оновлює список вживаних продуктів на новий.
```cs
public void UpdateConsumed(List<Product> products);
```

## CCLibrary.Products

### Product class
Namespace: CCLibrary.Products

#### Означення

Абстрактний клас, який використовується для узагальнення продуктів, що мають властивість представлення
маси нетто та рахувати кількість калорій.

```cs
public abstract class Product
```

#### Конструктори

- `public Product(ulong id, string name)`

#### Поля

- `public ulong Id`
- `public string Name`
- `public string Description`
- `protected double _netMass`

#### Властивості

- `public double NetMassInGrams`
- `public double NetMassInKilos`
- `public double NetMassInPounds`

#### Методи

##### GetCalories

Абстрактний метод розрахунку кількості калорій, що містяться в продукті.
```cs
public abstract double GetCalories();
```

##### GetEnergy

Метод використовує асбтрактцію `GetCalories()` та отримане значення конвертує в енергію, або ж Джоулі.
```cs
public double GetEnergy();
```

#### Виключення

- `ValueOutOfRangeException`

---

### Dish class
Namespace: CCLibrary.Products

#### Означення

Похідний клас від `Product`. Визначає страви, що можуть складатися з різноманітних продуктів.

```cs
public class Dish : Product
```

#### Конструктори

- `public Dish(string name, List<Consumable>? ingredients = null)`

#### Поля

- `private List<Consumable> _ingredients`

#### Методи

##### GetIngredients

Повертає копію списку інгредієнтів страви.
```cs
public List<Consumable> GetIngredients();
```

##### AddIngredient

Додати інгредієнт до страви.
```cs
public void AddIngredient(Consumable consumable);
```

##### UpdateIngredients

Оновити список інгредієнтів страви.
```cs
public void UpdateIngredients(List<Consumable> ingredients);
```

##### GetCalories

Підрахувати загальну кількість калорій усіх інгредієнтів страви.
```cs
public override double GetCalories();
```

---

### Consumable class
Namespace: CCLibrary.Products

#### Означення

Похідний клас від абстрактного `Product`. Визначає продукти, які можна вживати.

```cs
public class Consumable : Product
```

#### Конструктори

- `public Consumable(string name, double caloriesPerServing)`

#### Поля

- `protected double _caloriesPerServing`
- `protected double _servingSize`

#### Властивості

- `public double CaloriesPerServing`
- `public double ServingSizeInGrams`

#### Методи

##### GetCalories

Розраховує кількість калорій на основі полів `_caloriesPerServing`, `_servingSize` та `_netMass`.
```cs
public override double GetCalories();
```

#### Виключення

- `ValueOutOfRangeException`

---

### Food class
Namespace: CCLibrary.Products

#### Означення

Похідний клас від `Consumable`. Визначає тверду їжу та її властивості.

```cs
public class Food : Consumable
```

#### Конструктори

- `public Food(string name, double caloriesPerServing, Nutritions nutritions = new())`

#### Властивості

- `readonly struct Nutritions`

#### Методи

##### UpdateNutritions

Оновлює показники харчової цінності для цього продукту.
```cs
public void UpdateNutritions(Nutritions nutritions);
```

---

### Drink class
Namespace: CCLibrary.Products

#### Означення

Похідний клас від `Consumable`. Обособлює напої, є базовим класом для їх різновидів.

```cs
public class Drink : Consumable
```

#### Конструктори

- `public Drink(string name, double caloriesPerServing, DrinkTypes drinkType = DrinkTypes.Tap)`

#### Поля

- `protected DrinkTypes _drinkType`

#### Властивості

- `public DrinkTypes Type`
- `public bool IsCarbonated`

---

### EnergyDrink class
Namespace: CCLibrary.Products

#### Означення

Похідний клас від `Drink`. Визначає енергетичні напої.

```cs
public class EnergyDrink : Drink
```

#### Конструктори

- `public EnergyDrink(string name, double caloriesPerServing, Dictionary<string, double>? vitamins = null)`

#### Поля

- `protected Dictionary<string, double> _vitamins`

#### Методи

##### GetVitamins

Повертає копію словника, що містить в собі вітаміни та їх кількість в мг на 
`_servingSize` грам цього продукту.
```cs
public Dictionary<string, double> GetVitamins();
```

##### GetVitaminsTotal

Повертає новий словник, що містить в собі загальну кількість вітамінів на всю масу продукту.
```cs
public Dictionary<string, double> GetVitaminsTotal();
```

##### AddVitamin

Додає вказаний вітамін та його кількість у мг в розрахунку на `_servingSize` грам.
```cs
public void AddVitamin(string name, double value);
```

##### RemoveVitamin

Видаляє вказаний вітамін з продукту.
```cs
public void RemoveVitamin(string name);
```

#### Виключення

- `ValueOutOfRangeException`
- `VitaminAlreadyExistsException`
- `VitaminNotFoundException`

---

### AlcoholDrink class
Namespace: CCLibrary.Products

#### Означення

Похідний клас від `Drink`. Визначає алкогольні напої.

```cs
public class AlcoholDrink : Drink
```

#### Конструктори

- `public AlcoholDrink(string name, double caloriesPerServing, double alcoholContent)`

#### Поля

- `protected double _alcoholContent`

#### Властивості

- `public double AlcoholContent`

#### Виключення

- `ValueOutOfRangeException`

## CCLibrary.Exceptions

### ProfileAlreadyExistsException
Namespace: CCLibrary.Exceptions

Виникає якщо при створені нового профілю логін вже існує у базі даних.

Повідомлення за замовчуванням:
> Профіль вже існує!

### ProfiletNotFoundException
Namespace: CCLibrary.Exceptions

Виникає якщо профіль з вказаним логіном не знайдено у базі даних.

Повідомлення за замовчуванням:
> Профіль не знайдено!

### WrongPasswordException
Namespace: CCLibrary.Exceptions

Виникая якщо вказаний пароль введено неправильно.

Повідомлення за замовчуванням:
> Невірний пароль!

### ProductNotFoundException
Namespace: CCLibrary.Exceptions

Виникає якщо продукт з вказаними параметрами не знайдено в базі даних.

Повідомлення за замовчуванням:
> Продукт не знайдено!

### ValueOutOfRangeException
Namespace: CCLibrary.Exceptions

Виникає, якщо значення, що записується у властивість, або передається у метод, не відповідає
певному діапазону значень.

Наприклад:

- Можливо записати тільки додатні числа, від 0 або більше (іноді включно з нулем);
- Відсотки лежать у діапазоні від 0 до 100.


### VitaminAlreadyExistsException
Namespace: CCLibrary.Exceptions

Виникає при спробі додати вітамін до продукту, якщо він вже є у словнику.

Повідомлення за замовчуванням:
> Вітамін з такою назвою вже існує!

### VitaminNotFoundException
Namespace: CCLibrary.Exceptions

Виникає при спробі видалити вітамін з продукту, якщо він відсутній.

Повідомлення за замовчуванням:
> Вітамін з такою назвою не знайдено!