using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SharedData.Models;
using System.Net.Http.Json;

namespace FE_TravelDestinations
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly HttpClient _httpClient;
        private Destinations? selectedDestinations { get; set; }


        public MainWindow()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7085");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        //get  (create/fetch data)
        private void FetchData_Click(object sender, RoutedEventArgs e)
        {
            HttpResponseMessage response = _httpClient.GetAsync("Destinations").Result;
            if (response.IsSuccessStatusCode)
            {
                var hardwareList = response.Content.ReadFromJsonAsync<List<Destinations>>().Result;
                dataGrid.ItemsSource = hardwareList;
            }
            else
            {
                MessageBox.Show($"Error Code: {response.StatusCode} - Message: {response.ReasonPhrase}");

            }
        }

        //get  (create/fetch data)
        private void CreateDestination_Click(object sender, RoutedEventArgs e)
        {
            var addDestinationWindow = new AddDestinationWindow(null, null);
            addDestinationWindow.DestinationAddedOrUpdated += AddDestinationWindow_DestinationAddedOrUpdated;
            addDestinationWindow.Show();
        }

        private void AddDestinationWindow_DestinationAddedOrUpdated(object sender, EventArgs e)
        {
            FetchData_Click(this, new RoutedEventArgs()); // Refresh the data grid
        }


        //put (update)

        private void UpdateDestination_Click(object sender, RoutedEventArgs e)
        {
            if (selectedDestinations == null)
            {
                MessageBox.Show("Please select a destination to update.");
                return;
            }

            var updateDestinationNameWindow = new UpdateDestinationNameWindow(selectedDestinations);
            updateDestinationNameWindow.DestinationUpdated += UpdateDestinationNameWindow_DestinationUpdated;
            updateDestinationNameWindow.Show();
        }

        // Event handler that refreshes the data grid
        private void UpdateDestinationNameWindow_DestinationUpdated(object sender, EventArgs e)
        {
            FetchData_Click(this, new RoutedEventArgs());
        }


        //delete 

        private void DeleteDestination_Click(object sender, RoutedEventArgs e)
        {
            if (selectedDestinations == null)
            {
                MessageBox.Show("Please select a destination to delete.");
                return;
            }

            var deleteDestinationWindow = new DeleteDestinationWindow(selectedDestinations);
            deleteDestinationWindow.DestinationDeleted += DeleteDestinationWindow_DestinationDeleted;
            deleteDestinationWindow.ShowDialog(); // Use ShowDialog to make it modal if appropriate
        }

        private void DeleteDestinationWindow_DestinationDeleted(object sender, EventArgs e)
        {
            // Refresh the data grid to reflect the deletion
            FetchData_Click(this, new RoutedEventArgs());
        }



        private void RowSelected(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                selectedDestinations = (Destinations)dataGrid.SelectedItem;
                btnDelete.IsEnabled = true;
                btnUpdate.IsEnabled = true;
            }
            else
            {
                btnDelete.IsEnabled = false;
                btnUpdate.IsEnabled = false;
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}