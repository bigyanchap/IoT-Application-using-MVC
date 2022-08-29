using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ioxdemo.Models
{
    public class PinVM2
    {
        public short? PinId { get; set; }
        public string PinName { get; set; }
        public int PinTypeId { get; set; }
        public bool DeletePin { get; set; }
    }
}