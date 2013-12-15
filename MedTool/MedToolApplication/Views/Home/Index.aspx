<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<MedToolApplication.Models.MedFilterModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Ravimiinfo baas
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <script type="text/javascript">
    $(document).ready(function () {
      bindAutoCompleteEvents();
    });

    function bindAutoCompleteEvents() {
      $("#SelectedTreeElement").autocomplete({
        source: '<%=Url.Action("GetActiveTreeElement","Json") %>',
        minLength: 1
      });
    }
    
    function showShopSelectionControl() {
      showEditDialog('<%:Url.Action("GetShopSelectionList", "Home") %>', 'Apteekide valik', 500, 400, {});
    }
    
    function showEditDialog(url, title, height, width, params) {
      if (params == null) {
        params = {};
      }
      $.ajax({
        url: url,
        data: params,
        success: function (data) {
          $('#itemEditDialog').html("");
          $('#itemEditDialog').dialog({ modal: true, title: title, height: height, width: width, resizable: false });
          $('#itemEditDialog').html(data);
        },
        cache: false
      });
    }

    function closeEditForm() {
      $("#itemEditDialog").dialog('close');
    }
    
    function findProducts() {
      var form = $('#searchForm');
      $.ajax({
        url: form.attr('action'),
        type: form.attr('method'),
        data: form.serialize(),
        success: function (result) {
          $('#searchResult').html(result);
        },
        cache: false
      });
      return false;
    }

    function UpdateRunDate() {
      var confirm = window.confirm("Kas soovid andmeid uuendada täna öösel?");
      if(confirm==true) {
        $.ajax({
          url: '<%=Url.Action("UpdateNextScanDate","Json") %>',
          data: {},
          success: function (result) {
            $('#nextRunDiv').html(result);
          },
          cache: false
        });
        return false;
      }
    }
  </script>
    <h2><%: ViewBag.Message %></h2>
    <p>(Andmed viimati uuendatud: <%:Model.LastScan %> , järgmine andmete uuendamine: <span id="nextRunDiv"><%:Model.NextScan %></span>) <a href="javascript:{}" onclick="UpdateRunDate()">Uuenda andmeid täna</a></p>
      <%using (Html.BeginForm("SearchPrices", "Home", FormMethod.Post, new { id = "searchForm" }))
        {%>
        <table>
          <tr>
            <th colspan="7">Filtreerimise valikud</th>
          </tr>
          <tr>
            <td>Vali aktiivsed apteegid:</td>
            <td>
              <a href="javascript:{};" onclick="showShopSelectionControl();return false;">Vali apteegid</a>&nbsp;&nbsp;(<%:Model.SelectedShopCount %>)
            </td>
            <td>Vali aktiivne ATC puu element:</td>
            <td>
              <%: Html.TextBoxFor(m => m.SelectedTreeElement, new { @class = "ajax", style = "width:200px;" })%>
            </td>
            <td>
                Apt kood:
            </td>
            <td>
              <%: Html.TextBoxFor(m => m.SelectedAptCode, new{style="width:100px;"}) %>
            </td>
            <td>
              <input type="button" id="searchPrice" name="searchPrice" value="Otsi" onclick="findProducts()"/>
              <input type="submit" class="ui-button" id="generateExcel" name="Excel" value="Koosta Excel"/>
            </td>
          </tr>
        </table>
        <% } %>
        <br/>
        <div id="searchResult">
          <%Html.RenderPartial("ReportPartial", Model); %>
        </div>
        <div id="itemEditDialog"></div>
</asp:Content>
