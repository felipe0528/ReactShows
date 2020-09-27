using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class ShowsPresentation
    {
        public int QueryCount { get; set; }
        public List<Show> FilteredShows { get; set; }
    }
}
