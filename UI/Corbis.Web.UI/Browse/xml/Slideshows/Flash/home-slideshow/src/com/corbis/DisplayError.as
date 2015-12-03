package com.corbis {
	import flash.display.Sprite;
	import flash.text.TextField;
	import flash.text.TextFieldAutoSize;
	import flash.text.TextFormat;
	import flash.text.AntiAliasType;
	
	public class DisplayError extends Sprite {
		
		public function DisplayError($errorToDisplay:String) {
			var errorContainer:Sprite = new Sprite();
			var helvetica:Helvetica = new Helvetica();
			var errorFormat:TextFormat = new TextFormat();
				errorFormat.font = helvetica.fontName;
				errorFormat.color = 0xffffff;
				errorFormat.size = 25;
				
			var errorText:TextField = new TextField();
				errorText.defaultTextFormat = errorFormat;
				errorText.text = $errorToDisplay;
				errorText.selectable = false;
				errorText.autoSize = TextFieldAutoSize.LEFT;
				errorText.antiAliasType = AntiAliasType.ADVANCED;
				errorText.multiline = false;
				errorText.wordWrap = false;
			
			errorContainer.addChild(errorText);
			addChild(errorContainer);
		}

	}
}