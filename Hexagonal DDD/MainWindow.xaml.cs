using HexagonalDDD.Application.UseCases.Create_Customer;
using HexagonalDDD.Application.UseCases.Create_Vehicle;
using HexagonalDDD.Application.UseCases.Get_Available_Vehicles;
using HexagonalDDD.Application.UseCases.Get_Customers;
using HexagonalDDD.Application.UseCases.Get_Rented_Vehicles;
using HexagonalDDD.Application.UseCases.Rent_Vehicle;
using HexagonalDDD.Application.UseCases.Return_Vehicle;
using HexagonalDDD.Domain.Aggregates.Customer;
using HexagonalDDD.Domain.Aggregates.Vehicle;
using System;
using System.Threading.Tasks;
using System.Windows;
namespace WPF_Hexagonal_DDD
{
    public partial class MainWindow : Window
    {
        private readonly CreateVehicleHandler _createVehicleHandler;
        private readonly GetAvailableVehiclesHandler _getAvailableVehiclesHandler;
        private readonly GetCustomersHandler _getCustomersHandler;
        private readonly RentVehicleHandler _rentVehicleHandler;
        private readonly CreateCustomerHandler _createCustomerHandler;
        private readonly GetRentedVehiclesHandler _getRentedVehiclesHandler;
        private readonly ReturnVehicleHandler _returnVehicleHandler;

        public MainWindow(
            CreateVehicleHandler createVehicleHandler,
            GetAvailableVehiclesHandler getAvailableVehiclesHandler,
            GetCustomersHandler getCustomersHandler,
            RentVehicleHandler rentVehicleHandler,
            CreateCustomerHandler createCustomerHandler,
            GetRentedVehiclesHandler getRentedVehiclesHandler,
            ReturnVehicleHandler returnVehicleHandler)
        {
            InitializeComponent();

            _createVehicleHandler = createVehicleHandler;
            _getAvailableVehiclesHandler = getAvailableVehiclesHandler;
            _getCustomersHandler = getCustomersHandler;
            _rentVehicleHandler = rentVehicleHandler;
            _createCustomerHandler = createCustomerHandler;
            _getRentedVehiclesHandler = getRentedVehiclesHandler;

            Loaded += MainWindow_Loaded;
            _returnVehicleHandler = returnVehicleHandler;
        }
        private async void CreateVehicleButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            try
            {
                if (!ManufactureDatePicker.SelectedDate.HasValue)
                {
                    MessageBox.Show(
                        "Please select the manufacture date.");

                    return;
                }

                var command = new CreateVehicleCommand
                {
                    RegistrationNumber = RegistrationNumberTextBox.Text,
                    Brand = BrandTextBox.Text,
                    Model = ModelTextBox.Text,
                    ManufactureDate =
                        ManufactureDatePicker.SelectedDate.Value
                };

                await _createVehicleHandler.ExecuteAsync(command);

                MessageBox.Show(
                    "Vehicle created successfully.");

                await LoadAvailableVehiclesAsync();
                await LoadRentDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private async Task LoadAvailableVehiclesAsync()
        {
            var vehicles =
                await _getAvailableVehiclesHandler.ExecuteAsync();

            AvailableVehiclesDataGrid.ItemsSource = vehicles;
        }
        private async void MainWindow_Loaded(
            object sender,
            RoutedEventArgs e)
        {
            try
            {
                await LoadAvailableVehiclesAsync();
                await LoadRentDataAsync();
                await LoadRentedVehiclesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private async Task LoadRentDataAsync()
        {
            var customers =
                await _getCustomersHandler.ExecuteAsync();

            var vehicles =
                await _getAvailableVehiclesHandler.ExecuteAsync();

            CustomerComboBox.ItemsSource = customers;
            VehicleComboBox.ItemsSource = vehicles;
        }
        private async void CreateCustomerButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            try
            {
                var command = new CreateCustomerCommand
                {
                    Name = CustomerNameTextBox.Text
                };

                await _createCustomerHandler.ExecuteAsync(command);

                MessageBox.Show(
                    "Customer created successfully.");

                CustomerNameTextBox.Clear();

                await LoadRentDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private async void RentVehicleButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            try
            {
                var customer =
                    CustomerComboBox.SelectedItem as CustomerAggregate;

                var vehicle =
                    VehicleComboBox.SelectedItem as VehicleAggregate;

                if (customer == null)
                {
                    MessageBox.Show(
                        "Please select a customer.");

                    return;
                }

                if (vehicle == null)
                {
                    MessageBox.Show(
                        "Please select a vehicle.");

                    return;
                }

                var command = new RentVehicleCommand
                {
                    CustomerId = customer.Id,
                    VehicleId = vehicle.Id
                };

                await _rentVehicleHandler.ExecuteAsync(command);

                MessageBox.Show(
                    "Vehicle rented successfully.");

                await LoadAvailableVehiclesAsync();
                await LoadRentDataAsync();
                await LoadRentedVehiclesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private async Task LoadRentedVehiclesAsync()
        {
            var vehicles =
                await _getRentedVehiclesHandler.ExecuteAsync();

            RentedVehicleComboBox.ItemsSource = vehicles;
        }
        private async void ReturnVehicleButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            try
            {
                var vehicle =
                    RentedVehicleComboBox.SelectedItem as VehicleAggregate;

                if (vehicle == null)
                {
                    MessageBox.Show(
                        "Please select a vehicle.");

                    return;
                }

                var command = new ReturnVehicleCommand
                {
                    VehicleId = vehicle.Id
                };

                await _returnVehicleHandler.ExecuteAsync(command);

                MessageBox.Show(
                    "Vehicle returned successfully.");

                await LoadAvailableVehiclesAsync();
                await LoadRentDataAsync();
                await LoadRentedVehiclesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}