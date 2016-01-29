using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services.Description;

namespace MyBusinessCardSite.Models
{
    public class QuoteModel
    {
        public QuoteModel(int id, string author, string quote)
        {
            Id = id-1;
            Author = author;
            Quote = quote;
            do Next = new Random().Next(0, 100); while (Next==id);
        }

        public int Id { get; } 
        public string Author { get;}
        public string Quote { get;}
        public int Next { get; }
    }
}
