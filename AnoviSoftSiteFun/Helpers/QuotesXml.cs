using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AnoviSoftSiteFun.Helpers
{
    public class QuotesXml
    {
        [XmlArray("Quotes"), XmlArrayItem(typeof(QuoteXml), ElementName = "Quote")]
        public List<QuoteXml> Quotes { get; set; }

        public static QuotesXml GetQuotes(IEnumerable<ItQuote> quotes, IEnumerable<QuoteComment> comments,
            IEnumerable<QuoteLike> likes) =>
                new QuotesXml
                {
                    Quotes =
                        quotes.Select(
                            x =>
                                new QuoteXml
                                {
                                    Author = x.Author,
                                    Quote = x.Quote,
                                    Likes = likes.Count(y => y.Quote == x.id),
                                    Comments = comments.Select(y=> new QuoteCommentXml
                                    {
                                        User = y.UserName,
                                        Message = y.Comment,
                                        DateTime = $"{y.CreateTime:g}"
                                    })
                                    .ToList()
                                })
                    .ToList()
                };
    }
    [XmlRoot("QuotesXml")]
    public class QuoteXml
    {
        public string Author { get; set; }
        public string Quote { get; set; }
        public int Likes { get; set; }

        [XmlArray("Comments"), XmlArrayItem(typeof(QuoteCommentXml), ElementName = "Comment")]
        public List<QuoteCommentXml> Comments { get; set; }
    }

    [XmlRoot("QuoteXml")]
    public class QuoteCommentXml
    {
        public string User { get; set; }
        public string Message { get; set; }
        public string DateTime { get; set; }
    }
}
