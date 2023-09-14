using FluentFTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTP_console.Index
{
    internal class IndexedFIles
    {
        public int index { get; set; }
        public FtpListItem item { get; set; }
        /*
        public IndexedFIles(int index, FtpListItem item) { 
            this.index = index;
            this.item = item;
        }
        */

        public string toString()
        {
            return Convert.ToString(index) + item.Name + "      " + item.Type;
        }

    }
}
