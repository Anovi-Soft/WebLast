using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnoviSoftSiteFun.Models
{
    public class LikesModel
    {
        public IEnumerable<Helpers.QuoteLike> Likes { get; set; }
        public int QuoteId { get; set; }
    }
}
