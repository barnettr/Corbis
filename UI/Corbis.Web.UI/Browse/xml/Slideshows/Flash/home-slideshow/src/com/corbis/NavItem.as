package com.corbis {
	import flash.display.Sprite;
	import flash.display.GradientType;
	import flash.display.SpreadMethod;
	import flash.display.Shape;
	import flash.geom.Matrix;
	import flash.events.MouseEvent;
	import flash.events.Event;
	import flash.text.TextField;
	import flash.text.TextFieldAutoSize;
	import flash.text.TextFormatAlign;
	import flash.text.TextFormat;
	import flash.text.AntiAliasType;
	
	// Mine
	import com.corbis.CustomEvent;
	
	// 3rd Pary
	/*import com.reintroducing.events.CustomEvent;*/
	import gs.TweenLite;
	
	public class NavItem extends Sprite {
		private var _id:uint;
		private var _regularBackground:Sprite = new Sprite();
		private var _selectedBackground:Sprite = new Sprite();
		private var _titleText:TextField = new TextField();
		private var _selected:Boolean = new Boolean();
		private const _boxWidth:uint = 329;
		public static var lastSelected:NavItem;
		public static var navBoxes:Array = [];
		
		public static var CLICKED:String = "clicked"
		
		public function NavItem(title:String) {
			this.addEventListener(MouseEvent.ROLL_OVER, rollOverEvent);
			this.addEventListener(MouseEvent.ROLL_OUT, rollOutEvent);
			this.addEventListener(MouseEvent.CLICK, clickEvent);
			
			navBoxes.push(this);
			build(title);
		}
		
		public function get id():uint {
			return _id; 
		}
		public function set id($value:uint):void {
			if ($value !== _id) {
				_id = $value;
			}
		}
		public function get selected():Boolean {
			return _selected; 
		}

		public function set selected(value:Boolean):void {
			if (value !== _selected) {
				_selected = value;
			}
		}
		
		private function build($title:String):void{
			
			var textContainer:Sprite = new Sprite();
				textContainer.addChild(_selectedBackground);
				textContainer.addChild(_regularBackground);
			
			var regularBackgroundShape:Shape = new Shape();
			
			var regularFillType:String = GradientType.LINEAR;
			var regularColors:Array = [0x444444, 0x333333, 0x444444];
			var regularAlphas:Array = [1, 1, 1];
			var regularRatios:Array = [0x00, 0x88, 0xFF];
			var regularMatr:Matrix = new Matrix();
				regularMatr.createGradientBox(_boxWidth, 29, 4.71, 0, 0);
			var regularSpreadMethod:String = SpreadMethod.PAD;

				regularBackgroundShape.graphics.beginGradientFill(regularFillType, regularColors, regularAlphas, regularRatios, regularMatr, regularSpreadMethod);  
				regularBackgroundShape.graphics.lineStyle(1, 0x515151, 1, true);
				regularBackgroundShape.graphics.drawRoundRect(0,0,_boxWidth,29, 10);
				regularBackgroundShape.graphics.endFill();

			_regularBackground.addChild(regularBackgroundShape);
			
			var selectedBackgroundShape:Shape = new Shape();
			
			var selectedFillType:String = GradientType.LINEAR;
			var selectedColors:Array = [0x5e5e5e, 0x7d7d7d];
			var selectedAlphas:Array = [1, 1];
			var selectedRatios:Array = [0x00, 0xFF];
			var selectedMatr:Matrix = new Matrix();
				selectedMatr.createGradientBox(318, 29, 4.71, 0, 0);
			var selectedSpreadMethod:String = SpreadMethod.PAD;

				selectedBackgroundShape.graphics.beginGradientFill(selectedFillType, selectedColors, selectedAlphas, selectedRatios, selectedMatr, selectedSpreadMethod);  
				selectedBackgroundShape.graphics.lineStyle(1, 0x969696, 1, true);
				selectedBackgroundShape.graphics.drawRoundRect(0,0,_boxWidth,29, 10);
				selectedBackgroundShape.graphics.endFill();

			_selectedBackground.addChild(selectedBackgroundShape);
			
			
			var trebuchet:TrebuchetMS = new TrebuchetMS();
			
			var titleFormat:TextFormat = new TextFormat();
				titleFormat.font = trebuchet.fontName;
				titleFormat.color = 0x6699cc;
				titleFormat.size = 12;
				titleFormat.align = TextFormatAlign.CENTER;
				
			_titleText.defaultTextFormat = titleFormat;
			_titleText.text = $title;
			_titleText.selectable = false;
			_titleText.autoSize = TextFieldAutoSize.LEFT;
			_titleText.antiAliasType = AntiAliasType.ADVANCED;
			_titleText.multiline = false;
			_titleText.wordWrap = false;
			_titleText.x = (selectedBackgroundShape.width / 2) - (_titleText.width / 2);
			_titleText.y = (selectedBackgroundShape.height / 2) - (_titleText.height / 2);
				
			textContainer.addChild(_titleText);

			addChild(textContainer);
			
		}
		
		private function rollOverEvent($e:MouseEvent):void{
			if (NavItem.lastSelected != this) select();
		}
		
		private function rollOutEvent($e:MouseEvent):void{
			if (NavItem.lastSelected != this) deSelect();
		}
		
		private function clickEvent($e:MouseEvent):void{
			if (NavItem.lastSelected == this) return
			selectThisOneAndResetTheRest();
			
			var evt:CustomEvent = new CustomEvent(CustomEvent.NAV_CLICKED, {id:_id}, true);
			this.dispatchEvent(evt);
		}
		public function selectThisOneAndResetTheRest(){
			//trace("selectThisOneAndResetTheRest called on", this.name);
			select();
			NavItem.lastSelected = this;
			var item:Object
			for each (item in NavItem.navBoxes){
				if (item != NavItem.lastSelected) {
					/*trace("Running", item.name);*/
					item.deSelect();
				}
			}
		}
		
		public function select():void{
			TweenLite.to(_titleText, 0.5, {tint:0xffffff});
			TweenLite.to(_regularBackground, 0.5, {alpha:0});
		}
		public function deSelect():void {
			TweenLite.to(_titleText, 0.5, {tint:0x6699cc});
			TweenLite.to(_regularBackground, 0.5, {alpha:1});
		}
		
	}
}