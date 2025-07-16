namespace JourneyMate.Application.ViewModel
{
    public class RoleVm
    {
        public string Id { get; set; }
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }   // name in identity role  in "Name" not "Role name" 
        public RoleVm()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
