using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Http;

namespace NorthWind.Api.Controllers
{
    public abstract class baseController : ApiController
    {
        protected readonly ILog _logger = null;

        protected baseController()
        {
            _logger = LogManager.GetLogger(GetType());
        }
    }
}