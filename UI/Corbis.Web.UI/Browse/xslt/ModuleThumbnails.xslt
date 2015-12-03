<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
	<xsl:import href="../../Tools/XSLTemplate/common.xslt"/>

	<xsl:template match="ModuleThumbnails">
		<div class="ModuleThumbnails Module">
			<h4><xsl:value-of select="Title"/></h4>
			<xsl:if test="boolean(Images)">
				<ul>
					<xsl:for-each select="Images/Image">
						<li>
							<a>
								<xsl:attribute name="rel"><xsl:value-of select="@Rel"/></xsl:attribute>
								<xsl:if test="boolean(@Rel) and contains(@Rel,'popup')">
									<xsl:attribute name="target">_blank</xsl:attribute>
								</xsl:if>
								<xsl:attribute name="href"><xsl:value-of select="Link/@Url"/></xsl:attribute>
								<xsl:attribute name="title"><xsl:value-of select="Link/@Title"/></xsl:attribute>
								<img>
									<xsl:attribute name="src"><xsl:value-of select="@Src"/></xsl:attribute>
									<xsl:attribute name="alt"><xsl:value-of select="@Alt"/></xsl:attribute>
                                    <xsl:attribute name="title"><xsl:value-of select="@Alt"/></xsl:attribute>
								</img>
								<xsl:value-of select="Link/@Text"/>
							</a>
						</li>
					</xsl:for-each>
				</ul>
			</xsl:if>
		</div>
	</xsl:template>
</xsl:stylesheet>