using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using Facility1.DB;

namespace Facility1.Modals
{
    public partial class SellerModal : Window
    {
        private readonly facility_2024Entities db;
        private readonly Seller seller;
        public bool Success { get; private set; }

        public SellerModal(Window owner, Seller existingSeller = null)
        {
            InitializeComponent();
            Owner = owner;
            db = new facility_2024Entities();
            Success = false;

            // Cargar la lista de ubicaciones Alvin Cesar Sanchez Ochoa Alvarez
            cmbLocation.ItemsSource = db.Location.ToList();

            // Si se está editando un vendedor existente
            if (existingSeller != null)
            {
                seller = existingSeller;
                txtSellerNumber.Text = seller.SellerNumber.ToString();
                txtSellerName.Text = seller.SellerName;
                txtSellerSurname.Text = seller.SellerSurname;
                txtCreatedAt.Text = seller.SellerCreatedAt.ToString("g");
                txtSellerGoal.Text = seller.SellerGoal?.ToString() ?? "";
                txtSellerSales.Text = seller.SellerSales?.ToString() ?? "";
                cmbLocation.SelectedItem = db.Location.Find(seller.iLocation);
            }
            else
            {
                seller = new Seller
                {
                    SellerCreatedAt = DateTime.Now
                };
                txtCreatedAt.Text = seller.SellerCreatedAt.ToString("g"); // Mostrar la fecha de creación por defecto
            }
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            // Validar que los campos no estén vacíos Alvin Cesar Sanchez Ochoa Alvarez
            if (string.IsNullOrWhiteSpace(txtSellerName.Text) ||
                string.IsNullOrWhiteSpace(txtSellerSurname.Text) ||
                cmbLocation.SelectedItem == null)
            {
                MessageBox.Show("Please fill out all required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Asignar valores al objeto Seller
            seller.SellerName = txtSellerName.Text;
            seller.SellerSurname = txtSellerSurname.Text;
            seller.SellerGoal = string.IsNullOrWhiteSpace(txtSellerGoal.Text) ? (decimal?)null : decimal.Parse(txtSellerGoal.Text);
            seller.SellerSales = string.IsNullOrWhiteSpace(txtSellerSales.Text) ? (decimal?)null : decimal.Parse(txtSellerSales.Text);
            seller.iLocation = ((Location)cmbLocation.SelectedItem).iLocation;

            // Guardar o actualizar el vendedor Alvin Cesar Sanchez Ochoa Alvarez
            if (seller.iSeller == Guid.Empty)
            {
                seller.iSeller = Guid.NewGuid();
                db.Seller.Add(seller);
            }
            db.SaveChanges();

            Success = true;
            MessageBox.Show("Seller saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            Success = false;
            Close();
        }

        // Validación para permitir solo números y decimales en los campos de meta de ventas y ventas actuales
        private void NumericTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, "^[0-9]*(\\.[0-9]{0,2})?$");
        }
    }
}

