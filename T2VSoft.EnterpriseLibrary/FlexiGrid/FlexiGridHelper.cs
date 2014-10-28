using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace T2VSoft.EnterpriseLibrary.FlexiGrid
{
    public class FlexiGridHelper
    {
        public static XmlDocument GetXmlTemplate(string currentPage, string recTotal)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement rootNode = xmlDoc.CreateElement("rows");
            XmlElement pageNode = xmlDoc.CreateElement("page");
            pageNode.AppendChild(xmlDoc.CreateTextNode(currentPage));
            XmlElement totalNode = xmlDoc.CreateElement("total");
            totalNode.AppendChild(xmlDoc.CreateTextNode(recTotal));

            xmlDoc.AppendChild(rootNode);
            rootNode.AppendChild(pageNode);
            rootNode.AppendChild(totalNode);
            return xmlDoc;
        }

        public static XmlElement GetXmlRowData(XmlDocument xmlDoc, string id)
        {
            XmlElement rowNode = xmlDoc.CreateElement("row");
            rowNode.SetAttribute("id", id);
            xmlDoc.DocumentElement.PrependChild(rowNode);
            return rowNode;
        }
        public static XmlElement GetSelectCellElement(XmlDocument xmlDoc, string colName)
        {
            string steSelect = @"<SELECT >
                                         <OPTION VALUE=1>Action1</option>
                                         <OPTION VALUE=2>Action2</option>
                                </SELECT>";
            XmlElement mainNode = xmlDoc.CreateElement("cell");
            mainNode.InnerXml = "<![CDATA[" + steSelect + "]]>";
            return mainNode;
        }


        public static XmlElement GetIdCellElement(XmlDocument xmlDoc, string colName, string idColName)
        {
            XmlElement mainNode = xmlDoc.CreateElement("cell");
            string span = "<span class='IdSpan' rowID='" + idColName + "' >" + colName + " </span>";
            mainNode.InnerXml = "<![CDATA[" + span + "]]>";
            return mainNode;
        }
        public static XmlElement GetCellElement(XmlDocument xmlDoc, string data)
        {
            XmlElement mainNode = xmlDoc.CreateElement("cell");
            mainNode.InnerXml = "<![CDATA[" + data + "]]>";
            return mainNode;
        }
        public static XmlElement GetDateCellElement(XmlDocument xmlDoc, string data)
        {
            XmlElement mainNode = xmlDoc.CreateElement("cell");
            DateTime dtTime = new DateTime();
            dtTime = Convert.ToDateTime(data);
            mainNode.InnerXml = "<![CDATA[" + dtTime.ToString("yyyy-MM-dd") + "]]>";
            return mainNode;
        }
        public static XmlElement GetAttributeCellElement(XmlDocument xmlDoc, string data, string attr)
        {
            string spanData = "<span " + attr + " >" + data + " </span>";
            XmlElement node = FlexiGridHelper.GetCellElement(xmlDoc, spanData);
            return node;
        }
        public static XmlElement GetLinkCellElement(XmlDocument xmlDoc, string showColumn, string method)
        {
            string strInput = @"<input type='button' value='" + showColumn + "' onclick=\"" + method + "\" class='ListLink' />";
            XmlElement mainNode = xmlDoc.CreateElement("cell");
            mainNode.InnerXml = "<![CDATA[" + strInput + "]]>";
            return mainNode;
        }
        public static XmlElement GetALinkCellElement(XmlDocument xmlDoc, string showColumn, string url)
        {
            string strAlink = @"<a href='" + url + "' class='ListLink' target='_blank'>" + showColumn + "</a>";
            XmlElement mainNode = xmlDoc.CreateElement("cell");
            mainNode.InnerXml = "<![CDATA[" + strAlink + "]]>";
            return mainNode;
        }
        public static XmlElement GetRadioCellElement(XmlDocument xmlDoc, string groupName, string showColumn, string method)
        {
            string strInput = @"<input type='radio' name='" + groupName + "' value='" + showColumn + "' onclick=\"" + method + "\" />";
            XmlElement mainNode = xmlDoc.CreateElement("cell");
            mainNode.InnerXml = "<![CDATA[" + strInput + "]]>";
            return mainNode;
        }
        public static XmlElement GetCheckBoxCellElement(XmlDocument xmlDoc, bool flag)
        {
            string strInput = string.Empty;
            if (flag)
                strInput = @"<input type='checkbox'>";
            else
                strInput = @"<input type='checkbox' disabled='disabled'>";

            XmlElement node = xmlDoc.CreateElement("cell");
            node.InnerXml = "<![CDATA[" + strInput + "]]>";
            return node;
        }


        //B2B
        public static XmlElement GetLink2CellElement(XmlDocument xmlDoc, string showColumn, string method)
        {
            string[] lst = showColumn.Split(';');

            XmlElement mainNode = xmlDoc.CreateElement("cell");
            if (!string.IsNullOrEmpty(lst[0]))
            {
                string csvName = lst[0];
                string strInput = @"<input type='button' value='" + csvName.Substring(csvName.LastIndexOf("/") + 1) + "' onclick=\"" + method.Replace("URL", csvName) + "\" class='ListLink' />";

                if (lst.Length > 1 && !string.IsNullOrEmpty(lst[1]))
                {
                    string ediName = lst[1];
                    strInput += "<br>";
                    strInput += @"<input type='button' value='" + ediName.Substring(ediName.LastIndexOf("/") + 1) + "' onclick=\"" + method.Replace("URL", ediName) + "\" class='ListLink' />";
                }

                mainNode.InnerXml = "<![CDATA[" + strInput + "]]>";
            }

            return mainNode;
        }

    }
}
