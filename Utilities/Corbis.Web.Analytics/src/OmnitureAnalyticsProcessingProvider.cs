using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Web.UI;
using Corbis.Web.Utilities;
using Corbis.WebAnalytics.Contracts.V1;
using Corbis.Web.Entities;
using System.Collections;

namespace Corbis.Web.Analytics
{

    /// <summary>
    /// Summary description for OmnitureAnalyticsProcessingProvider
    /// </summary>
    public class OmnitureAnalyticsProcessingProvider : AnalyticsProcessingProvider
    {
        /// <summary>
        /// OmnitureAnalyticsProcessingPrivider constructor
        /// </summary>
        public OmnitureAnalyticsProcessingProvider()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        ///<summary>
        ///Checks the page to see if it contains a ScriptManger control.
        ///</summary>
        private ScriptManager GetScriptManager(Page page)
        {
            ScriptManager scriptManager = null;
            
            foreach(DictionaryEntry dictionaryEntry in page.Items)
            {
                if(dictionaryEntry.Key.ToString().IndexOf("System.Web.UI.ScriptManager") >=0)
                {
                    scriptManager = (ScriptManager)dictionaryEntry.Value;
                }
            }
            
            return scriptManager;
        }

        /// <summary>
        /// Process Analytics
        /// </summary>
        public override void ProcessAnalytics(EventType eventType, Dictionary<string, string> eventData)
        {
            if (System.Web.HttpContext.Current == null ||
                System.Web.HttpContext.Current.Handler == null)
            {
                return;
            }

            Page page = (Page)System.Web.HttpContext.Current.Handler;

            StringBuilder scriptBlock = new StringBuilder();
            
            scriptBlock.Append("<!-- SiteCatalyst code version: H.19.4." + Environment.NewLine);
            scriptBlock.Append("Copyright 1997-2009 Omniture, Inc. More info available at" + Environment.NewLine);
            scriptBlock.Append("http://www.omniture.com -->" + Environment.NewLine);
            scriptBlock.Append("<Script type=\"text/javascript\" language=\"javascript\" src=\"" + page.ResolveUrl("~/Scripts/SiteCatalyst.js") + "\"></Script>" + Environment.NewLine);
            scriptBlock.Append("<script language=\"JavaScript\"><!--" + Environment.NewLine);
            scriptBlock.Append("/* You may give each page an identifying name, server, and channel on" + Environment.NewLine);
            scriptBlock.Append("the next lines. */" + Environment.NewLine);
            scriptBlock.Append("s.pageName=\"" + StringHelper.GetEmptyIfNull(eventData, "pageName") + "\"" + Environment.NewLine);
            scriptBlock.Append("s.server=\"" + StringHelper.GetEmptyIfNull(eventData, "server") + "\"" + Environment.NewLine);
            scriptBlock.Append("s.channel=\"" + StringHelper.GetEmptyIfNull(eventData, "channel") + "\"" + Environment.NewLine);
            scriptBlock.Append("s.pageType=\"" + StringHelper.GetEmptyIfNull(eventData, "pageType") + "\"" + Environment.NewLine);
            scriptBlock.Append("s.prop1=\"" + StringHelper.GetEmptyIfNull(eventData, "prop1") + "\"" + Environment.NewLine);
            scriptBlock.Append("s.prop2=\"" + StringHelper.GetEmptyIfNull(eventData, "prop2") + "\"" + Environment.NewLine);
            scriptBlock.Append("s.prop3=\"" + StringHelper.GetEmptyIfNull(eventData, "prop3") + "\"" + Environment.NewLine);
            scriptBlock.Append("s.prop4=\"" + StringHelper.GetEmptyIfNull(eventData, "prop4") + "\"" + Environment.NewLine);
            scriptBlock.Append("s.prop5=\"" + StringHelper.GetEmptyIfNull(eventData, "prop5") + "\"" + Environment.NewLine);
            scriptBlock.Append("s.prop6=\"" + StringHelper.GetEmptyIfNull(eventData, "prop6") + "\"" + Environment.NewLine);
            scriptBlock.Append("s.prop7=\"" + StringHelper.GetEmptyIfNull(eventData, "prop7") + "\"" + Environment.NewLine);
            scriptBlock.Append("s.prop8=\"" + StringHelper.GetEmptyIfNull(eventData, "prop8") + "\"" + Environment.NewLine);
            scriptBlock.Append("s.prop9=\"" + StringHelper.GetEmptyIfNull(eventData, "prop9") + "\"" + Environment.NewLine);
            scriptBlock.Append("s.prop10=\"" + StringHelper.GetEmptyIfNull(eventData, "prop10") + "\"" + Environment.NewLine);
            scriptBlock.Append("s.prop11=\"" + StringHelper.GetEmptyIfNull(eventData, "prop11") + "\"" + Environment.NewLine);
            scriptBlock.Append("s.prop12=\"" + StringHelper.GetEmptyIfNull(eventData, "prop12") + "\"" + Environment.NewLine);
            scriptBlock.Append("s.prop13=\"" + StringHelper.GetEmptyIfNull(eventData, "prop13") + "\"" + Environment.NewLine);
            scriptBlock.Append("s.prop14=\"" + StringHelper.GetEmptyIfNull(eventData, "prop14") + "\"" + Environment.NewLine);
            scriptBlock.Append("/* Conversion Variables */" + Environment.NewLine);
            scriptBlock.Append("s.campaign=\"" + StringHelper.GetEmptyIfNull(eventData, "campaign") + "\"" + Environment.NewLine);
            scriptBlock.Append("s.state=\"" + StringHelper.GetEmptyIfNull(eventData, "state") + "\"" + Environment.NewLine);
            scriptBlock.Append("s.zip=\"" + StringHelper.GetEmptyIfNull(eventData, "zip") + "\"" + Environment.NewLine);
            scriptBlock.Append("s.events=\"" + StringHelper.GetEmptyIfNull(eventData, "events") + "\"" + Environment.NewLine);
            scriptBlock.Append("s.products=\"" + StringHelper.GetEmptyIfNull(eventData, "products") + "\"" + Environment.NewLine);
            scriptBlock.Append("s.purchaseID=\"" + StringHelper.GetEmptyIfNull(eventData, "purchaseID") + "\"" + Environment.NewLine);
            //scriptBlock.Append("s.eVar1=s.pageName" + Environment.NewLine);
            //scriptBlock.Append("s.eVar2=s.prop2" + Environment.NewLine);
            //scriptBlock.Append("s.eVar3=s.prop3" + Environment.NewLine);
            //scriptBlock.Append("s.eVar4=s.prop4" + Environment.NewLine);
            scriptBlock.Append("s.eVar5=\"" + StringHelper.GetEmptyIfNull(eventData, "eVar5") + "\"" + Environment.NewLine);
            scriptBlock.Append("s.eVar6=\"" + StringHelper.GetEmptyIfNull(eventData, "eVar6") + "\"" + Environment.NewLine);
            //scriptBlock.Append("s.eVar7=s.prop7" + Environment.NewLine);
            //scriptBlock.Append("s.eVar8=s.prop8" + Environment.NewLine);
            //scriptBlock.Append("s.eVar9=s.prop9" + Environment.NewLine);
            //scriptBlock.Append("s.eVar10=s.prop10" + Environment.NewLine);
            //scriptBlock.Append("s.eVar11=s.prop11" + Environment.NewLine);
            //scriptBlock.Append("s.eVar12=s.prop12" + Environment.NewLine);
            //scriptBlock.Append("s.eVar13=s.prop13" + Environment.NewLine);
            scriptBlock.Append("s.eVar14=\"" + StringHelper.GetEmptyIfNull(eventData, "eVar14") + "\"" + Environment.NewLine);
            scriptBlock.Append("/************* DO NOT ALTER ANYTHING BELOW THIS LINE ! **************/" + Environment.NewLine);
            scriptBlock.Append("var s_code=s.t();if(s_code)document.write(s_code)//--></script>" + Environment.NewLine);
            scriptBlock.Append("<!-- End SiteCatalyst code version: H.19.4. -->  ");


            //ScriptManager scriptManager = null;

            //scriptManager = GetScriptManager(page);

            //if (scriptManager == null)
            //{
                page.ClientScript.RegisterStartupScript(page.GetType(), "SiteCatalyst", scriptBlock.ToString(), false);
            //}
            //else
            //{
            //    scriptManager.regi
            //}
        }
    }

}