using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MongoDBAssesmentApp.Controllers;
using MongoDBAssesmentDataAccess.IService;
using MongoDBAssesmentDomain.Entity;
using Moq;

namespace MongoDBAssesmentXUnitTest
{
    public class UserControllerXUnitTest
    {
        private readonly Mock<IUserService> userService;
        private readonly Mock<IPGUserService> pgUserService;
        private readonly Mock<IValidator<Users>> userValidatorService;
        private readonly UsersValidator usersValidator;

        public UserControllerXUnitTest()
        {
            this.userService = new Mock<IUserService>();
            this.pgUserService = new Mock<IPGUserService>();
            this.userValidatorService = new Mock<IValidator<Users>>();
            this.usersValidator = new UsersValidator();
        }

        [Fact]
        public async Task Test_Get_ShouldReturn200()
        {
            // Arrange
            userService.Setup(_ => _.GetAllAsync(null)).ReturnsAsync(DummyMockData.GetUserList());
            var systemUnderTest = new UserController(userService.Object, userValidatorService.Object, pgUserService.Object);

            //Act
            var result = await systemUnderTest.Get();

            //Assert
            Assert.NotNull(result);
            int value = (result as OkObjectResult).StatusCode.Value;
            Assert.Equal(200, value);
        }

        [Fact]
        public async Task Test_Get_ShouldReturn204()
        {
            // Arrange
            userService.Setup(_ => _.GetAllAsync(null)).ReturnsAsync(new List<Users>() { });
            var systemUnderTest = new UserController(userService.Object, userValidatorService.Object, pgUserService.Object);

            //Act
            var result = await systemUnderTest.Get();

            //Assert
            int value = (result as NoContentResult).StatusCode;
            Assert.Equal(204, value);
        }
        [Fact]
        public async Task Save_ValidationError_ShouldReturn400()
        {
            // Arrange
            var createUser = new Users()
            {
                UserName = "user name with space",
                FirstName = "test",
                LastName = "test",
                Email = "test@gmail.com",
                DateOfBirth = DateTime.Now
            };
            var systemUnderTest = new UserController(userService.Object, userValidatorService.Object, pgUserService.Object);
            var validationResult = usersValidator.Validate(createUser);
            userValidatorService.Setup(item => item.Validate(createUser)).Returns(validationResult);
            // Act
            var result = await systemUnderTest.Save(createUser);

            //Result
            int statusCode = (result as BadRequestObjectResult).StatusCode.Value;
            Assert.Equal(400, statusCode);
        }

        [Fact]
        public async Task Save_CheckUserIfExist_ShouldReturn400()
        {
            // Arrange
            var createUser = new Users()
            {
                UserName = "usernamewithspace",
                FirstName = "test",
                LastName = "test",
                Email = "test@gmail.com",
                DateOfBirth = DateTime.Now.AddYears(-30)
            };
            var systemUnderTest = new UserController(userService.Object, userValidatorService.Object, pgUserService.Object);
            var validationResult = usersValidator.Validate(createUser);
            userValidatorService.Setup(item => item.Validate(createUser)).Returns(validationResult);
            userService.Setup(item => item.CheckIfUserExist(createUser.UserName, createUser.Email)).ReturnsAsync(true);

            // Act
            var result = await systemUnderTest.Save(createUser);

            //Result
            int statusCode = (result as BadRequestObjectResult).StatusCode.Value;
            Assert.Equal(400, statusCode);
        }
        [Fact]
        public async Task Save_ShouldReturn200()
        {
            // Arrange
            var createUser = new Users()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "usernamewithspace",
                FirstName = "test",
                LastName = "test",
                Email = "test@gmail.com",
                DateOfBirth = DateTime.Now.AddYears(-30)
            };
            var systemUnderTest = new UserController(userService.Object, userValidatorService.Object, pgUserService.Object);
            var validationResult = usersValidator.Validate(createUser);
            userValidatorService.Setup(item => item.Validate(createUser)).Returns(validationResult);
            userService.Setup(item => item.CheckIfUserExist(createUser.UserName, createUser.Email)).ReturnsAsync(false);

            // Act
            var result = await systemUnderTest.Save(createUser);

            //Result
            int statusCode = (result as OkObjectResult).StatusCode.Value;
            Assert.Equal(200, statusCode);
            userService.Verify(x => x.InsertAsync(createUser), Times.Once());
        }

    }
}