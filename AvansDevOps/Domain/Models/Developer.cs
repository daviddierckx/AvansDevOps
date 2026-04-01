namespace AvansDevOps.Domain.Models
{
    public class Developer
    {
        public string Name { get; set; }
        public Role Role { get; set; }

        public Developer(string name, Role role)
        {
            Name = name;
            Role = role;
        }
    }
}