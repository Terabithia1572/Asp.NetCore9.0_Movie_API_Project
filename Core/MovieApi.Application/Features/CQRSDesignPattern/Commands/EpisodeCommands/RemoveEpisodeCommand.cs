namespace MovieApi.Application.Features.CQRSDesignPattern.Commands.EpisodeCommands
{
    public class RemoveEpisodeCommand
    {
        public int EpisodeID { get; set; }

        public RemoveEpisodeCommand(int id)
        {
            EpisodeID = id;
        }
    }
}
