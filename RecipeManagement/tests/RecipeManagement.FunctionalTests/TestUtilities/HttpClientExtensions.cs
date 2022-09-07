namespace RecipeManagement.FunctionalTests.TestUtilities;

using System.Dynamic;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

public static class HttpClientExtensions
{
    public static HttpClient AddAuth(this HttpClient client, params string[] roles)
    {
        dynamic data = new ExpandoObject();
        data.sub = Guid.NewGuid();
        data.role = roles;
        client.SetFakeBearerToken((object)data);

        return client;
    }

    public static HttpClient AddAuth(this HttpClient client, string nameIdentifier)
    {
        dynamic data = new ExpandoObject();
        data.ClaimTypes.NameIdentifier = nameIdentifier;
        client.SetFakeBearerToken((object)data);

        return client;
    }
    
    public static async Task<HttpResponseMessage> GetRequestAsync(this HttpClient client, string url)
    {
        return await client.GetAsync(url).ConfigureAwait(false);
    }

    public static async Task<HttpResponseMessage> DeleteRequestAsync(this HttpClient client, string url)
    {
        return await client.DeleteAsync(url).ConfigureAwait(false);
    }

    public static async Task<HttpResponseMessage> PostJsonRequestAsync(this HttpClient client, string url, object value)
    {
        return await client.PostAsJsonAsync(url, value).ConfigureAwait(false);
    }

    public static async Task<HttpResponseMessage> PutJsonRequestAsync(this HttpClient client, string url, object value)
    {
        return await client.PutAsJsonAsync(url, value).ConfigureAwait(false);
    }
}