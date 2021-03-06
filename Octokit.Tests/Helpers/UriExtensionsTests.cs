﻿using System;
using System.Collections.Generic;
using Xunit;

namespace Octokit.Tests.Helpers
{
    public class UriExtensionsTests
    {
        public class TheApplyParametersMethod
        {
            [Fact]
            public void AppendsParametersAsQueryString()
            {
                var uri = new Uri("https://example.com");

                var uriWithParameters = uri.ApplyParameters(new Dictionary<string, string>
                {
                    {"foo", "fooval"},
                    {"bar", "barval"}
                });

                Assert.Equal(new Uri("https://example.com?foo=fooval&bar=barval"), uriWithParameters);
            }

            [Fact]
            public void AppendsParametersAsQueryStringToRelativeUri()
            {
                var uri = new Uri("issues", UriKind.Relative);

                var uriWithParameters = uri.ApplyParameters(new Dictionary<string, string>
                {
                    {"foo", "fooval"},
                    {"bar", "barval"}
                });

                Assert.Equal(new Uri("issues?foo=fooval&bar=barval", UriKind.Relative), uriWithParameters);
            }

            [Fact(Skip="I don't believe this test is valid")]
            public void OverwritesExistingParameters()
            {
                var uri = new Uri("https://example.com?crap=crapola");

                var uriWithParameters = uri.ApplyParameters(new Dictionary<string, string>
                {
                    {"foo", "fooval"},
                    {"bar", "barval"}
                });

                Assert.Equal(new Uri("https://example.com?foo=fooval&bar=barval"), uriWithParameters);
            }

            [Fact]
            public void DoesNotChangeUrlWhenParametersEmpty()
            {
                var uri = new Uri("https://example.com");

                var uriWithEmptyParameters = uri.ApplyParameters(new Dictionary<string, string>());
                var uriWithNullParameters = uri.ApplyParameters(null);

                Assert.Equal(uri, uriWithEmptyParameters);
                Assert.Equal(uri, uriWithNullParameters);
            }

            [Fact]
            public void CombinesExistingParametersWithNewParameters()
            {
                var uri = new Uri("https://api.github.com/repositories/1/milestones?state=closed&sort=due_date&direction=asc&page=2");

                var parameters = new Dictionary<string, string> { { "state", "open" }, { "sort", "other"} };

                var actual = uri.ApplyParameters(parameters);

                Assert.True(actual.Query.Contains("state=open"));
                Assert.True(actual.Query.Contains("sort=other"));
                Assert.True(actual.Query.Contains("direction=asc"));
                Assert.True(actual.Query.Contains("page=2"));
            }

            [Fact]
            public void EnsuresArgumentNotNull()
            {
                Uri uri = null;
                Assert.Throws<ArgumentNullException>(() => uri.ApplyParameters(new Dictionary<string, string>()));
            }
        }
    }
}
