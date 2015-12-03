//AS3///////////////////////////////////////////////////////////////////////////
// 
// Copyright 2009 Corbis.
// 
////////////////////////////////////////////////////////////////////////////////

package {
	//Flash
	import flash.display.LoaderInfo;
	import flash.display.Sprite;
	import flash.display.Stage;
	import flash.display.StageScaleMode;
	import flash.display.StageAlign;
	import flash.events.Event;
	import flash.xml.*;
	import flash.utils.Timer;
	
	//3rd Pary
	import br.com.stimuli.loading.BulkLoader;
	import br.com.stimuli.loading.BulkProgressEvent;
	/*import com.reintroducing.events.CustomEvent;*/
	
	//Corbis
	import com.corbis.XmlLoader;
	import com.corbis.DisplayError;
	import com.corbis.Navigation;
	import com.corbis.NavItem;
	import com.corbis.CustomEvent;
	import com.corbis.ImageStack;
	
	public class SlideshowHome extends Sprite {
		private var _path:String = "";
		private var _stories:Array = [] //This will contain an array of Objects;
		private var _imageStack:ImageStack;
		private var _imageTimer:Timer;
		private var _navigation:Navigation;
		private const _timerInterval:int = 6000;
		private const _speed:Number = 0.75;
		
		public function SlideshowHome() {
			setupStage();
			loadPath();
			loadXML();
		}
		
		private function loadPath():void{
			var paramObj:Object = LoaderInfo(this.root.loaderInfo).parameters;
			if (paramObj.xmlfile) {
				_path = paramObj.xmlfile;
			}
		}
		
		private function loadXML():void{
			if (_path == "") _path = "stories.xml";
			var xmlLoader:XmlLoader = new XmlLoader(_path); 
				xmlLoader.addEventListener(XmlLoader.LOADED, xmlIsLoaded);
				xmlLoader.addEventListener(XmlLoader.ERROR, xmlError);
		}
		private function xmlError($e:Event):void {
			// Ugh, make this a universally accessible class
			
			trace("There was an error with the XML");
			var xmlError:DisplayError = new DisplayError("XML is missing, sorry.")
				xmlError.x = (stage.stageWidth / 2) - (xmlError.width / 2)
				xmlError.y = (stage.stageHeight / 2) - (xmlError.height / 2)
				
				addChild(xmlError);
		}
		private function xmlIsLoaded($e:Event):void{
			trace("XML loaded");
			_imageStack = new ImageStack(_speed); //yeah, passing in the speed at this point is a little weird.
			addChild(_imageStack);
			_stories = $e.target.items;
			_imageStack.build($e.target.items, _path);
			addEventListener(CustomEvent.SET_NAV, setNav);
			createNavigation();
			/*createAttribution();*/
			
			//Need to tie this into an event that fires once the photos are loaded
			startTimer(); // Get the rolling images moving
		}
		private function createNavigation():void {
			_navigation = new Navigation(_stories);
			_navigation.x = (stage.stageWidth / 2) - (_navigation.width / 2);
			_navigation.y = stage.stageHeight - 70;
				
			addChild(_navigation);
			addEventListener(CustomEvent.NAV_CLICKED, navIsClicked);
		}
		
		private function navIsClicked($e:CustomEvent):void{
			trace($e.target.name, "was clicked");
			/*trace(this.getChildByName("_storyContainer"));*/
			/*_imageStack.showStory($e.target.name);*/
			if (_imageStack.currentStory != $e.target.id) {
				_imageStack.showStory($e.target.name);
				_imageStack.currentStory = $e.target.id;
				_imageTimer.reset();
			}
		}
		
		private function setNav($e:CustomEvent):void {
			trace("SetNav called", $e.params.id);
			_navigation.select($e.params.id);
		}
		
		private function startTimer():void{
			_imageTimer = new Timer(_timerInterval);
			_imageTimer.addEventListener("timer", timerHandler);
			_imageTimer.start();
		}
		private function timerHandler(e:Event = null){
			trace("Beep");
			_imageStack.nextImage();
		}

		
		private function setupStage():void {
			stage.scaleMode = StageScaleMode.NO_SCALE; //turn of scaling
			stage.align = StageAlign.TOP_LEFT; //align stage to top left
		}
	
	}
	
}