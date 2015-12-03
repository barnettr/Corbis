var PriceCategories = new Array(2);

function RFSize()
{
	this.sizeCode = "";
	this.price = "";
}


function ChangePrices(dropdown,offeringuid, isPremium)
{
	
	
	if(isPremium)
	{
		pricing = PriceCategories[1]
	}
	else
	{
		pricing = PriceCategories[0]
	}	
	
	var seat = dropdown.selectedIndex;
	if(!pricing) return;
	for(var j=0;j < pricing[seat].length;j++)
	{
		try
		{
			document.getElementById(offeringuid + "_" + pricing[seat][j].sizeCode).innerHTML = pricing[seat][j].price;
		}
		catch(e)
		{		
		}		
		
	}


}
