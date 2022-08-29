using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ioxdemo.Models
{
    public class ThingPinVM2
    {
        public int ThingId { get; set; }
        public string ThingName { get; set; }
        public IEnumerable<Pin> Pins { get; set; }
    }
}