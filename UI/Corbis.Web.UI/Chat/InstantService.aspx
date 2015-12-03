<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InstantService.aspx.cs" Inherits="Corbis.Web.UI.Chat.InstantService" Title="<%$ Resources: windowTitle %>"%>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <script type="text/javascript" language="javascript">
        function validateForm()
        {
            if (document.getElementById("fname").value == "") 
            {
                document.getElementById("trFname").style.backgroundColor="#F7F7EF";
                document.getElementById("trFname").style.color="red";
                document.getElementById("trFname").style.fontWeight="bold";
                return false;
            } else {
                return true;
            }
        }
       </script>
</head>
<body>
    <form name="chat_form"  method="post"  action="https://admin.instantservice.com/Customer">
    <div class="instantServiceTitle" >
        <Corbis:Localize   ID="pageTitle" runat="server" meta:resourcekey="pageTitle" />
    </div><br /><br />
    
     <div class="bold" >
        <Corbis:Label  ID="reqiredFieldNote" runat="server"  meta:resourcekey="reqiredFieldNote" ></Corbis:Label>
    </div> <br />  
   
      <div  id="trFname" name="trFname" class="instantServiceLabel">
          <Corbis:Label ID="firstNameLabel" runat="server" meta:resourcekey="firstNameLabel" />
      </div>  
      
      <div  class="instantServiceText">
          <input type="text" id="fname" name="fname" runat="server" style="width:250px;"/>           
       </div>
       
       <div class="clear"></div>
       
      <div class="instantServiceLabel">
          <Corbis:Label ID="lastNameLabel" runat="server" meta:resourcekey="lastNameLabel" />
      </div>
      
       <div class="instantServiceText">
          <input type="text" id="lname" name="lname" runat="server" style="width:250px;"/>
          
       </div>   
       
          <div class="clear"></div>
       
       <div  class="instantServiceLabel">
         <Corbis:Label ID="emailAddressLabel" runat="server" meta:resourcekey="emailAddressLabel"  />
      </div>
      
      <div class="instantServiceText">
         <input type="text" name="email" id="email" runat="server" style="width:250px;"  />          
    
      </div>
         
     
       <div class="clear"></div> <br />
    <div  class="instantServiceLabel">    
       <input type="submit" name="Button" id="btnSubmit" runat="server" meta:resourcekey="btnSubmit" onclick="return validateForm();" />     
    </div>
    <div>    
          <input type="hidden" name="ai" value="6163"/>
         <input type="hidden" name="di" id="di" runat="server" value="0"/>
    </div>
    
    </form>
</body>
</html>
