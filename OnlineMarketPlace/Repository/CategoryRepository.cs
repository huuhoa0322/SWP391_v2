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

        public async Task<List<CategoryModel>> GetCatgoryParentByShop(int shopId)
        {
            var parentIdHaveChildList = await _context.Categories
            .Where(c => _context.Products.Any(p => p.CategoryId == c.Id && p.SellerId == shopId)).Select(c => c.ParentId).Distinct()
            .ToListAsync();
            var parentIdNoneChildList = await _context.Categories
            .Where(c => c.ParentId == null && _context.Products.Any(p => p.CategoryId == c.Id && p.SellerId == shopId)).Distinct()
            .ToListAsync();
            var catergoriesParentList = new List<CategoryModel>();
            foreach (var id in parentIdHaveChildList)
            {
                var catergory = await _context.Categories.Where(c => c.Id == id).SingleAsync();
                catergoriesParentList.Add(catergory);
            }
            foreach (var parent in parentIdNoneChildList)
            {
                catergoriesParentList.Add(parent);
            }
            return catergoriesParentList;
        }

        public async Task<List<CategoryModel>> GetCatgoryChildByShop(int parentid, int shopId)
        {
            return await _context.Categories
            .Where(c => c.ParentId == parentid && _context.Products.Any(p => p.CategoryId == c.Id && p.SellerId == shopId))
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
