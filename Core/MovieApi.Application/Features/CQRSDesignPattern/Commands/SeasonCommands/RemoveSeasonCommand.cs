namespace MovieApi.Application.Features.CQRSDesignPattern.Commands.SeasonCommands
{
    public class RemoveSeasonCommand
    {
        public int SeasonID { get; set; }

        public RemoveSeasonCommand(int id)
        {
            SeasonID = id;
        }
    }
}
