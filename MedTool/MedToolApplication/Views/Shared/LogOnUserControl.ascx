<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%
  if (Request.IsAuthenticated)
  {
%>
        Sisse logitud: <strong><%: Page.User.Identity.Name%></strong>
        <%: Html.ActionLink("Logi välja", "LogOff", "Account")%>
<%
  }
  else
  {
%><br/>
<% } %>