//************************************************************************************************
// Copyright © 2021 Steven M. Cohn. All Rights Reserved.
//************************************************************************************************

namespace River.OneMoreAddIn.Settings
{
	using System;
	using System.Drawing;
	using System.Windows.Forms;
	using Resx = River.OneMoreAddIn.Properties.Resources;
	using River.OneMoreAddIn.UI;

	internal partial class RemindersSheet : SheetBase
	{
		private static Color DefaultStrikeoutTasksColor => ThemeManager.Instance.GetColor("GrayText");
		public static bool ColoredStrikeoutTasksActive => GetSettingsColletion().Get("coloredStrikeoutTasks", false);
		public static string StrikeoutTasksColor => GetSettingsColletion().Get("strikeoutTasksColor", DefaultStrikeoutTasksColor.ToRGBHtml());
		
		public RemindersSheet(SettingsProvider provider) : base(provider)
		{
			InitializeComponent();

			Name = nameof(RemindersSheet);
			Title = Resx.ribRemindersMenu_Label;

			if (NeedsLocalizing())
			{
				Localize(new string[]
				{
					"introBox",
					"colorCheckBox",
					"clickLabel",
					"colorLabel",
				});
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			colorCheckBox.Checked = ColoredStrikeoutTasksActive;
			if (colorCheckBox.Checked)
			{
				var color = StrikeoutTasksColor;
				colorBox.BackColor = ColorTranslator.FromHtml(color);
			}
			else
			{
				colorBox.BackColor = Color.Transparent;
			}
		}

		private void ChangeLineColor(object sender, EventArgs e)
		{
			var location = PointToScreen(colorBox.Location);

			using var dialog = new MoreColorDialog(Resx.PageColorDialog_Text,
				location.X + colorBox.Bounds.Location.X + (colorBox.Width / 2),
				location.Y - 50);

			dialog.Color = colorBox.BackColor;
			if (dialog.ShowDialog(this) == DialogResult.OK)
			{
				colorBox.BackColor = dialog.Color;
			}
		}

		private static SettingsCollection GetSettingsColletion(SettingsProvider settingsProvider = null)
		{
			var provider = settingsProvider ?? new SettingsProvider();
			return provider.GetCollection(nameof(RemindersSheet));
		}

		public override bool CollectSettings()
		{
			var settings = provider.GetCollection(Name);

			if (colorCheckBox.Checked)
			{
				settings.Add("strikeoutTasksColor", colorBox.BackColor.ToRGBHtml());
				settings.Add("coloredStrikeoutTasks", true);
			}
			else
			{
				settings.Remove("strikeoutTasksColor");
				settings.Remove("coloredStrikeoutTasks");
			}

			provider.SetCollection(settings);
			return false;
		}

		private void CheckedChanged(object sender, EventArgs e)
		{
			if (colorCheckBox.Checked)
			{
				clickLabel.Visible = true;
				colorBox.Enabled = true;
				colorBox.BackColor = DefaultStrikeoutTasksColor;
			}
			else
			{
				clickLabel.Visible = false;
				colorBox.Enabled = false;
				colorBox.BackColor = Color.Transparent;
			}
		}
	}
}
