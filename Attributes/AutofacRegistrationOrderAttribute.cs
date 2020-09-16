using System;
namespace MarkdownToGist.Attributes
{
    public class AutofacRegistrationOrderAttribute : Attribute
    {
        public const string AttributeName = "Order";

        public AutofacRegistrationOrderAttribute(int order)
        {
            Order = order;
        }

        public int Order { get; set; }
    }
}
