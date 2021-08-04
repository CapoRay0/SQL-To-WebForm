<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPager2.ascx.cs" Inherits="Ray0728am.UserControls.ucPager2" %>

<div>
    <a runat="server" id="aLinkFirst" href="#">First</a>
    <a runat="server" id="aLink1" href="#">1</a>
    <a runat="server" id="aLink2" href="#">2</a>
    <%--<a runat="server" id="aLink3" href="#">3</a>--%>
    <asp:Literal runat="server" ID="ltlCurrentPage"></asp:Literal>
    <a runat="server" id="aLink4" href="#">4</a>
    <a runat="server" id="aLink5" href="#">5</a>
    <a runat="server" id="aLinkLast" href="#">Last</a>
    <asp:Literal ID="ltpager" runat="server"></asp:Literal>
</div>
