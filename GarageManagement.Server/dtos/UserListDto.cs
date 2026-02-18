namespace GarageManagement.Server.dtos
{
    public class UserListDto
    {
        public long Id { get; set; }
        public string Username { get; set; } = string.Empty;    
        public string Email { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public long RoleId { get; set; }
        public bool? IsActive { get; set; }  
    }

        public class UpdateUserRoleDto
        {
            public long UserId { get; set; }
            public long RoleId { get; set; }
        }
    

}
