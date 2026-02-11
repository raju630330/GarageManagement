namespace GarageManagement.Server.RepoInterfaces
{
    public interface IHelperRepository
    {
        long GetUserId();
        string GetUsername();
        string GetEmail();
        string GetRole();
    }

}
