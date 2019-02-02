using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class TasksController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Tasks
        public IQueryable<Task> GetTasks()
        {
            string owner = HttpContext.Current.User.Identity.Name;
            return db.Tasks.Where(_task => _task.OwnerId == owner);

        }

        // GET: api/Tasks/5
        [ResponseType(typeof(Task))]
        public IHttpActionResult GetTask(int id)
        {
            string owner = HttpContext.Current.User.Identity.Name;
            Task task = db.Tasks.FirstOrDefault(_task => _task.Id == id && _task.OwnerId == owner);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        // PUT: api/Tasks/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTask(int id, Task task)
        {
            string owner = HttpContext.Current.User.Identity.Name;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (db.Tasks.AsNoTracking().FirstOrDefault(_task => _task.Id == id && _task.OwnerId == owner) == null)
            {
                return BadRequest();
            }
            if (id != task.Id)
            {
                return BadRequest();
            }
            task.OwnerId = owner;
            db.Entry(task).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Tasks
        [ResponseType(typeof(Task))]
        public IHttpActionResult PostTask(Task task)
        {
            string owner = HttpContext.Current.User.Identity.Name;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            task.OwnerId = owner;
            db.Tasks.Add(task);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = task.Id }, task);
        }

        // DELETE: api/Tasks/5
        [ResponseType(typeof(Task))]
        public IHttpActionResult DeleteTask(int id)
        {
            string owner = HttpContext.Current.User.Identity.Name;
            Task task = db.Tasks.FirstOrDefault(_task => _task.Id == id && _task.OwnerId == owner);
            if (task == null)
            {
                return NotFound();
            }

            db.Tasks.Remove(task);
            db.SaveChanges();

            return Ok(task);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TaskExists(int id)
        {
            return db.Tasks.Count(e => e.Id == id) > 0;
        }
    }
}