using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Brenda.Web.TagHelpers
{
    [HtmlTargetElement("tab-content", Attributes = "id")]
    public class TabContentTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _context;

        public TabContentTagHelper(IHttpContextAccessor context)
        {
            _context = context;
        }

        public string Id { get; set; }

        [HtmlAttributeName("index")]
        public bool Index { get; set; }

        [HtmlAttributeName("styles")]
        public string Styles { get; set; }

        [HtmlAttributeName("ref")]
        public string Ref { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var active = string.Empty;
            var path = _context.HttpContext.Request.Path.Value;
            if ((Index && path.Equals("/")) || (!Index && path.StartsWith(Ref ?? string.Empty)))
                active = "show active";

            output.TagName = "div";
            output.Attributes.SetAttribute("id", Id);
            output.Attributes.SetAttribute("class", $"tab-pane py-5 p-lg-0 {Styles} {active}");

            return base.ProcessAsync(context, output);
        }
    }
}
