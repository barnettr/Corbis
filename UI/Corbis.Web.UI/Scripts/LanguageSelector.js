
function HighlightDiv(divSelected)
{     
     divSelected.style.backgroundColor='#b4b4b4';  
      
     
     if (divSelected.getAttribute("class")=="menuItem selectedIcondiv" )
    {
        divSelected.style.backgroundImage="url('../Images/LanguageCheck.jpg')";
	    divSelected.style.backgroundPosition = "right -36px";
	    divSelected.style.backgroundRepeat= "no-repeat";
    
    }
}

function DeHighlightDiv(divSelected)
 { 
    divSelected.style.backgroundColor='#CCC';
    if (divSelected.getAttribute("class")=="menuItem selectedIcondiv" )
    {
        divSelected.style.backgroundImage="url('../Images/LanguageCheck.jpg')";
	    divSelected.style.backgroundPosition = "right -4px";
	    divSelected.style.backgroundRepeat= "no-repeat";
    
    }
   

}

