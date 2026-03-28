using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EventRegistration.Models;
using System.Collections.Generic;

namespace EventRegistration.Pages.Events
{
    public class RegisterModel : PageModel
    {
        public static List<Registration> registrations = new List<Registration>();

        [BindProperty]
        public Registration Registration { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            Registration.Id = registrations.Count + 1;
            registrations.Add(Registration);

            return RedirectToPage("Index");
        }
    }
}