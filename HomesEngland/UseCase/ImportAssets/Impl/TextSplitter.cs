﻿using System.Collections.Generic;

namespace HomesEngland.UseCase.ImportAssets.Impl
{
    public class TextSplitter : ITextSplitter
    {
        public IList<string> SplitIntoLines(string text)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
                return null;
            return text.Split("\n");
        }
    }
}