package com.corbis{
	import flash.display.Sprite;
	import flash.display.Bitmap;
	import flash.display.Shape;
	import flash.display.Stage;
	import flash.text.TextField;
	import flash.text.TextFieldAutoSize;
	import flash.text.TextFormatAlign;
	import flash.text.TextFormat;
	import flash.text.AntiAliasType;
	import flash.events.Event;
	import flash.events.MouseEvent;
	import flash.net.URLRequest;
	import flash.net.navigateToURL;
	import flash.external.ExternalInterface;

	// 3rd party
	import gs.TweenLite;
	
	public class Story extends Sprite {
		private var _infoObject:Object = {};
		private var _fadeToColour:uint;
		private var _container:Sprite;
		private var _myID:uint;
		private const _subheadMarginBottom:uint = 0;
		private const _titleMarginBottom:uint = 10;
		private const _paragraphMarginBottom:uint = 10
		private const _subheadFontSize:uint = 16;
		private const _titleFontSize:uint = 25;
		private const _paragraphFontSize:uint = 12;
		private const _linkFontSize:uint = 12;
		private const _textWidth:uint = 295;
		private const _backGroundBoxTopAndBottomPadding = 30;
		private const _backGroundBoxLeftAndRightPadding = 40;
		
		public function Story($image:Bitmap, $infoObject:Object){
			_infoObject = $infoObject;
			
			// This is all of the data available to you, intrepid developer.
			/*trace("\nStory Build", $infoObject.title);
						trace("------------>", $infoObject.imgSRC);
						trace("------------>", $infoObject.title);
						trace("------------>", $infoObject.subhead);
						trace("------------>", $infoObject.text);
						trace("------------>", $infoObject.linkText);
						trace("------------>", $infoObject.linkURL);
						trace("------------>", $infoObject.linkTarget);
						trace("------------>", $infoObject.attribution);
						trace("------------>", $infoObject.attributionURL);
						trace("------------>", $infoObject.options.fadeTo);
						trace("------------>", $infoObject.options.position);
						trace("------------>", $infoObject.options.verticalPosition);
						trace("------------>", $infoObject.options.textBackground);
						trace("------------>", $infoObject.options.textColor);
						trace("------------>", $infoObject.options.linkColor);
						trace("------------>", $infoObject.options.fadeTo);*/
						
			switch ($infoObject.options.fadeTo) {
				case "black" :
					_fadeToColour = 0x000000;
				break;
				case "white" :
					_fadeToColour = 0xffffff;
				break;
			}
			
			_container = new Sprite();
			_container.addChild($image);
			
			addChild(_container);
			
			buildBox();
			
		}
		
		public function get fadeToColour():uint {
			return _fadeToColour; 
		}

		public function set fadeToColour(value:uint):void {
			if (value !== _fadeToColour) {
				_fadeToColour = value;
			}
		}
		
		public function get myID():uint {
			return _myID; 
		}

		public function set myID(value:uint):void {
			if (value !== _myID) {
				_myID = value;
			}
		}
		
		private function buildBox():void {
			var contentContainer:Sprite = new Sprite();
			
			//Build out text
			var textContainer:Sprite = new Sprite();
			
			var titleSubheadColor:uint = 0
			var paragraphColor:uint = 0
			
			switch (_infoObject.options.textColor) {
				case "dark" :
					titleSubheadColor = 0x333333
					paragraphColor = 0x666666
				break;
				
				case "light" :
					titleSubheadColor = 0xffffff
					paragraphColor = 0x999999
				break
			}
			
			//Build out the text boxes
			
			//Only builds out a subhead if someone's entered it in
			if (_infoObject.subhead) {
				var subhead:Sprite = new Sprite();
				var subheadText:TextField = createTextBox(_subheadFontSize, _infoObject.subhead, titleSubheadColor);
					subhead.addChild(subheadText);
					subhead.y = 0
				textContainer.addChild(subhead);
			}
			
			//Add title
				var title:Sprite = new Sprite();
				var titleText:TextField = createTextBox(_titleFontSize, _infoObject.title, titleSubheadColor);
					title.addChild(titleText);
				if (!_infoObject.subhead) title.y = 0;
				if (_infoObject.subhead) title.y = subhead.height + _subheadMarginBottom;
					textContainer.addChild(title);
				
			//Add paragraph
				var paragraph:Sprite = new Sprite();
				var paragraphText:TextField = createTextBox(_paragraphFontSize, _infoObject.text, paragraphColor, 2);
					paragraph.addChild(paragraphText);
					paragraph.y = title.y + title.height + _titleMarginBottom;
				textContainer.addChild(paragraph);
			
			//Add Link
				var linkColor:uint = 0
				switch (_infoObject.options.linkColor) {
					case "white" : 
						linkColor = 0xffffff;
					break;
					case "blue" : 
						linkColor = 0x6699cc;
					break
				}
			
				var bottomLink:Sprite = new Sprite();
					bottomLink.buttonMode = true;
					bottomLink.mouseChildren = false;
					bottomLink.useHandCursor = true;
					bottomLink.addEventListener(MouseEvent.ROLL_OVER, rollOverEvent);
					bottomLink.addEventListener(MouseEvent.ROLL_OUT, rollOutEvent);
					bottomLink.addEventListener(MouseEvent.CLICK, clickEvent);
				var linkText:TextField = createTextBox(_linkFontSize, _infoObject.linkText, linkColor);
					bottomLink.addChild(linkText);
					bottomLink.y = paragraph.y + paragraph.height +_paragraphMarginBottom;
				textContainer.addChild(bottomLink);
				/*trace("Text container:", textContainer.height, textContainer.width);*/

			//Build out the SKU/Attribution link (this goes waaaay outside the bounding box, but is included)
				var attributionTextContainer:Sprite = new Sprite();
				
				var SKUbutton:Sprite = new Sprite();
					SKUbutton.buttonMode = true;
					SKUbutton.mouseChildren = false;
					SKUbutton.useHandCursor = true;
					SKUbutton.addEventListener(MouseEvent.ROLL_OVER, skuRollOverEvent);
					SKUbutton.addEventListener(MouseEvent.ROLL_OUT, skuRollOutEvent);
					SKUbutton.addEventListener(MouseEvent.CLICK, skuClickEvent);
					SKUbutton.addChild(createInlineTextBox(10, _infoObject.attributionName, 0x6699cc));
					SKUbutton.x = 0;
				attributionTextContainer.addChild(SKUbutton);
				
				var attribution:Sprite = new Sprite();
					attribution.addChild(createInlineTextBox(10, _infoObject.attribution, 0x999999));
					attribution.x = SKUbutton.x + SKUbutton.width;
				attributionTextContainer.addChild(attribution);
				attributionTextContainer.x = 1000 - attributionTextContainer.width - 3;
				attributionTextContainer.y = 520 - attributionTextContainer.height - 15;
				_container.addChild(attributionTextContainer);
				
			// Build the background (this is based on how much text is used, that's why it's last)
				var backgroundContainer:Sprite = new Sprite();
				var targetHeight:uint = textContainer.height + (_backGroundBoxTopAndBottomPadding * 2);
				var targetWidth:uint = textContainer.width + (_backGroundBoxLeftAndRightPadding * 2)
				/*trace("Targets:", targetHeight, targetWidth);*/
				
				var background:Shape = new Shape();
					background.graphics.beginFill(0x000000, 0.7);
					background.graphics.lineStyle(1, 0x666666, 0.6, true)
					background.graphics.drawRoundRect(0, 0, targetWidth, targetHeight, 10);
					background.graphics.endFill();
				if (_infoObject.options.textBackground == false) background.alpha = 0
				backgroundContainer.addChild(background);
				
				/*trace(backgroundContainer.height, backgroundContainer.width);*/
			
			contentContainer.addChild(backgroundContainer);
			
			//Get the background to line up properly
			textContainer.x = _backGroundBoxLeftAndRightPadding;
			textContainer.y = _backGroundBoxTopAndBottomPadding;
			
			contentContainer.addChild(textContainer);
			
			//Position the content based on options
				if (!_infoObject.options.verticalPosition) {
					contentContainer.y = (this.height / 2) - (contentContainer.height / 2)
				} else {
					contentContainer.y = _infoObject.options.verticalPosition;
				}
				
				switch (_infoObject.options.position) {
					case "right" :
						/*trace("Position right");*/
						contentContainer.x = this.width - contentContainer.width + 20;
					break;
					
					case "left" :
						/*trace("Position left");*/
						contentContainer.x = -10;
					break;
				};
				
			
			_container.addChild(contentContainer);			
			
			
		}
		
		private function createTextBox($fontSize:uint, $textToUse:String, $textColor:uint, $leading:Number = 0):TextField{
			
			var trebuchet:TrebuchetMS = new TrebuchetMS();
			
			var textFormat:TextFormat = new TextFormat();
				textFormat.font = trebuchet.fontName;
				textFormat.color = $textColor;
				textFormat.size = $fontSize;
				textFormat.leading = $leading;
			
			var textField:TextField = new TextField();	
				textField.defaultTextFormat = textFormat;
				//textField.embedFonts = true;
				textField.htmlText = $textToUse;
				textField.selectable = false;
				textField.antiAliasType = AntiAliasType.ADVANCED;
				textField.autoSize = TextFieldAutoSize.LEFT;
				textField.multiline = true;
				textField.wordWrap = true;
				textField.width = _textWidth;
				/*textField.background = true;*/
			
			return textField
		}
		
		private function createInlineTextBox($fontSize:uint, $textToUse:String, $textColor:uint):TextField{
			
			var trebuchet:TrebuchetMS = new TrebuchetMS();
			
			var textFormat:TextFormat = new TextFormat();
				textFormat.font = trebuchet.fontName;
				textFormat.color = $textColor;
				textFormat.size = $fontSize;
			
			var textField:TextField = new TextField();
				textField.embedFonts = true;
				textField.defaultTextFormat = textFormat;
				textField.text = $textToUse;
				textField.selectable = false;
				textField.antiAliasType = AntiAliasType.ADVANCED;
				textField.autoSize = TextFieldAutoSize.LEFT;
				textField.multiline = false;
				textField.width = _textWidth;
				/*textField.background = true;*/
			
			return textField
		}
		
		private function rollOverEvent($e:Event):void {
			/*trace("Read more rollover");*/
		}
		private function rollOutEvent($e:Event):void {
			/*trace("Read more rollout");*/
		}
		private function clickEvent($e:Event):void {
			trace("Read more clicked", _infoObject.title);
			
			/*
				External Interface call for some sweet Javascript action
				You have to make sure that you allow script access with:
					<param name="allowScriptAccess" value="always" />
				in your HTML.
			*/
			
			if (ExternalInterface.available) {
				try {
					ExternalInterface.call("OpenPopupWindow", _infoObject.linkURL, 900, 720);
				} 
				catch ($e:SecurityError) {
					trace("Uh oh, there was a security error:" + $e.message + "\n")
				}
				catch ($e:Error) {
					trace("Uh oh, an error occurred:" + $e.message + "\n")
				}
			} else {
				trace("Sorry, I can't seem to be able to access the external interface.")
			}
			
			
			
			
			/*var request:URLRequest = new URLRequest("http:://");*/
			//navigateToURL(request, _infoObject.linkTarget); 
		}
		private function skuRollOverEvent($e:Event):void {
			/*trace("Read more rollover");*/
		}
		private function skuRollOutEvent($e:Event):void {
			/*trace("Read more rollout");*/
		}
		private function skuClickEvent($e:Event):void {
			var request:URLRequest = new URLRequest(_infoObject.attributionURL);
			try {
				trace("I'm going to", _infoObject.attributionURL);
				navigateToURL(request, '_self'); 
			}
			catch (e:Error) {
				
			}
			
		}

		
		public function hide($speed:Number):void {
			TweenLite.to(_container, $speed, {alpha:0});
		}
		public function show($speed:Number):void {
			TweenLite.to(_container, $speed, {alpha:1});
		}
		
	}
}