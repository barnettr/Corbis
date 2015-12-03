<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="RfcdCategory.aspx.cs" Inherits="Corbis.Web.UI.Rfcd.RfcdCategory" EnableEventValidation="true" MasterPageFile="~/MasterPages/MasterBase.Master" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="AJAXExtentions" Namespace="AjaxControlToolkit.WCSFExtensions" Assembly="AjaxControlToolkit.WCSFExtensions" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:Content ID="rfcdCategoryContent" ContentPlaceHolderID="mainContent" runat="server">
    <asp:HiddenField ID="previousLanguageCode" EnableViewState="true" runat="server" />
          <asp:UpdatePanel ID="rfcdCategoryUpdatePanel" runat="server" >
                        <ContentTemplate>
<div id="pageContent">
    <div id="contentHolder" class="RfcdCategoryMarginLeft">
        <div id="topContentHolder" >
            <div id="alphabetLabel" >
                <h3><Corbis:Localize ID="alphabetTitle" runat="server" meta:resourcekey="alphabetTitle" /></h3>
            </div>
            <div id="alphabetCategory" class="RfcdCategoryAlphabet">            
                <div class="RfcdAlphabetFloatLeft"><Corbis:LinkButton ID="A" runat="server" meta:resourcekey="alphabetA" OnClick="RfcdByAlphabet_Click"></Corbis:LinkButton>&nbsp;&nbsp;</div>
                <div class="RfcdAlphabetFloatLeft"><Corbis:LinkButton ID="B" runat="server" meta:resourcekey="alphabetB" OnClick="RfcdByAlphabet_Click"></Corbis:LinkButton>&nbsp;&nbsp;</div>
                <div class="RfcdAlphabetFloatLeft"><Corbis:LinkButton ID="C" runat="server" meta:resourcekey="alphabetC" OnClick="RfcdByAlphabet_Click"></Corbis:LinkButton>&nbsp;&nbsp;</div>
                <div class="RfcdAlphabetFloatLeft"><Corbis:LinkButton ID="D" runat="server" meta:resourcekey="alphabetD" OnClick="RfcdByAlphabet_Click"></Corbis:LinkButton>&nbsp;&nbsp;</div>
                <div class="RfcdAlphabetFloatLeft"><Corbis:LinkButton ID="E" runat="server" meta:resourcekey="alphabetE" OnClick="RfcdByAlphabet_Click"></Corbis:LinkButton>&nbsp;&nbsp;</div>
                <div class="RfcdAlphabetFloatLeft"><Corbis:LinkButton ID="F" runat="server" meta:resourcekey="alphabetF" OnClick="RfcdByAlphabet_Click"></Corbis:LinkButton>&nbsp;&nbsp;</div>
                <div class="RfcdAlphabetFloatLeft"><Corbis:LinkButton ID="G" runat="server" meta:resourcekey="alphabetG" OnClick="RfcdByAlphabet_Click"></Corbis:LinkButton>&nbsp;&nbsp;</div>
                <div class="RfcdAlphabetFloatLeft"><Corbis:LinkButton ID="H" runat="server" meta:resourcekey="alphabetH" OnClick="RfcdByAlphabet_Click"></Corbis:LinkButton>&nbsp;&nbsp;</div>
                <div class="RfcdAlphabetFloatLeft"><Corbis:LinkButton ID="I" runat="server" meta:resourcekey="alphabetI" OnClick="RfcdByAlphabet_Click"></Corbis:LinkButton>&nbsp;&nbsp;</div>
                <div class="RfcdAlphabetFloatLeft"><Corbis:LinkButton ID="J" runat="server" meta:resourcekey="alphabetJ" OnClick="RfcdByAlphabet_Click"></Corbis:LinkButton>&nbsp;&nbsp;</div>
                <div class="RfcdAlphabetFloatLeft"><Corbis:LinkButton ID="K" runat="server" meta:resourcekey="alphabetK" OnClick="RfcdByAlphabet_Click"></Corbis:LinkButton>&nbsp;&nbsp;</div>
                <div class="RfcdAlphabetFloatLeft"><Corbis:LinkButton ID="L" runat="server" meta:resourcekey="alphabetL" OnClick="RfcdByAlphabet_Click"></Corbis:LinkButton>&nbsp;&nbsp;</div>
                <div class="RfcdAlphabetFloatLeft"><Corbis:LinkButton ID="M" runat="server" meta:resourcekey="alphabetM" OnClick="RfcdByAlphabet_Click"></Corbis:LinkButton>&nbsp;&nbsp;</div>
                <div class="RfcdAlphabetFloatLeft"><Corbis:LinkButton ID="N" runat="server" meta:resourcekey="alphabetN" OnClick="RfcdByAlphabet_Click"></Corbis:LinkButton>&nbsp;&nbsp;</div>
                <div class="RfcdAlphabetFloatLeft"><Corbis:LinkButton ID="O" runat="server" meta:resourcekey="alphabetO" OnClick="RfcdByAlphabet_Click"></Corbis:LinkButton>&nbsp;&nbsp;</div>
                <div class="RfcdAlphabetFloatLeft"><Corbis:LinkButton ID="P" runat="server" meta:resourcekey="alphabetP" OnClick="RfcdByAlphabet_Click"></Corbis:LinkButton>&nbsp;&nbsp;</div>
                <div class="RfcdAlphabetFloatLeft"><Corbis:LinkButton ID="Q" runat="server" meta:resourcekey="alphabetQ" OnClick="RfcdByAlphabet_Click"></Corbis:LinkButton>&nbsp;&nbsp;</div>
                <div class="RfcdAlphabetFloatLeft"><Corbis:LinkButton ID="R" runat="server" meta:resourcekey="alphabetR" OnClick="RfcdByAlphabet_Click"></Corbis:LinkButton>&nbsp;&nbsp;</div>
                <div class="RfcdAlphabetFloatLeft"><Corbis:LinkButton ID="S" runat="server" meta:resourcekey="alphabetS" OnClick="RfcdByAlphabet_Click"></Corbis:LinkButton>&nbsp;&nbsp;</div>
                <div class="RfcdAlphabetFloatLeft"><Corbis:LinkButton ID="T" runat="server" meta:resourcekey="alphabetT" OnClick="RfcdByAlphabet_Click"></Corbis:LinkButton>&nbsp;&nbsp;</div>
                <div class="RfcdAlphabetFloatLeft"><Corbis:LinkButton ID="U" runat="server" meta:resourcekey="alphabetU" OnClick="RfcdByAlphabet_Click"></Corbis:LinkButton>&nbsp;&nbsp;</div>
                <div class="RfcdAlphabetFloatLeft"><Corbis:LinkButton ID="V" runat="server" meta:resourcekey="alphabetV" OnClick="RfcdByAlphabet_Click"></Corbis:LinkButton>&nbsp;&nbsp;</div>
                <div class="RfcdAlphabetFloatLeft"><Corbis:LinkButton ID="W" runat="server" meta:resourcekey="alphabetW" OnClick="RfcdByAlphabet_Click"></Corbis:LinkButton>&nbsp;&nbsp;</div>
                <div class="RfcdAlphabetFloatLeft"><Corbis:LinkButton ID="X" runat="server" meta:resourcekey="alphabetX" OnClick="RfcdByAlphabet_Click"></Corbis:LinkButton>&nbsp;&nbsp;</div>
                <div class="RfcdAlphabetFloatLeft"><Corbis:LinkButton ID="Y" runat="server" meta:resourcekey="alphabetY" OnClick="RfcdByAlphabet_Click"></Corbis:LinkButton>&nbsp;&nbsp;</div>
                <div class="RfcdAlphabetFloatLeft"><Corbis:LinkButton ID="Z" runat="server" meta:resourcekey="alphabetZ" OnClick="RfcdByAlphabet_Click"></Corbis:LinkButton>&nbsp;&nbsp;</div>
            </div>
        </div>
        <br />
        <div id="bottomContentHolder" class="RfcdCategoryContent">
            <div id="categoryLabel" >
            <h3><Corbis:Localize ID="byCategoryTitle" runat="server" meta:resourcekey="byCategoryTitle" /></h3>
            </div>
            <div id="categoryHolder">            
                <div id="byCategoryContainer" runat="server" class="RfcdCategoryFloatLeft">
                    <asp:XmlDataSource ID="xmlDataSourceRfcdByCategory" EnableCaching="false" runat="server"></asp:XmlDataSource>
                    <asp:TreeView ID="treeViewRfcdByCategory" ExpandDepth="1" EnableClientScript="true" DataSourceID="xmlDataSourceRfcdByCategory"
                            runat="server" ShowExpandCollapse="true"  OnSelectedNodeChanged="RfcdByCategory_SelectedNodeChanged" > 
                        <DataBindings>
                            <asp:TreeNodeBinding DataMember="category" TextField="name" valueField="value" />
                            <asp:TreeNodeBinding DataMember="categories" TextField="" value="" />
                        </DataBindings>
                    </asp:TreeView>                   
                </div>
                <div  id="categoryDetail" class="RfcdCategoryDetailFloatLeft">                 
                            <Corbis:Label ID="categoryTitle" runat="server" ></Corbis:Label>
                            <asp:Repeater ID="repeaterRfcdEntity" runat="server" OnItemDataBound="RfCdEntity_ItemDataBound" >
                                <ItemTemplate>
                                    <div>
                                        <Corbis:LinkButton id="linkRfcdEntity" runat="server" CommandName='<%# DataBinder.Eval(Container.DataItem,"VolumeNumber") %>'> <%# DataBinder.Eval(Container.DataItem,"Title") %>  <%# !string.IsNullOrEmpty((DataBinder.Eval(Container.DataItem,"BasePrice") == null) ? string.Empty : DataBinder.Eval(Container.DataItem,"BasePrice").ToString()) ? "( " + DataBinder.Eval(Container.DataItem,"BasePrice") + " " +  DataBinder.Eval(Container.DataItem,"BasePriceUnit") + " )" :  "" %> </Corbis:LinkButton>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>

                </div>
            </div>
        </div>
    </div>  
</div>
        </ContentTemplate>
                    </asp:UpdatePanel>                 
</asp:Content>
