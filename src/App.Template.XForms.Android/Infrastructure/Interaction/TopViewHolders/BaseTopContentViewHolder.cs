using System;
using Android.Content;
using Android.Views;

namespace App.Template.XForms.Android.Infrastructure.Interaction.TopViewHolders
{
	public abstract class BaseTopContentViewHolder : ITopContentViewHolder
	{
		private readonly Lazy<ViewGroup> _lazyContentView;

	    protected BaseTopContentViewHolder(Context context, ViewGroup root)
		{
			Context = context;
			_lazyContentView = new Lazy<ViewGroup>(() => (ViewGroup)LayoutInflater.From(context).Inflate(ContentId, root));
		}

		public ViewGroup ContentView => _lazyContentView.Value;

		protected abstract int ContentId { get; }

		protected Context Context { get; }

		public virtual void OnStart()
		{

		}

		public virtual void OnPause()
		{

		}
	}
}