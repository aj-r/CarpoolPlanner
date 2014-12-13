<%@ Page Title="Users" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="CarpoolPlanner.Users" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Users</h2>
    <asp:Repeater ID="repUsers" OnItemDataBound="repUsers_ItemDataBound" runat="server">
        <HeaderTemplate>
            <div class="row light-separator">
                <c:AdminPanel IsAdmin="true" runat="server">
                    <ContentTemplate>
                        <div class="col-md-1"><h4>User ID</h4></div>
                    </ContentTemplate>
                </c:AdminPanel>
                <div class="col-md-2"><h4>Name</h4></div>
                <div class="col-md-2"><h4>Phone</h4></div>
                <div class="col-md-2"><h4>E-mail</h4></div>
                <div class="col-md-2"><h4>Driver?</h4></div>
                <c:AdminPanel IsAdmin="true" runat="server">
                    <ContentTemplate>
                        <div class="col-md-2"><h4>Status</h4></div>
                        <div class="col-md-1"><h4>Admin</h4></div>
                    </ContentTemplate>
                </c:AdminPanel>
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <div class="row light-separator" style="padding: 10px 0px;">
                <c:AdminPanel IsAdmin="true" runat="server">
                    <ContentTemplate>
                        <div class="col-md-1 control-label user-id" style="margin-top:8px;"><b><%# Eval("Id") %></b></div>
                    </ContentTemplate>
                </c:AdminPanel>
                <div class="col-md-2" style="margin-top:8px;"><%# Eval("Name") %></div>
                <div class="col-md-2" style="margin-top:8px;"><%# (bool)Eval("PhoneVisible") || App.CurrentUser.IsAdmin ? Eval("Phone") : "" %></div>
                <div class="col-md-2" style="margin-top:8px;"><%# (bool)Eval("EmailVisible") || App.CurrentUser.IsAdmin ? Eval("Email") : "" %></div>
                <div class="col-md-2" style="margin-top:8px;"><asp:Label ID="lblDriver" runat="server"></asp:Label></div>
                <c:AdminPanel IsAdmin="true" runat="server">
                    <ContentTemplate>
                        <div class="col-md-2">
                            <asp:DropDownList ID="ddlStatus" CssClass="form-control" onchange="ddlStatus_changed(this)" runat="server">
                                <asp:ListItem Value="Active">Active</asp:ListItem>
                                <asp:ListItem Value="Unapproved">Unapproved</asp:ListItem>
                                <asp:ListItem Value="Disabled">Disabled</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-1">
                            <asp:CheckBox ID="chkIsAdmin" onchange="chkIsAdmin_changed(this)" CssClass="checkbox" runat="server" />
                            <span class="message"></span>
                        </div>
                    </ContentTemplate>
                </c:AdminPanel>
                <%-- TODO: Add functionality to delete users entirely (only enabled if user is disabled) --%>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <script type="text/javascript">
        <%-- TODO: use PageMethods instead of manual request --%>
        function ddlStatus_changed(sender) {
            var row = $(sender).closest(".row");
            var id = row.find(".user-id").text();
            var status = sender.value;
            var messageLabel = row.find(".message");
            messageLabel.text("Saving...");
            messageLabel.removeClass("text-success");
            messageLabel.removeClass("text-danger");
            $.ajax({
                type: "POST",
                url: "Users.aspx/UpdateUserStatus",
                data: "{'userId':'" + id + "','status':'" + status + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json"
            }).done(function() {
                messageLabel.text("Updated successfully");
                messageLabel.addClass("text-success");
            }).error(function(error) {
                messageLabel.text("Update failed: " + error.responseText);
                messageLabel.addClass("text-danger");
            });
        }
        function chkIsAdmin_changed(sender) {
            var row = $(sender).closest(".row");
            var id = row.find(".user-id").text();
            var isAdmin = $(sender).find("input").prop("checked");
            var messageLabel = row.find(".message");
            messageLabel.text("Saving...");
            messageLabel.removeClass("text-success");
            messageLabel.removeClass("text-danger");
            $.ajax({
                type: "POST",
                url: "Users.aspx/UpdateUserIsAdmin",
                data: "{'userId':'" + id + "','isAdmin':" + (isAdmin ? "true" : "false") + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json"
            }).done(function() {
                messageLabel.addClass("text-success");
                messageLabel.text("Updated successfully");
            }).error(function(error) {
                messageLabel.addClass("text-danger");
                messageLabel.text("Update failed: " + error.responseText);
            });
        }
    </script>
</asp:Content>
