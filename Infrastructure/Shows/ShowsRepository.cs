using Application.Errors;
using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Shows
{
    public class ShowsRepository : IShowsRepository
    {
        private readonly DataContext _db;

        public ShowsRepository(DataContext context)
        {
            _db = context;
        }

        public async Task<bool> Exist(int showId)
        {
            var result = await _db.Shows.AnyAsync(x => x.idSite == showId);
            return result;
        }


        public async Task CreateMultiple(List<Show> entities) 
        {
            await _db.AddRangeAsync(entities);
            var status = await Save();

            if (!status)
                throw new RestException(HttpStatusCode.InternalServerError, new { Show = "Could not save" });
        }

        public async Task Create(Show entity)
        {
            await _db.AddAsync(entity);
            var status = await Save();

            if (!status)
                throw new RestException(HttpStatusCode.InternalServerError, new { Show = "Could not save" });
        }

        public async Task Delete(Show entity)
        {
            _db.Shows.Remove(entity);
            var status = await Save();
            if (!status)
                throw new RestException(HttpStatusCode.InternalServerError, new { Show = "Could not delete" });
        }

        public async Task<ShowsPresentation> FindAll(int? limit, int? offset, string sortedRating,
            string sortedChannel, string sortedGenere, string keywords, string language, 
            string genere, string channel, string day, string time)
        {
            var queryable = _db.Shows.Include(x=>x.genresObject)
                .Include(x=>x.schedule).ThenInclude(x=>x.daysOfWeek).Include(x=>x.image)
                .Include(x=>x.network)
                .Where(x=> String.IsNullOrEmpty(keywords) ? true : x.name.Contains(keywords) || x.summary.Contains(keywords))
                .Where(x=> String.IsNullOrEmpty(language) ? true : x.language.Contains(language))
                .Where(x=> String.IsNullOrEmpty(genere) ? true : x.genresObject.Any(y=>y.genereName.Contains(genere)))
                .Where(x=> String.IsNullOrEmpty(channel) ? true : x.network.name.Contains(channel))
                .Where(x=> String.IsNullOrEmpty(day) ? true : x.schedule.days.Any(x=>x.Contains(day)))
                .Where(x=> String.IsNullOrEmpty(time) ? true : x.schedule.time.Contains(time))
                .AsQueryable();

            if (sortedRating=="DES")
            {
                queryable = queryable.OrderByDescending(x=>x.ratingValue);
            }
            else if (sortedRating == "ASC")
            {
                queryable = queryable.OrderBy(x => x.ratingValue);
            }
            else if (sortedChannel == "DES")
            {
                queryable = queryable.OrderByDescending(x => x.network.name);
            }
            else if (sortedChannel == "ASC")
            {
                queryable = queryable.OrderBy(x => x.network.name);
            }
            else if (sortedGenere == "DES")
            {
                queryable = queryable.OrderByDescending(x => String.Join(", ", x.genres.ToArray()));
            }
            else if (sortedGenere == "ASC")
            {
                queryable = queryable.OrderBy(x => String.Join(", ", x.genres.ToArray()));
            }

            var showsList = await queryable
                .Skip(offset ?? 0)
                .Take(limit ?? 30).ToListAsync();

            ShowsPresentation showPresentation = new ShowsPresentation
            {
                FilteredShows = showsList,
                QueryCount = queryable.Count()
            };

            return showPresentation;
;
        }

        public Show FindById(int id)
        {
            try
            {
                Show show = GetShowByAPI(id);
                show.cast = GetCastByAPI(id);
                show.seasons = GetSeasonsByAPI(id);
                return show;
            }
            catch (Exception)
            {
                throw new RestException(HttpStatusCode.InternalServerError, new { Show = "Could not bring show" });
            }
        }

        private List<Season> GetSeasonsByAPI(int id)
        {
            dynamic response;
            string responseBody;
            responseBody = "";

            string requestURL = "http://api.tvmaze.com/shows/" + id + "/episodes";
            using (HttpClient client = new HttpClient())
            {
                response = client.GetAsync(requestURL);
                responseBody = response.Result.Content.ReadAsStringAsync().Result;


                if (response.Result.StatusCode.ToString() == "OK")
                {
                    List<Episode> episodes = JsonConvert.DeserializeObject<List<Episode>>(responseBody);

                    var query = episodes.GroupBy(x => x.season);

                    List<Season> seasons = new List<Season>();

                    foreach (var item in query)
                    {
                        Season season = new Season
                        {
                            seasonNumber = item.Key,
                            Episodes = item.ToList()
                        };
                        seasons.Add(season);
                    }
                    return seasons;
                }
                //Handle Rate Limit
                else
                {
                    throw new RestException(HttpStatusCode.InternalServerError, new { Show = "Could not bring episodes" });
                }
            }
        }

        private List<Actor> GetCastByAPI(int id)
        {
            dynamic response;
            string responseBody;
            responseBody = "";

            string requestURL = "http://api.tvmaze.com/shows/" + id + "/cast";
            using (HttpClient client = new HttpClient())
            {
                response = client.GetAsync(requestURL);
                responseBody = response.Result.Content.ReadAsStringAsync().Result;
                

                if (response.Result.StatusCode.ToString() == "OK")
                {
                    List<Actor> actors = JsonConvert.DeserializeObject<List<Actor>>(responseBody);
                    return actors;
                }
                //Handle Rate Limit
                else
                {
                    throw new RestException(HttpStatusCode.InternalServerError, new { Show = "Could not bring show" });
                }
            }
        }

        private Show GetShowByAPI(int id)
        {
            dynamic response;
            string responseBody;
            responseBody = "";

            string requestURL = "http://api.tvmaze.com/shows/" + id;
            using (HttpClient client = new HttpClient())
            {
                response = client.GetAsync(requestURL);
                responseBody = response.Result.Content.ReadAsStringAsync().Result;
                Show show = JsonConvert.DeserializeObject<Show>(responseBody);

                if (response.Result.StatusCode.ToString() == "OK")
                {
                    show.ratingValue = show.rating.average;
                    if (show.network != null)
                    {
                        show.network.id = 0;
                    }
                    show.idSite = show.id;
                    show.id = 0;

                    show.genresObject = show.genres.Select(x => new Genere
                    {
                        genereName = x
                    }).ToList();

                    show.schedule.daysOfWeek = show.schedule.days.Select(x => new DayObject
                    {
                        day = x
                    }).ToList();
                    
                }
                //Handle Rate Limit
                else
                {
                    throw new RestException(HttpStatusCode.InternalServerError, new { Show = "Could not bring show" });
                }

                return show;
            }
        }

        public async Task<int> GetHighestId()
        {
            try
            {
                var showId = await _db.Shows.MaxAsync(x => x.idSite);
                return showId;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public Task<ICollection<Show>> FindAll(int? limit, int? offset, bool sortedRating, bool sortedChannel, bool sortedGenere, string keywords, string language, string genere, string channel, string day, string time)
        {
            throw new NotImplementedException();
        }
    }
}
