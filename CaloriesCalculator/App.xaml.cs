using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CCLibrary.Data;

namespace CaloriesCalculator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Database Database;

        public App()
        {
            InitializeComponent();
            Database = new Database();
        }
    }
}
