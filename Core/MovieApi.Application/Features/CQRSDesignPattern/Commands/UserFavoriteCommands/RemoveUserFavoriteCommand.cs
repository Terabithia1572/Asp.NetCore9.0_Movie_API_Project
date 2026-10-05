namespace MovieApi.Application.Features.CQRSDesignPattern.Commands.UserFavoriteCommands
{
    public class RemoveUserFavoriteCommand
    {
        public int UserFavoriteID { get; set; }

        public RemoveUserFavoriteCommand(int id)
        {
            UserFavoriteID = id;
        }
    }
}
