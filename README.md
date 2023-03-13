# Курсова робота на тему: Віконний додаток "Калькулятор калорій"

|Курс, група|1, ІПЗ-22-2 \[2]|
|:-|:-|
|Розробник|Шевцов М. С.|
|Керівник|Чижмотря О. В.|

---

---

# Структура класів та документація бібліотеки `CCLibrary`

- [`Products`](#user-content-cclibraryproducts)
	- [Абстрактний `Product`](#user-content-product-class)
		- [`Dish`](#user-content-dish-class)
		- [`Consumable`](#user-content-consumable-class)
			- [`Food`](#user-content-food-class)
			- [`Drink`](#user-content-drink-class)
				- [`EnergyDrink`](#user-content-energydrink-class)
				- [`AlcoholDrink`](#user-content-alcoholdrink-class)
- [`User`](#user-content-cclibraryuser)
	- [`Profile`](#user-content-profile-class)
	- [`DailyConsumption`](#user-content-dailyconsumption-class)
- [`Data`](#user-content-cclibrarydata)
	- [`...`]
- [Виключення](#user-content-cclibraryexceptions)
	- [`ValueOutOfRangeException`](#user-content-valueoutofrangeexception)
	- [`VitaminAlreadyExistsException`](#user-content-vitaminalreadyexistsexception)
	- [`VitaminNotFoundException`](#user-content-vitaminnotfoundexception)

## CCLibrary.Products

Бібліотека, що містить в собі усі основні класи, якими оперує додаток.

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

## CCLibrary.Exceptions

Можливі виключення у бібліотеці та їх причини.

### ValueOutOfRangeException
Namespace: CCLibrary.Exceptions

Виникає, якщо значення, що записується у властивість, або передається у метод, не відповідає
певному діапазону значень.

Наприклад:

- Можливо записати тільки додатні числа, від 0 або більше (іноді включно з нулем);
- Відсотки лежать у діапазоні від 0 до 100.

```cs
public ValueOutOfRangeException(string message) : base(message)
```

### VitaminAlreadyExistsException
Namespace: CCLibrary.Exceptions

Виникає при спробі додати вітамін до продукту, якщо він вже є у словнику.

Повідомлення за замовчуванням:
> Вітамін з такою назвою вже існує!

```cs
public VitaminAlreadyExistsException(string message = message) : base(message)
```

### VitaminNotFoundException
Namespace: CCLibrary.Exceptions

Виникає при спробі видалити вітамін з продукту, якщо він відсутній.

Повідомлення за замовчуванням:
> Вітамін з такою назвою не знайдено!

```cs
public VitaminNotFoundException(string message = message) : base(message)
```
