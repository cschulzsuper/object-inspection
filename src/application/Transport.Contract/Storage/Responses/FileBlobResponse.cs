using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Super.Paula.Application.Storage.Responses
{
    public class FileBlobResponse
    {
        public string BTag { get; set; } = string.Empty;

        public string Container { get; set; } = string.Empty;

        public string UniqueName { get; set; } = string.Empty;
    }
}
