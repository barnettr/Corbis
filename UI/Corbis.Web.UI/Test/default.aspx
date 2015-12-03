<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/NoSearchBar.Master" %>
<%@ Register Assembly="Corbis.Web.UI.Controls" Namespace="Corbis.Web.UI.Controls" TagPrefix="Corbis" %>

<script runat="server">

    protected void GlassButtonEx3_Click(object sender, EventArgs e)
    {
        this.Response.Write("<h3>hey, you just clicked the button fullPostBackWithProgress.</h3>");
    }
</script>

<asp:Content ID="defaultContent" ContentPlaceHolderID="mainContent" runat="server">
<div style="height:150px"></div>

<div style="margin-left: 20px; width: 400px;">
<h1>Code Examples</h1>
<h3>#1: Modal popup</h3>
<blockquote>
This is a standard modal containing a single button.
</blockquote>

<Corbis:ModalPopup ContainerID="example1" runat="server" Width="300" Title="Example Title" Message="Example Popup">
    <Corbis:GlassButton runat="server" CausesValidation="false" Text="Close me" OnClientClick="HideModal('example1');return false;" />  
</Corbis:ModalPopup>

<a href="javascript:OpenModal('example1')">Open modal example 1</a>

<hr />

<h3>#2: Faux-Modal popup</h3>
<blockquote>
This is our faux-modal.  The background does not dim (well, a little), and the popup will be on top of the opening link.
<p>
I did have an issue passing in "this" to the opener as the centerOverElement; it currently does not work.  Since we aren't using it that way,
it shouldn't be a huge issue.  If you use the TextIconButtonControl (second link below), it changes the "this" in the call to the actual link ID, so
it works just fine.
</p>
</blockquote>

<script>
function OpenExample2(link)
{
    new CorbisUI.Popup('example2', { 
        showModalBackground: false,
        centerOverElement: link,
        closeOnLoseFocus: true,
        positionVert: 'middle', 
        positionHoriz: 'right'
    });    
}
</script>
<Corbis:ModalPopup ContainerID="example2" runat="server" Width="300" Title="Example 2" Message="Example Popup">
    <Corbis:GlassButton runat="server" CausesValidation="false" Text="Close me" OnClientClick="HideModal('example2');return false;" />  
</Corbis:ModalPopup>

<a id="testLink" href="javascript:OpenExample2('testLink')">Open modal example 2</a>
<br /><br />
<Corbis:TextIconButton  ID="openExample2TextIconButton" runat="server" Icon="clearAll" 
                        Text="Open modal example 2" OnClientClick="OpenExample2(this);" 
                    />


<hr />
<h3>#3: Faux-Modal popup from Div HTML</h3>
<blockquote>
    Notes: Uses CloseScript property of Corbis:ModalPopup.  This is needed because the default behavior of the close action is to hide the 
    containing div, not destroy it.  This example uses MochaUI.CloseModal for the close action.
    <p style="font-weight: bold">
    Any modal created from the Div HTML (createFromHTML: true) will need to implement this.
    </p>
</blockquote>
<script>
function OpenExample3(link)
{
    new CorbisUI.Popup('example3', { 
        createFromHTML: true,
        showModalBackground: false,
        centerOverElement: link,
        closeOnLoseFocus: true,
        positionVert: 'middle', 
        positionHoriz: 'right',
        replaceText: [ new Date().toDateString(), new Date().toLocaleTimeString() ]
    });    
}
</script>
<Corbis:ModalPopup ContainerID="example3" CloseScript="MochaUI.CloseModal('{0}');return false;" runat="server" Width="300" Title="Example 3" Message="This is an example.  Current date: {0}, current time: {1}">
    <Corbis:GlassButton runat="server" CausesValidation="false" Text="Close me" OnClientClick="MochaUI.CloseModal('example3');return false;" />  
</Corbis:ModalPopup>

<a id="A1" href="javascript:OpenExample3('A1')">Open modal example 3</a>

<hr />


<h3>#4: Styled Modal popup</h3>
<blockquote>
    How to style and structure modals to maintain consistency.  Widths vary.  There are no standards.
    <p style="font-weight: bold">
    Each modal will need to implement its styles.
    </p>
</blockquote>
<script>
    function OpenExample4(link) {
        new CorbisUI.Popup('example4', {
            createFromHTML: true,
            showModalBackground: false,
            centerOverElement: link,
            closeOnLoseFocus: true,
            positionVert: 'middle',
            positionHoriz: 'right',
            replaceText: [new Date().toDateString(), new Date().toLocaleTimeString()]
        });
    }
</script>
<Corbis:ModalPopup ID="example4" ContainerID="example4" CloseScript="MochaUI.CloseModal('{0}');return false;" runat="server" Width="300" Title="Example 4"
meta:resourcekey="message">

<p><span class="emphasize">This is text that is emphasized.</span>
This is the main body of text. Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Integer convallis. Nam ac lacus. Nullam tellus. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nam condimentum, dui ac varius tincidunt, magna leo sagittis nunc, eget condimentum elit purus at dui. Suspendisse tempor. </p>
<div class="deEmphasize">This text is a footnote, a side-comment.</div>    <Corbis:GlassButton ID="GlassButton1" runat="server" CausesValidation="false" Text="Close me" OnClientClick="MochaUI.CloseModal('example4');return false;" />  
</Corbis:ModalPopup>

<a id="A2" href="javascript:OpenExample4('A1')">Open modal example 4</a>

<hr />



</div>

<div style="padding:10px;background:#333333;border:solid 1px #666666;" id=checkboxes>
<script type="text/javascript">
var _cb = '<%=icb3.ClientID %>';

function selectAllChecks(checked)
{
    var cbColl = $('checkboxes').getElements('div.imageCheckbox');
    cbColl.each(function(cb){
            setCheckedNoEvent(cb, checked)
        });
}

function toggleGBDisabled()
{
    var gb = $('<%=gb8.ClientID %>');
    setGlassButtonDisabled(gb, !gb.hasClass('DisabledGlassButton'));
    return false;
}

function toggleTIBDisabled()
{
    var tib = $('<%=tib3.ClientID %>');
    setTextIconButtonDisabled(tib, !tib.hasClass('disabled'));
    return false;
}
</script>
<Corbis:ImageRadioButton OnClientChanged="alert('this.ClientID got clicked')" ContainerID=checkboxes GroupName=test id=ImageRadioButton1 runat=server Text="happy" />

<Corbis:ImageRadioButton ContainerID=checkboxes GroupName=test id=ImageRadioButton2 runat=server Text="ecstatic" />

<Corbis:ImageRadioButton ContainerID=checkboxes GroupName=test id=ImageRadioButton3 runat=server Text="blissed out" />


<corbis:ImageCheckbox OnClientChanged="alert('I (' + this.id + ') just got clicked')" TextStyle=Heading
   id=icb1 runat=server Text="Image checkbox (TextStyle=Heading) that responds to clicks"
/>

<Corbis:ImageCheckbox ID="icb2" runat="server" Text="Another image checkbox (TextStyle=Link) that just sits here. I bring nothing."
    TextStyle="Link" />

<corbis:ImageCheckbox OnClientChanged="alert('Hi, my checked state is ' + this.checked)"
   id=icb3 runat=server Text="my image checkbox that is gettin scripted (No TextStyle)"
/>
</div>
<div style="padding:10px;">
    <Corbis:GlassButton ID=gb1 OnClientClick="toggleCheckedState(_cb);return false;" runat=server Text="I Toggle"  />
    &nbsp;
    <Corbis:GlassButton ID=gb2 OnClientClick="setCheckedState(_cb,true);return false;" runat=server Text="I Check"  />
    &nbsp;
    <Corbis:GlassButton ID=gb3 OnClientClick="setCheckedState(_cb,false);return false;" runat=server Text="I Uncheck"  />
    <br /><br />
    <Corbis:GlassButton ButtonStyle="Gray" ID="gb4" OnClientClick="toggleCheckedStateNoEvent(_cb);return false;" runat=server Text="I Toggle Silently"  />
    &nbsp;
    <Corbis:GlassButton ID="gb5" runat="server" ButtonStyle="Gray" OnClientClick="setCheckedNoEvent(_cb,true);return false;" Text="I Check Silently" />
    &nbsp;
    <Corbis:GlassButton ButtonStyle="Gray" ID=gb6 OnClientClick="setCheckedNoEvent(_cb,false);return false;" runat=server Text="I Uncheck Silently"  />
    <br /><br />
    <Corbis:GlassButton ID=gb7 OnClientClick="alert(getCheckedState(_cb));return false;" runat=server Text="I alert checked state"  />
    <br /><br />
   
    <Corbis:TextIconButton Text="Select All" Icon=selectAll runat=server ID=tib1 OnClientClick=selectAllChecks(true) />&nbsp;
    <Corbis:TextIconButton ID="tib2" runat="server" Icon="clearAll" OnClientClick="selectAllChecks(false)"
        Text="Clear All" />
        
    <br /><br />
    
    <Corbis:TextIconButton Text="I toggle glassbutton disabled state" Icon=clearAll OnClientClick=toggleGBDisabled() runat=server ID=tib3 />
    &nbsp;
    <Corbis:GlassButton Text="I toggle icon button disabled state" ID=gb8 OnClientClick="toggleTIBDisabled();return false" runat=server />
    
<h3>#5: GlassButton Enhanced</h3>
<blockquote>
    These buttons blocks multiple clicks and can have progress signals.
    <p style="font-weight: bold">
    Refer to GlassButonEx for detail.
    </p>
</blockquote>
    
    <Corbis:GlassButtonEx Text="disable in 1 seconds" ID="gb9" DisablePeriod="1000" OnClientClick="return false;" runat="server" ProgressPosition="Left"  />
    <Corbis:GlassButtonEx Text="disable forever" ID="GlassButtonEx1" DisablePeriod="-1" OnClientClick="return false;" runat="server" ProgressPosition="Right"  />
    <Corbis:GlassButtonEx Text="never disable" ID="GlassButtonEx2" OnClientClick="return false;" runat="server" ProgressPosition="Right"  />
    
    <Corbis:GlassButtonEx Text="fullPostBackWithProgesss" ID="GlassButtonEx3" runat="server" 
        ProgressPosition="Right" onclick="GlassButtonEx3_Click"  />
</div>

<div>
    <Corbis:GlassButton id="saveFavoriteUsageButton" ButtonStyle="Outline" runat="server" Text="This is a test" />

</div>
<br /><br />

<div>
    <h3>Login using SSL in a popup window</h3>
    <script>
        function OpenLoginSSL()
        {
            var signIn = 'https://' + window.location.hostname + '/Test/SignIn.aspx';
            alert("Opening " + signIn);
            OpenNewIModal(signIn, 500, 400, 'SecureSignIn')
        }
    
        function handleSignIn(returnUrl)
        {
            MochaUI.CloseModal('SecureSignIn');
            alert("Modal closed, redirecting to " + returnUrl);
            window.location = returnUrl;
        }
        
        function handleAlreadySignedIn()
        {
            MochaUI.CloseModal('SecureSignIn');
            OpenModal('alreadySignedIn');
        }
    </script>

    <Corbis:GlassButton runat="server" OnClientClick="OpenLoginSSL();return false;" Text="I open the login modal securely!!!" />

    <Corbis:ModalPopup runat="server" containerId="alreadySignedIn" Title="Oops!" Message="You are already signed in!" />
</div>

<a href="http://pro.localhost.corbis.pre:8080/mediasetsearch/mediasetsearch.aspx?qlnk=iraq&rdt=-10000&settype=5"> test for MediaSet page</a>
</asp:Content>