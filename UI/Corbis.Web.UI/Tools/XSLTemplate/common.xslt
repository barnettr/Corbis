<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output method="html" indent="no" />
    <xsl:template match="Content">
        <xsl:choose>
            <xsl:when test="@type='HTML'">
                <xsl:apply-templates select="." mode="HTML" />
            </xsl:when>
            <xsl:when test="@type='Flash'">
                <xsl:apply-templates select="." mode="Flash" />
            </xsl:when>
            <xsl:when test="@type='Image'">
                <xsl:apply-templates select="." mode="Image" />
            </xsl:when>
        </xsl:choose>
    </xsl:template>

    <xsl:template match="Content" mode="HTML">
        <xsl:choose>
            <xsl:when test="*">
                <xsl:copy-of select="node()"  />
            </xsl:when>
            <xsl:otherwise>
                <xsl:value-of select="." disable-output-escaping="yes" />
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>

    <xsl:template match="Content" mode="Flash">
        <xsl:call-template name="renderFlash">
            <xsl:with-param name="flashpath" select="FlashPath" />
            <xsl:with-param name="bgcolor" select="BackgroundColor" />
            <xsl:with-param name="version" select="FlashVersion" />
            <xsl:with-param name="height" select="FlashHeight" />
            <xsl:with-param name="width" select="FlashWidth" />
            <xsl:with-param name="noflash" select="NoFlashContent" />
            <xsl:with-param name="parentname" select="name(..)" />
            <xsl:with-param name="position">
                <xsl:number count="Content"/>
            </xsl:with-param>
        </xsl:call-template>
    </xsl:template>

    <xsl:template match="Content" mode="Image">
        <xsl:call-template name="renderImage">
            <xsl:with-param name="href" select="ImageLink" />
            <xsl:with-param name="src" select="ImagePath" />
            <xsl:with-param name="alt" select="ImageAlt" />
            <xsl:with-param name="class" select="ImageClass" />
            <xsl:with-param name="style" select="ImageStyle" />
            <xsl:with-param name="width" select="ImageWidth" />
            <xsl:with-param name="height" select="ImageHeight" />
            <xsl:with-param name="onclick" select="ImageOnClick" />
            <xsl:with-param name="mouseoversrc" select="ImageMouseOver" />
            <xsl:with-param name="parentname" select="name(..)" />
            <xsl:with-param name="position">
                <xsl:number count="Content"/>
            </xsl:with-param>
        </xsl:call-template>
    </xsl:template>


    <xsl:template name="renderImage">
        <xsl:param name="href" />
        <xsl:param name="src" />
        <xsl:param name="alt" />
        <xsl:param name="class" />
        <xsl:param name="style" />
        <xsl:param name="width" />
        <xsl:param name="height" />
        <xsl:param name="onclick" />
        <xsl:param name="mouseoversrc" />
        <xsl:param name="parentname" />
        <xsl:param name="position" />
        <xsl:choose>
            <xsl:when test='$href'>
                <xsl:if test='$mouseoversrc'>
                    <script language="javascript">
                        var <xsl:value-of select="$parentname" /><xsl:value-of select="$position" />_over = new Image();
                        var <xsl:value-of select="$parentname" /><xsl:value-of select="$position" />_off = new Image();
                        <xsl:value-of select="$parentname" /><xsl:value-of select="$position" />_over.src = "<xsl:value-of select="$mouseoversrc" />";
                        <xsl:value-of select="$parentname" /><xsl:value-of select="$position" />_off.src = "<xsl:value-of select="$src" />";
                    </script>
                </xsl:if>
                <a>
                    <xsl:attribute name="href">
                        <xsl:value-of select="$href" />
                    </xsl:attribute>
                    <xsl:if test='$onclick'>
                        <xsl:attribute name="onclick">
                            <xsl:value-of select="$onclick" />
                        </xsl:attribute>
                    </xsl:if>
                    <img src="{$src}" border="0">
                        <xsl:if test='$alt'>
                            <xsl:attribute name="alt">
                                <xsl:value-of select="$alt" />
                            </xsl:attribute>
                        </xsl:if>
                        <xsl:if test='$width'>
                            <xsl:attribute name="width">
                                <xsl:value-of select="$width" />
                            </xsl:attribute>
                        </xsl:if>
                        <xsl:if test='$height'>
                            <xsl:attribute name="height">
                                <xsl:value-of select="$height" />
                            </xsl:attribute>
                        </xsl:if>
                        <xsl:if test='$style'>
                            <xsl:attribute name="style">
                                <xsl:value-of select="$style" />
                            </xsl:attribute>
                        </xsl:if>
                        <xsl:if test='$class'>
                            <xsl:attribute name="class">
                                <xsl:value-of select="$class" />
                            </xsl:attribute>
                        </xsl:if>
                        <xsl:if test='$mouseoversrc'>
                            <xsl:attribute name="onmouseover">
                                SwitchImage(this, <xsl:value-of select="$parentname" /><xsl:value-of select="$position" />_over.src);
                            </xsl:attribute>
                            <xsl:attribute name="onmouseout">
                                SwitchImage(this, <xsl:value-of select="$parentname" /><xsl:value-of select="$position" />_off.src);
                            </xsl:attribute>
                        </xsl:if>
                    </img>
                </a>
            </xsl:when>
            <xsl:otherwise>
                <img src="{$src}" border="0">
                    <xsl:if test='$alt'>
                        <xsl:attribute name="alt">
                            <xsl:value-of select="$alt" />
                        </xsl:attribute>
                    </xsl:if>
                    <xsl:if test='$width'>
                        <xsl:attribute name="width">
                            <xsl:value-of select="$width" />
                        </xsl:attribute>
                    </xsl:if>
                    <xsl:if test='$height'>
                        <xsl:attribute name="height">
                            <xsl:value-of select="$height" />
                        </xsl:attribute>
                    </xsl:if>
                    <xsl:if test='$style'>
                        <xsl:attribute name="style">
                            <xsl:value-of select="$style" />
                        </xsl:attribute>
                    </xsl:if>
                    <xsl:if test='$class'>
                        <xsl:attribute name="class">
                            <xsl:value-of select="$class" />
                        </xsl:attribute>
                    </xsl:if>
                </img>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>

    <xsl:template name="renderFlash">
        <xsl:param name="flashpath" />
        <xsl:param name="bgcolor" />
        <xsl:param name="version" />
        <xsl:param name="height" />
        <xsl:param name="width" />
        <xsl:param name="noflash" />
        <xsl:param name="parentname" />
        <xsl:param name="position" />
        <div>
            <xsl:attribute name="id">
                div<xsl:value-of select="$parentname" />num<xsl:value-of select="$position" />
            </xsl:attribute>
            <xsl:copy-of select="$noflash" />
        </div>

        <script type="text/javascript">
            var fo = new FlashObject("<xsl:value-of select="$flashpath" />", "div<xsl:value-of select="$parentname" />num<xsl:value-of select="$position" />flash", "<xsl:value-of select="$width" />", "<xsl:value-of select="$height" />", <xsl:value-of select="$version" />, "<xsl:value-of select="$bgcolor" />");
            fo.write("div<xsl:value-of select="$parentname" />num<xsl:value-of select="$position" />");
        </script>
    </xsl:template>
</xsl:stylesheet>

