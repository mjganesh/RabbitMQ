namespace RabbitMQ
{
    internal class Book
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string? BookName { get; set; }
        public string? Author { get; set; }
        public int Year { get; set; }
        public string? Publisher { get; set; }
        public decimal? Price { get; set; }
        public int ? SiNo { get; set; }
    }
}
