using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using PROJECT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Data.Entity.Validation;

namespace PROJECT.App_Start
{
    [HubName("chatHub")]
    public class ChatHub : Hub
    {
        #region---Data Members---
        static List<ApplicationUser> ConnectedUsers = new List<ApplicationUser>();
        //static List<UserDetail> ConnectedUsers = new List<UserDetail>();
        private ApplicationDbContext ApplicationDb = new ApplicationDbContext();
        static List<MessageDetail> CurrentMessage = new List<MessageDetail>();

        #endregion

        #region---Methods---

        public void Connect(string UserName, string UserID)
        {
            string id = Context.ConnectionId;
            ApplicationUser CurrentUser = ApplicationDb.Users.Where(u => u.Id == UserID).Single<ApplicationUser>();
            CurrentUser.ConnectionId = id;
            CurrentUser.Dp = CurrentUser.Dp;

            List<Requests> requests = ApplicationDb.Requests.Where(r => r.to_id == UserID && r.Accepted == true).ToList<Requests>();
            List<ApplicationUser> friends = (from r in requests
                                             from u in ApplicationDb.Users
                                             where r.from_id == u.Id
                                             select u).ToList<ApplicationUser>();
            try
            {
                ApplicationDb.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            if (ConnectedUsers.Count(x => x.ConnectionId == id) == 0)
            {
                ConnectedUsers.Add(CurrentUser);
            }
            //ApplicationUser CurrentUser = ApplicationDb.Users.Where(u => u.Id == Context.User.Identity.GetUserId()).Single<ApplicationUser>();
            //UserDetail CurrentUser = ConnectedUsers.Where(u => u.ConnectionId == id).FirstOrDefault();
            // send to caller
            List<string> conid = ConnectedUsers.Select(c => c.Id).ToList();
            List<string> fid = friends.Select(f => f.Id).ToList();
            List<string> intersec = conid.Intersect(fid).ToList();

            List<ApplicationUser> users = (from u in ApplicationDb.Users
                                           from intr in intersec
                                           where u.Id == intr
                                           select u).ToList<ApplicationUser>();
            
            users.Add(CurrentUser);

            Clients.Caller.onConnected(CurrentUser.ConnectionId, CurrentUser.UserName, users, CurrentMessage, UserID);
            //Clients.Caller.onConnected(CurrentUser.ConnectionId, CurrentUser.UserName, ConnectedUsers, CurrentMessage, UserID);
            // send to all except caller client
            users.Remove(CurrentUser);
            foreach(var user in users)
            {
                //Clients.AllExcept(CurrentUser.ConnectionId).onNewUserConnected(user.ConnectionId, user.UserName, UserID);
                Clients.Client(user.ConnectionId).onNewUserConnected(CurrentUser.ConnectionId, CurrentUser.UserName, UserID);
            }
            //Clients.AllExcept(CurrentUser.ConnectionId).onNewUserConnected(CurrentUser.ConnectionId, CurrentUser.UserName, UserID);
        }

        public void SendMessageToAll(string userName, string message)
        {
            // store last 100 messages in cache
            //AddMessageinCache(userName, message);

            // Broad cast message
            //Clients.All.messageReceived(userName, message);
        }

        public void SendPrivateMessage(string toUserId, string message)
        {
            try
            {
                string fromconnectionid = Context.ConnectionId;
                string strfromUserId = (ConnectedUsers.Where(u => u.ConnectionId == Context.ConnectionId).Select(u => u.Id).FirstOrDefault()).ToString();
                //int _fromUserId = 0;
                //int.TryParse(strfromUserId, out _fromUserId);
                //int _toUserId = 0;
                //int.TryParse(toUserId, out _toUserId);
                List<ApplicationUser> FromUsers = ConnectedUsers.Where(u => u.Id == strfromUserId).ToList();
                List<ApplicationUser> ToUsers = ConnectedUsers.Where(x => x.Id == toUserId).ToList();

                if (FromUsers.Count != 0 && ToUsers.Count() != 0)
                {
                    foreach (var ToUser in ToUsers)
                    {
                        // send to                                                                                            //Chat Title
                        Clients.Client(ToUser.ConnectionId).sendPrivateMessage(strfromUserId, FromUsers[0].UserName, FromUsers[0].UserName, message);
                    }


                    foreach (var FromUser in FromUsers)
                    {
                        // send to caller user                                                                                //Chat Title
                        Clients.Client(FromUser.ConnectionId).sendPrivateMessage(toUserId, FromUsers[0].UserName, ToUsers[0].UserName, message);
                    }
                    // send to caller user
                    //Clients.Caller.sendPrivateMessage(_toUserId.ToString(), FromUsers[0].UserName, message);
                    //ChatDB.Instance.SaveChatHistory(_fromUserId, _toUserId, message);
                    MessageDetail _MessageDeail = new MessageDetail { FromUserID = strfromUserId, FromUserName = FromUsers[0].UserName, ToUserID = toUserId, ToUserName = ToUsers[0].UserName, Message = message };
                    AddMessageinCache(_MessageDeail);
                }
            }
            catch { }
        }

        public void RequestLastMessage(string FromUserID, string ToUserID)
        {
            List<MessageDetail> CurrentChatMessages = (from u in CurrentMessage where ((u.FromUserID == FromUserID && u.ToUserID == ToUserID) || (u.FromUserID == ToUserID && u.ToUserID == FromUserID)) select u).ToList();
            //send to caller user
            Clients.Caller.GetLastMessages(ToUserID, CurrentChatMessages);
        }

        public void SendUserTypingRequest(string toUserId)
        {
            string strfromUserId = (ConnectedUsers.Where(u => u.ConnectionId == Context.ConnectionId).Select(u => u.Id).FirstOrDefault()).ToString();

            int _toUserId = 0;
            int.TryParse(toUserId, out _toUserId);
            List<ApplicationUser> ToUsers = ConnectedUsers.Where(x => x.Id == toUserId).ToList();

            foreach (var ToUser in ToUsers)
            {
                // send to                                                                                            
                Clients.Client(ToUser.ConnectionId).ReceiveTypingRequest(strfromUserId);
            }
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            if (stopCalled)
            {
                Clients.Caller.stopClient();
                Console.WriteLine(String.Format("Client {0} explicitly closed the connection.", Context.ConnectionId));
            }
            else
            {
                Clients.Caller.stopClient();
                Console.WriteLine(String.Format("Client {0} timed out .", Context.ConnectionId));
            }
            var item = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                ConnectedUsers.Remove(item);
                if (ConnectedUsers.Where(u => u.Id == item.Id).Count() == 0)
                {
                    var id = item.Id.ToString();
                    Clients.All.onUserDisconnected(id, item.UserName);
                }
            }
            return base.OnDisconnected(stopCalled);
        }
        #endregion

        #region---private Messages---
        private void AddMessageinCache(MessageDetail _MessageDetail)
        {
            CurrentMessage.Add(_MessageDetail);
            if (CurrentMessage.Count > 100)
                CurrentMessage.RemoveAt(0);
        }
        #endregion
    }
}