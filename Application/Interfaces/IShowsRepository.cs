using Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IShowsRepository
    {
        Task CreateMultiple(List<Domain.Show> entities);
        Task Create(Domain.Show entity);
        Task Delete(Domain.Show entity);
        Task<ShowsPresentation> FindAll(int? limit, int? offset, string sortedRating, string sortedChannel, string sortedGenere, string keywords, string language, string genere, string channel, string day, string time);
        Domain.Show FindById(int id);
        Task<bool> Save();
        Task<bool> Exist(int showId);
        Task<int> GetHighestId();
    }
}
