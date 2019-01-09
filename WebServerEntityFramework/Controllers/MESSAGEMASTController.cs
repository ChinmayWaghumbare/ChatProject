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
    public class MESSAGEMASTController : ApiController
    {
        private ChatMasterEntities db = new ChatMasterEntities();

        // GET: api/MESSAGEMAST
        public IQueryable<MESSAGEMAST> GetMESSAGEMASTs()
        {
            return db.MESSAGEMASTs;
        }

        // GET: api/MESSAGEMAST/5
        [ResponseType(typeof(MESSAGEMAST))]
        public async Task<IHttpActionResult> GetMESSAGEMAST(int id)
        {
            MESSAGEMAST mESSAGEMAST = await db.MESSAGEMASTs.FindAsync(id);
            if (mESSAGEMAST == null)
            {
                return NotFound();
            }

            return Ok(mESSAGEMAST);
        }

        // PUT: api/MESSAGEMAST/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMESSAGEMAST(int id, MESSAGEMAST mESSAGEMAST)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != mESSAGEMAST.ID)
            {
                return BadRequest();
            }

            db.Entry(mESSAGEMAST).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MESSAGEMASTExists(id))
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

        // POST: api/MESSAGEMAST
        [ResponseType(typeof(MESSAGEMAST))]
        public async Task<IHttpActionResult> PostMESSAGEMAST(MESSAGEMAST mESSAGEMAST)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MESSAGEMASTs.Add(mESSAGEMAST);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = mESSAGEMAST.ID }, mESSAGEMAST);
        }

        // DELETE: api/MESSAGEMAST/5
        [ResponseType(typeof(MESSAGEMAST))]
        public async Task<IHttpActionResult> DeleteMESSAGEMAST(int id)
        {
            MESSAGEMAST mESSAGEMAST = await db.MESSAGEMASTs.FindAsync(id);
            if (mESSAGEMAST == null)
            {
                return NotFound();
            }

            db.MESSAGEMASTs.Remove(mESSAGEMAST);
            await db.SaveChangesAsync();

            return Ok(mESSAGEMAST);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MESSAGEMASTExists(int id)
        {
            return db.MESSAGEMASTs.Count(e => e.ID == id) > 0;
        }


        public Task<int> AddNewMsg(MESSAGEMAST value)
        {
            value.FROM_USER = db.USERINFOes.Where(s => s.USER_NAME == value.USERINFO.USER_NAME).Select(s => s.ID).FirstOrDefault();
            value.TO_USER = db.USERINFOes.Where(s => s.USER_NAME == value.USERINFO1.USER_NAME).Select(s => s.ID).FirstOrDefault();
            if (value.MSG.Length >250)
            {
                value.PARTIAL = true;
            }
            else
            {
                value.PARTIAL = false;
            }
            value.DELIVERED = false;
            value.SENDTIME = DateTime.Now;

            db.Entry(value).State = EntityState.Added;
            return db.SaveChangesAsync();
        }

        public IQueryable<MESSAGEMAST> SendMsg(string fromUserName,string toUserName)
        {
            var data = db.MESSAGEMASTs.Where(s => s.USERINFO.USER_NAME == fromUserName && s.USERINFO1.USER_NAME == toUserName && s.DELIVERED==false).Select(s => s);
                return data;
        }

        public void UpdateMsg(MESSAGEMAST value)
        {

            value.DELIVERED = true;
            db.Entry(value).State = EntityState.Modified;
            db.SaveChangesAsync();
        }

        ////Use this when we want all messageList by using UserInfo Model
        //public IQueryable<ICollection<MESSAGEMAST>> getMessages(string userName)
        //{   
        //    return db.USERINFOes.Where(s => s.USER_NAME == userName).Select(s => s.MESSAGEMASTs1);
        //}

        public List<MESSAGEMAST> getMessages(string userName)
        {
            return db.MESSAGEMASTs.Include(s => s.USERINFO).Include(s => s.USERINFO1).Where(s => s.USERINFO1.USER_NAME == userName).Select(s => s).ToList();
        }

        public void getMessageList(string userName)
        {
            var data = db.MESSAGEMASTs.Where(s => s.USERINFO1.USER_NAME == userName).GroupBy(s => s.USERINFO.USER_NAME).Select(s=> s).ToList();

        }
    }
}