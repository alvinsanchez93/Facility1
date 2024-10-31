using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Facility1.DB;
using Facility1.Modals;

namespace Facility1.Views
{
    public partial class ClientsView : UserControl
    {
        private readonly facility_2024Entities db = new facility_2024Entities();
        public ObservableCollection<Client> Clients { get; set; }

        public ClientsView()
        {
            InitializeComponent();
            Clients = new ObservableCollection<Client>();
            DataContext = this;
            LoadClients();
        }

        private void LoadClients()
        {
            // Limpiar y cargar la lista de clientes desde la base de datos
            Clients.Clear();
            var clientsFromDB = db.Client.ToList();
            foreach (var client in clientsFromDB)
            {
                Clients.Add(client);
            }
        }

        private void CreateClient(object sender, RoutedEventArgs e)
        {
            // Abrir el modal para crear un nuevo cliente
            var clientModal = new ClientsModal(Window.GetWindow(this));
            clientModal.ShowDialog();
            if (clientModal.Success)
            {
                MessageBox.Show("Client created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadClients(); // Recargar la lista de clientes solo después de confirmar éxito
            }
        }

        private void EditClient(object sender, RoutedEventArgs e)
        {
            // Verificar si se seleccionó un cliente en el DataGrid
            if (dataGridClientes.SelectedItem is Client selectedClient)
            {
                // Crear y abrir el modal con los datos del cliente seleccionado para editar
                var clientModal = new ClientsModal(Window.GetWindow(this), selectedClient);
                clientModal.ShowDialog();
                if (clientModal.Success)
                {
                    MessageBox.Show("Client updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadClients(); // Recargar la lista de clientes
                }
            }
            else
            {
                MessageBox.Show("Please select a client to edit.", "Edit Client", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteClient(object sender, RoutedEventArgs e)
        {
            // Verificar si se seleccionó un cliente en el DataGrid
            if (dataGridClientes.SelectedItem is Client selectedClient)
            {
                // Mostrar confirmación antes de eliminar
                var result = MessageBox.Show("Are you sure you want to delete this client?", "Delete Client", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    db.Client.Remove(selectedClient); // Eliminar el cliente de la base de datos
                    db.SaveChanges(); // Guardar los cambios
                    MessageBox.Show("Client deleted successfully!", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadClients(); // Recargar la lista de clientes
                }
            }
            else
            {
                MessageBox.Show("Please select a client to delete.", "Delete Client", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}

