using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using Facility1.DB;

namespace Facility1.Modals
{
    public partial class ProductModal : Window
    {
        private readonly facility_2024Entities db;
        private readonly Product product;
        public bool Success { get; private set; }

        public ProductModal(Window owner, Product existingProduct = null)
        {
            InitializeComponent();
            Owner = owner;
            db = new facility_2024Entities();
            Success = false;

            // Cargar la lista de fabricantes en el ComboBox
            cmbManufacturer.ItemsSource = db.Manufacturer.ToList();

            // Si se pasa un producto existente, configurar el modal para editar
            if (existingProduct != null)
            {
                product = existingProduct;
                txtProductCode.Text = product.ProductCode;
                txtProductDescription.Text = product.ProductDescription;
                txtProductPrice.Text = product.ProductPrice.ToString();
                cmbManufacturer.SelectedItem = product.Manufacturer;
            }
            else
            {
                product = new Product();
            }
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            // Validar entrada de datos
            if (string.IsNullOrWhiteSpace(txtProductCode.Text) ||
                string.IsNullOrWhiteSpace(txtProductDescription.Text) ||
                string.IsNullOrWhiteSpace(txtProductPrice.Text) ||
                cmbManufacturer.SelectedItem == null)
            {
                MessageBox.Show("Please fill out all fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Asignar los valores del producto
            product.ProductCode = txtProductCode.Text;
            product.ProductDescription = txtProductDescription.Text;
            product.ProductPrice = decimal.Parse(txtProductPrice.Text);
            product.Manufacturer = (Manufacturer)cmbManufacturer.SelectedItem;

            // Guardar el producto en la base de datos
            if (product.iProduct == Guid.Empty)
            {
                product.iProduct = Guid.NewGuid();
                db.Product.Add(product);
            }
            db.SaveChanges();
            Success = true;
            MessageBox.Show("Product saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            Success = false;
            Close();
        }

        // Método para validar solo números en el campo de precio
        private void NumericTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, "^[0-9]*(\\.[0-9]{0,2})?$");
        }
    }
}

