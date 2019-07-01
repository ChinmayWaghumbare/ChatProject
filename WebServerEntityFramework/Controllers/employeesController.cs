
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebServerEntityFramework.Models;

namespace WebServerEntityFramework.Controllers
{
    public class employeesController : ApiController
    {
        private ChatMasterEntities db = new ChatMasterEntities();

        // GET: api/employees
        public IQueryable<employee> Getemployees()
        {
            return db.employees;
        }

        // GET: api/employees/5
        [ResponseType(typeof(employee))]
        public async Task<IHttpActionResult> Getemployee(string id)
        {
            employee employee = await db.employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        // PUT: api/employees/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putemployee(string id, employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employee.code)
            {
                return BadRequest();
            }

            db.Entry(employee).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!employeeExists(id))
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

        // POST: api/employees
        [ResponseType(typeof(employee))]
        public async Task<IHttpActionResult> Postemployee(employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.employees.Add(employee);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (employeeExists(employee.code))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = employee.code }, employee);
        }

        // DELETE: api/employees/5
        [ResponseType(typeof(employee))]
        public async Task<IHttpActionResult> Deleteemployee(string id)
        {
            employee employee = await db.employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            db.employees.Remove(employee);
            await db.SaveChangesAsync();

            return Ok(employee);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool employeeExists(string id)
        {
            return db.employees.Count(e => e.code == id) > 0;
        }
    }
}