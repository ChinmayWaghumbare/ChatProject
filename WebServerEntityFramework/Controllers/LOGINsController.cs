using System;
using System.Collections.Generic;
using System.Data;
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
    public class LOGINsController : ApiController
    {
        private ChatMasterEntities db = new ChatMasterEntities();

        // GET: api/LOGINs
        public IQueryable<LOGIN> GetLOGINs()
        {
            return db.LOGINs;
        }

        // GET: api/LOGINs/5
        [ResponseType(typeof(LOGIN))]
        public async Task<IHttpActionResult> GetLOGIN(int id)
        {
            LOGIN lOGIN = await db.LOGINs.FindAsync(id);
            if (lOGIN == null)
            {
                return NotFound();
            }

            return Ok(lOGIN);
        }

        // PUT: api/LOGINs/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutLOGIN(int id, LOGIN lOGIN)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != lOGIN.ID)
            {
                return BadRequest();
            }

            db.Entry(lOGIN).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LOGINExists(id))
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

        // POST: api/LOGINs
        [ResponseType(typeof(LOGIN))]
        public async Task<IHttpActionResult> PostLOGIN(LOGIN lOGIN)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.LOGINs.Add(lOGIN);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = lOGIN.ID }, lOGIN);
        }

        // DELETE: api/LOGINs/5
        [ResponseType(typeof(LOGIN))]
        public async Task<IHttpActionResult> DeleteLOGIN(int id)
        {
            LOGIN lOGIN = await db.LOGINs.FindAsync(id);
            if (lOGIN == null)
            {
                return NotFound();
            }

            db.LOGINs.Remove(lOGIN);
            await db.SaveChangesAsync();

            return Ok(lOGIN);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LOGINExists(int id)
        {
            return db.LOGINs.Count(e => e.ID == id) > 0;
        }


        public Task<int> AddNewUser(LOGIN newUser)
        {
            int count = db.LOGINs.Count(s => s.UID == newUser.UID);
            if (count > 0)
            {
                throw new Exception("UserId already exists.");
            }
            else
            {
                db.LOGINs.Add(newUser);
                return db.SaveChangesAsync();
            }

        }

        public int Login(LOGIN lgn)
        {
            int count = db.LOGINs.Count(s => s.UID == lgn.UID && s.UPWD == lgn.UPWD);

            var data = db.LOGINs.Where(s => s.UID == lgn.UID && s.UPWD == lgn.UPWD).Select(s =>s);
            LOGIN l = data.ToList<LOGIN>()[0];
            if (l!=null)
            {
                return l.ID;    
            }

            return 0;
        }

        public int deleteUser(string uid)
        {
            int count = db.LOGINs.Count(s => s.UID == uid);
            if (count == 1)
            {
                db.LOGINs.SqlQuery("DELETE FROM LOGIN WHERE UID=@p0",uid);
                return 1;
            }
            return 0;
        }

        public void updatePwd(LOGIN value,string old_pwd)
        {
            var data = db.LOGINs.Where(s => s.UID == value.UID && s.UPWD == old_pwd).Select(s => s);
            LOGIN l = data.ToList<LOGIN>()[0];

            l.UPWD = old_pwd;
            db.Entry(l).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}