using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;
using Flurl;
using Flurl.Util;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
namespace CoreMachine.Req;

public class Req
{
    private readonly HttpMethod _method;
    private readonly HttpRequestMessage _request;
    private Url _url = new();
    
    private Req(HttpMethod method)
    {
        _method = method;
        _request = new HttpRequestMessage();
    }

    public HttpRequestMessage Build()
    {
        _request.RequestUri = _url.ToUri();
        _request.Method = _method;
        return _request;
    }

    #region Body
    
    public Req WithJsonBody<T>(T body, JsonSerializerOptions? serializerOptions = null)
    {
        string json = JsonSerializer.Serialize(body, serializerOptions);
        _request.Content = new StringContent(json);
        return this;
    }

    public Req WithStringBody(string body)
    {
        _request.Content = new StringContent(body);
        return this;
    }

    public Req WithXmlBody<T>(T body)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        
        using var stringWriter = new StringWriter();
        using var xmlWriter = new XmlTextWriter(stringWriter);
        xmlWriter.Formatting = Formatting.Indented;

        serializer.Serialize(xmlWriter, body);
        
        _request.Content = new StringContent(stringWriter.ToString());
        return this;
    }

    #endregion
    
    #region Headers

    public Req WithHeaders(object obj)
    { 
        _request.Headers.Clear();
        foreach ((string key, object value) in obj.ToKeyValuePairs())
        {
            _request.Headers.Add(key, value.ToInvariantString());
        }
        return this;
    }

    public Req AddHeader(string key, string value)
    {
        _request.Headers.Add(key, value);
        return this;
    }

    public Req WithBasicAuth(string username, string password)
    {
        string encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
        _request.Headers.Add("Authorization", encoded);
        return this;
    }

    public Req WithBearerToken(string token)
    {
        _request.Headers.Add("Authorization", $"Bearer {token}");
        return this;
    }
    
    #endregion

    #region Url

    public Req AppendSegment(string segment)
    {
        _url.AppendPathSegment(segment);
        return this;
    }

    
    
    public Req WithQuery(object obj)
    {
        _url.SetQueryParams(obj);
        return this;
    }

    public Req WithQuery(string str)
    {
        _url.Query = str;
        return this;
    }
    
    public Req AddQueryParam(string key, object value)
    {
        _url.AppendQueryParam(key, value);
        return this;
    }

    public Req AppendQueryParam(string key, object value)
    {
        _url.AppendQueryParam(key, value);
        return this;
    }


    public Req To(string url)
    {
        _url = url;
        return this;
    }

    public Req To(Uri uri)
    {
        _url = uri;
        return this;
    }
    #endregion


    public static Req New(string method) => new(HttpMethod.Parse(method));
    public static Req New(HttpMethod method) => new(method);
    public static Req Get => new(HttpMethod.Get);
    public static Req Post => new(HttpMethod.Post);
    public static Req Put => new(HttpMethod.Put);
    public static Req Patch => new(HttpMethod.Patch);
    public static Req Delete => new(HttpMethod.Delete);
    public static Req Connect => new(HttpMethod.Connect);
    public static Req Trace => new(HttpMethod.Trace);
    public static Req Head => new(HttpMethod.Head);
    public static Req Options => new(HttpMethod.Options);
}

