namespace WebApplication1.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Title { get; private set; }
        public decimal Price { get; private set; }
        public string Description { get; private set; }
        public string Category { get; private set; }
        public string Image { get; private set; }
        //public int Rating { get; set; } // configurar objeto

        public Product() { }

        public Product(string title, decimal price, string description, string category, string image)
        {
            Title = title;
            Price = price;
            Description = description;
            Category = category;
            Image = image;
            Validate();
        }

        public void Update(string title, decimal price, string description, string category, string image)
        {
            Title = title;
            Price = price;
            Description = description;
            Category = category;
            Image = image;
            Validate();
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Title))
                throw new ArgumentException("Title cannot be empty.");

            if (string.IsNullOrWhiteSpace(Category))
                throw new ArgumentException("Category cannot be empty.");

            if (string.IsNullOrWhiteSpace(Description))
                throw new ArgumentException("Description cannot be empty.");

            if (string.IsNullOrWhiteSpace(Image))
                throw new ArgumentException("Image URL cannot be empty.");

            if (Price <= 0)
                throw new ArgumentException("Price must be greater than zero.");
        }
    }

}
