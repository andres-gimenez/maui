using System;
using Android.Content;
using Android.Views;
using Microsoft.Maui.Controls.Internals;
using AListView = Android.Widget.ListView;
using AView = Android.Views.View;

namespace Microsoft.Maui.Controls.Handlers.Compatibility
{
	public static class CellFactory
	{
		public static AView GetCell(Cell item, AView convertView, ViewGroup parent, Context context, View view)
		{
			CellRenderer renderer = CellRenderer.GetRenderer(item);
			if (renderer == null)
			{
				var mauiContext = view.FindMauiContext() ?? item.FindMauiContext();
				item.ConvertView = convertView;

				_ = item.ToNative(mauiContext);
				item.ConvertView = null;

				renderer = CellRenderer.GetRenderer(item);
			}

			AView result = renderer.GetCell(item, convertView, parent, context);

			if (view is TableView)
				UpdateMinimumHeightFromParent(context, result, (TableView)view);
			else if (view is ListView)
				UpdateMinimumHeightFromParent(context, result, (ListView)view);

			return result;
		}

		static void UpdateMinimumHeightFromParent(Context context, AView view, TableView table)
		{
			if (!table.HasUnevenRows && table.RowHeight > 0)
				view.SetMinimumHeight((int)context.ToPixels(table.RowHeight));
		}

		static void UpdateMinimumHeightFromParent(Context context, AView view, ListView listView)
		{
			if (!listView.HasUnevenRows && listView.RowHeight > 0)
				view.SetMinimumHeight((int)context.ToPixels(listView.RowHeight));
		}
	}
}