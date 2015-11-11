using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StoreApp.Models;
using System.Data.Entity;
using System.Threading.Tasks;

namespace StoreApp.DAL
{
    public class MaterialsRepository : IMaterialsRepository, IDisposable
    {
        private StorageDataBaseEntities context;

        private bool disposed = false;

        public MaterialsRepository(StorageDataBaseEntities context)
        {
            this.context = context;
        }

        public void DeleteMaterial(int id)
        {
            Material material = context.Materials.Find(id);
            context.Materials.Remove(material);
        }

        public void DeleteMaterial(Material material)
        {
            context.Materials.Remove(material);
        }

        public Task<Material> GetMaterialById(int id)
        {
            return context.Materials.FindAsync(id);
        }

        public async Task<IEnumerable<Material>> GetMaterials()
        {
            return await context.Materials.ToListAsync();
        }

        public void InsertMaterial(Material material)
        {
            context.Materials.Add(material);
        }

        public Task<int> Save()
        {
            return context.SaveChangesAsync();
        }

        public void UpdateMaterial(Material material)
        {
            context.Entry(material).State = EntityState.Modified;
        }

        public void Dispose(bool disposing)
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