/* 
	This class loads an XML stylesheet and returns the proper values as an array of Objects
*/
package com.corbis {	
	//Internal organs
	import flash.display.Sprite;
	import flash.events.Event;
	import flash.events.EventDispatcher;

	//3rd Party
	import br.com.stimuli.loading.BulkLoader;

	public class XmlLoader extends Sprite {
		private var _xmlLocation:String;	
		private var _loader:BulkLoader;
		private var _profileXML:XMLList;
		private var _items:Array = [];
		public static var LOADED:String = "loaded";
		public static var ERROR:String = "error";

		public function XmlLoader($location:String):void{
			//Create Loader
			_loader = new BulkLoader("xmlLoader");
			//Complete event listener
			_loader.addEventListener(BulkLoader.COMPLETE, onComplete);
			//Progress event listener
			/*_loader.addEventListener(BulkLoader.PROGRESS, onProgress);*/
			// Error event listener
			_loader.addEventListener(BulkLoader.ERROR, onAllError);

			grabXML($location); //Run with it
		}
		
		//Getter
		public function get items():Array{
			return _items;
		}
		
		private function grabXML($locationofXML:String):void {
			//trace("Grabbing XML");
			_loader.add($locationofXML, {type:"xml", id:"structureXML"});
			_loader.start();
		}

		private function onComplete($e:Event):void {
			//trace("XML loaded!");
			////trace(e.target.getContent("gridXML"));
			var structureXML:XML = $e.target.getXML("structureXML", true);
			var imagePath:String = structureXML.attribute("baseURL");
			//trace(imagePath);
			var loadedXML = structureXML.children();
			//trace(loadedXML);
			
			_loader.clear(); //Clear the instance
			_loader = null; //Nullify it

			/*position="left"
			vertical-position="" 
			text-background="true" 
			text-color="white" 
			link-color="blue"
			fade-to="black"*/
			var regexp:RegExp = /^(0|[1-9][0-9]*)$/;
			/*//trace(regexp.test("65645673"));*/
			
			var item:XML;
			var path:String;
			
			var i:int = 0;
			for (i; i < 3; i++) { //If you accidentally put 4 stories into the XML, it's only grabbing 3.
				var textOptions:Object = {};
				//Debug tracing to make sure that I'm getting the proper XML
				/*//trace("\n");
				//trace("Title:", loadedXML[i].title);
				//trace("text:", loadedXML[i].text);
				//trace("link:", loadedXML[i].link);
				//trace("path:", loadedXML[i].link.@href);*/
				
				//This checks that the inputted options are valid and sets default values
				
				//trace("\nThe following options are being set for" + loadedXML[i].title + ":", "loop #"+i);
				
					//Check the position parameter
					switch (String(loadedXML[i].options.@position).toLowerCase()) {
						case "right":
							textOptions.position = "right";
						break
						default:
							textOptions.position = "left"
						break;
					}
					//trace("Position =", textOptions.position);
					
					//Check vertical position, see if it's a number. If not, set it to nothing
					switch (regexp.test(String(loadedXML[i].options.@verticalPosition).toLowerCase())) {
						case true :
							textOptions.verticalPosition = int(loadedXML[i].options.@verticalPosition);
						break;
						default:
							textOptions.verticalPosition = null;
						break
					}
					//trace("Vertical Position =", textOptions.verticalPosition);
					
					//Check the textBackground property 
					switch (String(loadedXML[i].options.@textBackground).toLowerCase()) {
						case "false":
							textOptions.textBackground = Boolean(false);
						break
						default:
							textOptions.textBackground = Boolean(true);
						break;
					}
					//trace("Is there a text background:", textOptions.textBackground);
					
					//Check the textColor property
					switch (String(loadedXML[i].options.@textColor).toLowerCase()) {
						case "dark":
							textOptions.textColor = "dark"
						break
						default:
							textOptions.textColor = "light"
						break;
					}
					//trace("Text Colour:", textOptions.textColor);
					
					//Check the linkColor property
					switch (String(loadedXML[i].options.@linkColor).toLowerCase()) {
						case "white":
							textOptions.linkColor = "white"
						break
						default:
							textOptions.linkColor = "blue"
						break;
					}
					//trace("Link Colour:", textOptions.linkColor);
					
					//check the fadeTo property
					switch (String(loadedXML[i].options.@fadeTo).toLowerCase()) {
						case "white":
							textOptions.fadeTo = "white";
						break
						default:
							textOptions.fadeTo = "black";
						break;
					}
					//trace("Fade To:", textOptions.fadeTo);
					
				_items.push({
					imgSRC:String(imagePath + loadedXML[i].img.@src),
					title:String(loadedXML[i].title), 
					subhead:String(loadedXML[i].subhead),
					text:String(loadedXML[i].text), 
					linkText:String(loadedXML[i].link), 
					linkURL:String(loadedXML[i].link.@href),
					linkTarget:String(loadedXML[i].link.@target),
					attribution:String(loadedXML[i].attribution),
					attributionURL:String(loadedXML[i].attribution.@href),
					attributionName:String(loadedXML[i].attribution.@name),
					options:textOptions
				});
			}
			
			dispatchEvent(new Event(XmlLoader.LOADED)); //Let everyone know that the XML is loaded
		}

		private function onProgress($e:Event):void {
			//Hook up to progress bar here
			//trace($e.target.bytesTotal, $e.target.bytesLoaded);
		}

		private function onAllError($e:Event):void {
			//trace("Errord", $e.target);
			dispatchEvent(new Event(XmlLoader.ERROR)); //Let everyone know that the XML is loaded
		}
	}
}