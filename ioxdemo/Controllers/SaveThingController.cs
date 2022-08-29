using ioxdemo.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ioxdemo.Controllers
{
    public class SaveThingController : Controller
    {
        public ActionResult Index()
        {
            IoXDBContext db = new IoXDBContext();
            List<PinType> pinTypeList = db.PinTypes.ToList();
            ViewBag.PinTypeList = new SelectList(pinTypeList, "PinTypeId", "PinTypeName");
            return View();
        }
        /*Separate Edit View for the sake of modularity, clarity, and reduced complexity*/
        [HttpGet]
        public ActionResult Edit(int id)
        {
            IoXDBContext db = new IoXDBContext();
            ThingPinVM2 tpvm = (
                from t in db.Things
                where t.ThingId == id
                select new ThingPinVM2
                {
                    ThingId = id,
                    ThingName = t.ThingName,
                    Pins = db.Pins.Where(p => p.ThingId == id).ToList()
                }).FirstOrDefault();
            
            return View(tpvm);
        }
        [HttpPost]
        public ActionResult SaveThingPin(string ThingName, string PinsJson, int? ThingId)
        {
            IoXDBContext db = new IoXDBContext();
            try
            {
                if (ThingId ==null)/*Create New Thing & Pins.*/
                {
                    Thing thing = new Thing();
                    thing.ThingName = ThingName;
                    thing.SensorsInfo = "";
                    thing.IsDeleted = false;
                    thing.LastModified = DateTime.Now;
                    db.Things.Add(thing);
                    db.SaveChanges();
                    int latestThingId = thing.ThingId;

                    List<PinVM2> pinlist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PinVM2>>(PinsJson);
                    foreach (var item in pinlist)
                    {
                        Pin pin = new Pin();
                        pin.PinName = item.PinName;
                        pin.ThingId = latestThingId;
                        pin.PinTypeId = item.PinTypeId;

                        if (pin.PinTypeId == 1) pin.OnOff = false;/*Set Initial values*/
                        else if (pin.PinTypeId == 2) pin.Magnitude = 0;
                        else if (pin.PinTypeId == 3)
                        {
                            pin.OnFrom = DateTime.Now.AddDays(-2);
                            pin.OnTill = DateTime.Now.AddDays(-1);
                        }
                        db.Pins.Add(pin);
                        db.SaveChanges();
                    }
                }
                else/*Edit Existing Thing and Pins*/
                {
                    Thing thing = db.Things.FirstOrDefault(t => t.ThingId == ThingId);
                    thing.ThingName = ThingName;
                    thing.LastModified = DateTime.Now;
                    db.SaveChanges();
                    int latestThingId = thing.ThingId;
                    List<PinVM2> pinlist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PinVM2>>(PinsJson);
                    foreach (var item in pinlist)
                    {
                        Pin pin = db.Pins.FirstOrDefault(p => p.PinId == item.PinId) ?? new Pin();/*create new instance if null: non-coalescing*/
                        if (pin.Equals(new Pin()))/*create pin*/
                        {
                            pin.PinName = item.PinName;
                            pin.ThingId = latestThingId;
                            pin.PinTypeId = item.PinTypeId;

                            if (pin.PinTypeId == 1) pin.OnOff = false;/*Set Initial values*/
                            else if (pin.PinTypeId == 2) pin.Magnitude = 0;
                            else if (pin.PinTypeId == 3)
                            {
                                pin.OnFrom = DateTime.Now.AddDays(-2);
                                pin.OnTill = DateTime.Now.AddDays(-1);
                            }

                            db.Pins.Add(pin);
                        }
                        else if (item.DeletePin)/*delete pin*/
                        {
                            db.Pins.Remove(pin);
                        }
                        else if(item.PinId>0)/*edit pin*/
                        {
                            pin.PinName = item.PinName;
                            pin.PinTypeId = item.PinTypeId;
                        }
                    }
                    db.SaveChanges();
                }
                return RedirectToAction("Index", "PopulateThing");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}