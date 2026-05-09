using System;
using System.Collections.Generic;

namespace YFramework.Network.Http
{

public sealed class HttpRequestOptions
{
    private readonly Dictionary<string, string> m_headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

    public HttpRequestOptions(int? timeoutSeconds = null, string contentType = "application/octet-stream")
    {
        if (timeoutSeconds.HasValue && timeoutSeconds.Value <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(timeoutSeconds),
                timeoutSeconds.Value,
                "Timeout must be greater than zero.");
        }

        TimeoutSeconds = timeoutSeconds;
        ContentType = string.IsNullOrWhiteSpace(contentType) ? "application/octet-stream" : contentType.Trim();
        EnableRequestLog = true;
        EnableResponseLog = true;
    }

    public IReadOnlyDictionary<string, string> Headers => m_headers;
    public int? TimeoutSeconds { get; private set; }
    public string ContentType { get; private set; }
    public bool EnableRequestLog { get; set; }
    public bool EnableResponseLog { get; set; }
    public string RequestTag { get; set; }

    public static HttpRequestOptions CreateBinary(int? timeoutSeconds = null)
    {
        return new HttpRequestOptions(timeoutSeconds, "application/octet-stream");
    }

    public HttpRequestOptions SetHeader(string key, string value)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Header key cannot be empty.", nameof(key));
        }

        if (value == null)
        {
            m_headers.Remove(key);
            return this;
        }

        m_headers[key.Trim()] = value;
        return this;
    }

    public HttpRequestOptions SetTimeout(int timeoutSeconds)
    {
        if (timeoutSeconds <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(timeoutSeconds),
                timeoutSeconds,
                "Timeout must be greater than zero.");
        }

        TimeoutSeconds = timeoutSeconds;
        return this;
    }

    public HttpRequestOptions SetContentType(string contentType)
    {
        ContentType = string.IsNullOrWhiteSpace(contentType) ? "application/octet-stream" : contentType.Trim();
        return this;
    }

    public HttpRequestOptions Clone()
    {
        HttpRequestOptions copy = new HttpRequestOptions(TimeoutSeconds, ContentType)
        {
            EnableRequestLog = EnableRequestLog,
            EnableResponseLog = EnableResponseLog,
            RequestTag = RequestTag
        };

        foreach (KeyValuePair<string, string> pair in m_headers)
        {
            copy.m_headers[pair.Key] = pair.Value;
        }

        return copy;
    }
}

}
