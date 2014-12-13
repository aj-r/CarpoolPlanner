<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Accordion.ascx.cs" Inherits="CarpoolPlanner.UserControls.Accordion" %>
<asp:Repeater OnItemDataBound="repeater_ItemDataBound" runat="server">
    <ItemTemplate>
        <div class="collapsed"><asp:PlaceHolder ID="collapsed" runat="server" /></div>
        <div class="expanded"><asp:PlaceHolder ID="expanded" runat="server" /></div>
    </ItemTemplate>
</asp:Repeater>