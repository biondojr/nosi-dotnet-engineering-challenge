using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NOS.Engineering.Challenge.API.Controllers;
using NOS.Engineering.Challenge.API.Models;
using NOS.Engineering.Challenge.Managers;
using NOS.Engineering.Challenge.Models;

namespace NOS.Engineering.Challenge.API.Tests.Controllers
{
    public class ContentControllerTests
    {
        private readonly ContentController _contentController;
        private readonly Mock<IContentsManager> _contentsManagerMock = new();
        private readonly Content _fakeContent1;
        private readonly IList<Content> _fakeContentList;
        private readonly ContentInput _contentInput;

        public ContentControllerTests()
        {
            _contentController = new ContentController(_contentsManagerMock.Object);

            _fakeContent1 = new Content(Guid.NewGuid(), "Title 1", "Subtitle 1", "Description 1", "ImageUrl1", 90, DateTime.Now, DateTime.Now.AddMinutes(90), ["Action", "Horror"]);
            _fakeContentList = new List<Content> { _fakeContent1 };
            _contentInput = new ContentInput
            {
                Title = _fakeContent1.Title,
                SubTitle = _fakeContent1.SubTitle,
                Description = _fakeContent1.Description,
                ImageUrl = _fakeContent1.ImageUrl,
                Duration = _fakeContent1.Duration,
                StartTime = _fakeContent1.StartTime,
                EndTime = _fakeContent1.EndTime
            };
        }

        [Fact]
        public async void GetFilteredContentsAsync_Should_Return_Ok()
        {
            // Arrange
            _contentsManagerMock.Setup(service => service.GetFilteredContentsAsync(It.IsAny<FilterDto>())).ReturnsAsync(_fakeContentList);

            // Act
            var result = await _contentController.GetFilteredContentsAsync("Title", "Genre");

            // Assert
            Assert.IsType<OkObjectResult>(result);

            _contentsManagerMock.Verify(service => service.GetFilteredContentsAsync(It.IsAny<FilterDto>()), Times.Once);
        }

        [Fact]
        public async void GetFilteredContentsAsync_Should_Return_NotFound()
        {
            // Arrange
            _contentsManagerMock.Setup(service => service.GetFilteredContentsAsync(It.IsAny<FilterDto>())).ReturnsAsync(new List<Content>());

            // Act
            var result = await _contentController.GetFilteredContentsAsync("Title", "Genre");

            // Assert
            Assert.IsType<NotFoundResult>(result);

            _contentsManagerMock.Verify(service => service.GetFilteredContentsAsync(It.IsAny<FilterDto>()), Times.Once);
        }

        [Fact]
        public async void GetContentAsync_Should_Return_Ok()
        {
            // Arrange
            _contentsManagerMock.Setup(service => service.GetContentAsync(It.IsAny<Guid>())).ReturnsAsync(_fakeContent1);

            // Act
            var result = await _contentController.GetContentAsync(_fakeContent1.Id);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            _contentsManagerMock.Verify(service => service.GetContentAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async void GetContentAsync_Should_Return_NotFound()
        {
            // Arrange
            _contentsManagerMock.Setup(service => service.GetContentAsync(It.IsAny<Guid>()));

            // Act
            var result = await _contentController.GetContentAsync(_fakeContent1.Id);

            // Assert
            Assert.IsType<NotFoundResult>(result);

            _contentsManagerMock.Verify(service => service.GetContentAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async void CreateContentAsync_Should_Return_Ok()
        {
            // Arrange
            _contentsManagerMock.Setup(service => service.CreateContentAsync(It.IsAny<ContentDto>())).ReturnsAsync(_fakeContent1);

            // Act
            var result = await _contentController.CreateContentAsync(_contentInput);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            _contentsManagerMock.Verify(service => service.CreateContentAsync(It.IsAny<ContentDto>()), Times.Once);
        }

        [Fact]
        public async void CreateContentAsync_Should_Return_Problem()
        {
            // Arrange
            _contentsManagerMock.Setup(service => service.CreateContentAsync(It.IsAny<ContentDto>()));

            // Act
            var result = await _contentController.CreateContentAsync(_contentInput);

            // Assert
            Assert.IsType<ObjectResult>(result);

            _contentsManagerMock.Verify(service => service.CreateContentAsync(It.IsAny<ContentDto>()), Times.Once);
        }

        [Fact]
        public async void UpdateContentAsync_Should_Return_Ok()
        {
            // Arrange
            _contentsManagerMock.Setup(service => service.UpdateContentAsync(It.IsAny<Guid>(), It.IsAny<ContentDto>())).ReturnsAsync(_fakeContent1);

            // Act
            var result = await _contentController.UpdateContentAsync(_fakeContent1.Id, _contentInput);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            _contentsManagerMock.Verify(service => service.UpdateContentAsync(It.IsAny<Guid>(), It.IsAny<ContentDto>()), Times.Once);
        }

        [Fact]
        public async void UpdateContentAsync_Should_Return_NotFound()
        {
            // Arrange
            _contentsManagerMock.Setup(service => service.UpdateContentAsync(It.IsAny<Guid>(), It.IsAny<ContentDto>()));

            // Act
            var result = await _contentController.UpdateContentAsync(_fakeContent1.Id, _contentInput);

            // Assert
            Assert.IsType<NotFoundResult>(result);

            _contentsManagerMock.Verify(service => service.UpdateContentAsync(It.IsAny<Guid>(), It.IsAny<ContentDto>()), Times.Once);
        }

        [Fact]
        public async void DeleteContentAsync_Should_Return_Ok()
        {
            // Arrange
            _contentsManagerMock.Setup(service => service.DeleteContentAsync(It.IsAny<Guid>())).ReturnsAsync(_fakeContent1.Id);

            // Act
            var result = await _contentController.DeleteContentAsync(_fakeContent1.Id);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            _contentsManagerMock.Verify(service => service.DeleteContentAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async void AddGenresAsync_Should_Return_Ok()
        {
            // Arrange
            _contentsManagerMock.Setup(service => service.AddGenresAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<string>>())).ReturnsAsync(_fakeContent1);

            // Act
            var result = await _contentController.AddGenresAsync(_fakeContent1.Id, ["Action"]);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            _contentsManagerMock.Verify(service => service.AddGenresAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<string>>()), Times.Once);
        }

        [Fact]
        public async void AddGenresAsync_Should_Return_NotFound()
        {
            // Arrange
            _contentsManagerMock.Setup(service => service.AddGenresAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<string>>()));

            // Act
            var result = await _contentController.AddGenresAsync(_fakeContent1.Id, ["Action"]);

            // Assert
            Assert.IsType<NotFoundResult>(result);

            _contentsManagerMock.Verify(service => service.AddGenresAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<string>>()), Times.Once);
        }

        [Fact]
        public async void RemoveGenresAsync_Should_Return_Ok()
        {
            // Arrange
            _contentsManagerMock.Setup(service => service.RemoveGenresAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<string>>())).ReturnsAsync(_fakeContent1);

            // Act
            var result = await _contentController.RemoveGenresAsync(_fakeContent1.Id, ["Action"]);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            _contentsManagerMock.Verify(service => service.RemoveGenresAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<string>>()), Times.Once);
        }

        [Fact]
        public async void RemoveGenresAsync_Should_Return_NotFound()
        {
            // Arrange
            _contentsManagerMock.Setup(service => service.RemoveGenresAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<string>>()));

            // Act
            var result = await _contentController.RemoveGenresAsync(_fakeContent1.Id, ["Action"]);

            // Assert
            Assert.IsType<NotFoundResult>(result);

            _contentsManagerMock.Verify(service => service.RemoveGenresAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<string>>()), Times.Once);
        }
    }
}
