using Application.Errors;
using Application.Interfaces;
using Application.Show.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Show
{
    public class Details
    {
        public class Query : IRequest<ShowDto>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, ShowDto>
        {
            private readonly IShowsRepository _showRepo;
            public Handler(IShowsRepository showRepo)
            {
                _showRepo = showRepo;
            }

            public Task<ShowDto> Handle(Query request, CancellationToken cancellationToken)
            {
                return Task.Run(()=>
                        {
                            var show = _showRepo.FindById(request.Id);

                            if (show == null)
                                throw new RestException(HttpStatusCode.NotFound, new { Show = "Not found" });

                            var showDto = new ShowDto
                            {
                                Id = show.id,
                                IdAPI = show.idSite,
                                Language = show.language,
                                Name = show.name,
                                PhotoURL = show.image != null ? show.image.medium : null,
                                Channel = show.network != null ? show.network.name : null,
                                Summary = show.summary,
                                Rating = show.ratingValue,
                                Genere = show.genresObject.Count > 0 ? String.Join(", ", show.genresObject) : null,
                                Time = show.schedule != null ? show.schedule.time : null,
                                Days = show.schedule != null ? String.Join(", ", show.schedule.daysOfWeek) : null,
                                Seasons = show.seasons,
                                Cast = show.cast
                            };

                            return showDto;
                        }
                    );
            }
        }
    }
}
