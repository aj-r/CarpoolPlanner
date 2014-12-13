<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DateTimePicker.ascx.cs" Inherits="CarpoolPlanner.UserControls.DateTimePicker" %>
<asp:TextBox ID="txtDate" CssClass="form-control form-control-inline" runat="server" />
<script>
    $("#<%: txtDate.ClientID %>").datepicker({ dateFormat: 'dd-M-yy' });
</script>
<span id="Time" runat="server">
    <c:NumericUpDown ID="nudHour" CssClass="form-control form-control-inline" Minimum="0" Maximum="23" Width="60px" runat="server" />
    :
    <%-- TODO: pad display value to 2 digits (javascript?) --%>
    <c:NumericUpDown ID="nudMinute" CssClass="form-control form-control-inline" Minimum="0" Maximum="59" Width="60px" runat="server" />
</span>