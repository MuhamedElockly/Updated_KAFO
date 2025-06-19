namespace KAFO.Domain.Products
{
    public class Category : Base, ISoftDelete
    {
        public int Id { private set; get; }
        public string Name { set; get; }
        public string? Description { set; get; }
        public List<Product>? Products { set; get; }
        public bool IsDeleted { get; set; } = false;

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
