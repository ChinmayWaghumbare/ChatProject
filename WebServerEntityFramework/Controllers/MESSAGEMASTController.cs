using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
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




        public IQueryable<MESSAGEMAST> SendMsg(string fromUserName, string toUserName)
        {
            var data = db.MESSAGEMASTs.Where(s => s.USERINFO.USER_NAME == fromUserName && s.USERINFO1.USER_NAME == toUserName && s.DELIVERED == false).OrderBy(s=>s.SENDTIME).Select(s => s);
            return data;
        }

        //public void sendMsg(string toUser)
        //{

        //}
        [HttpPut]
        public void UpdateMsg(MessageData value)
        {
            DateTime lastMsgTime = (DateTime)value.mesg.SENDTIME;


            var data = db.MESSAGEMASTs.Where(s => ((s.USERINFO.USER_NAME == value.fromUser & s.USERINFO1.USER_NAME == value.toUser) )  & s.SENDTIME <= lastMsgTime & s.DELIVERED==false)
                                    .Select(s => s)
                                    .ToList();

            foreach (MESSAGEMAST msg in data)
            {
                msg.DELIVERED = true;
                db.Entry(msg).State = EntityState.Modified;

                db.SaveChanges();
            }
        }

        ////Use this when we want all messageList by using UserInfo Model
        //public IQueryable<ICollection<MESSAGEMAST>> getMessages(string userName)
        //{   
        //    return db.USERINFOes.Where(s => s.USER_NAME == userName).Select(s => s.MESSAGEMASTs1);
        //}

        public List<MESSAGEMAST> getMessages1(string userName)
        {
            return db.MESSAGEMASTs.Include(s => s.USERINFO).Include(s => s.USERINFO1).Where(s => s.USERINFO1.USER_NAME == userName).Select(s => s).ToList();
        }

        public List<MessageList> getMessageList(string userName)
        {
            #region old code
            //List<MessageList> msgList = new List<MessageList>();
            //var data = db.MESSAGEMASTs.Where(s => s.USERINFO1.USER_NAME == userName).GroupBy(s => s.USERINFO.USER_NAME).Select(s=> s).ToList();
            //foreach (var result in data)
            //{
            //    MessageList msg = new MessageList();
            //    msg.name = result.Key;
            //    msg.count = 0;
            //    foreach (var msgData in result)
            //    {
            //        if(msgData.DELIVERED==false)
            //           msg.count++;
            //    }
            //    msgList.Add(msg);   
            //}

            //return msgList;
            #endregion

            return (db.MESSAGEMASTs.Where(s => s.USERINFO1.USER_NAME == userName & s.DELIVERED==false)
                            .GroupBy(s => s.USERINFO.USER_NAME)
                            .Select(group => new
                            {
                                msg_count = group.Count(),
                                from_user = group.Key
                            })
            .Union(db.MESSAGEMASTs.Where(s => s.USERINFO.USER_NAME == userName)
                                    .GroupBy(s => s.USERINFO1.USER_NAME)
                                    .Select(group => new
                                    {
                                        msg_count = 0,
                                        from_user = group.Key
                                    }))
            ).GroupBy(s => s.from_user)
            .Select(s => new MessageList
            {
                count = s.Sum(s1 => s1.msg_count),
                name = s.Key
            }).ToList();

            
        }

        [HttpGet]
        public IEnumerable<MessageData> getNextMessages(string fromUser, string toUser, string lastMesgTime)//object lastMesgTime)
        {
            //JObject obj = JObject.Parse(lastMesgTime.ToString());
            //DateTime Time = Convert.ToDateTime(obj["lastMesgTime"].ToString());
            lastMesgTime = lastMesgTime.Replace('T', ' ');
            DateTime Time = DateTime.ParseExact(lastMesgTime, "yyyy-MM-dd HH:mm:ss.fffffff",CultureInfo.InvariantCulture);



            return db.MESSAGEMASTs.Where(s => ((s.USERINFO.USER_NAME == fromUser & s.USERINFO1.USER_NAME == toUser) | (s.USERINFO1.USER_NAME == fromUser & s.USERINFO.USER_NAME == toUser)) & s.SENDTIME>Time).OrderBy(s => s.SENDTIME)
                                       .Select(s => new MessageData
                                       {
                                           mesg = s,
                                           fromUser = s.USERINFO.USER_NAME,
                                           toUser = s.USERINFO1.USER_NAME
                                       })
                                       .Take(10)
                                       .ToList<MessageData>();
        }

        [HttpGet]
        public List<MessageData> getMessages(string fromUser, string toUser)
        {
            try
            {
                //return db.MESSAGEMASTs.Where(s =>( (s.USERINFO.USER_NAME == fromUser & s.USERINFO1.USER_NAME == toUser) | (s.USERINFO1.USER_NAME == fromUser & s.USERINFO.USER_NAME == toUser)) & s.DELIVERED==false).OrderBy(s => s.SENDTIME)
                //    .Select(s => new MessageData { mesg=s,
                //                                    fromUser=s.USERINFO.USER_NAME,
                //                                    toUser= s.USERINFO1.USER_NAME})
                //                                    .Take(10)
                //                                    .ToList<MessageData>();


                var dataTime = db.MESSAGEMASTs.Where(s => s.USERINFO.USER_NAME == fromUser & s.USERINFO1.USER_NAME == toUser & s.DELIVERED == false)
                                            .OrderBy(s => s.SENDTIME)
                                            .Select(s => (DateTime)s.SENDTIME)
                                            .FirstOrDefault();
                if (dataTime.Year < 2001)
                {
                    dataTime = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff"), "yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture);
                }

                if (dataTime != null)
                {
                    DateTime Time = Convert.ToDateTime(dataTime.ToString());
                    return db.MESSAGEMASTs.Where(s => ((s.USERINFO.USER_NAME == fromUser & s.USERINFO1.USER_NAME == toUser) | (s.USERINFO1.USER_NAME == fromUser & s.USERINFO.USER_NAME == toUser)) & s.SENDTIME >= Time).OrderBy(s => s.SENDTIME)
                                           .Select(s => new MessageData
                                                                    {
                                                                        mesg = s,
                                                                        fromUser = s.USERINFO.USER_NAME,
                                                                        toUser = s.USERINFO1.USER_NAME
                                                                    })
                                                        .Take(10)
                                                        .ToList<MessageData>();
                }
            }
            catch (Exception e) { }

            

            return new List<MessageData>();

        }


        [HttpGet]
        public IEnumerable<MessageData> getPrevMessages(string fromUser, string toUser, string firstMsgSendTime)
        {
            firstMsgSendTime=firstMsgSendTime.Replace('T',' ');
            DateTime Time = DateTime.ParseExact(firstMsgSendTime, "yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture);



            var data= db.MESSAGEMASTs.Where(s => ((s.USERINFO.USER_NAME == fromUser & s.USERINFO1.USER_NAME == toUser) | (s.USERINFO1.USER_NAME == fromUser & s.USERINFO.USER_NAME == toUser)) & s.SENDTIME < Time).OrderBy(s => s.SENDTIME)
                                       .Select(s => new MessageData
                                       {
                                           mesg = s,
                                           fromUser = s.USERINFO.USER_NAME,
                                           toUser = s.USERINFO1.USER_NAME
                                       })
                                       .Take(10)
                                       .ToList<MessageData>();

            var data1 = data.Select(s => s)
                            .OrderByDescending(s => s.mesg.SENDTIME)
                            .AsEnumerable<MessageData>();
            return data1;
            
        }

        
        //public IHttpActionResult addNewMessage(string msgData, string fromUser, string toUser)
        public IHttpActionResult addNewMessage(MessageData obj)
        {
            string lastMsgTime = string.Empty;
            string msgData = obj.mesg.MSG;
            int count= msgData.Length / 100+1;

            int fromUserId = db.USERINFOes.Where(s => s.USER_NAME == obj.fromUser).Select(s => s.ID).FirstOrDefault();
            int toUserId = db.USERINFOes.Where(s => s.USER_NAME == obj.toUser).Select(s => s.ID).FirstOrDefault();

            List<MESSAGEMAST> dataToSend = new List<MESSAGEMAST>();
            int i=0;
            do
            {
                string msgPartial = string.Empty;
                if (msgData.Length >= 100)
                {

                    msgPartial = msgData.Substring(i, 100);
                    msgData = msgData.Substring(100);

                    MESSAGEMAST msg = new MESSAGEMAST();
                    msg.MSG = msgPartial;
                    msg.FROM_USER = fromUserId;
                    msg.TO_USER  = toUserId;
                    msg.DELIVERED = false;
                    msg.MSG_PARTIAL = true;
                    msg.SENDTIME = DateTime.Now;


                    dataToSend.Add(msg);
                    count--;
                }
                else
                {
                    MESSAGEMAST msg = new MESSAGEMAST();
                    msg.MSG = msgData;


                    msg.FROM_USER = fromUserId;
                    msg.TO_USER = toUserId;
                    msg.DELIVERED = false;
                    msg.MSG_PARTIAL = false;
                    msg.SENDTIME = DateTime.Now;
                    dataToSend.Add(msg);
                    lastMsgTime = msg.SENDTIME.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
                    count--;
                }

            } while (count > 0);
            foreach(MESSAGEMAST msg in dataToSend )
                db.Entry(msg).State = EntityState.Added;
            db.SaveChanges();

            HubClass.PostToClient(obj.fromUser,obj.toUser);
            

            return Ok(lastMsgTime);
        }
    }
}