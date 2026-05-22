using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;

namespace YFramework.Network.Http
{

public sealed class HttpResult
{
    private readonly Dictionary<string, string> m_responseHeaders;
    private string m_cachedResponseText;

    private HttpResult(
        bool isSuccess,
        string method,
        string url,
        long statusCode,
        byte[] rawBytes,
        string errorMessage,
        double durationMs,
        Dictionary<string, string> responseHeaders,
        UnityWebRequest.Result requestResult)
    {
        IsSuccess = isSuccess;
        Method = method ?? string.Empty;
        Url = url ?? string.Empty;
        StatusCode = statusCode;
        RawBytes = CloneBytes(rawBytes);
        ErrorMessage = errorMessage ?? string.Empty;
        DurationMs = durationMs;
        RequestResult = requestResult;
        m_responseHeaders = responseHeaders != null
            ? new Dictionary<string, string>(responseHeaders, StringComparer.OrdinalIgnoreCase)
            : new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    }

    public bool IsSuccess { get; }
    public string Method { get; }
    public string Url { get; }
    public long StatusCode { get; }
    public byte[] RawBytes { get; }
    public string ErrorMessage { get; }
    public double DurationMs { get; }
    public UnityWebRequest.Result RequestResult { get; }
    public IReadOnlyDictionary<string, string> ResponseHeaders => m_responseHeaders;
    public bool HasPayload => RawBytes != null && RawBytes.Length > 0;

    public string ResponseText
    {
        get
        {
            if (m_cachedResponseText == null)
            {
                m_cachedResponseText = HasPayload ? Encoding.UTF8.GetString(RawBytes) : string.Empty;
            }

            return m_cachedResponseText;
        }
    }

    public static HttpResult Success(
        string method,
        string url,
        long statusCode,
        byte[] rawBytes,
        double durationMs,
        Dictionary<string, string> responseHeaders = null)
    {
        return new HttpResult(
            true,
            method,
            url,
            statusCode,
            rawBytes,
            string.Empty,
            durationMs,
            responseHeaders,
            UnityWebRequest.Result.Success);
    }

    public static HttpResult Failure(
        string method,
        string url,
        long statusCode,
        string errorMessage,
        byte[] rawBytes,
        double durationMs,
        Dictionary<string, string> responseHeaders = null,
        UnityWebRequest.Result requestResult = UnityWebRequest.Result.ConnectionError)
    {
        return new HttpResult(
            false,
            method,
            url,
            statusCode,
            rawBytes,
            errorMessage,
            durationMs,
            responseHeaders,
            requestResult);
    }

    public override string ToString()
    {
        if (IsSuccess)
        {
            return $"{Method} {Url} succeeded with status {StatusCode} in {DurationMs:F0}ms.";
        }

        return $"{Method} {Url} failed with status {StatusCode} ({RequestResult}): {ErrorMessage}";
    }

    private static byte[] CloneBytes(byte[] bytes)
    {
        if (bytes == null || bytes.Length == 0)
        {
            return Array.Empty<byte>();
        }

        byte[] copy = new byte[bytes.Length];
        Buffer.BlockCopy(bytes, 0, copy, 0, bytes.Length);
        return copy;
    }
}

}
