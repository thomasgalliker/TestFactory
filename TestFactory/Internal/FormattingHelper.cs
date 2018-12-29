namespace TestFactory.Internal
{
    internal static class FormattingHelper
    {
        internal static string Indent(int count)
        {
            return "".PadLeft(count);
        }
    }
}