using Brenda.Core;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brenda.Web.TagHelpers
{
    [HtmlTargetElement("img-format", Attributes = "src")]
    public class ImgFormatTagHelper : TagHelper
    {
        public string Src { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "symbol symbol-50 symbol-light mr-2");//h-50 align-self-center
            output.Content.AppendHtml($@"<span class=""symbol-label"" ><img src=""{Src}"" class=""h-50 align-self-center"" alt="""" ></span>");

            return base.ProcessAsync(context, output);
        }
    }
}
