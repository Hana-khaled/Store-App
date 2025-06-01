using Microsoft.AspNetCore.Identity;
using Store.Data.Entities.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository
{
    public class StoreIdentityContextSeed
    {
        public static async Task UserSeedAsync(UserManager<AppUser> userManager)
        {
            if(userManager != null && !userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Hana Khaled",
                    Email = "hana.khaled@gmail.com",
                    UserName = "hanaKhaled",
                    Address = new Address
                    {
                        FirstName="Hana",
                        LastName = "Khaled",
                        City = "Hadayk elqobah",
                        State = "Cairo",
                        Street = "Makka st",
                        PostalCode = "12345"
                        
                    }
                };
                await userManager.CreateAsync(user, "Password123@");
            }
        }
    }
}
