using Newtonsoft.Json;
using PharmaChain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PharmaChain
{
    // Класс для работы с соединением и запросами
    public class Session
    {
        private HttpClient _client;
        private string _accessToken;
        private string _address;
        private int _port;

        public Session(string address, int port)
        {
            _address = address;
            _port = port;
            _client = new HttpClient();
            TestConnection();
        }

        public User User { get; } = new User();
        public bool IsLoggedIn { get; set; }
        public string AccessToken => _accessToken;

        public async Task<T> GetJson<T>(string service, Dictionary<string, string> args) where T:class
        {
            string response = await GetResponse(service, args);
            return JsonConvert.DeserializeObject<T>(response);
        }

        public async Task<string> GetResponse(string service, Dictionary<string, string> args)
        {
            try
            {
                string requestUri = $"http://{_address}:{_port}/{service}";

                if (args != null)
                {
                    requestUri += "?";
                    foreach (KeyValuePair<string, string> arg in args)
                    {
                        requestUri += arg.Key + "=" + arg.Value + "&";
                    }
                }

                HttpResponseMessage response = await _client.GetAsync(requestUri);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "PharmaChain", MessageBoxButton.OK);
                return null;
            }
        }

        [DataContract]
        class TokenResponse
        {
            [DataMember]
            public bool Success;
            [DataMember]
            public string AccessToken;
        }

        public async void Login(string login, string password)
        {
            Dictionary<string, string> args = new Dictionary<string, string>();
            args.Add("login", login);
            args.Add("password", password);
            TokenResponse res = await GetJson<TokenResponse>("login", args);
            if (res.Success)
            {
                _accessToken = res.AccessToken;
                User.Login = login;
                IsLoggedIn = true;
                LoggedIn?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                MessageBox.Show("Неправильный логин или пароль!", "PharmaChain", MessageBoxButton.OK);
            }
        }

        public async void Logout()
        {
            Dictionary<string, string> args = new Dictionary<string, string>();
            args.Add("access_token", _accessToken);
            await GetResponse("logout", args);
            IsLoggedIn = false;
            LoggedOut?.Invoke(this, EventArgs.Empty);
        }

        public async void TestConnection()
        {
            await GetResponse("test", null);
        }

        public event EventHandler LoggedIn;
        public event EventHandler LoggedOut;
    }
}
