namespace TvMazeScraper.Core.DataAccess
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork CreateUnitOfWork();
    }
}
