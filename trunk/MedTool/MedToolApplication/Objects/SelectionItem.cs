using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedToolApplication.Objects
{
  public class SelectionItem
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public bool IsSelected { get; set; }
  }
}