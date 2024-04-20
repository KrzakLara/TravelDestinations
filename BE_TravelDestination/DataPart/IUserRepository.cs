namespace BE_TravelDestination.DataPart
{
    public interface IUserRepository
    {
        User FindByUsername(string username);
    }

}
