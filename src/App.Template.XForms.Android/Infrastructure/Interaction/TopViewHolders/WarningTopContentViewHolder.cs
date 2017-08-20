﻿using Android.Content;
using Android.Views;

namespace App.Template.XForms.Android.Infrastructure.Interaction.TopViewHolders
{
	public class WarningTopContentViewHolder : BaseTopContentViewHolder
	{
		public WarningTopContentViewHolder(Context context, ViewGroup root) : base(context, root)
		{

		}

		protected override int ContentId => Resource.Layout.warning_top_view;

		public override void OnStart()
		{

		}
	}
}