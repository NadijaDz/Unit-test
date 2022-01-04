using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using NormativeCalculator.Common.Enums;
using NormativeCalculator.Core.Entities;
using NormativeCalculator.Core.Helper;
using NormativeCalculator.Core.Models.DTOs;
using NormativeCalculator.Core.Models.Request;
using NormativeCalculator.Database;
using NormativeCalculator.Mapper.Mapping;
using NormativeCalculator.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NormativeCalculator.NUnitTest
{
    [TestFixture]
    public class RecipeNUnitTests
    {
        private RecipeService _recipeService;

        private NormativeCalculatorDBContext _context;

        private IMapper mapperMock;

        private DbContextOptions<NormativeCalculatorDBContext> _options;

        private Mock<IUserService> _userService;

        [OneTimeSetUp]
        public void SetUp()
        {
            _options = new DbContextOptionsBuilder<NormativeCalculatorDBContext>()
              .UseInMemoryDatabase(databaseName: "TempNormativeCalculator")
              .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
              .Options;
            _context = new NormativeCalculatorDBContext(_options);

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new NormativeCalculatorProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            mapperMock = mapper;

            _userService = new Mock<IUserService>();
            _userService.Setup(x => x.GetLoggedInUser()).Returns(Task.FromResult(new IdentityUser()
            {
                Id = Guid.NewGuid().ToString()
            }));

            _recipeService = new RecipeService(_context, mapperMock, _userService.Object);

            setUpAttributeForDatabase();
        }

        // ** Test add new recipe is working properly **
        [Test]
        public void InsertRecipeAsync_InputRequestRecipeWithMultipleIngredients_GetCorrectInsert()
        {
            //arrange
            var request = new AddRecipeRequest
            {
                Name = "Recipe 1",
                Description = "Description for Recipe 1",
                RecipeCategoryId = 5,
                CreatedAt = DateTime.Now,
                Ingredients = new List<AddRecipeIngredientsDto>
                {
                    new AddRecipeIngredientsDto {
                    IngredientId = 1,
                    MeasureUnit = Common.Enums.MeasureUnit.kg,
                    UnitQuantity = 15,
                    },
                    new AddRecipeIngredientsDto {
                    IngredientId = 2,
                    MeasureUnit = Common.Enums.MeasureUnit.L,
                    UnitQuantity = 10,
                    },
                    new AddRecipeIngredientsDto {
                    IngredientId = 3,
                    MeasureUnit = Common.Enums.MeasureUnit.gr,
                    UnitQuantity = 5,
                    }
                }
            };

            //act
            var result = _recipeService.InsertRecipeAsync(request);

            //assert
            var expectedRecipe = _context.Recipes.FirstOrDefault(r => r.Name == request.Name);

            Assert.AreEqual(expectedRecipe.Name, request.Name);
            Assert.AreEqual(expectedRecipe.Description, request.Description);
            Assert.AreEqual(expectedRecipe.RecipeCategoryId, request.RecipeCategoryId);
            Assert.AreEqual(expectedRecipe.RecipesIngredients.FirstOrDefault().IngredientId, request.Ingredients.FirstOrDefault().IngredientId);
            Assert.AreEqual(expectedRecipe.RecipesIngredients.FirstOrDefault().UnitQuantity, request.Ingredients.FirstOrDefault().UnitQuantity);
            Assert.AreEqual(expectedRecipe.RecipesIngredients.FirstOrDefault().MeasureUnit, request.Ingredients.FirstOrDefault().MeasureUnit);
        }

        [Test]
        public void InsertRecipeAsync_InputRequestWithoutCategory_ReturnNull()
        {
            //arrange
            var request = new AddRecipeRequest
            {
                Name = "Recipe 1",
                Description = "Description for Recipe 1"
            };

            //act
            var result = _recipeService.InsertRecipeAsync(request);

            //assert
            Assert.IsNull(result.Result);
        }

        // ** Test that user can’t add same ingredient twice for one recipe  **
        [Test]
        public void InsertRecipeIngredientsAsync_InputRequestWithTwoSameIngredients_ReturnInsertRecipeWithOneIngredient()
        {
            //arrange
            var request = new AddRecipeRequest
            {
                Name = "Recipe 3",
                Description = "Description for Recipe 3",
                RecipeCategoryId = 336995,
                CreatedAt = DateTime.Now,
                Ingredients = new List<AddRecipeIngredientsDto>
                {
                    new AddRecipeIngredientsDto {
                    IngredientId = 5,
                    MeasureUnit = Common.Enums.MeasureUnit.kg,
                    UnitQuantity = 15,
                    },
                    new AddRecipeIngredientsDto {
                    IngredientId = 5,
                    MeasureUnit = Common.Enums.MeasureUnit.kg,
                    UnitQuantity = 15,
                    }
                }
            };

            //act
            var result = _recipeService.InsertRecipeAsync(request);

            //assert
            var expectedRecipe = _context.Recipes.FirstOrDefault(r => r.Name == request.Name);
            var ids = expectedRecipe.RecipesIngredients.Select(x => x.IngredientId).ToList();

            Assert.That(ids, Is.Unique);
        }

        [Test]
        public void InsertRecipeIngredientsAsync_InputRequestWithMultipleIngredientsWhereTwoIsSame_ReturnRecipeWithOneOfDuplicateIngredient()
        {
            //arrange
            var request = new AddRecipeRequest
            {
                Name = "Recipe 4",
                Description = "Description for Recipe 4",
                RecipeCategoryId = 7775,
                CreatedAt = DateTime.Now,
                Ingredients = new List<AddRecipeIngredientsDto>
                {
                    new AddRecipeIngredientsDto {
                    IngredientId = 4,
                    MeasureUnit = Common.Enums.MeasureUnit.gr,
                    UnitQuantity = 200,
                    },
                    new AddRecipeIngredientsDto {
                    IngredientId = 4,
                    MeasureUnit = Common.Enums.MeasureUnit.gr,
                    UnitQuantity = 200,
                    },
                    new AddRecipeIngredientsDto {
                    IngredientId = 2,
                    MeasureUnit = Common.Enums.MeasureUnit.kg,
                    UnitQuantity = 3,
                    },
                    new AddRecipeIngredientsDto {
                    IngredientId = 1,
                    MeasureUnit = Common.Enums.MeasureUnit.ml,
                    UnitQuantity = 900,
                    },
                }
            };

            //act
            var result = _recipeService.InsertRecipeAsync(request);

            //assert
            var expectedRecipe = _context.Recipes.FirstOrDefault(r => r.Name == request.Name);

            Assert.That(expectedRecipe.RecipesIngredients, Is.Unique);
            Assert.That(expectedRecipe.RecipesIngredients.Count, Is.EqualTo(3));
        }

        [Test]
        public void InsertRecipeIngredientsAsync_InputRequestWithMultipleIngredientsWithSameIdButDifferentQuantity_ReturnRecipeWithOneOfDuplicateIngredient()
        {
            //arrange
            var request = new AddRecipeRequest
            {
                Name = "Recipe 22",
                Description = "Description for Recipe 22",
                RecipeCategoryId = 5,
                CreatedAt = DateTime.Now,
                Ingredients = new List<AddRecipeIngredientsDto>
                {
                    new AddRecipeIngredientsDto {
                    IngredientId = 4,
                    MeasureUnit = Common.Enums.MeasureUnit.gr,
                    UnitQuantity = 200,
                    },
                    new AddRecipeIngredientsDto {
                    IngredientId = 4,
                    MeasureUnit = Common.Enums.MeasureUnit.kg,
                    UnitQuantity = 5,
                    },
                    new AddRecipeIngredientsDto {
                    IngredientId = 2,
                    MeasureUnit = Common.Enums.MeasureUnit.kg,
                    UnitQuantity = 3,
                    },
                }
            };

            //act
            var result = _recipeService.InsertRecipeAsync(request);

            //assert
            var expectedRecipe = _context.Recipes.FirstOrDefault(r => r.Name == request.Name);

            Assert.That(expectedRecipe.RecipesIngredients, Is.Unique);
            Assert.That(expectedRecipe.RecipesIngredients.Count, Is.EqualTo(2));
        }

        // ** Test ingredient unit cost is calculating properly **

        [Test]
        public void CalculatedIngredientPrice_InputRequestQuantityInKg_ReturnCorrectPrice()
        {
            //act
            var quantity = 10;
            var unitPrice = (decimal)0.5;
            var result = CalculatedPrice.CalculatedIngredientPrice(quantity, MeasureUnit.kg, unitPrice);

            //assert
            var expectedCost = 5000;
            Assert.That(expectedCost, Is.EqualTo(result));
        }

        [Test]
        public void CalculatedIngredientPrice_InputRequestQuantityInGr_ReturnCorrectPrice()
        {

            //act
            var quantity = 500;
            var unitPrice = (decimal)0.8;
            var result = CalculatedPrice.CalculatedIngredientPrice(quantity, MeasureUnit.gr, unitPrice);

            //assert
            var expectedCost = 400;
            Assert.That(expectedCost, Is.EqualTo(result));
        }

        [Test]
        public void CalculatedIngredientPrice_InputRequestQuantityInKom_ReturnCorrectPrice()
        {
            //act
            var quantity = 5;
            var unitPrice = 10;
            var result = CalculatedPrice.CalculatedIngredientPrice(5, MeasureUnit.kom, 10);

            //assert
            var expectedCost = 50;
            Assert.That(expectedCost, Is.EqualTo(result));
        }

        // ** Test that recipe load more works properly  **

        [Test]
        public void GetRecipesAsync_RequestSkip0Take10_Return10OrLessRecipe()
        {
            //arrange
            var request = new RecipeSearchRequest
            {
                CategoryId = 5,
                Skip = 0
            };

            //act
            var result = _recipeService.GetRecipesAsync(request);

            //assert
            Assert.That(result.Result.Data.Count, Is.GreaterThan(0));
            Assert.That(result.Result.Data.Count, Is.LessThan(11));
        }

        [Test]
        public void GetRecipesAsync_RequestSkip10Take10_ReturnLastPage()
        {
            //arrange
            var request = new RecipeSearchRequest
            {
                CategoryId = 5,
                Skip = 10
            };
            //act
            var result = _recipeService.GetRecipesAsync(request);

            //assert
            var expectedNumberOfData = result.Result.TotalCount - 10;
            Assert.That(result.Result.Data.Count, Is.GreaterThan(0));
            Assert.That(result.Result.Data.Count, Is.EqualTo(expectedNumberOfData));
        }

        [Test]
        public void GetRecipesAsync_RequestSkip20Take10_Return0()
        {
            //arrange
            var request = new RecipeSearchRequest
            {
                CategoryId = 5,
                Skip = 20
            };
            //act
            var result = _recipeService.GetRecipesAsync(request);

            //assert
            Assert.That(result.Result.Data.Count, Is.EqualTo(0));
            Assert.That(result.Result.TotalCount, Is.LessThan(20));
        }

        // ** Test that recipe cost is calculating properly   **

        [Test]
        public void CalculatedRecipeTotalPrice_InputRecipeRequest_ReturnCorrectPrice()
        {
            //arrange
            var recipe = _context.Recipes.FirstOrDefault(x => x.Id == 1);

            //act
            var result = CalculatedPrice.CalculatedTotalPrice(recipe);

            //assert
            decimal expectedResult = 201.75m;

            Assert.That(expectedResult, Is.EqualTo(result));
        }

        [Test]
        public void CalculatedRecipeTotalPrice_InputRecipeRequest_ReturnInCorrectPrice()
        {
            //arrange
            var recipe = _context.Recipes.FirstOrDefault(x => x.Id == 1);

            //act
            var result = CalculatedPrice.CalculatedTotalPrice(recipe);

            //assert
            var expectedResult = 500;
            Assert.That(expectedResult, !Is.EqualTo(result));
        }

        public void setUpAttributeForDatabase()
        {
            _context.RecipeCategories.Add(new RecipeCategory
            {
                Id = 5,
                CategoryName = "Category 1"

            });

            for (int i = 0; i < 18; i++)
            {
                _context.Recipes.Add(new Recipe
                {
                    Id = i + 1,
                    RecipeCategoryId = 5
                });
            }

            _context.Ingredients.Add(new Ingredient
            {
                Id = 1,
                UnitPrice = (decimal)0.0035,
                UnitQuantity = 4,
                CostIngredient = 14,
                MeasureUnit = MeasureUnit.kg
            });

            _context.Ingredients.Add(new Ingredient
            {
                Id = 2,
                UnitPrice = (decimal)0.04,
                UnitQuantity = 2,
                CostIngredient = 9,
                MeasureUnit = MeasureUnit.ml

            });
            _context.Ingredients.Add(new Ingredient
            {
                Id = 3,
                UnitPrice = (decimal)0.04,
                UnitQuantity = 2,
                CostIngredient = 9,
                MeasureUnit = MeasureUnit.ml

            });
            _context.Ingredients.Add(new Ingredient
            {
                Id = 4,
                UnitPrice = (decimal)0.04,
                UnitQuantity = 2,
                CostIngredient = 9,
                MeasureUnit = MeasureUnit.ml

            });
            _context.Ingredients.Add(new Ingredient
            {
                Id = 5,
                UnitPrice = (decimal)0.04,
                UnitQuantity = 2,
                CostIngredient = 9,
                MeasureUnit = MeasureUnit.ml

            });

            _context.RecipesIngredients.Add(new RecipeIngredient
            {
                RecipeId = 1,
                IngredientId = 1,
                UnitQuantity = 500,
                MeasureUnit = MeasureUnit.gr
            });

            _context.RecipesIngredients.Add(new RecipeIngredient
            {
                RecipeId = 1,
                IngredientId = 2,
                UnitQuantity = 5,
                MeasureUnit = MeasureUnit.L
            });

            _context.SaveChanges();
        }
    };
}