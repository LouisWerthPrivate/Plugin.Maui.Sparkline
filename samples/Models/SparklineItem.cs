namespace MauiSparkline.Models;

public class SparklineItem
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public double Threshold { get; set; }
    public List<double> Sparkies { get; set; }
    
    public string LineColor => Sparkies.Last() < Threshold ? "Red" : "Green";
    
}