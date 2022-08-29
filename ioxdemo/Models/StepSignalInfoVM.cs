using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ioxdemo.Models
{
    public class StepSignalInfoVM
    {
        public short PinId { get; set; }

        public DateTime? OnFrom { get; set; }

        public DateTime? OnTill { get; set; }
    }
}