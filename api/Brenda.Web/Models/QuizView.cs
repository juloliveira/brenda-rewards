using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brenda.Web.Models
{
    public class QuizView
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }

        private IEnumerable<QuizView> _options;
        public IEnumerable<QuizView> Options 
        { 
            get { return _options.OrderBy(x => x.Order); } 
            set { _options = value; } 
        }
    }
}
