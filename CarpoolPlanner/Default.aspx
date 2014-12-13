<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CarpoolPlanner._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <h2>Upcoming Carpools</h2>
            <c:UserStatusPanel ID="uspUnapproved" Status="Unapproved" runat="server">
                <ContentTemplate>
                    <div class="text-danger">You cannot see upcoming trips because your account has not yet been approved by an administrator.</div>
                </ContentTemplate>
            </c:UserStatusPanel>
            
            <c:UserStatusPanel ID="uspActive" Status="Active" runat="server">
                <ContentTemplate>
                    <div>Select the days that you can come this week.</div><%-- TODO: generalize to not use the word "week" --%>
                    <asp:UpdatePanel UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnUpdate" />
                        </Triggers>
                        <ContentTemplate>
                            <asp:Repeater ID="repTrips" runat="server" OnItemDataBound="repTrips_ItemDataBound">
                                <ItemTemplate>
                                    <div class="repeater-item">
                                        <asp:HiddenField ID="hdnTripId" runat="server" />
                                        <h3><%# Eval("Trip.Name") %></h3>
                                        <asp:Repeater ID="repTripInstances" OnItemDataBound="repTripInstances_ItemDataBound" runat="server">
                                            <ItemTemplate>
                                                <div style="padding: 5px 0px;">
                                                    <asp:HiddenField ID="hdnTripInstanceId" runat="server" />
                                                    <label style="margin-right:10px;">
                                                        <c:TriStateCheckBox ID="chkAttending" runat="server" CssClass="checkbox" />
                                                        <asp:Label ID="lblInstance" runat="server" />
                                                    </label>
                                                    <c:DetailButton ID="btnDetail" runat="server">
                                                        <DetailTemplate>
                                                            <!-- TODO: enable/disable controls based on chkAttending.checked -->
                                                            <div class="detail-header"><%# GetDataItem() %></div>
                                                            <asp:RadioButtonList ID="rblCommuteMethod" CssClass="radio" runat="server">
                                                                <asp:ListItem Value="NeedRide" Selected="True">I will need a ride</asp:ListItem>
                                                                <asp:ListItem Value="Driver">I am a driver</asp:ListItem>
                                                                <asp:ListItem Value="HaveRide">I will have my own ride</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                            <div style="margin-top:8px;">
                                                                <asp:CheckBox ID="chkCanDriveIfNeeded" Text="I am willing to drive if we need more drivers" CssClass="checkbox" runat="server" />
                                                            </div>
                                                            <div style="display: inline-block; margin-left: 20px;">
                                                                Seats:
                                                                <c:NumericUpDown ID="nudSeats" Minimum="1" Width="70px" CssClass="form-control form-control-inline" runat="server" />
                                                            </div>
                                                            <hr style="margin:7px 0px;" />
                                                            <div><b>Total seats:</b> <asp:Label ID="lblSeatCount" runat="server" /></div>
                                                            <div><b>Total coming:</b> <asp:Label ID="lblAttendanceCount" runat="server" /></div>
                                                            <div><asp:Label ID="lblAttendees" runat="server" /></div>
                                                        </DetailTemplate>
                                                    </c:DetailButton>
                                                </div>
                                                
                                                <%-- TODO: give admin option to skip this instance --%>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                            <p>
                                <span id="info-message"></span>
                                <span id="success-message" class="text-success"><%: SuccessMessage %></span>
                                <span id="error-message" class="text-danger"><%: ErrorMessage %></span>
                                &nbsp;
                            </p>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:Label ID="lblEmptyTrips" Visible="false" runat="server">You have not selected any trips. Go to the trips page to select which trips to want to attend.</asp:Label>
                </ContentTemplate>
            </c:UserStatusPanel>
        </div>
    </div>
    <input id="btnUpdate" type="button" value="Save" class="btn btn-default" onclick="updateAttendance();" runat="server" />
    <%-- TODO: add Doodle link and ability to create doodle for admins (low priority) --%>
    <script type="text/javascript">
        function updateAttendance() {
            $("#info-message").text("Saving...");
            $("#success-message").text("");
            $("#error-message").text("");
            __doPostBack("<%: btnUpdate.UniqueID %>", "");
        };
    </script>
</asp:Content>
