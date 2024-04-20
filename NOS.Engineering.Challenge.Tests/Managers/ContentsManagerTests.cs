using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NOS.Engineering.Challenge.Database;
using NOS.Engineering.Challenge.Managers;
using NOS.Engineering.Challenge.Models;
using NuGet.Frameworks;

namespace NOS.Engineering.Challenge.Tests.Managers
{
    public class ContentsManagerTests
    {
        private readonly ContentsManager _contentsManager;
        private readonly Mock<IDatabase<Content?, ContentDto>> _databaseMock = new();
        private readonly Mock<ILogger<ContentsManager>> _loggerMock = new();
        private readonly Content _fakeContent1;
        private readonly Content _fakeContent2;
        private readonly IList<Content> _fakeContentList;
        private readonly ContentDto _contentDto;
        public ContentsManagerTests()
        {
            _contentsManager = new ContentsManager(_databaseMock.Object, _loggerMock.Object);

            _fakeContent1 = new Content(Guid.NewGuid(), "Title 1", "Subtitle 1", "Description 1", "ImageUrl1", 90, DateTime.Now, DateTime.Now.AddMinutes(90), ["Action", "Horror"]);
            _fakeContent2 = new Content(Guid.NewGuid(), "Title 2", "Subtitle 2", "Description 2", "ImageUrl2", 90, DateTime.Now, DateTime.Now.AddMinutes(90), ["Comedy", "Fantasy"]);
            _fakeContentList = new List<Content> { _fakeContent1, _fakeContent2 };
            _contentDto = new ContentDto(_fakeContent1.Title,
                _fakeContent1.SubTitle,
                _fakeContent1.Description,
                _fakeContent1.ImageUrl,
                _fakeContent1.Duration,
                _fakeContent1.StartTime,
                _fakeContent1.EndTime,
                _fakeContent1.GenreList);
        }

        [Fact]
        public async void GetFilteredContentsAsync_Should_Return_Single_Result()
        {
            // Arrange
            _databaseMock.Setup(service => service.ReadAll()).ReturnsAsync(_fakeContentList);

            var filterDto = new FilterDto("Title", "Action");

            // Act
            var result = await _contentsManager.GetFilteredContentsAsync(filterDto);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);

            _databaseMock.Verify(service => service.ReadAll(), Times.Once);
        }

        [Fact]
        public async void CreateContentAsync_Should_Return_Content()
        {
            // Arrange
            _databaseMock.Setup(service => service.Create(It.IsAny<ContentDto>())).ReturnsAsync(_fakeContent1);

            // Act
            var result = await _contentsManager.CreateContentAsync(_contentDto);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Content>(result);

            _databaseMock.Verify(service => service.Create(It.IsAny<ContentDto>()), Times.Once);
        }

        [Fact]
        public async void GetContentAsync_Should_Return_Content()
        {
            // Arrange
            _databaseMock.Setup(service => service.Read(It.IsAny<Guid>())).ReturnsAsync(_fakeContent1);

            // Act
            var result = await _contentsManager.GetContentAsync(_fakeContent1.Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Content>(result);

            _databaseMock.Verify(service => service.Read(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async void UpdateContentAsync_Should_Return_Content()
        {
            // Arrange
            _databaseMock.Setup(service => service.Update(It.IsAny<Guid>(), It.IsAny<ContentDto>())).ReturnsAsync(_fakeContent1);

            // Act
            var result = await _contentsManager.UpdateContentAsync(_fakeContent1.Id, _contentDto);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Content>(result);

            _databaseMock.Verify(service => service.Update(It.IsAny<Guid>(), It.IsAny<ContentDto>()), Times.Once);
        }

        [Fact]
        public async void DeleteContentAsync_Should_Return_Content()
        {
            // Arrange
            _databaseMock.Setup(service => service.Delete(It.IsAny<Guid>())).ReturnsAsync(_fakeContent1.Id);

            // Act
            var result = await _contentsManager.DeleteContentAsync(_fakeContent1.Id);

            // Assert
            Assert.IsType<Guid>(result);

            _databaseMock.Verify(service => service.Delete(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async void AddGenresAsync_Should_Return_Content()
        {
            // Arrange
            _databaseMock.Setup(service => service.Read(It.IsAny<Guid>())).ReturnsAsync(_fakeContent1);
            _databaseMock.Setup(service => service.Update(It.IsAny<Guid>(), It.IsAny<ContentDto>())).ReturnsAsync(_fakeContent1);

            // Act
            var result = await _contentsManager.AddGenresAsync(_fakeContent1.Id, ["Sci-fi"]);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Content>(result);

            _databaseMock.Verify(service => service.Read(It.IsAny<Guid>()), Times.Once);
            _databaseMock.Verify(service => service.Update(It.IsAny<Guid>(), It.IsAny<ContentDto>()), Times.Once);
        }

        [Fact]
        public async void RemoveGenresAsync_Should_Return_Content()
        {
            // Arrange
            _databaseMock.Setup(service => service.Read(It.IsAny<Guid>())).ReturnsAsync(_fakeContent1);
            _databaseMock.Setup(service => service.Update(It.IsAny<Guid>(), It.IsAny<ContentDto>())).ReturnsAsync(_fakeContent1);

            // Act
            var result = await _contentsManager.RemoveGenresAsync(_fakeContent1.Id, ["Sci-fi"]);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Content>(result);

            _databaseMock.Verify(service => service.Read(It.IsAny<Guid>()), Times.Once);
            _databaseMock.Verify(service => service.Update(It.IsAny<Guid>(), It.IsAny<ContentDto>()), Times.Once);
        }
    }
}
