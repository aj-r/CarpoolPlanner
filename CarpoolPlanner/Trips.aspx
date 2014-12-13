<%@ Page Title="Trips" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Trips.aspx.cs" Inherits="CarpoolPlanner.Trips" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="row">
        <div class="col-md-12">
            <h2>Available Carpools</h2>
            <div>Select the days you plan on coming. You will receive <a href="../Notifications.aspx" target="_blank" title="How do notifications work?">notifications</a> only on the days that you select.</div>
            <asp:UpdatePanel UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnCreateTrip" />
                    <asp:AsyncPostBackTrigger ControlID="btnUpdate" />
                </Triggers>
                <ContentTemplate>
                    <asp:Repeater ID="repTrips" runat="server" OnItemDataBound="repTrips_ItemDataBound">
                        <ItemTemplate>
                            <div class="repeater-item">
                                <asp:HiddenField ID="hdnTripId" runat="server" />
                                <h3 class="editable-region-header">
                                    <label>
                                        <asp:CheckBox ID="chkAttending" CssClass="checkbox chk-attending" onchange="chkAttending_changed(this);" runat="server" />
                                        <%# Eval("Name") %>
                                    </label>
                                </h3>
                                <c:AdminPanel ID="apEdit" runat="server">
                                    <ContentTemplate>
                                        <div class="edit-buttons">
                                            <asp:LinkButton ID="btnEditTripRecurrence" runat="server">Edit</asp:LinkButton>
                                            &nbsp;&nbsp;
                                            <asp:LinkButton ID="btnDeleteTripRecurrence" runat="server">Delete</asp:LinkButton>
                                        </div>
                                    </ContentTemplate>
                                </c:AdminPanel>
                                <div class="indent">
                                    <asp:Repeater ID="repTripRecurrences" OnItemDataBound="repTripRecurrences_ItemDataBound" runat="server">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdnTripRecurrenceId" runat="server" />
                                            <div>
                                                <label>
                                                    <asp:CheckBox ID="chkAttendingRecurrence" CssClass="checkbox chk-attending-recurrence" onchange="chkAttendingRecurrence_changed(this);" runat="server" />
                                                    <asp:Label ID="lblRecurrence" runat="server" />
                                                </label>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Label ID="lblEmptyTrips" Visible="false" runat="server">There are currently no trips. An administrator can create new trips.</asp:Label>
                    <p>
                        <span id="info-message"></span>
                        <span id="success-message" class="text-success"><%: SuccessMessage %></span>
                        &nbsp;
                    </p>
                </ContentTemplate>
            </asp:UpdatePanel>
            <input id="btnUpdate" type="button" value="Save" class="btn btn-default" onclick="updateAttendance();" runat="server" />
            
            <c:AdminPanel runat="server">
                <ContentTemplate>
                    <h2 style="margin-top: 80px;">Create New Trip</h2>
                    <asp:UpdatePanel UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server">
                        <ContentTemplate>
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <span class="control-label col-md-2">Name</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtNewTripName" CssClass="form-control" runat="server" required="required" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <span class="control-label col-md-2">Meeting Location</span>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtLocation" CssClass="form-control" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="panel">
                                <h3>Recurrence:</h3>
                                <asp:Repeater ID="repNewTripRecurrences" OnItemDataBound="repNewTripRecurrences_ItemDataBound" runat="server">
                                    <ItemTemplate>
                                        <div class="form-horizontal">
                                            <div><asp:LinkButton ID="btnDeleteTripRecurrence" runat="server">Delete</asp:LinkButton></div>
                                            <div class="form-group">
                                                <span class="control-label col-md-2">Every</span>
                                                <div class="col-md-2">
                                                    <c:NumericUpDown ID="nudEvery" Minimum="1" CssClass="form-control" runat="server" required="required" />
                                                </div>
                                                <div class="col-md-8">
                                                    <asp:DropDownList ID="ddlRecurrenceType" CssClass="form-control" runat="server">
                                                        <asp:ListItem Value="Daily">Days</asp:ListItem>
                                                        <asp:ListItem Value="Weekly">Weeks</asp:ListItem>
                                                        <asp:ListItem Value="Mothly">Months</asp:ListItem>
                                                        <asp:ListItem Value="MothlyByDayOfWeek">Months (by day of week)</asp:ListItem>
                                                        <asp:ListItem Value="Yearly">Years</asp:ListItem>
                                                        <asp:ListItem Value="YearlyByDayOfWeek">Years (by day of week)</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <span class="control-label col-md-2">Starting on</span>
                                                <div class="col-md-10">
                                                    <c:DateTimePicker ID="dtpStartDate" IsRequired="true" runat="server" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <span class="control-label col-md-2">Ending on</span>
                                                <div class="col-md-10">
                                                    <c:DateTimePicker ID="dtpEndDate" IsTimeVisible="false" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                    <SeparatorTemplate>
                                        <hr />
                                    </SeparatorTemplate>
                                </asp:Repeater>
                                <asp:LinkButton ID="btnAddTripRecurrence" OnClick="btnAddTripRecurrence_Click" runat="server">Add</asp:LinkButton>
                            </div>
                            <asp:Button ID="btnCreateTrip" Text="Create Trip" OnClick="btnCreateTrip_Click" ValidationGroup="NewTrip" CssClass="btn btn-default" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </c:AdminPanel>
        </div>
    </div>

    <script type="text/javascript">
        function chkAttending_changed(sender) {
            //$("#info-message").text("Saving...");
            //$("#success-message").text("");
            var checked = $(sender).prop("checked") || $(sender).find("input").prop("checked");
            $(sender).closest(".repeater-item").find(".indent input").prop("checked", checked);
            //__doPostBack("<%: repTrips.UniqueID %>", "");
        }
        function chkAttendingRecurrence_changed(sender) {
            //$("#info-message").text("Saving...");
            //$("#success-message").text("");
            var checked = $(sender).prop("checked") || $(sender).find("input").prop("checked");
            if (checked) {
                $(sender).closest(".repeater-item").find(".editable-region-header input").prop("checked", true);
            }
            //__doPostBack("<%: repTrips.UniqueID %>", "");
        }
        function updateAttendance() {
            $("#info-message").text("Saving...");
            $("#success-message").text("");
            $("#error-message").text("");
            __doPostBack("<%: btnUpdate.UniqueID %>", "");
        };
        $(document).ready(function() {
            $("form").validate({ onsubmit: false });
            $("#<%: btnCreateTrip.ClientID %>").click(function(evt) {
                var isValid = $("form").valid();
                if (!isValid)
                    evt.preventDefault();
            });
        });
    </script>
</asp:Content>
