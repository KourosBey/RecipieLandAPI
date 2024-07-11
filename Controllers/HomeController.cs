using Microsoft.AspNetCore.Mvc;
using RecipieLandAPI.Concrete;
using RecipieLandAPI.Entity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RecipieLandAPI.Controllers
{
    #region DTO CLass

    #region Base Classes
    public class BaseResult
    {
        public bool Success { get; set; }
        public string Description { get; set; }
        public static BaseResult Successed()
        {

            return new BaseResult
            {
                Success = true,
                Description = "",
            };
        }
        public static BaseResult Failure(string description)
        {
            return new BaseResult
            {
                Success = true,
                Description = description,
            };
        }
    }
    public class BaseResult<T>
    {
        public bool Success { get; set; }
        public string Description { get; set; }
        public T Data { get; set; }

        public static BaseResult<T> Successed(T data)
        {
            return new BaseResult<T>
            {
                Success = true,
                Description = "",
                Data=data,
            };
        }
        public static BaseResult<T> Failure(string description)
        {
            return new BaseResult<T>
            {
                Success = false,
                Description = description,
            };
        }
    }
    #endregion

    #region User Classes
    public class AddUser
    {
        public string email { get; set; }
        public string password { get; set; }
        public string biography { get; set; }
        public string location { get; set; }
        public string profession { get; set; }
        public string webSite { get; set; }
        public string avatar { get; set; }


    }
    public class GetUser
    {
        public string UserId { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string biography { get; set; }
        public string location { get; set; }
        public string profession { get; set; }
        public string webSite { get; set; }
        public string avatar { get; set; }
    }
    public class UpdateUser
    {
        public string UserId { get; set; }
        public string biography { get; set; }
        public string location { get; set; }
        public string profession { get; set; }
        public string webSite { get; set; }
        public string avatar { get; set; }
    }
    #endregion

    #region Recipie Classes
    public class GetRecipies
    {
        public string RecipieId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CategoryID { get; set; }
        public string CategoryName { get; set; }
        public int UserLikes { get; set; }
        public List<GetRecipieSteps> RecipieSteps { get; set; }
        public List<GetNutrition> RecipieNutritions { get; set; }
        public string ImageUrl { get; set; }
        public string PreparationTime { get; set; }
        public string Serve { get; set; }
        public string Carb { get; set; }
        public string Fat { get; set; }
        public string Protein { get; set; }
        public string Calories { get; set; }
        public string LikedValue { get; set; }
    }
    public class GetRecipieSteps
    {
        public string RecipieID { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
    }
    public class GetAllNutritions
    {
        public string NutritionID { get; set; }
        public string Description { get; set; }
        public string NutritionName { get; set; }
    }
    public class GetNutrition
    {
        public string NutritionID { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; }
    }
    public class AddRecipies
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string CategorieId { get; set; }
        public List<AddRecipieSteps> Steps { get; set; }
        public List<AddRecipieNutritions> Nutritions { get; set; }
        public string Carbs { get; set; }
        public string Fats { get; set; }
        public string Protein { get; set; }
        public string Calories { get; set; }
        public string PreparationTime { get; set; }
        public string Serve { get; set; }
        public string ImageUrl { get; set; }
    }
    public class AddRecipieNutritions
    {
        public string nutritionId { get; set; }
        public string value { get; set; }
        public int valueType { get; set; }
    }
    public class AddRecipieSteps
    {
        public string Description { get; set; }
        public int Order { get; set; }
    }
    public class UpdateRecipies
    {
        public string RecipieId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CategorieId { get; set; }
        public string Carbs { get; set; }
        public string Fats { get; set; }
        public string Protein { get; set; }
        public string Calories { get; set; }
        public string PreparationTime { get; set; }
        public string Serve { get; set; }
        public string ImageUrl { get; set; }
    }
    public class UpdateRecipieNutr
    {
        public string nutritionId { get; set; }
        public string value { get; set; }
        public int valueType { get; set; }
    }
    public class UpdateRecipieSteps
    {
        public string StepsId { get; set; }
        public string Description { get; set; }
    }

    #endregion
    #endregion


    [ApiController]
    public class HomeController : ControllerBase
    {
        private RecipieContext _context;
        public HomeController(RecipieContext context)
        {
            _context = context;
        }
        #region User 
        [HttpPost("/user/checkuser")]
        public BaseResult CheckUsers(string email, string password)
        {

            if (_context.Users.Any(x => x.Email == email))
            {
                return BaseResult.Failure("Email boþ gönderilemez");
            }
            if (password == null)
            {
                return BaseResult.Failure("Parola boþ gönderilemez");
            }
            if (password.Length < 8)
            {
                return BaseResult.Failure("Parola 8 haneden küçük olamaz");
            }
            return BaseResult.Successed();
        }
        [HttpPost("/user/register")]
        public BaseResult AddUsers(AddUser user)
        {
            if (user == null)
            {
                return BaseResult.Failure("Beklenmedik bir hata!");
            }
            DefaultUser addUser = new DefaultUser
            {
                Biography = user.biography,
                Location = user.location,
                Profession = user.profession,
                Password = user.password,
                Email = user.email,
                WebSite = user.webSite,
                AvatarUrl = user.avatar,
            };
            try
            {
                _context.Users.Add(addUser);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return BaseResult.Failure(ex.Message);
            }
            return BaseResult.Successed();
        }
        [HttpPost("/user/update")]
        public BaseResult UpdateUser(UpdateUser user)
        {
            if (user == null)
            {
                return BaseResult.Failure("Beklenmedik bir hata!");
            }
            var dbUser = _context.Users.FirstOrDefault(x => x.Id == Guid.Parse(user.UserId));
            if (dbUser == null)
                return BaseResult.Failure("Kullanýcý bulunamadý");

            dbUser.Biography = user.biography;
            dbUser.Location = user.location;
            dbUser.Profession = user.profession;
            dbUser.WebSite = user.webSite;
            dbUser.AvatarUrl = user.avatar;
            try
            {
                _context.Users.Update(dbUser);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return BaseResult.Failure(ex.Message);
            }
            return BaseResult.Successed();
        }

        [HttpPost("/user/get-user")]
        public BaseResult<GetUser> GetUserInfo(Guid userId)
        {
            GetUser getUser = new();
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                return  BaseResult<GetUser>.Failure("Kullanýcý bulunamadý");
            }
            getUser.UserId = userId.ToString();
            getUser.biography = user.Biography;
            getUser.location = user.Location;
            getUser.profession = user.Profession;
            getUser.password = user.Password;
            getUser.email = user.Email;
            getUser.webSite = user.WebSite;
            getUser.avatar = user.AvatarUrl;

            return  BaseResult<GetUser>.Successed(getUser);
        }
        [HttpPost("/sign-in/")]
        public BaseResult<string> SingIn(string email, string password)
        {
            if (email == null || password == null)
                return BaseResult<string>.Failure("Þifre veya parola boþ gönderilemez");
            if (_context.Users.Any(x => x.Email == email && x.Password == password))
                return BaseResult<string>.Successed(_context.Users.FirstOrDefault(x => x.Email == email && x.Password == password).Id.ToString());
            return BaseResult<string>.Failure("Þifre veya parola hatalý tekrar deneyin!!");
        }
        #endregion


    }
}