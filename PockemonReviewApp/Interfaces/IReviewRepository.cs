using PockemonReviewApp.Models;

namespace PockemonReviewApp.Interfaces
{
    public interface IReviewRepository
    {

        ICollection<Review> GetAllReviews();
        Review GetReview(int reviewId);
        ICollection<Review> GetReviewsOfAPokemon(int pokeId);
        bool ReviewExists(int Id);

        bool CreateReview(Review review);
        bool UpdateReview(Review review);
        bool Delete(Review review);

        bool Save();

       
    }
}
