using System;
using System.Collections.Generic;
using System.Text;
using Vita.Predictor.TextMatch;
using Xunit;

namespace Vita.Predictor.Tests.TextMatch
{
    [Collection("DataCollection")]
    public abstract class MatchShouldBase
    {
        protected DataFixture DataFixture;
        protected TextMatcher Matcher;

        public MatchShouldBase(DataFixture dataFixture)
        {
            DataFixture = dataFixture;
            DataFixture.Init();

            Matcher =
                new TextMatcher(DataFixture.Companies, DataFixture.Localities, DataFixture.Classifiers)
                {
                    UseCache = false
                };
            // _analyser.FlushCache();
        }


    }
}
