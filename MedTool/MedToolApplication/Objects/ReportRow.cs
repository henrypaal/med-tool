using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedToolApplication.Objects
{
  public class ReportRow
  {
    public string AptCode { get; set; }
    public string MedName { get; set; }
    public string ShopName { get; set; }
    public string AtcCode { get; set; }
    public string Price { get; set; }
    public string LastUpdated { get; set; }
  }
}