using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Models
{
    class TagWiki
    {
        public string tag_name { get; set; }
        public string body { get; set; }
        public string excerpt { get; set; }
        public DateTime? body_last_edit_date { get; set; }
        public DateTime? excerpt_last_edit_date { get; set; }
        public ShallowUser last_body_editor { get; set; }
        public ShallowUser last_excerpt_editor { get; set; }
    }
}
