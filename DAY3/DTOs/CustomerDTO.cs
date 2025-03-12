namespace DAY3.Dtos
{
    public class CustomerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<OrderDTO> Orders { get; set; } = new List<OrderDTO>();
    }
}
