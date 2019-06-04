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
        private Session _session;
        private bool _isLoggedIn;


        public LoginViewModel(Session session)
        {
            _session = session;
            _session.LoggedIn += _session_LoggedIn;
            _session.LoggedOut += _session_LoggedOut;
        }

        // Событие сессии заставит представление обновиться
        private void _session_LoggedIn(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(IsLoggedIn));
        }

        private void _session_LoggedOut(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(IsLoggedIn));
        }

        public bool IsLoggedIn => _session.IsLoggedIn;

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

        private ICommand _loginCommand;
        public ICommand LogInCommand => _loginCommand ?? (_loginCommand = new RelayCommand(dummy => LogIn()));


        private ICommand _logoutCommand;
        public ICommand LogOutCommand => _logoutCommand ?? (_logoutCommand = new RelayCommand(dummy => LogOut()));

        

        private async void LogIn()
        {
            _session.Login(Login, Password);
        }

        private async void LogOut()
        {
            _session.Logout();
        }
    }
}
