using System;
using System.CodeDom.Compiler;
using System.Security.Cryptography.X509Certificates;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace Services
{
    public class IdentityService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public IdentityService(AppDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
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

    }
}