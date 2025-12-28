using ArticlApp.Code;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticlApp.UnitTest.Helpers
{
    public class FilesHelperTests
    {
        [Fact]
        public void UploadFile_ReturnsGeneratedName()
        {
            // Arrange: mock environment
            var mockEnv = new Mock<IWebHostEnvironment>();
            mockEnv.Setup(e => e.WebRootPath).Returns(Path.GetTempPath());

            // fake file
            var fileMock = new Mock<IFormFile>();
            var content = "Fake content";
            var fileName = "test.jpg";
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(content));
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.CopyTo(It.IsAny<Stream>())).Callback((Stream stream) =>
            {
                ms.CopyTo(stream);
            });

            var helper = new FilesHelper(mockEnv.Object);

            // Act
            var result = helper.UploadFile(fileMock.Object, "Images");

            // Assert
            Assert.EndsWith("test.jpg", result);
            Assert.NotEqual(result, fileName); // GUID + name
        }
    }
}
