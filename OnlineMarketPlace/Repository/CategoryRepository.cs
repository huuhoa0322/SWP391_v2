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
        //private List<CategoryModel> _allcategory = _context.Categories.ToList();
        //private static OnlineShoppingContext _context;
        //private List<CategoryModel> _allcategory;

        public CategoryRepository()
        {
            //_context = new();
            //_allcategory = _context.Categories.ToList();
        }

        public async Task<List<CategoryModel>> GetCatgoryParent()
        {
            return await _context.Categories
            .Where(c => c.Parent == null)
            .ToListAsync();
        }

        public async Task<List<CategoryModel>> GetCatgoryChild(int parentid)
        {
            return await _context.Categories
            .Where(c => c.Parent != null && c.ParentId == parentid)
            .ToListAsync();
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
