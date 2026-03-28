using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models;
using System.Collections.Generic;
using System.Linq;

namespace StudentManagementSystem.Controllers
{
    public class StudentsController : Controller
    {
        private static List<Student> students = new List<Student>();

        public IActionResult Index()
        {
            return View(students);
        }

        public IActionResult Create()
        {
            return PartialView("_StudentForm", new Student());
        }

        [HttpPost]
        public IActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                student.Id = students.Count + 1;
                students.Add(student);
                return RedirectToAction("Index");
            }
            return PartialView("_StudentForm", student);
        }

        public IActionResult Edit(int id)
        {
            var student = students.FirstOrDefault(x => x.Id == id);
            return PartialView("_StudentForm", student);
        }

        [HttpPost]
        public IActionResult Edit(Student student)
        {
            var existing = students.FirstOrDefault(x => x.Id == student.Id);

            if (existing != null)
            {
                existing.Name = student.Name;
                existing.Email = student.Email;
                existing.Course = student.Course;
                existing.JoiningDate = student.JoiningDate;
            }

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var student = students.FirstOrDefault(x => x.Id == id);
            if (student != null)
                students.Remove(student);

            return RedirectToAction("Index");
        }
    }
}