using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using WebApplicationExcoticMandi.Models;

[assembly: OwinStartupAttribute(typeof(WebApplicationExcoticMandi.Startup))]
namespace WebApplicationExcoticMandi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateUserAndRoles();
        }
        public void CreateUserAndRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if(!roleManager.RoleExists("Admin"))
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                ApplicationUser applicationUser = new ApplicationUser();
                applicationUser.Email = "pandyahimanshu09@gmail.com";
                applicationUser.UserName = applicationUser.Email;
                string password = "kailash7568335";

                IdentityResult status = UserManager.Create(applicationUser, password);
                if(status.Succeeded)
                    {
                        UserManager.AddToRole(applicationUser.Id, "Admin");
                    }

            }
            if(!roleManager.RoleExists("Customer"))
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Customer";
                roleManager.Create(role);
            }
        }
    }
}
