﻿using System;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace ChristianSchulz.ObjectInspection.Server.SwaggerUI;

public static class SwaggerUIOptionsExtensions
{
    private const string AccessTokenRequestInterceptorJavaScript =
        """
              (req) => 
              {
                var queryString = new URLSearchParams(window.location.search);
                var accessToken = queryString.get('access_token');
            
                if(accessToken != null) 
                {
                  req.headers['Authorization'] = accessToken; 
                }

                return req; 
              }
            """;

    public static SwaggerUIOptions UseAccessTokenRequestInterceptor(this SwaggerUIOptions options)
    {
        var accessTokenRequestInterceptorJavaScript = AccessTokenRequestInterceptorJavaScript
            .Replace(Environment.NewLine, string.Empty);

        options.UseRequestInterceptor(accessTokenRequestInterceptorJavaScript);

        return options;
    }
}
