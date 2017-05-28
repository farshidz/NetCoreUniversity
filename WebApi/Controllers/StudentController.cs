using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreUniversity.Data;
using System.Net.Security;
using Microsoft.EntityFrameworkCore;
using NetCoreUniversity.Data.Models;

namespace NetCoreUniversity.WebApi.Controllers {

    [Produces("application/json")]
    [Route("api/student")]
    public class StudentController : Controller {

        private readonly IDataRepositories repositories;
        private readonly IPersonRepository<Student> studentRepo;

        public StudentController(IDataRepositories repositories) {
            this.repositories = repositories;
            this.studentRepo = repositories.StudentRepository();
        }

        [HttpGet]
        public async Task<ICollection<Student>> GetAll() {
            var all = await this.studentRepo.GetAllAsync();

            return all;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) {
            var item = await this.studentRepo.GetByIdAsync(id);
            if (item == null) {
                return NotFound();
            }


            return this.Ok(item);
        }

        [HttpGet("{id}/classes")]
        public async Task<IActionResult> GetClasses(int id) {
            var item = await this.studentRepo.GetByIdAsync(id);
            if (item == null) {
                return this.NotFound();
            }
            await this.studentRepo.LoadClassesAsync(item);

            return this.Ok(item.Classes);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Student student) {
            student.PersonId = 0;
            if (!this.ModelState.IsValid) {
                return this.BadRequest(this.ModelState);
            }
            this.studentRepo.Insert(student);
            await this.repositories.SaveAsync();

            return this.CreatedAtAction("Get", new { id = student.PersonId }, student);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Student student) {
            if (!this.ModelState.IsValid) {
                return this.BadRequest(this.ModelState);
            }
            this.studentRepo.Patch(student);

            try {
                await this.repositories.SaveAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!await this.studentRepo.ExistsAsync(student.PersonId)) {
                    return this.NotFound();
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {
            if (!this.ModelState.IsValid) {
                return this.BadRequest(this.ModelState);
            }

            var student = await this.studentRepo.GetByIdAsync(id);
            if (student == null) {
                return this.NotFound();
            }

            this.studentRepo.Delete(student);

            await this.repositories.SaveAsync();

            return Ok(student);
        }

    }
}
