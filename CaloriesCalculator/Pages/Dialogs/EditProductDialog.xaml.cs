using CCLibrary.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CaloriesCalculator.Pages.Dialogs
{
    public partial class EditProductDialog : Window
    {
        private double _calories;
        private double _servingSize;
        private double _mass;

        private Nutrition _nutrition;

        public Product Product { get; set; }

        public EditProductDialog(Product product)
        {
            Product = product.Copy();

            InitializeComponent();

            CaloriesTextBox.Text = ((Consumable)Product).CaloriesPerServing.ToString("0.##");
            ServingSizeTextBox.Text = ((Consumable)Product).ServingSizeInGrams.ToString("0.##");
            NetMassTextBox.Text = Product.NetMassInGrams.ToString("0.##");

            if (Product is Food food)
            {
                CarbonatedCheckBox.Visibility = Visibility.Collapsed;
                _nutrition = food.Nutrition;
                FatTextBox.Text = _nutrition.Fat.ToString("0.####");
                ProteintTextBox.Text = _nutrition.Protein.ToString("0.####");
                CarbsTextBox.Text = _nutrition.Carbs.ToString("0.####");
                FiberTextBox.Text = _nutrition.Fiber.ToString("0.####");
                SodiumTextBox.Text = _nutrition.Sodium.ToString("0.####");
            }
            else
            {
                CarbonatedCheckBox.IsChecked = ((Drink)Product).IsCarbonated;
                CarbonatedCheckBox.Visibility = Visibility.Visible;
                NutritionsRow.MaxHeight = 0;
            }

            InitializeTypes();
        }

        private void InitializeTypes()
        {
            Dictionary<int, string> comboBoxTypes = new();

            if (Product is Food food)
            {
                foreach (FoodTypes type in (FoodTypes[])Enum.GetValues(typeof(FoodTypes)))
                {
                    switch (type)
                    {
                        case FoodTypes.Fruit:
                            comboBoxTypes.Add((int)type, "Фрукт");
                            break;
                        case FoodTypes.Vegetable:
                            comboBoxTypes.Add((int)type, "Овоч");
                            break;
                        case FoodTypes.Meat:
                            comboBoxTypes.Add((int)type, "М'ясо");
                            break;
                        case FoodTypes.Fish:
                            comboBoxTypes.Add((int)type, "Риба");
                            break;
                        case FoodTypes.Baking:
                            comboBoxTypes.Add((int)type, "Випічка");
                            break;
                        case FoodTypes.Cereals:
                            comboBoxTypes.Add((int)type, "Пластівці");
                            break;
                        case FoodTypes.Other:
                            comboBoxTypes.Add((int)type, "Інше");
                            break;
                        default:
                            comboBoxTypes.Add((int)type, type.ToString());
                            break;
                    }
                }
                TypeComboBox.ItemsSource = comboBoxTypes;
                TypeComboBox.SelectedIndex = (int)food.Type;
            }
            else if (Product is Drink drink)
            {
                foreach (DrinkTypes type in (DrinkTypes[])Enum.GetValues(typeof(DrinkTypes)))
                {
                    switch (type)
                    {
                        case DrinkTypes.Tap:
                            comboBoxTypes.Add((int)type, "Питна вода");
                            break;
                        case DrinkTypes.Soda:
                            comboBoxTypes.Add((int)type, "Газованка");
                            break;
                        case DrinkTypes.Energy:
                            comboBoxTypes.Add((int)type, "Енергетик");
                            break;
                        case DrinkTypes.Alcohol:
                            comboBoxTypes.Add((int)type, "Алкоголь");
                            break;
                        case DrinkTypes.Juice:
                            comboBoxTypes.Add((int)type, "Сік");
                            break;
                        case DrinkTypes.Coffee:
                            comboBoxTypes.Add((int)type, "Кава");
                            break;
                        case DrinkTypes.Tea:
                            comboBoxTypes.Add((int)type, "Чай");
                            break;
                        case DrinkTypes.Milk:
                            comboBoxTypes.Add((int)type, "Молоко");
                            break;
                        case DrinkTypes.Other:
                            comboBoxTypes.Add((int)type, "Інше");
                            break;
                        default:
                            comboBoxTypes.Add((int)type, type.ToString());
                            break;
                    }
                }
                TypeComboBox.ItemsSource = comboBoxTypes;
                TypeComboBox.SelectedIndex = (int)drink.Type;
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Product.NetMassInGrams = _mass;
                ((Consumable)Product).ServingSizeInGrams = _servingSize;
                ((Consumable)Product).SetCalories(_calories);
            }
            catch
            {
                // ignored
            }

            if (Product is Food food)
                food.Nutrition = _nutrition;

            if (Product is Drink drink && CarbonatedCheckBox.IsChecked is not null)
                drink.IsCarbonated = (bool)CarbonatedCheckBox.IsChecked;

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;

        private double? CheckPositiveValue(object sender, bool zero = false)
        {
            TextBox textBox = (TextBox)sender;
            if (double.TryParse(textBox.Text, out double value))
                if ((zero && value >= 0) || (!zero && value > 0))
                {
                    textBox.BorderThickness = new Thickness(0);
                    return value;
                }
            textBox.BorderThickness = new Thickness(1);
            return null;
        }

        private void CaloriesTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var value = CheckPositiveValue(sender);
            if (value is null)
                return;
            _calories = (double)value;
        }

        private void ServingSizeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var value = CheckPositiveValue(sender);
            if (value is null)
                return;
            _servingSize = (double)value;
        }

        private void NetMassTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var value = CheckPositiveValue(sender);
            if (value is null)
                return;
            _mass = (double)value;
        }

        private void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object selectedItem = ((ComboBox)sender).SelectedItem;
            if (selectedItem is null)
                return;
            int selectedType = ((KeyValuePair<int, string>)selectedItem).Key;

            if (Product is Food food)
                food.Type = (FoodTypes)selectedType;
            else if (Product is Drink drink)
                drink.Type = (DrinkTypes)selectedType;
        }

        private void NutritionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var value = CheckPositiveValue(sender, true);
            if (value is null)
                return;
            switch (((TextBox)sender).Name)
            {
                case "FatTextBox":
                    _nutrition.Fat = (double)value;
                    break;
                case "ProteintTextBox":
                    _nutrition.Protein = (double)value;
                    break;
                case "CarbsTextBox":
                    _nutrition.Carbs = (double)value;
                    break;
                case "FiberTextBox":
                    _nutrition.Fiber = (double)value;
                    break;
                case "SodiumTextBox":
                    _nutrition.Sodium = (double)value;
                    break;
            }
        }
    }
}
