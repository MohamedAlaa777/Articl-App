using ArticlApp.Controllers;
using ArticlApp.Core;
using ArticlApp.UnitTest.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticlApp.UnitTest.Controllers
{
    public class CategoryControllerTests
    {
        [Fact]
        public void Index_ReturnsViewWithPagedData()
        {
            // Arrange
            var fake = new FakeDataHelper<Category>();
            for (int i = 1; i <= 20; i++)
                fake.Add(new Category { Id = i, Name = $"Category {i}" });

            var controller = new CategoryController(fake);

            // Act
            var result = controller.Index(null) as ViewResult;
            var model = result.Model as IEnumerable<Category>;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, model.Count()); // only first 10
            Assert.IsType<ViewResult>(result);
        }
    }
}
