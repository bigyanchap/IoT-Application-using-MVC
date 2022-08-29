using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ioxdemo.Models
{
    public class ThingPinVM
    {
        public short? PinId { get; set; }
        public int ThingId { get; set; }
        public string ThingName { get; set; }
        public string SensorsInfo { get; set; }
        public string PinName { get; set; }
        public int PinTypeId { get; set; }
        public bool? OnOff { get; set; }
        public IEnumerable<Pin> Pins { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime LastModified { get; set; }
    }
}