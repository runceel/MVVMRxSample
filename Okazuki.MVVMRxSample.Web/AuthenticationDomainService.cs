namespace Okazuki.MVVMRxSample.Web
{
    using System.Security.Principal;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server.ApplicationServices;
    using System.Threading;

    [EnableClientAccess]
    public class AuthenticationDomainService : AuthenticationBase<User>
    {
        protected override bool ValidateUser(string userName, string password)
        {
            // ログインに時間がかかるわ
            Thread.Sleep(1000);
            return userName == "user001" && password == "user001";
        }

        protected override User GetAuthenticatedUser(IPrincipal principal)
        {
            var user = base.GetAuthenticatedUser(principal);
            user.UserName = "山田　太郎";
            user.UserDepartment = "無職";
            return user;
        }
    }

    public class User : UserBase
    {
        [ProfileUsage(IsExcluded = true)]
        public string UserName { get; set; }

        [ProfileUsage(IsExcluded = true)]
        public string UserDepartment { get; set; }
    }
}