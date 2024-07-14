using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipieLandAPI.Concrete;
using RecipieLandAPI.Entity;

namespace RecipieLandAPI.Controllers
{
    [ApiController]
    public class RecipieController : ControllerBase
    {
        private RecipieContext _context;
        public RecipieController(RecipieContext context)
        {
            _context = context;
        }
        [HttpPost("/recipie/get-user-recipies")]
        public async Task<BaseResult<List<GetRecipies>>> GetUserRecipiesAll(string userId)
        {
            try
            {
                if (!_context.Users.Any(x => x.Id == Guid.Parse(userId)))
                    return BaseResult<List<GetRecipies>>.Failure("Kullanıcı silinmiş ya da değiştirilmiş");
                var userRecipies = await _context.UserRecipies
                    .Include(x => x.User)
                    .Include(x => x.Recipie)
                    .ThenInclude(x => x.Category)
                    .Include(x => x.Recipie)
                    .ThenInclude(x => x.RecipieSteps)
                    .AsNoTracking()
                    .Where(x => x.UserRecipieId == Guid.Parse(userId))
                    .ToListAsync();

                if (userRecipies.Count > 0)
                {
                    List<GetRecipies> recipies = new List<GetRecipies>();
                    try
                    {
                        recipies = userRecipies.Select(x => new GetRecipies
                        {
                            RecipieId = x.Recipie.Id.ToString(),
                            Calories = x.Recipie.Calories,
                            Carb = x.Recipie.Carb,
                            CategoryID = x.Recipie.CategoryID.ToString(),
                            CategoryName = x.Recipie.Category.CategoryName,
                            Description = x.Recipie.Description,
                            Fat = x.Recipie.Fat,
                            ImageUrl = x.Recipie.ImageUrl,
                            PreparationTime = x.Recipie.PreparationTime,
                            Protein = x.Recipie.Protein,
                            RecipieSteps = x.Recipie.RecipieSteps.OrderBy(x => x.StepNumber).Select(x => new GetRecipieSteps
                            {
                                Description = x.Description,
                                Order = x.StepNumber,
                                RecipieID = x.Id.ToString(),
                            }).ToList(),
                            RecipieNutritions = _context.RecipieNutritions.Where(y => y.RecipieId == x.Id).Include(x => x.Nutrition).Select(x => new
                            GetNutrition
                            {
                                NutritionID = x.Id.ToString(),
                                Value = x.Value,
                                ValueType = x.ValueType.ToString(),
                            }).ToList(),
                            Serve = x.Recipie.Serve,
                            Title = x.Recipie.Title,
                            UserLikes = x.Recipie.UserLikes != null ? x.Recipie.UserLikes.Count() : 0,
                            LikedValue = _context.UserLikedRecipies.Where(y => y.RecipieId == x.RecipieId).Count() > 0 ? (_context.UserLikedRecipies.Where(y => y.RecipieId == x.RecipieId).Select(x => Convert.ToInt32(x.value)).Sum() / _context.UserLikedRecipies.Where(y => y.RecipieId == x.RecipieId).Count()).ToString() : "0",

                        }).ToList();

                    }
                    catch (Exception ex)
                    {

                    }
                    return BaseResult<List<GetRecipies>>.Successed(recipies);
                }
                return BaseResult<List<GetRecipies>>.Failure("Kullanıcıya ait tarif bulunamadı");
            }
            catch (Exception ex)
            {
                return BaseResult<List<GetRecipies>>.Failure("Bilinmeyen bir hata oluştu");
            }
        }

        [HttpGet("/recipie/get-favorite-recipie")]
        public async Task<BaseResult<List<GetRecipies>>> GetFavoriteRecipie([FromQuery] string id)
        {
            List<Recipie> userRecipies = new();
            try
            {
                var userLikedRecipies = await _context.UserLikedRecipies.Where(x => x.UserId == Guid.Parse(id))
                    .AsNoTracking()
                    .Select(x => x.RecipieId)
                    .ToListAsync();
                if (userLikedRecipies.Count > 0)
                {
                    userRecipies = await _context.Recipies.Where(x => userLikedRecipies.Contains(x.Id)).ToListAsync();
                }
                if (userRecipies.Count > 0)
                {
                    List<GetRecipies> recipies = new List<GetRecipies>();
                    try
                    {
                        recipies = userRecipies.Select(x => new GetRecipies
                        {
                            RecipieId = x.Id.ToString(),
                            Calories = x.Calories,
                            Carb = x.Carb,
                            CategoryID = x.CategoryID.ToString(),
                            CategoryName = x.Category.CategoryName,
                            Description = x.Description,
                            Fat = x.Fat,
                            ImageUrl = x.ImageUrl,
                            PreparationTime = x.PreparationTime,
                            Protein = x.Protein,
                            RecipieSteps = x.RecipieSteps.OrderBy(x => x.StepNumber).Select(x => new GetRecipieSteps
                            {
                                Description = x.Description,
                                Order = x.StepNumber,
                                RecipieID = x.Id.ToString(),
                            }).ToList(),
                            RecipieNutritions = _context.RecipieNutritions.Where(y => y.RecipieId == x.Id).Include(x => x.Nutrition).Select(x => new
                            GetNutrition
                            {
                                NutritionID = x.Id.ToString(),
                                Value = x.Value,
                                ValueType = x.ValueType.ToString(),
                            }).ToList(),
                            Serve = x.Serve,
                            Title = x.Title,
                            UserLikes = x.UserLikes != null ? x.UserLikes.Count() : 0,
                            LikedValue = _context.UserLikedRecipies.Where(y => y.RecipieId == x.Id).Count() > 0 ? (_context.UserLikedRecipies.Where(y => y.RecipieId == x.Id).Select(x => Convert.ToInt32(x.value)).Sum() / _context.UserLikedRecipies.Where(y => y.RecipieId == x.Id).Count()).ToString() : "0",

                        }).OrderByDescending(x => Convert.ToInt32(x.LikedValue)).Take(10).ToList();

                    }
                    catch (Exception ex)
                    {

                    }
                    return BaseResult<List<GetRecipies>>.Successed(recipies);
                }
                return BaseResult<List<GetRecipies>>.Failure("Beğendiğiniz tarif bulunamadı");
            }
            catch (Exception ex)
            {
                return BaseResult<List<GetRecipies>>.Failure("Bilinmeyen bir hata oluştu");
            }
        }

        [HttpGet("/trending-recipes")]
        public async Task<BaseResult<List<GetRecipies>>> GetTrendingRecipies([FromQuery]string categoryId)
        {
            try
            {
                var userRecipies = await _context.UserRecipies
                    .Include(x => x.User)
                    .Include(x => x.Recipie)
                    .ThenInclude(x => x.Category)
                    .Include(x => x.Recipie)
                    .ThenInclude(x => x.RecipieSteps)
                    .Where(x=>x.Recipie.CategoryID==Guid.Parse(categoryId))
                    .AsNoTracking()
                    .ToListAsync();

                if (userRecipies.Count > 0)
                {
                    List<GetRecipies> recipies = new List<GetRecipies>();
                    try
                    {
                        recipies = userRecipies.Select(x => new GetRecipies
                        {
                            RecipieId = x.Recipie.Id.ToString(),
                            Calories = x.Recipie.Calories,
                            Carb = x.Recipie.Carb,
                            CategoryID = x.Recipie.CategoryID.ToString(),
                            CategoryName = x.Recipie.Category.CategoryName,
                            Description = x.Recipie.Description,
                            Fat = x.Recipie.Fat,
                            ImageUrl = x.Recipie.ImageUrl,
                            PreparationTime = x.Recipie.PreparationTime,
                            Protein = x.Recipie.Protein,
                            RecipieSteps = x.Recipie.RecipieSteps.OrderBy(x => x.StepNumber).Select(x => new GetRecipieSteps
                            {
                                Description = x.Description,
                                Order = x.StepNumber,
                                RecipieID = x.Id.ToString(),
                            }).ToList(),
                            RecipieNutritions = _context.RecipieNutritions.Where(y => y.RecipieId == x.Id).Include(x => x.Nutrition).Select(x => new
                            GetNutrition
                            {
                                NutritionID = x.Id.ToString(),
                                Value = x.Value,
                                ValueType = x.ValueType.ToString(),
                            }).ToList(),
                            Serve = x.Recipie.Serve,
                            Title = x.Recipie.Title,
                            UserLikes = x.Recipie.UserLikes != null ? x.Recipie.UserLikes.Count() : 0,
                            LikedValue = _context.UserLikedRecipies.Where(y => y.RecipieId == x.RecipieId).Count() > 0 ? (_context.UserLikedRecipies.Where(y => y.RecipieId == x.RecipieId).Select(x => Convert.ToInt32(x.value)).Sum() / _context.UserLikedRecipies.Where(y => y.RecipieId == x.RecipieId).Count()).ToString() : "0",

                        }).OrderByDescending(x => Convert.ToInt32(x.LikedValue)).Take(10).ToList();

                    }
                    catch (Exception ex)
                    {

                    }
                    return BaseResult<List<GetRecipies>>.Successed(recipies);
                }
                return BaseResult<List<GetRecipies>>.Failure("Kayıtlı herhangi tarif bulunamadı");
            }
            catch (Exception ex)
            {
                return BaseResult<List<GetRecipies>>.Failure("Bilinmeyen bir hata oluştu");
            }
        }

        [HttpGet("/recipie/get-trendingRecipie")]
        public async Task<BaseResult<List<GetRecipies>>> GetTrendingRecipies()
        {
            try
            {
                var userRecipies = await _context.UserRecipies
                    .Include(x => x.User)
                    .Include(x => x.Recipie)
                    .ThenInclude(x => x.Category)
                    .Include(x => x.Recipie)
                    .ThenInclude(x => x.RecipieSteps)
                    .AsNoTracking()
                    .ToListAsync();

                if (userRecipies.Count > 0)
                {
                    List<GetRecipies> recipies = new List<GetRecipies>();
                    try
                    {
                        recipies = userRecipies.Select(x => new GetRecipies
                        {
                            RecipieId = x.Recipie.Id.ToString(),
                            Calories = x.Recipie.Calories,
                            Carb = x.Recipie.Carb,
                            CategoryID = x.Recipie.CategoryID.ToString(),
                            CategoryName = x.Recipie.Category.CategoryName,
                            Description = x.Recipie.Description,
                            Fat = x.Recipie.Fat,
                            ImageUrl = x.Recipie.ImageUrl,
                            PreparationTime = x.Recipie.PreparationTime,
                            Protein = x.Recipie.Protein,
                            RecipieSteps = x.Recipie.RecipieSteps.OrderBy(x => x.StepNumber).Select(x => new GetRecipieSteps
                            {
                                Description = x.Description,
                                Order = x.StepNumber,
                                RecipieID = x.Id.ToString(),
                            }).ToList(),
                            RecipieNutritions = _context.RecipieNutritions.Where(y => y.RecipieId == x.Id).Include(x => x.Nutrition).Select(x => new
                            GetNutrition
                            {
                                NutritionID = x.Id.ToString(),
                                Value = x.Value,
                                ValueType = x.ValueType.ToString(),
                            }).ToList(),
                            Serve = x.Recipie.Serve,
                            Title = x.Recipie.Title,
                            UserLikes = x.Recipie.UserLikes != null ? x.Recipie.UserLikes.Count() : 0,
                            LikedValue = _context.UserLikedRecipies.Where(y => y.RecipieId == x.RecipieId).Count() > 0 ? (_context.UserLikedRecipies.Where(y => y.RecipieId == x.RecipieId).Select(x => Convert.ToInt32(x.value)).Sum() / _context.UserLikedRecipies.Where(y => y.RecipieId == x.RecipieId).Count()).ToString() : "0",

                        }).OrderByDescending(x => Convert.ToInt32(x.LikedValue)).Take(10).ToList();

                    }
                    catch (Exception ex)
                    {

                    }
                    return BaseResult<List<GetRecipies>>.Successed(recipies);
                }
                return BaseResult<List<GetRecipies>>.Failure("Kayıtlı herhangi tarif bulunamadı");
            }
            catch (Exception ex)
            {
                return BaseResult<List<GetRecipies>>.Failure("Bilinmeyen bir hata oluştu");
            }
        }
        [HttpGet("/recipie/get-recipie-detail")]
        public async Task<BaseResult<GetRecipies>> GetRecipieDetails([FromQuery]string recipieId)
        {
            try
            {
                var userRecipies = await _context.UserRecipies
                    .Include(x => x.User)
                    .Include(x => x.Recipie)
                    .ThenInclude(x => x.Category)
                    .Include(x => x.Recipie)
                    .ThenInclude(x => x.RecipieSteps)
                    .FirstOrDefaultAsync(x => x.RecipieId == Guid.Parse(recipieId));

                if (userRecipies != null)
                {
                   GetRecipies recipies = new GetRecipies();
                    try
                    {

                        recipies.RecipieId = userRecipies.Recipie.Id.ToString();
                        recipies.Calories = userRecipies.Recipie.Calories;
                        recipies.Carb = userRecipies.Recipie.Carb;
                        recipies.CategoryID = userRecipies.Recipie.CategoryID.ToString();
                        recipies.CategoryName = userRecipies.Recipie.Category.CategoryName;
                        recipies.Description = userRecipies.Recipie.Description;
                        recipies.Fat = userRecipies.Recipie.Fat;
                        recipies.ImageUrl = userRecipies.Recipie.ImageUrl;
                        recipies.PreparationTime = userRecipies.Recipie.PreparationTime;
                        recipies.Protein = userRecipies.Recipie.Protein;
                        recipies.RecipieSteps = userRecipies.Recipie.RecipieSteps.OrderBy(x => x.StepNumber).Select(x => new GetRecipieSteps
                            {
                                Description = x.Description,
                                Order = x.StepNumber,
                                RecipieID = x.Id.ToString(),
                            }).ToList();
                        recipies.RecipieNutritions = _context.RecipieNutritions.Where(y => y.RecipieId == userRecipies.RecipieId).Include(x => x.Nutrition).Select(x => new
                        GetNutrition
                        {
                            NutritionID = x.Id.ToString(),
                            Description=x.Nutrition.NutritionName,
                            Value = x.Value,
                            ValueType = x.ValueType.ToString(),
                        }).ToList();
                        recipies.Serve = userRecipies.Recipie.Serve;
                        recipies.Title = userRecipies.Recipie.Title;
                            recipies.UserLikes = userRecipies.Recipie.UserLikes != null ? userRecipies.Recipie.UserLikes.Count() : 0;
                        recipies.LikedValue = _context.UserLikedRecipies.Where(y => y.RecipieId == userRecipies.RecipieId).Count() > 0 ? (_context.UserLikedRecipies.Where(y => y.RecipieId == userRecipies.RecipieId).Select(x => Convert.ToInt32(x.value)).Sum() / _context.UserLikedRecipies.Where(y => y.RecipieId == userRecipies.RecipieId).Count()).ToString() : "0";

                    }
                    catch (Exception ex)
                    {

                    }
                    return BaseResult<GetRecipies>.Successed(recipies);
                }
                return BaseResult<GetRecipies>.Failure("Kayıtlı herhangi tarif bulunamadı");
            }
            catch (Exception ex)
            {
                return BaseResult<GetRecipies>.Failure("Bilinmeyen bir hata oluştu");
            }
        }
        [HttpPost("/recipie/get-recipies")]
        public async Task<BaseResult<List<GetRecipies>>> GetRecipiesAll(string userId)
        {
            try
            {
                if (!_context.Users.Any(x => x.Id == Guid.Parse(userId)))
                    return BaseResult<List<GetRecipies>>.Failure("Kullanıcı silinmiş ya da değiştirilmiş");
                var userRecipies = await _context.UserRecipies
                    .Include(x => x.User)
                    .Include(x => x.Recipie)
                    .ThenInclude(x => x.Category)
                    .Include(x => x.Recipie)
                    .ThenInclude(x => x.RecipieSteps)
                    .AsNoTracking()
                    .Where(x => x.UserRecipieId != Guid.Parse(userId))
                    .ToListAsync();

                if (userRecipies.Count > 0)
                {
                    List<GetRecipies> recipies = new List<GetRecipies>();
                    recipies = userRecipies.Select(x => new GetRecipies
                    {
                        RecipieId = x.Recipie.Id.ToString(),
                        Calories = x.Recipie.Calories,
                        Carb = x.Recipie.Carb,
                        CategoryID = x.Recipie.CategoryID.ToString(),
                        CategoryName = x.Recipie.Category.CategoryName,
                        Description = x.Recipie.Description,
                        Fat = x.Recipie.Fat,
                        ImageUrl = x.Recipie.ImageUrl,
                        PreparationTime = x.Recipie.PreparationTime,
                        Protein = x.Recipie.Protein,
                        RecipieSteps = x.Recipie.RecipieSteps.OrderBy(x => x.StepNumber).Select(x => new GetRecipieSteps
                        {
                            Description = x.Description,
                            Order = x.StepNumber,
                            RecipieID = x.Id.ToString(),
                        }).ToList(),
                        RecipieNutritions = _context.RecipieNutritions.Where(y => y.RecipieId == x.Id).Include(x => x.Nutrition).Select(x => new
                        GetNutrition
                        {
                            NutritionID = x.Id.ToString(),
                            Value = x.Value,
                            ValueType = x.ValueType.ToString(),
                        }).ToList(),
                        Serve = x.Recipie.Serve,
                        Title = x.Recipie.Title,
                        UserLikes = x.Recipie.UserLikes.Count(),
                        LikedValue = (_context.UserLikedRecipies.Where(y => y.RecipieId == x.RecipieId).Select(x => Convert.ToInt32(x.value)).Sum() / _context.UserLikedRecipies.Where(y => y.RecipieId == x.RecipieId).Count()).ToString(),
                    }).ToList();

                    return BaseResult<List<GetRecipies>>.Successed(recipies);
                }
                return BaseResult<List<GetRecipies>>.Failure("Tarif bulunamadı");
            }
            catch (Exception)
            {
                return BaseResult<List<GetRecipies>>.Failure("Bilinmeyen bir hata oluştu");
            }
        }
        [HttpPost("/recipie/set-recipie")]
        public async Task<BaseResult> SetRecipie([FromBody] AddRecipies addRec, [FromQuery] string userId)
        {
            // Recipie eklenecek kaldığın yer. DBye migration var ama atmadın
            if (!_context.Users.Any(x => x.Id == Guid.Parse(userId)))
            {
                return BaseResult.Failure("Kullanıcı bulunamadı");
            }
            if (addRec == null)
            {
                return BaseResult.Failure("Tarif bilgisi boş gönderilemez");
            }
            Recipie rec = new();
            rec.Carb = addRec.Carbs;
            rec.Protein = addRec.Protein;
            rec.Serve = addRec.Serve;
            rec.Title = addRec.Title;
            rec.Calories = addRec.Calories;
            rec.CategoryID = Guid.Parse(addRec.CategorieId);
            rec.Category = _context.RecipieCategories.FirstOrDefault(x => x.Id == Guid.Parse(addRec.CategorieId)) ?? null;
            rec.Description = addRec.Description;
            rec.PreparationTime = addRec.PreparationTime;
            rec.Fat = addRec.Fats;
            rec.ImageUrl = addRec.ImageUrl;
            rec.RecipieSteps = addRec.Steps.Select(x => new RecipieSteps
            {
                Description = x.Description,
                StepNumber = x.Order,
                RecipieId = rec.Id,
            }).ToList();
            try
            {

                _context.Recipies.Add(rec);
                await _context.SaveChangesAsync();
                UserRecipie addUserRecipie = new UserRecipie();
                addUserRecipie.RecipieId = rec.Id;
                addUserRecipie.Recipie = rec;
                addUserRecipie.User = _context.Users.FirstOrDefault(x => x.Id == Guid.Parse(userId));
                addUserRecipie.UserRecipieId = Guid.Parse(userId);
                _context.UserRecipies.Add(addUserRecipie);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BaseResult.Failure(ex.Message);
            }
            return BaseResult.Successed();
        }

        [HttpPost("/recipie/get-nutritions-recipie")]
        public async Task<BaseResult<List<GetAllNutritions>>> GetNutritionsAll()
        {
            try
            {
                var nutritions = await _context.Nutritions.Select(x => new GetAllNutritions
                {
                    NutritionID = x.Id.ToString(),
                    Description = x.NutritionDescription,
                    NutritionName = x.NutritionName,

                }).ToListAsync();
                if (nutritions.Count == 0)
                    return BaseResult<List<GetAllNutritions>>.Failure("Herhangi bir ürün bulunamadı lütfen ürün ekleyiniz");
                return BaseResult<List<GetAllNutritions>>.Successed(nutritions);
            }
            catch (Exception ex)
            {
                return BaseResult<List<GetAllNutritions>>.Failure(ex.Message);
            }
        }

        [HttpPost("/recipie/set-nutrition")]
        public async Task<BaseResult> GetNutritionsAll(string description, string NutritionName)
        {
            try
            {
                _context.Nutritions.Add(new Nutritions
                {
                    NutritionDescription = description,
                    NutritionName = NutritionName
                });
                await _context.SaveChangesAsync();
                return BaseResult.Successed();
            }
            catch (Exception ex)
            {
                return BaseResult.Failure(ex.Message);
            }
        }

        [HttpPost("/recipie/get-recipe-with-category")]
        public async Task<BaseResult<List<GetRecipies>>> GetRecipeCategory(String CategoryId)
        {
            try
            {
                var userRecipies = await _context.UserRecipies
                    .Include(x => x.User)
                    .Include(x => x.Recipie)
                    .ThenInclude(x => x.Category)
                    .Include(x => x.Recipie)
                    .ThenInclude(x => x.RecipieSteps)
                    .AsNoTracking()
                    .Where(x => x.Recipie.CategoryID == Guid.Parse(CategoryId))
                    .ToListAsync();

                if (userRecipies.Count > 0)
                {
                    List<GetRecipies> recipies = new List<GetRecipies>();
                    recipies = userRecipies.Select(x => new GetRecipies
                    {
                        RecipieId = x.Recipie.Id.ToString(),
                        Calories = x.Recipie.Calories,
                        Carb = x.Recipie.Carb,
                        CategoryID = x.Recipie.CategoryID.ToString(),
                        CategoryName = x.Recipie.Category.CategoryName,
                        Description = x.Recipie.Description,
                        Fat = x.Recipie.Fat,
                        ImageUrl = x.Recipie.ImageUrl,
                        PreparationTime = x.Recipie.PreparationTime,
                        Protein = x.Recipie.Protein,
                        RecipieSteps = x.Recipie.RecipieSteps.OrderBy(x => x.StepNumber).Select(x => new GetRecipieSteps
                        {
                            Description = x.Description,
                            Order = x.StepNumber,
                            RecipieID = x.Id.ToString(),
                        }).ToList(),
                        RecipieNutritions = _context.RecipieNutritions.Where(y => y.RecipieId == x.Id).Include(x => x.Nutrition).Select(x => new
                        GetNutrition
                        {
                            NutritionID = x.Id.ToString(),
                            Value = x.Value,
                            ValueType = x.ValueType.ToString(),
                        }).ToList(),
                        Serve = x.Recipie.Serve,
                        Title = x.Recipie.Title,
                        UserLikes = x.Recipie.UserLikes.Count(),
                        LikedValue = (_context.UserLikedRecipies.Where(y => y.RecipieId == x.RecipieId).Select(x => Convert.ToInt32(x.value)).Sum() / _context.UserLikedRecipies.Where(y => y.RecipieId == x.RecipieId).Count()).ToString(),
                    }).ToList();

                    return BaseResult<List<GetRecipies>>.Successed(recipies);
                }
                return BaseResult<List<GetRecipies>>.Failure("Tarif bulunamadı");
            }
            catch (Exception)
            {
                return BaseResult<List<GetRecipies>>.Failure("Bilinmeyen bir hata oluştu");
            }
        }

        public class IngreditDTO
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        [HttpGet("/ingredients")]
        public async Task<BaseResult<List<IngreditDTO>>> GetIngredits()
        {
            var categorie = _context.Nutritions.Select(x => new IngreditDTO
            {
                Id = x.Id.ToString(),
                Name = x.NutritionName
            }).ToList();
            return BaseResult<List<IngreditDTO>>.Successed(categorie);
        }
        [HttpGet("/recipie/get-category")]
        public async Task<BaseResult<List<RecipieCategory>>> GetCategories()
        {
            var categorie = _context.RecipieCategories.ToList();
            return BaseResult<List<RecipieCategory>>.Successed(categorie);
        }

    }
}
