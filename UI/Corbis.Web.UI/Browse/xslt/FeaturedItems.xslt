<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
	<xsl:import href="../../Tools/XSLTemplate/common.xslt"/>

  <xsl:template match="FeaturedItems">
		<ul class="FeaturedItems">
			<xsl:for-each select="//FeaturedItem">
				<li class="Node">
					<xsl:if test="boolean(@IsHero) and @IsHero = 'true'">
						<xsl:attribute name="class">Hero Node</xsl:attribute>
					</xsl:if>
					<xsl:if test="boolean(Image)">
						<a>
							<xsl:attribute name="rel"><xsl:value-of select="Image/Link/@Rel"/></xsl:attribute>
							<xsl:if test="boolean(Image/Link/@Rel) and contains(Image/Link/@Rel,'popup')">
								<xsl:attribute name="target">_blank</xsl:attribute>
							</xsl:if>
							<xsl:attribute name="href"><xsl:value-of select="Image/Link/@Url"/></xsl:attribute>
							<xsl:attribute name="title"><xsl:value-of select="Image/Link/@Title"/></xsl:attribute>
							<img>
								<xsl:attribute name="src"><xsl:value-of select="Image/@Src"/></xsl:attribute>
								<xsl:attribute name="alt"><xsl:value-of select="Image/@Alt"/></xsl:attribute>
                <xsl:attribute name="title"><xsl:value-of select="Image/@Alt"/></xsl:attribute>
							</img>
						</a>
						<xsl:if test="boolean(@IsHero) or Image/@AttributionLocation != 'UnderCopy'">
							<xsl:apply-templates select="Image"/>
						</xsl:if>
					</xsl:if>
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
					<xsl:if test="boolean(Image/@AttributionLocation) and Image/@AttributionLocation = 'UnderCopy'">
						<xsl:apply-templates select="Image"/>
					</xsl:if>
				</li>
			</xsl:for-each>
			</ul>
  </xsl:template>

	<xsl:template match="Image">
		<div>
			<xsl:attribute name="class">Caption <xsl:value-of select="@AttributionLocation"/></xsl:attribute>
			<xsl:if test="boolean(@Title)">
				<xsl:value-of select="@Title"/>
				<br />
			</xsl:if>
			<xsl:if test="boolean(@ImageID)">
				<a>
					<xsl:attribute name="rel"><xsl:value-of select="@Rel"/></xsl:attribute>
					<xsl:if test="boolean(@Rel) and contains(@Rel,'popup')">
						<xsl:attribute name="target">_blank</xsl:attribute>
					</xsl:if>
					<xsl:attribute name="href">/Search/SearchResults.aspx?q=<xsl:value-of select="@ImageID"/></xsl:attribute>
					<xsl:attribute name="title"><xsl:value-of select="@Title"/></xsl:attribute>
					<xsl:value-of select="@ImageID"/>
				</a>
			</xsl:if>
			<xsl:if test="boolean(@Attribution)">
				<xsl:value-of select="@Attribution"/>
			</xsl:if>
		</div>
	</xsl:template>
</xsl:stylesheet>
