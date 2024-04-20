using SharedData.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Net.Http.Json;

namespace FE_TravelDestinations
{
    /// <summary>
    /// Interaction logic for DeleteDestinationWindow.xaml
    /// </summary>
    public partial class DeleteDestinationWindow : Window
    {
        private readonly HttpClient _httpClient;
        public event EventHandler DestinationDeleted;

        private Destinations selectedDestination;
        public DeleteDestinationWindow(Destinations selectedDestination)
        {
            InitializeComponent();
            this.selectedDestination = selectedDestination;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7085/"),
                DefaultRequestHeaders = { Accept = { new MediaTypeWithQualityHeaderValue("application/json") } }
            };
            // You might not need to fetch all destinations if you're just deleting one
        }

        private async void DeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            // Confirmation dialog
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete this destination: {selectedDestination.Name}?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // User confirmed deletion
                HttpResponseMessage response = await _httpClient.DeleteAsync($"Destinations/{selectedDestination.Id}");
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Destination {selectedDestination.Name} deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    DestinationDeleted?.Invoke(this, EventArgs.Empty); // Notify the MainWindow
                    this.Close(); // Close the window after deletion
                }
                else
                {
                    MessageBox.Show($"Failed to delete destination. Error Code: {response.StatusCode} - Message: {response.ReasonPhrase}", "Deletion Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                // User canceled the deletion
                // Optionally handle this case, e.g., log the action or provide feedback
            }
        }

    }
}

