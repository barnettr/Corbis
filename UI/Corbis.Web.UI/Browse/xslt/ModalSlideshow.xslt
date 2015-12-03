<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
	<xsl:import href="../../Tools/XSLTemplate/common.xslt"/>

  <xsl:template match="Slideshow">
		<xsl:if test="boolean(@Url)">
			<script type="text/javascript">
				var flashvars = {<xsl:value-of select="@Flashvars"/>};
				var params = {<xsl:value-of select="@Params"/>};
				var attributes = {<xsl:value-of select="@Attributes"/>};
				swfobject.embedSWF('<xsl:value-of select="@Url"/>', 'FlashContent', '<xsl:value-of select="@Width"/>', '<xsl:value-of select="@Height"/>', "9.0.0", false, flashvars, params, attributes);
				function OpenExample2(link)
				{
					new CorbisUI.Popup('<xsl:value-of select="@ID"/>', {
						showModalBackground: false,
						centerOverElement: link,
						closeOnLoseFocus: true,
						positionVert: 'middle',
						positionHoriz: 'right'
					});
				}
			</script>
		</xsl:if>
		<div class="Slideshow">
			<xsl:attribute name="id"><xsl:value-of select="@ID"/></xsl:attribute>
			<div id="FlashContent">
				<xsl:value-of select="NoFlashCopy" disable-output-escaping="yes"/>
			</div>
		</div>
  </xsl:template>
	
</xsl:stylesheet>
