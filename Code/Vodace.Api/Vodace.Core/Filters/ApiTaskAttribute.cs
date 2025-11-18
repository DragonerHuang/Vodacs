using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;
using Vodace.Core.Configuration;
using Vodace.Core.Extensions;
using Vodace.Core.Quartz;
//using Vodace.Core.Quartz;

namespace Vodace.Core.Filters
{
        public interface IApiTaskFilter : IFilterMetadata
        {
            AuthorizationFilterContext OnAuthorization(AuthorizationFilterContext context);
        }
        public class ApiTaskAttribute : Attribute, IApiTaskFilter, IAllowAnonymous
        {
            public AuthorizationFilterContext OnAuthorization(AuthorizationFilterContext context)
            {
                return QuartzAuthorization.Validation(context) ;
            }
        }
 
}
