using Android.Content;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;

namespace App.Template.XForms.Android.Infrastructure.Interaction.TopViewHolders
{
    public class EditTopContentViewHolder : BaseTopContentViewHolder
    {
        private Animation _mErrorInAnim;
        private AnimationSet _mErrorXInAnim;

        private ImageView _mErrorX;

        public EditTopContentViewHolder(Context context, ViewGroup root) : base(context, root)
        {

        }

        protected override int ContentId => Resource.Layout.edit_top_view;

        public override void OnStart()
        {
            _mErrorInAnim = CreateExitAnimation();
            _mErrorXInAnim = (AnimationSet)AnimationUtils.LoadAnimation(Context, Resource.Animation.error_x_in);

            _mErrorX = (ImageView)ContentView.FindViewById(Resource.Id.error_top_x);

            ContentView.StartAnimation(_mErrorInAnim);
            _mErrorX.StartAnimation(_mErrorXInAnim);
        }

        protected Animation CreateExitAnimation()
        {
            var exitAnimSet = new AnimationSet(true);
            exitAnimSet.AddAnimation(new AlphaAnimation(0, 1) { Duration = 400 });
            exitAnimSet.AddAnimation(new Rotate3DAnimation(0, 100, 0, 50, 50) { Duration = 400 });

            return exitAnimSet;
        }
    }
}