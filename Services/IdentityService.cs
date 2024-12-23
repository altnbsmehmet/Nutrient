using System;
using System.CodeDom.Compiler;
using System.Security.Cryptography.X509Certificates;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;


namespace Services
{
    public class IdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IdentityService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
        }

        public async Task<SignInResult> SignInAsync(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
                return result;
            }
            return SignInResult.Failed;
        }

        public async Task<IdentityResult> SignUpAsync(string email, string username, string password, string firstname, string lastname, string gender, double height, double weight)
        {
            var user = new ApplicationUser { UserName = username, Email = email, FirstName = firstname, LastName = lastname, Gender = gender, Height = height, Weight = weight};
            var result = await _userManager.CreateAsync(user, password);
            return result;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task Update(string firstname, string lastname, double height, double weight)
        {
            var user = await GetCurrentUserAsync();

            user.FirstName = firstname;
            user.LastName = lastname;
            user.Height = height;
            user.Weight = weight;

            await _userManager.UpdateAsync(user);
        }

    }
}