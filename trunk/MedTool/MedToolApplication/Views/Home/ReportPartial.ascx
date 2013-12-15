<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<MedToolApplication.Models.MedFilterModel>" %>
<%@ Import Namespace="MedToolApplication.Objects" %>
Tulemusi kokku: <%:Model.ResultCount %> &nbsp;&nbsp; Maksimaalne tulemuste arv 1500
<table style="width: 100%;">
  <tr>
    <th>Apt kood</th>
    <th>Ravimi nimetus</th>
    <th>Apteegi nimetus</th>
    <th>ATC</th>
    <th>Hind</th>
    <th>Viimati uuendatud</th>
  </tr>
  <% if(Model.ReportRows!=null && Model.ReportRows.Count>0){ %>
  <% foreach (ReportRow reportRow in Model.ReportRows)
     {%>
      <tr>
        <td><%:reportRow.AptCode %></td>
        <td><%:reportRow.MedName %></td>
        <td><%:reportRow.ShopName %></td>
        <td><%:reportRow.AtcCode %></td>
        <td><%:reportRow.Price %></td>
        <td><%:reportRow.LastUpdated %></td>
      </tr>
    <% } %>
  <% } %>
</table>