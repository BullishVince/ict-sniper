using System.Text.Json;
public class DebugProps
{
    public int NumOfElements { get; set; }
    public string CurrVersion { get; set; }
    public double ChartWidth { get; set; }
    public double ChartHeight { get; set; }
    public override string ToString() => JsonSerializer.Serialize(this);
}