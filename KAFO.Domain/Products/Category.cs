namespace KAFO.Domain.Products
{
    public class Category : Base
    {
        public int Id { private set; get; }
        public string Name { set; get; }
        public string? Description { set; get; }
        public List<Product>? Products { set; get; }
        public Category(string name, string? description = null)
        {
            Name = name;
            Description = description;
        }
        private Category()
        {

        }
    }
}
