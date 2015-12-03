<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:exsl="http://exslt.org/common"
    exclude-result-prefixes="exsl">
  <xsl:import href="../templates/common.xslt"/>
  <xsl:param name="lcd" />
  <xsl:param name="css" />
  
  <xsl:output method="html" indent="yes" />
  <xsl:strip-space elements="div span a img href onclick onmouseover"/>

  <xsl:template match="/SearchTips">
    <xsl:value-of disable-output-escaping="yes" select="$css"/>
    <script type="text/javascript" src="/jslibrary/yui/yahoo-min.js"></script>
    <script type="text/javascript" src="/jslibrary/yui/dom-min.js"></script>
    <script type="text/javascript" src="/jslibrary/yui/event-min.js"></script>
    <script type="text/javascript" src="/jslibrary/yui/animation-min.js"></script>
    <script type="text/javascript" src="scripts/accordion-menu.js"></script>
    <script type="text/javascript" src="scripts/searchtips.js"></script>
    <div id="sectionLogo">
      <a id="TabLink0">
        <xsl:attribute name="Href">javascript:void(0)</xsl:attribute>
        <xsl:attribute name="onclick">ShowTab(0)</xsl:attribute>
        <xsl:attribute name="onmouseover">moveTitleSprite(0)</xsl:attribute>
        <xsl:attribute name="onmouseout">returnSprite()</xsl:attribute>
        
        <xsl:apply-templates select="PageHeaderImg/Content" />
      </a>
    </div>
    <br clear="all" />
    <div id="subhead">
      <xsl:apply-templates select="HowToSectionHeader/Content" />
    </div>
    
    <div id="keywordHead">
      <xsl:apply-templates select="KeywordSearching/KeywordImage" />
    </div>

    <br clear="all" />

    <!-- CENTER LEFT COLUMN -->
    <div id="layout_cleft_col">
      <!-- Column One -->
      <!-- Tabs -->
      <div id="tabNav">
        <span>
          <xsl:attribute name="style">
            background-image:url(/creative/searchtips/images/<xsl:value-of select="$lcd"/>/category_titles.gif);
          </xsl:attribute>

        <div id="navTitle">
          <xsl:attribute name="title">
            <xsl:value-of select="TabCallout/TabCalloutTitle"/>
          </xsl:attribute>
          <xsl:attribute name="style">
            background-image:url(/creative/searchtips/images/<xsl:value-of select="$lcd"/>/category_titles.gif);
          </xsl:attribute>
          <xsl:value-of select="TabCallout/TabCalloutTitle"/>
        </div>
        </span>
        <span>
          <xsl:attribute name="style">background-image:url(/creative/searchtips/images/<xsl:value-of select="$lcd"/>/icon_add.gif);</xsl:attribute>
          <div class="searchTipsTab" id="DivTab1">
            <a href="javascript:void(0);" id="TabLink1" onmouseover="moveTitleSprite(1)" onmouseout="returnSprite()" onclick="ShowTab(1)">
              <xsl:attribute name="style">background-image:url(/creative/searchtips/images/<xsl:value-of select="$lcd"/>/icon_add.gif);</xsl:attribute>
              <xsl:apply-templates select="Tab1/TabHeader/Content" />

            </a>
          </div>
        </span>
        <span>
          <xsl:attribute name="style">background-image:url(/creative/searchtips/images/<xsl:value-of select="$lcd"/>/icon_subtract.gif);</xsl:attribute>
        <div class="searchTipsTab" id="DivTab2">
          <a href="javascript:void(0);" id="TabLink2" onmouseover="moveTitleSprite(2)" onmouseout="returnSprite()" onclick="ShowTab(2)">
            <xsl:apply-templates select="Tab2/TabHeader/Content" />
            <xsl:attribute name="style">background-image:url(/creative/searchtips/images/<xsl:value-of select="$lcd"/>/icon_subtract.gif);</xsl:attribute>
          </a>
        </div>
        </span>
        <span>
          <xsl:attribute name="style">background-image:url(/creative/searchtips/images/<xsl:value-of select="$lcd"/>/icon_or.gif);</xsl:attribute>
        <div class="searchTipsTab" id="DivTab3">
          <a href="javascript:void(0);" id="TabLink3" onmouseover="moveTitleSprite(3)" onmouseout="returnSprite()" onclick="ShowTab(3)">
            <xsl:apply-templates select="Tab3/TabHeader/Content" />
            <xsl:attribute name="style">background-image:url(/creative/searchtips/images/<xsl:value-of select="$lcd"/>/icon_or.gif);</xsl:attribute>
          </a>
        </div>
      </span>
      
      
        <xsl:if test="$lcd = 'en-us' or $lcd = 'en-gb'">
          <span>
            <xsl:attribute name="style">background-image:url(/creative/searchtips/images/<xsl:value-of select="$lcd"/>/icon_asterisk.gif);</xsl:attribute>
            <div class="searchTipsTab" id="DivTab4">
              <a href="javascript:void(0);" id="TabLink4" onmouseover="moveTitleSprite(4)" onmouseout="returnSprite()" onclick="ShowTab(4)">
                <xsl:apply-templates select="Tab4/TabHeader/Content" />
                <xsl:attribute name="style">background-image:url(/creative/searchtips/images/<xsl:value-of select="$lcd"/>/icon_asterisk.gif);</xsl:attribute>
              </a>
            </div>
          </span>
          <span>
            <xsl:attribute name="style">background-image:url(/creative/searchtips/images/<xsl:value-of select="$lcd"/>/icon_quotes.gif);</xsl:attribute>
          <div class="searchTipsTab" id="DivTab5">
            <a href="javascript:void(0);" id="TabLink5" onmouseover="moveTitleSprite(5)" onmouseout="returnSprite()" onclick="ShowTab(5)">
              <xsl:apply-templates select="Tab5/TabHeader/Content" />
              <xsl:attribute name="style">background-image:url(/creative/searchtips/images/<xsl:value-of select="$lcd"/>/icon_quotes.gif);</xsl:attribute>
            </a>
          </div>
          </span>
        </xsl:if>
      </div>
      <div id="tabContent">
      <div id="DivContent0" class="searchTipsContent">
        <div>
          <xsl:apply-templates select="Tab0/TabContent/Content" />

        </div>
        <div style="clear:both;"></div>
      </div>
      <div id="DivContent1" class="searchTipsContent" style="display:none;">
        <div>
          <xsl:apply-templates select="Tab1/TabContent/Content" />

        </div>
        
      </div>
      <div id="DivContent2" class="searchTipsContent" style="display:none;">
        <div>
          <xsl:apply-templates select="Tab2/TabContent/Content" />
        </div>
        <div style="clear:both;"></div>
      </div>
      <div id="DivContent3" class="searchTipsContent" style="display:none;">
        <div>
          <xsl:apply-templates select="Tab3/TabContent/Content" />

        </div>
        <div style="clear:both;"></div>
      </div>
      <div id="DivContent4" class="searchTipsContent" style="display:none;">
        <div>
          <xsl:apply-templates select="Tab4/TabContent/Content" />

        </div>
        <div style="clear:both;"></div>
      </div>
      <div id="DivContent5" class="searchTipsContent" style="display:none;">
        <div>
          <xsl:apply-templates select="Tab5/TabContent/Content" />

        </div>
        <div style="clear:both;"></div>
      </div>
      </div>
      
    </div>
    <!-- END CENTER LEFT COLUMN -->
    
    <!-- RIGHT COLUMN -->
    <div id="layout_right_col">
      <!-- Keyword Searching -->
      <xsl:apply-templates select="KeywordSearching/Header/Image/Content" />
 


      <dl class="accordion-menu" id="accordion-menu">
          <xsl:for-each select="KeywordSearching/Category">




            <dt class="a-m-t">
              <xsl:attribute name="id">a-m-t-<xsl:value-of select="position()"></xsl:value-of></xsl:attribute>
              <b>
                <xsl:value-of select="Name" />
              </b>
            </dt>
            <dd class="a-m-d">
              <xsl:for-each select="Keyword">

                <div class="bd">
                  <a><xsl:attribute name="href">/search/search.aspx?txt=<xsl:value-of select="." /></xsl:attribute><xsl:value-of select="." /></a>
                </div>

              </xsl:for-each>
            </dd>

          </xsl:for-each>
         </dl>
      
    </div>
    
    <!-- END RIGHT COLUMN -->
    
    
    <!-- EXTRA LINKS AT THE BOTTOM -->
    <div class="extraLinksDiv">
      <xsl:for-each select="ExtraLinks/Link">
        <xsl:variable name="order">
          <xsl:value-of select="LinkOrder"/>
        </xsl:variable>
        <xsl:choose>
          <xsl:when test="$order = '1'">
            <a class="extraLink" style="margin-right:6px;">
              <xsl:attribute name="onclick">
                <xsl:value-of select="Href" />
              </xsl:attribute>
              <xsl:apply-templates select="Content" />
            </a>
          </xsl:when>
          <xsl:otherwise>
            <a class="extraLink">
              <xsl:attribute name="onclick">
                <xsl:value-of select="Href" />
              </xsl:attribute>
              <xsl:apply-templates select="Content" />
            </a>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:for-each>
    </div>
     
    <div class="boutiqueLink">
      <xsl:value-of select="ExtraLinks/LinkWithText/Content"/>
      <a class="extraLink">
        <xsl:attribute name="Href">
          <xsl:value-of select="ExtraLinks/LinkWithText/Link/Href"/>
        </xsl:attribute>
        <xsl:attribute name="title">
          <xsl:value-of select="ExtraLinks/LinkWithText/Link/Content"/>
        </xsl:attribute>
        <xsl:value-of select="ExtraLinks/LinkWithText/Link/Content"/>
      </a>
    </div>
 
    
    <br clear="all" />
    
  </xsl:template>
</xsl:stylesheet>
