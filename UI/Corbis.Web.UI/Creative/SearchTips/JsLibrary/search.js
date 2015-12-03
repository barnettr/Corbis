		////////////////////// end getCookie //////////////////////////////////
		// declarations...
		// The array values and the vElement
		// values are created by the individual .NET
		// controls and can be seen at the bottom 
		// of the page in view source....
		var collectionsCheckAll;
		var categorySelected;
		var categoryCheckAll;
		var collectionsAllOtherSources;
		var collectionsArray;
		var categoryArray;
		var orientationArray;
		var photosIllustrationArray;
		var AdvancedSearchVisible = false;
		var SearchResultsTemp = "";
		var bSubmitToken = false;
		var bAdvancedSearchOn = false;
		
		// Comment out in production...
		//window.onerror = errorHandler;
		window.onerror = null;
	
		if (!CookieDomain) {
			var CookieDomain = window.location.host;
		}
		if (!HttpUrl) {
			var HttpUrl = "http://" + window.location.host + "/";
		}
		if (!HttpsUrl) {
			var HttpsUrl = "https://" + window.location.host + "/";
		}
		
		function errorHandler(msg, url, lineNum)
		{
			alert(msg + '\n' + url + '\n' + lineNum + '\n');
			return true;
		}

		///	<summary>
		/// updateAvancedSearchFromCookie()
		///	</summary>
		function updateAdvancedSearchFromCookie()
		{
			if(GetCookie('SearchData') != null)
			{
			
				var searchData = GetCookie("SearchData")
				if(searchData.indexOf('as=True') !=-1)
				{
					AdvancedSearchVisible = true;
				}
				else
				{
					AdvancedSearchVisible = false;
				}
			}
		}
		
		function GetCookie (name) 
		{  

			var arg = name + "=";  

			var alen = arg.length;  

			var clen = document.cookie.length;  

			var i = 0;  

			while (i < clen) {    

			var j = i + alen;    

			if (document.cookie.substring(i, j) == arg) return getCookieVal (j);    

			i = document.cookie.indexOf(" ", i) + 1;    

			if (i == 0) break;   

			}  

			return null;

		}

		function getCookieVal (offset) {  

			var endstr = document.cookie.indexOf (";", offset);  

			if (endstr == -1) { endstr = document.cookie.length; }

			return unescape(document.cookie.substring(offset, endstr));

		}

		function DeleteCookie (name,path,domain) 
		{
		
			if (GetCookie(name)) 
			{
				
				document.cookie = name + "=" +
				((path) ? "; path=" + path : "") +
				((domain) ? "; domain=" + domain : "") +
				"; expires=Thu, 01-Jan-1970 00:00:01 GMT";
			}
		
		
		}

		function turnOnMSO()
		{
			var frm = document.frmSearch;
			var searchData = GetCookie("SearchData")	
			var newSearchData = searchData
			
			if(searchData != null)
			{
				if(bAdvancedSearchOn)
				{
					
					newSearchData = searchData + "&as=True";
					
				}
				else
				{
					
					if(searchData.indexOf("as=") !=-1)
					{
						newSearchData = searchData.replace(/&as=True/i, "");
						newSearchData = newSearchData.replace(/as=True/i, "");
								
						newSearchData = newSearchData.replace("&as=", "");
					}
					else
					{
						newSearchData = searchData + "&as=True";
						
					}
					
				}
						
				DeleteCookie("SearchData","/",CookieDomain);
				
				document.cookie = "SearchData=" + newSearchData + ";path=/;domain=" + CookieDomain;
			}
			else
			{
				
				newSearchData = "as=True";
				document.cookie = "SearchData=" + newSearchData + ";path=/;domain=" + CookieDomain;
			}
		}
		
		function SwitchTabs(tabNumber)
		{
			
			var preferences = GetCookie("Preferences");	
			var sessioninfo = GetCookie("SessionInfo");
			var newPreferences = preferences;
			var bHome = false;
			var lastTab = "0"
			
			var href = window.top.location.href.toLowerCase();
			
			
			if(sessioninfo != null)
			{
				
					var lTabIndex = sessioninfo.indexOf("ltab=")
					if(lTabIndex !=-1)
					{	
						lastTab = sessioninfo.substr(lTabIndex+5,1);
						if(lastTab != tabNumber)
						{
												
							
							bHome = true;
							
						}
					}
				
			}
			
			
			
			if(preferences != null)
			{
										
				if(preferences.indexOf("tab=") !=-1)
				{	
					newPreferences = preferences.replace(/tab=\d/, "tab=" + tabNumber);
				}
				else
				{
					newPreferences = preferences  + "&tab=" + tabNumber;
				}
				DeleteCookie("Preferences","/",CookieDomain);
				DeleteCookie("Preferences","/","");
				document.cookie = "Preferences=" + newPreferences + ";path=/;domain=" + CookieDomain;
			}
			else
			{
				newPreferences = "tab=" + tabNumber;
				document.cookie = "Preferences=" + newPreferences + ";path=/;domain=" + CookieDomain;
			}
			
			
								
				
			window.top.location = "http://"+window.location.host+"/default.aspx";
			
					
		}
		
		function SwitchTabsNoRefresh(tabNumber)
		{
			SwitchTabs(tabNumber)
			return;
			var preferences = GetCookie("Preferences")	
			var newPreferences = preferences
			
			if(preferences != null)
			{
				if(preferences.indexOf("tab=") !=-1)
				{	
					newPreferences = preferences.replace(/tab=\d/, "tab=" + tabNumber);
				}
				else
				{
					newPreferences = preferences  + "&tab=" + tabNumber;
				}
				
				DeleteCookie("Preferences","/",CookieDomain);
				
				document.cookie = "Preferences=" + newPreferences + ";path=/;domain=" + CookieDomain;
			}
			else
			{
			
				newPreferences = "tab=" + tabNumber;
				
				document.cookie = "Preferences=" + newPreferences + ";path=/;domain=" + CookieDomain;
				
			}
			
					
		}
		////////////////////////////////////
		///	<summary>
		///	Main flip-flop
		/// AdvancedSeachDiv is written to
		/// a .NET literal control in each panel
		/// after reading the cookie (if exists)
		/// on loading the page.  This is how
		/// the persistance across pages works.
		/// The if((getCookie... statement below
		/// is prevent having to double click the
		/// first time to flip between states.
		///	</summary>
		function layout()
		{
			if (AdvancedSearchVisible)
			{
				document.getElementById('AdvancedSearchDiv').style.display = "none";
				
				
				toggleCollectionSearch(0);
				toggleSearchInResults('on');	
				toggleOnOff('off');
				
				if(parent.document.getElementById('fmeASPIFrame'))
				{
					parent.document.getElementById('fmeASPIFrame').height = 150;					
				}
				resetAdvancedFilters();				
				
			}
			else
			{
				document.getElementById('AdvancedSearchDiv').style.display = "block";
				document.getElementById('CollectionsDiv').style.display = "block";
				
				
				toggleCollectionSearch(1);
				toggleSearchInResults('off');
				toggleOnOff('on');
				if(parent.document.getElementById('fmeASPIFrame'))
				{
					parent.document.getElementById('fmeASPIFrame').height = 610;
					
					
				}
				ToggleFilterMessage('off');
				categorySelected = '';	
			}
		}
		
		// Toggle Advance Search Panel
		function toggleAdvancedSearch()
		{	
			AdvancedSearchVisible = AdvancedSearchVisible ? false : true;
			
			// toggle cookie value WARNING: - 
			turnOnMSO();
		}
		
		function toggleCollectionSearch(OnToggleOff)
		{
		    // OnToggleOff - 0=OFF, 1=ON, 2=Toggle from current state.  Convert to 0 or 1.
		    if (OnToggleOff == 2) {
    		    if (document.getElementById('CollectionsDiv').style.display == "none") {
    		        OnToggleOff = 1;
    		    } else {
    		        OnToggleOff = 0;
    		    }
		    }
		    // Turn off
		    var searchData = GetCookie("SearchData");
			var newSearchData = searchData;
			
		    if (OnToggleOff == 0) {
				newSearchData = searchData.replace(/&cns=True/i, "");
				newSearchData = newSearchData.replace(/cns=True/i, "");
				newSearchData = newSearchData.replace("&cns=", "");
		        toggleOnOffCollections('off');
		        document.getElementById('CollectionsDiv').style.display = "none";
		        if(parent.document.getElementById('fmeASPIFrame'))
				{
				    if(parent.document.getElementById('fmeASPIFrame').height > 200)				    
					    parent.document.getElementById('fmeASPIFrame').height -= 240;
					
					
				}
		        resetCollections();
		    } else {
	            newSearchData = searchData + "&cns=True";
    		    toggleOnOffCollections('on');
		        document.getElementById('CollectionsDiv').style.display = "block";
		        if(parent.document.getElementById('fmeASPIFrame'))
				{
					parent.document.getElementById('fmeASPIFrame').height = 410;
					
					
				}
            }		    
			DeleteCookie("SearchData","/",CookieDomain);
			document.cookie = "SearchData=" + newSearchData + ";path=/;domain=" + CookieDomain;
		}
		
		function resetCollections()
		{
			if (collectionsArray) {
				for (var i=0; i < collectionsArray.length; i++)
				{
					document.getElementById(collectionsCheckAll[i]).checked = true;
					resetCollection(i);
				}
			}
		}
		
		function resetCollection(collectionNumber)
		{
			for (var i=0; i < document.getElementById(collectionsArray[collectionNumber]).options.length; i++)
			{
				document.getElementById(collectionsArray[collectionNumber]).options[i].selected = true;
			}
		}
		
		// Toggles the On/Off of the more search options link
		function toggleOnOff(OnOrOff)
		{
			if(OnOrOff == 'on')
			{
				document.getElementById('linkTurnOnOff').innerHTML = vTurnOff;
				
			}
			else
			{
				document.getElementById('linkTurnOnOff').innerHTML = vTurnOn;
				
			}
		}
		
		function toggleOnOffCollections(OnOrOff)
		{
			if(OnOrOff == 'on')
			{
				document.getElementById('linkTurnOnOffCollections').innerHTML = vCollTurnOff;
			}
			else
			{
				document.getElementById('linkTurnOnOffCollections').innerHTML = vCollTurnOn;
				
			}
		}
		
		// Toggles the On/Off of the more search options link
		function DisplayFilterMessage(OnOrOff, msg)
		{
			if(OnOrOff == 'on')
			{
				if(document.getElementById('lblResetFilters') != null)
				{
					document.getElementById('lblResetFilters').innerHTML = msg;
				}
			}
			else
			{
				if(document.getElementById('lblResetFilters') != null)
				{
					document.getElementById('lblResetFilters').innerHTML = "";
				}
			}
		}
		
		function ToggleFilterMessage(OnOrOff)
		{
			if(OnOrOff == 'on')
			{
				if(document.getElementById('lblResetFilters') != null)
				{
				
					document.getElementById('lblResetFilters').style.display = 'block';
				}
			}
			else
			{
			
				if(document.getElementById('lblResetFilters') != null)
				{
					document.getElementById('lblResetFilters').style.display = 'none';
				}
			}
		}
		
		// Toggles the Search in Results DropDown/text
		function toggleSearchInResults(OnOrOff)
		{
			
		}
		///	<summary>
		///	Search in Results/Categories
		/// Save the seach results if the
		/// user clicks on another selection
		/// and return them to the hidden field 
		/// if he returns to searching in results.
		///	</summary>
		function searchInResults(control)
		{
			if((control[control.selectedIndex].value != "")
				&& (document.getElementById('hdnSearchInResults').value != ""))
			{
				SearchResultsTemp = document.getElementById('hdnSearchInResults').value;
				document.getElementById('hdnSearchInResults').value = "";
			}
			
			else 
			{
				if((SearchResultsTemp != "") 
					&& (document.getElementById('hdnSearchInResults').value == ""))
				{
					document.getElementById('hdnSearchInResults').value = SearchResultsTemp;
				}
			}
			
		}
				
		function isNullCategories()
		{
			var categoryCheckedCount = 0;
			var categoryUncheckedCount = 0;
			for(var i = 0; i < categoryArray.length; i++)
			{
				if(document.getElementById(categoryArray[i]).checked == true)
					categoryCheckedCount++;
				else
					categoryUncheckedCount++;				
			}
			
			if((categoryCheckedCount == 0) || (categoryUncheckedCount == 0))
				return true; //is Null
			
			return false;
		}
		
		function isNullCollections()
		{
			var collectionsCheckedCount = 0;
			var collectionsUncheckedCount = 0;

			// Check the categories to see that at least one but not all are selected
			if (collectionsArray) {
				for (var i=0; i < collectionsArray.length; i++)
				{
					if (document.getElementById(collectionsCheckAll[i]).checked) {
						// All are checked, add to the checked count
						collectionsCheckedCount += document.getElementById(collectionsArray[i]).options.length;
					} else {
						for (var j=0; j < document.getElementById(collectionsArray[i]).options.length; i++)
						{
							if (document.getElementById(collectionsArray[collectionNumber]).options[j].selected) {
								collectionsCheckedCount++;
							} else {
								collectionsUncheckedCount++;
							}
						}
					}
				}
			}
		
			if((collectionsCheckedCount == 0) || (collectionsUncheckedCount == 0))
				return true; // is Null
			
			return false; // is not null
		}
		
		function isNullSpecifySource()
		{
			if (document.getElementById(vSpecifySource).value == "")
				return true; //null
			else
				return false; //not null
		}
		
		function isNullImageNumber()
		{
			if (document.getElementById(vImageNumbers).value == "")
				return true;
			else
				return false;
		}
		
		function isNullPhographer()
		{
			if (document.getElementById(vPhotographerName).value == "")
				return true;				
			else
				return false;				
		}
		
		function IsNotLocalizedDate(dateValue)
		{
			if(dateValue.indexOf('/') == -1) return true;
			var splitValues = dateValue.split("/");			
			var end1 = 12;			
			var end2 = 31;
			if(vLanguageCode != 'en-US')
			{
				//reverse day and month
				end1 = 31;
				end2 = 12;
			}
			
			var result = dateValue.match(/\d{1,2}\/\d{1,2}\/\d{4}/);			
			if((result == null)&&(dateValue != vDateFormat)) return true;	
								
			if((splitValues[0] > end1) || (splitValues[0] < 1))
			{
				return true;
			}
			else if((splitValues[1] > end2) || (splitValues[1] < 1))
			{
				return true;
			}
			else if(splitValues[2].length != 4)
			{
				return true;
			}
			else
			{
				return false;
		}
		}
		function isNullDatePhotographed()
		{
			var dateValue = document.getElementById(vDatePhotoOrCreated).value;
			
			
			if((dateValue == "")||(dateValue == null)) return true;	
					
		}
		
		function isNullLocation()
		{
			if (document.getElementById(vLocation).value == "")
				return true;
			else
				return false;
		}
		
		function isNullPointOfView()
		{
			var indexPOV = document.getElementById(vPointOfView).selectedIndex;					
								
			if (document.getElementById(vPointOfView).options[indexPOV].value == "")				
				return true;				
			else
				return false;
		}
		
		function isNullPeopleInImages()
		{
			var index = document.getElementById(vPeopleInImage).selectedIndex;					
								
			if (document.getElementById(vPeopleInImage).options[index].value == "")				
				return true;				
			else
				return false;	
		}
		
		function isNullImmediateAvailability()
		{
			var index = document.getElementById(vImmediateAvailability).selectedIndex;					
								
			if (document.getElementById(vImmediateAvailability).options[index].value == "")				
				return true;				
			else
				return false;
		}
		
		function isNullImagesMadeAvailable()
		{
			var isNull = true ;
			
			if(document.getElementById(vInTheLastRadio).checked)
			{
				return document.getElementById(vInTheLastTextBox).value == "";								
					}
				
			if(document.getElementById(vBetweenRadio).checked)
					{
				var betweenDateValue = document.getElementById(vBetweenTextBox).value;
				var andDateValue = document.getElementById(vAndTextBox).value;
				var result = betweenDateValue.match(/\w{1,2}\/\w{1,2}\/\w{4}/);								
				if(result == null) return true;	
				var result = andDateValue.match(/\d{1,2}\/\d{1,2}\/\d{4}/);				
				if(result == null) return true;					
						return false;
				
					}
				
			return isNull;
		}
		
		function isNullOrientation()
		{
			var checkedCount = 0;
			var uncheckedCount = 0;
			for(var i = 0; i < orientationArray.length; i++)
			{
				if(document.getElementById(orientationArray[i]).checked == false)
				{
					uncheckedCount++;
				}
				else
				{
					checkedCount++;
				}
			}
//			// If all checked or all unchecked IS NULL
//			if((uncheckedCount == 0) || (checkedCount == 0))
//			{
//				alert('is Null');
//				return true;
//			}
//			alert('not null');
			return false;
		}
		
		function selectBetween()
		{
			document.getElementById(vBetweenRadio).checked = true;
		}
		
		function isNullMagazine()
		{
			if (document.getElementById(vMagazine).value == "")
				return true;
			else
				return false;
		}
		
		function isNullDatePublished()
		{
			if((document.getElementById(vDatePublished).value != "") && (document.getElementById(vDatePublished).value != 'mm/dd/yyyy'))
			{
				if(!isDate(document.getElementById(vDatePublished).value))
				{
					document.getElementById(vDatePublished).focus();
					document.getElementById(vDatePublished).select();
					return true;
				}
				else
					return false;
			}
			return true;
		}
		
		function validateCurrentEventsSearchForm()
		{
			var isValid = false;

			if ((!isNullDatePhotographed()) 			
				|| (!isNullLocation()) 		
				|| (!isNullSpecifySource())	
				|| (!isNullImagesMadeAvailable())
				|| (!isNullImageNumber())		
				|| (!isNullCollections())
				|| (!isNullPhographer())
				|| (!isNullPointOfView())		
				|| (!isNullImmediateAvailability())	
				|| (document.getElementById(vKeywordsText).value != "")
				|| document.getElementById(vOnlyOnCD).checked
				)
			{
				
				isValid = true;
			}				
			
			
			if(isValid && document.getElementById(vInTheLastRadio).checked && document.getElementById(vInTheLastTextBox).value != "")
			{
				try
				{
					if(isNaN(parseInt(document.getElementById(vInTheLastTextBox).value)))
					{
						alert(vNumberError); // Localize this...
						document.getElementById(vInTheLastTextBox).focus();
						document.getElementById(vInTheLastTextBox).select();
						return false; //invalid.
					}
					else if (document.getElementById(vInTheLastTextBox).value < 1)
					{
					    alert(vNumberError); // Localize this...
						document.getElementById(vInTheLastTextBox).focus();
						document.getElementById(vInTheLastTextBox).select();
						return false; //invalid.
					}				
				}
				catch(e)
				{
					alert(vNumberError); // Localize this...
						document.getElementById(vInTheLastTextBox).focus();
						document.getElementById(vInTheLastTextBox).select();
						return false; //invalid.
				}
			}
					
			
			
			var badDate = false;
			
			
			
			
			if(!badDate)
			{
				if(document.getElementById(vBetweenRadio).checked)
				{
					var betweenDateValue = document.getElementById(vBetweenTextBox).value;
					var andDateValue = document.getElementById(vAndTextBox).value;
					
					
					if(IsNotLocalizedDate(andDateValue))
					{
					
						document.getElementById(vAndTextBox).focus();
						document.getElementById(vAndTextBox).select();
						alert(vPleaseEnterDateError);
						badDate = true; //invalid
					}
					else if(IsNotLocalizedDate(betweenDateValue) && betweenDateValue != "")
					{
						document.getElementById(vBetweenTextBox).focus();
						document.getElementById(vBetweenTextBox).select();
						alert(vPleaseEnterDateError);
						badDate = true; //invalid
					}				
					
					var betweenDate = new Date(betweenDateValue);
					if (vLanguageCode != 'en-US')
					{
						var betweenDate = new Date();
						// dd/mm/yyyy format
						var ddmmyyyy = betweenDateValue.split("/");
						if (ddmmyyyy.length == 3)
						{
							betweenDate = new Date(ddmmyyyy[1] + "/" + ddmmyyyy[0] + "/" + ddmmyyyy[2]);
						} 
						else 
						{
							badDate = true;
						}
						var andDate = new Date();
						// dd/mm/yyyy format
						ddmmyyyy = andDateValue.split("/");
						if (ddmmyyyy.length == 3)
						{
							andDate = new Date(ddmmyyyy[1] + "/" + ddmmyyyy[0] + "/" + ddmmyyyy[2]);
						} 
						else 
						{
							badDate = true;
						}
					}
					else 
					{
						var betweenDate = new Date(betweenDateValue);
						var andDate = new Date(andDateValue);
					}
					if(betweenDate>andDate)
					{
						alert(vEndDateError);
						document.getElementById(vBetweenTextBox).focus();
						document.getElementById(vBetweenTextBox).select();
						badDate = true; //invalid
					}
				}
			}
			
			if(badDate) return false;
			
			if(!isValid)
			{
				alert(vKeywordsEmptyError);
			}
			return isValid;
		}
		
		function validateCoreSearchForm()
		{
			var isValid = false;
			
			//Check RM and RF.
			if((!document.getElementById(vRoyaltyFree).checked) && 
			   (!document.getElementById(vRightsManaged).checked))
			{
				alert(vRightsError);
				return false;
			}
			
			if (document.getElementById(vOnlyOnCD).checked)
			{
				isValid = true;
			}
			
			if (!isNullCollections())
					isValid = true;	
				
			if (AdvancedSearchVisible == true)
			{
				// If these conditions are met, we dont need a keyword
				if (!isNullCollections())
					isValid = true;					
				if (!isNullSpecifySource())
					isValid = true;
				if (!isNullImageNumber())
					isValid = true;
				if (!isNullPhographer())
					isValid = true;
				
				if (!isNullLocation())
					isValid = true;
				if (!isNullPointOfView())
					isValid = true;
				/*
				if (!isNullPeopleInImages())
					return true;					 
					*/
				
				if (!isNullImmediateAvailability())
					isValid = true;
			
				
				if(!isNullImagesMadeAvailable())
					isValid = true;
			
				if (!isNullDatePhotographed())
				{
						isValid = true;
				}
				
				if(isValid && document.getElementById(vInTheLastRadio).checked && document.getElementById(vInTheLastTextBox).value != "")
				{

					try
					{
							
						if(isNaN(parseInt(document.getElementById(vInTheLastTextBox).value)))
						{
							alert(vNumberError); // Localize this...
							document.getElementById(vInTheLastTextBox).focus();
							document.getElementById(vInTheLastTextBox).select();
							return false; //invalid.
						}
					}
					catch(e)
					{
						
						alert(vNumberError); // Localize this...
						document.getElementById(vInTheLastTextBox).focus();
						document.getElementById(vInTheLastTextBox).select();
						return false; //invalid.
					}
				}
				
										
				var badDate = false;
									
				
				if(!badDate)
				{
					if(document.getElementById(vBetweenRadio).checked)
					{
						var betweenDateValue = document.getElementById(vBetweenTextBox).value;
						var andDateValue = document.getElementById(vAndTextBox).value;
						
						if(IsNotLocalizedDate(andDateValue))
						{
						
							document.getElementById(vAndTextBox).focus();
							document.getElementById(vAndTextBox).select();
							alert(vPleaseEnterDateError);
							badDate = true; //invalid
						}
						else if(IsNotLocalizedDate(betweenDateValue))
						{
							document.getElementById(vBetweenTextBox).focus();
							document.getElementById(vBetweenTextBox).select();
							alert(vPleaseEnterDateError);
							badDate = true; //invalid
						}			
		
						var betweenDate = new Date(betweenDateValue);
						if (vLanguageCode != 'en-US')
						{
							var betweenDate = new Date();
							// dd/mm/yyyy format
							var ddmmyyyy = betweenDateValue.split("/");
							if (ddmmyyyy.length == 3)
							{
								betweenDate = new Date(ddmmyyyy[1] + "/" + ddmmyyyy[0] + "/" + ddmmyyyy[2]);
							} 
							else 
							{
								badDate = true;
							}
							var andDate = new Date();
							// dd/mm/yyyy format
							ddmmyyyy = andDateValue.split("/");
							if (ddmmyyyy.length == 3)
							{
								andDate = new Date(ddmmyyyy[1] + "/" + ddmmyyyy[0] + "/" + ddmmyyyy[2]);
							} 
							else 
							{
								badDate = true;
							}
						}
						else 
						{
							var betweenDate = new Date(betweenDateValue);
							var andDate = new Date(andDateValue);
						}
						if(betweenDate>andDate)
						{
							alert(vEndDateError);
							document.getElementById(vBetweenTextBox).focus();
							document.getElementById(vBetweenTextBox).select();
							badDate = true; //invalid
						}		
					}
				}
			
				if(badDate) return false;
				
				if(!isValid)
				{
					if (document.getElementById(vKeywordsText).value == "")
					{
						alert(vKeywordsEmptyError);
					}
					else
					{
						isValid = true;
					}
				}
			}		
			else
			{	
				if(!isValid)
				{
					if (document.getElementById(vKeywordsText).value == "")
					{			
						alert(vKeywordsEmptyError);
						isValid = false;
					}
					else
					{
						isValid = true;
					}
				}
			}
			
			return isValid ;			
		}
		
		function validateOutlineSearchForm()
		{
			
			if (AdvancedSearchVisible == true)
			{
				// If these conditions are met, we dont need a keyword
				
				if (!isNullImageNumber())
					return true;
				if (!isNullMagazine())
					return true;
				if (!isNullPhographer())
					return true;
				if (!isNullLocation())
					return true;
				if (!isNullPointOfView())
					return true;				
			
				if (!isNullDatePhotographed())
						return true;
				if (!isNullImagesMadeAvailable())
					return true;
				else					
					{
						if (!isNullDatePublished())
						return true;
						else
						{
							alert(vKeywordsEmptyError);
							return false;
						}
					}
				
				// we came this far, with the empty keyword. err out.
				alert(vKeywordsEmptyError);
				return false;				
			}
			else
			{
				alert(vKeywordsEmptyError);
				return false;	
			}
			return true;	
		}
		
		function validateSygmaSearchForm()
		{
			if (AdvancedSearchVisible == true)
			{
				if (!isNullImageNumber())
						return true;
				if (!isNullDatePhotographed())
						return true;
				if (!isNullLocation())
						return true;
				if (!isNullImmediateAvailability())
						return true;
						
				if (!isNullImagesMadeAvailable())
						return true;
					else					
					{
						alert(vKeywordsEmptyError);
						return false;
					}
				
				// we came this far, with the empty keyword. err out.
				alert(vKeywordsEmptyError);
				return false;
			}
			else
			{
				alert(vKeywordsEmptyError);
				return false;	
			}
			return true;
		}
		
		function validateRFSearchForm()
		{
			if (document.getElementById(vOnlyOnCD).checked)
			{
				return true;
			}

			if (AdvancedSearchVisible == true)
			{
				if (!isNullImageNumber())
					return true;									
				if (!isNullDatePhotographed())
					return true;				
				if (!isNullPhographer())										
					return true;																			
				if (!isNullLocation())
					return true;
				if (!isNullPointOfView())
					return true;
						
				if (!isNullImagesMadeAvailable())
						return true;
					else					
					{	
						alert(vKeywordsEmptyError);
						return false;
					}
				// we came this far, with the empty keyword. err out.
				alert(vKeywordsEmptyError);
				return false;
			}
			else
			{
				alert(vKeywordsEmptyError);
				return false;	
			}
			return true;
		}	
		
		
		///	<summary>
		///	Client side validation
		/// Called from KeywordSearch control.
		///	</summary>
		
		function validateForm()
		{
			
			try
			{
			updateSearchInResultsValue();
			
			
			switch(vPanelName)
			{
				case 'core':
						return validateCoreSearchForm();
					case 'creative':					
						return validateCoreSearchForm();
					case 'currentevents':
						return validateCurrentEventsSearchForm();
				case 'outline':
				
					if (document.getElementById(vKeywordsText).value == "")
						return validateOutlineSearchForm();				
				case 'sygma':
					if (document.getElementById(vKeywordsText).value == "")
						return validateSygmaSearchForm();
				case 'rfsearch':
					if (document.getElementById(vKeywordsText).value == "")
						return validateRFSearchForm();
			}
			}
			catch(e)
			{
			
			return true;
		}
			return true;
		}
			
		function updateSearchInResultsValue()
		{
			if (vPanelName != 'outline')
			{
			document.getElementById('hdnSearchInResults').value = document.getElementById(vKeywordsText).value;
			}
			
		}
				
		
		///	<summary>
		///	 Clears the keyword text box when the
		///	 search in results checkbox is checked.
		///  and toggles the search in results.
		///  sets the hidden value to the results
		///  hidden input is also a part of the 
		///  .NET generated JavaScript HTML
		///  in the vSeachCheckBox and vSeachDropDown
		///  strings.
		///	</summary>
		function clearKeywordText(control)
		{
			if(control.checked)
			{
				
				// Add hidden field and transfer value w/(keywords) 
				if(document.getElementById('ddlSearchInResults') != null)
				{
					var index = document.getElementById('ddlSearchInResults').selectedIndex;
					if(document.getElementById('ddlSearchInResults').options[index].value == "")
					{
						document.getElementById('hdnSearchInResults').value =
							document.getElementById(vKeywordsText).value;
						document.getElementById(vKeywordsText).value = "";
					}
				}
				else
				{
					document.getElementById('hdnSearchInResults').value =
							document.getElementById(vKeywordsText).value;
						document.getElementById(vKeywordsText).value = "";
				}
				
			}
			else
			{
				//document.getElementById('hdnSearchInResults').value = "";	
			}
		}
		
		/// <summary>
		///	Reset filters from link
		/// Basic search filters
		/// </summary>
		function resetFilters()
		{
			if(vPanelName == 'core')
			{
				document.getElementById(vRightsManaged).checked = true;
				document.getElementById(vRoyaltyFree).checked = true;
				document.getElementById(vKeywordsText).value = "";
				
				
				for(var i = 0; i < photosIllustrationArray.length; i++)
				{
					document.getElementById(photosIllustrationArray[i]).checked = true;
				}
				for(var i = 0; i < colorBlackAndWhiteArray.length; i++)
				{
					document.getElementById(colorBlackAndWhiteArray[i]).checked = true;
				}
				document.getElementById(vModelReleaseOnly).checked = false;
				document.getElementById(vOnlyOnCD).checked = false;
			}
			
			if(vPanelName == 'creative')
			{
				document.getElementById(vRightsManaged).checked = true;
				document.getElementById(vRoyaltyFree).checked = true;
				document.getElementById(vKeywordsText).value = "";
				
				
				for(var i = 0; i < photosIllustrationArray.length; i++)
				{
					document.getElementById(photosIllustrationArray[i]).checked = true;
				}
				for(var i = 0; i < colorBlackAndWhiteArray.length; i++)
				{
					document.getElementById(colorBlackAndWhiteArray[i]).checked = true;
				}
				document.getElementById(vModelReleaseOnly).checked = false;
				document.getElementById(vOnlyOnCD).checked = false;
			}
			
			if(vPanelName == 'currentevents')
			{
				
				document.getElementById(vSpecifySource).value = "";
				document.getElementById(vDatePhotoOrCreated).value = "";
				document.getElementById(vLocation).value = "";
				
				// Images Made Available reset
				document.getElementById(vInTheLastRadio).checked = true;
				document.getElementById(vInTheLastTextBox).value = "";
				document.getElementById(vBetweenTextBox).value = vDateFormat;	// localize this
			
				document.getElementById(vAndTextBox).value = GetTodaysDateString();
				
			}
			
			if(vPanelName == 'outline')
			{
				document.getElementById(vKeywordsText).value = "";
				
				for(var i = 0; i < colorBlackAndWhiteArray.length; i++)
				{
					document.getElementById(colorBlackAndWhiteArray[i]).checked = true;
				}
			}
			if(vPanelName == 'rfsearch')
			{
				document.getElementById(vKeywordsText).value = "";
				
				for(var i = 0; i < photosIllustrationArray.length; i++)
				{
					document.getElementById(photosIllustrationArray[i]).checked = true;
				}
				for(var i = 0; i < colorBlackAndWhiteArray.length; i++)
				{
					document.getElementById(colorBlackAndWhiteArray[i]).checked = true;
				}
				document.getElementById(vModelReleaseOnly).checked = false;
				document.getElementById(vOnlyOnCD).checked = false;
			}
			if(vPanelName == 'sygma')
			{
				document.getElementById(vKeywordsText).value = "";
				document.getElementById(vSygmaCheck).checked = true;
				document.getElementById(vKipaCheck).checked = true;
				for(var i = 0; i < colorBlackAndWhiteArray.length; i++)
				{
					document.getElementById(colorBlackAndWhiteArray[i]).checked = true;
				}
				document.getElementById(vModelReleaseOnly).checked = false;
			}	
			
			if(document.getElementById('cbSearchInResults') != null)
			{
				document.getElementById('cbSearchInResults').checked = false;
			}
			if(document.getElementById('ddlSearchInResults') != null)
			{
				document.getElementById('ddlSearchInResults').selectedIndex = 0;
			}
			
			if(document.getElementById(vSearchInCheckBox))
			{
				document.getElementById(vSearchInCheckBox).checked = false;
			}
			categorySelected = "";
			resetAdvancedFilters();
			
		}
		
		function showSearchInResults()
		{
			
			document.getElementById(vSearchInResultsControl).style.display = 'block';
		}
		
		/// <summary>
		///	Reset Advanced filters from link or toggle...
		/// </summary>
		function resetAdvancedFilters()
		{
			try
			{
				if(vPanelName == 'core' || vPanelName == 'sygma' || vPanelName == 'creative'|| vPanelName == 'currentevents')
			{
				if((AdvancedSearchVisible == true) 
					&& (vLanguageCode != 'en-US')
					&& (vLanguageCode != 'en-GB'))
				{
					document.getElementById(vEnglishOnly).checked = false;
				}

				if(document.getElementById(categoryCheckAll) != null)
				{
					document.getElementById(categoryCheckAll).checked = true;
					SelectAllCategories();
					ToggleFilterMessage();
				}
				if(document.getElementById(collectionsCheckAll) != null)
				{
					document.getElementById(collectionsCheckAll).checked = true;
					SelectAllCollections();
				}
			}
				if(vPanelName == 'core' || vPanelName == 'creative')
			{
				
						if(document.getElementById(vPhotographerName))document.getElementById(vPhotographerName).value = "";
						if(document.getElementById(vSpecifySource))document.getElementById(vSpecifySource).value = "";
						if(document.getElementById(vDatePhotoOrCreated))document.getElementById(vDatePhotoOrCreated).value = "";
						if(document.getElementById(vLocation))document.getElementById(vLocation).value = "";
						if(document.getElementById(vImageNumbers))document.getElementById(vImageNumbers).value = "";
						if(document.getElementById(vPointOfView))document.getElementById(vPointOfView).selectedIndex = 0;
						if(document.getElementById(vPeopleInImage))document.getElementById(vPeopleInImage).selectedIndex = 0;
						if(document.getElementById(vImmediateAvailability))document.getElementById(vImmediateAvailability).selectedIndex = 0;
						if(document.getElementById(vSearchImageImageSets))document.getElementById(vSearchImageImageSets).selectedIndex = 0;
				
				}

				if(vPanelName == 'creative')
				{
					for(var i = 0; i < photosIllustrationArray.length; i++)
					{
						document.getElementById(photosIllustrationArray[i]).checked = true;
					}
					for(var i = 0; i < colorBlackAndWhiteArray.length; i++)
					{
						if(document.getElementById(colorBlackAndWhiteArray[i]))document.getElementById(colorBlackAndWhiteArray[i]).checked = true;
					}
				}
				
				if(vPanelName == 'currentevents')
				{
					if(document.getElementById(vRightsManaged))document.getElementById(vRightsManaged).checked = true;
					if(document.getElementById(vRoyaltyFree))document.getElementById(vRoyaltyFree).checked = true;
					if(document.getElementById(vOnlyOnCD))document.getElementById(vOnlyOnCD).checked = false;
					for(var i = 0; i < photosIllustrationArray.length; i++)
					{
						if(document.getElementById(photosIllustrationArray[i]))document.getElementById(photosIllustrationArray[i]).checked = true;
					}
					for(var i = 0; i < colorBlackAndWhiteArray.length; i++)
					{
						if(document.getElementById(colorBlackAndWhiteArray[i]))document.getElementById(colorBlackAndWhiteArray[i]).checked = true;
					}
					if(document.getElementById(vModelReleaseOnly))document.getElementById(vModelReleaseOnly).checked = false;
					if(document.getElementById(vPhotographerName))document.getElementById(vPhotographerName).value = "";				
					if(document.getElementById(vImageNumbers))document.getElementById(vImageNumbers).value = "";
					if(document.getElementById(vPointOfView))document.getElementById(vPointOfView).selectedIndex = 0;
					if(document.getElementById(vPeopleInImage))document.getElementById(vPeopleInImage).selectedIndex = 0;
					if(document.getElementById(vImmediateAvailability))document.getElementById(vImmediateAvailability).selectedIndex = 0;
					if(document.getElementById(vSearchImageImageSets))document.getElementById(vSearchImageImageSets).selectedIndex = 0;
				}
				
			if(vPanelName == 'outline')
			{
					if(document.getElementById(vImageNumbers))document.getElementById(vImageNumbers).value = "";
					if(document.getElementById(vDatePublished))document.getElementById(vDatePublished).value = vDateFormat;	
					if(document.getElementById(vMagazine))document.getElementById(vMagazine).value = "";	
					if(document.getElementById(vPhotographerName))document.getElementById(vPhotographerName).value = "";	
					if(document.getElementById(vLocation))document.getElementById(vLocation).value = "";
					if(document.getElementById(vPeopleInImage))document.getElementById(vPeopleInImage).selectedIndex = 0;
					if(document.getElementById(vPointOfView))document.getElementById(vPointOfView).selectedIndex = 0;
					if(document.getElementById(vNoPlacement))document.getElementById(vNoPlacement).checked = false;
					if(document.getElementById(vDatePhotoOrCreated))document.getElementById(vDatePhotoOrCreated).value = "";
			}
			if(vPanelName == 'rfsearch')
			{
					if(document.getElementById(vImageNumbers))document.getElementById(vImageNumbers).value = "";
					if(document.getElementById(vDatePhotoOrCreated))document.getElementById(vDatePhotoOrCreated).value = "";
					if(document.getElementById(vLocation))document.getElementById(vLocation).value = "";
					if(document.getElementById(vPhotographerName))document.getElementById(vPhotographerName).value = "";
					if(document.getElementById(vPeopleInImage))document.getElementById(vPeopleInImage).selectedIndex = 0;
					if(document.getElementById(vPointOfView))document.getElementById(vPointOfView).selectedIndex = 0;
				
			}
			if(vPanelName == 'sygma')
			{
					if(document.getElementById(vImageNumbers))document.getElementById(vImageNumbers).value = "";
					if(document.getElementById(vDatePhotoOrCreated))document.getElementById(vDatePhotoOrCreated).value = "";
					if(document.getElementById(vLocation))document.getElementById(vLocation).value = "";
					if(document.getElementById(vImmediateAvailability))document.getElementById(vImmediateAvailability).selectedIndex = 0;
					if(document.getElementById(vSearchImageImageSets))document.getElementById(vSearchImageImageSets).selectedIndex = 0;
				
			}
				
				if(orientationArray.length > 0)
				{
			//	Orientations reset
			for(var i = 0; i < orientationArray.length; i++)
			{
						if(document.getElementById(orientationArray[i]))document.getElementById(orientationArray[i]).checked = true;
			}
				}
				
				if(vPanelName != 'currentevents')
				{
			// Images Made Available reset
					if(document.getElementById(vInTheLastRadio))document.getElementById(vInTheLastRadio).checked = true;
					if(document.getElementById(vInTheLastTextBox))document.getElementById(vInTheLastTextBox).value = "";
					if(document.getElementById(vBetweenTextBox))document.getElementById(vBetweenTextBox).value = vDateFormat;	// localize this
					
					if(document.getElementById(vAndTextBox))document.getElementById(vAndTextBox).value = GetTodaysDateString();
				}
			}
			catch(e)
			{
			}
			
			
		}

		function GetTodaysDateString()
		{
			//set today's date
				var date = new Date();	
				var dateString = date.getMonth()+1 + "/" + date.getDate() + "/" + date.getFullYear();
						
				if(vLanguageCode != 'en-US')
				{
					dateString =  date.getDate() + "/" + (date.getMonth()+1) + "/" + date.getFullYear();
				}
				return dateString;
		}

        function CheckCollection(collectionNumber)
        {
			document.getElementById(collectionsCheckAll[collectionNumber]).checked = true;
			for (var i=0; i < document.getElementById(collectionsArray[collectionNumber]).options.length; i++)
			{
				if (!document.getElementById(collectionsArray[collectionNumber]).options[i].selected) 
				{
					document.getElementById(collectionsCheckAll[collectionNumber]).checked = false;
					break;
				}
			}
		}
		
		function SelectAllCollections(collectionNumber)
		{
			for (var i=0; i < document.getElementById(collectionsArray[collectionNumber]).options.length; i++)
			{
				document.getElementById(collectionsArray[collectionNumber]).options[i].selected = document.getElementById(collectionsCheckAll[collectionNumber]).checked;
			}
		}

		function CheckCategories()
		{
			var isEveryBoxChecked = true;
						
			for(i = 0; i < categoryArray.length; i++)
			{
				if(!document.getElementById(categoryArray[i]).checked)
				{
					isEveryBoxChecked = false;
				}				
			}
			
			if(isEveryBoxChecked)
			{
				document.getElementById(categoryCheckAll).checked = true;
			}
			else
			{
				document.getElementById(categoryCheckAll).checked = false;
			}					
		}
		
		
		
		function SelectAllCategories()
		{
		
			// Add exception for categories set by landing pages
			if(typeof(categorySelected) != "undefined" && categorySelected != "")
			{
			
				// Split the categorySelected into an array of #'s
				var categoriesSelected = categorySelected.split(",");
				// Uncheck every category
				document.getElementById(categoryCheckAll).checked = false;
				for(var i = 0; i < categoryArray.length; i++)
				{
					document.getElementById(categoryArray[i]).checked = false;
					for (var j=0; j < categoriesSelected.length; j++)
					{
						// Check the ones in the categorySelect
						if (categoriesSelected[j] == categoryValueArray[i]) 
						{
							document.getElementById(categoryArray[i]).checked = true;
						}
					}
				}
				categorySelected = "";
			} else {
				if(document.getElementById(categoryCheckAll).checked)
				{
					for(var i = 0; i < categoryArray.length; i++)
					{
						document.getElementById(categoryArray[i]).checked = true;
					}				
				}
				else
				{
					for(var i = 0; i < categoryArray.length; i++)
					{
						document.getElementById(categoryArray[i]).checked = false;
					}				
				}
			}
		}
		
			

/// <summary>
/// popupHelp()
/// Global redirect
///	</summary>	
function popupHelp()
{
	openHelp('/help/default.aspx?tab=content&pg=/Search/Search_Tips/Search_Tips_Overview.htm');
}
/// <summary>
/// Check date for mm/dd/yyyy format
///	between max and min dates
///	</summary>

// Declaring valid date character, minimum year and maximum year
// Change this for different date range
var dtCh= "/";
var minYear=1900;
var maxYear=2020;

/// <summary>
/// Used by isDate.
///	</summary>
function isInteger(s){
	var i;
    for (i = 0; i < s.length; i++){   
        // Check that current character is number.
        var c = s.charAt(i);
        if (((c < "0") || (c > "9"))) return false;
    }
    // All characters are numbers.
    return true;
}
/// <summary>
/// Used by isDate.
///	</summary>
function stripCharsInBag(s, bag){
	var i;
    var returnString = "";
    // Search through string's characters one by one.
    // If character is not in bag, append to returnString.
    for (i = 0; i < s.length; i++){   
        var c = s.charAt(i);
        if (bag.indexOf(c) == -1) returnString += c;
    }
    return returnString;
}
/// <summary>
/// Used by isDate.
///	</summary>
function daysInFebruary (year){
	// February has 29 days in any year evenly divisible by four,
    // EXCEPT for centurial years which are not also divisible by 400.
    return (((year % 4 == 0) && ( (!(year % 100 == 0)) || (year % 400 == 0))) ? 29 : 28 );
}
/// <summary>
/// Used by isDate.
///	</summary>
function DaysArray(n) {
	for (var i = 1; i <= n; i++) {
		this[i] = 31
		if (i==4 || i==6 || i==9 || i==11) {this[i] = 30;}
		if (i==2) {this[i] = 29}
   } 
   return this;
}
/// <summary>
/// Main date checking function
///	</summary>
function isDate(dtStr){
	var daysInMonth = DaysArray(12);
	var pos1=dtStr.indexOf(dtCh);
	var pos2=dtStr.indexOf(dtCh,pos1+1);
	var strMonth=dtStr.substring(0,pos1);
	var strDay=dtStr.substring(pos1+1,pos2);
	var strYear=dtStr.substring(pos2+1);
	strYr=strYear;
	if (strDay.charAt(0)=="0" && strDay.length>1) 
	{
		strDay=strDay.substring(1);
	}
	if (strMonth.charAt(0)=="0" && strMonth.length>1) 
	{
		strMonth=strMonth.substring(1);
	}
	for (var i = 1; i <= 3; i++) 
	{
		if(strYr.charAt(0)=="0" && strYr.length>1)
		{
			strYr=strYr.substring(1)
		}
	}
	month=parseInt(strMonth);
	day=parseInt(strDay);
	year=parseInt(strYr);
	if (pos1==-1 || pos2==-1)
	{
		alert(vPleaseEnterDateError + "\n" + " " + vDateFormatError);
		return false;
	}
	if (strMonth.length<1 || month<1 || month>12)
	{
		alert(strMonth + " " + vInvalidMonthError);
		return false;
	}
	if (strDay.length<1 || day<1 || day>31 || (month==2 && day>daysInFebruary(year)) || day > daysInMonth[month])
	{
		alert(strDay + " " + vIsNotADayInError + " " + strMonth);
		return false;
	}
	if (strYear.length != 4 || year==0 || year<minYear || year>maxYear)
	{
		alert(vInvalidYearError);
		return false;
	}
	if (dtStr.indexOf(dtCh,pos2+1)!=-1 || isInteger(stripCharsInBag(dtStr, dtCh))==false)
	{
		alert(vPleaseEnterDateError);
		return false;
	}
return true;
}
/// <summary>
/// Used to implement submit 
/// on pressing ENTER when
/// cursor is in KeyWordTextBox
/// in Netscape.  Called from
/// KeywordSearch user control.
///	</summary>
function checkIt(e)
{	
	// If Netscape e.which if not e.keyCode
	var charCode = (navigator.appName == "Netscape") ? e.which : e.keyCode;
	// charCode for enter key
	if(charCode == 13)
	{
		// Click the search button...
		document.getElementById(vSearchButton).click();
	}
}



/* Image Preview Control javascript functions */

function fnEnableHighlight(imgObj, corbisID)
{
	if(document.getElementById(imgObj + corbisID))	
	    document.getElementById(imgObj + corbisID).src = "/images/btn_box_on_cropped.gif";
	
	 if(document.getElementById('panel' + corbisID))	 
	    document.getElementById('panel' + corbisID).className = "PanelHighlightOn";
	
}

function fnAddToLightBox(productKey, corbisID, imgObj, hiddenLightBox, IsLoggedOn, position)
{
    var omnitureString = GetOmnitureQueryString(position);
	
	if (IsLoggedOn == false)
	{
		//User is NOT logged on
		top.frames[1].addToLightbox('', productKey, corbisID, false);
	}
	else
	{
		//User is logged on
		top.frames[1].location = "/previewpane/previewpanes.aspx?tab=Folders&mainpage=Search&actn=addlightbox&mediauids=" + productKey + "&corbisid=" + corbisID + omnitureString;
		fnEnableHighlight(imgObj, corbisID);
	}
	document.getElementById(hiddenLightBox).value = "";
	
}

function fnAddToQuickPic(productKey, corbisID, imgObj, IsLoggedOn, position)
{
    var omnitureString = GetOmnitureQueryString(position);
	var mainpage = "Search";
	if (typeof(isMyLightboxes) != "undefined") {
		// on the lightbox page
		mainpage = "Folders";
	}
	
	if (IsLoggedOn == false)
	{
		//user is NOT logged on
		top.frames[1].addToQuickPic('', productKey, corbisID, false);
	}
	else
	{
		//user is logged on
		top.frames[1].location = "/previewpane/previewpanes.aspx?tab=quickpic&mainpage=" + mainpage + "&actn=addquickpic&mediauids=" + productKey + "&corbisid=" + corbisID + omnitureString;
	}
	fnEnableHighlight(imgObj, corbisID);
}

function fnAddToCart(productKey, corbisID, imgObj, hiddenCart, IsLoggedOn, position, imgId, panelId)
{
    var omnitureString = GetOmnitureQueryString(position);
	var mainpage = "Search";
	if (typeof(isMyLightboxes) != "undefined") {
		mainpage = "Folders";
		productKeySplit = productKey.split('|');
		if (document.getElementById("cartUIDs").value.indexOf(productKeySplit[0]) > -1) {
			alert(folderMessages[5]);
			return;
		} else {
			document.getElementById("cartUIDs").value = document.getElementById("cartUIDs").value + "," + productKeySplit[0];
		}
	}
	
	if (IsLoggedOn == false)
	{
		//User is NOT logged on
		window.top.frames[1].addToCart(productKey, corbisID);
	}
	else
	{
		//User is logged on
		window.top.frames[1].location = "/previewpane/previewpanes.aspx?mainpage=" + mainpage + "&actn=addcart&mediauids=" + productKey + "&corbisid=" + corbisID + omnitureString;
		fnEnableHighlight(imgObj, corbisID);
		
	}
	
	var img = document.getElementById(imgId);
    var panel = document.getElementById(panelId);
    showCart(img, panel);
}
function BasicAddtoCart(imgId, panelId, mediauids, corbisid)
{
    //User is logged on
    var img = parent.frames[0].document.getElementById(imgId);
    var panel = parent.frames[0].document.getElementById(panelId);
    showCart(img, panel);
	window.top.frames[1].location = "/previewpane/previewpanes.aspx?tab=lightbox&mainpage=Search&actn=addcart&mediauids=" + mediauids + "&corbisid=" + corbisid;

}

function showCart(img, panel){
    var frameWidth;
    if (document.documentElement && document.documentElement.clientWidth)
	{
		frameWidth = document.documentElement.clientWidth;
	}
	else if (document.body)
	{
		frameWidth = document.body.clientWidth;
	}
    panel.style.display = "block";
    var cur = findPos(img);
    var left = cur[0] + (img.offsetWidth - panel.offsetWidth) / 2;
    var top = cur[1] + img.offsetHeight;
    if (left < 0) { left = 0; }
    if ((left + panel.offsetWidth) > frameWidth) { 
        left = frameWidth - panel.offsetWidth - 5;
    }
    panel.style.left = left +"px";
    panel.style.top = top +"px"; 
}

function findPos(obj) {
    var curleft = curtop = 0;
    if (obj.offsetParent) {
	    curleft = obj.offsetLeft
	    curtop = obj.offsetTop
	    while (obj = obj.offsetParent) {
		    curleft += obj.offsetLeft
		    curtop += obj.offsetTop
	    }
    }	   
    return [curleft,curtop];	    
}

function CloseCart(panelId)
{
    var panel = document.getElementById(panelId);
    panel.style.display="none";
}

function GetOmnitureQueryString(imgPos)
{
    
    var omnitureString = '';
	if(document.getElementById('imagesperpage'))
	{
	    omnitureString += "&ipp=" + document.getElementById('imagesperpage').value;
	}
	if(document.getElementById('pageNo'))
	{
	    omnitureString += "&pg=" + document.getElementById('pageNo').value;
	}
	if(document.getElementById('searchguid'))
	{
	    omnitureString += "&searchuid=" + document.getElementById('searchguid').value;
	}
	if(document.getElementById('segment'))
	{
	    omnitureString += "&segment=" + document.getElementById('segment').value;
	}
	
	omnitureString += "&pos=" + imgPos;
	
	return omnitureString

}
	
function fnRequestPricing(corbisID, productKey)
{
	MM_openBrWindow('/popup/requestPricing.aspx?mdid=' + corbisID + '&mediauids=' + productKey, 'requestPopup', 'status=yes,scrollbars=yes,resizable=yes,width=570,height=500,top=0,left=0');
}

function fnRFPricing(productKey, area,position)
{
	MM_openBrWindow('/popup/RFPricing.aspx?mediauids=' + productKey + '&Area=' + area + GetOmnitureQueryString(position), 'requestPopup', 'status=yes,scrollbars=yes,resizable=yes,width=770,height=570,top=0,left=0');
}

function fnRMPricing(productKey,position)
{
	MM_openBrWindow('/popup/RMPricing.aspx?mediauids=' + productKey + GetOmnitureQueryString(position), 'requestPopup', 'status=yes,scrollbars=yes,resizable=yes,width=770,height=570,top=0,left=0');
}

function fnFastLane(productKey, position,width,height)
{
    MM_openBrWindow('/popup/FastLane.aspx?id=' + productKey +  GetOmnitureQueryString(position),'requestPopup', 'status=no,scrollbars=yes,resizable=yes,width=' + width + ',height=' + height + ',top=0,left=0');
}

function setCartCount(numOfItems)
{
	try
	{
		var link = document.getElementById(aCartLink);
		
		if(link != null)
		{	
			
			//get the current count
			var currentText = "";
			if(link.innerText)
			{
				currentText=link.innerText;
			}
			else
			{
				currentText = link.text; //firefox
			}
			
			var spaceIndex = currentText.indexOf(" ");
			
			var cartUIDCount = parseInt(currentText.substr(0,spaceIndex));		
				
			//replace it with the new count
			var expression = new RegExp(cartUIDCount);
			var newText = currentText.replace(expression, numOfItems);				
			if(link.innerText)
			{
				link.innerText = newText;
			}
			else
			{
				link.innerHTML = newText;	
			}
		}
		
	}
	catch(e)
	{
		
	
	}
}

function fnGetLightBoxImage(corbisid)
{
	
	var lightBoxImg = document.getElementById("imgLB" + corbisid);
	
	return lightBoxImg;
}

function fnGetCartImage(corbisid)
{
	
	var cartImg = document.getElementById("imgCart" + corbisid);
	
	return cartImg;
}

function fnGetQuickPicImage(corbisid)
{
//alert(corbisid + " 2 ");

	var quickPicImg = document.getElementById("imgQPic" + corbisid);
	
	return quickPicImg;
}

function fnRemoveFromLightBox(corbisid)
{
//alert("fnRemoveFromLightBox corbisid = " + corbisid);
	fnDisableHighlight(corbisid, "imgLB");
}

function fnRemoveFromCart(corbisid)
{
	fnDisableHighlight(corbisid, "imgCart");
}

function fnRemoveFromQuickPic(corbisid)
{
//alert(corbisid + " 1 ");
	fnDisableHighlight(corbisid, "imgQPic");
}

function fnAddToLightBoxFromPP(corbisid)
{
	fnEnableHighlightForPP(corbisid, fnGetLightBoxImage(corbisid));
}

function fnAddToCartFromPP(corbisid)
{
	fnEnableHighlightForPP(corbisid, fnGetCartImage(corbisid));
}

function fnAddToQuickPicFromPP(corbisid)
{
	fnEnableHighlightForPP(corbisid, fnGetQuickPicImage(corbisid));
}
function fnCanDisable2(img)
{
//	alert("into fnCanDisable");
	if (img == null)
	{
//alert("img is null");
		return true;
	}
	else
	{
		if (img.src.indexOf("btn_box_off_cropped.gif") >= 0)
		{
//alert(img.id + " : " + img.src + " img is off : return true");
			return true;
		}
		else
		{
//alert(img.id + " : " + img.src + " img is on : return false");
			return false;
		}
	}
}

function fnCanDisable(corbisID)
{
	if (!document.getElementById('imgLB' + corbisID))
		return true;
	else
	{
		if ( (!document.getElementById('imgLB' + corbisID).src))
			return true;
		else
			{
				if (document.getElementById('imgLB' + corbisID).src.indexOf("btn_box_off_cropped.gif") >= 0)
				return true;
			}		
	}
	
	if (!document.getElementById('imgCart' + corbisID))
		return true;
	else
	{
		if ( (!document.getElementById('imgCart' + corbisID).src))
			return true;
		else
			{
				if (document.getElementById('imgCart' + corbisID).src.indexOf("btn_box_off_cropped.gif") >= 0)
				return true;
			}		
	}
	
	return false;
}

function fnDisableHighlight(corbisID, img)
{

	if (fnCanDisable(corbisID))
	{
		if(document.getElementById('panel' + corbisID))
		{
			document.getElementById('panel' + corbisID).className = "PanelHighlightOff";
		}
	}
	
	if (document.getElementById(img + corbisID))
	{			
		document.getElementById(img + corbisID).src = "/images/btn_box_off_cropped.gif";
	}	

}

function fnEnableHighlightForPP(corbisID, img)
{
	//background
	if(document.getElementById('panel' + corbisID))
	{
	document.getElementById('panel' + corbisID).className = "PanelHighlightOn";
	}
	
	//image
	if (img != null)
	{
		
		img.src = "/images/btn_box_on_cropped.gif";
	}
}

/* Image Preview Control javascript functions */

/* Search Assistance javascript functions */

function VerifyCheckboxes()
{
	var checkboxColl = document.getElementsByTagName("input");
	var aleastOneChecked = false;
	for (i = 0; i < checkboxColl.length; i++)
	{
		if (checkboxColl[i].type.toLowerCase() == "checkbox")
		{
			if (checkboxColl[i].getAttribute("mode") == "clarification")
			{
				if (checkboxColl[i].checked == true)
				{
					aleastOneChecked = true;
				}
			}
		}
	}
	if (aleastOneChecked == true)
	{
		return true;
	}
	else
	{
		var hiddenColl = document.getElementsByTagName("input");
		for (j = 0; j < hiddenColl.length; j++)
		{
			if (hiddenColl[j].type.toLowerCase() == "hidden")
			{
				if (hiddenColl[j].getAttribute("mode") == "clarification")
				{
					alert(hiddenColl[j].value);
				}
			}
		}
		return false;
	}
}

function fnAddImageSetToLightbox(isuid, IsLoggedOn)
{
	var imgArrayNew = document.getElementsByTagName("img");

	for (i = 0; i < imgArrayNew.length; i++)
	{
	    
		if (imgArrayNew[i].id.indexOf("imgLB") >= 0) 
		{
			imgArrayNew[i].src = "/images/btn_box_on_cropped.gif";
		    var pnlName = 	imgArrayNew[i].id;
		    pnlName = pnlName.replace('imgLB','panel');		    
		    document.getElementById(pnlName).className = "PanelHighlightOn";
		}
	}

	if (IsLoggedOn == false)
	{
		//User is NOT logged on
		top.frames[1].addToLightbox('addimageset', isuid, '', false);
	}
	else
	{
		//User is logged on
		top.frames[1].location = "/previewpane/previewpanes.aspx?tab=lightbox&mainpage=Search&actn=addimageset&mediauids=" + isuid + "&corbisid=";
	}
}

function fnAddSessionSetToLightbox(ssuid, IsLoggedOn)
{
	var imgArrayNew = document.getElementsByTagName("img");

	for (i = 0; i < imgArrayNew.length; i++)
	{
	    
		if (imgArrayNew[i].id.indexOf("imgLB") >= 0) 
		{
			imgArrayNew[i].src = "/images/btn_box_on_cropped.gif";
		    var pnlName = 	imgArrayNew[i].id;
		    pnlName = pnlName.replace('imgLB','panel');		    
		    document.getElementById(pnlName).className = "PanelHighlightOn";
		}
	}

	if (IsLoggedOn == false)
	{
		//User is NOT logged on - should not be an issue since login is required for 
		top.frames[1].addToLightbox('addsessionset', ssuid, '', false);
	}
	else
	{
		//User is logged on
		top.frames[1].location = "/previewpane/previewpanes.aspx?tab=lightbox&mainpage=Search&actn=addsessionset&mediauids=" + ssuid + "&corbisid=";
	}
}

function SetImagesMadeAvailableDate()
{	
	if(vAndTextBox)
	{
	if(document.getElementById(vAndTextBox).value == vDateFormat)
	{
		
		document.getElementById(vAndTextBox).value = GetTodaysDateString();
	}
	}
}

function openLogin()
{
			var popup = window.top.open('/login/login.aspx','pop','width=550,height=550');
			popup.focus();
}
		
function openHelp(url)
{
	// Open help window without SSL
	if(window.location.href.indexOf("https://") == 0) {
		url = HttpUrl + url;
		}
			var attr = "status=no,scrollbars=yes,resizable=yes,width=570,height=550";
	if (!url) {
		url = "/help";
	}
			var helpWin = window.open(url, "Help", attr);
			helpWin.focus();
}

function openChangeRoles()
{
			var popup = window.open('/login/ChangeRoles.aspx','pop','width=550,height=550,scrollbars=yes');
			popup.focus();
}
updateAdvancedSearchFromCookie();
/* Search Assistance javascript functions */

