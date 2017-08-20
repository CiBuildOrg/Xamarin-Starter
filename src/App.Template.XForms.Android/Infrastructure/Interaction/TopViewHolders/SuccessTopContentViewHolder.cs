using Android.Content;
using Android.Views;
using Android.Views.Animations;

namespace App.Template.XForms.Android.Infrastructure.Interaction.TopViewHolders
{
	public class SuccessTopContentViewHolder : BaseTopContentViewHolder
	{
		private AnimationSet _mSuccessLayoutAnimSet;
		private Animation _mSuccessBowAnim;

		private View _mSuccessLeftMask;
		private SuccessTickView _mSuccessTick;
		private View _mSuccessRightMask;

		public SuccessTopContentViewHolder(Context context, ViewGroup root) : base(context, root)
		{
		}

		protected override int ContentId => Resource.Layout.success_top_view;

		public override void OnStart()
		{
			_mSuccessTick = (SuccessTickView)ContentView.FindViewById(Resource.Id.success_top_tick);
			_mSuccessLeftMask = ContentView.FindViewById(Resource.Id.success_top_mask_left);
			_mSuccessRightMask = ContentView.FindViewById(Resource.Id.success_top_mask_right);
			_mSuccessLayoutAnimSet = (AnimationSet)AnimationUtils.LoadAnimation(Context, Resource.Animation.success_mask_layout);

			_mSuccessBowAnim = AnimationUtils.LoadAnimation(Context, Resource.Animation.success_bow_roate);
			_mSuccessLeftMask.StartAnimation(_mSuccessLayoutAnimSet.Animations[0]);
			_mSuccessRightMask.StartAnimation(_mSuccessLayoutAnimSet.Animations[1]);

			_mSuccessTick.StartTickAnim();
			_mSuccessRightMask.StartAnimation(_mSuccessBowAnim);
		}
	}
}
