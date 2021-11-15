using Felix.Common.Model;
using Felix.Data.Core.UnitOfWork;
using Felix.Identity.Api.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading.Tasks;

namespace Felix.Identity.Api.Services
{
    public class UserService : IUserService
    {

        private readonly ITokenService _tokenService;

        public UserService(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<SecurityTokenModel> GetToken(UserInfoModel model)
        {
            using var uow = new UnitOfWork<SqlConnection>();
            var user = (await uow.FastCrudRepository<IdnUserEntity>().FindAsync(x => x.Where($"{nameof(IdnUserEntity.UserName):C} = @ParamName AND {nameof(IdnUserEntity.UserToken):C} = @ParamPassword")
            .WithParameters(new { ParamName = model.UserName, ParamPassword = model.Password }))).FirstOrDefault();

            if (user == null)
                throw new CommonException("Invalid User");

            var roles = new string[] { "ADMIN", "USER" };

            return await _tokenService.GeneterateJwtToken(new TokenModel { Code = model.UserName, Roles = roles.ToList() });
        }

        public async Task<string> FindLdapDirectoryName(string name)
        {
            foreach (var domainName in new[] { "xxx.xx", "yyy.yyyy" })
            {
                using var principalContext = new PrincipalContext(ContextType.Domain, domainName);
                var user = await Task.Run(() => UserPrincipal.FindByIdentity(principalContext, IdentityType.Name, name));
                return user != null ? user.SamAccountName + user.Name : "Name yada sam";
            }

            return "";
        }
    }
}
