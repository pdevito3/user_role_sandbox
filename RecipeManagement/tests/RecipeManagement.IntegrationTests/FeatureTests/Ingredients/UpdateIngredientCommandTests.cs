namespace RecipeManagement.IntegrationTests.FeatureTests.Ingredients;

using RecipeManagement.SharedTestHelpers.Fakes.Ingredient;
using RecipeManagement.Domain.Ingredients.Dtos;
using SharedKernel.Exceptions;
using RecipeManagement.Domain.Ingredients.Features;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;
using RecipeManagement.SharedTestHelpers.Fakes.Recipe;

public class UpdateIngredientCommandTests : TestBase
{
    [Test]
    public async Task can_update_existing_ingredient_in_db()
    {
        // Arrange
        var fakeRecipeOne = FakeRecipe.Generate(new FakeRecipeForCreationDto().Generate());
        await InsertAsync(fakeRecipeOne);

        var fakeIngredientOne = FakeIngredient.Generate(new FakeIngredientForCreationDto()
            .RuleFor(i => i.RecipeId, _ => fakeRecipeOne.Id)
            .Generate());
        var updatedIngredientDto = new FakeIngredientForUpdateDto()
            .RuleFor(i => i.RecipeId, _ => fakeRecipeOne.Id)
            .Generate();
        await InsertAsync(fakeIngredientOne);

        var ingredient = await ExecuteDbContextAsync(db => db.Ingredients
            .FirstOrDefaultAsync(i => i.Id == fakeIngredientOne.Id));
        var id = ingredient.Id;

        // Act
        var command = new UpdateIngredient.Command(id, updatedIngredientDto);
        await SendAsync(command);
        var updatedIngredient = await ExecuteDbContextAsync(db => db.Ingredients.FirstOrDefaultAsync(i => i.Id == id));

        // Assert
        updatedIngredient.Should().BeEquivalentTo(updatedIngredientDto, options =>
            options.ExcludingMissingMembers());
    }
}