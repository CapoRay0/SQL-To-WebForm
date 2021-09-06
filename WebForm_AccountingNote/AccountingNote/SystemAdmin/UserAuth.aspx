<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserAuth.aspx.cs" Inherits="Ray0728am.SystemAdmin.UserAuth" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
 <div>
            <table>
                <tr>
                    <th>Account</th>
                    <td>
                        <asp:Literal ID="ltAccount" runat="server"></asp:Literal></td>
                </tr>
                <tr>
                    <th>Add Roles</th>
                    <td>
                        <asp:CheckBoxList ID="ckbRoleList" runat="server" DataValueField="ID" DataTextField="RoleName"></asp:CheckBoxList>
                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                    </td>
                </tr>

                <tr>
                    <th>Roles</th>
                    <td>
                        <asp:Repeater ID="rptRoleList" runat="server"></asp:Repeater>
                        <ItemTemplate>
                            <%#Eval("RoleName")%>
                            <asp:Button ID="btnDelete" runat="server" Text="Remove" 
                                CommandName="DeleteRole" CommandArgument='<%#Eval("ID") %>'/>
                        </ItemTemplate>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
