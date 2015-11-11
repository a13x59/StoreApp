using StoreApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoreApp.DAL
{
    public class UnitOfWork : IDisposable
    {
        private StorageDataBaseEntities context = new StorageDataBaseEntities();
        private GenericRepository<Material> materialsRepository;
        private GenericRepository<Detail> detailsRepository;
        private ProductsRepository productsRepository;
        private ProductDetailsRepository productsDetailRepository;

        private bool disposed = false;

        public GenericRepository<Material> MaterialsRepository
        {
            get
            {

                if (this.materialsRepository == null)
                {
                    this.materialsRepository = new GenericRepository<Material>(context);
                }
                return materialsRepository;
            }
        }

        public GenericRepository<Detail> DetailsRepository
        {
            get
            {

                if (this.detailsRepository == null)
                {
                    this.detailsRepository = new GenericRepository<Detail>(context);
                }
                return detailsRepository;
            }
        }

        //public GenericRepository<ProductsDetail> ProductsDetailRepository
        public ProductDetailsRepository ProductDetailsRepository
        {
            get
            {

                if (this.productsDetailRepository == null)
                {
                    this.productsDetailRepository = new ProductDetailsRepository(context);
                }
                return productsDetailRepository;
            }
        }

        public ProductsRepository ProductsRepository
        {
            get
            {
                if (this.productsRepository == null)
                {
                    this.productsRepository = new ProductsRepository(context);
                }
                return productsRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}