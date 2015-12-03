/*  Field Check JavaScript Library
Functions Available:
- trimIt
- verifyPassword
- verifyPasswordWithEncryptCheck
- isEmpty
- isPosInteger
- isInteger
*/

<%
Dim m_oFCJSContent
Set m_oFCJSContent = Server.CreateObject("C1UI.ContentProvider")

m_oFCJSContent.CountryCode = CountryCode
m_oFCJSContent.UserLanguageCode = g_sUserLanguageCode
m_oFCJSContent.LoadContent(Server.MapPath("/includes/content/" & g_sUserLanguageCode & "/FieldCheckJS.xml"))
%>

function checkEmailAddress(x){
   var email = x;
  if (x==null) {return false};
    if (email.indexOf("@") > 0){
      if (email.indexOf(".",email.indexOf("@")) > 0){
	     if (email.indexOf(" ") == -1){
		    if (email.indexOf(",") == -1){
			    return true;
		  }
	   }
	 }
   }
  return false;
}

function trimIt(TV){
  var TrimVal = TV;
  if (TV==null) {return false};
  if (TrimVal.length!=0) {
	  //Trim right side
	  if (TrimVal.charAt(0)==" ") {
		  do {
		    if (TrimVal.charAt(0)==" ") {
		       TrimVal = TrimVal.substring(1,TrimVal.length);//
		    } 
		  } while (TrimVal.charAt(0)==" ");
	  }
	  
	  //Trim left side
	  if (TrimVal.charAt(TrimVal.length-1)==" ") {
	     do {
	        if (TrimVal.charAt(TrimVal.length-1)==" ") {
	            TrimVal = TrimVal.substring(0,TrimVal.length-1);//
	        } 
	      } while (TrimVal.charAt(TrimVal.length-1)==" ");
	  }
  }
  return TrimVal;
}

function verifyPassword(P,VP){
    var P1 = P    //creates string object for string values passed
	var VP1 = VP 
	if (P1.length < 6){
	   return "<%=m_oFCJSContent.GetValue("PasswordSixAndTwentyText1")%>\n<%=m_oFCJSContent.GetValue("PasswordSixAndTwentyText2")%>";
	}
	else if (P1.length > 20){
	   return "<%=m_oFCJSContent.GetValue("PasswordSixAndTwentyText1")%>\n<%=m_oFCJSContent.GetValue("PasswordSixAndTwentyText2")%>";
	}
	else if (VP1.length != 0) {
	  if (P1.length == VP1.length){
		   for (var i=0; i < P1.length  ; i++){
		       if (P1.charAt(i) != VP1.charAt(i)){
				 return "<%=m_oFCJSContent.GetValue("ConfirmPasswordText1")%>\n<%=m_oFCJSContent.GetValue("ConfirmPasswordText2")%>";
			   }
		    }
		} 
		else{
		  return "<%=m_oFCJSContent.GetValue("ConfirmPasswordText1")%>\n<%=m_oFCJSContent.GetValue("ConfirmPasswordText2")%>";
		}
	}
	else if (VP1.length ==0){
	  return "<%=m_oFCJSContent.GetValue("ConfirmPasswordText1")%>\n<%=m_oFCJSContent.GetValue("ConfirmPasswordText2")%>";
	}
  return "OK";
}

// Checks password where input passwords may have been encrypted.  
// Allows encrypted 44 character passwords to be passed back and forth
// without failing validation.  The length of any password once 
// encrypted is 44 characters. 
function verifyPasswordWithEncryptCheck(P,VP,IP){
    	var P1 = P    //creates string object for string values passed
	var VP1 = VP 
	var IP1 = IP  //initial password

	if ((P1.length != 44)||(P1 != IP1)||(VP1 != IP1)){
	   if (P1.length < 6){
	      return "<%=m_oFCJSContent.GetValue("PasswordSixAndTwentyText1")%>\n<%=m_oFCJSContent.GetValue("PasswordSixAndTwentyText2")%>";
	   }
	   else if (P1.length > 20){
	      return "<%=m_oFCJSContent.GetValue("PasswordSixAndTwentyText1")%>\n<%=m_oFCJSContent.GetValue("PasswordSixAndTwentyText2")%>";
	   }
	   else if (VP1.length != 0) {
	       if (P1.length == VP1.length){
		   for (var i=0; i < P1.length  ; i++){
		       if (P1.charAt(i) != VP1.charAt(i)){
				 return "<%=m_oFCJSContent.GetValue("ConfirmPasswordText1")%>\n<%=m_oFCJSContent.GetValue("ConfirmPasswordText2")%>";
			   }
		    }
		} 
		else{
		  return "<%=m_oFCJSContent.GetValue("ConfirmPasswordText1")%>\n<%=m_oFCJSContent.GetValue("ConfirmPasswordText2")%>";
		}
	   }
	   else if (VP1.length ==0){
	      return "<%=m_oFCJSContent.GetValue("ConfirmPasswordText1")%>\n<%=m_oFCJSContent.GetValue("ConfirmPasswordText2")%>";
	   }
	}
  return "OK";
}


//general purpose function - input value entered?
function isEmpty(iS){
   var inputStr=trimIt(iS);
   if (inputStr == null || inputStr == ""){
      return true;
   } 
   return false;
}


// general purpose function to see 
// if a numeric input is a positive integer
function isPosInteger(inputVal){
    inputStr = inputVal.toString()
	for (var i=0; i<=inputStr.length; i++){
	  var oneChar = inputStr.charAt(i)
	  if (oneChar < "0" || oneChar > "9") {
	     return false;
	  }	
	}
	return true;
  }

  
 // general purpose function to see if a number is
 // positive or negative
 function isInteger(inputVal){
      inputStr = inputVBal.toString();
	  for (var i=0; i<inputStr.length; i++){
	     var oneChr = inputStr.charAt();
		 if (i==0 && oneChar=="-"){
		  continue;
		  }
		 if (oneChar<"0" || oneChar>"9") {
		   return false;
		 }
	   }
	   return true;
  }
<% Set m_oFCJSContent = Nothing %>