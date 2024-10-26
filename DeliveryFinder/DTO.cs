namespace DeliveryFinder;

public class DTO
{
    public int Id { get; set; }
    public string OrderID { get; set; }
    public decimal Weight { get; set; }
    public string OrderDistrict { get; set; }
    public DateTime DateTime { get; set; }

    public override string ToString()
    {
        return $"{Id}, {Weight}, {OrderID}, {OrderDistrict}, {DateTime}";
    }
}