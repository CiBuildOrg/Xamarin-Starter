using System;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Util;
using Android.Views;
using Android.Views.Animations;

namespace App.Template.XForms.Android.Infrastructure.Interaction
{
	[Register("scl.alertview.SuccessTickView")]
	public class SuccessTickView : View
	{
		private static float _mDensity = -1;
		private Paint _mPaint;
		private float ConstRadius => Dip2Px(1.2f);
		private float ConstRectWeight => Dip2Px(3);
		private float ConstLeftRectW => Dip2Px(15);
		private float ConstRightRectW => Dip2Px(25);
		private float MinLeftRectW => Dip2Px(3.3f);
		private float MaxRightRectW => ConstRightRectW + Dip2Px(6.7f);

		private float _mMaxLeftRectWidth;
		private float _mLeftRectWidth;
		private float _mRightRectWidth;
		private bool _mLeftRectGrowMode;

		public SuccessTickView(Context context) : base(context)
		{
			Init();
		}

		public SuccessTickView(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			Init();
		}

		private void Init()
		{
			_mPaint = new Paint();
			var color = ContextCompat.GetColor(Context, Resource.Color.success_stroke_color);
			_mPaint.Color = Color.Rgb(Color.GetRedComponent(color), Color.GetGreenComponent(color), Color.GetBlueComponent(color));
			_mLeftRectWidth = ConstLeftRectW;
			_mRightRectWidth = ConstRightRectW;
			_mLeftRectGrowMode = false;
		}

		protected override void OnDraw(Canvas canvas)
		{
			base.OnDraw(canvas);
			float totalW = Width;
			float totalH = Height;
			// rotate canvas first
			canvas.Rotate(45, totalW / 2, totalH / 2);

			totalW /= 1.2f;
			totalH /= 1.4f;
			_mMaxLeftRectWidth = (totalW + ConstLeftRectW) / 2 + ConstRectWeight - 1;

			RectF leftRect = new RectF();
			if (_mLeftRectGrowMode)
			{
				leftRect.Left = 0;
				leftRect.Right = leftRect.Left + _mLeftRectWidth;
				leftRect.Top = (totalH + ConstRightRectW) / 2;
				leftRect.Bottom = leftRect.Top + ConstRectWeight;
			}
			else
			{
				leftRect.Right = (totalW + ConstLeftRectW) / 2 + ConstRectWeight - 1;
				leftRect.Left = leftRect.Right - _mLeftRectWidth;
				leftRect.Top = (totalH + ConstRightRectW) / 2;
				leftRect.Bottom = leftRect.Top + ConstRectWeight;
			}

			canvas.DrawRoundRect(leftRect, ConstRadius, ConstRadius, _mPaint);

		    RectF rightRect = new RectF
		    {
		        Bottom = (totalH + ConstRightRectW) / 2 + ConstRectWeight - 1,
		        Left = (totalW + ConstLeftRectW) / 2
		    };
		    rightRect.Right = rightRect.Left + ConstRectWeight;
			rightRect.Top = rightRect.Bottom - _mRightRectWidth;
			canvas.DrawRoundRect(rightRect, ConstRadius, ConstRadius, _mPaint);
		}

		public float Dip2Px(float dpValue)
		{
			if (Math.Abs(_mDensity - -1) < 0.01)
			{
				_mDensity = Resources.DisplayMetrics.Density;
			}
			return dpValue * _mDensity + 0.5f;
		}

		protected class SelfAnimation : Animation
		{
			private readonly SuccessTickView _view;

			public SelfAnimation(SuccessTickView view)
			{
				_view = view;
			}

			protected override void ApplyTransformation(float interpolatedTime, Transformation t)
			{
				base.ApplyTransformation(interpolatedTime, t);
				_view._mLeftRectWidth = 0;
				_view._mRightRectWidth = 0;
				if (0.54 < interpolatedTime && 0.7 >= interpolatedTime)
				{  // grow left and right rect to right
					_view._mLeftRectGrowMode = true;
					_view._mLeftRectWidth = _view._mMaxLeftRectWidth * ((interpolatedTime - 0.54f) / 0.16f);
					if (0.65 < interpolatedTime)
					{
						_view._mRightRectWidth = _view.MaxRightRectW * ((interpolatedTime - 0.65f) / 0.19f);
					}
					_view.Invalidate();
				}
				else if (0.7 < interpolatedTime && 0.84 >= interpolatedTime)
				{ // shorten left rect from right, still grow right rect
					_view._mLeftRectGrowMode = false;
					_view._mLeftRectWidth = _view._mMaxLeftRectWidth * (1 - ((interpolatedTime - 0.7f) / 0.14f));
					_view._mLeftRectWidth = _view._mLeftRectWidth < _view.MinLeftRectW ? _view.MinLeftRectW : _view._mLeftRectWidth;
					_view._mRightRectWidth = _view.MaxRightRectW * ((interpolatedTime - 0.65f) / 0.19f);
					_view.Invalidate();
				}
				else if (0.84 < interpolatedTime && 1 >= interpolatedTime)
				{ // restore left rect width, shorten right rect to const
					_view._mLeftRectGrowMode = false;
					_view._mLeftRectWidth = _view.MinLeftRectW + (_view.ConstLeftRectW - _view.MinLeftRectW) * ((interpolatedTime - 0.84f) / 0.16f);
					_view._mRightRectWidth = _view.ConstRightRectW + (_view.MaxRightRectW - _view.ConstRightRectW) * (1 - ((interpolatedTime - 0.84f) / 0.16f));
					_view.Invalidate();
				}
			}
		}

		public void StartTickAnim()
		{
			// hide tick
			Invalidate();
            Animation tickAnim = new SelfAnimation(this)
            {
                Duration = 750,
                StartOffset = 100
            };
            StartAnimation(tickAnim);
		}
    }
}