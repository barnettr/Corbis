



function showDropDownMenu(menu,imageControlId)
{
    
     imageIcon=document.getElementById(imageControlId);
     imageIcon.src="../App_Themes/BlackBackground/Images/ArrowDown.jpg"
     menudiv=document.getElementById(menu);
     menudiv.style.display = "block";   

    return false;
}

function hideDropDownMenu(menu,imageControlId)
{
   imageIcon=document.getElementById(imageControlId);
   imageIcon.src="../App_Themes/BlackBackground/Images/Arrow.jpg";
   menudiv=document.getElementById(menu);
   menudiv.style.display = 'none';           
     
}

function __trapESC(menu)
{
	var key = window.event.keyCode;
	if (key == 27)
	{
		menu.style.display = 'none';
	}
}


  
  
  
  
  
