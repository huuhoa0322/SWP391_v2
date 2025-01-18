using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineMarketPlace.Models;

namespace OnlineMarketPlace.Repository
{
    public class CategoryRepository
    {
        private static OnlineShoppingContext _context = new();
        private List<CategoryModel> _allcategory = _context.Categories.ToList();

        public List<CategoryModel> GetCatgoryParent()
        {
            List<CategoryModel> res = new List<CategoryModel>();
            foreach (CategoryModel c in _allcategory)
            {
                if (c.Parent == null)
                {
                    res.Add(c);
                }
            }
            return res;
        }

        public List<CategoryModel> GetCatgoryChild(int parentid)
        {
            List<CategoryModel> res = new List<CategoryModel>();
            foreach (CategoryModel c in _allcategory)
            {
                if (c.Parent != null)
                {
                    if (c.ParentId == parentid) res.Add(c);
                }
            }
            return res;
        }

        //private readonly OnlineShoppingContext _context;

        //public CategoryRepository()
        //{
        //    _context = new();
        //}

        //public async Task<List<Category>> GetCategoryParentAsync()
        //{
        //    // Fetch all categories asynchronously
        //    var allCategories = await _context.Categories.ToListAsync();

        //    // Filter categories with no parent
        //    return allCategories.Where(c => c.Parent == null).ToList();
        //}

        //public async Task<List<Category>> GetCategoryChildAsync(int parentId)
        //{
        //    // Fetch all categories asynchronously
        //    var allCategories = await _context.Categories.ToListAsync();

        //    // Filter categories with the specified parentId
        //    var result = allCategories.Where(c => c.Parent != null && c.ParentId == parentId).ToList();

        //    // Return null if no matching categories are found
        //    return result;
        //}
    }
}
