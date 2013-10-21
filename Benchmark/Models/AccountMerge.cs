using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    class AccountMerge
    {
        public int? old_account_id { get; set; }
        public int? new_account_id { get; set; }
        public DateTime? merge_date { get; set; }
    }
}
