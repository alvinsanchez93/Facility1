using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Facility1.DB;
using Facility1.Modals;

namespace Facility1.Views
{
    public partial class ProductsView : UserControl
    {
        private readonly facility_2024Entities db = new facility_2024Entities();
        public ObservableCollection<Product> Products { get; set; }

        public ProductsView()
        {
            InitializeComponent();
            Products = new ObservableCollection<Product>();
            DataContext = this;
            LoadProducts();
        }

        // Método para cargar los productos de la base de datos
        private void LoadProducts()
        {
            Products.Clear();
            var productsFromDB = db.Product.Include("Manufacturer").ToList();
            foreach (var product in productsFromDB)
            {
                Products.Add(product);
            }
        }

        // Método para manejar la creación de un nuevo producto
        private void CreateProduct(object sender, RoutedEventArgs e)
        {
            var productModal = new ProductModal(Window.GetWindow(this));
            productModal.ShowDialog();
            if (productModal.Success)
            {
                MessageBox.Show("Product created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadProducts(); // Recargar la lista de productos después de crear uno nuevo
            }
        }

        // Método para manejar la creación de un nuevo fabricante
        private void CreateManufacturer(object sender, RoutedEventArgs e)
        {
            var manufacturerModal = new ManufacturerModal(Window.GetWindow(this));
            manufacturerModal.ShowDialog();
            if (manufacturerModal.Success)
            {
                MessageBox.Show("Manufacturer created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadProducts(); // Recargar la lista de productos para reflejar el nuevo fabricante
            }
        }

        // Método para manejar la edición de un producto seleccionado
        private void EditProduct(object sender, RoutedEventArgs e)
        {
            if (dataGridProductos.SelectedItem is Product selectedProduct)
            {
                var productModal = new ProductModal(Window.GetWindow(this), selectedProduct);
                productModal.ShowDialog();
                if (productModal.Success)
                {
                    MessageBox.Show("Product updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadProducts(); // Recargar la lista de productos después de la edición
                }
            }
            else
            {
                MessageBox.Show("Please select a product to edit.", "Edit Product", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Método para manejar la eliminación de un producto seleccionado
        private void DeleteProduct(object sender, RoutedEventArgs e)
        {
            if (dataGridProductos.SelectedItem is Product selectedProduct)
            {
                var result = MessageBox.Show("Are you sure you want to delete this product?", "Delete Product", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    db.Product.Remove(selectedProduct);
                    db.SaveChanges();
                    MessageBox.Show("Product deleted successfully!", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadProducts(); // Recargar la lista de productos después de la eliminación
                }
            }
            else
            {
                MessageBox.Show("Please select a product to delete.", "Delete Product", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}

