using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ShopexCoreV2.Data;
using ShopexCoreV2.Models;
using System.Data;
using System.Data.SqlClient;

namespace ShopexCoreV2.Pages
{
    public class LoginModel : PageModel
    {
        public string connectionString { get; set; }

        public bool ShowInvalidLogin { get; set; }
        public LoginModel(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("sold");
        }


        public void OnGet()
        {
            if(!string.IsNullOrEmpty(Request.Query["invalid"].ToString()))
            {
                var invalid = int.Parse(Request.Query["invalid"]);
                if (invalid == 1)
                {
                    ShowInvalidLogin = true;
                }
            }
            else
            {
                ShowInvalidLogin = false;
            }
        }

        public IActionResult OnPost()
        {
            // Validate Login;
            var user = new User();
            var us = Request.Form["Username"].ToString();
            var pass = Request.Form["password"].ToString();
            var parameters = new[]
            {
               new SqlParameter("@UserName", SqlDbType.NVarChar) {Value = us},
               new SqlParameter("@password", SqlDbType.NVarChar) {Value = pass},
            };
            using (var reader = SqlHelper1.ExecuteReader(connectionString, CommandType.StoredProcedure, "ValidateUser", parameters))
            {
                user = reader.Select(r =>
                new User
                {
                    Id = (int)r["Id"],
                    UserName = r["UserName"].ToString(),
                    Passwprd = r["Password"].ToString(),
                    Email = r["Email"].ToString(),
                    Mobile = r["Mobile"].ToString(),
                    Country = r["Country"].ToString(),
                    Currency = r["Currency"].ToString(),
                    Name = r["Name"].ToString()
                }).ToList().FirstOrDefault();
            }
            if (user != null)
            {
                HttpContext.Session.SetString("User", JsonConvert.SerializeObject(user));
                return RedirectToPage("Products");

            }
            else
            {
                ShowInvalidLogin = true;
                return this.Page();
            }
        }
    }
}
