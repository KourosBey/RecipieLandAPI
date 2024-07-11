using System.ComponentModel.DataAnnotations;

namespace RecipieLandAPI.Entity
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public DateTime UpdatedTime { get; set; } = DateTime.Now;
    }
    public class RecipieCategory : BaseEntity
    {
        public string CategoryName { get; set; }

    }

    public class Nutritions : BaseEntity
    {
        public string NutritionName { get; set; }
        public string NutritionDescription { get; set; }

    }

    public class NutritionRecipie : BaseEntity
    {
        public Guid NutritionId { get; set; }
        public Nutritions Nutrition { get; set; }
        public Guid RecipieId { get; set; }
        public Recipie Recipie { get; set; }
        public NutritionsValueType ValueType { get; set; }
        public string Value { get; set; }
    }

    public enum NutritionsValueType
    {
        NONE = 0,
        KG = 1,
        GR = 2,
        ADET = 3,
        LT = 4,
        ML = 5,
    }
    public class RecipieSteps : BaseEntity
    {
        public string Description { get; set; }
        public int StepNumber { get; set; }
        public Guid RecipieId { set; get; }


    }
    public class Recipie : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid CategoryID { get; set; }
        public RecipieCategory Category { get; set; }
        public ICollection<UserLikedRecipies> UserLikes { get; set; }
        public ICollection<RecipieSteps> RecipieSteps { get; set; }
        public string ImageUrl { get; set; }
        public string PreparationTime { get; set; }
        public string Serve { get; set; }
        public string Carb { get; set; }
        public string Fat { get; set; }
        public string Protein { get; set; }
        public string Calories { get; set; }
    }
    public class UserRecipie : BaseEntity
    {
        public Guid UserRecipieId { get; set; }
        public virtual DefaultUser User { get; set; }
        public Guid RecipieId { get; set; }
        public virtual Recipie Recipie { set; get; }
    }
    public class UserLikedRecipies : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid RecipieId { get; set; }
        public string value { get; set; }
    }

    public class UserLikedCategory
    {
        public Guid UserId { get; set; }
        public Guid CategoryID { get; set; }
    }
    public class UserFollowing
    {
        public Guid OwnUserId { get; set; }
        public Guid FollowerId { get; set; }

    }
    public class DefaultUser : BaseEntity
    {
        public ICollection<UserRecipie> OwnerRecipies { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ICollection<UserLikedCategory> LikeCategoryUsers { get; set; }
        public ICollection<UserLikedRecipies> LikedRecipies { get; set; }
        public ICollection<UserFollowing> Followers { get; set; }
        public string Biography { get; set; }
        public string Location { get; set; }
        public string Profession { get; set; }
        public string WebSite { get; set; }
        public string AvatarUrl { get; set; }
    }



}
