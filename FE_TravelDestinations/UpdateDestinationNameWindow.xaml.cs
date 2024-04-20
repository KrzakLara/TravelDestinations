using SharedData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
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

namespace FE_TravelDestinations
{
    /// <summary>
    /// Interaction logic for UpdateDestinationNameWindow.xaml
    /// </summary>
    public partial class UpdateDestinationNameWindow : Window
    {
        private HttpClient _httpClient = new HttpClient();
        public event EventHandler DestinationUpdated;
        public UpdateDestinationNameWindow(Destinations selectedDestinations)
        {
            InitializeComponent();
            FetchDestinationsAndPopulateComboBox();
        }

        private async void FetchDestinationsAndPopulateComboBox()
        {

            var response = await _httpClient.GetAsync("https://localhost:7085/destinations");
            if (response.IsSuccessStatusCode)
            {
                var destinations = await response.Content.ReadFromJsonAsync<List<Destinations>>();
                destinationsComboBox.ItemsSource = destinations;
                destinationsComboBox.DisplayMemberPath = "Name";
                destinationsComboBox.SelectedValuePath = "Id";
            }
            else
            {
                MessageBox.Show("Failed to load destinations.");
            }
        }

        private async void UpdateDestinationName_Click(object sender, RoutedEventArgs e)
        {
            if (destinationsComboBox.SelectedItem is Destinations selectedDestination)
            {
                selectedDestination.Name = newNameTextBox.Text;

                // Example PUT request - adjust to your API's needs
                var response = await _httpClient.PutAsJsonAsync($"https://localhost:7085/destinations/{selectedDestination.Id}", selectedDestination);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Destination name updated successfully.");

                    // Raise the event to notify subscribers
                    DestinationUpdated?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    MessageBox.Show("Failed to update the destination name.");
                }
            }
            else
            {
                MessageBox.Show("Please select a destination to update.");
            }
        }
    }
}