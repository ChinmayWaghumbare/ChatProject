using System;
using System.Collections.Generic;
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
    public class ONLINEUSERController : ApiController
    {
        private ChatMasterEntities db = new ChatMasterEntities();

        // GET: api/ONLINEUSER
        public IQueryable<ONLINEUSER> GetONLINEUSERS()
        {
            return db.ONLINEUSERS;
        }

        // GET: api/ONLINEUSER/5
        [ResponseType(typeof(ONLINEUSER))]
        public async Task<IHttpActionResult> GetONLINEUSER(int id)
        {
            ONLINEUSER oNLINEUSER = await db.ONLINEUSERS.FindAsync(id);
            if (oNLINEUSER == null)
            {
                return NotFound();
            }

            return Ok(oNLINEUSER);
        }

        // PUT: api/ONLINEUSER/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutONLINEUSER(int id, ONLINEUSER oNLINEUSER)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != oNLINEUSER.ID)
            {
                return BadRequest();
            }

            db.Entry(oNLINEUSER).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //if (!ONLINEUSERExists(id))
                //{
                //    return NotFound();
                //}
                //else
                //{
                //    throw;
                //}
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ONLINEUSER
        [ResponseType(typeof(ONLINEUSER))]
        public async Task<IHttpActionResult> PostONLINEUSER(ONLINEUSER oNLINEUSER)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ONLINEUSERS.Add(oNLINEUSER);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                //if (ONLINEUSERExists(oNLINEUSER.ID))
                //{
                //    return Conflict();
                //}
                //else
                //{
                //    throw;
                //}
            }

            return CreatedAtRoute("DefaultApi", new { id = oNLINEUSER.ID }, oNLINEUSER);
        }

        // DELETE: api/ONLINEUSER/5
        [ResponseType(typeof(ONLINEUSER))]
        public async Task<IHttpActionResult> DeleteONLINEUSER(int id)
        {
            ONLINEUSER oNLINEUSER = await db.ONLINEUSERS.FindAsync(id);
            if (oNLINEUSER == null)
            {
                return NotFound();
            }

            db.ONLINEUSERS.Remove(oNLINEUSER);
            await db.SaveChangesAsync();

            return Ok(oNLINEUSER);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ONLINEUSERExists(string userName)
        {
            return db.ONLINEUSERS.Count(s => s.USERINFO.USER_NAME == userName) > 0;
        }


        public Task<int>SetOnline(string userName)
        {
            int data = db.ONLINEUSERS.Where(s => s.USERINFO.USER_NAME == userName).Count();
            if (data == 0)
            {
                ONLINEUSER newObj = new ONLINEUSER();
                newObj.USERID = db.USERINFOes.Where(s => s.USER_NAME == userName).Select(s => s.ID).FirstOrDefault();
                db.Entry(newObj).State = EntityState.Added;
                return db.SaveChangesAsync();
            }
            return Task.FromResult(0);
        }

        [HttpDelete]
        public Task<int> RemoveOnline(string userName)
        {
            var data = db.ONLINEUSERS.Where(s => s.USERINFO.USER_NAME == userName).Select(s=>s).FirstOrDefault();
            if (data != null)
            {
                db.Entry(data).State = EntityState.Deleted;
                return db.SaveChangesAsync();
            }
            return Task.FromResult( 0);
            //int data = db.ONLINEUSERS.Where(s => s.USERINFO.USER_NAME == userName).Count();
            //if (data != 0)
            //{
            //    ONLINEUSER Obj = new ONLINEUSER();
            //    int userId = db.USERINFOes.Where(s => s.USER_NAME == userName).Select(s => s.ID).FirstOrDefault();
            //    Obj.USERID = userId;


            //    //db.ONLINEUSERS.SqlQuery("Delete FROM ONLINEUSERS");


            //    db.Entry(Obj).State = EntityState.Deleted;
            //    db.SaveChangesAsync();
            //    return 1;
            //}
            //return 0;
        }
    }
}