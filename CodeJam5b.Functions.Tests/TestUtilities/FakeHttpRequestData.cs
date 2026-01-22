using System.Net;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace CodeJam5b.Functions.Tests.TestUtilities;

internal sealed class FakeHttpRequestData : HttpRequestData
{
    public FakeHttpRequestData(FunctionContext functionContext, Uri url, Stream body)
        : base(functionContext)
    {
        Url = url;
        Body = body;
        Headers = new HttpHeadersCollection();
        Cookies = Array.Empty<IHttpCookie>();
        Identities = Array.Empty<ClaimsIdentity>();
        Method = "GET";
    }

    public override Stream Body { get; }
    public override HttpHeadersCollection Headers { get; }
    public override IReadOnlyCollection<IHttpCookie> Cookies { get; }
    public override Uri Url { get; }
    public override IEnumerable<ClaimsIdentity> Identities { get; }
    public override string Method { get; }

    public override HttpResponseData CreateResponse() => new FakeHttpResponseData(FunctionContext);
}

internal sealed class FakeHttpResponseData : HttpResponseData
{
    public FakeHttpResponseData(FunctionContext functionContext)
        : base(functionContext)
    {
        Headers = new HttpHeadersCollection();
        Body = new MemoryStream();
        StatusCode = HttpStatusCode.OK;
    }

    public override HttpStatusCode StatusCode { get; set; }
    public override HttpHeadersCollection Headers { get; set; }
    public override Stream Body { get; set; }

    // Tests don't validate cookie behavior; not supported by this fake for this worker version.
    public override HttpCookies Cookies => throw new NotSupportedException("Response cookies are not supported by FakeHttpResponseData.");

    // Helper used by tests to write JSON without requiring Functions worker serializer configuration.
    public async Task WriteJsonAsync<T>(T value, CancellationToken cancellationToken = default)
    {
        Headers.Add("Content-Type", "application/json; charset=utf-8");

        await JsonSerializer.SerializeAsync(
            Body,
            value,
            new JsonSerializerOptions(JsonSerializerDefaults.Web),
            cancellationToken);

        Body.Position = 0;
    }
}
