namespace DeliveryFinder;

public class DTO
{
    public string OrderID { get; set; }
    public decimal Weight { get; set; }
    public string OrderDistrict { get; set; }
    public DateTime DateTime { get; set; }

    public override string ToString()
    {
        return $"{Weight}, {OrderID}, {OrderDistrict}, {DateTime}";
    }
}