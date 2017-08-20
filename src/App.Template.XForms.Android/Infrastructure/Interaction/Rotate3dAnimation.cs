using Android.Graphics;
using Android.Views.Animations;

namespace App.Template.XForms.Android.Infrastructure.Interaction
{
	public class Rotate3DAnimation : Animation
	{
		private readonly Dimension _mPivotXType = Dimension.Absolute;
		private readonly Dimension _mPivotYType = Dimension.Absolute;
		private readonly float _mPivotXValue;
		private readonly float _mPivotYValue;

		private readonly float _mFromDegrees;
		private readonly float _mToDegrees;
		private float _mPivotX;
		private float _mPivotY;
		private Camera _mCamera;
		private readonly int _mRollType;

		public const int RollByX = 0;
		public const int RollByY = 1;
		public const int RollByZ = 2;

		public Rotate3DAnimation(int rollType, float fromDegrees, float toDegrees)
		{
			_mRollType = rollType;
			_mFromDegrees = fromDegrees;
			_mToDegrees = toDegrees;
			_mPivotX = 0.0f;
			_mPivotY = 0.0f;
		}

		public Rotate3DAnimation(int rollType, float fromDegrees, float toDegrees, float pivotX, float pivotY)
		{
			_mRollType = rollType;
			_mFromDegrees = fromDegrees;
			_mToDegrees = toDegrees;

			_mPivotXType = Dimension.Absolute;
			_mPivotYType = Dimension.Absolute;
			_mPivotXValue = pivotX;
			_mPivotYValue = pivotY;
			InitializePivotPoint();
		}

		public Rotate3DAnimation(int rollType, float fromDegrees, float toDegrees, Dimension pivotXType, float pivotXValue, Dimension pivotYType, float pivotYValue)
		{
			_mRollType = rollType;
			_mFromDegrees = fromDegrees;
			_mToDegrees = toDegrees;

			_mPivotXValue = pivotXValue;
			_mPivotXType = pivotXType;
			_mPivotYValue = pivotYValue;
			_mPivotYType = pivotYType;
			InitializePivotPoint();
		}

		private void InitializePivotPoint()
		{
			if (_mPivotXType == Dimension.Absolute)
			{
				_mPivotX = _mPivotXValue;
			}
			if (_mPivotYType == Dimension.Absolute)
			{
				_mPivotY = _mPivotYValue;
			}
		}

		public override void Initialize(int width, int height, int parentWidth, int parentHeight)
		{
			base.Initialize(width, height, parentWidth, parentHeight);
			_mCamera = new Camera();
			_mPivotX = ResolveSize(_mPivotXType, _mPivotXValue, width, parentWidth);
			_mPivotY = ResolveSize(_mPivotYType, _mPivotYValue, height, parentHeight);
		}

		protected override void ApplyTransformation(float interpolatedTime, Transformation t)
		{
			base.ApplyTransformation(interpolatedTime, t);
			var fromDegrees = _mFromDegrees;
			float degrees = fromDegrees + ((_mToDegrees - fromDegrees) * interpolatedTime);
			var matrix = t.Matrix;

			_mCamera.Save();
			switch (_mRollType)
			{
				case RollByX:
					_mCamera.RotateX(degrees);
					break;
				case RollByY:
					_mCamera.RotateY(degrees);
					break;
				case RollByZ:
					_mCamera.RotateZ(degrees);
					break;
			}
			_mCamera.GetMatrix(matrix);
			_mCamera.Restore();

			matrix.PreTranslate(-_mPivotX, -_mPivotY);
			matrix.PostTranslate(_mPivotX, _mPivotY);
		}
	}
}