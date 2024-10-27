using Facility1.DB;
using Facility1.Modals;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

//Alvin Cesar Sanchez Ochoa Alvarez
namespace Facility1.Views
{
    /// <summary>
    /// Lógica de interacción para OrdersView.xaml
    /// </summary>
    public partial class OrdersView : UserControl
    {
        private facility_2024Entities db = new facility_2024Entities();
        public ObservableCollection<Order> Orders { get; set; }

        public OrdersView()
        {
            Orders = new ObservableCollection<Order>();
            GetOrders();
            InitializeComponent();
            DataContext = this;
        }
        //Alvin Cesar Sanchez Ochoa Alvarez
        private void GetOrders()
        {
            Orders.Clear();
            var ordersFromDB = db.Order.ToList();
            foreach (Order order in ordersFromDB)
            {
                Orders.Add(order);
            }
        }

        private void CreateOrder(object sender, RoutedEventArgs e)
        {
            OrdersModal modal = new OrdersModal(Window.GetWindow(this));
            Opacity = 0.4;
            modal.ShowDialog();
            Opacity = 1;
            if (modal.Success)
            {
                MessageBox.Show("Order created successfully", "Order Created", MessageBoxButton.OK, MessageBoxImage.Information);
                GetOrders();
            }
        }
    }
}

