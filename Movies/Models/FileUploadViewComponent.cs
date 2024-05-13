using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace Movies.Models
{
    public class FileUploadViewComponent : ViewComponent
    {

        [HttpPost]
        public async Task<IViewComponentResult> InvokeAsync(IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string uploads = Path.Combine(Environment.CurrentDirectory, "~/images/" + file.FileName);
                    if (file.Length > 0)
                    {
                        string filePath = Path.Combine(uploads, file.FileName);
                        using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                    }
                }



            }
            return View();
        }
    }
}
