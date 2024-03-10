namespace Vyapari.Infra;
public class ProductRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int QuantityAvailable { get; set; }
    public string ImageUrl { get; set; }
}
