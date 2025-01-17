namespace OnlineMarketPlace.Models
{
    public class CategoriesList
    {
        public List<Category> CategoriesParent { get; set; }
        public List<List<Category>> CategoriesChild { get; set; }

        public CategoriesList(List<Category> categoriesParent, List<List<Category>> categoriesChild)
        {
            CategoriesParent = categoriesParent;
            CategoriesChild = categoriesChild;
        }
    }
}
