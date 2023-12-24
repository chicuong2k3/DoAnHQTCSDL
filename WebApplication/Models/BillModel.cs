namespace WebApplication.Models
{
    public class MyMedicine
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    public class BillModel
    {
        public string NameService { get; set; }
        public decimal SerVicePrice { set; get; }
        public List<MyMedicine> medicines { get; set; }
        public decimal Total {  get; set; }
    }
}
