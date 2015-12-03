//' ===========================================================================
//' UserAgentTest - function used for setting client user-agent values.

function UserAgentTest()
{
	var sUsrAgt		=	navigator.userAgent.toLowerCase();
	var iAppVer		=	parseInt( navigator.appVersion );

	var bIs_MacOS		=	sUsrAgt.indexOf( 'mac'     )!=-1;
	var bIs_Mozil		=	sUsrAgt.indexOf( 'mozilla' )!=-1;
	var bIs_Opera		=	sUsrAgt.indexOf( 'opera'   )!=-1;

	var bIs_Spoof		=	bIs_Opera || sUsrAgt.indexOf( 'spoofer' )!=-1;
	var bNo_Mozil		=	bIs_Spoof || sUsrAgt.indexOf( 'compatible' )!=-1;
	var bNo_Gecko		=	bIs_Spoof || sUsrAgt.indexOf( 'like gecko' )!=-1;

	this.Type_Ie4x		=	!bIs_Spoof && sUsrAgt.indexOf( 'msie 4'   )!=-1;
	this.Type_Ie5x		=	!bIs_Spoof && sUsrAgt.indexOf( 'msie 5'   )!=-1;

	this.Type_Ie50		=	!bIs_Spoof && sUsrAgt.indexOf( 'msie 5.0' )!=-1;
	this.Type_Ie55		=	!bIs_Spoof && sUsrAgt.indexOf( 'msie 5.5' )!=-1;

	this.Type_Ie6Up		=	!this.Type_Ie4x && !this.Type_Ie50 && !this.Type_Ie55 &&
					!bIs_Spoof && iAppVer > 3 && sUsrAgt.indexOf( 'msie' )!=-1;

	this.Type_Ns60		=	iAppVer == 5 && !bIs_Spoof && sUsrAgt.indexOf( 'netscape/6' )!=-1;
	this.Type_Ns40		=	iAppVer == 4 &&  bIs_Mozil && !bNo_Mozil;
	this.Type_Ns7Up		=	iAppVer >= 5 &&  bIs_Mozil && !bNo_Mozil && !this.Type_Ns60;

	this.Type_Ns7Mac	=	bIs_MacOS && this.Type_Ns7Up;

	this.Type_Op50		=	sUsrAgt.indexOf( 'opera 5' )!=-1 || sUsrAgt.indexOf( 'opera/5' )!=-1;
	this.Type_Op60		=	sUsrAgt.indexOf( 'opera 6' )!=-1 || sUsrAgt.indexOf( 'opera/6' )!=-1;
	this.Type_Op70		=	sUsrAgt.indexOf( 'opera 7' )!=-1 || sUsrAgt.indexOf( 'opera/7' )!=-1;

	this.Type_Gecko		=	!bIs_Spoof && !bNo_Gecko && sUsrAgt.indexOf( 'gecko'   )!=-1;
	this.Type_Safari	=	sUsrAgt.indexOf( 'safari'  )!=-1;

	this.Type_W3cDom	=	this.Type_Ie55  || this.Type_Ie6Up || this.Type_Ns60 || this.Type_Ns7Up ||
					this.Type_Gecko || this.Type_Op70;

	this.Type_Unkn		=	!this.Type_Ns40 && !this.Type_Ns60 && !this.Type_Ns7Up && !this.Type_Ie4x   && 
					!this.Type_Ie50 && !this.Type_Ie55 && !this.Type_Ie6Up && !this.Type_Safari && 
					!this.Type_Op50 && !this.Type_Op60 && !this.Type_Op70  && !this.Type_Gecko  &&
					!this.Type_Ie5x;

	this.Type_Down		=	!this.Type_Unkn && !( this.Type_W3cDom || this.Type_Ie4x || this.Type_Ie5x );


}
