using ioxdemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ioxdemo.Controllers
{
    public class PopulateThingController : Controller
    {
        IoXDBContext db = new IoXDBContext();
        
        public ActionResult Index()
        {
            List<ThingPinVM> thingPinVMlist = (
                from t in db.Things
                where t.IsDeleted==false
                orderby t.ThingName/*orderby t.LastModified didn't work as expected. Fix it.*/
                select new ThingPinVM
                {
                    ThingId = t.ThingId,
                    ThingName = t.ThingName,
                    SensorsInfo = t.SensorsInfo,
                    Pins = db.Pins.Where(p => p.ThingId == t.ThingId).ToList(),
                    IsDeleted = t.IsDeleted
                }).ToList();
            return View(thingPinVMlist);
        }

        

        [HttpPost]
        public void PutBinaryDynamicPin(int PinId)
        {
            Pin pin = db.Pins.FirstOrDefault(p => p.PinId == PinId);
            pin.OnOff = !pin.OnOff;
            db.SaveChanges();

            Thing thing = db.Things.FirstOrDefault(t=>t.ThingId==pin.ThingId);
            thing.LastModified = DateTime.Now;
            db.SaveChanges();
        }
        [HttpPost]
        public void PutMagnitudeDynamicPin(int PinId,int Magnitude)
        {
            Pin pin = db.Pins.FirstOrDefault(p => p.PinId == PinId);
            pin.Magnitude = Magnitude;
            if (pin.Magnitude == 0) pin.OnOff = false;
            else if (pin.Magnitude == 1023) pin.OnOff = true;
            else pin.OnOff = null;
            if (pin.OnFrom != null || pin.OnTill != null)
            {
                pin.OnFrom = null;
                pin.OnTill = null;
            }
            db.SaveChanges();

            Thing thing = db.Things.FirstOrDefault(t => t.ThingId == pin.ThingId);
            thing.LastModified = DateTime.Now;
            db.SaveChanges();
        }
        [HttpPost]
        public StepSignalInfoVM PutDateTimeDetailPin(int PinId, string OnFrom, string OnTill)
        {
            try
            {
                Pin pin = db.Pins.FirstOrDefault(p => p.PinId == PinId);
                

                DateTime pinOnFrom;
                DateTime pinOnTill;
                bool isPinOnFrom = DateTime.TryParse(OnFrom, out pinOnFrom);
                bool isPinOnTill = DateTime.TryParse(OnTill, out pinOnTill);
                /*validation*/
                if (!isPinOnFrom || !isPinOnTill)
                {
                    ViewBag.DateTimeFormatError = "You entered incorrect date-time format.";
                }
                else
                {
                    ViewBag.DateTimeFormatError = "";/* TODO: consume this on frontend.*/
                }

                pin.OnFrom = pinOnFrom;
                pin.OnTill = pinOnTill;
                if (DateTime.Now < pin.OnFrom && DateTime.Now <= pin.OnTill)
                {
                    pin.OnOff = false;
                }
                else if (pin.OnFrom < DateTime.Now && DateTime.Now < pin.OnTill)
                {
                    pin.OnOff = true;
                }
                else
                {
                    pin.OnOff = false;
                }
                if (pin.Magnitude != null) pin.Magnitude = null;
                db.SaveChanges();

                Thing thing = db.Things.FirstOrDefault(t => t.ThingId == pin.ThingId);
                thing.LastModified = DateTime.Now;
                db.SaveChanges();
                /*post back step signal info: single*/
                StepSignalInfoVM info = new StepSignalInfoVM();
                info.PinId = pin.PinId;
                info.OnFrom = pin.OnFrom;
                info.OnTill = pin.OnTill;
                return info;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
        [HttpPost]
        public StepSignalInfoVM PutDateTimeQuickPin(int PinId, string AddMinutes)
        {

            Pin pin = db.Pins.FirstOrDefault(p => p.PinId == PinId);
            pin.OnFrom = DateTime.Now;
            pin.OnTill = DateTime.Now.AddMinutes(Int32.Parse(AddMinutes));
            if (DateTime.Now < pin.OnFrom && DateTime.Now <= pin.OnTill)
            {
                pin.OnOff = false;
            }
            else if (pin.OnFrom < DateTime.Now && DateTime.Now < pin.OnTill)
            {
                pin.OnOff = true;
            }
            else
            {
                pin.OnOff = false;
            }
            if (pin.Magnitude != null) pin.Magnitude = null;
            db.SaveChanges();

            Thing thing = db.Things.FirstOrDefault(t => t.ThingId == pin.ThingId);
            thing.LastModified = DateTime.Now;
            db.SaveChanges();
            /*post back step signal info: single*/
            StepSignalInfoVM info = new StepSignalInfoVM();
            info.PinId = pin.PinId;
            info.OnFrom = pin.OnFrom;
            info.OnTill = pin.OnTill;
            return info;
        }
        
        [HttpPost]
        public void DeleteThing(int ThingId)
        {
            Thing thing = db.Things.FirstOrDefault(t => t.ThingId == ThingId);
            thing.IsDeleted = true;
            db.SaveChanges();
        }
        [HttpPost]
        public void PutDateTimeTerminatePin(int PinId)
        {
            Pin pin = db.Pins.FirstOrDefault(p => p.PinId == PinId);
            pin.OnFrom = DateTime.Now;
            pin.OnTill = DateTime.Now;
            pin.OnOff = false;
            db.SaveChanges();

            Thing thing = db.Things.FirstOrDefault(t => t.ThingId == pin.ThingId);
            thing.LastModified = DateTime.Now;
            db.SaveChanges();
        }

        /*Return Step Signal Infos as Json Array: All*/
        public object GetStepSignalInfos()
        {
            List<StepSignalInfoVM> pinStepSignalInfos =
                (from p in db.Pins
                 where p.PinTypeId == 3
                 orderby p.PinId
                 select new StepSignalInfoVM
                 {
                     PinId = p.PinId,
                     OnFrom = p.OnFrom,
                     OnTill = p.OnTill
                 }).ToList();
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(pinStepSignalInfos);
        }

        //BELOW THIS ALL METHODS ARE DELETABLE ANY TIME SOON.
        public ActionResult GetAllPinsForAThing(int ThingId)
        {
            List<Pin> pins = db.Pins.ToList();

            List<ThingPinVM> thingPinVM = pins.Select(
                    x => new ThingPinVM
                    {
                        PinId = x.PinId,
                        PinName = x.PinName,
                        PinTypeId = x.PinTypeId,
                        OnOff = x.OnOff,
                        ThingId = x.Thing.ThingId,
                        ThingName = x.Thing.ThingName,
                        SensorsInfo = x.Thing.SensorsInfo
                    }
                )
                .Where(p => p.ThingId == ThingId)
                .ToList();
            return View(thingPinVM);
            //return View("Test", thingPinVM);
        }


        public IEnumerable<ThingPinVM> GetPinsForOneThing(int ThingId)
        {
            List<Pin> pins = db.Pins.ToList();

            List<ThingPinVM> thingPinVMlist = pins.Select(
                    x => new ThingPinVM
                    {
                        PinId = x.PinId,
                        PinName = x.PinName,
                        PinTypeId = x.PinTypeId,
                        OnOff = x.OnOff
                    }
                )
                .Where(p => p.ThingId == ThingId)
                .ToList();
            return thingPinVMlist;
        }
        [HttpGet]
        public IEnumerable<ThingPinVM> GetPins()
        {
            List<Pin> pins = db.Pins.ToList();
            List<ThingPinVM> thingPinVMList = pins.Select(
                    p => new ThingPinVM
                    {
                        ThingId = p.ThingId,
                        PinId = p.PinId,
                        PinName = p.PinName,
                        PinTypeId = p.PinTypeId,
                        OnOff = p.OnOff
                    }
                ).
                ToList();
            return thingPinVMList;
        }
    }

}