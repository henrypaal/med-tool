using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MedToolApplication.Objects;

namespace MedToolApplication.Models
{
  public class MedFilterModel
  {
    public string SelectedShopName { get; set; }
    public string SelectedTreeElement { get; set; }
    public string SelectedAptCode { get; set; }
    public string LastScan {get; set; }
    public string NextScan {get; set; }
    public string NextPlannedScan {get; set; }

    public int SelectedShopCount { get; set; }
    public Dictionary<Guid, string> ActiveShopIdNameDictionary { get; set; }  

    public List<SelectionItem> ShopSelectionList { get; set; } 

    public List<ReportRow> ReportRows { get; set; }
    public Dictionary<string, List<ExcelReportRow>> ExcelReportRows { get; set; }

    public int ResultCount { get; set; }
    public string Excel { get; set; }
  }
}