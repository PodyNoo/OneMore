﻿//************************************************************************************************
// Copyright © 2020 Steven M Cohn. All rights reserved.
//************************************************************************************************

namespace River.OneMoreAddIn.Commands
{
	using River.OneMoreAddIn.Models;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	using System.Xml.Linq;
	using Resx = Properties.Resources;


	/// <summary>
	/// Resize and adjust images on the page
	/// </summary>
	internal class AdjustImagesCommand : Command
	{
		public AdjustImagesCommand()
		{
		}


		public override async Task Execute(params object[] args)
		{
			using var one = new OneNote(out var page, out var ns, OneNote.PageDetail.All);

			// starting at Outline should exclude all background images
			var elements = page.Root
				.Elements(ns + "Outline")
				.Descendants(ns + "Image")?
				.Where(e => e.Attribute("selected")?.Value == "all")
				.ToList();

			if ((elements == null) || !elements.Any())
			{
				elements = page.Root
					.Elements(ns + "Outline").Descendants(ns + "Image")
					.ToList();
			}

			if (elements.Any())
			{
				var updated = false;
				if (elements.Count == 1)
				{
					// resize single selected image only
					updated = ResizeOne(elements[0]);
				}
				else
				{
					// select many iamges, or all if none selected
					updated = ResizeMany(elements, page, page.Root.Elements(ns + "Image").Any());
				}

				if (updated)
				{
					await one.Update(page);
				}
			}
			else
			{
				UIHelper.ShowMessage(Resx.AdjustImagesDialog_noImages);
			}
		}


		private bool ResizeOne(XElement element)
		{
			var wrapper = new OneImage(element);
			using var image = wrapper.ReadImage();

			using var dialog = new AdjustImagesDialog(image, wrapper.Width, wrapper.Height);
			var result = dialog.ShowDialog(owner);
			if (result == DialogResult.OK)
			{
				var editor = dialog.GetImageEditor(image);
				if (editor.IsReady)
				{
					editor.Apply(wrapper);
					return true;
				}
			}

			return false;
		}


		private bool ResizeMany(IEnumerable<XElement> elements, Page page, bool hasBgImages)
		{
			using var dialog = new AdjustImagesDialog(hasBgImages);

			var result = dialog.ShowDialog(owner);
			if (result != DialogResult.OK)
			{
				return false;
			}

			var updated = false;
			foreach (var element in elements)
			{
				var wrapper = new OneImage(element);
				using var image = wrapper.ReadImage();

				var editor = dialog.GetImageEditor(image);

				// when pasting an image onto the page, width or height can be zero
				// OneNote ignores both if either is zero so we'll do the same...
				var viewWidth = wrapper.Width;
				if (viewWidth == 0)
				{
					viewWidth = image.Width;
				}

				if (editor.IsReady)
				{
					if (editor.AutoSize ||
						editor.Constraint == ImageEditor.SizeConstraint.All ||
						(
							editor.Constraint == ImageEditor.SizeConstraint.OnlyShrink &&
							viewWidth > editor.Size.Width) ||
						(
							editor.Constraint == ImageEditor.SizeConstraint.OnlyEnlarge &&
							viewWidth < editor.Size.Width))
					{
						using var edit = editor.Apply(wrapper);
						updated = true;
					}
					else
					{
						logger.WriteLine($"skipped image, size=[{wrapper.Width} x {wrapper.Width}]");
					}
				}
			}

			if (dialog.RepositionImages)
			{
				new StackBackgroundImagesCommand().StackImages(page);
				updated = true;
			}

			return updated;
		}
	}
}
