namespace Vyapari.Data;

public class BlackListRoute
{
    public int Id { get; set; }
    public string Route { get; set; }

    public ICollection<Role> AllowedRoles { get; set; }


}

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<BlackListRoute> BlackListRoutes { get; set; }
}

