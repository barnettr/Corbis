<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
	<xsl:import href="../../Tools/XSLTemplate/common.xslt"/>
	<xsl:include href="Slideshow.xslt"/>

  <xsl:template match="Collection">
		<div class="Collections Node">
			<xsl:apply-templates select="Slideshow"/>
			<h3><xsl:value-of select="Title" disable-output-escaping="yes"/></h3>
			<xsl:value-of select="Copy" disable-output-escaping="yes"/>
			<xsl:if test="boolean(Links)">
				<xsl:for-each select="Links">
					<ul class="Links">
						<xsl:for-each select="Link">
							<li>
								<xsl:if test="boolean(@PreText)">
									<span class="pretext"><xsl:value-of select="@PreText"/></span>
								</xsl:if>
								<a>
									<xsl:attribute name="rel"><xsl:value-of select="@Rel"/></xsl:attribute>
									<xsl:if test="boolean(@Rel) and contains(@Rel,'popup')">
										<xsl:attribute name="target">_blank</xsl:attribute>
									</xsl:if>
									<xsl:attribute name="href"><xsl:value-of select="@Url"/></xsl:attribute>
									<xsl:attribute name="title"><xsl:value-of select="@Title"/></xsl:attribute>
									<xsl:value-of select="@Text"/>
								</a>
							</li>
						</xsl:for-each>
					</ul>
				</xsl:for-each>
			</xsl:if>
		</div>
  </xsl:template>
	
</xsl:stylesheet>
