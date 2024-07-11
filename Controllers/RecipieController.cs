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
                    try { 
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
                        LikedValue = _context.UserLikedRecipies.Where(y => y.RecipieId == x.RecipieId).Count()>0 ? (_context.UserLikedRecipies.Where(y => y.RecipieId == x.RecipieId).Select(x => Convert.ToInt32(x.value)).Sum() / _context.UserLikedRecipies.Where(y => y.RecipieId == x.RecipieId).Count()).ToString():"0",

                    }).ToList();

                    }
                    catch(Exception ex)
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
        public async Task<BaseResult> SetRecipie(AddRecipies addRec, string userId)
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
        [HttpGet("/recipie/get-category")]
        public async Task<BaseResult<List<RecipieCategory>>> GetCategories()
        {
            var categorie = _context.RecipieCategories.ToList();
            return BaseResult<List<RecipieCategory>>.Successed(categorie);
        }

    }
}
