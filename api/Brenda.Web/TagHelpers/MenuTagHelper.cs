using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Brenda.Web.TagHelpers
{
    [HtmlTargetElement("menu", Attributes = "title, ref")]
    public class MenuTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _context;

        public MenuTagHelper(IHttpContextAccessor context)
        {
            _context = context;
        }

        public string Title { get; set; }
        public string Ref { get; set; }

        [HtmlAttributeName("index")]
        public bool Index { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var active = string.Empty;
            var path = _context.HttpContext.Request.Path.Value;
            if ((Index && (path.Equals(Ref))) || (!Index && path.StartsWith(Ref ?? string.Empty)))
                active = "menu-item-active";

            output.TagName = "li";
            output.Attributes.SetAttribute("class", $"menu-item {active}");
            output.Content.AppendHtml($@"<a href=""{Ref}"" class=""menu-link ""><span class=""menu-text"">{Title}</span></a>");

            return base.ProcessAsync(context, output);
        }
    }
}
