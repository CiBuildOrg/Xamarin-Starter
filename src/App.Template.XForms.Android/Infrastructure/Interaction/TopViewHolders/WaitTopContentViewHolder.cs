using Android.Content;
using Android.Views;

namespace App.Template.XForms.Android.Infrastructure.Interaction.TopViewHolders
{
	public class WaitTopContentViewHolder : BaseTopContentViewHolder
	{
		public WaitTopContentViewHolder(Context context, ViewGroup root) : base(context, root)
		{

		}

		protected override int ContentId => Resource.Layout.wait_top_view;

		public override void OnStart()
		{

		}
	}
}