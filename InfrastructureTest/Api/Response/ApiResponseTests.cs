//using System;
//using System.Net;
//using FluentAssertions;
//using Infrastructure.Api.Exceptions;
//using Infrastructure.Api.Response;
//using NUnit.Framework;
//
//namespace InfrastructureTest.Api.Response
//{
//    [TestFixture]
//    public class ApiResponseTests
//    {
//        public class TestResponse
//        {
//            public string Name { get; set; }
//        }
//
//        [Test]
//        public void GivenValidResponse_ApiResponseHas_StatusCode_200()
//        {
//            //arrange
//            //act
//            var response = new ApiResponse<TestResponse>(new TestResponse
//            {
//                Name = "test"
//            });
//            //assert
//            response.StatusCode.Should().Be(200);
//
//        }
//
//        [TestCase("data")]
//        [TestCase("json")]
//        public void GivenValidResponse_ApiResponseHas_Data(string data)
//        {
//            //arrange
//            //act
//            var response = new ApiResponse<string>(data);
//            //assert
//            response.Data.Should().NotBeNull();
//            response.Data.Should().BeEquivalentTo(data);
//        }
//
//        [Test]
//        public void GivenNullResponse_ApiResponseHas_NoData()
//        {
//            //arrange
//            //act
//            var response = new ApiResponse<TestResponse>((TestResponse)null);
//            //assert
//            response.Data.Should().BeNull();
//        }
//
//        [Test]
//        public void GivenNullResponse_ApiResponseHas_StatusCode_200()
//        {
//            //arrange
//            //act
//            var response = new ApiResponse<TestResponse>((TestResponse)null);
//            //assert
//            response.StatusCode.Should().Be(200);
//        }
//
//        [Test]
//        public void GivenBadRequestException_ApiResponseHas_StatusCode_400()
//        {
//            //arrange
//            //act
//            var response = new ApiResponse<object>(new BadRequestException());
//            //assert
//            response.StatusCode.Should().Be(400);
//        }
//
//        [TestCase(502)]
//        [TestCase(401)]
//        [TestCase(400)]
//        public void GivenApiException_ApiResponseHas_StatusCode_X(int statusCode)
//        {
//            //arrange
//            //act
//            var response = new ApiResponse<object>(new ApiException((HttpStatusCode)statusCode));
//            //assert
//            response.StatusCode.Should().Be(statusCode);
//        }
//
//        [TestCase("Unhandled Exception")]
//        [TestCase("Gateway Exception")]
//        public void GivenUnhandledException_ApiResponseHas_StatusCode_500(string exceptionMessage)
//        {
//            //arrange
//            //act
//            var response = new ApiResponse<object>(new Exception(exceptionMessage));
//            //assert
//            response.StatusCode.Should().Be(500);
//            response.Error.Errors.Should().NotBeNullOrEmpty();
//            response.Error.Errors[0].Message.Should().Be(exceptionMessage);
//        }
//    }
//}
