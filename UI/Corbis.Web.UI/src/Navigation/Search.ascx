<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="Search.ascx.cs" EnableViewState="false" Inherits="Corbis.Web.UI.Navigation.Search" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>

<div id="Search">
    <AJAXToolkit:TextBoxWatermarkExtender ID="keywordSearchWatermark" runat="server" TargetControlID="keywordSearch" WatermarkText="<%$ Resources: SearchImages %>" WatermarkCssClass="Optional" />
    <asp:Panel ID="searchPanel" runat="server" CssClass="Search">
        
        <div id="Keywords" class="Keywords" onmousedown="CorbisUI.ExtendedSearch.vars._isSearchFlyoutMousedown=true;event.cancelBubble=true;"><Corbis:TextBox onfocus="CorbisUI.ExtendedSearch.showSearchFlyout();this.focus();CorbisUI.ExtendedSearch.MoveToEnd();" onkeypress="return CorbisUI.ExtendedSearch.hijackTheEnter(event)" CssClass="keywordSearch" ID="keywordSearch" runat="server" onblur="CorbisUI.ExtendedSearch.hideSearchFlyout()"></Corbis:TextBox></div>
        <div id="optionsAppliedDiv" style="display:none;">
            <div class="optionsAppliedTextWrapper" onmouseover="CorbisUI.ExtendedSearch.OpenOptionsAppliedModal();" onmouseout="return CorbisUI.EnlargementTimerSearch.HideAppliedOptionsWindow();" id="optionsAppliedTextWrapper">
                <Corbis:Label ID="OptionsApplied" runat="server" CssClass="optionsAppliedText" meta:resourcekey="optionsAppliedText" />
            </div>
            <div class="optionsAppliedClose" id="optionsAppliedClose" onclick="CorbisUI.ExtendedSearch.deleteSearchOptions(); return false;">
                <Corbis:LinkButton  ID="deleteMoreSearchOptions" CssClass="Close" OnClientClick="return false;" runat="server" meta:resourcekey="deleteMoreSearchOptionsToolTip" Text="&nbsp;" />
            </div>
        </div>
        <div class="Go" onmousedown="CorbisUI.ExtendedSearch.vars._isSearchFlyoutMousedown=true;event.cancelBubble=true">
        <div class="Center">
        <Corbis:LinkButton CausesValidation="false" ID="search" runat="server" CssClass="button" meta:resourcekey="search" Onclientclick="CorbisUI.ExtendedSearch.combineSearchBuddy(); return false;"/></div></div>
        <div class="MSOButton" onmousedown="_isSearchFlyoutMousedown = true;event.cancelBubble = true" onclick="CorbisUI.ExtendedSearch.OpenMoreSearchOptions();return false;" >
            <div class="arrowMeUp"></div>
            <div class="GlassButton btnOrangedbdbdb" id="searchGlassButton">
		        <span class="Right"><span class="Center"><a href=#><Corbis:Localize runat="server" meta:resourcekey=moreSearchTitle /></a></span></span>
	        </div>
	    </div>
	    <div id="returnToPreviousSearchDiv" class="returnToPreviousSearchDiv" runat="server" visible="true">
	        <div class="GlassButton btnOrangedbdbdb">
	            <span class="Right"><span class="Center"><Corbis:HyperLink ID="returnPreviousSearchHyperlink" onclick="CorbisUI.ExtendedSearch.ShowSearchProgIndicator();" class="returnPreviousSearchHyperlink" runat="server" meta:resourcekey="returnPreviousSearchTitle" NavigateUrl="" /></span></span>
	        </div>
	    </div>
                    
	 </asp:Panel>
</div>

<%-- OPTIONS APPLIED POPUP --%>
<div id="optionsAppliedModal" style="width:310px; display:none; cursor:pointer" onclick="CorbisUI.ExtendedSearch.OpenMoreSearchOptions(); CorbisUI.ExtendedSearch.hideAppliedOptionsWindow(event); event.cancelBubble = true; return false;">
    <div id="optionsTitleWrapper" class="optionsTitleWrapper">
        <div id="optionsSubtitle" class="optionsSubtitle">
            <img class="close" src="../Images/iconClose.gif" onclick="CorbisUI.ExtendedSearch.hideAppliedOptionsWindow(event); event.cancelBubble = true; return false;"/>
            <Corbis:Label ID="optionsAppliedTitle" CssClass="optionsAppliedTitle" runat="server" meta:resourcekey="optionsAppliedTitle" /><br />
            <div class="subWrapper"><Corbis:Label ID="optionsAppliedSubTitle" CssClass="optionsAppliedSubTitle" runat="server" meta:resourcekey="optionsAppliedSubTitle" /></div>
        </div>
    </div>
    <div id="dataContainer" class="optionsAppliedDataContainer">
	    <div id="dateCreatedSummaryDiv" visible="false" class="pairing" runat="server">
	        <Corbis:Label ID="dateCreatedSummaryLabel" CssClass="optionsLightText" runat="server" meta:resourcekey="dateCreatedSummaryLabel" />
	        <Corbis:Label ID="dateCreatedSummary" CssClass="optionsValueText" runat="server"></Corbis:Label>
	    </div>
	    <div id="daysSummaryDiv" visible="false" class="pairing" runat="server">
	        <Corbis:Label ID="daysSummary" CssClass="optionsLightText" runat="server"></Corbis:Label>
	    </div>
	    <div id="dateRangeSummaryDiv" visible="false" class="pairing" runat="server">
	        <Corbis:Label ID="dateRangeSummaryLabel" CssClass="optionsLightText" runat="server" meta:resourcekey="dateRangeSummaryLabel" />
	        <Corbis:Label ID="beginDateSummary" CssClass="optionsValueText" runat="server" /> &
	        <Corbis:Label ID="endDateSummary" CssClass="optionsValueText" runat="server"></Corbis:Label>
	    </div>
	    <div id="locationSummaryDiv" visible="false" class="pairing" runat="server">
	        <Corbis:Label ID="locationSummaryLabel" CssClass="optionsLightText" runat="server" meta:resourcekey="locationSummaryLabel" />
	        <Corbis:Label ID="locationSummary" CssClass="optionsValueText" runat="server"></Corbis:Label>
	    </div>
	    <div id="photographerSummaryDiv" visible="false" class="pairing" runat="server">
	        <Corbis:Label ID="photographerSummaryLabel" CssClass="optionsLightText" runat="server" meta:resourcekey="photographerSummaryLabel" />
	        <Corbis:Label ID="photographerSummary" CssClass="optionsValueText" runat="server"></Corbis:Label>
	    </div>
	    <div id="providerSummaryDiv" visible="false" class="pairing" runat="server">
	        <Corbis:Label ID="providerSummaryLabel" CssClass="optionsLightText" runat="server" meta:resourcekey="providerSummaryLabel" />
	        <Corbis:Label ID="providerSummary" CssClass="optionsValueText" runat="server"></Corbis:Label>
        </div>
	    <div id="orientationSummaryDiv" visible="false" class="pairing" runat="server">
	        <Corbis:Label ID="orientationSummaryLabel" CssClass="optionsLightText" runat="server" meta:resourcekey="orientationSummaryLabel" />
            <Corbis:Label ID="orientationSummary" Visible="false" CssClass="optionsValueText" runat="server"></Corbis:Label>
	    </div>
	    <div id="pointOfViewSummaryDiv" visible="false" class="pairing" runat="server">
	        <Corbis:Label ID="pointOfViewSummaryLabel" CssClass="optionsLightText" runat="server" meta:resourcekey="pointOfViewSummaryLabel" />
	        <Corbis:Label ID="pointOfViewSummary" CssClass="optionsValueText" runat="server"></Corbis:Label>
	    </div>
	    <div id="numberOfPeopleSummaryDiv" visible="false" class="pairing" runat="server">
	        <Corbis:Label ID="numberOfPeopleSummaryLabel" CssClass="optionsLightText" runat="server" meta:resourcekey="numberOfPeopleSummaryLabel" />
	        <Corbis:Label ID="numberOfPeopleSummary" CssClass="optionsValueText" runat="server"></Corbis:Label>
	    </div>
	    <div id="immediateAvailabilitySummaryDiv" visible="false" class="pairing" runat="server">
	        <Corbis:Label ID="immediateAvailabilitySummaryLabel" CssClass="optionsLightText" runat="server" meta:resourcekey="immediateAvailabilitySummaryLabel" />
	        <Corbis:Label ID="immediateAvailabilitySummary" CssClass="optionsValueText" runat="server"></Corbis:Label>
	    </div>
	    <div id="imageNumbersSummaryDiv" visible="false" class="pairing" runat="server">
	        <Corbis:Label ID="imageNumbersSummaryLabel" CssClass="optionsLightText" runat="server" meta:resourcekey="imageNumbersSummaryLabel" />
	        <Corbis:Label ID="imageNumbersSummary" CssClass="optionsValueText" runat="server"></Corbis:Label>
	    </div>
	    <div id="premiumCollectionsSummaryDiv" visible="false" class="pairing" runat="server">
	        <Corbis:Label ID="premiumCollectionsSummaryLabel" CssClass="optionsLightText" runat="server" meta:resourcekey="premiumCollectionsSummaryLabel" />
	        <Corbis:Label ID="premiumCollectionsCountSummary" CssClass="optionsValueText" runat="server"></Corbis:Label>/<Corbis:Label ID="premiumCollectionsTotalSummary" CssClass="optionsValueText" runat="server"></Corbis:Label>
        </div>
	    <div id="standardCollectionsSummaryDiv" visible="false" class="pairing" runat="server">
            <Corbis:Label ID="standardCollectionsSummaryLabel" CssClass="optionsLightText" runat="server" meta:resourcekey="standardCollectionsSummaryLabel" />
	        <Corbis:Label ID="standardCollectionsCountSummary" CssClass="optionsValueText" runat="server"></Corbis:Label>/<Corbis:Label ID="standardCollectionsTotalSummary" CssClass="optionsValueText" runat="server"></Corbis:Label>
        </div>
	    <div id="valueCollectionsSummaryDiv" visible="false" class="pairing" runat="server">
            <Corbis:Label ID="valueCollectionsSummaryLabel" CssClass="optionsLightText" runat="server" meta:resourcekey="valueCollectionsSummaryLabel" />
	        <Corbis:Label ID="valueCollectionsCountSummary" CssClass="optionsValueText" runat="server"></Corbis:Label>/<Corbis:Label ID="valueCollectionsTotalSummary" CssClass="optionsValueText" runat="server"></Corbis:Label>
	    </div>
	    <div id="superValueCollectionsSummaryDiv" visible="false" class="pairing" runat="server">
	        <Corbis:Label ID="superValueCollectionsSummaryLabel" CssClass="optionsLightText" runat="server" meta:resourcekey="superValueCollectionsSummaryLabel" />
	        <Corbis:Label ID="superValueCollectionsCountSummary" CssClass="optionsValueText" runat="server"></Corbis:Label>/<Corbis:Label ID="superValueCollectionsTotalSummary" CssClass="optionsValueText" runat="server"></Corbis:Label>
	    </div> 
        <img alt="" id="dataContainerDivider" runat="server" style="display:block; height:8px;" src="/Images/spacer.gif" />      
    </div>
    
    <div class="Clr"></div>
</div>
<%-- END OPTIONS APPLIED MODAL --%>

<div id="searchProgIndicator" onclick="CorbisUI.ExtendedSearch.hideMSOWindow(true)">
    <div class="mask"></div>
    <div id="searchProgContents">
	    <img border="0" alt="" src="/images/ajax-loader2.gif" />
	    <br />
	    <h1><Corbis:Label ID="searching" runat="server" meta:resourcekey="searching" /></h1>
	    <div id="processingFilters">
	        <Corbis:Label ID="procFilters" runat="server" meta:resourcekey="procFilters" />
	    </div>
    </div>
</div>

<div id="moreSearchOptions" onmousedown="CorbisUI.ExtendedSearch.vars._isSearchFlyoutMousedown=true;event.cancelBubble=true" style="width:775px;height:435px;display:none;position:absolute;top:0px;left:0px;z-index:999997">
   
    <Corbis:Label ID="moreSearchTitle" runat="server" CssClass="moreTitle" meta:resourcekey="moreSearchTitle" />
    <asp:UpdatePanel ID="MSOUpdater" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="hiddenMSOTrigger" />
        </Triggers>
        <ContentTemplate>
        <asp:Button ID="hiddenMSOTrigger" runat="server" CssClass="hdn" />
        <asp:TextBox ID="hiddenMSOValue" CssClass="hdn" EnableViewState="false" runat="server" Text="UnOpened" />
        <div style="position:absolute;top: 100px; left: 350px;" id="MSOIndicator">
            <img src="/Images/ajax-loader2.gif">
        </div>
        <div id="MSOContentDiv" runat="server" visible="false">
            <div id="aColumn" class="aSearchColumn">
            <div class="MB_5"><Corbis:Label ID="dateCreatedLabel" runat="server" CssClass="columnAContent" meta:resourcekey="dateCreatedLabel" /></div>
            <Corbis:Textbox ID="dateCreated" runat="server" MaxLength="20" Width="200"></Corbis:Textbox>
            <Corbis:Label ID="exampleLabel" runat="server" CssClass="egText" Width="190" meta:resourcekey="exampleLabel" />
            
            <Corbis:Label ID="imageAvailableLabel" runat="server" CssClass="availText" meta:resourcekey="imageAvailableLabel" />
            
            <div class="roundMe IMA_lastDays">
                <div class="inRoundMe">
                    <div class="topLeftCorner">&nbsp;</div>
                    <div class="topRightCorner">&nbsp;</div>
                        <div class="MT_5 MB_5">
                            <div style="vertical-align:text-top;">
                                <div style="display:inline-block;position:relative;top:2px;" class="imageRadio" onclick="radioClicked('radioDaysDiv')" id="radioDaysDiv">
                                    <img class="imageRadio" alt="" src="/Images/radio_on.png" id="daysButtonImg" runat="server" />
                                    <Corbis:RadioButton CssClass="imageRadio" GroupName="dateButton"  ID="daysButton" runat="server"  meta:resourcekey="daysButtonLabel" Checked="true"/> 
                                </div>
                                <Corbis:Textbox ID="days" runat="server" onfocus="radioClicked('radioDaysDiv')" MaxLength="6" Width="30" Height="20" />&nbsp;&nbsp; 
                                <Corbis:Label ID="daysTextLabel" runat="server" CssClass="columnAContent" meta:resourcekey="daysTextLabel" />
                            </div>
                        </div>
                        <div class="bottomLeftCorner">&nbsp;</div>
                    <div class="bottomRightCorner">&nbsp;</div>
                </div>
            </div>
           
            <div class="roundMe IMA_between">
                <div class="inRoundMe">
                    <div class="topLeftCorner">&nbsp;</div>
                    <div class="topRightCorner">&nbsp;</div>
                        <div class="MB_5">
                            <div id="betweenDataContainer">
                                <div class="imageRadio" onclick="radioClicked('radioBetweenDiv');CorbisUI.MSOSearch.clearRadioDaysText();" id="radioBetweenDiv">
                                    <img alt="" style="position:relative;top:2px;margin-right:-20px" class="imageRadio" src="/Images/radio_off.png" id="betweenButtonImg" runat="server" />
                                    <Corbis:RadioButton 
                                        CssClass="imageRadio between" ID="betweenButton" 
                                        GroupName="dateButton" runat="server"
                                        meta:resourcekey="betweenButton"
                                        Checked="false"
                                    /> 
                                </div>
                                <div id="dateDataContainer">
                    
                                            <Corbis:TextBox
                                                runat="server" ID="beginDate" CssClass="beginDate" onfocus="CorbisUI.ExtendedSearch.showCalendar1();CorbisUI.MSOSearch.clearRadioDaysText();this.select();" 
                                                style="width:77px"
                                            />
                                            <AJAXToolkit:CalendarExtender 
                                                ID="msoStartDateExtend" runat="server"  
                                                TargetControlID="beginDate" 
                                                CssClass="aeCalendar" 
                                                OnClientDateSelectionChanged="CorbisUI.ExtendedSearch.checkStartDate" 
                                                BehaviorID="Calendar1" 
                                                
                                            />
                                         
                                            <Corbis:Label ID="andLabel" runat="server" CssClass="columnAContent" meta:resourcekey="andLabel"/>&nbsp;&nbsp;
                                            <Corbis:TextBox style="width:77px" runat="server" 
                                                ID="endDate" onfocus="CorbisUI.ExtendedSearch.showCalendar2();CorbisUI.MSOSearch.clearRadioDaysText();this.select();" CssClass="endDate"
                                            />
                                            <AJAXToolkit:CalendarExtender 
                                                ID="msoEndDateExtend" runat="server"  
                                                TargetControlID="endDate" 
                                                CssClass="aeCalendar rightCalendar" 
                                                OnClientDateSelectionChanged="CorbisUI.ExtendedSearch.checkEndDate" 
                                                BehaviorID="Calendar2" 
                                            />
                                        
                                           


                                </div>
                            </div>
                        </div>
                        <div class="bottomLeftCorner">&nbsp;</div>
                    <div class="bottomRightCorner">&nbsp;</div>
                </div>
            </div>
            
            <div class="locText">
                <Corbis:Label id="locationLabel" runat="server" meta:resourcekey="locationLabel" />
                <Corbis:Textbox ID="location" runat="server" MaxLength="100" Width="207"></Corbis:Textbox>
            </div>
            
            <div class="photoText">
                <Corbis:Label id="photographerLabel" runat="server" meta:resourcekey="photographerLabel" />
                <Corbis:Textbox ID="photographer" runat="server" MaxLength="100" Width="207"></Corbis:Textbox>
            </div>
            
            <div class="providerText">
                <Corbis:Label  id="providerLabel" runat="server" meta:resourcekey="providerLabel" />
                <Corbis:Textbox ID="provider" runat="server" MaxLength="100" Width="207"></Corbis:Textbox>
            </div>
        </div>
        
        <div id="bColumn" class="bSearchColumn">
            <div class="MB_5"><Corbis:Label ID="orientationLabel" runat="server" CssClass="columnAContent" meta:resourcekey="orientationLabel" /></div>
            
           <div class="roundMe MB_20">
                <div class="inRoundMe">
                    <div class="topLeftCorner">&nbsp;</div>
                    <div class="topRightCorner">&nbsp;</div>
                    <div class="floatLeft">
                        <div class="checkLblPair"><Corbis:ImageCheckbox ID="horizontalCheckbox" checked="true" runat="server"></Corbis:ImageCheckbox>
                        <Corbis:Label ID="horizontalLabel" runat="server" meta:resourcekey="horizontalLabel" /></div>
                        <div class="checkLblPair"><Corbis:ImageCheckbox ID="panoramaCheckbox" checked="true" runat="server"></Corbis:ImageCheckbox>
                        <Corbis:Label ID="verticalLabel" runat="server" meta:resourcekey="panoramaLabel" /></div>
                    </div>
                    <div class="floatRight">
                        <div class="checkLblPair"><Corbis:ImageCheckbox ID="verticalCheckbox" checked="true" runat="server"></Corbis:ImageCheckbox>
                           <Corbis:Label ID="panoramaLabel" runat="server" meta:resourcekey="verticalLabel" />
                        </div>
                    </div>
                    <div class="MB_5">&nbsp;</div>
                    <div class="bottomLeftCorner">&nbsp;</div>
                    <div class="bottomRightCorner">&nbsp;</div>
                </div>
          </div>       
            <div class="MB_20">
                <Corbis:Label CssClass="MB_5 pointOfView" id="pointOfViewLabel" runat="server" meta:resourcekey="pointOfViewLabel" />
                <Corbis:DropDownList ID="pointOfView" runat="server" Width="210"></Corbis:DropDownList>
            </div>
            
            <div class="MB_20 numberOfPeople">
                <Corbis:Label CssClass="MB_5" id="numberOfPeopleLabel" runat="server" meta:resourcekey="numberOfPeopleLabel" />
                <Corbis:DropDownList ID="numberOfPeople" onchange="CorbisUI.MSOSearch.verifyNoPeopleChecked();" runat="server" Width="210"></Corbis:DropDownList>
            </div>
            
            <div class="MB_20">
                <Corbis:Label CssClass="MB_5" id="immediateAvailabilityLabel" runat="server" meta:resourcekey="immediateAvailabilityLabel" />
                <Corbis:DropDownList ID="immediateAvailablility" runat="server" Width="210"></Corbis:DropDownList><br />
                <Corbis:Label CssClass="MT_5" id="rMOnlyLabel" runat="server" style="width:180px;" meta:resourcekey="rMOnlyLabel" />
            </div>
            
            <Corbis:Label CssClass="MB_5" id="imageNumbersLabel" runat="server" meta:resourcekey="imageNumbersLabel" />
            <Corbis:TextBox ID="imageNumbers" CssClass="imageNumbers" onfocus="CorbisUI.MSOSearch.clearText();" onblur="CorbisUI.MSOSearch.resetText();" runat="server" TextMode="MultiLine" Width="207" Height="65" meta:resourcekey="imageNumbers" />
            
        </div>
        
        <div id="cColumn" class="cSearchColumn">
            
            <div id="MSO_accordion">
			    <asp:Repeater ID="mcg" runat="server" OnItemDataBound="mcg_ItemDataBound">
				    <ItemTemplate>
					    <div id="<%# DataBinder.Eval(Container.DataItem, "DisplayGroupId").ToString() %>" class="MSO_toggler"><div class="left"><div class="right"><div class="center"><span title="<%# GetMarketingCollectionToolTip((Corbis.MarketingCollection.Contracts.V3.MarketingCollectionGroupType)DataBinder.Eval(Container.DataItem, "DisplayGroupId"))%>" class="title"><%# Corbis.Web.UI.CorbisBasePage.GetEnumDisplayText<Corbis.MarketingCollection.Contracts.V3.MarketingCollectionGroupType>((Corbis.MarketingCollection.Contracts.V3.MarketingCollectionGroupType)DataBinder.Eval(Container.DataItem, "DisplayGroupId"))%></span><span class="counts">0/0</span></div></div></div></div>
					    <div class="MSO_element">
						    <div class="collectionOptionsWrap">
						        <div class="collectionsPadding">
							        <asp:Repeater ID="mc" runat="server">
								        <ItemTemplate>
									        <div class="checkboxWrap">
										        <Corbis:ImageCheckbox ID="mccb" CssClass="ImageCheckbox" checked="true" Text='<%# DataBinder.Eval(Container.DataItem, "DisplayName")%>' value='<%# DataBinder.Eval(Container.DataItem, "Id")%>' runat="server"></Corbis:ImageCheckbox>
									        </div>
								        </ItemTemplate>
							        </asp:Repeater>
						        </div>
						    </div>
						    <div class="selectLinksWrap"><a href="javascript:void(0)" class="selectAllLink"><Corbis:Localize ID="selectAllLabel" runat="server" meta:resourcekey="selectAllText" /></a> &middot; <a href="javascript:void(0)" class="deselectAllLink"><Corbis:Localize ID="deselectAllLabel" runat="server" meta:resourcekey="deselectAllText" /></a></div>
						    <div class="elementFooter"><div class="right"> </div></div>

					    </div>
				    </ItemTemplate>
			    </asp:Repeater>
            </div>
            
            <div style="float:right">
                <Corbis:GlassButton  ID="extendedSearch" runat="server" ButtonBackground="gray36"
                    OnClientClick="LogOmnitureEvent('event6');return CorbisUI.ExtendedSearch.combineSearchBuddy();"
                    meta:resourcekey="search"
                />&nbsp;
                <Corbis:GlassButton ID="cancelButton" runat="server" Buttonstyle="Gray"
                    Onclientclick="CorbisUI.ExtendedSearch.cancelMSOChanges();CorbisUI.ExtendedSearch.hideMSOWindow(true);return false;" ButtonBackground="gray36"
                    meta:resourcekey="cancelButton"
                />
            </div>
            </div>
        </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:HiddenField ID="showOptionsAppliedStyle" runat="server" Value="false" />
    
    
        
</div>
<div id="searchFlyout" style="display:none;width:190px;position:absolute;top:182px;left:20px;z-index: 9999999;" onmousedown="CorbisUI.ExtendedSearch.vars._isSearchFlyoutMousedown=true;event.cancelBubble=true" onclick="CorbisUI.ExtendedSearch.testCategoriesChecked(true);">
    <div style="background:#666666;padding:0px 0px 10px 10px;position:relative;top:9px" >
        <Corbis:ImageCheckbox ID="creative" runat="server" Checked="true" meta:resourceKey="creative"  Style="font-weight: bold" TextStyle="Heading"/>
        <div style="border-top: 1px dotted #000;height: 1px;width: 170px;margin:10px 0px;">
            <img alt="" class="imgDivider" src="../Images/spacer.gif" />
        </div>
        <Corbis:ImageCheckbox style="font-weight:bold" ID="editorial" runat="server" Checked="true" meta:resourceKey="editorial" OnClientChanged="CorbisUI.ExtendedSearch.EditorialChanged_flyout(this.checked)" TextStyle="Heading" />
        <div class="ML_20" id="EditorialChildrenDiv" style="padding-right: 0px">
            <Corbis:ImageCheckbox ID="documentary" style="color:#dedede" runat="server" Checked="true" meta:resourceKey="documentary" OnClientChanged="CorbisUI.ExtendedSearch.setEditorialCheckedState_flyout(this.checked)" />
            <Corbis:ImageCheckbox ID="fineArt" runat="server" Checked="true" meta:resourceKey="fineArt" OnClientChanged="CorbisUI.ExtendedSearch.setEditorialCheckedState_flyout(this.checked)"  Style="color: #dedede"/>
            <Corbis:ImageCheckbox ID="archival" runat="server" Checked="true" meta:resourceKey="archival" OnClientChanged="CorbisUI.ExtendedSearch.setEditorialCheckedState_flyout(this.checked)" Style="color: #dedede" />
            <Corbis:ImageCheckbox ID="currentEvents" runat="server" Checked="true" meta:resourceKey="currentEvents" OnClientChanged="CorbisUI.ExtendedSearch.setEditorialCheckedState_flyout(this.checked)" Style="color: #dedede" />
            <Corbis:ImageCheckbox ID="entertainment" runat="server" Checked="true" meta:resourceKey="entertainment" OnClientChanged="CorbisUI.ExtendedSearch.setEditorialCheckedState_flyout(this.checked)"  Style="color: #dedede"/>
            <Corbis:ImageCheckbox ID="outline" runat="server" Checked="true" meta:resourceKey="outline" OnClientChanged="CorbisUI.ExtendedSearch.setEditorialCheckedState_flyout(this.checked)"  Style="color: #dedede"/>
        </div>
        <div style="border-top: 1px dotted #000;height: 1px;width: 170px;margin:10px 0px;">
            <img alt="" class="imgDivider" src="../Images/spacer.gif" />
        </div>
        <Corbis:ImageCheckbox ID="rightsManaged" TextStyle="Heading" style="font-weight:bold" runat="server" Checked="true" meta:resourceKey="rightsManaged" />
        <Corbis:ImageCheckbox style="margin-bottom:-11px;font-weight:bold" ID="royaltyFree" runat="server" Checked="true" meta:resourceKey="royaltyFree" TextStyle="Heading" />
        <!-- END - LICENSE TYPES -->
   </div>
</div>

<Corbis:ModalPopup ContainerID="noSearchResultsWarning" runat="server" meta:resourcekey="noSearchResultsWarning">
    <div id="noSearchResultsWrapper" onmousedown="_isSearchFlyoutMousedown = true;event.cancelBubble = true">
        <p class="searchWarning">
            <Corbis:Label ID="searchWarningIntro" runat="server" meta:resourcekey="searchWarningIntro" />
        </p>
        
        <p class="searchWarning" textKey=Category>
            <Corbis:Label ID="noCategoryErr" runat="server" meta:resourcekey="noCategoryErr" />
        </p>
        <p class="searchWarning" textKey=License style="display:none">
            <Corbis:Label ID="noLicenseErr" runat="server" meta:resourcekey="noLicenseErr" />
        </p>
        <p class="searchWarning" textKey=Color style="display:none">
            <Corbis:Label ID="Label3" runat="server" meta:resourcekey="noColorErr" />
        </p>
        <p class="searchWarning" textKey=Photo style="display:none">
            <Corbis:Label ID="Label4" runat="server" meta:resourcekey="noPhotoErr" />
        </p>
        <p class="searchWarning" textKey=NotNumeric style="display:none">
            <Corbis:Label ID="notNumericErr" runat="server" meta:resourcekey="notNumericErr" />
        </p>
        <p class="searchWarning" textKey=NotDate style="display:none">
            <Corbis:Label ID="notDateErr" runat="server" meta:resourcekey="notDateErr" />
        </p>
        <p class="searchWarning" textKey=TooLateDate style="display:none">
            <Corbis:Label ID="tooLateDateErr" runat="server" meta:resourcekey="tooLateDateErr" />
        </p>
          <p class="searchWarning" textKey=DateErr style="display:none">
            <Corbis:Label ID="DateErr" runat="server" meta:resourcekey="DateErr" />
        </p>
        <p style="color:#cc0000"><IMG SRC="/Images/redleftarrow.gif" style="margin-right:5px;vertical-align:middle" />
            <Corbis:Label ID="Label1" runat="server" meta:resourcekey="changeSelection" />
        </p>
        <p style="padding-top:3px">
            <Corbis:GlassButton ID="CloseSearchError" runat="server"  OnClientClick="MochaUI.CloseModal('noSearchResultsWarning');return false;" meta:resourcekey="CloseSearchError" />
        </p>
    </div>
</Corbis:ModalPopup>

<script language="javascript" type="text/javascript">

CorbisUI.GlobalVars.Search = {
    showOptionsAppliedStyle: '<%= showOptionsAppliedStyle.ClientID %>',
    sb_KeywordSearch: $('<%= keywordSearch.ClientID %>'),
    sb_SearchImages: '<%= GetLocalResourceObject("SearchImages")%>',
    sb_EmptySearchStringAlert: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(GetLocalResourceObject("EmptySearchStringAlert").ToString())%>',
    sb_ImageNumbers: '<%=imageNumbers.ClientID %>'
};

    CorbisUI.ExtendedSearch.vars.SearchUri = '<%= Corbis.Web.UI.SiteUrls.SearchResults %>';
    CorbisUI.ExtendedSearch.vars.HiddenMSOTrigger = $('<%= this.hiddenMSOTrigger.ClientID %>');
    CorbisUI.ExtendedSearch.vars.HiddenMSOValue = $('<%= this.hiddenMSOValue.ClientID %>');
    CorbisUI.ExtendedSearch.vars.HiddenMSOValue.value = "UnOpened";
    CorbisUI.ExtendedSearch.vars.KeywordSearch = $('<%= keywordSearch.ClientID %>');
    CorbisUI.ExtendedSearch.vars.LastMSOFilters = '<%= MsoSearchParameters %>';
    CorbisUI.ExtendedSearch.vars.ImageNumbers = '<%=imageNumbers.ClientID %>';
    CorbisUI.ExtendedSearch.vars.SearchImages = '<%= GetLocalResourceObject("SearchImages")%>';
    CorbisUI.ExtendedSearch.vars.EmptySearchStringAlert = '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(GetLocalResourceObject("EmptySearchStringAlert").ToString())%>';
    CorbisUI.ExtendedSearch.vars.checkBoxIDS.creative = '<%=creative.ClientID %>';
    CorbisUI.ExtendedSearch.vars.checkBoxIDS.editorial = '<%=editorial.ClientID %>';
    CorbisUI.ExtendedSearch.vars.checkBoxIDS.documentary = '<%=documentary.ClientID %>';
    CorbisUI.ExtendedSearch.vars.checkBoxIDS.fineArt = '<%=fineArt.ClientID %>';
    CorbisUI.ExtendedSearch.vars.checkBoxIDS.archival = '<%=archival.ClientID %>';
    CorbisUI.ExtendedSearch.vars.checkBoxIDS.currentEvents = '<%=currentEvents.ClientID %>';
    CorbisUI.ExtendedSearch.vars.checkBoxIDS.entertainment = '<%=entertainment.ClientID %>';
    CorbisUI.ExtendedSearch.vars.checkBoxIDS.outline = '<%=outline.ClientID %>';
    CorbisUI.ExtendedSearch.vars.checkBoxIDS.rmLicense = '<%=rightsManaged.ClientID %>';
    CorbisUI.ExtendedSearch.vars.checkBoxIDS.rfLicense = '<%=royaltyFree.ClientID %>';
    CorbisUI.ExtendedSearch.vars.DaysButton = '<%=daysButton.ClientID %>';
    CorbisUI.ExtendedSearch.vars.DaysText = '<%= days.ClientID %>';
    CorbisUI.ExtendedSearch.vars.MSOStartDateExtend = '<%=msoStartDateExtend.BehaviorID %>';
    CorbisUI.ExtendedSearch.vars.MSOEndDateExtend = '<%=msoEndDateExtend.BehaviorID %>';
    CorbisUI.ExtendedSearch.vars.SearchClientId = '<%=search.ClientID %>';
    CorbisUI.ExtendedSearch.vars.DeleteMSO = '<%=deleteMoreSearchOptions.ClientID %>';
    CorbisUI.ExtendedSearch.vars.ImageNumbersText = '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(GetLocalResourceObject("imageNumbers.Text").ToString())%>';
    CorbisUI.ExtendedSearch.vars.DateCreated = '<%=dateCreated.ClientID %>';
    CorbisUI.ExtendedSearch.vars.Location = '<%=location.ClientID %>';
    CorbisUI.ExtendedSearch.vars.Photographer = '<%=photographer.ClientID %>';
    CorbisUI.ExtendedSearch.vars.Provider = '<%=provider.ClientID %>';

    CorbisUI.ExtendedSearch.vars.Horizontal = '<%=horizontalCheckbox.ClientID %>';
    CorbisUI.ExtendedSearch.vars.Vertical = '<%=verticalCheckbox.ClientID %>';
    CorbisUI.ExtendedSearch.vars.Panoramic = '<%=panoramaCheckbox.ClientID %>';
    CorbisUI.ExtendedSearch.vars.PointOfView = '<%=pointOfView.ClientID %>';
    CorbisUI.ExtendedSearch.vars.NumberOfPeople = '<%=numberOfPeople.ClientID %>';
    CorbisUI.ExtendedSearch.vars.ImmediateAvailablility = '<%=immediateAvailablility.ClientID %>';

    CorbisUI.ExtendedSearch.vars.BetweenButton = '<%=betweenButton.ClientID %>';
    CorbisUI.ExtendedSearch.vars.StartDate = '<%=beginDate.ClientID %>';
    CorbisUI.ExtendedSearch.vars.EndDate = '<%=endDate.ClientID %>';
    CorbisUI.ExtendedSearch.vars.DefaultStartDate = '<%=DefaultStartDate %>';
    CorbisUI.ExtendedSearch.vars.DefaultEndDate = '<%=DefaultEndDate %>';
    CorbisUI.ExtendedSearch.vars.DateTimeFormat = '<%=Corbis.Framework.Globalization.Language.CurrentCulture.DateTimeFormat.ShortDatePattern %>';
    CorbisUI.ExtendedSearch.vars.DateSeparator = '<%=Corbis.Framework.Globalization.Language.CurrentCulture.DateTimeFormat.DateSeparator %>';
    // A "Manual Search" is one where the user has selected the criteria and hit the 'go' button.
    // If the Return to Previous Search button is showing, it is not a manual search.
    CorbisUI.ExtendedSearch.vars.IsManualSearch = <%= (!ShowReturnToPreviousSearch).ToString().ToLower() %>;

    CorbisUI.MSOSearch.vars.AlertDateText = '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(GetLocalResourceObject("AlertDateText").ToString()) %>';
    CorbisUI.MSOSearch.vars.notDateErr = '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(GetLocalResourceObject("notDateErr").ToString()) %>';

    CorbisUI.MSOSearch.vars.CollectionCountStringFormat = "<Corbis:Localize ID='CollectionCountStringFormat' runat='server' meta:resourcekey='CollectionCountStringFormat' />";
    CorbisUI.MSOSearch.vars.numberOfPeopleValue = '<%=numberOfPeople.Text %>';


    window.addEvent('domready', function() {
        // set the flyout
        if (!CorbisUI.ExtendedSearch.vars._isResultsPage) CorbisUI.ExtendedSearch.setEditorialCheckedState_flyout(true);

        CorbisUI.EnlargementTimerSearch.DecodeQuerystring();

        // Show Options Applied Style in the Search box
        CorbisUI.ExtendedSearch.showOptionAppliedStyle();
    });



</script>
