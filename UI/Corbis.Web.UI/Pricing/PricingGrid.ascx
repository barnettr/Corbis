<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PricingGrid.ascx.cs" Inherits="Corbis.Web.UI.Pricing.PricingGrid" %>
<%@ Register Assembly="Corbis.Web.UI.Controls" Namespace="Corbis.Web.UI.Controls" TagPrefix="Corbis" %>

    <div class="PricingGrid">
        <input type="hidden" name="hidAttributeUID" id="hidAttributeUID" runat="server" />
        <input type="hidden" name="hidAttributeValueUID" id="hidAttributeValueUID" runat="server" />
        <input type="hidden" name="hidUseTypeUID" id="hidUseTypeUID" runat="server" />


                <div class="RepeaterContainer rounded">
                    <div class="LicensingDetailsHeader">
                        <h3><asp:Label runat="server" ID="lblLicensingDetails" meta:resourcekey="lblLicensingDetails" /></h3>
                    </div>
                    <div class="RFPricingRepeater" id="repeaterInnerDiv"/>

                    <asp:Repeater ID="rpRSPricing" runat="server">
                        <ItemTemplate>
                        <div class="Clear ListItemWrap">
                        <div class="RFPricingRowLeft">&nbsp;</div>
                        <div class="RFPricingRepeaterDataRow">
                            <input type="hidden" name="AttributeUID" value="<%# Eval("AttributeUid") %>" />
                            <input type="hidden" name="ValueUID" value="<%# Eval("ValueUid") %>" />
                            <input type="hidden" name="UseTypeUID" value="<%# Eval("UseTypeUid") %>" />
                            <input type="hidden" name="EffectivePrice" value="<%# Eval("EffectivePrice") %>" />
                            <ul>
                                <li>
                                    <img src="/Images/info.gif" alt="" title="" />
                                    <asp:Label width="350px" ID="lblDisplayText" runat="server" Text='<%# Eval("DisplayText") %>'></asp:Label>
                                </li>
                                <li class="Right">
                                    <asp:Label width="70px" ID="lblPriceText" runat="server" Text='<%# Eval("EffectivePrice") + "" + Eval("Currencycode") + "" %>'></asp:Label>
                                </li>
                            </ul>
                            </div>
                            <div class="RFPricingRowRight">&nbsp;</div>
                            </div>
                          </ItemTemplate>
                          <FooterTemplate>
                            </div>
                          </FooterTemplate>
                          
                    </asp:Repeater>
                </div>

</div>