using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ioxdemo.Models
{
    public class PinVM
    {
        public short? PinId { get; set; }
        public int ThingId { get; set; }
        public string PinName { get; set; }
        public string PinTypeId { get; set; }
        public bool? OnOff { get; set; }
        public int? Magnitude { get; set; }
        public DateTime? OnFrom { get; set; }
        public DateTime? OnTill { get; set; }
        public byte[] LastModified { get; set; }
    }
}