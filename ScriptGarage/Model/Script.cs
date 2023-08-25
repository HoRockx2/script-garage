using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptGarage.Model
{
    public class Script
    {
        // has Title, Cmd, Author, Description, SdbNeeded
        public string Title { get; set; }
        public string Cmd { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public bool SdbNeeded { get; set; }

    }
}
