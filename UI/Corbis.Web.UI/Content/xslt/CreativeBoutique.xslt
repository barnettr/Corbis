<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:import href="~/Tools/XSLTTemplate/common.xslt"/>
    <xsl:output method="html" indent="yes" />
    <xsl:param name="filter"></xsl:param>
    <xsl:param name="lcd"></xsl:param>
    <xsl:param name="dynpath"></xsl:param>
    <xsl:param name="loggedIn"></xsl:param>
    <xsl:strip-space elements="*"/>

    <xsl:template match="RMBoutique">
        <div id="screen"></div>
        <div id="gallery">
            <a href="#close" onclick="return showGallery(false);">
                [ <xsl:value-of select="Labels/Close"/> ]
            </a>
            <iframe id="galleryFrame" frameborder="0" scrolling="no"></iframe>
        </div>
        <script>
            var tabs = new Array( <xsl:apply-templates mode="tabsArray" select="." /> );
            var tabCounts = new Array( <xsl:apply-templates mode="tabCountsArray" select="." /> );
            var blogNavOlder = '<xsl:value-of select="Labels/Older"/>';
            var blogNavNewer = '<xsl:value-of select="Labels/Newer"/>';
        </script>

        <table border="0" width="940" cellpadding="0" cellspacing="0" class="rmb">
            <!-- feature -->
            <tr>
                <td width="600">
                    <img width="600" height="400">
                        <xsl:attribute name="src">
                            images/features/<xsl:value-of select="Features/Feature[1]/FeatureImage" />
                        </xsl:attribute>
                        <xsl:attribute name="alt">
                            <xsl:value-of select="Features/Feature[1]/FeatureTitle" />
                        </xsl:attribute>
                    </img>
                </td>
                <td width="20">
                    <img src="images/pixel.gif" width="20" height="1" />
                </td>
                <td width="320" valign="top" class="mainFeature">
                    <!-- hero feature -->
                    <img width="320" height="116">
                        <xsl:attribute name="src">
                            content/<xsl:value-of select="$lcd" />/title.gif
                        </xsl:attribute>
                        <xsl:attribute name="alt">
                            <xsl:value-of select="Labels/CreativeBoutique" />
                        </xsl:attribute>
                    </img>
                    <xsl:apply-templates mode="featureBuilder" select="Features/Feature[1]" />
                </td>
            </tr>
            <!-- spacer -->
            <tr>
                <td colspan="3" height="20">
                    <a name="features"></a>
                    <img src="images/pixel.gif" width="1" height="20" />
                </td>
            </tr>
            <!-- channel headers -->
            <tr>
                <th>
                    <xsl:value-of select="Labels/Features" />
                    <!--<img height="32">
						<xsl:attribute name="src">content/en-us/header_features.gif</xsl:attribute>
						<xsl:attribute name="alt"><xsl:value-of select="Labels/Features" /></xsl:attribute>
					</img>-->
                </th>
                <td>
                    <img src="images/pixel.gif" width="1" height="1" />
                </td>
                <th style="background-image:url('images/bg_header2.gif');">
                    <xsl:value-of select="Labels/Wallpapers" />
                    <!--<img height="32">
						<xsl:attribute name="src">content/en-us/header_wallpapers.gif</xsl:attribute>
						<xsl:attribute name="alt"><xsl:value-of select="Labels/Wallpapers" /></xsl:attribute>
					</img>-->
                </th>
            </tr>
            <!-- channel content -->
            <tr>
                <td>
                    <xsl:apply-templates mode="blog" select="." />
                </td>
                <td>
                    <img src="images/pixel.gif" width="1" height="1" />
                </td>
                <td>
                    <xsl:apply-templates mode="right" select="." />
                </td>
            </tr>
            <!-- spacer -->
            <tr>
                <td colspan="3" height="20">
                    <img src="images/pixel.gif" width="1" height="20" />
                </td>
            </tr>
            <!-- new collections -->
            <tr>
                <th colspan="3" style="background-image:url('images/bg_header2.gif');">
                    <xsl:value-of select="Labels/NewCollections" />
                    <!--<img height="32">
						<xsl:attribute name="src">content/en-us/header_collections.gif</xsl:attribute>
						<xsl:attribute name="alt"><xsl:value-of select="Labels/NewCollections" /></xsl:attribute>
					</img>-->
                </th>
            </tr>
            <tr>
                <td colspan="3">
                    <xsl:apply-templates mode="collections" select="." />
                </td>
            </tr>
        </table>

        <div id="screen"></div>
        <div id="gallery">
            <a href="#close" onclick="return showGallery(false);">
                [ <xsl:value-of select="Labels/Close"/> ]
            </a>
            <iframe id="galleryFrame" frameborder="0" scrolling="no"></iframe>
        </div>
    </xsl:template>


    <xsl:template match="*" mode="tabsArray">
        <xsl:for-each select="Categories/Category">
            '<xsl:value-of select="@name" />'<xsl:if test="not(position() = last())">,</xsl:if>
        </xsl:for-each>
    </xsl:template>

    <xsl:variable name="PastFeatures" select="document($dynpath)"></xsl:variable>

    <xsl:template match="*" mode="tabCountsArray">
        '<xsl:value-of select="count(//Feature[position()>1]) + count($PastFeatures//Features//Feature[position()>1])"/>',
        <xsl:variable name="heroCategory">
            <xsl:value-of select="//Feature[1]/FeatureCategory"/>
        </xsl:variable>
        <xsl:for-each select="Categories/Category[@name!='all']">
            <xsl:variable name="name">
                <xsl:value-of select="@name"/>
            </xsl:variable>
            <xsl:choose>
                <xsl:when test="$name=$heroCategory">
                    '<xsl:value-of select="count(//Feature[./FeatureCategory=$name])+ count($PastFeatures//Features/Feature[./FeatureCategory=$name])-1"/>'
                </xsl:when>
                <xsl:otherwise>
                    '<xsl:value-of select="count(//Feature[./FeatureCategory=$name]) + count($PastFeatures//Features/Feature[./FeatureCategory=$name])"/>'
                </xsl:otherwise>
            </xsl:choose>
            <xsl:if test="not(position() = last())">,</xsl:if>
        </xsl:for-each>
    </xsl:template>


    <xsl:template match="*" mode="blog">
        <div id="blogNav">
            <xsl:for-each select="//Category">
                <div class="blogNavItem">
                    <xsl:attribute name="id">
                        <xsl:value-of select="@name"/>Filter
                    </xsl:attribute>
                    <a>
                        <xsl:attribute name="href">
                            #<xsl:value-of select="@name"/>
                        </xsl:attribute>
                        <xsl:attribute name="onclick">
                            return setFilter('<xsl:value-of select="@name"/>',1,true);
                        </xsl:attribute>
                        <xsl:value-of select="."/>
                    </a>
                </div>
            </xsl:for-each>
        </div>

        <div id="featuresDiv">
            <!-- the "all" category -->
            <div id="features_allThingDiv" class="featureCategories">
                <xsl:for-each select="//Feature[position()>1]">
                    <div class="feature">
                        <img width="225" height="150" align="left">
                            <xsl:attribute name="src">
                                /creative/tools/getResizedImage.aspx?src=/creative/boutique/images/features/<xsl:value-of select="FeatureImage"/>&amp;size=225
                            </xsl:attribute>
                            <xsl:attribute name="alt">
                                <xsl:value-of select="FeatureTitle"/>
                            </xsl:attribute>
                        </img>
                        <xsl:apply-templates mode="featureBuilder" select="."/>
                    </div>
                    <xsl:text>&#xa;</xsl:text>
                    <xsl:text>&#xa;</xsl:text>
                </xsl:for-each>

                <!-- PAST FEATURES -->
                <xsl:variable name="PastFeatures" select="document($dynpath)"></xsl:variable>
                <xsl:if test="$PastFeatures">
                    <xsl:for-each select="$PastFeatures//Features//Feature[position()>0]">
                        <div class="feature">
                            <img width="225" height="150" align="left">
                                <xsl:attribute name="src">
                                    /creative/tools/getResizedImage.aspx?src=/creative/boutique/images/features/<xsl:value-of select="FeatureImage"/>&amp;size=225
                                </xsl:attribute>
                                <xsl:attribute name="alt">
                                    <xsl:value-of select="FeatureTitle"/>
                                </xsl:attribute>
                            </img>
                            <xsl:apply-templates mode="pastFeatureBuilder" select="."/>
                        </div>
                        <xsl:text>&#xa;</xsl:text>
                        <xsl:text>&#xa;</xsl:text>
                    </xsl:for-each>
                </xsl:if>
                <!-- END PAST FEATURES -->
            </div>
            <xsl:text>&#xa;</xsl:text>
            <xsl:text>&#xa;</xsl:text>
            <!-- END the "all" category -->


            <!-- filtered by category -->
            <xsl:for-each select="//Category[@name!='all']">
                <div class="featureCategories">
                    <xsl:attribute name="id">
                        features_<xsl:value-of select="@name"/>ThingDiv
                    </xsl:attribute>
                    <xsl:variable name="category">
                        <xsl:value-of select="@name"/>
                    </xsl:variable>
                    <xsl:for-each select="//Feature[position()>1][./FeatureCategory=$category]">
                        <div class="feature">
                            <img width="225" height="150" align="left">
                                <xsl:attribute name="src">
                                    /creative/tools/getResizedImage.aspx?src=/creative/boutique/images/features/<xsl:value-of select="FeatureImage"/>&amp;size=225
                                </xsl:attribute>
                                <xsl:attribute name="alt">
                                    <xsl:value-of select="FeatureTitle"/>
                                </xsl:attribute>
                            </img>
                            <xsl:apply-templates mode="featureBuilder" select="."/>
                        </div>
                        <xsl:text>&#xa;</xsl:text>
                        <xsl:text>&#xa;</xsl:text>
                    </xsl:for-each>

                    <!-- PAST FEATURES by Catgegory -->

                    <xsl:variable name="PastFeatures" select="document($dynpath)"></xsl:variable>
                    <xsl:if test="$PastFeatures">
                        <xsl:variable name="pastCategory">
                            <xsl:value-of select="@name"/>
                        </xsl:variable>
                        <xsl:for-each select="$PastFeatures//Features//Feature[./FeatureCategory=$category]">
                            <div class="feature">

                                <img width="225" height="150" align="left">
                                    <xsl:attribute name="src">
                                        /creative/tools/getResizedImage.aspx?src=/creative/boutique/images/features/<xsl:value-of select="FeatureImage"/>&amp;size=225
                                    </xsl:attribute>
                                    <xsl:attribute name="alt">
                                        <xsl:value-of select="FeatureTitle"/>
                                    </xsl:attribute>
                                </img>

                                <xsl:apply-templates mode="pastFeatureBuilder" select="."/>


                            </div>
                            <xsl:text>&#xa;</xsl:text>
                            <xsl:text>&#xa;</xsl:text>
                        </xsl:for-each>
                    </xsl:if>
                    <!-- END PAST FEATURES -->
                </div>
            </xsl:for-each>
            <!-- END filtered by category -->
        </div>

        <div id="blogPagination"></div>
    </xsl:template>


    <xsl:template match="Feature" mode="featureBuilder">
        <!-- header -->
        <p class="featureHead">
            <xsl:value-of select="FeatureTitle" disable-output-escaping="yes" />
            <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
            <sup>
                <xsl:value-of select="FeatureSuperscript" disable-output-escaping="yes"/>
            </sup>
        </p>
        <p class="featureDesc">
            <!-- category -->
            <b>
                <xsl:apply-templates mode="categories" select=".">
                    <xsl:with-param name="text" select="FeatureCategory" />
                </xsl:apply-templates>
            </b>&#160;&#8226;&#160;<xsl:value-of select="FeatureText" disable-output-escaping="yes"/>
            <xsl:for-each select="FeatureLink">
                <br />
                <span class="categoryLink">
                    <xsl:if test="($loggedIn and contains(.,'.pdf')) or not(contains(.,'.pdf'))">
                        <!-- <a ... --><xsl:text disable-output-escaping="yes">&lt;</xsl:text>a href="<xsl:value-of select="." />"<xsl:if test="contains(.,'gallery.aspx')"> onclick="return showGallery(true,this.href);"</xsl:if><xsl:if test="contains(.,'.pdf')"> target="_blank"</xsl:if>
                        <xsl:text disable-output-escaping="yes">&gt;</xsl:text>
                    </xsl:if>
                    <xsl:choose>
                        <xsl:when test="contains(.,'gallery.aspx')">
                            <xsl:value-of select="//LinkTexts/LinkText[@name='gallery']"/>
                        </xsl:when>
                        <xsl:when test="contains(.,'searchFrame.aspx')">
                            <xsl:value-of select="//LinkTexts/LinkText[@name='search']"/>
                        </xsl:when>
                        <xsl:when test="contains(.,'.pdf')">
                            <xsl:value-of select="//LinkTexts/LinkText[@name='download']"/>&#160;(<xsl:value-of select="@filesize"/>&#160;<xsl:value-of select="@filetype"/>)
                        </xsl:when>
                        <xsl:otherwise>[error: link text not found]</xsl:otherwise>
                    </xsl:choose>

                    <xsl:if test="($loggedIn and contains(.,'.pdf')) or not(contains(.,'.pdf'))">
                        <!-- /a -->
                        <xsl:text disable-output-escaping="yes">&lt;</xsl:text>/a<xsl:text disable-output-escaping="yes">&gt;</xsl:text>
                    </xsl:if>
                </span>
            </xsl:for-each>

            <!-- opt login text for downloads -->
            <xsl:if test="not($loggedIn) and contains(.,'.pdf')">
                <div class="signIn">
                    <xsl:value-of select="//Labels/SignInText" disable-output-escaping="yes"/>
                </div>
            </xsl:if>
        </p>
    </xsl:template>


    <xsl:template match="Feature" mode="pastFeatureBuilder">
        <!-- header -->
        <p class="featureHead">
            <xsl:value-of select="FeatureTitle" disable-output-escaping="yes" />
            <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
            <sup>
                <xsl:value-of select="FeatureSuperscript" disable-output-escaping="yes"/>
            </sup>
        </p>
        <p class="featureDesc">
            <!-- category -->
            <b>
                <xsl:apply-templates mode="categories" select=".">
                    <xsl:with-param name="text" select="FeatureCategory" />
                </xsl:apply-templates>
            </b>&#160;&#8226;&#160;<xsl:value-of select="FeatureText" disable-output-escaping="yes"/>
            <xsl:for-each select="FeatureLink">
                <br />
                <span class="categoryLink">
                    <xsl:if test="($loggedIn and contains(.,'.pdf')) or not(contains(.,'.pdf'))">
                        <xsl:text disable-output-escaping="yes">&lt;</xsl:text>a href="<xsl:value-of select="." />"<xsl:if test="contains(.,'gallery.aspx')"> onclick="return showGallery(true,this.href);"</xsl:if><xsl:if test="contains(.,'.pdf')"> target="_blank"</xsl:if>
                        <xsl:text disable-output-escaping="yes">&gt;</xsl:text>
                    </xsl:if>
                    <xsl:choose>
                        <xsl:when test="contains(.,'gallery.aspx')">
                            <xsl:value-of select="//LinkTexts/LinkText[@name='gallery']"/>
                        </xsl:when>
                        <xsl:when test="contains(.,'searchFrame.aspx')">
                            <xsl:value-of select="//LinkTexts/LinkText[@name='search']"/>
                        </xsl:when>
                        <xsl:when test="contains(.,'.pdf')">
                            <xsl:value-of select="//LinkTexts/LinkText[@name='download']"/>&#160;(<xsl:value-of select="@filesize"/>&#160;<xsl:value-of select="@filetype"/>)
                        </xsl:when>
                        <xsl:otherwise>[error: link text not found]</xsl:otherwise>
                    </xsl:choose>

                    <xsl:if test="($loggedIn and contains(.,'.pdf')) or not(contains(.,'.pdf'))">
                        <!-- /a -->
                        <xsl:text disable-output-escaping="yes">&lt;</xsl:text>/a<xsl:text disable-output-escaping="yes">&gt;</xsl:text>
                    </xsl:if>
                </span>
            </xsl:for-each>

            <!-- opt login text for downloads -->
            <xsl:if test="not($loggedIn) and contains(.,'.pdf')">
                <div class="signIn">
                    <xsl:value-of select="//Labels/SignInText" disable-output-escaping="yes"/>
                </div>
            </xsl:if>
        </p>
    </xsl:template>


    <xsl:template match="*" mode="right">
        <div id="wallpaper">
            <a target="_blank">
                <xsl:attribute name="href">
                    <xsl:value-of select="Wallpaper/WallpaperFile"/>
                </xsl:attribute>
                <img width="314" height="341" style="margin:2px;">
                    <xsl:attribute name="src">
                        images/wallpaper/<xsl:value-of select="Wallpaper/WallpaperImage"/>
                    </xsl:attribute>
                    <xsl:attribute name="alt">
                        <xsl:value-of select="Wallpaper/WallpaperDownloadText" disable-output-escaping="yes"/>
                    </xsl:attribute>
                </img>
            </a>
            <p class="downloadWallpaper">
                <a target="_blank">
                    <xsl:attribute name="href">
                        <xsl:value-of select="Wallpaper/WallpaperFile"/>
                    </xsl:attribute>
                    <xsl:value-of select="Wallpaper/WallpaperDownloadText" disable-output-escaping="yes"/>
                </a>
            </p>
        </div>

        <xsl:apply-templates mode="adverts" select="."/>

        <div id="searchTips">
            <p>
                <img>
                    <xsl:attribute name="src">
                        content/<xsl:value-of select="$lcd" />/searchanywhere.gif
                    </xsl:attribute>
                    <xsl:attribute name="alt">
                        <xsl:value-of select="SearchTips/SearchTips"/>
                    </xsl:attribute>
                </img>
                <br />
                <xsl:value-of select="SearchTips/SearchTipsText" disable-output-escaping="yes"/>
            </p>
        </div>

        <!-- <div id="feedback">
      <p>
        <xsl:value-of select="Feedback/FeedbackText" disable-output-escaping="yes"/>
      </p>
    </div> -->
    </xsl:template>

    <xsl:template match="*" mode="adverts">


        <xsl:for-each select="//AdvertBox">

            <div id="grayBox">
                <img>
                    <xsl:attribute name="src">
                        content/<xsl:value-of select="$lcd" />/<xsl:value-of select="AdvertBoxImage" />
                    </xsl:attribute>
                    <xsl:attribute name="alt">
                        <xsl:value-of select="AdvertBoxTitle"/>
                    </xsl:attribute>
                </img>
                <p>
                    <xsl:value-of select="AdvertBoxTextBegin" disable-output-escaping="yes"/>
                    <a>
                        <xsl:attribute name="href">
                            <xsl:value-of select="AdvertBoxLink" disable-output-escaping="yes"/>
                        </xsl:attribute>
                        <xsl:attribute name="title">
                            <xsl:value-of select="AdvertBoxTitle" disable-output-escaping="yes"/>
                        </xsl:attribute>

                        <xsl:value-of select="AdvertBoxLinkText" disable-output-escaping="yes"/>
                    </a>
                    <xsl:value-of select="AdvertBoxTextEnd" disable-output-escaping="yes"/>
                </p>
            </div>

        </xsl:for-each>
    </xsl:template>

    <xsl:template match="*" mode="collections">
        <div id="collections">
            <xsl:for-each select="//Collection">
                <div>

                    <!--<xsl:attribute name="href"><xsl:value-of select="CollectionLink" /></xsl:attribute>-->

                    <xsl:text disable-output-escaping="yes">&lt;</xsl:text>a href="<xsl:value-of select="CollectionLink" />"
                    <xsl:if test="contains(.,'gallery.aspx')"> onclick="return showGallery(true,this.href);"</xsl:if>
                    <xsl:if test="contains(.,'.pdf')"> target="_blank"</xsl:if>
                    <xsl:text disable-output-escaping="yes">&gt;</xsl:text>
                    <xsl:choose>
                        <xsl:when test="contains(.,'gallery.aspx')">
                            <xsl:value-of select="//Collection/CollectionLink[@name='gallery']"/>
                        </xsl:when>
                        <xsl:when test="contains(.,'searchFrame.aspx')">
                            <xsl:value-of select="//Collection/CollectionLink[@name='search']"/>
                        </xsl:when>


                    </xsl:choose>

                    <img height="95" width="95">
                        <xsl:attribute name="src">
                            images/collections/<xsl:value-of select="CollectionImage"/>
                        </xsl:attribute>
                        <xsl:attribute name ="alt">
                            <xsl:value-of select="CollectionName" />
                        </xsl:attribute>
                    </img>
                    <br />
                    <xsl:value-of select="CollectionName" disable-output-escaping="yes" />
                    <!-- /a -->
                    <xsl:text disable-output-escaping="yes">&lt;</xsl:text>/a<xsl:text disable-output-escaping="yes">&gt;</xsl:text>




                </div>
            </xsl:for-each>
        </div>
    </xsl:template>


    <xsl:template match="*" mode="categories">
        <xsl:param name="text" />
        <xsl:value-of select="//Categories/Category[@name=$text]" />
    </xsl:template>


    <xsl:template match="*" mode="categorylinktext">
        <xsl:param name="text" />
        <xsl:value-of select="//CategoryLinks/CategoryLinkText[@name=$text]" />
    </xsl:template>

</xsl:stylesheet>