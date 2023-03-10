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
- [`Drink`]()

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

### Food class
Namespace: CCLibrary

#### Означення

Похідний клас від `Consumable`. Визначає тверду їжу та її властивості.

```cs
internal class Food : Consumable
```

#### Конструктори

- `public Food(ulong id, double caloriesPerServing)`
- `public Food(ulong id, string name, double caloriesPerServing)`

#### Поля

- 

#### Властивості

- 

#### Методи

