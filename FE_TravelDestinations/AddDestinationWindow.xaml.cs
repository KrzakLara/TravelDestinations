using SharedData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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
    /// Interaction logic for AddDestinationWindow.xaml
    /// </summary>
    public partial class AddDestinationWindow : Window
    {
        private readonly HttpClient _httpClient;
        private Destinations? _selectedDestination;
        public event EventHandler DestinationAddedOrUpdated;
        public AddDestinationWindow(Destinations? selectedDestination, int? getById)
        {
            InitializeComponent(); // Ensures the XAML components are initialized
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7085")
            };
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (selectedDestination != null)
            {
                _selectedDestination = selectedDestination;
                PopulateHardwareFields();
            }
            else if (getById.HasValue)
            {
                FetchDestination(getById.Value);
            }
            else
            {
                _selectedDestination = null;
            }
        }

        private async void FetchDestination(int id)
        {
            var response = await _httpClient.GetAsync($"Destinations/{id}");
            if (response.IsSuccessStatusCode)
            {
                _selectedDestination = await response.Content.ReadFromJsonAsync<Destinations>();
                PopulateHardwareFields();
            }
            else
            {
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
            }
        }

        private void PopulateHardwareFields()
        {
            if (_selectedDestination == null) return;

            name.Text = _selectedDestination.Name;
            type.Text = _selectedDestination.Type.ToString();
            code.Text = _selectedDestination.Code;
            price.Text = _selectedDestination.Price.ToString();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            var dto = new Destinations
            {
                Name = name.Text,
                Type = Enum.Parse<DestinationsType>(type.Text),
                Code = code.Text,
                Price = Convert.ToDouble(price.Text)
            };

            HttpResponseMessage response;

            if (_selectedDestination != null)
            {
                dto.Id = _selectedDestination.Id;
                response = await _httpClient.PutAsync($"Destinations/{dto.Id}", new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json"));
            }
            else
            {
                response = await _httpClient.PostAsync("Destinations", new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json"));
            }

            if (response.IsSuccessStatusCode)
            {
                DestinationAddedOrUpdated?.Invoke(this, EventArgs.Empty); // Raise the event
                Close();
            }
            else
            {
                MessageBox.Show($"Error Code: {response.StatusCode} - Message: {response.ReasonPhrase}");
            }
        }
    }
}
