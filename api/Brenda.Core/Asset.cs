using System;
using System.Collections.Generic;
using System.Linq;

namespace Brenda.Core
{
    public class Asset : Entity
    {
        private readonly IList<Quiz> _quiz;

        protected Asset()
        {
            _quiz = new List<Quiz>();
        }

        public Asset(string title, Guid customerId, Guid formatId) : this()
        {
            Title = title;
            CustomerId = customerId;
            ActionId = formatId;
            Enable = false;
        }

        public string Title { get; set; }
        public bool Enable { get; set; }

        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }

        public Guid ActionId { get; set; }
        public Action Action { get; set; }

        public string Resource { get; set; }

        public IEnumerable<Quiz> Questions { get { return _quiz; } }

        public Quiz AddQuiz()
        {
            if (!Identifiers.Actions.IsQuiz(ActionId)) throw new ArgumentException();
            var quiz = new Quiz(this);
            _quiz.Add(quiz);

            return quiz;
        }
    }

    public class Quiz : Entity
    {
        private readonly IList<Quiz> _options;

        protected Quiz()
        {
            _options = new List<Quiz>();
        }

        public Quiz(Asset asset) : this()
        {
            AssetId = asset.Id;
            Order = asset.Questions.Count() + 1;

            Description = null;
        }

        public Quiz(Quiz parent)
        {
            ParentId = parent.Id;
            Parent = parent;
            Description = null;
            Order = parent.Options.Count() + 1;
        }

        public string Description { get; set; }

        public Guid? AssetId { get; set; }
        public Asset Asset { get; set; }

        public Guid? ParentId { get; set; }
        public Quiz Parent { get; set; }

        public int Order { get; set; }

        public IEnumerable<Quiz> Options { get { return _options; } }

        public Quiz AddOption()
        {
            if (!AssetId.HasValue) throw new ArgumentException("Quiz deve ser uma pergunta.");
            var quizOption = new Quiz(this);
            _options.Add(quizOption);

            return quizOption;
        }

    }
}
