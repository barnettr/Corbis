package com.corbis {	
	//Flash
	import flash.display.Sprite;
	import flash.display.DisplayObject;
	import flash.events.Event;
	
	//3rd Pary
	import br.com.stimuli.loading.BulkLoader;
	import br.com.stimuli.loading.BulkProgressEvent;
	import gs.TweenLite;
	
	public class ImageStack extends Sprite {
		private var _photoLoader:BulkLoader = new BulkLoader("photoLoader")
		private var _storyContainer:Sprite = new Sprite();
		private var _stories:Array = [];
		private var _imageArray:Array = [];
		private var _currentStory:uint;
		private var _speed:Number;
		private var _spinner:Spinner;

		public function ImageStack($speed:Number) {
			_speed = $speed;
			_photoLoader.addEventListener(BulkLoader.COMPLETE, photoComplete);
			_photoLoader.addEventListener(BulkLoader.ERROR, photoError);
			_photoLoader.addEventListener(BulkLoader.PROGRESS, photoProgress);
			
			_spinner = new Spinner();
			_spinner.width = 27;
			_spinner.height = 27;
			_spinner.x = 1000 / 2
			_spinner.y = 520 / 2

			_spinner.addEventListener(Event.ENTER_FRAME, spinPreloader);
				
			addChild(_spinner);
		}
		
		private function spinPreloader($e:Event){
			//trace("running");
			$e.target.rotation -= 45;
		}
		
		public function get currentStory():uint {
			return _currentStory; 
		}

		public function set currentStory(value:uint):void {
			if (value !== _currentStory) {
				_currentStory = value;
			}
		}
		
		public function build($stories:Array, $imagepath:String):void {
			_stories = $stories;
			var i:int = 0;
			
			for (i; i < $stories.length; i++){
				//trace("Adding:", $path + $stories[i].imgSRC);
				_photoLoader.add($stories[i].imgSRC, {id:$stories[i].imgSRC, type:"image"});
			}
			
			_photoLoader.start();
			
		}
		private function photoComplete($e:Event):void {
			TweenLite.to(_spinner, _speed, {autoAlpha:0, onComplete:function(){
				_spinner.removeEventListener(Event.ENTER_FRAME, spinPreloader);
			}});
			
			//trace("Photos loaded");

			_storyContainer.name = "_storyContainer"

			var i:int = 0;
			for (i; i < _stories.length; i++) {
				var story:Story = new Story($e.target.getBitmap(_stories[i].imgSRC), _stories[i]);
					story.name = _stories[i].imgSRC;
					story.myID = i;
					story.alpha = 0;
					story.visible = false;
					_storyContainer.addChildAt(story, 0);
					_imageArray.push(story);
			}
			runNextStory(_stories[0].imgSRC);
			var evt:CustomEvent = new CustomEvent(CustomEvent.SET_NAV, {id:_stories[0].imgSRC}, true);
			this.dispatchEvent(evt)
			addChild(_storyContainer);
			_currentStory = 0;
			
		}
		
		private function photoProgress($e:Event):void {
			//trace("loading images");
		}
		private function photoError($e:Event):void {
			//trace("Photo loading error");
			var photoError:DisplayError = new DisplayError("Sorry, I couldn't find those images for you.")
				photoError.x = (stage.stageWidth / 2) - (photoError.width / 2)
				photoError.y = (stage.stageHeight / 2) - (photoError.height / 2)
				
				addChild(photoError);
		}
		
		public function showStory($storyToShow:String):void{
			//trace("\nShow Story called");
			var i:int = 0;
			for (i; i < _imageArray.length; i++) {
				//trace("loopin", _imageArray[i].alpha);
				if (_imageArray[i].alpha != 0) {
					TweenLite.to(_imageArray[i], _speed, {autoAlpha:0, onComplete:runNextStory($storyToShow)});
					break
				}
			}
			/*runNextStory($storyToShow);*/
		}
		
		private function runNextStory($storyToSwitchTo:String){
			//trace("Run Next Story called", "_currentStory", _currentStory);
			var i:int = 0;
			for (i; i < _imageArray.length; i++) {
				if (_imageArray[i].name == $storyToSwitchTo) TweenLite.to(_imageArray[i], _speed, {autoAlpha:1, delay:_speed / 2})
			}
		}
		
		public function nextImage():void {
			//trace("NextImage Called");
			//trace("CurrentStory = ", _currentStory);
			_currentStory++
			//trace("Current story incremented to", _currentStory);
			if (_currentStory == 3) {
				//trace("CurrentStory == 3, resetting");
				_currentStory = 0
			};
			var evt:CustomEvent = new CustomEvent(CustomEvent.SET_NAV, {id:_imageArray[_currentStory].name}, true);
			this.dispatchEvent(evt)
			
			showStory(_imageArray[_currentStory].name);
			
		}		
	}
}