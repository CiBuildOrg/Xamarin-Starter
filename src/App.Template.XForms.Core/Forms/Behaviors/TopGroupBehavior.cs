using System.Threading.Tasks;
using Xamarin.Forms;

namespace App.Template.XForms.Core.Forms.Behaviors
{
    internal class TopGroupBehaviour : Behavior<View>
    {
        public Label TitleLabel { get; set; }
        public Image TopIcon { get; set; }

        private readonly ImageSource _heroImageDefault;
        private readonly ImageSource _heroImageSuccess;

        private View _rootView;

        public TopGroupBehaviour()
        {
            _heroImageDefault = ImageSource.FromResource(Assets.Path.HeroImageDefault);
            _heroImageSuccess = ImageSource.FromResource(Assets.Path.HeroImageSuccess);
        }

        protected override void OnAttachedTo(View bindable)
        {
            base.OnAttachedTo(bindable);

            _rootView = bindable;
        }

        public async Task ViewStart(int delay = 350)
        {
            TopIcon.Opacity = 0;
            TopIcon.Source = _heroImageDefault;
            TopIcon.TranslationY -= TopIcon.HeightRequest * 0.25;
            await Task.Delay(delay);

            await Task.WhenAll(
                DropInCard(1250)
            );
        }

        public async Task SwitchStateToDefault()
        {
            await FlipImageTo(_heroImageDefault);
        }

        public async Task SwitchStateToBusy()
        {
            if (TopIcon.Source != _heroImageDefault)
                await FlipImageTo(_heroImageDefault);
        }

        public async Task SwitchStateToSuccess()
        {
            await Task.WhenAll(
                FlipImageTo(_heroImageSuccess),
                FillScreen()
            );

            await FlipImageTo(_heroImageSuccess);
        }

        private async Task FlipImageTo(ImageSource newImage)
        {
            if (TopIcon.Source == newImage)
                return;

            await Task.WhenAll(
                TopIcon.RotateYTo(90, 250, Easing.SinIn),
                TopIcon.FadeTo(0, 150)
            );

            TopIcon.Source = newImage;
            TopIcon.RotationY = -90;

            await Task.WhenAll(
                TopIcon.RotateYTo(0, 250, Easing.SinIn),
                TopIcon.FadeTo(100, 150)
            );
        }

        private async Task DropInCard(uint duration)
        {
            TopIcon.Rotation = 100;
            TopIcon.RotationX = 120;
            TopIcon.Opacity = 0;

            var durationFade = (uint)(duration * 0.75);
            var durationXRotation = (uint)(duration * 0.7);

            await Task.WhenAll(
                TopIcon.FadeTo(1, durationFade),
                TopIcon.RotateTo(0, duration, Easing.SpringOut),
                TopIcon.RotateXTo(0, durationXRotation, Easing.SpringOut)
                );
        }
        
        private async Task FillScreen()
        {
            var width = Application.Current.MainPage.Width;
            var height = Application.Current.MainPage.Height;

            await Task.WhenAll(
                TopIcon.TranslateTo(0, height / 8, 500),
                TopIcon.ScaleTo(1.5, 500),
                TitleLabel.FadeTo(0, 300),
                _rootView.LayoutTo(Rectangle.FromLTRB(0, 0, width, height), 800, Easing.SinIn)
            );
        }
    }
}
