using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedToolApplication.Objects
{
  public class ExcelReportRow
  {
    public string AptCode { get; set; }
    public string MedName { get; set; }
    public Guid ShopId { get; set; }
    public string AtcCode { get; set; }
    public decimal Price { get; set; }
  }
}