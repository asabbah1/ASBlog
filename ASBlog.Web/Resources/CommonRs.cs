using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASBlog.Web.Resources
{
    public class GeneralRs
    {
        public int status { get; set; }
        public string errorMessage { get; set; }
    }
    public class ErrorRs
    {
        public string type { get; set; }
        public string title { get; set; }
        public int status { get; set; }
        public string detail { get; set; }
        public string traceId { get; set; }
    }
}
