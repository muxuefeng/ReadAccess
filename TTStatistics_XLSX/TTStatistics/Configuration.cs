using System;
using System.Xml;

public static class ConfigurationManager
{
    private static string m_strToken = "1.1.1005";
    private static string m_strConfigurationFile;
    private static bool m_bLoaded = false;
    private static System.Xml.XmlDocument m_xmlDoc = new System.Xml.XmlDocument();

#pragma warning disable 0168
    public static bool LoadConfiguration(string strConfigurationFile)
    {
        if (m_strConfigurationFile == strConfigurationFile)
            return true;

        bool bReset = false;
        string strDiscCode = null;
        string strPrtPath = null;
        string strCurCul = null;
        string strConnectionString = null;

        try
        {
            m_xmlDoc.Load(strConfigurationFile);
            if (m_xmlDoc.DocumentElement.Name != "Configuration")
            {
                m_bLoaded = false;
                return false;
            }

            m_strConfigurationFile = strConfigurationFile;
            m_bLoaded = true;

            System.Xml.XmlNodeList ndLstMsg = m_xmlDoc.SelectNodes("Configuration/ConfigSections/Token/self::*");
            if (ndLstMsg.Count != 1 || ndLstMsg[0].InnerText != m_strToken)
            {
                strDiscCode = GetUserSettingString("DiscCode");
                strPrtPath = GetUserSettingString("PrtPath");
                strCurCul = GetUserSettingString("CultureName");
                strConnectionString = GetUserSettingString("ConnectionString");
                bReset = true;

                throw (new System.Exception());
            }
        }
        catch (System.Exception ex)
        {
            System.Text.StringBuilder strXML = new System.Text.StringBuilder();
            strXML.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            strXML.AppendLine("<Configuration>");
            strXML.AppendLine(" <ConfigSections>");
            strXML.AppendLine(String.Format("   <Token>{0}</Token>", m_strToken));
            strXML.AppendLine(" </ConfigSections>");
            strXML.AppendLine(" <UserSettings>");
            strXML.AppendLine("     <CultureName></CultureName>");
            strXML.AppendLine("     <Path>D:</Path>");
            strXML.AppendLine("     <MatchAPI></MatchAPI>");
            strXML.AppendLine("     <ResultAPI></ResultAPI>");
            strXML.AppendLine(" </UserSettings>");
            strXML.AppendLine("</Configuration>");

            m_xmlDoc.LoadXml(strXML.ToString());

            m_strConfigurationFile = strConfigurationFile;
            m_bLoaded = true;

            if (bReset)
            {
                SetUserSettingString("DiscCode", strDiscCode);
                SetUserSettingString("ConnectionString", strConnectionString);
                SetUserSettingString("CultureName", strCurCul);
                SetUserSettingString("PrtPath", strPrtPath);
            }
        }
        return true;
    }
#pragma warning restore 0168

    public static bool SaveConfiguration()
    {
        if (!m_bLoaded) return true;

        try
        {
            m_xmlDoc.Save(m_strConfigurationFile);
        }
        catch (System.Exception)
        {
            return false;
        }
        return true;
    }

    public static string GetUserSettingString(string strItem)
    {
        if (!m_bLoaded) return "";

        string strSel = "/Configuration/UserSettings/" + strItem + "/self::*";
        System.Xml.XmlNodeList ndLstMsg = m_xmlDoc.SelectNodes(strSel);

        if (ndLstMsg.Count < 1) return "";

        return ndLstMsg[0].InnerText;
    }

    public static void SetUserSettingString(string strItem, string strValue)
    {
        if (!m_bLoaded) return;

        string strSel = "/Configuration/UserSettings/" + strItem + "/self::*";
        System.Xml.XmlNodeList ndLstMsg = m_xmlDoc.SelectNodes(strSel);

        if (ndLstMsg.Count == 1)
            ndLstMsg[0].InnerText = strValue;
    }

    public static string GetPluginSettingString(string strDiscCode, string strItem)
    {
        if (!m_bLoaded) return "";

        string strSel = "/Configuration/PluginSettings/" + strDiscCode + "/" + strItem + "/self::*";
        System.Xml.XmlNodeList ndLstMsg = m_xmlDoc.SelectNodes(strSel);

        if (ndLstMsg.Count < 1) return "";

        return ndLstMsg[0].InnerText;
    }

    public static void SetPluginSettingString(string strDiscCode, string strItem, string strValue)
    {
        if (!m_bLoaded) return;

        string strSel = "/Configuration/PluginSettings/" + strDiscCode + "/" + strItem + "/self::*";
        System.Xml.XmlNodeList ndList = m_xmlDoc.SelectNodes(strSel);

        if (ndList.Count == 1)
            ndList[0].InnerText = strValue;
        else
        {
            XmlNode nodePlugin, nodeDiscipline, nodeItem;
            strSel = "/Configuration/PluginSettings/" + strDiscCode + "/self::*";
            ndList = m_xmlDoc.SelectNodes(strSel);

            if (ndList.Count < 1)
            {
                strSel = "/Configuration/PluginSettings/self::*";
                ndList = m_xmlDoc.SelectNodes(strSel);

                if (ndList.Count < 1)
                {
                    strSel = "/Configuration/self::*";
                    ndList = m_xmlDoc.SelectNodes(strSel);

                    if (ndList.Count < 1) return;

                    nodePlugin = m_xmlDoc.CreateNode(XmlNodeType.Element, "PluginSettings", null);
                    ndList[0].AppendChild(nodePlugin);
                }
                else
                    nodePlugin = ndList[0];

                nodeDiscipline = m_xmlDoc.CreateNode(XmlNodeType.Element, strDiscCode, null);
                nodePlugin.AppendChild(nodeDiscipline);
            }
            else
                nodeDiscipline = ndList[0];

            nodeItem = m_xmlDoc.CreateNode(XmlNodeType.Element, strItem, null);
            nodeDiscipline.AppendChild(nodeItem);
            nodeItem.InnerText = strValue;
        }
    }


}
