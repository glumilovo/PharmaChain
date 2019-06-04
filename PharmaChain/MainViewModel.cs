using IronBarCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaChain
{
    public class MainViewModel : ViewModelBase
    {
        private LoginViewModel _loginVM;
        private RequestViewModel _requestVM;
        private Session _session;

        public MainViewModel()
        {
            // Теперь красивше
            _session = new Session("localhost", 8888);
            _session.LoggedIn += _session_LoggedIn;
            _session.LoggedOut += _session_LoggedOut;

            _loginVM = new LoginViewModel(_session);
            _requestVM = new RequestViewModel(_session);
        }

        private void _session_LoggedOut(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(IsLoggedIn));
        }

        private void _session_LoggedIn(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(IsLoggedIn));
        }

        public LoginViewModel LoginVM => _loginVM;
        public RequestViewModel RequestVM => _requestVM;

        public Session Session => _session;
        public bool IsLoggedIn => _session.IsLoggedIn;
    }
}
