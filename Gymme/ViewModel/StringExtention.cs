namespace Gymme.ViewModel
{
    public static class StringExtention
    {
        public static string UppercaseFirst(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            char[] chars = str.ToCharArray();
            chars[0] = char.ToUpper(chars[0]);
            return new string(chars);
        }
    }
}