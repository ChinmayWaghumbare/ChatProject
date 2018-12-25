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
    public class USERINFOController : ApiController
    {
        private ChatMasterEntities db = new ChatMasterEntities();

        // GET: api/USERINFO
        public IQueryable<USERINFO> GetUSERINFOes()
        {
            return db.USERINFOes;
        }

        // GET: api/USERINFO/5
        [ResponseType(typeof(USERINFO))]
        public async Task<IHttpActionResult> GetUSERINFO(int id)
        {
            USERINFO userInfoObj = await db.USERINFOes.FindAsync(id);
            if (userInfoObj == null)
            {
                return NotFound();
            }

            return Ok(userInfoObj);
        }

        // PUT: api/USERINFO/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUSERINFO(int id, USERINFO uSERINFO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != uSERINFO.ID)
            {
                return BadRequest();
            }

            db.Entry(uSERINFO).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //if (!USERINFOExists(id))
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

        // POST: api/USERINFO
        [ResponseType(typeof(USERINFO))]
        public async Task<IHttpActionResult> PostUSERINFO(USERINFO uSERINFO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.USERINFOes.Add(uSERINFO);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = uSERINFO.ID }, uSERINFO);
        }

        // DELETE: api/USERINFO/5
        [ResponseType(typeof(USERINFO))]
        public async Task<IHttpActionResult> DeleteUSERINFO(int id)
        {
            USERINFO uSERINFO = await db.USERINFOes.FindAsync(id);
            if (uSERINFO == null)
            {
                return NotFound();
            }

            db.USERINFOes.Remove(uSERINFO);
            await db.SaveChangesAsync();

            return Ok(uSERINFO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool USERINFOExists(string userName,int loginId)
        {
            return db.USERINFOes.Count(e => e.USER_NAME == userName && e.LOGIN_ID==loginId) > 0;
        }


        public Task<int> AddUserInfo(USERINFO newUserInfo)
        {
            //newUserInfo.USER_NAME = "User" + new Random(1000);
            int count = 1;// = db.USERINFOes.Count(s => s.USER_NAME == newUserInfo.USER_NAME);
            while (count != 0)
            {
                newUserInfo.USER_NAME = "User" + new Random().Next(5000);
                count = db.USERINFOes.Count(s => s.USER_NAME == newUserInfo.USER_NAME);
            }

            newUserInfo.LOGIN.ID = db.LOGINs.Where(s => s.UID == newUserInfo.LOGIN.UID).Select(s => s.ID).FirstOrDefault();
            if (!USERINFOExists(newUserInfo.USER_NAME, newUserInfo.LOGIN.ID))
            {
                db.Entry(newUserInfo.LOGIN).State = EntityState.Unchanged;
                db.Entry(newUserInfo).State = EntityState.Added;
                return db.SaveChangesAsync();
            }

            return Task.FromResult(0);
        }

        public Task<int> updateUserInfo([FromUri]string oldUserName,[FromBody]USERINFO value)
        {
            //var data = db.USERINFOes.Where(s => s.LOGIN.UID == value.LOGIN.UID).Select(s => s).FirstOrDefault();
            var data = db.USERINFOes.Where(s => s.USER_NAME == oldUserName).Select(s => s).FirstOrDefault();
            if (data != null)
            {
                data.USER_NAME = value.USER_NAME;

                db.Entry(data).State = EntityState.Modified;
                return db.SaveChangesAsync();
            }
            return Task.FromResult(0);
        }

        [HttpGet]
        public string getUserInfo([FromUri]string uid)
        {
            var data = db.USERINFOes.Include("LOGIN").Where(s => s.LOGIN.UID == uid).Select(s => s).FirstOrDefault();
            if (data != null)
            {
                return data.USER_NAME;
            }

            return "";
                
        }
    }
}