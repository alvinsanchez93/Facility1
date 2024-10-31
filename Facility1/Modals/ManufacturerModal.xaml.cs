using System;
using System.Windows;
using Facility1.DB;

namespace Facility1.Modals
{
    public partial class ManufacturerModal : Window
    {
        private readonly facility_2024Entities db;
        public bool Success { get; private set; }

        public ManufacturerModal(Window owner)
        {
            InitializeComponent();
            Owner = owner;
            db = new facility_2024Entities();
            Success = false;
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            // Validar que los campos no estén vacíos
            if (string.IsNullOrWhiteSpace(txtManufacturerCode.Text) ||
                string.IsNullOrWhiteSpace(txtManufacturerName.Text))
            {
                MessageBox.Show("Please fill out all fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Crear y guardar el nuevo fabricante
            var newManufacturer = new Manufacturer
            {
                iManufacturer = Guid.NewGuid(),
                ManufacturerCode = txtManufacturerCode.Text,
                ManufacturerName = txtManufacturerName.Text
            };
            db.Manufacturer.Add(newManufacturer);
            db.SaveChanges();

            Success = true;
            MessageBox.Show("Manufacturer saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            Success = false;
            Close();
        }
    }
}
