<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SCALING WITH BEANSTALK</title>
    <link href="//netdna.bootstrapcdn.com/font-awesome/4.0.3/css/font-awesome.css" rel="stylesheet">
</head>
<body>
    <h1>SUPER IMPORTANT AND SECURE APP
        </h1>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" ID="ctrScriptManager" />
        
        <asp:UpdatePanel runat="server" ID="pnlUpdate">
            <ContentTemplate>
                <asp:Timer runat="server" ID="ctrTimer" Interval="5000" OnTick="ctrTimer_Tick"></asp:Timer>
                <asp:Panel ID="pnlSettings" runat="server">
                    <div>
                        Min # of Instances:
                        <br />
                        <asp:Button ID="btnSet1" runat="server" CommandArgument="1" Text="Decrease to 1" Visible="false" CssClass="button fa-bug" OnClick="btnUpdate_Click" />
                        <asp:Button ID="btnSet2" runat="server" CommandArgument="2" Text="Increase to 2" Visible="false"  OnClick="btnUpdate_Click" />
                    </div>
                    
                    
                    <br />
                    
                </asp:Panel>
                <div>
                    Current Environment Health:
                    <asp:Label ID="lblHealth" runat="server" />
                </div>
                <div>
                    Current Environment Status:
                    <asp:Label ID="lblStatus" runat="server" />
                </div>
                <asp:Label ID="lblGenerated" runat="server" />
                <br />
                <asp:GridView ID="gvOptions" runat="server" AutoGenerateColumns="false">
                    <Columns>
                        <asp:BoundField DataField="OptionName" HeaderText="Min and Max Number of Instances" />
                        <asp:BoundField DataField="Value" HeaderText="Number" />
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
            <ProgressTemplate>
                <i class="fa fa-refresh fa-spin fa-5x" style="color: red"></i>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </form>
</body>
</html>
