using Application.Interfaces;
using Application.Show.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Show
{
    public class List
    {
        public class ShowsEnvelope
        {
            public List<ShowDto> Shows { get; set; }
            public int ShowsCount { get; set; }
        }
        public class Query : IRequest<ShowsEnvelope>
        {
            public Query(int? limit, int? offset, string sortedRating, string sortedChannel, string sortedGenere,
                string keywords, string language, string genere, string channel, string day, string time)
            {
                Limit = limit;
                Offset = offset;
                SortedRating=sortedRating;
                SortedChannel=sortedChannel;
                SortedGenere=sortedGenere;
                Keywords= keywords;
                Language=language;
                Genere=genere;
                Channel=channel;
                Day=day;
                Time=time;
            }
            public int? Limit { get; set; }
            public int? Offset { get; set; }
            public string SortedRating { get; set; }
            public string SortedChannel { get; set; }
            public string SortedGenere { get; set; }
            public string Keywords { get; set; }
            public string Language { get; set; }
            public string Genere { get; set; }
            public string Channel { get; set; }
            public string Day { get; set; }
            public string Time { get; set; }
        }

        public class Handler : IRequestHandler<Query, ShowsEnvelope>
        {
            private readonly IShowsRepository _showRepo;
            public Handler(IShowsRepository showRepo)
            {
                _showRepo = showRepo;
            }

            public async Task<ShowsEnvelope> Handle(Query request, CancellationToken cancellationToken)
            {

                var showsPresentation =await _showRepo.FindAll(request.Limit, request.Offset, request.SortedRating, request.SortedChannel,
                    request.SortedGenere, request.Keywords, request.Language, request.Genere,
                    request.Channel, request.Day, request.Time);

                var showsDTO = showsPresentation.FilteredShows.Select(x => new ShowDto
                {
                    Id = x.id,
                    IdAPI = x.idSite,
                    Name = x.name,
                    PhotoURL = x.image != null ? x.image.medium : null,
                    Chanel = x.network != null ? x.network.name : null,
                    Summary = x.summary,
                    Rating = x.ratingValue,
                    Genere = x.genresObject.Count > 0 ? String.Join(", ", x.genresObject.Select(x=>x.genereName)) : null,
                    Time = x.schedule != null ? x.schedule.time : null,
                    Days = x.schedule != null ?(x.schedule.daysOfWeek!=null? String.Join(", ", x.schedule.daysOfWeek.Select(x => x.day)) : null ): null
                }).ToList();

                return new ShowsEnvelope
                {
                    Shows = showsDTO,
                    ShowsCount = showsPresentation.QueryCount
                };
            }
        }
    }
}
