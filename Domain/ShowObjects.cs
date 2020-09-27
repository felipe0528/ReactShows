using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain
{
    public class Schedule
    {
        public int Id { get; set; }
        public string time { get; set; }
        [NotMapped]
        public List<string> days { get; set; }
        public List<DayObject> daysOfWeek { get; set; }
    }

    public class DayObject
    {
        public int Id { get; set; }
        public string day { get; set; }
    }

    public class Rating
    {
        public double? average { get; set; }
    }

    public class Country
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string timezone { get; set; }
    }

    public class Network
    {
        public int id { get; set; }
        public string name { get; set; }
        public Country country { get; set; }
    }

    public class Externals
    {
        public object tvrage { get; set; }
        public object thetvdb { get; set; }
        public object imdb { get; set; }
    }

    public class Self
    {
        public int Id { get; set; }
        public string href { get; set; }
    }

    public class Previousepisode
    {
        public int Id { get; set; }
        public string href { get; set; }
    }

    public class Nextepisode
    {
        public int Id { get; set; }
        public string href { get; set; }
    }

    
    public class Show
    {
        public int id { get; set; }
        public int idSite { get; set; }
        public string url { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string language { get; set; }
        [NotMapped]
        public List<string> genres { get; set; }
        public List<Genere> genresObject { get; set; }
        public string status { get; set; }
        public int? runtime { get; set; }
        public string premiered { get; set; }
        public string officialSite { get; set; }
        public Schedule schedule { get; set; }
        [NotMapped]
        public Rating rating { get; set; }
        public double? ratingValue { get; set; }
        public int? weight { get; set; }
        public Network network { get; set; }
        //public string? webChannel { get; set; }
        [NotMapped]
        public Externals externals { get; set; }
        public Image image { get; set; }
        public string summary { get; set; }
        public int? updated { get; set; }
        [NotMapped]
        public List<Season> seasons { get; set; }
        [NotMapped]
        public List<Actor> cast { get; set; }
    }

    public class Season
    {
        public int seasonNumber { get; set; }
        public List<Episode> Episodes { get; set; }
    }

    public class Episode
    {
        public int id { get; set; }
        public string url { get; set; }
        public string name { get; set; }
        public int season { get; set; }
        public int number { get; set; }
    }

    public class Actor
    {
        public Person person { get; set; }
        public Character character { get; set; }
    }

    public class Character
    {
        public int id { get; set; }
        public string url { get; set; }
        public string name { get; set; }
    }

    public class Person
    {
        public int id { get; set; }
        public string url { get; set; }
        public string name { get; set; }
    }

    public class Genere
    {
        public int Id { get; set; }
        public string genereName { get; set; }
    }

    public class Image
    {
        public int Id { get; set; }
        public string medium { get; set; }
        public string original { get; set; }
    }
}
