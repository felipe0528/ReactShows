using Application.Interfaces;
using Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace Infrastructure.Shows
{
    public class ShowScraper
    {
        public static async System.Threading.Tasks.Task ScrapAsync(IShowsRepository showRepo)
        {
            int id = 0;
            //count of retry for not found
            int count = 0;
            string responseBody;
            dynamic response;
            var highestId = await showRepo.GetHighestId();
            if (highestId > 0)
            {
                double divition = highestId / 250;
                id = (int)Math.Floor(divition);
            }
            do
            {
                responseBody = "";
                
                string requestURL = "http://api.tvmaze.com/shows?page=" + id ;
                using (HttpClient client = new HttpClient())
                {
                    response = client.GetAsync(requestURL);
                    responseBody = response.Result.Content.ReadAsStringAsync().Result;
                    List<Show> shows = JsonConvert.DeserializeObject<List<Show>>(responseBody);

                    if (response.Result.StatusCode.ToString() == "OK")
                    {
                        foreach (var item in shows)
                        {
                            item.ratingValue = item.rating.average;
                            if (item.network!=null)
                            {
                                item.network.id = 0;
                            }
                            item.idSite = item.id;
                            item.id = 0;

                            item.genresObject = item.genres.Select(x => new Genere
                            {
                                genereName = x
                            }).ToList();

                            item.schedule.daysOfWeek = item.schedule.days.Select(x => new DayObject
                            {
                                day = x
                            }).ToList();

                            try
                            {
                                if (!await showRepo.Exist(item.idSite))
                                {
                                    await showRepo.Create(item);
                                }
                            }
                            catch (Exception e)
                            {
                                
                            }
                        }
                    }
                    //Handle Rate Limit
                    else if (response.Result.StatusCode.ToString() == "TooManyRequests")
                    {
                        Thread.Sleep(12000);
                        continue;
                    }
                    else if (response.Result.StatusCode.ToString() == "NotFound")
                    {
                        count++;
                        continue;
                    }
                    else
                    {
                        //Send Exception to Exception Logger
                    }

                    id++;
                }
            }

            while (count <= 3);

        }
    }
}
