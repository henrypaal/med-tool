<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<MedToolApplication.Models.MedFilterModel>" %>
<%@ Import Namespace="MedToolApplication.Objects" %>
<%@ Import Namespace="UserServices.Helpers" %>
<script type="text/javascript">
  $(document).ready(function () {
    $('.itemSelection').click(function (event) {
      if (event.target.type !== 'checkbox') {
        $(':checkbox', this).trigger('click');
      }
    });
  });
</script>
<%using (Html.BeginForm("AddShopSelectionList", "Home", FormMethod.Post, new {id = "shopSelection"})){%>
  <div id = "mainSplitter" style = "height: 380px; overflow-y: auto; overflow-x: hidden;border-top: 1px solid #e8eef4;">
  <% int index = 0;%>
  <table id="shopListTable">
    <% foreach (SelectionItem medShop in Model.ShopSelectionList)
       {%>
      <tr class="itemSelection">
        <td>
          <%: Html.CheckBox(ElementIdGenerator.GetElementId("ShopSelectionList", "IsSelected", index),
                                      medShop.IsSelected) %>	
        </td>
        <td>
          <%: medShop.Name %> (<%:medShop.Code %>)							
          <% if (medShop.Id != Guid.Empty)
             { %>
            <%: Html.Hidden(ElementIdGenerator.GetElementId("ShopSelectionList", "Id", index), medShop.Id) %>
          <% } %>
          <%: Html.Hidden(ElementIdGenerator.GetElementId("ShopSelectionList", "Name", index),
                                    medShop.Name) %>
          <% index++; %>
        </td>
      </tr>
    <% } %>
  </table>
  </div>
  <div style="padding-top: 30px;">
    <input type="submit" name="save" id="save" value="Salvesta valik" onclick="closeEditForm()"/>
    <span style="padding-left: 15px;"><input type="button" name="close" id="close" value="Sulge aken" onclick="closeEditForm()"/></span>
  </div>
<% } %>