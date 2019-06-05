using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PharmaChain
{
    public class RequestViewModel : ViewModelBase
    {
        private Session _session;
        public RequestViewModel(Session session)
        {
            FromDate = DateTime.Today;
            ToDate = DateTime.Today;
            _session = session;
        }

        public string Name { get; set; }
        public int Portion { get; set; }
        public string Company { get; set; }
        public string Seller { get; set; }
        public int Amount { get; set; }
        public DateTime FromDate { get; set; } 
        public DateTime ToDate { get; set; }

        private ICommand _sendRequestCommand;
        public ICommand SendRequestCommand
        {
            get { return _sendRequestCommand ?? (_sendRequestCommand = new RelayCommand(param => Generate())); }
        }

        

        #region методы

        [DataContract]
        class GenerateResponse
        {
            [DataMember]
            public string Hash;
        }

        private async void Generate()
        {
            string uriData = $"%7B%22name%22%3A%22{Name}%22%2C%22portion%22%3A%22{Portion}%22%2C%22company%22%3A%22{Company}%22%2C%22seller%22%3A%22{Seller}%22%2C%22from_date%22%3A%22{FromDate.ToString()}%22%2C%22to_date%22%3A%22{ToDate.ToString()}%22%7D";
            Dictionary<string, string> args = new Dictionary<string, string>();
            args.Add("access_token", _session.AccessToken);
            args.Add("data", uriData);
            GenerateResponse res = await _session.GetJson<GenerateResponse>("generate", args);
            if(res.Hash != string.Empty)
            {
                IronBarCode.BarcodeWriter.CreateBarcode(res.Hash, IronBarCode.BarcodeWriterEncoding.QRCode).SaveAsPng(res.Hash + ".png");
                MessageBox.Show("Операция выполнена успешно", "PharmaChain", MessageBoxButton.OK);
            }
            else
            {
                MessageBox.Show("Ошибка токена", "PharmChain", MessageBoxButton.OK);
            }
        }
        #endregion

    }
}
