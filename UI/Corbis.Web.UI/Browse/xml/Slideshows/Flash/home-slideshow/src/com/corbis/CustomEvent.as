package com.corbis {
	import flash.events.Event;
	
	public class CustomEvent extends Event {
//- PRIVATE & PROTECTED VARIABLES -------------------------------------------------------------------------
		
//- PUBLIC & INTERNAL VARIABLES ---------------------------------------------------------------------------
		
		public static const DEFAULT_NAME:String = "com.reintroducing.events.CustomEvent";
		
		// event constants
		public static const NAV_CLICKED:String = "navClicked";
		public static const SET_NAV:String = "netNav";
		public static const UNHIDE:String = "unhide"
		
		public var params:Object;
		
//- CONSTRUCTOR	-------------------------------------------------------------------------------------------
	
		public function CustomEvent($type:String, $params:Object, $bubbles:Boolean = false, $cancelable:Boolean = false) {
			super($type, $bubbles, $cancelable);
			this.params = $params;
		}
		
//- PRIVATE & PROTECTED METHODS ---------------------------------------------------------------------------
		
		
//- PUBLIC & INTERNAL METHODS -----------------------------------------------------------------------------
		
	
//- EVENT HANDLERS ----------------------------------------------------------------------------------------
	
	
//- GETTERS & SETTERS -------------------------------------------------------------------------------------
	
	
//- HELPERS -----------------------------------------------------------------------------------------------
	
		public override function clone():Event {
			return new CustomEvent(type, this.params, bubbles, cancelable);
		}
		
		public override function toString():String {
			return formatToString("CustomEvent", "params", "type", "bubbles", "cancelable");
		}
	
//- END CLASS ---------------------------------------------------------------------------------------------
	}
}