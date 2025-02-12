﻿
using PockemonReviewApp.Models;

namespace PockemonReviewApp.Interfaces
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int id);      
        ICollection<Pokemon> GetPokemonByCategory(int categoryId);
        bool CategoryExists(int Id);

        bool CreateCategory(Category category);

        bool UpdateCategory(Category category);

        bool DeleteCategory(Category category);

        bool Save();

    }
}
