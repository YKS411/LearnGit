using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using VueMVCDemo.Models;

namespace VueMVCDemo.Controllers
{
    public class StudentController : Controller
    {
        private readonly VueMVCDemoContext _context;

        public StudentController(VueMVCDemoContext context)
        {
            _context = context;
        }

        // GET: Student
        public async Task<IActionResult> Index()
        {
            return View(await _context.StudentModel.ToListAsync());
        }

        // GET: Student/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentModel = await _context.StudentModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentModel == null)
            {
                return NotFound();
            }

            //return View(studentModel);
            return Content(JsonConvert.SerializeObject(studentModel));
        }

        // GET: Student/Create
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(StudentModel studentModel)
        {
            var student = await _context.StudentModel.FirstOrDefaultAsync(c => c.Id == studentModel.Id);
            if (student == null)
            {
                _context.Add(studentModel);  
            }
            else
            {
                student.Name = studentModel.Name;
                student.Age = studentModel.Age;
                student.Sex = studentModel.Sex;
            }
            await _context.SaveChangesAsync();

            var students = await _context.StudentModel.ToListAsync();
            //return View(studentModel);
            return Content(JsonConvert.SerializeObject(students));
        }

        // GET: Student/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentModel = await _context.StudentModel.FindAsync(id);
            if (studentModel == null)
            {
                return NotFound();
            }
            return View(studentModel);
        }

        
        [HttpPost]
        public async Task<IActionResult> Update(StudentModel studentModel)
        {
            var student = _context.StudentModel.FirstOrDefault(s => s.Id == studentModel.Id);
            if (student==null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    student.Name = studentModel.Name;
                    student.Age = studentModel.Age;
                    student.Sex = studentModel.Sex;
                    //_context.Update(studentModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentModelExists(studentModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var students = await _context.StudentModel.ToListAsync();
            // return View(studentModel);
            return Content(JsonConvert.SerializeObject(students));
        }

        // GET: Student/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentModel = await _context.StudentModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentModel == null)
            {
                return NotFound();
            }

            return View(studentModel);
        }

        // POST: Student/Delete/5
        //[HttpPost, ActionName("Delete")]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var studentModel = await _context.StudentModel.FindAsync(id);
        //    _context.StudentModel.Remove(studentModel);
        //    await _context.SaveChangesAsync();
        //    // return RedirectToAction(nameof(Index));
        //    return Content("delete success");
        //}


        [HttpPost]
        public JsonResult Delete(int Id)
        {
            var studentModel = _context.StudentModel.Find(Id);
            if (studentModel == null)
            {
                return Json("not find data!");
            }
            _context.Remove(studentModel);
            _context.SaveChanges();
            return Json("delete success");
        }

        private bool StudentModelExists(int id)
        {
            return _context.StudentModel.Any(e => e.Id == id);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _context.StudentModel.ToListAsync();
            return Content(JsonConvert.SerializeObject(students));
        }
    }
}
