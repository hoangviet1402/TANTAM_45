using BussinessObject;
using BussinessObject.Models.User;
using System.Net;
using System.Web.Http;

namespace TanTamApi.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        /// <summary>
        /// Get user list with pagination and filtering
        /// </summary>
        [HttpGet]
        [Route("list")]
        public IHttpActionResult GetUserList([FromUri] UserListRequest request)
        {
            if (request == null)
            {
                request = new UserListRequest();
            }

            var result = BoFactory.User.GetUserListAsync(request);

            return Content(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Get user detail by id
        /// </summary>
        [HttpGet]
        [Route("detail")]
        public IHttpActionResult GetUserDetail([FromUri] int user_id, [FromUri] int company_id)
        {
            var request = new UserDetailRequest { UserId = user_id, CompanyId = company_id };

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = BoFactory.User.GetUserDetailAsync(request);

            return Content(HttpStatusCode.OK, result);
        }

        // Note: The following endpoints are commented out in the original controller
        // They can be implemented later when needed

        /*
        /// <summary>
        /// Create new user
        /// </summary>
        [HttpPost]
        [Route("create")]
        public IHttpActionResult CreateUser([FromBody] CreateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Implement create user
            return Ok();
        }

        /// <summary>
        /// Update user information
        /// </summary>
        [HttpPut]
        [Route("update")]
        public IHttpActionResult UpdateUser([FromBody] UpdateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Implement update user
            return Ok();
        }

        /// <summary>
        /// Update user profile
        /// </summary>
        [HttpPut]
        [Route("profile/update")]
        public IHttpActionResult UpdateUserProfile([FromBody] UpdateUserProfileRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Implement update user profile
            return Ok();
        }

        /// <summary>
        /// Change user password
        /// </summary>
        [HttpPut]
        [Route("password/change")]
        public IHttpActionResult ChangeUserPassword([FromBody] ChangeUserPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Implement change password
            return Ok();
        }

        /// <summary>
        /// Update user status (active/inactive)
        /// </summary>
        [HttpPut]
        [Route("status/update")]
        public IHttpActionResult UpdateUserStatus([FromBody] UserStatusRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Implement update user status
            return Ok();
        }
        */
    }
}