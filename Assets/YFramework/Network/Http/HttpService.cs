using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Stopwatch = System.Diagnostics.Stopwatch;

namespace YFramework.Network.Http
{

public sealed class HttpService
{
    private sealed class AllowAllCertificatesHandler : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }

    public HttpService(HttpEnvironment environment)
    {
        Environment = environment ?? throw new ArgumentNullException(nameof(environment));
    }

    public HttpEnvironment Environment { get; private set; }

    public void SetEnvironment(HttpEnvironment environment)
    {
        Environment = environment ?? throw new ArgumentNullException(nameof(environment));
    }

    public Task<HttpResult> GetAsync(string relativeOrAbsolutePath, HttpRequestOptions options = null)
    {
        return SendAsync(UnityWebRequest.kHttpVerbGET, relativeOrAbsolutePath, null, options);
    }

    public Task<HttpResult> PostRawAsync(
        string relativeOrAbsolutePath,
        byte[] payload,
        HttpRequestOptions options = null)
    {
        return SendAsync(UnityWebRequest.kHttpVerbPOST, relativeOrAbsolutePath, payload ?? Array.Empty<byte>(), options);
    }

    private async Task<HttpResult> SendAsync(
        string method,
        string relativeOrAbsolutePath,
        byte[] payload,
        HttpRequestOptions options)
    {
        if (Environment == null)
        {
            throw new InvalidOperationException("HttpService environment is not configured.");
        }

        HttpRequestOptions requestOptions = options?.Clone() ?? HttpRequestOptions.CreateBinary();
        string requestUrl;

        try
        {
            requestUrl = Environment.ResolveUrl(relativeOrAbsolutePath);
        }
        catch (Exception exception)
        {
            Debug.LogError($"[Network][HTTP] Failed to resolve url '{relativeOrAbsolutePath}': {exception.Message}");
            return HttpResult.Failure(method, relativeOrAbsolutePath, 0, exception.Message, null, 0d);
        }

        using (UnityWebRequest request = CreateRequest(method, requestUrl, payload, requestOptions))
        {
            LogRequest(method, requestUrl, payload, requestOptions);

            Stopwatch stopwatch = Stopwatch.StartNew();

            try
            {
                await SendWebRequestAsync(request);
            }
            catch (Exception exception)
            {
                stopwatch.Stop();
                Debug.LogError($"[Network][HTTP] {method} {requestUrl} threw exception: {exception}");
                return HttpResult.Failure(
                    method,
                    requestUrl,
                    request.responseCode,
                    exception.Message,
                    request.downloadHandler?.data,
                    stopwatch.Elapsed.TotalMilliseconds,
                    request.GetResponseHeaders(),
                    request.result);
            }

            stopwatch.Stop();
            HttpResult result = BuildResult(method, requestUrl, request, stopwatch.Elapsed.TotalMilliseconds);
            // LogTraceResult(result);
            // LogResponseHeaders(result);
            // LogResult(result, requestOptions);
            return result;
        }
    }

    private UnityWebRequest CreateRequest(
        string method,
        string requestUrl,
        byte[] payload,
        HttpRequestOptions options)
    {
        UnityWebRequest request = new UnityWebRequest(requestUrl, method)
        {
            downloadHandler = new DownloadHandlerBuffer(),
            timeout = options.TimeoutSeconds ?? Environment.DefaultTimeoutSeconds
        };

        request.disposeDownloadHandlerOnDispose = true;
        request.disposeUploadHandlerOnDispose = true;

        if (string.Equals(method, UnityWebRequest.kHttpVerbPOST, StringComparison.OrdinalIgnoreCase))
        {
            request.uploadHandler = new UploadHandlerRaw(payload ?? Array.Empty<byte>());
            request.SetRequestHeader("Content-Type", options.ContentType);
        }

        foreach (KeyValuePair<string, string> header in options.Headers)
        {
            request.SetRequestHeader(header.Key, header.Value);
        }

        if (Environment.AllowInvalidCertificates &&
            requestUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            // 仅环境层可以决定是否放宽证书校验，避免业务层把例外写死。
            request.certificateHandler = new AllowAllCertificatesHandler();
            request.disposeCertificateHandlerOnDispose = true;
        }

        return request;
    }

    private void LogRequest(
        string method,
        string requestUrl,
        byte[] payload,
        HttpRequestOptions options)
    {
        if (!options.EnableRequestLog)
        {
            return;
        }

        int payloadSize = payload?.Length ?? 0;
        string tagPrefix = string.IsNullOrWhiteSpace(options.RequestTag) ? string.Empty : $"[{options.RequestTag}] ";
    }

    private static void LogResult(HttpResult result, HttpRequestOptions options)
    {
        if (result == null)
        {
            return;
        }

        if (result.IsSuccess)
        {
            if (!options.EnableResponseLog)
            {
                return;
            }
            return;
        }

        Debug.LogError(
            $"[Network][HTTP] {result.Method} {result.Url} failed status={result.StatusCode} result={result.RequestResult} durationMs={result.DurationMs:F0} error={result.ErrorMessage}");
    }

    private static void LogTraceResult(HttpResult result)
    {
        if (result == null)
        {
            return;
        }
    }

    private static void LogResponseHeaders(HttpResult result)
    {
        if (result?.ResponseHeaders == null)
        {
            return;
        }

        foreach (KeyValuePair<string, string> header in result.ResponseHeaders)
        {
            Debug.Log($"[Network][HTTP][Header] method={result.Method} url={result.Url} {header.Key}={header.Value}");
        }
    }

    private static HttpResult BuildResult(
        string method,
        string requestUrl,
        UnityWebRequest request,
        double durationMs)
    {
        Dictionary<string, string> responseHeaders = request.GetResponseHeaders();
        byte[] responseBytes = request.downloadHandler?.data;

        if (request.result == UnityWebRequest.Result.Success)
        {
            return HttpResult.Success(method, requestUrl, request.responseCode, responseBytes, durationMs, responseHeaders);
        }

        string errorMessage = string.IsNullOrWhiteSpace(request.error)
            ? $"Request failed with result '{request.result}'."
            : request.error;

        return HttpResult.Failure(
            method,
            requestUrl,
            request.responseCode,
            errorMessage,
            responseBytes,
            durationMs,
            responseHeaders,
            request.result);
    }

    private static Task SendWebRequestAsync(UnityWebRequest request)
    {
        UnityWebRequestAsyncOperation operation = request.SendWebRequest();
        if (operation.isDone)
        {
            return Task.CompletedTask;
        }

        TaskCompletionSource<bool> completionSource = new TaskCompletionSource<bool>();
        operation.completed += _ => completionSource.TrySetResult(true);
        return completionSource.Task;
    }
}

}
