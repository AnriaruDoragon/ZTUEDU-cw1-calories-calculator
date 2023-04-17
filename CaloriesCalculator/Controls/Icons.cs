using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using CCLibrary.Products;

namespace CaloriesCalculator;

public static class Icons
{
    private enum Scope
    {
        UI,
        Products
    }

    private static readonly Dictionary<Scope, IEnumerable<string>> _icons = new()
    {
        {
            Scope.UI,
            new List<string>()
            {
                "Profile", "Food", "History", "Gear"
            }
        },
        {
            Scope.Products,
            new List<string>()
            {
                "Fruit", "Vegetable", "Meat", "Fish", "Baking", "Cereals",
                "Energy", "Alcohol", "Juice", "Coffee", "Tea", "Milk"
            }
        }
    };

    /// <summary>
    /// Returns a DrawingImage icon for ui display with provided name.
    /// </summary>
    public static DrawingImage? GetUIIconImage(string iconName)
    {
        IEnumerable<string> icons = _icons[Scope.UI];

        if (!icons.Contains(iconName))
            return null;

        return Application.Current.Resources[$"{iconName}DrawingImage"] as DrawingImage;
    }

    /// <summary>
    /// Returns a DrawingImage icon for provided product.
    /// </summary>
    public static DrawingImage? GetProductIconImage(Product product)
    {
        IEnumerable<string> icons = _icons[Scope.Products];

        string iconString = "ProductDrawingImage";

        if (product is Food food)
        {
            iconString = icons.Contains(food.Type.ToString())
                ? $"{food.Type}DrawingImage"
                : "ProductDrawingImage";
        }
        else if (product is Drink drink)
        {
            iconString = icons.Contains(drink.Type.ToString())
                ? $"{drink.Type}DrawingImage"
                : "DrinkDrawingImage";
        }

        return Application.Current.Resources[iconString] as DrawingImage;
    }
}
