namespace TvMazeScraper.Core.Model
{
    public class ShowToActor
    {
        public long ShowId { get; private set; }
        public Show Show { get; private set; }
        public long ActorId { get; private set; }
        public Actor Actor { get; private set; }

        public ShowToActor() { }

        public ShowToActor(long showId, long actorId)
        {
            ShowId = showId;
            ActorId = actorId;
        }

        public ShowToActor(Show show, Actor actor)
        {
            Show = show;
            ShowId = show?.Id ?? 0;
            Actor = actor;
            ActorId = actor?.Id ?? 0;
        }
    }
}