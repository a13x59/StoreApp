using StoreApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace StoreApp.DAL
{
    public interface IMaterialsRepository : IDisposable
    {
        Task<IEnumerable<Material>> GetMaterials();

        Task<Material> GetMaterialById(int materialId);

        void InsertMaterial(Material material);

        void DeleteMaterial(int materialId);

        void DeleteMaterial(Material material);

        void UpdateMaterial(Material material);

        Task<int> Save();
    }
}