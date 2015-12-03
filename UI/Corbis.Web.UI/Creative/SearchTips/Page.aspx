<Html>
<head runat="server"></head>
<body>

<div id="TemplateBodyHolder" style="background-image:url(./images/en-US/left_collage.jpg); background-repeat:no-repeat; background-position: 0px -5px; margin-top:0px; width:999px; height:633px; padding:1px 0 0 0; margin-right:auto; margin-left:auto; padding-top:1px;">

  <style type="text/css">
    .bd {
        font-size: 10px;
    }
  </style>
  <link rel="stylesheet" type="text/css" href="./css/searchtips.css" />
  <link rel="stylesheet" type="text/css" href="./css/searchtips_ns.css" />
  <%   
      If InStr(1, Request.ServerVariables("HTTP_USER_AGENT"), "MSIE 7.0") Then
  %> 
    <link rel="stylesheet" type="text/css" href="./css/searchtips_ie.css" />
  <% 
  End If
  %>
  
  <link rel="stylesheet" type="text/css" href="./css/accordion-menu.css"/>
  
  <script type="text/javascript" src="./jslibrary/yui/yahoo-min.js"></script>
  <script type="text/javascript" src="./jslibrary/yui/dom-min.js"></script>
  <script type="text/javascript" src="./jslibrary/yui/event-min.js"></script>
  <script type="text/javascript" src="./jslibrary/yui/animation-min.js"></script>
  <script type="text/javascript" src="./scripts/accordion-menu.js"></script>
  <script type="text/javascript" src="./scripts/searchtips.js"></script>
  <div id="sectionLogo" style="margin:9px 0 10px 47px;">
    <a id="TabLink0" href="javascript:void(0)" onclick="ShowTab(0)" onmouseover="moveTitleSprite(0)" onmouseout="returnSprite()">
        <img src="./images/en-US/searchtips_title.gif" alt="Search Tips" border="0" width="184" height="32">
    </a>
  </div>
  <br clear="all">
  
  <div id="subhead">
    <img src="./images/en-us/searchtip_subtitle.gif" alt="How to get the most out of searching on Corbis" border="0" width="602" height="32">
  </div>
  
  <div id="keywordHead">
    <img src="./images/en-us/keyword_search.gif" alt="Keyword Search" border="0" width="283" height="85">
  </div>
  
  <br clear="all">
  
  <div id="layout_cleft_col">
    <div id="tabNav">
        <span style="background-image: url(./Images/en-us/category_titles.gif);">
            <div id="navTitle" title="Choose a tip" style="background-image: url(./images/en-us/category_titles.gif); background-position: 0pt -285px;">
                Choose a tip
            </div>
         </span>
            
         <span style="background-image: url(./Images/en-us/icon_add.gif);">
            <div class="searchTipsTab" id="DivTab1">
                <a href="javascript:void(0);" id="TabLink1" onmouseover="moveTitleSprite(1)" onmouseout="returnSprite()" onclick="ShowTab(1)" style="background-image: url(./images/en-us/icon_add.gif);"></a>
                
            </div>
        </span>
        
        <span style="background-image: url(./images/en-us/icon_subtract.gif);">
            <div class="searchTipsTab" id="DivTab2">
                <a href="javascript:void(0);" id="TabLink2" onmouseover="moveTitleSprite(2)" onmouseout="returnSprite()" onclick="ShowTab(2)" style="background-image: url(./images/en-us/icon_subtract.gif);"></a>
            </div>
        </span>
        
        <span style="background-image: url(./images/en-us/icon_or.gif);">
            <div class="searchTipsTab" id="DivTab3">
                <a href="javascript:void(0);" id="TabLink3" onmouseover="moveTitleSprite(3)" onmouseout="returnSprite()" onclick="ShowTab(3)" style="background-image: url(./images/en-us/icon_or.gif);"></a>
                
            </div>
        </span>
        
        <span style="background-image: url(./images/en-us/icon_asterisk.gif);">
            <div class="searchTipsTab" id="DivTab4">
                <a href="javascript:void(0);" id="TabLink4" onmouseover="moveTitleSprite(4)" onmouseout="returnSprite()" onclick="ShowTab(4)" style="background-image: url(./images/en-us/icon_asterisk.gif);"></a>
            </div>
        </span>
        
        <span style="background-image: url(./images/en-us/icon_quotes.gif);">
            <div class="searchTipsTab" id="DivTab5">
                <a href="javascript:void(0);" id="TabLink5" onmouseover="moveTitleSprite(5)" onmouseout="returnSprite()" onclick="ShowTab(5)" style="background-image: url(./images/en-us/icon_quotes.gif);"></a>
                
            </div>
        </span>
        
        </div>
        
        <div id="tabContent">
            <div id="DivContent0" class="searchTipsContent">
                <div>
                    <div id="tab0Content">
                        <img src="./images/en-us/looking_for_something.gif">
                    </div>
                </div>
                <div style="clear: both;"></div>
            </div>
            
            <div id="DivContent1" class="searchTipsContent" style="display: none;">
                <div>
                    <p class="tabDesc">
                          To get more precise results, use more specific terms or
                          add terms to your search. A plus sign or blank space
                          between search terms means AND. Each image will satisfy
                          all your criteria.</p>
                <div class="whiteCenterBox">
                    <span class="fauxSearchTitle">BROAD: </span>
                    <span class="fauxSearch">computer</span>
                    <br clear="all"><p></p>
                    <div class="imageHolders">
                        <img src="./images/42-18502692.jpg">
                        <br>42-18502692
                </div> <div class="imageHolders"><img src="./Images/42-18490527.jpg"><br>
              42-18490527
            </div><div class="imageHolders"><img src="./Images/42-16855798.jpg"><br>
              42-16855798
            </div><div class="imageHolders"><img src="./Images/42-18675520.jpg"><br>
              42-18675520
            </div><p></p><br clear="all"><br><span class="fauxSearchTitle">SPECIFIC:</span><span class="fauxSearch">laptop young woman</span><br clear="all"><p></p><div class="imageHolders"><img src="./Images/42-18751051.jpg"><br>
              42-18751051
            </div><div class="imageHolders"><img src="./Images/42-18627660.jpg"><br>
              42-18627660
            </div><div class="imageHolders"><img src="./Images/42-18155108.jpg"><br>
              42-18155108
            </div><div class="imageHolders"><img src="./Images/42-15476045.jpg"><br>
              42-15476045
            </div><p></p></div></div></div><div id="DivContent2" class="searchTipsContent" style="display: none;"><div><p class="tabDesc">
          Use a minus sign or the word NOT to eliminate unwanted images.
        </p><div class="whiteCenterBox"><span class="fauxSearchTitle">BROAD:</span><span class="fauxSearch">children</span><br clear="all"><p></p><div class="imageHolders"><img src="./Images/42-18502388.jpg"><br>
              42-18502388
            </div><div class="imageHolders"><img src="./Images/42-16623614.jpg"><br>
              42-16623614
            </div><div class="imageHolders"><img src="./Images/42-18838913.jpg"><br>
              42-18838913
            </div><div class="imageHolders"><img src="./Images/42-19352597.jpg"><br>
              42-19352597
            </div><p></p><br clear="all"><br><span class="fauxSearchTitle">SPECIFIC:</span><span class="fauxSearch">children -adults</span><br clear="all"><p></p><div class="imageHolders"><img src="./Images/42-18733923.jpg"><br>
              42-18733923
            </div><div class="imageHolders"><img src="./Images/42-18495693.jpg"><br>
              42-18495693
            </div><div class="imageHolders"><img src="./Images/42-19504029.jpg"><br>
              42-19504029
            </div><div class="imageHolders"><img src="./Images/42-17842279.jpg"><br>
              42-17842279
            </div><p></p></div></div><div style="clear: both;"></div></div><div id="DivContent3" class="searchTipsContent" style="display: none;"><div><p class="tabDesc">
          Use OR between search terms for broader or more varied results. Each image will satisfy at least one of your criteria.
        </p><div class="whiteCenterBox"><span class="fauxSearchTitle">INSTEAD OF:</span><span class="fauxSearch">monkey</span><br clear="all"><p></p><div class="imageHolders"><img src="./Images/42-18574886.jpg"><br>
              42-18574886
            </div><div class="imageHolders"><img src="./Images/42-18537476.jpg"><br>
              42-18537476
            </div><div class="imageHolders"><img src="./Images/U1296601INP.jpg"><br>
              U1296601INP
            </div><div class="imageHolders"><img src="./Images/42-17915583.jpg"><br>
              42-17915583
            </div><p></p><br clear="all"><br><span class="fauxSearchTitle">TRY THIS:</span><span class="fauxSearch">monkey or ape</span><br clear="all"><p></p><div class="imageHolders"><img src="./Images/42-15411646.jpg"><br>
              42-15411646
            </div><div class="imageHolders"><img src="./Images/42-15411404.jpg"><br>
              42-15411404
            </div><div class="imageHolders"><img src="./Images/42-18952347.jpg"><br>
              42-18952347
            </div><div class="imageHolders"><img src="./Images/42-17424543.jpg"><br>
              42-17424543
            </div><p></p></div></div><div style="clear: both;"></div></div><div id="DivContent4" class="searchTipsContent" style="display: none;"><div><p class="tabDesc">
          Use an asterisk * to find variations of word endings; e.g.
          roller* matches rollerblading, roller-skating, etc. Or to
          broaden a search by date; e.g. 198*
        </p><div class="whiteCenterBox"><span class="fauxSearchTitle">INSTEAD OF:</span><span class="fauxSearch">rollerblading</span><br clear="all"><p></p><div class="imageHolders"><img src="./Images/42-15534239.jpg"><br>
              42-15534239
            </div><div class="imageHolders"><img src="./Images/42-15136287.jpg"><br>
              42-15136287
            </div><div class="imageHolders"><img src="./Images/SKT-02SC002-001.jpg"><br>
              SKT-02SC002-001
            </div><div class="imageHolders"><img src="./Images/42-18715762.jpg"><br>
              42-18715762
            </div><p></p><br clear="all"><br><span class="fauxSearchTitle">TRY THIS:</span><span class="fauxSearch">roller*</span><br clear="all"><p></p><div class="imageHolders"><img src="./Images/42-16109816.jpg"><br>
              42-16109816
            </div><div class="imageHolders"><img src="./Images/42-17751190.jpg"><br>
              42-17751190
            </div><div class="imageHolders"><img src="./Images/42-17536417.jpg"><br>
              42-17536417
            </div><div class="imageHolders"><img src="./Images/42-16271269.jpg"><br>
              42-16271269
            </div><p></p></div></div><div style="clear: both;"></div></div><div id="DivContent5" class="searchTipsContent" style="display: none;"><div><p class="tabDesc">
          Want fewer results? Use "quotes" to specify an exact match
          on a phrase, name, etc.; e.g. "George Burns" or "Rebel Without a Cause"
        </p><div class="whiteCenterBox"><span class="fauxSearchTitle">INSTEAD OF:</span><span class="fauxSearch">George Burns</span><br clear="all"><p></p><div class="imageHolders"><img src="./Images/42-16489881.jpg"><br>
              42-16489881
            </div><div class="imageHolders"><img src="./Images/42-18023293.jpg"><br>
              42-18023293
            </div><div class="imageHolders"><img src="./Images/42-15359678.jpg"><br>
              42-15359678
            </div><div class="imageHolders"><img src="./Images/JS1568078.jpg"><br>
              JS1568078
            </div><p></p><br clear="all"><br><span class="fauxSearchTitle">TRY THIS:</span><span class="fauxSearch">"George Burns"</span><br clear="all"><p></p><div class="imageHolders"><img src="./Images/OZ001232.jpg"><br>
              OZ001232
            </div><div class="imageHolders"><img src="./Images/U530414ACME.jpg"><br>
              U530414ACME
            </div><div class="imageHolders"><img src="./Images/BE024222.jpg"><br>
              BE024222
            </div><div class="imageHolders"><img src="./Images/ZCV88193.jpg"><br>
              ZCV88193
            </div><p></p></div></div><div style="clear: both;"></div></div></div></div>
            <div id="layout_right_col"><img src="./Images/en-US/keyword_natureandwildlife.jpg" alt="Search Tips" border="0" width="269" height="56"><dl class="accordion-menu" id="accordion-menu"><dt class="a-m-t" id="a-m-t-1"><b>People</b></dt><dd class="a-m-d"><div class="bd"><a href="javascript:void(0)" onclick="reloadparentCloseChild('q','African%20Americans'); return false;">African Americans</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Hispanic'); return false;">Hispanic</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Middle%20Easterners'); return false;">Middle Easterners</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Asians'); return false;">Asians</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Couples'); return false;">Couples</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Homecoming%20queen'); return false;">Homecoming queen</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Real%20people'); return false;">Real people</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Tween'); return false;">Tween</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Teenager'); return false;">Teenager</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Wealthy%20people'); return false;">Wealthy people</a></div></dd><dt class="a-m-t" id="a-m-t-2"><b>Time and Place</b></dt><dd class="a-m-d"><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Street%20scenes'); return false;">Street scenes</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Summer'); return false;">Summer</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Classroom'); return false;">Classroom</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Coffee%20shop'); return false;">Coffee shop</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Home%20office'); return false;">Home office</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Suburb'); return false;">Suburb</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Barbeque'); return false;">Barbeque</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Happy%20hour'); return false;">Happy hour</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Weekend'); return false;">Weekend</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Indoors'); return false;">Indoors</a></div></dd><dt class="a-m-t" id="a-m-t-3"><b>Compositional</b></dt><dd class="a-m-d"><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Lens%20flare'); return false;">Lens flare</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Half-length'); return false;">Half-length</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Low%20section'); return false;">Low section</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','View%20from%20above'); return false;">View from above</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Red'); return false;">Red</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Bright%20colors'); return false;">Bright colors</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Background'); return false;">Background</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Copy%20space'); return false;">Copy space</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Photographic%20studies'); return false;">Photographic studies</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Single%20object'); return false;">Single object</a></div></dd><dt class="a-m-t" id="a-m-t-4"><b>Conceptual</b></dt><dd class="a-m-d"><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Away%20from%20it%20all'); return false;">Away from it all</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Crime%20scene%20'); return false;">Crime scene </a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Drama%20queen'); return false;">Drama queen</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Erotic'); return false;">Erotic</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Humor'); return false;">Humor</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Night%20life'); return false;">Night life</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Retro'); return false;">Retro</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Role%20reversal'); return false;">Role reversal</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Trendy'); return false;">Trendy</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Unexpected'); return false;">Unexpected</a></div></dd><dt class="a-m-t" id="a-m-t-5"><b>Lifestyle</b></dt><dd class="a-m-d"><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Human%20relationships'); return false;">Human relationships</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Ecotourism'); return false;">Ecotourism</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','LOHAS'); return false;">LOHAS</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Cuddling'); return false;">Cuddling</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Girls%E2%80%99%20night%20out'); return false;">Girls’ night out</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Multi-tasking'); return false;">Multi-tasking</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Playing%20dress%20up'); return false;">Playing dress up</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Real%20people'); return false;">Real people</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Texting'); return false;">Texting</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Weekend'); return false;">Weekend</a></div></dd><dt class="a-m-t" id="a-m-t-6"><b>Business and Finance</b></dt><dd class="a-m-d"><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Connectivity'); return false;">Connectivity</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','On%20the%20go'); return false;">On the go</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Overworked'); return false;">Overworked</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Conference%20room'); return false;">Conference room</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Customer%20service%20representative'); return false;">Customer service representative</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Finance'); return false;">Finance</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Homeshoring'); return false;">Homeshoring</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Job%20seeker'); return false;">Job seeker</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Office%20romance'); return false;">Office romance</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Podcasting'); return false;">Podcasting</a></div></dd><dt class="a-m-t" id="a-m-t-7"><b>Health and Beauty</b></dt><dd class="a-m-d"><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Botox'); return false;">Botox</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Sonogram'); return false;">Sonogram</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Alternative%20medicine'); return false;">Alternative medicine</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Facial'); return false;">Facial</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Fitness'); return false;">Fitness</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Grooming'); return false;">Grooming</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Hairstyle'); return false;">Hairstyle</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Body%20image'); return false;">Body image</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Spa'); return false;">Spa</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Windblown'); return false;">Windblown</a></div></dd><dt class="a-m-t" id="a-m-t-8"><b>Sports</b></dt><dd class="a-m-d"><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Extreme%20sports'); return false;">Extreme sports</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Agility'); return false;">Agility</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Dedication'); return false;">Dedication</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Discipline'); return false;">Discipline</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Number%20one'); return false;">Number one</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Gymnastics'); return false;">Gymnastics</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Bench%20press'); return false;">Bench press</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Cheering'); return false;">Cheering</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Finish%20line'); return false;">Finish line</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Snowboarding'); return false;">Snowboarding</a></div></dd><dt class="a-m-t" id="a-m-t-9"><b>Travel</b></dt><dd class="a-m-d"><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Adventure'); return false;">Adventure</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Mancation'); return false;">Mancation</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Boutique%20hotel'); return false;">Boutique hotel</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Ecotourism'); return false;">Ecotourism</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Hotel%20room'); return false;">Hotel room</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Landmark'); return false;">Landmark</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Room%20service'); return false;">Room service</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Sunbathing'); return false;">Sunbathing</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Barcelona'); return false;">Barcelona</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','India'); return false;">India</a></div></dd><dt class="a-m-t" id="a-m-t-10"><b>Nature and Wildlife</b></dt><dd class="a-m-d"><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Outdoors'); return false;">Outdoors</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Environmentalism'); return false;">Environmentalism</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Lush'); return false;">Lush</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Beluga%20whale'); return false;">Beluga whale</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Dahlia'); return false;">Dahlia</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Eroding'); return false;">Eroding</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Global%20warming'); return false;">Global warming</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','One%20animal'); return false;">One animal</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Predator'); return false;">Predator</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Sunset'); return false;">Sunset</a></div></dd><dt class="a-m-t" id="a-m-t-11"><b>Home and Architecture</b></dt><dd class="a-m-d"><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Interior%20design'); return false;">Interior design</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Mid-century%20modern'); return false;">Mid-century modern</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Shabby%20chic'); return false;">Shabby chic</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Chandelier'); return false;">Chandelier</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Club%20chair'); return false;">Club chair</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Converted%20industrial%20building'); return false;">Converted industrial building</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Kitchen'); return false;">Kitchen</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Mounted%20animal'); return false;">Mounted animal</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Recessed%20lighting'); return false;">Recessed lighting</a></div><div class="bd"><a href="#" onclick="reloadparentCloseChild('q','Zebra%20skin%20pattern'); return false;">Zebra skin pattern</a></div></dd></dl></div><a class="extraLink pdf" href="/creative/searchtips/content/en-US/KeywordGuide_EN.pdf" title="DOWNLOAD KEYWORD GUIDE (PDF)">DOWNLOAD KEYWORD GUIDE (PDF)</a><a class="pdf" href="" title=""></a>
            
      </a></div>
      </div>
        </div><br clear="all">
<script language="javascript" type="text/javascript">

    var AccordionMenu = (function() {
        var YUD = YAHOO.util.Dom;
        var YUE = YAHOO.util.Event;
        var oMenuSetting = {};
        var oMenuCache = {};
        var dLastHoverTitle;
        YUD.addClass(document.documentElement, 'accordion-menu-js');

        function getDT(e) {
            var dEl = YUE.getTarget(e);

            if ((e.tagName + '').toUpperCase() == 'DD') {
                var dt = e.previousSibling;
                while (dt) {
                    if (dt.tagName && dt.tagName.toUpperCase() == 'DT') { break; };
                    dt = dt.previousSibling
                };

                if (!dt || dt.tagName.toUpperCase() != 'DT') { return; }
                else { return (dt.tagName === 'DT') ? dt : null };
            }
            else if (e.clientX) {
                var found = false;
                while (dEl.parentNode) {
                    if (YUD.hasClass(dEl, 'a-m-t')) { found = true; break; };
                    dEl = dEl.parentNode;
                };
                if (!found) { return null }
                else { return (dEl.tagName === 'DT') ? dEl : null };
            };
        };



        function getDD(dt) {
            if (!dt) { return null; };
            var dd = dt.nextSibling;

            while (dd) {
                if (dd.tagName && dd.tagName.toUpperCase() == 'DD') { break; };
                dd = dd.nextSibling;

            };
            if (!dd || dd.tagName.toUpperCase() != 'DD') { return; }
            else { return dd };

        };

        function expand(dl, dt, dd) {

            dl.hasAnimation += 1;
            YUD.addClass(dd, 'a-m-d-before-expand');
            var oAttr = { height: { from: 0, to: dd.offsetHeight} };

            YUD.removeClass(dd, 'a-m-d-before-expand');

            var onComplete = function() {
                oAnim.onComplete.unsubscribe(onComplete);
                oAnim.stop();
                YUD.removeClass(dd, 'a-m-d-anim');
                YUD.addClass(dd, 'a-m-d-expand');
                onComplete = null;
                dl.hasAnimation -= 1;
                var dt = getDT(dd);
                YUD.addClass(dt, 'a-m-t-expand');
                if (oMenuCache[dl.id] && oMenuCache[dl.id].onOpen && dd.style.height != '') {
                    oMenuCache[dl.id].onOpen({ dl: dl, dt: dt, dd: dd });
                };
                dd.style.height = '';

            };

            var onTween = function() {
                if (dd.style.height) {
                    YUD.addClass(dd, 'a-m-d-anim');
                    oAnim.onTween.unsubscribe(onTween);
                    onTween = null;
                    dd.oAnim = null;
                };

            };

            if (dd.oAnim) {
                dd.oAnim.stop();
                dd.oAnim = null;
                dl.hasAnimation -= 1;
            };
            var oEaseType = YAHOO.util.Easing.easeOut;
            var seconds = 0.5;
            if (oMenuCache[dl.id]) {
                oEaseType = oMenuCache[dl.id]['easeOut'] ? oEaseType : YAHOO.util.Easing.easeIn;
                seconds = oMenuCache[dl.id]['seconds'];

                if (!oMenuCache[dl.id]['animation']) {
                    var oAnim = { onComplete: { unsubscribe: function() { } }, stop: function() { } };
                    onComplete();
                    return;
                };
            };


            var oAnim = new YAHOO.util.Anim(dd, oAttr, seconds, oEaseType);
            oAnim.onComplete.subscribe(onComplete);
            oAnim.onTween.subscribe(onTween);
            oAnim.animate();
            dd.oAnim = oAnim;

        };

        function collapse(dl, dt, dd) {
            dl.hasAnimation += 1;
            YUD.addClass(dd, 'a-m-d-anim');
            var oAttr = { height: { from: dd.offsetHeight, to: 0} };


            var onComplete = function() {
                oAnim.onComplete.unsubscribe(onComplete);
                YUD.removeClass(dd, 'a-m-d-anim');
                YUD.removeClass(dd, 'a-m-d-expand');
                dd.style.height = '';
                dd.oAnim = null;
                onComplete = null;
                dl.hasAnimation -= 1;
                var dt = getDT(dd);
                YUD.removeClass(dt, 'a-m-t-expand');
                if (oMenuCache[dl.id] && oMenuCache[dl.id].onOpen) {
                    oMenuCache[dl.id].onClose({ dl: dl, dt: dt, dd: dd });
                };

            };

            if (dd.oAnim) {
                dd.oAnim.stop();
                dd.oAnim = null;
                dl.hasAnimation -= 1;
            };

            var oEaseType = YAHOO.util.Easing.easeOut;
            var seconds = 0.5;
            if (oMenuCache[dl.id]) {
                oEaseType = oMenuCache[dl.id]['easeOut'] ? oEaseType : YAHOO.util.Easing.easeIn;
                seconds = oMenuCache[dl.id]['seconds'];
                if (!oMenuCache[dl.id]['animation']) {
                    var oAnim = { onComplete: { unsubscribe: function() { } }, stop: function() { } };
                    onComplete();
                    return;
                };
            };

            var oAnim = new YAHOO.util.Anim(dd, oAttr, seconds, oEaseType);
            oAnim.onComplete.subscribe(onComplete);
            oAnim.animate();
            dd.oAnim = oAnim;
        };

        function collapseAll(dl, dt, dd) {
            var aOtherDD = YUD.getElementsByClassName('a-m-d-expand', 'dd', dl);
            for (var i = 0; i < aOtherDD.length; i++) {
                var otherDD = aOtherDD[i];
                if (otherDD != dd) {
                    collapse(dl, null, otherDD);
                };
            };
        }


        var onMenuMouseover = function(e) {

            var dMenuTitle = getDT(e);
            if (!dMenuTitle) { return; };
            if (dLastHoverTitle) {
                YUD.removeClass(dLastHoverTitle, 'a-m-t-hover');
            };
            //Added by Eli Huntington 3-27-2008
            swapKeywordImage(getDT(e).id);

            //END
            YUD.addClass(dMenuTitle, 'a-m-t-hover');
            dLastHoverTitle = dMenuTitle;
            YUE.preventDefault(e);

            return false;
        };

        var onMenuMouseout = function(e) {
            var dMenuTitle = getDT(e);
            if (!dMenuTitle) { return; };
            if (dLastHoverTitle && dLastHoverTitle != dMenuTitle) {
                YUD.removeClass(dLastHoverTitle, 'a-m-t-hover');
                YUD.removeClass(dLastHoverTitle, 'a-m-t-down');
            };
            YUD.removeClass(dMenuTitle, 'a-m-t-down');
            YUD.removeClass(dMenuTitle, 'a-m-t-hover');
            dLastHoverTitle = null;
            YUE.preventDefault(e);
            return false;
        };

        var onMenuMousedown = function(e) {
            var dMenuTitle = getDT(e);
            if (!dMenuTitle) { return; };
            YUD.addClass(dMenuTitle, 'a-m-t-down');
            YUE.preventDefault(e);
            return false;
        };

        var onMenuClick = function(e) {
            var dt = getDT(e);
            if (!dt) { return; };
            var dd = getDD(dt);



            if (!dd) { return; };
            var dl = dt.parentNode;

            if (dl.hasAnimation == null) {
                dl.hasAnimation = 0;
            }
            if (dl.hasAnimation > 0) { return; };
            YUD.removeClass(dt, 'a-m-t-down');

            if (YUD.hasClass(dd, 'a-m-d-expand')) {
                collapse(dl, dt, dd);
            }
            else {
                if (oMenuCache[dl.id] && oMenuCache[dl.id].dependent == false) { }
                else { collapseAll(dl, dt, dd); }
                expand(dl, dt, dd);
            };
            YUE.preventDefault(e);
            return false;
        };


        YUE.on(document, 'mouseover', onMenuMouseover);
        YUE.on(document, 'mouseout', onMenuMouseout);
        YUE.on(document, 'mousedown', onMenuMousedown);
        YUE.on(document, 'click', onMenuClick);

        var oApi = {

            openDtById: function(sId) {

                var dt = document.getElementById(sId);
                if (!dt) { return; };
                if (!YUD.hasClass(dt, 'a-m-t')) { return; };
                var dl = dt.parentNode;
                var dd = getDD(dt);
                if (dl.hasAnimation == null) { dl.hasAnimation = 0; };

                if (dl.hasAnimation > 0) { return; };
                if (YUD.hasClass(dd, 'a-m-d-expand')) { return; };
                if (oMenuCache[dl.id] && oMenuCache[dl.id].dependent == false) { }
                else { collapseAll(dl, dt, dd); }
                expand(dl, dt, dd);
            },

            closeDtById: function(sId) {
                var dt = document.getElementById(sId);
                if (!dt) { return; };
                if (!YUD.hasClass(dt, 'a-m-t')) { return; };
                var dl = dt.parentNode;
                var dd = getDD(dt);
                if (dl.hasAnimation == null) { dl.hasAnimation = 0; };
                if (dl.hasAnimation > 0) { return; };
                if (!YUD.hasClass(dd, 'a-m-d-expand')) { return; };
                collapse(dl, dt, dd);
            },


            setting: function(id, oOptions) {
                if (!oOptions) { return; };

                if (typeof (id) != 'string') { return; };

                var setMunu = function(dl) {
                    dl = dl || this;
                    dl.hasAnimation = 0;
                    oMenuCache[dl.id] =
			{
			    element: dl,
			    dependent: false,
			    onOpen: function() { },
			    onClose: function() { },
			    seconds: 0.9,
			    easeOut: true,
			    openedIds: ['a-m-t-1'],
			    animation: true
			};

                    oMenu = oMenuCache[dl.id];

                    if (typeof (oOptions['animation']) == 'boolean') {
                        oMenu['animation'] = !!oOptions['animation'];

                    };


                    if (typeof (oOptions['dependent']) == 'boolean') {
                        oMenu['dependent'] = !!oOptions['dependent'];
                    };

                    if (typeof (oOptions['easeOut']) == 'boolean') {
                        oMenu['easeOut'] = !!oOptions['easeOut'];
                    };

                    if (typeof (oOptions['seconds']) == 'number') {
                        oMenu['seconds'] = Math.max(0, oOptions['seconds']);
                    };

                    if (typeof (oOptions['onOpen']) == 'function') {
                        oMenu['onOpen'] = oOptions['onOpen'];
                    };

                    if (typeof (oOptions['onClose']) == 'function') {
                        oMenu['onClose'] = oOptions['onClose'];
                    };

                    if (oOptions['openedIds'].shift) {
                        oMenu['openedIds'] = oOptions['openedIds'];
                    };


                    for (var i = 0; i < oMenu['openedIds'].length; i++) {
                        var sId = oMenu['openedIds'][i];
                        var dt = document.getElementById(sId);

                        if (dt && dt.tagName.toUpperCase() == 'DT') {
                            var dl = dt.parentNode;
                            var dd = getDD(dt);
                            expand(dl, dt, dd);
                        }
                        else if (!dt) {
                            function onDtAvailable() {
                                var dt = this;
                                if (dt.tagName.toUpperCase() == 'DT') {
                                    var dl = dt.parentNode;
                                    var dd = getDD(dt);
                                    expand(dl, dt, dd);
                                };
                            };

                            YUE.onAvailable(sId, onDtAvailable);
                        }
                    };


                };

                if (document.getElementById(id)) {
                    setMunu(document.getElementById(id))
                }
                else {
                    YUE.onAvailable(id, setMunu);
                };
            }
        }; //endof api

        return oApi;

    })();



</script>
</body>
</Html>
