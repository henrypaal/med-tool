using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Helmes.ExcelLib;
using MedToolApplication.Models;
using MedToolApplication.Objects;
using Services;
using Services.Classes;

namespace MedToolApplication.Controllers
{
  public class HomeController : Controller
  {
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

    [Authorize]
    public ActionResult Index()
    {
      ViewBag.Message = "Südameapteegi ravimiinfo baas";
      MedFilterModel model=new MedFilterModel();
      GetMedFilterModelWithPrices(model,false);
      GetKeyValueDates(model);
      return View(model);
    }

    [Authorize]
    [HttpPost]
    public ActionResult SearchPrices(MedFilterModel model)
    {
      bool isExcelOutput = model.Excel!=null && model.Excel=="Koosta Excel";
      GetMedFilterModelWithPrices(model, isExcelOutput);
      if(isExcelOutput)
      {
        ExcelWorkbook excelWorkbook = GenerateExcel(model.ExcelReportRows, model.ActiveShopIdNameDictionary);
        byte[] excelInBytes = ConvertStringToByteArray(excelWorkbook.GetXML());
        return File(excelInBytes, "application/ms-excel", "Ravimiinfo_hinnakiri_" + DateTime.Now.ToString("yyyyMMdd")+".xls");
      }

      return PartialView("ReportPartial", model);
    }

    [HttpGet]
    public ActionResult GetShopSelectionList()
    {
      MedFilterModel model=new MedFilterModel();
      List<MedShop> shopList = Db.MedShopSet.Where(w=>!w.Deleted).ToList();

      model.ShopSelectionList =
        shopList.Select(s => new SelectionItem() {Id = s.Id, Name = s.Name, Code=s.Code, IsSelected = s.IsSelected}).OrderBy(o=>o.Name).ToList();
      return PartialView("ShopSelectionList", model);
    }

    [HttpPost]
    public ActionResult AddShopSelectionList(MedFilterModel model)
    {
      List<MedShop> shopList = Db.MedShopSet.Where(w => !w.Deleted).ToList();
      if(model.ShopSelectionList!=null && model.ShopSelectionList.Count>0)
      {
        List<Guid> selectedShops = model.ShopSelectionList.Where(w => w.IsSelected).Select(s => s.Id).ToList();
        shopList.ForEach(f=>f.IsSelected=selectedShops.Contains(f.Id));
        Db.SaveChanges();
      }
      return RedirectToAction("Index");
    }

    private void GetKeyValueDates(MedFilterModel model)
    {
      KeyValue kvLastUpdate = db.KeyValueSet.FirstOrDefault(w => w.Code == PricelistScanner.LastUpdateDateKey);
      if(kvLastUpdate!=null)
      {
        DateTime dt=DateTime.Now;
        if (DateTime.TryParse(kvLastUpdate.Value, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
        {
          model.LastScan = dt.ToString("dd MMMM yyyy HH:mm");
        }
      }
      KeyValue kvNextUpdate = db.KeyValueSet.FirstOrDefault(w => w.Code == PricelistScanner.NextRunDateKey);
      if(kvNextUpdate!=null && !string.IsNullOrEmpty(kvNextUpdate.Value))
      {
        model.NextScan = DateTime.Parse(kvNextUpdate.Value).ToString("dd MMMM yyyy HH:mm");
      }
    }

    private void GetMedFilterModelWithPrices(MedFilterModel model, bool isExcelOutput)
    {
      int maxResults = 1500;
      List<MedShop> activeShops = Db.MedShopSet.Where(w => w.IsSelected).ToList();
      List<Guid> activeShopsIds = activeShops.Select(s => s.Id).ToList();
      model.SelectedShopCount = activeShops.Count;

      IQueryable<MedPriceList> query = Db.MedPriceListSet.Where(w => !w.Deleted && activeShopsIds.Contains(w.MedShopId));
      
      if(!string.IsNullOrEmpty(model.SelectedAptCode))
      {
        query = query.Where(w => w.AptCode.ToLower().Contains(model.SelectedAptCode.ToLower()));
      }
      if(!string.IsNullOrEmpty(model.SelectedShopName))
      {
        query = query.Where(w => w.MedShop.Name.ToLower().Contains(model.SelectedShopName.ToLower()));
      }
      if(!string.IsNullOrEmpty(model.SelectedTreeElement))
      {
        query = query.Where(w => w.ATC.ToLower().Contains(model.SelectedTreeElement));
      }
      

      if (isExcelOutput)
      {
        model.ActiveShopIdNameDictionary = activeShops.OrderBy(o => o.Name).ToDictionary(k => k.Id, k => k.Name);
        model.ExcelReportRows = query.GroupBy(g => g.AptCode).OrderBy(o => o.Key).ToDictionary(k => k.Key,
                                                                                               k =>k.Select(s =>new ExcelReportRow()
                                                                                                   {
                                                                                                     AtcCode = s.ATC,
                                                                                                     MedName = s.Name,
                                                                                                     Price = s.Price,
                                                                                                     ShopId = s.MedShopId
                                                                                                   }).ToList());
                                                                                                         
        model.ResultCount = model.ExcelReportRows.Count;
      }
      else
      {
        List<MedPriceList> pricelist = query.OrderBy(o => o.AptCode).ThenBy(o => o.MedShop.Name).Take(maxResults).ToList();
        model.ReportRows = pricelist.Select(
          s =>
          new ReportRow()
            {
              AptCode = s.AptCode,
              MedName = s.Name,
              ShopName = s.MedShop.Name,
              AtcCode = s.ATC,
              Price = s.Price.ToString(),
              LastUpdated = s.MedShop.LastUpdated.ToShortDateString()
            }).ToList();
        model.ResultCount = model.ReportRows.Count;
      }
    }

    public ExcelWorkbook GenerateExcel(Dictionary<string, List<ExcelReportRow>> data, Dictionary<Guid, string> selectedShops)
    {
      ExcelWorkbook workbook = new ExcelWorkbook("Südameapteegi_ravimiinfo", "Südameapteegi ravimiinfo süsteem, fail on genereeritud " + DateTime.Now, "Südameapteek", false) { Styles = new List<ExcelStyle>(1) };
      workbook.Styles.Add(new ExcelStyle(false, "black", false) { ID = "reg" });
      workbook.Styles.Add(new ExcelStyle(true, "black", false) { ID = "bold" });
      workbook.Styles.Add(new ExcelStyle(true, "red", false) { ID = "warn" });

      workbook.Worksheet.Table = new Helmes.ExcelLib.ExcelTable();
      ExcelRow headerRow=new ExcelRow();
      
      //if (data != null && data.Count > ushort.MaxValue)
      //{
      //  ExcelRow warningRow = new ExcelRow();
      //  warningRow.Cells.Add(new ExcelCell("warn", "NB! Failis on kokku " + data.Count + " rida, mida on rohkem kui " + ushort.MaxValue + ", mida saab Excelis maksimaalselt näidata. Seetõttu on fail poolik ning tuleb kitsendada väljavõtte filtrit!!"));
      //  workbook.Worksheet.Table.Rows.Add(warningRow);
      //}

      headerRow.Cells.Add(new ExcelCell("bold", "Apt kood"));
      headerRow.Cells.Add(new ExcelCell("bold", "Ravimi nimetus"));
      headerRow.Cells.Add(new ExcelCell("bold", "ATC"));
      foreach (KeyValuePair<Guid,string> selectedShop in selectedShops)
      {
        headerRow.Cells.Add(new ExcelCell("bold", selectedShop.Value));
      }

      workbook.Worksheet.Table.Rows.Add(headerRow);
      foreach (KeyValuePair<string,List<ExcelReportRow>> reportRow in data)
      {
        ExcelRow dataRow = new ExcelRow();
        dataRow.Cells.Add(new ExcelCell("reg", reportRow.Key));
        if(reportRow.Value!=null)
        {
          dataRow.Cells.Add(new ExcelCell("reg", reportRow.Value.FirstOrDefault().MedName));
          dataRow.Cells.Add(new ExcelCell("reg", reportRow.Value.FirstOrDefault().AtcCode));
          foreach (KeyValuePair<Guid, string> selectedShop in selectedShops)
          {
            ExcelReportRow rpr = reportRow.Value.FirstOrDefault(w => w.ShopId == selectedShop.Key);
            if (rpr != null)
            {
              dataRow.Cells.Add(new ExcelCell("reg", rpr.Price));
            }else
            {
              dataRow.Cells.Add(new ExcelCell());
            }
              
          }
        }
        workbook.Worksheet.Table.Rows.Add(dataRow);
      }

      return workbook;
    }

    public static byte[] ConvertStringToByteArray(string input)
    {
      return Encoding.UTF8.GetBytes(input);
    }


  }
}
