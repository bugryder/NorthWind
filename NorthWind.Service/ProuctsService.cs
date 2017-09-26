using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NorthWind.Repository;
using NorthWind.Domain;

namespace NorthWind.Service
{
    public class ProuctsService : BaseService
    {

        private NorthWind.Repository.NorthWind.NorthWindProductsRepository _northWindProductsRepository = new NorthWind.Repository.NorthWind.NorthWindProductsRepository();


        public List<Products> GetProucts(int? ProuctID)
        {
            List<Products> ProductsList = _northWindProductsRepository.GetProducts(ProuctID);

            return ProductsList;
        }

        public int AddProucts(Products Prouct)
        {
            //SupplierID ,CategoryID check
            int result = 0;
            if (Prouct != null)
            {
                result =  _northWindProductsRepository.AddProducts(Prouct);
            }
            return result;
        }

        public Boolean UpdateProucts(Products Prouct)
        {
            bool result = false;

            //SupplierID ,CategoryID check
            if (Prouct != null)
            {
                result = _northWindProductsRepository.UpdateProducts(Prouct) > 0;
            }

            return result;
        }

        public Boolean DeleteProucts(int ProuctID)
        {
            bool result = false;

            Products Products = _northWindProductsRepository.GetProducts(ProuctID).SingleOrDefault();

            if (Products != null)
            {
                result= _northWindProductsRepository.DeleteProducts(ProuctID) > 0;

            }

            return result;
        }


    }
}
