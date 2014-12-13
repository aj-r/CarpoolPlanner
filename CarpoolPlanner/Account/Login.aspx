<%@ Page Title="Log in" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CarpoolPlanner.Account.Login" Async="true" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2>Log in</h2>
    <section id="loginForm">
        <div class="form-horizontal">
            <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                <p class="text-danger">
                    <asp:Literal runat="server" ID="FailureText" />
                </p>
            </asp:PlaceHolder>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="txtUserName" CssClass="col-md-2 control-label">User name</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox ID="txtUserName" CssClass="form-control" runat="server" required="required" />
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="txtPassword" CssClass="col-md-2 control-label">Password</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox ID="txtPassword" TextMode="Password" CssClass="form-control" runat="server" required="required" />
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-offset-2 col-md-10">
                    <asp:CheckBox runat="server" ID="RememberMe" Text="Remember me" CssClass="checkbox" />
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-offset-2 col-md-10">
                    <asp:Button runat="server" OnClick="LogIn" Text="Log in" CssClass="btn btn-default" />
                </div>
            </div>
            <div class="form-group">
            <div class="col-md-offset-2 col-md-10" style="margin-top:15px;">
                <asp:HyperLink runat="server" ID="RegisterHyperLink" ViewStateMode="Disabled">Register as a new user</asp:HyperLink>
            </div>
            </div>
        </div>
    </section>
    <script>
        $(document).ready(function() {
            $("form").validate();
        });
    </script>
</asp:Content>
