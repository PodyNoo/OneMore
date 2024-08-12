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
	using River.OneMoreAddIn.UI;

	/// <summary>
	/// Toggles strikethrough text next to all completed/incompleted tags
	/// </summary>
	internal class StrikeoutTasksCommand : Command
	{

		private static string DefaultTextColor => ThemeManager.Instance.GetColor("ControlText").ToRGBHtml(); // FIX: not working in dark mode, while hard-coded #000000 seems to work
		private StyleAnalyzer styleAnalyzer;

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
			styleAnalyzer = new StyleAnalyzer(page.Root);

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
			}

			if (modified)
			{
				await one.Update(page);
			}
		}

		private bool RestyleText(XCData cdata, bool completed)
		{
			var currentStyle = new Style(styleAnalyzer.CollectFrom(cdata.Parent, true));
			Style newStyle = null;

			var coloredStrikeoutTasks = RemindersSheet.ColoredStrikeoutTasksActive;
			var strikeoutTasksColor = RemindersSheet.StrikeoutTasksColor;
			string currentTextColor = null;

			if (coloredStrikeoutTasks &&
				!string.IsNullOrWhiteSpace(currentStyle.Color) &&
				!currentStyle.Color.Equals(Style.Automatic))
			{
				currentTextColor = ColorTranslator.FromHtml(currentStyle.Color).ToRGBHtml();
			}

			if (completed)
			{
				if (!currentStyle.IsStrikethrough)
				{
					newStyle ??= new Style(currentStyle);
					newStyle.IsStrikethrough = true;
				}
				if (coloredStrikeoutTasks && (currentTextColor == null || currentTextColor != strikeoutTasksColor))
				{
					newStyle ??= new Style(currentStyle);
					newStyle.Color = strikeoutTasksColor;
					newStyle.ApplyColors = true;
				}
			}
			else
			{
				if (currentStyle.IsStrikethrough)
				{
					newStyle ??= new Style(currentStyle);
					newStyle.IsStrikethrough = false;
				}
				if (coloredStrikeoutTasks && currentTextColor != null && currentTextColor == strikeoutTasksColor)
				{
					newStyle ??= new Style(currentStyle);
					newStyle.Color = DefaultTextColor;
					newStyle.ApplyColors = true;
				}
			}

			if (newStyle is not null)
			{
				new Stylizer(newStyle).ApplyStyle(cdata);
				return true;
			}

			return false;
		}
	}
}
/*
![CDATA[<span style='text-decoration:line-through'>Line OneOneOne<br>asdfasf</span>]]/
*/
