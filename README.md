# Курсова робота на тему: Віконний додаток "Калькулятор калорій"

|Курс, група|1, ІПЗ-22-2 \[2]|
|:-|:-|
|Розробник|Шевцов М. С.|
|Керівник|Чижмотря О. В.|

---

---

# Структура та документація [CCLibrary (wiki)](../../wikis/CCLibrary):

- [Data](../../wikis/CCLibrary/Data-namespace)
	- [Settings](../../wikis/CCLibrary/Data-namespace/Settings-class)
	- [Database](../../wikis/CCLibrary/Data-namespace/Database-class)
	- [DataContext](../../wikis/CCLibrary/Data-namespace/DataContext-class)
- [User](../../wikis/CCLibrary/User-namespace)
	- [Profile](../../wikis/CCLibrary/User-namespace/Profile-abstract-class)
	- [DailyConsumption](../../wikis/CCLibrary/User-namespace/DailyConsumption-class)
- [Products namespace](../../wikis/CCLibrary/Products-namespace)
	- [Product](../../wikis/CCLibrary/Products-namespace/Product-abstract-class)
		- [Dish](../../wikis/CCLibrary/Products-namespace/Dish-class)
		- [Consumable](../../wikis/CCLibrary/Products-namespace/Consumable-class)
			- [Food](../../wikis/CCLibrary/Products-namespace/Food-class)
			- [Drink](../../wikis/CCLibrary/Products-namespace/Drink-class)
				- [EnergyDrink](../../wikis/CCLibrary/Products-namespace/EnergyDrink-class)
				- [AlcoholDrink](../../wikis/CCLibrary/Products-namespace/AlcoholDrink-class)
- [Exceptions](../../wikis/CCLibrary/Exceptions-namespace)
	- [SettingsException](../../wikis/CCLibrary/Exceptions-namespace#settingsexception)
	- [ProfileAlreadyExistsException](../../wikis/CCLibrary/Exceptions-namespace#profilealreadyexistsexception)
	- [ProfiletNotFoundException](../../wikis/CCLibrary/Exceptions-namespace#profiletnotfoundexception)
	- [WrongPasswordException](../../wikis/CCLibrary/Exceptions-namespace#wrongpasswordexception)
	- [ProductNotFoundException](../../wikis/CCLibrary/Exceptions-namespace#productnotfoundexception)
	- [ValueOutOfRangeException](../../wikis/CCLibrary/Exceptions-namespace#valueoutofrangeexception)
	- [VitaminAlreadyExistsException](../../wikis/CCLibrary/Exceptions-namespace#vitaminalreadyexistsexception)
	- [VitaminNotFoundException](../../wikis/CCLibrary/Exceptions-namespace#vitaminnotfoundexception)
