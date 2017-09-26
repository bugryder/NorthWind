using Newtonsoft.Json;
using NorthWind.Domain;
using NorthWind.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;

namespace NorthWind.Api.Controllers
{
    public class ProductsController : baseController
    {
        ProuctsService _prouctsService = new ProuctsService();

        // GET api/Products/5
        public HttpResponseMessage Get(int id)
        {
            _logger.DebugFormat("Invoking [{0}]", MethodInfo.GetCurrentMethod().Name);
            try
            {
                Products item = _prouctsService.GetProucts(id).SingleOrDefault();

                if (item == null)
                {
                    var message = string.Format("Product with id = {0} not found", id);
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, item);
                }


            }
            catch (Exception e)
            {

                _logger.Error(e.Message, e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError , e.Message);

            }

        }

        // POST: api/Products
        public HttpResponseMessage Post([FromBody]Products Products)
        {
            _logger.DebugFormat("Invoking [{0}]", MethodInfo.GetCurrentMethod().Name);
            try
            {
                var ProductID = _prouctsService.AddProucts(Products);
                if (ProductID != 0)
                {
                    Products item = _prouctsService.GetProucts(ProductID).SingleOrDefault();
                    return Request.CreateResponse(HttpStatusCode.OK, item);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Add Error");
                }

            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }



        public HttpResponseMessage Put([FromBody]Products Products)
        {
            _logger.DebugFormat("Invoking [{0}]", MethodInfo.GetCurrentMethod().Name);
            try
            {
                Products item = _prouctsService.GetProucts(Products.ProductID).SingleOrDefault();
                if (item == null)
                {
                    var message = string.Format("Product with id = {0} not found", Products.ProductID);
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
                }
                else
                {
                    if (_prouctsService.UpdateProucts(Products))
                    {
                        item = _prouctsService.GetProucts(Products.ProductID).SingleOrDefault();
                        return Request.CreateResponse(HttpStatusCode.OK, item);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Add Error");
                    }
                }



            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError , e.Message);
            }
        }

        // DELETE: api/Products/5
        public HttpResponseMessage Delete(int id)
        {
            _logger.DebugFormat("Invoking [{0}]", MethodInfo.GetCurrentMethod().Name);
            try
            {
                Products item = _prouctsService.GetProucts(id).SingleOrDefault();

                if (item == null)
                {
                    var message = string.Format("Product with id = {0} not found", id);
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
                }
                else
                {
                    _prouctsService.DeleteProucts(id);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }


            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                throw;
            }

        }
    }
}
