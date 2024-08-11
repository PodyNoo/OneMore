//************************************************************************************************
// Copyright © 2021 Steven M. Cohn. All Rights Reserved.
//************************************************************************************************

namespace River.OneMoreAddIn.Settings
{
	using System;
	using System.Drawing;
	using System.Windows.Forms;
	using Resx = River.OneMoreAddIn.Properties.Resources;


	internal partial class RemindersSheet : SheetBase
	{

		private readonly Color defaultColor = Color.DarkGray;

		public RemindersSheet(SettingsProvider provider) : base(provider)
		{
			InitializeComponent();

			Name = "RemindersSheet";
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
			var settings = provider.GetCollection(Name);
			colorCheckBox.Checked = settings.Get("coloredStrikeoutTasks", false);
			if (colorCheckBox.Checked)
			{
				var color = settings.Get("strikeoutTasksColor", defaultColor.ToRGBHtml());
				colorBox.BackColor = ColorTranslator.FromHtml(color);
			}
			else
			{
				colorBox.BackColor = Color.Transparent;
			}
		}

		private void ChangeLineColor(object sender, System.EventArgs e)
		{
			var location = PointToScreen(colorBox.Location);

			using var dialog = new UI.MoreColorDialog(Resx.PageColorDialog_Text,
				location.X + colorBox.Bounds.Location.X + (colorBox.Width / 2),
				location.Y - 50);

			dialog.Color = colorBox.BackColor;
			if (dialog.ShowDialog(this) == DialogResult.OK)
			{
				colorBox.BackColor = dialog.Color;
			}
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

		private void colorCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (colorCheckBox.Checked)
			{
				clickLabel.Visible = true;
				colorBox.Enabled = true;
				colorBox.BackColor = defaultColor;
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
