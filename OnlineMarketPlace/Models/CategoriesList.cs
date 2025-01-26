

namespace OnlineMarketPlace.Models
{
    public class CategoriesList
    {
        public List<CategoryModel> CategoriesParent { get; set; }
        public List<List<CategoryModel>> CategoriesChild { get; set; }

        public CategoriesList(List<CategoryModel> categoriesParent, List<List<CategoryModel>> categoriesChild)
        {
            CategoriesParent = categoriesParent;
            CategoriesChild = categoriesChild;
        }

        public CategoriesList()
        {
        }
    }
}
