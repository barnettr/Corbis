<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:import href="../templates/common.xslt"/>
  <xsl:output method="html" indent="no" />
  <xsl:strip-space elements="div span a img href onclick onmouseover"/>
  <xsl:template match="/SearchTips">
    <table cellpadding="0" cellspacing="0" border="0" align="center" width="891" style="margin-bottom: 30px;">
      <tr valign="top">
        <td>
          <!-- Column One -->
          <img src="/creative/searchtips/images/longbar.gif" width="619" height="6" alt="" />
          <br />
          <xsl:apply-templates select="PageHeaderImg/Content" />
          <br />

          <img src="http://cache.corbis.com/pro/pxCCCCCC.gif" width="619" height="1" />
          <br />

          <xsl:apply-templates select="HowToSectionHeader/Content" />
          <br />

          <!-- Tabs -->
          <div style="margin-top:21px; width:619; background-color: #262626; height:30px;">
            <div class="searchTipsTab" onclick="ShowTab(1)" id="DivTab1" style="height: 30px; width:153; background-color:#8B8E85; margin-right:2px;">
              <xsl:apply-templates select="Tab1/TabHeader/Content" />
            </div>
            <div class="searchTipsTab" onclick="ShowTab(2)" id="DivTab2" style="width:152; background-color:#FFFFFF; margin-right:2px;">
              <xsl:apply-templates select="Tab2/TabHeader/Content" />
            </div>
            <div class="searchTipsTab" onclick="ShowTab(3)" id="DivTab3" style="width:152; background-color:#FFFFFF; margin-right:2px;">
              <xsl:apply-templates select="Tab3/TabHeader/Content" />
            </div>
            <div class="searchTipsTab" onclick="ShowTab(4)" id="DivTab4" style="width:154; background-color:#FFFFFF;">
              <xsl:apply-templates select="Tab4/TabHeader/Content" />
            </div>
          </div>
          <div id="DivContent1" class="searchTipsContent">
            <div style="padding: 10px 10px 10px 10px; display:block;">
              <xsl:apply-templates select="Tab1/TabContent/Content" />
            </div>
            <div style="clear:both;margin-bottom:10px;"></div>
          </div>
          <div id="DivContent2" class="searchTipsContent" style="display:none;">
            <div style="padding: 10px 10px 10px 10px;">
              <xsl:apply-templates select="Tab2/TabContent/Content" />
            </div>
            <div style="clear:both;margin-bottom:10px;"></div>
          </div>
          <div id="DivContent3" class="searchTipsContent" style="display:none;">
            <div style="padding: 10px 10px 10px 10px;">
              <xsl:apply-templates select="Tab3/TabContent/Content" />
            </div>
            <div style="clear:both;margin-bottom:10px;"></div>
          </div>
          <div id="DivContent4" class="searchTipsContent" style="display:none;">
            <div style="padding: 10px 10px 10px 10px;">
              <xsl:apply-templates select="Tab4/TabContent/Content" />
            </div>
            <div style="clear:both;margin-bottom:10px;"></div>
          </div>


          <div class="latestSearchTipDiv">
            <div style="padding: 10px 10px 10px 10px;color: #999999">
              <xsl:apply-templates select="LatestSearchTip/HeaderImage/Content" /><br />
              <br />
              <xsl:for-each select="LatestSearchTip/Tip">
                <div class="latestTipDiv">
                  <xsl:attribute name="id">
                    LatestTip<xsl:value-of select="position()"></xsl:value-of>
                  </xsl:attribute>
                  <xsl:if test="position() &gt; 1">
                    <xsl:attribute name="style">display:none;</xsl:attribute>
                  </xsl:if>
                  <xsl:apply-templates select="Content" />
                </div>
                <div style="clear:both;"></div>
                <script language="javascript">LatestSearchTipCount++;</script>
              </xsl:for-each>
              <br />
              <a href="javascript:GoLatestSearchTip(-1)" class="latestSearchTipNav">
                <xsl:value-of select="LatestSearchTip/BackText"></xsl:value-of>
              </a>
              |
              <a href="javascript:GoLatestSearchTip(1)" class="latestSearchTipNav">
                <xsl:value-of select="LatestSearchTip/NextText"></xsl:value-of>
              </a>
            </div>
          </div>

          <xsl:for-each select="ExtraLinks/Link">
            <xsl:choose>
              <xsl:when test="@type = 'Separator'">
                <div style="height:19px;"></div>
              </xsl:when>
              <xsl:otherwise>
                <div class="extraLink" onmouseover="HighlightExtraLink(this, true)" onmouseout="HighlightExtraLink(this, false)">
                  <xsl:attribute name="onclick">
                    <xsl:value-of select="Href" />
                  </xsl:attribute>
                  <div style="padding-top:10px;">
                    <xsl:apply-templates select="Content" />
                  </div>
                </div>
              </xsl:otherwise>
            </xsl:choose>

          </xsl:for-each>


        </td>
        <td style="width:34px;">
          <!-- Spacer Column -->
        </td>
        <td>
          <img src="/creative/searchtips/images/shortbar.gif" width="238" height="6" alt="" />
          <br />
          <img src="http://cache.corbis.com/pro/pxCCCCCC.gif" width="238" height="1" style="margin-top:95px" />
          <br />
          <!-- Keyword Searching -->
          <div class="keywordSearching" style="margin-bottom:21px;">
            <xsl:apply-templates select="KeywordSearching/Header/Content" />
            <xsl:for-each select="KeywordSearching/Category">
              <div class="keywordSearchingDiv" style="display:none;">
                <xsl:attribute name="id">KeywordSearching<xsl:value-of select="position()"></xsl:value-of></xsl:attribute>

                <b>
                  <xsl:value-of select="Name" />
                </b>

                <table width="238" border="0" style="margin-top:10px;margin-bottom:20px;" cellpadding="0" cellspacing="0">
                  <tr valign="top">
                    <td width="50%">
                      <xsl:for-each select="Keyword">
                        <a>
                          <xsl:attribute name="href">
                            /search/searchFrame.aspx?txt=<xsl:value-of select="." />
                          </xsl:attribute>
                          <xsl:value-of select="." />
                        </a>
                        <br />
                        <xsl:if test="position() = (count(../Keyword) div 2) or position() = ((count(../Keyword)+ 1) div 2)">
                          <xsl:text disable-output-escaping="yes"><![CDATA[</td><td>]]></xsl:text>
                        </xsl:if>
                      </xsl:for-each>
                    </td>
                  </tr>
                </table>
              </div>
              <div style="clear:both;"></div>
              <script language="javascript">KeywordSearchingCount++;</script>
            </xsl:for-each>
            <br />
            <a href="javascript:GoKeywordSearching(-1)" class="keywordSearchingNav">
              <xsl:value-of select="KeywordSearching/BackText"></xsl:value-of>
            </a>
            |
            <a href="javascript:GoKeywordSearching(1)" class="keywordSearchingNav">
              <xsl:value-of select="KeywordSearching/NextText"></xsl:value-of>
            </a>
          </div>

          <!-- <img src="http://cache.corbis.com/pro/pxCCCCCC.gif" width="238" height="1" />
										<br /> -->

          <div class="creativeKeywordsDiv" style="display:none;">
            <xsl:apply-templates select="CreativeKeywords/Header/Content" />
            <table style="margin-top:21px; visibility:hidden;" id="ImgScrollerTable">
              <tr>
                <td style="vertical-align:top; padding-top:75px;">
                  <a href="javascript:void(0)" id="ImagescrollBack">
                    <img src="images/arrow_left.gif" id="back" style="margin-right:10px;" />
                  </a>
                </td>
                <td>
                  <div id="Imagescroll" style="width:180px; height: 250px; overflow:hidden;" >
                    <!-- Set width of this div to width of all items in it -->
                    <div id="ImagescrollInnerDiv">
                      <xsl:attribute name="style">
                        overflow:hidden;width:<xsl:value-of select="count(CreativeKeywords/Keyword)*180" />px;
                      </xsl:attribute>
                      <xsl:for-each select="CreativeKeywords/Keyword">

                        <div id="divContent" class="creativeKeywordItem" style="border:1px solid red">
                          <span style="position:absolute;z-index:10;display:none;width:170px;height:170px;background-color:#8B8E85;color:white;cursor:pointer;">
                            <xsl:attribute name="onmouseout">
                              ShowDescription(<xsl:value-of select="position()"/>, false)
                            </xsl:attribute>
                            <xsl:attribute name="onmouseover">
                              ShowDescription(<xsl:value-of select="position()"/>, true)
                            </xsl:attribute>
                            <xsl:attribute name="onclick">
                              document.location='<xsl:value-of select="Link" />'
                            </xsl:attribute>
                            <xsl:attribute name="id">
                              KeywordDescription<xsl:value-of select="position()"/>
                            </xsl:attribute>
                            <table style="height:170px;width:170px;color:white">
                              <tr>
                                <td>
                                  <xsl:value-of select="Description"/>
                                </td>
                              </tr>
                            </table>
                          </span>
                          <a>
                            <xsl:attribute name="href">
                              <xsl:value-of select="Link" />
                            </xsl:attribute>
                            <img style="position:relative; z-index:5;" border="0">
                              <xsl:attribute name="src">
                                <xsl:value-of select="Image"/>
                              </xsl:attribute>
                              <xsl:attribute name="hspace">
                                <xsl:value-of select="floor((170-ImageWidth) div 2)" />
                              </xsl:attribute>
                              <xsl:attribute name="vspace">
                                <xsl:value-of select="floor((170-ImageHeight) div 2)" />
                              </xsl:attribute>
                              <xsl:attribute name="onmouseover">
                                ShowDescription(<xsl:value-of select="position()"/>, true)
                              </xsl:attribute>
                            </img>
                          </a>
                          <img src="images/px8B8E85.gif" width="170" height="170" style="position:static; margin-left: 0px; margin-top: -170px; margin-bottom: 6px; z-index:1;" />
                          <br />
                          <table style="margin-left:auto;margin-right:auto;margin-top:10px;width:170px;text-align:center;color:white;">
                            <tr>
                              <td>
                                <a style="color:white;text-decoration:none;size:+1">
                                  <xsl:attribute name="href">
                                    <xsl:value-of select="Link" />
                                  </xsl:attribute>
                                  <xsl:value-of select="Name" />
                                </a>
                              </td>
                            </tr>
                          </table>
                        </div>
                        <script>creativeKeywordItems++</script>
                      </xsl:for-each>
                    </div>
                  </div>
                </td>
                <td style="vertical-align:top; padding-top:75px;">
                  <a href="javascript:void(0)" id="ImagescrollForward">
                    <img src="images/arrow_right.gif" id="forward" />
                  </a>
                </td>
              </tr>
            </table>
          </div>
        </td>

      </tr>
    </table>
  </xsl:template>


</xsl:stylesheet>
