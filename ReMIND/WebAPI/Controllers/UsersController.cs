using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebAPI.Models;
using WebAPI.DAL;
using SharedPCL.DataContracts;

namespace WebAPI.Controllers
{
    public class UsersController : ApiController
    {
        private ReMinderContext db = new ReMinderContext();

        [HttpGet]
        [ResponseType(typeof(UserDTO))]
        public IHttpActionResult LoginUser(string email, string passwordMD5)
        {
            User user = db.Users.Where(x => x.Email == email && x.PasswordHash == passwordMD5).FirstOrDefault();
            if (user == null)
            {
                return Ok(new UserDTO() { Status = SharedPCL.Enums.UserStatus.NotFound });
            }
            return Ok(new UserDTO() { FullName = user.FullName, ID = user.ID, Status = SharedPCL.Enums.UserStatus.OK });
        }


        [HttpGet]
        [ResponseType(typeof(UserDTO))]
        public IHttpActionResult CreateUser(string email, string passwordMD5, string fullName)
        {
            User user = db.Users.Where(x => x.Email == email).FirstOrDefault();
            if (user != null)
            {
                return Ok(new UserDTO() { Status = SharedPCL.Enums.UserStatus.EmailExists });
            }
            user = new User()
            {
                Email = email,
                PasswordHash = passwordMD5,
                FullName = fullName
            };
            db.Users.Add(user);
            db.SaveChanges();

            return Ok(new UserDTO() { FullName = user.FullName, ID = user.ID, Status = SharedPCL.Enums.UserStatus.OK });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.ID == id) > 0;
        }
    }
}