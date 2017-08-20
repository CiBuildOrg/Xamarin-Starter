using Android.Views;

namespace App.Template.XForms.Android.Infrastructure.Interaction.TopViewHolders
{
	public interface ITopContentViewHolder
	{
		ViewGroup ContentView { get; }

		void OnStart();

		void OnPause();
	}
}