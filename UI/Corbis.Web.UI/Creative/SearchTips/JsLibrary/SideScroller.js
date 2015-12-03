// SideScroller v1.0 by Travis O.
// Requires YAHOO UI Library:
//<script language="javascript" src="/jslibrary/yui/yahoo-min.js"></script>
//<script language="javascript" src="/jslibrary/yui/dom-min.js"></script>
//<script language="javascript" src="/jslibrary/yui/event-min.js"></script>
//<script language="javascript" src="/jslibrary/yui/animation-min.js"></script>
//
// Example usage: (scroller1 is the ID)
//  <div id="scroller1" style="width:204px; height: 150px; overflow:hidden;">
//		<div style="width:612px;" id="scroller1InnerDiv">
//			<div class="imgDiv">Content</div>...(multiple content divs here, all same width)
//		</div>
//  </div>
//  
// Scrolling links:  Set with IDs = scroller1Back and scroller1Forward
//
// Init:
// 	ssConfigs["scroller1"] = new SideScrollerConfig(3, 102, 1000);
//	SideScroller.init("scroller1");


var ssConfigs = new Array();

function SideScrollerConfig(numShown, nodeWidth, nodePadding, scrollSpeed)
{
	this.numShown = numShown;
	this.nodeWidth = nodeWidth;
	this.nodePadding = nodePadding;
	this.scrollSpeed = scrollSpeed;
	this.currentFirst = 1;
	this.contentNodes = new Array();
	this.isBusy = false;
	this.cssClass = "";
}

var SideScroller =
{
	init:function(instanceId) {
			YAHOO.util.Event.addListener(document.getElementById(instanceId + 'Forward'), 'click', this.scrollNext, instanceId);
			YAHOO.util.Event.addListener(document.getElementById(instanceId + 'Back'), 'click', this.scrollLast, instanceId);
			// Copy the last two items to the positions before this one
			for(var i=0; i < document.getElementById(instanceId + 'InnerDiv').getElementsByTagName('div').length; i++) {
				// Add to our div array
				ssConfigs[instanceId].cssClass = document.getElementById(instanceId + 'InnerDiv').getElementsByTagName('div')[i].className;
				ssConfigs[instanceId].contentNodes[i] = document.getElementById(instanceId + 'InnerDiv').getElementsByTagName('div')[i].innerHTML;
			}
			document.getElementById(instanceId + 'InnerDiv').innerHTML = "";
			
			var x=0;
			for(var i=0; i < ssConfigs[instanceId].numShown+2; i++)
			{
				this.appendNode(instanceId, ssConfigs[instanceId].contentNodes[x]);
				x++;
				if (x > ssConfigs[instanceId].contentNodes.length - 1) { x = 0; }
			}
			
			var anim = new YAHOO.util.Scroll(instanceId, { scroll: { to: [(ssConfigs[instanceId].nodeWidth + ssConfigs[instanceId].nodePadding), 0] } }, .01);
			anim.animate();
		},
		
	appendNode:function(instanceId, nodeContents)
		{
			var tempDiv = document.createElement('div');
			tempDiv.className = ssConfigs[instanceId].cssClass;
			tempDiv.innerHTML = nodeContents;
			document.getElementById(instanceId + 'InnerDiv').appendChild(tempDiv);
		},
	insertNode:function(instanceId, nodeContents)
		{
			var tempDiv = document.createElement('div');
			tempDiv.className = ssConfigs[instanceId].cssClass;
			tempDiv.style.overflow = "hidden";
			tempDiv.innerHTML = nodeContents;
			// Always insert nodes as 1px; animate after insert
			tempDiv.style.width = "1px";
			document.getElementById(instanceId + 'InnerDiv').insertBefore(tempDiv, document.getElementById(instanceId + 'InnerDiv').getElementsByTagName('div')[0]);
		},
		
	scrollNext:function(e, instanceId)
		{
			if (!ssConfigs[instanceId].isBusy) {
				ssConfigs[instanceId].isBusy = true;
				// Shrink 1st item
				document.getElementById(instanceId + 'InnerDiv').getElementsByTagName('div')[0].style.overflow = "hidden";
				var anim = new YAHOO.util.Anim(document.getElementById(instanceId + 'InnerDiv').getElementsByTagName('div')[0], { width: { from: ssConfigs[instanceId].nodeWidth, to: 1 } }, ssConfigs[instanceId].scrollSpeed/1000, YAHOO.util.Easing.easeIn);
				anim.animate();
				// Remove first item after it shrinks down
				setTimeout("document.getElementById('" + instanceId + "InnerDiv').removeChild(document.getElementById('" + instanceId + "InnerDiv').getElementsByTagName('div')[0]);ssConfigs['" + instanceId + "'].isBusy=false;", ssConfigs[instanceId].scrollSpeed);
				// Append next item
				ssConfigs[instanceId].currentFirst++;
				if (ssConfigs[instanceId].currentFirst >= ssConfigs[instanceId].contentNodes.length) { ssConfigs[instanceId].currentFirst = 0; }
				var showMe = ssConfigs[instanceId].numShown + ssConfigs[instanceId].currentFirst;
				if (showMe >= ssConfigs[instanceId].contentNodes.length) {	showMe = showMe - ssConfigs[instanceId].contentNodes.length; }
				SideScroller.appendNode(instanceId, ssConfigs[instanceId].contentNodes[showMe]);
				return false;
			}
		},
		
	scrollLast:function(e, instanceId)
		{
			if (!ssConfigs[instanceId].isBusy) {
				ssConfigs[instanceId].isBusy = true;
				// Remove last item
				document.getElementById(instanceId + 'InnerDiv').removeChild(document.getElementById(instanceId + 'InnerDiv').getElementsByTagName('div')[ssConfigs[instanceId].numShown + 1]);
				
				// Insert a new item
				ssConfigs[instanceId].currentFirst--;
				if (ssConfigs[instanceId].currentFirst < 0) { ssConfigs[instanceId].currentFirst = ssConfigs[instanceId].contentNodes.length - 1; }
				var showMe = ssConfigs[instanceId].currentFirst - 1;
				if (showMe < 0) {	showMe = showMe + ssConfigs[instanceId].contentNodes.length; }
				SideScroller.insertNode(instanceId, ssConfigs[instanceId].contentNodes[showMe]);
				var anim = new YAHOO.util.Anim(document.getElementById(instanceId + 'InnerDiv').getElementsByTagName('div')[0], { width: { from: 1, to: ssConfigs[instanceId].nodeWidth } }, ssConfigs[instanceId].scrollSpeed/1000, YAHOO.util.Easing.easeIn);
				anim.animate();
				setTimeout("ssConfigs['" + instanceId + "'].isBusy=false;", ssConfigs[instanceId].scrollSpeed);
				return false;
			}
		}
}

