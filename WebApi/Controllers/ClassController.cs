using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreUniversity.Data;
using System.Net.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NetCoreUniversity.Data.Models;

namespace NetCoreUniversity.WebApi.Controllers {

    [Produces("application/json")]
    [Route("api/class")]
    public class ClassController : Controller {

        private readonly IDataRepositories repositories;
        private readonly IClassRepository classRepo;

        public ClassController(IDataRepositories repositories) {
            this.repositories = repositories;
            this.classRepo = repositories.ClassRepository();
        }

        [HttpGet]
        public async Task<ICollection<Class>> GetAll() {
            var all = await this.classRepo.GetAllAsync();

            // Note: response code is always 200
            return all;
        }

        [HttpGet("{classId}")]
        public async Task<IActionResult> Get(int classId) {
            var item = await this.classRepo.GetByIdAsync(classId);
            if (item == null) {
                return this.NotFound();
            }

            return this.Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Class c) {
            c.ClassId = 0;
            if (!this.ModelState.IsValid) {
                return this.BadRequest(this.ModelState);
            }

            this.classRepo.Insert(c);
            await this.repositories.SaveAsync();

            return this.CreatedAtAction("Get", new { classId = c.ClassId }, c);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Class c) {
            if (!this.ModelState.IsValid) {
                return this.BadRequest(this.ModelState);
            }
            this.classRepo.Patch(c);

            try {
                await this.repositories.SaveAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!await this.classRepo.ExistsAsync(c.ClassId)) {
                    return this.NotFound();
                }
                throw;
            }

            return this.NoContent();
        }

        [HttpDelete("{classId}")]
        public async Task<IActionResult> Delete(int classId) {
            if (!this.ModelState.IsValid) {
                return this.BadRequest(this.ModelState);
            }

            var c = await this.classRepo.GetByIdAsync(classId);
            if (c == null) {
                return this.NotFound();
            }

            this.classRepo.Delete(c);

            await this.repositories.SaveAsync();

            return this.Ok(c);
        }

        [HttpGet("{classId}/students")]
        public async Task<IActionResult> GetStudents(int classId) {
            var item = await this.classRepo.GetByIdAsync(classId);
            if (item == null) {
                return this.NotFound();
            }
            await this.classRepo.LoadStudentsAsync(item);

            return this.Ok(item.Students);
        }

        /// <summary>
        /// Adds a student to this class. Adds an existing student if only 
        /// PersonId is given, otherwise attempts to 
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        [HttpPost("{classId}/students")]
        public async Task<IActionResult> AddStudent(int classId,
            [FromBody] Student student) {
            var item = await this.classRepo.GetByIdAsync(classId);
            if (item == null) {
                return this.NotFound();
            }
            await this.classRepo.LoadStudentsAsync(item);

            return this.Ok(item.Students);
        }

    }
}
