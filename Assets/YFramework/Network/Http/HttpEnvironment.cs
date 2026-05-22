using System;

namespace YFramework.Network.Http
{

public sealed class HttpEnvironment
{
    public const string DevLocalName = "DevLocal";
    public const string TestName = "Test";
    public const string ProdName = "Prod";

    public HttpEnvironment(
        string environmentName,
        string baseUrl,
        bool allowHttp,
        bool allowInvalidCertificates = false,
        int defaultTimeoutSeconds = 15)
    {
        if (string.IsNullOrWhiteSpace(environmentName))
        {
            throw new ArgumentException("Network environment name cannot be empty.", nameof(environmentName));
        }

        if (defaultTimeoutSeconds <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(defaultTimeoutSeconds),
                defaultTimeoutSeconds,
                "Default timeout must be greater than zero.");
        }

        EnvironmentName = environmentName.Trim();
        AllowHttp = allowHttp;
        AllowInvalidCertificates = allowInvalidCertificates;
        DefaultTimeoutSeconds = defaultTimeoutSeconds;
        BaseUrl = NormalizeBaseUrl(baseUrl, allowHttp);
    }

    public string EnvironmentName { get; }
    public string BaseUrl { get; }
    public bool AllowHttp { get; }
    public bool AllowInvalidCertificates { get; }
    public int DefaultTimeoutSeconds { get; }
    public bool IsDevLocal => string.Equals(EnvironmentName, DevLocalName, StringComparison.OrdinalIgnoreCase);

    public static HttpEnvironment CreateDevLocal(
        string baseUrl,
        int defaultTimeoutSeconds = 15,
        bool allowInvalidCertificates = false)
    {
        return new HttpEnvironment(
            DevLocalName,
            baseUrl,
            allowHttp: true,
            allowInvalidCertificates: allowInvalidCertificates,
            defaultTimeoutSeconds: defaultTimeoutSeconds);
    }

    public static HttpEnvironment CreateTest(string baseUrl, int defaultTimeoutSeconds = 15)
    {
        return new HttpEnvironment(
            TestName,
            baseUrl,
            allowHttp: true,
            allowInvalidCertificates: false,
            defaultTimeoutSeconds: defaultTimeoutSeconds);
    }

    public static HttpEnvironment CreateProd(string baseUrl, int defaultTimeoutSeconds = 15)
    {
        return new HttpEnvironment(
            ProdName,
            baseUrl,
            allowHttp: true,
            allowInvalidCertificates: false,
            defaultTimeoutSeconds: defaultTimeoutSeconds);
    }

    public string ResolveUrl(string relativeOrAbsolutePath)
    {
        if (string.IsNullOrWhiteSpace(relativeOrAbsolutePath))
        {
            return BaseUrl;
        }

        string trimmedPath = relativeOrAbsolutePath.Trim();
        if (Uri.TryCreate(trimmedPath, UriKind.Absolute, out Uri absoluteUri))
        {
            ValidateScheme(absoluteUri, AllowHttp);
            return absoluteUri.AbsoluteUri.TrimEnd('/');
        }

        string relativePath = trimmedPath.TrimStart('/');
        return string.IsNullOrEmpty(relativePath) ? BaseUrl : $"{BaseUrl}/{relativePath}";
    }

    public override string ToString()
    {
        return $"{EnvironmentName} ({BaseUrl})";
    }

    private static string NormalizeBaseUrl(string baseUrl, bool allowHttp)
    {
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new ArgumentException("BaseUrl cannot be empty.", nameof(baseUrl));
        }

        if (!Uri.TryCreate(baseUrl.Trim(), UriKind.Absolute, out Uri parsedUri))
        {
            throw new ArgumentException($"BaseUrl is not a valid absolute URI: {baseUrl}", nameof(baseUrl));
        }

        ValidateScheme(parsedUri, allowHttp);
        return parsedUri.AbsoluteUri.TrimEnd('/');
    }

    private static void ValidateScheme(Uri uri, bool allowHttp)
    {
        if (uri == null)
        {
            throw new ArgumentNullException(nameof(uri));
        }

        if (uri.Scheme == Uri.UriSchemeHttps)
        {
            return;
        }

        if (allowHttp && uri.Scheme == Uri.UriSchemeHttp)
        {
            return;
        }

        throw new InvalidOperationException(
            $"Network environment does not allow scheme '{uri.Scheme}' for url '{uri.AbsoluteUri}'.");
    }
}

}
