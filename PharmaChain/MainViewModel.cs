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

        public MainViewModel()
        {
            _loginVM = new LoginViewModel();
            _requestVM = new RequestViewModel();
        }

        public LoginViewModel LoginVM => _loginVM;
        public RequestViewModel RequestVM => _requestVM;
    }
}
