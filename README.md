# Курсова робота на тему: Віконний додаток "Калькулятор калорій"

|Курс, група|1, ІПЗ-22-2 \[2]|
|:-|:-|
|Розробник|Шевцов М. С.|
|Керівник|Чижмотря О. В.|

---

---

# Структура класів та документація

- [Абстрактний `Product`](#user-content-product-class)
- [`Consumable`](#user-content-consumable-class)
- [`Food`](#user-content-food-class)
- [`Drink`](#user-content-drink-class)


## CCLibrary

Бібліотека, що містить в собі усі основні класи, якими оперує додаток.

### Product class
Namespace: CCLibrary

#### Означення

Абстрактний клас, який використовується для узагальнення продуктів, що мають властивість представлення
маси нетто та рахувати кількість калорій.

```cs
public abstract class Product
```

#### Конструктори

- `public Product(ulong id)`
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
Namespace: CCLibrary

#### Означення

Похідний клас від абстрактного `Product`. Визначає продукти, які можна вживати.

```cs
internal class Consumable : Product
```

#### Конструктори

- `public Consumable(ulong id, double caloriesPerServing)`
- `public Consumable(ulong id, string name, double caloriesPerServing)`

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
Namespace: CCLibrary

#### Означення

Похідний клас від `Consumable`. Визначає тверду їжу та її властивості.

```cs
internal class Food : Consumable
```

#### Конструктори

- `public Food(ulong id, double caloriesPerServing, Nutritions nutritions = new())`
- `public Food(ulong id, string name, double caloriesPerServing, Nutritions nutritions = new())`

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
Namespace: CCLibrary

#### Означення

Похідний клас від `Consumable`. Обособлює напої, є базовим класом для їх різновидів.

```cs
internal class Drink : Consumable
```

#### Конструктори

- `public Drink(ulong id, double caloriesPerServing)`
- `public Drink(ulong id, string name, double caloriesPerServing)`

---

### EnergyDrink class
Namespace: CCLibrary

#### Означення

Похідний клас від `Drink`. Визначає енергетичні напої.

```cs
internal class EnergyDrink : Drink
```

#### Конструктори

- `public EnergyDrink(ulong id, double caloriesPerServing, Dictionary<string, double>? vitamins = null)`
- `public EnergyDrink(ulong id, string name, double caloriesPerServing, Dictionary<string, double>? vitamins = null)`

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
Namespace: CCLibrary

#### Означення

Похідний клас від `Drink`. Визначає алкогольні напої.

```cs
internal class AlcoholDrink : Drink
```

#### Конструктори

- `public AlcoholDrink(ulong id, double caloriesPerServing, double alcoholContent)`
- `public AlcoholDrink(ulong id, string name, double caloriesPerServing, double alcoholContent)`

#### Поля

- `protected double _alcoholContent`

#### Властивості

- `public double AlcoholContent`

#### Виключення

- `ValueOutOfRangeException`

---