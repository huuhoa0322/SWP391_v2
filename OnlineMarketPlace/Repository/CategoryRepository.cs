using OnlineMarketPlace.Models;

namespace OnlineMarketPlace.Repository
{
    public class CategoryRepository
    {
        private static OnlineShoppingContext _context = new();
        private List<Category> _allcategory = _context.Categories.ToList();

        public List<Category> GetCatgoryParent()
        {
            List<Category> res = new List<Category>();
            foreach(Category c in _allcategory)
            {
                if(c.Parent == null) 
                {
                    res.Add(c);
                }
            }
            return res;
        }

        public List<Category> GetCatgoryChild(int parentid)
        {
            List<Category> res = new List<Category>();
            foreach (Category c in _allcategory)
            {
                if (c.Parent != null)
                {
                    if(c.ParentId == parentid) res.Add(c);
                }
            }
            return res;
        }


    }
}
