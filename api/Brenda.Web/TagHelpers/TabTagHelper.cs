using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Brenda.Web.TagHelpers
{
    [HtmlTargetElement("tab", Attributes = "title, tab")]
    public class TabTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _context;

        public TabTagHelper(IHttpContextAccessor context)
        {
            _context = context;
        }

        public string Title { get; set; }
        public string Tab { get; set; }
        public bool Index { get; set; }

        [HtmlAttributeName("ref")]
        public string Ref { get; set; }

        [HtmlAttributeName("link-styles")]
        public string LinkStyles { get; set; }

        [HtmlAttributeName("item-styles")]
        public string ItemStyles { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var active = string.Empty;
            var tabAttributes = string.Empty;
            var path = _context.HttpContext.Request.Path.Value;
            if (!string.IsNullOrEmpty(Tab))
                tabAttributes = $@"data-toggle=""tab"" data-target=""#{Tab}"" role=""tab""";

            if ((Index && path.Equals("/")) || (!Index && path.StartsWith(Ref ?? string.Empty)))
                active = "active";

            output.TagName = "li";
            output.Attributes.SetAttribute("class", $"nav-item {ItemStyles}");
            output.Content.AppendHtml($@"<a href=""#"" {tabAttributes} class=""nav-link {LinkStyles} {active}"">{Title}</a>");

            return base.ProcessAsync(context, output);
        }
    }
}
