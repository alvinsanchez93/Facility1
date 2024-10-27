using Facility1.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Facility1.Modals
{
    public partial class OrdersModal : Window
    {
        private facility_2024Entities db = new facility_2024Entities();

        public List<Seller> Sellers { get; set; }
        public List<Client> Clients { get; set; }
        public List<Product> Products { get; set; }
        public ObservableCollection<OrderProduct> OrderProducts { get; set; }
        public bool Success { get; set; }

        public OrdersModal(Window parentWindow)
        {
            Owner = parentWindow;
            OrderProducts = new ObservableCollection<OrderProduct>();
            GetSellers();
            GetClients();
            GetProducts();
            InitializeComponent();
            DataContext = this;
        }

        private void GetSellers() => Sellers = db.Seller.ToList();
        private void GetClients() => Clients = db.Client.ToList();
        private void GetProducts() => Products = db.Product.ToList();

        private void QuantityTextChange(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }

        private void AddProduct(object sender, RoutedEventArgs e)
        {
            Product selectedProduct = (Product)cmbProduct.SelectedItem;
            if (selectedProduct == null || string.IsNullOrEmpty(txtQuantity.Text) || !int.TryParse(txtQuantity.Text, out int quantity))
            {
                MessageBox.Show("Please select a product and enter a valid quantity.");
                return;
            }

            decimal amount = quantity * selectedProduct.ProductPrice;
            OrderProduct product = new OrderProduct
            {
                Product = selectedProduct,
                OPQuantity = quantity,
                OPAmmount = amount
            };
            OrderProducts.Add(product);
        }

        private void DeletedProduct(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            OrderProduct product = button.DataContext as OrderProduct;
            OrderProducts.Remove(product);
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Success = false;
            Close();
        }

        private void CreateOrder(object sender, RoutedEventArgs e)
        {
            Seller selectedSeller = (Seller)cmbSeller.SelectedItem;
            Client selectedClient = (Client)cmbClient.SelectedItem;

            if (selectedSeller == null || selectedClient == null || !OrderProducts.Any())
            {
                MessageBox.Show("Please complete the order details.");
                return;
            }

            Guid iOrder = Guid.NewGuid();
            decimal total = OrderProducts.Sum(p => p.OPAmmount.Value);
            List<OrderProduct> products = OrderProducts.Select(p => new OrderProduct
            {
                iOrder = iOrder,
                iProduct = p.Product.iProduct,
                OPQuantity = p.OPQuantity,
                OPAmmount = p.OPAmmount
            }).ToList();

            Order OrderToSave = new Order
            {
                iOrder = iOrder,
                OrderCreatedAt = DateTime.Now,
                iSeller = selectedSeller.iSeller,
                iClient = selectedClient.iClient,
                OrderTotal = total
            };
            db.Order.Add(OrderToSave);
            db.OrderProduct.AddRange(products);
            db.SaveChanges();
            Success = true;
            Close();
        }
    }
}

