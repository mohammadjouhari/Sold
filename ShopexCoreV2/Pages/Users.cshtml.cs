using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopexCoreV2.Data;
using ShopexCoreV2.Models;
using System.Data;

namespace ShopexCoreV2.Pages
{
    public class UsersModel : PageModel
    {
        public string connectionString { get; set; }
        public UsersModel(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("sold");
        }
        [BindProperty]
        public List<User> users { get; set; }


        public IActionResult OnGet()
        {
            var user = HttpContext.Session.Get("User");
            if (user == null)
            {
                return RedirectToPage("Login");
            }
            else
            {
                users = new List<User>();
                using (var reader = SqlHelper1.ExecuteReader(connectionString, CommandType.Text, "SELECT * FROM Users", null))
                {
                    users = reader.Select(r =>
                    new User
                    {
                        Id = (int)r["Id"],
                        Email = r["Email"].ToString(),
                        Mobile = r["Mobile"].ToString(),
                        Passwprd = r["Password"].ToString(),
                    }).ToList();
                }
                return this.Page();
            }
        }
    }
}
