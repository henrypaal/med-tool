<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<MedToolApplication.Models.LogOnModel>" %>

<asp:Content ID="loginTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Sisselogimine
</asp:Content>

<asp:Content ID="loginContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Logi sisse</h2>
    <div style="color:#696969;"> 
        Palun sisesta oma kasutajanimi ja parool. <%: Html.ActionLink("Registreeri", "Register") %>,kui sa seda veel teinud ei ole.
    </div><br/>

    <script src="<%: Url.Content("~/Scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js") %>" type="text/javascript"></script>

    <% using (Html.BeginForm()) { %>
        <%: Html.ValidationSummary(true, "Sisselogimine ebaõnnestus. Palun kontrolli, kas kasutajanimi ja parool on õigesti sisestatud!") %>
        <div>
            <fieldset>
                <legend>Kasutaja andmed</legend>
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.UserName) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.UserName) %>
                    <%: Html.ValidationMessageFor(m => m.UserName) %>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Password) %>
                </div>
                <div class="editor-field">
                    <%: Html.PasswordFor(m => m.Password) %>
                    <%: Html.ValidationMessageFor(m => m.Password) %>
                </div>
                
                <div class="editor-label">
                    <%: Html.CheckBoxFor(m => m.RememberMe) %>
                    <%: Html.LabelFor(m => m.RememberMe) %>
                </div>
                
                <p>
                    <input type="submit" value="Log On" />
                </p>
            </fieldset>
        </div>
    <% } %>
</asp:Content>
