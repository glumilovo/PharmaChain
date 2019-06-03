using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace PharmaChain
{
    public class LoginViewModel : ViewModelBase
    {
        private string _login;
        private string _password;
        private bool _isLoggedIn;
        private HttpClient _client;
        private string _accessToken;

        public LoginViewModel()
        {
            _client = new HttpClient();
            TestConnection();
        }

        public string Login
        {
            get { return _login; }
            set
            {
                if(_login != value)
                {
                    _login = value;
                    OnPropertyChanged(nameof(Login));
                }
            }

        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }

        }

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set
            {
                if (_isLoggedIn != value)
                {
                    _isLoggedIn = value;
                    OnPropertyChanged(nameof(IsLoggedIn));
                }
            }
        }

        private ICommand _loginCommand;
        public ICommand LogInCommand => _loginCommand ?? (_loginCommand = new RelayCommand(dummy => LogIn()));


        private ICommand _logoutCommand;
        public ICommand LogOutCommand => _logoutCommand ?? (_logoutCommand = new RelayCommand(dummy => LogOut()));

        [DataContract]
        class TokenResponse
        {   
            [DataMember]
            public bool Success;
            [DataMember]
            public string AccessToken;
        }

        private async void LogIn()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync($"http://localhost:8888/login?login={Login}&password={Password}");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                TokenResponse tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseBody);
                if (tokenResponse.Success)
                {
                    _accessToken = tokenResponse.AccessToken;
                    IsLoggedIn = true;
                }
                else
                {
                    MessageBox.Show("Неправильный логин или пароль!", "PharmaChain", MessageBoxButton.OK);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "PharmaChain", MessageBoxButton.OK);
            }
        }

        private async void LogOut()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync($"http://localhost:8888/logout?access_token={_accessToken}");
                response.EnsureSuccessStatusCode();
                IsLoggedIn = false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "PharmaChain", MessageBoxButton.OK);
            }
        }

        private async void TestConnection()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("http://localhost:8888/test");
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
                MessageBox.Show(e.Message, "PharmaChain", MessageBoxButton.OK);
            }
        }
    }
}
