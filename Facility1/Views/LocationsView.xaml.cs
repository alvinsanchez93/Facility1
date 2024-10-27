using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Facility1.DB;
using Facility1.Modals;

namespace Facility1.Views
{
    /// <summary>
    /// Lógica de interacción para LocationsView.xaml
    /// </summary>
    public partial class LocationsView : UserControl
    {
        private facility_2024Entities db = new facility_2024Entities();
        public List<Location> Locations { get; set; }

        public List<City> Cities { get; set; }
       
        public LocationsView()
        {
            GetLocations();
            GetCities();
            InitializeComponent();
        }

        private void GetLocations()
        {
            Locations = db.Location.ToList();
        }

        private void GetCities()
        {
            Cities = db.City.ToList();
        }

        private void CreateLocation(object sender, RoutedEventArgs e)
        {
            LocationsModal modal = new LocationsModal(Window.GetWindow(this));
            Opacity = 0.4;
            modal.ShowDialog();
            Opacity = 1;
            if (modal.Succes)
            {
                MessageBox.Show("Location Created  Successfully", "Location Created", MessageBoxButton.OK, MessageBoxImage.Information);
                GetLocations();
            }
        }

        private void EditLocation(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Location location = button.DataContext as Location;
            LocationsModal modal = new LocationsModal(Window.GetWindow(this), location.iLocation);
            Opacity = 1;
            if (modal.Succes)
            {
                MessageBox.Show("Location Update Succesfully", "Location Update", MessageBoxButton.OK, MessageBoxImage.Information);
                GetLocations();
            }
        }

        private void DeleteLocation(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Location location = button.DataContext as Location;
            MessageBoxResult result = MessageBox.Show("Are you sure to Deleted this location?", "Deleted Location", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                db.Location.Remove(location);
                db.SaveChanges();
                MessageBox.Show("Location Deleter  Succesfully", "Location Update", MessageBoxButton.OK, MessageBoxImage.Information);
                GetLocations();
            }
        }
    }
}
