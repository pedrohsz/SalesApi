namespace SalesApi.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Title { get; private set; }
        public decimal Price { get; private set; }
        public string Description { get; private set; }
        public string Category { get; private set; }
        public string Image { get; private set; }

        public Product() { }

        public Product(string title, decimal price, string description, string category, string image)
        {
            Title = ValidateTitle(title);
            Price = ValidatePrice(price);
            Description = ValidateDescription(description);
            Category = category;
            Image = image;
        }

        public Product(Guid id, string title, decimal price, string description, string category, string image)
        {
            Id = id;
            Title = ValidateTitle(title);
            Price = ValidatePrice(price);
            Description = ValidateDescription(description);
            Category = category;
            Image = image;
        }

        public void UpdateTitle(string title)
        {
            Title = ValidateTitle(title);
        }

        public void UpdatePrice(decimal price)
        {
            Price = ValidatePrice(price);
        }

        public void UpdateDescription(string description)
        {
            Description = ValidateDescription(description);
        }

        public void UpdateCategory(string category)
        {
            Category = category;
        }

        public void UpdateImage(string image)
        {
            Image = image;
        }

        public void UpdateProduct(string title, decimal price, string description, string category, string image)
        {
            Title = ValidateTitle(title);
            Price = ValidatePrice(price);
            Description = ValidateDescription(description);
            Category = category;
            Image = image;
        }

        private string ValidateTitle(string title)
        {
            return !string.IsNullOrWhiteSpace(title) ? title : throw new ArgumentException("Title cannot be empty.");
        }

        private decimal ValidatePrice(decimal price)
        {
            return price >= 0 ? price : throw new ArgumentException("Price cannot be negative.");
        }

        private string ValidateDescription(string description)
        {
            return !string.IsNullOrWhiteSpace(description) ? description : throw new ArgumentException("Description cannot be empty.");
        }
    }
}
