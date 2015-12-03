<%@ Page Language="C#" AutoEventWireup="true" %>
<script runat="server" language="c#">
    private void Page_Load(object sender, System.EventArgs e)
	{
	    if (Request.QueryString["cmd"] == "alreadyLoggedIn")
	    {
	        string returnScript = "alert('I am javascript in a nonsecure iframe in this popup: \\n" + Request.Url.AbsoluteUri + "!');parent.parent.handleAlreadySignedIn()";
            ClientScript.RegisterStartupScript(this.GetType(), "redirectUserSignedIn", returnScript, true);
	    }
	    else
	    {
            string returnScript = "alert('I am javascript in a nonsecure iframe in this popup: \\n" + Request.Url.AbsoluteUri + "!');parent.parent.handleSignIn('" + Request.QueryString["cmd"] + "');";
            ClientScript.RegisterStartupScript(this.GetType(), "redirectUserSignedIn", returnScript, true);
	    }
    }
</script>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>