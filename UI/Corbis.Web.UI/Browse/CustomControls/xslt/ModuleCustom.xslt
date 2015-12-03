<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
	<xsl:import href="../../../Tools/XSLTemplate/common.xslt"/>

	<xsl:template match="ModuleCustom">
		<div class="ModuleCustom Module">
			<h4><xsl:value-of select="Title"/></h4>
			<xsl:value-of select="Copy" disable-output-escaping="yes"/>
			<xsl:if test="boolean(Links)">
				<ul class="Links">
					<xsl:for-each select="Links/Link">
						<li>
							<a>
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