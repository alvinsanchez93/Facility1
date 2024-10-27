using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Facility1.DB;

namespace Facility1.Modals
{
    /// <summary>
    /// Lógica de interacción para LocationsModal.xaml
    /// </summary>
    public partial class LocationsModal : Window
    {
        private facility_2024Entities db = new facility_2024Entities();
        public List<City> Cities { get; set; }
        public bool Succes { get; set; }
        private Guid iLocation { get; set; }

        public LocationsModal(Window parentWindow)
        {
            Owner = parentWindow;
            GetCities();
            InitializeComponent();
            DataContext = this;
        }

        public LocationsModal(Window parentWindow, Guid location)
        {
            Owner = parentWindow;
            iLocation = location;
            GetCities();
            InitializeComponent();
            DataContext = this;
            LoadLocationData(); // Cargar datos para la edición
        }

        private void LoadLocationData()
        {
            var locationToEdit = db.Location.FirstOrDefault(loc => loc.iLocation == iLocation);
            if (locationToEdit != null)
            {
                txtName.Text = locationToEdit.LocationName;
                txtGoal.Text = locationToEdit.LocationGoal.ToString();
                cmbCity.SelectedValue = locationToEdit.iCity;
            }
        }

        public void GoalTextChange(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsValidDecimalInput(e.Text, (TextBox)sender);
        }

        private bool IsValidDecimalInput(string input, TextBox textBox)
        {
            if (input == "." && textBox.Text.Contains("."))
                return false;

            return decimal.TryParse(textBox.Text + input, out _);
        }

        private void GetCities()
        {
            Cities = db.City.ToList();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Succes = false;
            Close();
        }

        private void SaveLocation(object sender, RoutedEventArgs e)
        {
            City selectedCity = (City)cmbCity.SelectedItem;

            if (iLocation != null && iLocation != Guid.Empty) // estamos editando
            {
                Location locationToEdit = db.Location.FirstOrDefault(location => location.iLocation == iLocation);
                locationToEdit.LocationName = txtName.Text;
                locationToEdit.LocationGoal = Decimal.Parse(txtGoal.Text);
                locationToEdit.iCity = selectedCity.iCity;
                db.SaveChanges();
                Succes = true;
                Close();
            }
            else // estamos creando una nueva sucursal
            {
                if (db.Location.Any(location => location.LocationName.Trim().ToLower() == txtName.Text.Trim().ToLower()))
                {
                    Succes = false;
                    MessageBox.Show("Location already exists", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    Location locationToSave = new Location
                    {
                        iLocation = Guid.NewGuid(),
                        LocationName = txtName.Text,
                        LocationGoal = decimal.Parse(txtGoal.Text),
                        LocationSales = 0,
                        iCity = selectedCity.iCity
                    };
                    db.Location.Add(locationToSave);
                    db.SaveChanges();
                    Succes = true;
                    Close();
                }
            }
        }
    }
}

