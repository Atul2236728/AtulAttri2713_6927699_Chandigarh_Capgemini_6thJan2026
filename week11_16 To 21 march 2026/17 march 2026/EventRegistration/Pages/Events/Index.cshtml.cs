using EventRegistration.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace EventRegistration.Pages.Events
{
    public class IndexModel : PageModel
    {
        public List<Registration> Registrations { get; set; } = new List<Registration>();

        public void OnGet()
        {
            Registrations = RegisterModel.registrations;
        }

        public IActionResult OnPostDelete(int id)
        {
            var item = RegisterModel.registrations.FirstOrDefault(x => x.Id == id);

            if (item != null)
            {
                RegisterModel.registrations.Remove(item);
            }

            return RedirectToPage();
        }
    }
}