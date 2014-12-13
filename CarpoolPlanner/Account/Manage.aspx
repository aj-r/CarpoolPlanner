<%@ Page Title="Manage Account" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="CarpoolPlanner.Account.Manage" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h2>Manage Account</h2>
    <h3><%: App.CurrentUserId %></h3>

    <asp:UpdatePanel UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnUpdate" />
        </Triggers>
        <ContentTemplate>
            <div class="form-horizontal">
                <div class="form-group">
                    <asp:Label AssociatedControlID="txtName" CssClass="col-md-2 control-label" runat="server">Name</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtName" CssClass="form-control" runat="server" />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label AssociatedControlID="rblCommuteMethod" CssClass="col-md-2 control-label" runat="server">Commuting</asp:Label>
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
                        <c:NumericUpDown ID="nudSeats" Minimum="1" Width="70px" CssClass="form-control" runat="server" />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtEmail" CssClass="col-md-2 control-label">E-mail</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server" />
                        <asp:CheckBox ID="chkEmailNotify" Text="Send notifications via e-mail (not yet implemented)" CssClass="checkbox checkbox-inline" runat="server" />
                        <asp:CheckBox ID="chkEmailVisible" Text="Visible to other users" CssClass="checkbox" Style="margin-bottom: 10px;" runat="server" />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtPhone" CssClass="col-md-2 control-label">Phone</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtPhone" CssClass="form-control" runat="server" />
                        <asp:CheckBox ID="chkPhoneNotify" Text="Send notifications via SMS" CssClass="checkbox checkbox-inline" runat="server" />&nbsp;&nbsp;(<a href="../Notifications.aspx" target="_blank" title="How do notifications work?">?</a>)
                        <asp:CheckBox ID="chkPhoneVisible" Text="Visible to other users" CssClass="checkbox" Style="margin-bottom: 10px;" runat="server" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-offset-2 col-md-10">
                        <asp:Button ID="btnUpdate" Text="Update" OnClick="btnUpdate_Click" CssClass="btn btn-default" UseSubmitBehavior="false" runat="server" />
                    </div>
                </div>
            </div>
            <p>
                <span id="update-info-message"></span>
                <span id="update-success-message" class="text-success"><%: UpdateSuccessMessage %></span>
                <span id="update-error-message" class="text-danger"><%: UpdateErrorMessage %></span>
                &nbsp;
            </p>
            <script>
            </script>

        </ContentTemplate>
    </asp:UpdatePanel>
    <hr />
    <div class="form-horizontal">
        <h4>Change Password</h4>
        <asp:UpdatePanel UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnChangePassword" />
            </Triggers>
            <ContentTemplate>
                <div class="form-group">
                    <asp:Label runat="server" ID="CurrentPasswordLabel" AssociatedControlID="CurrentPassword" CssClass="col-md-2 control-label">Current password</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="CurrentPassword" TextMode="Password" CssClass="form-control" required="required" />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" ID="NewPasswordLabel" AssociatedControlID="NewPassword" CssClass="col-md-2 control-label">New password</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="NewPassword" TextMode="Password" CssClass="form-control" required="required" />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" ID="ConfirmNewPasswordLabel" AssociatedControlID="ConfirmNewPassword" CssClass="col-md-2 control-label">Confirm new password</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="ConfirmNewPassword" TextMode="Password" CssClass="form-control" required="required" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-offset-2 col-md-10">
                        <asp:Button ID="btnChangePassword" runat="server" Text="Change Password" OnClick="ChangePassword_Click" CssClass="btn btn-default" />
                    </div>
                </div>
                <p>
                    <span id="password-info-message"></span>
                    <span id="password-success-message" class="text-success"><%: ChangePasswordSuccessMessage %></span>
                    <span id="password-error-message" class="text-danger"><%: ChangePasswordErrorMessage %></span>
                    &nbsp;
                </p>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script>
        $(document).ready(function() {
            $('form').validate({ onsubmit: false });
        });
        // Run the following on page load as well as postbacks
        registerPageLoad(function() {
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
            $("#<%: btnChangePassword.ClientID %>").click(function(evt) {
                $('#password-info-message').text('Updating...');
                $('#password-success-message').text('');
                $('#password-error-message').text('');
                var isValid = $('form').valid();
                if (!isValid)
                    evt.preventDefault();
            });
            $('#<%: btnUpdate.ClientID %>').attr('onclick', '');
            $('#<%: btnUpdate.ClientID %>').click(function(e) {
                $('#update-info-message').text('');
                $('#update-success-message').text('');
                $('#update-error-message').text('');
                var phoneNotifications = $("#<%: chkPhoneNotify.ClientID %>").prop('checked') && $("#<%: txtPhone.ClientID %>").val() != '';
                var emailNotifications = $("#<%: chkEmailNotify.ClientID %>").prop('checked') && $("#<%: txtEmail.ClientID %>").val() != '';
                if (!phoneNotifications && !emailNotifications) {
                    if (!confirm("You have opted out of all notifications. This means that you must manually log in to the website for every single carpool to confirm that you are coming. Are you sure you want to do this?")) {
                        return;
                    }
                }
                $('#update-info-message').text('Updating...');
                __doPostBack('<%: btnUpdate.UniqueID %>', '');
            });
        });
    </script>
</asp:Content>
