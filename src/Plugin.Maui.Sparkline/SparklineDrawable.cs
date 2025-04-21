using System;
using Microsoft.Maui.Graphics;

namespace Plugin.Maui.Sparkline;

public class SparklineDrawable : IDrawable
{
    private IList<double>? CurrentValues { get; set; }
    private IList<double>? _previousValues;
    private float _animationProgress = 1f;
    public double? Threshold { get; set; }
    public Color ThresholdLineColor { get; set; } = Colors.Gray;
    public Color LineColor { get; set; } = Colors.Black;

    public void AnimateTo(IList<double> newValues)
    {
        _previousValues = CurrentValues?.ToList();
        CurrentValues = newValues;

        _animationProgress = 0f;
    }

    public void UpdateProgress(float progress)
    {
        _animationProgress = progress;
    }
    
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        if (CurrentValues == null || CurrentValues.Count < 2)
            return;

        var valuesToDraw = InterpolateValues(_previousValues, CurrentValues, _animationProgress);

        var width = dirtyRect.Width;
        var height = dirtyRect.Height;
        var min = valuesToDraw.Min();
        var max = valuesToDraw.Max();
        var range = max - min;
        range = range == 0 ? 1 : range;

        var stepX = width / (valuesToDraw.Count - 1);

        var points = valuesToDraw
            .Select((v, i) =>
            {
                var x = i * stepX;
                var y = (float)(height - ((v - min) / range) * height);
                return new PointF(x, y);
            }).ToList();

        var path = new PathF();
        path.MoveTo(points[0]);

        for (var i = 1; i < points.Count - 1; i++)
        {
            var curr = points[i];
            var next = points[i + 1];

            var controlX = (curr.X + next.X) / 2;
            var controlY = (curr.Y + next.Y) / 2;

            path.QuadTo(curr.X, curr.Y, controlX, controlY);
        }

        path.LineTo(points.Last().X, points.Last().Y);

        canvas.StrokeColor = LineColor;
        canvas.StrokeSize = 1;
        canvas.DrawPath(path);

        // Draw threshold line if applicable
        if (Threshold.HasValue && Threshold.Value >= min && Threshold.Value <= max)
        {
            var y = (float)(height - ((Threshold.Value - min) / range) * height);
            canvas.StrokeColor = ThresholdLineColor;
            canvas.StrokeSize = 1;
            canvas.StrokeDashPattern = [4, 4];
            canvas.DrawLine(0, y, width, y);
            canvas.StrokeDashPattern = null;
        }
    }


    private IList<double> InterpolateValues(IList<double>? from, IList<double> to, float progress)
    {
        var result = new List<double>();

        for (var i = 0; i < to.Count; i++)
        {
            var start = (from != null && i < from.Count) ? from[i] : to[i];
            var end = to[i];
            result.Add(start + (end - start) * progress);
        }

        return result;
    }
}
