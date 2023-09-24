using System;

public static class FilterLetterOrDigit
{
    public static bool FillerSpecialCharacters(string name)
    {
        char[] chr = name.ToCharArray();
        bool rs = false;
        foreach (char character in chr)
        {
            if (!char.IsLetterOrDigit(character))
                rs = true;
        }
        return rs;
    }
}
