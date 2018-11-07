namespace TestFactory.Extensions
{
    internal static class FormattingHelper
    {
        internal static string Indent(int count)
        {
            return "".PadLeft(count);
        }
    }
}