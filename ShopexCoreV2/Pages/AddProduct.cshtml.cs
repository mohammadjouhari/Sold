using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ShopexCoreV2.Data;
using ShopexCoreV2.Models;
using System.Data;
using System.Data.SqlClient;

namespace ShopexCoreV2.Pages
{
    public class AddProductModel : PageModel
    {
        public string connectionString { get; set; }

        [BindProperty]
        public IFormFile ImagePath { get; set; }
        public AddProductModel(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("sold");
        }
        public void OnGet()
        {
        }

        public void OnPost()
        {
            var user = new User();
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
            {
               user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));
            }
            else
            {
                user.Id = -1;
            }
            var product = new Product();
            product.Title = Request.Form["Title"].ToString();
            product.Description = Request.Form["Description"].ToString();
            product.Price = int.Parse(Request.Form["Price"].ToString());
            product.Model = Request.Form["Model"].ToString();
            product.Type = Request.Form["Type"].ToString();
            // Svae Image in wrrot;
            var fileName = Path.GetFileName(ImagePath.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\ProductImages", fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                ImagePath.CopyTo(fileStream);
            }
            var parameters = new[]
            {
               new SqlParameter("@Title", SqlDbType.NVarChar) {Value = product.Title},
               new SqlParameter("@Description", SqlDbType.NVarChar) {Value = product.Description},
               new SqlParameter("@Price", SqlDbType.Int) {Value = product.Price},
               new SqlParameter("@ModelNumber", SqlDbType.NVarChar) {Value = product.Model},
               new SqlParameter("@Type", SqlDbType.NVarChar) {Value = product.Type},
               new SqlParameter("@ImagePath", SqlDbType.NVarChar) {Value = ImagePath.FileName},
               new SqlParameter("@UserId", SqlDbType.Int) {Value = user.Id},

            };
            SqlHelper1.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, "InsertProduct", parameters);
            Response.Redirect("Products");
        }
    }
}
