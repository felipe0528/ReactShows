using Domain;
using Infraestructure.Show;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Show.DTOs
{
    public class ShowDto
    {
        public int Id { get; set; }
        public int IdAPI { get; set; }
        public string Name { get; set; }
        public string PhotoURL { get; set; }
        public string Channel { get; set; }
        public string Summary { get; set; }
        public double? Rating { get; set; }
        public string Genere { get; set; }
        public string Language { get; set; }
        public string Time { get; set; }
        public string Days { get; set; }
        public List<Actor> Cast { get; set; }
        public List<Season> Seasons { get; set; }
    }
}
