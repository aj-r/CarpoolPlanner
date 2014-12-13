<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DetailButton.ascx.cs" Inherits="CarpoolPlanner.UserControls.DetailButton" %>
<div style="display:inline-block;">
    <img id="btnOpenDetails" src="/Content/details.png" class="image-button" title="Details" runat="server" />
    <div class="details-container">
        <div id="details" class="details" style="display: none;" runat="server">
            <img id="x" src="/Content/x.png" class="x image-button" runat="server" />
            <asp:HiddenField ID="hdnVisible" Value="false" runat="server" />
            <asp:PlaceHolder ID="ph" runat="server" />
        </div>
    </div>
    <script type="text/javascript">
        function <%: ClientID %>_load() {
            $("#<%: btnOpenDetails.ClientID %>").click(function() {
                var details = $("#<%: details.ClientID %>");
                var isVisible = details.css("display") == "block";
                // Hide all other details
                $(".details").css("display", "none");
                $(".details input[type=hidden]").val("false");
                if (!isVisible)
                    details.css("display", "block");
                var hdnVisible = $("#<%: hdnVisible.ClientID %>");
                hdnVisible.val((!isVisible).toString().toLowerCase());
            });
            $("#<%: x.ClientID %>").click(function() {
                $(".details").css("display", "none");
                var hdnVisible = $("#<%: hdnVisible.ClientID %>");
                hdnVisible.val("false");
            });
        }
        registerPageLoad(<%: ClientID %>_load);
    </script>
</div>