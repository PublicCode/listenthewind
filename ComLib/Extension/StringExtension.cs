namespace ComLib.Extension
{
    public static class StringExtension
    {
        public static string TrimOne(this string param, params char[] trimChars)
        {
            foreach(var c in trimChars)
            {
                if(param[0]==c)
                {
                    param = param.Substring(1, param.Length - 1);
                    break;
                }
            }
            foreach(var c in trimChars)
            {
                if(param[param.Length-1]==c)
                {
                    param = param.Substring(0, param.Length - 1);
                }
            }
            return param;
        }
    }
}
