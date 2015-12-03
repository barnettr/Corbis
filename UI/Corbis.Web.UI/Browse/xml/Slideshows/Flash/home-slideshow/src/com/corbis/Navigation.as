package com.corbis {	
	//Flash
	import flash.display.Sprite;

	// Mine
	import com.corbis.NavItem;

	public class Navigation extends Sprite {
		private var _itemArray:Array = [];
		private var _completedNav:Sprite = new Sprite();
		private var _padding:int = 3;
		private var _navItemArray:Array = [];

		public function Navigation($arrayOfItemsToUse:Array) {
			_itemArray = $arrayOfItemsToUse;
			buildNavigation();
		}

		private function buildNavigation():void {
			var navContainer:Sprite = new Sprite();

			var i:int = 0;
			for (i; i < _itemArray.length; i++) {
				var navItem:NavItem = new NavItem(_itemArray[i].title);
					navItem.id = i;
					navItem.name = _itemArray[i].imgSRC;
				if (navContainer.width == 0) {
					navItem.x = 0;
				} else {
					navItem.x = navContainer.width + _padding;
				}
				navItem.buttonMode = true;  
				navItem.useHandCursor = true;
				navItem.mouseChildren = false;
				_navItemArray.push(navItem);
				navContainer.addChild(navItem);
			}
			addChild(navContainer);

		}
		
		public function select($name:String):void{
			//trace("\n", this, ".select has been called with", $name);
			var item:Object = {};
			for each (item in _navItemArray) {
				//trace("Running ye olde loop on", item.name);
				if ($name == item.name) {
					//trace("T-T-T-T-True!");
					item.selectThisOneAndResetTheRest();
				}
			}
		}
	}

}