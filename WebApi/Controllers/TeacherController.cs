using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreUniversity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCoreUniversity.Data.Models;

namespace NetCoreUniversity.WebApi.Controllers {

    [Produces("application/json")]
    [Route("api/teacher")]
    public class TeacherController : Controller {

        private readonly IDataRepositories repositories;
        private readonly IPersonRepository<Teacher> teacherRepo;

        public TeacherController(IDataRepositories repositories) {
            this.repositories = repositories;
            this.teacherRepo = repositories.TeacherRepository();
        }

        [HttpGet]
        public async Task<ICollection<Teacher>> GetAll() {
            var all = await this.teacherRepo.GetAllAsync();

            return all;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) {
            var item = await this.teacherRepo.GetByIdAsync(id);
            if (item == null) {
                return NotFound();
            }

            return this.Ok(item);
        }

        [HttpGet("{id}/classes")]
        public async Task<IActionResult> GetClasses(int id) {
            var item = await this.teacherRepo.GetByIdAsync(id);
            if (item == null) {
                return this.NotFound();
            }
            await this.teacherRepo.LoadClassesAsync(item);

            return this.Ok(item.Classes);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Teacher teacher) {
            teacher.PersonId = 0;
            if (!this.ModelState.IsValid) {
                return this.BadRequest(this.ModelState);
            }
            this.teacherRepo.Insert(teacher);
            await this.repositories.SaveAsync();

            return this.CreatedAtAction("Get", new { id = teacher.PersonId }, teacher);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Teacher teacher) {
            if (!this.ModelState.IsValid) {
                return this.BadRequest(this.ModelState);
            }
            this.teacherRepo.Patch(teacher);

            try {
                await this.repositories.SaveAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!await this.teacherRepo.ExistsAsync(teacher.PersonId)) {
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

            var teacher = await this.teacherRepo.GetByIdAsync(id);
            if (teacher == null) {
                return this.NotFound();
            }

            this.teacherRepo.Delete(teacher);

            await this.repositories.SaveAsync();

            return Ok(teacher);
        }

    }
}