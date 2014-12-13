<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="CarpoolPlanner.Account.Register" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2>Register</h2>
    <p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>

    <div class="form-horizontal">
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="txtUserId" CssClass="col-md-2 control-label">User ID*</asp:Label>
            <div class="col-md-10">
                <asp:TextBox ID="txtUserId" CssClass="form-control" runat="server" required="required" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Password" CssClass="col-md-2 control-label">Password*</asp:Label>
            <div class="col-md-10">
                <asp:TextBox ID="Password" TextMode="Password" CssClass="form-control" runat="server" required="required" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="ConfirmPassword" CssClass="col-md-2 control-label">Confirm password*</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="ConfirmPassword" TextMode="Password" CssClass="form-control" required="required" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="txtName" CssClass="col-md-2 control-label">Name</asp:Label>
            <div class="col-md-10">
                <asp:TextBox ID="txtName" CssClass="form-control" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="ConfirmPassword" CssClass="col-md-2 control-label">Commuting</asp:Label>
            <div class="col-md-10">
                <asp:RadioButtonList ID="rblCommuteMethod" CssClass="radio" runat="server">
                    <asp:ListItem Value="NeedRide" Selected="True">I will need a ride</asp:ListItem>
                    <asp:ListItem Value="Driver">I am a driver</asp:ListItem>
                    <asp:ListItem Value="HaveRide">I will have my own ride</asp:ListItem>
                </asp:RadioButtonList>
                <div style="margin-top:8px;">
                    <asp:CheckBox ID="chkCanDriveIfNeeded" Text="I am willing to drive on days we need more drivers" CssClass="checkbox" runat="server" />
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-2 control-label">
                <asp:Label AssociatedControlID="nudSeats" runat="server">Number of seats</asp:Label>
                <br />
                <span class="small">(inluding your own seat)</span>
            </div>
            <div class="col-md-10">
                <c:NumericUpDown ID="nudSeats" Value="5" Minimum="1" Width="70px" CssClass="form-control" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="txtEmail" CssClass="col-md-2 control-label">E-mail</asp:Label>
            <div class="col-md-10">
                <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server" />
                <asp:CheckBox ID="chkEmailNotify" Text="Send notifications via e-mail (not yet implemented)" CssClass="checkbox" Enabled="false" runat="server" />
                <asp:CheckBox ID="chkEmailVisible" Text="Visible to other users" Checked="true" CssClass="checkbox" Style="margin-bottom: 10px;" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="txtPhone" CssClass="col-md-2 control-label">Phone</asp:Label>
            <div class="col-md-10">
                <asp:TextBox ID="txtPhone" CssClass="form-control" runat="server" />
                <asp:CheckBox ID="chkPhoneNotify" Text="Send notifications via SMS" Checked="true" CssClass="checkbox checkbox-inline" runat="server" />&nbsp;&nbsp;(<a href="../Notifications.aspx" target="_blank" title="How do notifications work?">?</a>)
                <asp:CheckBox ID="chkPhoneVisible" Text="Visible to other users" Checked="true" CssClass="checkbox" Style="margin-bottom: 10px;" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button runat="server" OnClick="CreateUser_Click" Text="Register" CssClass="btn btn-default" />
            </div>
        </div>
        <asp:ValidationSummary runat="server" CssClass="text-danger" />
    </div>
    <script>
        $(document).ready(function() {
            $('#<%: ConfirmPassword.ClientID %>').attr('equalto', '#<%: Password.ClientID %>');
            $('form').validate();
            $('form').submit(function(e) {
                var phoneNotifications = $("#<%: chkPhoneNotify.ClientID %>").prop('checked') && $("#<%: txtPhone.ClientID %>").val() != '';
                var emailNotifications = $("#<%: chkEmailNotify.ClientID %>").prop('checked') && $("#<%: txtEmail.ClientID %>").val() != '';
                if (!phoneNotifications && !emailNotifications) {
                    if (!confirm("You have opted out of all notifications. This means that you must manually log in to the website for every single carpool to confirm that you are coming. Are you sure you want to do this?")) {
                        e.preventDefault();
                    }
                }
            });
        });
        $('#<%: chkCanDriveIfNeeded.ClientID %>, [id^=<%: rblCommuteMethod.ClientID %>_]').change(function() {
            setSeatsEnabled();
        });
        function setSeatsEnabled() {
            var isDriver = $('#<%: rblCommuteMethod.ClientID %>_1').prop('checked');
            var chkCanDriveIfNeeded = $("#<%: chkCanDriveIfNeeded.ClientID %>");
            var disabled = chkCanDriveIfNeeded.prop('disabled');
            if (disabled != isDriver) {
                chkCanDriveIfNeeded.prop('disabled', isDriver);
                chkCanDriveIfNeeded.prop('checked', isDriver);
            }
            var canDrive = chkCanDriveIfNeeded.prop('checked');
            $('#<%: nudSeats.ClientID %>').disableAndClear(!isDriver && !canDrive);
        }
        setSeatsEnabled();
    </script>
</asp:Content>
