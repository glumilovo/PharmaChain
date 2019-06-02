using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaChain
{
    public class RequestViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string Portion { get; set; }
        public string Company { get; set; }
        public string Seller { get; set; }
        public string Amount { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
