using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Models.User;
using System.Security.Claims;

namespace Logic.Shared
{
    public abstract class LogicBase
    {
        private readonly HttpContext _context;
        private readonly IUserUnitOfWork _userUnitOfWork;
        private UserModel? _currentUser;

        public UserModel? CurrentUser => _currentUser;

        protected LogicBase(IHttpContextAccessor httpContextAccessor, IUserUnitOfWork userUnitOfWork)
        {
            _context = httpContextAccessor.HttpContext;
            _userUnitOfWork = userUnitOfWork;
            Task.Run(async () => await LoadCurrentUser()).Wait();
        }

        private async Task LoadCurrentUser()
        {
            var claims = _context.User.Claims;
            
            var emailAddress = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(emailAddress))
            {
                throw new UnauthorizedAccessException();
            }

            _currentUser = await _userUnitOfWork.GetUserByEmail(emailAddress, true, false);
        }
    }
}
