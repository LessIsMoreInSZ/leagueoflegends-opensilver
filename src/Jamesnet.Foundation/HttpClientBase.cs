using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Threading;

namespace Jamesnet.Foundation;

[AttributeUsage(AttributeTargets.Method)]
public class RequireAuthenticationAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Method)]
public class OptionalAuthenticationAttribute : Attribute
{
}

public abstract class HttpClientBase
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly JwtTokenManager _jwtTokenManager;

    protected HttpClientBase(
        HttpClient httpClient,
        JsonSerializerOptions jsonOptions,
        JwtTokenManager jwtTokenManager)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _jsonOptions = jsonOptions ?? throw new ArgumentNullException(nameof(jsonOptions));
        _jwtTokenManager = jwtTokenManager ?? throw new ArgumentNullException(nameof(jwtTokenManager));
    }

    protected async Task<T> SendAsync<T>(HttpRequestMessage request, CancellationToken cancellationToken = default, [CallerMemberName] string callerMethod = "")
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        try
        {
            await ApplyAuthenticationIfRequired(request, callerMethod);

            var response = await _httpClient.SendAsync(request, cancellationToken);

            if (cancellationToken.IsCancellationRequested)
            {
                return default(T);
            }

            await HandleResponseAsync(response, callerMethod);

            string content;
            if (response.Content != null)
            {
                content = await response.Content.ReadAsStringAsync();
            }
            else
            {
                content = string.Empty;
            }

            if (typeof(T) == typeof(string))
            {
                return (T)(object)content;
            }

            if (string.IsNullOrEmpty(content))
            {
                throw new InvalidOperationException("Response content is empty");
            }

            var result = JsonSerializer.Deserialize<T>(content, _jsonOptions);
            if (result == null)
            {
                throw new InvalidOperationException("Deserialization resulted in null");
            }

            return result;
        }
        catch (Exception ex)
        {
            if (ex is OperationCanceledException && cancellationToken.IsCancellationRequested)
            {
                return default(T);
            }
            await HandleExceptionAsync(ex, callerMethod);
            throw;
        }
    }

    protected async Task<bool> SendAsync(HttpRequestMessage request, [CallerMemberName] string callerMethod = "")
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        try
        {
            await ApplyAuthenticationIfRequired(request, callerMethod);

            var response = await _httpClient.SendAsync(request);

            await HandleResponseAsync(response, callerMethod);

            return true;
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(ex, callerMethod);
            throw;
        }
    }

    protected async Task<HttpRequestMessage> CreateRequestAsync(HttpMethod method, string url, object? content = null)
    {
        if (method == null) throw new ArgumentNullException(nameof(method));
        if (string.IsNullOrEmpty(url)) throw new ArgumentNullException(nameof(url));

        var request = new HttpRequestMessage(method, url);
        if (content != null)
        {
            string jsonString = JsonSerializer.Serialize(content, _jsonOptions);
            request.Content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");
        }
        return request;
    }

    private async Task ApplyAuthenticationIfRequired(HttpRequestMessage request, string callerMethod)
    {
        var method = GetType().GetMethod(callerMethod, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        var requireAuth = method?.GetCustomAttribute<RequireAuthenticationAttribute>() != null;
        var isOptionalAuth = method?.GetCustomAttribute<OptionalAuthenticationAttribute>() != null;

        var token = _jwtTokenManager.GetTokenFromCookie();

        if (!string.IsNullOrEmpty(token))
        {
            // 토큰이 있으면 무조건 인증 헤더 추가
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Trim());
        }
        else if (requireAuth)
        {
            // RequireAuthentication인데 토큰이 없으면 에러
            throw new InvalidOperationException("JWT token is required but not set");
        }
        // Optional이거나 어트리뷰트가 없는 경우는 토큰이 없어도 통과
    }

    private async Task HandleResponseAsync(HttpResponseMessage response, string callerMethod)
    {
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            var method = GetType().GetMethod(callerMethod, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var requireAuth = method?.GetCustomAttribute<RequireAuthenticationAttribute>() != null;
            var isOptionalAuth = method?.GetCustomAttribute<OptionalAuthenticationAttribute>() != null;
            var token = _jwtTokenManager.GetTokenFromCookie();

            if (requireAuth || (isOptionalAuth && !string.IsNullOrEmpty(token)))
            {
                // RequireAuth이거나, Optional이면서 토큰이 있었던 경우
                _jwtTokenManager.RemoveTokenFromCookie();
                throw new UnauthorizedAccessException("Authentication failed. Token has been removed.");
            }
        }

        response.EnsureSuccessStatusCode();
    }

    private async Task HandleExceptionAsync(Exception ex, string callerMethod)
    {
        var method = GetType().GetMethod(callerMethod, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        var requireAuth = method?.GetCustomAttribute<RequireAuthenticationAttribute>() != null;
        var isOptionalAuth = method?.GetCustomAttribute<OptionalAuthenticationAttribute>() != null;
        var token = _jwtTokenManager.GetTokenFromCookie();

        if (ex is UnauthorizedAccessException && (requireAuth || (isOptionalAuth && !string.IsNullOrEmpty(token))))
        {
            _jwtTokenManager.RemoveTokenFromCookie();
        }
    }
}