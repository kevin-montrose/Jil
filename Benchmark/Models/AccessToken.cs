using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    class AccessToken
    {
        public string access_token { get; set; }
        public DateTime? expires_on_date { get; set; }
        public int? account_id { get; set; }
        public List<string> scope { get; set; }
    }
}
