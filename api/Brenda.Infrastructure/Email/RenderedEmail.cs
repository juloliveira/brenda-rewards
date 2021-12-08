using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Brenda.Infrastructure.Email
{
    public class RenderedEmail
    {
        private string _subject;
        public RenderedEmail(string htmlEmail)
        {
            HtmlEmail = htmlEmail;
        }

        public string HtmlEmail { get; private set; }

        public string Subject { 
            get 
            {
                if (string.IsNullOrEmpty(_subject))
                    _subject = Regex.Match(
                        HtmlEmail, 
                        @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>",
                        RegexOptions.IgnoreCase)
                    .Groups["Title"].Value;

                return _subject.Trim();
            } 
        }

    }
}
