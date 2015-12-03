using System;
using System.Xml;
using System.Collections;
using System.Diagnostics;
using System.IO;

namespace Corbis.Web.UI.Presenters.Tools
{
    /// <summary>
    /// Nice interface for xml operations.
    /// </summary>
    public class XMLService
    {
        //DOM way	
#if (DEBUG)
        private FileStream myDebugLog;
        private TextWriterTraceListener myListener;
#endif

        //Default constructor
        public XMLService() { }

        public XMLService(string sLog)
        {
#if (DEBUG)
            if (sLog != null)
            {
                myDebugLog = new FileStream(sLog, FileMode.Append);
                //Creates the new trace listener
                myListener = new TextWriterTraceListener(myDebugLog);
                Debug.Listeners.Add(myListener);
                //Debug.Listeners.Add(New TextWriterTraceListener(Console.Out));
                Debug.AutoFlush = true;
                Debug.Indent();
            }
#endif
        }

        //nParent/sPrefix/sNamespaceURI are optional
        public XmlElement CreateElement(XmlDocument xd, XmlNode nParent, string sLocalName, string sPrefix, string sNamespaceURI)
        {
            XmlElement el = null;

            try
            {
                if (sNamespaceURI != null && sNamespaceURI.Equals(string.Empty))
                    sNamespaceURI = null;

                if (sPrefix != null && sPrefix.Equals(string.Empty))
                    sPrefix = null;

                el = xd.CreateElement(sPrefix, sLocalName, sNamespaceURI);
                if (nParent != null)
                    nParent.AppendChild(el);
            }
            catch (Exception e)
            {
#if (DEBUG)
                if (myListener != null)
                    Debug.Write(e.Message, e.Source);
#endif
                //Default Exception Handling behavior
                throw e;
            }

            return el;
        }

        public void AppendChild(XmlNode nParent, XmlNode nChild)
        {
            try
            {
                nParent.AppendChild(nChild);
            }
            catch (Exception e)
            {
#if (DEBUG)
                if (myListener != null)
                    Debug.Write(e.Message, e.Source);
#endif
                //Default Exception Handling behavior
                throw e;
            }
        }

        public XmlAttribute AppendAttribute(XmlElement el, XmlAttribute at)
        {
            XmlAttribute atReturn = null;

            try
            {
                if (at == null)
                    throw new Exception("No attribute defined");


                if (at != null)
                {
                    el.SetAttributeNode(at); //add a new attribute
                    atReturn = at;
                }
            }
            catch (Exception e)
            {
#if (DEBUG)
                if (myListener != null)
                    Debug.Write(e.Message, e.Source);
#endif
                //Default Exception Handling behavior
                throw e;
            }

            return atReturn;
        }

        public XmlAttribute CreateAttribute(XmlElement el, string sName, string sValue, string sNamespaceURI)
        {
            XmlAttribute atReturn = null;

            try
            {
                if (sName == null)
                    throw new Exception("No attribute defined");

                if (string.IsNullOrEmpty(sNamespaceURI))
                    sNamespaceURI = null;

                if (el.HasAttribute(sName, sNamespaceURI))
                    throw new Exception("Attribute already exists. Attribute name:" + sName);

                atReturn = el.SetAttributeNode(sName, sNamespaceURI);
                atReturn.Value = sValue;
                //el.SetAttribute(sName.ToUpper(), sNamespaceURI, sValue);
            }
            catch (Exception e)
            {
#if (DEBUG)
                if (myListener != null)
                    Debug.Write(e.Message, e.Source);
#endif
                //Default Exception Handling behavior
                throw e;
            }

            return atReturn;
        }

        public XmlAttribute UpdateAttributeValue(XmlNode xn, string sName, string sValue, string sNamespaceURI)
        {
            XmlAttribute atReturn = null;
            XmlAttributeCollection atc = null;

            if (sName == null)
                throw new Exception("No attribute defined");

            if (sNamespaceURI != null && sNamespaceURI.Equals(string.Empty))
                sNamespaceURI = null;

            atc = xn.Attributes;
            if (sNamespaceURI != null) atReturn = atc[sName, sNamespaceURI];
            else atReturn = atc[sName];
            if (atReturn != null)
                atReturn.Value = sValue;
            else
                throw new Exception("Attribute does not exist. Attribute name: " + sName);

            return atReturn;
        }

        //XmlNode is optional
        public XmlText CreateText(XmlDocument xd, XmlNode nParent, string sText)
        {
            XmlText t = null;

            //if (m_objErr != null && m_objErr.isDebugMode()) Monitor.in();
            try
            {
                t = xd.CreateTextNode(sText);

                if (nParent != null)
                    nParent.AppendChild(t);
            }
            catch (Exception e)
            {
#if (DEBUG)
                if (myListener != null)
                    Debug.Write(e.Message, e.Source);
#endif
                //Default Exception Handling behavior
                throw e;
            }

            return t;
        }

        public XmlNode GetRootNode(XmlDocument xd)
        {
            return xd;
        }

        public XmlNode GetNode(XmlNode nParent, string sAttr, string sValue, string sNamespaceURI_attr, bool bRequired)
        {
            XmlNodeList nl = null;
            XmlNode nTemp = null, nReturn = null;
            ArrayList alNodes = new ArrayList();
            int iNumNodeList = 0;

            try
            {
                if (nParent != null && sAttr != null && sValue != null)
                {
                    if (sNamespaceURI_attr != null && sNamespaceURI_attr.Equals(string.Empty))
                        sNamespaceURI_attr = null;

                    nl = nParent.ChildNodes;
                    if (nl != null)
                    {
                        iNumNodeList = nl.Count;
                        for (int i = 0; i < iNumNodeList; i++)
                        {
                            nTemp = nl[i];
                            if (nTemp.NodeType != XmlNodeType.Element) continue;
                            if (!SearchAttrNode(nTemp, sAttr, sValue, sNamespaceURI_attr)) continue;
                            alNodes.Add(nTemp);
                        }
                    }
                }

                if (bRequired && (alNodes.Count == 0))
                    throw new Exception("Node is not found, given attribute: " + sAttr + " and value: " + sValue);

                if (alNodes.Count > 1)
                    throw new Exception("More than one node were found, given attribute: " + sAttr + " and value: " + sValue);

                if (alNodes.Count == 1)
                    nReturn = (XmlNode)alNodes[0];
                else if (alNodes.Count == 0)
                    nReturn = null;
            }
            catch (Exception e)
            {
#if (DEBUG)
                if (myListener != null)
                    Debug.Write(e.Message, e.Source);
#endif
                //Default Exception Handling behavior
                throw e;
            }

            return nReturn;
        }

        public string GetTextNode(XmlNode nParent, bool bRequired)
        {
            string sText = null;
            XmlNodeList nl = null;
            XmlNode nTemp = null;
            int iNumNodeList = 0;

            try
            {
                if (nParent != null)
                {
                    nl = nParent.ChildNodes;
                    if (nl != null)
                    {
                        iNumNodeList = nl.Count;
                        for (int i = 0; i < iNumNodeList; i++)
                        {
                            nTemp = nl[i];
                            if (nTemp.NodeType != XmlNodeType.Text) continue;
                            sText = nTemp.Value;
                            break;
                        }
                    }
                }

                if (bRequired && sText == null)
                    throw new Exception("Node is not found");
            }
            catch (Exception e)
            {
#if (DEBUG)
                if (myListener != null)
                    Debug.Write(e.Message, e.Source);
#endif
                //Default Exception Handling behavior
                throw e;
            }

            return sText;
        }

        public XmlNode GetNode(XmlNode nParent, string sAttr, string sNamespaceURI_attr, bool bRequired)
        {
            XmlNodeList nl = null;
            XmlNode nTemp = null, nReturn = null;
            ArrayList alNodes = new ArrayList();
            int iNumNodeList = 0;

            try
            {
                if (nParent != null && sAttr != null)
                {
                    if (sNamespaceURI_attr != null && sNamespaceURI_attr.Equals(string.Empty))
                        sNamespaceURI_attr = null;

                    nl = nParent.ChildNodes;
                    if (nl != null)
                    {
                        iNumNodeList = nl.Count;
                        for (int i = 0; i < iNumNodeList; i++)
                        {
                            nTemp = nl[i];
                            if (nTemp.NodeType != XmlNodeType.Element) continue;
                            if (!SearchAttrNode(nTemp, sAttr, sNamespaceURI_attr)) continue;

                            alNodes.Add(nTemp);
                        }
                    }
                }

                if (bRequired && (alNodes.Count == 0))
                    throw new Exception("Node is not found, given attribute: " + sAttr);

                if (alNodes.Count > 1)
                    throw new Exception("More than one node were found, given attribute: " + sAttr);

                if (alNodes.Count == 1)
                    nReturn = (XmlNode)alNodes[0];
                else if (alNodes.Count == 0)
                    nReturn = null;
            }
            catch (Exception e)
            {
#if (DEBUG)
                if (myListener != null)
                    Debug.Write(e.Message, e.Source);
#endif
                //Default Exception Handling behavior
                throw e;
            }

            return nReturn;
        }

        /**
        * sNodeName/sNamespaceURI_node/sNamespaceURI_attr are optional
        */
        public XmlNode GetNode(XmlNode nParent, string sNodeName, string sNamespaceURI_node, string sAttr, string sValue, string sNamespaceURI_attr, bool bRequired)
        {
            XmlNode nTemp = null, nReturn = null;
            ArrayList alNodes = new ArrayList();
            ArrayList alNodeList = null;
            int iNumNodeList = 0;

            try
            {
                if (nParent != null && sNodeName != null && sAttr != null && sValue != null)
                {
                    if (sNamespaceURI_node != null && sNamespaceURI_node.Equals(string.Empty))
                        sNamespaceURI_node = null;
                    if (sNamespaceURI_attr != null && sNamespaceURI_attr.Equals(string.Empty))
                        sNamespaceURI_attr = null;

                    alNodeList = GetNodes(nParent, sNodeName, sNamespaceURI_node, bRequired);
                    if (alNodeList != null)
                    {
                        iNumNodeList = alNodeList.Count;
                        for (int i = 0; i < iNumNodeList; i++)
                        {
                            nTemp = (XmlNode)alNodeList[i];
                            if (!SearchAttrNode(nTemp, sAttr, sValue, sNamespaceURI_attr)) continue;

                            alNodes.Add(nTemp);
                        }
                    }
                }

                if (bRequired && (alNodes.Count == 0))
                    throw new Exception("Node: " + sNodeName + " is not found!");

                if (alNodes.Count > 1)
                    throw new Exception("More than one node: " + sNodeName + " were found");

                if (alNodes.Count == 1)
                    nReturn = (XmlNode)alNodes[0];
                else if (alNodes.Count == 0)
                    nReturn = null;
            }
            catch (Exception e)
            {
#if (DEBUG)
                if (myListener != null)
                    Debug.Write(e.Message, e.Source);
#endif
                //Default Exception Handling behavior
                throw e;
            }

            return nReturn;
        }


        /*overloading method
         * sNamespaceURI is optional
         * */
        public XmlNode GetNode(XmlNode nParent, bool bRequired, string sNodeName, string sNamespaceURI)
        {
            XmlNodeList nl = null;
            XmlNode nReturn = null;
            ArrayList alNodes = new ArrayList();
            int iNumNodeList = 0;

            try
            {
                if (nParent != null && sNodeName != null)
                {
                    nl = nParent.ChildNodes;
                    //Check correct field node

                    if (nl != null)
                    {
                        if (sNamespaceURI != null && sNamespaceURI.Equals(string.Empty))
                            sNamespaceURI = null;

                        iNumNodeList = nl.Count;
                        for (int i = 0; i < iNumNodeList; i++)
                        {
                            if (nl.Item(i).NodeType != XmlNodeType.Element) continue;
                            if (!nl.Item(i).LocalName.ToUpper().Equals(sNodeName.ToUpper())) continue;
                            if (sNamespaceURI != null && !nl.Item(i).NamespaceURI.Equals(sNamespaceURI)) continue;

                            alNodes.Add(nl.Item(i));
                        }
                    }
                }
                else
                {
                    throw new Exception("Error parameter!");
                }

                if (bRequired && (alNodes.Count == 0))
                    throw new Exception("Node: " + sNodeName + " is not found!");

                if (alNodes.Count > 1)
                    //raise an error
                    throw new Exception("More than one node: " + sNodeName + " are found!");

                if (alNodes.Count == 1)
                    nReturn = (XmlNode)alNodes[0];
                else if (alNodes.Count == 0)
                    nReturn = null;
            }
            catch (Exception e)
            {
#if (DEBUG)
                if (myListener != null)
                    Debug.Write(e.Message, e.Source);
#endif
                //Default Exception Handling behavior
                throw e;
            }

            return nReturn;
        }

        //sNamespaceURI is optional
        public ArrayList GetNodes(XmlNode nParent, string sNodeName, string sNamespaceURI, bool bRequired)
        {
            XmlNodeList nl = null;
            ArrayList alNodes = new ArrayList();
            ArrayList alReturn = null;
            int iNumNodeList = 0;

            try
            {
                if ((nParent != null) && (sNodeName != null))
                {
                    nl = nParent.ChildNodes;
                    //Check correct field node

                    if (nl != null)
                    {
                        if (sNamespaceURI != null && sNamespaceURI.Equals(string.Empty))
                            sNamespaceURI = null;

                        iNumNodeList = nl.Count;
                        for (int i = 0; i < iNumNodeList; i++)
                        {
                            if (nl.Item(i).NodeType != XmlNodeType.Element) continue;
                            if (!nl.Item(i).LocalName.ToUpper().Equals(sNodeName.ToUpper())) continue;
                            if (sNamespaceURI != null && !nl.Item(i).NamespaceURI.Equals(sNamespaceURI)) continue;

                            alNodes.Add(nl.Item(i));
                        }
                    }
                }
                else
                {
                    throw new Exception("Error parameter!");
                }

                if (bRequired && (alNodes.Count == 0))
                    throw new Exception("Node: " + sNodeName + " is required!");

                if (alNodes.Count == 0) alReturn = null;
                else alReturn = alNodes;
            }
            catch (Exception e)
            {
#if (DEBUG)
                if (myListener != null)
                    Debug.Write(e.Message, e.Source);
#endif
                //Default Exception Handling behavior
                throw e;
            }

            return alReturn;
        }

        /**
        * sNamespaceURI_node/sNamespaceURI_attr are optional
        * overloading method
        */
        public ArrayList GetNodes(XmlNode nParent, string sNodeName, string sNamespaceURI_node, string sAttr, string sValue, string sNamespaceURI_attr, bool bRequired)
        {
            XmlNodeList nl = null;
            XmlNode nTemp = null;
            ArrayList alNodes = new ArrayList();
            ArrayList alNodeList = null;
            ArrayList alReturn = null;
            int iNumNodeList = 0;

            try
            {
                if ((nParent != null) && (sAttr != null))
                {
                    if (sNodeName != null)
                    {
                        if (sNamespaceURI_node != null && sNamespaceURI_node.Equals(string.Empty))
                            sNamespaceURI_node = null;
                        if (sNamespaceURI_attr != null && sNamespaceURI_attr.Equals(string.Empty))
                            sNamespaceURI_attr = null;

                        alNodeList = GetNodes(nParent, sNodeName, sNamespaceURI_node, bRequired);

                        if (alNodeList != null)
                        {
                            iNumNodeList = alNodeList.Count;
                            for (int i = 0; i < iNumNodeList; i++)
                            {
                                nTemp = (XmlNode)alNodeList[i];
                                if (SearchAttrNode(nTemp, sAttr, sValue, sNamespaceURI_attr)) continue;

                                alNodes.Add(nTemp);
                            }
                        }
                    }
                    else
                    {
                        nl = nParent.ChildNodes;
                        if (nl != null)
                        {
                            iNumNodeList = nl.Count;
                            for (int i = 0; i < iNumNodeList; i++)
                            {
                                if (nl.Item(i).NodeType != XmlNodeType.Element) continue;
                                if (SearchAttrNode(nl.Item(i), sAttr, sValue, sNamespaceURI_attr)) continue;

                                alNodes.Add(nl.Item(i));
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception("Error parameter!");
                }

                if (bRequired && (alNodes.Count == 0))
                    throw new Exception("Node: " + sNodeName + " is required!");

                if (alNodes.Count == 0)
                    alReturn = null;
                else
                    alReturn = alNodes;
            }
            catch (Exception e)
            {
#if (DEBUG)
                if (myListener != null)
                    Debug.Write(e.Message, e.Source);
#endif
                //Default Exception Handling behavior
                throw e;
            }

            return alReturn;
        }

        /**
        * sNamespaceURI_attr are optional
        * overloading method
        */
        public ArrayList GetNodes(XmlNode nParent, string sAttr, string sValue, string sNamespaceURI_attr, bool bRequired)
        {
            XmlNodeList nl = null;
            ArrayList alNodes = new ArrayList();
            ArrayList alReturn = null;
            int iNumNodeList = 0;

            try
            {
                if ((nParent != null) && (sAttr != null) && (sValue != null))
                {
                    nl = nParent.ChildNodes;
                    if (nl != null)
                    {
                        iNumNodeList = nl.Count;
                        for (int i = 0; i < iNumNodeList; i++)
                        {
                            if (nl.Item(i).NodeType != XmlNodeType.Element) continue;
                            if (SearchAttrNode(nl.Item(i), sAttr, sValue, sNamespaceURI_attr)) continue;

                            alNodes.Add(nl.Item(i));
                        }
                    }
                }
                else
                {
                    throw new Exception("Error parameter!");
                }

                if (bRequired && (alNodes.Count == 0))
                    throw new Exception("Node is not found, given attribute: " + sAttr + " and value: " + sValue);

                if (alNodes.Count == 0)
                    alReturn = null;
                else
                    alReturn = alNodes;
            }
            catch (Exception e)
            {
#if (DEBUG)
                if (myListener != null)
                    Debug.Write(e.Message, e.Source);
#endif
                //Default Exception Handling behavior
                throw e;
            }

            return alReturn;
        }

        /**
        * sNamespaceURI_node/sNamespaceURI_attr are optional
        * overloading method
        */
        public ArrayList GetNodes(XmlNode nParent, bool bRequired, string sAttr, string sNamespaceURI_attr)
        {
            XmlNodeList nl = null;
            ArrayList alNodes = new ArrayList();
            ArrayList alReturn = null;
            int iNumNodeList = 0;

            try
            {
                if ((nParent != null) && (sAttr != null))
                {
                    nl = nParent.ChildNodes;
                    if (nl != null)
                    {
                        iNumNodeList = nl.Count;
                        for (int i = 0; i < iNumNodeList; i++)
                        {
                            if (nl.Item(i).NodeType != XmlNodeType.Element) continue;
                            if (SearchAttrNode(nl.Item(i), sAttr, sNamespaceURI_attr)) continue;

                            alNodes.Add(nl.Item(i));
                        }
                    }
                }
                else
                {
                    throw new Exception("Error parameter!");
                }

                if (bRequired && (alNodes.Count == 0))
                    throw new Exception("Node is not found, given attribute: " + sAttr);

                if (alNodes.Count == 0)
                    alReturn = null;
                else
                    alReturn = alNodes;
            }
            catch (Exception e)
            {
#if (DEBUG)
                if (myListener != null)
                    Debug.Write(e.Message, e.Source);
#endif
                //Default Exception Handling behavior
                throw e;
            }

            return alReturn;
        }

        /**
        * If more than one identical sAttr is found, it's an error.
        * sValue and sNamespaceURI is optional
        */
        public bool SearchAttrNode(XmlNode nParent, string sAttr, string sValue, string sNamespaceURI)
        {
            bool bFound = false;
            XmlAttributeCollection acAttributes = null;
            XmlNode nAttr = null;
            int iNumAttr = 0;

            try
            {
                if (nParent != null && sAttr != null && sValue != null)
                {
                    acAttributes = nParent.Attributes;
                    //Attribute nodes in a field node
                    if (acAttributes != null)
                    {
                        if (sNamespaceURI != null && sNamespaceURI.Equals(string.Empty))
                            sNamespaceURI = null;

                        iNumAttr = acAttributes.Count;
                        //Walk thru each individual attribute
                        if (sValue != null)
                        {
                            for (int x = 0; x < iNumAttr; x++)
                            {
                                nAttr = acAttributes.Item(x);
                                if (!nAttr.LocalName.ToUpper().Equals(sAttr)) continue;
                                if (!nAttr.Value.ToUpper().Equals(sValue.ToUpper())) continue;
                                if (sNamespaceURI != null && !nAttr.NamespaceURI.Equals(sNamespaceURI)) continue;

                                bFound = true;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception("Missing parameters");
                }
            }
            catch (Exception e)
            {
#if (DEBUG)
                if (myListener != null)
                    Debug.Write(e.Message, e.Source);
#endif
                //Default Exception Handling behavior
                throw e;
            }

            return bFound;
        }

        /**
        * If more than one identical sAttr is found, it's an error.
        * sNamespaceURI is optional
        */
        public bool SearchAttrNode(XmlNode nParent, string sAttr, string sNamespaceURI)
        {
            bool bFound = false;
            XmlAttributeCollection acAttributes = null;
            XmlNode nAttr = null;
            int iNumAttr = 0;

            try
            {
                if (nParent != null && sAttr != null)
                {
                    acAttributes = nParent.Attributes;
                    //Attribute nodes in a field node
                    if (acAttributes != null)
                    {
                        if (sNamespaceURI != null && sNamespaceURI.Equals(string.Empty))
                            sNamespaceURI = null;

                        iNumAttr = acAttributes.Count;
                        //Walk thru each individual attribute
                        for (int x = 0; x < iNumAttr; x++)
                        {
                            nAttr = acAttributes.Item(x);
                            if (!nAttr.LocalName.ToUpper().Equals(sAttr)) continue;
                            if (sNamespaceURI != null && !nAttr.NamespaceURI.Equals(sNamespaceURI)) continue;

                            bFound = true;
                            break;
                        }
                    }
                }
                else
                {
                    throw new Exception("Missing parameters");
                }
            }
            catch (Exception e)
            {
#if (DEBUG)
                if (myListener != null)
                    Debug.Write(e.Message, e.Source);
#endif
                //Default Exception Handling behavior
                throw e;
            }

            return bFound;
        }

        //sNamespaceURI is optional
        public string GetAttributeValue(XmlNode nParent, string sAttr, string sNamespaceURI, bool bRequired)
        {
            XmlNode nAttr = null;
            XmlAttributeCollection acAttributes = null;
            string sReturn = null;
            int iNumAttr = 0;

            try
            {
                if (nParent != null && sAttr != null)
                {
                    acAttributes = nParent.Attributes;
                    //Attribute nodes in a field node
                    if (acAttributes != null)
                    {
                        if (sNamespaceURI != null && sNamespaceURI.Equals(string.Empty))
                            sNamespaceURI = null;

                        iNumAttr = acAttributes.Count;
                        //Walk thru each individual attribute
                        for (int x = 0; x < iNumAttr; x++)
                        {
                            nAttr = acAttributes.Item(x);
                            if (!nAttr.LocalName.ToUpper().Equals(sAttr.ToUpper())) continue;
                            if (sNamespaceURI != null && !nAttr.NamespaceURI.Equals(sNamespaceURI)) continue;

                            sReturn = nAttr.Value;
                        }

                        if (bRequired && (sReturn == null))
                            throw new Exception("Cannot find attributes. Attribute:" + sAttr);
                    }
                    else
                    {
                        if (bRequired)
                            throw new Exception("Cannot find attributes. Attribute:" + sAttr);
                    }
                }
                else
                {
                    throw new Exception("Parameter error. Either nParent or sAttr is null.");
                }
            }
            catch (Exception e)
            {
#if (DEBUG)
                if (myListener != null)
                    Debug.Write(e.Message, e.Source);
#endif
                //Default Exception Handling behavior
                throw e;
            }

            return sReturn;
        }

        //sNamespaceURI is optional
        public XmlAttribute GetAttribute(XmlElement el, string sAttrName, string sNamespaceURI)
        {
            if (sNamespaceURI != null && sNamespaceURI.Equals(string.Empty))
                sNamespaceURI = null;

            return el.GetAttributeNode(sAttrName.ToUpper(), sNamespaceURI);
        }
    }
}
