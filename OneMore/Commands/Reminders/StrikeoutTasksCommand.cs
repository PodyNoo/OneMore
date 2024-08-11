//************************************************************************************************
// Copyright © 2020 Steven M Cohn.  All rights reserved.
//************************************************************************************************

namespace River.OneMoreAddIn.Commands
{
	using River.OneMoreAddIn.Styles;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Xml.Linq;
	using River.OneMoreAddIn.Settings;
	using System.Drawing;

	/// <summary>
	/// Toggles strikethrough text next to all completed/incompleted tags
	/// </summary>
	internal class StrikeoutTasksCommand : Command
	{
		public StrikeoutTasksCommand()
		{
		}


		public override async Task Execute(params object[] args)
		{
			// built-in checkable/completeable tags
			var symbols = new List<int> {
				3,  // To do
				8,  // Client request
				12, // Schedule meeting | Callback
				28, // To do priority 1
				71, // To do priority 2
				94, // Discuss with <Person A> | B>
				95  // Discuss with manager
			};

			await using var one = new OneNote(out var page, out var ns);
			var indexes =
				page.Root.Elements(ns + "TagDef")
				.Where(e => symbols.Contains(int.Parse(e.Attribute("symbol").Value)))
				.Select(e => e.Attribute("index").Value)
				.ToList();

			if (indexes.Count == 0)
			{
				return;
			}

			var elements = page.Root.Descendants(ns + "Tag")
				.Where(e => indexes.Contains(e.Attribute("index").Value));

			if (!elements.Any())
			{
				return;
			}

			var modified = false;

			foreach (var element in elements)
			{
				var completed = element.Attribute("completed")?.Value == "true";

				var cdatas =
					from e in element.NodesAfterSelf().OfType<XElement>()
					where e.Name.LocalName == "T"
					let c = e.Nodes().OfType<XCData>().FirstOrDefault()
					where c != null
					select c;

				foreach (var cdata in cdatas)
				{
					if (!string.IsNullOrEmpty(cdata.Value))
					{
						modified |= RestyleText(cdata, completed);
					}
				}

				//var disabled = element.Attribute("disabled");
				//if (completed && (disabled == null || disabled.Value != "true"))
				//{
				//	element.SetAttributeValue("disabled", "true");
				//}
				//else if (!completed && (disabled != null || disabled.Value == "true"))
				//{
				//	disabled.Remove();
				//}
			}

			if (modified)
			{
				await one.Update(page);
			}
		}

		private static bool RestyleText(XCData cdata, bool completed)
		{
			var modified = false;
			var wrapper = cdata.GetWrapper();
			var span = wrapper.Elements("span").FirstOrDefault(e => e.Attribute("style") != null);

			var provider = new SettingsProvider();
			var settings = provider.GetCollection(nameof(RemindersSheet));
			var coloredStrikeoutTasks = settings.Get("coloredStrikeoutTasks", false);
			string strikeoutTasksColor = settings.Get("strikeoutTasksColor", string.Empty);
			string currentStrikeoutTasksColor = null;

			if (coloredStrikeoutTasks)
			{
				var oeStyleAttribute = cdata.Parent.Parent.Attribute("style");
				if (oeStyleAttribute != null)
				{
					// To find last color attribute (can be multiples)
					var parts = oeStyleAttribute.Value.Split(
						new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries)
						.ToList();
					var lastColor = parts.FindLast(p => p.Trim().StartsWith("color:"));

					if (lastColor != null)
					{
						var color = lastColor.Replace("color:", string.Empty).Trim();
						currentStrikeoutTasksColor = ColorTranslator.FromHtml(color).ToRGBHtml();
					}
				}
			}

			if (completed)
			{
				if (span == null)
				{
					string styleContent = "text-decoration:line-through";
					if (coloredStrikeoutTasks)
					{
						styleContent += $";color:{strikeoutTasksColor}";
					}

					wrapper.FirstNode.ReplaceWith(
						new XElement("span",
							new XAttribute("style", styleContent),
							cdata.Value
						));

					modified = true;
				}
				else
				{
					var style = new Style(span.Attribute("style").Value);
					if (!style.IsStrikethrough)
					{
						style.IsStrikethrough = true;
						modified = true;
					}
					var css = style.ToCss(false);
					if (coloredStrikeoutTasks && (currentStrikeoutTasksColor == null || currentStrikeoutTasksColor != strikeoutTasksColor))
					{
						css += $";color:{strikeoutTasksColor}";
						modified = true;
					}
					if (modified)
					{
						if (string.IsNullOrEmpty(css))
						{
							wrapper.Value = span.Value;
						}
						else
						{
							span.SetAttributeValue("style", css);
						}
					}
				}
			}
			else
			{
				if (span != null)
				{
					var style = new Style(span.Attribute("style").Value);
					if (style.IsStrikethrough)
					{
						style.IsStrikethrough = false;
						modified = true;
					}
					var css = style.ToCss(false);
					if (coloredStrikeoutTasks && currentStrikeoutTasksColor != null && currentStrikeoutTasksColor == strikeoutTasksColor)
					{
						if (css.Trim().Length > 0)
						{
							css += ';';
						}
						css += "color:#000000";
						modified = true;
					}
					if (modified)
					{
						if (string.IsNullOrEmpty(css))
						{
							wrapper.Value = span.Value;
						}
						else
						{
							span.SetAttributeValue("style", css);
						}
					}
				}
				else
				{
					if (coloredStrikeoutTasks && currentStrikeoutTasksColor != null && currentStrikeoutTasksColor == strikeoutTasksColor)
					{
						wrapper.FirstNode.ReplaceWith(
							new XElement("span",
								new XAttribute("style", "color:#000000"),
								cdata.Value
						));

						modified = true;
					}
				}
			}

			cdata.Value = wrapper.GetInnerXml().Replace("&amp;", "&");

			return modified;
		}
	}
}
/*
![CDATA[<span style='text-decoration:line-through'>Line OneOneOne<br>asdfasf</span>]]/
*/
