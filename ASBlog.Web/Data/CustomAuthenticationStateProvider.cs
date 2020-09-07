using ASBlog.Web.Resources;
using ASBlog.Web.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ASBlog.Web.Data
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        public ILocalStorageService _localStorageService { get; }
        public IUserService _userService { get; set; }
        private readonly HttpClient _httpClient;

        public CustomAuthenticationStateProvider(ILocalStorageService localStorageService,
            IUserService userService,
            HttpClient httpClient)
        {
            //throw new Exception("CustomAuthenticationStateProviderException");
            _localStorageService = localStorageService;
            _userService = userService;
            _httpClient = httpClient;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var accessToken = await _localStorageService.GetItemAsync<string>("accessToken");
            ClaimsIdentity identity;

            if (!string.IsNullOrEmpty(accessToken))
            {
                UserRs user = await _userService.GetUserAsync();
                identity = GetClaimsIdentity(user);
            }
            else
            {
                identity = new ClaimsIdentity();
            }

            var claimsPrincipal = new ClaimsPrincipal(identity);

            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }

        public async Task MarkUserAsAuthenticated(LoginRs loginRs)
        {
            await _localStorageService.SetItemAsync("accessToken", loginRs.accessToken);

            UserRs user = await _userService.GetUserAsync();

            var identity = GetClaimsIdentity(user);

            var claimsPrincipal = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public async Task MarkUserAsLoggedOut()
        {
            await _localStorageService.RemoveItemAsync("accessToken");

            var identity = new ClaimsIdentity();

            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        private ClaimsIdentity GetClaimsIdentity(UserRs user)
        {
            var claimsIdentity = new ClaimsIdentity();

            if (user.email != null)
            {
                claimsIdentity = new ClaimsIdentity(new[]
                                {
                                    new Claim(ClaimTypes.NameIdentifier, user.email),
                                    new Claim(ClaimTypes.Name, user.firstName + " " + user.lastName)
                                }, "apiauth_type");

                var roleClaims = user.roles.Select(r => new Claim(ClaimTypes.Role, r));
                claimsIdentity.AddClaims(roleClaims);
            }

            return claimsIdentity;

        }
    }
}
