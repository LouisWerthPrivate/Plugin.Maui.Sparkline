namespace Plugin.Maui.Sparkline;


public partial class SparklineView : ContentView
{
    private readonly SparklineDrawable _drawable = new();

    public static readonly BindableProperty ValuesProperty = BindableProperty.Create(
        nameof(Values), typeof(IList<double>), typeof(SparklineView), propertyChanged: OnDrawablePropertyChanged);

    public static readonly BindableProperty LineColorProperty = BindableProperty.Create(
        nameof(LineColor), typeof(Color), typeof(SparklineView), Colors.Black, propertyChanged: OnDrawablePropertyChanged);

    public static readonly BindableProperty ThresholdProperty = BindableProperty.Create(
        nameof(Threshold), typeof(double?), typeof(SparklineView), null, propertyChanged: OnDrawablePropertyChanged);

    public static readonly BindableProperty ThresholdLineColorProperty = BindableProperty.Create(
        nameof(ThresholdLineColor), typeof(Color), typeof(SparklineView), Colors.Gray, propertyChanged: OnDrawablePropertyChanged);

    public IList<double> Values
    {
        get => (IList<double>)GetValue(ValuesProperty);
        set => SetValue(ValuesProperty, value);
    }

    public Color LineColor
    {
        get => (Color)GetValue(LineColorProperty);
        set => SetValue(LineColorProperty, value);
    }


    public double? Threshold
    {
        get => (double?)GetValue(ThresholdProperty);
        set => SetValue(ThresholdProperty, value);
    }

    public Color ThresholdLineColor
    {
        get => (Color)GetValue(ThresholdLineColorProperty);
        set => SetValue(ThresholdLineColorProperty, value);
    }

    public SparklineView()
    {
        InitializeComponent();
        Graphics.Drawable = _drawable;
    }

    private static void OnDrawablePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is SparklineView view)
        {
            view._drawable.LineColor = view.LineColor;
            view._drawable.Threshold = view.Threshold;
            view._drawable.ThresholdLineColor = view.ThresholdLineColor;

            if (view.Values != null)
            {
                view._drawable.AnimateTo(view.Values);
                view.AnimateProgress();
            }
        }
    }

    private void AnimateProgress()
    {
        var animation = new Animation(v =>
        {
            _drawable.UpdateProgress((float)v);
            Graphics.Invalidate();
        }, 0, 1);

        animation.Commit(this, "SparklineAnim", length: 400, easing: Easing.CubicInOut);
    }
}