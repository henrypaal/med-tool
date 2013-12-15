using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Services;
using Services.Classes;

namespace MedToolApplication.Controllers
{
  public class JsonController : Controller
  {
    //
    // GET: /Json/

    private MedToolEntities db;

    public MedToolEntities Db
    {
      get
      {
        if (db == null)
        {
          db = new MedToolEntities();
        }
        return db;
      }
    }

    [HttpGet, ValidateInput(false)]
    public JsonResult GetShopNames(string term)
    {
        term= term ?? string.Empty;
        List<string> data = Db.MedShopSet.Where(w=>w.Name.ToLower().Contains(term) && w.IsSelected).OrderBy(o=>o.Name).Select(s=>s.Name).ToList();
        return Json(data, JsonRequestBehavior.AllowGet);
    }

    [HttpGet, ValidateInput(false)]
    public JsonResult GetActiveTreeElement(string term)
    {
      term = term ?? string.Empty;
      List<string> data = Db.AtcTreeThirdLevelSet.Where(w => w.IsActive && w.Key.ToLower().Contains(term)).OrderBy(o => o.Key).Select(s => s.Key).ToList();
      return Json(data, JsonRequestBehavior.AllowGet);
    }
    
    [HttpGet, ValidateInput(false)]
    public JsonResult UpdateNextScanDate()
    {
      KeyValue nextrun = Db.KeyValueSet.FirstOrDefault(f => f.Code == PricelistScanner.NextRunDateKey);
      PricelistScanner.UpdateNextRunTime(Db, nextrun, true);
      db.SaveChanges();
      string data = DateTime.Parse(nextrun.Value).ToString("dd MMMM yyyy HH:mm"); ;
      return Json(data, JsonRequestBehavior.AllowGet);
    }
  }
}