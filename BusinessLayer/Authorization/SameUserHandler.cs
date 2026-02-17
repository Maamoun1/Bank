using DataAccessLayer.Entities;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Authorization
{
    public class SameUserHandler : AuthorizationHandler<SameUserRequirement,int>
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SameUserRequirement requirement, int resourceUserId)
        {

            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int loggedUserId))
            {

                return Task.CompletedTask;

            }

            if (context.User.IsInRole("Admin"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
            
            if (int.TryParse(userIdClaim.Value, out int loggedInUserId))
            {
                if (loggedInUserId == resourceUserId)
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;


        }

        }
    }


