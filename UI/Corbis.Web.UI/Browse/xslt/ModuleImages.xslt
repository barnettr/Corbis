<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
	<xsl:import href="../../Tools/XSLTemplate/common.xslt"/>

	<xsl:template match="ModuleImages">
		<div class="ModuleImages Module">
			<h4><xsl:value-of select="Title"/></h4>
			<ul class="Images">
				<xsl:for-each select="Images/Image[position() &lt; 5]">
					<li>
						<xsl:choose>
							<xsl:when test="position() = 1">
								<xsl:attribute name="class">first</xsl:attribute>
							</xsl:when>
							<xsl:when test="position() = last()">
								<xsl:attribute name="class">last</xsl:attribute>
							</xsl:when>
						</xsl:choose>
						<a>
							<xsl:attribute name="rel"><xsl:value-of select="@Rel"/></xsl:attribute>
							<xsl:if test="boolean(@Rel) and contains(@Rel,'popup')">
								<xsl:attribute name="target">_blank</xsl:attribute>
							</xsl:if>
							<xsl:attribute name="href"><xsl:value-of select="@Url"/></xsl:attribute>
							<xsl:attribute name="title"><xsl:value-of select="@Title"/></xsl:attribute>
							<xsl:apply-templates select="image" />
							<img>
								<xsl:attribute name="src"><xsl:value-of select="@Src"/></xsl:attribute>
								<xsl:attribute name="alt"><xsl:value-of select="@Alt"/></xsl:attribute>
                                <xsl:attribute name="title"><xsl:value-of select="@Alt"/></xsl:attribute>
							</img>
						</a>
					</li>
				</xsl:for-each>
			</ul>
			<xsl:if test="boolean(Links)">
				<ul class="Links">
					<xsl:for-each select="Links/Link">
						<li>
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
			</xsl:if>
		</div>
	</xsl:template>
</xsl:stylesheet>