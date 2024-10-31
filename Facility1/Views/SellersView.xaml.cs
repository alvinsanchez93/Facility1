using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Facility1.DB;
using Facility1.Modals;

namespace Facility1.Views
{
    public partial class SellersView : UserControl
    {
        private readonly facility_2024Entities db = new facility_2024Entities();
        public ObservableCollection<Seller> Sellers { get; set; }

        public SellersView()
        {
            InitializeComponent();
            Sellers = new ObservableCollection<Seller>();
            DataContext = this;
            LoadSellers();
        }

        // Método para cargar la lista de vendedores desde la base de datos
        private void LoadSellers()
        {
            Sellers.Clear();
            var sellersFromDB = db.Seller.Include("Location").ToList();
            foreach (var seller in sellersFromDB)
            {
                Sellers.Add(seller);
            }
        }

        // Método para crear un nuevo vendedor
        private void CreateSeller(object sender, RoutedEventArgs e)
        {
            var sellerModal = new SellerModal(Window.GetWindow(this));
            sellerModal.ShowDialog();
            if (sellerModal.Success)
            {
                MessageBox.Show("Seller created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadSellers(); // Recargar la lista de vendedores después de agregar uno nuevo
            }
        }

        // Método para editar un vendedor existente
        private void EditSeller(object sender, RoutedEventArgs e)
        {
            if (dataGridSellers.SelectedItem is Seller selectedSeller)
            {
                var sellerModal = new SellerModal(Window.GetWindow(this), selectedSeller);
                sellerModal.ShowDialog();
                if (sellerModal.Success)
                {
                    MessageBox.Show("Seller updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadSellers(); // Recargar la lista de vendedores después de editar
                }
            }
            else
            {
                MessageBox.Show("Please select a seller to edit.", "Edit Seller", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Método para eliminar un vendedor seleccionado
        private void DeleteSeller(object sender, RoutedEventArgs e)
        {
            if (dataGridSellers.SelectedItem is Seller selectedSeller)
            {
                var result = MessageBox.Show("Are you sure you want to delete this seller?", "Delete Seller", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    db.Seller.Remove(selectedSeller);
                    db.SaveChanges();
                    MessageBox.Show("Seller deleted successfully!", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadSellers(); // Recargar la lista de vendedores después de eliminar
                }
            }
            else
            {
                MessageBox.Show("Please select a seller to delete.", "Delete Seller", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}

