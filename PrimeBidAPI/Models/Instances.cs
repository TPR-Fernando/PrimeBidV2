namespace PrimeBidAPI.Models
{

    class UserZLogged
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    /// <summary>
    /// Store Shared Instances
    /// </summary>
    public class Instances
    {
        internal static ISession CurrentSession { get; set; }
        internal static UserZLogged CurrentUser { get; set; }
    }
}
