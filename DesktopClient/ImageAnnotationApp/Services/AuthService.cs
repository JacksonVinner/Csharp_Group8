using ImageAnnotationApp.Models;

namespace ImageAnnotationApp.Services
{
    public class AuthService
    {
        private readonly HttpClientService _httpClient;
        private User? _currentUser;

        public User? CurrentUser => _currentUser;
        public bool IsAuthenticated => _currentUser != null;
        public bool IsAdmin => _currentUser?.Role == "Admin";

        public AuthService()
        {
            _httpClient = HttpClientService.Instance;
        }

        public async Task<AuthResponse> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var response = await _httpClient.PostAsync<AuthResponse>("auth/login", loginDto);
                if (response != null)
                {
                    _httpClient.SetToken(response.Token);
                    _currentUser = new User
                    {
                        Username = response.Username,
                        Role = response.Role
                    };
                    return response;
                }
                throw new Exception("登录失败");
            }
            catch (Exception ex)
            {
                throw new Exception($"登录失败: {ex.Message}", ex);
            }
        }

        public async Task<AuthResponse> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                var response = await _httpClient.PostAsync<AuthResponse>("auth/register", registerDto);
                if (response != null)
                {
                    _httpClient.SetToken(response.Token);
                    _currentUser = new User
                    {
                        Username = response.Username,
                        Role = response.Role
                    };
                    return response;
                }
                throw new Exception("注册失败");
            }
            catch (Exception ex)
            {
                throw new Exception($"注册失败: {ex.Message}", ex);
            }
        }

        public void Logout()
        {
            _httpClient.SetToken(null);
            _currentUser = null;
        }

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                await _httpClient.GetAsync<object>("auth/test");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
