﻿using System;
using System.Text;
using System.Xml.Linq;

namespace DANFS.DataAccess
{
	public class ShipXMLTransformer
	{
		public static string GetHTML(XDocument shipDocument)
		{
			StringBuilder sb = new StringBuilder();

			ProcessShipElement(shipDocument.Root, sb);

			return sb.ToString();
		}

		private static void ProcessShipElement(XElement element, StringBuilder sb)
		{
			if (element.Name == "date")
			{
				//Just insert a date marker.
				sb.Append("<span id=\"date-");
				sb.Append(element.Attribute("date_guid").Value);
				sb.Append("\" style=\"color:navy\">");
			}

			if (element.Name == "p")
			{
				sb.Append("<p>");
			}

			if (element.Name == "i")
			{
				sb.Append("<i>");
			}

			if (element.Name == "root")
			{
				sb.Append("<html><body>");
			}

			if (element.Name == "shipmetrics")
			{
				sb.Append("<table>");
			}

			if (element.Name == "metric")
			{
				sb.Append("<tr>");
			}

			if (element.Name == "key" ||
			   element.Name == "value")
			{
				sb.Append("<td>");
			}

			if (element.Name == "LOCATION")
			{
				sb.Append("<span id=\"location-");
				sb.Append(element.Attribute("location_guid").Value);
				sb.Append("\" style=\"color:red\">");
			}

			foreach (var childNode in element.Nodes())
			{
				if (childNode is XText)
				{
					sb.Append((childNode as XText).Value);
				}
				else if (childNode is XElement)
				{
					ProcessShipElement((childNode as XElement), sb);
				}
			}

			if (element.Name == "root")
			{
				sb.Append("</body></html>");
			}


			if (element.Name == "p")
			{
				sb.Append("</p>");
			}

			if (element.Name == "i")
			{
				sb.Append("</i>");
			}

			if (element.Name == "date" || element.Name == "LOCATION")
			{
				sb.Append("</span>");
			}

			if (element.Name == "shipmetrics")
			{
				sb.Append("</table>");
			}

			if (element.Name == "metric")
			{
				sb.Append("</tr>");
			}

			if (element.Name == "key" ||
			   element.Name == "value")
			{
				sb.Append("</td>");
			}
		}
	}
}

