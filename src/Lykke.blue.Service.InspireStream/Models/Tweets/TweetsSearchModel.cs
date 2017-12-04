using System;
using System.ComponentModel.DataAnnotations;

namespace Lykke.blue.Service.InspireStream.Models.Tweets
{
    public class TweetsSearchModel
    {
        public bool IsExtendedSearch { get; set; }

        [Required]
        public string AccountEmail { get; set; }

        [Required]
        public string SearchQuery { get; set; }

        public int MaxResult { get; set; }

        public DateTime UntilDate { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }
    }
}
