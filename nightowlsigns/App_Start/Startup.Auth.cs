using System;
using System.Web.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using nightowlsign.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Security;

namespace nightowlsign
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
          //AddUserAndRole(ApplicationDbContext.Create());
         //  SeedUsers();
         //   var success = AddUserAndRole(ApplicationDbContext.Create());
         //    Security security= new Security();
//security.AddUserToRole(ApplicationDbContext.Create(),"gray.pritchett@optusnet.com.au", "Admin");

        }


        internal class Security
        {
           // = new ApplicationDbContext();

            internal void AddUserToRole(ApplicationDbContext context ,string userName, string roleName)
            {
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                try
                {
                    var user = UserManager.FindByName(userName);
                    UserManager.AddToRole(user.Id, roleName);
                    context.SaveChanges();
                }
                catch
                {
                    throw;
                }
            }
        }



        private void SeedUsers()
        {

            string _role = "Admin";
            if (!Roles.RoleExists(_role))
                Roles.CreateRole(_role);

            if (!Roles.IsUserInRole("sam@elitetiger.com", _role))
                Roles.AddUserToRole("sam@elitetiger.com", _role);
            if (!Roles.IsUserInRole("ctyquin@goa.com.au", _role))
                Roles.AddUserToRole("ctyquin@goa.com.au", _role);
            if (!Roles.IsUserInRole("GClarke@bne.mcgees.com.au", _role))
                Roles.AddUserToRole("GClarke@bne.mcgees.com.au", _role);
            if (!Roles.IsUserInRole("alex@propsol.com.au", _role))
                Roles.AddUserToRole("alex@propsol.com.au", _role);
            if (!Roles.IsUserInRole("pcrooke@bretts.com.au", _role))
                Roles.AddUserToRole("pcrooke@bretts.com.au", _role);
            if (!Roles.IsUserInRole("fraser@rsaarchitects.net", _role))
                Roles.AddUserToRole("fraser@rsaarchitects.net", _role);
        }



        bool AddUserAndRole(nightowlsign.Models.ApplicationDbContext context)
        {
            IdentityResult ir;
            var rm = new RoleManager<IdentityRole>
                (new RoleStore<IdentityRole>(context));
            ir = rm.Create(new IdentityRole("Admin"));
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));



            var user = new ApplicationUser()
            {
                UserName = "sam@elitetiger.com",
                Email = "sam@elitetiger.com"
            };

            ir = um.Create(user, "P@ssword1");
            //if (ir.Succeeded == false)
            //    return ir.Succeeded;
            ir = um.AddToRole(user.Id, "Admin");

            user = new ApplicationUser()
            {
                UserName = "sam@elitetiger.com",
                Email = "sam@elitetiger.com",
            };

            ir = um.Create(user, "P@ssword1");
            //if (ir.Succeeded == false)
            //    return ir.Succeeded;
            ir = um.AddToRole(user.Id, "Admin");

            user = new ApplicationUser()
            {
                UserName = "GClarke@bne.mcgees.com.au",
                Email = "GClarke@bne.mcgees.com.au",
            };

            ir = um.Create(user, "P@ssword1");
            //if (ir.Succeeded == false)
            //    return ir.Succeeded;
            ir = um.AddToRole(user.Id, "Admin");

            user = new ApplicationUser()
            {
                UserName = "alex@propsol.com.au",
                Email = "alex@propsol.com.au",
            };

            ir = um.Create(user, "P@ssword1");
            //if (ir.Succeeded == false)
            //    return ir.Succeeded;
            ir = um.AddToRole(user.Id, "Admin");

            user = new ApplicationUser()
            {
                UserName = "pcrooke@bretts.com.au",
                Email = "pcrooke@bretts.com.au",
            };

            ir = um.Create(user, "P@ssword1");

            ir = um.AddToRole(user.Id, "Admin");


            user = new ApplicationUser()
            {
                UserName = "fraser@rsaarchitects.net",
                Email = "fraser@rsaarchitects.net",
            };

            ir = um.Create(user, "P@ssword1");

            ir = um.AddToRole(user.Id, "Admin");

            return ir.Succeeded;

        }
    }
}