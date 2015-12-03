 /* BEGIN remember page state fcns */
		        var creativeKeywordItems = 0;
		        var currTab = 0;
		        var currCat = 0;
		        var currImg = 1;
		        
		        //Borrowed from Prototype.js
		        function $() {
                  var elements = new Array();

                  for (var i = 0; i < arguments.length; i++) {
                    var element = arguments[i];
                    if (typeof element == 'string')
                      element = document.getElementById(element);

                    if (arguments.length == 1)
                      return element;

                    elements.push(element);
                  }

                  return elements;
                }
                //END Borrowed from Prototype.js
                
                
		        function initCurrObj(){
		            //get existing values, if they exist
		                if( document.cookie.indexOf("searchTips_currTab")!=-1 ) currTab = getCookie("searchTips_currTab");
		                if( document.cookie.indexOf("searchTips_currCat")!=-1 ) currCat = getCookie("searchTips_currCat");
		                if( document.cookie.indexOf("searchTips_currImg")!=-1 ) currImg = getCookie("searchTips_currImg");
		            //set tabs
		                ShowTab(currTab, 'tabNav');
		                
		           
		        }
		       
		        function setCurrObj( objName, i ){
		            document.cookie = 'searchTips_'+objName+'='+i;
		        }
		        
		        
		        function setCurrImg( dir ){
		            if( dir=='forward' ){
		                currImg++;
		                if( currImg == creativeKeywordItems+1 ) currImg = 1;
		            }
		            else{
		                currImg--;
		                if( currImg <= 0 ) currImg = creativeKeywordItems;
		            }
		            document.cookie = 'searchTips_currImg='+currImg;
		        }
		        
		        function getCookie(c_name){
                    if (document.cookie.length>0){
                        c_start=document.cookie.indexOf(c_name + "=")
                        if (c_start!=-1){ 
                            c_start=c_start + c_name.length+1;
                            c_end=document.cookie.indexOf(";",c_start)
                            if (c_end==-1) c_end=document.cookie.length;
                            return unescape(document.cookie.substring(c_start,c_end));
                        } 
                    }
                    return "";
                } 
		    /* END remember page state fcns */
		
			function ShowTab(tabNum) {
			    //find all the nav links of a parent HTML element
			    //loop through them and then decide if the current tab
			    //should be the top
			
			    setCurrObj('currTab',tabNum); //set current taba
			    
			    
			    
			    for(i=0; i<6; i++){
			    		//Skip zero since we only want the last five children of tabNav	   
				   
				    if(tabNum != i){
				        // Hide  tabs
    				    var myTab = $("DivContent" + i);
    				    var myLink = $("TabLink" + i);
    				    myTab.style.display = "none";
    				    myLink.className='off'; 
                    				     
				    }else{
	    			    // Set visibility of selected tab content
	    			    var myTab = $("DivContent" + tabNum);
	    			    var myLink = $("TabLink" + tabNum);
	    			    
    				    myTab.style.display = "";
    				    myLink.className='on';
    				    
    				    moveTitleSprite(tabNum);

    				}
    			}

			}
			function moveTitleSprite(position){
			    //The green nav title box is a sprite image with 6 positions
			    //each usable area is 57px tall, so we are just leveraging the
			    //cookie with the tab setting to remember where to put the 
			    //sprite.
			    var navTitle = $("navTitle");
			    navTitle.style.backgroundPosition = '0px -' + position*57 + 'px';
			}
			function returnSprite(){
			    //move the sprite back to normal.
			    var navTitle = $("navTitle");
			    navTitle.style.backgroundPosition = '0 -' + getCookie("searchTips_currTab")*57;
			}
			function swapKeywordImage(callerid){
			 
			    var imgParent = $("layout_right_col");
			        //This function will work properly as long as the Keyword image is the first
			        //child element in the 'layout_right_col' div
			        //If the image becomes the second or third element in the parent div
			        //the array pointer below will need to change from [0] to [1] or [2]
			    var imgSelf = imgParent.getElementsByTagName('img')[0];
			    
			    var imgPath = "./images";
   
			    switch(callerid){
			        case "a-m-t-1":
			            imgSelf.src=imgPath + "/en-US/keyword_people.jpg";
			        break;
			        case "a-m-t-2":
			            imgSelf.src = imgPath + "/en-US/keyword_timeandplace.jpg";
			        break;
			        case "a-m-t-3":
			            imgSelf.src = imgPath + "/en-US/keyword_compositional.jpg";
			        break;
			        case "a-m-t-4":
			            imgSelf.src = imgPath + "/en-US/keyword_conceptual.jpg";
			        break;
			        case "a-m-t-5":
			            imgSelf.src = imgPath + "/en-US/keyword_lifestyle.jpg";
			        break;
			        case "a-m-t-6":
			            imgSelf.src = imgPath + "/en-US/keyword_businessandfinance.jpg";
			        break;
			        case "a-m-t-7":
			            imgSelf.src = imgPath + "/en-US/keyword_healthandbeauty.jpg";
			        break;
			        case "a-m-t-8":
			            imgSelf.src = imgPath + "/en-US/keyword_sports.jpg";
			        break;
			        case "a-m-t-9":
			            imgSelf.src = imgPath + "/en-US/keyword_travel.jpg";
			        break;
			        case "a-m-t-10":
			            imgSelf.src = imgPath + "/en-US/keyword_natureandwildlife.jpg";
			        break;
			        case "a-m-t-11":
			            imgSelf.src = imgPath + "/en-US/keyword_homeandarchitecture.jpg";
			        break;
			    }

			}

			function reloadparentCloseChild(qKey, qVal) {
			    var url = "/Search/SearchResults.aspx?" + qKey + "=" + qVal;
			    if (window.opener && !window.opener.closed) {
			        window.opener.location.href = url;
			    }
			    else {
			        window.open(url);
			    }
			    //self.close();
			}

