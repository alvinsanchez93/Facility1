using System;
using System.Windows;
using Facility1.DB;

namespace Facility1.Modals
{
    public partial class ClientsModal : Window
    {
        private readonly facility_2024Entities db;
        private Client existingClient;
        public bool Success { get; private set; } // Indicador de éxito

        // Constructor para crear un nuevo cliente
        public ClientsModal(Window owner)
        {
            InitializeComponent();
            db = new facility_2024Entities();
            Owner = owner;
            Success = false;
        }

        // Constructor para editar un cliente existente Alvin Cesar Sanchez Ochoa Alvarez
        public ClientsModal(Window owner, Client clientToEdit) : this(owner)
        {
            existingClient = clientToEdit;
            LoadClientData();
        }

        private void LoadClientData()
        {
            // Cargar datos del cliente en los campos de texto si es una edición Alvin Cesar Sanchez Ochoa Alvarez
            if (existingClient != null)
            {
                txtClientName.Text = existingClient.ClientName;
                txtCreditLimit.Text = existingClient.ClientCreditLimit?.ToString("F2") ?? "0.00";
            }
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            // Validar entrada de datos
            if (string.IsNullOrWhiteSpace(txtClientName.Text) || !decimal.TryParse(txtCreditLimit.Text, out decimal creditLimit))
            {
                MessageBox.Show("Please enter a valid name and credit limit.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (existingClient == null)
            {
                // Crear un nuevo cliente solo si no existe uno
                var newClient = new Client
                {
                    iClient = Guid.NewGuid(),
                    ClientName = txtClientName.Text,
                    ClientCreditLimit = creditLimit
                };
                db.Client.Add(newClient);
            }
            else
            {
                // Actualizar el cliente existente Alvin Cesar Sanchez Ochoa Alvarez
                existingClient.ClientName = txtClientName.Text;
                existingClient.ClientCreditLimit = creditLimit;
            }

            db.SaveChanges(); // Guardar los cambios en la base de datos
            Success = true; // Indicar que la operación fue exitosa
            MessageBox.Show("Client saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            Close(); // Cerrar el modal inmediatamente después de guardar
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            // Cerrar el modal sin guardar cambios
            Success = false;
            Close();
        }

        private void CreditLimitTextChange(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            // Permitir solo entrada numérica en el campo de límite de crédito
            e.Handled = !decimal.TryParse(e.Text, out _);
        }
    }
}

